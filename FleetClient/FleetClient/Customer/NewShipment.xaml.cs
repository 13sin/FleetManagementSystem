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
    public sealed partial class NewShipment : Page
    {
        static HttpClient client = new HttpClient();


        public NewShipment()
        {
            //RunTTAsync().GetAwaiter();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
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
            RunAsync().GetAwaiter();

        }

        async Task RunAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            Address originAddress = new Address { Name = oName.Text, Streetname = oAddress.Text, City = oCity.Text, Email = oEmail.Text, Postalcode = oZipCode.Text, Province = oProvince.SelectedValue.ToString(), Phone = oPhoneNumber.Text };
            Address destinationAddress = new Address { Name = destName.Text, Streetname = destAddress.Text, City = destCity.Text, Email = destEmail.Text, Postalcode = destZipCode.Text, Province = destProvince.SelectedValue.ToString(), Phone = destPhoneNumber.Text };
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int customerid = (int)localSettings.Values["customerID"];
            Customer customer = await GetCustomerAsync(customerid);
            Origin origin = new Origin { Address = originAddress };
            Destination destination = new Destination { Address = destinationAddress };
            DateTime odatetime = new DateTime(oDate.Date.Year, oDate.Date.Month, oDate.Date.Day, oTime.Time.Hours, oTime.Time.Minutes, oTime.Time.Seconds);
            DateTime destdatetime = new DateTime(destDate.Date.Year, destDate.Date.Month, destDate.Date.Day, destTime.Time.Hours, destTime.Time.Minutes, destTime.Time.Seconds);

            Shipment shipment = new Shipment
            {
                Origin = origin,
                CustomerId = customer.Id,
                Destination = destination,
                Commodity = commodity.Text,
                BrokerRate = decimal.Parse(shipmentRate.Text),
                EquipmentType = "43\" Trailer",
                FreightType = Freighttype.SelectedValue.ToString(),
                Weight = double.Parse(weight.Text),
                DestinationApptNumber = DestinationReferenceNumber.Text,
                OriginApptNumber = OriginReferenceNumber.Text,
                Notes = "note for shipment",
                DestinationApptDatetime = destdatetime,
                OriginApptDatetime = odatetime
            };
           
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(shipment);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PostAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments", content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Created Shipment";
            }
            Debug.WriteLine(response);
        }

            private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rootPivot.SelectedIndex == rootPivot.Items.Count - 1)
            {
                SaveButton.Visibility = Visibility.Visible;
                NextButton.Content = "Back";
            }
            else
            {
                SaveButton.Visibility = Visibility.Collapsed;
                NextButton.Content = "Next";
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Click" + rootPivot.SelectedIndex);
            if (rootPivot.SelectedIndex < rootPivot.Items.Count - 1)
            {
                // If not at the last item, go to the next one.
                rootPivot.SelectedIndex += 1;
            }
            else
            {
                // The last PivotItem is selected, so loop around to the first item.
                rootPivot.SelectedIndex = 0;
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

        //}

        async Task<Customer> GetCustomerAsync(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Customers/"+id);
            json = await response.Content.ReadAsStringAsync();
            Customer c = JsonConvert.DeserializeObject<Customer>(json);
            return c;
        }
    }
}
