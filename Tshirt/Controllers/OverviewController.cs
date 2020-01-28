using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tshirt.Classes;
using Tshirt.Context;
using Tshirt.Models;
using SautinSoft.Document;
using System.Drawing.Imaging;


namespace Tshirt.Controllers
{

    public class OverviewController : Controller
    {
        private tshirtContext db = new tshirtContext();
        private object postedFile;

        // GET: Overview
        private static void Main(string[] args)
        {
            using (var dbContext = new tshirtContext())
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
        public ActionResult Singleproductpage(int? imagename)
        {
            var Singlepageimagelist = db.tshirtImages.Where(d => d.id == imagename).FirstOrDefault();
            ViewBag.singleimage = Singlepageimagelist;
            return View();
        }
        [HttpPost]
        public ActionResult Singleproductpage(int? tshirtid, string companymassege, string name, string email, string width, string hight, string address, string phonenumber)
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
        public ActionResult Thankyoupage(int? imagename)
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
        public ActionResult BuyerRegiser(string fullName, string userName, string inputEmail, string password)
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
        public ActionResult CompanyRegister(string companymassege, string companyname, string email, string ownerName, string owneridnumber, string tshirt, string tshirtprint, string offsetprint,
                                            string digitalprint, string plastic, string mug, string companylocation, string companyaddress, string agree)
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

            Random random = new Random();
            int randomNumber = random.Next(0, 1000000);
            companys.otpcode = randomNumber;

            db.companyRegisters.Add(companys);
            db.SaveChanges();

            return RedirectToAction("SentMail", "Overview", new { name = companyname, otpcode = randomNumber, id = companys.companyRegisterId, emails = email });
        }

        public ActionResult SentMail(string name, int otpcode, int id, string emails)
        {
            string body = this.PopulateBody(name, otpcode);
            Email.SendMail("req1", emails, "confirm Your Email", body);
            return RedirectToAction("confirmationRegister", "Overview", new { ids = id });
        }
        private string PopulateBody(string userName, int otpcode)
        {
            string date = DateTime.Now.ToString();
            int y = 0;
            string body = string.Empty;
            string otp = otpcode.ToString();

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Overview/templateemail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{date}", date);
            body = body.Replace("{otpcode}", otp);
            return body;

        }
        public ActionResult confirmationRegister(int ids)
        {
            ViewBag.ids = ids;
            return View();
        }
        [HttpPost]
        public ActionResult confirmationRegister(string confirmation, int idvalue, string username, string password)
        {
            var register = db.companyRegisters.Where(d => d.companyRegisterId == idvalue).FirstOrDefault();
            int comfirm = Convert.ToInt32(confirmation);
            if (register.otpcode == comfirm)
            {
                Login logindetails = new Login();
                logindetails.loginName = register.ownerName;
                logindetails.userName = username;
                logindetails.loginRole = "buyer";
                string pwd = SHA.GenerateSHA256String(username + password);
                logindetails.loginPassword = pwd;
                logindetails.companyRegisterLoginId = idvalue;

                db.logins.Add(logindetails);
                db.SaveChanges();
                return RedirectToAction("SuccessfullyRegister", "Overview", new { name = username });
            }
            return View();
        }

        public ActionResult templateemail()
        {
            return View();
        }

