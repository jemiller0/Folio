using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Fee2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Fee2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Fee2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var f2 = folioServiceContext.FindFee2(id, true);
            if (f2 == null) Response.Redirect("Default.aspx");
            f2.Content = f2.Content != null ? JsonConvert.DeserializeObject<JToken>(f2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Fee2FormView.DataSource = new[] { f2 };
            Title = $"Fee {f2.Title}";
        }

        protected void Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Payment2sPermission"] == null) return;
            var id = (Guid?)Fee2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePointId", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"accountId == \"{id}\"",
                Global.GetCqlFilter(Payment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Payment2sRadGrid, "CreationTime", "dateAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "TypeAction", "typeAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "Comments", "comments"),
                Global.GetCqlFilter(Payment2sRadGrid, "Notify", "notify"),
                Global.GetCqlFilter(Payment2sRadGrid, "Amount", "amountAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "RemainingAmount", "balance"),
                Global.GetCqlFilter(Payment2sRadGrid, "TransactionInformation", "transactionInformation"),
                Global.GetCqlFilter(Payment2sRadGrid, "ServicePoint.Name", "createdAt", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Payment2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Payment2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Payment2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Payment2sRadGrid.DataSource = folioServiceContext.Payment2s(out var i, where, Payment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Payment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Payment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Payment2sRadGrid.PageSize * Payment2sRadGrid.CurrentPageIndex, Payment2sRadGrid.PageSize, true);
            Payment2sRadGrid.VirtualItemCount = i;
            if (Payment2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Payment2sRadGrid.AllowFilteringByColumn = Payment2sRadGrid.VirtualItemCount > 10;
                Payment2sPanel.Visible = Fee2FormView.DataKey.Value != null && Session["Payment2sPermission"] != null && Payment2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void RefundReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RefundReason2sPermission"] == null) return;
            var id = (Guid?)Fee2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"accountId == \"{id}\"",
                Global.GetCqlFilter(RefundReason2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RefundReason2sRadGrid, "Name", "nameReason"),
                Global.GetCqlFilter(RefundReason2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(RefundReason2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RefundReason2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RefundReason2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RefundReason2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RefundReason2sRadGrid.DataSource = folioServiceContext.RefundReason2s(out var i, where, RefundReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RefundReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RefundReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RefundReason2sRadGrid.PageSize * RefundReason2sRadGrid.CurrentPageIndex, RefundReason2sRadGrid.PageSize, true);
            RefundReason2sRadGrid.VirtualItemCount = i;
            if (RefundReason2sRadGrid.MasterTableView.FilterExpression == "")
            {
                RefundReason2sRadGrid.AllowFilteringByColumn = RefundReason2sRadGrid.VirtualItemCount > 10;
                RefundReason2sPanel.Visible = Fee2FormView.DataKey.Value != null && Session["RefundReason2sPermission"] != null && RefundReason2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void WaiveReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["WaiveReason2sPermission"] == null) return;
            var id = (Guid?)Fee2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"accountId == \"{id}\"",
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Name", "nameReason"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            WaiveReason2sRadGrid.DataSource = folioServiceContext.WaiveReason2s(out var i, where, WaiveReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, WaiveReason2sRadGrid.PageSize * WaiveReason2sRadGrid.CurrentPageIndex, WaiveReason2sRadGrid.PageSize, true);
            WaiveReason2sRadGrid.VirtualItemCount = i;
            if (WaiveReason2sRadGrid.MasterTableView.FilterExpression == "")
            {
                WaiveReason2sRadGrid.AllowFilteringByColumn = WaiveReason2sRadGrid.VirtualItemCount > 10;
                WaiveReason2sPanel.Visible = Fee2FormView.DataKey.Value != null && Session["WaiveReason2sPermission"] != null && WaiveReason2sRadGrid.VirtualItemCount > 0;
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
