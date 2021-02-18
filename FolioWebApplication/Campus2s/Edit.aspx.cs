using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Campus2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Campus2FormView.DataSource = new[] { c2 };
            Title = $"Campus {c2.Name}";
        }

        protected void Library2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Library2sPermission"] == null) return;
            var id = (Guid?)Campus2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CampusId", "campusId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Library2sRadGrid.DataSource = folioServiceContext.Library2s(out var i, Global.GetCqlFilter(Library2sRadGrid, d, $"campusId == \"{id}\""), Library2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Library2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Library2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Library2sRadGrid.PageSize * Library2sRadGrid.CurrentPageIndex, Library2sRadGrid.PageSize, true);
            Library2sRadGrid.VirtualItemCount = i;
            if (Library2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Library2sRadGrid.AllowFilteringByColumn = Library2sRadGrid.VirtualItemCount > 10;
                Library2sPanel.Visible = Campus2FormView.DataKey.Value != null && Session["Library2sPermission"] != null && Library2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Location2sPermission"] == null) return;
            var id = (Guid?)Campus2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Location2sRadGrid.DataSource = folioServiceContext.Location2s(out var i, Global.GetCqlFilter(Location2sRadGrid, d, $"campusId == \"{id}\""), Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Location2sRadGrid.PageSize * Location2sRadGrid.CurrentPageIndex, Location2sRadGrid.PageSize, true);
            Location2sRadGrid.VirtualItemCount = i;
            if (Location2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Location2sRadGrid.AllowFilteringByColumn = Location2sRadGrid.VirtualItemCount > 10;
                Location2sPanel.Visible = Campus2FormView.DataKey.Value != null && Session["Location2sPermission"] != null && Location2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
