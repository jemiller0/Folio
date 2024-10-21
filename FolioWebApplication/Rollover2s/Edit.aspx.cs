using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Rollover2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Rollover2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Rollover2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var r2 = folioServiceContext.FindRollover2(id, true);
            if (r2 == null) Response.Redirect("Default.aspx");
            r2.Content = r2.Content != null ? JsonConvert.DeserializeObject<JToken>(r2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Rollover2FormView.DataSource = new[] { r2 };
            Title = $"Rollover {r2.Id}";
        }

        protected void RolloverBudget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudget2sPermission"] == null) return;
            var id = (Guid?)Rollover2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "RolloverId", "ledgerRolloverId" }, { "Name", "name" }, { "FundDetailsName", "fundDetails.name" }, { "FundDetailsCode", "fundDetails.code" }, { "FundDetailsFundStatus", "fundDetails.fundStatus" }, { "FundDetailsFundTypeId", "fundDetails.fundTypeId" }, { "FundDetailsFundTypeName", "fundDetails.fundTypeName" }, { "FundDetailsExternalAccountNo", "fundDetails.externalAccountNo" }, { "FundDetailsDescription", "fundDetails.description" }, { "FundDetailsRestrictByLocations", "fundDetails.restrictByLocations" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Credits", "credits" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerRolloverId == \"{id}\"",
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsName", "fundDetails.name"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsCode", "fundDetails.code"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundStatus", "fundDetails.fundStatus"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundType.Name", "fundDetails.fundTypeId", "name", folioServiceContext.FolioServiceClient.FundTypes),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundTypeName", "fundDetails.fundTypeName"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsExternalAccountNo", "fundDetails.externalAccountNo"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsDescription", "fundDetails.description"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsRestrictByLocations", "fundDetails.restrictByLocations"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Credits", "credits"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CashBalance", "cashBalance")
            }.Where(s => s != null)));
            RolloverBudget2sRadGrid.DataSource = folioServiceContext.RolloverBudget2s(out var i, where, RolloverBudget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverBudget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverBudget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverBudget2sRadGrid.PageSize * RolloverBudget2sRadGrid.CurrentPageIndex, RolloverBudget2sRadGrid.PageSize, true);
            RolloverBudget2sRadGrid.VirtualItemCount = i;
            if (RolloverBudget2sRadGrid.MasterTableView.FilterExpression == "")
            {
                RolloverBudget2sRadGrid.AllowFilteringByColumn = RolloverBudget2sRadGrid.VirtualItemCount > 10;
                RolloverBudget2sPanel.Visible = Rollover2FormView.DataKey.Value != null && Session["RolloverBudget2sPermission"] != null && RolloverBudget2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void RolloverBudgetsRolloversRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetsRolloversPermission"] == null) return;
            var id = (Guid?)Rollover2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRollover2(id, true).RolloverBudgetsRollovers ?? new RolloverBudgetsRollover[] { };
            RolloverBudgetsRolloversRadGrid.DataSource = l;
            RolloverBudgetsRolloversRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetsRolloversPanel.Visible = Rollover2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetsRolloversPermission"] == "Edit" || Session["RolloverBudgetsRolloversPermission"] != null && l.Any());
        }

        protected void RolloverEncumbrancesRolloversRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverEncumbrancesRolloversPermission"] == null) return;
            var id = (Guid?)Rollover2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRollover2(id, true).RolloverEncumbrancesRollovers ?? new RolloverEncumbrancesRollover[] { };
            RolloverEncumbrancesRolloversRadGrid.DataSource = l;
            RolloverEncumbrancesRolloversRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverEncumbrancesRolloversPanel.Visible = Rollover2FormView.DataKey.Value != null && ((string)Session["RolloverEncumbrancesRolloversPermission"] == "Edit" || Session["RolloverEncumbrancesRolloversPermission"] != null && l.Any());
        }

        protected void RolloverError2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverError2sPermission"] == null) return;
            var id = (Guid?)Rollover2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerRolloverId == \"{id}\"",
                Global.GetCqlFilter(RolloverError2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorType", "errorType"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "FailedAction", "failedAction"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorMessage", "errorMessage"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RolloverError2sRadGrid.DataSource = folioServiceContext.RolloverError2s(out var i, where, RolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverError2sRadGrid.PageSize * RolloverError2sRadGrid.CurrentPageIndex, RolloverError2sRadGrid.PageSize, true);
            RolloverError2sRadGrid.VirtualItemCount = i;
            if (RolloverError2sRadGrid.MasterTableView.FilterExpression == "")
            {
                RolloverError2sRadGrid.AllowFilteringByColumn = RolloverError2sRadGrid.VirtualItemCount > 10;
                RolloverError2sPanel.Visible = Rollover2FormView.DataKey.Value != null && Session["RolloverError2sPermission"] != null && RolloverError2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void RolloverProgress2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverProgress2sPermission"] == null) return;
            var id = (Guid?)Rollover2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerRolloverId == \"{id}\"",
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OverallRolloverStatus", "overallRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "FinancialRolloverStatus", "financialRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OrdersRolloverStatus", "ordersRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RolloverProgress2sRadGrid.DataSource = folioServiceContext.RolloverProgress2s(out var i, where, RolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverProgress2sRadGrid.PageSize * RolloverProgress2sRadGrid.CurrentPageIndex, RolloverProgress2sRadGrid.PageSize, true);
            RolloverProgress2sRadGrid.VirtualItemCount = i;
            if (RolloverProgress2sRadGrid.MasterTableView.FilterExpression == "")
            {
                RolloverProgress2sRadGrid.AllowFilteringByColumn = RolloverProgress2sRadGrid.VirtualItemCount > 10;
                RolloverProgress2sPanel.Visible = Rollover2FormView.DataKey.Value != null && Session["RolloverProgress2sPermission"] != null && RolloverProgress2sRadGrid.VirtualItemCount > 0;
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
