using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Budget2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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

        protected void Budget2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var b2 = folioServiceContext.FindBudget2(id, true);
            if (b2 == null) Response.Redirect("Default.aspx");
            b2.Content = b2.Content != null ? JsonConvert.DeserializeObject<JToken>(b2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Budget2FormView.DataSource = new[] { b2 };
            Title = $"Budget {b2.Name}";
        }

        protected void BudgetAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindBudget2(id, true).BudgetAcquisitionsUnits ?? new BudgetAcquisitionsUnit[] { };
            BudgetAcquisitionsUnitsRadGrid.DataSource = l;
            BudgetAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            BudgetAcquisitionsUnitsPanel.Visible = Budget2FormView.DataKey.Value != null && ((string)Session["BudgetAcquisitionsUnitsPermission"] == "Edit" || Session["BudgetAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void BudgetExpenseClass2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetExpenseClass2sPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "ExpenseClassId", "expenseClassId" }, { "Status", "status" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"budgetId == \"{id}\"",
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Status", "status")
            }.Where(s => s != null)));
            BudgetExpenseClass2sRadGrid.DataSource = folioServiceContext.BudgetExpenseClass2s(out var i, where, BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetExpenseClass2sRadGrid.PageSize * BudgetExpenseClass2sRadGrid.CurrentPageIndex, BudgetExpenseClass2sRadGrid.PageSize, true);
            BudgetExpenseClass2sRadGrid.VirtualItemCount = i;
            if (BudgetExpenseClass2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetExpenseClass2sRadGrid.AllowFilteringByColumn = BudgetExpenseClass2sRadGrid.VirtualItemCount > 10;
                BudgetExpenseClass2sPanel.Visible = Budget2FormView.DataKey.Value != null && Session["BudgetExpenseClass2sPermission"] != null && BudgetExpenseClass2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void BudgetGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetGroup2sPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"budgetId == \"{id}\"",
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Group.Name", "groupId", "name", folioServiceContext.FolioServiceClient.FinanceGroups),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds)
            }.Where(s => s != null)));
            BudgetGroup2sRadGrid.DataSource = folioServiceContext.BudgetGroup2s(out var i, where, BudgetGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetGroup2sRadGrid.PageSize * BudgetGroup2sRadGrid.CurrentPageIndex, BudgetGroup2sRadGrid.PageSize, true);
            BudgetGroup2sRadGrid.VirtualItemCount = i;
            if (BudgetGroup2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetGroup2sRadGrid.AllowFilteringByColumn = BudgetGroup2sRadGrid.VirtualItemCount > 10;
                BudgetGroup2sPanel.Visible = Budget2FormView.DataKey.Value != null && Session["BudgetGroup2sPermission"] != null && BudgetGroup2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void BudgetTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetTagsPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindBudget2(id, true).BudgetTags ?? new BudgetTag[] { };
            BudgetTagsRadGrid.DataSource = l;
            BudgetTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            BudgetTagsPanel.Visible = Budget2FormView.DataKey.Value != null && ((string)Session["BudgetTagsPermission"] == "Edit" || Session["BudgetTagsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
