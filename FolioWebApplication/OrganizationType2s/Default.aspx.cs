using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrganizationType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrganizationType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrganizationType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Status", "status" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrganizationType2sRadGrid.DataSource = folioServiceContext.OrganizationType2s(where, OrganizationType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrganizationType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrganizationType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrganizationType2sRadGrid.PageSize * OrganizationType2sRadGrid.CurrentPageIndex, OrganizationType2sRadGrid.PageSize, true);
            OrganizationType2sRadGrid.VirtualItemCount = folioServiceContext.CountOrganizationType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"OrganizationType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tStatus\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Status", "status" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrganizationType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var ot2 in folioServiceContext.OrganizationType2s(where, OrganizationType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrganizationType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrganizationType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ot2.Id}\t{Global.TextEncode(ot2.Name)}\t{Global.TextEncode(ot2.Status)}\t{ot2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ot2.CreationUser?.Username)}\t{ot2.CreationUserId}\t{ot2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ot2.LastWriteUser?.Username)}\t{ot2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
