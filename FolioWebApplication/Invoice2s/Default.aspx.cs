using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Invoice2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Invoice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovedBy.Username", "approvedBy", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
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
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ManualPayment", "manualPayment"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(out var i, where, Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Invoice2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tAccountingCode\tAdjustmentsTotal\tApprovedBy\tApprovedById\tApprovalDate\tBatchGroup\tBatchGroupId\tBillTo\tBillToId\tCheckSubscriptionOverlap\tCancellationNote\tCurrency\tEnclosure\tExchangeRate\tExportToAccounting\tNumber\tInvoiceDate\tLockTotal\tNote\tPaymentDueDate\tPaymentDate\tPaymentTerms\tPaymentMethod\tStatus\tSource\tSubTotal\tTotal\tVendorInvoiceNumber\tDisbursementNumber\tVoucherNumber\tPayment\tPaymentId\tDisbursementDate\tVendor\tVendorId\tAccountNumber\tManualPayment\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Invoice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovedBy.Username", "approvedBy", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
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
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ManualPayment", "manualPayment"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var i2 in folioServiceContext.Invoice2s(where, Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{i2.Id}\t{Global.TextEncode(i2.AccountingCode)}\t{i2.AdjustmentsTotal}\t{Global.TextEncode(i2.ApprovedBy?.Username)}\t{i2.ApprovedById}\t{i2.ApprovalDate:M/d/yyyy}\t{Global.TextEncode(i2.BatchGroup?.Name)}\t{i2.BatchGroupId}\t{i2.BillTo?.Id}\t{i2.BillToId}\t{i2.CheckSubscriptionOverlap}\t{Global.TextEncode(i2.CancellationNote)}\t{Global.TextEncode(i2.Currency)}\t{i2.Enclosure}\t{i2.ExchangeRate}\t{i2.ExportToAccounting}\t{Global.TextEncode(i2.Number)}\t{i2.InvoiceDate:M/d/yyyy}\t{i2.LockTotal}\t{Global.TextEncode(i2.Note)}\t{i2.PaymentDueDate:M/d/yyyy}\t{i2.PaymentDate:M/d/yyyy}\t{Global.TextEncode(i2.PaymentTerms)}\t{Global.TextEncode(i2.PaymentMethod)}\t{Global.TextEncode(i2.Status)}\t{Global.TextEncode(i2.Source)}\t{i2.SubTotal}\t{i2.Total}\t{Global.TextEncode(i2.VendorInvoiceNumber)}\t{Global.TextEncode(i2.DisbursementNumber)}\t{Global.TextEncode(i2.VoucherNumber)}\t{i2.Payment?.Amount}\t{i2.PaymentId}\t{i2.DisbursementDate:M/d/yyyy}\t{Global.TextEncode(i2.Vendor?.Name)}\t{i2.VendorId}\t{Global.TextEncode(i2.AccountNumber)}\t{i2.ManualPayment}\t{i2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.CreationUser?.Username)}\t{i2.CreationUserId}\t{i2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastWriteUser?.Username)}\t{i2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
