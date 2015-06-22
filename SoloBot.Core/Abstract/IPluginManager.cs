namespace SoloBot.Core.Abstract
{
    using System;

    /// <summary>
    /// Defines what a plugin manager consists of.
    /// </summary>
    public interface IPluginManager : IDisposable
    {
        /// <summary>
        /// Loads the plugins.
        /// </summary>
        void InitializePlugins();

        /// <summary>
        /// Gets the path where the plugins are stored.
        /// </summary>
        /// <returns>Plugin path.</returns>
        string GetConfigurationPath();

        /// <summary>
        /// Gets a string array containing an array of the plugin info in string format.
        /// </summary>
        /// <returns>Plugin information array.</returns>
        string[][] GetAllPluginInfo();
    }
}