using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace v1web.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}