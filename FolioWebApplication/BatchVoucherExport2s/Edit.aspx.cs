using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolioWebApplication.BatchVoucherExport2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BatchVoucherExport2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BatchVoucherExport2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var bve2 = folioServiceContext.FindBatchVoucherExport2(id, true);
            if (bve2 == null) Response.Redirect("Default.aspx");
            BatchVoucherExport2FormView.DataSource = new[] { bve2 };
            Title = $"Batch Voucher Export {bve2.Id}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
