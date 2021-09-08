using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolioWebApplication.BoundWithPart2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BoundWithPart2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BoundWithPart2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var bwp2 = folioServiceContext.FindBoundWithPart2(id, true);
            if (bwp2 == null) Response.Redirect("Default.aspx");
            BoundWithPart2FormView.DataSource = new[] { bwp2 };
            Title = $"Bound With Part {bwp2.Id}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
