using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using SoloBot.IRC.Interface;
using SoloBot.IRC.Plugin.ClientExample.Properties;
using SoloBot.Log;
using SoloBot.Plugins.Core.Models;

namespace SoloBot.IRC.Plugin.ClientExample
{
    /// <summary>
    ///     Example plugin for an IRC client.
    /// </summary>
    [Export(typeof (IIrcPlugin))]
    public class ClientExample : IrcClientPluginBase
    {
        /// <summary>
        ///     The SmartIRC4net client.
        /// </summary>
        private IrcClient _client;

        /// <summary>
        ///     SoloBot logger for exceptions.
        /// </summary>
        private SoloBotLogger _logger;

        /// <summary>
        ///     Initializes the plugin.
        ///     This is where we'll put the plugin details and load some objects.
        /// </summary>
        public override void Initialize()
        {
            Name = "Client Example";
            Description = "An example IRC client plugin.";
            Version = "0.01";

            _client = new IrcClient();
            _logger = SoloBotLogger.GetLogger();
        }

        /// <summary>
        ///     Starts the IRC client.
        /// </summary>
        public override void Start()
        {
            _client.Encoding = Encoding.UTF8;
            _client.SendDelay = 200;
            _client.AutoReconnect = true;
            _client.AutoRelogin = true;
            _client.AutoRetry = true;
            _client.AutoRejoin = true;
            _client.ActiveChannelSyncing = true;
            _client.OnRawMessage += Client_OnRawMessage;
            _client.OnConnected += Client_OnConnected;
            try
            {
                _client.Connect("irc.twitch.tv", 6667);
            }
            catch (ConnectionException e)
            {
                _logger.Log(e.ToString());
                throw new Exception();
            }

            try
            {
                _client.Login("S0L0B0T", "S0L0B0T", 3, "S0L0B0T", Settings.Default.AuthKey);
                new Thread(_client.Listen).Start();
                // Run the client listen loop on different thread so it doesn't block the main thread.
            }
            catch (ConnectionException ce)
            {
                _logger.Log(ce.ToString());
                throw new Exception();
            }
            catch (Exception e)
            {
                _logger.Log(e.ToString());
                throw new Exception();
            }
        }

        private void Client_OnConnected(object sender, EventArgs e)
        {
            SendCommand("CAP REQ :twitch.tv/membership");
        }

        /// <summary>
        ///     Stops the IRC client.
        /// </summary>
        public override void Stop()
        {
            _client.Disconnect();
            _client.OnRawMessage -= Client_OnRawMessage;
        }

        /// <summary>
        ///     Used to send a raw IRC command to the server.
        /// </summary>
        /// <param name="command">The raw command to send.</param>
        public override void SendCommand(string command)
        {
            _client.WriteLine(command);
        }

        /// <summary>
        ///     Disposes of the objects used.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (_client != null)
            {
                _client.Disconnect();
                _client = null;
            }

            if (_logger != null)
            {
                _logger.Dispose();
                _logger = null;
            }
        }

        /// <summary>
        ///     Event handler that is triggered when the client receives a raw IRC message.
        /// </summary>
        /// <param name="sender">The client.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_OnRawMessage(object sender, IrcEventArgs e)
        {
            // Converts SmartIrc4net's event into SoloBot's event format. You must send the raw IRC message.
            Task.Factory.StartNew(
                () => OnRawMessageReceived(this, new Core.Models.IrcEventArgs(e.Data.RawMessage, e.Data.Channel)));
            // Throw the events async so we dont block incoming messages.
        }
    }
}