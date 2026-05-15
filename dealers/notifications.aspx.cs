using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace Project_Tracking.dealers
{
    public partial class notifications : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/auth_pages/login.aspx");
                return;
            }
        }

        [WebMethod(EnableSession = true)]
        public static List<object> GetNotifications()
        {
            var list = new List<object>();
            if (HttpContext.Current.Session["UserId"] == null) return list;

            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = @"SELECT id, message, type, is_read, created_at
                   FROM notifications
                   WHERE user_id = @userId
                   ORDER BY created_at DESC
                   LIMIT 50";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userId", userId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new
                        {
                            id = Convert.ToInt32(r["id"]),
                            message = r["message"]?.ToString() ?? "",
                            type = r["type"]?.ToString() ?? "",
                            isRead = Convert.ToInt32(r["is_read"]) == 1,
                            date = Convert.ToDateTime(r["created_at"]).ToString("MMM dd, yyyy hh:mm tt")
                        });
                    }
                }
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        public static void MarkAllRead()
        {
            if (HttpContext.Current.Session["UserId"] == null) return;

            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = "UPDATE notifications SET is_read=1 WHERE user_id=@userId";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod(EnableSession = true)]
        public static int GetUnreadCount()
        {
            if (HttpContext.Current.Session["UserId"] == null) return 0;

            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM notifications WHERE user_id=@userId AND is_read=0";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userId", userId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}