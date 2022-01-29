using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LedgerRollover2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LedgerRollover2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LedgerRollover2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var lr2 = folioServiceContext.FindLedgerRollover2(id, true);
            if (lr2 == null) Response.Redirect("Default.aspx");
            lr2.Content = lr2.Content != null ? JsonConvert.DeserializeObject<JToken>(lr2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            LedgerRollover2FormView.DataSource = new[] { lr2 };
            Title = $"Ledger Rollover {lr2.Id}";
        }

        protected void LedgerRolloverBudgetsRolloversRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRolloverBudgetsRolloversPermission"] == null) return;
            var id = (Guid?)LedgerRollover2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindLedgerRollover2(id, true).LedgerRolloverBudgetsRollovers ?? new LedgerRolloverBudgetsRollover[] { };
            LedgerRolloverBudgetsRolloversRadGrid.DataSource = l;
            LedgerRolloverBudgetsRolloversRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LedgerRolloverBudgetsRolloversPanel.Visible = LedgerRollover2FormView.DataKey.Value != null && ((string)Session["LedgerRolloverBudgetsRolloversPermission"] == "Edit" || Session["LedgerRolloverBudgetsRolloversPermission"] != null && l.Any());
        }

        protected void LedgerRolloverEncumbrancesRolloversRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRolloverEncumbrancesRolloversPermission"] == null) return;
            var id = (Guid?)LedgerRollover2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindLedgerRollover2(id, true).LedgerRolloverEncumbrancesRollovers ?? new LedgerRolloverEncumbrancesRollover[] { };
            LedgerRolloverEncumbrancesRolloversRadGrid.DataSource = l;
            LedgerRolloverEncumbrancesRolloversRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LedgerRolloverEncumbrancesRolloversPanel.Visible = LedgerRollover2FormView.DataKey.Value != null && ((string)Session["LedgerRolloverEncumbrancesRolloversPermission"] == "Edit" || Session["LedgerRolloverEncumbrancesRolloversPermission"] != null && l.Any());
        }

        protected void LedgerRolloverError2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRolloverError2sPermission"] == null) return;
            var id = (Guid?)LedgerRollover2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerRolloverId == \"{id}\"",
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "ErrorType", "errorType"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "FailedAction", "failedAction"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "ErrorMessage", "errorMessage"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LedgerRolloverError2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LedgerRolloverError2sRadGrid.DataSource = folioServiceContext.LedgerRolloverError2s(out var i, where, LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRolloverError2sRadGrid.PageSize * LedgerRolloverError2sRadGrid.CurrentPageIndex, LedgerRolloverError2sRadGrid.PageSize, true);
            LedgerRolloverError2sRadGrid.VirtualItemCount = i;
            if (LedgerRolloverError2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRolloverError2sRadGrid.AllowFilteringByColumn = LedgerRolloverError2sRadGrid.VirtualItemCount > 10;
                LedgerRolloverError2sPanel.Visible = LedgerRollover2FormView.DataKey.Value != null && Session["LedgerRolloverError2sPermission"] != null && LedgerRolloverError2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void LedgerRolloverProgress2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRolloverProgress2sPermission"] == null) return;
            var id = (Guid?)LedgerRollover2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerRolloverId == \"{id}\"",
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "OverallRolloverStatus", "overallRolloverStatus"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "FinancialRolloverStatus", "financialRolloverStatus"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "OrdersRolloverStatus", "ordersRolloverStatus"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LedgerRolloverProgress2sRadGrid.DataSource = folioServiceContext.LedgerRolloverProgress2s(out var i, where, LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRolloverProgress2sRadGrid.PageSize * LedgerRolloverProgress2sRadGrid.CurrentPageIndex, LedgerRolloverProgress2sRadGrid.PageSize, true);
            LedgerRolloverProgress2sRadGrid.VirtualItemCount = i;
            if (LedgerRolloverProgress2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRolloverProgress2sRadGrid.AllowFilteringByColumn = LedgerRolloverProgress2sRadGrid.VirtualItemCount > 10;
                LedgerRolloverProgress2sPanel.Visible = LedgerRollover2FormView.DataKey.Value != null && Session["LedgerRolloverProgress2sPermission"] != null && LedgerRolloverProgress2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
