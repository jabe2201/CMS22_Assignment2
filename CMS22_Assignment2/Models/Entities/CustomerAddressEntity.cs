using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models.Entities
{
    public class CustomerAddressEntity
    {
        [Key]
        public int AddressId { get; set; }
        public string StreetName { get; set; } = null!;
        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value.Trim().Replace(" ",""); }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value.Trim(); }
        }
    }
}
