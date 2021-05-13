using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Ledger2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Ledger2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Ledger2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "FiscalYearOneId", "fiscalYearOneId" }, { "LedgerStatus", "ledgerStatus" }, { "Allocated", "allocated" }, { "Available", "available" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "Currency", "currency" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" }, { "AwaitingPayment", "awaitingPayment" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" } };
            Ledger2sRadGrid.DataSource = folioServiceContext.Ledger2s(out var i, Global.GetCqlFilter(Ledger2sRadGrid, d), Ledger2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Ledger2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Ledger2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Ledger2sRadGrid.PageSize * Ledger2sRadGrid.CurrentPageIndex, Ledger2sRadGrid.PageSize, true);
            Ledger2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Ledger2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "FiscalYearOneId", "fiscalYearOneId" }, { "LedgerStatus", "ledgerStatus" }, { "Allocated", "allocated" }, { "Available", "available" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "Currency", "currency" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" }, { "AwaitingPayment", "awaitingPayment" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" } };
            Response.Write("Id\tName\tCode\tDescription\tFiscalYearOne\tFiscalYearOneId\tLedgerStatus\tAllocated\tAvailable\tNetTransfers\tUnavailable\tCurrency\tRestrictEncumbrance\tRestrictExpenditures\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tInitialAllocation\tAllocationTo\tAllocationFrom\tTotalFunding\tCashBalance\tAwaitingPayment\tEncumbered\tExpenditures\tOverEncumbrance\tOverExpended\r\n");
            foreach (var l2 in folioServiceContext.Ledger2s(Global.GetCqlFilter(Ledger2sRadGrid, d), Ledger2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Ledger2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Ledger2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{l2.Id}\t{Global.TextEncode(l2.Name)}\t{Global.TextEncode(l2.Code)}\t{Global.TextEncode(l2.Description)}\t{Global.TextEncode(l2.FiscalYearOne?.Name)}\t{l2.FiscalYearOneId}\t{Global.TextEncode(l2.LedgerStatus)}\t{l2.Allocated}\t{l2.Available}\t{l2.NetTransfers}\t{l2.Unavailable}\t{Global.TextEncode(l2.Currency)}\t{l2.RestrictEncumbrance}\t{l2.RestrictExpenditures}\t{l2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.CreationUser?.Username)}\t{l2.CreationUserId}\t{l2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.LastWriteUser?.Username)}\t{l2.LastWriteUserId}\t{l2.InitialAllocation}\t{l2.AllocationTo}\t{l2.AllocationFrom}\t{l2.TotalFunding}\t{l2.CashBalance}\t{l2.AwaitingPayment}\t{l2.Encumbered}\t{l2.Expenditures}\t{l2.OverEncumbrance}\t{l2.OverExpended}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
