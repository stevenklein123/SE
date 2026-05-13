using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Services;

namespace Project_Tracking.auth_pages
{
    public partial class personal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                Response.Write("DB ERROR: " + ex.Message);
            }
        }
        [WebMethod]
        public static object SaveTempProfile(
           string firstName,
           string lastName,
           string middleName,
           string birthday,
           string contactNumber,
           string mobileNumber,
           string address,
           string zipCode,
           int termsAccepted,
           string email
       )
        {
            // ❌ TERMS REQUIRED
            if (termsAccepted != 1)
                return new { success = false, message = "Please accept Terms and Conditions." };

            // ❌ EMPTY VALIDATION
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(address))
            {
                return new { success = false, message = "Please fill in required fields." };
            }

            // ❌ CONTACT NUMBER LIMIT (11 digits PH)
            if (contactNumber.Length > 11 || mobileNumber.Length > 11)
            {
                return new { success = false, message = "Contact/Mobile number must be 11 digits only." };
            }

            // ❌ ZIP CODE LIMIT (4 digits PH)
            if (zipCode.Length > 4)
            {
                return new { success = false, message = "ZIP code must be 4 digits only." };
            }

            var tempProfile = new Dictionary<string, string>
            {
                { "first_name", firstName },
                { "middle_name", middleName },
                { "last_name", lastName },
                { "email", email },
                { "birthday", birthday },
                { "contact_number", contactNumber },
                { "mobile_number", mobileNumber },
                { "address", address },
                { "zip_code", zipCode },
                { "terms_accepted", "1" }
            };

            HttpContext.Current.Session["TempProfile"] = tempProfile;

            return new
            {
                success = true,
                redirect = "/auth_pages/register.aspx"
            };
        }
    }
}