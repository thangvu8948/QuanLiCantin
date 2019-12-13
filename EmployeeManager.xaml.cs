using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Common;
using System.ComponentModel;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for EmployeeManager.xaml
    /// </summary>
    public partial class EmployeeManager : UserControl
    {
        private static ObservableCollection<Employee> EMPLOYEES{ get; set; } = EmployeeSQL.GetAllEmployees();
        private static ListCollectionView DisplayedEmployee { get; set; } = null;


        public EmployeeManager()
        {
            InitializeComponent();
            EM_UI.Children.Remove(BlockScreen);
            EM_UI.Children.Remove(RemoveRecordBox);
            EM_UI.Children.Remove(EmpAddBox);

            DisplayedEmployee = new ListCollectionView(EMPLOYEES)
            {
                Filter = null
            };
            EmployeeTable.ItemsSource = DisplayedEmployee;
        }

        class Employee : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _id, _name, _loginName, _password;
            private int _role;

            public string ID 
            {
                get{ return _id;}
                set{ _id = value; }
            }
            public int Role 
            {
                get { return _role; }
                set { if (_role != value) { _role = value; NotifyPropertyChanged("Role"); } }
            }
            public string Name
            {
                get { return _name; }
                set { if (_name != value) { _name = value; NotifyPropertyChanged("Name"); } }
            }
            public string LoginName
            {
                get { return _loginName; }
                set { if (_loginName != value) { _loginName = value; NotifyPropertyChanged("LoginName"); } }
            }
            public string Password
            {
                get { return _password; }
                set { if (_password != value) { _password = value; NotifyPropertyChanged("Password"); } }
            }

            public Employee(string id, int role, string name, string loginName, string password)
            {
                ID = id;
                Role = role;
                Name = name;
                LoginName = loginName;
                Password = password;
            }

            public Employee((string, int, string, string, string) initializer)
                => (ID, Role, Name, LoginName, Password) = initializer;

            private void NotifyPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

///--------------------------------------------------------------------------------------------------

        class EmployeeSQL
        {
            public static ObservableCollection<Employee> GetAllEmployees()
            {
                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM NhanVien", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var emp_list = new ObservableCollection<Employee>();
                            while (r.Read())
                            {
                                var id = Convert.ToString(r.GetValue(0)).Trim();
                                var role = Convert.ToInt32(r.GetValue(1));
                                var name = Convert.ToString(r.GetValue(2)).Trim();
                                var loginName = Convert.ToString(r.GetValue(3)).Trim();
                                var password = Convert.ToString(r.GetValue(4)).Trim();
                                emp_list.Add(new Employee(id, role, name, loginName, password));
                            }
                            return emp_list;
                        }
                        return null;
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("Lỗi truy xuất dũ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return null;
            }


            public static bool AddEmployee(string id, int role, string name, string loginName, string password)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO NhanVien VALUES (@id, @role, @name, @loginName, @password)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@loginName", loginName);
                    cmd.Parameters.AddWithValue("@password", password);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi thêm dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }

            public static bool AddAccount(string loginName, string password, int type)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO TaiKhoan VALUES (@loginName, @password, @type)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@loginName", loginName);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@type", type);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi thêm dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }

            public static bool FindAccount(string loginName)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"SELECT * FROM TaiKhoan WHERE ID = @loginName";

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@loginName", loginName);
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return true;
                        }
                        return false;
                    }
                    cmd.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi thêm dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return false;
            }

            public static bool RemoveEmployee(string id)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"DELETE FROM NhanVien WHERE MaNV = @id";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi xóa dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }

            public static bool UpdateEmployee(string id, int role, string name, string loginName, string password)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"UPDATE NhanVien " +
                    $"SET LoaiNV = @role, Ten = @name, TenDN = @loginName, MatKhau = @password " +
                    $"WHERE MaNV = @id";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@loginName", loginName);
                    cmd.Parameters.AddWithValue("@password", password);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi cập nhật dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }
        }




        private void ShowAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            DisplayedEmployee.Filter = null;
        }

