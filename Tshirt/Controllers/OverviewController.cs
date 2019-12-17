using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Tshirt.Classes;
using Tshirt.Context;
using Tshirt.Models;

namespace Tshirt.Controllers
{
    
    public class OverviewController : Controller
    {
        private ceylonprintEntities3 ceylonprintmodelobject = new ceylonprintEntities3();
        // GET: Overview
        private static void Main(string [] args)
        {
            using (var dbContext=new tshirtContext())
            {
                int a = 0;
                dbContext.Database.Initialize(true);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Progamming(string pagename)
        {
            
            var tshirtDetails = ceylonprintmodelobject.TshirtImages.Where(d => d.category == pagename).ToList();
            ViewBag.tshirtdetailspasstheview = tshirtDetails;
            ViewBag.listcount = tshirtDetails.Count();
            return View();
        }
        public ActionResult Singleproductpage(int ? imagename)
        {
            var Singlepageimagelist = ceylonprintmodelobject.TshirtImages.Where(d => d.id == imagename).FirstOrDefault();
            ViewBag.singleimage = Singlepageimagelist;
            return View();
        }
        [HttpPost]
       public ActionResult Singleproductpage(int ? tshirtid, string companymassege,string name,string email,int ? width, int ? hight,string address,string phonenumber)
        {
            return RedirectToAction("Thankyoupage", "Overview", new { imagename = tshirtid });
            
        }
        public ActionResult Thankyoupage(int ? imagename)
        {
            ViewBag.imagenamedetails = imagename;
            return View();
        }
        public ActionResult Customizeorder()
        {
            return View();
        }
        public ActionResult Compnypage()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult BuyerRegiser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BuyerRegiser(string fullName,string userName,string inputEmail,string password)
        {
            return View();
        }


        public ActionResult CompanyRegister()
        {
            return View();
        }
        [HttpPost]
       public ActionResult CompanyRegister(string companymassege,string companyname,string email,string ownerName,string owneridnumber,string tshirt,string tshirtprint,string offsetprint,
                                            string digitalprint,string plastic,string mug,string companylocation,string companyaddress,string agree)
        {
            return RedirectToAction("SuccessfullyRegister", "Overview");
        }
        public ActionResult SuccessfullyRegister()
        {
            return View();
        }
        public ActionResult EmailVerifications(string htmlString)
        {
           
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("dilanpiyananda90829@gmail.com");
                    message.To.Add(new MailAddress("dilan@fourssh.net"));
                    message.Subject = "Test";
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = htmlString;
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("dilanpiyananda90829@gmail.com", "902420533vV");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                catch (Exception) { }
            
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(string name,string password)
        {
            var ExistingMember = ceylonprintmodelobject.logins.Where(x => x.loginName == name).FirstOrDefault();
            if (ExistingMember != null)
            {
                string pwd = SHA.GenerateSHA256String(name + password);
                var user = ceylonprintmodelobject.logins.Where(d => d.loginName == name && d.loginPassword == pwd || d.loginRole == name && d.loginPassword == pwd).FirstOrDefault();
                Session["Name"] = user.loginName.ToString();
                Session["companyrole"] = user.loginRole.ToString();
                if (user != null && user.loginRole == "company")
                {
                    return RedirectToAction("Compnypage", "Overview");
                }
                else if (user != null && user.loginRole == "buyer")
                {
                    return RedirectToAction("Compnypage", "Overview");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}