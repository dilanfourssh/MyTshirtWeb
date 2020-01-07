using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        private tshirtContext db = new tshirtContext();
        private object postedFile;

        // GET: Overview
        private static void Main(string [] args)
        {
            using (var dbContext=new tshirtContext())
            {
                
                dbContext.Database.Initialize(true);
            }
        }
        public ActionResult Index()
        {
            var tshirtDetails = db.bestProducts.ToList();
            ViewBag.tshirtdetailspasstheview = tshirtDetails;
            ViewBag.listcount = tshirtDetails.Count();
            //return View();
            return View();
        }
        public ActionResult Progamming(string pagename)
        {
            
                var tshirtDetails = db.tshirtImages.Where(d => d.category == pagename).ToList();
                ViewBag.tshirtdetailspasstheview = tshirtDetails;
                ViewBag.listcount = tshirtDetails.Count();
                return View();
            
        }
        public ActionResult Singleproductpage(int ? imagename)
        {
            var Singlepageimagelist = db.tshirtImages.Where(d => d.id == imagename).FirstOrDefault();
            ViewBag.singleimage = Singlepageimagelist;
            return View();
        }
        [HttpPost]
       public ActionResult Singleproductpage(int ? tshirtid, string companymassege,string name,string email,string  width, string  hight,string address,string phonenumber)
        {
           

            Tshirtorder tshirtobject = new Tshirtorder();
            tshirtobject.orderDescription = companymassege;
            tshirtobject.customerName = name;
            tshirtobject.email = email;
            tshirtobject.width = width;
            tshirtobject.hight = hight;
            tshirtobject.address = address;
            tshirtobject.phoneNumber = phonenumber;
            tshirtobject.date = DateTime.Now;

            db.tshirtorders.Add(tshirtobject);
            db.SaveChanges();
            
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
            if (Session["Name"] != null)
            {
                return View();
            }
            return RedirectToAction("login", "Overview");
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
            Login logins = new Login();
            logins.loginName = fullName;
            logins.loginRole = "buyer";
            logins.loginEmail = inputEmail;
            logins.userName = userName;
            string pwd = SHA.GenerateSHA256String(userName + password);
            logins.loginPassword = pwd;
            db.logins.Add(logins);
            db.SaveChanges();
            return RedirectToAction("SuccessfullyRegister", "Overview");
        }


        public ActionResult CompanyRegister()
        {
          
                return View();
            
        }
        [HttpPost]
       public ActionResult CompanyRegister(string companymassege,string companyname,string email,string ownerName,string owneridnumber,string tshirt,string tshirtprint,string offsetprint,
                                            string digitalprint,string plastic,string mug,string companylocation,string companyaddress,string agree)
        {
            CompanyRegister companys = new CompanyRegister();
            companys.companyMassege = companymassege;
            companys.companyName = companyname;
            companys.email = email;
            companys.ownerName = ownerName;
            companys.ownerIdnumber = owneridnumber;
            companys.tshirt = tshirt;
            companys.tshirtprint = tshirtprint;
            companys.offsetprint = offsetprint;
            companys.digitalprint = digitalprint;
            companys.plastic = plastic;
            companys.mug = mug;
            companys.companylocation = companylocation;
            companys.companyaddress = companyaddress;
            companys.agree = agree;

            db.companyRegisters.Add(companys);
            db.SaveChanges();

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
            var dta = db.ColingOffs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult login(string name,string password)
        {
            var ExistingMember = db.logins.Where(x => x.userName == name).FirstOrDefault();
            if (ExistingMember != null)
            {
                string pwd = SHA.GenerateSHA256String(name + password);
                var user = db.logins.Where(d => d.userName == name && d.loginPassword == pwd || d.loginEmail == name && d.loginPassword == pwd).FirstOrDefault();
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
        public ActionResult SendOfferCategory()
        {
            if(Session["Name"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Overview");
            }
            
        }
        public ActionResult OfferPage(string offer)
        {
            if (Session["Name"] != null)
            {
                var tshirtDetails = db.offers.Where(d => d.offerName == offer && d.offerConfirmWeb == "1" && d.buyerConfirmOffer == "no").ToList();
                ViewBag.tshirtdetailspasstheview = tshirtDetails;
                ViewBag.listcount = tshirtDetails.Count();
                return View();
            }
            return RedirectToAction("login", "Overview");
        }
        public ActionResult OfferSinglePage(int ? id)
        {
            if (Session["Name"] != null)
            {
                var Singlepageimagelist = db.offers.Where(d => d.offerId == id).FirstOrDefault();
                ViewBag.singleimage = Singlepageimagelist;
                return View();
            }
            return RedirectToAction("login", "Overview");
        }
        public ActionResult RequestOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RequestOrder(string name,string title,string designdate,string deleverydate, string deleveryaddress,string printcatogory,string printcolor,string discription,string designprice,string printprice, HttpPostedFileBase uploadsample)
        {
            RequestOrder order = new RequestOrder();
            string path = Server.MapPath("~/img/orderimage/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            uploadsample.SaveAs(path + Path.GetFileName(uploadsample.FileName));
            ViewBag.Message = "File uploaded successfully.";
            order.name = name;
            order.title = title;
            order.designdate = designdate;
            order.deleverydate = deleverydate;
            order.deleveryaddress = deleveryaddress;
            order.printcatogory = printcatogory;
            order.printcolor = printcolor;
            order.discription = discription;
            order.designprice = designprice;
            order.printprice = printprice;
            order.uploadsample = uploadsample.FileName;
            order.adminconfirm = 0;

            db.requestOrders.Add(order);
            db.SaveChanges();


            return View();
        }
    }
}