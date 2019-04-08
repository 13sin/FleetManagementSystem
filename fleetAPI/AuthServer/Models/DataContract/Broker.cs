using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AuthServer.Models.DataContract
{
    public partial class Broker
    {
        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string Mc { get; set; }

        public Address Address { get; set; }
    }
}
