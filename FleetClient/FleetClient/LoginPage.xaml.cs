using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FleetClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public class login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RememberMe { get; set; }
    }
    public sealed partial class LoginPage : Page
    {
        static HttpClient client = new HttpClient();
        public LoginPage()
        {
            this.InitializeComponent();
        }


        async Task RunAsync()
        {
            //client.BaseAddress = new Uri("http://tamasdeep1624-eval-test.apigee.net/proxyfleetapi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            login login = new login { Email = username.Text, Password = password.Password, RememberMe = "true" };
            string json = JsonConvert.SerializeObject(login);
            Debug.WriteLine(json);
            HttpContent content;
            HttpResponseMessage response;
            content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PostAsync("http://feetauthserver.us-east-1.elasticbeanstalk.com/EmployeeApi/auth", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("From login page role " + response);
                dynamic resp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                string token = resp.token;
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["token"] = token;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = tokenHandler.ReadJwtToken(token);
                string role = (string)jwt.Payload["role"];
                string userid = (string)jwt.Payload["nameid"];
                await SetUserInfo(userid);
                Debug.WriteLine("From login page " + jwt.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault());
                Debug.WriteLine("From login page role " + role);
                if (role == "Customer")
                {
                    this.Frame.Navigate(typeof(CustomerHome));
                }else if(role == "Broker")
                {
                    this.Frame.Navigate(typeof(BrokerHome));
                }
                else if (role == "Carrier")
                {
                    this.Frame.Navigate(typeof(CarrierHome));
                }
                else
                {
                    success.Text = resp.Error;
                    SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 225, 0, 0));
                    success.Foreground = myBrush;
                }
                //this.Frame.Navigate(typeof(CustomerHome));
               
                
            }
            else
            {
                success.Text = "unable to login";
                SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 225, 0, 0));
                success.Foreground = myBrush;
            }
            Debug.WriteLine(response);
        }

        async Task SetUserInfo(string userid)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apikey", "NbqYQDjspLDvorREUZAnyHZyCC3GoPGs");
            client.DefaultRequestHeaders.Add("userid", userid);
            HttpResponseMessage response;
            Debug.WriteLine(client.DefaultRequestHeaders);
            response = await client.PostAsync("http://feetauthserver.us-east-1.elasticbeanstalk.com/EmployeeApi/GetUserInfo", null);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Setting user info");
                Debug.WriteLine("SetUserInfo " + response);
                dynamic resp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["firstname"] = (string)resp.employee.firstName;
                localSettings.Values["lastname"] = (string)resp.employee.lastName;
                localSettings.Values["brokerID"] = (int)resp.user.brokerID;
                localSettings.Values["CarrierID"] = (int)resp.user.carrierID;
                localSettings.Values["customerID"] = (int)resp.user.customerID;
            }
            else
            {
                Debug.WriteLine("user not found");
            }
            Debug.WriteLine(response);
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            RunAsync().GetAwaiter(); 
        }
    }
}
