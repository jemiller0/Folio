using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Voucher2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Voucher2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Voucher2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
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
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(out var i, where, Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Voucher2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tAccountingCode\tAccountNumber\tAmount\tBatchGroup\tBatchGroupId\tDisbursementNumber\tDisbursementDate\tDisbursementAmount\tEnclosure\tInvoiceCurrency\tInvoice\tInvoiceId\tExchangeRate\tExportToAccounting\tStatus\tSystemCurrency\tType\tVoucherDate\tNumber\tVendor\tVendorId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Voucher2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
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
            foreach (var v2 in folioServiceContext.Voucher2s(where, Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{v2.Id}\t{Global.TextEncode(v2.AccountingCode)}\t{Global.TextEncode(v2.AccountNumber)}\t{v2.Amount}\t{Global.TextEncode(v2.BatchGroup?.Name)}\t{v2.BatchGroupId}\t{Global.TextEncode(v2.DisbursementNumber)}\t{v2.DisbursementDate:M/d/yyyy}\t{v2.DisbursementAmount}\t{v2.Enclosure}\t{Global.TextEncode(v2.InvoiceCurrency)}\t{Global.TextEncode(v2.Invoice?.Number)}\t{v2.InvoiceId}\t{v2.ExchangeRate}\t{v2.ExportToAccounting}\t{Global.TextEncode(v2.Status)}\t{Global.TextEncode(v2.SystemCurrency)}\t{Global.TextEncode(v2.Type)}\t{v2.VoucherDate:M/d/yyyy}\t{Global.TextEncode(v2.Number)}\t{Global.TextEncode(v2.Vendor?.Name)}\t{v2.VendorId}\t{v2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(v2.CreationUser?.Username)}\t{v2.CreationUserId}\t{v2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(v2.LastWriteUser?.Username)}\t{v2.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void Voucher2sVoucherItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherItem2sPermission"] == null) return;
            var rg = (RadGrid)sender;
            var id = (Guid?)((GridDataItem)rg.Parent.Parent).GetDataKeyValue("Id");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"voucherId == \"{id}\"",
                Global.GetCqlFilter(rg, "Id", "id"),
                Global.GetCqlFilter(rg, "Amount", "amount"),
                Global.GetCqlFilter(rg, "AccountNumber", "externalAccountNumber"),
                Global.GetCqlFilter(rg, "SubTransaction.Amount", "subTransactionId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(rg, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(rg, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(rg, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(rg, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            rg.DataSource = folioServiceContext.VoucherItem2s(out var i, where, rg.MasterTableView.SortExpressions.Count > 0 ? $"{d[rg.MasterTableView.SortExpressions[0].FieldName]}{(rg.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, rg.PageSize * rg.CurrentPageIndex, rg.PageSize, true);
            rg.VirtualItemCount = i;
            if (rg.MasterTableView.FilterExpression == "")
            {
                rg.AllowFilteringByColumn = rg.VirtualItemCount > 10;
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
