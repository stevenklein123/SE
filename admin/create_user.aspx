d<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="create_user.aspx.cs" Inherits="Project_Tracking.admin.create_user" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Create User</title>
</head>
<body>
    <form id="form1" runat="server">

        <h2>Create New User</h2>

        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        <br /><br />

        Username:
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
        <br /><br />

        Password:
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br /><br />

        Role:
        <asp:DropDownList ID="ddlRole" runat="server">
            <asp:ListItem Text="Admin" Value="admin"></asp:ListItem>
            <asp:ListItem Text="Dealer" Value="dealer"></asp:ListItem>
            <asp:ListItem Text="User" Value="user"></asp:ListItem>
        </asp:DropDownList>

        <br /><br />

        <asp:Button ID="btnCreate" runat="server" Text="Create User" OnClick="btnCreate_Click" />

    </form>
</body>
</html>
