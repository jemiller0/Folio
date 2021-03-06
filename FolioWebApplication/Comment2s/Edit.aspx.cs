using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolioWebApplication.Comment2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Comment2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Comment2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var c2 = folioServiceContext.FindComment2(id, true);
            if (c2 == null) Response.Redirect("Default.aspx");
            Comment2FormView.DataSource = new[] { c2 };
            Title = $"Comment {c2.Id}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
