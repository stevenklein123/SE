using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Project_Tracking.auth_pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/dealers/dashboard.aspx");
            }
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
                Response.Redirect("~/dealers/dashboard.aspx");
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

                string sql = "SELECT user_id, username, password_hash, role FROM users WHERE username=@username LIMIT 1";

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
                                string usernameValue = reader["username"].ToString();
                                string role = reader["role"].ToString();

                                Session["UserId"] = userId;
                                Session["Username"] = usernameValue;
                                Session["Role"] = role;
                                Session["IsLoggedIn"] = true;

                                // UNIQUE CHAT SESSION
                                Session["ChatSessionId"] =
                                    userId + "-" + Guid.NewGuid().ToString();

                                // JWT TOKEN FOR CHATBASE
                                Session["chat_token"] =
                                    GenerateChatToken(userId, usernameValue);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool VerifyPassword(string password, string hash)
        {
            string[] parts = hash.Split(':');

            int iter = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            string stored = parts[2];

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256))
            {
                string newHash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return newHash == stored;
            }
        }
        private string GenerateChatToken(
        string userId,
        string username)
        {
            var secret =
                ConfigurationManager
                .AppSettings["ChatSecret"];

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    claims: new[]
                    {
                new Claim("user_id", userId),
                new Claim("username", username)
                    },
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}