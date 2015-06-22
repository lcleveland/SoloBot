namespace SoloBot.Core.Abstract
{
    using System;

    /// <summary>
    /// Defines the base of what a plugin should have.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Gets or sets the plugin name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the plugin description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
        string Version { get; set; }
    }
}