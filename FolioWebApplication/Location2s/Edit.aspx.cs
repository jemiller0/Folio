using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Location2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Location2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Location2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var l2 = folioServiceContext.FindLocation2(id, true);
            if (l2 == null) Response.Redirect("Default.aspx");
            l2.Content = l2.Content != null ? JsonConvert.DeserializeObject<JToken>(l2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Location2FormView.DataSource = new[] { l2 };
            Title = $"Location {l2.Name}";
        }

        protected void ActualCostRecord2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"item.effectiveLocationId == \"{id}\"",
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
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveLocationName", "item.effectiveLocation"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemLoanType.Name", "item.loanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
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
            ActualCostRecord2sRadGrid.DataSource = folioServiceContext.ActualCostRecord2s(out var i, where, ActualCostRecord2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ActualCostRecord2sRadGrid.PageSize * ActualCostRecord2sRadGrid.CurrentPageIndex, ActualCostRecord2sRadGrid.PageSize, true);
            ActualCostRecord2sRadGrid.VirtualItemCount = i;
            if (ActualCostRecord2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ActualCostRecord2sRadGrid.AllowFilteringByColumn = ActualCostRecord2sRadGrid.VirtualItemCount > 10;
                ActualCostRecord2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["ActualCostRecord2sPermission"] != null && ActualCostRecord2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ActualCostRecord2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"item.permanentLocationId == \"{id}\"",
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "LossType", "lossType"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "LossDate", "lossDate"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserBarcode", "user.barcode"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserFirstName", "user.firstName"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserLastName", "user.lastName"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserMiddleName", "user.middleName"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserGroup.Name", "user.patronGroupId", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "UserGroupName", "user.patronGroup"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemMaterialType.Name", "item.materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemMaterialTypeName", "item.materialType"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemPermanentLocationName", "item.permanentLocation"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEffectiveLocation.Name", "item.effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEffectiveLocationName", "item.effectiveLocation"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemLoanType.Name", "item.loanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemLoanTypeName", "item.loanType"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemHolding.ShortId", "item.holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemVolume", "item.volume"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemEnumeration", "item.enumeration"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemChronology", "item.chronology"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemDisplaySummary", "item.displaySummary"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "ItemCopyNumber", "item.copyNumber"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "Fee.Title", "feeFine.accountId", "title", folioServiceContext.FolioServiceClient.Fees),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "FeeBilledAmount", "feeFine.billedAmount"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "Owner.Name", "feeFine.ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "OwnerName", "feeFine.owner"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "FeeType.Name", "feeFine.typeId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "FeeTypeName", "feeFine.type"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "Status", "status"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "AdditionalInfoForStaff", "additionalInfoForStaff"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "AdditionalInfoForPatron", "additionalInfoForPatron"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ActualCostRecord2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ActualCostRecord2s1RadGrid.DataSource = folioServiceContext.ActualCostRecord2s(out var i, where, ActualCostRecord2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ActualCostRecord2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ActualCostRecord2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ActualCostRecord2s1RadGrid.PageSize * ActualCostRecord2s1RadGrid.CurrentPageIndex, ActualCostRecord2s1RadGrid.PageSize, true);
            ActualCostRecord2s1RadGrid.VirtualItemCount = i;
            if (ActualCostRecord2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                ActualCostRecord2s1RadGrid.AllowFilteringByColumn = ActualCostRecord2s1RadGrid.VirtualItemCount > 10;
                ActualCostRecord2s1Panel.Visible = Location2FormView.DataKey.Value != null && Session["ActualCostRecord2sPermission"] != null && ActualCostRecord2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemLocationId == \"{id}\"",
                Global.GetCqlFilter(CheckIn2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "OccurredDateTime", "occurredDateTime"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "RequestQueueSize", "requestQueueSize"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ServicePoint.Name", "servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(CheckIn2sRadGrid, "PerformedByUser.Username", "performedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(out var i, where, CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = i;
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SourceId", "sourceId" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"effectiveLocationId == \"{id}\"",
                Global.GetCqlFilter(Holding2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Holding2sRadGrid, "Source.Name", "sourceId", "name", folioServiceContext.FolioServiceClient.Sources),
                Global.GetCqlFilter(Holding2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Holding2sRadGrid, "HoldingType.Name", "holdingsTypeId", "name", folioServiceContext.FolioServiceClient.HoldingTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Holding2sRadGrid, "Location.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberType.Name", "callNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberPrefix", "callNumberPrefix"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberSuffix", "callNumberSuffix"),
                Global.GetCqlFilter(Holding2sRadGrid, "ShelvingTitle", "shelvingTitle"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionFormat", "acquisitionFormat"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(Holding2sRadGrid, "IllPolicy.Name", "illPolicyId", "name", folioServiceContext.FolioServiceClient.IllPolicies),
                Global.GetCqlFilter(Holding2sRadGrid, "RetentionPolicy", "retentionPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "DigitizationPolicy", "digitizationPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "ItemCount", "numberOfItems"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceivingHistoryDisplayType", "receivingHistory.displayType"),
                Global.GetCqlFilter(Holding2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(out var i, where, Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = i;
            if (Holding2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2sRadGrid.AllowFilteringByColumn = Holding2sRadGrid.VirtualItemCount > 10;
                Holding2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Holding2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SourceId", "sourceId" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"permanentLocationId == \"{id}\"",
                Global.GetCqlFilter(Holding2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Holding2s1RadGrid, "Source.Name", "sourceId", "name", folioServiceContext.FolioServiceClient.Sources),
                Global.GetCqlFilter(Holding2s1RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Holding2s1RadGrid, "HoldingType.Name", "holdingsTypeId", "name", folioServiceContext.FolioServiceClient.HoldingTypes),
                Global.GetCqlFilter(Holding2s1RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Holding2s1RadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2s1RadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2s1RadGrid, "CallNumberType.Name", "callNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Holding2s1RadGrid, "CallNumberPrefix", "callNumberPrefix"),
                Global.GetCqlFilter(Holding2s1RadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Holding2s1RadGrid, "CallNumberSuffix", "callNumberSuffix"),
                Global.GetCqlFilter(Holding2s1RadGrid, "ShelvingTitle", "shelvingTitle"),
                Global.GetCqlFilter(Holding2s1RadGrid, "AcquisitionFormat", "acquisitionFormat"),
                Global.GetCqlFilter(Holding2s1RadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(Holding2s1RadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(Holding2s1RadGrid, "IllPolicy.Name", "illPolicyId", "name", folioServiceContext.FolioServiceClient.IllPolicies),
                Global.GetCqlFilter(Holding2s1RadGrid, "RetentionPolicy", "retentionPolicy"),
                Global.GetCqlFilter(Holding2s1RadGrid, "DigitizationPolicy", "digitizationPolicy"),
                Global.GetCqlFilter(Holding2s1RadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Holding2s1RadGrid, "ItemCount", "numberOfItems"),
                Global.GetCqlFilter(Holding2s1RadGrid, "ReceivingHistoryDisplayType", "receivingHistory.displayType"),
                Global.GetCqlFilter(Holding2s1RadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Holding2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Holding2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Holding2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Holding2s1RadGrid.DataSource = folioServiceContext.Holding2s(out var i, where, Holding2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2s1RadGrid.PageSize * Holding2s1RadGrid.CurrentPageIndex, Holding2s1RadGrid.PageSize, true);
            Holding2s1RadGrid.VirtualItemCount = i;
            if (Holding2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2s1RadGrid.AllowFilteringByColumn = Holding2s1RadGrid.VirtualItemCount > 10;
                Holding2s1Panel.Visible = Location2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Holding2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SourceId", "sourceId" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"temporaryLocationId == \"{id}\"",
                Global.GetCqlFilter(Holding2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(Holding2s2RadGrid, "Source.Name", "sourceId", "name", folioServiceContext.FolioServiceClient.Sources),
                Global.GetCqlFilter(Holding2s2RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Holding2s2RadGrid, "HoldingType.Name", "holdingsTypeId", "name", folioServiceContext.FolioServiceClient.HoldingTypes),
                Global.GetCqlFilter(Holding2s2RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Holding2s2RadGrid, "Location.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2s2RadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2s2RadGrid, "CallNumberType.Name", "callNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Holding2s2RadGrid, "CallNumberPrefix", "callNumberPrefix"),
                Global.GetCqlFilter(Holding2s2RadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Holding2s2RadGrid, "CallNumberSuffix", "callNumberSuffix"),
                Global.GetCqlFilter(Holding2s2RadGrid, "ShelvingTitle", "shelvingTitle"),
                Global.GetCqlFilter(Holding2s2RadGrid, "AcquisitionFormat", "acquisitionFormat"),
                Global.GetCqlFilter(Holding2s2RadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(Holding2s2RadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(Holding2s2RadGrid, "IllPolicy.Name", "illPolicyId", "name", folioServiceContext.FolioServiceClient.IllPolicies),
                Global.GetCqlFilter(Holding2s2RadGrid, "RetentionPolicy", "retentionPolicy"),
                Global.GetCqlFilter(Holding2s2RadGrid, "DigitizationPolicy", "digitizationPolicy"),
                Global.GetCqlFilter(Holding2s2RadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Holding2s2RadGrid, "ItemCount", "numberOfItems"),
                Global.GetCqlFilter(Holding2s2RadGrid, "ReceivingHistoryDisplayType", "receivingHistory.displayType"),
                Global.GetCqlFilter(Holding2s2RadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Holding2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Holding2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Holding2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Holding2s2RadGrid.DataSource = folioServiceContext.Holding2s(out var i, where, Holding2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2s2RadGrid.PageSize * Holding2s2RadGrid.CurrentPageIndex, Holding2s2RadGrid.PageSize, true);
            Holding2s2RadGrid.VirtualItemCount = i;
            if (Holding2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2s2RadGrid.AllowFilteringByColumn = Holding2s2RadGrid.VirtualItemCount > 10;
                Holding2s2Panel.Visible = Location2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"effectiveLocationId == \"{id}\"",
                Global.GetCqlFilter(Item2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
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
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
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
                Item2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"permanentLocationId == \"{id}\"",
                Global.GetCqlFilter(Item2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2s1RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2s1RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
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
                Global.GetCqlFilter(Item2s1RadGrid, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
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
            Item2s1RadGrid.DataSource = folioServiceContext.Item2s(out var i, where, Item2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2s1RadGrid.PageSize * Item2s1RadGrid.CurrentPageIndex, Item2s1RadGrid.PageSize, true);
            Item2s1RadGrid.VirtualItemCount = i;
            if (Item2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Item2s1RadGrid.AllowFilteringByColumn = Item2s1RadGrid.VirtualItemCount > 10;
                Item2s1Panel.Visible = Location2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Item2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"temporaryLocationId == \"{id}\"",
                Global.GetCqlFilter(Item2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2s2RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2s2RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Item2s2RadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Item2s2RadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Item2s2RadGrid, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(Item2s2RadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveShelvingOrder", "effectiveShelvingOrder"),
                Global.GetCqlFilter(Item2s2RadGrid, "CallNumber", "itemLevelCallNumber"),
                Global.GetCqlFilter(Item2s2RadGrid, "CallNumberPrefix", "itemLevelCallNumberPrefix"),
                Global.GetCqlFilter(Item2s2RadGrid, "CallNumberSuffix", "itemLevelCallNumberSuffix"),
                Global.GetCqlFilter(Item2s2RadGrid, "CallNumberType.Name", "itemLevelCallNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveCallNumberType.Name", "effectiveCallNumberComponents.typeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Item2s2RadGrid, "Volume", "volume"),
                Global.GetCqlFilter(Item2s2RadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Item2s2RadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Item2s2RadGrid, "ItemIdentifier", "itemIdentifier"),
                Global.GetCqlFilter(Item2s2RadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Item2s2RadGrid, "PiecesCount", "numberOfPieces"),
                Global.GetCqlFilter(Item2s2RadGrid, "PiecesDescription", "descriptionOfPieces"),
                Global.GetCqlFilter(Item2s2RadGrid, "MissingPiecesCount", "numberOfMissingPieces"),
                Global.GetCqlFilter(Item2s2RadGrid, "MissingPiecesDescription", "missingPieces"),
                Global.GetCqlFilter(Item2s2RadGrid, "MissingPiecesTime", "missingPiecesDate"),
                Global.GetCqlFilter(Item2s2RadGrid, "DamagedStatus.Name", "itemDamagedStatusId", "name", folioServiceContext.FolioServiceClient.ItemDamagedStatuses),
                Global.GetCqlFilter(Item2s2RadGrid, "DamagedStatusTime", "itemDamagedStatusDate"),
                Global.GetCqlFilter(Item2s2RadGrid, "Status", "status.name"),
                Global.GetCqlFilter(Item2s2RadGrid, "StatusLastWriteTime", "status.date"),
                Global.GetCqlFilter(Item2s2RadGrid, "MaterialType.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Item2s2RadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2s2RadGrid, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(Item2s2RadGrid, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2s2RadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Item2s2RadGrid, "InTransitDestinationServicePoint.Name", "inTransitDestinationServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2s2RadGrid, "OrderItem.Number", "purchaseOrderLineIdentifier", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Item2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Item2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Item2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Item2s2RadGrid, "LastCheckInDateTime", "lastCheckIn.dateTime"),
                Global.GetCqlFilter(Item2s2RadGrid, "LastCheckInServicePoint.Name", "lastCheckIn.servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Item2s2RadGrid, "LastCheckInStaffMember.Username", "lastCheckIn.staffMemberId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Item2s2RadGrid.DataSource = folioServiceContext.Item2s(out var i, where, Item2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2s2RadGrid.PageSize * Item2s2RadGrid.CurrentPageIndex, Item2s2RadGrid.PageSize, true);
            Item2s2RadGrid.VirtualItemCount = i;
            if (Item2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Item2s2RadGrid.AllowFilteringByColumn = Item2s2RadGrid.VirtualItemCount > 10;
                Item2s2Panel.Visible = Location2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "IsDcb", "isDcb" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" }, { "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number" }, { "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemEffectiveLocationIdAtCheckOut == \"{id}\"",
                Global.GetCqlFilter(Loan2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Loan2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Loan2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Loan2sRadGrid, "LoanTime", "loanDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "ReturnTime", "returnDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "SystemReturnTime", "systemReturnDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "Action", "action"),
                Global.GetCqlFilter(Loan2sRadGrid, "ActionComment", "actionComment"),
                Global.GetCqlFilter(Loan2sRadGrid, "ItemStatus", "itemStatus"),
                Global.GetCqlFilter(Loan2sRadGrid, "RenewalCount", "renewalCount"),
                Global.GetCqlFilter(Loan2sRadGrid, "LoanPolicy.Name", "loanPolicyId", "name", folioServiceContext.FolioServiceClient.LoanPolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "CheckoutServicePoint.Name", "checkoutServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2sRadGrid, "CheckinServicePoint.Name", "checkinServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2sRadGrid, "Group.Name", "patronGroupIdAtCheckout", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(Loan2sRadGrid, "DueDateChangedByRecall", "dueDateChangedByRecall"),
                Global.GetCqlFilter(Loan2sRadGrid, "IsDcb", "isDcb"),
                Global.GetCqlFilter(Loan2sRadGrid, "DeclaredLostDate", "declaredLostDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "ClaimedReturnedDate", "claimedReturnedDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "OverdueFinePolicy.Name", "overdueFinePolicyId", "name", folioServiceContext.FolioServiceClient.OverdueFinePolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "LostItemPolicy.Name", "lostItemPolicyId", "name", folioServiceContext.FolioServiceClient.LostItemFeePolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled"),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled"),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number"),
                Global.GetCqlFilter(Loan2sRadGrid, "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date")
            }.Where(s => s != null)));
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, where, Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void LocationServicePointsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LocationServicePointsPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindLocation2(id, true).LocationServicePoints ?? new LocationServicePoint[] { };
            LocationServicePointsRadGrid.DataSource = l;
            LocationServicePointsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LocationServicePointsPanel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["LocationServicePointsPermission"] == "Edit" || Session["LocationServicePointsPermission"] != null && l.Any());
        }

        protected void LocationSettingsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LocationSettingsPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.LocationSettings(load: true).Where(ls => ls.LocationId == id).ToArray();
            LocationSettingsRadGrid.DataSource = l;
            LocationSettingsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LocationSettingsPanel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["LocationSettingsPermission"] == "Edit" || Session["LocationSettingsPermission"] != null && l.Any());
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DisplaySummary", "displaySummary" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "BindItemId", "bindItemId" }, { "BindItemTenantId", "bindItemTenantId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "ReceivingTenantId", "receivingTenantId" }, { "DisplayOnHolding", "displayOnHolding" }, { "DisplayToPublic", "displayToPublic" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "Barcode", "barcode" }, { "AccessionNumber", "accessionNumber" }, { "CallNumber", "callNumber" }, { "DiscoverySuppress", "discoverySuppress" }, { "CopyNumber", "copyNumber" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "IsBound", "isBound" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" }, { "StatusUpdatedDate", "statusUpdatedDate" }, { "ClaimingInterval", "claimingInterval" }, { "InternalNote", "internalNote" }, { "ExternalNote", "externalNote" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"locationId == \"{id}\"",
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemId", "bindItemId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemTenantId", "bindItemTenantId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Receiving2sRadGrid, "Title.Title", "titleId", "title", folioServiceContext.FolioServiceClient.Titles),
                Global.GetCqlFilter(Receiving2sRadGrid, "Holding.ShortId", "holdingId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingTenantId", "receivingTenantId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayOnHolding", "displayOnHolding"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayToPublic", "displayToPublic"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Receiving2sRadGrid, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingStatus", "receivingStatus"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Supplement", "supplement"),
                Global.GetCqlFilter(Receiving2sRadGrid, "IsBound", "isBound"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiptTime", "receiptDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiveTime", "receivedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "StatusUpdatedDate", "statusUpdatedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(Receiving2sRadGrid, "InternalNote", "internalNote"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ExternalNote", "externalNote"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Receiving2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, where, Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
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
