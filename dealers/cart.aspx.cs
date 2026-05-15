using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services;

namespace Project_Tracking.dealers
{
    public partial class cart : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e) {}
        string ConnStr => ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        // ================= GET CART =================
        [WebMethod]
        public static List<object> GetCart()
        {
            var list = new List<object>();

            string cs =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            int userId =
                Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();

                string sql = @"
                SELECT 
                    c.cart_id,
                    c.product_id,
                    p.product_name,
                    p.price,
                    c.quantity,
                    p.image_path
                FROM cart_items c
                JOIN products p
                    ON c.product_id = p.product_id
                WHERE c.user_id=@uid";

                MySqlCommand cmd =
                    new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@uid", userId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new
                        {
                            id = r["cart_id"],
                            productId = r["product_id"],
                            name = r["product_name"].ToString(),
                            price = Convert.ToDecimal(r["price"]),
                            quantity = Convert.ToInt32(r["quantity"]),
                            image = r["image_path"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        // ================= UPDATE QTY =================
        [WebMethod]
        public static void UpdateQty(int id, int qty)
        {
            int userId =
                Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            string connStr =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            using (MySqlConnection conn =
                new MySqlConnection(connStr))
            {
                conn.Open();

                if (qty <= 0)
                {
                    MySqlCommand del = new MySqlCommand(@"
                    DELETE FROM cart_items
                    WHERE cart_id=@id
                    AND user_id=@uid", conn);

                    del.Parameters.AddWithValue("@id", id);
                    del.Parameters.AddWithValue("@uid", userId);

                    del.ExecuteNonQuery();
                    return;
                }

                MySqlCommand cmd = new MySqlCommand(@"
                UPDATE cart_items
                SET quantity=@qty
                WHERE cart_id=@id
                AND user_id=@uid", conn);

                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@uid", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ================= CLEAR CART =================
        [WebMethod]
        public static void ClearCart()
        {
            int userId =
                Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            string connStr =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            using (MySqlConnection conn =
                new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                DELETE FROM cart_items
                WHERE user_id=@uid", conn);

                cmd.Parameters.AddWithValue("@uid", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ================= REMOVE ITEM =================
        [WebMethod]
        public static void RemoveItem(int id)
        {
            int userId =
                Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            string connStr =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            using (MySqlConnection conn =
                new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                DELETE FROM cart_items
                WHERE cart_id=@id
                AND user_id=@uid", conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@uid", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ================= GET USER EMAIL =================
        private static string GetUserEmail(int userId)
        {
            string cs =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            using (MySqlConnection conn =
                new MySqlConnection(cs))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(@"
                SELECT email
                FROM users
                WHERE user_id=@id
                LIMIT 1", conn);

                cmd.Parameters.AddWithValue("@id", userId);

                object result = cmd.ExecuteScalar();

                return result != null
                    ? result.ToString()
                    : "";
            }
        }

        // ================= CHECKOUT =================
        [WebMethod]
        public static object Checkout(
            string shipping,
            string paymentMethod)
        {
            int userId =
                Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            string cs =
                ConfigurationManager.ConnectionStrings["MyDbConn"]
                .ConnectionString;

            string email = "";

            using (MySqlConnection conn =
                new MySqlConnection(cs))
            {
                conn.Open();

                var items =
                    new List<(int pid, string name, decimal price, int qty)>();

                decimal total = 0;

                // ================= GET CART =================
                MySqlCommand get = new MySqlCommand(@"
                SELECT
                    c.product_id,
                    p.product_name,
                    p.price,
                    c.quantity,
                    p.stock
                FROM cart_items c
                JOIN products p
                    ON c.product_id = p.product_id
                WHERE c.user_id=@uid", conn);

                get.Parameters.AddWithValue("@uid", userId);

                using (var r = get.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int qty =
                            Convert.ToInt32(r["quantity"]);

                        decimal price =
                            Convert.ToDecimal(r["price"]);

                        int stock =
                            Convert.ToInt32(r["stock"]);

                        if (stock < qty)
                        {
                            return new
                            {
                                error =
                                "Insufficient stock for " +
                                r["product_name"]
                            };
                        }

                        items.Add((
                            Convert.ToInt32(r["product_id"]),
                            r["product_name"].ToString(),
                            price,
                            qty
                        ));

                        total += price * qty;
                    }
                }

                decimal shippingFee =
                    shipping == "Delivery" ? 50 : 0;

                total += shippingFee;

                // ================= INSERT ORDER =================
                MySqlCommand order = new MySqlCommand(@"
                INSERT INTO orders_table
                (
                    user_id,
                    order_date,
                    status,
                    total_amount,
                    item_count,
                    shipping_method
                )
                VALUES
                (
                    @uid,
                    NOW(),
                    'Pending',
                    @total,
                    @count,
                    @ship
                );

                SELECT LAST_INSERT_ID();", conn);

                order.Parameters.AddWithValue("@uid", userId);
                order.Parameters.AddWithValue("@total", total);
                order.Parameters.AddWithValue("@count", items.Count);
                order.Parameters.AddWithValue("@ship", shipping);

                long orderId =
                    Convert.ToInt64(order.ExecuteScalar());

                string referenceCode =
                    "TRX" +
                    DateTime.Now.ToString("yyyyMMddHHmmss") +
                    orderId;

                // ================= PAYMENT =================
                MySqlCommand pay = new MySqlCommand(@"
                INSERT INTO payments
                (
                    order_id,
                    payment_method,
                    amount,
                    payment_status,
                    reference_no
                )
                VALUES
                (
                    @oid,
                    @method,
                    @amount,
                    'Paid',
                    @ref
                )", conn);

                pay.Parameters.AddWithValue("@oid", orderId);
                pay.Parameters.AddWithValue("@method", paymentMethod);
                pay.Parameters.AddWithValue("@amount", total);
                pay.Parameters.AddWithValue("@ref", referenceCode);

                pay.ExecuteNonQuery();

                // ================= TRANSACTION =================
                MySqlCommand trx = new MySqlCommand(@"
                INSERT INTO transactions
                (
                    user_id,
                    order_id,
                    reference_code,
                    total_amount,
                    status,
                    created_at
                )
                VALUES
                (
                    @uid,
                    @oid,
                    @ref,
                    @total,
                    'Completed',
                    NOW()
                );

                SELECT LAST_INSERT_ID();", conn);

                trx.Parameters.AddWithValue("@uid", userId);
                trx.Parameters.AddWithValue("@oid", orderId);
                trx.Parameters.AddWithValue("@ref", referenceCode);
                trx.Parameters.AddWithValue("@total", total);

                long transactionId =

                    Convert.ToInt64(trx.ExecuteScalar());

                // ================= ITEMS + STOCK =================
                foreach (var i in items)
                {
                    MySqlCommand ins = new MySqlCommand(@"
                    INSERT INTO transaction_items
                    (
                        transaction_id,
                        product_name,
                        price,
                        quantity,
                        subtotal
                    )
                    VALUES
                    (
                        @tid,
                        @name,
                        @price,
                        @qty,
                        @sub
                    )", conn);

                    ins.Parameters.AddWithValue("@tid", transactionId);
                    ins.Parameters.AddWithValue("@name", i.name);
                    ins.Parameters.AddWithValue("@price", i.price);
                    ins.Parameters.AddWithValue("@qty", i.qty);
                    ins.Parameters.AddWithValue("@sub", i.price * i.qty);

                    ins.ExecuteNonQuery();

                }

                // ================= GET EMAIL =================
                email = GetUserEmail(userId);

                if (string.IsNullOrEmpty(email))
                {
                    return new
                    {
                        error = "User email not found."
                    };
                }

                // ================= CLEAR CART =================
                MySqlCommand clear = new MySqlCommand(@"
                DELETE FROM cart_items
                WHERE user_id=@uid", conn);

                clear.Parameters.AddWithValue("@uid", userId);

                clear.ExecuteNonQuery();

                // ================= SEND EMAIL =================
                try
                {
                    var smtp =
                        new SmtpClient("smtp.gmail.com");

                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                    smtp.Credentials =
                        new NetworkCredential(
                            "fernandezklein123@gmail.com",
                            "qcuzspmznkpwvxam"
                        );

                    smtp.DeliveryMethod =
                        SmtpDeliveryMethod.Network;

                    var mail = new MailMessage();

                    mail.From =
                        new MailAddress(
                            "fernandezklein123@gmail.com"
                        );

                    mail.To.Add(email);

                    mail.Subject =
                        "AVON Order Confirmation - " +
                        referenceCode;

                    mail.Body = $@"
                    Hi,

                    Your order has been placed successfully.

                    Reference Code: {referenceCode}
                    Payment Method: {paymentMethod}
                    Shipping Method: {shipping}
                    Total Amount: ₱{total}

                    Thank you for shopping with AVON.
                    ";

                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    return new
                    {
                        error = ex.Message
                    };
                }

                // ================= RETURN SUCCESS =================
                return new
                {
                    success = true,
                    orderId,
                    transactionId,
                    referenceCode,
                    total,
                    shipping,
                    paymentMethod,
                    email
                };
            }
        }
    }
}