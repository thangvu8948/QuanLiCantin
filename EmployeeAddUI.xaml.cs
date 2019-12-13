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
    /// Interaction logic for EmployeeAddUI.xaml
    /// </summary>
    public partial class EmployeeAddUI : UserControl
    {
        private readonly Brush correctColor = Brushes.LightGreen;
        private readonly Brush incorrectColor = Brushes.Red;

        private bool validID, validRole, validName, validLoginName, validPassword = false;
        private int role;

        public EmployeeAddUI()
        {
            InitializeComponent();
        }

        private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = IDBox.Text.Length;
            validID = len > 0 && len <= 5;
            IDBox.BorderBrush = validID ? correctColor : incorrectColor;
        }

        private void RoleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validRole = int.TryParse(RoleBox.Text, out role);
            validRole &= (role == 1 || role == 2);
            RoleBox.BorderBrush = validRole ? correctColor : incorrectColor;
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = PasswordBox.Text.Length;
            validPassword = len > 0 && len <= 30;
            PasswordBox.BorderBrush = validPassword ? correctColor : incorrectColor;
        }

        private void LoginNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = LoginNameBox.Text.Length;
            validLoginName = len > 0 && len <= 30;
            LoginNameBox.BorderBrush = validLoginName ? correctColor : incorrectColor;
        }


        private void EMP_Add_UI_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
        }

        private void EMP_Add_UI_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var len = NameBox.Text.Length;
            validName = len > 0 && len <= 30;
            NameBox.BorderBrush = validName ? correctColor : incorrectColor;
        }



        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }

        public (string, int, string, string, string) GetInputData()
        {
            return (IDBox.Text, role, NameBox.Text, LoginNameBox.Text, PasswordBox.Text);
        }

        public bool AllValid()
        {
            return validID && validName && validLoginName && validPassword && validRole == true;
        }
    }


}
