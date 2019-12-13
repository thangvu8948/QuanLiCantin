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
    /// Interaction logic for RemoveRecordUI.xaml
    /// </summary>
    public partial class RemoveRecordUI : UserControl
    {
        private bool isEmpty;

        public RemoveRecordUI()
        {
            InitializeComponent();

            InputBox.BorderBrush = Brushes.Cyan;

        }

        public void Reset()
        {
            InputBox.Text = string.Empty;
            isEmpty = false;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isEmpty = InputBox.Text.Length == 0;
            InputBox.BorderBrush = isEmpty ? Brushes.Red : Brushes.Green;
        }

        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        public bool IsValid()
        {
            return !isEmpty;
        }

        private void RM_UI_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
        }

        private void RM_UI_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.5;
        }
    }
}
