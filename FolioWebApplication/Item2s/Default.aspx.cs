using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Item2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Item2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Item2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2sRadGrid, d), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Item2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Response.Write("Id\tShortId\tHolding\tHoldingId\tDiscoverySuppress\tAccessionNumber\tBarcode\tCallNumber\tCallNumberPrefix\tCallNumberSuffix\tCallNumberType\tCallNumberTypeId\tEffectiveCallNumber\tEffectiveCallNumberPrefix\tEffectiveCallNumberSuffix\tEffectiveCallNumberType\tEffectiveCallNumberTypeId\tVolume\tEnumeration\tChronology\tItemIdentifier\tCopyNumber\tPiecesCount\tPiecesDescription\tMissingPiecesCount\tMissingPiecesDescription\tMissingPiecesTime\tDamagedStatus\tDamagedStatusId\tDamagedStatusTime\tStatusName\tStatusDate\tMaterialType\tMaterialTypeId\tPermanentLoanType\tPermanentLoanTypeId\tTemporaryLoanType\tTemporaryLoanTypeId\tPermanentLocation\tPermanentLocationId\tTemporaryLocation\tTemporaryLocationId\tEffectiveLocation\tEffectiveLocationId\tInTransitDestinationServicePoint\tInTransitDestinationServicePointId\tOrderItem\tOrderItemId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tLastCheckInDateTime\tLastCheckInServicePoint\tLastCheckInServicePointId\tLastCheckInStaffMember\tLastCheckInStaffMemberId\r\n");
            foreach (var i2 in folioServiceContext.Item2s(Global.GetCqlFilter(Item2sRadGrid, d), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{i2.Id}\t{i2.ShortId}\t{i2.Holding?.ShortId}\t{i2.HoldingId}\t{i2.DiscoverySuppress}\t{Global.TextEncode(i2.AccessionNumber)}\t{Global.TextEncode(i2.Barcode)}\t{Global.TextEncode(i2.CallNumber)}\t{Global.TextEncode(i2.CallNumberPrefix)}\t{Global.TextEncode(i2.CallNumberSuffix)}\t{Global.TextEncode(i2.CallNumberType?.Name)}\t{i2.CallNumberTypeId}\t{Global.TextEncode(i2.EffectiveCallNumber)}\t{Global.TextEncode(i2.EffectiveCallNumberPrefix)}\t{Global.TextEncode(i2.EffectiveCallNumberSuffix)}\t{Global.TextEncode(i2.EffectiveCallNumberType?.Name)}\t{i2.EffectiveCallNumberTypeId}\t{Global.TextEncode(i2.Volume)}\t{Global.TextEncode(i2.Enumeration)}\t{Global.TextEncode(i2.Chronology)}\t{Global.TextEncode(i2.ItemIdentifier)}\t{Global.TextEncode(i2.CopyNumber)}\t{Global.TextEncode(i2.PiecesCount)}\t{Global.TextEncode(i2.PiecesDescription)}\t{Global.TextEncode(i2.MissingPiecesCount)}\t{Global.TextEncode(i2.MissingPiecesDescription)}\t{i2.MissingPiecesTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.DamagedStatus?.Name)}\t{i2.DamagedStatusId}\t{i2.DamagedStatusTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.StatusName)}\t{i2.StatusDate:M/d/yyyy}\t{Global.TextEncode(i2.MaterialType?.Name)}\t{i2.MaterialTypeId}\t{Global.TextEncode(i2.PermanentLoanType?.Name)}\t{i2.PermanentLoanTypeId}\t{Global.TextEncode(i2.TemporaryLoanType?.Name)}\t{i2.TemporaryLoanTypeId}\t{Global.TextEncode(i2.PermanentLocation?.Name)}\t{i2.PermanentLocationId}\t{Global.TextEncode(i2.TemporaryLocation?.Name)}\t{i2.TemporaryLocationId}\t{Global.TextEncode(i2.EffectiveLocation?.Name)}\t{i2.EffectiveLocationId}\t{Global.TextEncode(i2.InTransitDestinationServicePoint?.Name)}\t{i2.InTransitDestinationServicePointId}\t{Global.TextEncode(i2.OrderItem?.Number)}\t{i2.OrderItemId}\t{i2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.CreationUser?.Username)}\t{i2.CreationUserId}\t{i2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastWriteUser?.Username)}\t{i2.LastWriteUserId}\t{i2.LastCheckInDateTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastCheckInServicePoint?.Name)}\t{i2.LastCheckInServicePointId}\t{Global.TextEncode(i2.LastCheckInStaffMember?.Username)}\t{i2.LastCheckInStaffMemberId}\r\n");
            Response.End();
        }

        protected void Item2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyCheckIn2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a check in");
                if (folioServiceContext.AnyFee2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyLoan2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyReceiving2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a receiving");
                if (folioServiceContext.AnyRequest2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a request");
                folioServiceContext.DeleteItem2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)gei.FindControl("DeleteCustomValidator");
                cv.IsValid = false;
                e.Canceled = true;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
