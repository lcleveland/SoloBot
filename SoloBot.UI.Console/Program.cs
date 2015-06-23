namespace SoloBot.UI.Console
{
    using SoloBot.IRC;
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
            new Thread(new ThreadStart(client.Start)).Start();
            while (true)
            {
                string cmd = System.Console.ReadLine();
                client.SendCommand(cmd);
            }
        }
    }
}