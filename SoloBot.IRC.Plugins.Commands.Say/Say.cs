namespace SoloBot.IRC.Plugins.Commands.Say
{
    using IrcMessageSharp;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IIRCCommand))]
    public class Say : IIRCCommand
    {
        private string command = "!say";
        private string name = "Say Plugin";
        private string description = "Plugin that makes the client speak.";
        private string version = "0.01";

        public string Command
        {
            get { return this.command; }
        }

        public void ReceiveRawCommand(IIRCPlugin sender, string command)
        {
            IrcMessage message;
            if (!IrcMessage.TryParse(command, out message))
            {
                return;
            }
            foreach (var item in message.Params)
            {
                if (item.StartsWith("!say"))
                {
                    sender.SendCommand("privmsg " + sender.Channel + " :" + item.Substring(item.IndexOf(' ') + 1));
                }
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public string Version
        {
            get { return this.version; }
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
            Console.WriteLine(this.Name + " Loaded!");
        }
    }
}