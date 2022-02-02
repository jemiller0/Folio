using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.InstanceNoteType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["InstanceNoteType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void InstanceNoteType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(InstanceNoteType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            InstanceNoteType2sRadGrid.DataSource = folioServiceContext.InstanceNoteType2s(out var i, where, InstanceNoteType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceNoteType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceNoteType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InstanceNoteType2sRadGrid.PageSize * InstanceNoteType2sRadGrid.CurrentPageIndex, InstanceNoteType2sRadGrid.PageSize, true);
            InstanceNoteType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"InstanceNoteType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var int2 in folioServiceContext.InstanceNoteType2s(Global.GetCqlFilter(InstanceNoteType2sRadGrid, d), InstanceNoteType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceNoteType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceNoteType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{int2.Id}\t{Global.TextEncode(int2.Name)}\t{Global.TextEncode(int2.Source)}\t{int2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(int2.CreationUser?.Username)}\t{int2.CreationUserId}\t{int2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(int2.LastWriteUser?.Username)}\t{int2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
