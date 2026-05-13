using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.auth_pages
{
    public partial class change_password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/auth_pages/login.aspx");
            }

        }

        protected void BtnChange_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (Session["UserId"] == null)
                return;

            int userId = Convert.ToInt32(Session["UserId"]);

            string current = txtCurrent.Text?.Trim();
            string nw = txtNew.Text?.Trim();
            string confirm = txtConfirm.Text?.Trim();

            if (string.IsNullOrEmpty(current) ||
                string.IsNullOrEmpty(nw) ||
                string.IsNullOrEmpty(confirm))
            {
                lblMsg.Text = "Please fill all fields.";
                return;
            }

            if (nw.Length < 6)
            {
                lblMsg.Text = "Password must be at least 6 characters.";
                return;
            }

            if (nw != confirm)
            {
                lblMsg.Text = "Passwords do not match.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                // GET OLD HASH
                string dbHash = null;

                using (var cmd = new MySqlCommand(
                    "SELECT password_hash FROM users WHERE user_id=@id LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    dbHash = cmd.ExecuteScalar() as string;
                }

                if (dbHash == null)
                {
                    lblMsg.Text = "User not found.";
                    return;
                }

                if (!VerifyPassword(current, dbHash))
                {
                    lblMsg.Text = "Current password is incorrect.";
                    return;
                }

                // UPDATE PASSWORD
                string newHash = HashPassword(nw);

                using (var cmd = new MySqlCommand(
                    "UPDATE users SET password_hash=@hash WHERE user_id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@hash", newHash);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            // logout after change
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/auth_pages/login.aspx");
        }

        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                var parts = hash.Split(':');
                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                string stored = parts[2];

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    string check = Convert.ToBase64String(pbkdf2.GetBytes(32));
                    return check == stored;
                }
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            int iterations = 10000;
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, iterations, HashAlgorithmName.SHA256))
            {
                string hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return $"{iterations}:{Convert.ToBase64String(salt)}:{hash}";
            }
        }
    }
}