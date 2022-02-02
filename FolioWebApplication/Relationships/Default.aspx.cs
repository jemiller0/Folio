using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Relationships
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RelationshipsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RelationshipsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RelationshipsRadGrid, "Id", "id"),
                Global.GetCqlFilter(RelationshipsRadGrid, "SuperInstance.Title", "superInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(RelationshipsRadGrid, "SubInstance.Title", "subInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(RelationshipsRadGrid, "InstanceRelationshipType.Name", "instanceRelationshipTypeId", "name", folioServiceContext.FolioServiceClient.InstanceRelationshipTypes),
                Global.GetCqlFilter(RelationshipsRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RelationshipsRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RelationshipsRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RelationshipsRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RelationshipsRadGrid.DataSource = folioServiceContext.Relationships(out var i, where, RelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RelationshipsRadGrid.PageSize * RelationshipsRadGrid.CurrentPageIndex, RelationshipsRadGrid.PageSize, true);
            RelationshipsRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Relationships.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tSuperInstance\tSuperInstanceId\tSubInstance\tSubInstanceId\tInstanceRelationshipType\tInstanceRelationshipTypeId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var r in folioServiceContext.Relationships(Global.GetCqlFilter(RelationshipsRadGrid, d), RelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r.Id}\t{Global.TextEncode(r.SuperInstance?.Title)}\t{r.SuperInstanceId}\t{Global.TextEncode(r.SubInstance?.Title)}\t{r.SubInstanceId}\t{Global.TextEncode(r.InstanceRelationshipType?.Name)}\t{r.InstanceRelationshipTypeId}\t{r.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r.CreationUser?.Username)}\t{r.CreationUserId}\t{r.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r.LastWriteUser?.Username)}\t{r.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
