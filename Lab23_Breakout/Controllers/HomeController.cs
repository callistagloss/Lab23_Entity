using Lab23_Breakout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab23_Breakout.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();

        public ActionResult Index()
        {
            //var user = Session["LoggedInUser"];
            //List<User> p = (List<User>)user;
            //User u = p[0];

            //ViewBag.User = u;

            User u = (User)Session["LoggedInUser"];

            ViewBag.User = u;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult MakeNewUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();


            return RedirectToAction("Shop");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string Password)
        {
            List<User> Users = db.Users.ToList();

            var output = Users
            .Where(u =>
            u.firstName == userName &&
            u.password == Password);

            foreach (User u in Users)
            {
                if(u.firstName == userName && u.password == Password)
                {
                    Session["LoggedInUser"] = u;
                    Session["Cash"] = 250;
                }
            }
            //string output = "";
            Session["LoggedInUser"] = output;

            return RedirectToAction("Index");
        }
        public ActionResult Shop()
        {
            List<Item> p = db.Items.ToList();
            return View(p);
        }

        [HttpPost]
        public ActionResult Shop(int id)
        {
            List<Item> p = db.Items.ToList();
            Session["Cash"] = 250;

            foreach (Item i in p)
            {
                if(i.id == id)
                {
                    if((int)Session["Cash"] >= i.Price)
                    {
                        Session["Cash"] = (int)Session["Cash"] - i.Price;
                    
                    }
                    else
                    {
                        return RedirectToAction("TooPoor");
                    }

                }
                
            }
            //figure out which item matches id, then use logged in cash session to see if they're too poor for the item
            //if they have enough, set up a viewbag that displays how much money is left, else redirect to error page saying i'm poor
            return View(p);
        }
    }
}