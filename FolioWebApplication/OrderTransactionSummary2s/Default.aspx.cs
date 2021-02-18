using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderTransactionSummary2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrderTransactionSummary2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrderTransactionSummary2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "NumTransactions", "numTransactions" } };
            OrderTransactionSummary2sRadGrid.DataSource = folioServiceContext.OrderTransactionSummary2s(out var i, Global.GetCqlFilter(OrderTransactionSummary2sRadGrid, d), OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderTransactionSummary2sRadGrid.PageSize * OrderTransactionSummary2sRadGrid.CurrentPageIndex, OrderTransactionSummary2sRadGrid.PageSize, true);
            OrderTransactionSummary2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"OrderTransactionSummary2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "NumTransactions", "numTransactions" } };
            Response.Write("Id\tOrder2\tNumTransactions\r\n");
            foreach (var ots2 in folioServiceContext.OrderTransactionSummary2s(Global.GetCqlFilter(OrderTransactionSummary2sRadGrid, d), OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ots2.Id}\t{Global.TextEncode(ots2.Order2?.Number)}\t{ots2.NumTransactions}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
