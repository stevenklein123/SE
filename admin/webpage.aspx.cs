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
            {
                LoadProducts();

                // SUCCESS MESSAGE
                if (Session["msg"] != null)
                {
                    Response.Write("<script>alert('" + Session["msg"].ToString() + "');</script>");
                    Session.Remove("msg");
                }
            }
        }

        // LOAD PRODUCTS
        void LoadProducts()
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlDataAdapter da =
                    new MySqlDataAdapter(
                        "SELECT * FROM products ORDER BY product_id DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                lblTotalCount.Text = "Total Products: " + dt.Rows.Count;
            }
        }

        // CLEAR INPUTS
        void ClearFields()
        {
            // ADD PRODUCT
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
            txtDescription.Text = "";

            // UPDATE PRODUCT
            txtUpdateProductID.Text = "";
            txtUpdateName.Text = "";
            txtUpdatePrice.Text = "";
            txtUpdateStock.Text = "";
            txtUpdateDescription.Text = "";

            // DELETE PRODUCT
            txtProductID.Text = "";
        }

        // REFRESH BUTTON
        protected void btnViewProduct_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // SAVE PRODUCT
        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();

                    string img = "";

                    // IMAGE UPLOAD
                    if (fuProductImage.HasFile)
                    {
                        string ext =
                            Path.GetExtension(fuProductImage.FileName).ToLower();

                        string file =
                            Guid.NewGuid().ToString() + ext;

                        string folderPath =
                            Server.MapPath("~/uploads/");

                        // CREATE FOLDER
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        // SAVE IMAGE
                        fuProductImage.SaveAs(folderPath + file);

                        img = "/uploads/" + file;
                    }

                    // INSERT PRODUCT
                    MySqlCommand cmd = new MySqlCommand(
                        @"INSERT INTO products
                        (
                            product_name,
                            price,
                            stock,
                            description,
                            image_path
                        )
                        VALUES
                        (
                            @n,
                            @p,
                            @s,
                            @d,
                            @i
                        )", con);

                    cmd.Parameters.AddWithValue("@n", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@p", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@s", txtStock.Text);
                    cmd.Parameters.AddWithValue("@d", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@i", img);

                    cmd.ExecuteNonQuery();
                }

                // SUCCESS MESSAGE
                Session["msg"] = "Product Added Successfully!";

                // REDIRECT TO PREVENT DUPLICATE INSERT
                Response.Redirect("webpage.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<script>alert('Error: " +
                    ex.Message.Replace("'", "") +
                    "');</script>");
            }
        }

        // DELETE PRODUCT
        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();

                    MySqlCommand cmd =
                        new MySqlCommand(
                            "DELETE FROM products WHERE product_id=@id", con);

                    cmd.Parameters.AddWithValue("@id", txtProductID.Text);

                    cmd.ExecuteNonQuery();
                }

                Session["msg"] = "Product Deleted Successfully!";

                Response.Redirect("webpage.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<script>alert('Error: " +
                    ex.Message.Replace("'", "") +
                    "');</script>");
            }
        }

        // UPDATE PRODUCT
        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        @"UPDATE products
                        SET
                            product_name=@n,
                            price=@p,
                            stock=@s,
                            description=@d
                        WHERE product_id=@id", con);

                    cmd.Parameters.AddWithValue("@id", txtUpdateProductID.Text);
                    cmd.Parameters.AddWithValue("@n", txtUpdateName.Text);
                    cmd.Parameters.AddWithValue("@p", txtUpdatePrice.Text);
                    cmd.Parameters.AddWithValue("@s", txtUpdateStock.Text);
                    cmd.Parameters.AddWithValue("@d", txtUpdateDescription.Text);

                    cmd.ExecuteNonQuery();
                }

                Session["msg"] = "Product Updated Successfully!";

                Response.Redirect("webpage.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<script>alert('Error: " +
                    ex.Message.Replace("'", "") +
                    "');</script>");
            }
        }
    }
}