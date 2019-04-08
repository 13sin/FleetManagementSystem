using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fleetAPI.Models.Data
{
    public partial class Shipment
    {
        public Shipment()
        {
            ShipmentOrder = new HashSet<ShipmentOrder>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? BrokerId { get; set; }
        public int? OriginId { get; set; }
        public int? DestinationId { get; set; }
        public string OriginApptNumber { get; set; }

        [DateLessThan("DestinationApptDatetime")]
        public DateTime? OriginApptDatetime { get; set; }
        public string DestinationApptNumber { get; set; }

        public DateTime? DestinationApptDatetime { get; set; }
        public string FreightType { get; set; }
        public string Commodity { get; set; }
        public double? Weight { get; set; }
        public string EquipmentType { get; set; }
        public decimal? BrokerRate { get; set; }
        public string Notes { get; set; }

        public Broker Broker { get; set; }
        public Customer Customer { get; set; }
        public Destination Destination { get; set; }
        public Origin Origin { get; set; }

        [JsonIgnore]
        public ICollection<ShipmentOrder> ShipmentOrder { get; set; }
    }

    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
