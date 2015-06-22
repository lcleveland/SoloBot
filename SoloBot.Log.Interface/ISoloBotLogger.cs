namespace SoloBot.Log.Interface
{
    using SoloBot.Core.Abstract;

    /// <summary>
    /// Defines a logging plugin.
    /// </summary>
    public interface ISoloBotLogger : IPlugin
    {
        /// <summary>
        /// Logs a message send to the plugin.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);
    }
}