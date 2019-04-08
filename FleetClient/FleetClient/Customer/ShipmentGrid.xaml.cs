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
    public sealed partial class ShipmentGrid : UserControl
    {
        
        static HttpClient client = new HttpClient();
        public ShipmentGrid()
        {
            InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int customerid = (int) localSettings.Values["customerID"];
            Initshipmentgrid(customerid).GetAwaiter();
           

        }
        public async Task Initshipmentgrid(int id)
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments");
            
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                IEnumerable<Shipment> Shipments = JsonConvert.DeserializeObject<IEnumerable<Shipment>>(json);
                shipmentdataGrid.ItemsSource = Shipments.Where(x=>x.CustomerId == id);
                shipmentdataGrid.UpdateLayout();
                return;
            }
            else
            {
                success.Text = "unable to load shipments";
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        async Task RundeleteAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            
            //HttpContent content;
            HttpResponseMessage response;
            Shipment shipment = shipmentdataGrid.SelectedItem as Shipment;
            Debug.WriteLine("shipment broker id " + shipment.BrokerId);
            if (shipment.BrokerId == null)
            {
                Debug.WriteLine(client.DefaultRequestHeaders);
                Debug.WriteLine("shipment" + shipment.Id);
                response = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Shipments/" + shipment.Id);

                Debug.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    int customerid = (int)localSettings.Values["customerID"];
                    await Initshipmentgrid(customerid);
                    success.Text = "Successfully Deleted";
                    success.Visibility = Visibility.Visible;
                }
            }
            else
            {
                success.Text = "Please cancel shipment before deleting";
                success.Visibility = Visibility.Visible;
            }
        }

        private void shipmentdataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
            success.Visibility = Visibility.Collapsed;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(EditShipment), shipmentdataGrid.SelectedItem as Shipment);
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            await RundeleteAsync();
            close_Click(sender, e);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            shipmentdataGrid.SelectedItems.Clear();
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
    }
}
