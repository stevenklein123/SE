<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Project_Tracking.auth_pages.register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Register | Avon Dealer Portal</title>

    <link rel="icon" type="image/png" href="assets/images/avon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=Inter:wght@300;400;600&display=swap" rel="stylesheet" />
    <link href="../assets/style/register.css" rel="stylesheet" />
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
</head>
<body>

<form id="form1" runat="server">

    <div class="auth-container">

        <div class="auth-header">
            <img src="../assets/images/avon.png" alt="AVON Logo" class="auth-logo-img" />
            <h1>Join Avon</h1>
            <p>Create Your Dealer Account</p>
        </div>

        <div id="errorMessage" class="error-message">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>

        <div class="form-group">
            <label for="username">Username</label>
            <input type="text" id="username" runat="server" class="form-input" placeholder="Choose username" />
        </div>

        <div class="form-group">
            <label for="email">Email Address</label>
            <input type="email" id="email" runat="server" class="form-input" placeholder="your@email.com" />
        </div>

        <div class="form-group">
            <label for="password">Password</label>
            <div class="password-wrap">
                <input type="password"
                       id="password"
                       runat="server"
                       class="form-input"
                       placeholder="Create password" />
                <span class="passShow"
                      onclick="showPassword('password', this)">
                      Show
                </span>
            </div>
        </div>

        <div class="form-group">
            <label for="confirmPassword">Confirm Password</label>
            <div class="password-wrap">
                <input type="password"
                       id="confirmPassword"
                       runat="server"
                       class="form-input"
                       placeholder="Confirm password" />
                <span class="passShow"
                      onclick="showPassword('confirmPassword', this)">
                      Show
                </span>
            </div>
        </div>

        <div class="password-requirements">
            <strong>Password Rules:</strong>
            <ul>
                <li>At least 8 characters</li>
                <li>Uppercase letter</li>
                <li>Lowercase letter</li>
                <li>Number</li>
                <li>Special character</li>
            </ul>
        </div>

        <asp:Button ID="btnRegister"
            runat="server"
            Text="Create Account"
            CssClass="btn-register"
            OnClick="BtnRegister_Click" />

        <div class="auth-footer">
            <p>Already have an account?
                <a href="login.aspx">Login here</a>
            </p>
        </div>

    </div>

</form>

<script src="../assets/script/register.js"></script>

</body>
</html>
