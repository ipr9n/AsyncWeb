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
    public class UserProfileController : Controller
    {
        ApplicationDbContext _db;
        private ApplicationDbContext Db
        {
            get
            {
                if (_db != null) return _db;
                else
                {
                    _db = new ApplicationDbContext();
                    return _db;
                }
            }
        }

      private string UserId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {
            if(!Db.UserProfiles.Any(x=>x.Id==UserId))
            {
                Db.Users.First(x => x.Id == UserId).UserProfile = new UserProfile()
                {
                    Age = 18,
                    FirstName = "Lesha",
                    LastName = "Evdokimov"
                };

                await Db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Read()
        {
            var userProfile = await Db.UserProfiles.FindAsync(UserId);
            return RedirectToAction("Index","Home");
        }

        public async Task<ActionResult> Update()
        {
            var userProfile = await Db.UserProfiles.FindAsync(UserId);
            if (userProfile != null)
            {
                userProfile.LastName = "New";
                userProfile.FirstName = "New";
                userProfile.Age = 25;

                await Db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Delete()
        {
            var userProfile = await Db.UserProfiles.FindAsync(UserId);

            if(userProfile != null)
            Db.UserProfiles.Remove(userProfile);

            await Db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}