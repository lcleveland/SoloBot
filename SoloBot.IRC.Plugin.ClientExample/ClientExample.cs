namespace SoloBot.IRC.Plugin.ClientExample
{
    using Meebey.SmartIrc4net;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using SoloBot.Log;
    using SoloBot.Plugins.Core.Models;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IIRCPlugin))]
    public class ClientExample : IRCClientPluginBase
    {
        private IrcClient client;
        private SoloBotLogger logger;

        public override void Initialize()
        {
            this.Name = "Client Example";
            this.Description = "An example IRC client plugin.";
            this.Version = "0.01";

            this.client = new IrcClient();
            this.logger = SoloBotLogger.GetLogger();
        }

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
                logger.Log(e.ToString());
                throw new Exception();
            }

            try
            {
                this.client.Login(Properties.Settings.Default.Username, Properties.Settings.Default.Username, 3, Properties.Settings.Default.Username, Properties.Settings.Default.AuthKey);
                this.client.Listen();
            }
            catch (ConnectionException ce)
            {
                logger.Log(ce.ToString());
                throw new Exception();
            }
            catch (Exception e)
            {
                logger.Log(e.ToString());
                throw new Exception();
            }
        }

        private void Client_OnJoin(object sender, JoinEventArgs e)
        {
            this.Channel = e.Channel;
        }

        private void Client_OnRawMessage(object sender, IrcEventArgs e)
        {
            this.OnRawMessageReceived(this, new IRCEventArgs(e.Data.Message)); // Converts SmartIrc4net's event into SoloBot's event format.
        }

        public override void Stop()
        {
            this.client.Disconnect();
            this.client.OnRawMessage -= Client_OnRawMessage;
        }

        public override void SendCommand(string command)
        {
            this.client.WriteLine(command);
        }

        public override void Dispose()
        {
            this.client = null;
            this.logger = null;
        }
    }
}