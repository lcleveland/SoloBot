namespace SoloBot.Plugins.Core.Models
{
    using SoloBot.Log.Interface;

    public abstract class SoloBotLoggerPluginBase : ISoloBotLogger
    {
        private string name;
        private string description;
        private string version;

        public string Name
        {
            get
            {
                return this.name;
            }

            protected set
            {
                this.name = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            protected set
            {
                this.description = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }

            protected set
            {
                this.version = value;
            }
        }

        public abstract void Initialize();

        public abstract void Dispose();

        public abstract void Log(string message);
    }
}