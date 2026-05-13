using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.dealers
{
    public partial class orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static List<Order> GetOrders(string filter)
        {
            List<Order> orderList = new List<Order>();
            string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            // Ensure we only return orders for the current logged in user
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["UserId"] == null)
            {
                return orderList;
            }

            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                // Build query filtering by user
                string query = "SELECT order_id, order_date, status, total_amount, item_count FROM orders_table WHERE user_id = @uid";

                if (filter != "all")
                {
                    query += " AND status = @status";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                if (filter != "all")
                {
                    cmd.Parameters.AddWithValue("@status", filter);
                }

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderList.Add(new Order
                            {
                                id = reader["order_id"].ToString(),
                                // Safe conversion
                                date = reader["order_date"] != DBNull.Value ?
                                       Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd") : "",
                                status = reader["status"].ToString().ToLower(),
                                total = Convert.ToDouble(reader["total_amount"]),
                                items = Convert.ToInt32(reader["item_count"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Bubble up as a generic exception; consider logging in production
                    throw new Exception("Database error: " + ex.Message);
                }
            }
            return orderList;
        }

        public class Order
        {
            public string id { get; set; }
            public string date { get; set; }
            public string status { get; set; }
            public double total { get; set; }
            public int items { get; set; }
        }

        [System.Web.Services.WebMethod]
        public static int GetUnreadCount()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["UserId"] == null)
                return 0;

            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr))
            {
                conn.Open();

                // Get user created_at to avoid showing old/global notifications to brand new users
                DateTime userCreated = DateTime.MinValue;
                MySql.Data.MySqlClient.MySqlCommand getUser = new MySql.Data.MySqlClient.MySqlCommand("SELECT created_at FROM users WHERE user_id=@uid LIMIT 1", conn);
                getUser.Parameters.AddWithValue("@uid", userId);
                var u = getUser.ExecuteScalar();
                if (u != null && u != DBNull.Value)
                {
                    userCreated = Convert.ToDateTime(u);
                }

                string query = "SELECT COUNT(*) FROM notifications WHERE is_read = 0 AND created_at >= @since";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@since", userCreated);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        [System.Web.Services.WebMethod]
        public static List<object> GetNotifications()
        {
            List<object> list = new List<object>();
            string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr))
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["UserId"] == null)
                    return list;

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                conn.Open();

                // filter notifications to those created after user signup so new users don't see old system alerts
                DateTime userCreated = DateTime.MinValue;
                MySql.Data.MySqlClient.MySqlCommand getUser = new MySql.Data.MySqlClient.MySqlCommand("SELECT created_at FROM users WHERE user_id=@uid LIMIT 1", conn);
                getUser.Parameters.AddWithValue("@uid", userId);
                var u = getUser.ExecuteScalar();
                if (u != null && u != DBNull.Value)
                {
                    userCreated = Convert.ToDateTime(u);
                }

                string query = "SELECT message, is_read, created_at FROM notifications WHERE created_at >= @since ORDER BY created_at DESC LIMIT 10";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@since", userCreated);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new
                        {
                            message = reader["message"].ToString(),
                            is_read = Convert.ToBoolean(reader["is_read"]),
                            date = Convert.ToDateTime(reader["created_at"]).ToString("MMM dd, hh:mm tt")
                        });
                    }
                }
            }
            return list;
        }
    }
}