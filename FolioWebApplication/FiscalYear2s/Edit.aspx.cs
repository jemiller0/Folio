using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            fy2.Content = fy2.Content != null ? JsonConvert.DeserializeObject<JToken>(fy2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            FiscalYear2FormView.DataSource = new[] { fy2 };
            Title = $"Fiscal Year {fy2.Name}";
        }

        protected void Budget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Budget2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fiscalYearId == \"{id}\"",
                Global.GetCqlFilter(Budget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Budget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Budget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(Budget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(Budget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(Budget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Budget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(Budget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(Budget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(Budget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(Budget2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Budget2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Budget2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(Budget2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(Budget2sRadGrid, "CashBalance", "cashBalance")
            }.Where(s => s != null)));
            Budget2sRadGrid.DataSource = folioServiceContext.Budget2s(out var i, where, Budget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Budget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Budget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Budget2sRadGrid.PageSize * Budget2sRadGrid.CurrentPageIndex, Budget2sRadGrid.PageSize, true);
            Budget2sRadGrid.VirtualItemCount = i;
            if (Budget2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Budget2sRadGrid.AllowFilteringByColumn = Budget2sRadGrid.VirtualItemCount > 10;
                Budget2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Budget2sPermission"] != null && Budget2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void BudgetGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetGroup2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fiscalYearId == \"{id}\"",
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Group.Name", "groupId", "name", folioServiceContext.FolioServiceClient.FinanceGroups),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Fund.Name", "fundId", "name", folioServiceContext.FolioServiceClient.Funds)
            }.Where(s => s != null)));
            BudgetGroup2sRadGrid.DataSource = folioServiceContext.BudgetGroup2s(out var i, where, BudgetGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetGroup2sRadGrid.PageSize * BudgetGroup2sRadGrid.CurrentPageIndex, BudgetGroup2sRadGrid.PageSize, true);
            BudgetGroup2sRadGrid.VirtualItemCount = i;
            if (BudgetGroup2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetGroup2sRadGrid.AllowFilteringByColumn = BudgetGroup2sRadGrid.VirtualItemCount > 10;
                BudgetGroup2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["BudgetGroup2sPermission"] != null && BudgetGroup2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void FiscalYearAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FiscalYearAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFiscalYear2(id, true).FiscalYearAcquisitionsUnits ?? new FiscalYearAcquisitionsUnit[] { };
            FiscalYearAcquisitionsUnitsRadGrid.DataSource = l;
            FiscalYearAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FiscalYearAcquisitionsUnitsPanel.Visible = FiscalYear2FormView.DataKey.Value != null && ((string)Session["FiscalYearAcquisitionsUnitsPermission"] == "Edit" || Session["FiscalYearAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void Ledger2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Ledger2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "FiscalYearOneId", "fiscalYearOneId" }, { "LedgerStatus", "ledgerStatus" }, { "Allocated", "allocated" }, { "Available", "available" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "Currency", "currency" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" }, { "AwaitingPayment", "awaitingPayment" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fiscalYearOneId == \"{id}\"",
                Global.GetCqlFilter(Ledger2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Ledger2sRadGrid, "LedgerStatus", "ledgerStatus"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Ledger2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Ledger2sRadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(Ledger2sRadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(Ledger2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Ledger2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Ledger2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Ledger2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Ledger2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(Ledger2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(Ledger2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(Ledger2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(Ledger2sRadGrid, "CashBalance", "cashBalance"),
                Global.GetCqlFilter(Ledger2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(Ledger2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(Ledger2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(Ledger2sRadGrid, "OverExpended", "overExpended")
            }.Where(s => s != null)));
            Ledger2sRadGrid.DataSource = folioServiceContext.Ledger2s(out var i, where, Ledger2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Ledger2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Ledger2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Ledger2sRadGrid.PageSize * Ledger2sRadGrid.CurrentPageIndex, Ledger2sRadGrid.PageSize, true);
            Ledger2sRadGrid.VirtualItemCount = i;
            if (Ledger2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Ledger2sRadGrid.AllowFilteringByColumn = Ledger2sRadGrid.VirtualItemCount > 10;
                Ledger2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Ledger2sPermission"] != null && Ledger2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void LedgerRollover2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRollover2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fromFiscalYearId == \"{id}\"",
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "ToFiscalYear.Name", "toFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "NeedCloseBudgets", "needCloseBudgets"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CurrencyFactor", "currencyFactor"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LedgerRollover2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LedgerRollover2sRadGrid.DataSource = folioServiceContext.LedgerRollover2s(out var i, where, LedgerRollover2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRollover2sRadGrid.PageSize * LedgerRollover2sRadGrid.CurrentPageIndex, LedgerRollover2sRadGrid.PageSize, true);
            LedgerRollover2sRadGrid.VirtualItemCount = i;
            if (LedgerRollover2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRollover2sRadGrid.AllowFilteringByColumn = LedgerRollover2sRadGrid.VirtualItemCount > 10;
                LedgerRollover2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["LedgerRollover2sPermission"] != null && LedgerRollover2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void LedgerRollover2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LedgerRollover2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerId", "ledgerId" }, { "FromFiscalYearId", "fromFiscalYearId" }, { "ToFiscalYearId", "toFiscalYearId" }, { "RestrictEncumbrance", "restrictEncumbrance" }, { "RestrictExpenditures", "restrictExpenditures" }, { "NeedCloseBudgets", "needCloseBudgets" }, { "CurrencyFactor", "currencyFactor" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"toFiscalYearId == \"{id}\"",
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "FromFiscalYear.Name", "fromFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "RestrictEncumbrance", "restrictEncumbrance"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "RestrictExpenditures", "restrictExpenditures"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "NeedCloseBudgets", "needCloseBudgets"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "CurrencyFactor", "currencyFactor"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LedgerRollover2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LedgerRollover2s1RadGrid.DataSource = folioServiceContext.LedgerRollover2s(out var i, where, LedgerRollover2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRollover2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRollover2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRollover2s1RadGrid.PageSize * LedgerRollover2s1RadGrid.CurrentPageIndex, LedgerRollover2s1RadGrid.PageSize, true);
            LedgerRollover2s1RadGrid.VirtualItemCount = i;
            if (LedgerRollover2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                LedgerRollover2s1RadGrid.AllowFilteringByColumn = LedgerRollover2s1RadGrid.VirtualItemCount > 10;
                LedgerRollover2s1Panel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["LedgerRollover2sPermission"] != null && LedgerRollover2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"sourceFiscalYearId == \"{id}\"",
                Global.GetCqlFilter(Transaction2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentEncumbrance.Amount", "awaitingPayment.encumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Transaction2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Transaction2sRadGrid, "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment"),
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
                Global.GetCqlFilter(Transaction2sRadGrid, "Invoice.Number", "sourceInvoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Transaction2sRadGrid, "InvoiceItem.Number", "sourceInvoiceLineId", "invoiceLineNumber", folioServiceContext.FolioServiceClient.InvoiceItems),
                Global.GetCqlFilter(Transaction2sRadGrid, "ToFund.Name", "toFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2sRadGrid, "TransactionType", "transactionType"),
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
                Transaction2sPanel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void Transaction2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)FiscalYear2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fiscalYearId == \"{id}\"",
                Global.GetCqlFilter(Transaction2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentEncumbrance.Amount", "awaitingPayment.encumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Description", "description"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "ExpendedAmount", "encumbrance.amountExpended"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Status", "encumbrance.status"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "OrderType", "encumbrance.orderType"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "OrderStatus", "encumbrance.orderStatus"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Subscription", "encumbrance.subscription"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "ReEncumber", "encumbrance.reEncumber"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Order.Number", "encumbrance.sourcePurchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(Transaction2s1RadGrid, "OrderItem.Number", "encumbrance.sourcePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Transaction2s1RadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
                Global.GetCqlFilter(Transaction2s1RadGrid, "FromFund.Name", "fromFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2s1RadGrid, "InvoiceCancelled", "invoiceCancelled"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "PaymentEncumbrance.Amount", "paymentEncumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Source", "source"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "SourceFiscalYear.Name", "sourceFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Invoice.Number", "sourceInvoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Transaction2s1RadGrid, "InvoiceItem.Number", "sourceInvoiceLineId", "invoiceLineNumber", folioServiceContext.FolioServiceClient.InvoiceItems),
                Global.GetCqlFilter(Transaction2s1RadGrid, "ToFund.Name", "toFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2s1RadGrid, "TransactionType", "transactionType"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Transaction2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Transaction2s1RadGrid.DataSource = folioServiceContext.Transaction2s(out var i, where, Transaction2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2s1RadGrid.PageSize * Transaction2s1RadGrid.CurrentPageIndex, Transaction2s1RadGrid.PageSize, true);
            Transaction2s1RadGrid.VirtualItemCount = i;
            if (Transaction2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2s1RadGrid.AllowFilteringByColumn = Transaction2s1RadGrid.VirtualItemCount > 10;
                Transaction2s1Panel.Visible = FiscalYear2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
