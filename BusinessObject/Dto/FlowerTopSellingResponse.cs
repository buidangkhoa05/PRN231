using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class FlowerTopSellingResponse
    {
        public int FlowerBouquetId { get; set; }
        public string FlowerBouquetName { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
