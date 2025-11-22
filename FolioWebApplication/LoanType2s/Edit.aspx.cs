using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LoanType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoanType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LoanType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var lt2 = folioServiceContext.FindLoanType2(id, true);
            if (lt2 == null) Response.Redirect("Default.aspx");
            lt2.Content = lt2.Content != null ? JsonConvert.DeserializeObject<JToken>(lt2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            LoanType2FormView.DataSource = new[] { lt2 };
            Title = $"Loan Type {lt2.Name}";
        }

        protected void ActualCostRecord2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null) return;
            var id = (Guid?)LoanType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"item.loanTypeId == \"{id}\"",
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LossType", "lossType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LossDate", "lossDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserBarcode", "user.barcode"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserFirstName", "user.firstName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserLastName", "user.lastName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserMiddleName", "user.middleName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserGroup.Name", "user.patronGroupId", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserGroupName", "user.patronGroup"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemMaterialType.Name", "item.materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemMaterialTypeName", "item.materialType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemPermanentLocation.Name", "item.permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemPermanentLocationName", "item.permanentLocation"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveLocation.Name", "item.effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveLocationName", "item.effectiveLocation"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemLoanTypeName", "item.loanType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemHolding.ShortId", "item.holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemVolume", "item.volume"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEnumeration", "item.enumeration"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemChronology", "item.chronology"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemDisplaySummary", "item.displaySummary"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemCopyNumber", "item.copyNumber"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Fee.Title", "feeFine.accountId", "title", folioServiceContext.FolioServiceClient.Fees),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeBilledAmount", "feeFine.billedAmount"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Owner.Name", "feeFine.ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "OwnerName", "feeFine.owner"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeType.Name", "feeFine.typeId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeTypeName", "feeFine.type"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "AdditionalInfoForStaff", "additionalInfoForStaff"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "AdditionalInfoForPatron", "additionalInfoForPatron"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ActualCostRecord2sRadGrid.DataSource = folioServiceContext.ActualCostRecord2s(where, ActualCostRecord2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ActualCostRecord2sRadGrid.PageSize * ActualCostRecord2sRadGrid.CurrentPageIndex, ActualCostRecord2sRadGrid.PageSize, true);
            ActualCostRecord2sRadGrid.VirtualItemCount = folioServiceContext.CountActualCostRecord2s(where);
            if (ActualCostRecord2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ActualCostRecord2sRadGrid.AllowFilteringByColumn = ActualCostRecord2sRadGrid.VirtualItemCount > 10;
                ActualCostRecord2sPanel.Visible = LoanType2FormView.DataKey.Value != null && Session["ActualCostRecord2sPermission"] != null && ActualCostRecord2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)LoanType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "Order", "order" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"permanentLoanTypeId == \"{id}\"",
                Global.GetCqlFilter(Item2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Item2sRadGrid, "Order", "order"),
                Global.GetCqlFilter(Item2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Item2sRadGrid, "DisplaySummary", "displaySummary"),
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
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(where, Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = folioServiceContext.CountItem2s(where);
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = LoanType2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)LoanType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "Order", "order" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"temporaryLoanTypeId == \"{id}\"",
                Global.GetCqlFilter(Item2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2s1RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2s1RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Item2s1RadGrid, "Order", "order"),
                Global.GetCqlFilter(Item2s1RadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Item2s1RadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Item2s1RadGrid, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(Item2s1RadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveShelvingOrder", "effectiveShelvingOrder"),
                Global.GetCqlFilter(Item2s1RadGrid, "CallNumber", "itemLevelCallNumber"),
                Global.GetCqlFilter(Item2s1RadGrid, "CallNumberPrefix", "itemLevelCallNumberPrefix"),
                Global.GetCqlFilter(Item2s1RadGrid, "CallNumberSuffix", "itemLevelCallNumberSuffix"),
                Global.GetCqlFilter(Item2s1RadGrid, "CallNumberType.Name", "itemLevelCallNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveCallNumberType.Name", "effectiveCallNumberComponents.typeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2s1RadGrid, "Volume", "volume"),
                Global.GetCqlFilter(Item2s1RadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Item2s1RadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Item2s1RadGrid, "ItemIdentifier", "itemIdentifier"),
                Global.GetCqlFilter(Item2s1RadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Item2s1RadGrid, "PiecesCount", "numberOfPieces"),
                Global.GetCqlFilter(Item2s1RadGrid, "PiecesDescription", "descriptionOfPieces"),
                Global.GetCqlFilter(Item2s1RadGrid, "MissingPiecesCount", "numberOfMissingPieces"),
                Global.GetCqlFilter(Item2s1RadGrid, "MissingPiecesDescription", "missingPieces"),
                Global.GetCqlFilter(Item2s1RadGrid, "MissingPiecesTime", "missingPiecesDate"),
                Global.GetCqlFilter(Item2s1RadGrid, "DamagedStatus.Name", "itemDamagedStatusId", "name", folioServiceContext.FolioServiceClient.ItemDamagedStatuses),
                Global.GetCqlFilter(Item2s1RadGrid, "DamagedStatusTime", "itemDamagedStatusDate"),
                Global.GetCqlFilter(Item2s1RadGrid, "Status", "status.name"),
                Global.GetCqlFilter(Item2s1RadGrid, "StatusLastWriteTime", "status.date"),
                Global.GetCqlFilter(Item2s1RadGrid, "MaterialType.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Item2s1RadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2s1RadGrid, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2s1RadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2s1RadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2s1RadGrid, "InTransitDestinationServicePoint.Name", "inTransitDestinationServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2s1RadGrid, "OrderItem.Number", "purchaseOrderLineIdentifier", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Item2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Item2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Item2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2s1RadGrid, "LastCheckInDateTime", "lastCheckIn.dateTime"),
                Global.GetCqlFilter(Item2s1RadGrid, "LastCheckInServicePoint.Name", "lastCheckIn.servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2s1RadGrid, "LastCheckInStaffMember.Username", "lastCheckIn.staffMemberId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Item2s1RadGrid.DataSource = folioServiceContext.Item2s(where, Item2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2s1RadGrid.PageSize * Item2s1RadGrid.CurrentPageIndex, Item2s1RadGrid.PageSize, true);
            Item2s1RadGrid.VirtualItemCount = folioServiceContext.CountItem2s(where);
            if (Item2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Item2s1RadGrid.AllowFilteringByColumn = Item2s1RadGrid.VirtualItemCount > 10;
                Item2s1Panel.Visible = LoanType2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2s1RadGrid.VirtualItemCount > 0;
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
