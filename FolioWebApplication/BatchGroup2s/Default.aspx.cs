using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchGroup2s
{
    public partial class Default : System.Web.UI.Page
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

        protected void BatchGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BatchGroup2sRadGrid.DataSource = folioServiceContext.BatchGroup2s(where, BatchGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BatchGroup2sRadGrid.PageSize * BatchGroup2sRadGrid.CurrentPageIndex, BatchGroup2sRadGrid.PageSize, true);
            BatchGroup2sRadGrid.VirtualItemCount = folioServiceContext.CountBatchGroup2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BatchGroup2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BatchGroup2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var bg2 in folioServiceContext.BatchGroup2s(where, BatchGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BatchGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BatchGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bg2.Id}\t{Global.TextEncode(bg2.Name)}\t{Global.TextEncode(bg2.Description)}\t{bg2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bg2.CreationUser?.Username)}\t{bg2.CreationUserId}\t{bg2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bg2.LastWriteUser?.Username)}\t{bg2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
