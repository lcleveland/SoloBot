namespace SoloBot.IRC.Plugins.Commands.Say
{
    using IrcMessageSharp;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using SoloBot.Plugins.Core.Models;
    using System.ComponentModel.Composition;

    [Export(typeof(IIRCCommand))]
    public class Say : IRCCommandPluginBase
    {
        public override void ReceiveRawCommand(IIRCPlugin sender, string command)
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

        public override void Initialize()
        {
            this.Name = "Say Plugin";
            this.Description = "Plugin that makes the client speak.";
            this.Version = "0.01";
            this.Command = "!say";
        }

        public override void Dispose()
        {
        }
    }
}