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
    public sealed partial class EditTrailer : Page
    {
        public Trailer selectedtrailer = null;
        static HttpClient client = new HttpClient();
        public EditTrailer()
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
            selectedtrailer = (Trailer)e.Parameter;
            license.Text = selectedtrailer.LicensePlate;
            make.Text = selectedtrailer.Make;
            model.Text = selectedtrailer.Model;
            vinnum.Text = selectedtrailer.Vin;
            trailertype.Text = selectedtrailer.TrailerType;
            year.Text = selectedtrailer.Year;
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

        async Task RunAsync()
        {
            Address carrierAddress = new Address { Id = selectedtrailer.Carrier.Address.Id, Name = "ABC trucking", Streetname = "162 West Point", City = "Scarborough", Email = "admin@abctrucking.com", Postalcode = "h0h0h0", Province = "ON", Phone = "6476081234" };
            Carrier carrier = new Carrier { Id = selectedtrailer.Carrier.Id, AddressId = selectedtrailer.Carrier.Address.Id, Address = carrierAddress, Ctpat = "5784878", Mc = "787878", Cvor = "784512478", Usdot = "784521" };
            Trailer trailer = new Trailer { Id = selectedtrailer.Id, Carrier = carrier, CarrierId = selectedtrailer.Carrier.Id, LicensePlate = license.Text, Make = make.Text, Vin = vinnum.Text, TrailerType = trailertype.Text, Year = year.Text, Model = model.Text, Location = carrier.Address.City + ", " + carrier.Address.Province };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json = JsonConvert.SerializeObject(trailer);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trailers/" + selectedtrailer.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Updated Trailer";
            }
            Debug.WriteLine(response);
        }

        private void TrailerkUpdate_Click(object sender, RoutedEventArgs e)
        {
            RunAsync().GetAwaiter();
        }
    }
}
