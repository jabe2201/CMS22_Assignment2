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

       public async void Create(CustomerRequest customerReq)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == customerReq.Email);
                if(customer == null)
                {
                    customer = new CustomerEntity
                    {
                        FirstName = customerReq.FirstName,
                        LastName = customerReq.LastName,
                        Email = customerReq.Email,
                        Phone = customerReq.Phone,
                    };
                }
                else
                {
                    MessageBox.Show("Det finns redan en kund med denna Mail.");
                }

                var addressResult = await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName.ToLower() == customerReq.StreetName.ToLower() && x.PostalCode == customerReq.PostalCode.Trim() && x.City.ToLower() == customerReq.City.ToLower());
                if(addressResult == null)
                {
                    addressResult = new CustomerAddressEntity
                    {
                        StreetName = customerReq.StreetName,
                        PostalCode = customerReq.PostalCode,
                        City = customerReq.City,
                    };
                    _context.Addresses.Add(addressResult);
                    await _context.SaveChangesAsync();
                }
                customer.AddressId = addressResult.AddressId;
               
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                MessageBox.Show("Kund har lagts till");
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); 
            
                MessageBox.Show("Kund kunde inte läggas till.");
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
                
                var addressEntity = await _context.Addresses.FindAsync(customerEntity.AddressId);

                customerReq.StreetName = addressEntity.StreetName;
                customerReq.PostalCode = addressEntity.PostalCode;
                customerReq.City = addressEntity.City;

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

                var addressResult = await _context.Addresses.FindAsync(customerEntity.AddressId);
                if(addressResult.StreetName.ToLower() != customerRequest.StreetName.ToLower() && addressResult.PostalCode != customerRequest.PostalCode.Trim() && addressResult.City.ToLower() != customerRequest.City.ToLower())
                {
                    addressResult = new CustomerAddressEntity
                    {
                        StreetName = customerRequest.StreetName,
                        PostalCode = customerRequest.PostalCode,
                        City = customerRequest.City,
                    };
                    customerEntity.AddressId = addressResult.AddressId;
                    _context.Add(addressResult);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    addressResult.StreetName = customerRequest.StreetName;
                    addressResult.PostalCode = customerRequest.PostalCode;
                    addressResult.City = customerRequest.City;

                    _context.Entry(addressResult).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                _context.Entry(customerEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                MessageBox.Show("Kunde inte uppdatera Produkt.");
            }
        }
    }
}
