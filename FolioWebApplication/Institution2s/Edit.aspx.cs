using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Institution2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Institution2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Institution2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var i2 = folioServiceContext.FindInstitution2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Institution2FormView.DataSource = new[] { i2 };
            Title = $"Institution {i2.Name}";
        }

        protected void Campus2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Campus2sPermission"] == null) return;
            var id = (Guid?)Institution2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "InstitutionId", "institutionId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"institutionId == \"{id}\"",
                Global.GetCqlFilter(Campus2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Campus2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Campus2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Campus2sRadGrid.DataSource = folioServiceContext.Campus2s(where, Campus2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Campus2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Campus2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Campus2sRadGrid.PageSize * Campus2sRadGrid.CurrentPageIndex, Campus2sRadGrid.PageSize, true);
            Campus2sRadGrid.VirtualItemCount = folioServiceContext.CountCampus2s(where);
            if (Campus2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Campus2sRadGrid.AllowFilteringByColumn = Campus2sRadGrid.VirtualItemCount > 10;
                Campus2sPanel.Visible = Institution2FormView.DataKey.Value != null && Session["Campus2sPermission"] != null && Campus2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Location2sPermission"] == null) return;
            var id = (Guid?)Institution2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "IsFloatingCollection", "isFloatingCollection" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"institutionId == \"{id}\"",
                Global.GetCqlFilter(Location2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Location2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Location2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Location2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Location2sRadGrid, "DiscoveryDisplayName", "discoveryDisplayName"),
                Global.GetCqlFilter(Location2sRadGrid, "IsActive", "isActive"),
                Global.GetCqlFilter(Location2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
                Global.GetCqlFilter(Location2sRadGrid, "Library.Name", "libraryId", "name", folioServiceContext.FolioServiceClient.Libraries),
                Global.GetCqlFilter(Location2sRadGrid, "PrimaryServicePoint.Name", "primaryServicePoint", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Location2sRadGrid, "IsFloatingCollection", "isFloatingCollection"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Location2sRadGrid.DataSource = folioServiceContext.Location2s(where, Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Location2sRadGrid.PageSize * Location2sRadGrid.CurrentPageIndex, Location2sRadGrid.PageSize, true);
            Location2sRadGrid.VirtualItemCount = folioServiceContext.CountLocation2s(where);
            if (Location2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Location2sRadGrid.AllowFilteringByColumn = Location2sRadGrid.VirtualItemCount > 10;
                Location2sPanel.Visible = Institution2FormView.DataKey.Value != null && Session["Location2sPermission"] != null && Location2sRadGrid.VirtualItemCount > 0;
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
