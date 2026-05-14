using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace Project_Tracking.admin
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null || Session["Role"] == null)
            {
                Response.Redirect("~/auth_pages/login.aspx");
                return;
            }

            if (Session["Role"].ToString().ToLower() != "admin")
            {
                Response.Redirect("~/dealers/dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStats();
            }
        }

        private void LoadStats()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // PRODUCTS
                MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM products", con);
                lblTotalProducts.Text = cmd1.ExecuteScalar().ToString();

                MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM products WHERE stock <= 10", con);
                lblLowStock.Text = cmd2.ExecuteScalar().ToString();

                MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM products WHERE stock = 0", con);
                lblOutStock.Text = cmd3.ExecuteScalar().ToString();

                // SALES
                MySqlCommand cmd4 = new MySqlCommand("SELECT IFNULL(SUM(total_amount),0) FROM orders_table", con);
                lblSales.Text = Convert.ToDecimal(cmd4.ExecuteScalar()).ToString("N0");

                // PENDING ORDERS COUNT
                MySqlCommand cmd5 = new MySqlCommand(
                    "SELECT COUNT(*) FROM orders_table WHERE status='Pending'", con);

                lblPendingOrders.Text = cmd5.ExecuteScalar().ToString();
            }
        }
    }
}