namespace SoloBot.IRC.Plugins.SmartTwitch
{
    using Meebey.SmartIrc4net;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// SmartIRC4net IRC client plugin that doesn't use the plugin base.
    /// </summary>
    [Export(typeof(IIRCPlugin))]
    public class Twitch : IIRCPlugin
    {
        #region Private Fields

        /// <summary>
        /// The IRC client.
        /// </summary>
        private IrcClient client = new IrcClient();

        /// <summary>
        /// The current channel.
        /// </summary>
        private string channel;

        /// <summary>
        /// The plugin name.
        /// </summary>
        private string name = "SmartIrc4net IRC client";

        /// <summary>
        /// The plugin description.
        /// </summary>
        private string description = "An IRC client that utilizes the SmartIrc4net framework.";

        /// <summary>
        /// The plugin version.
        /// </summary>
        private string version = "0.01";

        #endregion Private Fields

        #region Events

        /// <summary>
        /// The SoloBot IRC message event.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the current channel.
        /// </summary>
        public string Channel
        {
            get
            {
                return this.channel;
            }

            private set
            {
                if (this.channel != value)
                {
                    this.channel = value;
                }
            }
        }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version
        {
            get
            {
                return this.version;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Starts the client.
        /// </summary>
        public void Start() // Todo: Pass in connection details.
        {
            this.client.Encoding = System.Text.Encoding.UTF8;
            this.client.SendDelay = 200;
            this.client.ActiveChannelSyncing = true; // Allows channels
            this.client.OnRawMessage += this.Client_OnRawMessage;
            this.client.OnJoin += this.Client_OnJoin;
            try
            {
                this.client.Connect("irc.twitch.tv", 6667);
            }
            catch (ConnectionException e)
            {
                Console.WriteLine("Couldn't connect: " + e.Message);
                throw new Exception();
            }

            try
            {
                this.client.Login(Properties.Settings.Default.Username, Properties.Settings.Default.Username, 3, Properties.Settings.Default.Username, Properties.Settings.Default.AuthKey);
                this.client.Listen();
            }
            catch (ConnectionException)
            {
                throw new Exception(); // Todo: Log Exceptions
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(); // Todo: Log Exceptions
            }
        }

        /// <summary>
        /// Stops the client.
        /// </summary>
        public void Stop()
        {
            this.client.Disconnect();
        }

        /// <summary>
        /// Sends a raw IRC command.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public void SendCommand(string command)
        {
            this.client.WriteLine(command);
        }

        /// <summary>
        /// Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
            this.client = null;
        }

        #endregion Public Methods

        #region Event Handlers

        /// <summary>
        /// Pushes a message into the SoloBot message event system.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">SoloBot IRCEventArgs object.</param>
        public void OnRawMessageReceived(IIRCPlugin sender, IRCEventArgs e)
        {
            if (this.RawMessageReceived != null)
            {
                this.RawMessageReceived(sender, e);
            }
        }

        /// <summary>
        /// Handler for the SmartIRC4net message event that is used to convert to the SoloBot message event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnRawMessage(object sender, IrcEventArgs e)
        {
            this.OnRawMessageReceived(this, new IRCEventArgs(e.Data.RawMessage));
        }

        /// <summary>
        /// Handler for the SmartIRC4net event that is thrown when a channel join message is detected.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnJoin(object sender, JoinEventArgs e)
        {
            this.Channel = e.Channel;
        }

        #endregion Event Handlers
    }
}