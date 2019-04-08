using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AuthServer.Models.DataContract
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int AddressId { get; set; }

        public Address Address { get; set; }
    }
}
