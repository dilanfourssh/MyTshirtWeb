using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tshirt.Classes;
using Tshirt.Context;
using Tshirt.Models;

namespace Tshirt.Controllers
{
    public class formController : Controller
    {
        private tshirtContext db = new tshirtContext();
        // GET: bootstrapresposiveform
        public ActionResult Index()
        {
            var tshirtDetails = db.bestProducts.FirstOrDefault();
            return View();
        }
       
        [HttpPost]
        public ActionResult Index(string x)
        {
            return RedirectToAction("createhtml", "form", new { mydata = x });
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
            return RedirectToAction("bootstrapformdownload", "form");

        }
        public ActionResult bootstrapformdownload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult bootstrapformdownload(int x)
        {
            string fileName = "rectan.html";// Replace Your Filename with your required filename

            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            Response.TransmitFile(Server.MapPath("~/img/orderimage/" + fileName));//Place "YourFolder" your server folder Here

            Response.End();
            return RedirectToAction("nexthtml", "Home");
        }
        
    }
}