using System;
using SoloBot.Core.Abstract;
using SoloBot.Core.Models;

namespace SoloBot.IRC.Interface
{
    /// <summary>
    ///     Defines what an IRC client plugin should have.
    /// </summary>
    public interface IIrcPlugin : IPlugin
    {
        /// <summary>
        ///     Event that can be used to relay messages from the IRC client to the IRC command plugins.
        /// </summary>
        event EventHandler<IrcEventArgs> RawMessageReceived;

        /// <summary>
        ///     Starts the IRC client.
        /// </summary>
        void Start();

        /// <summary>
        ///     Starts the IRC client.
        /// </summary>
        void Stop();

        /// <summary>
        ///     Used to send raw IRC commands.
        /// </summary>
        /// <param name="command">The raw command to send.</param>
        void SendCommand(string command);

        /// <summary>
        ///     Event handler that relays the IRC clients message received event to the plugin systems message received event.
        /// </summary>
        /// <param name="sender">The object that is sending the event, usually (this).</param>
        /// <param name="e">The event arguments.</param>
        void OnRawMessageReceived(IIrcPlugin sender, IrcEventArgs e);
    }
}