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
using System.Data.Entity;

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
            var tshirtDetails = db.addprojects.ToList();
            ViewBag.tshirtdetailspasstheview = tshirtDetails;
            ViewBag.listcount = tshirtDetails.Count();
            //return View();
            return View();
        }
        public ActionResult Template()
        {
            
                ViewBag.template = db.addprojects.Where(d => d.category == "web").ToList();
                int countingposition= db.addprojects.Where(d => d.category == "web").Count();
                ViewBag.positions = (countingposition / 3) + 1;
                ViewBag.countingposition = countingposition;

            
            return View();
        }
        public ActionResult Android()
        {

            ViewBag.template = db.addprojects.Where(d => d.category == "android").ToList();
            int countingposition = db.addprojects.Where(d => d.category == "android").Count();
            ViewBag.positions = (countingposition / 3) + 1;
            ViewBag.countingposition = countingposition;


            return View();
        }
        [HttpPost]
        public ActionResult Android(addproject adds)
        {
            return RedirectToAction("TemplateDownload", "Home", new { tempplateid = adds.id });

        }
        public ActionResult Bootstrap()
        {

            ViewBag.template = db.addprojects.Where(d => d.category == "web").ToList();
            int countingposition = db.addprojects.Where(d => d.category == "web").Count();
            ViewBag.positions = (countingposition / 3) + 1;
            ViewBag.countingposition = countingposition;


            return View();
        }
        [HttpPost]
        public ActionResult Bootstrap(addproject adds)
        {
            return RedirectToAction("TemplateDownload", "Home",new {tempplateid = adds.id});
            
        }
        public ActionResult Flutter()
        {

            ViewBag.template = db.addprojects.Where(d => d.category == "flutter").ToList();
            int countingposition = db.addprojects.Where(d => d.category == "flutter").Count();
            ViewBag.positions = (countingposition / 3) + 1;
            ViewBag.countingposition = countingposition;


            return View();
        }

        [HttpPost]
        public ActionResult Flutter(addproject adds)
        {
            return RedirectToAction("TemplateDownload", "Home", new { tempplateid = adds.id });

        }
        public ActionResult TemplateDownload(int tempplateid)
        {
            ViewBag.templatedownload = db.addprojects.Where(d => d.id == tempplateid).FirstOrDefault();  
            return View();
        }
        [HttpPost]
        public ActionResult TemplateDownload(addproject adds)
        {
            Session["templateidsend"] = adds.id;
            int downing = Convert.ToInt32(Session["templateidsend"]);

            if (Session["email"] != null)
            {
                string fileName = adds.zipfilename;// Replace Your Filename with your required filename
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(Server.MapPath("~/img/orderimage/sourcefile/" + fileName));//Place "YourFolder" your server folder Here
                

                Downloahistory download = new Downloahistory();
                download.addProjectId = downing;
                download.downloademail = Session["email"].ToString();
                download.downloadTime = DateTime.Now;
                db.downloahistories.Add(download);
                db.SaveChanges();

                var downloadupdate = db.addprojects.Where(d => d.id == downing).FirstOrDefault();
                downloadupdate.download = downloadupdate.download + 1;
                db.addprojects.Attach(downloadupdate);
                db.Entry(downloadupdate).State = EntityState.Modified;
                db.SaveChanges();
                Response.End();
                return RedirectToAction("ThankYou", "Home");
                
            }
            else
            {
               
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult ThankYou()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Register logins)
        {
            var ExistingMember = db.registers.Where(x => x.email == logins.email).FirstOrDefault();
            if (ExistingMember != null)
            {
                string pwd = SHA.GenerateSHA256String(logins.email + logins.password);
                var user = db.registers.Where(d => d.email == logins.email && d.password == pwd).FirstOrDefault();
                if (user != null)
                {
                    Session["email"] = user.email.ToString();
                    Session["id"] = user.Id.ToString();
                    var previousurl = System.Web.HttpContext.Current.Request.UrlReferrer;
                  
                    if (user.logtype == 1)
                    {
                        Session["logtype"] = user.logtype;
                        return RedirectToAction("Administrator", "Home");
                    }
                    else
                    {
                        //Session["downloadzipfile"] = adds.zipfilename;
                        //Session["previouspage"] = "TemplateDownload";
                        //if(Session["previouspage"] != null)
                        //{
                        //    return RedirectToAction()
                        //}
                        if (Session["templateidsend"] == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            var a = Session["templateidsend"];
                            int templatid = Convert.ToInt32(Session["templateidsend"]);
                            return RedirectToAction("TemplateDownload", "Home", new { tempplateid = templatid });
                        }

                        
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register registration)
        {
            int emailstatus = db.registers.Where(d => d.email == registration.email).Count();
            if(registration.password == registration.password2 && emailstatus == 0)
            {
                string pwd = SHA.GenerateSHA256String(registration.email + registration.password);
                registration.password = pwd;
                db.registers.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult SignOut()
        {
            Session["email"] = null;
            Session["logtype"] = null;
            Session["id"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Administrator()
        {
            int ids =Convert.ToInt32( Session["id"]);
            var mydbs = db.registers.Where(d => d.Id == ids).FirstOrDefault();
            ViewBag.myproject = db.addprojects.Where(d => d.sessionid == ids).ToList();
            if (mydbs.logtype == 1) { 
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
       
        [HttpPost]
        public ActionResult Administrator(string category, HttpPostedFileBase imgname1, HttpPostedFileBase imgname2, HttpPostedFileBase imgname3, HttpPostedFileBase zipfilename,string tittle,string smallDescription, string largeDescription,int? id)
        {
            if (tittle == null && id != null)
            {
                var objecttrace = db.addprojects.Where(d => d.id == id).FirstOrDefault();
                db.addprojects.Remove(objecttrace);
                db.SaveChanges();
                return RedirectToAction("Administrator", "Home");
            }
            else
            {
                string path = Server.MapPath("~/img/orderimage/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string path1 = Server.MapPath("~/img/orderimage/sourcefile/");
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }

                int idmy = Convert.ToInt32(Session["id"]);

                string count = db.addprojects.Count().ToString();
                string imgnamesone = Session["email"].ToString() + "1" + count + ".jpg";
                string imgnamestwo = Session["email"].ToString() + "2" + count + ".jpg";
                string imgnamesthree = Session["email"].ToString() + "3" + count + ".jpg";
                string zipimagename = Session["email"].ToString() + "originalimage" + count + ".zip";

                imgname1.SaveAs(path + System.IO.Path.GetFileName(imgnamesone));
                imgname2.SaveAs(path + System.IO.Path.GetFileName(imgnamestwo));
                imgname3.SaveAs(path + System.IO.Path.GetFileName(imgnamesthree));
                zipfilename.SaveAs(path1 + System.IO.Path.GetFileName(zipimagename));

                addproject projectmyupload = new addproject();
                projectmyupload.imgname1 = imgnamesone;
                projectmyupload.imgname2 = imgnamestwo;
                projectmyupload.imgname3 = imgnamesthree;
                projectmyupload.zipfilename = zipimagename;
                projectmyupload.category = category;
                projectmyupload.folderpath = "";
                projectmyupload.sessionid = idmy;
                projectmyupload.tittle = tittle;
                projectmyupload.smallDescription = smallDescription;
                projectmyupload.largeDescription = largeDescription;

                db.addprojects.Add(projectmyupload);
                db.SaveChanges();
                return View();
            }
        }

        //my second project**********************************************************************************************************************************************************************************************
        
    }
    }
