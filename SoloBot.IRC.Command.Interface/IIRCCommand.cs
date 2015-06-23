namespace SoloBot.IRC.Command.Interface
{
    using SoloBot.Core.Abstract;
    using SoloBot.Core.Models;
    using System;

    /// <summary>
    /// Defines what an IRC command plugin should have.
    /// </summary>
    public interface IIRCCommand : IPlugin
    {
        /// <summary>
        /// Event that receives the messages from the IRC client plugins.
        /// </summary>
        event EventHandler<IRCEventArgs> RawMessageReceived;

        /// <summary>
        /// Gets the command that the plugin is triggered from.
        /// </summary>
        string Command { get; }
    }
}