using CMS22_Assignment2.Contexts;
using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Services
{
    public class OrderServices
    {
        private readonly SqlContext _context;

        public OrderServices(SqlContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(OrderModel orderModel)
        {
            var order = new OrderEntity
            {
                CustomerId = orderModel.CustomerId,
                Orderdate = orderModel.DateTime
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
