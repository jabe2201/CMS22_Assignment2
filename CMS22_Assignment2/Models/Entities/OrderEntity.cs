using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models.Entities
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime Orderdate { get; set; }
        [Column (TypeName ="money")]
        public decimal OrderSum { get; set; }

        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;
        public ICollection<OrderRowEntity>? OrderRows { get; set; }
    }
}
