using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("FlowerBouquet")]
public partial class FlowerBouquet
{
    [Key]
    [Column("FlowerBouquetID")]
    public int FlowerBouquetId { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string FlowerBouquetName { get; set; } = null!;

    [StringLength(220)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public byte? FlowerBouquetStatus { get; set; }

    [Column("SupplierID")]
    public int? SupplierId { get; set; }

    [StringLength(250)]
    public string? Morphology { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("FlowerBouquets")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("FlowerBouquet")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("FlowerBouquets")]
    public virtual Supplier? Supplier { get; set; }
}
