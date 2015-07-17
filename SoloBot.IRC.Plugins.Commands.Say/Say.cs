using System.ComponentModel.Composition;
using IrcMessageSharp;
using SoloBot.Core.Models;
using SoloBot.IRC.Command.Interface;
using SoloBot.IRC.Interface;
using SoloBot.Plugins.Core.Models;

namespace SoloBot.IRC.Plugins.Commands.Say
{
    /// <summary>
    ///     Example command plugin.
    /// </summary>
    [Export(typeof (IIrcCommand))]
    public class Say : IrcCommandPluginBase
    {
        /// <summary>
        ///     Called by the plugin handler when it has an IRC command to handle.
        /// </summary>
        /// <param name="sender">The IRC client plugin that got the message.</param>
        /// <param name="command">The raw IRC message.</param>
        public override void ReceiveRawCommand(IIrcPlugin sender, IrcEventArgs command)
        {
            IrcMessage message;
            if (!IrcMessage.TryParse(command.Message, out message))
            {
                return;
            }

            foreach (var item in message.Params)
            {
                if (item.StartsWith("!say"))
                {
                    sender.SendCommand("privmsg " + command.Channel + " :" + item.Substring(item.IndexOf(' ') + 1));
                }
            }
        }

        /// <summary>
        ///     Sets up the plugin to run.
        /// </summary>
        public override void Initialize()
        {
            Name = "Say Plugin";
            Description = "Plugin that makes the client speak.";
            Version = "0.01";
            Command = "!say";
        }
    }
}