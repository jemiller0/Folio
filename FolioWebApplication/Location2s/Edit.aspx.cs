using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Location2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Location2FormView.DataSource = new[] { l2 };
            Title = $"Location {l2.Name}";
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(out var i, Global.GetCqlFilter(CheckIn2sRadGrid, d, $"itemLocationId == \"{id}\""), CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = i;
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(out var i, Global.GetCqlFilter(Holding2sRadGrid, d, $"permanentLocationId == \"{id}\""), Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = i;
            if (Holding2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2sRadGrid.AllowFilteringByColumn = Holding2sRadGrid.VirtualItemCount > 10;
                Holding2sPanel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["Holding2sPermission"] == "Edit" || Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0);
            }
        }

        protected void Holding2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            Holding2s1RadGrid.DataSource = folioServiceContext.Holding2s(out var i, Global.GetCqlFilter(Holding2s1RadGrid, d, $"temporaryLocationId == \"{id}\""), Holding2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2s1RadGrid.PageSize * Holding2s1RadGrid.CurrentPageIndex, Holding2s1RadGrid.PageSize, true);
            Holding2s1RadGrid.VirtualItemCount = i;
            if (Holding2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2s1RadGrid.AllowFilteringByColumn = Holding2s1RadGrid.VirtualItemCount > 10;
                Holding2s1Panel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["Holding2sPermission"] == "Edit" || Session["Holding2sPermission"] != null && Holding2s1RadGrid.VirtualItemCount > 0);
            }
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2sRadGrid, d, $"effectiveLocationId == \"{id}\""), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["Item2sPermission"] == "Edit" || Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0);
            }
        }

        protected void Item2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2s1RadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2s1RadGrid, d, $"permanentLocationId == \"{id}\""), Item2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2s1RadGrid.PageSize * Item2s1RadGrid.CurrentPageIndex, Item2s1RadGrid.PageSize, true);
            Item2s1RadGrid.VirtualItemCount = i;
            if (Item2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Item2s1RadGrid.AllowFilteringByColumn = Item2s1RadGrid.VirtualItemCount > 10;
                Item2s1Panel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["Item2sPermission"] == "Edit" || Session["Item2sPermission"] != null && Item2s1RadGrid.VirtualItemCount > 0);
            }
        }

        protected void Item2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2s2RadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2s2RadGrid, d, $"temporaryLocationId == \"{id}\""), Item2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2s2RadGrid.PageSize * Item2s2RadGrid.CurrentPageIndex, Item2s2RadGrid.PageSize, true);
            Item2s2RadGrid.VirtualItemCount = i;
            if (Item2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Item2s2RadGrid.AllowFilteringByColumn = Item2s2RadGrid.VirtualItemCount > 10;
                Item2s2Panel.Visible = Location2FormView.DataKey.Value != null && ((string)Session["Item2sPermission"] == "Edit" || Session["Item2sPermission"] != null && Item2s2RadGrid.VirtualItemCount > 0);
            }
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Location2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2sRadGrid, d, $"itemEffectiveLocationIdAtCheckOut == \"{id}\""), Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d, $"locationId == \"{id}\""), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Location2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
