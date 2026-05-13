<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="webpage.aspx.cs"
    Inherits="Project_Tracking.admin.webpage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AVON INVENTORY</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="../assets/style/global_desktop.css" />
    <link rel="stylesheet" href="../assets/style/global_mobile.css" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        body {
            background: #f5f5f5;
            overflow-x: hidden;
        }

        .navbar-custom {
            background-color: #e4004b;
            color: white;
        }

        .sidebar {
            position: fixed;
            top: 0;
            left: -300px;
            width: 260px;
            height: 100%;
            background: #e4004b;
            color: white;
            transition: 0.3s;
            z-index: 1050;
            padding: 20px;
        }

        .sidebar.active {
            left: 0;
        }

        .overlay {
            position: fixed;
            inset: 0;
            background: rgba(0,0,0,0.2);
            backdrop-filter: blur(6px);
            opacity: 0;
            visibility: hidden;
        }

        .overlay.active {
            opacity: 1;
            visibility: visible;
        }

        .content {
            padding: 20px;
        }

        .card-custom {
            border-radius: 15px;
            box-shadow: 0 3px 10px rgba(0,0,0,0.1);
        }

        .slide-panel {
            position: fixed;
            top: 0;
            right: -400px;
            width: 350px;
            height: 100%;
            background: white;
            box-shadow: -3px 0 10px rgba(0,0,0,0.2);
            transition: 0.3s;
            z-index: 1100;
            padding: 20px;
        }

        .slide-panel.active {
            right: 0;
        }

        .panel-content {
            overflow-y: auto;
            height: 100%;
        }

        .btn-sm-custom {
            font-size: 12px;
            padding: 6px 10px;
        }
    </style>
</head>

<body>

<form id="form1" runat="server">

<div class="overlay" id="overlay" onclick="toggleSidebar()"></div>

<div class="sidebar" id="sidebar">
    <h5 onclick="toggleSidebar()">MENU</h5>
</div>

<nav class="navbar navbar-custom px-3">
    <button type="button" class="btn text-white" onclick="toggleSidebar()">
        <i class="bi bi-list fs-3"></i>
    </button>

    <span class="mx-auto fw-bold fs-4">AVON</span>
</nav>

<div class="content container-fluid">

<div class="card card-custom p-3 mb-3">

    <h6 class="mb-3">Inventory Management</h6>

    <div class="d-flex flex-wrap gap-2">

        <asp:Button ID="btnAddProduct"
            runat="server"
            Text="＋ Add"
            CssClass="btn btn-danger btn-sm-custom"
            OnClientClick="openAddProductPanel(); return false;" />

        <asp:Button ID="btnViewProduct"
            runat="server"
            Text="👁 View"
            CssClass="btn btn-dark btn-sm-custom"
            OnClick="btnViewProduct_Click" />

        <asp:Button ID="btnUpdateProduct"
            runat="server"
            Text="✏ Update"
            CssClass="btn btn-secondary btn-sm-custom"
            OnClientClick="openUpdateProductPanel(); return false;" />

        <asp:Button ID="btnDeleteProduct"
            runat="server"
            Text="🗑 Delete"
            CssClass="btn btn-outline-danger btn-sm-custom"
            OnClientClick="openDeleteProductPanel(); return false;" />

    </div>
</div>

<!-- ADD PANEL -->
<div id="addProductPanel" class="slide-panel">

    <div class="panel-content">

        <h5>Add Product</h5>

        <asp:TextBox ID="txtProductName"
            runat="server"
            CssClass="form-control"
            placeholder="Product Name"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtPrice"
            runat="server"
            CssClass="form-control"
            placeholder="Price"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtStock"
            runat="server"
            CssClass="form-control"
            placeholder="Stock"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtDescription"
            runat="server"
            CssClass="form-control"
            TextMode="MultiLine"
            placeholder="Description"></asp:TextBox>
        <br />

        <asp:FileUpload ID="fuProductImage"
            runat="server"
            CssClass="form-control" />
        <br />

        <asp:Button ID="btnSaveProduct"
            runat="server"
            Text="Save"
            CssClass="btn btn-danger w-100"
            OnClick="btnSaveProduct_Click" />

    </div>
</div>

<!-- DELETE PANEL -->
<div id="deleteProductPanel" class="slide-panel">

    <div class="panel-content">

        <h5>Delete Product</h5>

        <asp:TextBox ID="txtProductID"
            runat="server"
            CssClass="form-control"
            placeholder="Product ID"></asp:TextBox>
        <br />

        <asp:Button ID="btnDeleteProductSubmit"
            runat="server"
            Text="Delete"
            CssClass="btn btn-danger w-100"
            OnClick="btnDeleteProduct_Click" />

    </div>
</div>

<!-- UPDATE PANEL -->
<div id="updateProductPanel" class="slide-panel">

    <div class="panel-content">

        <h5>Update Product</h5>

        <asp:TextBox ID="txtUpdateProductID"
            runat="server"
            CssClass="form-control"
            placeholder="Product ID"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtUpdateName"
            runat="server"
            CssClass="form-control"
            placeholder="Product Name"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtUpdatePrice"
            runat="server"
            CssClass="form-control"
            placeholder="Price"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtUpdateStock"
            runat="server"
            CssClass="form-control"
            placeholder="Stock"></asp:TextBox>
        <br />

        <asp:TextBox ID="txtUpdateDescription"
            runat="server"
            CssClass="form-control"
            TextMode="MultiLine"
            placeholder="Description"></asp:TextBox>
        <br />

        <asp:Button ID="btnSaveUpdate"
            runat="server"
            Text="Update"
            CssClass="btn btn-secondary w-100"
            OnClick="btnSaveUpdate_Click" />

    </div>
</div>

<!-- GRIDVIEW -->
<asp:GridView ID="GridView1"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-bordered table-striped text-center">

    <Columns>

        <asp:BoundField DataField="product_id" HeaderText="ID" />
        <asp:BoundField DataField="product_name" HeaderText="Name" />
        <asp:BoundField DataField="price" HeaderText="Price" />
        <asp:BoundField DataField="stock" HeaderText="Stock" />
        <asp:BoundField DataField="description" HeaderText="Description" />

        <asp:TemplateField HeaderText="Image">
            <ItemTemplate>

                <img src='<%# Eval("image_path") %>'
                    style="width:60px;height:60px;object-fit:cover;border-radius:8px;" />

            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

</div>

</form>

<script>

    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }

    function openAddProductPanel() {
        document.getElementById("addProductPanel").classList.add("active");
    }

    function openDeleteProductPanel() {
        document.getElementById("deleteProductPanel").classList.add("active");
    }

    function openUpdateProductPanel() {
        document.getElementById("updateProductPanel").classList.add("active");
    }

</script>

</body>
</html>