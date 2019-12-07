using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Warehouse.xaml
    /// </summary>
    public partial class Warehouse : UserControl
    {
        private static ObservableCollection<WarehouseItem> ITEMS { get; set; } = null;
        private static CollectionView DisplayItems { get; set; } = null;
        private readonly bool use_sql = false;


        public Warehouse()
        {
            InitializeComponent();
            WH_UI.Children.Remove(ItemAddBox);
            if (!use_sql)
                ITEMS = new ObservableCollection<WarehouseItem>(LoadRandomItem());
            else
                ITEMS = new ObservableCollection<WarehouseItem>(WarehouseSQL.GetAllItems());
            DisplayItems = new ListCollectionView(ITEMS);
            ItemTable.ItemsSource = DisplayItems;
            
        }


        class WarehouseItem : INotifyPropertyChanged, IComparable<WarehouseItem>
        {
            private string _id, _name, _unit;
            private double _qu;

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public string ID
            {
                get
                {
                    return _id;
                }
                set
                {
                    if (_id != value)
                    {
                        _id = value;
                        NotifyPropertyChanged("ID");
                    }
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
                    if (_name != value)
                    {
                        _name = value;
                        NotifyPropertyChanged("Name");
                    }
                }
            }
            public string QuantityUnit
            {
                get
                {
                    return _unit;
                }
                set
                {
                    if (_unit != value)
                    {
                        _unit = value;
                        NotifyPropertyChanged("QuantityUnit");
                    }
                }
            }
            public double Quantity
            {
                get
                {
                    return _qu;
                }
                set
                {
                    if (_qu != value)
                    {
                        _qu = value;
                        NotifyPropertyChanged("Quantity");
                    }
                }
            }


            public WarehouseItem(string id, string name, string unit, double quantity)
            {
                _id = id;
                _name = name;
                _unit = unit;
                _qu = quantity;
            }

            public WarehouseItem((string, string, string, double) initializer)
                => (_id, _name, _unit, _qu) = initializer;

            public override bool Equals(object obj)
            {
                return obj is WarehouseItem && Equals(obj as WarehouseItem);
            }

            public bool Equals(WarehouseItem item)
            {
                return ID == item.ID;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

            public int CompareTo(WarehouseItem item)
            {
                if (item == null)
                    return 1;
                return ID.CompareTo(item.ID);
            }
        }

        private ObservableCollection<WarehouseItem> LoadRandomItem()
        {
            var list = new ObservableCollection<WarehouseItem>
            {
                new WarehouseItem("1", "Gạo", "kg", 25),
                new WarehouseItem("2", "Thịt bò", "kg", 5),
                new WarehouseItem("3", "Spaghetti", "kg", 7.2),
                new WarehouseItem("4", "Sữa bò", "lít", 50),
                new WarehouseItem("5", "Yogurt", "hộp", 70),
                new WarehouseItem("6", "Phở", "kg", 1.5),
                new WarehouseItem("7", "Siro dâu", "lít", 18),
                new WarehouseItem("8", "Dầu ăn", "lít", 100),
            };

            return list;
        }

        class WarehouseSQL
        {
            public static ObservableCollection<WarehouseItem> GetAllItems()
            {
                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM HangTonKho", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var item_list = new ObservableCollection<WarehouseItem>();
                            while (r.Read())
                            {
                                var id = Convert.ToString(r.GetValue(1)).Trim();
                                var name = Convert.ToString(r.GetValue(2));
                                var unit = Convert.ToString(r.GetValue(3));
                                var quantity = Convert.ToDouble(r.GetValue(4));

                                item_list.Add(new WarehouseItem(id, name, unit, quantity));
                            }
                            return item_list;
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

            public static ObservableCollection<WarehouseItem> GetItemsByProperty
                (int columnIndex, string value_to_match)
            {
                string[] properties = { "ID", "Ten_MH", "ĐVT", "KhoiLuongTon" };
                string query = $"SELECT * FROM HangTonKho where {properties[columnIndex]} = @value_to_match";

                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@value_to_match", value_to_match);
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var item_list = new ObservableCollection<WarehouseItem>();
                            while (r.Read())
                            {
                                var id = Convert.ToString(r.GetValue(1)).Trim();
                                var name = Convert.ToString(r.GetValue(2));
                                var unit = Convert.ToString(r.GetValue(3));
                                var quantity = Convert.ToDouble(r.GetValue(4));

                                item_list.Add(new WarehouseItem(id, name, unit, quantity));
                            }
                            return item_list;
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

            public static void UpdateItemByName(string name, int columnIndex, string updatedValue)
            {
                var conn = DBUtils.GetDBConnection();
                string[] properties = { "ID", "Ten_MH", "ĐVT", "KhoiLuongTon" };

                string query = $"UPDATE HangTonKho " +
                    $"SET {properties[columnIndex]} = @new_value " +
                    $"WHERE Ten_MH = @name";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@new_value", updatedValue);
                    affectedRows = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error updating data");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    if (affectedRows > 0)
                        ITEMS = GetAllItems();
                    conn.Close();
                    conn.Dispose();
                }
            }

            public static void AddItem(string id, string name, string unit, double quantity)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO HangTonKho " +
                    $"VALUES = (@id, @name, @unit, @quantity)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@unit", unit);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    affectedRows = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error inserting item");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    if (affectedRows > 0)
                        ITEMS = GetAllItems();
                    conn.Close();
                    conn.Dispose();
                }

            }
        }


        private void ItemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        //Warehouse UI (Un)load functions

        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void WarehouseUI_Unloaded(object sender, RoutedEventArgs e)
        {
            if (WH_UI.Children.Contains(ItemAddBox))
                WH_UI.Children.Remove(ItemAddBox);
        }

        protected bool itemAddFirstLoad = true;

        private void ItemAddBox_Loaded(object sender, RoutedEventArgs e)
        {
            ItemAddBox.EmptyAllField();
        }


        private void ItemAddBox_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (!WH_UI.Children.Contains(ItemAddBox))
                 WH_UI.Children.Add(ItemAddBox);
            if (itemAddFirstLoad is true)
            {
                ItemAddBox.Confirm.Click += Item_Add_Confirm;
                ItemAddBox.Quit.Click += Item_Add_Quit;
                itemAddFirstLoad = false;
            }
        }

        private void Item_Add_Confirm(object sender, RoutedEventArgs e)
        {
            var item = new WarehouseItem(ItemAddBox.GetInputData());
            if (use_sql)
            {
                WarehouseSQL.AddItem(item.ID, item.Name, item.QuantityUnit, item.Quantity);
                ITEMS = WarehouseSQL.GetAllItems();
            }
            else
            {
                ITEMS.Add(item);
            }
            WH_UI.Children.Remove(ItemAddBox);
        }

        private void Item_Add_Quit(object sender, RoutedEventArgs e)
        {
            WH_UI.Children.Remove(ItemAddBox);
        }

        private void ItemAddBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ItemAddBox.Opacity = 1.0;
        }

        private void ItemAddBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ItemAddBox.Opacity = 0.6;
        }

        private void ShowAllItemsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayItems = new ListCollectionView(ITEMS);
        }
    }
}