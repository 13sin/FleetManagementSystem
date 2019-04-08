using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AuthServer.Models.DataContract
{
    public partial class Carrier
    {


        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string Mc { get; set; }
        public string Usdot { get; set; }
        public string Cvor { get; set; }
        public string Ctpat { get; set; }

        public Address Address { get; set; }
        
    }
}
