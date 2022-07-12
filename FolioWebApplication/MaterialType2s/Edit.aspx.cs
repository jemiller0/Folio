using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.MaterialType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MaterialType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void MaterialType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var mt2 = folioServiceContext.FindMaterialType2(id, true);
            if (mt2 == null) Response.Redirect("Default.aspx");
            mt2.Content = mt2.Content != null ? JsonConvert.DeserializeObject<JToken>(mt2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            MaterialType2FormView.DataSource = new[] { mt2 };
            Title = $"Material Type {mt2.Name}";
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)MaterialType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"materialTypeId == \"{id}\"",
                Global.GetCqlFilter(Fee2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Fee2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Fee2sRadGrid, "RemainingAmount", "remaining"),
                Global.GetCqlFilter(Fee2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "PaymentStatusName", "paymentStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Fee2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Fee2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType", "materialType"),
                Global.GetCqlFilter(Fee2sRadGrid, "ItemStatusName", "itemStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Location", "location"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "ReturnedTime", "returnedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(Fee2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Fee2sRadGrid, "FeeType.Name", "feeFineId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(Fee2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Fee2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances)
            }.Where(s => s != null)));
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, where, Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = MaterialType2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)MaterialType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"materialTypeId == \"{id}\"",
                Global.GetCqlFilter(Item2sRadGrid, "Id", "id"),
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
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "InTransitDestinationServicePoint.Name", "inTransitDestinationServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2sRadGrid, "OrderItem.Number", "purchaseOrderLineIdentifier", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
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
                Item2sPanel.Visible = MaterialType2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)MaterialType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"eresource.materialType == \"{id}\"",
                Global.GetCqlFilter(OrderItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AgreementId", "agreementId"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AutomaticExport", "automaticExport"),
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
                OrderItem2sPanel.Visible = MaterialType2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void OrderItem2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)MaterialType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"physical.materialType == \"{id}\"",
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AgreementId", "agreementId"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "AutomaticExport", "automaticExport"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
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
                Global.GetCqlFilter(OrderItem2s1RadGrid, "PhysicalMaterialSupplier.Name", "physical.materialSupplier", "name", folioServiceContext.FolioServiceClient.Organizations),
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
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2s1RadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, where, OrderItem2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2s1RadGrid.PageSize * OrderItem2s1RadGrid.CurrentPageIndex, OrderItem2s1RadGrid.PageSize, true);
            OrderItem2s1RadGrid.VirtualItemCount = i;
            if (OrderItem2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2s1RadGrid.AllowFilteringByColumn = OrderItem2s1RadGrid.VirtualItemCount > 10;
                OrderItem2s1Panel.Visible = MaterialType2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2s1RadGrid.VirtualItemCount > 0;
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
