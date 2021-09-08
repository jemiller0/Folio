using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
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
            var i2 = id == null && (string)Session["Instance2sPermission"] == "Edit" ? new Instance2 { CatalogedDate = DateTime.Now.Date } : folioServiceContext.FindInstance2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            Instance2FormView.DataSource = new[] { i2 };
            Title = $"Instance {i2.Title}";
        }

        protected void InstanceTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.InstanceType2s(orderBy: "name").ToArray();
        }

        protected void IssuanceModeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.IssuanceModes(orderBy: "name").ToArray();
        }

        protected void StatusRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Statuses(orderBy: "name").ToArray();
        }

        protected void Instance2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)Instance2FormView.DataKey.Value;
            var i2 = id != null ? folioServiceContext.FindInstance2(id) : new Instance2 { Id = Guid.NewGuid(), Source = "FOLIO", CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            i2.Version = (int?)e.NewValues["Version"];
            i2.MatchKey = Global.Trim((string)e.NewValues["MatchKey"]);
            i2.Title = Global.Trim((string)e.NewValues["Title"]);
            i2.PublicationPeriodStart = (int?)e.NewValues["PublicationPeriodStart"];
            i2.PublicationPeriodEnd = (int?)e.NewValues["PublicationPeriodEnd"];
            i2.InstanceTypeId = (Guid?)Guid.Parse((string)e.NewValues["InstanceTypeId"]);
            i2.IssuanceModeId = (string)e.NewValues["IssuanceModeId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["IssuanceModeId"]) : null;
            i2.CatalogedDate = (DateTime?)e.NewValues["CatalogedDate"];
            i2.PreviouslyHeld = (bool?)e.NewValues["PreviouslyHeld"];
            i2.StaffSuppress = (bool?)e.NewValues["StaffSuppress"];
            i2.DiscoverySuppress = (bool?)e.NewValues["DiscoverySuppress"];
            i2.StatusId = (string)e.NewValues["StatusId"] != "" ? (Guid?)Guid.Parse((string)e.NewValues["StatusId"]) : null;
            i2.LastWriteTime = DateTime.Now;
            i2.LastWriteUserId = (Guid?)Session["UserId"];
            if (id == null) folioServiceContext.Insert(i2); else folioServiceContext.Update(i2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={i2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void Instance2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void Instance2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)Instance2FormView.DataKey.Value;
            try
            {
                if (folioServiceContext.AnyFee2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyHolding2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a holding");
                if (folioServiceContext.AnyOrderItem2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a order item");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"precedingInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"succeedingInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.AnyRecord2s($"externalIdsHolder.instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRelationships($"subInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationships($"superInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyTitle2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a title");
                folioServiceContext.DeleteInstance2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
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

        protected void Holding2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                e.Item.OwnerTableView.InsertItem(new Holding2());
                e.Canceled = true;
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
                Holding2sPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["Holding2sPermission"] == "Edit" || Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0);
            }
        }

        protected void Holding2sRadGrid_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = gei is GridEditFormInsertItem ? null : (Guid?)gei.GetDataKeyValue("Id");
            var d = new Dictionary<string, object>();
            gei.ExtractValues(d);
            var h2 = id != null ? folioServiceContext.FindHolding2(id) : new Holding2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            h2.Version = (int?)d["Version"];
            h2.HoldingTypeId = (string)d["HoldingTypeId"] != "" ? (Guid?)Guid.Parse((string)d["HoldingTypeId"]) : null;
            h2.LocationId = (Guid?)Guid.Parse((string)d["LocationId"]);
            h2.TemporaryLocationId = (string)d["TemporaryLocationId"] != "" ? (Guid?)Guid.Parse((string)d["TemporaryLocationId"]) : null;
            h2.EffectiveLocationId = (string)d["EffectiveLocationId"] != "" ? (Guid?)Guid.Parse((string)d["EffectiveLocationId"]) : null;
            h2.CallNumberTypeId = (string)d["CallNumberTypeId"] != "" ? (Guid?)Guid.Parse((string)d["CallNumberTypeId"]) : null;
            h2.CallNumberPrefix = Global.Trim((string)d["CallNumberPrefix"]);
            h2.CallNumber = Global.Trim((string)d["CallNumber"]);
            h2.CallNumberSuffix = Global.Trim((string)d["CallNumberSuffix"]);
            h2.ShelvingTitle = Global.Trim((string)d["ShelvingTitle"]);
            h2.AcquisitionFormat = Global.Trim((string)d["AcquisitionFormat"]);
            h2.AcquisitionMethod = Global.Trim((string)d["AcquisitionMethod"]);
            h2.ReceiptStatus = Global.Trim((string)d["ReceiptStatus"]);
            h2.IllPolicyId = (string)d["IllPolicyId"] != "" ? (Guid?)Guid.Parse((string)d["IllPolicyId"]) : null;
            h2.RetentionPolicy = Global.Trim((string)d["RetentionPolicy"]);
            h2.DigitizationPolicy = Global.Trim((string)d["DigitizationPolicy"]);
            h2.CopyNumber = Global.Trim((string)d["CopyNumber"]);
            h2.ItemCount = Global.Trim((string)d["ItemCount"]);
            h2.ReceivingHistoryDisplayType = Global.Trim((string)d["ReceivingHistoryDisplayType"]);
            h2.DiscoverySuppress = (bool?)d["DiscoverySuppress"];
            h2.SourceId = (string)d["SourceId"] != "" ? (Guid?)Guid.Parse((string)d["SourceId"]) : null;
            h2.LastWriteTime = DateTime.Now;
            h2.LastWriteUserId = (Guid?)Session["UserId"];
            if (id == null) folioServiceContext.Insert(h2); else folioServiceContext.Update(h2);
        }

        protected void Holding2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
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
                var cv = (CustomValidator)gei.FindControl("DeleteCustomValidator");
                cv.IsValid = false;
                e.Canceled = true;
            }
        }

        protected void Holding2sHoldingTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.HoldingType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Holding2sTemporaryLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Holding2sEffectiveLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Holding2sCallNumberTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.CallNumberType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sIllPolicyRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.IllPolicy2s(orderBy: "name").ToArray();
        }

        protected void Holding2sSourceRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Source2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                e.Item.OwnerTableView.InsertItem(new Item2());
                e.Canceled = true;
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

        protected void Holding2sItem2sRadGrid_UpdateCommand(object sender, GridCommandEventArgs e)
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

        protected void Holding2sItem2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
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

        protected void Holding2sItem2sCallNumberTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.CallNumberType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sDamagedStatusRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ItemDamagedStatus2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sStatusNameRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new string[] { "Aged to lost", "Available", "Awaiting pickup", "Awaiting delivery", "Checked out", "Claimed returned", "Declared lost", "In process", "In process (non-requestable)", "In transit", "Intellectual item", "Long missing", "Lost and paid", "Missing", "On order", "Paged", "Restricted", "Order closed", "Unavailable", "Unknown", "Withdrawn" };
        }

        protected void Holding2sItem2sMaterialTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.MaterialType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sPermanentLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sTemporaryLoanTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.LoanType2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sPermanentLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sTemporaryLocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sInTransitDestinationServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
        }

        protected void Holding2sItem2sOrderItemRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            if (rcb.SelectedValue != "")
            {
                var id = Guid.Parse(rcb.SelectedValue);
                var oi2 = folioServiceContext.FindOrderItem2(id);
                rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
            }
        }

        protected void Holding2sItem2sOrderItemRadComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            foreach (var oi2 in folioServiceContext.OrderItem2s($"poLineNumber == \"{FolioServiceClient.EncodeCql(e.Text)}*\"", take: 100).OrderBy(oi2 => oi2.Number)) rcb.Items.Add(new RadComboBoxItem(oi2.Number, oi2.Id.ToString()));
        }

        protected void Holding2sItem2sOrderItemCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args) => args.IsValid = folioServiceContext.AnyOrderItem2s($"poLineNumber = \"{FolioServiceClient.EncodeCql(args.Value)}\"");

        protected void Holding2sItem2sLastCheckInServicePointRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.ServicePoint2s(orderBy: "name").ToArray();
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
