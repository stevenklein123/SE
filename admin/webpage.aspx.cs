using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.admin
{
    public partial class webpage : Page
    {
        string connString = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        void LoadProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = @"SELECT 
                                product_id,
                                product_name,
                                CONCAT('₱', price) AS price,
                                stock,
                                description,
                                image_path,
                                created_at
                                FROM products";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            string desc = txtDescription.Text.Trim();

            double price;
            int stock;

            if (!double.TryParse(txtPrice.Text, out price))
            {
                Response.Write("<script>alert('Invalid Price');</script>");
                return;
            }

            if (!int.TryParse(txtStock.Text, out stock))
            {
                Response.Write("<script>alert('Invalid Stock');</script>");
                return;
            }

            // IMAGE UPLOAD
            string imagePath = "";

            if (fuProductImage.HasFile)
            {
                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(fuProductImage.FileName);

                string folderPath = Server.MapPath("~/uploads/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                fuProductImage.SaveAs(Path.Combine(folderPath, fileName));

                imagePath = "/uploads/" + fileName;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = @"INSERT INTO products
                                (product_name, price, stock, description, image_path)
                                VALUES
                                (@name, @price, @stock, @desc, @img)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@img", imagePath);

                    try
                    {
                        cmd.ExecuteNonQuery();

                        Response.Write("<script>alert('Product Added Successfully!');</script>");

                        txtProductName.Text = "";
                        txtPrice.Text = "";
                        txtStock.Text = "";
                        txtDescription.Text = "";

                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" +
                            ex.Message.Replace("'", "") +
                            "');</script>");
                    }
                }
            }
        }

        protected void btnViewProduct_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(txtProductID.Text, out id))
            {
                Response.Write("<script>alert('Invalid Product ID');</script>");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = "DELETE FROM products WHERE product_id=@id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Response.Write("<script>alert('Product Deleted Successfully!');</script>");

                        txtProductID.Text = "";

                        LoadProducts();
                    }
                    else
                    {
                        Response.Write("<script>alert('No Product Found');</script>");
                    }
                }
            }
        }

        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(txtUpdateProductID.Text, out id))
            {
                Response.Write("<script>alert('Invalid Product ID');</script>");
                return;
            }

            string name = txtUpdateName.Text.Trim();
            string priceText = txtUpdatePrice.Text.Trim();
            string stockText = txtUpdateStock.Text.Trim();
            string desc = txtUpdateDescription.Text.Trim();

            string setQuery = "";

            if (!string.IsNullOrEmpty(name))
                setQuery += "product_name=@name, ";

            if (!string.IsNullOrEmpty(priceText))
                setQuery += "price=@price, ";

            if (!string.IsNullOrEmpty(stockText))
                setQuery += "stock=@stock, ";

            if (!string.IsNullOrEmpty(desc))
                setQuery += "description=@desc, ";

            if (string.IsNullOrEmpty(setQuery))
            {
                Response.Write("<script>alert('Fill at least one field');</script>");
                return;
            }

            setQuery = setQuery.TrimEnd(',', ' ');

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = "UPDATE products SET " +
                               setQuery +
                               " WHERE product_id=@id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@name", name);

                    if (!string.IsNullOrEmpty(priceText))
                        cmd.Parameters.AddWithValue("@price", Convert.ToDouble(priceText));

                    if (!string.IsNullOrEmpty(stockText))
                        cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(stockText));

                    if (!string.IsNullOrEmpty(desc))
                        cmd.Parameters.AddWithValue("@desc", desc);

                    try
                    {
                        cmd.ExecuteNonQuery();

                        Response.Write("<script>alert('Updated Successfully!');</script>");

                        txtUpdateProductID.Text = "";
                        txtUpdateName.Text = "";
                        txtUpdatePrice.Text = "";
                        txtUpdateStock.Text = "";
                        txtUpdateDescription.Text = "";

                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" +
                            ex.Message.Replace("'", "") +
                            "');</script>");
                    }
                }
            }
        }
    }
}