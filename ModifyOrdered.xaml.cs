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
        private string _id;
        public ModifyOrdered(int data, string id)
        {
            InitializeComponent();
            alterSoluongBox.Text = data.ToString();
            _id = id;
        }
        public int alteredCount;
        private void DieuChinhSoluongClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(alterSoluongBox.Text);
                if (temp < 0) throw new Exception("Vui lòng nhập số lớn hơn 0");
                int available = GetAvailable();
                if (temp > available) throw new Exception("Không đủ số lượng");
                else
                {
                    alteredCount = temp;
                }
                this.DialogResult = true;
                this.Close();
            } catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private int GetAvailable()
        {
            for (int i = 0; i < Global.products.Count; i++)
            {
                if (Global.products[i].ID == _id)
                {
                    return Global.products[i].Remain;
                }
            }
            return 0;
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
