using fleetAPI.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FleetClient
{
    public sealed partial class SentShipmentOrderGrid : UserControl
    {
        
        static HttpClient client = new HttpClient();
        public SentShipmentOrderGrid()
        {
            InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int brokerID = (int) localSettings.Values["brokerID"];
            InitshipmentOrdergrid(brokerID).GetAwaiter();
           

        }
        public async Task InitshipmentOrdergrid(int id)
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/ShipmentOrders");
            
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                IEnumerable<ShipmentOrder> ShipmentOrders = JsonConvert.DeserializeObject<IEnumerable<ShipmentOrder>>(json);
                shipmentOrderdataGrid.ItemsSource = ShipmentOrders.Where(x=>x.Shipment.BrokerId == id && x.CarrierId != null);
                shipmentOrderdataGrid.UpdateLayout();
                return;
            }
            else
            {
                success.Text = "unable to load shipments Order";
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        async Task CancelAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");

            //HttpContent content;
            HttpResponseMessage response;
            ShipmentOrder shipmentorder = shipmentOrderdataGrid.SelectedItem as ShipmentOrder;
            Debug.WriteLine(client.DefaultRequestHeaders);
            Debug.WriteLine("shipment " + shipmentorder.ShipmentId);
            shipmentorder.CarrierId = null;
            string json = JsonConvert.SerializeObject(shipmentorder);
            Debug.WriteLine(json);
            HttpContent content;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PutAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/ShipmentOrders/" + shipmentorder.Id, content);

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                int brokerID = (int)localSettings.Values["brokerID"];
                await InitshipmentOrdergrid(brokerID);
                success.Text = "Successfully Canceled Shipment Order";
                success.Visibility = Visibility.Visible;
            }
        }

        private void shipmentdataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
            success.Visibility = Visibility.Collapsed;
        }


        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            await CancelAsync();
            close_Click(sender, e);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            shipmentOrderdataGrid.SelectedItems.Clear();
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
    }
}
