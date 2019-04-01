using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace fleetAPI.Models.Data
{
    public partial class Origin
    {
        public Origin()
        {
            Shipment = new HashSet<Shipment>();
        }

        public int Id { get; set; }
        public int? AddressId { get; set; }

        public Address Address { get; set; }
        [JsonIgnore]
        public ICollection<Shipment> Shipment { get; set; }
    }
}
