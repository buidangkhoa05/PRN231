using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class OrderDetailResponse
    {
        public int OrderId { get; set; }

        public int FlowerBouquetId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public FlowerBouquetResponse FlowerBouquet { get; set; } = null!;
    }
}
