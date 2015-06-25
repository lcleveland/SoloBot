namespace SoloBot.IRC.Command
{
    using SoloBot.Core.Abstract;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Configuration;

    /// <summary>
    /// Manages the IRC client command plugins.
    /// </summary>
    public class PluginHandler : IPluginHandler
    {
        /// <summary>
        /// Catalog used to load plugins.
        /// </summary>
        private AggregateCatalog catalog = new AggregateCatalog();

        /// <summary>
        /// Gets or sets the list of plugins that are loaded.
        /// </summary>
        [ImportMany(typeof(IIRCCommand))]
        public List<IIRCCommand> PluginList { get; set; }

        /// <summary>
        /// Initializes the plugins.
        /// </summary>
        public void InitializePlugins()
        {
            this.PluginList = new List<IIRCCommand>();
            this.catalog.Catalogs.Add(new DirectoryCatalog(this.GetConfigurationPath(), "*.dll"));
            CompositionContainer compositionContainer = new CompositionContainer(this.catalog);
            compositionContainer.ComposeParts(this);
            foreach (IIRCCommand plugin in this.PluginList)
            {
                plugin.Initialize();
            }
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
        /// Distributes the commands to the command plugins.
        /// </summary>
        /// <param name="sender">The IRC client sending the command.</param>
        /// <param name="command">The raw IRC command.</param>
        public void SendCommand(IIRCPlugin sender, IRCEventArgs command)
        {
            foreach (IIRCCommand plugin in this.PluginList)
            {
                plugin.ReceiveRawCommand(sender, command);
            }
        }

        /// <summary>
        /// Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            List<string[]> pluginInfo = new List<string[]>();
            foreach (IIRCCommand plugin in this.PluginList)
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the plugin manager
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.catalog != null)
                {
                    this.catalog.Dispose();
                    this.catalog = null;
                }

                if (this.PluginList != null)
                {
                    this.PluginList.Clear();
                    this.PluginList = null;
                }
            }
        }
    }
}