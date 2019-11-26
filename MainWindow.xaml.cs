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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool signInMode = false; //Default: sign in as staff
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_SignInStaff(object sender, RoutedEventArgs e)
        {
            deleteSomethingInMain();
        }

        private void Button_SignInManager(object sender, RoutedEventArgs e)
        {
            deleteSomethingInMain();
        }

        private void deleteSomethingInMain()
        {
            stackpanel.Children.Remove(textSignInChosen);
            var buttons = stackpanel.Children.OfType<Button>().ToList();
            foreach (var button in buttons)
            stackpanel.Children.Remove(button);
        }
        private void drawLogin()
        {
            TextBlock textLoginUser = new TextBlock();
            textLoginUser.Text = "Tên đăng nhập";
        }
    }
}
