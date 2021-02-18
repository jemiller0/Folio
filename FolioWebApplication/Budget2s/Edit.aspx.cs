using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Budget2s
{
    public partial class Edit : System.Web.UI.Page
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

        protected void Budget2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var b2 = folioServiceContext.FindBudget2(id, true);
            if (b2 == null) Response.Redirect("Default.aspx");
            Budget2FormView.DataSource = new[] { b2 };
            Title = $"Budget {b2.Name}";
        }

        protected void BudgetExpenseClass2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetExpenseClass2sPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "ExpenseClassId", "expenseClassId" }, { "Status", "status" } };
            BudgetExpenseClass2sRadGrid.DataSource = folioServiceContext.BudgetExpenseClass2s(out var i, Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, d, $"budgetId == \"{id}\""), BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetExpenseClass2sRadGrid.PageSize * BudgetExpenseClass2sRadGrid.CurrentPageIndex, BudgetExpenseClass2sRadGrid.PageSize, true);
            BudgetExpenseClass2sRadGrid.VirtualItemCount = i;
            if (BudgetExpenseClass2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetExpenseClass2sRadGrid.AllowFilteringByColumn = BudgetExpenseClass2sRadGrid.VirtualItemCount > 10;
                BudgetExpenseClass2sPanel.Visible = Budget2FormView.DataKey.Value != null && Session["BudgetExpenseClass2sPermission"] != null && BudgetExpenseClass2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void GroupFundFiscalYear2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["GroupFundFiscalYear2sPermission"] == null) return;
            var id = (Guid?)Budget2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            GroupFundFiscalYear2sRadGrid.DataSource = folioServiceContext.GroupFundFiscalYear2s(out var i, Global.GetCqlFilter(GroupFundFiscalYear2sRadGrid, d, $"budgetId == \"{id}\""), GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, GroupFundFiscalYear2sRadGrid.PageSize * GroupFundFiscalYear2sRadGrid.CurrentPageIndex, GroupFundFiscalYear2sRadGrid.PageSize, true);
            GroupFundFiscalYear2sRadGrid.VirtualItemCount = i;
            if (GroupFundFiscalYear2sRadGrid.MasterTableView.FilterExpression == "")
            {
                GroupFundFiscalYear2sRadGrid.AllowFilteringByColumn = GroupFundFiscalYear2sRadGrid.VirtualItemCount > 10;
                GroupFundFiscalYear2sPanel.Visible = Budget2FormView.DataKey.Value != null && Session["GroupFundFiscalYear2sPermission"] != null && GroupFundFiscalYear2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
