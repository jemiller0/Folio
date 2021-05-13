using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Transaction2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Transaction2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Transaction2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tAmount\tAwaitingPaymentEncumbrance\tAwaitingPaymentEncumbranceId\tAwaitingPaymentReleaseEncumbrance\tCurrency\tDescription\tAwaitingPaymentAmount\tExpendedAmount\tInitialEncumberedAmount\tStatus\tOrderType\tOrderStatus\tSubscription\tReEncumber\tOrder\tOrderId\tOrderItem\tOrderItemId\tExpenseClass\tExpenseClassId\tFiscalYear\tFiscalYearId\tFromFund\tFromFundId\tPaymentEncumbrance\tPaymentEncumbranceId\tSource\tSourceFiscalYear\tSourceFiscalYearId\tInvoice\tInvoiceId\tInvoiceItem\tInvoiceItemId\tToFund\tToFundId\tTransactionType\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var t2 in folioServiceContext.Transaction2s(Global.GetCqlFilter(Transaction2sRadGrid, d), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{t2.Id}\t{t2.Amount}\t{t2.AwaitingPaymentEncumbrance?.Amount}\t{t2.AwaitingPaymentEncumbranceId}\t{t2.AwaitingPaymentReleaseEncumbrance}\t{Global.TextEncode(t2.Currency)}\t{Global.TextEncode(t2.Description)}\t{t2.AwaitingPaymentAmount}\t{t2.ExpendedAmount}\t{t2.InitialEncumberedAmount}\t{Global.TextEncode(t2.Status)}\t{Global.TextEncode(t2.OrderType)}\t{Global.TextEncode(t2.OrderStatus)}\t{t2.Subscription}\t{t2.ReEncumber}\t{Global.TextEncode(t2.Order?.Number)}\t{t2.OrderId}\t{Global.TextEncode(t2.OrderItem?.Number)}\t{t2.OrderItemId}\t{Global.TextEncode(t2.ExpenseClass?.Name)}\t{t2.ExpenseClassId}\t{Global.TextEncode(t2.FiscalYear?.Name)}\t{t2.FiscalYearId}\t{Global.TextEncode(t2.FromFund?.Name)}\t{t2.FromFundId}\t{t2.PaymentEncumbrance?.Amount}\t{t2.PaymentEncumbranceId}\t{Global.TextEncode(t2.Source)}\t{Global.TextEncode(t2.SourceFiscalYear?.Name)}\t{t2.SourceFiscalYearId}\t{Global.TextEncode(t2.Invoice?.Number)}\t{t2.InvoiceId}\t{Global.TextEncode(t2.InvoiceItem?.Number)}\t{t2.InvoiceItemId}\t{Global.TextEncode(t2.ToFund?.Name)}\t{t2.ToFundId}\t{Global.TextEncode(t2.TransactionType)}\t{t2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.CreationUser?.Username)}\t{t2.CreationUserId}\t{t2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.LastWriteUser?.Username)}\t{t2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
