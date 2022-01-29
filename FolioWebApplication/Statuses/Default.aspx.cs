using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Statuses
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StatusesPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StatusesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(StatusesRadGrid, "Id", "id"),
                Global.GetCqlFilter(StatusesRadGrid, "Code", "code"),
                Global.GetCqlFilter(StatusesRadGrid, "Name", "name"),
                Global.GetCqlFilter(StatusesRadGrid, "Source", "source"),
                Global.GetCqlFilter(StatusesRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(StatusesRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(StatusesRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(StatusesRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            StatusesRadGrid.DataSource = folioServiceContext.Statuses(out var i, where, StatusesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatusesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatusesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StatusesRadGrid.PageSize * StatusesRadGrid.CurrentPageIndex, StatusesRadGrid.PageSize, true);
            StatusesRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Statuses.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tCode\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var s in folioServiceContext.Statuses(Global.GetCqlFilter(StatusesRadGrid, d), StatusesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatusesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatusesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{s.Id}\t{Global.TextEncode(s.Code)}\t{Global.TextEncode(s.Name)}\t{Global.TextEncode(s.Source)}\t{s.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s.CreationUser?.Username)}\t{s.CreationUserId}\t{s.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s.LastWriteUser?.Username)}\t{s.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
