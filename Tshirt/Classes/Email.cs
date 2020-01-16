using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Tshirt.Classes
{
    public class Email
    {
        public static void SendMail(Boolean status, string ip)
        {
            try

            {
                var senderemail = new MailAddress(ConfigurationManager.AppSettings["bcsenderemail"].ToString(), "Sender");
                var receiveremail = new MailAddress(ConfigurationManager.AppSettings["bcreceiveremail"].ToString(), "Receiver");

                var password = ConfigurationManager.AppSettings["bcpasswd"].ToString();
                    var sub = "Administrator Entered Wrong Password";
                    var body = "Admin Has Entered the Wrong Password" + Environment.NewLine +
                         "Ip = " + ip; ;

                            if (status)
                            {
                                sub = "Administrator Ip Blocked";
                                body = "Mr. Admin Your Ip has been blocked!" + Environment.NewLine +
                                    "Ip = " + ip;
                    
                            }

                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["host"].ToString(),
                    Port = Int32.Parse(ConfigurationManager.AppSettings["Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderemail.Address, password)
                };
                            using (var message = new MailMessage(senderemail, receiveremail)
                {
                    Subject = sub,
                                Body = body,
                                IsBodyHtml = true
                            }
                            )
                            {
                                smtp.Send(message);
                            }
                        }
                        catch (Exception e)
                        {
                            e.InnerException.StackTrace.ToString();
                            //ViewBag.Error = "There are some problems in sending email";
                        }
                    }

                    public static void SendMail(string checkmail, string subject, string messaage)
            {
                var senderemail = new MailAddress(ConfigurationManager.AppSettings["bcsenderemail"].ToString(), "Sender");
                var receiveremail = new MailAddress(ConfigurationManager.AppSettings["bcreceiveremail"].ToString(), "Sender");
                var password = ConfigurationManager.AppSettings["bcpasswd"].ToString();

                if (checkmail == "Req1")
                {
                    senderemail = new MailAddress(ConfigurationManager.AppSettings["bcsenderemail"].ToString(), "Sender");
                    receiveremail = new MailAddress(ConfigurationManager.AppSettings["bcreceiveremail"].ToString(), "Receiver");
                    password = ConfigurationManager.AppSettings["bcpasswd"].ToString();

                }
                else if (checkmail == "Cont1")
                {
                    senderemail = new MailAddress(ConfigurationManager.AppSettings["consenderemail"].ToString(), "Sender");
                    receiveremail = new MailAddress(ConfigurationManager.AppSettings["conreceiveremail"].ToString(), "Receiver");
                    password = ConfigurationManager.AppSettings["conpasswd"].ToString();

                }

                var sub = subject;
                //var body = "MemberShip No = " + tempGroupData.membershipNo.ToString()  +  ", Email = " + tempGroupData.email.ToString() + ", BC No = " + tempGroupData.bcNo.ToString();

                string body = messaage;

                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["host"].ToString(),
                    Port = Int32.Parse(ConfigurationManager.AppSettings["Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderemail.Address, password)
                };

                using (var message = new MailMessage(senderemail, receiveremail)
                {
                    Subject = sub,
                    Body = body
                }
                )
                {
                    smtp.Send(message);
                }
}
        public static void SendMail(string checkmail, string resever, string subject, string messaage)
        {
            var senderemail = new MailAddress(ConfigurationManager.AppSettings["eemail"].ToString(), "Ceylonprint Confirmation");
            var receiveremail = new MailAddress(resever, "User");
            var password = ConfigurationManager.AppSettings["epwd"].ToString();



            var sub = subject;
            //var body = "MemberShip No = " + tempGroupData.membershipNo.ToString()  +  ", Email = " + tempGroupData.email.ToString() + ", BC No = " + tempGroupData.bcNo.ToString();

            string body = messaage;

            var smtp = new SmtpClient
            {
       
                Host = ConfigurationManager.AppSettings["host"].ToString(),
                Port = Int32.Parse(ConfigurationManager.AppSettings["Port"]),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(senderemail.Address, password),
        
                };

            using (var message = new MailMessage(senderemail, receiveremail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true

            }
            )
            {
                smtp.Send(message);
            }
        }
            }
}