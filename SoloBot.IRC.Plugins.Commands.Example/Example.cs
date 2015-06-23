namespace SoloBot.IRC.Plugins.Commands.Example
{
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using System.ComponentModel.Composition;

    [Export(typeof(IIRCCommand))]
    public class Example : IIRCCommand
    {
        private string name = "Example Command Plugin";
        private string description = "An example plugin.";
        private string version = "0.01";
        private string command = "!example";

        public string Command
        {
            get
            {
                return command;
            }

            private set
            {
                this.command = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }

        public void Dispose()
        {
        }

        public void ReceiveRawCommand(IIRCPlugin sender, string command)
        {
            if (command.Contains(this.Command))
            {
                sender.SendCommand("privmsg " + sender.Channel + " :" + this.Description);
            }
        }


        public void Initialize()
        {
        }
    }
}