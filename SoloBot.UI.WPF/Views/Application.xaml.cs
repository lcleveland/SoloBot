using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using IrcMessageSharp;
using SoloBot.Core.Models;
using SoloBot.IRC;

namespace SoloBot.UI.WPF.Views
{
    /// <summary>
    ///     Interaction logic for Application.XAML
    /// </summary>
    public partial class Application : INotifyPropertyChanged
    {
        private readonly Client _client;
        private int _chatCount;
        private int _messagesReceived;

        public Application()
        {
            DataContext = this;
            _client = Client.GetClient();
            _client.RawMessageReceived += Client_RawMessageReceived;
            Closing += Application_Closing;
            InitializeComponent();
            MainInput.KeyUp += MainInput_KeyUp;
            Task.Factory.StartNew(() => _client.Start());
        }

        public int MessagesReceived
        {
            get { return _messagesReceived; }

            set
            {
                _messagesReceived = value;
                OnPropertyChanged("MessagesReceived");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textbox = (TextBox) sender;
                _client.SendCommand(textbox.Text);
                textbox.Text = string.Empty;
            }
        }

        private void Application_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Client_RawMessageReceived(object sender, IrcEventArgs e)
        {
            IrcMessage message;
            MessagesReceived++;
            try
            {
                IrcMessage.TryParse(e.Message, out message);
                Dispatcher.Invoke(delegate
                {
                    this.RawScreen.Text += e.Message + "\n\n";
                    if (!message.IsPrefixServer && message.Params.Count == 2)
                    {
                        this.ParseScreen.Text += message.Prefix.Substring(0, message.Prefix.IndexOf('!')) + ": " +
                                                 message.Params[1] + "\n\n";
                    }
                    this.RawScrollViewer.ScrollToBottom();
                    this.ParseScrollViewer.ScrollToBottom();
                    if (this._chatCount >= 250)
                    {
                        this.RawScreen.Text = string.Empty;
                        this.ParseScreen.Text = string.Empty;
                        this._chatCount = 0;
                    }
                });

                _chatCount++;
            }
            catch (Exception)
            {
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}