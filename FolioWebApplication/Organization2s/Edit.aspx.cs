using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Organization2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Organization2FormView.DataSource = new[] { o2 };
            Title = $"Organization {o2.Name}";
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "ChkSubscriptionOverlap", "chkSubscriptionOverlap" }, { "Currency", "currency" }, { "EnclosureNeeded", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDue", "paymentDue" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNo", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(out var i, Global.GetCqlFilter(Invoice2sRadGrid, d, $"vendorId == \"{id}\""), Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = i;
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "NumberPrefix", "poNumberPrefix" }, { "NumberSuffix", "poNumberSuffix" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "WorkflowStatus", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2sRadGrid, d, $"vendor == \"{id}\""), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "Description", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description2", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorReferenceNumber", "vendorDetail.refNumber" }, { "VendorReferenceNumberType", "vendorDetail.refNumberType" }, { "VendorAccount", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2sRadGrid, d, $"eresource.accessProvider == \"{id}\""), OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderItem2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "Description", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description2", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorReferenceNumber", "vendorDetail.refNumber" }, { "VendorReferenceNumberType", "vendorDetail.refNumberType" }, { "VendorAccount", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2s1RadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2s1RadGrid, d, $"physical.materialSupplier == \"{id}\""), OrderItem2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2s1RadGrid.PageSize * OrderItem2s1RadGrid.CurrentPageIndex, OrderItem2s1RadGrid.PageSize, true);
            OrderItem2s1RadGrid.VirtualItemCount = i;
            if (OrderItem2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2s1RadGrid.AllowFilteringByColumn = OrderItem2s1RadGrid.VirtualItemCount > 10;
                OrderItem2s1Panel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2s1RadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
