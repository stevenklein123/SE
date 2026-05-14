using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.admin
{
    public partial class admin_orders : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders("Pending");
            }
        }

        private void LoadOrders(string status)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = @"
                SELECT 
                    o.order_id,
                    p.reference_no,
                    u.username,
                    o.total_amount,
                    o.item_count,
                    o.shipping_method,
                    o.order_date,
                    o.status
                FROM orders_table o

                INNER JOIN users u 
                    ON u.user_id = o.user_id

                LEFT JOIN payments p
                    ON p.order_id = o.order_id

                WHERE o.status = @status

                ORDER BY o.order_id DESC";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@status", status);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void FilterPending(object sender, EventArgs e)
        {
            LoadOrders("Pending");
        }

        protected void FilterApproved(object sender, EventArgs e)
        {
            LoadOrders("Approved");
        }

        protected void FilterRejected(object sender, EventArgs e)
        {
            LoadOrders("Rejected");
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ApproveOrder")
            {
                UpdateStatus(id, "Approved");
            }
            else if (e.CommandName == "RejectOrder")
            {
                UpdateStatus(id, "Rejected");
            }

            LoadOrders("Pending");
        }

        private void UpdateStatus(int id, string status)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string sql = "UPDATE orders_table SET status=@status WHERE order_id=@id";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Approved":
                    return "status-approved";

                case "Rejected":
                    return "status-rejected";

                default:
                    return "status-pending";
            }
        }
    }
}