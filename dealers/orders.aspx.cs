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
                    SELECT 
                        o.order_id,
                        p.reference_no,
                        o.total_amount,
                        o.status,
                        o.order_date,
                        o.item_count,
                        o.shipping_method
                    FROM orders_table o
                    LEFT JOIN payments p ON p.order_id = o.order_id
                    WHERE o.user_id = @userId
                    AND (@status = 'all' OR o.status = @status)
                    ORDER BY o.order_id DESC";

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
                            refcode = r["reference_no"],
                            total = r["total_amount"],
                            status = r["status"],
                            date = Convert.ToDateTime(r["order_date"]).ToString("MMM dd yyyy"),
                            itemCount = r["item_count"],
                            shipping = r["shipping_method"]
                        });
                    }
                }
            }

            return list;
        }
    }
}