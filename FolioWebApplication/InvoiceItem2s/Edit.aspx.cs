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
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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

        protected void InvoiceItemAdjustmentsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemAdjustmentsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id, true).InvoiceItemAdjustments ?? new InvoiceItemAdjustment[] { };
            InvoiceItemAdjustmentsRadGrid.DataSource = l;
            InvoiceItemAdjustmentsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemAdjustmentsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemAdjustmentsPermission"] == "Edit" || Session["InvoiceItemAdjustmentsPermission"] != null && l.Any());
        }

        protected void InvoiceItemFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemFundsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id, true).InvoiceItemFunds ?? new InvoiceItemFund[] { };
            InvoiceItemFundsRadGrid.DataSource = l;
            InvoiceItemFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemFundsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemFundsPermission"] == "Edit" || Session["InvoiceItemFundsPermission"] != null && l.Any());
        }

        protected void InvoiceItemReferenceNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemReferenceNumbersPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id, true).InvoiceItemReferenceNumbers ?? new InvoiceItemReferenceNumber[] { };
            InvoiceItemReferenceNumbersRadGrid.DataSource = l;
            InvoiceItemReferenceNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemReferenceNumbersPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemReferenceNumbersPermission"] == "Edit" || Session["InvoiceItemReferenceNumbersPermission"] != null && l.Any());
        }

        protected void InvoiceItemTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItemTagsPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInvoiceItem2(id, true).InvoiceItemTags ?? new InvoiceItemTag[] { };
            InvoiceItemTagsRadGrid.DataSource = l;
            InvoiceItemTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InvoiceItemTagsPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && ((string)Session["InvoiceItemTagsPermission"] == "Edit" || Session["InvoiceItemTagsPermission"] != null && l.Any());
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)InvoiceItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "EncumbranceAmountCredited", "encumbrance.amountCredited" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "VoidedAmount", "voidedAmount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"sourceInvoiceLineId == \"{id}\"",
                Global.GetCqlFilter(Transaction2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentEncumbrance.Amount", "awaitingPayment.encumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment"),
                Global.GetCqlFilter(Transaction2sRadGrid, "EncumbranceAmountCredited", "encumbrance.amountCredited"),
                Global.GetCqlFilter(Transaction2sRadGrid, "ExpendedAmount", "encumbrance.amountExpended"),
                Global.GetCqlFilter(Transaction2sRadGrid, "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Status", "encumbrance.status"),
                Global.GetCqlFilter(Transaction2sRadGrid, "OrderType", "encumbrance.orderType"),
                Global.GetCqlFilter(Transaction2sRadGrid, "OrderStatus", "encumbrance.orderStatus"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Subscription", "encumbrance.subscription"),
                Global.GetCqlFilter(Transaction2sRadGrid, "ReEncumber", "encumbrance.reEncumber"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Order.Number", "encumbrance.sourcePurchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(Transaction2sRadGrid, "OrderItem.Number", "encumbrance.sourcePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Transaction2sRadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
                Global.GetCqlFilter(Transaction2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2sRadGrid, "FromFund.Name", "fromFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2sRadGrid, "InvoiceCancelled", "invoiceCancelled"),
                Global.GetCqlFilter(Transaction2sRadGrid, "PaymentEncumbrance.Amount", "paymentEncumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Transaction2sRadGrid, "SourceFiscalYear.Name", "sourceFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2sRadGrid, "Invoice.Number", "sourceInvoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Transaction2sRadGrid, "ToFund.Name", "toFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2sRadGrid, "TransactionType", "transactionType"),
                Global.GetCqlFilter(Transaction2sRadGrid, "VoidedAmount", "voidedAmount"),
                Global.GetCqlFilter(Transaction2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Transaction2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Transaction2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Transaction2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(where, Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = folioServiceContext.CountTransaction2s(where);
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = InvoiceItem2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
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
