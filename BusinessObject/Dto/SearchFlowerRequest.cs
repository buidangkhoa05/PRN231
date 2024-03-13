using BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class SearchFlowerRequest : SearchBaseReq
    {
        public int? CategoryId { get; set; } = null;
    }
}
