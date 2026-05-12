using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrganizationType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrganizationType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrganizationType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ot2 = folioServiceContext.FindOrganizationType2(id, true);
            if (ot2 == null) Response.Redirect("Default.aspx");
            ot2.Content = ot2.Content != null ? JsonConvert.DeserializeObject<JToken>(ot2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            OrganizationType2FormView.DataSource = new[] { ot2 };
            Title = $"Organization Type {ot2.Name}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
