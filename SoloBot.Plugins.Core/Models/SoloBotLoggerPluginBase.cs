using System;
using SoloBot.Log.Interface;

namespace SoloBot.Plugins.Core.Models
{
    /// <summary>
    ///     Abstract base class for a logger plugin.
    /// </summary>
    public abstract class SoloBotLoggerPluginBase : ISoloBotLogger
    {
        /// <summary>
        ///     Gets or sets the plugin name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        ///     Gets or sets the plugin description.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        ///     Gets or sets the plugin version.
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        ///     Used to initialize the plugin.
        ///     This is where the plugin details will be set.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     Called by the plugin handler when there is a message to log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public abstract void Log(string message);

        /// <summary>
        ///     Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes of the plugin.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}