///--------------------------------------------------------------------------------------------------


        public bool EmployeeFilter(object obj)
        {
            var emp = obj as Employee;
            int role = RoleSearchBox.SelectedIndex + 1;
            var name = NameSearchBox.Text;
            if (role > 0 && role < 3)
            {
                if (emp.Role == role)
                    return emp.Name.ToLower().Contains(name.ToLower().Trim());
                else
                    return false;
            }
            else
            {
                if (name.Length > 0)
                    return emp.Name.ToLower().Contains(name.ToLower().Trim());
                return true;
            }
        }

        private void SearchEmployees_Click(object sender, RoutedEventArgs e)
        {
           DisplayedEmployee.Filter = new Predicate<object>(EmployeeFilter);
        }

        ///--------------------------------------------------------------------------------------------------

        protected bool empAddFirstLoad = true;
        protected bool addClick, updateClick = false;


        private void AddEmployees_Click(object sender, RoutedEventArgs e)
        {
            addClick = true; updateClick = false;

            Global.HighlightButton(AddEmployee);

            if (!EM_UI.Children.Contains(BlockScreen))
                EM_UI.Children.Add(BlockScreen);

            EM_UI.Children.Add(EmpAddBox);
            EmpAddBox.Title.Text = "Thêm nhân viên";

            if (empAddFirstLoad is true)
            {
                EmpAddBox.Confirm.Click += Add_Confirm_Click;
                EmpAddBox.Quit.Click += Add_Quit_Click;
                empAddFirstLoad = false;
            }
        }


        private void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            addClick = false; updateClick = true;

            Global.HighlightButton(UpdateEmployee);

            if (!EM_UI.Children.Contains(BlockScreen))
                EM_UI.Children.Add(BlockScreen);

            EM_UI.Children.Add(EmpAddBox);
            EmpAddBox.Title.Text = "Sửa thông tin nhân viên";

            if (empAddFirstLoad is true)
            {
                EmpAddBox.Confirm.Click += Add_Confirm_Click;
                EmpAddBox.Quit.Click += Add_Quit_Click;
                empAddFirstLoad = false;
            }
        }

        private void Add_Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (EmpAddBox.AllValid())
            {
                var emp = new Employee(EmpAddBox.GetInputData());
                bool success = false;
                //Insert
                if (addClick == true)
                {
                    bool n = EmployeeSQL.FindAccount(emp.LoginName);
                    if (n == false)
                    {
                        EmployeeSQL.AddAccount(emp.LoginName, emp.Password, emp.Role);
                    }
                    bool dupLoginName = EMPLOYEES.Any(i => i.LoginName == emp.LoginName);
                    if (!dupLoginName)
                        success = EmployeeSQL.AddEmployee(emp.ID, emp.Role, emp.Name, emp.LoginName, emp.Password);
                    else
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại");
                    }
                    if (success)
                        EMPLOYEES.Add(emp);

                }

                //Update
                else
                {
                    success = EmployeeSQL.UpdateEmployee(emp.ID, emp.Role, emp.Name, emp.LoginName, emp.Password);
                    if (success)
                    {
                        for (int i = 0, sz = EMPLOYEES.Count; i < sz; ++i)
                        {
                            if (EMPLOYEES[i].ID == emp.ID)
                            {
                                EMPLOYEES[i] = emp;
                                break;
                            }
                        }
                    }
                }

                if (success)
                {
                    EM_UI.Children.Remove(EmpAddBox);
                    EM_UI.Children.Remove(BlockScreen);
                    Global.UnhighlightButton(addClick == true ? AddEmployee : UpdateEmployee);
                    DisplayedEmployee.Filter = null;
                }
            }
            else
            {
                return;
            }
        }

        private void Add_Quit_Click(object sender, RoutedEventArgs e)
        {
            EM_UI.Children.Remove(EmpAddBox);
            EM_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(addClick == true ? AddEmployee : UpdateEmployee);
        }


 ///--------------------------------------------------------------------------------------------------

        protected bool removeFirstLoad = true;


        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!EM_UI.Children.Contains(BlockScreen))
                EM_UI.Children.Add(BlockScreen);
            EM_UI.Children.Add(RemoveRecordBox);
            Global.HighlightButton(RemoveEmployee);

            if (removeFirstLoad == true)
            {
                RemoveRecordBox.Confirm.Click += RemoveConfirm_Click;
                RemoveRecordBox.Quit.Click += RemoveQuit_Click;
                removeFirstLoad = false;
            }
        }

        private void RemoveRecordBox_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveRecordBox.Title.Text = "Xóa nhân viên";
            RemoveRecordBox.OptionName.Text = "Nhập ID nhân viên:";
            RemoveRecordBox.Reset();
        }


        private void RemoveConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (RemoveRecordBox.IsValid())
            {
                var input = RemoveRecordBox.InputBox.Text;

                int index = 0;

              
                for (int i = 0, sz = EMPLOYEES.Count; i < sz; ++i)
                {
                    if (EMPLOYEES[i].ID == input)
                    {
                        index = i; break;
                    }
                }

                
                if (EMPLOYEES[index].Name.ToUpper() == MainWindow.GetUsername())
                {
                    MessageBox.Show("Không thể xóa tài khoản đang đăng nhập");
                    return;
                }

                else
                {
                    bool success = EmployeeSQL.RemoveEmployee(input);
                    if (success)
                    {
                        EMPLOYEES.RemoveAt(index);
                        EM_UI.Children.Remove(RemoveRecordBox);
                        EM_UI.Children.Remove(BlockScreen);
                        Global.UnhighlightButton(RemoveEmployee);
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void RemoveQuit_Click(object sender, RoutedEventArgs e)
        {
            EM_UI.Children.Remove(RemoveRecordBox);
            EM_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(RemoveEmployee);
        }


///--------------------------------------------------------------------------------------------------

        private void EmployeeTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

///--------------------------------------------------------------------------------------------------

        private void RoleSearchBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void NameSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EmpAddBox_Loaded(object sender, RoutedEventArgs e)
        {
            EmpAddBox.Reset();
        }
    }
}
