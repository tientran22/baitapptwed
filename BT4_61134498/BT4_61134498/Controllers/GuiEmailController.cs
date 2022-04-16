using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BT4_61134498.Models;

namespace BT4_61134498.Controllers
{
    public class GuiEmailController : Controller
    {
        // GET: GuiEmail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MailInfo model)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress(model.From);
            mail.To.Add(model.To);
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential(model.From, model.Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return RedirectToAction("Index", "GuiEmail");
        }
    }
}