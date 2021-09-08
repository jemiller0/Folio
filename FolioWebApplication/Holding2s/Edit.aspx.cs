using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Holding2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Holding2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Holding2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var h2 = id == null && (string)Session["Holding2sPermission"] == "Edit" ? new Holding2() : folioServiceContext.FindHolding2(id, true);
            if (h2 == null) Response.Redirect("Default.aspx");
            Holding2FormView.DataSource = new[] { h2 };
            Title = $"Holding {h2.ShortId}";
        }

        protected void HoldingTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.HoldingType2s(orderBy: "name").ToArray();
        }

        protected void InstanceRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            if (rcb.SelectedValue != "")
            {
                var id = Guid.Parse(rcb.SelectedValue);
                var i2 = folioServiceContext.FindInstance2(id);
                rcb.Items.Add(new RadComboBoxItem(i2.Title, i2.Id.ToString()));
            }
        }

        protected void InstanceRadComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            foreach (var i2 in folioServiceContext.Instance2s($"title == \"{FolioServiceClient.EncodeCql(e.Text)}*\"", take: 100).OrderBy(i2 => i2.Title)) rcb.Items.Add(new RadComboBoxItem(i2.Title, i2.Id.ToString()));
        }

        protected void InstanceCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args) => args.IsValid = folioServiceContext.AnyInstance2s($"title = \"{FolioServiceClient.EncodeCql(args.Value)}\"");

        protected void LocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void TemporaryLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void EffectiveLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void CallNumberTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.CallNumberType2s(orderBy: "name").ToArray();
        }

        protected void IllPolicyRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.IllPolicy2s(orderBy: "name").ToArray();
        }

        protected void SourceRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Source2s(orderBy: "name").ToArray();
        }

        protected void Holding2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)Holding2FormView.DataKey.Value;
            var h2 = id != null ? folioServiceContext.FindHolding2(id) : new Holding2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            h2.Version = (int?)e.NewValues["Version"];
            h2.HoldingTypeId = (string)e.NewValues["HoldingTypeId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["HoldingTypeId"]) : null;
            if ((string)e.NewValues["InstanceId"] == "")
            {
                var rfv = (RequiredFieldValidator)Holding2FormView.FindControl("InstanceRequiredFieldValidator");
                rfv.IsValid = false;
                e.Cancel = true;
                return;
            }
            h2.InstanceId = (Guid?)Guid.Parse((string)e.NewValues["InstanceId"]);
            h2.LocationId = (Guid?)Guid.Parse((string)e.NewValues["LocationId"]);
            h2.TemporaryLocationId = (string)e.NewValues["TemporaryLocationId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["TemporaryLocationId"]) : null;
            h2.EffectiveLocationId = (string)e.NewValues["EffectiveLocationId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["EffectiveLocationId"]) : null;
            h2.CallNumberTypeId = (string)e.NewValues["CallNumberTypeId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["CallNumberTypeId"]) : null;
            h2.CallNumberPrefix = Global.Trim((string)e.NewValues["CallNumberPrefix"]);
            h2.CallNumber = Global.Trim((string)e.NewValues["CallNumber"]);
            h2.CallNumberSuffix = Global.Trim((string)e.NewValues["CallNumberSuffix"]);
            h2.ShelvingTitle = Global.Trim((string)e.NewValues["ShelvingTitle"]);
            h2.AcquisitionFormat = Global.Trim((string)e.NewValues["AcquisitionFormat"]);
            h2.AcquisitionMethod = Global.Trim((string)e.NewValues["AcquisitionMethod"]);
            h2.ReceiptStatus = Global.Trim((string)e.NewValues["ReceiptStatus"]);
            h2.IllPolicyId = (string)e.NewValues["IllPolicyId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["IllPolicyId"]) : null;
            h2.RetentionPolicy = Global.Trim((string)e.NewValues["RetentionPolicy"]);
            h2.DigitizationPolicy = Global.Trim((string)e.NewValues["DigitizationPolicy"]);
            h2.CopyNumber = Global.Trim((string)e.NewValues["CopyNumber"]);
            h2.ItemCount = Global.Trim((string)e.NewValues["ItemCount"]);
            h2.ReceivingHistoryDisplayType = Global.Trim((string)e.NewValues["ReceivingHistoryDisplayType"]);
            h2.DiscoverySuppress = (bool?)e.NewValues["DiscoverySuppress"];
            h2.SourceId = (string)e.NewValues["SourceId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["SourceId"]) : null;
            h2.LastWriteTime = DateTime.Now;
            h2.LastWriteUserId = (Guid?)Session["UserId"];
            if (id == null) folioServiceContext.Insert(h2); else folioServiceContext.Update(h2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={h2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void Holding2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void Holding2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)Holding2FormView.DataKey.Value;
            try
            {
                if (folioServiceContext.AnyBoundWithPart2s($"holdingsRecordId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a bound with part");
                if (folioServiceContext.AnyFee2s($"holdingsRecordId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyItem2s($"holdingsRecordId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a item");
                if (folioServiceContext.AnyReceiving2s($"holdingId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a receiving");
                folioServiceContext.DeleteHolding2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        protected void BoundWithPart2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BoundWithPart2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BoundWithPart2sRadGrid.DataSource = folioServiceContext.BoundWithPart2s(out var i, Global.GetCqlFilter(BoundWithPart2sRadGrid, d, $"holdingsRecordId == \"{id}\""), BoundWithPart2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BoundWithPart2sRadGrid.PageSize * BoundWithPart2sRadGrid.CurrentPageIndex, BoundWithPart2sRadGrid.PageSize, true);
            BoundWithPart2sRadGrid.VirtualItemCount = i;
            if (BoundWithPart2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BoundWithPart2sRadGrid.AllowFilteringByColumn = BoundWithPart2sRadGrid.VirtualItemCount > 10;
                BoundWithPart2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["BoundWithPart2sPermission"] != null && BoundWithPart2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d, $"holdingsRecordId == \"{id}\""), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Item2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                e.Item.OwnerTableView.InsertItem(new Item2());
                e.Canceled = true;
            }
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2sRadGrid, d, $"holdingsRecordId == \"{id}\""), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["Item2sPermission"] == "Edit" || Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0);
            }
        }

        protected void Item2sRadGrid_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = gei is GridEditFormInsertItem ? null : (Guid?)gei.GetDataKeyValue("Id");
            var d = new Dictionary<string, object>();
            gei.ExtractValues(d);
            var i2 = id != null ? folioServiceContext.FindItem2(id) : new Item2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            i2.Version = (int?)d["Version"];
            i2.DiscoverySuppress = (bool?)d["DiscoverySuppress"];
            i2.AccessionNumber = Global.Trim((string)d["AccessionNumber"]);
            i2.Barcode = Global.Trim((string)d["Barcode"]);
            i2.CallNumber = Global.Trim((string)d["CallNumber"]);
            i2.CallNumberPrefix = Global.Trim((string)d["CallNumberPrefix"]);
            i2.CallNumberSuffix = Global.Trim((string)d["CallNumberSuffix"]);
            i2.CallNumberTypeId = (string)d["CallNumberTypeId"] != "" ? (Guid?)Guid.Parse((string)d["CallNumberTypeId"]) : null;
            i2.Volume = Global.Trim((string)d["Volume"]);
            i2.Enumeration = Global.Trim((string)d["Enumeration"]);
            i2.Chronology = Global.Trim((string)d["Chronology"]);
            i2.ItemIdentifier = Global.Trim((string)d["ItemIdentifier"]);
            i2.CopyNumber = Global.Trim((string)d["CopyNumber"]);
            i2.PiecesCount = Global.Trim((string)d["PiecesCount"]);
            i2.PiecesDescription = Global.Trim((string)d["PiecesDescription"]);
            i2.MissingPiecesCount = Global.Trim((string)d["MissingPiecesCount"]);
            i2.MissingPiecesDescription = Global.Trim((string)d["MissingPiecesDescription"]);
            i2.MissingPiecesTime = (DateTime?)d["MissingPiecesTime"];
            i2.DamagedStatusId = (string)d["DamagedStatusId"] != "" ? (Guid?)Guid.Parse((string)d["DamagedStatusId"]) : null;
            i2.DamagedStatusTime = (DateTime?)d["DamagedStatusTime"];
            i2.StatusName = Global.Trim((string)d["StatusName"]);
            i2.MaterialTypeId = (Guid?)Guid.Parse((string)d["MaterialTypeId"]);
            i2.PermanentLoanTypeId = (Guid?)Guid.Parse((string)d["PermanentLoanTypeId"]);
            i2.TemporaryLoanTypeId = (string)d["TemporaryLoanTypeId"] != "" ? (Guid?)Guid.Parse((string)d["TemporaryLoanTypeId"]) : null;
            i2.PermanentLocationId = (string)d["PermanentLocationId"] != "" ? (Guid?)Guid.Parse((string)d["PermanentLocationId"]) : null;
            i2.TemporaryLocationId = (string)d["TemporaryLocationId"] != "" ? (Guid?)Guid.Parse((string)d["TemporaryLocationId"]) : null;
            i2.InTransitDestinationServicePointId = (string)d["InTransitDestinationServicePointId"] != "" ? (Guid?)Guid.Parse((string)d["InTransitDestinationServicePointId"]) : null;
            i2.OrderItemId = (string)d["OrderItemId"] != "" ? (Guid?)Guid.Parse((string)d["OrderItemId"]) : null;
            i2.LastCheckInDateTime = (DateTime?)d["LastCheckInDateTime"];
            i2.LastCheckInServicePointId = (string)d["LastCheckInServicePointId"] != "" ? (Guid?)Guid.Parse((string)d["LastCheckInServicePointId"]) : null;
            i2.LastWriteTime = DateTime.Now;
            i2.LastWriteUserId = (Guid?)Session["UserId"];
            if (id == null) folioServiceContext.Insert(i2); else folioServiceContext.Update(i2);
        }

        protected void Item2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyBoundWithPart2s($"itemId == \"{id}\"")) throw new Exception("Item cannot be deleted because it is being referenced by a bound with part");
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

        protected void Item2sCallNumberTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.CallNumberType2s(orderBy: "name").ToArray();
        }

        protected void Item2sDamagedStatusRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ItemDamagedStatus2s(orderBy: "name").ToArray();
        }

        protected void Item2sStatusNameRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new string[] { "Aged to lost", "Available", "Awaiting pickup", "Awaiting delivery", "Checked out", "Claimed returned", "Declared lost", "In process", "In process (non-requestable)", "In transit", "Intellectual item", "Long missing", "Lost and paid", "Missing", "On order", "Paged", "Restricted", "Order closed", "Unavailable", "Unknown", "Withdrawn" };
        }

        protected void Item2sMaterialTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.MaterialType2s(orderBy: "name").ToArray();
        }

        protected void Item2sPermanentLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void Item2sTemporaryLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void Item2sPermanentLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Item2sTemporaryLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Item2sInTransitDestinationServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
        }

        protected void Item2sOrderItemRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            if (rcb.SelectedValue != "")
            {
                var id = Guid.Parse(rcb.SelectedValue);
                var oi2 = folioServiceContext.FindOrderItem2(id);
                rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
            }
        }

        protected void Item2sOrderItemRadComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            foreach (var oi2 in folioServiceContext.OrderItem2s($"poLineNumber == \"{FolioServiceClient.EncodeCql(e.Text)}*\"", take: 100).OrderBy(oi2 => oi2.Number)) rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
        }

        protected void Item2sOrderItemCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args) => args.IsValid = folioServiceContext.AnyOrderItem2s($"poLineNumber = \"{FolioServiceClient.EncodeCql(args.Value)}\"");

        protected void Item2sLastCheckInServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "DisplayOnHolding", "displayOnHolding" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "DiscoverySuppress", "discoverySuppress" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d, $"holdingId == \"{id}\""), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
