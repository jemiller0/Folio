using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Campus2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Campus2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Campus2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var c2 = folioServiceContext.FindCampus2(id, true);
            if (c2 == null) Response.Redirect("Default.aspx");
            c2.Content = c2.Content != null ? JsonConvert.DeserializeObject<JToken>(c2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Campus2FormView.DataSource = new[] { c2 };
            Title = $"Campus {c2.Name}";
        }

        protected void Library2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Library2sPermission"] == null) return;
            var id = (Guid?)Campus2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CampusId", "campusId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"campusId == \"{id}\"",
                Global.GetCqlFilter(Library2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Library2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Library2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Library2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Library2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Library2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Library2sRadGrid.DataSource = folioServiceContext.Library2s(where, Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Library2sRadGrid.PageSize * Library2sRadGrid.CurrentPageIndex, Library2sRadGrid.PageSize, true);
            Library2sRadGrid.VirtualItemCount = folioServiceContext.CountLibrary2s(where);
            if (Library2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Library2sRadGrid.AllowFilteringByColumn = Library2sRadGrid.VirtualItemCount > 10;
                Library2sPanel.Visible = Campus2FormView.DataKey.Value != null && Session["Library2sPermission"] != null && Library2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Location2sPermission"] == null) return;
            var id = (Guid?)Campus2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "IsFloatingCollection", "isFloatingCollection" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"campusId == \"{id}\"",
                Global.GetCqlFilter(Location2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Location2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Location2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Location2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Location2sRadGrid, "DiscoveryDisplayName", "discoveryDisplayName"),
                Global.GetCqlFilter(Location2sRadGrid, "IsActive", "isActive"),
                Global.GetCqlFilter(Location2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
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
                Location2sPanel.Visible = Campus2FormView.DataKey.Value != null && Session["Location2sPermission"] != null && Location2sRadGrid.VirtualItemCount > 0;
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
