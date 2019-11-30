using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data.Common;
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

        public Warehouse()
        {
            InitializeComponent();
        }

        class WarehouseItem
        {
            public string Image { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string QuantityUnit { get; set; }
            public double Quantity { get; set; }

            public WarehouseItem(string img, string id, string name, string unit, double quantity)
            {
                Image = img;
                ID = id;
                Name = name;
                QuantityUnit = unit;
                Quantity = quantity;
            }
        }

        class WarehouseQuery
        {
            public static BindingList<WarehouseItem> GetAll(SqlConnection conn)
            {
                using (var cmd = new SqlCommand("Select * from HANGTONKHO", conn))

                using (DbDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        var item_list = new BindingList<WarehouseItem>();
                        while (r.Read())
                        {
                            var image = "Image/acer_swift_3.jpg";
                            var id = Convert.ToString(r.GetValue(1)).Trim();
                            var name = Convert.ToString(r.GetValue(2));
                            var unit = Convert.ToString(r.GetValue(3));
                            var quantity = Convert.ToDouble(r.GetValue(4));

                            item_list.Add(new WarehouseItem(image, id, name, unit, quantity));
                        }
                        return item_list;
                    }
                    return null;
                }
            }
        }

        BindingList<WarehouseItem> item_list = null;

        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
            var conn = DBUtils.GetDBConnection();
            conn.Open();
            item_list = WarehouseQuery.GetAll(conn);
            conn.Close();
        }
    }
}
