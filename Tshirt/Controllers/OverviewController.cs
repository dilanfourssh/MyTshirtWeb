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
        public ActionResult Progamming()
        {
            var tshirtDetails = ceylonprintmodelobject.TshirtImages.Where(d => d.category == "Progamming").ToList();
            ViewBag.tshirtdetailspasstheview = tshirtDetails;
            ViewBag.listcount = tshirtDetails.Count();
            return View();
        }
        public ActionResult Classes()
        {

            return View();
        }
        public ActionResult Couple()
        {
            return View();
        }
        public ActionResult Family()
        {
            return View();
        }
        public ActionResult Other()
        {
            return View();
        }
        public ActionResult Games()
        {
           
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