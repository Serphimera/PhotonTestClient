using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ExitGames.Client.Photon;

namespace PhotonTestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotonPeer Peer { get; set; }
        public bool PeerConnecting { get; set; }
        public bool PeerConnected { get; set; }
        public DispatcherTimer DispatcherTimer { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            PeerConnecting = false;
            PeerConnected = false;
            PhotonListener listener = new PhotonListener {Main = this};
            Peer = new PhotonPeer(listener, ConnectionProtocol.Udp);

            DisconnectButton.IsEnabled = false;
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += dispatcherTimer_tick;
            DispatcherTimer.Interval = new TimeSpan(0,0,0,1);
            StatusIndicator.Fill = new SolidColorBrush(Colors.Red);
            StatusTextBox.Foreground = new SolidColorBrush(Colors.Black);
        }


        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            StatusTextBox.AppendText("Trying to connect to " + IPAddress.Text + " on default port 5055...\n");
            ConnectButton.IsEnabled = false;
            if (Peer.Connect(IPAddress.Text + ":5055", "MasterServer"))
            {
                PeerConnecting = true;
                StatusTextBox.AppendText("Connection initialization in progress....\n");
                DispatcherTimer.Start();
            }
            else
            {
                StatusTextBox.AppendText("Connection initialization failed...\n");
            }
        }

        private void DisconnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            StatusTextBox.AppendText("Trying to disconnect from the server at " + IPAddress.Text + "...\n");
            Peer.Disconnect();
        }

        private void dispatcherTimer_tick(object sender, EventArgs e)
        {
            Service();
            if (PeerConnected)
            {
                StatusIndicator.Fill = new SolidColorBrush(Colors.Green);
            }
            else if (PeerConnecting && !PeerConnected)
            {
                StatusIndicator.Fill = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                StatusIndicator.Fill = new SolidColorBrush(Colors.Red);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Service()
        {
            Peer.Service();
        }

    }
}
