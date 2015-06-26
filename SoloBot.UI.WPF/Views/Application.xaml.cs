namespace SoloBot.UI.WPF.Views
{
    using IrcMessageSharp;
    using SoloBot.IRC;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Application.XAML
    /// </summary>
    public partial class Application : Window
    {
        private Client client;

        public Application()
        {
            this.client = Client.GetClient();
            this.client.RawMessageReceived += this.Client_RawMessageReceived;
            this.Closing += this.Application_Closing;
            this.InitializeComponent();
            this.MainInput.KeyUp += this.MainInput_KeyUp;
            Task.Factory.StartNew(() => this.client.Start());
        }

        private void MainInput_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textbox = (System.Windows.Controls.TextBox)sender;
                this.client.SendCommand(textbox.Text);
                textbox.Text = string.Empty;
            }
        }

        private void Application_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Client_RawMessageReceived(object sender, Core.Models.IRCEventArgs e)
        {
            IrcMessage message;
            try
            {
                IrcMessage.TryParse(e.Message, out message);
                Dispatcher.Invoke((Action)delegate
                {
                    this.MainScreen.Text += e.Message + "\n";
                    this.MainScrollViewer.ScrollToBottom();
                });
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}