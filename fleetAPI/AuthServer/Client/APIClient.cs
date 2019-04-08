using AuthServer.Models.DataContract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AuthServer.Client
{
    public class APIClient
    {
        private static readonly HttpClient client = new HttpClient();
        public static Address GetAddress(IFormCollection collection)
        {
            Address address = new Address();
            address.Name = collection["Name"];
            address.City = collection["City"];
            address.Email = collection["Email"];
            address.Phone = collection["Phone"];
            address.Postalcode = collection["Postalcode"];
            address.Province = collection["Province"];
            address.Streetname = collection["Streetname"];
            return address;
        }

        public static async Task<Customer> PostCustomer(Customer customer)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Customers", customer);
            String response =await httpResponse.Content.ReadAsStringAsync();
            Customer returncustomer  = JsonConvert.DeserializeObject<Customer>(response);

            //Debug.WriteLine(returncustomer.Address.Name);

            return returncustomer;
        }

        public static async Task<bool> DeleteCustomer(int customerId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage httpResponse = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Customers/"+customerId);
            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<Customer> GetCustomer(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage httpresponse = await client.GetAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Customers/"+id);
            string response = await httpresponse.Content.ReadAsStringAsync();
            Customer returncustomer = JsonConvert.DeserializeObject<Customer>(response);
            return returncustomer;
        }

        public static async Task<Customer> EditCustomer(Customer customer)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage httpResponse = await client.PutAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Customers/"+customer.Id, customer);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Customer returncustomer = JsonConvert.DeserializeObject<Customer>(response);
            return returncustomer;
        }

        public static async Task<Broker> PostBroker(Broker broker)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Broker API call " + broker.Address.Name);
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Brokers", broker);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Broker returnbroker = JsonConvert.DeserializeObject<Broker>(response);
            Debug.WriteLine("broker response "+response);
            //Debug.WriteLine(returncustomer.Address.Name);

            return returnbroker;
        }


        public static async Task<bool> DeleteBroker(int brokerId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage httpResponse = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Brokers/" + brokerId);
            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<Broker> GetBroker(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage httpresponse = await client.GetAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Brokers/" + id);
            string response = await httpresponse.Content.ReadAsStringAsync();
            Broker returnbroker = JsonConvert.DeserializeObject<Broker>(response);
            return returnbroker;
        }

        public static async Task<Broker> EditBroker(Broker broker)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Broker API call " + broker.Id);
            Debug.WriteLine("Broker API call " + broker.Address.Name);
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Brokers/" + broker.Id, broker);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Broker returnbroker = JsonConvert.DeserializeObject<Broker>(response);
            return returnbroker;
        }

        public static async Task<Carrier> PostCarrier(Carrier Carrier)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Carrier API call " + Carrier.Address.Name);
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Carriers", Carrier);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Carrier returnCarrier = JsonConvert.DeserializeObject<Carrier>(response);
            Debug.WriteLine("Carrier response " + response);
            //Debug.WriteLine(returncustomer.Address.Name);

            return returnCarrier;
        }

        public static async Task<bool> DeleteCarrier(int carrierId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage httpResponse = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Carriers/" + carrierId);
            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<Carrier> GetCarrier(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage httpresponse = await client.GetAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Carriers/" + id);
            string response = await httpresponse.Content.ReadAsStringAsync();
            Carrier returnCarrier = JsonConvert.DeserializeObject<Carrier>(response);
            return returnCarrier;
        }

        public static async Task<Carrier> EditCarrier(Carrier Carrier)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Carrier API call " + Carrier.Id);
            Debug.WriteLine("Carrier API call " + Carrier.Address.Name);
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Carriers/" + Carrier.Id, Carrier);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Carrier returnCarrier = JsonConvert.DeserializeObject<Carrier>(response);
            return returnCarrier;
        }

        public static async Task<Driver> PostDriver(Driver Driver)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Carrier API call " + Driver.Address.Name);
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Drivers", Driver);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Driver returnDriver = JsonConvert.DeserializeObject<Driver>(response);
            Debug.WriteLine("Carrier response " + response);
            Debug.WriteLine(returnDriver.Address.Name);

            return returnDriver;
        }

        public static async Task<bool> DeleteDriver(int driverid)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage httpResponse = await client.DeleteAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Drivers/" + driverid);
            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<Driver> GetDriver(int id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage httpresponse = await client.GetAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Drivers/" + id);
            string response = await httpresponse.Content.ReadAsStringAsync();
            Driver returnDriver = JsonConvert.DeserializeObject<Driver>(response);
            return returnDriver;
        }

        public static async Task<Driver> EditDriver(Driver Driver)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Driver API call " + Driver.Id);
            Debug.WriteLine("Driver API call " + Driver.Address.Name);
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync("http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/Drivers/" + Driver.Id, Driver);
            String response = await httpResponse.Content.ReadAsStringAsync();
            Driver returnDriver = JsonConvert.DeserializeObject<Driver>(response);
            return returnDriver;
        }
    }
}
