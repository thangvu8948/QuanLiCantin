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
    /// Interaction logic for ThanhToanWindow.xaml
    /// </summary>
    public partial class ThanhToanWindow : Window
    {
        public ThanhToanWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OrderedList.ItemsSource = Global.orders;

            long total = getTotalCost();
            TotalCost.Text = total.ToString();
        }

        long getTotalCost()
        {
            long total = 0;
            foreach (var order in Global.orders)
            {
                total += order.PriceOfOne * order.SoLuong;
            }
            return total;
        }

        private void ThanhToanClick(object sender, RoutedEventArgs e)
        {
            string sql = "select * from HoaDon";
            SqlConnection conn = DBUtils.GetDBConnection();

            //lấy danh sách hóa đơn
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                int i = 0;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            i++;
                        }
                    }
                    reader.Close();
                }
               
                var mahd = "HD" + (i + 1).ToString();
                var time = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                sql = $"insert HoaDon values('{mahd}', '{time}', {getTotalCost()})";
                cmd.CommandText = sql;
                DbDataReader reader3 = cmd.ExecuteReader();
                reader3.Close();
                 //Thêm chi tiết hóa đơn
                int countOrder = Global.orders.Count;
                for (int j = 0; j < countOrder; j++)
                {
                    sql = $"insert ChiTiet values ('{mahd}', '{Global.orders[j].ID}', {Global.orders[j].SoLuong}, {Global.orders[j].PriceOfOne})";
                    cmd.CommandText = sql;
                    DbDataReader reader2 = cmd.ExecuteReader();
                    reader2.Close();
                }
            } catch(Exception err)
            {
                MessageBox.Show("Error in database");
                Debug.WriteLine("error: " + err.Message);
                return;
            }


            MessageBox.Show("Đã thanh toán");
            this.DialogResult = true;
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
