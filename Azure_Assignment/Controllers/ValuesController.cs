using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class ValuesController : Controller
    {
        // GET: Values
        public ActionResult Index()
        {
            return View();
        }

        private DataPalkia db = new DataPalkia();

        // GET api/values

        public String CheckUsername(String username)
        {
            if (username.Trim().Length < 6 || username.Trim().Length > 50)
            {
                return "Username must be between 6 to 50 characters";
            }
            else
            {
                if (db.Users.Find(username) != null)
                    return "Username already exists";
                else
                    return null;
            }
            
        }

        public String CheckPassword(String password)
        {
            if (password.Trim().Length < 8 || password.Trim().Length > 50)
                return "Password must be between 8 to 50 characters";
            else
                return null;
        }

        public String CheckEmail(String emailAddress)
        {
            db.Users.Where(user => user.Email == emailAddress);
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                
                return null;

            }
            catch (FormatException)
            {
                return "Email is invalid";
            }
        }

        

    }
}