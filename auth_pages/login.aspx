<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Project_Tracking.auth_pages.login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Log in | AVON Dealer Portal</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=Inter:wght@300;400;600&display=swap" rel="stylesheet">
    <link href="../assets/style/login.css" rel="stylesheet" />
</head>
<body>

<div class="auth-container">

    <div class="auth-header">
        <img src="../assets/images/avon.png" class="auth-logo-img" />
        <h1>Log in Page</h1>
        <p>Welcome back! Please login to your account.</p>
    </div>

    <form id="form1" runat="server">

        <div class="error-message">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>

        <div class="form-group">
            <label>Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input"></asp:TextBox>
        </div>

            <div class="form-group">
                <label>Password</label>

                <div class="password-wrap">
                    <asp:TextBox ID="txtPassword" runat="server"
                        ClientIDMode="Static"
                        TextMode="Password"
                        CssClass="form-input" />
                    <span class="passShow toggle-pass"
                          data-target="txtPassword">
                        Show
                    </span>
                </div>
            </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-login"
            OnClick="BtnLogin_Click" />
          
        <a class="regMess" href="personal.aspx">Register Here!</a>

    </form>

</div>

<script src="../assets/script/global-password-toggle.js"></script>
</body>
</html>