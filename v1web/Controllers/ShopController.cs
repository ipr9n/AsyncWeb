using Microsoft.AspNet.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using v1web.Models;

namespace v1web.Controllers
{
    public class ShopController : Controller
    {
        private string MyId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: Shop
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var prod = db.Products.ToList();
                return View(prod);
            }
        }

        public async Task<ActionResult> Cart()
        {
            using (var db = new ApplicationDbContext())
            {
                var prod = db.Users.First(x => x.Id == MyId).Cart.CartProducts.Select(x => x.Product).ToList();
                return View(prod);
            }
        }

        public async Task<ActionResult> AddToCart(int productId, int count = 1)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Users.First(x => x.Id == MyId).Cart.CartProducts.Add(new CartProduct()
                {
                    Product = await db.Products.FindAsync(productId)
                });
                await db.SaveChangesAsync();
            }

            var summary = GetCartForUser(MyId);
            return Json(summary);
        }

        public async Task<ActionResult> GetCartSummary()
        {
            return Json(GetCartForUser(MyId));
        }

        public async Task<ActionResult> RemoveFromCart(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Users.First(x => x.Id == MyId).Cart.CartProducts.Remove(db.Users.First(x => x.Id == MyId).Cart.CartProducts.First(p => p.ProductId == id));

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        private CartSummary GetCartForUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                return new CartSummary()
                {
                    TotalCount = db.Users.First(x => x.Id == userId).Cart.CartProducts.Count(),
                    TotalCoast = db.Users.First(x => x.Id == userId).Cart.TotalPrice
                };
            }
        }

        public async Task<ActionResult> InitializeProduct()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Products.Add(new Product() { Name = "Сыр", Price = 57 });
                db.Products.Add(new Product() { Name = "Пельмени", Price = 32 });
                db.Products.Add(new Product() { Name = "Макароны", Price = 284 });
                db.Products.Add(new Product() { Name = "Рыба", Price = 522 });

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}