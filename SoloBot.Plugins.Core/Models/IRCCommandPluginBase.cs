using System;
using SoloBot.Core.Models;
using SoloBot.IRC.Command.Interface;
using SoloBot.IRC.Interface;

namespace SoloBot.Plugins.Core.Models
{
    /// <summary>
    ///     Abstract base class for an IRC command plugin.
    /// </summary>
    public abstract class IrcCommandPluginBase : IIrcCommand
    {
        /// <summary>
        ///     Gets or sets the plugin command.
        /// </summary>
        public string Command { get; protected set; }

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
        ///     Is called by the plugin handler when a command is received from the IRC client plugins.
        /// </summary>
        /// <param name="sender">IRC client plugin.</param>
        /// <param name="command">Raw IRC command.</param>
        public abstract void ReceiveRawCommand(IIrcPlugin sender, IrcEventArgs command);

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