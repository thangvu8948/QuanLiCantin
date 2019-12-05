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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    { 

        public ManagerWindow()
        {
            InitializeComponent();
            ManagerUI.Children.Remove(WH_UI);
            
        }

        private void Food_Click(object sender, RoutedEventArgs e)
        {
            Food.Foreground = Brushes.Yellow;
            Employee.Foreground = Brushes.White;
            Warehouse.Foreground = Brushes.White;
            Exit.Foreground = Brushes.White;
            ManagerUI.Children.Remove(WH_UI);
        }


        private void Food_MouseEnter(object sender, MouseEventArgs e)
        {
            Food.Background = Brushes.Gray;
        }

        private void Food_MouseLeave(object sender, MouseEventArgs e)
        {
            Food.Background = LeftPanel.Background;
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            Food.Foreground = Brushes.White;
            Employee.Foreground = Brushes.Yellow;
            Warehouse.Foreground = Brushes.White;
            Exit.Foreground = Brushes.White;
            ManagerUI.Children.Remove(WH_UI);
        }

        private void Employee_MouseEnter(object sender, MouseEventArgs e)
        {
            Employee.Background = Brushes.Gray;
        }

        private void Employee_MouseLeave(object sender, MouseEventArgs e)
        {
            Employee.Background = LeftPanel.Background;
        }

        private void Warehouse_Click(object sender, RoutedEventArgs e)
        {
            if (!ManagerUI.Children.Contains(WH_UI))
            {
                Food.Foreground = Brushes.White;
                Employee.Foreground = Brushes.White;
                Warehouse.Foreground = Brushes.Yellow;
                Exit.Foreground = Brushes.White;
                ManagerUI.Children.Add(WH_UI);
            }
        }

        private void Warehouse_MouseEnter(object sender, MouseEventArgs e)
        {
            Warehouse.Background = Brushes.Gray;
        }

        private void Warehouse_MouseLeave(object sender, MouseEventArgs e)
        {
            Warehouse.Background = LeftPanel.Background;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var main_window = new MainWindow();
            main_window.Show();
            this.Close();
        }

        private void Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Exit.Background = Brushes.Gray;
        }

        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Exit.Background = LeftPanel.Background;
        }

        private void Warehouse_Loaded(object sender, RoutedEventArgs e)
        {
 
        }

        private void QuitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}