using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public static CultureInfo locale = new CultureInfo("vi-VN");
        
        int mode = 0;
        private bool warehouseItemMode = false;

        public ManagerWindow()
        {
            InitializeComponent();
            UsernameBox.Text = MainWindow.GetUsername();
            Food.Foreground = Brushes.Yellow;

            ManagerUI.Children.Remove(WH_Storage_UI);
            ManagerUI.Children.Remove(WH_Items_UI);
            
            ManagerUI.Children.Remove(EMP_UI);
        }

        private void Food_Click(object sender, RoutedEventArgs e)
        {
            if (mode != 0)
            {
                mode = 0;

                Food.Foreground = Brushes.Yellow;
                Employee.Foreground = Brushes.White;
                Warehouse.Foreground = Brushes.White;
                Exit.Foreground = Brushes.White;

                OptionIndicator.Margin = Food.Margin;

                if (!warehouseItemMode)
                    ManagerUI.Children.Remove(WH_Storage_UI);
                else
                    ManagerUI.Children.Remove(WH_Items_UI);
                
                ManagerUI.Children.Remove(EMP_UI);
                ManagerUI.Children.Add(MM_UI);
            }
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
            if (mode != 1)
            {
                mode = 1;

                Food.Foreground = Brushes.White;
                Employee.Foreground = Brushes.Yellow;
                Warehouse.Foreground = Brushes.White;
                Exit.Foreground = Brushes.White;

                OptionIndicator.Margin = Employee.Margin;

                if (!warehouseItemMode)
                    ManagerUI.Children.Remove(WH_Storage_UI);
                else
                    ManagerUI.Children.Remove(WH_Items_UI);
               
                ManagerUI.Children.Remove(MM_UI);
                
                ManagerUI.Children.Add(EMP_UI);
            }
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
            if (mode != 2)
            {
                mode = 2;

                Food.Foreground = Brushes.White;
                Employee.Foreground = Brushes.White;
                Warehouse.Foreground = Brushes.Yellow;
                Exit.Foreground = Brushes.White;

                OptionIndicator.Margin = Warehouse.Margin;
                ManagerUI.Children.Remove(EMP_UI);
                ManagerUI.Children.Remove(MM_UI);

                if (warehouseItemMode)
                    ManagerUI.Children.Add(WH_Items_UI);
                else
                    ManagerUI.Children.Add(WH_Storage_UI);
            }
            else
            {
                warehouseItemMode = !warehouseItemMode;
                if (warehouseItemMode)
                {
                    ManagerUI.Children.Remove(WH_Storage_UI);
                    ManagerUI.Children.Add(WH_Items_UI);
                }
                else
                {
                    ManagerUI.Children.Remove(WH_Items_UI);
                    ManagerUI.Children.Add(WH_Storage_UI);
                }
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

        private void EMP_UI_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WH_Items_UI_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WH_Storage_UI_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MM_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}