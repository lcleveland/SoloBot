namespace SoloBot.IRC
{
    using SoloBot.Core.Models;
    using SoloBot.IRC.Command;
    using SoloBot.IRC.Interface;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Instantiates a new IRC client using plugins.
    /// </summary>
    public class Client : IDisposable
    {
        /// <summary>
        /// The plugin client.
        /// </summary>
        private static Client singletonClient;

        /// <summary>
        /// The command plugins.
        /// </summary>
        private static IRCCommands singletonCommands;

        /// <summary>
        /// Handles loading the IRC client plugins.
        /// </summary>
        private PluginHandler pluginHandler;

        /// <summary>
        /// Prevents a default instance of the <see cref="Client" /> class from being created.
        /// </summary>
        private Client()
        {
            // Todo: Make sure plugin folders exist and if not create it.
            this.pluginHandler = new PluginHandler();
            singletonCommands = IRCCommands.GetCommands();
            this.pluginHandler.InitializePlugins();
        }

        /// <summary>
        /// Event used for binding command plugins to receive the IRC client messages.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Static method to create a new Client object.
        /// </summary>
        /// <returns>The Client object.</returns>
        public static Client GetClient()
        {
            if (singletonClient == null)
            {
                singletonClient = new Client();
            }

            return singletonClient;
        }

        /// <summary>
        /// Gets the name, description, and version of all plugins loaded by the client.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            List<string[]> pluginInfo = new List<string[]>();
            foreach (var item in this.pluginHandler.GetAllPluginInfo())
            {
                pluginInfo.Add(item);
            }

            foreach (var item in singletonCommands.GetAllPluginInfo())
            {
                pluginInfo.Add(item);
            }

            return pluginInfo.ToArray();
        }

        /// <summary>
        /// Gets the information for all loaded IRC client plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetIRCClientPluginInfo()
        {
            return this.pluginHandler.GetAllPluginInfo();
        }

        /// <summary>
        /// Gets the information for all loaded IRC command plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetCommandPluginInfo()
        {
            return singletonCommands.GetAllPluginInfo();
        }

        /// <summary>
        /// Sends the start signal to the IRC client plugins.
        /// </summary>
        public void Start()
        {
            this.pluginHandler.RawMessageReceived += this.RawMessageReceived;
            this.pluginHandler.RawMessageReceived += this.PluginHandler_RawMessageReceived;
            this.pluginHandler.Start();
        }

        /// <summary>
        /// Sends the stop signal to the IRC client plugins.
        /// </summary>
        public void Stop()
        {
            this.pluginHandler.Stop();
            this.pluginHandler.RawMessageReceived -= this.RawMessageReceived;
            this.pluginHandler.RawMessageReceived -= this.PluginHandler_RawMessageReceived;
        }

        /// <summary>
        /// Sends a command to the IRC client plugins.
        /// </summary>
        /// <param name="command">Raw IRC command to send.</param>
        public void SendCommand(string command)
        {
            this.pluginHandler.SendCommand(command);
        }

        /// <summary>
        /// Disposes of the Client object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the client object.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.pluginHandler != null)
                {
                    this.pluginHandler.Dispose();
                    this.pluginHandler = null;
                }

                if (singletonClient != null)
                {
                    singletonClient.Dispose();
                    singletonClient = null;
                }

                if (singletonCommands != null)
                {
                    singletonCommands.Dispose();
                    singletonCommands = null;
                }
            }
        }

        /// <summary>
        /// Test function for when the plugin handler receives a message from any plugin.
        /// </summary>
        /// <param name="sender">IIRCPlugin object</param>
        /// <param name="e">Message object</param>
        private void PluginHandler_RawMessageReceived(object sender, IRCEventArgs e)
        {
            singletonCommands.SendCommand((IIRCPlugin)sender, e);
        }
    }
}