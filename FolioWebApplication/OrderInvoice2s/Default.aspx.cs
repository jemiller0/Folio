using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderInvoice2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrderInvoice2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrderInvoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OrderId", "purchaseOrderId" }, { "InvoiceId", "invoiceId" } };
            OrderInvoice2sRadGrid.DataSource = folioServiceContext.OrderInvoice2s(out var i, Global.GetCqlFilter(OrderInvoice2sRadGrid, d), OrderInvoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderInvoice2sRadGrid.PageSize * OrderInvoice2sRadGrid.CurrentPageIndex, OrderInvoice2sRadGrid.PageSize, true);
            OrderInvoice2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"OrderInvoice2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OrderId", "purchaseOrderId" }, { "InvoiceId", "invoiceId" } };
            Response.Write("Id\tOrder\tOrderId\tInvoice\tInvoiceId\r\n");
            foreach (var oi2 in folioServiceContext.OrderInvoice2s(Global.GetCqlFilter(OrderInvoice2sRadGrid, d), OrderInvoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{oi2.Id}\t{Global.TextEncode(oi2.Order?.Number)}\t{oi2.OrderId}\t{Global.TextEncode(oi2.Invoice?.Number)}\t{oi2.InvoiceId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
