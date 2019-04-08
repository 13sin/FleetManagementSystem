using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthServer.Models
{
    public partial class EmployeeInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [NotMapped]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
