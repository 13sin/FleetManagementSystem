using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//Data for Carrier Controller
namespace fleetAPI.Models.Data
{
    public partial class Carrier
    {
        public Carrier()
        {
            Driver = new HashSet<Driver>();
            ShipmentOrder = new HashSet<ShipmentOrder>();
            Trailer = new HashSet<Trailer>();
            Truck = new HashSet<Truck>();
        }

        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string Mc { get; set; }
        public string Usdot { get; set; }
        public string Cvor { get; set; }
        public string Ctpat { get; set; }

        public Address Address { get; set; }
        [JsonIgnore]
        public ICollection<Driver> Driver { get; set; }
        [JsonIgnore]
        public ICollection<ShipmentOrder> ShipmentOrder { get; set; }
        [JsonIgnore]
        public ICollection<Trailer> Trailer { get; set; }
        [JsonIgnore]
        public ICollection<Truck> Truck { get; set; }
    }
}
