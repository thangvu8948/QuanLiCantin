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
        private static ObservableCollection<Import> IMPORTS { get; set; } = null;
        private static CollectionView DisplayedItems { get; set; } = null;
        private readonly bool use_sql = false;

        private readonly Color purple = Color.FromArgb(0xFF, 67, 0x3A, 0xB7);

        public Warehouse()
        {
            InitializeComponent();
            WH_UI.Children.Remove(ImportAddBox);
            IMPORTS = new ObservableCollection<Import>(LoadRandomItem());
            DisplayedItems = new ListCollectionView(IMPORTS)
            {
                Filter = null
            };
            ItemTable.ItemsSource = DisplayedItems;        
        }


        class Import
        {
            public string MLK { get; set; }
            public string MaHH { get; set; }
            public double SLDN { get; set; }
            public double SLCN { get; set; }
            public DateTime NLK { get; set; }

            public Import(string mlk, string mahh, double sldn, double slcn, DateTime nlk)
            {
                MLK = mlk;
                MaHH = mahh;
                SLDN = sldn;
                SLCN = slcn;
                NLK = nlk;
            }

            public Import((string, string, double, double, DateTime) initializer)
                => (MLK, MaHH, SLDN, SLCN, NLK) = initializer;
        }

        private ObservableCollection<Import> LoadRandomItem()
        {
            var list = new ObservableCollection<Import>
            {
                new Import("LK0001","HH001",100,91,new DateTime(2019,11,15)),
                new Import("LK0002","HH002",10,8,new DateTime(2019,11,15)),
                new Import("LK0003","HH003",5,4,new DateTime(2019,11,15)),
                new Import("LK0004","HH004",15,13.3,new DateTime(2019,11,15)),
                new Import("LK0005","HH005",20,10.5,new DateTime(2019,11,15)),
                new Import("LK0006","HH006",15,12.5,new DateTime(2019,11,15)),
                new Import("LK0007","HH007",10,9,new DateTime(2019,11,15)),
                new Import("LK0008","HH001",121,80,new DateTime(2019,11,16))
        };

            return list;
        }

        private void ItemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void WarehouseUI_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void WarehouseUI_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                WH_UI.Children.Remove(addBox);
        }

        protected bool itemAddFirstLoad = true;
        protected bool addClick, updateClick = false;

        private void ImportAddBox_Loaded(object sender, RoutedEventArgs e)
        {
            ImportAddBox.EmptyAllField();
        }


        private void ImportAddBox_Unloaded(object sender, RoutedEventArgs e)
        {

        }



///-----------------------------------------------------------------------------------

        private void ShowAllItemsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayedItems.Filter = null;
        }



        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateItem.Foreground = Brushes.Black;
            UpdateItem.Background = Brushes.White;
            AddItemButton.Foreground = Brushes.White;
            AddItemButton.Background = new SolidColorBrush(purple);
            DeleteItem.Foreground = Brushes.Black;
            DeleteItem.Background = Brushes.White;

            addClick = true; updateClick = false;

            foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                WH_UI.Children.Remove(addBox);


            WH_UI.Children.Add(ImportAddBox);
            if (itemAddFirstLoad is true)
            {
                ImportAddBox.Confirm.Click += Confirm_Click;
                ImportAddBox.Quit.Click += Quit_Click;
                itemAddFirstLoad = false;
            }
        }


        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateItem.Foreground = Brushes.Black;
            UpdateItem.Background = Brushes.White;
            AddItemButton.Foreground = Brushes.Black;
            AddItemButton.Background = Brushes.White;
            DeleteItem.Foreground = Brushes.White;
            DeleteItem.Background = new SolidColorBrush(purple);

        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateItem.Foreground = Brushes.White;
            UpdateItem.Background = new SolidColorBrush(purple);
            AddItemButton.Foreground = Brushes.Black;
            AddItemButton.Background = Brushes.White;
            DeleteItem.Foreground = Brushes.Black;
            DeleteItem.Background = Brushes.White;

            addClick = false; updateClick = true;

            foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                WH_UI.Children.Remove(addBox);

            WH_UI.Children.Add(ImportAddBox);

            if (itemAddFirstLoad is true)
            {
                ImportAddBox.Confirm.Click += Confirm_Click;
                ImportAddBox.Quit.Click += Quit_Click;
                itemAddFirstLoad = false;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ImportAddBox.AllValid())
            {
                var imp = new Import(ImportAddBox.GetInputData());
                MessageBox.Show($"{imp.MLK}|{imp.MaHH}|{imp.SLDN}|{imp.SLCN}|{imp.NLK.ToString()}");
                //Insert
                if (addClick == true)
                {
                    var existing_item = IMPORTS.FirstOrDefault(i => i.MLK == imp.MLK);
                    if (existing_item == null)
                    {
                        IMPORTS.Add(imp);
                        foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                            WH_UI.Children.Remove(addBox);
                    }
                    else
                        MessageBox.Show("Mã lưu kho đã tồn tại, xin nhập lại");
                }

                //Update
                else
                {
                    bool found = false;
                    for (int i = 0; i < IMPORTS.Count; ++i)
                    {
                        if (IMPORTS[i].MLK == imp.MLK)
                        {
                            IMPORTS[i] = imp;
                            found = true;
                        }
                    }
                    DisplayedItems = new ListCollectionView(IMPORTS)
                    {
                        Filter = null
                    };
                    if (found)
                    {
                        foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                            WH_UI.Children.Remove(addBox);
                    }
                    else
                        MessageBox.Show("Mã lưu kho không tồn tại");
                }
            }
            else
                return;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            foreach (var addBox in WH_UI.Children.OfType<WarehouseAddUI>().ToList())
                WH_UI.Children.Remove(addBox);
        }



 ///-----------------------------------------------------------------------------------
        private void ImportAddBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ImportAddBox.Opacity = 1.0;
        }

        private void ImportAddBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ImportAddBox.Opacity = 0.6;
        }


///-----------------------------------------------------------------------------------
        public bool Contains(object obj)
        {
            var item = obj as Import;
            return (item.MLK.ToLower().Contains(FindBox.Text.ToLower().Trim()));
        }

        private void FindBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (FindBox.Text != string.Empty)
                DisplayedItems.Filter = new Predicate<object>(Contains);
        }
    }
}