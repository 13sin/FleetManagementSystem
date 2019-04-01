using fleetAPI.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FleetClient
{
    public sealed partial class ShipmentDetails : ContentDialog
    {
        static HttpClient client = new HttpClient();
        Shipment Shipment { get; set; }
        int shipmentid;
        public ShipmentDetails(int id)
        {
            this.InitializeComponent();
            this.shipmentid = id;
            RunAsync().GetAwaiter();

        }

        private  void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        async Task RunAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            string json;
            //HttpContent content;
            HttpResponseMessage response;

            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Shipments/"+ shipmentid);

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                Shipment = JsonConvert.DeserializeObject<Shipment>(json);
                shiporder.Text = Shipment.Id.ToString();
                broker.Text = Shipment.Broker.Address.Name;
                origin.Text = Shipment.Origin.Address.Name;
                dest.Text = Shipment.Destination.Address.Name;
                rate.Text = "$"+Shipment.BrokerRate.ToString();
                return;
            }
            else
            {
                return;
            }
        }
    }
}
