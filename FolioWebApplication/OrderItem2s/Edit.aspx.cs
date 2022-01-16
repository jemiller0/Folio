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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            InvoiceItem2sRadGrid.DataSource = folioServiceContext.InvoiceItem2s(out var i, Global.GetCqlFilter(InvoiceItem2sRadGrid, d, $"poLineId == \"{id}\""), InvoiceItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InvoiceItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InvoiceItem2sRadGrid.PageSize * InvoiceItem2sRadGrid.CurrentPageIndex, InvoiceItem2sRadGrid.PageSize, true);
            InvoiceItem2sRadGrid.VirtualItemCount = i;
            if (InvoiceItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                InvoiceItem2sRadGrid.AllowFilteringByColumn = InvoiceItem2sRadGrid.VirtualItemCount > 10;
                InvoiceItem2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["InvoiceItem2sPermission"] != null && InvoiceItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2sRadGrid, d, $"purchaseOrderLineIdentifier == \"{id}\""), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2sRadGrid, d, $"packagePoLineId == \"{id}\""), OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
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
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d, $"poLineId == \"{id}\""), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Title2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Title2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(out var i, Global.GetCqlFilter(Title2sRadGrid, d, $"poLineId == \"{id}\""), Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = i;
            if (Title2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Title2sRadGrid.AllowFilteringByColumn = Title2sRadGrid.VirtualItemCount > 10;
                Title2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Title2sPermission"] != null && Title2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Transaction2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Transaction2sPermission"] == null) return;
            var id = (Guid?)OrderItem2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "AwaitingPaymentEncumbranceId", "awaitingPayment.encumbranceId" }, { "AwaitingPaymentReleaseEncumbrance", "awaitingPayment.releaseEncumbrance" }, { "Currency", "currency" }, { "Description", "description" }, { "AwaitingPaymentAmount", "encumbrance.amountAwaitingPayment" }, { "ExpendedAmount", "encumbrance.amountExpended" }, { "InitialEncumberedAmount", "encumbrance.initialAmountEncumbered" }, { "Status", "encumbrance.status" }, { "OrderType", "encumbrance.orderType" }, { "OrderStatus", "encumbrance.orderStatus" }, { "Subscription", "encumbrance.subscription" }, { "ReEncumber", "encumbrance.reEncumber" }, { "OrderId", "encumbrance.sourcePurchaseOrderId" }, { "OrderItemId", "encumbrance.sourcePoLineId" }, { "ExpenseClassId", "expenseClassId" }, { "FiscalYearId", "fiscalYearId" }, { "FromFundId", "fromFundId" }, { "InvoiceCancelled", "invoiceCancelled" }, { "PaymentEncumbranceId", "paymentEncumbranceId" }, { "Source", "source" }, { "SourceFiscalYearId", "sourceFiscalYearId" }, { "InvoiceId", "sourceInvoiceId" }, { "InvoiceItemId", "sourceInvoiceLineId" }, { "ToFundId", "toFundId" }, { "TransactionType", "transactionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Transaction2sRadGrid.DataSource = folioServiceContext.Transaction2s(out var i, Global.GetCqlFilter(Transaction2sRadGrid, d, $"encumbrance.sourcePoLineId == \"{id}\""), Transaction2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Transaction2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Transaction2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Transaction2sRadGrid.PageSize * Transaction2sRadGrid.CurrentPageIndex, Transaction2sRadGrid.PageSize, true);
            Transaction2sRadGrid.VirtualItemCount = i;
            if (Transaction2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Transaction2sRadGrid.AllowFilteringByColumn = Transaction2sRadGrid.VirtualItemCount > 10;
                Transaction2sPanel.Visible = OrderItem2FormView.DataKey.Value != null && Session["Transaction2sPermission"] != null && Transaction2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
