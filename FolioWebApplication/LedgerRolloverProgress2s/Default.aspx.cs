using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LedgerRolloverProgress2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LedgerRolloverProgress2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LedgerRolloverProgress2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LedgerRolloverProgress2sRadGrid.DataSource = folioServiceContext.LedgerRolloverProgress2s(out var i, Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, d), LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRolloverProgress2sRadGrid.PageSize * LedgerRolloverProgress2sRadGrid.CurrentPageIndex, LedgerRolloverProgress2sRadGrid.PageSize, true);
            LedgerRolloverProgress2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LedgerRolloverProgress2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tLedgerRollover\tLedgerRolloverId\tOverallRolloverStatus\tBudgetsClosingRolloverStatus\tFinancialRolloverStatus\tOrdersRolloverStatus\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var lrp2 in folioServiceContext.LedgerRolloverProgress2s(Global.GetCqlFilter(LedgerRolloverProgress2sRadGrid, d), LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lrp2.Id}\t{lrp2.LedgerRollover?.Id}\t{lrp2.LedgerRolloverId}\t{Global.TextEncode(lrp2.OverallRolloverStatus)}\t{Global.TextEncode(lrp2.BudgetsClosingRolloverStatus)}\t{Global.TextEncode(lrp2.FinancialRolloverStatus)}\t{Global.TextEncode(lrp2.OrdersRolloverStatus)}\t{lrp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lrp2.CreationUser?.Username)}\t{lrp2.CreationUserId}\t{lrp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lrp2.LastWriteUser?.Username)}\t{lrp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
