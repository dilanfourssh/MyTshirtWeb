using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tshirt.Context;
using Tshirt.Models;

namespace Tshirt.Controllers
{
    
    public class OverviewController : Controller
    {
        private ceylonprintEntities ceylonprintmodelobject = new ceylonprintEntities();
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
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult BuyerRegiser()
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
            return View();
        }
        public ActionResult SuccessfullyRegister()
        {
            return View();
        }
    }
}