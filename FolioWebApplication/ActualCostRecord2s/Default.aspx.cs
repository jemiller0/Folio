using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ActualCostRecord2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ActualCostRecord2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ActualCostRecord2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tLossType\tLossDate\tExpirationDate\tUserBarcode\tUserFirstName\tUserLastName\tUserMiddleName\tUserGroup\tUserGroupId\tUserGroupName\tItemBarcode\tItemMaterialType\tItemMaterialTypeId\tItemMaterialTypeName\tItemPermanentLocation\tItemPermanentLocationId\tItemPermanentLocationName\tItemEffectiveLocation\tItemEffectiveLocationId\tItemEffectiveLocationName\tItemLoanType\tItemLoanTypeId\tItemLoanTypeName\tItemHolding\tItemHoldingId\tItemEffectiveCallNumber\tItemEffectiveCallNumberPrefix\tItemEffectiveCallNumberSuffix\tItemVolume\tItemEnumeration\tItemChronology\tItemDisplaySummary\tItemCopyNumber\tInstanceTitle\tFee\tFeeId\tFeeBilledAmount\tOwner\tOwnerId\tOwnerName\tFeeType\tFeeTypeId\tFeeTypeName\tStatus\tAdditionalInfoForStaff\tAdditionalInfoForPatron\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
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
            foreach (var acr2 in folioServiceContext.ActualCostRecord2s(where, ActualCostRecord2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{acr2.Id}\t{Global.TextEncode(acr2.LossType)}\t{acr2.LossDate:M/d/yyyy}\t{acr2.ExpirationDate:M/d/yyyy}\t{Global.TextEncode(acr2.UserBarcode)}\t{Global.TextEncode(acr2.UserFirstName)}\t{Global.TextEncode(acr2.UserLastName)}\t{Global.TextEncode(acr2.UserMiddleName)}\t{Global.TextEncode(acr2.UserGroup?.Name)}\t{acr2.UserGroupId}\t{Global.TextEncode(acr2.UserGroupName)}\t{Global.TextEncode(acr2.ItemBarcode)}\t{Global.TextEncode(acr2.ItemMaterialType?.Name)}\t{acr2.ItemMaterialTypeId}\t{Global.TextEncode(acr2.ItemMaterialTypeName)}\t{Global.TextEncode(acr2.ItemPermanentLocation?.Name)}\t{acr2.ItemPermanentLocationId}\t{Global.TextEncode(acr2.ItemPermanentLocationName)}\t{Global.TextEncode(acr2.ItemEffectiveLocation?.Name)}\t{acr2.ItemEffectiveLocationId}\t{Global.TextEncode(acr2.ItemEffectiveLocationName)}\t{Global.TextEncode(acr2.ItemLoanType?.Name)}\t{acr2.ItemLoanTypeId}\t{Global.TextEncode(acr2.ItemLoanTypeName)}\t{acr2.ItemHolding?.ShortId}\t{acr2.ItemHoldingId}\t{Global.TextEncode(acr2.ItemEffectiveCallNumber)}\t{Global.TextEncode(acr2.ItemEffectiveCallNumberPrefix)}\t{Global.TextEncode(acr2.ItemEffectiveCallNumberSuffix)}\t{Global.TextEncode(acr2.ItemVolume)}\t{Global.TextEncode(acr2.ItemEnumeration)}\t{Global.TextEncode(acr2.ItemChronology)}\t{Global.TextEncode(acr2.ItemDisplaySummary)}\t{Global.TextEncode(acr2.ItemCopyNumber)}\t{Global.TextEncode(acr2.InstanceTitle)}\t{Global.TextEncode(acr2.Fee?.Title)}\t{acr2.FeeId}\t{acr2.FeeBilledAmount}\t{Global.TextEncode(acr2.Owner?.Name)}\t{acr2.OwnerId}\t{Global.TextEncode(acr2.OwnerName)}\t{Global.TextEncode(acr2.FeeType?.Name)}\t{acr2.FeeTypeId}\t{Global.TextEncode(acr2.FeeTypeName)}\t{Global.TextEncode(acr2.Status)}\t{Global.TextEncode(acr2.AdditionalInfoForStaff)}\t{Global.TextEncode(acr2.AdditionalInfoForPatron)}\t{acr2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(acr2.CreationUser?.Username)}\t{acr2.CreationUserId}\t{acr2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(acr2.LastWriteUser?.Username)}\t{acr2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
