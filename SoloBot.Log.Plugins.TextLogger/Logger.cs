namespace SoloBot.Log.Plugins.TextLogger
{
    using SoloBot.Log.Interface;
    using SoloBot.Plugins.Core.Models;
    using System.ComponentModel.Composition;
    using System.IO;

    /// <summary>
    /// Plugin that logs to a text file.
    /// </summary>
    [Export(typeof(ISoloBotLogger))]
    public class Logger : SoloBotLoggerPluginBase
    {
        /// <summary>
        /// Logs a message to the text file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void Log(string message)
        {
            StreamWriter sw = File.AppendText("PluginLog.txt");
            sw.WriteLine(message);
            sw.Close();
        }

        public override void Initialize()
        {
            this.Name = "SoloBot Logger";
            this.Description = "Plugin that logs to a text file.";
            this.Version = "0.01";
        }

        public override void Dispose()
        {
        }
    }
}