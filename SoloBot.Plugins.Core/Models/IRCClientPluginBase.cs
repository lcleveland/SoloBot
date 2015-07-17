using System;
using System.ComponentModel.Composition;
using SoloBot.Core.Models;
using SoloBot.IRC.Interface;

namespace SoloBot.Plugins.Core.Models
{
    /// <summary>
    ///     Serves as the base for creating a new IRC client plugin.
    /// </summary>
    [Export(typeof (IIrcPlugin))]
    public abstract class IrcClientPluginBase : IIrcPlugin
    {
        /// <summary>
        ///     The SoloBot IRC message event.
        /// </summary>
        public event EventHandler<IrcEventArgs> RawMessageReceived;

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
        ///     Initializes the plugin.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     Starts the client.
        /// </summary>
        public abstract void Start();

        /// <summary>
        ///     Stops the client.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        ///     Sends a raw IRC command.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public abstract void SendCommand(string command);

        /// <summary>
        ///     Pushes a message into the SoloBot message event system.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">SoloBot IRCEventArgs object.</param>
        public void OnRawMessageReceived(IIrcPlugin sender, IrcEventArgs e)
        {
            if (RawMessageReceived != null)
            {
                RawMessageReceived(sender, e);
            }
        }

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