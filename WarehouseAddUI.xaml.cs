using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for WarehouseAddUI.xaml
    /// </summary>
    public partial class WarehouseAddUI : UserControl
    {
        public WarehouseAddUI()
        {
            InitializeComponent();
            EmptyAllField();
        }

        public void EmptyAllField()
        {
            IDInputBox.Text = null;
            NameInputBox.Text = null;
            UnitInputBox.Text = null;
            QuantityInputBox.Text = null;
        }

        private void IDInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UnitInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NameInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void QuantityInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }

        public (string, string, string, double) GetInputData()
        {
            (string, string, string, double) returnValue;
            double quantity = 0;
            try
            {
                quantity = Convert.ToDouble(QuantityInputBox.Text);
            }
            catch (FormatException e)
            {
                MessageBox.Show($"Error recognizing inputted quantity");
                Debug.WriteLine($"{e.Message}");
            }
            returnValue = (IDInputBox.Text, NameInputBox.Text, UnitInputBox.Text, quantity);
            return returnValue;
        }
    }
}
