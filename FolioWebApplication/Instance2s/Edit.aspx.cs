using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Instance2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Instance2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Instance2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var i2 = folioServiceContext.FindInstance2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Instance2FormView.DataSource = new[] { i2 };
            Title = $"Instance {i2.Title}";
        }

        protected void AlternativeTitlesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AlternativeTitlesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).AlternativeTitles ?? new AlternativeTitle[] { };
            AlternativeTitlesRadGrid.DataSource = l;
            AlternativeTitlesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AlternativeTitlesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["AlternativeTitlesPermission"] == "Edit" || Session["AlternativeTitlesPermission"] != null && l.Any());
        }

        protected void ClassificationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ClassificationsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Classifications ?? new Classification[] { };
            ClassificationsRadGrid.DataSource = l;
            ClassificationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ClassificationsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ClassificationsPermission"] == "Edit" || Session["ClassificationsPermission"] != null && l.Any());
        }

        protected void ContributorsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContributorsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Contributors ?? new Contributor[] { };
            ContributorsRadGrid.DataSource = l;
            ContributorsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContributorsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ContributorsPermission"] == "Edit" || Session["ContributorsPermission"] != null && l.Any());
        }

        protected void EditionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["EditionsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Editions ?? new Edition[] { };
            EditionsRadGrid.DataSource = l;
            EditionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            EditionsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["EditionsPermission"] == "Edit" || Session["EditionsPermission"] != null && l.Any());
        }

        protected void ElectronicAccessesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ElectronicAccessesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).ElectronicAccesses ?? new ElectronicAccess[] { };
            ElectronicAccessesRadGrid.DataSource = l;
            ElectronicAccessesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ElectronicAccessesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ElectronicAccessesPermission"] == "Edit" || Session["ElectronicAccessesPermission"] != null && l.Any());
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d, $"instanceId == \"{id}\""), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(out var i, Global.GetCqlFilter(Holding2sRadGrid, d, $"instanceId == \"{id}\""), Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = i;
            if (Holding2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2sRadGrid.AllowFilteringByColumn = Holding2sRadGrid.VirtualItemCount > 10;
                Holding2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Holding2sItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var rg = (RadGrid)sender;
            var id = (Guid?)((GridDataItem)rg.Parent.Parent).GetDataKeyValue("Id");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            rg.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(rg, d, $"holdingsRecordId == \"{id}\""), rg.MasterTableView.SortExpressions.Count > 0 ? $"{d[rg.MasterTableView.SortExpressions[0].FieldName]}{(rg.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, rg.PageSize * rg.CurrentPageIndex, rg.PageSize, true);
            rg.VirtualItemCount = i;
            if (rg.MasterTableView.FilterExpression == "")
            {
                rg.AllowFilteringByColumn = rg.VirtualItemCount > 10;
            }
        }

        protected void IdentifiersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["IdentifiersPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Identifiers ?? new Identifier[] { };
            IdentifiersRadGrid.DataSource = l;
            IdentifiersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            IdentifiersPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["IdentifiersPermission"] == "Edit" || Session["IdentifiersPermission"] != null && l.Any());
        }

        protected void InstanceFormat2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceFormat2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).InstanceFormat2s ?? new InstanceFormat2[] { };
            InstanceFormat2sRadGrid.DataSource = l;
            InstanceFormat2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceFormat2sPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceFormat2sPermission"] == "Edit" || Session["InstanceFormat2sPermission"] != null && l.Any());
        }

        protected void InstanceNatureOfContentTermsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceNatureOfContentTermsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).InstanceNatureOfContentTerms ?? new InstanceNatureOfContentTerm[] { };
            InstanceNatureOfContentTermsRadGrid.DataSource = l;
            InstanceNatureOfContentTermsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceNatureOfContentTermsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceNatureOfContentTermsPermission"] == "Edit" || Session["InstanceNatureOfContentTermsPermission"] != null && l.Any());
        }

        protected void InstanceStatisticalCodesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceStatisticalCodesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).InstanceStatisticalCodes ?? new InstanceStatisticalCode[] { };
            InstanceStatisticalCodesRadGrid.DataSource = l;
            InstanceStatisticalCodesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceStatisticalCodesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceStatisticalCodesPermission"] == "Edit" || Session["InstanceStatisticalCodesPermission"] != null && l.Any());
        }

        protected void InstanceTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceTagsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).InstanceTags ?? new InstanceTag[] { };
            InstanceTagsRadGrid.DataSource = l;
            InstanceTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceTagsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceTagsPermission"] == "Edit" || Session["InstanceTagsPermission"] != null && l.Any());
        }

        protected void LanguagesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LanguagesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Languages ?? new Language[] { };
            LanguagesRadGrid.DataSource = l;
            LanguagesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LanguagesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["LanguagesPermission"] == "Edit" || Session["LanguagesPermission"] != null && l.Any());
        }

        protected void Note2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Note2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Note2s ?? new Note2[] { };
            Note2sRadGrid.DataSource = l;
            Note2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            Note2sPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["Note2sPermission"] == "Edit" || Session["Note2sPermission"] != null && l.Any());
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2sRadGrid, d, $"instanceId == \"{id}\""), OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void PhysicalDescriptionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PhysicalDescriptionsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).PhysicalDescriptions ?? new PhysicalDescription[] { };
            PhysicalDescriptionsRadGrid.DataSource = l;
            PhysicalDescriptionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PhysicalDescriptionsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PhysicalDescriptionsPermission"] == "Edit" || Session["PhysicalDescriptionsPermission"] != null && l.Any());
        }

        protected void PrecedingSucceedingTitle2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "Hrid", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PrecedingSucceedingTitle2sRadGrid.DataSource = folioServiceContext.PrecedingSucceedingTitle2s(out var i, Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, d, $"precedingInstanceId == \"{id}\""), PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PrecedingSucceedingTitle2sRadGrid.PageSize * PrecedingSucceedingTitle2sRadGrid.CurrentPageIndex, PrecedingSucceedingTitle2sRadGrid.PageSize, true);
            PrecedingSucceedingTitle2sRadGrid.VirtualItemCount = i;
            if (PrecedingSucceedingTitle2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PrecedingSucceedingTitle2sRadGrid.AllowFilteringByColumn = PrecedingSucceedingTitle2sRadGrid.VirtualItemCount > 10;
                PrecedingSucceedingTitle2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["PrecedingSucceedingTitle2sPermission"] != null && PrecedingSucceedingTitle2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void PrecedingSucceedingTitle2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "Hrid", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PrecedingSucceedingTitle2s1RadGrid.DataSource = folioServiceContext.PrecedingSucceedingTitle2s(out var i, Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, d, $"succeedingInstanceId == \"{id}\""), PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PrecedingSucceedingTitle2s1RadGrid.PageSize * PrecedingSucceedingTitle2s1RadGrid.CurrentPageIndex, PrecedingSucceedingTitle2s1RadGrid.PageSize, true);
            PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount = i;
            if (PrecedingSucceedingTitle2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                PrecedingSucceedingTitle2s1RadGrid.AllowFilteringByColumn = PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount > 10;
                PrecedingSucceedingTitle2s1Panel.Visible = Instance2FormView.DataKey.Value != null && Session["PrecedingSucceedingTitle2sPermission"] != null && PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void PublicationFrequenciesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationFrequenciesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).PublicationFrequencies ?? new PublicationFrequency[] { };
            PublicationFrequenciesRadGrid.DataSource = l;
            PublicationFrequenciesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationFrequenciesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationFrequenciesPermission"] == "Edit" || Session["PublicationFrequenciesPermission"] != null && l.Any());
        }

        protected void PublicationRangesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationRangesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).PublicationRanges ?? new PublicationRange[] { };
            PublicationRangesRadGrid.DataSource = l;
            PublicationRangesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationRangesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationRangesPermission"] == "Edit" || Session["PublicationRangesPermission"] != null && l.Any());
        }

        protected void PublicationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Publications ?? new Publication[] { };
            PublicationsRadGrid.DataSource = l;
            PublicationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationsPermission"] == "Edit" || Session["PublicationsPermission"] != null && l.Any());
        }

        protected void RelationshipsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            RelationshipsRadGrid.DataSource = folioServiceContext.Relationships(out var i, Global.GetCqlFilter(RelationshipsRadGrid, d, $"subInstanceId == \"{id}\""), RelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RelationshipsRadGrid.PageSize * RelationshipsRadGrid.CurrentPageIndex, RelationshipsRadGrid.PageSize, true);
            RelationshipsRadGrid.VirtualItemCount = i;
            if (RelationshipsRadGrid.MasterTableView.FilterExpression == "")
            {
                RelationshipsRadGrid.AllowFilteringByColumn = RelationshipsRadGrid.VirtualItemCount > 10;
                RelationshipsPanel.Visible = Instance2FormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && RelationshipsRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Relationships1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Relationships1RadGrid.DataSource = folioServiceContext.Relationships(out var i, Global.GetCqlFilter(Relationships1RadGrid, d, $"superInstanceId == \"{id}\""), Relationships1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Relationships1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Relationships1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Relationships1RadGrid.PageSize * Relationships1RadGrid.CurrentPageIndex, Relationships1RadGrid.PageSize, true);
            Relationships1RadGrid.VirtualItemCount = i;
            if (Relationships1RadGrid.MasterTableView.FilterExpression == "")
            {
                Relationships1RadGrid.AllowFilteringByColumn = Relationships1RadGrid.VirtualItemCount > 10;
                Relationships1Panel.Visible = Instance2FormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && Relationships1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void SeriesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["SeriesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Series ?? new Series[] { };
            SeriesRadGrid.DataSource = l;
            SeriesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            SeriesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["SeriesPermission"] == "Edit" || Session["SeriesPermission"] != null && l.Any());
        }

        protected void SubjectsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["SubjectsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id).Subjects ?? new Subject[] { };
            SubjectsRadGrid.DataSource = l;
            SubjectsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            SubjectsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["SubjectsPermission"] == "Edit" || Session["SubjectsPermission"] != null && l.Any());
        }

        protected void Title2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Title2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(out var i, Global.GetCqlFilter(Title2sRadGrid, d, $"instanceId == \"{id}\""), Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = i;
            if (Title2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Title2sRadGrid.AllowFilteringByColumn = Title2sRadGrid.VirtualItemCount > 10;
                Title2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Title2sPermission"] != null && Title2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
