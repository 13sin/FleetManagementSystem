using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetInvoiceManagement.Models
{
    public class Invoice
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Invoice Title")]
        [Remote("IsInvoiceTitleExists","Invoices",ErrorMessage = "Invoice Title already exists")]
        public string InvoiceTitle { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }
        
        [Required]
        [Display(Name = "Tax")]
        public double Tax { get; set; }

        [Display(Name = "Sales")]
        public double Sales { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
    }
}
