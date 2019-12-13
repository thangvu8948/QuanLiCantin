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
    /// Interaction logic for MenuItemAddUI.xaml
    /// </summary>
    public partial class MenuItemAddUI : UserControl
    {
        private bool validID, validPrice, validCount, validName, validType;
        private readonly Brush correctColor = Brushes.LightGreen;
        private readonly Brush incorrectColor = Brushes.Red;

        private int price, count, id;

        public MenuItemAddUI()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            IDBox.Text = string.Empty; IDBox.BorderBrush = Brushes.Cyan;
            PriceBox.Text = string.Empty; PriceBox.BorderBrush = Brushes.Cyan;
            CountBox.Text = string.Empty; CountBox.BorderBrush = Brushes.Cyan;
            NameBox.Text = string.Empty; NameBox.BorderBrush = Brushes.Cyan;
            MenuItemType.BorderBrush = Brushes.Cyan;
            validID = validPrice = validCount = validName = validType = false;
        }

        private void MenuItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validType = MenuItemType.SelectedIndex != -1;
            MenuItemType.BorderBrush = validType ? correctColor : incorrectColor;
        }


        private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validID = IDBox.Text.Length > 0 && IDBox.Text.Length <= 5;
            validID &= int.TryParse(IDBox.Text, out id);
            IDBox.BorderBrush = validID ? correctColor : incorrectColor;
        }

        private void PriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validPrice = int.TryParse(PriceBox.Text, out price);
            validPrice &= (price >= 0);
            PriceBox.BorderBrush = validPrice ? correctColor : incorrectColor;
        }

        private void CountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validCount = int.TryParse(CountBox.Text, out count);
            validCount &= (count >= 0);
            CountBox.BorderBrush = validCount ? correctColor : incorrectColor;
        }



        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validName = NameBox.Text.Length > 0 && NameBox.Text.Length <= 50;
            NameBox.BorderBrush = validName ? correctColor : incorrectColor;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool AllValid()
        {
            return validCount && validID && validName && validType && validPrice == true;
        }

        public (int, string, int, int, int, string) GetInputData()
        {
            return (id, NameBox.Text, 
                price, count, MenuItemType.SelectedIndex + 1, 
                $"Images/{IDBox.Text}/download.jpg");
        }

        private void WHItemAddUI_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
        }

        private void WHItemAddUI_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.5;
        }
    }
}
