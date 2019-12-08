using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Content = "Vai trò:\n0:Nhân viên, 1:Quản lý, 2:Thủ kho"
            };
            InfoButton.ToolTip = tooltip;


            if (!use_sql)
                EMPLOYEES = new ObservableCollection<Employee>(LoadRandomEmployees());
            DisplayedEmployee = new ListCollectionView(EMPLOYEES);
            EmployeeTable.ItemsSource = DisplayedEmployee;
        }

        class Employee
        {
            private string _id, _name;
            private int _role;
            private DateTime _birthday, _startDate;

            public string ID
            {
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                }
            }
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            public int Role
            {
                get
                {
                    return _role;
                }
                set
                {
                    _role = value;
                }
            }
            public DateTime Birthday
            {
                get
                {
                    return _birthday;
                }
                set
                {
                    _birthday = value;
                }
            }
            public DateTime StartDate
            {
                get
                {
                    return _startDate;
                }
                set
                {
                    _startDate = value;
                }
            }

            public Employee(string id, string name, int role, DateTime birthday, DateTime startDate)
            {
                _id = id;
                _name = name;
                _role = role;
                _birthday = birthday;
                _startDate = startDate;
            }

            public Employee((string, string, int, DateTime, DateTime) initializer)
                => (_id, _name, _role, _birthday, _startDate) = initializer;
        }

        private ObservableCollection<Employee> LoadRandomEmployees()
        {
            var list = new ObservableCollection<Employee>
            {
                new Employee("175", "Trần Văn A", 1, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("814", "Nguyễn Thị B", 2, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("902", "Bùi Văn C", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("228", "Lê Nguyễn D", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("015", "Nguyễn Văn E", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("662", "Phạm Thị F", 1, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("441", "Phạm Văn G", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("450", "Nguyễn Lê H", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("003", "Trần Thị T", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("221", "Hoàng Văn H", 0, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11)),
                new Employee("663", "Trần Lê N", 2, new DateTime(1989, 11, 5), new DateTime(2018, 7, 11))
            };

            return list;
        }


        private void ShowAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            DisplayedEmployee.Filter = null;
        }

        public bool EmployeeFilter(object obj)
        {
            var emp = obj as Employee;
            int role = EmployeeSearchBox.RoleSelection.SelectedIndex;
            var name = EmployeeSearchBox.NameSearchBox.Text;
            if (role > -1 && role < 3)
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
