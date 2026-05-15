using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Project_Tracking.admin
{
    public partial class sales : System.Web.UI.Page
    {
        protected string chartLabels = "[]";
        protected string chartData = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYears();
                LoadData();
            }
        }

        private void LoadYears()
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem("All Years", ""));

            for (int y = 2024; y <= 2026; y++)
                ddlYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string query = @"
                    SELECT 
                        DATE_FORMAT(order_date, '%Y-%m') AS sales_month,
                        SUM(total_amount) AS total_sales,
                        COUNT(order_id) AS order_count
                    FROM orders_table
                    WHERE status = 'Approved'
                ";

                if (!string.IsNullOrEmpty(ddlMonth.SelectedValue))
                    query += " AND MONTH(order_date) = @month";

                if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
                    query += " AND YEAR(order_date) = @year";

                query += @"
                    GROUP BY DATE_FORMAT(order_date, '%Y-%m')
                    ORDER BY sales_month ASC
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);

                if (!string.IsNullOrEmpty(ddlMonth.SelectedValue))
                    cmd.Parameters.AddWithValue("@month", ddlMonth.SelectedValue);

                if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
                    cmd.Parameters.AddWithValue("@year", ddlYear.SelectedValue);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvSales.DataSource = dt;
                gvSales.DataBind();

                BuildChart(dt);
                ComputeTotal(dt);
            }
        }

        private void BuildChart(DataTable dt)
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();

            labels.Append("[");
            data.Append("[");

            foreach (DataRow row in dt.Rows)
            {
                labels.Append($"'{row["sales_month"]}',");
                data.Append($"{row["total_sales"]},");
            }

            if (dt.Rows.Count > 0)
            {
                labels.Length--;
                data.Length--;
            }

            labels.Append("]");
            data.Append("]");

            chartLabels = labels.ToString();
            chartData = data.ToString();
        }

        private void ComputeTotal(DataTable dt)
        {
            decimal total = 0;

            foreach (DataRow row in dt.Rows)
                total += Convert.ToDecimal(row["total_sales"]);

            lblTotalRevenue.Text = total.ToString("N2");
        }
    }
}