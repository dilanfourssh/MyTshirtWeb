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
using iTextSharp.text;
using System.Globalization;

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
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                
                var checkb = s.type.IndexOf("-");
                
                string whatistypemyform = "";
                if (checkb != -1)
                {
                    whatistypemyform = s.type.Substring(0, checkb);
                }

                string mytext = "";
                string dropdownlists = "";
                string dropdownhtml = "";
                if (textInfo.ToLower(s.type).Trim() == "textbox")
                {
                    var b = k.IndexOf("endtext");
                    mytext = k.Substring(0, b-1);
                    mytext = mytext.Replace("{name}", s.name);
                   
                }
                if (textInfo.ToLower(s.type).Trim() == "e-mails")
                {
                    var a = k.IndexOf("fromemail");
                    var b = k.IndexOf("endemail");
                    mytext = k.Substring((a+ "fromemail".Length + 1), (b-a- "fromemail".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);

                }
                if (textInfo.ToLower(s.type).Trim() == "textaria")
                {
                    var a = k.IndexOf("starttextaria");
                    var b = k.IndexOf("endtextaria");
                    mytext = k.Substring((a + "starttextaria".Length + 1), (b - a - "starttextaria".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);

                }
                if (textInfo.ToLower(s.type).Trim() == "fileuploader")
                {
                    var a = k.IndexOf("uploadfile");
                    var b = k.IndexOf("enduploadfile");
                    mytext = k.Substring((a + "uploadfile".Length + 1), (b - a - "uploadfile".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);

                }
                if (textInfo.ToLower(s.type).Trim() == "password")
                {
                    var a = k.IndexOf("passwordstart");
                    var b = k.IndexOf("passwordend");
                    mytext = k.Substring((a + "passwordstart".Length + 1), (b - a - "passwordstart".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);

                }
                if (textInfo.ToLower(s.type).Length > 15)
                {
                    string[] dropmenu = new string[10];
                    var b = s.type.IndexOf('-');
                    var c = s.type.IndexOf(',');
                   
                    string typeis = s.type.Substring(0, b);
                    string dropdownname = s.type.Substring((b+1),(s.type.Length - b - 1));
                    string nameis;
                    if (textInfo.ToLower(typeis).Trim() == "dropdown")
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
                if(textInfo.ToLower(s.type).Trim() == "submit")
                {
                    var a = k.IndexOf("fromsubmit");
                    var b = k.IndexOf("fromsubmitend");
                    mytext = k.Substring((a + "fromsubmit".Length + 1), (b - a - "fromsubmit".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);
                }
                if (textInfo.ToLower(whatistypemyform).Trim() == "checkbox")
                {
                    var n = s.type.Substring((checkb+1),(s.type.Length - checkb - 1));
                    string[] words = n.Split(',');
                    string checkboxhtmloption = "";
                    foreach(var a in words)
                    {
                        var checknamefirst = k.IndexOf("checkboxoption1234");
                        var checknameend = k.IndexOf("checkboxoptionend1234");
                        //string yds = k.Substring(checknamefirst+1,(k.Length-checknamefirst-1));
                        string checkhtml = k.Substring((checknamefirst+ "checkboxoption1234".Length + 1), (checknameend - checknamefirst - "checkboxoption1234".Length - 1));
                        checkhtml = checkhtml.Replace("{checkoption}", a);
                        checkboxhtmloption = checkboxhtmloption + checkhtml;


                    }
                    var d = k.IndexOf("boxcheckstart");
                    var b = k.IndexOf("boxcheckend");

                    mytext = k.Substring((d + "boxcheckstart".Length + 1), (b - d - "boxcheckstart".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);
                    mytext = mytext.Replace("{includecheckboxoption}", checkboxhtmloption);

                }
                if (textInfo.ToLower(whatistypemyform).Trim() == "radio")
                {
                    var n = s.type.Substring((checkb + 1), (s.type.Length - checkb - 1));
                    string[] words = n.Split(',');
                    string checkboxhtmloption = "";
                    foreach (var a in words)
                    {
                        var checknamefirst = k.IndexOf("radiobutton123");
                        var checknameend = k.IndexOf("radiobutton123end");
                        //string yds = k.Substring(checknamefirst+1,(k.Length-checknamefirst-1));
                        string checkhtml = k.Substring((checknamefirst + "radiobutton123".Length + 1), (checknameend - checknamefirst - "radiobutton123".Length - 1));
                        checkhtml = checkhtml.Replace("{checkoption}", a);
                        checkboxhtmloption = checkboxhtmloption + checkhtml;


                    }
                    var d = k.IndexOf("radiomain44");
                    var b = k.IndexOf("radiomain44end");

                    mytext = k.Substring((d + "radiomain44".Length + 1), (b - d - "radiomain44".Length - 1));
                    mytext = mytext.Replace("{name}", s.name);
                    mytext = mytext.Replace("{includeradioboxoption}", checkboxhtmloption);

                }
                y = y + mytext;
            }
            return y;
        }
    }
}