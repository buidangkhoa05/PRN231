using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class SupplierResponse
    {
        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public string? SupplierAddress { get; set; }

        public string? Telephone { get; set; }
    }
}
