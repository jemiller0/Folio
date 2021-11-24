using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchGroup2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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

        protected void BatchVoucherExport2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BatchVoucherExport2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Status", "status" }, { "Message", "message" }, { "BatchGroupId", "batchGroupId" }, { "Start", "start" }, { "End", "end" }, { "BatchVoucherId", "batchVoucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BatchVoucherExport2sRadGrid.DataSource = folioServiceContext.BatchVoucherExport2s(out var i, Global.GetCqlFilter(BatchVoucherExport2sRadGrid, d, $"batchGroupId == \"{id}\""), BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BatchVoucherExport2sRadGrid.PageSize * BatchVoucherExport2sRadGrid.CurrentPageIndex, BatchVoucherExport2sRadGrid.PageSize, true);
            BatchVoucherExport2sRadGrid.VirtualItemCount = i;
            if (BatchVoucherExport2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BatchVoucherExport2sRadGrid.AllowFilteringByColumn = BatchVoucherExport2sRadGrid.VirtualItemCount > 10;
                BatchVoucherExport2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["BatchVoucherExport2sPermission"] != null && BatchVoucherExport2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void BatchVoucherExportConfig2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BatchVoucherExportConfig2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BatchGroupId", "batchGroupId" }, { "EnableScheduledExport", "enableScheduledExport" }, { "Format", "format" }, { "StartTime", "startTime" }, { "UploadUri", "uploadURI" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BatchVoucherExportConfig2sRadGrid.DataSource = folioServiceContext.BatchVoucherExportConfig2s(out var i, Global.GetCqlFilter(BatchVoucherExportConfig2sRadGrid, d, $"batchGroupId == \"{id}\""), BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BatchVoucherExportConfig2sRadGrid.PageSize * BatchVoucherExportConfig2sRadGrid.CurrentPageIndex, BatchVoucherExportConfig2sRadGrid.PageSize, true);
            BatchVoucherExportConfig2sRadGrid.VirtualItemCount = i;
            if (BatchVoucherExportConfig2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BatchVoucherExportConfig2sRadGrid.AllowFilteringByColumn = BatchVoucherExportConfig2sRadGrid.VirtualItemCount > 10;
                BatchVoucherExportConfig2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["BatchVoucherExportConfig2sPermission"] != null && BatchVoucherExportConfig2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(out var i, Global.GetCqlFilter(Invoice2sRadGrid, d, $"batchGroupId == \"{id}\""), Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = i;
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Voucher2sPermission"] == null) return;
            var id = (Guid?)BatchGroup2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "VendorStreetAddress1", "vendorAddress.addressLine1" }, { "VendorStreetAddress2", "vendorAddress.addressLine2" }, { "VendorCity", "vendorAddress.city" }, { "VendorState", "vendorAddress.stateRegion" }, { "VendorPostalCode", "vendorAddress.zipCode" }, { "VendorCountryCode", "vendorAddress.country" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(out var i, Global.GetCqlFilter(Voucher2sRadGrid, d, $"batchGroupId == \"{id}\""), Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = i;
            if (Voucher2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Voucher2sRadGrid.AllowFilteringByColumn = Voucher2sRadGrid.VirtualItemCount > 10;
                Voucher2sPanel.Visible = BatchGroup2FormView.DataKey.Value != null && Session["Voucher2sPermission"] != null && Voucher2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
