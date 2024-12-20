using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RolloverBudget2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RolloverBudget2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RolloverBudget2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var rb2 = folioServiceContext.FindRolloverBudget2(id, true);
            if (rb2 == null) Response.Redirect("Default.aspx");
            rb2.Content = rb2.Content != null ? JsonConvert.DeserializeObject<JToken>(rb2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            RolloverBudget2FormView.DataSource = new[] { rb2 };
            Title = $"Rollover Budget {rb2.Name}";
        }

        protected void RolloverBudgetAcquisitionsUnit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetAcquisitionsUnit2sPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetAcquisitionsUnit2s ?? new RolloverBudgetAcquisitionsUnit2[] { };
            RolloverBudgetAcquisitionsUnit2sRadGrid.DataSource = l;
            RolloverBudgetAcquisitionsUnit2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetAcquisitionsUnit2sPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetAcquisitionsUnit2sPermission"] == "Edit" || Session["RolloverBudgetAcquisitionsUnit2sPermission"] != null && l.Any());
        }

        protected void RolloverBudgetAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetAcquisitionsUnits ?? new RolloverBudgetAcquisitionsUnit[] { };
            RolloverBudgetAcquisitionsUnitsRadGrid.DataSource = l;
            RolloverBudgetAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetAcquisitionsUnitsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetAcquisitionsUnitsPermission"] == "Edit" || Session["RolloverBudgetAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetAllocatedFromNamesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetAllocatedFromNamesPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetAllocatedFromNames ?? new RolloverBudgetAllocatedFromName[] { };
            RolloverBudgetAllocatedFromNamesRadGrid.DataSource = l;
            RolloverBudgetAllocatedFromNamesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetAllocatedFromNamesPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetAllocatedFromNamesPermission"] == "Edit" || Session["RolloverBudgetAllocatedFromNamesPermission"] != null && l.Any());
        }

        protected void RolloverBudgetAllocatedToNamesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetAllocatedToNamesPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetAllocatedToNames ?? new RolloverBudgetAllocatedToName[] { };
            RolloverBudgetAllocatedToNamesRadGrid.DataSource = l;
            RolloverBudgetAllocatedToNamesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetAllocatedToNamesPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetAllocatedToNamesPermission"] == "Edit" || Session["RolloverBudgetAllocatedToNamesPermission"] != null && l.Any());
        }

        protected void RolloverBudgetExpenseClassDetailsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetExpenseClassDetailsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetExpenseClassDetails ?? new RolloverBudgetExpenseClassDetail[] { };
            RolloverBudgetExpenseClassDetailsRadGrid.DataSource = l;
            RolloverBudgetExpenseClassDetailsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetExpenseClassDetailsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetExpenseClassDetailsPermission"] == "Edit" || Session["RolloverBudgetExpenseClassDetailsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetFromFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetFromFundsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetFromFunds ?? new RolloverBudgetFromFund[] { };
            RolloverBudgetFromFundsRadGrid.DataSource = l;
            RolloverBudgetFromFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetFromFundsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetFromFundsPermission"] == "Edit" || Session["RolloverBudgetFromFundsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetLocationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetLocationsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetLocations ?? new RolloverBudgetLocation[] { };
            RolloverBudgetLocationsRadGrid.DataSource = l;
            RolloverBudgetLocationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetLocationsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetLocationsPermission"] == "Edit" || Session["RolloverBudgetLocationsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetOrganizationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetOrganizationsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetOrganizations ?? new RolloverBudgetOrganization[] { };
            RolloverBudgetOrganizationsRadGrid.DataSource = l;
            RolloverBudgetOrganizationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetOrganizationsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetOrganizationsPermission"] == "Edit" || Session["RolloverBudgetOrganizationsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetTagsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetTags ?? new RolloverBudgetTag[] { };
            RolloverBudgetTagsRadGrid.DataSource = l;
            RolloverBudgetTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetTagsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetTagsPermission"] == "Edit" || Session["RolloverBudgetTagsPermission"] != null && l.Any());
        }

        protected void RolloverBudgetToFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudgetToFundsPermission"] == null) return;
            var id = (Guid?)RolloverBudget2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRolloverBudget2(id, true).RolloverBudgetToFunds ?? new RolloverBudgetToFund[] { };
            RolloverBudgetToFundsRadGrid.DataSource = l;
            RolloverBudgetToFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RolloverBudgetToFundsPanel.Visible = RolloverBudget2FormView.DataKey.Value != null && ((string)Session["RolloverBudgetToFundsPermission"] == "Edit" || Session["RolloverBudgetToFundsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
