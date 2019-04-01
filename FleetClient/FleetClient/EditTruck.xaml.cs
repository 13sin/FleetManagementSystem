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
    public sealed partial class EditTruck : Page
    {
        public Truck selectedtruck = null;
        static HttpClient client = new HttpClient();
        public EditTruck()
        {
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
            selectedtruck = (Truck)e.Parameter;
            license.Text = selectedtruck.LicensePlate;
            make.Text = selectedtruck.Make;
            model.Text = selectedtruck.Model;
            vinnum.Text = selectedtruck.Vin;
            trucktype.Text = selectedtruck.TruckType;
            year.Text = selectedtruck.Year;
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

        private void TruckUpdate_Click(object sender, RoutedEventArgs e)
        {
            RunAsync().GetAwaiter();
        }


        async Task RunAsync()
        {
            Address carrierAddress = new Address { Id = selectedtruck.Carrier.Address.Id, Name = "ABC trucking", Streetname = "162 West Point", City = "Scarborough", Email = "admin@abctrucking.com", Postalcode = "h0h0h0", Province = "ON", Phone = "6476081234" };
            Carrier carrier = new Carrier { Id = selectedtruck.Carrier.Id, AddressId = selectedtruck.Carrier.Address.Id, Address = carrierAddress, Ctpat = "5784878", Mc = "787878", Cvor = "784512478", Usdot = "784521" };
            Truck truck = new Truck { Id = selectedtruck.Id, Carrier = carrier, CarrierId = selectedtruck.Carrier.Id, LicensePlate = license.Text, Make = make.Text, Vin = vinnum.Text, TruckType = trucktype.Text, Year = year.Text, Model = model.Text, Location = carrier.Address.City + ", " + carrier.Address.Province };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(truck);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trucks/" + selectedtruck.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Updated Truck";
            }
            Debug.WriteLine(response);
        }
    }
}
