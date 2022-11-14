using CMS22_Assignment2.Contexts;
using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
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
    public class CustomerServices
    {
        private readonly SqlContext _context;

        public CustomerServices(SqlContext context)
        {
            _context = context;
        }

       public async Task<IEnumerable<CustomerRequest>> GetAllAsync()
        {
            var customers = new List<CustomerRequest>();

            try
            {
                foreach (var customer in await _context.Customers.ToListAsync())
                {
                    customers.Add(new CustomerRequest
                    {
                        Id = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        FullName = $"{customer.FirstName} {customer.LastName}"
                    }) ;
                }
                return customers;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return customers;
        }

        public async Task<ActionResult> GetAsync(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);
                if (customerEntity == null)
                    return new NotFoundResult();

                return new OkObjectResult(new CustomerRequest
                {
                    Id = customerEntity.CustomerId,
                    FirstName = customerEntity.FirstName,
                    LastName = customerEntity.LastName
                });
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
    }
}
