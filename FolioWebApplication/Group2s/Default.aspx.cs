using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Group2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Group2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Group2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "group" }, { "Description", "desc" }, { "ExpirationOffsetInDays", "expirationOffsetInDays" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Group2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Group2sRadGrid, "Name", "group"),
                Global.GetCqlFilter(Group2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(Group2sRadGrid, "ExpirationOffsetInDays", "expirationOffsetInDays"),
                Global.GetCqlFilter(Group2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Group2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Group2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Group2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Group2sRadGrid.DataSource = folioServiceContext.Group2s(out var i, where, Group2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Group2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Group2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Group2sRadGrid.PageSize * Group2sRadGrid.CurrentPageIndex, Group2sRadGrid.PageSize, true);
            Group2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Group2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "group" }, { "Description", "desc" }, { "ExpirationOffsetInDays", "expirationOffsetInDays" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tExpirationOffsetInDays\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var g2 in folioServiceContext.Group2s(Global.GetCqlFilter(Group2sRadGrid, d), Group2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Group2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Group2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{g2.Id}\t{Global.TextEncode(g2.Name)}\t{Global.TextEncode(g2.Description)}\t{g2.ExpirationOffsetInDays}\t{g2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(g2.CreationUser?.Username)}\t{g2.CreationUserId}\t{g2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(g2.LastWriteUser?.Username)}\t{g2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
