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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BoundWithPart2sRadGrid.DataSource = folioServiceContext.BoundWithPart2s(out var i, where, BoundWithPart2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BoundWithPart2sRadGrid.PageSize * BoundWithPart2sRadGrid.CurrentPageIndex, BoundWithPart2sRadGrid.PageSize, true);
            BoundWithPart2sRadGrid.VirtualItemCount = i;
            if (BoundWithPart2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BoundWithPart2sRadGrid.AllowFilteringByColumn = BoundWithPart2sRadGrid.VirtualItemCount > 10;
                BoundWithPart2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["BoundWithPart2sPermission"] != null && BoundWithPart2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
                Global.GetCqlFilter(CheckIn2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "OccurredDateTime", "occurredDateTime"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "RequestQueueSize", "requestQueueSize"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemLocation.Name", "itemLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ServicePoint.Name", "servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(CheckIn2sRadGrid, "PerformedByUser.Username", "performedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(out var i, where, CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = i;
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
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
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
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
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType1.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
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
                Fee2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ItemAdministrativeNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ItemAdministrativeNotesPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindItem2(id, true).ItemAdministrativeNotes ?? new ItemAdministrativeNote[] { };
            ItemAdministrativeNotesRadGrid.DataSource = l;
            ItemAdministrativeNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ItemAdministrativeNotesPanel.Visible = Item2FormView.DataKey.Value != null && ((string)Session["ItemAdministrativeNotesPermission"] == "Edit" || Session["ItemAdministrativeNotesPermission"] != null && l.Any());
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "IsDcb", "isDcb" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" }, { "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number" }, { "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
                Global.GetCqlFilter(Loan2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Loan2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "ItemEffectiveLocationAtCheckOut.Name", "itemEffectiveLocationIdAtCheckOut", "name", folioServiceContext.FolioServiceClient.Locations),
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
                Loan2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DisplaySummary", "displaySummary" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "BindItemId", "bindItemId" }, { "BindItemTenantId", "bindItemTenantId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "ReceivingTenantId", "receivingTenantId" }, { "DisplayOnHolding", "displayOnHolding" }, { "DisplayToPublic", "displayToPublic" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "Barcode", "barcode" }, { "AccessionNumber", "accessionNumber" }, { "CallNumber", "callNumber" }, { "DiscoverySuppress", "discoverySuppress" }, { "CopyNumber", "copyNumber" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "IsBound", "isBound" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" }, { "StatusUpdatedDate", "statusUpdatedDate" }, { "ClaimingInterval", "claimingInterval" }, { "InternalNote", "internalNote" }, { "ExternalNote", "externalNote" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemId", "bindItemId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemTenantId", "bindItemTenantId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Location.Name", "locationId", "name", folioServiceContext.FolioServiceClient.Locations),
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
                Receiving2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)Item2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"itemId == \"{id}\"",
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Request2sRadGrid, "CancellationReason.Name", "cancellationReasonId", "name", folioServiceContext.FolioServiceClient.CancellationReasons),
                Global.GetCqlFilter(Request2sRadGrid, "CancelledByUser.Username", "cancelledByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "CancellationAdditionalInformation", "cancellationAdditionalInformation"),
                Global.GetCqlFilter(Request2sRadGrid, "CancelledDate", "cancelledDate"),
                Global.GetCqlFilter(Request2sRadGrid, "Position", "position"),
                Global.GetCqlFilter(Request2sRadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(Request2sRadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterFirstName", "requester.firstName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterLastName", "requester.lastName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterMiddleName", "requester.middleName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterBarcode", "requester.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterPatronGroup", "requester.patronGroup"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyFirstName", "proxy.firstName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyLastName", "proxy.lastName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyMiddleName", "proxy.middleName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyBarcode", "proxy.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyPatronGroup", "proxy.patronGroup"),
                Global.GetCqlFilter(Request2sRadGrid, "FulfillmentPreference", "fulfillmentPreference"),
                Global.GetCqlFilter(Request2sRadGrid, "DeliveryAddressType.Name", "deliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(Request2sRadGrid, "RequestExpirationDate", "requestExpirationDate"),
                Global.GetCqlFilter(Request2sRadGrid, "HoldShelfExpirationDate", "holdShelfExpirationDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PickupServicePoint.Name", "pickupServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Request2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Request2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Request2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsPrintCount", "printDetails.printCount"),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsRequester.Username", "printDetails.requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsIsPrinted", "printDetails.isPrinted"),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsPrintEventDate", "printDetails.printEventDate"),
                Global.GetCqlFilter(Request2sRadGrid, "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexShelvingOrder", "searchIndex.shelvingOrder"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName"),
                Global.GetCqlFilter(Request2sRadGrid, "ItemLocationCode", "itemLocationCode")
            }.Where(s => s != null)));
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(out var i, where, Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = i;
            if (Request2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Request2sRadGrid.AllowFilteringByColumn = Request2sRadGrid.VirtualItemCount > 10;
                Request2sPanel.Visible = Item2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
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
