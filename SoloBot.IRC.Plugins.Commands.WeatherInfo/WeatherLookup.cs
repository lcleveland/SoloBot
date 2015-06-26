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

    [Export(typeof(IIRCCommand))]
    public class WeatherLookup : IRCCommandPluginBase
    {
        private WeatherService weatherService;

        public override void Initialize()
        {
            this.Name = "Weather Lookup";
            this.Description = "Finds the current weather for designated city.";
            this.Version = "0.01";
            this.Command = "!wl";

            WeatherServiceConfiguration config = new WeatherServiceConfiguration();
            config.AppKey = Properties.Settings.Default.APIKey;
            weatherService = new WeatherService(config);
        }

        public override void ReceiveRawCommand(Interface.IIRCPlugin sender, Core.Models.IRCEventArgs command)
        {
            Task.Factory.StartNew(() => this.HanldeCommand(sender, command));
        }

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
                        var weather = weatherService.GetCurrentWeather(city, "", country);
                        sender.SendCommand("privmsg " + command.Channel + " :Current weather for " + weather.CityName + "," + country.ToUpper() + ": Temperature: " + String.Format("{0:0.00}", ((weather.CurrentCondition.Temperature * (9.0 / 5)) + 32.0)) + "F Humidity: " + weather.CurrentCondition.Humidity);
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