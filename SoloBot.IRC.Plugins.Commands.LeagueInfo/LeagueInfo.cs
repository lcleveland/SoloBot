using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using IrcMessageSharp;
using RiotSharp;
using RiotSharp.StatsEndpoint;
using SoloBot.Core.Models;
using SoloBot.IRC.Command.Interface;
using SoloBot.IRC.Interface;
using SoloBot.IRC.Plugins.Commands.LeagueInfo.Properties;
using SoloBot.Plugins.Core.Models;

namespace SoloBot.IRC.Plugins.Commands.LeagueInfo
{
    /// <summary>
    ///     Plugin that displays some basic League of Legends information.
    /// </summary>
    [Export(typeof (IIrcCommand))]
    public class LeagueInfo : IrcCommandPluginBase
    {
        /// <summary>
        ///     API client.
        /// </summary>
        private RiotApi _api;

        /// <summary>
        ///     Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            Name = "League Information";
            Description = "Finds information on specified League of Legends user account.";
            Version = "0.01";
            Command = "!li";

            _api = RiotApi.GetInstance(Settings.Default.APIKey);
        }

        /// <summary>
        ///     Handles received a command from the plugin handler.
        /// </summary>
        /// <param name="sender">The IRC client that got the message.</param>
        /// <param name="command">The command to check.</param>
        public override void ReceiveRawCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            Task.Factory.StartNew(() => HandleCommand(sender, command));
            // Handle messages in a different thread so we dont block the incoming messages.
        }

        /// <summary>
        ///     Disposes of objects.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _api = null;
            }
        }

        /// <summary>
        ///     Handles received a command from the plugin handler.
        /// </summary>
        /// <param name="sender">The IRC client that got the message.</param>
        /// <param name="command">The command to check.</param>
        private void HandleCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            try
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
                        var accountName = item.Substring(item.IndexOf(' ') + 1);
                        var summoner = _api.GetSummoner(Region.na, accountName);
                        var statSummaries = summoner.GetStatsSummaries();
                        var stats =
                            statSummaries.FirstOrDefault(
                                stat => stat.PlayerStatSummaryType == PlayerStatsSummaryType.RankedSolo5x5);
                        if (stats == null)
                        {
                            throw new Exception();
                        }
                        var wins = stats.Wins;
                        var losses = stats.Losses;
                        var totKills = stats.AggregatedStats.TotalChampionKills;
                        sender.SendCommand("privmsg " + command.Channel + " :" + accountName + " :" +
                                           "Wins: " + wins +
                                           " Losses: " + losses +
                                           " Total Hero Kills: " + totKills);
                    }
                    else if (item.StartsWith("?" + Command))
                    {
                        sender.SendCommand("privmsg " + command.Channel + " :Command Help: !li <username>: " +
                                           Description);
                    }
                }
            }
            catch (Exception)
            {
                sender.SendCommand("privmsg " + command.Channel + " :Invalid Lookup");
            }
        }
    }
}