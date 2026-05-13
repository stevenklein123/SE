using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Tracking.dealers
{
    public partial class transaction : System.Web.UI.Page
    {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/auth_pages/login.aspx");
                    return;
                }
            }

            [WebMethod(EnableSession = true)]
            public static string GetReceiptHtml(int id)
            {
                if (HttpContext.Current.Session["UserId"] == null)
                    return "";

                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
                string html = "";

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    // get transaction header and join orders (transactions now contains order_id)
                    MySqlCommand hdr = new MySqlCommand(@"
                    SELECT t.transaction_id, t.reference_code, t.total_amount, t.status as transaction_status, t.created_at,
                           o.shipping_method as order_shipping, o.status as order_status
                    FROM transactions t
                    LEFT JOIN orders_table o ON o.order_id = t.order_id
                    WHERE t.transaction_id = @id", conn);

                    hdr.Parameters.AddWithValue("@id", id);

                    string refcode = "";
                    decimal total = 0;
                    string status = "";
                    DateTime created = DateTime.Now;
                    string shipping = "";

                    // Read header (joined with orders) and prefer order-level fields when available
                    using (var r = hdr.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            refcode = r["reference_code"]?.ToString() ?? "";
                            total = r["total_amount"] != DBNull.Value ? Convert.ToDecimal(r["total_amount"]) : 0;
                            var oStatus = r["order_status"] != DBNull.Value ? r["order_status"]?.ToString() : null;
                            status = !string.IsNullOrEmpty(oStatus) ? oStatus : (r["transaction_status"]?.ToString() ?? string.Empty);
                            created = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now;
                            shipping = r["order_shipping"]?.ToString() ?? string.Empty;
                        }
                    }
                    // get items
                    MySqlCommand items = new MySqlCommand(@"
                    SELECT product_name, price, quantity, subtotal
                    FROM transaction_items
                    WHERE transaction_id = @id", conn);

                    items.Parameters.AddWithValue("@id", id);

                    var rows = new List<string>();

                    using (var r = items.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            string name = r["product_name"].ToString();
                            decimal price = r["price"] != DBNull.Value ? Convert.ToDecimal(r["price"]) : 0;
                            int qty = r["quantity"] != DBNull.Value ? Convert.ToInt32(r["quantity"]) : 0;
                            decimal sub = r["subtotal"] != DBNull.Value ? Convert.ToDecimal(r["subtotal"]) : 0;

                            rows.Add($"<tr><td>{HttpUtility.HtmlEncode(name)}</td><td>{qty}</td><td>₱{price}</td><td>₱{sub}</td></tr>");
                        }
                    }

                    // determine shipping fee (business rule: Delivery +₱50, Pickup free)
                    decimal shippingFee = 0m;
                    if (!string.IsNullOrEmpty(shipping) && shipping.Equals("Delivery", StringComparison.OrdinalIgnoreCase)) shippingFee = 50m;

                    decimal grandTotal = total + shippingFee;

                    // build HTML using StringBuilder to avoid complex verbatim interpolation
                    var sb = new StringBuilder();
                    sb.AppendLine("<html>");
                    sb.AppendLine("<head>");
                    sb.AppendLine("<meta charset='utf-8' />");
                    sb.AppendLine("<title>Receipt " + HttpUtility.HtmlEncode(refcode) + "</title>");
                    sb.AppendLine("<style>");
                    sb.AppendLine("body{font-family: Arial, sans-serif;}");
                    sb.AppendLine("table{width:100%;border-collapse:collapse}");
                    sb.AppendLine("th,td{border:1px solid #ddd;padding:8px}");
                    sb.AppendLine("th{background:#f4f4f4}");
                    sb.AppendLine("</style>");
                    sb.AppendLine("</head>");
                    sb.AppendLine("<body>");
                    sb.AppendLine("<h2>Receipt</h2>");
                    sb.AppendLine("<p><strong>Reference:</strong> " + HttpUtility.HtmlEncode(refcode) + "</p>");
                    sb.AppendLine("<p><strong>Date:</strong> " + created.ToString("yyyy-MM-dd HH:mm") + "</p>");
                    sb.AppendLine("<p><strong>Status:</strong> " + HttpUtility.HtmlEncode(status) + "</p>");
                    sb.AppendLine("<p><strong>Shipping:</strong> " + HttpUtility.HtmlEncode(shipping) + "</p>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<thead><tr><th>Product</th><th>Qty</th><th>Price</th><th>Subtotal</th></tr></thead>");
                    sb.AppendLine("<tbody>");
                    sb.AppendLine(string.Join("", rows));
                    sb.AppendLine("</tbody>");
                    sb.AppendLine("</table>");
                    sb.AppendLine("<div style=\"margin-top:12px;text-align:right\"> ");
                    sb.AppendLine("<div>Items total: <strong>₱" + total.ToString("F2") + "</strong></div>");
                    sb.AppendLine("<div>Shipping fee: <strong>₱" + shippingFee.ToString("F2") + "</strong></div>");
                    sb.AppendLine("<h3 style=\"margin:6px 0 0\">Grand total: ₱" + grandTotal.ToString("F2") + "</h3>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</body>");
                    sb.AppendLine("</html>");

                    html = sb.ToString();
                }

                return html;
            }

            [WebMethod(EnableSession = true)]
            public static List<object> GetTransactions()
            {
                var list = new List<object>();

                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                    // join orders to include order-level status/shipping when available
                    MySqlCommand cmd = new MySqlCommand(@"
                    SELECT t.transaction_id, t.reference_code, t.total_amount, t.status as transaction_status, t.created_at,
                           o.status as order_status, o.shipping_method as order_shipping
                    FROM transactions t
                    LEFT JOIN orders_table o ON o.order_id = t.order_id
                    WHERE t.user_id=@userId
                    ORDER BY t.created_at DESC", conn);

                    cmd.Parameters.AddWithValue("@userId", userId);

                    // read transactions and use joined order fields when available
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int tid = Convert.ToInt32(r["transaction_id"]);
                            string refc = r["reference_code"]?.ToString() ?? string.Empty;
                            decimal tot = r["total_amount"] != DBNull.Value ? Convert.ToDecimal(r["total_amount"]) : 0;
                            string txnStatus = r["transaction_status"]?.ToString() ?? string.Empty;
                            string orderStatus = r["order_status"]?.ToString() ?? string.Empty;
                            string orderShipping = r["order_shipping"]?.ToString() ?? string.Empty;
                            DateTime created = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.MinValue;

                            string finalStatus = !string.IsNullOrEmpty(orderStatus) ? orderStatus : txnStatus;

                            list.Add(new
                            {
                                id = tid,
                                refcode = refc,
                                total = tot,
                                status = finalStatus,
                                date = created == DateTime.MinValue ? string.Empty : created.ToString("MMM dd yyyy"),
                                shipping = orderShipping
                            });
                        }
                    }
                }

                return list;
            }

            [WebMethod(EnableSession = true)]
            public static object GetReceiptDetails(int id)
            {
                if (HttpContext.Current.Session["UserId"] == null) return new { error = "Not authenticated" };

                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    // header (transaction + order)
                    var headerCmd = new MySqlCommand(@"
                    SELECT t.transaction_id, t.reference_code, t.total_amount, t.status as transaction_status, t.created_at, t.payment_method,
                           o.shipping_method as order_shipping, o.status as order_status, o.total_amount as order_total
                    FROM transactions t
                    LEFT JOIN orders_table o ON o.order_id = t.order_id
                    WHERE t.transaction_id = @id LIMIT 1", conn);
                    headerCmd.Parameters.AddWithValue("@id", id);

                    object header = null;
                    using (var r = headerCmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var refcode = r["reference_code"]?.ToString() ?? string.Empty;
                            var txnTotal = r["total_amount"] != DBNull.Value ? Convert.ToDecimal(r["total_amount"]) : 0m;
                            var txnStatus = r["transaction_status"]?.ToString() ?? string.Empty;
                            var created = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.MinValue;
                            var payment = r["payment_method"]?.ToString() ?? string.Empty;
                            var ship = r["order_shipping"]?.ToString() ?? string.Empty;
                            header = new
                            {
                                transactionId = Convert.ToInt32(r["transaction_id"]),
                                reference = refcode,
                                total = txnTotal,
                                status = !string.IsNullOrEmpty(r["order_status"]?.ToString()) ? r["order_status"].ToString() : txnStatus,
                                date = created == DateTime.MinValue ? "" : created.ToString("yyyy-MM-dd HH:mm"),
                                paymentMethod = payment,
                                shippingMethod = ship
                            };
                        }
                    }

                    // items
                    var items = new List<object>();
                    var itemCmd = new MySqlCommand(@"
                    SELECT product_name, price, quantity, subtotal
                    FROM transaction_items
                    WHERE transaction_id=@id", conn);
                    itemCmd.Parameters.AddWithValue("@id", id);
                    using (var r = itemCmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            items.Add(new
                            {
                                name = r["product_name"]?.ToString() ?? string.Empty,
                                price = r["price"],
                                qty = r["quantity"],
                                subtotal = r["subtotal"]
                            });
                        }
                    }

                    return new { header = header, items = items };
                }
            }

            [WebMethod(EnableSession = true)]
            public static List<object> GetReceipt(int id)
            {
                var list = new List<object>();

                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                    SELECT * FROM transaction_items
                    WHERE transaction_id=@id", conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new
                            {
                                name = r["product_name"].ToString(),
                                price = r["price"],
                                qty = r["quantity"],
                                subtotal = r["subtotal"]
                            });
                        }
                    }
                }

                return list;
            }

            [WebMethod(EnableSession = true)]
            public static void SendMessage(string message)
            {
                string cs = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                    MySqlCommand cmd = new MySqlCommand(@"
                    INSERT INTO messages (user_id, message)
                    VALUES (@userId,@msg)", conn);

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@msg", message);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }