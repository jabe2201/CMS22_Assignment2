using CMS22_Assignment2.Contexts;
using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CMS22_Assignment2.Services
{
    public class ProductServices
    {
        private readonly SqlContext _context;

        public ProductServices(SqlContext context)
        {
            _context = context;
        }

        public async void Create(ProductRequest productRequest)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.ProductName.ToLower() == productRequest.ProductName.ToLower());
                if (product == null)
                {
                    product = new ProductEntity
                    {
                        ProductName = productRequest.ProductName,
                        ProductDescription = productRequest.ProductDescription,
                        Price = productRequest.Price,
                    };
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    MessageBox.Show("Det finns redan en produkt med detta namn");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                MessageBox.Show("Produkt kunde inte läggas till.");
            }

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
                    ProductDescription = productEntity.ProductDescription,
                    Price = productEntity.Price
                };
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new ProductRequest();
        }

        public async void UpdateProduct(int id, ProductRequest productRequest)
        {
            //try
            //{
            var productEntity = await _context.Products.FindAsync(id);
           
            productEntity.ProductName = productRequest.ProductName;
            productEntity.ProductDescription = productRequest.ProductDescription;
            productEntity.Price = productRequest.Price;

            _context.Entry(productEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //}
            //catch (Exception ex) {Debug.WriteLine(ex.Message);

            //    MessageBox.Show("Kunde inte uppdatera Produkt.");
            //}
        }
    }
}
