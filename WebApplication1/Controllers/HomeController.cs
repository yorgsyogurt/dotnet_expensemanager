using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost] 
        [ActionName("signin")]
        public ActionResult signin(string txt_uname, string txt_pwd)
        {
            List<Password> passwords = dbPasswords();
            List<User> users = db.Users.ToList();
            User curUser = null;
            Password curPass = null;
           
            foreach (User u in users) {
                if (u.username == txt_uname || u.email == txt_uname)
                {
                    curUser = u;
                }
            }

            foreach (Password p in passwords)
            {
                if (curUser != null)
                {
                    if (p.userId == curUser.Id)
                    {
                        curPass = p;
                    }
                }
            }

            if (curUser != null && curPass.text == txt_pwd.GetHashCode())
            {
                return RedirectToAction("Index");
            }
            if (curUser == null)
                return RedirectToAction("Login", new { error = "username"});
            return RedirectToAction("Login", new { error = "pass" });
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

        public ActionResult AutocompleteSearch(string term)
        {
            var models = db.Operations.ToList().Where(a => a.name.Contains(term))
                            .Select(a => new { value = a.name })
                            .Distinct();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("login")]
        public ActionResult login(string txt_uname, string txt_pwd)
        {

            List<User> users = db.Users.ToList();
            User curUser = null;
            foreach (User u in users)
            {
                if (u.username == txt_uname || u.email == txt_uname)
                {
                    curUser = u;
                }
            }

            if (curUser != null && txt_pwd == "qwerty")
            {

                return RedirectToAction("Index");
            }
            if (curUser == null)
                return RedirectToAction("Login", new { error = "username" });
            return RedirectToAction("Login", new { error = "pass" });
        }

        public List<Password> dbPasswords()
        {
            List<Password> ans = new List<Password>();
            for(int i = 0; i < 12; i++)
            {
                ans.Add(new Password());
            }
            return ans;
        }

    }
}