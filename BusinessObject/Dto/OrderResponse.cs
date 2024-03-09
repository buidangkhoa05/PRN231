using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public int? AccountId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public decimal? Total { get; set; }

        public string? OrderStatus { get; set; }

        public ICollection<OrderDetailResponse> OrderDetails { get; set; } = new List<OrderDetailResponse>();
    }


}
