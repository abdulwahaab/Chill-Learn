using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ChillLearn.Controllers
{
    public class utility
    {
        #region Create cookie
        public static void CreateCookie(string CookieName, string[] keys, string[] values, bool Expired)
        {

            HttpCookie c = new HttpCookie(CookieName);
            if (keys != null)
            {
                for (int x = 0; x < keys.Length; x++)
                    c.Values.Add(keys[x], values[x]);
                if (!Expired)
                    // c.Expires = DateTime.Now.AddYears(2);
                    c.Expires = DateTime.Now.AddDays(2);
            }// end  if 1
            else
                //  c.Expires = DateTime.Now.AddYears(-2);
                c.Expires = DateTime.Now.AddDays(-2);
            HttpContext.Current.Response.Cookies.Add(c);
        }

        #endregion

        #region Read from cookie

        public static string ReadFromCookie(string CookieName)
        {
            try
            {
                string exists;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
                if (cookie != null)
                {
                    return CookieName;
                }
                return exists = null;
            }
            catch
            {

                return null;
            }

        }



        #endregion

        #region remove cookie

        public static void RemoveCookie(string CookieName)
        {
            CreateCookie(CookieName, null, null, false);
        }

        #endregion

        #region sendemail

        public static string RecoverPassword(string password, string emailId, string name)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(emailId);
                mail.From = new MailAddress("dmng23@gmail.com");
                mail.Subject = "بيانات الحساب على موقع التوظيف (مدارس الخندق )/ استعادة كلمة المرور";
                string userMessage = "";

                userMessage = userMessage + emailId + "<br/><b>الايميل:</b> ";
                userMessage = userMessage + name + "<br/><b>إسم المستخدم:</b> ";
                userMessage = userMessage + password + "<br/><b>كلمة المرور: </b>";

                //string Body = name + "عزيزي " + ", <br/><br/>تفاصيل الدخول الي حسابكم هي <br/></br> " + userMessage + "<br/><br/>شكرا لاستخدامكم موقعنا";

                string Body2 = @"<html lang=""en"">
                                    <head>    
                                        <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
                                        <title>
                                            Mr.alaa Mohamed Elockle
                                        </title>
                                        <style>
                                            table {border-collapse: collapse;width: 100%;}
                                            th, td {text-align: left;padding: 8px;}
                                            tr:nth-child(even){background-color: #ffd10f}
                                            th {background-color: #4CAF50;color: white;text-align: center;}
                                            </style>

                                    </head>
                                    <body>
                                          <h2>مدارس الخندق الاهلية</h2>  
                                          <h2>قسم التوظيف</h2>
                                         <h2>البيانات الخاصة بحسابكم لدى مدارس الخندق قسم :- التوظيف </h2>
                                        <table>
                                                <tr>
                                                    <th colspan=""2"">البيانات الاساسية</th>
                                                </tr>
                                                <tr>
                                                    <td>" + emailId + "<br/><b>الايميل:</b> " + @"</td>
                                                    <td>" + name + "<br/><b>الايميل:</b> " + @"</td>
                                                    <td>" + password + "<br/><b>الايميل:</b> " + @"</td>
                                                </tr>
                                                <tr>
                                                    <td style=""background-color: #4CAF50;color: white;text-align: center;"">This Email has been developed by Mr.alaa Moahmed @2017</td>
                                                </tr>
                                        </table>
                                    </body>
                                </html>";

                mail.Body = Body2;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //SMTP Server Address of gmail
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("dmng23@gmail.com", "147852258741");
                // Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return "Please check your email for account login detail.";
            }
            catch
            {
                return "Error............";
            }
        }


        #endregion
        // end send emails
    }
}