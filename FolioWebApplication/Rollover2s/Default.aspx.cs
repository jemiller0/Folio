using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Rollover2s
{
    public partial class Default : System.Web.UI.Page
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

        protected void Rollover2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "RolloverType", "rolloverType" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Rollover2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Rollover2sRadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(Rollover2sRadGrid, "RolloverType", "rolloverType"),
                Global.GetCqlFilter(Rollover2sRadGrid, "FromFiscalYear.Name", "fromFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Rollover2sRadGrid, "ToFiscalYear.Name", "toFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Rollover2sRadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(Rollover2sRadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(Rollover2sRadGrid, "NeedCloseBudgets", "needCloseBudgets"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CurrencyFactor", "currencyFactor"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Rollover2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Rollover2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Rollover2sRadGrid.DataSource = folioServiceContext.Rollover2s(out var i, where, Rollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Rollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Rollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Rollover2sRadGrid.PageSize * Rollover2sRadGrid.CurrentPageIndex, Rollover2sRadGrid.PageSize, true);
            Rollover2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Rollover2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tLedger\tLedgerId\tRolloverType\tFromFiscalYear\tFromFiscalYearId\tToFiscalYear\tToFiscalYearId\tRestrictEncumbrance\tRestrictExpenditures\tNeedCloseBudgets\tCurrencyFactor\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "RolloverType", "rolloverType" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Rollover2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Rollover2sRadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(Rollover2sRadGrid, "RolloverType", "rolloverType"),
                Global.GetCqlFilter(Rollover2sRadGrid, "FromFiscalYear.Name", "fromFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Rollover2sRadGrid, "ToFiscalYear.Name", "toFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Rollover2sRadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(Rollover2sRadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(Rollover2sRadGrid, "NeedCloseBudgets", "needCloseBudgets"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CurrencyFactor", "currencyFactor"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Rollover2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Rollover2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Rollover2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var r2 in folioServiceContext.Rollover2s(where, Rollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Rollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Rollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{Global.TextEncode(r2.Ledger?.Name)}\t{r2.LedgerId}\t{Global.TextEncode(r2.RolloverType)}\t{Global.TextEncode(r2.FromFiscalYear?.Name)}\t{r2.FromFiscalYearId}\t{Global.TextEncode(r2.ToFiscalYear?.Name)}\t{r2.ToFiscalYearId}\t{r2.RestrictEncumbrance}\t{r2.RestrictExpenditures}\t{r2.NeedCloseBudgets}\t{r2.CurrencyFactor}\t{r2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.CreationUser?.Username)}\t{r2.CreationUserId}\t{r2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.LastWriteUser?.Username)}\t{r2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
