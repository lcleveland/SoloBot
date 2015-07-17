using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using SoloBot.Core.Abstract;
using SoloBot.Log.Interface;

namespace SoloBot.Log
{
    /// <summary>
    ///     Class that handles loading the logger plugins.
    /// </summary>
    public class PluginHandler : IPluginHandler
    {
        /// <summary>
        ///     Catalog used to load plugins.
        /// </summary>
        private AggregateCatalog _catalog = new AggregateCatalog();

        /// <summary>
        ///     Gets or sets the list that holds the loaded plugins.
        /// </summary>
        [ImportMany(typeof (ISoloBotLogger))]
        public List<ISoloBotLogger> PluginList { get; set; }

        /// <summary>
        ///     Initializes the plugins.
        /// </summary>
        public void InitializePlugins()
        {
            PluginList = new List<ISoloBotLogger>();
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
        ///     Disposes of the plugin manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
        ///     Sends the log message to the plugins to handle.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void Log(string message)
        {
            foreach (var plugin in PluginList)
            {
                plugin.Log(message);
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
    }
}