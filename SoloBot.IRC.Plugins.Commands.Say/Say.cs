namespace SoloBot.IRC.Plugins.Commands.Say
{
    using IrcMessageSharp;
    using SoloBot.IRC.Command.Interface;
    using SoloBot.IRC.Interface;
    using SoloBot.Plugins.Core.Models;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Example command plugin.
    /// </summary>
    [Export(typeof(IIRCCommand))]
    public class Say : IRCCommandPluginBase
    {
        /// <summary>
        /// Called by the plugin handler when it has an IRC command to handle.
        /// </summary>
        /// <param name="sender">The IRC client plugin that got the message.</param>
        /// <param name="command">The raw IRC message.</param>
        public override void ReceiveRawCommand(IIRCPlugin sender, string command)
        {
            IrcMessage message;
            if (!IrcMessage.TryParse(command, out message))
            {
                return;
            }

            foreach (var item in message.Params)
            {
                if (item.StartsWith("!say"))
                {
                    sender.SendCommand("privmsg " + sender.Channel + " :" + item.Substring(item.IndexOf(' ') + 1));
                }
            }
        }

        /// <summary>
        /// Sets up the plugin to run.
        /// </summary>
        public override void Initialize()
        {
            this.Name = "Say Plugin";
            this.Description = "Plugin that makes the client speak.";
            this.Version = "0.01";
            this.Command = "!say";
        }

        /// <summary>
        /// Disposes the plugin.
        /// </summary>
        public override void Dispose()
        {
        }
    }
}