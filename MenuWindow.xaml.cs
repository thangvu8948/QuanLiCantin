using System;
using System.Collections.Generic;
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
            var tab = sender as TabItem;
            tab.Foreground = Brushes.Yellow;
            tab.Background = Brushes.Black;

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
                List<Product> products = getProductList(conn);
                if (products != null)
                {
                    MessageBox.Show($"{products[0]._name} \r\n {products[0]._price}");
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

        private List<Product> getProductList(SqlConnection conn)
        {
            string sql = "Select * from THUCDON";

            // Tạo một đối tượng Command.
            SqlCommand cmd = new SqlCommand();

            // Liên hợp Command với Connection.
            cmd.Connection = conn;
            cmd.CommandText = sql;


            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    List<Product> listProduct = new List<Product>();
                    while (reader.Read())
                    {
                        // Chỉ số của cột Emp_ID trong câu lệnh SQL.
                        string id = Convert.ToString(reader.GetValue(0)); // 0
                        string tenmon = Convert.ToString(reader.GetValue(1));
                        long giatien = Convert.ToInt64(reader.GetValue(2));
                        int soluong = Convert.ToInt32(reader.GetValue(3));
                        int loai = Convert.ToInt32(reader.GetValue(4));

                        listProduct.Add(new Product(id, tenmon, loai, giatien, soluong));
                    }
                    return listProduct;
                }
                return null;
            }
        }
    }
   
}
