using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AuthServer.Models.DataContract
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Streetname { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Postalcode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
