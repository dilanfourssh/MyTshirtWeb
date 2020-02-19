using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tshirt.Classes;

namespace Tshirt.Controllers
{
    public class sha256Controller : Controller
    {
        // GET: sha256
        public ActionResult Index()
        {
            ViewBag.result = 0;
            ViewBag.y256 = "active";
            ViewBag.y512 = "";
            ViewBag.y1 = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string value, string hash)
        {
            if (value == "sha256")
            {
                string pwd = SHA.GenerateSHA256String(hash);
                ViewBag.result = pwd;
                ViewBag.y256 = "active";
                ViewBag.y512 = "";
                ViewBag.y1 = "";
                return View();
            }
            else if (value == "sha512")
            {
                string pwd1 = SHA.GenerateSHA512String(hash);
                ViewBag.result = pwd1;
                ViewBag.y256 = "";
                ViewBag.y512 = "active";
                ViewBag.y1 = "";
                return View();
            }
            else
            {
                string pwd2 = SHA.GenerateSHA1String(hash);
                ViewBag.result = pwd2;
                ViewBag.y256 = "";
                ViewBag.y512 = "";
                ViewBag.y1 = "active";
                return View();
            }
            return View();
        }
       
    }
}
