using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Models.Data
{
    public partial class Address
    {
        public Address()
        {
            Broker = new HashSet<Broker>();
            Carrier = new HashSet<Carrier>();
            Customer = new HashSet<Customer>();
            Destination = new HashSet<Destination>();
            Driver = new HashSet<Driver>();
            Origin = new HashSet<Origin>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Streetname { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Postalcode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public ICollection<Broker> Broker { get; set; }
        [JsonIgnore]
        public ICollection<Carrier> Carrier { get; set; }
        [JsonIgnore]
        public ICollection<Customer> Customer { get; set; }
        [JsonIgnore]
        public ICollection<Destination> Destination { get; set; }
        [JsonIgnore]
        public ICollection<Driver> Driver { get; set; }
        [JsonIgnore]
        public ICollection<Origin> Origin { get; set; }
    }
}
