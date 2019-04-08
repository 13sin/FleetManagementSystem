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
    public sealed partial class NewShipmentOrder : Page
    {
        static HttpClient client = new HttpClient();
        IEnumerable<Shipment> shipments;
        IEnumerable<Broker> brokers;
        Broker selectedbroker;
        Shipment selectedshipment;
        //IEnumerable<Truck> trucks;
        //IEnumerable<Trailer> trailers;

        public NewShipmentOrder()
        {
            //RunTTAsync().GetAwaiter();
            InitBrokerList().GetAwaiter();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int customerid = (int)localSettings.Values["customerID"];
            InitShipmentListById(customerid).GetAwaiter();
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
            if(selectedbroker != null && selectedshipment != null)
            {
               UpdateShipmentAsync().GetAwaiter();
               SaveAsync().GetAwaiter();
            }
            else
            {
                success.Text = "Please make selection";
            }
            

        }

        async Task UpdateShipmentAsync()
        {
            //selectedshipment.Broker = selectedbroker;
            selectedshipment.BrokerId = selectedbroker.Id;
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(selectedshipment);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments/"+selectedshipment.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Updated Shipment";
            }
            else
            {
                success.Text = "failed to update";
            }
            Debug.WriteLine(response);
        }

        async Task SaveAsync()
        {
            //selectedshipment.Broker = selectedbroker;
            selectedshipment.BrokerId = selectedbroker.Id;
            ShipmentOrder shipmentOrder = new ShipmentOrder { ShipmentId= selectedshipment.Id,  Notes = "this is note for shipment order" };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(shipmentOrder);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PostAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/ShipmentOrders", content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Created Shipment Order";
            }
            else
            {
                success.Text = "failed to save";
            }
            Debug.WriteLine(response);
        }


        private void brokersearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void brokersearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void brokersearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                selectedbroker = (Broker)args.SelectedItem;
                sender.Text = selectedbroker.Address.Name;
            }
            else
            {
                // Use args.QueryText to determine what to do.
            }
        }


        async Task InitShipmentListById(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                shipments = JsonConvert.DeserializeObject<IEnumerable<Shipment>>(json);
                shipmentlist.ItemsSource = shipments.Where(x => x.CustomerId == id && x.BrokerId== null);
            }

        }
        async Task InitBrokerList()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Brokers/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                brokers = JsonConvert.DeserializeObject<IEnumerable<Broker>>(json);
                brokersearch.ItemsSource = brokers;
            }
        }

        private void shipmentlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shipmentlist.SelectedItem != null)
            {
                selectedshipment = (Shipment)shipmentlist.SelectedItem;
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
