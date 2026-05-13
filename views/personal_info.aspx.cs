using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.views
{
    public partial class personal_info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                // not authenticated - redirect to login
                Response.Redirect("~/auth_pages/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }

        }
        private void LoadProfile()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = @"SELECT up.first_name, up.last_name, up.middle_name, up.birthday, up.contact_number, up.mobile_number, up.address, up.zip_code, up.terms_accepted, u.email
                               FROM user_profiles up
                               LEFT JOIN users u ON u.user_id = up.user_id
                               WHERE up.user_id = @uid LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            lblFirst.Text = r["first_name"]?.ToString() ?? string.Empty;
                            lblMiddle.Text = r["middle_name"]?.ToString() ?? string.Empty;
                            lblLast.Text = r["last_name"]?.ToString() ?? string.Empty;
                            lblEmail.Text = r["email"]?.ToString() ?? string.Empty;
                            lblBirthday.Text = r["birthday"] != DBNull.Value ? Convert.ToDateTime(r["birthday"]).ToString("yyyy-MM-dd") : string.Empty;
                            lblContact.Text = r["contact_number"]?.ToString() ?? string.Empty;
                            lblMobile.Text = r["mobile_number"]?.ToString() ?? string.Empty;
                            lblAddress.Text = r["address"]?.ToString() ?? string.Empty;
                            lblZip.Text = r["zip_code"]?.ToString() ?? string.Empty;
                        }
                        else
                        {
                            // profile not found - try to fetch email from users table
                            r.Close();
                            using (var cmd2 = new MySqlCommand("SELECT email FROM users WHERE user_id=@uid LIMIT 1", con))
                            {
                                cmd2.Parameters.AddWithValue("@uid", userId);
                                using (var r2 = cmd2.ExecuteReader())
                                {
                                    if (r2.Read()) lblEmail.Text = r2["email"]?.ToString() ?? string.Empty;
                                }
                            }

                            lblFirst.Text = lblLast.Text = lblMiddle.Text = string.Empty;
                        }
                    }
                }
            }
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            // Redirect to the edit page (personal.aspx) where the user can edit and save
            Response.Redirect("~/auth_pages/personal.aspx");
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object GetProfile()
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                    return new { error = "Not authenticated" };

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string sql = @"SELECT up.first_name, up.last_name, up.middle_name, up.birthday, up.contact_number, up.mobile_number, up.address, up.zip_code, u.email
                                    FROM user_profiles up
                                    LEFT JOIN users u ON u.user_id = up.user_id
                                    WHERE up.user_id = @uid LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@uid", userId);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                return new
                                {
                                    first_name = r["first_name"]?.ToString() ?? string.Empty,
                                    middle_name = r["middle_name"]?.ToString() ?? string.Empty,
                                    last_name = r["last_name"]?.ToString() ?? string.Empty,
                                    email = r["email"]?.ToString() ?? string.Empty,
                                    birthday = r["birthday"] != DBNull.Value ? Convert.ToDateTime(r["birthday"]).ToString("yyyy-MM-dd") : string.Empty,
                                    contact_number = r["contact_number"]?.ToString() ?? string.Empty,
                                    mobile_number = r["mobile_number"]?.ToString() ?? string.Empty,
                                    address = r["address"]?.ToString() ?? string.Empty,
                                    zip_code = r["zip_code"]?.ToString() ?? string.Empty
                                };
                            }
                        }
                    }
                }

                return new { error = "Profile not found" };
            }
            catch (Exception ex)
            {
                return new { error = "Server error: " + ex.Message };
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object UpdateProfile(string firstName, string middleName, string lastName, string birthday, string contactNumber, string mobileNumber, string address, string zipCode)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                    return new { success = false, message = "Not authenticated" };

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();

                    // Try update first
                    string update = @"UPDATE user_profiles
                                      SET first_name=@first, middle_name=@middle, last_name=@last, birthday=@bday, contact_number=@contact, mobile_number=@mobile, address=@addr, zip_code=@zip
                                      WHERE user_id=@uid";

                    using (MySqlCommand cmd = new MySqlCommand(update, con))
                    {
                        cmd.Parameters.AddWithValue("@first", firstName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@middle", middleName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@last", lastName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@bday", string.IsNullOrEmpty(birthday) ? (object)DBNull.Value : birthday);
                        cmd.Parameters.AddWithValue("@contact", contactNumber ?? string.Empty);
                        cmd.Parameters.AddWithValue("@mobile", mobileNumber ?? string.Empty);
                        cmd.Parameters.AddWithValue("@addr", address ?? string.Empty);
                        cmd.Parameters.AddWithValue("@zip", zipCode ?? string.Empty);
                        cmd.Parameters.AddWithValue("@uid", userId);

                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            // insert if no existing profile
                            string ins = @"INSERT INTO user_profiles(user_id, first_name, middle_name, last_name, birthday, contact_number, mobile_number, address, zip_code, terms_accepted)
                                             VALUES(@uid,@first,@middle,@last,@bday,@contact,@mobile,@addr,@zip,1)";
                            using (MySqlCommand cmd2 = new MySqlCommand(ins, con))
                            {
                                cmd2.Parameters.AddWithValue("@uid", userId);
                                cmd2.Parameters.AddWithValue("@first", firstName ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@middle", middleName ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@last", lastName ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@bday", string.IsNullOrEmpty(birthday) ? (object)DBNull.Value : birthday);
                                cmd2.Parameters.AddWithValue("@contact", contactNumber ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@mobile", mobileNumber ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@addr", address ?? string.Empty);
                                cmd2.Parameters.AddWithValue("@zip", zipCode ?? string.Empty);
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return new { success = true };
            }
            catch (Exception ex)
            {
                return new { success = false, message = "Server error: " + ex.Message };
            }
        }
    }
}