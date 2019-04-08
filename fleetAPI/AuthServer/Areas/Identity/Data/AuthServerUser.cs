using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AuthServerUser class
    public class AuthServerUser : IdentityUser
    {
        [PersonalData]
        public int brokerID { get; set; } = -1;
        [PersonalData]
        public int carrierID { get; set; } = -1;
        [PersonalData]
        public int customerID { get; set; } = -1;
    }
}
