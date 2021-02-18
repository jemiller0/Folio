using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Fund2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Fund2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Fund2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var f2 = folioServiceContext.FindFund2(id, true);
            if (f2 == null) Response.Redirect("Default.aspx");
            Fund2FormView.DataSource = new[] { f2 };
            Title = $"Fund {f2.Name}";
        }

        protected void Budget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Budget2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Budget2sRadGrid.DataSource = folioServiceContext.Budget2s(out var i, Global.GetCqlFilter(Budget2sRadGrid, d, $"fundId == \"{id}\""), Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Budget2sRadGrid.PageSize * Budget2sRadGrid.CurrentPageIndex, Budget2sRadGrid.PageSize, true);
            Budget2sRadGrid.VirtualItemCount = i;
            if (Budget2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Budget2sRadGrid.AllowFilteringByColumn = Budget2sRadGrid.VirtualItemCount > 10;
                Budget2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["Budget2sPermission"] != null && Budget2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void GroupFundFiscalYear2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["GroupFundFiscalYear2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            GroupFundFiscalYear2sRadGrid.DataSource = folioServiceContext.GroupFundFiscalYear2s(out var i, Global.GetCqlFilter(GroupFundFiscalYear2sRadGrid, d, $"fundId == \"{id}\""), GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(GroupFundFiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, GroupFundFiscalYear2sRadGrid.PageSize * GroupFundFiscalYear2sRadGrid.CurrentPageIndex, GroupFundFiscalYear2sRadGrid.PageSize, true);
            GroupFundFiscalYear2sRadGrid.VirtualItemCount = i;
            if (GroupFundFiscalYear2sRadGrid.MasterTableView.FilterExpression == "")
            {
                GroupFundFiscalYear2sRadGrid.AllowFilteringByColumn = GroupFundFiscalYear2sRadGrid.VirtualItemCount > 10;
                GroupFundFiscalYear2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["GroupFundFiscalYear2sPermission"] != null && GroupFundFiscalYear2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d, $"fromFundId == \"{id}\""), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2s1RadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2s1RadGrid, d, $"toFundId == \"{id}\""), Transaction2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2s1RadGrid.PageSize * Transaction2s1RadGrid.CurrentPageIndex, Transaction2s1RadGrid.PageSize, true);
            Transaction2s1RadGrid.VirtualItemCount = i;
            if (Transaction2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2s1RadGrid.AllowFilteringByColumn = Transaction2s1RadGrid.VirtualItemCount > 10;
                Transaction2s1Panel.Visible = Fund2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2s1RadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
