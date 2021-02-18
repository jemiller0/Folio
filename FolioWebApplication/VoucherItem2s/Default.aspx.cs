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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "ExternalAccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            VoucherItem2sRadGrid.DataSource = folioServiceContext.VoucherItem2s(out var i, Global.GetCqlFilter(VoucherItem2sRadGrid, d), VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, VoucherItem2sRadGrid.PageSize * VoucherItem2sRadGrid.CurrentPageIndex, VoucherItem2sRadGrid.PageSize, true);
            VoucherItem2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"VoucherItem2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "ExternalAccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tAmount\tExternalAccountNumber\tSubTransaction\tSubTransactionId\tVoucher\tVoucherId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var vi2 in folioServiceContext.VoucherItem2s(Global.GetCqlFilter(VoucherItem2sRadGrid, d), VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{vi2.Id}\t{vi2.Amount}\t{Global.TextEncode(vi2.ExternalAccountNumber)}\t{vi2.SubTransaction?.Amount}\t{vi2.SubTransactionId}\t{vi2.Voucher?.Id}\t{vi2.VoucherId}\t{vi2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(vi2.CreationUser?.Username)}\t{vi2.CreationUserId}\t{vi2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(vi2.LastWriteUser?.Username)}\t{vi2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
