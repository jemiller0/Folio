using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Fund2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            f2.Content = f2.Content != null ? JsonConvert.DeserializeObject<JToken>(f2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Fund2FormView.DataSource = new[] { f2 };
            Title = $"Fund {f2.Name}";
        }

        protected void AllocatedFromFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AllocatedFromFundsPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).AllocatedFromFunds ?? new AllocatedFromFund[] { };
            AllocatedFromFundsRadGrid.DataSource = l;
            AllocatedFromFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AllocatedFromFundsPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["AllocatedFromFundsPermission"] == "Edit" || Session["AllocatedFromFundsPermission"] != null && l.Any());
        }

        protected void AllocatedToFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AllocatedToFundsPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).AllocatedToFunds ?? new AllocatedToFund[] { };
            AllocatedToFundsRadGrid.DataSource = l;
            AllocatedToFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AllocatedToFundsPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["AllocatedToFundsPermission"] == "Edit" || Session["AllocatedToFundsPermission"] != null && l.Any());
        }

        protected void Budget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Budget2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Credits", "credits" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fundId == \"{id}\"",
                Global.GetCqlFilter(Budget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Budget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Budget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(Budget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(Budget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(Budget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Budget2sRadGrid, "Credits", "credits"),
                Global.GetCqlFilter(Budget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(Budget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(Budget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(Budget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(Budget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(Budget2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
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
                Budget2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["Budget2sPermission"] != null && Budget2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void BudgetGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BudgetGroup2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "GroupId", "groupId" }, { "FiscalYearId", "fiscalYearId" }, { "FundId", "fundId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fundId == \"{id}\"",
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "Group.Name", "groupId", "name", folioServiceContext.FolioServiceClient.FinanceGroups),
                Global.GetCqlFilter(BudgetGroup2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears)
            }.Where(s => s != null)));
            BudgetGroup2sRadGrid.DataSource = folioServiceContext.BudgetGroup2s(out var i, where, BudgetGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BudgetGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BudgetGroup2sRadGrid.PageSize * BudgetGroup2sRadGrid.CurrentPageIndex, BudgetGroup2sRadGrid.PageSize, true);
            BudgetGroup2sRadGrid.VirtualItemCount = i;
            if (BudgetGroup2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BudgetGroup2sRadGrid.AllowFilteringByColumn = BudgetGroup2sRadGrid.VirtualItemCount > 10;
                BudgetGroup2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["BudgetGroup2sPermission"] != null && BudgetGroup2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void FundAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FundAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).FundAcquisitionsUnits ?? new FundAcquisitionsUnit[] { };
            FundAcquisitionsUnitsRadGrid.DataSource = l;
            FundAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FundAcquisitionsUnitsPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["FundAcquisitionsUnitsPermission"] == "Edit" || Session["FundAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void FundLocation2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FundLocation2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).FundLocation2s ?? new FundLocation2[] { };
            FundLocation2sRadGrid.DataSource = l;
            FundLocation2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FundLocation2sPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["FundLocation2sPermission"] == "Edit" || Session["FundLocation2sPermission"] != null && l.Any());
        }

        protected void FundOrganization2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FundOrganization2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).FundOrganization2s ?? new FundOrganization2[] { };
            FundOrganization2sRadGrid.DataSource = l;
            FundOrganization2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FundOrganization2sPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["FundOrganization2sPermission"] == "Edit" || Session["FundOrganization2sPermission"] != null && l.Any());
        }

        protected void FundTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FundTagsPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFund2(id, true).FundTags ?? new FundTag[] { };
            FundTagsRadGrid.DataSource = l;
            FundTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FundTagsPanel.Visible = Fund2FormView.DataKey.Value != null && ((string)Session["FundTagsPermission"] == "Edit" || Session["FundTagsPermission"] != null && l.Any());
        }

        protected void RolloverBudget2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RolloverBudget2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "BudgetId", "budgetId" }, { "RolloverId", "ledgerRolloverId" }, { "Name", "name" }, { "FundDetailsName", "fundDetails.name" }, { "FundDetailsCode", "fundDetails.code" }, { "FundDetailsFundStatus", "fundDetails.fundStatus" }, { "FundDetailsFundTypeId", "fundDetails.fundTypeId" }, { "FundDetailsFundTypeName", "fundDetails.fundTypeName" }, { "FundDetailsExternalAccountNo", "fundDetails.externalAccountNo" }, { "FundDetailsDescription", "fundDetails.description" }, { "FundDetailsRestrictByLocations", "fundDetails.restrictByLocations" }, { "BudgetStatus", "budgetStatus" }, { "AllowableEncumbrance", "allowableEncumbrance" }, { "AllowableExpenditure", "allowableExpenditure" }, { "Allocated", "allocated" }, { "AwaitingPayment", "awaitingPayment" }, { "Available", "available" }, { "Credits", "credits" }, { "Encumbered", "encumbered" }, { "Expenditures", "expenditures" }, { "NetTransfers", "netTransfers" }, { "Unavailable", "unavailable" }, { "OverEncumbrance", "overEncumbrance" }, { "OverExpended", "overExpended" }, { "FundId", "fundId" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "InitialAllocation", "initialAllocation" }, { "AllocationTo", "allocationTo" }, { "AllocationFrom", "allocationFrom" }, { "TotalFunding", "totalFunding" }, { "CashBalance", "cashBalance" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fundId == \"{id}\"",
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Budget.Name", "budgetId", "name", folioServiceContext.FolioServiceClient.Budgets),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Rollover.Id", "ledgerRolloverId", "id", folioServiceContext.FolioServiceClient.Rollovers),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsName", "fundDetails.name"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsCode", "fundDetails.code"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundStatus", "fundDetails.fundStatus"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundType.Name", "fundDetails.fundTypeId", "name", folioServiceContext.FolioServiceClient.FundTypes),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsFundTypeName", "fundDetails.fundTypeName"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsExternalAccountNo", "fundDetails.externalAccountNo"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsDescription", "fundDetails.description"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FundDetailsRestrictByLocations", "fundDetails.restrictByLocations"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "BudgetStatus", "budgetStatus"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllowableEncumbrance", "allowableEncumbrance"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllowableExpenditure", "allowableExpenditure"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Allocated", "allocated"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AwaitingPayment", "awaitingPayment"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Credits", "credits"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Encumbered", "encumbered"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Expenditures", "expenditures"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "NetTransfers", "netTransfers"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "Unavailable", "unavailable"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "OverEncumbrance", "overEncumbrance"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "OverExpended", "overExpended"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "InitialAllocation", "initialAllocation"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllocationTo", "allocationTo"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "AllocationFrom", "allocationFrom"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "TotalFunding", "totalFunding"),
                Global.GetCqlFilter(RolloverBudget2sRadGrid, "CashBalance", "cashBalance")
            }.Where(s => s != null)));
            RolloverBudget2sRadGrid.DataSource = folioServiceContext.RolloverBudget2s(out var i, where, RolloverBudget2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverBudget2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverBudget2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverBudget2sRadGrid.PageSize * RolloverBudget2sRadGrid.CurrentPageIndex, RolloverBudget2sRadGrid.PageSize, true);
            RolloverBudget2sRadGrid.VirtualItemCount = i;
            if (RolloverBudget2sRadGrid.MasterTableView.FilterExpression == "")
            {
                RolloverBudget2sRadGrid.AllowFilteringByColumn = RolloverBudget2sRadGrid.VirtualItemCount > 10;
                RolloverBudget2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["RolloverBudget2sPermission"] != null && RolloverBudget2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "EncumbranceAmountCredited", "encumbrance.amountCredited" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "VoidedAmount", "voidedAmount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"fromFundId == \"{id}\"",
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
                Transaction2sPanel.Visible = Fund2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Transaction2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)Fund2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "EncumbranceAmountCredited", "encumbrance.amountCredited" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "VoidedAmount", "voidedAmount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"toFundId == \"{id}\"",
                Global.GetCqlFilter(Transaction2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentEncumbrance.Amount", "awaitingPayment.encumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Description", "description"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "EncumbranceAmountCredited", "encumbrance.amountCredited"),
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
                Global.GetCqlFilter(Transaction2s1RadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2s1RadGrid, "FromFund.Name", "fromFundId", "name", folioServiceContext.FolioServiceClient.Funds),
                Global.GetCqlFilter(Transaction2s1RadGrid, "InvoiceCancelled", "invoiceCancelled"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "PaymentEncumbrance.Amount", "paymentEncumbranceId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Source", "source"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "SourceFiscalYear.Name", "sourceFiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Transaction2s1RadGrid, "Invoice.Number", "sourceInvoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Transaction2s1RadGrid, "InvoiceItem.Number", "sourceInvoiceLineId", "invoiceLineNumber", folioServiceContext.FolioServiceClient.InvoiceItems),
                Global.GetCqlFilter(Transaction2s1RadGrid, "TransactionType", "transactionType"),
                Global.GetCqlFilter(Transaction2s1RadGrid, "VoidedAmount", "voidedAmount"),
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
                Transaction2s1Panel.Visible = Fund2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2s1RadGrid.VirtualItemCount > 0;
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
