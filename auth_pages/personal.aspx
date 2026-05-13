<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="personal.aspx.cs" Inherits="Project_Tracking.auth_pages.personal" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Personal Information</title>
    <link href="../assets/style/personal.css" rel="stylesheet" />
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
</head>

<body>

<form id="form1" runat="server">

<div class="dashboard-container">

    <main class="main-content">

        <h2>Personal Information</h2>

        <div class="profile-card">

            <div id="profileMsg"></div>

            <div class="form-group">
                <label>First Name</label>
                <input type="text" id="firstName" />
            </div>

            <div class="form-group">
                <label>Middle Name</label>
                <input type="text" id="middleName" />
            </div>

            <div class="form-group">
                <label>Last Name</label>
                <input type="text" id="lastName" />
            </div>

            <div class="form-group">
                <label>Email Address</label>
                <input type="email" id="email" />
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Birthday</label>
                    <input type="date" id="birthday" />
                </div>

                <div class="form-group">
                    <label>ZIP Code</label>
                    <input type="text" id="zipCode" maxlength="4" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Contact Number (Hotline)</label>
                    <input type="text" id="contactNumber" maxlength="11" />
                </div>

                <div class="form-group">
                    <label>Mobile Number</label>
                    <input type="text" id="mobileNumber" maxlength="11" />
                </div>
            </div>

            <div class="form-group">
                <label>Home Address</label>
                <textarea id="address" rows="2"></textarea>
            </div>

            <div style="margin-top:10px;">
                <input type="checkbox" id="termsAccepted" />
                <label for="termsAccepted">
                    I accept the 
                    <a href="../view/term_condition.aspx" target="_blank">Terms and Conditions</a>
                </label>
            </div>

            <button type="button" id="continueBtn">
                Continue to Registration
            </button>

            <a href="login.aspx">Login here</a>

        </div>

    </main>

</div>

</form>

<script src="<%= ResolveUrl("~/assets/script/personal.js") %>"></script>

</body>
</html>