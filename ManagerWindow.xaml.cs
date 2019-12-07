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

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly System.Windows.Threading.DispatcherTimer clock = null;
        private bool is_clock_loaded = false;
        private readonly CultureInfo locale = new CultureInfo("vi-VN");
        public ManagerWindow()
        {
            InitializeComponent();
            UsernameBox.Text = $"{MainWindow.GetUsername().Trim().ToUpper()}";
            Food.Foreground = Brushes.Yellow;
            ManagerUI.Children.Remove(WH_UI);
            clock = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            if (is_clock_loaded is false)
            {
                clock.Tick += Clock_Tick;
                is_clock_loaded = true;
            }
            clock.Start();
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            Timer.Text = 
                DateTime.Now.ToString("HH:mm:ss\nddd, dd/MM/yyyy", locale);
        }

        private void Food_Click(object sender, RoutedEventArgs e)
        {
            Food.Foreground = Brushes.Yellow;
            Employee.Foreground = Brushes.White;
            Warehouse.Foreground = Brushes.White;
            Exit.Foreground = Brushes.White;

            OptionIndicator.Margin = Food.Margin;
            ManagerMode.Text = string.Empty;

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

            ManagerMode.Text = "QUẢN LÝ NHÂN VIÊN";
            OptionIndicator.Margin = Employee.Margin;

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

                OptionIndicator.Margin = Warehouse.Margin;
                ManagerMode.Text = "QUẢN LÝ KHO";

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
            clock.Stop();
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
    }
}