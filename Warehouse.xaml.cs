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
    /// Interaction logic for Warehouse.xaml
    /// </summary>
    public partial class Warehouse : UserControl
    {
        private static ObservableCollection<Storage> STORAGES { get; set; } = null;
        private static ListCollectionView DisplayedItems { get; set; } = null;
        private readonly Color purple = Color.FromArgb(0xFF, 67, 0x3A, 0xB7);

        public Warehouse()
        {
            InitializeComponent();
            WH_UI.Children.Remove(StorageAddBox);
            WH_UI.Children.Remove(BlockScreen);

            STORAGES = new ObservableCollection<Storage>(StorageSQL.GetAllStorages());
            DisplayedItems = new ListCollectionView(STORAGES)
            {
                Filter = null
            };
            ItemTable.ItemsSource = DisplayedItems;        
        }


        class Storage : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _mlk, _mahh;
            private double _sldn, _slcn;
            private DateTime _nlk;

            public string MLK
            {
                get { return _mlk; }
                set { _mlk = value; }
            }
            public string MaHH 
            {
                get { return _mahh; }
                set { if (_mahh != value) { _mahh = value; NotifyPropertyChanged("MaHH");  } }
            }
            public double SLDN
            {
                get { return _sldn; }
                set { if (_sldn != value) { _sldn = value; NotifyPropertyChanged("SLDN"); } }
            }

            public double SLCN 
            {
                get { return _slcn; }
                set { if (_slcn != value) { _slcn = value; NotifyPropertyChanged("SLCN"); } }
            }

            public DateTime NLK
            {
                get { return _nlk; }
                set { if (_nlk != value) { _nlk = value; NotifyPropertyChanged("NLK"); } }
            }

            public Storage(string mlk, string mahh, double sldn, double slcn, DateTime nlk)
            {
                MLK = mlk;
                MaHH = mahh;
                SLDN = sldn;
                SLCN = slcn;
                NLK = nlk;
            }

            public Storage((string, string, double, double, DateTime) initializer)
                => (MLK, MaHH, SLDN, SLCN, NLK) = initializer;


            private void NotifyPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

///-----------------------------------------------------------------------------------

        class StorageSQL
        {
            public static ObservableCollection<Storage> GetAllStorages()
            {
                var conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Kho", conn))
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            var storageList = new ObservableCollection<Storage>();
                            while (r.Read())
                            {
                                var mlk = Convert.ToString(r.GetValue(0));
                                var mahh = Convert.ToString(r.GetValue(1));
                                var sldn = Convert.ToDouble(r.GetValue(2));
                                var slcn = Convert.ToDouble(r.GetValue(3));
                                var nlk = Convert.ToDateTime(r.GetValue(4));
                                storageList.Add(new Storage(mlk, mahh, sldn, slcn, nlk));
                            }
                            return storageList;
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


            public static bool AddStorage(string mlk, string mahh, double sldn, double slcn, DateTime nlk)
            {
                var conn = DBUtils.GetDBConnection();

                string query =
                    $"INSERT INTO Kho VALUES (@mlk, @mahh, @sldn, @slcn, @nlk)";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mlk", mlk);
                    cmd.Parameters.AddWithValue("@mahh", mahh);
                    cmd.Parameters.AddWithValue("@sldn", sldn);
                    cmd.Parameters.AddWithValue("@slcn", slcn);
                    cmd.Parameters.AddWithValue("@nlk", nlk);
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

            public static bool RemoveStorage(string id)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"DELETE FROM Kho WHERE MaNV = @id";

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

            public static bool UpdateStorage(string mlk, string mahh, double sldn, double slcn, DateTime nlk)
            {
                var conn = DBUtils.GetDBConnection();

                string query = $"UPDATE Kho " +
                    $"SET MaHH = @mahh, SoLuongDauNgay = @sldn, SoLuongCuoiNgay = @slcn, NgayLuuKho = @nlk " +
                    $"WHERE MaLuuKho = @mlk";

                int affectedRows = 0;

                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mlk", mlk);
                    cmd.Parameters.AddWithValue("@mahh", mahh);
                    cmd.Parameters.AddWithValue("@sldn", sldn);
                    cmd.Parameters.AddWithValue("@slcn", slcn);
                    cmd.Parameters.AddWithValue("@nlk", nlk);
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

            HighlightButton(AddItemButton);
            WH_UI.Children.Add(BlockScreen);
            WH_UI.Children.Add(StorageAddBox);

            if (itemAddFirstLoad is true)
            {
                StorageAddBox.Confirm.Click += Add_Confirm_Click;
                StorageAddBox.Quit.Click += Add_Quit_Click;
                itemAddFirstLoad = false;
            }
        }


        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            addClick = false; updateClick = true;

            HighlightButton(UpdateItem);
            WH_UI.Children.Add(BlockScreen);
            WH_UI.Children.Add(StorageAddBox);

            if (itemAddFirstLoad is true)
            {
                StorageAddBox.Confirm.Click += Add_Confirm_Click;
                StorageAddBox.Quit.Click += Add_Quit_Click;
                itemAddFirstLoad = false;
            }
        }

        private void Add_Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (StorageAddBox.AllValid())
            {
                var imp = new Storage(StorageAddBox.GetInputData());
                bool success;
                //Insert
                if (addClick == true)
                {
                    success = StorageSQL.AddStorage(imp.MLK, imp.MaHH, imp.SLDN, imp.SLCN, imp.NLK);
                    if (success)
                        STORAGES.Add(imp);
                }

                //Update
                else
                {
                    success = StorageSQL.UpdateStorage(imp.MLK, imp.MaHH, imp.SLDN, imp.SLCN, imp.NLK);
                    if (success)
                    {
                        for (int i = 0, sz = STORAGES.Count; i < sz; ++i)
                        {
                            if (STORAGES[i].MLK == imp.MLK)
                                STORAGES[i] = imp;
                        }
                    }
                }

                if (success)
                {
                    WH_UI.Children.Remove(StorageAddBox);
                    WH_UI.Children.Remove(BlockScreen);
                    UnhighlightButton(addClick == true ? AddItemButton : UpdateItem);
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
            WH_UI.Children.Remove(StorageAddBox);
            WH_UI.Children.Remove(BlockScreen);
            UnhighlightButton(addClick == true ? AddItemButton : UpdateItem);
        }

        private void StorageAddBox_MouseEnter(object sender, MouseEventArgs e)
        {
            StorageAddBox.Opacity = 1.0;
        }

        private void StorageAddBox_MouseLeave(object sender, MouseEventArgs e)
        {
            StorageAddBox.Opacity = 0.5;
        }


///-----------------------------------------------------------------------------------


        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            HighlightButton(DeleteItem);
            WH_UI.Children.Add(BlockScreen);
        }

        ///-----------------------------------------------------------------------------------
        public bool ItemFilter(object obj)
        {
            var item = obj as Storage;
            if (MLKFindBox.Text.Length == 0 && MHHFindBox.Text.Length > 0)
                return item.MaHH.ToLower().Contains(MHHFindBox.Text.ToLower().Trim());
            else if (MHHFindBox.Text.Length == 0 && MLKFindBox.Text.Length > 0)
                return item.MLK.ToLower().Contains(MLKFindBox.Text.ToLower().Trim());
            else return true;
        }


        private void FindBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (MLKFindBox.Text.Length > 0 || MHHFindBox.Text.Length > 0)
                DisplayedItems.Filter = new Predicate<object>(ItemFilter);
            else
                DisplayedItems.Filter = null;
        }

///-----------------------------------------------------------------------------------
        private void ItemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void StorageAddBox_Loaded(object sender, RoutedEventArgs e)
        {
            StorageAddBox.EmptyAllField();
        }


        private void WarehouseUI_Unloaded(object sender, RoutedEventArgs e)
        {
            WH_UI.Children.Remove(StorageAddBox);
        }

        ///-----------------------------------------------------------------------------------
        private void UnhighlightButton(Button button)
        {
            button.Foreground = Brushes.Black;
            button.Background = Brushes.White;
            button.BorderBrush = Brushes.Black;
        }

        private void MHHFindBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void HighlightButton(Button clickedButton)
        {
            clickedButton.Foreground = Brushes.Cyan;
            clickedButton.Background = new SolidColorBrush(purple);
            clickedButton.BorderBrush = Brushes.Cyan;
        }
    }

}