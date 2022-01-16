using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BatchVoucherExportConfig2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BatchVoucherExportConfig2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BatchVoucherExportConfig2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var bvec2 = folioServiceContext.FindBatchVoucherExportConfig2(id, true);
            if (bvec2 == null) Response.Redirect("Default.aspx");
            bvec2.Content = bvec2.Content != null ? JsonConvert.DeserializeObject<JToken>(bvec2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            BatchVoucherExportConfig2FormView.DataSource = new[] { bvec2 };
            Title = $"Batch Voucher Export Config {bvec2.Id}";
        }

        protected void BatchVoucherExportConfigWeekdaysRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BatchVoucherExportConfigWeekdaysPermission"] == null) return;
            var id = (Guid?)BatchVoucherExportConfig2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindBatchVoucherExportConfig2(id, true).BatchVoucherExportConfigWeekdays ?? new BatchVoucherExportConfigWeekday[] { };
            BatchVoucherExportConfigWeekdaysRadGrid.DataSource = l;
            BatchVoucherExportConfigWeekdaysRadGrid.AllowFilteringByColumn = l.Count() > 10;
            BatchVoucherExportConfigWeekdaysPanel.Visible = BatchVoucherExportConfig2FormView.DataKey.Value != null && ((string)Session["BatchVoucherExportConfigWeekdaysPermission"] == "Edit" || Session["BatchVoucherExportConfigWeekdaysPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
