using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.VoucherItem2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            vi2.Content = vi2.Content != null ? JsonConvert.DeserializeObject<JToken>(vi2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            VoucherItem2FormView.DataSource = new[] { vi2 };
            Title = $"Voucher Item {vi2.AccountNumber}";
        }

        protected void VoucherItemFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherItemFundsPermission"] == null) return;
            var id = (Guid?)VoucherItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindVoucherItem2(id, true).VoucherItemFunds ?? new VoucherItemFund[] { };
            VoucherItemFundsRadGrid.DataSource = l;
            VoucherItemFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            VoucherItemFundsPanel.Visible = VoucherItem2FormView.DataKey.Value != null && ((string)Session["VoucherItemFundsPermission"] == "Edit" || Session["VoucherItemFundsPermission"] != null && l.Any());
        }

        protected void VoucherItemInvoiceItemsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherItemInvoiceItemsPermission"] == null) return;
            var id = (Guid?)VoucherItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindVoucherItem2(id, true).VoucherItemInvoiceItems ?? new VoucherItemInvoiceItem[] { };
            VoucherItemInvoiceItemsRadGrid.DataSource = l;
            VoucherItemInvoiceItemsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            VoucherItemInvoiceItemsPanel.Visible = VoucherItem2FormView.DataKey.Value != null && ((string)Session["VoucherItemInvoiceItemsPermission"] == "Edit" || Session["VoucherItemInvoiceItemsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
