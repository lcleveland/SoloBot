using System;

namespace SoloBot.Core.Models
{
    /// <summary>
    ///     Class that is used for the messaging events.
    /// </summary>
    public class IrcEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IrcEventArgs" /> class.
        /// </summary>
        /// <param name="message">The IRC message to pass.</param>
        /// <param name="channel">The IRC channel the message originates from.</param>
        public IrcEventArgs(string message, string channel)
        {
            Message = message;
            Channel = channel;
        }

        /// <summary>
        ///     Gets or sets the IRC message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the IRC channel the message originated from.
        /// </summary>
        public string Channel { get; set; }
    }
}