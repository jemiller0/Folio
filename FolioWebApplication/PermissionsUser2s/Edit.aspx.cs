using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PermissionsUser2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PermissionsUser2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PermissionsUser2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var pu2 = folioServiceContext.FindPermissionsUser2(id, true);
            if (pu2 == null) Response.Redirect("Default.aspx");
            pu2.Content = pu2.Content != null ? JsonConvert.DeserializeObject<JToken>(pu2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            PermissionsUser2FormView.DataSource = new[] { pu2 };
            Title = $"Permissions User {pu2.Id}";
        }

        protected void PermissionsUserPermissionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PermissionsUserPermissionsPermission"] == null) return;
            var id = (Guid?)PermissionsUser2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindPermissionsUser2(id).PermissionsUserPermissions ?? new PermissionsUserPermission[] { };
            PermissionsUserPermissionsRadGrid.DataSource = l;
            PermissionsUserPermissionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PermissionsUserPermissionsPanel.Visible = PermissionsUser2FormView.DataKey.Value != null && ((string)Session["PermissionsUserPermissionsPermission"] == "Edit" || Session["PermissionsUserPermissionsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
