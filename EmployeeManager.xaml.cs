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

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for EmployeeManager.xaml
    /// </summary>
    public partial class EmployeeManager : UserControl
    {
        private static ObservableCollection<Employee> EMPLOYEES{ get; set; } = null;
        private static ListCollectionView DisplayedEmployee { get; set; } = null;
        private readonly bool use_sql = false;


        public EmployeeManager()
        {
            InitializeComponent();
            EM_UI.Children.Remove(EmployeeSearchBox);

            var tooltip = new ToolTip
            {
                Content = "Vai trò:\n1:Quản lý, 2:Nhân viên"
            };
            InfoButton.ToolTip = tooltip;

            if (!use_sql)
                EMPLOYEES = new ObservableCollection<Employee>(LoadRandomEmployees());
            else
                EMPLOYEES = new ObservableCollection<Employee>(EmployeeSQL.GetAllEmployees());
            DisplayedEmployee = new ListCollectionView(EMPLOYEES);
            EmployeeTable.ItemsSource = DisplayedEmployee;
        }

        class Employee
        {
            public string ID { get; set; }
            public int Role { get; set; }
            public string Name { get; set; }
            public string LoginName { get; set; }
            public string Password { get; set; }

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
        }

        private ObservableCollection<Employee> LoadRandomEmployees()
        {
            var list = new ObservableCollection<Employee>
            {
                new Employee("175", 2, "Trần Văn A", "AAA", "AAA1"),
                new Employee("922", 2, "Nguyễn Thị B", "BBB", "BBB1"),
                new Employee("028", 1, "Phạm Hoàng C", "CCC", "CCC1"),
                new Employee("332", 2, "Nguyễn Văn D", "DDD", "DDD1"),
                new Employee("997", 2, "Lê Quốc E", "EEE", "EEE1"),
            };

            return list;
        }



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

                catch (Exception e)
                {
                    MessageBox.Show("Error loading data");
                    Debug.WriteLine($"Error: {e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return null;
            }


            public static void AddEmployee(string id, int role, string name, string loginName, string password)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO NhanVien VALUES = (@id, @role, @name, @loginName, @password)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@birthday", loginName);
                    cmd.Parameters.AddWithValue("@startDate", password);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error inserting data");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    if (affectedRows > 0)
                    {
                        EMPLOYEES = GetAllEmployees();
                    }
                    conn.Close();
                    conn.Dispose();
                }

            }

            public static void RemoveEmployeeByID(string id)
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
                catch (Exception e)
                {
                    MessageBox.Show("Error removing data");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    if (affectedRows > 0)
                    {
                        EMPLOYEES = GetAllEmployees();
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }

            public static void UpdateEmployeeByID(string id, int columnIndex, string updatedValue)
            {
                string[] properties = { "MaNV", "LoaiNV", "Ten", "TenDN", "MatKhau" };
                var conn = DBUtils.GetDBConnection();

                string query = $"UPDATE NhanVien SET {properties[columnIndex]} = @updatedValue WHERE MANV = @id";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@updatedValue", updatedValue);
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error updating data");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    if (affectedRows > 0)
                    {
                        EMPLOYEES = GetAllEmployees();
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
        }




        private void ShowAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            DisplayedEmployee.Filter = null;
        }

        public bool EmployeeFilter(object obj)
        {
            var emp = obj as Employee;
            int role = EmployeeSearchBox.RoleSelection.SelectedIndex + 1;
            var name = EmployeeSearchBox.NameSearchBox.Text;
            if (role > 0 && role < 3)
            {
                if (emp.Role == role)
                    return emp.Name.ToLower().Contains(name.ToLower().Trim());
                else
                    return false;
            }
            else
            {
                return emp.Name.ToLower().Contains(name.ToLower().Trim());
            }
        }


        protected bool search_first_load = true;

        private void SearchEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (!EM_UI.Children.Contains(EmployeeSearchBox))
                EM_UI.Children.Add(EmployeeSearchBox);
            if (search_first_load is true)
            {
                EmployeeSearchBox.Confirm.Click += Confirm_Click;
                EmployeeSearchBox.Quit.Click += Quit_Click;
                search_first_load = false;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            EM_UI.Children.Remove(EmployeeSearchBox);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            DisplayedEmployee.Filter = new Predicate<object>(EmployeeFilter);
            EM_UI.Children.Remove(EmployeeSearchBox);
        }

        private void EmployeeSearchUI_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void EmployeeTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddEmployees_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RemoveEmployees_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EmployeeSearchBox_MouseEnter(object sender, MouseEventArgs e)
        {
            EmployeeSearchBox.Opacity = 1;
        }

        private void EmployeeSearchBox_MouseLeave(object sender, MouseEventArgs e)
        {
            EmployeeSearchBox.Opacity = 0.6;
        }
    }
}
