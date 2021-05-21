using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vonage;
using Vonage.Request;
using Send_SMS_Task.Models;
using Send_SMS_Task.DAL;
using System.Data.SqlClient;

namespace Send_SMS_Task.Controllers
{
    public class SMSController : Controller
    {
        Innovs_IT_TaskEntities db;
        public SMSController()
        {
            db = new Innovs_IT_TaskEntities();
        }
        SMSOP sms = new SMSOP();
        public ActionResult SendSMS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendSMS(string content)
        {

            Message message = new Message();
            message.smsContent = content;
            db.Messages.Add(message);
            db.SaveChanges();

            var credentials = Credentials.FromApiKeyAndSecret(
                "07f67afd",
                "BmBye4nt60cUIG58"
                );
            sms.Send(message);
            ViewBag.status = "Sended Successfully";
            return View();
        }

        public ActionResult Allmessages()
        {
            return View(db.Messages.ToList());
        }
        public ActionResult smsUsers(int id)
        {
            var users = (from user in db.Users
                         join um in db.UserMessages on user.id equals um.userId
                         where um.messageId==id
                         select user).ToList();
            return View(users);
        }

    }
}