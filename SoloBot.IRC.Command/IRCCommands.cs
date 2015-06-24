namespace SoloBot.IRC.Command
{
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
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
        }

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
        /// Distributes the commands to the command plugins.
        /// </summary>
        /// <param name="sender">The IRC client sending the command.</param>
        /// <param name="command">The raw IRC command.</param>
        public void SendCommand(IIRCPlugin sender, IRCEventArgs command)
        {
            this.pluginHandler.SendCommand(sender, command);
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.pluginHandler = null;
            singletonCommands = null;
        }

        /// <summary>
        /// Disposes of the IRCCommands object.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.pluginHandler != null)
            {
                this.pluginHandler.Dispose();
                this.pluginHandler = null;
            }

            if (singletonCommands != null) 
            {
                singletonCommands.Dispose();
                singletonCommands = null;
            }
        }
    }
}