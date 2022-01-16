using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Item2s
{
    public partial class Edit : System.Web.UI.Page
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

        protected void Item2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var i2 = folioServiceContext.FindItem2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Item2FormView.DataSource = new[] { i2 };
            Title = $"Item {i2.ShortId}";
        }

        protected void BoundWithPart2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BoundWithPart2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BoundWithPart2sRadGrid.DataSource = folioServiceContext.BoundWithPart2s(out var i, Global.GetCqlFilter(BoundWithPart2sRadGrid, d, $"itemId == \"{id}\""), BoundWithPart2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BoundWithPart2sRadGrid.PageSize * BoundWithPart2sRadGrid.CurrentPageIndex, BoundWithPart2sRadGrid.PageSize, true);
            BoundWithPart2sRadGrid.VirtualItemCount = i;
            if (BoundWithPart2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BoundWithPart2sRadGrid.AllowFilteringByColumn = BoundWithPart2sRadGrid.VirtualItemCount > 10;
                BoundWithPart2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["BoundWithPart2sPermission"] != null && BoundWithPart2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(out var i, Global.GetCqlFilter(CheckIn2sRadGrid, d, $"itemId == \"{id}\""), CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = i;
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void CirculationNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CirculationNotesPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).CirculationNotes ?? new CirculationNote[] { };
            CirculationNotesRadGrid.DataSource = l;
            CirculationNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            CirculationNotesPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["CirculationNotesPermission"] == "Edit" || Session["CirculationNotesPermission"] != null && l.Any());
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d, $"itemId == \"{id}\""), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void ItemElectronicAccessesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemElectronicAccessesPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemElectronicAccesses ?? new ItemElectronicAccess[] { };
            ItemElectronicAccessesRadGrid.DataSource = l;
            ItemElectronicAccessesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemElectronicAccessesPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemElectronicAccessesPermission"] == "Edit" || Session["ItemElectronicAccessesPermission"] != null && l.Any());
        }

        protected void ItemFormerIdsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemFormerIdsPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemFormerIds ?? new ItemFormerId[] { };
            ItemFormerIdsRadGrid.DataSource = l;
            ItemFormerIdsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemFormerIdsPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemFormerIdsPermission"] == "Edit" || Session["ItemFormerIdsPermission"] != null && l.Any());
        }

        protected void ItemNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemNotesPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemNotes ?? new ItemNote[] { };
            ItemNotesRadGrid.DataSource = l;
            ItemNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemNotesPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemNotesPermission"] == "Edit" || Session["ItemNotesPermission"] != null && l.Any());
        }

        protected void ItemStatisticalCodesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemStatisticalCodesPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemStatisticalCodes ?? new ItemStatisticalCode[] { };
            ItemStatisticalCodesRadGrid.DataSource = l;
            ItemStatisticalCodesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemStatisticalCodesPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemStatisticalCodesPermission"] == "Edit" || Session["ItemStatisticalCodesPermission"] != null && l.Any());
        }

        protected void ItemTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemTagsPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemTags ?? new ItemTag[] { };
            ItemTagsRadGrid.DataSource = l;
            ItemTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemTagsPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemTagsPermission"] == "Edit" || Session["ItemTagsPermission"] != null && l.Any());
        }

        protected void ItemYearCaptionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemYearCaptionsPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemYearCaptions ?? new ItemYearCaption[] { };
            ItemYearCaptionsRadGrid.DataSource = l;
            ItemYearCaptionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemYearCaptionsPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemYearCaptionsPermission"] == "Edit" || Session["ItemYearCaptionsPermission"] != null && l.Any());
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2sRadGrid, d, $"itemId == \"{id}\""), Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "DisplayOnHolding", "displayOnHolding" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "DiscoverySuppress", "discoverySuppress" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d, $"itemId == \"{id}\""), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(out var i, Global.GetCqlFilter(Request2sRadGrid, d, $"itemId == \"{id}\""), Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = i;
            if (Request2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Request2sRadGrid.AllowFilteringByColumn = Request2sRadGrid.VirtualItemCount > 10;
                Request2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
