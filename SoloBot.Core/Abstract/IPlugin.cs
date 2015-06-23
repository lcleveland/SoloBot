namespace SoloBot.Core.Abstract
{
    using System;

    /// <summary>
    /// Defines the base of what a plugin should have.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        void Initialize();
    }
}