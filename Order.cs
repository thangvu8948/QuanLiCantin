using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCantin
{
    class Order : INotifyPropertyChanged
    {
        private int soluong;
        public string ID { get; set; }
        public string Name { get; set; }
        public int SoLuong
        {
            get
            {
                return soluong;
            }
            set
            {
                soluong = value;
                OnPropertyChanged("SoLuong");
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }

        public long PriceOfOne { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
