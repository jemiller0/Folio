using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AgreementItem2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AgreementItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AgreementItem2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ai2 = folioServiceContext.FindAgreementItem2(id, true);
            if (ai2 == null) Response.Redirect("Default.aspx");
            ai2.Content = ai2.Content != null ? JsonConvert.DeserializeObject<JToken>(ai2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            AgreementItem2FormView.DataSource = new[] { ai2 };
            Title = $"Agreement Item {ai2.StartDate:d}";
        }

        protected void AgreementItemOrderItemsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AgreementItemOrderItemsPermission"] == null) return;
            var id = (Guid?)AgreementItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindAgreementItem2(id, true).AgreementItemOrderItems ?? new AgreementItemOrderItem[] { };
            AgreementItemOrderItemsRadGrid.DataSource = l;
            AgreementItemOrderItemsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AgreementItemOrderItemsPanel.Visible = AgreementItem2FormView.DataKey.Value != null && ((string)Session["AgreementItemOrderItemsPermission"] == "Edit" || Session["AgreementItemOrderItemsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
