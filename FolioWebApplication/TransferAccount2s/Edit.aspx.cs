using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolioWebApplication.TransferAccount2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TransferAccount2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void TransferAccount2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ta2 = folioServiceContext.FindTransferAccount2(id, true);
            if (ta2 == null) Response.Redirect("Default.aspx");
            TransferAccount2FormView.DataSource = new[] { ta2 };
            Title = $"Transfer Account {ta2.Name}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
