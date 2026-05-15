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
                INNER JOIN users u ON u.user_id = o.user_id
                LEFT JOIN payments p ON p.order_id = o.order_id
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

        protected void FilterPending(object sender, EventArgs e) { LoadOrders("Pending"); }
        protected void FilterApproved(object sender, EventArgs e) { LoadOrders("Approved"); }
        protected void FilterRejected(object sender, EventArgs e) { LoadOrders("Rejected"); }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ApproveOrder")
                UpdateStatus(id, "Approved");
            else if (e.CommandName == "RejectOrder")
                UpdateStatus(id, "Rejected");

            LoadOrders("Pending");
        }

        private void UpdateStatus(int id, string status)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // Check current status
                MySqlCommand check = new MySqlCommand(
                    "SELECT status FROM orders_table WHERE order_id=@id", con);
                check.Parameters.AddWithValue("@id", id);
                string currentStatus = check.ExecuteScalar()?.ToString();

                if (currentStatus != "Pending")
                    return;

                // Update order status
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE orders_table SET status=@status WHERE order_id=@id", con);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                // Get user_id while connection is open
                MySqlCommand getUser = new MySqlCommand(
                    "SELECT user_id FROM orders_table WHERE order_id=@id", con);
                getUser.Parameters.AddWithValue("@id", id);
                int userId = Convert.ToInt32(getUser.ExecuteScalar());

                // Deduct stock if approved
                if (status == "Approved")
                {
                    string getItems = @"
                SELECT ti.product_name, SUM(ti.quantity) as quantity, p.product_id
                FROM transaction_items ti
                JOIN transactions t ON t.transaction_id = ti.transaction_id
                JOIN products p ON p.product_name = ti.product_name
                WHERE t.order_id = @id
                GROUP BY ti.product_name, p.product_id";

                    MySqlCommand getCmd = new MySqlCommand(getItems, con);
                    getCmd.Parameters.AddWithValue("@id", id);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(getCmd).Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        MySqlCommand stock = new MySqlCommand(@"
                    UPDATE products 
                    SET stock = stock - @qty 
                    WHERE product_id = @pid", con);
                        stock.Parameters.AddWithValue("@qty", Convert.ToInt32(row["quantity"]));
                        stock.Parameters.AddWithValue("@pid", Convert.ToInt32(row["product_id"]));
                        stock.ExecuteNonQuery();
                    }
                }

                // Insert notification
                string message = status == "Approved"
                    ? $"Your order #{id} has been approved! 🎉"
                    : $"Your order #{id} has been rejected.";

                string type = status == "Approved" ? "order_approved" : "order_rejected";

                MySqlCommand notif = new MySqlCommand(@"
            INSERT INTO notifications (user_id, message, type)
            VALUES (@userId, @message, @type)", con);
                notif.Parameters.AddWithValue("@userId", userId);
                notif.Parameters.AddWithValue("@message", message);
                notif.Parameters.AddWithValue("@type", type);
                notif.ExecuteNonQuery();
            }
        }

        public string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Approved": return "status-approved";
                case "Rejected": return "status-rejected";
                default: return "status-pending";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadOrders("Pending");
                return;
            }

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
                INNER JOIN users u ON u.user_id = o.user_id
                LEFT JOIN payments p ON p.order_id = o.order_id
                WHERE p.reference_no LIKE @keyword
                ORDER BY o.order_id DESC";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
    }
}