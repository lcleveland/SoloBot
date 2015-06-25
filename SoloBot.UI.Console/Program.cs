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
            Console.WriteLine("Loaded IRC Client Plugins:");
            foreach (var item in client.GetIRCClientPluginInfo())
            {
                Console.WriteLine(pluginInfoToString(item));
            }

            Console.WriteLine("\nLoaded Command Plugins:");
            foreach (var item in client.GetCommandPluginInfo())
            {
                Console.WriteLine(pluginInfoToString(item));
            }

            Console.WriteLine();
            client.Start();
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

        /// <summary>
        /// Formats the plugin info to a string.
        /// </summary>
        /// <param name="info">Plugin info to format.</param>
        /// <returns></returns>
        private static string pluginInfoToString(string[] info)
        {
            return "Name: " + info[0] + "\n\nDescription: " + info[1] + "\n\nVersion: " + info[2] + "\n";
        }
    }
}