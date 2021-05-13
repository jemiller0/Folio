using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
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
            var i2 = id == null && (string)Session["Item2sPermission"] == "Edit" ? new Item2() : folioServiceContext.FindItem2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            Item2FormView.DataSource = new[] { i2 };
            Title = $"Item {i2.ShortId}";
        }

        protected void HoldingRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            if (rcb.SelectedValue != "")
            {
                var id = Guid.Parse(rcb.SelectedValue);
                var h2 = folioServiceContext.FindHolding2(id);
                rcb.Items.Add(new RadComboBoxItem(h2.ShortId.ToString(), h2.Id.ToString()));
            }
        }

        protected void HoldingRadComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            foreach (var h2 in folioServiceContext.Holding2s($"hrid == e.Text", take: 100).OrderBy(h2 => h2.ShortId)) rcb.Items.Add(new RadComboBoxItem(h2.ShortId.ToString(), h2.Id.ToString()));
        }

        protected void HoldingCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args) => args.IsValid = folioServiceContext.AnyHolding2s($"hrid = args.Value");

        protected void CallNumberTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.CallNumberType2s(orderBy: "name").ToArray();
        }

        protected void DamagedStatusRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ItemDamagedStatus2s(orderBy: "name").ToArray();
        }

        protected void StatusNameRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new string[] { "Aged to lost", "Available", "Awaiting pickup", "Awaiting delivery", "Checked out", "Claimed returned", "Declared lost", "In process", "In process (non-requestable)", "In transit", "Intellectual item", "Long missing", "Lost and paid", "Missing", "On order", "Paged", "Restricted", "Order closed", "Unavailable", "Unknown", "Withdrawn" };
        }

        protected void MaterialTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.MaterialType2s(orderBy: "name").ToArray();
        }

        protected void PermanentLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void TemporaryLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void PermanentLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void TemporaryLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void InTransitDestinationServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
        }

        protected void OrderItemRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            if (rcb.SelectedValue != "")
            {
                var id = Guid.Parse(rcb.SelectedValue);
                var oi2 = folioServiceContext.FindOrderItem2(id);
                rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
            }
        }

        protected void OrderItemRadComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            foreach (var oi2 in folioServiceContext.OrderItem2s($"poLineNumber == \"{FolioServiceClient.EncodeCql(e.Text)}*\"", take: 100).OrderBy(oi2 => oi2.Number)) rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
        }

        protected void OrderItemCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args) => args.IsValid = folioServiceContext.AnyOrderItem2s($"poLineNumber = \"{FolioServiceClient.EncodeCql(args.Value)}\"");

        protected void LastCheckInServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
        }

        protected void Item2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)Item2FormView.DataKey.Value;
            var i2 = id != null ? folioServiceContext.FindItem2(id) : new Item2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            i2.Version = (int?)e.NewValues["Version"];
            if ((string)e.NewValues["HoldingId"] == "")
            {
                var rfv = (RequiredFieldValidator)Item2FormView.FindControl("HoldingRequiredFieldValidator");
                rfv.IsValid = false;
                e.Cancel = true;
                return;
            }
            i2.HoldingId = (Guid?)Guid.Parse((string)e.NewValues["HoldingId"]);
            i2.DiscoverySuppress = (bool?)e.NewValues["DiscoverySuppress"];
            i2.AccessionNumber = Global.Trim((string)e.NewValues["AccessionNumber"]);
            i2.Barcode = Global.Trim((string)e.NewValues["Barcode"]);
            i2.EffectiveShelvingOrder = Global.Trim((string)e.NewValues["EffectiveShelvingOrder"]);
            i2.CallNumber = Global.Trim((string)e.NewValues["CallNumber"]);
            i2.CallNumberPrefix = Global.Trim((string)e.NewValues["CallNumberPrefix"]);
            i2.CallNumberSuffix = Global.Trim((string)e.NewValues["CallNumberSuffix"]);
            i2.CallNumberTypeId = (string)e.NewValues["CallNumberTypeId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["CallNumberTypeId"]) : null;
            i2.Volume = Global.Trim((string)e.NewValues["Volume"]);
            i2.Enumeration = Global.Trim((string)e.NewValues["Enumeration"]);
            i2.Chronology = Global.Trim((string)e.NewValues["Chronology"]);
            i2.ItemIdentifier = Global.Trim((string)e.NewValues["ItemIdentifier"]);
            i2.CopyNumber = Global.Trim((string)e.NewValues["CopyNumber"]);
            i2.PiecesCount = Global.Trim((string)e.NewValues["PiecesCount"]);
            i2.PiecesDescription = Global.Trim((string)e.NewValues["PiecesDescription"]);
            i2.MissingPiecesCount = Global.Trim((string)e.NewValues["MissingPiecesCount"]);
            i2.MissingPiecesDescription = Global.Trim((string)e.NewValues["MissingPiecesDescription"]);
            i2.MissingPiecesTime = (DateTime?)e.NewValues["MissingPiecesTime"];
            i2.DamagedStatusId = (string)e.NewValues["DamagedStatusId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["DamagedStatusId"]) : null;
            i2.DamagedStatusTime = (DateTime?)e.NewValues["DamagedStatusTime"];
            i2.StatusName = Global.Trim((string)e.NewValues["StatusName"]);
            i2.StatusDate = (DateTime?)e.NewValues["StatusDate"];
            i2.MaterialTypeId = (Guid?)Guid.Parse((string)e.NewValues["MaterialTypeId"]);
            i2.PermanentLoanTypeId = (Guid?)Guid.Parse((string)e.NewValues["PermanentLoanTypeId"]);
            i2.TemporaryLoanTypeId = (string)e.NewValues["TemporaryLoanTypeId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["TemporaryLoanTypeId"]) : null;
            i2.PermanentLocationId = (string)e.NewValues["PermanentLocationId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["PermanentLocationId"]) : null;
            i2.TemporaryLocationId = (string)e.NewValues["TemporaryLocationId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["TemporaryLocationId"]) : null;
            i2.InTransitDestinationServicePointId = (string)e.NewValues["InTransitDestinationServicePointId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["InTransitDestinationServicePointId"]) : null;
            i2.OrderItemId = (string)e.NewValues["OrderItemId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["OrderItemId"]) : null;
            i2.LastCheckInDateTime = (DateTime?)e.NewValues["LastCheckInDateTime"];
            i2.LastCheckInServicePointId = (string)e.NewValues["LastCheckInServicePointId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["LastCheckInServicePointId"]) : null;
            i2.LastWriteTime = DateTime.Now;
            i2.LastWriteUserId = (Guid?)Session["UserId"];
            if (id == null) folioServiceContext.Insert(i2); else folioServiceContext.Update(i2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={i2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void Item2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void Item2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)Item2FormView.DataKey.Value;
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
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
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
