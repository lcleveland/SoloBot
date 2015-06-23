namespace SoloBot.UI.Console
{
    using SoloBot.IRC;
    using SoloBot.Log;
    using System.Threading;

    public class Program
    {
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