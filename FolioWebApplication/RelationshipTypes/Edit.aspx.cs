using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.RelationshipTypes
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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

        protected void RelationshipTypeFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var rt = folioServiceContext.FindRelationshipType(id, true);
            if (rt == null) Response.Redirect("Default.aspx");
            RelationshipTypeFormView.DataSource = new[] { rt };
            Title = $"Relationship Type {rt.Name}";
        }

        protected void InstanceRelationshipsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)RelationshipTypeFormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            InstanceRelationshipsRadGrid.DataSource = folioServiceContext.Relationships(out var i, Global.GetCqlFilter(InstanceRelationshipsRadGrid, d, $"instanceRelationshipTypeId == \"{id}\""), InstanceRelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceRelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceRelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InstanceRelationshipsRadGrid.PageSize * InstanceRelationshipsRadGrid.CurrentPageIndex, InstanceRelationshipsRadGrid.PageSize, true);
            InstanceRelationshipsRadGrid.VirtualItemCount = i;
            if (InstanceRelationshipsRadGrid.MasterTableView.FilterExpression == "")
            {
                InstanceRelationshipsRadGrid.AllowFilteringByColumn = InstanceRelationshipsRadGrid.VirtualItemCount > 10;
                InstanceRelationshipsPanel.Visible = RelationshipTypeFormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && InstanceRelationshipsRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
