using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PrecedingSucceedingTitle2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PrecedingSucceedingTitle2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var pst2 = folioServiceContext.FindPrecedingSucceedingTitle2(id, true);
            if (pst2 == null) Response.Redirect("Default.aspx");
            pst2.Content = pst2.Content != null ? JsonConvert.DeserializeObject<JToken>(pst2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            PrecedingSucceedingTitle2FormView.DataSource = new[] { pst2 };
            Title = $"Preceding Succeeding Title {pst2.Title}";
        }

        protected void PrecedingSucceedingTitleIdentifiersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PrecedingSucceedingTitleIdentifiersPermission"] == null) return;
            var id = (Guid?)PrecedingSucceedingTitle2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindPrecedingSucceedingTitle2(id, true).PrecedingSucceedingTitleIdentifiers ?? new PrecedingSucceedingTitleIdentifier[] { };
            PrecedingSucceedingTitleIdentifiersRadGrid.DataSource = l;
            PrecedingSucceedingTitleIdentifiersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PrecedingSucceedingTitleIdentifiersPanel.Visible = PrecedingSucceedingTitle2FormView.DataKey.Value != null && ((string)Session["PrecedingSucceedingTitleIdentifiersPermission"] == "Edit" || Session["PrecedingSucceedingTitleIdentifiersPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
