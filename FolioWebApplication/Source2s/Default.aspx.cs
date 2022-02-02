using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Source2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Source2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Source2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Source2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Source2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Source2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Source2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Source2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Source2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Source2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Source2sRadGrid.DataSource = folioServiceContext.Source2s(out var i, where, Source2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Source2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Source2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Source2sRadGrid.PageSize * Source2sRadGrid.CurrentPageIndex, Source2sRadGrid.PageSize, true);
            Source2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Source2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Source2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Source2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Source2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Source2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Source2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Source2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Source2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var s2 in folioServiceContext.Source2s(where, Source2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Source2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Source2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{s2.Id}\t{Global.TextEncode(s2.Name)}\t{Global.TextEncode(s2.Source)}\t{s2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s2.CreationUser?.Username)}\t{s2.CreationUserId}\t{s2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s2.LastWriteUser?.Username)}\t{s2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
