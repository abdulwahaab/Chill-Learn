using ChillLearn.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ChillLearn.Controllers
{
    public class Utility
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

        public static bool SendAccountActivationEmail(string emailTo, string name, string activationLink)
        {
            try
            {
                string emailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                string smtpClient = ConfigurationManager.AppSettings["SmtpClient"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtpClient);
                mail.From = new MailAddress(emailAddress);
                mail.To.Add(emailTo);
                if (CultureHelper.IsRighToLeft() == "rtl")
                {
                    mail.Subject = "بيانات الحساب على موقع تشل ليرن / تفعيل الحساب";
                    string userMessage = "";
                    userMessage = userMessage + emailTo + "<br/><b>الايميل:</b> ";
                    userMessage = userMessage + name + "<br/><b>إسم المستخدم:</b> ";

                    string emailBody = @"<html lang=""en"" style=""direction:rtl;text-align:center;"">
                                    <head><meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
                                        <title>
                                            تفعيل حسابكم فى موقع تشل ليرن
                                        </title>
                                        <style>
                                            table {
                                                border-collapse: collapse;
                                                width: 100%;
                                            }

                                            th, td {
                                                text-align: left;
                                                padding: 8px;
                                            }

                                            tr:nth-child(even) {
                                                background-color: #ffd10f
                                            }

                                            th {
                                                background-color: #4CAF50;
                                                color: white;
                                                text-align: center;
                                            }
                                        </style>
                                    </head>
                                        <body>
                                            <h2>موقع تشل ليرن</h2>
                                            <h2>ادراه حسابات المدرسين</h2>
                                            <h2>فعل حسابك وقدم أوراق إعتمادك</h2>
                                            <table>
                                                <tr>
                                                    <th colspan=""2"">موقع تشـل ليـرن لخدمات التعليم الإليكتروني  برجاء إكمال خطوات التسجيل, حتى تتمكن  من إستخدام المنصة فور إنطلاقنا.</th>
                                                </tr>
                                            </table>
                                            <p>
                                                أهلًا بمعلمنا المتميز @@@@@
                                            </p>
                                            <p>
                                                نهنئك اليوم فقد قطعت خطوة ممتازة في عملك معنا علي موقع تشـل ليـرن . نرجو تفعيل حسابك بالضغط على الرابط ادناه:
                                            </p>
                                            <p>activationlink</p>
                                            <p>وبعد تفعيل حسابكم, نرجو توفير الأتي, ليقوم  فريق العمل بمراجعة الأوراق التي ستقوم  بتسليمها، وتثبيتك كمعلم رسمياً لدينا:</p>
                                            <p>
                                                -	الشهادة الجامعية.
                                            </p>
                                            <p>
                                                -	بطاقة إثبات أو جواز السفر.
                                            </p>
                                            <p>
                                                -	معلومات حسابك البنكي.
                                            </p>
                                            <p>
                                                -	إثبات خبرة ( إختياري)
                                            </p>
                                            <p>
                                                لقد قمنا بهذه الخطوة حرصًا منا علي ضم المتميزين فقط في مجتمع تشـل ليـرن ، وأنت اليوم على بعد خطوة لتصبح  واحدًا منهم!
                                            </p>
                                            <p>
                                                شكرأ لك علي إستخدامك تشـل ليـرن ، ونتطلع إلى المزيد من التميز معك.
                                            </p>
                                            <p>
                                                جميع الحقوق محفوظة لموقع تشـل ليـرن  2019.
                                            </p>
                                            <p>
                                                تم إرسال هذه الرسالة إلي البريد الإلكتروني  useremail،
                                            </p>
                                            <p>
                                                لأنك مشترك في أحد خدمات موقع تشـل ليـرن ، فضلًا لا تقم بالرد علي هذه الرسالة، لقد تم إرسال هذه الرسالة بواسطة نظام البريد الإلكتروني التلقائي
                                            </p>
                                            <p>
                                                ، إذا كنت تريد مراسلة فريق عمل تشـل ليـرن ، يرجي التواصل معنا عن طريق
                                                Info@ChillLearn.com
                                            </p>
                                        </body>
                                        </html>";
                    emailBody = emailBody.Replace("activationlink", activationLink);
                    emailBody = emailBody.Replace("useremail", emailTo);
                    emailBody = emailBody.Replace("@@@@@", name);
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;
                }
                else
                {
                    mail.Subject = "Account information on Chillearn / activate you account";
                    string userMessage = "";
                    userMessage = userMessage + emailTo + "<br/><b>Email:</b> ";
                    userMessage = userMessage + name + "<br/><b>User name:</b> ";

                    string emailBody = @"<html lang=""en"" style=""direction:rtl;text-align:center;"">
                                    <head><meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
                                        <title>
                                        Activate your account in the Chillearn website
                                        </title>
                                        <style>
                                            table {
                                                border-collapse: collapse;
                                                width: 100%;
                                            }

                                            th, td {
                                                text-align: left;
                                                padding: 8px;
                                            }

                                            tr:nth-child(even) {
                                                background-color: #ffd10f
                                            }

                                            th {
                                                background-color: #4CAF50;
                                                color: white;
                                                text-align: center;
                                            }
                                        </style>
                                    </head>
                                        <body>
                                            <h2>Chillearn website</h2>
                                            <h2>Teacher Account mangment</h2>
                                            <h2>Activate your account and submit your credentials</h2>
                                            <table>
                                                <tr>
                                                    <th colspan=""2"">
                                                   Please complete the registration process, so you can use the platform as soon as we launch.
                                                </th>
                                                </tr>
                                            </table>
                                            <p>
                                                Welcome to our outstanding teacher @@@@@
                                            </p>
                                            <p>
                                            We congratulate you today for making a great stride in your work with ChillLearn. Please activate your account by clicking on the link below:
                                            </p>
                                            <p>activationlink</p>
                                            <p>After activating your account, please provide the following, so the team will review the papers you will hand over, and install you as an official teacher</p>
                                                <p>
                                                -	University degree.
                                                </p>
                                                <p>
                                                - Card ID or passport.                                   
                                                </p>
                                                <p>
                                                - Your bank account information.
                                                </p>
                                                <p>
                                                Proof of experience (optional)
                                                </p>
                                                <p>
                                                We've taken this step to include only the best in the Chilren community, and you are now one step closer to becoming one of them!                     
                                                </p>
                                                <p>
                                                Thank you for using ChillLearn, and look forward to your further excellence.                    
                                                </p>
                                                <p>
                                                All rights reserved for Chilearn 2019.                          
                                                </p>
                                                <p>
                                                This message was sent to useremail,
                                                </p>
                                                <p>
                                                Because you are a subscriber to a ChillLearn service, please do not reply to this message. This message was sent by the automatic email system. 
                                                </p>
                                                <p>
                                                If you would like to contact the Shell Lern ​​team, please contact us at    
                                                Info@ChillLearn.com
                                            </p>
                                        </body>
                                        </html>";
                    emailBody = emailBody.Replace("activationlink", activationLink);
                    emailBody = emailBody.Replace("useremail", emailTo);
                    emailBody = emailBody.Replace("@@@@@", name);
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;
                }
                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailAddress, password);
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        // end send emails
    }
}