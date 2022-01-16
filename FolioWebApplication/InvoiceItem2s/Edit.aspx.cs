using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.InvoiceItem2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["InvoiceItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void InvoiceItem2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ii2 = folioServiceContext.FindInvoiceItem2(id, true);
            if (ii2 == null) Response.Redirect("Default.aspx");
            ii2.Content = ii2.Content != null ? JsonConvert.DeserializeObject<JToken>(ii2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            InvoiceItem2FormView.DataSource = new[] { ii2 };
            Title = $"Invoice Item {ii2.Number}";
        }

        protected void InvoiceItemAdjustmentFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemAdjustmentFundsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id).InvoiceItemAdjustmentFunds ?? new InvoiceItemAdjustmentFund[] { };
            InvoiceItemAdjustmentFundsRadGrid.DataSource = l;
            InvoiceItemAdjustmentFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemAdjustmentFundsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemAdjustmentFundsPermission"] == "Edit" || Session["InvoiceItemAdjustmentFundsPermission"] != null && l.Any());
        }

        protected void InvoiceItemAdjustmentsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemAdjustmentsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id).InvoiceItemAdjustments ?? new InvoiceItemAdjustment[] { };
            InvoiceItemAdjustmentsRadGrid.DataSource = l;
            InvoiceItemAdjustmentsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemAdjustmentsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemAdjustmentsPermission"] == "Edit" || Session["InvoiceItemAdjustmentsPermission"] != null && l.Any());
        }

        protected void InvoiceItemFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemFundsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id).InvoiceItemFunds ?? new InvoiceItemFund[] { };
            InvoiceItemFundsRadGrid.DataSource = l;
            InvoiceItemFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemFundsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemFundsPermission"] == "Edit" || Session["InvoiceItemFundsPermission"] != null && l.Any());
        }

        protected void InvoiceItemReferenceNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemReferenceNumbersPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id).InvoiceItemReferenceNumbers ?? new InvoiceItemReferenceNumber[] { };
            InvoiceItemReferenceNumbersRadGrid.DataSource = l;
            InvoiceItemReferenceNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemReferenceNumbersPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemReferenceNumbersPermission"] == "Edit" || Session["InvoiceItemReferenceNumbersPermission"] != null && l.Any());
        }

        protected void InvoiceItemTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemTagsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id).InvoiceItemTags ?? new InvoiceItemTag[] { };
            InvoiceItemTagsRadGrid.DataSource = l;
            InvoiceItemTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemTagsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemTagsPermission"] == "Edit" || Session["InvoiceItemTagsPermission"] != null && l.Any());
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d, $"sourceInvoiceLineId == \"{id}\""), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
