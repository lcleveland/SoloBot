namespace SoloBot.IRC
{
    using SoloBot.Core.Abstract;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Interface;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Configuration;

    /// <summary>
    /// Manages the IRC client plugins.
    /// </summary>
    public class PluginHandler : IPluginManager
    {
        /// <summary>
        /// Catalog used to load plugins.
        /// </summary>
        private AggregateCatalog catalog = new AggregateCatalog();

        /// <summary>
        /// Binding point for the plugin message events to be forwarded to the client object.
        /// </summary>
        public event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Gets or sets the list of plugins that are loaded.
        /// </summary>
        [ImportMany(typeof(IIRCPlugin))]
        public List<IIRCPlugin> PluginList { get; set; }

        /// <summary>
        /// Initializes the plugins.
        /// </summary>
        public void InitializePlugins()
        {
            this.PluginList = new List<IIRCPlugin>();
            this.catalog.Catalogs.Add(new DirectoryCatalog(this.GetConfigurationPath(), "*.dll"));
            CompositionContainer compositionContainer = new CompositionContainer(this.catalog);
            compositionContainer.ComposeParts(this);
        }

        /// <summary>
        /// Retrieves the path to the plugins.
        /// </summary>
        /// <returns>The plugin path.</returns>
        public string GetConfigurationPath()
        {
            Configuration pluginConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
            AppSettingsSection pluginConfigAppSettings = (AppSettingsSection)pluginConfig.GetSection("appSettings");
            return pluginConfigAppSettings.Settings["PluginPath"].Value;
        }

        /// <summary>
        /// Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            List<string[]> pluginInfo = new List<string[]>();
            foreach (IIRCPlugin plugin in this.PluginList)
            {
                pluginInfo.Add(new string[] 
                {
                    plugin.Name,
                    plugin.Description,
                    plugin.Version
                });
            }

            return pluginInfo.ToArray();
        }

        /// <summary>
        /// Disposes of the plugin manager.
        /// </summary>
        public void Dispose()
        {
            this.catalog.Dispose();
            this.catalog = null;
            this.PluginList.Clear();
            this.PluginList = null;
        }

        #region IRC Client Plugin Methods

        /// <summary>
        /// Starts all IRC client plugins.
        /// </summary>
        public void Start()
        {
            foreach (IIRCPlugin plugin in this.PluginList)
            {
                plugin.RawMessageReceived += this.RawMessageReceived;
                plugin.Start();
            }
        }

        /// <summary>
        /// Stops all IRC client plugins.
        /// </summary>
        public void Stop()
        {
            foreach (IIRCPlugin plugin in this.PluginList)
            {
                plugin.Stop();
            }
        }

        /// <summary>
        /// Send a command to all IRC client plugins.
        /// </summary>
        /// <param name="command">Raw IRC command to send.</param>
        public void SendCommand(string command)
        {
            foreach (IIRCPlugin plugin in this.PluginList)
            {
                plugin.SendCommand(command);
            }
        }

        #endregion IRC Client Plugin Methods
    }
}