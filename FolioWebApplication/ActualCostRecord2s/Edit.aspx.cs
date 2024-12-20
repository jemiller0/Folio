using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ActualCostRecord2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ActualCostRecord2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var acr2 = folioServiceContext.FindActualCostRecord2(id, true);
            if (acr2 == null) Response.Redirect("Default.aspx");
            acr2.Content = acr2.Content != null ? JsonConvert.DeserializeObject<JToken>(acr2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            ActualCostRecord2FormView.DataSource = new[] { acr2 };
            Title = $"Actual Cost Record {acr2.Id}";
        }

        protected void ActualCostRecordContributorsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecordContributorsPermission"] == null) return;
            var id = (Guid?)ActualCostRecord2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindActualCostRecord2(id, true).ActualCostRecordContributors ?? new ActualCostRecordContributor[] { };
            ActualCostRecordContributorsRadGrid.DataSource = l;
            ActualCostRecordContributorsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ActualCostRecordContributorsPanel.Visible = ActualCostRecord2FormView.DataKey.Value != null && ((string)Session["ActualCostRecordContributorsPermission"] == "Edit" || Session["ActualCostRecordContributorsPermission"] != null && l.Any());
        }

        protected void ActualCostRecordIdentifiersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecordIdentifiersPermission"] == null) return;
            var id = (Guid?)ActualCostRecord2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindActualCostRecord2(id, true).ActualCostRecordIdentifiers ?? new ActualCostRecordIdentifier[] { };
            ActualCostRecordIdentifiersRadGrid.DataSource = l;
            ActualCostRecordIdentifiersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ActualCostRecordIdentifiersPanel.Visible = ActualCostRecord2FormView.DataKey.Value != null && ((string)Session["ActualCostRecordIdentifiersPermission"] == "Edit" || Session["ActualCostRecordIdentifiersPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
