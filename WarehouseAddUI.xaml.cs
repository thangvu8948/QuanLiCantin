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
        private bool validBegin, validEnd, validDate, validPK, validMHH = false;
        private double SLDN, SLCN;
        private DateTime NLK;
        private readonly Brush correctColor = new SolidColorBrush(Color.FromArgb(0xE5, 0, 0xFF, 0xFF));
        private readonly Brush incorrectColor = Brushes.Red;

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
            validPK = MLKBox.Text.Length > 0 && MLKBox.Text.Length <= 6;
            MLKBox.BorderBrush = validPK == true ? correctColor : incorrectColor;
        }


        private void MaHH_TextChanged(object sender, TextChangedEventArgs e)
        {
            validMHH = MaHHBox.Text.Length > 0 && MaHHBox.Text.Length <= 5;
            MLKBox.BorderBrush = validPK == true ? correctColor : incorrectColor;
        }

        private void SLDN_TextChanged(object sender, TextChangedEventArgs e)
        {
            validBegin = double.TryParse(SLDNBox.Text, out SLDN) && (SLDN > 0.0);
            SLDNBox.BorderBrush = validBegin == true ? correctColor : incorrectColor;
        }

        private void DateBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var tooltip = new ToolTip
            {
                Content = "Vui lòng nhập đúng định dạng dd/MM/yyyy\nVí dụ: 02/09/2019 thay vì 2/9/2019"
            };
            DateBox.ToolTip = tooltip;
        }

        private void SLCN_TextChanged(object sender, TextChangedEventArgs e)
        {
            validEnd = double.TryParse(SLCNBox.Text, out SLCN) && (SLCN > 0.0);
            SLCNBox.BorderBrush = validEnd == true ? correctColor : incorrectColor;
        }


        private void Date_TextChanged(object sender, TextChangedEventArgs e)
        {
            validDate = DateTime.TryParseExact(
                DateBox.Text, "dd/MM/yyyy", 
                ManagerWindow.locale, DateTimeStyles.None,
                out NLK);
            DateBox.BorderBrush = validDate == true ? correctColor : incorrectColor;
        }

        public bool AllValid()
        {
            return validBegin && validEnd && validDate && validPK && validMHH == true;
        }
    }
}
