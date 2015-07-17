using System;
using SoloBot.Core.Models;
using SoloBot.IRC.Interface;

namespace SoloBot.IRC.Command
{
    /// <summary>
    ///     Instantiates a new IRC client command control using plugins.
    /// </summary>
    public class IrcCommands : IDisposable
    {
        /// <summary>
        ///     The plugin commands.
        /// </summary>
        private static IrcCommands _singletonCommands;

        /// <summary>
        ///     Handles loading the IRC client plugins.
        /// </summary>
        private PluginHandler _pluginHandler;

        /// <summary>
        ///     Prevents a default instance of the <see cref="IrcCommands" /> class from being created.
        /// </summary>
        private IrcCommands()
        {
            _pluginHandler = new PluginHandler();
            _pluginHandler.InitializePlugins();
        }

        /// <summary>
        ///     Disposes of the IRCCommands object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            _pluginHandler = null;
            _singletonCommands = null;
        }

        /// <summary>
        ///     Static method to create a new IRCCommands object.
        /// </summary>
        /// <returns>The IRCCommands object.</returns>
        public static IrcCommands GetCommands()
        {
            return _singletonCommands ?? (_singletonCommands = new IrcCommands());
        }

        /// <summary>
        ///     Distributes the commands to the command plugins.
        /// </summary>
        /// <param name="sender">The IRC client sending the command.</param>
        /// <param name="command">The raw IRC command.</param>
        public void SendCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            _pluginHandler.SendCommand(sender, command);
        }

        /// <summary>
        ///     Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            return _pluginHandler.GetAllPluginInfo();
        }

        /// <summary>
        ///     Disposes of the IRCCommands object.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_pluginHandler != null)
            {
                _pluginHandler.Dispose();
                _pluginHandler = null;
            }

            if (_singletonCommands != null)
            {
                _singletonCommands.Dispose();
                _singletonCommands = null;
            }
        }
    }
}