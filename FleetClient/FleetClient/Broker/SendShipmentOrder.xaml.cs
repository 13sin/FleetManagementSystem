using fleetAPI.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class SendShipmentOrder : Page
    {
        static HttpClient client = new HttpClient();
        IEnumerable<ShipmentOrder> shipmentorders;
        IEnumerable<Carrier> carriers;
        Carrier selectedcarrier;
        ShipmentOrder selectedshipmentorder;
        //IEnumerable<Truck> trucks;
        //IEnumerable<Trailer> trailers;

        public SendShipmentOrder()
        {
            //RunTTAsync().GetAwaiter();
            InitCarrierList().GetAwaiter();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int brokerID = (int)localSettings.Values["brokerID"];
            InitShipmentOrderListById(brokerID).GetAwaiter();
            this.InitializeComponent();
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }



        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(selectedcarrier != null && selectedshipmentorder != null)
            {
                UpdateShipmentOrderAsync().GetAwaiter();
               
            }
            else
            {
                success.Text = "Please make selection";
            }
            

        }

        async Task UpdateShipmentOrderAsync()
        {
            //selectedshipment.Broker = selectedbroker;
            selectedshipmentorder.CarrierId = selectedcarrier.Id;
            selectedshipmentorder.CarrierRate = Convert.ToDecimal(carrierrate.Text);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(selectedshipmentorder);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/ShipmentOrders/"+ selectedshipmentorder.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Sent Shipment";
            }
            else
            {
                success.Text = "failed to Sent";
            }
            Debug.WriteLine(response);
        }

        private void carriersearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void carriersearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void carriersearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                selectedcarrier = (Carrier)args.SelectedItem;
                sender.Text = selectedcarrier.Address.Name;
            }
            else
            {
                // Use args.QueryText to determine what to do.
            }
        }


        async Task InitShipmentOrderListById(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/ShipmentOrders/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                shipmentorders = JsonConvert.DeserializeObject<IEnumerable<ShipmentOrder>>(json);
                shipmentOrderlist.ItemsSource = shipmentorders.Where(x => x.Shipment.BrokerId == id);
            }

        }
        async Task InitCarrierList()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Carriers/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                carriers = JsonConvert.DeserializeObject<IEnumerable<Carrier>>(json);
                carriersearch.ItemsSource = carriers;
            }
        }

        private void shipmentorderlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shipmentOrderlist.SelectedItem != null)
            {
                selectedshipmentorder = (ShipmentOrder)shipmentOrderlist.SelectedItem;
            }
        }


        //async Task RunTTAsync()
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
        //    string json;
        //    //HttpContent content;
        //    HttpResponseMessage response;

        //    Debug.WriteLine(client.DefaultRequestHeaders);
        //    response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trucks/");

        //    Debug.WriteLine(response);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        json = await response.Content.ReadAsStringAsync();
        //        trucks = JsonConvert.DeserializeObject<IEnumerable<Truck>>(json);
        //        truckid.ItemsSource = trucks.Select(t => t.Id);
        //    }

        //    response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trailers/");

        //    Debug.WriteLine(response);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        json = await response.Content.ReadAsStringAsync();
        //        trailers = JsonConvert.DeserializeObject<IEnumerable<Trailer>>(json);
        //        trailerid.ItemsSource = trailers.Select(t=>t.Id);
        //    }
    }
}
