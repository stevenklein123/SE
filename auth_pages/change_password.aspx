<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change_password.aspx.cs" Inherits="Project_Tracking.auth_pages.change_password" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Change Password | AVON Dealer Portal</title>

    <link href="<%= ResolveUrl("~/assets/style/change_password.css") %>" rel="stylesheet" />
    <link href="../assets/script/register.css" rel="stylesheet"/>
</head>

<body>

<form id="form1" runat="server">

<div class="auth-container">

    <div class="auth-header">
        <h1>Change Password</h1>
        <p>Update your account security</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="error-message"></asp:Label>

    <div class="form-group">
        <label>Current Password</label>

        <div class="password-wrap">
            <asp:TextBox ID="txtCurrent" runat="server"
                TextMode="Password"
                CssClass="form-input" />

            <span class="passShow toggle-pass"
                  data-target="<%= txtCurrent.ClientID %>">
                Show
            </span>
        </div>
    </div>

    <!-- NEW PASSWORD -->
    <div class="form-group">
        <label>New Password</label>

        <div class="password-wrap">
            <asp:TextBox ID="txtNew" runat="server"
                TextMode="Password"
                CssClass="form-input" />

            <span class="passShow toggle-pass"
                  data-target="<%= txtNew.ClientID %>">
                Show
            </span>
        </div>
    </div>

    <div class="form-group">
        <label>Confirm New Password</label>

        <div class="password-wrap">
            <asp:TextBox ID="txtConfirm" runat="server"
                TextMode="Password"
                CssClass="form-input" />

            <span class="passShow toggle-pass"
                  data-target="<%= txtConfirm.ClientID %>">
                Show
            </span>
        </div>
    </div>

    <asp:Button ID="btnChange"
        runat="server"
        Text="Change Password"
        CssClass="btn-register"
        OnClick="BtnChange_Click"
        OnClientClick="return validateChange();" />

    <div class="auth-footer">
        <a href="../dealers/dashboard.aspx">Cancel</a>
    </div>

</div>

</form>

<script src="<%= ResolveUrl("../assets/script/global-password-toggle.js") %>"></script>
<script src="<%= ResolveUrl("../assets/script/change_password.js") %>"></script>

</body>
</html>
