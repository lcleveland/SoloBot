namespace SoloBot.IRC.Plugin.ClientExample
{
    using Meebey.SmartIrc4net;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using SoloBot.Log;
    using SoloBot.Plugins.Core.Models;
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Example plugin for an IRC client.
    /// </summary>
    [Export(typeof(IIRCPlugin))]
    public class ClientExample : IRCClientPluginBase
    {
        /// <summary>
        /// The SmartIRC4net client.
        /// </summary>
        private IrcClient client;

        /// <summary>
        /// SoloBot logger for exceptions.
        /// </summary>
        private SoloBotLogger logger;

        /// <summary>
        /// Initializes the plugin.
        /// This is where we'll put the plugin details and load some objects.
        /// </summary>
        public override void Initialize()
        {
            this.Name = "Client Example";
            this.Description = "An example IRC client plugin.";
            this.Version = "0.01";

            this.client = new IrcClient();
            this.logger = SoloBotLogger.GetLogger();
        }

        /// <summary>
        /// Starts the IRC client.
        /// </summary>
        public override void Start()
        {
            this.client.Encoding = System.Text.Encoding.UTF8;
            this.client.SendDelay = 200;
            this.client.ActiveChannelSyncing = true;
            this.client.OnRawMessage += this.Client_OnRawMessage;
            this.client.OnJoin += this.Client_OnJoin;
            try
            {
                this.client.Connect("irc.twitch.tv", 6667);
            }
            catch (ConnectionException e)
            {
                this.logger.Log(e.ToString());
                throw new Exception();
            }

            try
            {
                this.client.Login("S0L0B0T", "S0L0B0T", 3, "S0L0B0T", Properties.Settings.Default.AuthKey);
                this.client.Listen();
            }
            catch (ConnectionException ce)
            {
                this.logger.Log(ce.ToString());
                throw new Exception();
            }
            catch (Exception e)
            {
                this.logger.Log(e.ToString());
                throw new Exception();
            }
        }

        /// <summary>
        /// Stops the IRC client.
        /// </summary>
        public override void Stop()
        {
            this.client.Disconnect();
            this.client.OnRawMessage -= this.Client_OnRawMessage;
            this.client.OnJoin -= this.Client_OnJoin;
        }

        /// <summary>
        /// Used to send a raw IRC command to the server.
        /// </summary>
        /// <param name="command">The raw command to send.</param>
        public override void SendCommand(string command)
        {
            this.client.WriteLine(command);
        }

        /// <summary>
        /// Disposes of the objects used.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.client != null)
            {
                this.client.Disconnect();
                this.client = null;
            }

            if (this.logger != null)
            {
                this.logger.Dispose();
                this.logger = null;
            }
        }

        /// <summary>
        /// Event handler that is triggered when the client joins a channel.
        /// Used to set the current channel property.
        /// </summary>
        /// <param name="sender">The client.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnJoin(object sender, JoinEventArgs e)
        {
            if (this.Channel != e.Channel)
            {
                this.Channel = e.Channel;
            }
        }

        /// <summary>
        /// Event handler that is triggered when the client receives a raw IRC message.
        /// </summary>
        /// <param name="sender">The client.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnRawMessage(object sender, IrcEventArgs e)
        {
            this.OnRawMessageReceived(this, new IRCEventArgs(e.Data.RawMessage, e.Data.Channel)); // Converts SmartIrc4net's event into SoloBot's event format. You must send the raw IRC message.
        }
    }
}