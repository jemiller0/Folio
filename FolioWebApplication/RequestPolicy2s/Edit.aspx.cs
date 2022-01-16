using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RequestPolicy2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RequestPolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RequestPolicy2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var rp2 = folioServiceContext.FindRequestPolicy2(id, true);
            if (rp2 == null) Response.Redirect("Default.aspx");
            rp2.Content = rp2.Content != null ? JsonConvert.DeserializeObject<JToken>(rp2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            RequestPolicy2FormView.DataSource = new[] { rp2 };
            Title = $"Request Policy {rp2.Name}";
        }

        protected void RequestPolicyRequestTypesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RequestPolicyRequestTypesPermission"] == null) return;
            var id = (Guid?)RequestPolicy2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRequestPolicy2(id).RequestPolicyRequestTypes ?? new RequestPolicyRequestType[] { };
            RequestPolicyRequestTypesRadGrid.DataSource = l;
            RequestPolicyRequestTypesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RequestPolicyRequestTypesPanel.Visible = RequestPolicy2FormView.DataKey.Value != null && ((string)Session["RequestPolicyRequestTypesPermission"] == "Edit" || Session["RequestPolicyRequestTypesPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
