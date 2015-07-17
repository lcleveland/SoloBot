using System;
using System.Linq;
using SoloBot.Core.Models;
using SoloBot.IRC.Command;
using SoloBot.IRC.Interface;

namespace SoloBot.IRC
{
    /// <summary>
    ///     Instantiates a new IRC client using plugins.
    /// </summary>
    public class Client : IDisposable
    {
        /// <summary>
        ///     The plugin client.
        /// </summary>
        private static Client _singletonClient;

        /// <summary>
        ///     The command plugins.
        /// </summary>
        private static IrcCommands _singletonCommands;

        /// <summary>
        ///     Handles loading the IRC client plugins.
        /// </summary>
        private PluginHandler _pluginHandler;

        /// <summary>
        ///     Prevents a default instance of the <see cref="Client" /> class from being created.
        /// </summary>
        private Client()
        {
            // Todo: Make sure plugin folders exist and if not create it.
            _pluginHandler = new PluginHandler();
            _singletonCommands = IrcCommands.GetCommands();
            _pluginHandler.InitializePlugins();
        }

        /// <summary>
        ///     Disposes of the Client object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Event used for binding command plugins to receive the IRC client messages.
        /// </summary>
        public event EventHandler<IrcEventArgs> RawMessageReceived;

        /// <summary>
        ///     Static method to create a new Client object.
        /// </summary>
        /// <returns>The Client object.</returns>
        public static Client GetClient()
        {
            return _singletonClient ?? (_singletonClient = new Client());
        }

        /// <summary>
        ///     Gets the name, description, and version of all plugins loaded by the client.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            var pluginInfo = _pluginHandler.GetAllPluginInfo().ToList();
            pluginInfo.AddRange(_singletonCommands.GetAllPluginInfo());

            return pluginInfo.ToArray();
        }

        /// <summary>
        ///     Gets the information for all loaded IRC client plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetIrcClientPluginInfo()
        {
            return _pluginHandler.GetAllPluginInfo();
        }

        /// <summary>
        ///     Gets the information for all loaded IRC command plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetCommandPluginInfo()
        {
            return _singletonCommands.GetAllPluginInfo();
        }

        /// <summary>
        ///     Sends the start signal to the IRC client plugins.
        /// </summary>
        public void Start()
        {
            _pluginHandler.RawMessageReceived += RawMessageReceived;
            _pluginHandler.RawMessageReceived += PluginHandler_RawMessageReceived;
            _pluginHandler.Start();
        }

        /// <summary>
        ///     Sends the stop signal to the IRC client plugins.
        /// </summary>
        public void Stop()
        {
            _pluginHandler.Stop();
            _pluginHandler.RawMessageReceived -= RawMessageReceived;
            _pluginHandler.RawMessageReceived -= PluginHandler_RawMessageReceived;
        }

        /// <summary>
        ///     Sends a command to the IRC client plugins.
        /// </summary>
        /// <param name="command">Raw IRC command to send.</param>
        public void SendCommand(string command)
        {
            _pluginHandler.SendCommand(command);
        }

        /// <summary>
        ///     Disposes of the client object.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_pluginHandler != null)
                {
                    _pluginHandler.Dispose();
                    _pluginHandler = null;
                }

                if (_singletonClient != null)
                {
                    _singletonClient.Dispose();
                    _singletonClient = null;
                }

                if (_singletonCommands != null)
                {
                    _singletonCommands.Dispose();
                    _singletonCommands = null;
                }
            }
        }

        /// <summary>
        ///     Test function for when the plugin handler receives a message from any plugin.
        /// </summary>
        /// <param name="sender">IIRCPlugin object</param>
        /// <param name="e">Message object</param>
        private void PluginHandler_RawMessageReceived(object sender, IrcEventArgs e)
        {
            _singletonCommands.SendCommand((IIrcPlugin) sender, e);
        }
    }
}