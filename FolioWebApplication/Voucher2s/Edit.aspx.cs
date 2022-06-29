using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Voucher2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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

        protected void VoucherAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Voucher2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindVoucher2(id, true).VoucherAcquisitionsUnits ?? new VoucherAcquisitionsUnit[] { };
            VoucherAcquisitionsUnitsRadGrid.DataSource = l;
            VoucherAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            VoucherAcquisitionsUnitsPanel.Visible = Voucher2FormView.DataKey.Value != null && ((string)Session["VoucherAcquisitionsUnitsPermission"] == "Edit" || Session["VoucherAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void VoucherItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["VoucherItem2sPermission"] == null) return;
            var id = (Guid?)Voucher2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AccountNumber", "externalAccountNumber" }, { "SubTransactionId", "subTransactionId" }, { "VoucherId", "voucherId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"voucherId == \"{id}\"",
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "AccountNumber", "externalAccountNumber"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(VoucherItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            VoucherItem2sRadGrid.DataSource = folioServiceContext.VoucherItem2s(out var i, where, VoucherItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(VoucherItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, VoucherItem2sRadGrid.PageSize * VoucherItem2sRadGrid.CurrentPageIndex, VoucherItem2sRadGrid.PageSize, true);
            VoucherItem2sRadGrid.VirtualItemCount = i;
            if (VoucherItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                VoucherItem2sRadGrid.AllowFilteringByColumn = VoucherItem2sRadGrid.VirtualItemCount > 10;
                VoucherItem2sPanel.Visible = Voucher2FormView.DataKey.Value != null && Session["VoucherItem2sPermission"] != null && VoucherItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
