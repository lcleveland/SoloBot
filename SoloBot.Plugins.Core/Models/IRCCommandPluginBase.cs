namespace SoloBot.Plugins.Core.Models
{
    using SoloBot.Core.Models;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using System;

    /// <summary>
    /// Abstract base class for an IRC command plugin.
    /// </summary>
    public abstract class IRCCommandPluginBase : IIRCCommand
    {
        /// <summary>
        /// The command to match.
        /// </summary>
        private string command;

        /// <summary>
        /// The plugin name.
        /// </summary>
        private string name;

        /// <summary>
        /// The plugin description.
        /// </summary>
        private string description;

        /// <summary>
        /// The plugin version.
        /// </summary>
        private string version;

        /// <summary>
        /// Gets or sets the plugin command.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the plugin name.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the plugin description.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
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

        /// <summary>
        /// Used to initialize the plugin.
        /// This is where the plugin details will be set.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Is called by the plugin handler when a command is received from the IRC client plugins.
        /// </summary>
        /// <param name="sender">IRC client plugin.</param>
        /// <param name="command">Raw IRC command.</param>
        public abstract void ReceiveRawCommand(IIRCPlugin sender, IRCEventArgs command);

        /// <summary>
        /// Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the plugin.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}