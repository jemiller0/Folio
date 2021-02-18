using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Library2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Library2FormView.DataSource = new[] { l2 };
            Title = $"Library {l2.Name}";
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Location2sPermission"] == null) return;
            var id = (Guid?)Library2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Location2sRadGrid.DataSource = folioServiceContext.Location2s(out var i, Global.GetCqlFilter(Location2sRadGrid, d, $"libraryId == \"{id}\""), Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Location2sRadGrid.PageSize * Location2sRadGrid.CurrentPageIndex, Location2sRadGrid.PageSize, true);
            Location2sRadGrid.VirtualItemCount = i;
            if (Location2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Location2sRadGrid.AllowFilteringByColumn = Location2sRadGrid.VirtualItemCount > 10;
                Location2sPanel.Visible = Library2FormView.DataKey.Value != null && Session["Location2sPermission"] != null && Location2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
