using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Library2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Library2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Library2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var l2 = folioServiceContext.FindLibrary2(id, true);
            if (l2 == null) Response.Redirect("Default.aspx");
            l2.Content = l2.Content != null ? JsonConvert.DeserializeObject<JToken>(l2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Library2FormView.DataSource = new[] { l2 };
            Title = $"Library {l2.Name}";
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Location2sPermission"] == null) return;
            var id = (Guid?)Library2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "IsFloatingCollection", "isFloatingCollection" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"libraryId == \"{id}\"",
                Global.GetCqlFilter(Location2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Location2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Location2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Location2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Location2sRadGrid, "DiscoveryDisplayName", "discoveryDisplayName"),
                Global.GetCqlFilter(Location2sRadGrid, "IsActive", "isActive"),
                Global.GetCqlFilter(Location2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
                Global.GetCqlFilter(Location2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
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
                Location2sPanel.Visible = Library2FormView.DataKey.Value != null && Session["Location2sPermission"] != null && Location2sRadGrid.VirtualItemCount > 0;
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
