using SoloBot.Core.Models;
using SoloBot.IRC;

namespace SoloBot.UI.Console
{
    /// <summary>
    ///     Text interface to operate the IRC bot.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Main method to run the IRC bot.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            var client = Client.GetClient();
            client.RawMessageReceived += Client_RawMessageReceived;
            System.Console.WriteLine("Loaded IRC Client Plugins:");
            foreach (var item in client.GetIrcClientPluginInfo())
            {
                System.Console.WriteLine(PluginInfoToString(item));
            }

            System.Console.WriteLine("\nLoaded Command Plugins:");
            foreach (var item in client.GetCommandPluginInfo())
            {
                System.Console.WriteLine(PluginInfoToString(item));
            }

            System.Console.WriteLine();
            client.Start();
            while (true)
            {
                var cmd = System.Console.ReadLine();
                client.SendCommand(cmd);
            }
        }

        /// <summary>
        ///     Sends IRC client messages to the console.
        /// </summary>
        /// <param name="sender">IIRCPlugin object</param>
        /// <param name="e">IRC Event Arguments.</param>
        private static void Client_RawMessageReceived(object sender, IrcEventArgs e)
        {
            System.Console.WriteLine(e.Message);
        }

        /// <summary>
        ///     Formats the plugin info to a string.
        /// </summary>
        /// <param name="info">Plugin info to format.</param>
        /// <returns></returns>
        private static string PluginInfoToString(string[] info)
        {
            return "Name: " + info[0] + "\n\nDescription: " + info[1] + "\n\nVersion: " + info[2] + "\n";
        }
    }
}