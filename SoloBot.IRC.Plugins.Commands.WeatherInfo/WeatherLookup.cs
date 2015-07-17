using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ByteBlocks.LocationServices;
using IrcMessageSharp;
using SoloBot.Core.Models;
using SoloBot.IRC.Command.Interface;
using SoloBot.IRC.Interface;
using SoloBot.IRC.Plugins.Commands.WeatherInfo.Properties;
using SoloBot.Plugins.Core.Models;

namespace SoloBot.IRC.Plugins.Commands.WeatherInfo
{
    /// <summary>
    ///     Plugin that retrieves the weather for a city.
    /// </summary>
    [Export(typeof (IIrcCommand))]
    public class WeatherLookup : IrcCommandPluginBase
    {
        /// <summary>
        ///     The weather API wrapper service.
        /// </summary>
        private WeatherService _weatherService;

        /// <summary>
        ///     Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            Name = "Weather Lookup";
            Description = "Finds the current weather for designated city.";
            Version = "0.01";
            Command = "!wl";

            var config = new WeatherServiceConfiguration {AppKey = Settings.Default.APIKey};
            _weatherService = new WeatherService(config);
        }

        /// <summary>
        ///     Receives the command from the plugin handler.
        /// </summary>
        /// <param name="sender">IRC client that sent the command.</param>
        /// <param name="command">IRC command.</param>
        public override void ReceiveRawCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            Task.Factory.StartNew(() => HanldeCommand(sender, command));
        }

        /// <summary>
        ///     Handles the command asynchronously
        /// </summary>
        /// <param name="sender">IRC client that sent the command.</param>
        /// <param name="command">IRC command.</param>
        private void HanldeCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            IrcMessage message;
            if (!IrcMessage.TryParse(command.Message, out message))
            {
                return;
            }

            foreach (var item in message.Params)
            {
                if (item.StartsWith(Command))
                {
                    try
                    {
                        var location = item.Substring(item.IndexOf(' ') + 1).Split(',');
                        var city = location[0];
                        var province = string.Empty;
                        var country = string.Empty;
                        if (location.Length == 2)
                        {
                            country = location[1];
                        }
                        else
                        {
                            province = location[1];
                            country = location[2];
                        }

                        var weather = _weatherService.GetCurrentWeather(city, province, country);
                        sender.SendCommand("privmsg " + command.Channel +
                                           " :" + weather.CityName + "," + country.ToUpper() +
                                           " (" + weather.Coordinate.Latitude + ", " + weather.Coordinate.Longitude +
                                           "): " +
                                           string.Format("{0}",
                                               Math.Round((weather.CurrentCondition.Temperature*(9.0/5.0)) + 32.0)) +
                                           "°" +
                                           " H: " + string.Format("{0}%", Math.Round(weather.CurrentCondition.Humidity)));
                    }
                    catch (Exception)
                    {
                        sender.SendCommand("privmsg " + command.Channel + " :Invalid Lookup");
                        return;
                    }
                }
                else if (item.StartsWith("?" + Command))
                {
                    sender.SendCommand("privmsg " + command.Channel +
                                       " :Command Help: !wl <city>,<state | province>,<country>: " + Description);
                }
            }
        }
    }
}