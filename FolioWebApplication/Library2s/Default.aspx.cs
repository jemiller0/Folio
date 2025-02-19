using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Library2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Library2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Library2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CampusId", "campusId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Library2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Library2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Library2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Library2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
                Global.GetCqlFilter(Library2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Library2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Library2sRadGrid.DataSource = folioServiceContext.Library2s(where, Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Library2sRadGrid.PageSize * Library2sRadGrid.CurrentPageIndex, Library2sRadGrid.PageSize, true);
            Library2sRadGrid.VirtualItemCount = folioServiceContext.CountLibrary2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Library2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCode\tCampus\tCampusId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CampusId", "campusId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Library2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Library2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Library2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Library2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
                Global.GetCqlFilter(Library2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Library2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var l2 in folioServiceContext.Library2s(where, Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{l2.Id}\t{Global.TextEncode(l2.Name)}\t{Global.TextEncode(l2.Code)}\t{Global.TextEncode(l2.Campus?.Name)}\t{l2.CampusId}\t{l2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.CreationUser?.Username)}\t{l2.CreationUserId}\t{l2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.LastWriteUser?.Username)}\t{l2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
