using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } 
        public decimal OrderSum { get; set; }
        public int CustomerId { get; set; }


    }
}
