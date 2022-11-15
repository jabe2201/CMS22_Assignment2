using CMS22_Assignment2.Contexts;
using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task CreateAsync(OrderEntity order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRowsAsync(ObservableCollection<OrderRowModel> orderRows, int orderId)
        {
            foreach(var row in orderRows)
            {
                var orderRow = new OrderRowEntity
                {
                    OrderId = orderId,
                    ProductId = row.ProductId,
                    Quantity = row.Quantity,
                    Price = row.Price
                };
                _context.Add(orderRow);
            }
            await _context.SaveChangesAsync();
        }
    }
}
