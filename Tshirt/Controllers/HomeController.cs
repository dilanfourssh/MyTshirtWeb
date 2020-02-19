using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using iTextSharp.text.pdf;
using PdfiumViewer;
using iTextSharp.text.pdf.parser;


namespace Tshirt.Controllers
{
    public class HomeController : Controller
    {

        private tshirtContext db = new tshirtContext();
        private object postedFile;

        // GET: Home
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

            return RedirectToAction("Thankyoupage", "Home", new { imagename = tshirtid });

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
            return RedirectToAction("login", "Home");
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
            return RedirectToAction("SuccessfullyRegister", "Home");
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

            return RedirectToAction("SentMail", "Home", new { name = companyname, otpcode = randomNumber, id = companys.companyRegisterId, emails = email });
        }

        public ActionResult SentMail(string name, int otpcode, int id, string emails)
        {
            string body = this.PopulateBody(name, otpcode);
            Email.SendMail("req1", emails, "confirm Your Email", body);
            return RedirectToAction("confirmationRegister", "Home", new { ids = id });
        }
        private string PopulateBody(string userName, int otpcode)
        {
            string date = DateTime.Now.ToString();
            int y = 0;
            string body = string.Empty;
            string otp = otpcode.ToString();

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Home/templateemail.cshtml")))
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
                return RedirectToAction("SuccessfullyRegister", "Home", new { name = username });
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
            return RedirectToAction("ComingSoon", "Home");
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
                    return RedirectToAction("Compnypage", "Home");
                }
                else if (user != null && user.loginRole == "buyer")
                {
                    return RedirectToAction("Userpage", "Home");
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
                return RedirectToAction("Index", "Home");
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
            return RedirectToAction("login", "Home");
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
                return RedirectToAction("login", "Home");
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

                return RedirectToAction("SendOfferCategory", "Home");
            }
            else
            {
                return RedirectToAction("login", "Home");
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
                return RedirectToAction("login", "Home");
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
            uploadoriginal.SaveAs(path + System.IO.Path.GetFileName(uploadoriginal.FileName));
            uploadsample.SaveAs(path + System.IO.Path.GetFileName(uploadsample.FileName));
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
                return RedirectToAction("login", "Home");
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
            return RedirectToAction("login", "Home");

        }

