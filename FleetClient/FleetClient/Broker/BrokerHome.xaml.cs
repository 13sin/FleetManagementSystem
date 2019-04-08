using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FleetClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrokerHome : Page
    {
        public BrokerHome()
        {
            this.InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //NewShipmentOrder newShipmentOrder = new NewShipmentOrder();
            Frame.Navigate(typeof(SendShipmentOrder));
        }

        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rootPivot.SelectedIndex == 0)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                int BrokerID = (int)localSettings.Values["BrokerID"];
                shipmentordergridcontrol.InitshipmentOrdergrid(BrokerID).GetAwaiter();
            }
        }

    }
}
