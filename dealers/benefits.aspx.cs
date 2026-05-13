using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.dealers
{
    public partial class benefits : System.Web.UI.Page
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
                LoadUserBenefits();
            }

        }

        private void LoadUserBenefits()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            int userId = Convert.ToInt32(Session["UserId"]);

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // GET SALES
                string getUser = "SELECT total_sales FROM users WHERE user_id = @id";
                MySqlCommand cmdUser = new MySqlCommand(getUser, con);
                cmdUser.Parameters.AddWithValue("@id", userId);

                object result = cmdUser.ExecuteScalar();
                decimal totalSales = result != null ? Convert.ToDecimal(result) : 0;

                // GET BENEFITS BASED ON SALES
                string getBenefits = @"
                    SELECT *
                    FROM rank_benefits
                    WHERE @sales >= min_sales
                    ORDER BY min_sales DESC
                    LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(getBenefits, con);
                cmd.Parameters.AddWithValue("@sales", totalSales);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SetBenefits(
                            reader["rank_name"].ToString(),
                            reader["discount"].ToString(),
                            reader["earnings"].ToString(),
                            reader["suki_perks"].ToString(),
                            reader["total_potential"].ToString(),
                            reader["next_goal"].ToString()
                        );
                    }
                    else
                    {
                        // fallback kapag walang match
                        SetBenefits("Bronze", "0%", "₱0", "₱0", "₱0", "No data available");
                    }
                }
            }
        }

        private void SetBenefits(string rank, string disc, string earn, string suki, string total, string goal)
        {
            rankBadge.InnerText = rank;
            rankBadge.Attributes["class"] = "badge large " + rank.ToLower();

            lblDiscount.InnerText = disc;
            lblEarnFromDiscount.InnerText = earn;
            lblSukiPerks.InnerText = suki;
            lblTotalPotential.InnerText = total;
            lblNextGoal.InnerText = goal;
        }
    }
}