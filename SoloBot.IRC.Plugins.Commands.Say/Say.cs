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
        public string Command
        {
            get { throw new NotImplementedException(); }
        }

        public void ReceiveRawCommand(IIRCPlugin sender, string command)
        {
            var message = IrcMessage.Parse(command);
            foreach (var item in message.Params)
            {
                if (item.StartsWith("!say"))
                {
                    sender.SendCommand("privmsg " + sender.Channel + " : " + item.Substring(item.IndexOf(' ') + 1));
                }
            }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}