        public ActionResult SuccessfullyRegister(string name)
        {
            ViewBag.name = name;
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
            return RedirectToAction("ComingSoon", "Overview");
            var dta = db.ColingOffs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult login(string name, string password)
        {
            var ExistingMember = db.logins.Where(x => x.userName == name).FirstOrDefault();
            if (ExistingMember != null)
            {
                string pwd = SHA.GenerateSHA256String(name + password);
                var user = db.logins.Where(d => d.userName == name && d.loginPassword == pwd || d.loginEmail == name && d.loginPassword == pwd).FirstOrDefault();
                Session["Name"] = user.loginName.ToString();
                Session["companyrole"] = user.loginRole.ToString();
                Session["id"] = user.loginId;
                if (user != null && user.loginRole == "company")
                {
                    return RedirectToAction("Compnypage", "Overview");
                }
                else if (user != null && user.loginRole == "buyer")
                {
                    return RedirectToAction("Userpage", "Overview");
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
            if (Session["Name"] != null)
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
        public ActionResult OfferSinglePage(int? id)
        {
            if (Session["Name"] != null)
            {
                var Singlepageimagelist = db.offers.Where(d => d.offerId == id).FirstOrDefault();
                ViewBag.singleimage = Singlepageimagelist;
                return View();
            }
            else
            {
                return RedirectToAction("login", "Overview");
            }
        }
        [HttpPost]
        public ActionResult OfferSinglePage(Replyoffer replyoffer)
        {
            if (Session["Name"] != null)
            {
                int id = Convert.ToInt32(Session["id"]);
                replyoffer.sessionId = id;
                db.replyoffers.Add(replyoffer);
                db.SaveChanges();

                return RedirectToAction("SendOfferCategory", "Overview");
            }
            else
            {
                return RedirectToAction("login", "Overview");
            }
        }

        public ActionResult RequestOrder()
        {
            if (Session["Name"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Overview");
            }
        }

        [HttpPost]
        public ActionResult RequestOrder(string name, string title, string designdate, string deleverydate, string deleveryaddress, string printcatogory, string printcolor, string discription, string designprice, string printprice, HttpPostedFileBase uploadoriginal, HttpPostedFileBase uploadsample)
        {
            RequestOrder order = new RequestOrder();
            string path = Server.MapPath("~/img/orderimage/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            uploadoriginal.SaveAs(path + Path.GetFileName(uploadoriginal.FileName));
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
            order.uploadoriginal = uploadoriginal.FileName;
            order.adminconfirm = 0;

            db.requestOrders.Add(order);
            db.SaveChanges();

            Offer offerpagesave = new Offer();
            offerpagesave.offerName = printcatogory;
            offerpagesave.offerEntryDate = DateTime.Now;
            int numberofdate = Convert.ToInt32(designdate);
            offerpagesave.offerNumberOfDay = numberofdate;
            if (uploadsample.FileName == "")
            {
                offerpagesave.offerImage = "thirtoffer.png";
            }
            else
            {
                offerpagesave.offerImage = uploadsample.FileName;
            }
            offerpagesave.offerConfirmWeb = "0";
            offerpagesave.offerDescriptions = discription;
            offerpagesave.offerDelete = 0;
            int dis = Convert.ToInt32(designprice);
            int pri = Convert.ToInt32(printprice);
            offerpagesave.offerAmount = dis + pri;
            offerpagesave.offerDeleveryAddress = deleveryaddress;
            offerpagesave.buyerConfirmOffer = "no";
            int id = Convert.ToInt32(Session["id"]);
            offerpagesave.offerManId = id;

            db.offers.Add(offerpagesave);
            db.SaveChanges();

            return View();
        }
        public ActionResult Userpage()
        {
            if (Session["Name"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Overview");
            }
        }
        
        public ActionResult MyOffer()
        {

            if (Session["Name"] != null)
            {
                int id = Convert.ToInt32(Session["id"]);
                var tshirtDetails = db.offers.Where(d => d.offerManId == id).ToList();
                ViewBag.tshirtdetailspasstheview = tshirtDetails;
                ViewBag.listcount = tshirtDetails.Count();
                return View();
            }
            return RedirectToAction("login", "Overview");
      
        }
        
        public ActionResult MySingleOffer(int ? id)
        {
            int ids = Convert.ToInt32(Session["id"]);
            var checktrueuser = db.offers.Where(d => d.offerId == id).FirstOrDefault();
            if (Session["Name"] != null && ids == checktrueuser.offerManId)
            {
                var Singlepageimagelist = db.offers.Where(d => d.offerId == id).FirstOrDefault();
                ViewBag.singleimage = Singlepageimagelist;

                var replySinglePage = db.replyoffers.Where(d => d.offerId == id).OrderByDescending(d=>d.replyOfferId).ToList();
                if(replySinglePage == null)
                {

                }
                else
                {
                    ViewBag.reply = replySinglePage;
                }
                
                return View();
            }
            return RedirectToAction("login", "Overview");

        }
        public ActionResult WebDesign()
        {
            var tshirtDetails = db.webs.ToList();
            ViewBag.tshirtdetailspasstheview = tshirtDetails;
            ViewBag.listcount = tshirtDetails.Count();
            return View();
        }
        public ActionResult DragandDropTshirt()
        {
            return View();
        }
        public ActionResult ComingSoon()
        {

            return View();
        }
        public ActionResult dilan902420533v()
        {
            return View(db.tshirtorders.OrderByDescending(d=>d.tshirtorderId).ToList());
        }
        public ActionResult PageLogo()
        {
            return View();
        }
        public ActionResult convertor()
        {
            DocumentCore dc = DocumentCore.Load(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\convertor.pdf",
            new PdfLoadOptions()
            {
                DetectTables = true,
                ConversionMode = PdfConversionMode.Continuous,
                PageIndex = 0,
                PageCount = 1
            });

            dc.Save(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\After.html", new HtmlFixedSaveOptions()
            {
                Version = HtmlVersion.Html5,
                CssExportMode = CssExportMode.Inline,
                EmbedImages = true
            });
            using (StreamReader reader = new StreamReader(Server.MapPath("~/img/orderimage/after.html")))
            {
                string body = reader.ReadToEnd();
            }
            //body = body.Replace("{date}", date);
            //body = body.Replace("{otpcode}", otp);
            return View();
        }
    }
}