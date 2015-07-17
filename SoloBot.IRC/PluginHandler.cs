using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using SoloBot.Core.Abstract;
using SoloBot.Core.Models;
using SoloBot.IRC.Interface;

namespace SoloBot.IRC
{
    /// <summary>
    ///     Manages the IRC client plugins.
    /// </summary>
    public class PluginHandler : IPluginHandler
    {
        /// <summary>
        ///     Catalog used to load plugins.
        /// </summary>
        private AggregateCatalog _catalog = new AggregateCatalog();

        /// <summary>
        ///     Gets or sets the list of plugins that are loaded.
        /// </summary>
        [ImportMany(typeof (IIrcPlugin))]
        public List<IIrcPlugin> PluginList { get; set; }

        /// <summary>
        ///     Initializes the plugins.
        /// </summary>
        public void InitializePlugins()
        {
            PluginList = new List<IIrcPlugin>();
            _catalog.Catalogs.Add(new DirectoryCatalog(GetConfigurationPath(), "*.dll"));
            var compositionContainer = new CompositionContainer(_catalog);
            compositionContainer.ComposeParts(this);
            foreach (var plugin in PluginList)
            {
                plugin.Initialize();
            }
        }

        /// <summary>
        ///     Retrieves the path to the plugins.
        /// </summary>
        /// <returns>The plugin path.</returns>
        public string GetConfigurationPath()
        {
            var pluginConfig = ConfigurationManager.OpenExeConfiguration(GetType().Assembly.Location);
            var pluginConfigAppSettings = (AppSettingsSection) pluginConfig.GetSection("appSettings");
            return pluginConfigAppSettings.Settings["PluginPath"].Value;
        }

        /// <summary>
        ///     Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            return PluginList.Select(plugin => new[]
            {
                plugin.Name, plugin.Description, plugin.Version
            }).ToArray();
        }

        /// <summary>
        ///     Disposes of the plugin manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Binding point for the plugin message events to be forwarded to the client object.
        /// </summary>
        public event EventHandler<IrcEventArgs> RawMessageReceived;

        #region IRC Client Plugin Methods

        /// <summary>
        ///     Starts all IRC client plugins.
        /// </summary>
        public void Start()
        {
            foreach (var plugin in PluginList)
            {
                plugin.RawMessageReceived += RawMessageReceived;
                plugin.Start();
            }
        }

        /// <summary>
        ///     Stops all IRC client plugins.
        /// </summary>
        public void Stop()
        {
            foreach (var plugin in PluginList)
            {
                plugin.Stop();
            }
        }

        /// <summary>
        ///     Send a command to all IRC client plugins.
        /// </summary>
        /// <param name="command">Raw IRC command to send.</param>
        public void SendCommand(string command)
        {
            foreach (var plugin in PluginList)
            {
                var pluginTemp = plugin;
                Task.Factory.StartNew(() => pluginTemp.SendCommand(command));
                // Send through seperate thread to eliminate blocking from plugins and to speed up responses.
            }
        }

        /// <summary>
        ///     Disposes of the plugin manager
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_catalog != null)
                {
                    _catalog.Dispose();
                    _catalog = null;
                }

                if (PluginList != null)
                {
                    PluginList.Clear();
                    PluginList = null;
                }
            }
        }

        #endregion IRC Client Plugin Methods
    }
}