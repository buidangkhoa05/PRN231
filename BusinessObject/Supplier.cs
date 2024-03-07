using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [StringLength(50)]
    public string? SupplierName { get; set; }

    [StringLength(150)]
    public string? SupplierAddress { get; set; }

    [StringLength(15)]
    public string? Telephone { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; } = new List<FlowerBouquet>();
}
