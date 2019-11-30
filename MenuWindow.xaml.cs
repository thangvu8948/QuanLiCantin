using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void TabControl_Selected(object sender, RoutedEventArgs e)
        {
/*            var tab = sender as TabItem;
            tab.Foreground = Brushes.Yellow;
            tab.Background = Brushes.Black;*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Getting Connection ...");
            SqlConnection conn = DBUtils.GetDBConnection();

            try
            {
                Debug.WriteLine("Openning Connection ...");
                conn.Open();
                Debug.WriteLine("Connection successful!");
                BindingList<Product> products = ProductDAO.GetAllProducts(_typeFood);
                if (products != null)
                {
                    MessageBox.Show($"{products[0].Name} \r\n {products[0].Price}");
                } else
                {
                    MessageBox.Show("No food");

                }
                MessageBox.Show("OK", "APp");
            }
            catch (Exception e1)
            {
                MessageBox.Show("Failed", "app");
                Debug.WriteLine("Error: " + e1.Message);
            }

        }

        class Product
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int Type { get; set; }
            public long Price { get; set; }
            public int Remain { get; set; }
            public string Image { get; set; }


        }
        class Order
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int Soluong { get; set; }
            public long PriceOfOne { get; set; }
        }

        BindingList<Product> _products = null;
        BindingList<Order> _orders = null;
        int _typeFood = 0;

        class ProductDAO : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public static BindingList<Product> GetAllProducts( int type)
            {
                string sql = "Select * from MonAn";
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    switch (type)
                    {
                        case 0: sql = "Select * from MonAn"; break;
                        case 1: sql = "Select * from MonAn where MALOAI = 1"; break;
                        case 2: sql = "Select * from MonAn where MALOAI = 2"; break;
                        case 3: sql = "Select * from MonAn where MALOAI = 3"; break;
                    }

                    // Tạo một đối tượng Command.
                    SqlCommand cmd = new SqlCommand();

                    // Liên hợp Command với Connection.
                    cmd.Connection = conn;
                    cmd.CommandText = sql;


                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            BindingList<Product> listProduct = new BindingList<Product>();
                            while (reader.Read())
                            {
                                // Chỉ số của cột Emp_ID trong câu lệnh SQL.
                                string id = Convert.ToString(reader.GetValue(0)).Trim(); // 0
                                string tenmon = Convert.ToString(reader.GetValue(1));
                                long giatien = Convert.ToInt64(reader.GetValue(2));
                                int soluong = Convert.ToInt32(reader.GetValue(3));
                                int loai = Convert.ToInt32(reader.GetValue(4));


                                var product = new Product()
                                {
                                    ID = id,
                                    Name = tenmon,
                                    Price = giatien,
                                    Remain = soluong,
                                    Type = loai,
                                    Image = $"Images/{id}/download.jpg"
                                };
                                listProduct.Add(product);
                            }
                            conn.Close();

                            return listProduct;
                        }
                        conn.Close();
                        return null;
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Failed", "app");
                    Debug.WriteLine("Error: " + e1.Message);
                }
                return null;
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _typeFood = 0;
            _orders = new BindingList<Order>();
            OrderedList.ItemsSource = _orders;

            _products = ProductDAO.GetAllProducts(_typeFood);
            MenuList.ItemsSource = _products;
            MenuList.MouseLeftButtonUp += ChooseProduct;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         //   var obj = sender as ListViewItem;
            MessageBox.Show($"This is");
        }

        private void ChooseProduct(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            Product item = (Product)MenuList.ItemContainerGenerator.ItemFromContainer(dep);

            var sl = new ChonSoLuongWindow();

            if (sl.ShowDialog() == true)
            {
                var order = new Order()
                {
                    ID = item.ID,
                    Soluong = sl.AlteredSoluong,
                    Name = item.Name,
                    PriceOfOne = item.Price
                };
                _orders.Add(order);

            }


            //    MessageBox.Show($"This is {item.Name}");
        }

        private void StackPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
   
        private void All_select(object sender, RoutedEventArgs e)
        {
            _typeFood = 0;
            if (_products != null)
            {
                _products.Clear();
                _products = ProductDAO.GetAllProducts(_typeFood);
                MenuList.ItemsSource = _products;
            }

        }

        private void Bf_select(object sender, RoutedEventArgs e)
        {
            _typeFood = 1;
            if (_products != null)
                _products.Clear();
            _products = ProductDAO.GetAllProducts(_typeFood);
            MenuList.ItemsSource = _products;
        }

        private void Dr_select(object sender, RoutedEventArgs e)
        {
            _typeFood = 3;
            if (_products != null)
                _products.Clear();
            _products = ProductDAO.GetAllProducts(_typeFood);
            MenuList.ItemsSource = _products;
        }

        private void La_select(object sender, RoutedEventArgs e)
        {
            _typeFood = 2;
            if (_products != null)
                _products.Clear();
            _products = ProductDAO.GetAllProducts(_typeFood);
            MenuList.ItemsSource = _products;

        }
    }
    
}
