using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Ledger2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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

        protected void Ledger2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var l2 = folioServiceContext.FindLedger2(id, true);
            if (l2 == null) Response.Redirect("Default.aspx");
            l2.Content = l2.Content != null ? JsonConvert.DeserializeObject<JToken>(l2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Ledger2FormView.DataSource = new[] { l2 };
            Title = $"Ledger {l2.Name}";
        }

        protected void Fund2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fund2sPermission"] == null) return;
            var id = (Guid?)Ledger2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "AccountNumber", "externalAccountNo" }, { "FundStatus", "fundStatus" }, { "FundTypeId", "fundTypeId" }, { "LedgerId", "ledgerId" }, { "Name", "name" }, { "RestrictByLocations", "restrictByLocations" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerId == \"{id}\"",
                Global.GetCqlFilter(Fund2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Fund2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Fund2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Fund2sRadGrid, "AccountNumber", "externalAccountNo"),
                Global.GetCqlFilter(Fund2sRadGrid, "FundStatus", "fundStatus"),
                Global.GetCqlFilter(Fund2sRadGrid, "FundType.Name", "fundTypeId", "name", folioServiceContext.FolioServiceClient.FundTypes),
                Global.GetCqlFilter(Fund2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Fund2sRadGrid, "RestrictByLocations", "restrictByLocations"),
                Global.GetCqlFilter(Fund2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fund2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fund2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fund2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Fund2sRadGrid.DataSource = folioServiceContext.Fund2s(where, Fund2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fund2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fund2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fund2sRadGrid.PageSize * Fund2sRadGrid.CurrentPageIndex, Fund2sRadGrid.PageSize, true);
            Fund2sRadGrid.VirtualItemCount = folioServiceContext.CountFund2s(where);
            if (Fund2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fund2sRadGrid.AllowFilteringByColumn = Fund2sRadGrid.VirtualItemCount > 10;
                Fund2sPanel.Visible = Ledger2FormView.DataKey.Value != null && Session["Fund2sPermission"] != null && Fund2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void LedgerAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Ledger2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindLedger2(id, true).LedgerAcquisitionsUnits ?? new LedgerAcquisitionsUnit[] { };
            LedgerAcquisitionsUnitsRadGrid.DataSource = l;
            LedgerAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LedgerAcquisitionsUnitsPanel.Visible = Ledger2FormView.DataKey.Value != null && ((string)Session["LedgerAcquisitionsUnitsPermission"] == "Edit" || Session["LedgerAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void Rollover2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Rollover2sPermission"] == null) return;
            var id = (Guid?)Ledger2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "RolloverType", "rolloverType" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ledgerId == \"{id}\"",
                Global.GetCqlFilter(Rollover2sRadGrid, "Id", "id"),
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
            Rollover2sRadGrid.DataSource = folioServiceContext.Rollover2s(where, Rollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Rollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Rollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Rollover2sRadGrid.PageSize * Rollover2sRadGrid.CurrentPageIndex, Rollover2sRadGrid.PageSize, true);
            Rollover2sRadGrid.VirtualItemCount = folioServiceContext.CountRollover2s(where);
            if (Rollover2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Rollover2sRadGrid.AllowFilteringByColumn = Rollover2sRadGrid.VirtualItemCount > 10;
                Rollover2sPanel.Visible = Ledger2FormView.DataKey.Value != null && Session["Rollover2sPermission"] != null && Rollover2sRadGrid.VirtualItemCount > 0;
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
