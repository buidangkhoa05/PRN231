using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

[Table("Account")]
public partial class Account
{
    [Key]
    [Column("AccountID")]
    public int AccountId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(180)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

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

    public byte? AccountStatus { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Fullname { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [InverseProperty("Account")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
