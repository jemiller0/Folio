using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchVoucherExportConfig2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BatchVoucherExportConfig2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BatchVoucherExportConfig2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BatchGroupId", "batchGroupId" }, { "EnableScheduledExport", "enableScheduledExport" }, { "Format", "format" }, { "StartTime", "startTime" }, { "UploadUri", "uploadURI" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BatchVoucherExportConfig2sRadGrid.DataSource = folioServiceContext.BatchVoucherExportConfig2s(out var i, Global.GetCqlFilter(BatchVoucherExportConfig2sRadGrid, d), BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BatchVoucherExportConfig2sRadGrid.PageSize * BatchVoucherExportConfig2sRadGrid.CurrentPageIndex, BatchVoucherExportConfig2sRadGrid.PageSize, true);
            BatchVoucherExportConfig2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BatchVoucherExportConfig2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BatchGroupId", "batchGroupId" }, { "EnableScheduledExport", "enableScheduledExport" }, { "Format", "format" }, { "StartTime", "startTime" }, { "UploadUri", "uploadURI" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tBatchGroup\tBatchGroupId\tEnableScheduledExport\tFormat\tStartTime\tUploadUri\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var bvec2 in folioServiceContext.BatchVoucherExportConfig2s(Global.GetCqlFilter(BatchVoucherExportConfig2sRadGrid, d), BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchVoucherExportConfig2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bvec2.Id}\t{Global.TextEncode(bvec2.BatchGroup?.Name)}\t{bvec2.BatchGroupId}\t{bvec2.EnableScheduledExport}\t{Global.TextEncode(bvec2.Format)}\t{Global.TextEncode(bvec2.StartTime)}\t{Global.TextEncode(bvec2.UploadUri)}\t{bvec2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bvec2.CreationUser?.Username)}\t{bvec2.CreationUserId}\t{bvec2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bvec2.LastWriteUser?.Username)}\t{bvec2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
