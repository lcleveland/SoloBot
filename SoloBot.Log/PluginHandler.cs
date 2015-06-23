namespace SoloBot.Log
{
    using SoloBot.Core.Abstract;
    using SoloBot.Log.Interface;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Configuration;

    /// <summary>
    /// Class that handles loading the logger plugins.
    /// </summary>
    public class PluginHandler : IPluginManager
    {
        /// <summary>
        /// Catalog used to load plugins.
        /// </summary>
        private AggregateCatalog catalog = new AggregateCatalog();

        /// <summary>
        /// Gets or sets the list that holds the loaded plugins.
        /// </summary>
        [ImportMany(typeof(ISoloBotLogger))]
        public List<ISoloBotLogger> PluginList { get; set; }

        /// <summary>
        /// Initializes the plugins.
        /// </summary>
        public void InitializePlugins()
        {
            this.PluginList = new List<ISoloBotLogger>();
            this.catalog.Catalogs.Add(new DirectoryCatalog(this.GetConfigurationPath(), "*.dll"));
            CompositionContainer compositionContainer = new CompositionContainer(this.catalog);
            compositionContainer.ComposeParts(this);
            foreach (ISoloBotLogger plugin in this.PluginList)
            {
                plugin.Initialize();
            }
        }

        /// <summary>
        /// Sends the log message to the plugins to handle.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void Log(string message)
        {
            foreach (ISoloBotLogger plugin in this.PluginList)
            {
                plugin.Log(message);
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
        /// Disposes of the plugin manager.
        /// </summary>
        public void Dispose()
        {
            this.catalog.Dispose();
            this.catalog = null;
            this.PluginList.Clear();
            this.PluginList = null;
        }

        /// <summary>
        /// Gets the name, description, and version of the loaded plugins.
        /// </summary>
        /// <returns>String array containing plugin information.</returns>
        public string[][] GetAllPluginInfo()
        {
            List<string[]> pluginInfo = new List<string[]>();
            foreach (ISoloBotLogger plugin in this.PluginList)
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
    }
}