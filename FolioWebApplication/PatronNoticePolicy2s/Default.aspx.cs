using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PatronNoticePolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PatronNoticePolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PatronNoticePolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Active", "active" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PatronNoticePolicy2sRadGrid.DataSource = folioServiceContext.PatronNoticePolicy2s(out var i, where, PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PatronNoticePolicy2sRadGrid.PageSize * PatronNoticePolicy2sRadGrid.CurrentPageIndex, PatronNoticePolicy2sRadGrid.PageSize, true);
            PatronNoticePolicy2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"PatronNoticePolicy2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Active", "active" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tActive\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var pnp2 in folioServiceContext.PatronNoticePolicy2s(Global.GetCqlFilter(PatronNoticePolicy2sRadGrid, d), PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronNoticePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{pnp2.Id}\t{Global.TextEncode(pnp2.Name)}\t{Global.TextEncode(pnp2.Description)}\t{pnp2.Active}\t{pnp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pnp2.CreationUser?.Username)}\t{pnp2.CreationUserId}\t{pnp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pnp2.LastWriteUser?.Username)}\t{pnp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
