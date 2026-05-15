<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="personal_info.aspx.cs" Inherits="Project_Tracking.views.personal_info" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Personal Information</title>
    <link href="~/assets/style/dashboard.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../assets/style/personal_info.css" rel="stylesheet" />
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">

            <%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>
            <uc:Sidebar runat="server" ID="Sidebar1" />

            <main class="main-content">
                <h2>🧑‍💼 My Profile</h2>

                <div class="info-card">
                    <asp:Label ID="lblMsg" runat="server" ClientIDMode="Static" 
                    style="display:block; margin-bottom:8px;"></asp:Label>

                    <div class="info-row">
                                <div class="info-label">First Name</div>
                                <div class="info-value">
                                    <asp:Label ID="lblFirst" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanFirst"><%= lblFirst.Text %></span>
                                    <input id="inputFirst" class="edit-input" type="text" value="<%= lblFirst.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Middle Name</div>
                                <div class="info-value">
                                    <asp:Label ID="lblMiddle" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanMiddle"><%= lblMiddle.Text %></span>
                                    <input id="inputMiddle" class="edit-input" type="text" value="<%= lblMiddle.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Last Name</div>
                                <div class="info-value">
                                    <asp:Label ID="lblLast" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanLast"><%= lblLast.Text %></span>
                                    <input id="inputLast" class="edit-input" type="text" value="<%= lblLast.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Email</div>
                                <div class="info-value">
                                    <asp:Label ID="lblEmail" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanEmail"><%= lblEmail.Text %></span>
                                    <input id="inputEmail" class="edit-input" type="email" value="<%= lblEmail.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Birthday</div>
                                <div class="info-value">
                                    <asp:Label ID="lblBirthday" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanBirthday"><%= lblBirthday.Text %></span>
                                    <input id="inputBirthday" class="edit-input" type="date" value="<%= lblBirthday.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Contact Number</div>
                                <div class="info-value">
                                    <asp:Label ID="lblContact" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanContact"><%= lblContact.Text %></span>
                                    <input id="inputContact" class="edit-input" type="text" value="<%= lblContact.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Mobile Number</div>
                                <div class="info-value">
                                    <asp:Label ID="lblMobile" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanMobile"><%= lblMobile.Text %></span>
                                    <input id="inputMobile" class="edit-input" type="text" value="<%= lblMobile.Text %>" disabled />
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">Address</div>
                                <div class="info-value">
                                    <asp:Label ID="lblAddress" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanAddress"><%= lblAddress.Text %></span>
                                    <textarea id="inputAddress" class="edit-input" rows="2" disabled><%= lblAddress.Text %></textarea>
                                </div>
                            </div>

                            <div class="info-row">
                                <div class="info-label">ZIP Code</div>
                                <div class="info-value">
                                    <asp:Label ID="lblZip" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
                                    <span id="spanZip"><%= lblZip.Text %></span>
                                    <input id="inputZip" class="edit-input" type="text" value="<%= lblZip.Text %>" disabled />
                                </div>
                            </div>

                    <div style="margin-top:16px;text-align:right;">
                        <button id="btnEdit" type="button" class="btn-register">Edit</button>
                        <button id="btnSave" type="button" class="btn-register" style="display:none;">Save</button>
                        <button id="btnCancel" type="button" class="btn-register" style="display:none;background:#ccc;color:#333;">Cancel</button>
                    </div>
                </div>
    <script src="/assets/script/personal_info.js"></script>
    <script src="/assets/script/script.js"></script>
            </main>
        </div>
    </form>
</body>
</html>

