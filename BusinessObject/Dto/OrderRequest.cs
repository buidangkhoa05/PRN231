using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class OrderRequest
    {
        public  IEnumerable<OrderDetailRequest> OrderDetails { get; set; } = new List<OrderDetailRequest>();
    }

    public class OrderReqValidator : AbstractValidator<OrderRequest>
    {
        public OrderReqValidator()
        {
            RuleForEach(x => x.OrderDetails).ChildRules(x => new OrderDetailReqValidator());
        }
    }

    public class OrderDetailRequest
    {
        public int FlowerBouquetId { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; } = 0;

    }

    public class OrderDetailReqValidator : AbstractValidator<OrderDetailRequest>
    {
        public OrderDetailReqValidator()
        {
            RuleFor(x => x.FlowerBouquetId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.Discount).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
