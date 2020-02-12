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
namespace Tshirt.Classes
{
    public class webform
    {
        private tshirtContext db = new tshirtContext();
        public string layout(List<bootstrap_type> typeour,string k)
        {
            
            string y="";
            
            foreach (var s in typeour)
            {
                string mytext = "";
                string dropdownlists = "";
                string dropdownhtml = "";
                if (s.type == "textbox")
                {
                    var b = k.IndexOf("endtext");
                    mytext = k.Substring(0, b-1);
                    mytext = mytext.Replace("{name}", s.name);
                   
                }
                if (s.type == "e-mails")
                {
                    var a = k.IndexOf("fromemail");
                    var b = k.IndexOf("endemail");
                    mytext = k.Substring((a+ "fromemail".Length + 1), (b-a- "fromemail".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);

                }
                if(s.type.Length > 15)
                {
                    string[] dropmenu = new string[10];
                    var b = s.type.IndexOf('-');
                    var c = s.type.IndexOf(',');
                   
                    string typeis = s.type.Substring(0, b);
                    string dropdownname = s.type.Substring((b+1),(s.type.Length - b - 1));
                    string nameis;
                    if (typeis == "dropdown")
                    {
                        var firstoption = k.IndexOf("fromdropdownoption");
                        var secondoption = k.IndexOf("enddropdownoption");
                        
                        for (int i = 0; i < 10; i++)
                        {
                            string dropdownitemhtml = k.Substring((firstoption+ "fromdropdownoption".Length + 1), (secondoption - firstoption - "fromdropdownoption".Length - 1));
                            var result = dropdownname.IndexOf(',');
                            if (result == -1)
                            {
                                nameis = dropdownname;
                                dropdownitemhtml = dropdownitemhtml.Replace("{option}", nameis);
                                dropdownlists = dropdownlists + dropdownitemhtml;
                                break;
                            }
                            nameis = dropdownname.Substring(0,result);
                            
                            dropdownname = dropdownname.Substring(result + 1 ,(dropdownname.Length - result - 1) );
                            dropdownitemhtml = dropdownitemhtml.Replace("{option}", nameis);
                            dropdownlists = dropdownlists + dropdownitemhtml;
                        }
                        var firstnum = k.IndexOf("fromdropdown");
                        var secondfrom = k.IndexOf("enddropdown");
                        dropdownhtml = k.Substring((firstnum+ "fromdropdown".Length + 1), (secondfrom - "fromdropdown".Length - firstnum - 1));
                        dropdownhtml = dropdownhtml.Replace("{name}", s.name);
                        dropdownhtml = dropdownhtml.Replace("include_option", dropdownlists);
                        mytext = dropdownhtml;

                    }
                }
                if(s.type == "submit")
                {
                    var a = k.IndexOf("fromsubmit");
                    var b = k.IndexOf("fromsubmitend");
                    mytext = k.Substring((a + "fromsubmit".Length + 1), (b - a - "fromsubmit".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);
                }
                y = y + mytext;
            }
            return y;
        }
    }
}