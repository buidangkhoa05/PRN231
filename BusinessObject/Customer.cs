using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("Customer")]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(180)]
    [Unicode(false)]
    public string CustomerName { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string Country { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public byte? CustomerStatus { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
