using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RelationshipTypes
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RelationshipTypesPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RelationshipTypesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RelationshipTypesRadGrid, "Id", "id"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "Name", "name"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RelationshipTypesRadGrid.DataSource = folioServiceContext.RelationshipTypes(out var i, where, RelationshipTypesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipTypesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipTypesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RelationshipTypesRadGrid.PageSize * RelationshipTypesRadGrid.CurrentPageIndex, RelationshipTypesRadGrid.PageSize, true);
            RelationshipTypesRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"RelationshipTypes.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RelationshipTypesRadGrid, "Id", "id"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "Name", "name"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RelationshipTypesRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var rt in folioServiceContext.RelationshipTypes(where, RelationshipTypesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipTypesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipTypesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rt.Id}\t{Global.TextEncode(rt.Name)}\t{rt.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rt.CreationUser?.Username)}\t{rt.CreationUserId}\t{rt.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rt.LastWriteUser?.Username)}\t{rt.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
