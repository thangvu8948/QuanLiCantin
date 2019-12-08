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
    /// Interaction logic for EmployeeSearchUI.xaml
    /// </summary>
    public partial class EmployeeSearchUI : UserControl
    {
        public EmployeeSearchUI()
        {
            InitializeComponent();
            EMP_Search_UI.NameSearchBox.Text = string.Empty;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RoleSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NameSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
