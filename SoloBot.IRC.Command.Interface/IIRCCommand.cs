using SoloBot.Core.Abstract;
using SoloBot.Core.Models;
using SoloBot.IRC.Interface;

namespace SoloBot.IRC.Command.Interface
{
    /// <summary>
    ///     Defines what an IRC command plugin should have.
    /// </summary>
    public interface IIrcCommand : IPlugin
    {
        /// <summary>
        ///     Gets the command that the plugin is triggered from.
        /// </summary>
        string Command { get; }

        /// <summary>
        ///     Receives and handles raw IRC commands
        /// </summary>
        /// <param name="sender">The IRC client who received the command.</param>
        /// <param name="command">The raw IRC command.</param>
        void ReceiveRawCommand(IIrcPlugin sender, IrcEventArgs command);
    }
}