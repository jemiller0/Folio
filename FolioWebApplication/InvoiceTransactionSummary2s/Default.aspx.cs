using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.InvoiceTransactionSummary2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["InvoiceTransactionSummary2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void InvoiceTransactionSummary2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "NumPendingPayments", "numPendingPayments" }, { "NumPaymentsCredits", "numPaymentsCredits" } };
            InvoiceTransactionSummary2sRadGrid.DataSource = folioServiceContext.InvoiceTransactionSummary2s(out var i, Global.GetCqlFilter(InvoiceTransactionSummary2sRadGrid, d), InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InvoiceTransactionSummary2sRadGrid.PageSize * InvoiceTransactionSummary2sRadGrid.CurrentPageIndex, InvoiceTransactionSummary2sRadGrid.PageSize, true);
            InvoiceTransactionSummary2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"InvoiceTransactionSummary2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "NumPendingPayments", "numPendingPayments" }, { "NumPaymentsCredits", "numPaymentsCredits" } };
            Response.Write("Id\tInvoice2\tNumPendingPayments\tNumPaymentsCredits\r\n");
            foreach (var its2 in folioServiceContext.InvoiceTransactionSummary2s(Global.GetCqlFilter(InvoiceTransactionSummary2sRadGrid, d), InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceTransactionSummary2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{its2.Id}\t{Global.TextEncode(its2.Invoice2?.Number)}\t{its2.NumPendingPayments}\t{its2.NumPaymentsCredits}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
