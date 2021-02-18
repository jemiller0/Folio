using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolioWebApplication.RefundReason2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RefundReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RefundReason2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var rr2 = folioServiceContext.FindRefundReason2(id, true);
            if (rr2 == null) Response.Redirect("Default.aspx");
            RefundReason2FormView.DataSource = new[] { rr2 };
            Title = $"Refund Reason {rr2.Name}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
