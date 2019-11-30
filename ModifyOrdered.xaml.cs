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
    /// Interaction logic for ModifyOrdered.xaml
    /// </summary>
    public partial class ModifyOrdered : Window
    {
        public ModifyOrdered(int data)
        {
            InitializeComponent();
            alterSoluongBox.Text = data.ToString();
        }
        public int alteredCount;
        private void DieuChinhSoluongClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(alterSoluongBox.Text);
                if (temp < 0) throw new Exception("Nhỏ hơn 0");
                else
                {
                    alteredCount = temp;
                }
                this.DialogResult = true;
                this.Close();
            } catch(Exception exc)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ");
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            alteredCount = 0;
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
