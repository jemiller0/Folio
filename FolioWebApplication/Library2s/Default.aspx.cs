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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Library2sRadGrid.DataSource = folioServiceContext.Library2s(out var i, Global.GetCqlFilter(Library2sRadGrid, d), Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Library2sRadGrid.PageSize * Library2sRadGrid.CurrentPageIndex, Library2sRadGrid.PageSize, true);
            Library2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Library2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CampusId", "campusId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tCampus\tCampusId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var l2 in folioServiceContext.Library2s(Global.GetCqlFilter(Library2sRadGrid, d), Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
