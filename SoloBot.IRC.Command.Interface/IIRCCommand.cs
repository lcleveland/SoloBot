namespace SoloBot.IRC.Command.Interface
{
    using SoloBot.Core.Abstract;
    using SoloBot.IRC.Interface;

    /// <summary>
    /// Defines what an IRC command plugin should have.
    /// </summary>
    public interface IIRCCommand : IPlugin
    {
        /// <summary>
        /// Gets the command that the plugin is triggered from.
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Receives and handles raw IRC commands
        /// </summary>
        /// <param name="sender">The IRC client who received the command.</param>
        /// <param name="command">The raw IRC command.</param>
        void ReceiveRawCommand(IIRCPlugin sender, string command);
    }
}