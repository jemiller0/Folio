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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "EnclosureNeeded", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "VoucherNumber", "voucherNumber" }, { "VendorId", "vendorId" }, { "VendorStreetAddress1", "vendorAddress.addressLine1" }, { "VendorStreetAddress2", "vendorAddress.addressLine2" }, { "VendorCity", "vendorAddress.city" }, { "VendorState", "vendorAddress.stateRegion" }, { "VendorPostalCode", "vendorAddress.zipCode" }, { "VendorCountryCode", "vendorAddress.country" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(out var i, Global.GetCqlFilter(Voucher2sRadGrid, d), Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Voucher2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "EnclosureNeeded", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "VoucherNumber", "voucherNumber" }, { "VendorId", "vendorId" }, { "VendorStreetAddress1", "vendorAddress.addressLine1" }, { "VendorStreetAddress2", "vendorAddress.addressLine2" }, { "VendorCity", "vendorAddress.city" }, { "VendorState", "vendorAddress.stateRegion" }, { "VendorPostalCode", "vendorAddress.zipCode" }, { "VendorCountryCode", "vendorAddress.country" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tAccountingCode\tAccountNumber\tAmount\tBatchGroup\tBatchGroupId\tDisbursementNumber\tDisbursementDate\tDisbursementAmount\tEnclosureNeeded\tInvoiceCurrency\tInvoice\tInvoiceId\tExchangeRate\tExportToAccounting\tStatus\tSystemCurrency\tType\tVoucherDate\tVoucherNumber\tVendor\tVendorId\tVendorStreetAddress1\tVendorStreetAddress2\tVendorCity\tVendorState\tVendorPostalCode\tVendorCountryCode\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var v2 in folioServiceContext.Voucher2s(Global.GetCqlFilter(Voucher2sRadGrid, d), Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{v2.Id}\t{Global.TextEncode(v2.AccountingCode)}\t{Global.TextEncode(v2.AccountNumber)}\t{v2.Amount}\t{Global.TextEncode(v2.BatchGroup?.Name)}\t{v2.BatchGroupId}\t{Global.TextEncode(v2.DisbursementNumber)}\t{v2.DisbursementDate:M/d/yyyy}\t{v2.DisbursementAmount}\t{v2.EnclosureNeeded}\t{Global.TextEncode(v2.InvoiceCurrency)}\t{Global.TextEncode(v2.Invoice?.Number)}\t{v2.InvoiceId}\t{v2.ExchangeRate}\t{v2.ExportToAccounting}\t{Global.TextEncode(v2.Status)}\t{Global.TextEncode(v2.SystemCurrency)}\t{Global.TextEncode(v2.Type)}\t{v2.VoucherDate:M/d/yyyy}\t{Global.TextEncode(v2.VoucherNumber)}\t{Global.TextEncode(v2.Vendor?.Name)}\t{v2.VendorId}\t{Global.TextEncode(v2.VendorStreetAddress1)}\t{Global.TextEncode(v2.VendorStreetAddress2)}\t{Global.TextEncode(v2.VendorCity)}\t{Global.TextEncode(v2.VendorState)}\t{Global.TextEncode(v2.VendorPostalCode)}\t{Global.TextEncode(v2.VendorCountryCode)}\t{v2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(v2.CreationUser?.Username)}\t{v2.CreationUserId}\t{v2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(v2.LastWriteUser?.Username)}\t{v2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
