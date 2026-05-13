using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace Project_Tracking.dealers
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
                lblWelcome.Text = "Welcome, " + Session["Username"].ToString();
                LoadAvonProducts();
            }

        }
        private void LoadAvonProducts()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT product_id, product_name, price, stock, image_path
                    FROM products
                    ORDER BY product_id DESC", conn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptProducts.DataSource = dt;
                rptProducts.DataBind();
            }
        }

        // ================= ADD TO CART FIXED =================
        [WebMethod]
        public static string AddToCart(int productId, int quantity)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                    return "Error: Not logged in";

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    // Check product stock first
                    MySqlCommand stockCheck = new MySqlCommand(@"
                        SELECT stock FROM products WHERE product_id=@pid", conn);
                    stockCheck.Parameters.AddWithValue("@pid", productId);

                    object stockResult = stockCheck.ExecuteScalar();
                    if (stockResult == null)
                        return "Error: Product not found";

                    int stock = Convert.ToInt32(stockResult);

                    if (stock < quantity)
                        return "Error: Not enough stock. Available: " + stock;

                    // Check if item already in cart
                    MySqlCommand check = new MySqlCommand(@"
                        SELECT quantity FROM cart_items 
                        WHERE user_id=@uid AND product_id=@pid", conn);
                    check.Parameters.AddWithValue("@uid", userId);
                    check.Parameters.AddWithValue("@pid", productId);

                    object cartResult = check.ExecuteScalar();

                    if (cartResult != null)
                    {
                        // Update quantity
                        MySqlCommand update = new MySqlCommand(@"
                            UPDATE cart_items
                            SET quantity = quantity + @qty
                            WHERE user_id=@uid AND product_id=@pid", conn);
                        update.Parameters.AddWithValue("@qty", quantity);
                        update.Parameters.AddWithValue("@uid", userId);
                        update.Parameters.AddWithValue("@pid", productId);
                        update.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new cart item
                        MySqlCommand insert = new MySqlCommand(@"
                            INSERT INTO cart_items (user_id, product_id, price, quantity, created_at)
                            SELECT @uid, product_id, price, @qty, NOW()
                            FROM products WHERE product_id=@pid", conn);
                        insert.Parameters.AddWithValue("@uid", userId);
                        insert.Parameters.AddWithValue("@pid", productId);
                        insert.Parameters.AddWithValue("@qty", quantity);
                        insert.ExecuteNonQuery();
                    }

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public static int GetCartCount()
        {
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT IFNULL(SUM(quantity),0)
                    FROM cart_items
                    WHERE user_id=@uid", conn);

                cmd.Parameters.AddWithValue("@uid", userId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ================= LOW STOCK WARNINGS =================
        [WebMethod]
        public static List<object> GetLowStockWarnings()
        {
            var warnings = new List<object>();
            string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT product_id, product_name, stock
                    FROM products
                    WHERE stock <= 5
                    ORDER BY stock ASC", conn);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        warnings.Add(new
                        {
                            productId = r["product_id"],
                            name = r["product_name"].ToString(),
                            stock = Convert.ToInt32(r["stock"])
                        });
                    }
                }
            }

            return warnings;
        }
    }
}