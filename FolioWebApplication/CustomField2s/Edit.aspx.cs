using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CustomField2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomField2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CustomField2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var cf2 = folioServiceContext.FindCustomField2(id, true);
            if (cf2 == null) Response.Redirect("Default.aspx");
            cf2.Content = cf2.Content != null ? JsonConvert.DeserializeObject<JToken>(cf2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            CustomField2FormView.DataSource = new[] { cf2 };
            Title = $"Custom Field {cf2.Name}";
        }

        protected void CustomFieldValuesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CustomFieldValuesPermission"] == null) return;
            var id = (Guid?)CustomField2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindCustomField2(id, true).CustomFieldValues ?? new CustomFieldValue[] { };
            CustomFieldValuesRadGrid.DataSource = l;
            CustomFieldValuesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            CustomFieldValuesPanel.Visible = CustomField2FormView.DataKey.Value != null && ((string)Session["CustomFieldValuesPermission"] == "Edit" || Session["CustomFieldValuesPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
