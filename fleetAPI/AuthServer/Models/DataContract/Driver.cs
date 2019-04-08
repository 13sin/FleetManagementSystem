using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace  AuthServer.Models.DataContract
{
    public partial class Driver
    {

        public int Id { get; set; }
        public int? AddressId { get; set; }
        public int CarrierId { get; set; }
        public string UserId { get; set; }
        public string LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseState { get; set; }
        public Address Address { get; set; }
    }
}
