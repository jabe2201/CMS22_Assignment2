using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using CMS22_Assignment2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMS22_Assignment2
{
    
    public partial class MainWindow : Window
    {
        private readonly CustomerServices _customerServices;
        private readonly ProductServices _productServices;
        private readonly OrderServices _orderServices;
        private ObservableCollection<OrderRowModel> _orderRows = new ObservableCollection<OrderRowModel>();

        private enum MenuState
        {
            MainWindow,
            CustomerWindow,
            ProductWindow
        }

        public MainWindow(CustomerServices customerServices, ProductServices productServices, OrderServices orderServices)
        {
            InitializeComponent();
            _customerServices = customerServices;
            _productServices = productServices;
            _orderServices = orderServices;
            MenuPresenter(MenuState.MainWindow);
        }

        private async Task PopulateComboBoxes()
        {
            var customers = new ObservableCollection<KeyValuePair<int, string>>();
            foreach (var customer in await _customerServices.GetAllAsync())
                customers.Add(new KeyValuePair<int, string>(customer.Id, customer.FullName));

            cb_Customer.ItemsSource = customers;
            cb_CustomerEdit.ItemsSource = customers;

            var products = new ObservableCollection<KeyValuePair<int, string>>();
            foreach (var product in await _productServices.GetAllAsync())
                products.Add(new KeyValuePair<int, string>(product.Id, product.ProductName));

            cb_Products.ItemsSource = products;
            cb_ProductEdit.ItemsSource = products;
        }

        private void RefreshOrderRows()
        {
            lv_OrderRows.ItemsSource = _orderRows;
        }

        private void MenuPresenter(MenuState menuState)
        {
            PopulateComboBoxes().ConfigureAwait(false);
            switch (menuState)
            {
                case MenuState.MainWindow:
                    OrderView.Visibility = Visibility.Visible;
                    CustomerView.Visibility = Visibility.Collapsed;
                    ProductView.Visibility = Visibility.Collapsed;
                    break;
                case MenuState.CustomerWindow:
                    OrderView.Visibility = Visibility.Collapsed;
                    CustomerView.Visibility = Visibility.Visible;
                    ProductView.Visibility = Visibility.Collapsed;
                    break;

                case MenuState.ProductWindow:
                    OrderView.Visibility = Visibility.Collapsed;
                    CustomerView.Visibility = Visibility.Collapsed;
                    ProductView.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ClearAllFields()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Mail.Text = "";
            tb_Phone.Text = "";
            tb_StreetAddress.Text = "";
            tb_PostalCode.Text = "";
            tb_City.Text = "";
            tb_ProductName.Text = "";
            tb_ProductDescription.Text = "";
            tb_ProductPrice.Text = "";
        }

        private async void bt_Add_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = (KeyValuePair<int, string>)cb_Products.SelectedItem;
                var productKey = product.Key;

                var productReq = await _productServices.GetAsync(productKey);

                var orderRow = new OrderRowModel
                {
                    ProductId = productKey,
                    Price = productReq.Price,
                    Quantity = int.Parse(tb_Quantity.Text),
                    ProductName = productReq.ProductName
                };

                _orderRows.Add(orderRow);
                RefreshOrderRows();
                tb_Quantity.Text = "";
                cb_Products.SelectedIndex = -1;
                
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        private async void bt_PutOrder_ClickAsync(object sender, RoutedEventArgs e)
        {
            var customer = (KeyValuePair<int, string>)cb_Customer.SelectedItem;
            var customerKey = customer.Key;
            decimal sum = 0;
            foreach(var price in _orderRows)
            {
                sum += price.Price;
            }

            var order = new OrderEntity
            {
                CustomerId = customerKey,
                Orderdate = DateTime.Now,
                Sum = sum
            };
            await _orderServices.CreateAsync(order);
            await _orderServices.CreateRowsAsync(_orderRows, order.OrderId);
            _orderRows.Clear();
            RefreshOrderRows();
            cb_Customer.SelectedIndex = -1;
            cb_Products.SelectedIndex = -1;
        }

        private void bt_ViewToCustomer_Click(object sender, RoutedEventArgs e)
        {
            MenuPresenter(MenuState.CustomerWindow);
        }

        private void bt_ViewToProduct_Click(object sender, RoutedEventArgs e)
        {
            MenuPresenter(MenuState.ProductWindow);
        }

        private void bt_ReturnCustomerView_Click(object sender, RoutedEventArgs e)
        {
            MenuPresenter(MenuState.MainWindow);
            ClearAllFields();
        }

        private void bt_ReturnProductView_Click(object sender, RoutedEventArgs e)
        {
            MenuPresenter(MenuState.MainWindow);
            ClearAllFields();
        }

        private void bt_AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new CustomerRequest
            {
                FirstName = tb_FirstName.Text,
                LastName = tb_LastName.Text,
                Email = tb_Mail.Text,
                Phone = tb_Phone.Text,
                StreetName = tb_StreetAddress.Text,
                PostalCode = tb_PostalCode.Text,
                City = tb_City.Text
            };
            ClearAllFields();
            _customerServices.Create(customer);
        }

        private void bt_EditCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_ProductAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = new ProductRequest
            {
                ProductName = tb_ProductName.Text,
                ProductDescription = tb_ProductDescription.Text,
                Price = decimal.Parse(tb_ProductPrice.Text)
            };
            ClearAllFields();
            _productServices.Create(product);
        }

        private async void cb_ProductEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = (KeyValuePair<int, string>)cb_ProductEdit.SelectedItem;

           var productReq =await _productServices.GetAsync(product.Key);
            
            tb_ProductName.Text = productReq.ProductName;
            tb_ProductDescription.Text = productReq.ProductDescription;
            tb_ProductPrice.Text = productReq.Price.ToString();

        }

        private void bt_ProductEdit_Click(object sender, RoutedEventArgs e)
        {
            var product = (KeyValuePair<int, string>)cb_ProductEdit.SelectedItem;
            var productKey = product.Key;

            var productReq = new ProductRequest
            {
                ProductName = tb_ProductName.Text,
                ProductDescription = tb_ProductDescription.Text,
                Price = decimal.Parse(tb_ProductPrice.Text)
            };
            ClearAllFields();
            _productServices.UpdateProduct(productKey, productReq);
        }
    }
}
