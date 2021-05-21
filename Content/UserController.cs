using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Send_SMS_Task.Models;

namespace Send_SMS_Task.Content
{
    
    public class UserController : Controller
    {
        Innovs_IT_TaskEntities db = new Innovs_IT_TaskEntities();

        public ActionResult Login()
        {
            if (Request.Cookies["Innove_IT"] != null)
            {
                Session["userid"] = Request.Cookies["Innove_IT"].Values["username"];
                return RedirectToAction("SendSMS", "SMS");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(User _user, string _remeberme)
        {
            User user = db.Users.Where(n => n.userName == _user.userName && n.password == _user.password).FirstOrDefault();
            if (user != null)
            {
                Session.Add("userid", user.userName);
                if (_remeberme == "true")
                {
                    HttpCookie co = new HttpCookie("Innove_IT");
                    co.Values.Add("id", user.userName);
                    co.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(co);
                }
                return RedirectToAction("SendSMS", "SMS");
            }
            else
            {
                ViewBag.status = "invalid email or password";
                return View();
            }
        }

    }
}