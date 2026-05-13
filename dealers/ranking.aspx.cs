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
    public partial class ranking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string query = "SELECT username, email, total_sales, rank FROM users ORDER BY total_sales DESC";

                MySqlCommand cmd = new MySqlCommand(query, con);

                rptUsers.DataSource = cmd.ExecuteReader();
                rptUsers.DataBind();
            }
        }
    }
}