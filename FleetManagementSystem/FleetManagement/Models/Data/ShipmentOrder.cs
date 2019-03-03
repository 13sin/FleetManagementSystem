using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Models.Data
{
    public partial class ShipmentOrder
    {
        public int Id { get; set; }
        public int? ShipmentId { get; set; }
        public int? CarrierId { get; set; }
        public int? TruckId { get; set; }
        public int? TrailerId { get; set; }
        public int? DriverId { get; set; }
        public decimal? CarrierRate { get; set; }
        public string Notes { get; set; }

        public Carrier Carrier { get; set; }
        public Driver Driver { get; set; }
        public Shipment Shipment { get; set; }
        public Trailer Trailer { get; set; }
        public Truck Truck { get; set; }
    }
}
