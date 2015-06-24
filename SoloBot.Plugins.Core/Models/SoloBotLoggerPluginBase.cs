namespace SoloBot.Plugins.Core.Models
{
    using SoloBot.Log.Interface;
    using System;

    /// <summary>
    /// Abstract base class for a logger plugin.
    /// </summary>
    public abstract class SoloBotLoggerPluginBase : ISoloBotLogger
    {
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
        /// Called by the plugin handler when there is a message to log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public abstract void Log(string message);

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