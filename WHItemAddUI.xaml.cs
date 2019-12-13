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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for WHItemAddUI.xaml
    /// </summary>
    public partial class WHItemAddUI : UserControl
    {
        private bool validID = false, validRemain = false, validName = false;
        private double remain;
        private readonly Brush correctColor = Brushes.LightGreen;
        private readonly Brush incorrectColor = Brushes.Red;


        public WHItemAddUI()
        {
            InitializeComponent();
        }

        private void RemainBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validRemain = double.TryParse(RemainBox.Text, out remain);
            validRemain &= (remain > 0.0);
            RemainBox.BorderBrush = validRemain ? correctColor : incorrectColor;
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = NameBox.Text.Length;
            validName = len > 0 && len <= 50;
            NameBox.BorderBrush = validID ? correctColor : incorrectColor;
        }


        private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = IDBox.Text.Length;
            validID = len > 0 && len <= 5;
            IDBox.BorderBrush = validID ? correctColor : incorrectColor;
        }

        public (string, string, double) GetInputData()
        {
            return (IDBox.Text, NameBox.Text, remain);
        }

        public bool AllValid()
        {
            return validID && validName && validRemain == true;
        }

        private void WHItemAddUI_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
        }

        private void WHItemAddUI_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
