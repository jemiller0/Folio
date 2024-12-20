using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RolloverProgress2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RolloverProgress2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RolloverProgress2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "Rollover.Id", "ledgerRolloverId", "id", folioServiceContext.FolioServiceClient.Rollovers),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OverallRolloverStatus", "overallRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "FinancialRolloverStatus", "financialRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OrdersRolloverStatus", "ordersRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RolloverProgress2sRadGrid.DataSource = folioServiceContext.RolloverProgress2s(where, RolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverProgress2sRadGrid.PageSize * RolloverProgress2sRadGrid.CurrentPageIndex, RolloverProgress2sRadGrid.PageSize, true);
            RolloverProgress2sRadGrid.VirtualItemCount = folioServiceContext.CountRolloverProgress2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"RolloverProgress2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tRollover\tRolloverId\tOverallRolloverStatus\tBudgetsClosingRolloverStatus\tFinancialRolloverStatus\tOrdersRolloverStatus\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "OverallRolloverStatus", "overallRolloverStatus" }, { "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus" }, { "FinancialRolloverStatus", "financialRolloverStatus" }, { "OrdersRolloverStatus", "ordersRolloverStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "Rollover.Id", "ledgerRolloverId", "id", folioServiceContext.FolioServiceClient.Rollovers),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OverallRolloverStatus", "overallRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "BudgetsClosingRolloverStatus", "budgetsClosingRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "FinancialRolloverStatus", "financialRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "OrdersRolloverStatus", "ordersRolloverStatus"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverProgress2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var rp2 in folioServiceContext.RolloverProgress2s(where, RolloverProgress2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverProgress2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rp2.Id}\t{rp2.Rollover?.Id}\t{rp2.RolloverId}\t{Global.TextEncode(rp2.OverallRolloverStatus)}\t{Global.TextEncode(rp2.BudgetsClosingRolloverStatus)}\t{Global.TextEncode(rp2.FinancialRolloverStatus)}\t{Global.TextEncode(rp2.OrdersRolloverStatus)}\t{rp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rp2.CreationUser?.Username)}\t{rp2.CreationUserId}\t{rp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rp2.LastWriteUser?.Username)}\t{rp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
