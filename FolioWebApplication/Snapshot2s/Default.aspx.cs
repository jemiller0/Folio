using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Snapshot2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Snapshot2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Snapshot2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "jobExecutionId" }, { "Status", "status" }, { "ProcessingStartedDate", "processingStartedDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "CreationTime", "metadata.createdDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastWriteTime", "metadata.updatedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Snapshot2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "ProcessingStartedDate", "processingStartedDate"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Snapshot2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Snapshot2sRadGrid, "LastWriteTime", "metadata.updatedDate")
            }.Where(s => s != null)));
            Snapshot2sRadGrid.DataSource = folioServiceContext.Snapshot2s(out var i, where, Snapshot2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Snapshot2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Snapshot2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Snapshot2sRadGrid.PageSize * Snapshot2sRadGrid.CurrentPageIndex, Snapshot2sRadGrid.PageSize, true);
            Snapshot2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Snapshot2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tStatus\tProcessingStartedDate\tCreationUser\tCreationUserId\tCreationTime\tLastWriteUser\tLastWriteUserId\tLastWriteTime\r\n");
            var d = new Dictionary<string, string>() { { "Id", "jobExecutionId" }, { "Status", "status" }, { "ProcessingStartedDate", "processingStartedDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "CreationTime", "metadata.createdDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastWriteTime", "metadata.updatedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Snapshot2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "ProcessingStartedDate", "processingStartedDate"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Snapshot2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Snapshot2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Snapshot2sRadGrid, "LastWriteTime", "metadata.updatedDate")
            }.Where(s => s != null)));
            foreach (var s2 in folioServiceContext.Snapshot2s(where, Snapshot2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Snapshot2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Snapshot2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{s2.Id}\t{Global.TextEncode(s2.Status)}\t{s2.ProcessingStartedDate:M/d/yyyy}\t{Global.TextEncode(s2.CreationUser?.Username)}\t{s2.CreationUserId}\t{s2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s2.LastWriteUser?.Username)}\t{s2.LastWriteUserId}\t{s2.LastWriteTime:M/d/yyyy HH:mm:ss}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
