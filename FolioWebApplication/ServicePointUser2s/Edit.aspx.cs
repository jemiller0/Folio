using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ServicePointUser2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ServicePointUser2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ServicePointUser2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var spu2 = folioServiceContext.FindServicePointUser2(id, true);
            if (spu2 == null) Response.Redirect("Default.aspx");
            spu2.Content = spu2.Content != null ? JsonConvert.DeserializeObject<JToken>(spu2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            ServicePointUser2FormView.DataSource = new[] { spu2 };
            Title = $"Service Point User {spu2.Id}";
        }

        protected void ServicePointUserServicePointsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ServicePointUserServicePointsPermission"] == null) return;
            var id = (Guid?)ServicePointUser2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindServicePointUser2(id, true).ServicePointUserServicePoints ?? new ServicePointUserServicePoint[] { };
            ServicePointUserServicePointsRadGrid.DataSource = l;
            ServicePointUserServicePointsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ServicePointUserServicePointsPanel.Visible = ServicePointUser2FormView.DataKey.Value != null && ((string)Session["ServicePointUserServicePointsPermission"] == "Edit" || Session["ServicePointUserServicePointsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
