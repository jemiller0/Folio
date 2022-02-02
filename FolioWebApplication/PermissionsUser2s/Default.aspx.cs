using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PermissionsUser2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PermissionsUser2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PermissionsUser2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PermissionsUser2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PermissionsUser2sRadGrid.DataSource = folioServiceContext.PermissionsUser2s(out var i, where, PermissionsUser2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PermissionsUser2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PermissionsUser2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PermissionsUser2sRadGrid.PageSize * PermissionsUser2sRadGrid.CurrentPageIndex, PermissionsUser2sRadGrid.PageSize, true);
            PermissionsUser2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"PermissionsUser2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tUser\tUserId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var pu2 in folioServiceContext.PermissionsUser2s(Global.GetCqlFilter(PermissionsUser2sRadGrid, d), PermissionsUser2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PermissionsUser2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PermissionsUser2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{pu2.Id}\t{Global.TextEncode(pu2.User?.Username)}\t{pu2.UserId}\t{pu2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pu2.CreationUser?.Username)}\t{pu2.CreationUserId}\t{pu2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pu2.LastWriteUser?.Username)}\t{pu2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
