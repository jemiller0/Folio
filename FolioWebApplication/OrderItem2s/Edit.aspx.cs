using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderItem2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrderItem2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var oi2 = folioServiceContext.FindOrderItem2(id, true);
            if (oi2 == null) Response.Redirect("Default.aspx");
            oi2.Content = oi2.Content != null ? JsonConvert.DeserializeObject<JToken>(oi2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            OrderItem2FormView.DataSource = new[] { oi2 };
            Title = $"Order Item {oi2.Number}";
        }

        protected void InvoiceItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InvoiceItem2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNumber" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "Comment", "comment" }, { "Description", "description" }, { "InvoiceId", "invoiceId" }, { "Number", "invoiceLineNumber" }, { "InvoiceLineStatus", "invoiceLineStatus" }, { "OrderItemId", "poLineId" }, { "ProductId", "productId" }, { "ProductIdTypeId", "productIdType" }, { "Quantity", "quantity" }, { "ReleaseEncumbrance", "releaseEncumbrance" }, { "SubscriptionInfo", "subscriptionInfo" }, { "SubscriptionStartDate", "subscriptionStart" }, { "SubscriptionEndDate", "subscriptionEnd" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"poLineId == \"{id}\"",
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AccountNumber", "accountNumber"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Invoice.Number", "invoiceId", "folioInvoiceNo", folioServiceContext.FolioServiceClient.Invoices),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "Number", "invoiceLineNumber"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "InvoiceLineStatus", "invoiceLineStatus"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "ProductId", "productId"),
                Global.GetCqlFilter(InvoiceItem2sRadGrid, "ProductIdType.Name", "productIdType", "name", folioServiceContext.FolioServiceClient.IdTypes),
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
                InvoiceItem2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["InvoiceItem2sPermission"] != null && InvoiceItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"purchaseOrderLineIdentifier == \"{id}\"",
                Global.GetCqlFilter(Item2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2sRadGrid, "Version", "_version"),
                Global.GetCqlFilter(Item2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Item2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Item2sRadGrid, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(Item2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveShelvingOrder", "effectiveShelvingOrder"),
                Global.GetCqlFilter(Item2sRadGrid, "CallNumber", "itemLevelCallNumber"),
                Global.GetCqlFilter(Item2sRadGrid, "CallNumberPrefix", "itemLevelCallNumberPrefix"),
                Global.GetCqlFilter(Item2sRadGrid, "CallNumberSuffix", "itemLevelCallNumberSuffix"),
                Global.GetCqlFilter(Item2sRadGrid, "CallNumberType.Name", "itemLevelCallNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveCallNumberType.Name", "effectiveCallNumberComponents.typeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2sRadGrid, "Volume", "volume"),
                Global.GetCqlFilter(Item2sRadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Item2sRadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Item2sRadGrid, "ItemIdentifier", "itemIdentifier"),
                Global.GetCqlFilter(Item2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Item2sRadGrid, "PiecesCount", "numberOfPieces"),
                Global.GetCqlFilter(Item2sRadGrid, "PiecesDescription", "descriptionOfPieces"),
                Global.GetCqlFilter(Item2sRadGrid, "MissingPiecesCount", "numberOfMissingPieces"),
                Global.GetCqlFilter(Item2sRadGrid, "MissingPiecesDescription", "missingPieces"),
                Global.GetCqlFilter(Item2sRadGrid, "MissingPiecesTime", "missingPiecesDate"),
                Global.GetCqlFilter(Item2sRadGrid, "DamagedStatus.Name", "itemDamagedStatusId", "name", folioServiceContext.FolioServiceClient.ItemDamagedStatuses),
                Global.GetCqlFilter(Item2sRadGrid, "DamagedStatusTime", "itemDamagedStatusDate"),
                Global.GetCqlFilter(Item2sRadGrid, "Status", "status.name"),
                Global.GetCqlFilter(Item2sRadGrid, "StatusLastWriteTime", "status.date"),
                Global.GetCqlFilter(Item2sRadGrid, "MaterialType.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "InTransitDestinationServicePoint.Name", "inTransitDestinationServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Item2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Item2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2sRadGrid, "LastCheckInDateTime", "lastCheckIn.dateTime"),
                Global.GetCqlFilter(Item2sRadGrid, "LastCheckInServicePoint.Name", "lastCheckIn.servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2sRadGrid, "LastCheckInStaffMember.Username", "lastCheckIn.staffMemberId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, where, Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"packagePoLineId == \"{id}\"",
                Global.GetCqlFilter(OrderItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AgreementId", "agreementId"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
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
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceAccessProvider.Name", "eresource.accessProvider", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseCode", "eresource.license.code"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseDescription", "eresource.license.description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseReference", "eresource.license.reference"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceMaterialType.Name", "eresource.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceResourceUrl", "eresource.resourceUrl"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(OrderItem2sRadGrid, "IsPackage", "isPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "OrderFormat", "orderFormat"),
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
                Global.GetCqlFilter(OrderItem2sRadGrid, "Requester", "requester"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Rush", "rush"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Selector", "selector"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "TitleOrPackage", "titleOrPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorInstructions", "vendorDetail.instructions"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorNote", "vendorDetail.noteFromVendor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorCustomerId", "vendorDetail.vendorAccount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, where, OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItemAlertsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemAlertsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemAlerts ?? new OrderItemAlert[] { };
            OrderItemAlertsRadGrid.DataSource = l;
            OrderItemAlertsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemAlertsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemAlertsPermission"] == "Edit" || Session["OrderItemAlertsPermission"] != null && l.Any());
        }

        protected void OrderItemClaimsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemClaimsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemClaims ?? new OrderItemClaim[] { };
            OrderItemClaimsRadGrid.DataSource = l;
            OrderItemClaimsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemClaimsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemClaimsPermission"] == "Edit" || Session["OrderItemClaimsPermission"] != null && l.Any());
        }

        protected void OrderItemContributorsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemContributorsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemContributors ?? new OrderItemContributor[] { };
            OrderItemContributorsRadGrid.DataSource = l;
            OrderItemContributorsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemContributorsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemContributorsPermission"] == "Edit" || Session["OrderItemContributorsPermission"] != null && l.Any());
        }

        protected void OrderItemFundsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemFundsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemFunds ?? new OrderItemFund[] { };
            OrderItemFundsRadGrid.DataSource = l;
            OrderItemFundsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemFundsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemFundsPermission"] == "Edit" || Session["OrderItemFundsPermission"] != null && l.Any());
        }

        protected void OrderItemLocation2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemLocation2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemLocation2s ?? new OrderItemLocation2[] { };
            OrderItemLocation2sRadGrid.DataSource = l;
            OrderItemLocation2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemLocation2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemLocation2sPermission"] == "Edit" || Session["OrderItemLocation2sPermission"] != null && l.Any());
        }

        protected void OrderItemProductIdsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemProductIdsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemProductIds ?? new OrderItemProductId[] { };
            OrderItemProductIdsRadGrid.DataSource = l;
            OrderItemProductIdsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemProductIdsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemProductIdsPermission"] == "Edit" || Session["OrderItemProductIdsPermission"] != null && l.Any());
        }

        protected void OrderItemReferenceNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemReferenceNumbersPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemReferenceNumbers ?? new OrderItemReferenceNumber[] { };
            OrderItemReferenceNumbersRadGrid.DataSource = l;
            OrderItemReferenceNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemReferenceNumbersPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemReferenceNumbersPermission"] == "Edit" || Session["OrderItemReferenceNumbersPermission"] != null && l.Any());
        }

        protected void OrderItemReportingCodesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemReportingCodesPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemReportingCodes ?? new OrderItemReportingCode[] { };
            OrderItemReportingCodesRadGrid.DataSource = l;
            OrderItemReportingCodesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemReportingCodesPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemReportingCodesPermission"] == "Edit" || Session["OrderItemReportingCodesPermission"] != null && l.Any());
        }

        protected void OrderItemTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemTagsPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemTags ?? new OrderItemTag[] { };
            OrderItemTagsRadGrid.DataSource = l;
            OrderItemTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemTagsPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemTagsPermission"] == "Edit" || Session["OrderItemTagsPermission"] != null && l.Any());
        }

        protected void OrderItemVolumesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItemVolumesPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrderItem2(id, true).OrderItemVolumes ?? new OrderItemVolume[] { };
            OrderItemVolumesRadGrid.DataSource = l;
            OrderItemVolumesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrderItemVolumesPanel.Visible = OrderItem2FormView.DataKey.Value != null && ((string)Session["OrderItemVolumesPermission"] == "Edit" || Session["OrderItemVolumesPermission"] != null && l.Any());
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "DisplayOnHolding", "displayOnHolding" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "DiscoverySuppress", "discoverySuppress" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"poLineId == \"{id}\"",
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Caption", "caption"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Receiving2sRadGrid, "Location.Name", "locationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Receiving2sRadGrid, "Title.Title", "titleId", "title", folioServiceContext.FolioServiceClient.Titles),
                Global.GetCqlFilter(Receiving2sRadGrid, "Holding.ShortId", "holdingId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayOnHolding", "displayOnHolding"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingStatus", "receivingStatus"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Supplement", "supplement"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiptTime", "receiptDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiveTime", "receivedDate")
            }.Where(s => s != null)));
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, where, Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Title2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Title2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"poLineId == \"{id}\"",
                Global.GetCqlFilter(Title2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Title2sRadGrid, "ExpectedReceiptDate", "expectedReceiptDate"),
                Global.GetCqlFilter(Title2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Title2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Title2sRadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(Title2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(Title2sRadGrid, "PackageName", "packageName"),
                Global.GetCqlFilter(Title2sRadGrid, "OrderItemNumber", "poLineNumber"),
                Global.GetCqlFilter(Title2sRadGrid, "PublishedDate", "publishedDate"),
                Global.GetCqlFilter(Title2sRadGrid, "ReceivingNote", "receivingNote"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionFrom", "subscriptionFrom"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionTo", "subscriptionTo"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionInterval", "subscriptionInterval"),
                Global.GetCqlFilter(Title2sRadGrid, "IsAcknowledged", "isAcknowledged"),
                Global.GetCqlFilter(Title2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Title2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Title2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Title2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(out var i, where, Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = i;
            if (Title2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Title2sRadGrid.AllowFilteringByColumn = Title2sRadGrid.VirtualItemCount > 10;
                Title2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Title2sPermission"] != null && Title2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"encumbrance.sourcePoLineId == \"{id}\"",
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
                Global.GetCqlFilter(Transaction2sRadGrid, "ExpenseClass.Name", "expenseClassId", "name", folioServiceContext.FolioServiceClient.ExpenseClasses),
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
                Transaction2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
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
