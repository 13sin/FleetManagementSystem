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
    public sealed partial class InProgressShipmentGrid : UserControl
    {
        
        static HttpClient client = new HttpClient();
        public InProgressShipmentGrid()
        {
            InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int customerid = (int) localSettings.Values["customerID"];
            RunAsync(customerid).GetAwaiter();
           

        }
        async Task RunAsync(int id)
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
                inprogrssshipmentdataGrid.ItemsSource = Shipments.Where(x=>x.CustomerId == id && x.BrokerId != null);
                inprogrssshipmentdataGrid.UpdateLayout();
                return;
            }
            else
            {
                return;
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
            Shipment shipment = inprogrssshipmentdataGrid.SelectedItem as Shipment;
            Debug.WriteLine(client.DefaultRequestHeaders);
            Debug.WriteLine("shipment "+ shipment.Id);
            shipment.BrokerId = null;
            string json = JsonConvert.SerializeObject(shipment);
            Debug.WriteLine(json);
            HttpContent content;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PutAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Shipments/" + shipment.Id, content);

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                int customerid = (int)localSettings.Values["customerID"];
                //await RunAsync(customerid);
                success.Text = "Successfully Canceled Shipment";
                success.Visibility = Visibility.Visible;
            }
        }

        async Task ShipmentOrderdeleteAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");

            Debug.WriteLine("deleting shipment order....");
            Shipment selectshipment = (Shipment)inprogrssshipmentdataGrid.SelectedItem;
            Debug.WriteLine("selected shipment id "+ selectshipment.Id);
            HttpResponseMessage response;
            IEnumerable<ShipmentOrder> ShipmentOrders = null;
            response = await client.GetAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/ShipmentOrders/");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ShipmentOrders = JsonConvert.DeserializeObject<IEnumerable<ShipmentOrder>>(json);

            }
            
            ShipmentOrder deleteshipmentOrder = ShipmentOrders.Where(x => x.ShipmentId == selectshipment.Id).FirstOrDefault();
            Debug.WriteLine("deleted shipent id " + deleteshipmentOrder.Id);
            response = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/ShipmentOrders/" + deleteshipmentOrder.Id);

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                int customerid = (int)localSettings.Values["customerID"];
                await RunAsync(customerid);
                success.Text = "Successfully Deleted";
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
            navigationFrame.Navigate(typeof(EditShipment), inprogrssshipmentdataGrid.SelectedItem as Shipment);
        }

        private async void cancel_Click(object sender, RoutedEventArgs e)
        {
            await CancelAsync();
            await ShipmentOrderdeleteAsync();
            close_Click(sender, e);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            inprogrssshipmentdataGrid.SelectedItems.Clear();
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
    }
}
