using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCantin
{
    class Product
    {
        public Product(string id, string name, int type, long price, int remain)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._price = price;
            this._remain = remain;
        }
        public string _id { get; set; }
        public string _name { get; set; }
        public int _type { get; set; }
        public long _price { get; set; }
        public int _remain { get; set; }


    }
}
