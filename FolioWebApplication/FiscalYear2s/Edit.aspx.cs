using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.FiscalYear2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FiscalYear2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FiscalYear2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var fy2 = folioServiceContext.FindFiscalYear2(id, true);
            if (fy2 == null) Response.Redirect("Default.aspx");
            FiscalYear2FormView.DataSource = new[] { fy2 };
            Title = $"Fiscal Year {fy2.Name}";
        }

        protected void Budget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Budget2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            Budget2sRadGrid.DataSource = folioServiceContext.Budget2s(out var i, Global.GetCqlFilter(Budget2sRadGrid, d, $"fiscalYearId == \"{id}\""), Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Budget2sRadGrid.PageSize * Budget2sRadGrid.CurrentPageIndex, Budget2sRadGrid.PageSize, true);
            Budget2sRadGrid.VirtualItemCount = i;
            if (Budget2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Budget2sRadGrid.AllowFilteringByColumn = Budget2sRadGrid.VirtualItemCount > 10;
                Budget2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Budget2sPermission"] != null && Budget2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void BudgetGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetGroup2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            BudgetGroup2sRadGrid.DataSource = folioServiceContext.BudgetGroup2s(out var i, Global.GetCqlFilter(BudgetGroup2sRadGrid, d, $"fiscalYearId == \"{id}\""), BudgetGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetGroup2sRadGrid.PageSize * BudgetGroup2sRadGrid.CurrentPageIndex, BudgetGroup2sRadGrid.PageSize, true);
            BudgetGroup2sRadGrid.VirtualItemCount = i;
            if (BudgetGroup2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetGroup2sRadGrid.AllowFilteringByColumn = BudgetGroup2sRadGrid.VirtualItemCount > 10;
                BudgetGroup2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["BudgetGroup2sPermission"] != null && BudgetGroup2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Ledger2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Ledger2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "FiscalYearOneId", "fiscalYearOneId" }, { "LedgerStatus", "ledgerStatus" }, { "Allocated", "allocated" }, { "Available", "available" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "Currency", "currency" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" }, { "AwaitingPayment", "awaitingPayment" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" } };
            Ledger2sRadGrid.DataSource = folioServiceContext.Ledger2s(out var i, Global.GetCqlFilter(Ledger2sRadGrid, d, $"fiscalYearOneId == \"{id}\""), Ledger2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Ledger2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Ledger2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Ledger2sRadGrid.PageSize * Ledger2sRadGrid.CurrentPageIndex, Ledger2sRadGrid.PageSize, true);
            Ledger2sRadGrid.VirtualItemCount = i;
            if (Ledger2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Ledger2sRadGrid.AllowFilteringByColumn = Ledger2sRadGrid.VirtualItemCount > 10;
                Ledger2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Ledger2sPermission"] != null && Ledger2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void LedgerRollover2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRollover2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LedgerRollover2sRadGrid.DataSource = folioServiceContext.LedgerRollover2s(out var i, Global.GetCqlFilter(LedgerRollover2sRadGrid, d, $"fromFiscalYearId == \"{id}\""), LedgerRollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRollover2sRadGrid.PageSize * LedgerRollover2sRadGrid.CurrentPageIndex, LedgerRollover2sRadGrid.PageSize, true);
            LedgerRollover2sRadGrid.VirtualItemCount = i;
            if (LedgerRollover2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRollover2sRadGrid.AllowFilteringByColumn = LedgerRollover2sRadGrid.VirtualItemCount > 10;
                LedgerRollover2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["LedgerRollover2sPermission"] != null && LedgerRollover2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void LedgerRollover2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRollover2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LedgerRollover2s1RadGrid.DataSource = folioServiceContext.LedgerRollover2s(out var i, Global.GetCqlFilter(LedgerRollover2s1RadGrid, d, $"toFiscalYearId == \"{id}\""), LedgerRollover2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRollover2s1RadGrid.PageSize * LedgerRollover2s1RadGrid.CurrentPageIndex, LedgerRollover2s1RadGrid.PageSize, true);
            LedgerRollover2s1RadGrid.VirtualItemCount = i;
            if (LedgerRollover2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRollover2s1RadGrid.AllowFilteringByColumn = LedgerRollover2s1RadGrid.VirtualItemCount > 10;
                LedgerRollover2s1Panel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["LedgerRollover2sPermission"] != null && LedgerRollover2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d, $"sourceFiscalYearId == \"{id}\""), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2s1RadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2s1RadGrid, d, $"fiscalYearId == \"{id}\""), Transaction2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2s1RadGrid.PageSize * Transaction2s1RadGrid.CurrentPageIndex, Transaction2s1RadGrid.PageSize, true);
            Transaction2s1RadGrid.VirtualItemCount = i;
            if (Transaction2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2s1RadGrid.AllowFilteringByColumn = Transaction2s1RadGrid.VirtualItemCount > 10;
                Transaction2s1Panel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2s1RadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
