﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        [Column (TypeName ="money")]
        public decimal Price { get; set; }

        public ICollection<OrderRowEntity>? OrderRows { get; set; }
    }
}
