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
    public sealed partial class EditShipment : Page
    {
        public Shipment selectedshipment = null;
        static HttpClient client = new HttpClient();
        public EditShipment()
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
            selectedshipment = (Shipment)e.Parameter;
            shipmentRate.Text = ""+selectedshipment.BrokerRate;
            commodity.Text = selectedshipment.Commodity;
            weight.Text = ""+selectedshipment.Weight;
            Freighttype.SelectedIndex = 1;
            cName.Text = selectedshipment.Customer.Address.Name;
            cPhoneNumber.Text = selectedshipment.Customer.Address.Phone;
            cEmail.Text = selectedshipment.Customer.Address.Email;
            cAddress.Text = selectedshipment.Customer.Address.Streetname;
            cCity.Text = selectedshipment.Customer.Address.City;
            cZipCode.Text = selectedshipment.Customer.Address.Postalcode;
            oName.Text = selectedshipment.Origin.Address.Name;
            oPhoneNumber.Text = selectedshipment.Origin.Address.Phone;
            oEmail.Text = selectedshipment.Origin.Address.Email;
            oAddress.Text = selectedshipment.Origin.Address.Streetname;
            oCity.Text = selectedshipment.Origin.Address.City;
            oZipCode.Text = selectedshipment.Origin.Address.Postalcode;
            OriginReferenceNumber.Text = selectedshipment.OriginApptNumber;
            destName.Text = selectedshipment.Destination.Address.Name;
            destPhoneNumber.Text = selectedshipment.Destination.Address.Phone;
            destEmail.Text = selectedshipment.Destination.Address.Email;
            destAddress.Text = selectedshipment.Destination.Address.Streetname;
            destCity.Text = selectedshipment.Destination.Address.City;
            destZipCode.Text = selectedshipment.Destination.Address.Postalcode;
            DestinationReferenceNumber.Text = selectedshipment.DestinationApptNumber;
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            RunAsync().GetAwaiter();
        }

        async Task RunAsync()
        {
            Address customerAddress = new Address { Id = selectedshipment.Customer.AddressId, Name = cName.Text, Streetname = cAddress.Text, City = cCity.Text, Email = cEmail.Text, Postalcode = cZipCode.Text, Province = cProvince.SelectedValue.ToString(), Phone = cPhoneNumber.Text };
            Address originAddress = new Address { Id = selectedshipment.Origin.Address.Id, Name = oName.Text, Streetname = oAddress.Text, City = oCity.Text, Email = oEmail.Text, Postalcode = oZipCode.Text, Province = oProvince.SelectedValue.ToString(), Phone = oPhoneNumber.Text };
            Address destinationAddress = new Address { Id = selectedshipment.Destination.Address.Id, Name = destName.Text, Streetname = destAddress.Text, City = destCity.Text, Email = destEmail.Text, Postalcode = destZipCode.Text, Province = destProvince.SelectedValue.ToString(), Phone = destPhoneNumber.Text };
            Customer customer = new Customer { Id = selectedshipment.Customer.Id, Address = customerAddress };
            Broker broker = new Broker { Id = selectedshipment.Broker.Id, AddressId = selectedshipment.Broker.AddressId, Address = customerAddress, Mc = "9652365" };
            Origin origin = new Origin { Id = selectedshipment.Origin.Id, AddressId = selectedshipment.Origin.AddressId, Address = originAddress };
            Destination destination = new Destination { Id = selectedshipment.Destination.Id, AddressId = selectedshipment.Destination.AddressId, Address = destinationAddress };
            DateTime odatetime = new DateTime(oDate.Date.Year, oDate.Date.Month, oDate.Date.Day, oTime.Time.Hours, oTime.Time.Minutes, oTime.Time.Seconds);
            DateTime destdatetime = new DateTime(destDate.Date.Year, destDate.Date.Month, destDate.Date.Day, destTime.Time.Hours, destTime.Time.Minutes, destTime.Time.Seconds);

            Shipment shipment = new Shipment
            {
                Id = selectedshipment.Id,
                BrokerId = broker.Id,
                CustomerId = customer.Id,
                DestinationId = destination.Id,
                OriginId = origin.Id,
                Broker = broker,
                Customer = customer,
                Origin = origin,
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
            response = await client.PutAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments/" + selectedshipment.Id, content);
            if (response.IsSuccessStatusCode)
            {
                success.Text = "Successfully Updated Shipment";
            }
            Debug.WriteLine(response);
        }

        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rootPivot.SelectedIndex == rootPivot.Items.Count - 1)
            {
                EditButton.Visibility = Visibility.Visible;
                NextButton.Content = "Back";
            }
            else
            {
                EditButton.Visibility = Visibility.Collapsed;
                NextButton.Content = "Next";
            }
        }
    }
}
