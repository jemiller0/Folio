using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderTemplate2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrderTemplate2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrderTemplate2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ot2 = folioServiceContext.FindOrderTemplate2(id, true);
            if (ot2 == null) Response.Redirect("Default.aspx");
            OrderTemplate2FormView.DataSource = new[] { ot2 };
            Title = $"Order Template {ot2.Id}";
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)OrderTemplate2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2sRadGrid, d, $"template == \"{id}\""), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = OrderTemplate2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
