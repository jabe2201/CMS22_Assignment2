using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using CMS22_Assignment2.Services;
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
            PopulateComboBoxes().ConfigureAwait(false);
        }

        public async Task PopulateComboBoxes()
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

        public void RefreshOrderRows()
        {
            lv_OrderRows.ItemsSource = _orderRows;
        }

        private void MenuPresenter(MenuState menuState)
        {
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
        }

        private void bt_ReturnProductView_Click(object sender, RoutedEventArgs e)
        {
            MenuPresenter(MenuState.MainWindow);
        }
    }
}
