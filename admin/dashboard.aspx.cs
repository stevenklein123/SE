using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Project_Tracking.admin
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/auth_pages/login.aspx");
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

                // TOTAL PRODUCTS
                MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM products", con);
                lblTotalProducts.Text = cmd1.ExecuteScalar().ToString();

                // LOW STOCK
                MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM products WHERE stock <= 10", con);
                lblLowStock.Text = cmd2.ExecuteScalar().ToString();

                // OUT OF STOCK
                MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM products WHERE stock = 0", con);
                lblOutStock.Text = cmd3.ExecuteScalar().ToString();

                // TOTAL SALES
                MySqlCommand cmd4 = new MySqlCommand("SELECT IFNULL(SUM(total_amount),0) FROM orders_table", con);
                decimal sales = Convert.ToDecimal(cmd4.ExecuteScalar());
                lblSales.Text = sales.ToString("N0");
            }
        }
    }
}