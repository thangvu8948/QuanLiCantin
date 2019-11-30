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
        public void LoadItem()
        {
            var item1 = new WarehouseItem("09125", "Gao", "kg", 25);
            var item2 = new WarehouseItem("63821", "Thit bo", "kg", 5);
            var item3 = new WarehouseItem("51004", "Spaghetti", "kg", 7.2);
            var item4 = new WarehouseItem("19822", "Sua bo", "Lit", 50);
            var item5 = new WarehouseItem("09125", "Gao", "kg", 25);
            var item6 = new WarehouseItem("63821", "Thit bo", "kg", 5);
            var item7 = new WarehouseItem("51004", "Spaghetti", "kg", 7.2);
            var item8 = new WarehouseItem("19822", "Sua bo", "Lit", 50);

            var items = new List<WarehouseItem>{
                item1, item2, item3, item4,
                item5, item6, item7, item8,
                item1, item2, item3, item4,
                item5, item6, item7, item8,
                item1, item2, item3, item4,
                item5, item6, item7, item8
            };

            foreach (var item in items)
                ItemList.Items.Add(item);

        }


        public Warehouse()
        {
            InitializeComponent();
        }

        class WarehouseItem
        {
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string ItemQuantityUnit { get; set; }
            public double ItemQuantity { get; set; }

            public WarehouseItem(string id, string name, string unit, double quantity)
            {
                ItemID = id;
                ItemName = name;
                ItemQuantityUnit = unit;
                ItemQuantity = quantity;
            }
        }

        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
            LoadItem();
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /*
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
}

BindingList<WarehouseItem> item_list = null;

private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
{
   var conn = DBUtils.GetDBConnection();
   conn.Open();
   item_list = WarehouseQuery.GetAll(conn);
   conn.Close();
}
*/
    }
}
