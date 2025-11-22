using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Instance2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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
            var i2 = folioServiceContext.FindInstance2(id, true);
            if (i2 == null) Response.Redirect("Default.aspx");
            i2.Content = i2.Content != null ? JsonConvert.DeserializeObject<JToken>(i2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Instance2FormView.DataSource = new[] { i2 };
            Title = $"Instance {i2.Title}";
        }

        protected void AdministrativeNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AdministrativeNotesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).AdministrativeNotes ?? new AdministrativeNote[] { };
            AdministrativeNotesRadGrid.DataSource = l;
            AdministrativeNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AdministrativeNotesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["AdministrativeNotesPermission"] == "Edit" || Session["AdministrativeNotesPermission"] != null && l.Any());
        }

        protected void AlternativeTitlesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["AlternativeTitlesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).AlternativeTitles ?? new AlternativeTitle[] { };
            AlternativeTitlesRadGrid.DataSource = l;
            AlternativeTitlesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            AlternativeTitlesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["AlternativeTitlesPermission"] == "Edit" || Session["AlternativeTitlesPermission"] != null && l.Any());
        }

        protected void ClassificationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ClassificationsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Classifications ?? new Classification[] { };
            ClassificationsRadGrid.DataSource = l;
            ClassificationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ClassificationsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ClassificationsPermission"] == "Edit" || Session["ClassificationsPermission"] != null && l.Any());
        }

        protected void ContributorsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContributorsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Contributors ?? new Contributor[] { };
            ContributorsRadGrid.DataSource = l;
            ContributorsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContributorsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ContributorsPermission"] == "Edit" || Session["ContributorsPermission"] != null && l.Any());
        }

        protected void EditionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["EditionsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Editions ?? new Edition[] { };
            EditionsRadGrid.DataSource = l;
            EditionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            EditionsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["EditionsPermission"] == "Edit" || Session["EditionsPermission"] != null && l.Any());
        }

        protected void ElectronicAccessesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ElectronicAccessesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).ElectronicAccesses ?? new ElectronicAccess[] { };
            ElectronicAccessesRadGrid.DataSource = l;
            ElectronicAccessesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ElectronicAccessesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["ElectronicAccessesPermission"] == "Edit" || Session["ElectronicAccessesPermission"] != null && l.Any());
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceId == \"{id}\"",
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
                Global.GetCqlFilter(Fee2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings)
            }.Where(s => s != null)));
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(where, Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = folioServiceContext.CountFee2s(where);
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SourceId", "sourceId" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceId == \"{id}\"",
                Global.GetCqlFilter(Holding2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Holding2sRadGrid, "Source.Name", "sourceId", "name", folioServiceContext.FolioServiceClient.Sources),
                Global.GetCqlFilter(Holding2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Holding2sRadGrid, "HoldingType.Name", "holdingsTypeId", "name", folioServiceContext.FolioServiceClient.HoldingTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "Location.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberType.Name", "callNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberPrefix", "callNumberPrefix"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberSuffix", "callNumberSuffix"),
                Global.GetCqlFilter(Holding2sRadGrid, "ShelvingTitle", "shelvingTitle"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionFormat", "acquisitionFormat"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(Holding2sRadGrid, "IllPolicy.Name", "illPolicyId", "name", folioServiceContext.FolioServiceClient.IllPolicies),
                Global.GetCqlFilter(Holding2sRadGrid, "RetentionPolicy", "retentionPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "DigitizationPolicy", "digitizationPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "ItemCount", "numberOfItems"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceivingHistoryDisplayType", "receivingHistory.displayType"),
                Global.GetCqlFilter(Holding2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(where, Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = folioServiceContext.CountHolding2s(where);
            if (Holding2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2sRadGrid.AllowFilteringByColumn = Holding2sRadGrid.VirtualItemCount > 10;
                Holding2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Holding2sItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Item2sPermission"] == null) return;
            var rg = (RadGrid)sender;
            var id = (Guid?)((GridDataItem)rg.Parent.Parent).GetDataKeyValue("Id");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingId", "holdingsRecordId" }, { "Order", "order" }, { "DiscoverySuppress", "discoverySuppress" }, { "DisplaySummary", "displaySummary" }, { "AccessionNumber", "accessionNumber" }, { "Barcode", "barcode" }, { "EffectiveShelvingOrder", "effectiveShelvingOrder" }, { "CallNumber", "itemLevelCallNumber" }, { "CallNumberPrefix", "itemLevelCallNumberPrefix" }, { "CallNumberSuffix", "itemLevelCallNumberSuffix" }, { "CallNumberTypeId", "itemLevelCallNumberTypeId" }, { "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber" }, { "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix" }, { "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix" }, { "EffectiveCallNumberTypeId", "effectiveCallNumberComponents.typeId" }, { "Volume", "volume" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "ItemIdentifier", "itemIdentifier" }, { "CopyNumber", "copyNumber" }, { "PiecesCount", "numberOfPieces" }, { "PiecesDescription", "descriptionOfPieces" }, { "MissingPiecesCount", "numberOfMissingPieces" }, { "MissingPiecesDescription", "missingPieces" }, { "MissingPiecesTime", "missingPiecesDate" }, { "DamagedStatusId", "itemDamagedStatusId" }, { "DamagedStatusTime", "itemDamagedStatusDate" }, { "Status", "status.name" }, { "StatusLastWriteTime", "status.date" }, { "MaterialTypeId", "materialTypeId" }, { "PermanentLoanTypeId", "permanentLoanTypeId" }, { "TemporaryLoanTypeId", "temporaryLoanTypeId" }, { "PermanentLocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "InTransitDestinationServicePointId", "inTransitDestinationServicePointId" }, { "OrderItemId", "purchaseOrderLineIdentifier" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastCheckInDateTime", "lastCheckIn.dateTime" }, { "LastCheckInServicePointId", "lastCheckIn.servicePointId" }, { "LastCheckInStaffMemberId", "lastCheckIn.staffMemberId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"holdingsRecordId == \"{id}\"",
                Global.GetCqlFilter(rg, "Id", "id"),
                Global.GetCqlFilter(rg, "ShortId", "hrid"),
                Global.GetCqlFilter(rg, "Order", "order"),
                Global.GetCqlFilter(rg, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(rg, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(rg, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(rg, "Barcode", "barcode"),
                Global.GetCqlFilter(rg, "EffectiveShelvingOrder", "effectiveShelvingOrder"),
                Global.GetCqlFilter(rg, "CallNumber", "itemLevelCallNumber"),
                Global.GetCqlFilter(rg, "CallNumberPrefix", "itemLevelCallNumberPrefix"),
                Global.GetCqlFilter(rg, "CallNumberSuffix", "itemLevelCallNumberSuffix"),
                Global.GetCqlFilter(rg, "CallNumberType.Name", "itemLevelCallNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(rg, "EffectiveCallNumber", "effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(rg, "EffectiveCallNumberPrefix", "effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(rg, "EffectiveCallNumberSuffix", "effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(rg, "EffectiveCallNumberType.Name", "effectiveCallNumberComponents.typeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(rg, "Volume", "volume"),
                Global.GetCqlFilter(rg, "Enumeration", "enumeration"),
                Global.GetCqlFilter(rg, "Chronology", "chronology"),
                Global.GetCqlFilter(rg, "ItemIdentifier", "itemIdentifier"),
                Global.GetCqlFilter(rg, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(rg, "PiecesCount", "numberOfPieces"),
                Global.GetCqlFilter(rg, "PiecesDescription", "descriptionOfPieces"),
                Global.GetCqlFilter(rg, "MissingPiecesCount", "numberOfMissingPieces"),
                Global.GetCqlFilter(rg, "MissingPiecesDescription", "missingPieces"),
                Global.GetCqlFilter(rg, "MissingPiecesTime", "missingPiecesDate"),
                Global.GetCqlFilter(rg, "DamagedStatus.Name", "itemDamagedStatusId", "name", folioServiceContext.FolioServiceClient.ItemDamagedStatuses),
                Global.GetCqlFilter(rg, "DamagedStatusTime", "itemDamagedStatusDate"),
                Global.GetCqlFilter(rg, "Status", "status.name"),
                Global.GetCqlFilter(rg, "StatusLastWriteTime", "status.date"),
                Global.GetCqlFilter(rg, "MaterialType.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(rg, "PermanentLoanType.Name", "permanentLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(rg, "TemporaryLoanType.Name", "temporaryLoanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(rg, "PermanentLocation.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(rg, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(rg, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(rg, "InTransitDestinationServicePoint.Name", "inTransitDestinationServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(rg, "OrderItem.Number", "purchaseOrderLineIdentifier", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(rg, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(rg, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(rg, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(rg, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(rg, "LastCheckInDateTime", "lastCheckIn.dateTime"),
                Global.GetCqlFilter(rg, "LastCheckInServicePoint.Name", "lastCheckIn.servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(rg, "LastCheckInStaffMember.Username", "lastCheckIn.staffMemberId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            rg.DataSource = folioServiceContext.Item2s(where, rg.MasterTableView.SortExpressions.Count > 0 ? $"{d[rg.MasterTableView.SortExpressions[0].FieldName]}{(rg.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, rg.PageSize * rg.CurrentPageIndex, rg.PageSize, true);
            rg.VirtualItemCount = folioServiceContext.CountItem2s(where);
            if (rg.MasterTableView.FilterExpression == "")
            {
                rg.AllowFilteringByColumn = rg.VirtualItemCount > 10;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void IdentifiersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["IdentifiersPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Identifiers ?? new Identifier[] { };
            IdentifiersRadGrid.DataSource = l;
            IdentifiersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            IdentifiersPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["IdentifiersPermission"] == "Edit" || Session["IdentifiersPermission"] != null && l.Any());
        }

        protected void InstanceFormat2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceFormat2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).InstanceFormat2s ?? new InstanceFormat2[] { };
            InstanceFormat2sRadGrid.DataSource = l;
            InstanceFormat2sRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceFormat2sPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceFormat2sPermission"] == "Edit" || Session["InstanceFormat2sPermission"] != null && l.Any());
        }

        protected void InstanceNatureOfContentTermsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceNatureOfContentTermsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).InstanceNatureOfContentTerms ?? new InstanceNatureOfContentTerm[] { };
            InstanceNatureOfContentTermsRadGrid.DataSource = l;
            InstanceNatureOfContentTermsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceNatureOfContentTermsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceNatureOfContentTermsPermission"] == "Edit" || Session["InstanceNatureOfContentTermsPermission"] != null && l.Any());
        }

        protected void InstanceNotesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceNotesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).InstanceNotes ?? new InstanceNote[] { };
            InstanceNotesRadGrid.DataSource = l;
            InstanceNotesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceNotesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceNotesPermission"] == "Edit" || Session["InstanceNotesPermission"] != null && l.Any());
        }

        protected void InstanceStatisticalCodesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceStatisticalCodesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).InstanceStatisticalCodes ?? new InstanceStatisticalCode[] { };
            InstanceStatisticalCodesRadGrid.DataSource = l;
            InstanceStatisticalCodesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceStatisticalCodesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceStatisticalCodesPermission"] == "Edit" || Session["InstanceStatisticalCodesPermission"] != null && l.Any());
        }

        protected void InstanceTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["InstanceTagsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).InstanceTags ?? new InstanceTag[] { };
            InstanceTagsRadGrid.DataSource = l;
            InstanceTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            InstanceTagsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["InstanceTagsPermission"] == "Edit" || Session["InstanceTagsPermission"] != null && l.Any());
        }

        protected void LanguagesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LanguagesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Languages ?? new Language[] { };
            LanguagesRadGrid.DataSource = l;
            LanguagesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LanguagesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["LanguagesPermission"] == "Edit" || Session["LanguagesPermission"] != null && l.Any());
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "DetailsIsAcknowledged", "details.isAcknowledged" }, { "DetailsIsBinderyActive", "details.isBinderyActive" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceId == \"{id}\"",
                Global.GetCqlFilter(OrderItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Agreement.Name", "agreementId", "name", folioServiceContext.FolioServiceClient.Agreements),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AutomaticExport", "automaticExport"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ClaimingActive", "claimingActive"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Collection", "collection"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalUnitListPrice", "cost.listUnitPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicUnitListPrice", "cost.listUnitPriceElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Currency", "cost.currency"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AdditionalCost", "cost.additionalCost"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Discount", "cost.discount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DiscountType", "cost.discountType"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ExchangeRate", "cost.exchangeRate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalQuantity", "cost.quantityPhysical"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicQuantity", "cost.quantityElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EstimatedPrice", "cost.poLineEstimatedPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "InternalNote", "description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceivingNote", "details.receivingNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DetailsIsAcknowledged", "details.isAcknowledged"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DetailsIsBinderyActive", "details.isBinderyActive"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionFrom", "details.subscriptionFrom"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionInterval", "details.subscriptionInterval"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionTo", "details.subscriptionTo"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Donor", "donor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivated", "eresource.activated"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivationDue", "eresource.activationDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceCreateInventory", "eresource.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceTrial", "eresource.trial"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceExpectedActivationDate", "eresource.expectedActivation"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceUserLimit", "eresource.userLimit"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceAccessProvider.Name", "eresource.accessProvider", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseCode", "eresource.license.code"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseDescription", "eresource.license.description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseReference", "eresource.license.reference"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceMaterialType.Name", "eresource.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceResourceUrl", "eresource.resourceUrl"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "IsPackage", "isPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastEdiExportDate", "lastEDIExportDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "OrderFormat", "orderFormat"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PackageOrderItem.Number", "packagePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PaymentStatus", "paymentStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalCreateInventory", "physical.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialType.Name", "physical.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialSupplier.Name", "physical.materialSupplier", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalReceiptDue", "physical.receiptDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Description", "poLineDescription"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Number", "poLineNumber"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PublicationYear", "publicationDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Order.Number", "purchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptDate", "receiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "RenewalNote", "renewalNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Requester", "requester"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Rush", "rush"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Selector", "selector"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "TitleOrPackage", "titleOrPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorInstructions", "vendorDetail.instructions"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorNote", "vendorDetail.noteFromVendor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorCustomerId", "vendorDetail.vendorAccount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SuppressInstanceFromDiscovery", "suppressInstanceFromDiscovery"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(where, OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = folioServiceContext.CountOrderItem2s(where);
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PhysicalDescriptionsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PhysicalDescriptionsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).PhysicalDescriptions ?? new PhysicalDescription[] { };
            PhysicalDescriptionsRadGrid.DataSource = l;
            PhysicalDescriptionsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PhysicalDescriptionsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PhysicalDescriptionsPermission"] == "Edit" || Session["PhysicalDescriptionsPermission"] != null && l.Any());
        }

        protected void PrecedingSucceedingTitle2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "ShortId", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"precedingInstanceId == \"{id}\"",
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "SucceedingInstance.Title", "succeedingInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PrecedingSucceedingTitle2sRadGrid.DataSource = folioServiceContext.PrecedingSucceedingTitle2s(where, PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PrecedingSucceedingTitle2sRadGrid.PageSize * PrecedingSucceedingTitle2sRadGrid.CurrentPageIndex, PrecedingSucceedingTitle2sRadGrid.PageSize, true);
            PrecedingSucceedingTitle2sRadGrid.VirtualItemCount = folioServiceContext.CountPrecedingSucceedingTitle2s(where);
            if (PrecedingSucceedingTitle2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PrecedingSucceedingTitle2sRadGrid.AllowFilteringByColumn = PrecedingSucceedingTitle2sRadGrid.VirtualItemCount > 10;
                PrecedingSucceedingTitle2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["PrecedingSucceedingTitle2sPermission"] != null && PrecedingSucceedingTitle2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PrecedingSucceedingTitle2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "ShortId", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"succeedingInstanceId == \"{id}\"",
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "PrecedingInstance.Title", "precedingInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "Title", "title"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PrecedingSucceedingTitle2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PrecedingSucceedingTitle2s1RadGrid.DataSource = folioServiceContext.PrecedingSucceedingTitle2s(where, PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PrecedingSucceedingTitle2s1RadGrid.PageSize * PrecedingSucceedingTitle2s1RadGrid.CurrentPageIndex, PrecedingSucceedingTitle2s1RadGrid.PageSize, true);
            PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount = folioServiceContext.CountPrecedingSucceedingTitle2s(where);
            if (PrecedingSucceedingTitle2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                PrecedingSucceedingTitle2s1RadGrid.AllowFilteringByColumn = PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount > 10;
                PrecedingSucceedingTitle2s1Panel.Visible = Instance2FormView.DataKey.Value != null && Session["PrecedingSucceedingTitle2sPermission"] != null && PrecedingSucceedingTitle2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PublicationFrequenciesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationFrequenciesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).PublicationFrequencies ?? new PublicationFrequency[] { };
            PublicationFrequenciesRadGrid.DataSource = l;
            PublicationFrequenciesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationFrequenciesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationFrequenciesPermission"] == "Edit" || Session["PublicationFrequenciesPermission"] != null && l.Any());
        }

        protected void PublicationRangesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationRangesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).PublicationRanges ?? new PublicationRange[] { };
            PublicationRangesRadGrid.DataSource = l;
            PublicationRangesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationRangesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationRangesPermission"] == "Edit" || Session["PublicationRangesPermission"] != null && l.Any());
        }

        protected void PublicationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PublicationsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Publications ?? new Publication[] { };
            PublicationsRadGrid.DataSource = l;
            PublicationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PublicationsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["PublicationsPermission"] == "Edit" || Session["PublicationsPermission"] != null && l.Any());
        }

        protected void RelationshipsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"subInstanceId == \"{id}\"",
                Global.GetCqlFilter(RelationshipsRadGrid, "Id", "id"),
                Global.GetCqlFilter(RelationshipsRadGrid, "SuperInstance.Title", "superInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(RelationshipsRadGrid, "InstanceRelationshipType.Name", "instanceRelationshipTypeId", "name", folioServiceContext.FolioServiceClient.InstanceRelationshipTypes),
                Global.GetCqlFilter(RelationshipsRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RelationshipsRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RelationshipsRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RelationshipsRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RelationshipsRadGrid.DataSource = folioServiceContext.Relationships(where, RelationshipsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RelationshipsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RelationshipsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RelationshipsRadGrid.PageSize * RelationshipsRadGrid.CurrentPageIndex, RelationshipsRadGrid.PageSize, true);
            RelationshipsRadGrid.VirtualItemCount = folioServiceContext.CountRelationships(where);
            if (RelationshipsRadGrid.MasterTableView.FilterExpression == "")
            {
                RelationshipsRadGrid.AllowFilteringByColumn = RelationshipsRadGrid.VirtualItemCount > 10;
                RelationshipsPanel.Visible = Instance2FormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && RelationshipsRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Relationships1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RelationshipsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuperInstanceId", "superInstanceId" }, { "SubInstanceId", "subInstanceId" }, { "InstanceRelationshipTypeId", "instanceRelationshipTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"superInstanceId == \"{id}\"",
                Global.GetCqlFilter(Relationships1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Relationships1RadGrid, "SubInstance.Title", "subInstanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Relationships1RadGrid, "InstanceRelationshipType.Name", "instanceRelationshipTypeId", "name", folioServiceContext.FolioServiceClient.InstanceRelationshipTypes),
                Global.GetCqlFilter(Relationships1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Relationships1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Relationships1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Relationships1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Relationships1RadGrid.DataSource = folioServiceContext.Relationships(where, Relationships1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Relationships1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Relationships1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Relationships1RadGrid.PageSize * Relationships1RadGrid.CurrentPageIndex, Relationships1RadGrid.PageSize, true);
            Relationships1RadGrid.VirtualItemCount = folioServiceContext.CountRelationships(where);
            if (Relationships1RadGrid.MasterTableView.FilterExpression == "")
            {
                Relationships1RadGrid.AllowFilteringByColumn = Relationships1RadGrid.VirtualItemCount > 10;
                Relationships1Panel.Visible = Instance2FormView.DataKey.Value != null && Session["RelationshipsPermission"] != null && Relationships1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceId == \"{id}\"",
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
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
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(where, Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = folioServiceContext.CountRequest2s(where);
            if (Request2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Request2sRadGrid.AllowFilteringByColumn = Request2sRadGrid.VirtualItemCount > 10;
                Request2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void SeriesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["SeriesPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Series ?? new Series[] { };
            SeriesRadGrid.DataSource = l;
            SeriesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            SeriesPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["SeriesPermission"] == "Edit" || Session["SeriesPermission"] != null && l.Any());
        }

        protected void SubjectsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["SubjectsPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindInstance2(id, true).Subjects ?? new Subject[] { };
            SubjectsRadGrid.DataSource = l;
            SubjectsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            SubjectsPanel.Visible = Instance2FormView.DataKey.Value != null && ((string)Session["SubjectsPermission"] == "Edit" || Session["SubjectsPermission"] != null && l.Any());
        }

        protected void Title2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Title2sPermission"] == null) return;
            var id = (Guid?)Instance2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"instanceId == \"{id}\"",
                Global.GetCqlFilter(Title2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Title2sRadGrid, "ExpectedReceiptDate", "expectedReceiptDate"),
                Global.GetCqlFilter(Title2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Title2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Title2sRadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(Title2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(Title2sRadGrid, "PackageName", "packageName"),
                Global.GetCqlFilter(Title2sRadGrid, "OrderItemNumber", "poLineNumber"),
                Global.GetCqlFilter(Title2sRadGrid, "PublishedDate", "publishedDate"),
                Global.GetCqlFilter(Title2sRadGrid, "ReceivingNote", "receivingNote"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionFrom", "subscriptionFrom"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionTo", "subscriptionTo"),
                Global.GetCqlFilter(Title2sRadGrid, "SubscriptionInterval", "subscriptionInterval"),
                Global.GetCqlFilter(Title2sRadGrid, "ClaimingActive", "claimingActive"),
                Global.GetCqlFilter(Title2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(Title2sRadGrid, "IsAcknowledged", "isAcknowledged"),
                Global.GetCqlFilter(Title2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Title2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Title2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Title2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(where, Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = folioServiceContext.CountTitle2s(where);
            if (Title2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Title2sRadGrid.AllowFilteringByColumn = Title2sRadGrid.VirtualItemCount > 10;
                Title2sPanel.Visible = Instance2FormView.DataKey.Value != null && Session["Title2sPermission"] != null && Title2sRadGrid.VirtualItemCount > 0;
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
