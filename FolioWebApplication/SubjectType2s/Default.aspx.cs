using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.SubjectType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SubjectType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void SubjectType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(SubjectType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(SubjectType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            SubjectType2sRadGrid.DataSource = folioServiceContext.SubjectType2s(where, SubjectType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[SubjectType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(SubjectType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, SubjectType2sRadGrid.PageSize * SubjectType2sRadGrid.CurrentPageIndex, SubjectType2sRadGrid.PageSize, true);
            SubjectType2sRadGrid.VirtualItemCount = folioServiceContext.CountSubjectType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"SubjectType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(SubjectType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(SubjectType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(SubjectType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var st2 in folioServiceContext.SubjectType2s(where, SubjectType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[SubjectType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(SubjectType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{st2.Id}\t{Global.TextEncode(st2.Name)}\t{Global.TextEncode(st2.Source)}\t{st2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(st2.CreationUser?.Username)}\t{st2.CreationUserId}\t{st2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(st2.LastWriteUser?.Username)}\t{st2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
