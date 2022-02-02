using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

namespace FolioWebApplication.Holding2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            var h2 = folioServiceContext.FindHolding2(id, true);
            if (h2 == null) Response.Redirect("Default.aspx");
            h2.Content = h2.Content != null ? JsonConvert.DeserializeObject<JToken>(h2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Holding2FormView.DataSource = new[] { h2 };
            Title = $"Holding {h2.ShortId}";
        }

        protected void BoundWithPart2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BoundWithPart2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"holdingsRecordId == \"{id}\"",
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BoundWithPart2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
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
                BoundWithPart2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["BoundWithPart2sPermission"] != null && BoundWithPart2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExtentsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ExtentsPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).Extents ?? new Extent[] { };
            ExtentsRadGrid.DataSource = l;
            ExtentsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ExtentsPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["ExtentsPermission"] == "Edit" || Session["ExtentsPermission"] != null && l.Any());
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"holdingsRecordId == \"{id}\"",
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
                Global.GetCqlFilter(Fee2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType1.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "FeeType.Name", "feeFineId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(Fee2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances)
            }.Where(s => s != null)));
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, where, Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void HoldingElectronicAccessesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingElectronicAccessesPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingElectronicAccesses ?? new HoldingElectronicAccess[] { };
            HoldingElectronicAccessesRadGrid.DataSource = l;
            HoldingElectronicAccessesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingElectronicAccessesPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingElectronicAccessesPermission"] == "Edit" || Session["HoldingElectronicAccessesPermission"] != null && l.Any());
        }

        protected void HoldingEntriesRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                var gei = (GridEditableItem)e.Item;
                var id = (string)gei.GetDataKeyValue("Id");
                var holdingId = (Guid)gei.GetDataKeyValue("HoldingId");
                var he = folioServiceContext.FindHolding2(holdingId, true).HoldingEntries.SingleOrDefault(he2 => he2.Id == id);
                if (he == null)
                    HoldingEntriesRadGrid.Rebind();
                else
                {
                    var author = he.Holding.Instance.Contributors.FirstOrDefault(c => c.Primary.Value)?.Name;
                    var locationName = he.Holding.HoldingNotes.Where(hn => hn.HoldingNoteType.Name == "Unbound location").FirstOrDefault()?.Note;
                    var m = Regex.Match(locationName ?? "", @"^.*? - (.+?) - .*$", RegexOptions.Compiled);
                    var locationCode = m.Success ? m.Groups[1].Value : locationName;
                    var label = new Label
                    {
                        Font = new Font { Family = "Arial Narrow", Size = 11, Weight = FontWeight.Normal },
                        Orientation = Orientation.Landscape,
                        IsSerial = true,
                        Text = $"{locationCode} {he.Holding.CallNumber} {he.Holding.CopyNumber}\r\n{(author != null ? Global.Truncate(author, 44) + "\r\n" : null)}{(he.Holding.Instance.Title != null ? Global.Truncate(he.Holding.Instance.Title, 44) + "\r\n" : null)}{he.Enumeration} {he.Chronology}\r\nBib: {he.Holding.Instance.ShortId} Hold: {he.Holding.ShortId} Rec'd: {he.Holding.LastWriteTime:d}"
                    };
                    Global.Print(label, this, folioServiceContext);
                }
            }
        }

        protected void HoldingEntriesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingEntriesPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingEntries ?? new HoldingEntry[] { };
            HoldingEntriesRadGrid.DataSource = l;
            HoldingEntriesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingEntriesPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingEntriesPermission"] == "Edit" || Session["HoldingEntriesPermission"] != null && l.Any());
        }

        protected void HoldingFormerIdsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingFormerIdsPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingFormerIds ?? new HoldingFormerId[] { };
            HoldingFormerIdsRadGrid.DataSource = l;
            HoldingFormerIdsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingFormerIdsPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingFormerIdsPermission"] == "Edit" || Session["HoldingFormerIdsPermission"] != null && l.Any());
        }

        protected void HoldingNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingNotesPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingNotes ?? new HoldingNote[] { };
            HoldingNotesRadGrid.DataSource = l;
            HoldingNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingNotesPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingNotesPermission"] == "Edit" || Session["HoldingNotesPermission"] != null && l.Any());
        }

        protected void HoldingStatisticalCodesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingStatisticalCodesPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingStatisticalCodes ?? new HoldingStatisticalCode[] { };
            HoldingStatisticalCodesRadGrid.DataSource = l;
            HoldingStatisticalCodesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingStatisticalCodesPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingStatisticalCodesPermission"] == "Edit" || Session["HoldingStatisticalCodesPermission"] != null && l.Any());
        }

        protected void HoldingTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["HoldingTagsPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).HoldingTags ?? new HoldingTag[] { };
            HoldingTagsRadGrid.DataSource = l;
            HoldingTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            HoldingTagsPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["HoldingTagsPermission"] == "Edit" || Session["HoldingTagsPermission"] != null && l.Any());
        }

        protected void IndexStatementsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["IndexStatementsPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).IndexStatements ?? new IndexStatement[] { };
            IndexStatementsRadGrid.DataSource = l;
            IndexStatementsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            IndexStatementsPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["IndexStatementsPermission"] == "Edit" || Session["IndexStatementsPermission"] != null && l.Any());
        }

        protected void Item2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "DiscoverySuppress", "discoverySuppress" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "StatusName", "status.name" }, { "StatusDate", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"holdingsRecordId == \"{id}\"",
                Global.GetCqlFilter(Item2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Item2sRadGrid, "Version", "_version"),
                Global.GetCqlFilter(Item2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Item2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
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
                Global.GetCqlFilter(Item2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Item2sRadGrid, "StatusDate", "status.date"),
                Global.GetCqlFilter(Item2sRadGrid, "MaterialType.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Item2sRadGrid, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
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
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, where, Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "DisplayOnHolding", "displayOnHolding" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "DiscoverySuppress", "discoverySuppress" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"holdingId == \"{id}\"",
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Caption", "caption"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Receiving2sRadGrid, "Location.Name", "locationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Receiving2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Receiving2sRadGrid, "Title.Title", "titleId", "title", folioServiceContext.FolioServiceClient.Titles),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayOnHolding", "displayOnHolding"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingStatus", "receivingStatus"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Supplement", "supplement"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiptTime", "receiptDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiveTime", "receivedDate")
            }.Where(s => s != null)));
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, where, Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void SupplementStatementsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["SupplementStatementsPermission"] == null) return;
            var id = (Guid?)Holding2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindHolding2(id, true).SupplementStatements ?? new SupplementStatement[] { };
            SupplementStatementsRadGrid.DataSource = l;
            SupplementStatementsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            SupplementStatementsPanel.Visible = Holding2FormView.DataKey.Value != null && ((string)Session["SupplementStatementsPermission"] == "Edit" || Session["SupplementStatementsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
