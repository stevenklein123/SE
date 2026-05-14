using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text;

namespace Project_Tracking.admin
{
    public partial class sales : System.Web.UI.Page
    {
        protected string chartLabels = "";
        protected string chartData = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYears();
                LoadData();
            }
        }

        // LOAD YEARS DROPDOWN
        private void LoadYears()
        {
            ddlYear.Items.Clear();

            for (int y = 2024; y <= 2026; y++)
            {
                ddlYear.Items.Add(new System.Web.UI.WebControls.ListItem(y.ToString(), y.ToString()));
            }
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
                        rep_name,
                        DATE_FORMAT(sale_date, '%Y-%m') AS month,
                        SUM(sales_amount) AS total_sales
                    FROM sales
                    WHERE 1=1
                ";

                // FILTER MONTH
                if (!string.IsNullOrEmpty(ddlMonth.SelectedValue))
                {
                    query += " AND MONTH(sale_date) = @month";
                }

                // FILTER YEAR
                if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
                {
                    query += " AND YEAR(sale_date) = @year";
                }

                query += @"
                    GROUP BY rep_name, DATE_FORMAT(sale_date, '%Y-%m')
                    ORDER BY sale_date;
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

            foreach (DataRow row in dt.Rows)
            {
                labels.Append($"'{row["month"]}',");
                data.Append(row["total_sales"] + ",");
            }

            chartLabels = labels.ToString().TrimEnd(',');
            chartData = data.ToString().TrimEnd(',');
        }

        private void ComputeTotal(DataTable dt)
        {
            decimal total = 0;

            foreach (DataRow row in dt.Rows)
            {
                total += Convert.ToDecimal(row["total_sales"]);
            }

            lblTotalRevenue.Text = total.ToString("N2");
        }
    }
}