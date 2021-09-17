using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Voucher2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Voucher2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Voucher2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var v2 = folioServiceContext.FindVoucher2(id, true);
            if (v2 == null) Response.Redirect("Default.aspx");
            v2.Content = v2.Content != null ? JsonConvert.DeserializeObject<JToken>(v2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Voucher2FormView.DataSource = new[] { v2 };
            Title = $"Voucher {v2.Number}";
        }

        protected void VoucherItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherItem2sPermission"] == null) return;
            var id = (Guid?)Voucher2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            VoucherItem2sRadGrid.DataSource = folioServiceContext.VoucherItem2s(out var i, Global.GetCqlFilter(VoucherItem2sRadGrid, d, $"voucherId == \"{id}\""), VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, VoucherItem2sRadGrid.PageSize * VoucherItem2sRadGrid.CurrentPageIndex, VoucherItem2sRadGrid.PageSize, true);
            VoucherItem2sRadGrid.VirtualItemCount = i;
            if (VoucherItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                VoucherItem2sRadGrid.AllowFilteringByColumn = VoucherItem2sRadGrid.VirtualItemCount > 10;
                VoucherItem2sPanel.Visible = Voucher2FormView.DataKey.Value != null && Session["VoucherItem2sPermission"] != null && VoucherItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
