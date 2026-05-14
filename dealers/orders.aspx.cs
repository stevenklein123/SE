using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web.Services;
using System.Web.UI;

namespace Project_Tracking.dealers
{
    public partial class orders : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/auth_pages/login.aspx");
                return;
            }
        }

        // ✔ GET ORDERS FOR DEALER (AJAX)
        [WebMethod(EnableSession = true)]
        public static object GetOrders(string status)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            int userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);

            var list = new System.Collections.Generic.List<object>();

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                string sql = @"
                    SELECT order_id, reference_code, total_amount, status, order_date
                    FROM orders_table
                    WHERE user_id=@userId
                    AND (@status='all' OR status=@status)
                    ORDER BY order_id DESC";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@status", status);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new
                        {
                            id = r["order_id"],
                            refcode = r["reference_code"],
                            total = r["total_amount"],
                            status = r["status"],
                            date = Convert.ToDateTime(r["order_date"]).ToString("MMM dd yyyy")
                        });
                    }
                }
            }

            return list;
        }
    }
}