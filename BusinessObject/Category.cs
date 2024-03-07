using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string CategoryName { get; set; } = null!;

    [StringLength(150)]
    public string? CategoryDescription { get; set; }

    [StringLength(150)]
    public string? CategoryNote { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; } = new List<FlowerBouquet>();
}
