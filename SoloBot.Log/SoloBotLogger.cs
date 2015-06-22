namespace SoloBot.Log
{
    using System;

    /// <summary>
    /// Instantiates a new logger using plugins.
    /// </summary>
    public class SoloBotLogger : IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static SoloBotLogger singletonLogger;

        /// <summary>
        /// Handles loading the logger plugins.
        /// </summary>
        private PluginHandler pluginHandler;

        /// <summary>
        /// Prevents a default instance of the <see cref="SoloBotLogger" /> class from being created.
        /// </summary>
        private SoloBotLogger()
        {
            this.pluginHandler = new PluginHandler();
            this.pluginHandler.InitializePlugins();
        }

        /// <summary>
        /// Static method used to create a new SoloBotLogger object.
        /// </summary>
        /// <returns>SoloBotLogger object.</returns>
        public static SoloBotLogger GetLogger()
        {
            if (singletonLogger == null)
            {
                singletonLogger = new SoloBotLogger();
            }

            return singletonLogger;
        }

        /// <summary>
        /// Sends a message to be logged by the logger plugins.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            this.pluginHandler.Log(message);
        }

        /// <summary>
        /// Disposes of the SoloBotLogger object.
        /// </summary>
        public void Dispose()
        {
            this.pluginHandler = null;
            singletonLogger = null;
        }
    }
}