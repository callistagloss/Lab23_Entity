using Lab23_Breakout.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User u)
        {
            User user = db.Users.Where(user1 => user1.email == u.email && user1.password == u.password).ToList().First();

            Session["User"] = user;
            return RedirectToAction("Shop");

            //List<User> Users = db.Users.ToList();

            //var output = Users
            //.Where(u =>
            //u.firstName == userName &&
            //u.password == Password);

            //foreach (User u in Users)
            //{
            //    if (u.firstName == userName && u.password == Password)
            //    {
            //        Session["LoggedInUser"] = u;
            //        Session["Cash"] = 10000;
            //    }
            //}
            ////string output = "";
            //Session["LoggedInUser"] = output;

            //return RedirectToAction("Index");
        }
        public ActionResult Shop()
        {
            List<Item> itemList = db.Items.ToList();
            return View(itemList);
        }
        //public ActionResult Buy(int id)
        //{
        //    Item purchase = db.Items.Find(id);
        //    if (Session["LoggedInUser"] == null)
        //    {
        //        User buyer = (User)Session["LoggedInUser"];

        //        if (buyer.money > purchase.Price && purchase.Quantity > 0)
        //        {
        //            buyer.money = purchase.Price;
        //            purchase.Quantity--;
        //            db.Users.AddOrUpdate(buyer);
        //            db.Items.AddOrUpdate(purchase);

        //            db.SaveChanges();

        //            return RedirectToAction("Shop");
        //        }
        //        else
        //        {
        //            return RedirectToAction("TooPoor");
        //        }
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult BuyItem(Item i)
        {
            decimal cost = decimal.Parse(i.Price.ToString());
            User user = (User)Session["User"];
            if (user.money >= cost)
            {
                User user1 = db.Users.SingleOrDefault(u => u.id == user.id);
                Item item = db.Items.SingleOrDefault(x => x.id == i.id);
                decimal difference = decimal.Parse(user1.money.ToString()) - cost;
                user1.money = difference;
                item.Quantity -= 1;
                UserItem ui = new UserItem() { ItemID = item.id, UserID = user1.id };
                db.UserItems.Add(ui);
                db.Users.AddOrUpdate(user1);
                db.Items.AddOrUpdate(item);
                //db.SaveChanges();
                Session["User"] = user1;
            }
            else
            {
                Session["UserFunds"] = user.money;
                Session["ItemCost"] = i.Price;

                return RedirectToAction("TooPoor");

            }
            return RedirectToAction("Shop");
        }

        //    public ActionResult Shop()
        //{
        //    List<Item> p = db.Items.ToList();
        //    return View(p);
        //}


        //[HttpPost]
        //public ActionResult Shop(int id)
        //{
        //    List<Item> p = db.Items.ToList();
        //    Session["Cash"] = 250;

        //    foreach (Item i in p)
        //    {
        //        if (i.id == id)
        //        {
        //            if ((int)Session["Cash"] >= i.Price)
        //            {
        //                Session["Cash"] = (int)Session["Cash"] - i.Price;

        //            }
        //            else
        //            {
        //                return RedirectToAction("TooPoor");
        //            }

        //        }

        //    }
        //    return View(p);


        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult TooPoor()
        {
            return View();
        }
    }
}
    
