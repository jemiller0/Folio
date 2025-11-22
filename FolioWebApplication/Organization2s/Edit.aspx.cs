using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Organization2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Organization2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Organization2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var o2 = folioServiceContext.FindOrganization2(id, true);
            if (o2 == null) Response.Redirect("Default.aspx");
            o2.Content = o2.Content != null ? JsonConvert.DeserializeObject<JToken>(o2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Organization2FormView.DataSource = new[] { o2 };
            Title = $"Organization {o2.Name}";
        }

        protected void CurrenciesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CurrenciesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).Currencies ?? new Currency[] { };
            CurrenciesRadGrid.DataSource = l;
            CurrenciesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            CurrenciesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["CurrenciesPermission"] == "Edit" || Session["CurrenciesPermission"] != null && l.Any());
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "OperationMode", "operationMode" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "FiscalYearId", "fiscalYearId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "NextInvoiceLineNumber", "nextInvoiceLineNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"vendorId == \"{id}\"",
                Global.GetCqlFilter(Invoice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovedBy.Username", "approvedBy", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
                Global.GetCqlFilter(Invoice2sRadGrid, "BillTo.Id", "billTo", "id", folioServiceContext.FolioServiceClient.Configurations),
                Global.GetCqlFilter(Invoice2sRadGrid, "CheckSubscriptionOverlap", "chkSubscriptionOverlap"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CancellationNote", "cancellationNote"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Enclosure", "enclosureNeeded"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExchangeRate", "exchangeRate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "OperationMode", "operationMode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Number", "folioInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "InvoiceDate", "invoiceDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LockTotal", "lockTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Note", "note"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDueDate", "paymentDue"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDate", "paymentDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentTerms", "paymentTerms"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Invoice2sRadGrid, "SubTotal", "subTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Total", "total"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VendorInvoiceNumber", "vendorInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementNumber", "disbursementNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VoucherNumber", "voucherNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Payment.Amount", "paymentId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementDate", "disbursementDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ManualPayment", "manualPayment"),
                Global.GetCqlFilter(Invoice2sRadGrid, "NextInvoiceLineNumber", "nextInvoiceLineNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(where, Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = folioServiceContext.CountInvoice2s(where);
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OpenedById", "openedById" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "NextNumber", "nextPolNumber" }, { "FiscalYearId", "fiscalYearId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"vendor == \"{id}\"",
                Global.GetCqlFilter(Order2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Order2sRadGrid, "Approved", "approved"),
                Global.GetCqlFilter(Order2sRadGrid, "ApprovedBy.Username", "approvedById", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "AssignedTo.Username", "assignedTo", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonReason", "closeReason.reason"),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonNote", "closeReason.note"),
                Global.GetCqlFilter(Order2sRadGrid, "OpenedBy.Username", "openedById", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "OrderDate", "dateOrdered"),
                Global.GetCqlFilter(Order2sRadGrid, "Manual", "manualPo"),
                Global.GetCqlFilter(Order2sRadGrid, "Number", "poNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OrderType", "orderType"),
                Global.GetCqlFilter(Order2sRadGrid, "Reencumber", "reEncumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingInterval", "ongoing.interval"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingIsSubscription", "ongoing.isSubscription"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingManualRenewal", "ongoing.manualRenewal"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingNotes", "ongoing.notes"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewPeriod", "ongoing.reviewPeriod"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingRenewalDate", "ongoing.renewalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewDate", "ongoing.reviewDate"),
                Global.GetCqlFilter(Order2sRadGrid, "Status", "workflowStatus"),
                Global.GetCqlFilter(Order2sRadGrid, "NextNumber", "nextPolNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Order2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Order2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(where, Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = folioServiceContext.CountOrder2s(where);
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "DetailsIsAcknowledged", "details.isAcknowledged" }, { "DetailsIsBinderyActive", "details.isBinderyActive" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"eresource.accessProvider == \"{id}\"",
                Global.GetCqlFilter(OrderItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Agreement.Name", "agreementId", "name", folioServiceContext.FolioServiceClient.Agreements),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AutomaticExport", "automaticExport"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ClaimingActive", "claimingActive"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Collection", "collection"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalUnitListPrice", "cost.listUnitPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicUnitListPrice", "cost.listUnitPriceElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Currency", "cost.currency"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AdditionalCost", "cost.additionalCost"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Discount", "cost.discount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DiscountType", "cost.discountType"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ExchangeRate", "cost.exchangeRate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalQuantity", "cost.quantityPhysical"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicQuantity", "cost.quantityElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EstimatedPrice", "cost.poLineEstimatedPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "InternalNote", "description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceivingNote", "details.receivingNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DetailsIsAcknowledged", "details.isAcknowledged"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DetailsIsBinderyActive", "details.isBinderyActive"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionFrom", "details.subscriptionFrom"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionInterval", "details.subscriptionInterval"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionTo", "details.subscriptionTo"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Donor", "donor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivated", "eresource.activated"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivationDue", "eresource.activationDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceCreateInventory", "eresource.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceTrial", "eresource.trial"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceExpectedActivationDate", "eresource.expectedActivation"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceUserLimit", "eresource.userLimit"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseCode", "eresource.license.code"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseDescription", "eresource.license.description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseReference", "eresource.license.reference"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceMaterialType.Name", "eresource.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceResourceUrl", "eresource.resourceUrl"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(OrderItem2sRadGrid, "IsPackage", "isPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastEdiExportDate", "lastEDIExportDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "OrderFormat", "orderFormat"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PackageOrderItem.Number", "packagePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PaymentStatus", "paymentStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalCreateInventory", "physical.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialType.Name", "physical.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialSupplier.Name", "physical.materialSupplier", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalReceiptDue", "physical.receiptDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Description", "poLineDescription"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Number", "poLineNumber"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PublicationYear", "publicationDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Order.Number", "purchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptDate", "receiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "RenewalNote", "renewalNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Requester", "requester"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Rush", "rush"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Selector", "selector"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "TitleOrPackage", "titleOrPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorInstructions", "vendorDetail.instructions"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorNote", "vendorDetail.noteFromVendor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorCustomerId", "vendorDetail.vendorAccount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(where, OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = folioServiceContext.CountOrderItem2s(where);
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItem2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "DetailsIsAcknowledged", "details.isAcknowledged" }, { "DetailsIsBinderyActive", "details.isBinderyActive" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"physical.materialSupplier == \"{id}\"",
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Agreement.Name", "agreementId", "name", folioServiceContext.FolioServiceClient.Agreements),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AutomaticExport", "automaticExport"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ClaimingActive", "claimingActive"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Collection", "collection"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalUnitListPrice", "cost.listUnitPrice"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ElectronicUnitListPrice", "cost.listUnitPriceElectronic"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Currency", "cost.currency"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AdditionalCost", "cost.additionalCost"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Discount", "cost.discount"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "DiscountType", "cost.discountType"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ExchangeRate", "cost.exchangeRate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalQuantity", "cost.quantityPhysical"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ElectronicQuantity", "cost.quantityElectronic"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EstimatedPrice", "cost.poLineEstimatedPrice"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "InternalNote", "description"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ReceivingNote", "details.receivingNote"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "DetailsIsAcknowledged", "details.isAcknowledged"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "DetailsIsBinderyActive", "details.isBinderyActive"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "SubscriptionFrom", "details.subscriptionFrom"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "SubscriptionInterval", "details.subscriptionInterval"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "SubscriptionTo", "details.subscriptionTo"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Donor", "donor"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceActivated", "eresource.activated"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceActivationDue", "eresource.activationDue"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceCreateInventory", "eresource.createInventory"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceTrial", "eresource.trial"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceExpectedActivationDate", "eresource.expectedActivation"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceUserLimit", "eresource.userLimit"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceAccessProvider.Name", "eresource.accessProvider", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceLicenseCode", "eresource.license.code"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceLicenseDescription", "eresource.license.description"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceLicenseReference", "eresource.license.reference"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceMaterialType.Name", "eresource.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "EresourceResourceUrl", "eresource.resourceUrl"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "IsPackage", "isPackage"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "LastEdiExportDate", "lastEDIExportDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "OrderFormat", "orderFormat"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PackageOrderItem.Number", "packagePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PaymentStatus", "paymentStatus"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalCreateInventory", "physical.createInventory"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalMaterialType.Name", "physical.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalReceiptDue", "physical.receiptDue"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Description", "poLineDescription"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Number", "poLineNumber"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PublicationYear", "publicationDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Order.Number", "purchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ReceiptDate", "receiptDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "RenewalNote", "renewalNote"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Requester", "requester"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Rush", "rush"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Selector", "selector"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Source", "source"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "TitleOrPackage", "titleOrPackage"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "VendorInstructions", "vendorDetail.instructions"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "VendorNote", "vendorDetail.noteFromVendor"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "VendorCustomerId", "vendorDetail.vendorAccount"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2s1RadGrid.DataSource = folioServiceContext.OrderItem2s(where, OrderItem2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2s1RadGrid.PageSize * OrderItem2s1RadGrid.CurrentPageIndex, OrderItem2s1RadGrid.PageSize, true);
            OrderItem2s1RadGrid.VirtualItemCount = folioServiceContext.CountOrderItem2s(where);
            if (OrderItem2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2s1RadGrid.AllowFilteringByColumn = OrderItem2s1RadGrid.VirtualItemCount > 10;
                OrderItem2s1Panel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrganizationAccountsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAccountsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationAccounts ?? new OrganizationAccount[] { };
            OrganizationAccountsRadGrid.DataSource = l;
            OrganizationAccountsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAccountsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAccountsPermission"] == "Edit" || Session["OrganizationAccountsPermission"] != null && l.Any());
        }

        protected void OrganizationAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationAcquisitionsUnits ?? new OrganizationAcquisitionsUnit[] { };
            OrganizationAcquisitionsUnitsRadGrid.DataSource = l;
            OrganizationAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAcquisitionsUnitsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAcquisitionsUnitsPermission"] == "Edit" || Session["OrganizationAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void OrganizationAddressesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAddressesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationAddresses ?? new OrganizationAddress[] { };
            OrganizationAddressesRadGrid.DataSource = l;
            OrganizationAddressesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAddressesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAddressesPermission"] == "Edit" || Session["OrganizationAddressesPermission"] != null && l.Any());
        }

        protected void OrganizationAgreementsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAgreementsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationAgreements ?? new OrganizationAgreement[] { };
            OrganizationAgreementsRadGrid.DataSource = l;
            OrganizationAgreementsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAgreementsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAgreementsPermission"] == "Edit" || Session["OrganizationAgreementsPermission"] != null && l.Any());
        }

        protected void OrganizationAliasesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAliasesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationAliases ?? new OrganizationAlias[] { };
            OrganizationAliasesRadGrid.DataSource = l;
            OrganizationAliasesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAliasesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAliasesPermission"] == "Edit" || Session["OrganizationAliasesPermission"] != null && l.Any());
        }

        protected void OrganizationChangelogsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationChangelogsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationChangelogs ?? new OrganizationChangelog[] { };
            OrganizationChangelogsRadGrid.DataSource = l;
            OrganizationChangelogsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationChangelogsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationChangelogsPermission"] == "Edit" || Session["OrganizationChangelogsPermission"] != null && l.Any());
        }

        protected void OrganizationContactsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationContactsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationContacts ?? new OrganizationContact[] { };
            OrganizationContactsRadGrid.DataSource = l;
            OrganizationContactsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationContactsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationContactsPermission"] == "Edit" || Session["OrganizationContactsPermission"] != null && l.Any());
        }

        protected void OrganizationEmailsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationEmailsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationEmails ?? new OrganizationEmail[] { };
            OrganizationEmailsRadGrid.DataSource = l;
            OrganizationEmailsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationEmailsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationEmailsPermission"] == "Edit" || Session["OrganizationEmailsPermission"] != null && l.Any());
        }

        protected void OrganizationInterfacesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationInterfacesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationInterfaces ?? new OrganizationInterface[] { };
            OrganizationInterfacesRadGrid.DataSource = l;
            OrganizationInterfacesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationInterfacesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationInterfacesPermission"] == "Edit" || Session["OrganizationInterfacesPermission"] != null && l.Any());
        }

        protected void OrganizationPhoneNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationPhoneNumbersPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationPhoneNumbers ?? new OrganizationPhoneNumber[] { };
            OrganizationPhoneNumbersRadGrid.DataSource = l;
            OrganizationPhoneNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationPhoneNumbersPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationPhoneNumbersPermission"] == "Edit" || Session["OrganizationPhoneNumbersPermission"] != null && l.Any());
        }

        protected void OrganizationPrivilegedContactsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationPrivilegedContactsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationPrivilegedContacts ?? new OrganizationPrivilegedContact[] { };
            OrganizationPrivilegedContactsRadGrid.DataSource = l;
            OrganizationPrivilegedContactsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationPrivilegedContactsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationPrivilegedContactsPermission"] == "Edit" || Session["OrganizationPrivilegedContactsPermission"] != null && l.Any());
        }

        protected void OrganizationTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationTagsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationTags ?? new OrganizationTag[] { };
            OrganizationTagsRadGrid.DataSource = l;
            OrganizationTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationTagsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationTagsPermission"] == "Edit" || Session["OrganizationTagsPermission"] != null && l.Any());
        }

        protected void OrganizationTypesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationTypesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationTypes ?? new OrganizationType[] { };
            OrganizationTypesRadGrid.DataSource = l;
            OrganizationTypesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationTypesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationTypesPermission"] == "Edit" || Session["OrganizationTypesPermission"] != null && l.Any());
        }

        protected void OrganizationUrlsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationUrlsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id, true).OrganizationUrls ?? new OrganizationUrl[] { };
            OrganizationUrlsRadGrid.DataSource = l;
            OrganizationUrlsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationUrlsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationUrlsPermission"] == "Edit" || Session["OrganizationUrlsPermission"] != null && l.Any());
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Voucher2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "OperationMode", "operationMode" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"vendorId == \"{id}\"",
                Global.GetCqlFilter(Voucher2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Voucher2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementNumber", "disbursementNumber"),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementDate", "disbursementDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "DisbursementAmount", "disbursementAmount"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Enclosure", "enclosureNeeded"),
                Global.GetCqlFilter(Voucher2sRadGrid, "InvoiceCurrency", "invoiceCurrency"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Invoice.Number", "invoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(Voucher2sRadGrid, "ExchangeRate", "exchangeRate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "OperationMode", "operationMode"),
                Global.GetCqlFilter(Voucher2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Voucher2sRadGrid, "SystemCurrency", "systemCurrency"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Type", "type"),
                Global.GetCqlFilter(Voucher2sRadGrid, "VoucherDate", "voucherDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "Number", "voucherNumber"),
                Global.GetCqlFilter(Voucher2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Voucher2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Voucher2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(where, Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = folioServiceContext.CountVoucher2s(where);
            if (Voucher2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Voucher2sRadGrid.AllowFilteringByColumn = Voucher2sRadGrid.VirtualItemCount > 10;
                Voucher2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Voucher2sPermission"] != null && Voucher2sRadGrid.VirtualItemCount > 0;
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
