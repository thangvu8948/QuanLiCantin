using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCantin
{
    class Global
    {
        public static List<Product> products = new List<Product>();
        public static List<Order> orders = new List<Order>();
        public static NhanVien nhanVien = new NhanVien();
        private static readonly Color purple = Color.FromArgb(0xFF, 67, 0x3A, 0xB7);

        public static void UnhighlightButton(Button button)
        {
            button.Foreground = Brushes.Black;
            button.Background = Brushes.White;
            button.BorderBrush = Brushes.Black;
        }

        public static void HighlightButton(Button clickedButton)
        {
            clickedButton.Foreground = Brushes.Cyan;
            clickedButton.Background = new SolidColorBrush(purple);
            clickedButton.BorderBrush = Brushes.Cyan;
        }
    }
}
