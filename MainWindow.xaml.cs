using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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


        const int leftPadding = 150;

        private void Button_SignInStaff(object sender, RoutedEventArgs e)
        {
            deleteSomethingInMain();
            drawLogin();
        }

        private void Button_SignInManager(object sender, RoutedEventArgs e)
        {
            deleteSomethingInMain();
            signInMode = true;
            drawLogin();
        }

        private void deleteSomethingInMain()
        {
            canvas.Children.Remove(textSignInChosen);
            var buttons = canvas.Children.OfType<Button>().ToList();
            foreach (var button in buttons)
                canvas.Children.Remove(button);
        }
        static TextBox boxLoginUser;
        static PasswordBox boxLoginPassword;
        private void drawLogin()
        {
            TextBlock textLoginUser = new TextBlock();
            textLoginUser.Text = "Tên đăng nhập";
            textLoginUser.Foreground = Brushes.White;
            textLoginUser.Height = 30;
            textLoginUser.Width = 200;
            textLoginUser.FontSize = 20;
            Canvas.SetTop(textLoginUser, 200);
            Canvas.SetLeft(textLoginUser, leftPadding);
            canvas.Children.Add(textLoginUser);

            boxLoginUser = new TextBox();
            boxLoginUser.Height = 40;
            boxLoginUser.Width = 500;
            boxLoginUser.FontSize = 18;
            boxLoginUser.Background = Brushes.Black;
            boxLoginUser.Foreground = Brushes.White;

            boxLoginUser.BorderThickness = new Thickness(3);
            Canvas.SetTop(boxLoginUser, 240);
            Canvas.SetLeft(boxLoginUser, leftPadding);
            canvas.Children.Add(boxLoginUser);

            TextBlock textLoginPassword = new TextBlock();
            textLoginPassword.Text = "Mật khẩu";
            textLoginPassword.Foreground = Brushes.White;
            textLoginPassword.Height = 30;
            textLoginPassword.Width = 200;
            textLoginPassword.FontSize = 20;
            Canvas.SetTop(textLoginPassword, 280);
            Canvas.SetLeft(textLoginPassword, leftPadding);
            canvas.Children.Add(textLoginPassword);

            boxLoginPassword = new PasswordBox();
            boxLoginPassword.Height = 40;
            boxLoginPassword.Width = 500;
            boxLoginPassword.FontSize = 18;
            boxLoginPassword.Background = Brushes.Black;
            boxLoginPassword.Foreground = Brushes.White;
            boxLoginPassword.BorderThickness = new Thickness(3);
            Canvas.SetTop(boxLoginPassword, 320);
            Canvas.SetLeft(boxLoginPassword, leftPadding);
            canvas.Children.Add(boxLoginPassword);


            //Button Back
            Button buttonBack = new Button();
            buttonBack.Height = 50;
            buttonBack.Width = 220;
            buttonBack.BorderThickness = new Thickness(2);
            buttonBack.BorderBrush = Brushes.Aqua;
            buttonBack.Background = Brushes.Transparent;
            buttonBack.Foreground = Brushes.Aquamarine;
            buttonBack.Content = "Quay lại";
            buttonBack.Click += BackToMainFromLogin;
            Canvas.SetTop(buttonBack, 380);
            Canvas.SetLeft(buttonBack, leftPadding);
            canvas.Children.Add(buttonBack);

            //Button Sign in
            Button buttonSignIn = new Button();
            buttonSignIn.IsDefault = true;
            buttonSignIn.Height = 50;
            buttonSignIn.Width = 220;
            buttonSignIn.BorderThickness = new Thickness(2);
            buttonSignIn.BorderBrush = Brushes.Aqua;

            buttonSignIn.Background = Brushes.Transparent;
            buttonSignIn.Foreground = Brushes.Aquamarine;
            buttonSignIn.Content = "Đăng nhập";
            buttonSignIn.Click += SignIn;
            Canvas.SetTop(buttonSignIn, 380);
            Canvas.SetLeft(buttonSignIn, 430);
            canvas.Children.Add(buttonSignIn);
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            
            if (boxLoginUser.Text.Length == 0 || boxLoginPassword.Password.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đủ các trường");
                return;
            }
            int accountType = signInMode == false ? 2 : 1;
            string sql = $"Select * from NhanVien where TenDN = '{boxLoginUser.Text}' and LoaiNV={accountType} and MatKhau='{boxLoginPassword.Password}'";
            SqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();


                // Tạo một đối tượng Command.
                SqlCommand cmd = new SqlCommand();

                // Liên hợp Command với Connection.
                cmd.Connection = conn;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (signInMode == false)
                        {
                            MenuWindow menu = new MenuWindow();
                            menu.Show();
                            this.Close();                        }
                        else
                        {
                            var mng = new ManagerWindow();
                            mng.Show();
                            this.Close();
                        }
                        return;
                    }
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu");
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Lỗi kết nối, vui lòng thử lại");
            }
        }

        private void BackToMainFromLogin(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            signInMode = false;
            main.Show();
            this.Close();
        }

        public static string GetUsername()
        {
            return boxLoginUser.Text;
        }
    }
}
