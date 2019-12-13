using System;
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
        private int _available;
        public ChonSoLuongWindow(int available)
        {
            InitializeComponent();
            _available = available;
        }

        private void SendBack(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(Soluong.Text);
                if (temp <= 0) throw new Exception("Vui lòng nhập lớn hơn 0");
                if (temp > _available) throw new Exception("Không đủ số lượng");
                else
                {
                    AlteredSoluong = temp;
                }
                this.DialogResult = true;
                this.Close();
                return;
            } catch(Exception err)
            {
                MessageBox.Show(err.Message);
            } 
            
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
