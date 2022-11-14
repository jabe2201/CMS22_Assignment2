using CMS22_Assignment2.Contexts;
using CMS22_Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Services
{
    public class ProductServices
    {
        private readonly SqlContext _context;

        public ProductServices(SqlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductRequest>> GetAllAsync()
        {
            var products = new List<ProductRequest>();

            try
            {
                foreach (var product in await _context.Products.ToListAsync())
                {
                    products.Add(new ProductRequest
                    {
                        Id = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price
                    });
                }
                return products;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return products;
        }

        public async Task<ProductRequest> GetAsync(int id)
        {
            try
            {
                var productEntity = await _context.Products.FindAsync(id);
                if (productEntity == null)
                    return new ProductRequest();

                return new ProductRequest
                {
                    Id = productEntity.ProductId,
                    ProductName = productEntity.ProductName,
                    Price = productEntity.Price
                };
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new ProductRequest();
        }
    }
}
