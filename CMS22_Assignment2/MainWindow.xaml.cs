using CMS22_Assignment2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_PutOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
