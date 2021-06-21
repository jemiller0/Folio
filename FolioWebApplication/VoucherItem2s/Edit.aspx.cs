using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.VoucherItem2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["VoucherItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void VoucherItem2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var vi2 = folioServiceContext.FindVoucherItem2(id, true);
            if (vi2 == null) Response.Redirect("Default.aspx");
            VoucherItem2FormView.DataSource = new[] { vi2 };
            Title = $"Voucher Item {vi2.AccountNumber}";
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
