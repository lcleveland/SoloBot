namespace SoloBot.IRC
{
    using SoloBot.Core.Models;
    using System;

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
            this.pluginHandler.InitializePlugins();
        }

        /// <summary>
        /// Event used for binding command plugins to receive the IRC client messages.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Static method to create a new Client object.
        /// </summary>
        /// <returns>New Client object.</returns>
        public static Client GetClient()
        {
            if (singletonClient == null)
            {
                singletonClient = new Client();
            }

            return singletonClient;
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
            this.pluginHandler = null;
            singletonClient = null;
        }

        /// <summary>
        /// Test function for when the plugin handler receives a message from any plugin.
        /// </summary>
        /// <param name="sender">IIRCPlugin object</param>
        /// <param name="e">Message object</param>
        private void PluginHandler_RawMessageReceived(object sender, IRCEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}