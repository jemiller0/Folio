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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Budget2sRadGrid.DataSource = folioServiceContext.Budget2s(out var i, Global.GetCqlFilter(Budget2sRadGrid, d), Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Budget2sRadGrid.PageSize * Budget2sRadGrid.CurrentPageIndex, Budget2sRadGrid.PageSize, true);
            Budget2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Budget2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tBudgetStatus\tAllowableEncumbrance\tAllowableExpenditure\tAllocated\tAwaitingPayment\tAvailable\tEncumbered\tExpenditures\tNetTransfers\tUnavailable\tOverEncumbrance\tOverExpended\tFund\tFundId\tFiscalYear\tFiscalYearId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var b2 in folioServiceContext.Budget2s(Global.GetCqlFilter(Budget2sRadGrid, d), Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{b2.Id}\t{Global.TextEncode(b2.Name)}\t{Global.TextEncode(b2.BudgetStatus)}\t{b2.AllowableEncumbrance}\t{b2.AllowableExpenditure}\t{b2.Allocated}\t{b2.AwaitingPayment}\t{b2.Available}\t{b2.Encumbered}\t{b2.Expenditures}\t{b2.NetTransfers}\t{b2.Unavailable}\t{b2.OverEncumbrance}\t{b2.OverExpended}\t{Global.TextEncode(b2.Fund?.Name)}\t{b2.FundId}\t{Global.TextEncode(b2.FiscalYear?.Name)}\t{b2.FiscalYearId}\t{b2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.CreationUser?.Username)}\t{b2.CreationUserId}\t{b2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.LastWriteUser?.Username)}\t{b2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
