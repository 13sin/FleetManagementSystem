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
    public sealed partial class AssignShipmentOrder : Page
    {
        static HttpClient client = new HttpClient();
        IEnumerable<ShipmentOrder> shipmentorders;
        IEnumerable<Driver> drivers;
        IEnumerable<Truck> trucks;
        IEnumerable<Trailer> trailer;
        Driver selecteddriver;
        ShipmentOrder selectedshipmentorder;
        Truck selectedtruck;
        Trailer selectedtrailer;
        //IEnumerable<Truck> trucks;
        //IEnumerable<Trailer> trailers;

        public AssignShipmentOrder()
        {
            //RunTTAsync().GetAwaiter();
            
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int CarrierID = (int)localSettings.Values["CarrierID"];
            InitDriverList(CarrierID).GetAwaiter();
            InitShipmentOrderListById(CarrierID).GetAwaiter();
            InitTruckList(CarrierID).GetAwaiter();
            InitTrailerList(CarrierID).GetAwaiter();
            InitDriverList(CarrierID).GetAwaiter();
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
            if(selecteddriver != null && selectedshipmentorder != null && selectedtruck != null && selectedtrailer != null)
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
            selectedshipmentorder.DriverId = selecteddriver.Id;
            selectedshipmentorder.TruckId = selectedtruck.Id;
            selectedshipmentorder.TrailerId = selectedtrailer.Id;

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
                success.Text = "Driver Successfully Assigned";
            }
            else
            {
                success.Text = "failed to Sent";
            }
            Debug.WriteLine(response);
        }

        private void driversearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void driversearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void driversearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                selecteddriver = (Driver)args.SelectedItem;
                sender.Text = selecteddriver.Address.Name;
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
                shipmentOrderlist.ItemsSource = shipmentorders.Where(x => x.CarrierId == id && x.DriverId == null);
            }

        }
        async Task InitDriverList(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Drivers/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                drivers = JsonConvert.DeserializeObject<IEnumerable<Driver>>(json);
                driversearch.ItemsSource = drivers.Where(x=>x.CarrierId == id);
            }
        }

        async Task InitTruckList(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trucks/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                trucks = JsonConvert.DeserializeObject<IEnumerable<Truck>>(json);
                truckslist.ItemsSource = trucks.Where(x => x.CarrierId == id);
            }
        }

        async Task InitTrailerList(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trailers/");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                trailer = JsonConvert.DeserializeObject<IEnumerable<Trailer>>(json);
                trailerslist.ItemsSource = trailer.Where(x => x.CarrierId == id);
            }
        }

        private void shipmentorderlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shipmentOrderlist.SelectedItem != null)
            {
                selectedshipmentorder = (ShipmentOrder)shipmentOrderlist.SelectedItem;
            }
        }

        private void truckslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (truckslist.SelectedItem != null)
            {
                selectedtruck = (Truck)truckslist.SelectedItem;
            }
        }

        private void trailerslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (trailerslist.SelectedItem != null)
            {
                selectedtrailer = (Trailer)trailerslist.SelectedItem;
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
