using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.VoucherItem2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["VoucherItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void VoucherItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "AccountNumber", "externalAccountNumber"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Voucher.Number", "voucherId", "voucherNumber", folioServiceContext.FolioServiceClient.Vouchers),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            VoucherItem2sRadGrid.DataSource = folioServiceContext.VoucherItem2s(where, VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, VoucherItem2sRadGrid.PageSize * VoucherItem2sRadGrid.CurrentPageIndex, VoucherItem2sRadGrid.PageSize, true);
            VoucherItem2sRadGrid.VirtualItemCount = folioServiceContext.CountVoucherItem2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"VoucherItem2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tAmount\tAccountNumber\tSubTransactionId\tVoucher\tVoucherId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "AccountNumber", "externalAccountNumber"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Voucher.Number", "voucherId", "voucherNumber", folioServiceContext.FolioServiceClient.Vouchers),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var vi2 in folioServiceContext.VoucherItem2s(where, VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{vi2.Id}\t{vi2.Amount}\t{Global.TextEncode(vi2.AccountNumber)}\t{vi2.SubTransactionId}\t{Global.TextEncode(vi2.Voucher?.Number)}\t{vi2.VoucherId}\t{vi2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(vi2.CreationUser?.Username)}\t{vi2.CreationUserId}\t{vi2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(vi2.LastWriteUser?.Username)}\t{vi2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
