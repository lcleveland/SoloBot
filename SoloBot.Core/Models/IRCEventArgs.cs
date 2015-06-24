namespace SoloBot.Core.Models
{
    using System;

    /// <summary>
    /// Class that is used for the messaging events.
    /// </summary>
    public class IRCEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IRCEventArgs" /> class.
        /// </summary>
        /// <param name="message">The IRC message to pass.</param>
        /// <param name="channel">The IRC channel the message originates from.</param>
        public IRCEventArgs(string message, string channel)
        {
            this.Message = message;
            this.Channel = channel;
        }

        /// <summary>
        /// Gets or sets the IRC message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the IRC channel the message originated from.
        /// </summary>
        public string Channel { get; set; }
    }
}