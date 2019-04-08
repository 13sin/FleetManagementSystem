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
    public sealed partial class EditDriver : Page
    {
        public Driver selectedDriver = null;
        static HttpClient client = new HttpClient();
        public EditDriver()
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
            selectedDriver = (Driver)e.Parameter;
            dName.Text = selectedDriver.Address.Name;
            
            dPhoneNumber.Text = selectedDriver.Address.Phone;
            dEmail.Text = selectedDriver.Address.Email;
            dAddress.Text = selectedDriver.Address.Streetname;
            dCity.Text = selectedDriver.Address.City;
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
            Address carrierAddress = new Address { Id = selectedDriver.Carrier.Address.Id,  Name = "ABC trucking", Streetname = "162 West Point", City = "Scarborough", Email = "admin@abctrucking.com", Postalcode = "h0h0h0", Province = "ON", Phone = "6476081234" };
            Carrier carrier = new Carrier { Id = selectedDriver.Carrier.Id, AddressId = selectedDriver.Carrier.Address.Id, Address = carrierAddress, Ctpat = "5784878", Mc = "787878", Cvor = "784512478", Usdot = "784521" };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            Address driverAddress = new Address { Id= selectedDriver.Address.Id,  Name = dName.Text, Streetname = dAddress.Text, City = dCity.Text, Email = dEmail.Text, Postalcode = dZipCode.Text, Province = dZipCode.Text, Phone = dPhoneNumber.Text };
            Driver driver = new Driver { Id=selectedDriver.Id, CarrierId=carrier.Id, AddressId=selectedDriver.AddressId, Address = driverAddress, LicenseNumber = "7874-78478", LicenseType = "AZ", LicenseState = "Ontario", Carrier = carrier };

            string json = JsonConvert.SerializeObject(driver);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Drivers/" + selectedDriver.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Updated Driver";
            }
            Debug.WriteLine(response);
        }

        private void DriverUpdate_Click(object sender, RoutedEventArgs e)
        {
            RunAsync().GetAwaiter();
        }
    }
}
