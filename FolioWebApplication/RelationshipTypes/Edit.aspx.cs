using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RelationshipTypes
{
    public partial class Edit : System.Web.UI.Page
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

        protected void RelationshipTypeFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var rt = folioServiceContext.FindRelationshipType(id, true);
            if (rt == null) Response.Redirect("Default.aspx");
            rt.Content = rt.Content != null ? JsonConvert.DeserializeObject<JToken>(rt.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            RelationshipTypeFormView.DataSource = new[] { rt };
            Title = $"Relationship Type {rt.Name}";
        }

        protected void InstanceRelationshipsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)RelationshipTypeFormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceRelationshipTypeId == \"{id}\"",
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "Id", "id"),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "SuperInstance.Title", "superInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "SubInstance.Title", "subInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(InstanceRelationshipsRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            InstanceRelationshipsRadGrid.DataSource = folioServiceContext.Relationships(out var i, where, InstanceRelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceRelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceRelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InstanceRelationshipsRadGrid.PageSize * InstanceRelationshipsRadGrid.CurrentPageIndex, InstanceRelationshipsRadGrid.PageSize, true);
            InstanceRelationshipsRadGrid.VirtualItemCount = i;
            if (InstanceRelationshipsRadGrid.MasterTableView.FilterExpression == "")
            {
                InstanceRelationshipsRadGrid.AllowFilteringByColumn = InstanceRelationshipsRadGrid.VirtualItemCount > 10;
                InstanceRelationshipsPanel.Visible = RelationshipTypeFormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && InstanceRelationshipsRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
