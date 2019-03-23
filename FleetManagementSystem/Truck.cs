using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace fleetAPI.Models.Data
{
    public partial class Truck
    {
        public Truck()
        {
            ShipmentOrder = new HashSet<ShipmentOrder>();
        }

        public int Id { get; set; }
        public int CarrierId { get; set; }
        public int? OdometerId { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Vin { get; set; }
        public string Location { get; set; }
        public string TruckType { get; set; }

        public Carrier Carrier { get; set; }
        [JsonIgnore]
        public ICollection<ShipmentOrder> ShipmentOrder { get; set; }
    }
}
