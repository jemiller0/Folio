using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Invoice2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Invoice2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Invoice2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var i2 = folioServiceContext.FindInvoice2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Invoice2FormView.DataSource = new[] { i2 };
            Title = $"Invoice {i2.Number}";
        }

        protected void InvoiceItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItem2sPermission"] == null) return;
            var id = (Guid?)Invoice2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNumber" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "Comment", "comment" }, { "Description", "description" }, { "InvoiceId", "invoiceId" }, { "Number", "invoiceLineNumber" }, { "InvoiceLineStatus", "invoiceLineStatus" }, { "OrderItemId", "poLineId" }, { "ProductId", "productId" }, { "ProductIdTypeId", "productIdType" }, { "Quantity", "quantity" }, { "ReleaseEncumbrance", "releaseEncumbrance" }, { "SubscriptionInfo", "subscriptionInfo" }, { "SubscriptionStartDate", "subscriptionStart" }, { "SubscriptionEndDate", "subscriptionEnd" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            InvoiceItem2sRadGrid.DataSource = folioServiceContext.InvoiceItem2s(out var i, Global.GetCqlFilter(InvoiceItem2sRadGrid, d, $"invoiceId == \"{id}\""), InvoiceItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InvoiceItem2sRadGrid.PageSize * InvoiceItem2sRadGrid.CurrentPageIndex, InvoiceItem2sRadGrid.PageSize, true);
            InvoiceItem2sRadGrid.VirtualItemCount = i;
            if (InvoiceItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                InvoiceItem2sRadGrid.AllowFilteringByColumn = InvoiceItem2sRadGrid.VirtualItemCount > 10;
                InvoiceItem2sPanel.Visible = Invoice2FormView.DataKey.Value != null && Session["InvoiceItem2sPermission"] != null && InvoiceItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderInvoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderInvoice2sPermission"] == null) return;
            var id = (Guid?)Invoice2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OrderId", "purchaseOrderId" }, { "InvoiceId", "invoiceId" } };
            OrderInvoice2sRadGrid.DataSource = folioServiceContext.OrderInvoice2s(out var i, Global.GetCqlFilter(OrderInvoice2sRadGrid, d, $"invoiceId == \"{id}\""), OrderInvoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderInvoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderInvoice2sRadGrid.PageSize * OrderInvoice2sRadGrid.CurrentPageIndex, OrderInvoice2sRadGrid.PageSize, true);
            OrderInvoice2sRadGrid.VirtualItemCount = i;
            if (OrderInvoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderInvoice2sRadGrid.AllowFilteringByColumn = OrderInvoice2sRadGrid.VirtualItemCount > 10;
                OrderInvoice2sPanel.Visible = Invoice2FormView.DataKey.Value != null && Session["OrderInvoice2sPermission"] != null && OrderInvoice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)Invoice2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d, $"sourceInvoiceId == \"{id}\""), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = Invoice2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Voucher2sPermission"] == null) return;
            var id = (Guid?)Invoice2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "EnclosureNeeded", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "VendorStreetAddress1", "vendorAddress.addressLine1" }, { "VendorStreetAddress2", "vendorAddress.addressLine2" }, { "VendorCity", "vendorAddress.city" }, { "VendorState", "vendorAddress.stateRegion" }, { "VendorPostalCode", "vendorAddress.zipCode" }, { "VendorCountryCode", "vendorAddress.country" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(out var i, Global.GetCqlFilter(Voucher2sRadGrid, d, $"invoiceId == \"{id}\""), Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = i;
            if (Voucher2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Voucher2sRadGrid.AllowFilteringByColumn = Voucher2sRadGrid.VirtualItemCount > 10;
                Voucher2sPanel.Visible = Invoice2FormView.DataKey.Value != null && Session["Voucher2sPermission"] != null && Voucher2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
