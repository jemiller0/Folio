using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BudgetExpenseClass2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BudgetExpenseClass2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BudgetExpenseClass2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "ExpenseClassId", "expenseClassId" }, { "Status", "status" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Status", "status")
            }.Where(s => s != null)));
            BudgetExpenseClass2sRadGrid.DataSource = folioServiceContext.BudgetExpenseClass2s(where, BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetExpenseClass2sRadGrid.PageSize * BudgetExpenseClass2sRadGrid.CurrentPageIndex, BudgetExpenseClass2sRadGrid.PageSize, true);
            BudgetExpenseClass2sRadGrid.VirtualItemCount = folioServiceContext.CountBudgetExpenseClass2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BudgetExpenseClass2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tBudget\tBudgetId\tExpenseClass\tExpenseClassId\tStatus\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "ExpenseClassId", "expenseClassId" }, { "Status", "status" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Status", "status")
            }.Where(s => s != null)));
            foreach (var bec2 in folioServiceContext.BudgetExpenseClass2s(where, BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bec2.Id}\t{Global.TextEncode(bec2.Budget?.Name)}\t{bec2.BudgetId}\t{Global.TextEncode(bec2.ExpenseClass?.Name)}\t{bec2.ExpenseClassId}\t{Global.TextEncode(bec2.Status)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
