namespace SoloBot.IRC.Command
{
    using SoloBot.Core.Models;
    using System;

    /// <summary>
    /// Instantiates a new IRC client command control using plugins.
    /// </summary>
    public class IRCCommands : IDisposable
    {
        /// <summary>
        /// The plugin commands.
        /// </summary>
        private static IRCCommands singletonCommands;

        /// <summary>
        /// Handles loading the IRC client plugins.
        /// </summary>
        private PluginHandler pluginHandler;

        /// <summary>
        /// Prevents a default instance of the <see cref="IRCCommands" /> class from being created.
        /// </summary>
        private IRCCommands()
        {
            this.pluginHandler = new PluginHandler();
            this.pluginHandler.InitializePlugins();
            this.pluginHandler.RawMessageReceived += this.RawMessageReceived;
        }

        /// <summary>
        /// Event used to send commands to the command plugins.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Static method to create a new IRCCommands object.
        /// </summary>
        /// <returns>The IRCCommands object.</returns>
        public static IRCCommands GetCommands()
        {
            if (singletonCommands == null)
            {
                singletonCommands = new IRCCommands();
            }

            return singletonCommands;
        }

        /// <summary>
        /// Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            return this.pluginHandler.GetAllPluginInfo();
        }

        /// <summary>
        /// Disposes of the IRCCommands object.
        /// </summary>
        public void Dispose()
        {
            this.pluginHandler = null;
            singletonCommands = null;
        }
    }
}