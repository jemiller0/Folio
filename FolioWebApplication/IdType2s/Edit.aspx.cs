using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.IdType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IdType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var it2 = folioServiceContext.FindIdType2(id, true);
            if (it2 == null) Response.Redirect("Default.aspx");
            it2.Content = it2.Content != null ? JsonConvert.DeserializeObject<JToken>(it2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            IdType2FormView.DataSource = new[] { it2 };
            Title = $"Id Type {it2.Name}";
        }

        protected void InvoiceItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItem2sPermission"] == null) return;
            var id = (Guid?)IdType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNumber" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "Comment", "comment" }, { "Description", "description" }, { "InvoiceId", "invoiceId" }, { "Number", "invoiceLineNumber" }, { "Status", "invoiceLineStatus" }, { "OrderItemId", "poLineId" }, { "ProductId", "productId" }, { "ProductIdTypeId", "productIdType" }, { "Quantity", "quantity" }, { "ReleaseEncumbrance", "releaseEncumbrance" }, { "SubscriptionInfo", "subscriptionInfo" }, { "SubscriptionStartDate", "subscriptionStart" }, { "SubscriptionEndDate", "subscriptionEnd" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"productIdType == \"{id}\"",
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AccountNumber", "accountNumber"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Invoice.Number", "invoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Number", "invoiceLineNumber"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Status", "invoiceLineStatus"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "ProductId", "productId"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Quantity", "quantity"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "ReleaseEncumbrance", "releaseEncumbrance"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "SubscriptionInfo", "subscriptionInfo"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "SubscriptionStartDate", "subscriptionStart"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "SubscriptionEndDate", "subscriptionEnd"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "SubTotal", "subTotal"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Total", "total"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            InvoiceItem2sRadGrid.DataSource = folioServiceContext.InvoiceItem2s(out var i, where, InvoiceItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InvoiceItem2sRadGrid.PageSize * InvoiceItem2sRadGrid.CurrentPageIndex, InvoiceItem2sRadGrid.PageSize, true);
            InvoiceItem2sRadGrid.VirtualItemCount = i;
            if (InvoiceItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                InvoiceItem2sRadGrid.AllowFilteringByColumn = InvoiceItem2sRadGrid.VirtualItemCount > 10;
                InvoiceItem2sPanel.Visible = IdType2FormView.DataKey.Value != null && Session["InvoiceItem2sPermission"] != null && InvoiceItem2sRadGrid.VirtualItemCount > 0;
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
