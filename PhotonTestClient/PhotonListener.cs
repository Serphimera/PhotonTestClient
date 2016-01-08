using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ExitGames.Client.Photon;

namespace PhotonTestClient
{
    public class PhotonListener : IPhotonPeerListener
    {
        public MainWindow Main;

        public void DebugReturn(DebugLevel level, string message)
        {
            Main.Dispatcher.Invoke(
                () => { Main.StatusTextBox.AppendText("DebugReturn called with: " + level + " - " + message + "\n"); });
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            Main.Dispatcher.Invoke(() =>
            {
                Main.StatusTextBox.AppendText("OnOperationResponse called with: " + operationResponse.OperationCode +
                                              " - " + operationResponse.ReturnCode +
                " - " + operationResponse.DebugMessage + "\n")
                ;
            });
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            Main.Dispatcher.Invoke(
                () => Main.StatusTextBox.AppendText("OnStatusChanged called with " + statusCode + "\n"));

            if (statusCode == StatusCode.Connect)
            {
                Main.DisconnectButton.IsEnabled = true;
                Main.PeerConnected = true;
            }
            else
            {
                Main.DisconnectButton.IsEnabled = false;
                Main.ConnectButton.IsEnabled = true;
                Main.StatusIndicator.Fill = new SolidColorBrush(Colors.Red);
                Main.PeerConnecting = false;
                Main.PeerConnecting = false;
                Main.DispatcherTimer.Stop();
            }
        }

        public void OnEvent(EventData eventData)
        {
            Main.Dispatcher.Invoke(() => Main.StatusTextBox.AppendText("OnEvent called with " + eventData.Code));
        }
    }
}
