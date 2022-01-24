using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            BoundWithPart2sRadGrid.DataSource = folioServiceContext.BoundWithPart2s(out var i, Global.GetCqlFilter(BoundWithPart2sRadGrid, d, $"holdingsRecordId == \"{id}\""), BoundWithPart2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BoundWithPart2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BoundWithPart2sRadGrid.PageSize * BoundWithPart2sRadGrid.CurrentPageIndex, BoundWithPart2sRadGrid.PageSize, true);
            BoundWithPart2sRadGrid.VirtualItemCount = i;
            if (BoundWithPart2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BoundWithPart2sRadGrid.AllowFilteringByColumn = BoundWithPart2sRadGrid.VirtualItemCount > 10;
                BoundWithPart2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["BoundWithPart2sPermission"] != null && BoundWithPart2sRadGrid.VirtualItemCount > 0;
            }
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
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d, $"holdingsRecordId == \"{id}\""), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
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
                    var label = new Label
                    {
                        Font = new Font { Family = "Arial Narrow", Size = 11, Weight = FontWeight.Normal },
                        Orientation = Orientation.Landscape,
                        IsSerial = true,
                        Text = $"{he.Holding.Location.Code} {he.Holding.CallNumber} {he.Holding.CopyNumber}\r\n{(he.Holding.Instance.Author != null ? Global.Truncate(he.Holding.Instance.Author, 44) + "\r\n" : null)}{(he.Holding.Instance.Title != null ? Global.Truncate(he.Holding.Instance.Title, 44) + "\r\n" : null)}{he.Enumeration} {he.Chronology}\r\nBib: {he.Holding.Instance.ShortId} Hold: {he.Holding.ShortId} Rec'd: {he.Holding.LastWriteTime:d}"
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
            Item2sRadGrid.DataSource = folioServiceContext.Item2s(out var i, Global.GetCqlFilter(Item2sRadGrid, d, $"holdingsRecordId == \"{id}\""), Item2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Item2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Item2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Item2sRadGrid.PageSize * Item2sRadGrid.CurrentPageIndex, Item2sRadGrid.PageSize, true);
            Item2sRadGrid.VirtualItemCount = i;
            if (Item2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Item2sRadGrid.AllowFilteringByColumn = Item2sRadGrid.VirtualItemCount > 10;
                Item2sPanel.Visible = Holding2FormView.DataKey.Value != null && Session["Item2sPermission"] != null && Item2sRadGrid.VirtualItemCount > 0;
            }
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
