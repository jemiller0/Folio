using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Budget2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Budget2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Budget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Credits", "credits" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Budget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Budget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Budget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(Budget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(Budget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(Budget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Budget2sRadGrid, "Credits", "credits"),
                Global.GetCqlFilter(Budget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(Budget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(Budget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(Budget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(Budget2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Budget2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(Budget2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(Budget2sRadGrid, "CashBalance", "cashBalance")
            }.Where(s => s != null)));
            Budget2sRadGrid.DataSource = folioServiceContext.Budget2s(where, Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Budget2sRadGrid.PageSize * Budget2sRadGrid.CurrentPageIndex, Budget2sRadGrid.PageSize, true);
            Budget2sRadGrid.VirtualItemCount = folioServiceContext.CountBudget2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Budget2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tBudgetStatus\tAllowableEncumbrance\tAllowableExpenditure\tAllocated\tAwaitingPayment\tAvailable\tCredits\tEncumbered\tExpenditures\tNetTransfers\tUnavailable\tOverEncumbrance\tOverExpended\tFund\tFundId\tFiscalYear\tFiscalYearId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tInitialAllocation\tAllocationTo\tAllocationFrom\tTotalFunding\tCashBalance\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Credits", "credits" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Budget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Budget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Budget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(Budget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(Budget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(Budget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Budget2sRadGrid, "Credits", "credits"),
                Global.GetCqlFilter(Budget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(Budget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(Budget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(Budget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(Budget2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Budget2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(Budget2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(Budget2sRadGrid, "CashBalance", "cashBalance")
            }.Where(s => s != null)));
            foreach (var b2 in folioServiceContext.Budget2s(where, Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{b2.Id}\t{Global.TextEncode(b2.Name)}\t{Global.TextEncode(b2.BudgetStatus)}\t{b2.AllowableEncumbrance}\t{b2.AllowableExpenditure}\t{b2.Allocated}\t{b2.AwaitingPayment}\t{b2.Available}\t{b2.Credits}\t{b2.Encumbered}\t{b2.Expenditures}\t{b2.NetTransfers}\t{b2.Unavailable}\t{b2.OverEncumbrance}\t{b2.OverExpended}\t{Global.TextEncode(b2.Fund?.Name)}\t{b2.FundId}\t{Global.TextEncode(b2.FiscalYear?.Name)}\t{b2.FiscalYearId}\t{b2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.CreationUser?.Username)}\t{b2.CreationUserId}\t{b2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.LastWriteUser?.Username)}\t{b2.LastWriteUserId}\t{b2.InitialAllocation}\t{b2.AllocationTo}\t{b2.AllocationFrom}\t{b2.TotalFunding}\t{b2.CashBalance}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
