using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;

namespace Project_Tracking.admin
{
    public partial class create_user : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string role = ddlRole.SelectedValue;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            string hash = HashPassword(password);
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // 1. CHECK IF USER EXISTS
                string checkSql = "SELECT COUNT(*) FROM users WHERE username = @username";

                using (MySqlCommand checkCmd = new MySqlCommand(checkSql, con))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = "Username already exists!";
                        return;
                    }
                }

                // 2. INSERT USER
                string sql = @"
                    INSERT INTO users (username, password_hash, role)
                    VALUES (@username, @password_hash, @role)";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", hash);
                    cmd.Parameters.AddWithValue("@role", role);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMsg.ForeColor = Color.Green;
                        lblMsg.Text = "User created successfully!";
                    }
                    else
                    {
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = "Failed to create user.";
                    }
                }
            }
        }

        private string HashPassword(string password)
        {
            int iter = 10000;

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iter,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                return iter + ":" +
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash);
            }
        }
    }
}