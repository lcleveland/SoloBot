using SoloBot.Core.Abstract;

namespace SoloBot.Log.Interface
{
    /// <summary>
    ///     Defines a logging plugin.
    /// </summary>
    public interface ISoloBotLogger : IPlugin
    {
        /// <summary>
        ///     Logs a message send to the plugin.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);
    }
}