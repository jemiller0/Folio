using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FinanceGroup2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FinanceGroup2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FinanceGroup2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var fg2 = folioServiceContext.FindFinanceGroup2(id, true);
            if (fg2 == null) Response.Redirect("Default.aspx");
            fg2.Content = fg2.Content != null ? JsonConvert.DeserializeObject<JToken>(fg2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            FinanceGroup2FormView.DataSource = new[] { fg2 };
            Title = $"Finance Group {fg2.Name}";
        }

        protected void BudgetGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetGroup2sPermission"] == null) return;
            var id = (Guid?)FinanceGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"groupId == \"{id}\"",
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds)
            }.Where(s => s != null)));
            BudgetGroup2sRadGrid.DataSource = folioServiceContext.BudgetGroup2s(out var i, where, BudgetGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetGroup2sRadGrid.PageSize * BudgetGroup2sRadGrid.CurrentPageIndex, BudgetGroup2sRadGrid.PageSize, true);
            BudgetGroup2sRadGrid.VirtualItemCount = i;
            if (BudgetGroup2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetGroup2sRadGrid.AllowFilteringByColumn = BudgetGroup2sRadGrid.VirtualItemCount > 10;
                BudgetGroup2sPanel.Visible = FinanceGroup2FormView.DataKey.Value != null && Session["BudgetGroup2sPermission"] != null && BudgetGroup2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void FinanceGroupAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FinanceGroupAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)FinanceGroup2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFinanceGroup2(id, true).FinanceGroupAcquisitionsUnits ?? new FinanceGroupAcquisitionsUnit[] { };
            FinanceGroupAcquisitionsUnitsRadGrid.DataSource = l;
            FinanceGroupAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FinanceGroupAcquisitionsUnitsPanel.Visible = FinanceGroup2FormView.DataKey.Value != null && ((string)Session["FinanceGroupAcquisitionsUnitsPermission"] == "Edit" || Session["FinanceGroupAcquisitionsUnitsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
