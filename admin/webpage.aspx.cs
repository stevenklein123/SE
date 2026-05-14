using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;

namespace Project_Tracking.admin
{
    public partial class webpage : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadProducts();
        }

        void LoadProducts()
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM products", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnViewProduct_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                string img = "";

                if (fuProductImage.HasFile)
                {
                    string file = Guid.NewGuid() + Path.GetExtension(fuProductImage.FileName);
                    string path = Server.MapPath("~/uploads/");
                    fuProductImage.SaveAs(path + file);
                    img = "/uploads/" + file;
                }

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO products(product_name,price,stock,description,image_path) VALUES(@n,@p,@s,@d,@i)", con);

                cmd.Parameters.AddWithValue("@n", txtProductName.Text);
                cmd.Parameters.AddWithValue("@p", txtPrice.Text);
                cmd.Parameters.AddWithValue("@s", txtStock.Text);
                cmd.Parameters.AddWithValue("@d", txtDescription.Text);
                cmd.Parameters.AddWithValue("@i", img);

                cmd.ExecuteNonQuery();
                LoadProducts();
            }
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM products WHERE product_id=@id", con);
                cmd.Parameters.AddWithValue("@id", txtProductID.Text);
                cmd.ExecuteNonQuery();

                LoadProducts();
            }
        }

        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE products SET product_name=@n, price=@p, stock=@s, description=@d WHERE product_id=@id", con);

                cmd.Parameters.AddWithValue("@id", txtUpdateProductID.Text);
                cmd.Parameters.AddWithValue("@n", txtUpdateName.Text);
                cmd.Parameters.AddWithValue("@p", txtUpdatePrice.Text);
                cmd.Parameters.AddWithValue("@s", txtUpdateStock.Text);
                cmd.Parameters.AddWithValue("@d", txtUpdateDescription.Text);

                cmd.ExecuteNonQuery();
                LoadProducts();
            }
        }
    }
}