using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace v1web.Models
{
    public class Cart
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
       
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<CartProduct> CartProducts { get; set; }

        public Cart()
        {
            CartProducts = new List<CartProduct>();
        }

        public float TotalPrice
        {
            get
            {
                return CartProducts.Sum(x => x.Product.Price);
            }
        }

    }
}