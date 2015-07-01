namespace SoloBot.UI.WPF.Views
{
    using IrcMessageSharp;
    using SoloBot.IRC;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Application.XAML
    /// </summary>
    public partial class Application : Window, INotifyPropertyChanged
    {
        private Client client;
        private int chatCount = 0;
        private int messagesReceived = 0;

        public int MessagesReceived
        {
            get
            {
                return this.messagesReceived;
            }

            set
            {
                this.messagesReceived = value;
                this.OnPropertyChanged("MessagesReceived");
            }
        }

        public Application()
        {
            this.DataContext = this;
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
            this.MessagesReceived++;
            try
            {
                IrcMessage.TryParse(e.Message, out message);
                Dispatcher.Invoke((Action)delegate
                {
                    this.RawScreen.Text += e.Message + "\n\n";
                    if (!message.IsPrefixServer && message.Params.Count == 2)
                    {
                        this.ParseScreen.Text += message.Prefix.Substring(0, message.Prefix.IndexOf('!')) + ": " + message.Params[1] + "\n\n";
                    }
                    this.RawScrollViewer.ScrollToBottom();
                    this.ParseScrollViewer.ScrollToBottom();
                    if (this.chatCount >= 250)
                    {
                        this.RawScreen.Text = string.Empty;
                        this.ParseScreen.Text = string.Empty;
                        this.chatCount = 0;
                    }
                });

                this.chatCount++;
            }
            catch (Exception)
            {
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}