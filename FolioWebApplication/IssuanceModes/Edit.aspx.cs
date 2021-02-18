using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.IssuanceModes
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IssuanceModesPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IssuanceModeFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var im = folioServiceContext.FindIssuanceMode(id, true);
            if (im == null) Response.Redirect("Default.aspx");
            IssuanceModeFormView.DataSource = new[] { im };
            Title = $"Issuance Mode {im.Name}";
        }

        protected void Instance2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Instance2sPermission"] == null) return;
            var id = (Guid?)IssuanceModeFormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors.0.name" }, { "PublicationYear", "publication.0.dateOfPublication" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Instance2sRadGrid.DataSource = folioServiceContext.Instance2s(out var i, Global.GetCqlFilter(Instance2sRadGrid, d, $"modeOfIssuanceId == \"{id}\""), Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Instance2sRadGrid.PageSize * Instance2sRadGrid.CurrentPageIndex, Instance2sRadGrid.PageSize, true);
            Instance2sRadGrid.VirtualItemCount = i;
            if (Instance2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Instance2sRadGrid.AllowFilteringByColumn = Instance2sRadGrid.VirtualItemCount > 10;
                Instance2sPanel.Visible = IssuanceModeFormView.DataKey.Value != null && ((string)Session["Instance2sPermission"] == "Edit" || Session["Instance2sPermission"] != null && Instance2sRadGrid.VirtualItemCount > 0);
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
