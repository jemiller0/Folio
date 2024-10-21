using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderTemplate2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            ot2.Content = ot2.Content != null ? JsonConvert.DeserializeObject<JToken>(ot2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            OrderTemplate2FormView.DataSource = new[] { ot2 };
            Title = $"Order Template {ot2.Name}";
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)OrderTemplate2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "NextPolNumber", "nextPolNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"template == \"{id}\"",
                Global.GetCqlFilter(Order2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Order2sRadGrid, "Approved", "approved"),
                Global.GetCqlFilter(Order2sRadGrid, "ApprovedBy.Username", "approvedById", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "AssignedTo.Username", "assignedTo", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonReason", "closeReason.reason"),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonNote", "closeReason.note"),
                Global.GetCqlFilter(Order2sRadGrid, "OrderDate", "dateOrdered"),
                Global.GetCqlFilter(Order2sRadGrid, "Manual", "manualPo"),
                Global.GetCqlFilter(Order2sRadGrid, "Number", "poNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OrderType", "orderType"),
                Global.GetCqlFilter(Order2sRadGrid, "Reencumber", "reEncumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingInterval", "ongoing.interval"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingIsSubscription", "ongoing.isSubscription"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingManualRenewal", "ongoing.manualRenewal"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingNotes", "ongoing.notes"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewPeriod", "ongoing.reviewPeriod"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingRenewalDate", "ongoing.renewalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewDate", "ongoing.reviewDate"),
                Global.GetCqlFilter(Order2sRadGrid, "Vendor.Name", "vendor", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Order2sRadGrid, "Status", "workflowStatus"),
                Global.GetCqlFilter(Order2sRadGrid, "NextPolNumber", "nextPolNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Order2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, where, Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = OrderTemplate2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
