using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("AccountID")]
    public int? AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShippedDate { get; set; }

    [Column(TypeName = "money")]
    public decimal? Total { get; set; }

    [StringLength(10)]
    public string? OrderStatus { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Orders")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
