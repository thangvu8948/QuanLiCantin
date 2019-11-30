using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    }
}
