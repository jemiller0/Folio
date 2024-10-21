using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ExpenseClass2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ExpenseClass2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ExpenseClass2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ec2 = folioServiceContext.FindExpenseClass2(id, true);
            if (ec2 == null) Response.Redirect("Default.aspx");
            ec2.Content = ec2.Content != null ? JsonConvert.DeserializeObject<JToken>(ec2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            ExpenseClass2FormView.DataSource = new[] { ec2 };
            Title = $"Expense Class {ec2.Name}";
        }

        protected void BudgetExpenseClass2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetExpenseClass2sPermission"] == null) return;
            var id = (Guid?)ExpenseClass2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "ExpenseClassId", "expenseClassId" }, { "Status", "status" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"expenseClassId == \"{id}\"",
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetExpenseClass2sRadGrid, "Status", "status")
            }.Where(s => s != null)));
            BudgetExpenseClass2sRadGrid.DataSource = folioServiceContext.BudgetExpenseClass2s(out var i, where, BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetExpenseClass2sRadGrid.PageSize * BudgetExpenseClass2sRadGrid.CurrentPageIndex, BudgetExpenseClass2sRadGrid.PageSize, true);
            BudgetExpenseClass2sRadGrid.VirtualItemCount = i;
            if (BudgetExpenseClass2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetExpenseClass2sRadGrid.AllowFilteringByColumn = BudgetExpenseClass2sRadGrid.VirtualItemCount > 10;
                BudgetExpenseClass2sPanel.Visible = ExpenseClass2FormView.DataKey.Value != null && Session["BudgetExpenseClass2sPermission"] != null && BudgetExpenseClass2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)ExpenseClass2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "EncumbranceAmountCredited", "encumbrance.amountCredited" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "VoidedAmount", "voidedAmount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"expenseClassId == \"{id}\"",
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
                Global.GetCqlFilter(Transaction2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2sRadGrid, "FromFund.Name", "fromFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2sRadGrid, "InvoiceCancelled", "invoiceCancelled"),
                Global.GetCqlFilter(Transaction2sRadGrid, "PaymentEncumbrance.Amount", "paymentEncumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Transaction2sRadGrid, "SourceFiscalYear.Name", "sourceFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2sRadGrid, "Invoice.Number", "sourceInvoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Transaction2sRadGrid, "InvoiceItem.Number", "sourceInvoiceLineId", "invoiceLineNumber", folioServiceContext.FolioServiceClient.InvoiceItems),
                Global.GetCqlFilter(Transaction2sRadGrid, "ToFund.Name", "toFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2sRadGrid, "TransactionType", "transactionType"),
                Global.GetCqlFilter(Transaction2sRadGrid, "VoidedAmount", "voidedAmount"),
                Global.GetCqlFilter(Transaction2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Transaction2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Transaction2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Transaction2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, where, Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = ExpenseClass2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
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
