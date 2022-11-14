using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models
{
    public class OrderRowModel
    {
        public int OrCustomerId { get; set; }

        public string OrProductName { get; set; } = null!;

        public int OrProductId { get; set; }

        public decimal OrPrice { get; set; }

        public int OrQuantity { get; set; }

    }
}
