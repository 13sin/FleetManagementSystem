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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FleetClient
{
    public sealed partial class TrailerGrid : UserControl
    {
        static HttpClient client = new HttpClient();
        public TrailerGrid()
        {
            RunAsync().GetAwaiter();
            this.InitializeComponent();
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
            response = await client.GetAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trailers");

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                IEnumerable<Trailer> trucks = JsonConvert.DeserializeObject<IEnumerable<Trailer>>(json);
                dataGrid.ItemsSource = trucks;
                dataGrid.UpdateLayout();
                return;
            }
        }

        async Task RundeleteAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");

            //HttpContent content;
            HttpResponseMessage response;
            Trailer trailer = dataGrid.SelectedItem as Trailer;
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.DeleteAsync("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi/api/Trailers/" + trailer.Id);

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                RunAsync().GetAwaiter();
                success.Text = "Succesfully Deleted";
                success.Visibility = Visibility.Visible;
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(EditTrailer), dataGrid.SelectedItem as Trailer);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            RundeleteAsync().GetAwaiter();
            close_Click(sender, e);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectedItems.Clear();
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
            success.Visibility = Visibility.Collapsed;
        }
    }
}
