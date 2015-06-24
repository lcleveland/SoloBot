namespace SoloBot.UI.Console
{
    using SoloBot.Core.Models;
    using SoloBot.IRC;
    using System;
    using System.Threading;

    /// <summary>
    /// Text interface to operate the IRC bot.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method to run the IRC bot.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            Client client = Client.GetClient();
            client.RawMessageReceived += Client_RawMessageReceived;
            new Thread(new ThreadStart(client.Start)).Start();
            while (true)
            {
                string cmd = System.Console.ReadLine();
                client.SendCommand(cmd);
            }
        }

        /// <summary>
        /// Sends IRC client messages to the console.
        /// </summary>
        /// <param name="sender">IIRCPlugin object</param>
        /// <param name="e">IRC Event Arguments.</param>
        private static void Client_RawMessageReceived(object sender, IRCEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}