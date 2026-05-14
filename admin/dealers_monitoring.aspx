<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="dealers_monitoring.aspx.cs"
    Inherits="Project_Tracking.admin.dealers_monitoring" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Q2 Monitoring Sales</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background:#f5f6fa; font-family:Arial; }
        .top-header {
            background:#fff; padding:20px; border-radius:15px;
            margin-bottom:20px; box-shadow:0 2px 10px rgba(0,0,0,0.05);
        }
        .title { font-size:28px; font-weight:bold; }
        .sub   { color:#777; font-size:14px; margin-top:5px; }
        .stat-badges { display:flex; gap:10px; flex-wrap:wrap; margin-top:12px; }
        .stat-box {
            background:#f8f9fa; border:1px solid #dee2e6;
            border-radius:10px; padding:8px 16px; text-align:center; min-width:100px;
        }
        .stat-box .val { font-size:18px; font-weight:bold; }
        .stat-box .lbl { font-size:11px; color:#777; text-transform:uppercase; }
        .table-box {
            background:#fff; padding:20px; border-radius:15px;
            box-shadow:0 2px 10px rgba(0,0,0,0.05);
        }
        th {
            background:#dff0d8 !important; color:#2e7d32 !important;
            font-size:11px; text-transform:uppercase; white-space:nowrap;
        }
        td { font-size:13px; vertical-align:middle; white-space:nowrap; }
        .badge-rank {
            padding:4px 12px; border-radius:20px; color:#fff;
            font-size:11px; font-weight:bold; display:inline-block;
        }
        .rank-New      { background:#6c757d; }
        .rank-Bronze   { background:#cd7f32; }
        .rank-Silver   { background:#95a5a6; }
        .rank-Gold     { background:#f39c12; }
        .rank-Platinum { background:#8e44ad; }
        .rank-Diamond  { background:#1a5276; }
        .total-sales   { font-weight:bold; color:#000; }
        .next-target   { color:#2c3e50; font-weight:bold; }
        .goal          { color:#27ae60; font-weight:bold; }
        .pending       { color:#e67e22; font-weight:bold; }
        .gap-done      { color:#27ae60; font-size:12px; }
        .gap-need      { color:#e74c3c; font-size:12px; }
        tfoot td {
            font-weight:bold; background:#dff0d8 !important; color:#2e7d32 !important;
        }
        .rank-legend {
            display:flex; gap:10px; flex-wrap:wrap;
            margin-bottom:14px; font-size:12px; align-items:center;
        }
        .rank-legend span { display:flex; align-items:center; gap:5px; }
        .dot { width:10px; height:10px; border-radius:50%; display:inline-block; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="container-fluid p-4">

    <!-- Header -->
    <div class="top-header">
        <div class="title">Q2 MONITORING SALES</div>
        <div class="sub">as of <asp:Label ID="lblDate" runat="server" /></div>
        <div class="stat-badges">
            <div class="stat-box">
                <div class="val text-primary"><asp:Label ID="lblTotalDealers" runat="server" /></div>
                <div class="lbl">Total Dealers</div>
            </div>
            <div class="stat-box">
                <div class="val text-success"><asp:Label ID="lblGrandTotal" runat="server" /></div>
                <div class="lbl">Q2 Total Sales</div>
            </div>
            <div class="stat-box">
                <div class="val" style="color:#1a5276"><asp:Label ID="lblDiamondCount" runat="server" /></div>
                <div class="lbl">Diamond</div>
            </div>
            <div class="stat-box">
                <div class="val" style="color:#8e44ad"><asp:Label ID="lblPlatinumCount" runat="server" /></div>
                <div class="lbl">Platinum</div>
            </div>
            <div class="stat-box">
                <div class="val" style="color:#f39c12"><asp:Label ID="lblGoldCount" runat="server" /></div>
                <div class="lbl">Gold</div>
            </div>
            <div class="stat-box">
                <div class="val" style="color:#95a5a6"><asp:Label ID="lblSilverCount" runat="server" /></div>
                <div class="lbl">Silver</div>
            </div>
            <div class="stat-box">
                <div class="val" style="color:#cd7f32"><asp:Label ID="lblBronzeCount" runat="server" /></div>
                <div class="lbl">Bronze</div>
            </div>
        </div>
    </div>

    <!-- Table -->
    <div class="table-box">
        <div class="rank-legend">
            <strong>Rank:</strong>
            <span><span class="dot" style="background:#6c757d"></span>New &lt;₱1,600</span>
            <span><span class="dot" style="background:#cd7f32"></span>Bronze ₱1,600</span>
            <span><span class="dot" style="background:#95a5a6"></span>Silver ₱3,700</span>
            <span><span class="dot" style="background:#f39c12"></span>Gold ₱15,000</span>
            <span><span class="dot" style="background:#8e44ad"></span>Platinum ₱26,250</span>
            <span><span class="dot" style="background:#1a5276"></span>Diamond ₱75,000</span>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvSales"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover"
                GridLines="None"
                ShowFooter="True"
                OnRowDataBound="gvSales_RowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                        <FooterTemplate><strong>TOTAL</strong></FooterTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="dealer_name" HeaderText="AVON REPS" />

                    <asp:TemplateField HeaderText="SEGMENT">
                        <ItemTemplate>
                            <span class='badge-rank rank-<%# Eval("computed_rank") %>'>
                                <%# Eval("computed_rank") %>
                            </span>
                        </ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <%-- ✅ FIX: Gamitin ang String.Format para sa peso values, hindi FormatPeso() --%>
                    <asp:TemplateField HeaderText="APRIL SALES">
                        <ItemTemplate><%# Convert.ToDecimal(Eval("april_sales")) == 0 ? "–" : String.Format("₱{0:N2}", Eval("april_sales")) %></ItemTemplate>
                        <FooterTemplate><asp:Label ID="lblFootApril" runat="server" /></FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="MAY SALES">
                        <ItemTemplate><%# Convert.ToDecimal(Eval("may_sales")) == 0 ? "–" : String.Format("₱{0:N2}", Eval("may_sales")) %></ItemTemplate>
                        <FooterTemplate><asp:Label ID="lblFootMay" runat="server" /></FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="JUNE SALES">
                        <ItemTemplate><%# Convert.ToDecimal(Eval("june_sales")) == 0 ? "–" : String.Format("₱{0:N2}", Eval("june_sales")) %></ItemTemplate>
                        <FooterTemplate><asp:Label ID="lblFootJune" runat="server" /></FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="TOTAL SALES (SUKI PTS)">
                        <ItemTemplate>
                            <span class="total-sales">
                                <%# String.Format("₱{0:N2}", Eval("total_sales")) %>
                            </span>
                        </ItemTemplate>
                        <FooterTemplate><asp:Label ID="lblFootTotal" runat="server" /></FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="TO SILVER (3,700)">
                        <ItemTemplate><asp:Label ID="lblSilverGap"   runat="server" /></ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="TO GOLD (15,000)">
                        <ItemTemplate><asp:Label ID="lblGoldGap"     runat="server" /></ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="TO PLATINUM (26,250)">
                        <ItemTemplate><asp:Label ID="lblPlatinumGap" runat="server" /></ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="TO DIAMOND (75,000)">
                        <ItemTemplate><asp:Label ID="lblDiamondGap"  runat="server" /></ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="NEXT TARGET">
                        <ItemTemplate>
                            <span class="next-target">
                                <%# String.Format("₱{0:N2}", Eval("next_target")) %>
                            </span>
                        </ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="REMARK">
                        <ItemTemplate><asp:Label ID="lblRemark" runat="server" /></ItemTemplate>
                        <FooterTemplate>&nbsp;</FooterTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>
</form>
</body>
</html>
