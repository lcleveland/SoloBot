namespace SoloBot.IRC.Plugins.Commands.WeatherInfo
{
    using ByteBlocks.LocationServices;
    using IrcMessageSharp;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using SoloBot.Plugins.Core.Models;
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    /// <summary>
    /// Plugin that retrieves the weather for a city.
    /// </summary>
    [Export(typeof(IIRCCommand))]
    public class WeatherLookup : IRCCommandPluginBase
    {
        /// <summary>
        /// The weather API wrapper service.
        /// </summary>
        private WeatherService weatherService;

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            this.Name = "Weather Lookup";
            this.Description = "Finds the current weather for designated city.";
            this.Version = "0.01";
            this.Command = "!wl";

            WeatherServiceConfiguration config = new WeatherServiceConfiguration();
            config.AppKey = Properties.Settings.Default.APIKey;
            this.weatherService = new WeatherService(config);
        }

        /// <summary>
        /// Receives the command from the plugin handler.
        /// </summary>
        /// <param name="sender">IRC client that sent the command.</param>
        /// <param name="command">IRC command.</param>
        public override void ReceiveRawCommand(Interface.IIRCPlugin sender, Core.Models.IRCEventArgs command)
        {
            Task.Factory.StartNew(() => this.HanldeCommand(sender, command));
        }

        /// <summary>
        /// Handles the command asynchronously
        /// </summary>
        /// <param name="sender">IRC client that sent the command.</param>
        /// <param name="command">IRC command.</param>
        private void HanldeCommand(IIRCPlugin sender, IRCEventArgs command)
        {
            IrcMessage message;
            if (!IrcMessage.TryParse(command.Message, out message))
            {
                return;
            }

            foreach (var item in message.Params)
            {
                if (item.StartsWith(this.Command))
                {
                    try
                    {
                        string[] location = item.Substring(item.IndexOf(' ') + 1).Split(',');
                        string city = location[0];
                        string country = location[1];
                        var weather = this.weatherService.GetCurrentWeather(city, string.Empty, country);
                        sender.SendCommand("privmsg " + command.Channel +
                            " :Current weather for " + weather.CityName + "," + country.ToUpper() +
                            ": Temperature: " + string.Format("{0}", Math.Round((weather.CurrentCondition.Temperature * (9.0 / 5.0)) + 32.0)) +
                            " Humidity: " + string.Format("{0}", Math.Round(weather.CurrentCondition.Humidity)));
                    }
                    catch (Exception)
                    {
                        sender.SendCommand("privmsg " + command.Channel + " :Invalid Lookup");
                        return;
                    }
                }
                else if (item.StartsWith("?" + this.Command))
                {
                    sender.SendCommand("privmsg " + command.Channel + " :Command Help: !wl <city>,<country>: " + this.Description);
                }
            }
        }
    }
}