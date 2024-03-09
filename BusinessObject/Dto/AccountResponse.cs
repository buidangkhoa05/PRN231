using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class AccountResponse
    {
        public int AccountId { get; set; }

        public string? Email { get; set; }

        public string Username { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public byte? AccountStatus { get; set; }

        public string? Fullname { get; set; }

        public string Role { get; set; } = null!;

        public ICollection<OrderResponse> Orders { get; set; } = new List<OrderResponse>();
    }
}
