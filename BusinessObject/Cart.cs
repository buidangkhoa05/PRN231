using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Cart
    {
    }

    public class CartItem
    {
        public int FlowerBouquetId { get; set; }
        public string FlowerBouquetName { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; }
    }

}
