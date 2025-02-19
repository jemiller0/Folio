using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchGroup2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BatchGroup2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BatchGroup2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var bg2 = folioServiceContext.FindBatchGroup2(id, true);
            if (bg2 == null) Response.Redirect("Default.aspx");
            bg2.Content = bg2.Content != null ? JsonConvert.DeserializeObject<JToken>(bg2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            BatchGroup2FormView.DataSource = new[] { bg2 };
            Title = $"Batch Group {bg2.Name}";
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "FiscalYearId", "fiscalYearId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "NextInvoiceLineNumber", "nextInvoiceLineNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"batchGroupId == \"{id}\"",
                Global.GetCqlFilter(Invoice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovedBy.Username", "approvedBy", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "BillTo.Id", "billTo", "id", folioServiceContext.FolioServiceClient.Configurations),
                Global.GetCqlFilter(Invoice2sRadGrid, "CheckSubscriptionOverlap", "chkSubscriptionOverlap"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CancellationNote", "cancellationNote"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Enclosure", "enclosureNeeded"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExchangeRate", "exchangeRate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Number", "folioInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "InvoiceDate", "invoiceDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LockTotal", "lockTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Note", "note"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDueDate", "paymentDue"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDate", "paymentDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentTerms", "paymentTerms"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Invoice2sRadGrid, "SubTotal", "subTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Total", "total"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VendorInvoiceNumber", "vendorInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementNumber", "disbursementNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VoucherNumber", "voucherNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Payment.Amount", "paymentId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementDate", "disbursementDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Vendor.Name", "vendorId", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Invoice2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ManualPayment", "manualPayment"),
                Global.GetCqlFilter(Invoice2sRadGrid, "NextInvoiceLineNumber", "nextInvoiceLineNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(where, Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = folioServiceContext.CountInvoice2s(where);
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Voucher2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"batchGroupId == \"{id}\"",
                Global.GetCqlFilter(Voucher2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementNumber", "disbursementNumber"),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementDate", "disbursementDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementAmount", "disbursementAmount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Enclosure", "enclosureNeeded"),
                Global.GetCqlFilter(Voucher2sRadGrid, "InvoiceCurrency", "invoiceCurrency"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Invoice.Number", "invoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Voucher2sRadGrid, "ExchangeRate", "exchangeRate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Voucher2sRadGrid, "SystemCurrency", "systemCurrency"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Type", "type"),
                Global.GetCqlFilter(Voucher2sRadGrid, "VoucherDate", "voucherDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Number", "voucherNumber"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Vendor.Name", "vendorId", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Voucher2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Voucher2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(where, Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = folioServiceContext.CountVoucher2s(where);
            if (Voucher2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Voucher2sRadGrid.AllowFilteringByColumn = Voucher2sRadGrid.VirtualItemCount > 10;
                Voucher2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["Voucher2sPermission"] != null && Voucher2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
