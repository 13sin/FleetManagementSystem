using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace fleetAPI.Models.Data
{
    public partial class Broker
    {
        public Broker()
        {
            Shipment = new HashSet<Shipment>();
        }

        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string Mc { get; set; } 

        public Address Address { get; set; }
        [JsonIgnore]
        public ICollection<Shipment> Shipment { get; set; }
    }
}
