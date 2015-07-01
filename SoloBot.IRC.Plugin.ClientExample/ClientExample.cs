﻿namespace SoloBot.IRC.Plugin.ClientExample
{
    using Meebey.SmartIrc4net;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using SoloBot.Log;
    using SoloBot.Plugins.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;

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

        private List<string> channels;

        /// <summary>
        /// Initializes the plugin.
        /// This is where we'll put the plugin details and load some objects.
        /// </summary>
        public override void Initialize()
        {
            this.Name = "Client Example";
            this.Description = "An example IRC client plugin.";
            this.Version = "0.01";

            this.channels = new List<string>();
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
            this.client.PingTimeout = 60;
            this.client.AutoReconnect = true;
            this.client.AutoRelogin = true;
            this.client.AutoRetry = true;
            this.client.AutoRejoin = true;
            this.client.ActiveChannelSyncing = true;
            this.client.OnRawMessage += this.Client_OnRawMessage;
            this.client.OnConnected += Client_OnConnected;
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
                new Thread(new ThreadStart(this.client.Listen)).Start(); // Run the client listen loop on different thread so it doesn't block the main thread.
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

        private void Client_OnConnected(object sender, EventArgs e)
        {
            this.SendCommand("CAP REQ :twitch.tv/membership");
        }

        /// <summary>
        /// Stops the IRC client.
        /// </summary>
        public override void Stop()
        {
            this.client.Disconnect();
            this.client.OnRawMessage -= this.Client_OnRawMessage;
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
        /// Event handler that is triggered when the client receives a raw IRC message.
        /// </summary>
        /// <param name="sender">The client.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnRawMessage(object sender, IrcEventArgs e)
        {
            // Converts SmartIrc4net's event into SoloBot's event format. You must send the raw IRC message.
            Task.Factory.StartNew(() => this.OnRawMessageReceived(this, new IRCEventArgs(e.Data.RawMessage, e.Data.Channel))); // Throw the events async so we dont block incoming messages.
        }
    }
}