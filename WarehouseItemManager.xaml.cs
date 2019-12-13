using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace QuanLiCantin
{
    /// <summary>
    /// Interaction logic for WarehouseItemManager.xaml
    /// </summary>
    public partial class WarehouseItemManager : UserControl
    {
        private static ObservableCollection<Item> ITEMS { get; set; } = ItemSQL.GetAllItems();
        private static ListCollectionView DisplayedItems { get; set; } = null;

        public WarehouseItemManager()
        {
            InitializeComponent();
            WH_I_UI.Children.Remove(BlockScreen);
            WH_I_UI.Children.Remove(RemoveRecordBox);
            WH_I_UI.Children.Remove(ItemAddBox);

            DisplayedItems = new ListCollectionView(ITEMS)
            {
                Filter = null
            };
            ItemTable.ItemsSource = DisplayedItems;
        }


        class Item : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _id, _name;
            private double _remain;

            public string MaHH
            {
                get { return _id; }
                set { _id = value; }
            }

            public string TenHH
            {
                get { return _name; }
                set { if (_name != value){ _name = value; NotifyPropertyChanged("TenHH"); } }
            }

            public double KhoiLuong
            {
                get { return _remain; }
                set { if (_remain != value) { _remain = value; NotifyPropertyChanged("KhoiLuong"); } }
            }
            public Item(string id, string name, double remain)
            {
                MaHH = id;
                TenHH = name;
                KhoiLuong = remain;
            }

            public Item((string, string, double) initializer)
                => (MaHH, TenHH, KhoiLuong) = initializer;


            private void NotifyPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ///-----------------------------------------------------------------------------------

        class ItemSQL
        {
            public static ObservableCollection<Item> GetAllItems()
            {
                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM HangHoa", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var storageList = new ObservableCollection<Item>();
                            while (r.Read())
                            {
                                var id = Convert.ToString(r.GetValue(0)).Trim();
                                var name = Convert.ToString(r.GetValue(1)).Trim();
                                var remain = Convert.ToDouble(r.GetValue(2));
                                storageList.Add(new Item(id, name, remain));
                            }
                            return storageList;
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


            public static bool AddItem(string id, string name, double remain)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO HangHoa VALUES (@id, @name, @remain)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@remain", remain);
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

            public static bool RemoveItem(string id)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"DELETE FROM HangHoa WHERE MaHH = @id";

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

            public static bool UpdateItem(string id, string name, double remain)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"UPDATE HangHoa " +
                    $"SET TenHH = @name, SoLuong = @remain WHERE MaHH = @id";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@remain", remain);
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

        ///-----------------------------------------------------------------------------------

        public bool Contains(object obj)
        {
            var item = obj as Item;
            return item.TenHH.ToLower().Contains(ItemFindBox.Text.ToLower().Trim());
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayedItems.Filter = ItemFindBox.Text.Length > 0 ? new Predicate<object>(Contains) : null;
        }


        private void ItemFindBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void ShowAllItemsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayedItems.Filter = null;
        }


        ///-----------------------------------------------------------------------------------

        protected bool itemAddFirstLoad = true;
        protected bool addClick, updateClick = false;


        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            addClick = true; updateClick = false;

            Global.HighlightButton(AddItemButton);

            if (!WH_I_UI.Children.Contains(BlockScreen))
                WH_I_UI.Children.Add(BlockScreen);

            WH_I_UI.Children.Add(ItemAddBox);
            ItemAddBox.Title.Text = "Thêm hàng hóa";

            if (itemAddFirstLoad is true)
            {
                ItemAddBox.Confirm.Click += Add_Confirm_Click;
                ItemAddBox.Quit.Click += Add_Quit_Click;
                itemAddFirstLoad = false;
            }
        }


        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            addClick = false; updateClick = true;

            Global.HighlightButton(UpdateItem);

            if (!WH_I_UI.Children.Contains(BlockScreen))
                WH_I_UI.Children.Add(BlockScreen);

            WH_I_UI.Children.Add(ItemAddBox);
            ItemAddBox.Title.Text = "Sửa thông tin hàng hóa";

            if (itemAddFirstLoad is true)
            {
                ItemAddBox.Confirm.Click += Add_Confirm_Click;
                ItemAddBox.Quit.Click += Add_Quit_Click;
                itemAddFirstLoad = false;
            }
        }

        private void Add_Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ItemAddBox.AllValid())
            {
                var item = new Item(ItemAddBox.GetInputData());
                bool success;
                //Insert
                if (addClick == true)
                {
                    success = ItemSQL.AddItem(item.MaHH, item.TenHH, item.KhoiLuong);
                    if (success)
                        ITEMS.Add(item);
                }

                //Update
                else
                {
                    success = ItemSQL.UpdateItem(item.MaHH, item.TenHH, item.KhoiLuong);
                    if (success)
                    {
                        for (int i = 0, sz = ITEMS.Count; i < sz; ++i)
                        {
                            if (ITEMS[i].MaHH == item.MaHH)
                            {
                                ITEMS[i] = item;
                                break;
                            }
                        }
                    }
                }

                if (success)
                {
                    WH_I_UI.Children.Remove(ItemAddBox);
                    WH_I_UI.Children.Remove(BlockScreen);
                    Global.UnhighlightButton(addClick == true ? AddItemButton : UpdateItem);
                    DisplayedItems.Filter = null;
                }
            }
            else
            {
                return;
            }
        }

        private void Add_Quit_Click(object sender, RoutedEventArgs e)
        {
            WH_I_UI.Children.Remove(ItemAddBox);
            WH_I_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(addClick == true ? AddItemButton : UpdateItem);
        }


        ///-----------------------------------------------------------------------------------

        protected bool removeFirstLoad = true;


        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (!WH_I_UI.Children.Contains(BlockScreen))
                WH_I_UI.Children.Add(BlockScreen);
            WH_I_UI.Children.Add(RemoveRecordBox);
            Global.HighlightButton(DeleteItem);

            if (removeFirstLoad == true)
            {
                RemoveRecordBox.Confirm.Click += RemoveConfirm_Click;
                RemoveRecordBox.Quit.Click += RemoveQuit_Click;
                removeFirstLoad = false;
            }
        }

        private void RemoveRecordBox_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveRecordBox.Title.Text = "Xóa hàng hóa";
            RemoveRecordBox.OptionName.Text = "Nhập mã hàng hóa:";
            RemoveRecordBox.InputBox.Text = string.Empty;
        }


        private void RemoveConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (RemoveRecordBox.IsValid())
            {
                var input = RemoveRecordBox.InputBox.Text;
                bool success = ItemSQL.RemoveItem(input);
                if (success)
                {
                    for (int i = 0, sz = ITEMS.Count; i < sz; ++i)
                    {
                        if (ITEMS[i].MaHH == input)
                        {
                            ITEMS.RemoveAt(i);
                            break;
                        }
                    }
                    WH_I_UI.Children.Remove(RemoveRecordBox);
                    WH_I_UI.Children.Remove(BlockScreen);
                    Global.UnhighlightButton(DeleteItem);
                }
            }
            else
            {
                return;
            }
        }

        private void RemoveQuit_Click(object sender, RoutedEventArgs e)
        {
            WH_I_UI.Children.Remove(RemoveRecordBox);
            WH_I_UI.Children.Remove(BlockScreen);
            Global.UnhighlightButton(DeleteItem);
        }


        ///-----------------------------------------------------------------------------------

        private void ItemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ItemAddBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
