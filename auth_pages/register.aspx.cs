using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Project_Tracking.auth_pages
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("dealers/dashboard.aspx");
            }

            // If there's a TempProfile in session, prefill email (and optionally other fields)
            var temp = Session["TempProfile"] as System.Collections.Generic.Dictionary<string, string>;
            if (temp != null)
            {
                try
                {
                    if (temp.ContainsKey("email") && !string.IsNullOrWhiteSpace(temp["email"]))
                        email.Value = temp["email"];
                }
                catch { }
            }
        }
        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            string user = username.Value.Trim();
            string mail = email.Value.Trim();
            string pass = password.Value;
            string confirm = confirmPassword.Value;

            if (string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(mail) ||
                string.IsNullOrWhiteSpace(pass) ||
                string.IsNullOrWhiteSpace(confirm))
            {
                ShowError("All fields are required.");
                return;
            }

            if (!IsValidEmail(mail))
            {
                ShowError("Invalid email address.");
                return;
            }

            if (!IsValidPassword(pass))
            {
                ShowError("Weak password. Use 8+ chars, upper, lower, number, special.");
                return;
            }

            if (pass != confirm)
            {
                ShowError("Passwords do not match.");
                return;
            }

            // EMAIL lang ang iche-check natin
            if (EmailExists(mail))
            {
                ShowError("Email already registered.");
                return;
            }

            if (SaveUser(user, mail, pass))
            {
                // If there is a temp profile in session, associate it with the created user
                try
                {
                    var temp = Session["TempProfile"] as System.Collections.Generic.Dictionary<string, string>;
                    if (temp != null)
                    {
                        int newUserId = 0;
                        string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(cs))
                        {
                            con.Open();
                            using (var cmd = new MySqlCommand("SELECT user_id FROM users WHERE email=@email LIMIT 1", con))
                            {
                                cmd.Parameters.AddWithValue("@email", mail);
                                using (var r = cmd.ExecuteReader())
                                {
                                    if (r.Read()) newUserId = Convert.ToInt32(r["user_id"]);
                                }
                            }

                            if (newUserId > 0)
                            {
                                using (var ins = new MySqlCommand(@"INSERT INTO user_profiles (user_id, first_name, last_name, middle_name, birthday, contact_number, mobile_number, address, zip_code, terms_accepted, created_at) VALUES (@uid,@fn,@ln,@mn,@bd,@cn,@mn2,@addr,@zip,@terms,NOW())", con))
                                {
                                    ins.Parameters.AddWithValue("@uid", newUserId);
                                    ins.Parameters.AddWithValue("@fn", temp.ContainsKey("first_name") ? temp["first_name"] : string.Empty);
                                    ins.Parameters.AddWithValue("@ln", temp.ContainsKey("last_name") ? temp["last_name"] : string.Empty);
                                    ins.Parameters.AddWithValue("@mn", temp.ContainsKey("middle_name") ? temp["middle_name"] : string.Empty);
                                    string bdVal = temp.ContainsKey("birthday") ? temp["birthday"] : string.Empty;
                                    ins.Parameters.AddWithValue("@bd", string.IsNullOrEmpty(bdVal) ? (object)DBNull.Value : Convert.ToDateTime(bdVal));
                                    ins.Parameters.AddWithValue("@cn", temp.ContainsKey("contact_number") ? temp["contact_number"] : string.Empty);
                                    ins.Parameters.AddWithValue("@mn2", temp.ContainsKey("mobile_number") ? temp["mobile_number"] : string.Empty);
                                    ins.Parameters.AddWithValue("@addr", temp.ContainsKey("address") ? temp["address"] : string.Empty);
                                    ins.Parameters.AddWithValue("@zip", temp.ContainsKey("zip_code") ? temp["zip_code"] : string.Empty);
                                    ins.Parameters.AddWithValue("@terms", temp.ContainsKey("terms_accepted") && temp["terms_accepted"] == "1" ? 1 : 0);
                                    ins.ExecuteNonQuery();
                                }
                            }
                        }

                        // Clear temp profile after consuming
                        Session.Remove("TempProfile");
                    }
                }
                catch { }

                Response.Redirect("login.aspx");
            }
            else
            {
                ShowError("Registration failed.");
            }
        }

        private void ShowError(string msg)
        {
            lblError.Text = msg;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var m = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!Regex.IsMatch(password, "[A-Z]")) return false;
            if (!Regex.IsMatch(password, "[a-z]")) return false;
            if (!Regex.IsMatch(password, "[0-9]")) return false;

            // mas safe special char checker
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) return false;

            return true;
        }

        private bool EmailExists(string email)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = "SELECT COUNT(*) FROM users WHERE email=@email";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool SaveUser(string username, string email, string password)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            string hash = HashPassword(password);

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = @"INSERT INTO users
                               (username,password_hash,role,email,created_at)
                               VALUES
                               (@username,@password,'dealer',@email,NOW())";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hash);
                    cmd.Parameters.AddWithValue("@email", email);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private string HashPassword(string password)
        {
            const int iterations = 10000;
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                return iterations + ":" +
                       Convert.ToBase64String(salt) + ":" +
                       Convert.ToBase64String(hash);
            }
        }
    }
}