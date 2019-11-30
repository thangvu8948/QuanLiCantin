﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChonSoLuongWindow.xaml
    /// </summary>
    public partial class ChonSoLuongWindow : Window
    {
        public int AlteredSoluong = 1;

        public ChonSoLuongWindow()
        {
            InitializeComponent();

        }

        private void SendBack(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(Soluong.Text);
                if (temp < 0) throw new Exception("Nhỏ hơn 0");
                else
                {
                    AlteredSoluong = temp;
                }
                this.DialogResult = true;
                this.Close();
                return;
            } catch(Exception err)
            {
                MessageBox.Show("Vui lòng nhập số");
            } 
            
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
