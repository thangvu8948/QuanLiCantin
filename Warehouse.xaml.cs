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
        ObservableCollection<WarehouseItem> wh_items = null;
        readonly bool load_from_sql = false;

        private void Rebind()
        {
            ItemTable.ItemsSource = wh_items;
        }

        public Warehouse()
        {
            InitializeComponent();
            if (load_from_sql == true)
            {
                wh_items = WarehouseSQL.GetItemsFromName();
            }
            else
            {
                wh_items = LoadRandomItem();
            }
            Rebind();
            DataContext = this;
        }

        class WarehouseItem : INotifyPropertyChanged
        {
            private string _id, _name, _unit;
            private double _qu;

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public string ID
            { 
                get
                {
                    return _id;
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
                    NotifyPropertyChanged();
                }
            }
            public string QuantityUnit
            {
                get
                {
                    return _unit;
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
                    _qu = value;
                    NotifyPropertyChanged();
                }
            }


            public WarehouseItem(string id, string name, string unit, double quantity)
            {
                _id = id;
                _name = name;
                _unit = unit;
                _qu = quantity;
            }
        }

        private ObservableCollection<WarehouseItem> LoadRandomItem()
        {
            var list = new ObservableCollection<WarehouseItem>();

            list.Add(new WarehouseItem("09125", "Gao", "kg", 25));
            list.Add(new WarehouseItem("63821", "Thit bo", "kg", 5));
            list.Add(new WarehouseItem("51004", "Spaghetti", "kg", 7.2));
            list.Add(new WarehouseItem("19822", "Sua bo", "Lit", 50));

            return list;
        }

        class WarehouseSQL
        {
            public static ObservableCollection<WarehouseItem> GetItemsFromName(string itemName = null)
            {
                string query = itemName is null ?
                    "SELECT * from HangTonKho" :
                    $"SELECT * from HangTonKho WHERE TenMH = '{itemName}'";

                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    
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

            public static void UpdateItemQuantity(string itemName, double updatedValue)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"UPDATE HangTonKho SET KhoiLuongTon = @quantity WHERE TenMH = @name";
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@quantity", System.Data.SqlDbType.Float).Value = updatedValue;
                    cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = itemName;

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error updating data");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
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
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@unit", System.Data.SqlDbType.NVarChar).Value = unit;
                    cmd.Parameters.Add("@quantity", System.Data.SqlDbType.Float).Value = quantity;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error inserting item");
                    Debug.WriteLine($"Error:{e.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ItemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rebind();
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (load_from_sql == false)
                 wh_items.Add(new WarehouseItem("A", "A", "A", 1));
        }
    }
}
