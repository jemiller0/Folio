using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.GroupFundFiscalYear2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["GroupFundFiscalYear2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void GroupFundFiscalYear2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            GroupFundFiscalYear2sRadGrid.DataSource = folioServiceContext.GroupFundFiscalYear2s(out var i, Global.GetCqlFilter(GroupFundFiscalYear2sRadGrid, d), GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, GroupFundFiscalYear2sRadGrid.PageSize * GroupFundFiscalYear2sRadGrid.CurrentPageIndex, GroupFundFiscalYear2sRadGrid.PageSize, true);
            GroupFundFiscalYear2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"GroupFundFiscalYear2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            Response.Write("Id\tBudget\tBudgetId\tGroup\tGroupId\tFiscalYear\tFiscalYearId\tFund\tFundId\r\n");
            foreach (var gffy2 in folioServiceContext.GroupFundFiscalYear2s(Global.GetCqlFilter(GroupFundFiscalYear2sRadGrid, d), GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{gffy2.Id}\t{Global.TextEncode(gffy2.Budget?.Name)}\t{gffy2.BudgetId}\t{Global.TextEncode(gffy2.Group?.Name)}\t{gffy2.GroupId}\t{Global.TextEncode(gffy2.FiscalYear?.Name)}\t{gffy2.FiscalYearId}\t{Global.TextEncode(gffy2.Fund?.Name)}\t{gffy2.FundId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
