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
        private static ObservableCollection<AMenuItem> MENU_ITEMS { get; set; } = MenuSQL.GetAllMenuItems();
        private static ListCollectionView DisplayedMenuItems { get; set; } = null;
        

        public MenuManager()
        {
            InitializeComponent();
            
            MM_UI.Children.Remove(RemoveRecordBox);
            MM_UI.Children.Remove(BlockScreen);
            MM_UI.Children.Remove(MenuAddBox);

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

            private string name;
            private int id, count, price, type;
            private string pic;

            public int MAMON
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

            public AMenuItem(int id, string name, int price, int count, int type, string pic)
            {
                MAMON = id;
                TENMON = name;
                GIATIEN = price;
                SOLUONG = count;
                MALOAI = type;
                HinhAnh = pic;
            }

            public AMenuItem((int, string, int, int, int, string) initializer)
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
                    using (var cmd = new SqlCommand("SELECT * FROM MonAn ORDER BY cast(MAMON as int) asc", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var menuList = new ObservableCollection<AMenuItem>();
                            while (r.Read())
                            {
                                var id = Convert.ToInt32(r.GetValue(0));
                                var name = Convert.ToString(r.GetValue(1)).Trim();
                                var price = Convert.ToInt32(r.GetValue(2));
                                var count = Convert.ToInt32(r.GetValue(3));
                                var type = Convert.ToInt32(r.GetValue(4));
                                var pic = Convert.ToString(r.GetValue(5)).Trim();
                                menuList.Add(new AMenuItem(id, name, price, count, type, pic));
                            }
                            return menuList;
                        }
                        return null;
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show($"Lỗi truy xuất dữ liệu");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return null;
            }


            public static bool AddMenuItem
                (int id, string name, int price, int count, string type)
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

            public static bool RemoveMenuItem(int id)
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

            public static bool UpdateMenuItem
                (int id, string name, int price, int count, string type)
            {
                var conn = DBUtils.GetDBConnection();                

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

        protected bool removeFirstLoad = true;

        private void RemoveRecordBox_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveRecordBox.Title.Text = "Xóa món";
            RemoveRecordBox.OptionName.Text = "ID món cần xóa:";
            RemoveRecordBox.Reset();
        }

        private void RemoveFromMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!MM_UI.Children.Contains(BlockScreen))
                MM_UI.Children.Add(BlockScreen);
            MM_UI.Children.Add(RemoveRecordBox);
            Global.HighlightButton(RemoveFromMenu);

            if (removeFirstLoad)
            {
                RemoveRecordBox.Confirm.Click += RemoveConfirm_Click;
                RemoveRecordBox.Quit.Click += RemoveQuit_Click;
                removeFirstLoad = false;
            }
        }

        private void RemoveConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (RemoveRecordBox.IsValid())
            {
                var input = Convert.ToInt32(RemoveRecordBox.InputBox.Text);
                bool success = MenuSQL.RemoveMenuItem(input);
                if (success)
                {
                    for (int i = 0, sz = MENU_ITEMS.Count; i < sz; ++i)
                    {
                        if (MENU_ITEMS[i].MAMON == input)
                        {
                            MENU_ITEMS.RemoveAt(i);
                            break;
                        }
                    }
                    MM_UI.Children.Remove(RemoveRecordBox);
                    MM_UI.Children.Remove(BlockScreen);
                    Global.UnhighlightButton(RemoveFromMenu);
                }
            }
            else
            {
                return;
            }
        }

        private void RemoveQuit_Click(object sender, RoutedEventArgs e)
        {
            MM_UI.Children.Remove(RemoveRecordBox);
            MM_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(RemoveFromMenu);
        }

///--------------------------------------------------------------------------------------------------

        protected bool itemAddFirstLoad = true;
        protected bool addClick, updateClick = false;


        private void AddToMenu_Click(object sender, RoutedEventArgs e)
        {
            addClick = true; updateClick = false;

            Global.HighlightButton(AddToMenu);

            if (!MM_UI.Children.Contains(BlockScreen))
                MM_UI.Children.Add(BlockScreen);

            MM_UI.Children.Add(MenuAddBox);
            MenuAddBox.Title.Text = "Thêm món";

            if (itemAddFirstLoad is true)
            {
                MenuAddBox.Confirm.Click += Confirm_Click;
                MenuAddBox.Quit.Click += Quit_Click; ;
                itemAddFirstLoad = false;
            }
        }


        private void UpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            addClick = false; updateClick = true;

            Global.HighlightButton(UpdateMenu);

            if (!MM_UI.Children.Contains(BlockScreen))
                MM_UI.Children.Add(BlockScreen);

            MM_UI.Children.Add(MenuAddBox);
            MenuAddBox.Title.Text = "Chỉnh sửa món";

            if (itemAddFirstLoad is true)
            {
                MenuAddBox.Confirm.Click += Confirm_Click;
                MenuAddBox.Quit.Click += Quit_Click; ;
                itemAddFirstLoad = false;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            MM_UI.Children.Remove(MenuAddBox);
            MM_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(addClick ? AddToMenu : UpdateMenu);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (MenuAddBox.AllValid())
            {
                var item = new AMenuItem(MenuAddBox.GetInputData());
                bool success;
                //Insert
                if (addClick == true)
                {
                    success =  MenuSQL.AddMenuItem(item.MAMON, item.TENMON, item.GIATIEN, item.SOLUONG, item.MALOAI.ToString());
                    if (success)
                        MENU_ITEMS.Add(item);
                }

                //Update
                else
                {
                    success = MenuSQL.UpdateMenuItem(item.MAMON, item.TENMON, item.GIATIEN, item.SOLUONG, item.MALOAI.ToString());
                    if (success)
                    {
                        for (int i = 0, sz = MENU_ITEMS.Count; i < sz; ++i)
                        {
                            if (MENU_ITEMS[i].MAMON == item.MAMON)
                            {
                                MENU_ITEMS[i] = item;
                                break;
                            }
                        }
                    }
                }

                if (success)
                {
                    MM_UI.Children.Remove(MenuAddBox);
                    MM_UI.Children.Remove(BlockScreen);
                    Global.UnhighlightButton(addClick == true ? AddToMenu : UpdateMenu);
                    DisplayedMenuItems.Filter = null;
                }
            }
            else
            {
                return;
            }
        }


        ///--------------------------------------------------------------------------------------------------

        private void MenuItemTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuAddBox_Loaded(object sender, RoutedEventArgs e)
        {
            MenuAddBox.Reset();
        }

        private void MenuItemNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
