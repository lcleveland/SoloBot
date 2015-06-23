namespace SoloBot.Log.Plugins.TextLogger
{
    using SoloBot.Log.Interface;
    using System.ComponentModel.Composition;
    using System.IO;

    /// <summary>
    /// Plugin that logs to a text file.
    /// </summary>
    [Export(typeof(ISoloBotLogger))]
    public class Logger : ISoloBotLogger
    {
        /// <summary>
        /// Plugin name.
        /// </summary>
        private string name = "SoloBot Logger Plugin";

        /// <summary>
        /// Plugin description.
        /// </summary>
        private string description = "Plugin that logs to a text file.";

        /// <summary>
        /// Plugin version.
        /// </summary>
        private string version = "0.01";

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version
        {
            get
            {
                return this.version;
            }
        }

        /// <summary>
        /// Logs a message to the text file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            StreamWriter sw = File.AppendText("PluginLog.txt");
            sw.WriteLine(message);
            sw.Close();
        }

        /// <summary>
        /// Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
        }


        public void Initialize()
        {
        }
    }
}