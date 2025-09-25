using Gma.System.MouseKeyHook;
using Quasar.Common.Enums;
using Quasar.Common.Helpers;
using Quasar.Common.Messages;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;
using Quasar.Server.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmRemoteDesktop : Form
    {
        /// <summary>
        /// States whether remote mouse input is enabled.
        /// </summary>
        private bool _enableMouseInput;

        /// <summary>
        /// States whether remote keyboard input is enabled.
        /// </summary>
        private bool _enableKeyboardInput;

        /// <summary>
        /// Holds the state of the local keyboard hooks.
        /// </summary>
        private IKeyboardMouseEvents _keyboardHook;

        /// <summary>
        /// Holds the state of the local mouse hooks.
        /// </summary>
        private IKeyboardMouseEvents _mouseHook;

        /// <summary>
        /// A list of pressed keys for synchronization between key down & -up events.
        /// </summary>
        private readonly List<Keys> _keysPressed;

        /// <summary>
        /// The client which can be used for the remote desktop.
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly RemoteDesktopHandler _remoteDesktopHandler;

        /// <summary>
        /// Indicates if this form owns the handler (first form for a client).
        /// </summary>
        private readonly bool _ownsHandler;

        /// <summary>
        /// Holds the opened remote desktop forms for each client.
        /// </summary>
        private static readonly Dictionary<Client, List<FrmRemoteDesktop>> OpenedForms = new Dictionary<Client, List<FrmRemoteDesktop>>();

        /// <summary>
        /// Holds the shared handlers for each client to prevent conflicts.
        /// </summary>
        private static readonly Dictionary<Client, RemoteDesktopHandler> SharedHandlers = new Dictionary<Client, RemoteDesktopHandler>();

        /// <summary>
        /// Creates a new remote desktop form for the client or gets the current open form, if there exists one already.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        /// <param name="allowMultiple">If true, allows creating multiple forms for the same client.</param>
        /// <returns>
        /// Returns a new remote desktop form for the client if there is none currently open, otherwise creates a new one.
        /// </returns>
        public static FrmRemoteDesktop CreateNewOrGetExisting(Client client, bool allowMultiple = false)
        {
            if (!allowMultiple && OpenedForms.ContainsKey(client) && OpenedForms[client].Count > 0)
            {
                return OpenedForms[client][0];
            }
            
            FrmRemoteDesktop r = new FrmRemoteDesktop(client);
            
            if (!OpenedForms.ContainsKey(client))
                OpenedForms[client] = new List<FrmRemoteDesktop>();
            
            OpenedForms[client].Add(r);
            
            r.Disposed += (sender, args) => 
            {
                if (OpenedForms.ContainsKey(client))
                {
                    OpenedForms[client].Remove(r);
                    if (OpenedForms[client].Count == 0)
                    {
                        OpenedForms.Remove(client);
                        // Clean up shared handler when last form closes
                        if (SharedHandlers.ContainsKey(client))
                        {
                            SharedHandlers[client].Dispose();
                            SharedHandlers.Remove(client);
                        }
                    }
                }
            };
            
            return r;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmRemoteDesktop"/> class using the given client.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        public FrmRemoteDesktop(Client client)
        {
            _connectClient = client;
            _keysPressed = new List<Keys>();

            // Use shared handler to prevent conflicts between multiple windows
            if (!SharedHandlers.ContainsKey(client))
            {
                SharedHandlers[client] = new RemoteDesktopHandler(client);
                _ownsHandler = true;  // First form owns the handler
            }
            else
            {
                _ownsHandler = false;  // Subsequent forms share the handler
            }
            
            _remoteDesktopHandler = SharedHandlers[client];

            RegisterMessageHandler();
            InitializeComponent();
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
        /// Registers the remote desktop message handler for client communication.
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _remoteDesktopHandler.DisplaysChanged += DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged += UpdateImage;
            
            // Only register the handler once (when the first form is created)
            if (_ownsHandler)
            {
                MessageHandler.Register(_remoteDesktopHandler);
            }
        }

        /// <summary>
        /// Unregisters the remote desktop message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            // Only unregister if this form owns the handler
            if (_ownsHandler)
            {
                MessageHandler.Unregister(_remoteDesktopHandler);
            }
            
            _remoteDesktopHandler.DisplaysChanged -= DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged -= UpdateImage;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// Subscribes to local mouse and keyboard events for remote desktop input.
        /// </summary>
        private void SubscribeEvents()
        {
            // TODO: Check Hook.GlobalEvents vs Hook.AppEvents below
            // TODO: Maybe replace library with .NET events like on Linux
            if (PlatformHelper.RunningOnMono) // Mono/Linux
            {
                this.KeyDown += OnKeyDown;
                this.KeyUp += OnKeyUp;
            }
            else // Windows
            {
                _keyboardHook = Hook.GlobalEvents();
                _keyboardHook.KeyDown += OnKeyDown;
                _keyboardHook.KeyUp += OnKeyUp;

                _mouseHook = Hook.AppEvents();
                _mouseHook.MouseWheel += OnMouseWheelMove;
            }
        }

        /// <summary>
        /// Unsubscribes from local mouse and keyboard events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            if (PlatformHelper.RunningOnMono) // Mono/Linux
            {
                this.KeyDown -= OnKeyDown;
                this.KeyUp -= OnKeyUp;
            }
            else // Windows
            {
                if (_keyboardHook != null)
                {
                    _keyboardHook.KeyDown -= OnKeyDown;
                    _keyboardHook.KeyUp -= OnKeyUp;
                    _keyboardHook.Dispose();
                }
                if (_mouseHook != null)
                {
                    _mouseHook.MouseWheel -= OnMouseWheelMove;
                    _mouseHook.Dispose();
                }
            }
        }

        /// <summary>
        /// Starts the remote desktop stream and begin to receive desktop frames.
        /// </summary>
        private void StartStream()
        {
            try
            {
                ToggleConfigurationControls(true);

                picDesktop.Start();
                // Subscribe to the new frame counter.
                picDesktop.SetFrameUpdatedEvent(frameCounter_FrameUpdated);

                this.ActiveControl = picDesktop;

                int monitorToUse;
                // Check if "All Monitors" is selected
                if (cbMonitors.SelectedItem != null && cbMonitors.SelectedItem.ToString() == "All Monitors")
                {
                    monitorToUse = 999; // Special value for all monitors
                }
                else
                {
                    monitorToUse = cbMonitors.SelectedIndex;
                }
                
                // Validate monitor index before starting
                if (monitorToUse < 0)
                {
                    throw new InvalidOperationException("No valid monitor selected");
                }
                
                _remoteDesktopHandler.BeginReceiveFrames(barQuality.Value, monitorToUse);
            }
            catch (Exception ex)
            {
                // Handle streaming errors gracefully
                ToggleConfigurationControls(false);
                MessageBox.Show($"Failed to start remote desktop streaming: {ex.Message}", 
                    "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Stops the remote desktop stream.
        /// </summary>
        private void StopStream()
        {
            ToggleConfigurationControls(false);

            picDesktop.Stop();
            // Unsubscribe from the frame counter. It will be re-created when starting again.
            picDesktop.UnsetFrameUpdatedEvent(frameCounter_FrameUpdated);

            this.ActiveControl = picDesktop;

            _remoteDesktopHandler.EndReceiveFrames();
        }

        /// <summary>
        /// Toggles the activatability of configuration controls in the status/configuration panel.
        /// </summary>
        /// <param name="started">When set to <code>true</code> the configuration controls get enabled, otherwise they get disabled.</param>
        private void ToggleConfigurationControls(bool started)
        {
            btnStart.Enabled = !started;
            btnStop.Enabled = started;
            barQuality.Enabled = !started;
            cbMonitors.Enabled = !started;
        }

        /// <summary>
        /// Toggles the visibility of the status/configuration panel.
        /// </summary>
        /// <param name="visible">Decides if the panel should be visible.</param>
        private void TogglePanelVisibility(bool visible)
        {
            panelTop.Visible = visible;
            btnShow.Visible = !visible;
            this.ActiveControl = picDesktop;
        }

        /// <summary>
        /// Called whenever the remote displays changed.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="displays">The currently available displays.</param>
        private void DisplaysChanged(object sender, int displays)
        {
            try
            {
                {
                    // For general forms, show all monitors
                    cbMonitors.Items.Clear();
                    for (int i = 0; i < displays; i++)
                        cbMonitors.Items.Add($"Display {i + 1}");
                    
                    // Add "All Monitors" option if there are multiple monitors
                    if (displays > 1)
                    {
                        cbMonitors.Items.Add("All Monitors");
                    }
                    
                    if (cbMonitors.Items.Count > 0)
                        cbMonitors.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // Prevent crashes from display detection issues
                this.Text = WindowHelper.GetWindowTitle("Remote Desktop (Error)", _connectClient);
                btnStart.Enabled = false;
            }
        }

        /// <summary>
        /// Updates the current desktop image by drawing it to the desktop picturebox.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="bmp">The new desktop image to draw.</param>
        private void UpdateImage(object sender, Bitmap bmp)
        {
            picDesktop.UpdateImage(bmp, false);
        }

        private void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Desktop", _connectClient);

            OnResize(EventArgs.Empty); // trigger resize event to align controls 

            _remoteDesktopHandler.RefreshDisplays();
        }

        /// <summary>
        /// Updates the title with the current frames per second.
        /// </summary>
        /// <param name="e">The new frames per second.</param>
        private void frameCounter_FrameUpdated(FrameUpdatedEventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", WindowHelper.GetWindowTitle("Remote Desktop", _connectClient), e.CurrentFramesPerSecond.ToString("0.00"));
        }

        private void FrmRemoteDesktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            // all cleanup logic goes here
            UnsubscribeEvents();
            if (_remoteDesktopHandler.IsStarted) StopStream();
            UnregisterMessageHandler();
            
            // Only dispose the handler if this form owns it
            if (_ownsHandler)
            {
                _remoteDesktopHandler.Dispose();
                SharedHandlers.Remove(_connectClient);
            }
            
            picDesktop.Image?.Dispose();
        }

        private void FrmRemoteDesktop_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            _remoteDesktopHandler.LocalResolution = picDesktop.Size;
            panelTop.Left = (this.Width - panelTop.Width) / 2;
            btnShow.Left = (this.Width - btnShow.Width) / 2;
            btnHide.Left = (panelTop.Width - btnHide.Width) / 2;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMonitors.Items.Count == 0)
                {
                    MessageBox.Show("No remote display detected.\nPlease wait till the client sends a list with available displays.",
                        "Starting failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                SubscribeEvents();
                StartStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start remote desktop: {ex.Message}", 
                    "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            UnsubscribeEvents();
            StopStream();
        }

        #region Remote Desktop Input

        private void picDesktop_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftDown;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightDown;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                _remoteDesktopHandler.SendMouseEvent(action, true, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseUp(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftUp;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightUp;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                _remoteDesktopHandler.SendMouseEvent(action, false, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                _remoteDesktopHandler.SendMouseEvent(MouseAction.MoveCursor, false, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void OnMouseWheelMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                int selectedDisplayIndex = cbMonitors.SelectedIndex;
                _remoteDesktopHandler.SendMouseEvent(e.Delta == 120 ? MouseAction.ScrollUp : MouseAction.ScrollDown,
                    false, 0, 0, selectedDisplayIndex);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && this.ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                if (_keysPressed.Contains(e.KeyCode))
                    return;

                _keysPressed.Add(e.KeyCode);

                _remoteDesktopHandler.SendKeyboardEvent((byte)e.KeyCode, true);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && this.ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                _keysPressed.Remove(e.KeyCode);

                _remoteDesktopHandler.SendKeyboardEvent((byte)e.KeyCode, false);
            }
        }

        private bool IsLockKey(Keys key)
        {
            return ((key & Keys.CapsLock) == Keys.CapsLock)
                   || ((key & Keys.NumLock) == Keys.NumLock)
                   || ((key & Keys.Scroll) == Keys.Scroll);
        }


        #endregion

        #region Remote Desktop Configuration

        private void barQuality_Scroll(object sender, EventArgs e)
        {
            int value = barQuality.Value;
            lblQualityShow.Text = value.ToString();

            if (value < 25)
                lblQualityShow.Text += " (low)";
            else if (value >= 85)
                lblQualityShow.Text += " (best)";
            else if (value >= 75)
                lblQualityShow.Text += " (high)";
            else if (value >= 25)
                lblQualityShow.Text += " (mid)";

            this.ActiveControl = picDesktop;
        }

        private void btnMouse_Click(object sender, EventArgs e)
        {
            if (_enableMouseInput)
            {
                this.picDesktop.Cursor = Cursors.Default;
                btnMouse.Image = Properties.Resources.mouse_delete;
                toolTipButtons.SetToolTip(btnMouse, "Enable mouse input.");
                _enableMouseInput = false;
            }
            else
            {
                this.picDesktop.Cursor = Cursors.Hand;
                btnMouse.Image = Properties.Resources.mouse_add;
                toolTipButtons.SetToolTip(btnMouse, "Disable mouse input.");
                _enableMouseInput = true;
            }

            this.ActiveControl = picDesktop;
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            if (_enableKeyboardInput)
            {
                this.picDesktop.Cursor = Cursors.Default;
                btnKeyboard.Image = Properties.Resources.keyboard_delete;
                toolTipButtons.SetToolTip(btnKeyboard, "Enable keyboard input.");
                _enableKeyboardInput = false;
            }
            else
            {
                this.picDesktop.Cursor = Cursors.Hand;
                btnKeyboard.Image = Properties.Resources.keyboard_add;
                toolTipButtons.SetToolTip(btnKeyboard, "Disable keyboard input.");
                _enableKeyboardInput = true;
            }

            this.ActiveControl = picDesktop;
        }

        #endregion

        private void btnHide_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(false);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(true);
        }
    }
}
