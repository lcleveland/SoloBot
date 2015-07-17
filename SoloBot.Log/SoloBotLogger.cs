using System;

namespace SoloBot.Log
{
    /// <summary>
    ///     Instantiates a new logger using plugins.
    /// </summary>
    public class SoloBotLogger : IDisposable
    {
        /// <summary>
        ///     The logger.
        /// </summary>
        private static SoloBotLogger _singletonLogger;

        /// <summary>
        ///     Handles loading the logger plugins.
        /// </summary>
        private PluginHandler _pluginHandler;

        /// <summary>
        ///     Prevents a default instance of the <see cref="SoloBotLogger" /> class from being created.
        /// </summary>
        private SoloBotLogger()
        {
            _pluginHandler = new PluginHandler();
            _pluginHandler.InitializePlugins();
        }

        /// <summary>
        ///     Disposes of the SoloBotLogger object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Static method used to create a new SoloBotLogger object.
        /// </summary>
        /// <returns>SoloBotLogger object.</returns>
        public static SoloBotLogger GetLogger()
        {
            return _singletonLogger ?? (_singletonLogger = new SoloBotLogger());
        }

        /// <summary>
        ///     Sends a message to be logged by the logger plugins.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            _pluginHandler.Log(message);
        }

        /// <summary>
        ///     Disposes of the SoloBotLogger object.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_pluginHandler != null)
            {
                _pluginHandler.Dispose();
                _pluginHandler = null;
            }

            if (_singletonLogger != null)
            {
                _singletonLogger.Dispose();
                _singletonLogger = null;
            }
        }
    }
}