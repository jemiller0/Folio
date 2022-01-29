using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LedgerRollover2s
{
    public partial class Default : System.Web.UI.Page
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

        protected void LedgerRollover2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "FromFiscalYear.Name", "fromFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "ToFiscalYear.Name", "toFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "NeedCloseBudgets", "needCloseBudgets"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CurrencyFactor", "currencyFactor"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LedgerRollover2sRadGrid.DataSource = folioServiceContext.LedgerRollover2s(out var i, where, LedgerRollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRollover2sRadGrid.PageSize * LedgerRollover2sRadGrid.CurrentPageIndex, LedgerRollover2sRadGrid.PageSize, true);
            LedgerRollover2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LedgerRollover2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tLedger\tLedgerId\tFromFiscalYear\tFromFiscalYearId\tToFiscalYear\tToFiscalYearId\tRestrictEncumbrance\tRestrictExpenditures\tNeedCloseBudgets\tCurrencyFactor\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var lr2 in folioServiceContext.LedgerRollover2s(Global.GetCqlFilter(LedgerRollover2sRadGrid, d), LedgerRollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lr2.Id}\t{Global.TextEncode(lr2.Ledger?.Name)}\t{lr2.LedgerId}\t{Global.TextEncode(lr2.FromFiscalYear?.Name)}\t{lr2.FromFiscalYearId}\t{Global.TextEncode(lr2.ToFiscalYear?.Name)}\t{lr2.ToFiscalYearId}\t{lr2.RestrictEncumbrance}\t{lr2.RestrictExpenditures}\t{lr2.NeedCloseBudgets}\t{lr2.CurrencyFactor}\t{lr2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lr2.CreationUser?.Username)}\t{lr2.CreationUserId}\t{lr2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lr2.LastWriteUser?.Username)}\t{lr2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
