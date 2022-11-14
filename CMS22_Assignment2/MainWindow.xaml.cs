using CMS22_Assignment2.Models;
using CMS22_Assignment2.Models.Entities;
using CMS22_Assignment2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerServices _customerServices;
        private readonly ProductServices _productServices;
        private readonly OrderServices _orderServices;
        private ObservableCollection<OrderRowModel> _orderRows = new ObservableCollection<OrderRowModel>();

        public MainWindow(CustomerServices customerServices, ProductServices productServices)
        {
            InitializeComponent();
            _customerServices = customerServices;
            _productServices = productServices;
            PopulateComboBoxes().ConfigureAwait(false);
        }

        public async Task PopulateComboBoxes()
        {
            var customers = new ObservableCollection<KeyValuePair<int, string>>();
            foreach (var customer in await _customerServices.GetAllAsync())
                customers.Add(new KeyValuePair<int, string>(customer.Id, customer.FullName));

            cb_Customer.ItemsSource = customers;

            var products = new ObservableCollection<KeyValuePair<int, string>>();
            foreach (var product in await _productServices.GetAllAsync())
                products.Add(new KeyValuePair<int, string>(product.Id, product.ProductName));

            cb_Products.ItemsSource = products;
        }

        private async void bt_Add_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = (KeyValuePair<int, string>)cb_Customer.SelectedItem;
                var customerKey = customer.Key;

                var product = (KeyValuePair<int, string>)cb_Products.SelectedItem;
                var productKey = product.Key;

                var productReq = await _productServices.GetAsync(productKey);

                var orderRow = new OrderRowModel
                {
                    OrCustomerId = customerKey,
                    OrProductId = productKey,
                    OrPrice = productReq.Price,
                    OrProductName = productReq.ProductName,
                    OrQuantity = int.Parse(tb_Quantity.Text)
                };

                _orderRows.Add(orderRow);
                RefreshOrderRows();
                tb_Quantity.Text = "";
                cb_Products.SelectedIndex = -1;
                
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }

        private void bt_PutOrder_Click(object sender, RoutedEventArgs e)
        {
            var customer = (KeyValuePair<int, string>)cb_Customer.SelectedItem;
            var customerKey = customer.Key;

            var order = new OrderModel
            {
                CustomerId = customerKey,
                DateTime = DateTime.Now
            };

        }

        public void RefreshOrderRows()
        {
            lv_OrderRows.ItemsSource = _orderRows;
        }
    }
}
