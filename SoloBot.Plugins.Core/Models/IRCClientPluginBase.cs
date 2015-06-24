namespace SoloBot.Plugins.Core.Models
{
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Serves as the base for creating a new IRC client plugin.
    /// </summary>
    [Export(typeof(IIRCPlugin))]
    public abstract class IRCClientPluginBase : IIRCPlugin
    {
        /// <summary>
        /// The current channel.
        /// </summary>
        private string channel;

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
        /// The SoloBot IRC message event.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Gets or sets the current channel.
        /// </summary>
        public string Channel
        {
            get
            {
                return this.channel;
            }

            protected set
            {
                if (this.channel != value)
                {
                    this.channel = value;
                }
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
        /// Initializes the plugin.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Starts the client.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the client.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Sends a raw IRC command.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public abstract void SendCommand(string command);

        /// <summary>
        /// Pushes a message into the SoloBot message event system.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">SoloBot IRCEventArgs object.</param>
        public void OnRawMessageReceived(IIRCPlugin sender, IRCEventArgs e)
        {
            if (this.RawMessageReceived != null)
            {
                this.RawMessageReceived(sender, e);
            }
        }

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