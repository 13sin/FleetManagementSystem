using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace fleetAPI.Models.Data
{
    public partial class Driver
    {
        public Driver()
        {
            ShipmentOrder = new HashSet<ShipmentOrder>();
        }

        public int Id { get; set; }
        public int? AddressId { get; set; }
        public int CarrierId { get; set; }
        public string LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseState { get; set; }

        public Address Address { get; set; }
        public Carrier Carrier { get; set; }
        [JsonIgnore]
        public ICollection<ShipmentOrder> ShipmentOrder { get; set; }
    }
}
