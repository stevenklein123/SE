using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.admin
{
    public partial class dealers_monitoring : Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["MyDbConn"]
            .ConnectionString;

        // ── Rank thresholds (Suki Points = Total Sales 1:1) ──
        // Based on Avon Rewards chart
        const decimal NEW_MAX = 1_599.99m;
        const decimal BRONZE_MIN = 1_600m;
        const decimal SILVER_MIN = 3_700m;
        const decimal GOLD_MIN = 15_000m;
        const decimal PLATINUM_MIN = 26_250m;  // P26,250 from image
        const decimal DIAMOND_MIN = 75_000m;

        // Footer totals
        decimal _aprilTotal = 0, _mayTotal = 0, _juneTotal = 0, _grandTotal = 0;
        int _diamondCt = 0, _platinumCt = 0, _goldCt = 0, _silverCt = 0, _bronzeCt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
                LoadMonitoring();
            }
        }

        private void LoadMonitoring()
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // Pull Q2 Approved orders for current year
                // total_sales = suki points (1 peso = 1 point)
                string sql = @"
                    SELECT
                        CONCAT(
                            IFNULL(up.first_name,''), ' ',
                            IFNULL(up.last_name,'')
                        ) AS dealer_name,

                        COALESCE(SUM(
                            CASE WHEN MONTH(o.order_date) = 4
                                 AND YEAR(o.order_date) = YEAR(CURDATE())
                                 AND o.status = 'Approved'
                            THEN o.total_amount ELSE 0 END
                        ), 0) AS april_sales,

                        COALESCE(SUM(
                            CASE WHEN MONTH(o.order_date) = 5
                                 AND YEAR(o.order_date) = YEAR(CURDATE())
                                 AND o.status = 'Approved'
                            THEN o.total_amount ELSE 0 END
                        ), 0) AS may_sales,

                        COALESCE(SUM(
                            CASE WHEN MONTH(o.order_date) = 6
                                 AND YEAR(o.order_date) = YEAR(CURDATE())
                                 AND o.status = 'Approved'
                            THEN o.total_amount ELSE 0 END
                        ), 0) AS june_sales,

                        COALESCE(SUM(
                            CASE WHEN MONTH(o.order_date) IN (4,5,6)
                                 AND YEAR(o.order_date) = YEAR(CURDATE())
                                 AND o.status = 'Approved'
                            THEN o.total_amount ELSE 0 END
                        ), 0) AS total_sales

                    FROM users u
                    INNER JOIN user_profiles up ON up.user_id = u.user_id
                    LEFT JOIN orders_table o    ON o.user_id  = u.user_id
                    WHERE u.role = 'Dealer'
                    GROUP BY u.user_id, up.first_name, up.last_name
                    ORDER BY total_sales DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                // Add computed columns
                dt.Columns.Add("computed_rank", typeof(string));
                dt.Columns.Add("next_target", typeof(decimal));

                foreach (DataRow r in dt.Rows)
                {
                    decimal ts = Convert.ToDecimal(r["total_sales"]);
                    r["computed_rank"] = GetRank(ts);
                    r["next_target"] = GetNextTarget(ts);
                }

                gvSales.DataSource = dt;
                gvSales.DataBind();

                // Update users.rank and users.total_sales
                UpdateUserRanks(con);
            }
        }

        // ── Compute rank from total suki points ──
        private string GetRank(decimal points)
        {
            if (points >= DIAMOND_MIN) return "Diamond";
            if (points >= PLATINUM_MIN) return "Platinum";
            if (points >= GOLD_MIN) return "Gold";
            if (points >= SILVER_MIN) return "Silver";
            if (points >= BRONZE_MIN) return "Bronze";
            return "New";
        }

        // ── Next threshold to reach ──
        private decimal GetNextTarget(decimal points)
        {
            if (points >= DIAMOND_MIN) return DIAMOND_MIN;   // already max
            if (points >= PLATINUM_MIN) return DIAMOND_MIN;
            if (points >= GOLD_MIN) return PLATINUM_MIN;
            if (points >= SILVER_MIN) return GOLD_MIN;
            if (points >= BRONZE_MIN) return SILVER_MIN;
            return BRONZE_MIN;
        }

        // ── Auto-update users.rank and users.total_sales ──
        private void UpdateUserRanks(MySqlConnection con)
        {
            string updateSql = @"
                UPDATE users u
                SET
                    u.total_sales = (
                        SELECT COALESCE(SUM(o.total_amount), 0)
                        FROM orders_table o
                        WHERE o.user_id = u.user_id
                          AND o.status = 'Approved'
                          AND MONTH(o.order_date) IN (4,5,6)
                          AND YEAR(o.order_date) = YEAR(CURDATE())
                    ),
                    u.rank = (
                        SELECT
                            CASE
                                WHEN COALESCE(SUM(o.total_amount),0) >= 75000  THEN 'Diamond'
                                WHEN COALESCE(SUM(o.total_amount),0) >= 26250  THEN 'Platinum'
                                WHEN COALESCE(SUM(o.total_amount),0) >= 15000  THEN 'Gold'
                                WHEN COALESCE(SUM(o.total_amount),0) >= 3700   THEN 'Silver'
                                WHEN COALESCE(SUM(o.total_amount),0) >= 1600   THEN 'Bronze'
                                ELSE 'New'
                            END
                        FROM orders_table o
                        WHERE o.user_id = u.user_id
                          AND o.status = 'Approved'
                          AND MONTH(o.order_date) IN (4,5,6)
                          AND YEAR(o.order_date) = YEAR(CURDATE())
                    )
                WHERE u.role = 'Dealer'";

            new MySqlCommand(updateSql, con).ExecuteNonQuery();
        }

        // ── RowDataBound ──
        protected void gvSales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;
                decimal total = Convert.ToDecimal(row["total_sales"]);
                decimal nextTgt = Convert.ToDecimal(row["next_target"]);
                string rank = row["computed_rank"].ToString();

                // Accumulate footer totals
                _aprilTotal += Convert.ToDecimal(row["april_sales"]);
                _mayTotal += Convert.ToDecimal(row["may_sales"]);
                _juneTotal += Convert.ToDecimal(row["june_sales"]);
                _grandTotal += total;

                // Count per rank
                switch (rank)
                {
                    case "Diamond": _diamondCt++; break;
                    case "Platinum": _platinumCt++; break;
                    case "Gold": _goldCt++; break;
                    case "Silver": _silverCt++; break;
                    case "Bronze": _bronzeCt++; break;
                }

                // Silver gap
                SetGapLabel(e.Row, "lblSilverGap", SILVER_MIN, total);
                SetGapLabel(e.Row, "lblGoldGap", GOLD_MIN, total);
                SetGapLabel(e.Row, "lblPlatinumGap", PLATINUM_MIN, total);
                SetGapLabel(e.Row, "lblDiamondGap", DIAMOND_MIN, total);

                // Remark
                Label lblRemark = (Label)e.Row.FindControl("lblRemark");
                bool isGoal = total >= nextTgt;
                lblRemark.Text = isGoal ? "✔ GOAL" : "PENDING";
                lblRemark.CssClass = isGoal ? "goal" : "pending";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Footer totals
                ((Label)e.Row.FindControl("lblFootApril")).Text = FormatPeso(_aprilTotal);
                ((Label)e.Row.FindControl("lblFootMay")).Text = FormatPeso(_mayTotal);
                ((Label)e.Row.FindControl("lblFootJune")).Text = FormatPeso(_juneTotal);
                ((Label)e.Row.FindControl("lblFootTotal")).Text = $"<strong>{FormatPeso(_grandTotal)}</strong>";

                // Header stat labels
                lblTotalDealers.Text = gvSales.Rows.Count.ToString();
                lblGrandTotal.Text = FormatPeso(_grandTotal);
                lblDiamondCount.Text = _diamondCt.ToString();
                lblPlatinumCount.Text = _platinumCt.ToString();
                lblGoldCount.Text = _goldCt.ToString();
                lblSilverCount.Text = _silverCt.ToString();
                lblBronzeCount.Text = _bronzeCt.ToString();
            }
        }

        // ── Helper: set gap label ──
        private void SetGapLabel(GridViewRow row, string controlId, decimal threshold, decimal total)
        {
            Label lbl = (Label)row.FindControl(controlId);
            decimal gap = threshold - total;
            if (gap <= 0)
            {
                lbl.Text = "✔ Done";
                lbl.CssClass = "gap-done";
            }
            else
            {
                lbl.Text = $"₱{gap:N2} to go";
                lbl.CssClass = "gap-need";
            }
        }

        // ── Helper: format peso ──
        protected string FormatPeso(object val)
        {
            if (val == null || val == DBNull.Value) return "–";
            decimal d = Convert.ToDecimal(val);
            if (d == 0) return "–";
            return $"₱{d:N2}";
        }
    }
}