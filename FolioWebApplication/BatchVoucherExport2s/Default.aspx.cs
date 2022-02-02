using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchVoucherExport2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BatchVoucherExport2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BatchVoucherExport2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Status", "status" }, { "Message", "message" }, { "BatchGroupId", "batchGroupId" }, { "Start", "start" }, { "End", "end" }, { "BatchVoucherId", "batchVoucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Message", "message"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Start", "start"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "End", "end"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BatchVoucherExport2sRadGrid.DataSource = folioServiceContext.BatchVoucherExport2s(out var i, where, BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BatchVoucherExport2sRadGrid.PageSize * BatchVoucherExport2sRadGrid.CurrentPageIndex, BatchVoucherExport2sRadGrid.PageSize, true);
            BatchVoucherExport2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BatchVoucherExport2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tStatus\tMessage\tBatchGroup\tBatchGroupId\tStart\tEnd\tBatchVoucher\tBatchVoucherId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Status", "status" }, { "Message", "message" }, { "BatchGroupId", "batchGroupId" }, { "Start", "start" }, { "End", "end" }, { "BatchVoucherId", "batchVoucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Message", "message"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "Start", "start"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "End", "end"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BatchVoucherExport2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var bve2 in folioServiceContext.BatchVoucherExport2s(where, BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExport2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bve2.Id}\t{Global.TextEncode(bve2.Status)}\t{Global.TextEncode(bve2.Message)}\t{Global.TextEncode(bve2.BatchGroup?.Name)}\t{bve2.BatchGroupId}\t{bve2.Start:M/d/yyyy}\t{bve2.End:M/d/yyyy}\t{bve2.BatchVoucher?.Id}\t{bve2.BatchVoucherId}\t{bve2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bve2.CreationUser?.Username)}\t{bve2.CreationUserId}\t{bve2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bve2.LastWriteUser?.Username)}\t{bve2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
