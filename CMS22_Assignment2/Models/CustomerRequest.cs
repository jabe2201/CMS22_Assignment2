using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models
{
    public class CustomerRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? FullName 
        {
            get { return FullName; }
            set { FullName = $"{FirstName} {LastName}"; }
        }






    }
}
