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
    public sealed partial class CarrierHome : Page
    {
        public CarrierHome()
        {
            this.InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //NewShipmentOrder newShipmentOrder = new NewShipmentOrder();
            Frame.Navigate(typeof(AssignShipmentOrder));
        }

        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rootPivot.SelectedIndex == 0)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                int CarrierID = (int)localSettings.Values["CarrierID"];
                carriershipmentordergridcontrol.InitcarriershipmentOrdergrid(CarrierID).GetAwaiter();
            }
        }

        private void newtruck_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewTruck));
        }

        private void newtrailer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewTrailer));
        }
    }
}
