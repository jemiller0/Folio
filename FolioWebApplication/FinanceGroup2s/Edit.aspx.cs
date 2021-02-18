using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            FinanceGroup2FormView.DataSource = new[] { fg2 };
            Title = $"Finance Group {fg2.Name}";
        }

        protected void GroupFundFiscalYear2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["GroupFundFiscalYear2sPermission"] == null) return;
            var id = (Guid?)FinanceGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            GroupFundFiscalYear2sRadGrid.DataSource = folioServiceContext.GroupFundFiscalYear2s(out var i, Global.GetCqlFilter(GroupFundFiscalYear2sRadGrid, d, $"groupId == \"{id}\""), GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, GroupFundFiscalYear2sRadGrid.PageSize * GroupFundFiscalYear2sRadGrid.CurrentPageIndex, GroupFundFiscalYear2sRadGrid.PageSize, true);
            GroupFundFiscalYear2sRadGrid.VirtualItemCount = i;
            if (GroupFundFiscalYear2sRadGrid.MasterTableView.FilterExpression == "")
            {
                GroupFundFiscalYear2sRadGrid.AllowFilteringByColumn = GroupFundFiscalYear2sRadGrid.VirtualItemCount > 10;
                GroupFundFiscalYear2sPanel.Visible = FinanceGroup2FormView.DataKey.Value != null && Session["GroupFundFiscalYear2sPermission"] != null && GroupFundFiscalYear2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
