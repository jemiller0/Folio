using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Interface2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Interface2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Interface2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var i2 = folioServiceContext.FindInterface2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Interface2FormView.DataSource = new[] { i2 };
            Title = $"Interface {i2.Name}";
        }

        protected void InterfaceTypesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InterfaceTypesPermission"] == null) return;
            var id = (Guid?)Interface2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInterface2(id, true).InterfaceTypes ?? new InterfaceType[] { };
            InterfaceTypesRadGrid.DataSource = l;
            InterfaceTypesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InterfaceTypesPanel.Visible = Interface2FormView.DataKey.Value != null && ((string)Session["InterfaceTypesPermission"] == "Edit" || Session["InterfaceTypesPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
