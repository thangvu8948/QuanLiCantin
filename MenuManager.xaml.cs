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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for MenuManager.xaml
    /// </summary>
    public partial class MenuManager : UserControl
    {
        private static ObservableCollection<AMenuItem> MENU_ITEMS { get; set; } = null;
        private static ListCollectionView DisplayedMenuItems { get; set; } = null;
        private readonly Color purple = Color.FromArgb(0xFF, 67, 0x3A, 0xB7);
        

        public MenuManager()
        {
            InitializeComponent();
            MENU_ITEMS = new ObservableCollection<AMenuItem>(MenuSQL.GetAllMenuItems());
            DisplayedMenuItems = new ListCollectionView(MENU_ITEMS)
            {
                Filter = null
            };
            MenuTable.ItemsSource = DisplayedMenuItems;
        }

        class AMenuItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string id, name;
            private int count, price, type;
            private string pic;

            public string MAMON
            {
                get { return id; }
                set { id = value; }
            }
            public string TENMON
            {
                get { return name; }
                set { if (name != value) { name = value; NotifyPropertyChanged("TENMON"); } }
            }
            public int GIATIEN
            {
                get { return price; }
                set { if (price != value) { price = value; NotifyPropertyChanged("GIATIEN"); } }
            }

            public int SOLUONG
            {
                get { return count; }
                set { if (count != value) { count = value; NotifyPropertyChanged("SOLUONG"); } }
            }

            public int MALOAI
            {
                get { return type; }
                set { if (type != value) { type = value; NotifyPropertyChanged("MALOAI"); } }
            }

            public string HinhAnh
            {
                get { return pic; }
                set { if (pic != value) { pic = value; NotifyPropertyChanged("HinhAnh"); } }
            }

            public AMenuItem(string id, string name, int price, int count, int type, string pic)
            {
                MAMON = id;
                TENMON = name;
                GIATIEN = price;
                SOLUONG = count;
                MALOAI = type;
                HinhAnh = pic;
            }

            public AMenuItem((string, string, int, int, int, string) initializer)
                => (MAMON, TENMON, GIATIEN, SOLUONG, MALOAI, HinhAnh) = initializer;

            private void NotifyPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        class MenuSQL
        {
            public static ObservableCollection<AMenuItem> GetAllMenuItems()
            {
                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM MonAn", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var menuList = new ObservableCollection<AMenuItem>();
                            while (r.Read())
                            {
                                var id = r.GetString(0);
                                var name = r.GetString(1);
                                var price = r.GetInt32(2);
                                var count = r.GetInt32(3);
                                var type = Convert.ToInt32(r.GetValue(4));
                                var pic = r.GetString(5);
                                menuList.Add(new AMenuItem(id, name, price, count, type, pic));
                            }
                            return menuList;
                        }
                        return null;
                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show($"Lỗi truy xuất dữ liệu\n{e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return null;
            }


            public static bool AddMenuItem
                (string id, string name, int price, int count, string type)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO MonAn VALUES (@id, @name, @price, @count, @type, @pic)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@pic", $"Images/{id}/download.jpg");
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Lỗi khi thêm dữ liệu\n{e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }

            public static bool RemoveMenuItem(string id)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"DELETE FROM MonAn WHERE MAMON = @id";

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
                    MessageBox.Show($"Lỗi khi xóa dữ liệu\n{e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }

            public static bool UpdateMenuItem
                (string id, string name, int price, int count, string type)
            {
                var conn = DBUtils.GetDBConnection();
                string pic;
                

                string query = $"UPDATE MonAn " +
                    $"SET TENMON = @name, GIATIEN = @price, SOLUONG = @count, MALOAI = @type, HinhAnh = @pic " +
                    $"WHERE MAMON = @id";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@pic", $"Images/{id}/download.jpg");
                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Lỗi khi cập nhật dữ liệu\n{e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                return affectedRows > 0;
            }
        }

        private void MenuTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ItemTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

///--------------------------------------------------------------------------------------------------
    
        public bool MenuFilter(object obj)
        {
            var mItem = obj as AMenuItem;
            int type = MenuItemTypeBox.SelectedIndex + 1;
            var name = MenuItemNameBox.Text;
            if (type > 0 && type < 4)
            {
                if (mItem.MALOAI == type)
                    return mItem.TENMON.ToLower().Contains(name.ToLower().Trim());
                else
                    return false;
            }
            else
            {
                if (name.Length > 0)
                    return mItem.TENMON.ToLower().Contains(name.ToLower().Trim());
                return true;
            }
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisplayedMenuItems.Filter = new Predicate<object>(MenuFilter);
        }


        private void ShowAllMenuItems_Click(object sender, RoutedEventArgs e)
        {
            DisplayedMenuItems.Filter = null;
        }

///--------------------------------------------------------------------------------------------------


        private void RemoveFromMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateMenu_Click(object sender, RoutedEventArgs e)
        {

        }

///--------------------------------------------------------------------------------------------------

        private void MenuItemTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItemNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
