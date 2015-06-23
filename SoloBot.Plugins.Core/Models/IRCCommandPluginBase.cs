namespace SoloBot.Plugins.Core.Models
{
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using System;

    public abstract class IRCCommandPluginBase : IIRCCommand
    {
        private string command;
        private string name;
        private string description;
        private string version;

        public string Command
        {
            get
            {
                return this.command;
            }

            protected set
            {
                this.command = value;
            }
        }

        public abstract void ReceiveRawCommand(IIRCPlugin sender, string command);

        public string Name
        {
            get
            {
                return this.name;
            }

            protected set
            {
                this.name = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            protected set
            {
                this.description = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }

            protected set
            {
                this.version = value;
            }
        }

        public abstract void Initialize();

        public abstract void Dispose();
    }
}