        public ActionResult MySingleOffer(int? id)
        {
            int ids = Convert.ToInt32(Session["id"]);
            var checktrueuser = db.offers.Where(d => d.offerId == id).FirstOrDefault();
            if (Session["Name"] != null && ids == checktrueuser.offerManId)
            {
                var Singlepageimagelist = db.offers.Where(d => d.offerId == id).FirstOrDefault();
                ViewBag.singleimage = Singlepageimagelist;

                var replySinglePage = db.replyoffers.Where(d => d.offerId == id).OrderByDescending(d => d.replyOfferId).ToList();
                if (replySinglePage == null)
                {

                }
                else
                {
                    ViewBag.reply = replySinglePage;
                }

                return View();
            }
            return RedirectToAction("login", "Home");

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
            return View(db.tshirtorders.OrderByDescending(d => d.tshirtorderId).ToList());
        }
        public ActionResult PageLogo()
        {

            return View();

        }
        public ActionResult convertor()
        {
            DocumentCore dc = DocumentCore.Load(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\web1.pdf",
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

            string pdfPath = @"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\web1.pdf";
            string outputPath = @"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\web3.jpg";
            PdfReader reader = new PdfReader(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\rechtangel.pdf");
            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;

            for (int i = 1; i <= intPageNum; i++)
            {
                string text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                words = text.Split('\n');
                for (int j = 0, len = words.Length; j < len; j++)
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                }
            }

            return View();
        }
        public ActionResult nexthtml()
        {
            return View();
        }
        [HttpPost]
        public ActionResult nexthtml(string x)
        {
            return RedirectToAction("createhtml", "Home", new { mydata = x });
        }
        public ActionResult createhtml(string mydata)
        {
            string passviewhtml = "";
            List<bootstrap_type> mytypeform = new List<bootstrap_type>();
            //PdfReader reader = new PdfReader(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\Dilan.pdf");
            //int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;
            string k;
            string kfirst;
            var insertdata = new bootstrap_type();

            //string text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
            string text = mydata;

            words = text.Split('\n');
            for (int j = 0, len = words.Length; j < len; j++)
            {
                if (words[j] != " ")
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                    line = line.Trim();
                    //int start = (line.Length - 8);

                    //string whattype = line.Substring(start, 7);
                    //string whatname = line.Substring(0, (start - 1));
                    var insertdata1 = new bootstrap_type();

                    var a = line.IndexOf('(');
                    var b = line.IndexOf(')');
                    if (a == -1)
                    {

                    }
                    else
                    {
                        string whattype = line.Substring((a + 1), (b - a - 1));
                        string whatname = line.Substring(0, a);

                        insertdata1.type = whattype;
                        insertdata1.name = whatname;

                        insertdata = insertdata1;


                        mytypeform.Add(insertdata);
                    }
                }
            }
            webform dt = new webform();
            using (StreamReader readering = new StreamReader(Server.MapPath("~/Views/Home/PageLogo.cshtml")))
            {
                k = readering.ReadToEnd();
            }
            string s = dt.layout(mytypeform, k);
            using (StreamReader readering1 = new StreamReader(Server.MapPath("~/Views/Home/createhtml.cshtml")))
            {
                kfirst = readering1.ReadToEnd();
            }
            kfirst = kfirst.Replace("{codebody}", s);
            var titlehave = mytypeform.Where(d => d.type == "tittle").FirstOrDefault();
            if (titlehave != null)
            {
                kfirst = kfirst.Replace("{tittle}", titlehave.name);
                passviewhtml = kfirst;
            }
            string path = Server.MapPath("~/img/orderimage/rectan.html");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(kfirst);
                }
            }


            //using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Home/templateemail.cshtml")))
            //{
            //    body = reader.ReadToEnd();
            //}
            //body = body.Replace("{date}", date);
            //body = body.Replace("{otpcode}", otp);
            //return body;
            return RedirectToAction("Yourformview", "Home");

        }
        public ActionResult Yourformview()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Yourformview(int x)
        {
            string fileName = "rectan.html";// Replace Your Filename with your required filename

            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            Response.TransmitFile(Server.MapPath("~/img/orderimage/" + fileName));//Place "YourFolder" your server folder Here

            Response.End();
            return RedirectToAction("nexthtml", "Home");
        }
        [HttpPost]
        public ActionResult createhtml(string name, int a)
        {
            string body;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Home/createhtml.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            using (FileStream fs = new FileStream(@"C:\Users\Owner\source\repos\newupdatetshirtfebruary\MyTshirtWeb\Tshirt\img\orderimage\rechtangel.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(body);
                }
            }
            return View();
        }
        public ActionResult MathsConvertor()
        {
            string y = "10*x+45-(50*y+455)=550x-(45y + 444)";
            char[] charSeparators = y.ToCharArray();
            foreach (var a in charSeparators)
            {
                var x = a.ToString();
                if (x == "^")
                {

                }
                else
                {
                    int value = 0;
                    foreach (var b in charSeparators)
                    {
                        var strings = b.ToString();

                        if (strings == "a" || strings == "b" || strings == "c" || strings == "d" || strings == "e" || strings == "f" || strings == "g" || strings == "h" || strings == "i" || strings == "j" || strings == "k" || strings == "l" || strings == "m" || strings == "n" || strings == "o" || strings == "p" || strings == "q" || strings == "r" || strings == "s" || strings == "t" || strings == "u" || strings == "v" || strings == "w" || strings == "x" || strings == "y" || strings == "z" ||
                            strings == "A" || strings == "B" || strings == "C" || strings == "D" || strings == "E" || strings == "F" || strings == "G" || strings == "H" || strings == "I" || strings == "J" || strings == "K" || strings == "L" || strings == "M" || strings == "N" || strings == "O" || strings == "P" || strings == "Q" || strings == "R" || strings == "S" || strings == "T" || strings == "U" || strings == "V" || strings == "W" || strings == "X" || strings == "Y" || strings == "Z")
                        {
                            value++;
                        }
                    }
                    if (value > 1)
                    {
                        //not a result 
                    }
                    else
                    {

                    }

                }
            }
            return View();
        }
    }
    }
