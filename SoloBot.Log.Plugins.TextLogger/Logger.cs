using System.ComponentModel.Composition;
using System.IO;
using SoloBot.Log.Interface;
using SoloBot.Plugins.Core.Models;

namespace SoloBot.Log.Plugins.TextLogger
{
    /// <summary>
    ///     Plugin that logs to a text file.
    /// </summary>
    [Export(typeof (ISoloBotLogger))]
    public class Logger : SoloBotLoggerPluginBase
    {
        /// <summary>
        ///     Logs a message to the text file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void Log(string message)
        {
            var sw = File.AppendText("PluginLog.txt");
            sw.WriteLine(message);
            sw.Close();
        }

        /// <summary>
        ///     Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            Name = "SoloBot Logger";
            Description = "Plugin that logs to a text file.";
            Version = "0.01";
        }
    }
}