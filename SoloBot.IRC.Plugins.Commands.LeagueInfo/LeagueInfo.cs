namespace SoloBot.IRC.Plugins.Commands.LeagueInfo
{
    using IrcMessageSharp;
    using RiotSharp;
    using RiotSharp.StatsEndpoint;
    using SoloBot.Core.Models;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using SoloBot.Plugins.Core.Models;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    /// Plugins that displays some basic League of Legends information.
    /// </summary>
    [Export(typeof(IIRCCommand))]
    public class LeagueInfo : IRCCommandPluginBase
    {
        /// <summary>
        /// API client.
        /// </summary>
        private RiotApi api;

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            this.Name = "League Information";
            this.Description = "Finds information on specified League of Legends user account.";
            this.Version = "0.01";
            this.Command = "!li";

            this.api = RiotApi.GetInstance(Properties.Settings.Default.APIKey);
        }

        /// <summary>
        /// Handles received a command from the plugin handler.
        /// </summary>
        /// <param name="sender">The IRC client that got the message.</param>
        /// <param name="command">The command to check.</param>
        public override void ReceiveRawCommand(IIRCPlugin sender, IRCEventArgs command)
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
                    string accountName = item.Substring(item.IndexOf(' ') + 1);
                    try
                    {
                        var summoner = this.api.GetSummoner(Region.na, accountName);
                        var statSummaries = summoner.GetStatsSummaries();
                        var stats = statSummaries.FirstOrDefault(stat => stat.PlayerStatSummaryType == PlayerStatsSummaryType.RankedSolo5x5);
                        if (stats == null)
                        {
                            throw new RiotSharpException();
                        }
                        else
                        {
                            var wins = stats.Wins;
                            var losses = stats.Losses;
                            var totKills = stats.AggregatedStats.TotalChampionKills;
                            sender.SendCommand("privmsg " + command.Channel + " :" + accountName + " :" +
                                "Wins: " + wins +
                                " Losses: " + losses +
                                " Total Hero Kills: " + totKills);
                        }
                    }
                    catch (RiotSharpException)
                    {
                        sender.SendCommand("privmsg " + command.Channel + " :Invalid Command");
                        return;
                    }
                }
                else if (item.StartsWith("?" + this.Command))
                {
                    sender.SendCommand("privmsg " + command.Channel + " :!li <username>: " + this.Description);
                }
            }
        }

        /// <summary>
        /// Disposes of objects.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.api != null)
                {
                    this.api = null;
                }
            }
        }
    }
}