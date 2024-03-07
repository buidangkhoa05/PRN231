using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[PrimaryKey("OrderId", "FlowerBouquetId")]
[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Key]
    [Column("FlowerBouquetID")]
    public int FlowerBouquetId { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }

    [ForeignKey("FlowerBouquetId")]
    [InverseProperty("OrderDetails")]
    public virtual FlowerBouquet FlowerBouquet { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;
}
