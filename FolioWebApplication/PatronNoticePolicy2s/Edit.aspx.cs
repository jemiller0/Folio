using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PatronNoticePolicy2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PatronNoticePolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PatronNoticePolicy2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var pnp2 = folioServiceContext.FindPatronNoticePolicy2(id, true);
            if (pnp2 == null) Response.Redirect("Default.aspx");
            pnp2.Content = pnp2.Content != null ? JsonConvert.DeserializeObject<JToken>(pnp2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            PatronNoticePolicy2FormView.DataSource = new[] { pnp2 };
            Title = $"Patron Notice Policy {pnp2.Name}";
        }

        protected void PatronNoticePolicyFeeFineNoticesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronNoticePolicyFeeFineNoticesPermission"] == null) return;
            var id = (Guid?)PatronNoticePolicy2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindPatronNoticePolicy2(id, true).PatronNoticePolicyFeeFineNotices ?? new PatronNoticePolicyFeeFineNotice[] { };
            PatronNoticePolicyFeeFineNoticesRadGrid.DataSource = l;
            PatronNoticePolicyFeeFineNoticesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PatronNoticePolicyFeeFineNoticesPanel.Visible = PatronNoticePolicy2FormView.DataKey.Value != null && ((string)Session["PatronNoticePolicyFeeFineNoticesPermission"] == "Edit" || Session["PatronNoticePolicyFeeFineNoticesPermission"] != null && l.Any());
        }

        protected void PatronNoticePolicyLoanNoticesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronNoticePolicyLoanNoticesPermission"] == null) return;
            var id = (Guid?)PatronNoticePolicy2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindPatronNoticePolicy2(id, true).PatronNoticePolicyLoanNotices ?? new PatronNoticePolicyLoanNotice[] { };
            PatronNoticePolicyLoanNoticesRadGrid.DataSource = l;
            PatronNoticePolicyLoanNoticesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PatronNoticePolicyLoanNoticesPanel.Visible = PatronNoticePolicy2FormView.DataKey.Value != null && ((string)Session["PatronNoticePolicyLoanNoticesPermission"] == "Edit" || Session["PatronNoticePolicyLoanNoticesPermission"] != null && l.Any());
        }

        protected void PatronNoticePolicyRequestNoticesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronNoticePolicyRequestNoticesPermission"] == null) return;
            var id = (Guid?)PatronNoticePolicy2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindPatronNoticePolicy2(id, true).PatronNoticePolicyRequestNotices ?? new PatronNoticePolicyRequestNotice[] { };
            PatronNoticePolicyRequestNoticesRadGrid.DataSource = l;
            PatronNoticePolicyRequestNoticesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PatronNoticePolicyRequestNoticesPanel.Visible = PatronNoticePolicy2FormView.DataKey.Value != null && ((string)Session["PatronNoticePolicyRequestNoticesPermission"] == "Edit" || Session["PatronNoticePolicyRequestNoticesPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
