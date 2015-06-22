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
        public IRCEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the IRC message.
        /// </summary>
        public string Message { get; set; }
    }
}