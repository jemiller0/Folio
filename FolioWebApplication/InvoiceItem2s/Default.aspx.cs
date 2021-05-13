using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.InvoiceItem2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["InvoiceItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void InvoiceItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNumber" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "Comment", "comment" }, { "Description", "description" }, { "InvoiceId", "invoiceId" }, { "Number", "invoiceLineNumber" }, { "InvoiceLineStatus", "invoiceLineStatus" }, { "OrderItemId", "poLineId" }, { "ProductId", "productId" }, { "ProductIdTypeId", "productIdType" }, { "Quantity", "quantity" }, { "ReleaseEncumbrance", "releaseEncumbrance" }, { "SubscriptionInfo", "subscriptionInfo" }, { "SubscriptionStart", "subscriptionStart" }, { "SubscriptionEnd", "subscriptionEnd" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            InvoiceItem2sRadGrid.DataSource = folioServiceContext.InvoiceItem2s(out var i, Global.GetCqlFilter(InvoiceItem2sRadGrid, d), InvoiceItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InvoiceItem2sRadGrid.PageSize * InvoiceItem2sRadGrid.CurrentPageIndex, InvoiceItem2sRadGrid.PageSize, true);
            InvoiceItem2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"InvoiceItem2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNumber" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "Comment", "comment" }, { "Description", "description" }, { "InvoiceId", "invoiceId" }, { "Number", "invoiceLineNumber" }, { "InvoiceLineStatus", "invoiceLineStatus" }, { "OrderItemId", "poLineId" }, { "ProductId", "productId" }, { "ProductIdTypeId", "productIdType" }, { "Quantity", "quantity" }, { "ReleaseEncumbrance", "releaseEncumbrance" }, { "SubscriptionInfo", "subscriptionInfo" }, { "SubscriptionStart", "subscriptionStart" }, { "SubscriptionEnd", "subscriptionEnd" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tAccountingCode\tAccountNumber\tAdjustmentsTotal\tComment\tDescription\tInvoice\tInvoiceId\tNumber\tInvoiceLineStatus\tOrderItem\tOrderItemId\tProductId\tProductIdType\tProductIdTypeId\tQuantity\tReleaseEncumbrance\tSubscriptionInfo\tSubscriptionStart\tSubscriptionEnd\tSubTotal\tTotal\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ii2 in folioServiceContext.InvoiceItem2s(Global.GetCqlFilter(InvoiceItem2sRadGrid, d), InvoiceItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ii2.Id}\t{Global.TextEncode(ii2.AccountingCode)}\t{Global.TextEncode(ii2.AccountNumber)}\t{ii2.AdjustmentsTotal}\t{Global.TextEncode(ii2.Comment)}\t{Global.TextEncode(ii2.Description)}\t{Global.TextEncode(ii2.Invoice?.Number)}\t{ii2.InvoiceId}\t{Global.TextEncode(ii2.Number)}\t{Global.TextEncode(ii2.InvoiceLineStatus)}\t{Global.TextEncode(ii2.OrderItem?.Number)}\t{ii2.OrderItemId}\t{Global.TextEncode(ii2.ProductId)}\t{Global.TextEncode(ii2.ProductIdType?.Name)}\t{ii2.ProductIdTypeId}\t{ii2.Quantity}\t{ii2.ReleaseEncumbrance}\t{Global.TextEncode(ii2.SubscriptionInfo)}\t{ii2.SubscriptionStart:M/d/yyyy}\t{ii2.SubscriptionEnd:M/d/yyyy}\t{ii2.SubTotal}\t{ii2.Total}\t{ii2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ii2.CreationUser?.Username)}\t{ii2.CreationUserId}\t{ii2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ii2.LastWriteUser?.Username)}\t{ii2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
