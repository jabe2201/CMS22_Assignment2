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
using System.Windows;

namespace CMS22_Assignment2.Services
{
    public class CustomerServices
    {
        private readonly SqlContext _context;

        public CustomerServices(SqlContext context)
        {
            _context = context;
        }

       public async void CreateAsync(CustomerRequest customerReq)
       {
            try
            {
                var customer = _context.Customers.FirstOrDefault(x => x.Email == customerReq.Email);
                if (customer == null)
                {
                    customer = new CustomerEntity
                    {
                        FirstName = customerReq.FirstName,
                        LastName = customerReq.LastName,
                        Email = customerReq.Email,
                        Phone = customerReq.Phone,
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Kund har lagts till");
                }
                else
                {
                    MessageBox.Show("Det finns redan en kund med denna Mail.");
                }
            }
            catch
            {
                MessageBox.Show("Kunde inte lägga till kund.");
            }
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

        public async Task<CustomerRequest> GetAsync(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);
                
                var customerReq = new CustomerRequest
                {
                    Id = customerEntity.CustomerId,
                    FirstName = customerEntity.FirstName,
                    LastName = customerEntity.LastName,
                    Email = customerEntity.Email,
                    Phone = customerEntity.Phone,
                };
               

                return customerReq;
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            return new CustomerRequest();
        }

        public async void UpdateCustomer(int id, CustomerRequest customerRequest)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);

                customerEntity.FirstName = customerRequest.FirstName;
                customerEntity.LastName = customerRequest.LastName;
                customerEntity.Phone = customerRequest.Phone;
                customerEntity.Email = customerRequest.Email;

                _context.Entry(customerEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                MessageBox.Show("Kund har uppdaterats");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                MessageBox.Show("Kunde inte uppdatera Kundinformation.");
            }
        }
    }
}
