using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
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
        private static bool validBegin, validEnd, validDate = false;
        private double SLDN, SLCN;
        private DateTime NLK;

        public WarehouseAddUI()
        {
            InitializeComponent();
            EmptyAllField();
        }

        public void EmptyAllField()
        {
            MLKBox.Text = null;
            MaHHBox.Text = null;
            SLDNBox.Text = null;
            SLCNBox.Text = null;
            DateBox.Text = null;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public (string, string, double, double, DateTime) GetInputData()
        {
            return (MLKBox.Text, MaHHBox.Text, SLDN, SLCN, NLK);
        }

        private void MLK_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void MaHH_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SLDN_TextChanged(object sender, TextChangedEventArgs e)
        {
            validBegin = double.TryParse(SLDNBox.Text, out SLDN);
        }

        private void SLCN_TextChanged(object sender, TextChangedEventArgs e)
        {
            validEnd = double.TryParse(SLCNBox.Text, out SLCN);
        }


        private void Date_TextChanged(object sender, TextChangedEventArgs e)
        {
            validDate = DateTime.TryParseExact(
                DateBox.Text, "dd/MM/yyyy", 
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out NLK);
        }

        public bool AllValid()
        {
            return validBegin && validEnd && validDate == true;
        }
    }
}
