using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web.UI;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project_Tracking.auth_pages
{
    public partial class login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ❌ REMOVE AUTO REDIRECT (important fix)
            // login page should NEVER auto redirect
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please fill all fields.";
                return;
            }

            if (Authenticate(username, password))
            {
                RedirectUser(Session["Role"].ToString());
            }
            else
            {
                lblError.Text = "Invalid username or password.";
            }
        }

        private bool Authenticate(string username, string password)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = @"
                    SELECT user_id, username, password_hash, role
                    FROM users
                    WHERE username = @username
                    LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hash = reader["password_hash"].ToString();

                            if (VerifyPassword(password, hash))
                            {
                                string userId = reader["user_id"].ToString();
                                string dbUsername = reader["username"].ToString();
                                string role = reader["role"].ToString().ToLower(); // IMPORTANT FIX

                                Session["UserId"] = userId;
                                Session["Username"] = dbUsername;
                                Session["Role"] = role;
                                Session["IsLoggedIn"] = true;

                                Session["ChatSessionId"] = Guid.NewGuid().ToString();
                                Session["chat_token"] = GenerateChatToken(userId, dbUsername, role);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void RedirectUser(string role)
        {
            role = role.ToLower(); // safety fix

            switch (role)
            {
                case "admin":
                    Response.Redirect("~/admin/dashboard.aspx");
                    break;

                case "dealer":
                    Response.Redirect("~/dealers/dashboard.aspx");
                    break;

                default:
                    Response.Redirect("~/user/dashboard.aspx");
                    break;
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            string[] parts = hash.Split(':');

            int iter = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            string stored = parts[2];

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iter,
                HashAlgorithmName.SHA256))
            {
                string newHash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return newHash == stored;
            }
        }

        private string GenerateChatToken(string userId, string username, string role)
        {
            var secret = ConfigurationManager.AppSettings["ChatSecret"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("user_id", userId),
                    new Claim("username", username),
                    new Claim("role", role)
                },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}