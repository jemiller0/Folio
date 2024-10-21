using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Receiving2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Receiving2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DisplaySummary", "displaySummary" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "BindItemId", "bindItemId" }, { "BindItemTenantId", "bindItemTenantId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "ReceivingTenantId", "receivingTenantId" }, { "DisplayOnHolding", "displayOnHolding" }, { "DisplayToPublic", "displayToPublic" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "Barcode", "barcode" }, { "AccessionNumber", "accessionNumber" }, { "CallNumber", "callNumber" }, { "DiscoverySuppress", "discoverySuppress" }, { "CopyNumber", "copyNumber" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "IsBound", "isBound" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" }, { "StatusUpdatedDate", "statusUpdatedDate" }, { "ClaimingInterval", "claimingInterval" }, { "InternalNote", "internalNote" }, { "ExternalNote", "externalNote" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Receiving2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tDisplaySummary\tComment\tFormat\tItem\tItemId\tBindItemId\tBindItemTenantId\tLocation\tLocationId\tOrderItem\tOrderItemId\tTitle\tTitleId\tHolding\tHoldingId\tReceivingTenantId\tDisplayOnHolding\tDisplayToPublic\tEnumeration\tChronology\tBarcode\tAccessionNumber\tCallNumber\tDiscoverySuppress\tCopyNumber\tReceivingStatus\tSupplement\tIsBound\tReceiptTime\tReceiveTime\tStatusUpdatedDate\tClaimingInterval\tInternalNote\tExternalNote\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DisplaySummary", "displaySummary" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "BindItemId", "bindItemId" }, { "BindItemTenantId", "bindItemTenantId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "ReceivingTenantId", "receivingTenantId" }, { "DisplayOnHolding", "displayOnHolding" }, { "DisplayToPublic", "displayToPublic" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "Barcode", "barcode" }, { "AccessionNumber", "accessionNumber" }, { "CallNumber", "callNumber" }, { "DiscoverySuppress", "discoverySuppress" }, { "CopyNumber", "copyNumber" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "IsBound", "isBound" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" }, { "StatusUpdatedDate", "statusUpdatedDate" }, { "ClaimingInterval", "claimingInterval" }, { "InternalNote", "internalNote" }, { "ExternalNote", "externalNote" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
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
            foreach (var r2 in folioServiceContext.Receiving2s(where, Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{Global.TextEncode(r2.DisplaySummary)}\t{Global.TextEncode(r2.Comment)}\t{Global.TextEncode(r2.Format)}\t{r2.Item?.ShortId}\t{r2.ItemId}\t{r2.BindItemId}\t{r2.BindItemTenantId}\t{Global.TextEncode(r2.Location?.Name)}\t{r2.LocationId}\t{Global.TextEncode(r2.OrderItem?.Number)}\t{r2.OrderItemId}\t{Global.TextEncode(r2.Title?.Title)}\t{r2.TitleId}\t{r2.Holding?.ShortId}\t{r2.HoldingId}\t{r2.ReceivingTenantId}\t{r2.DisplayOnHolding}\t{r2.DisplayToPublic}\t{Global.TextEncode(r2.Enumeration)}\t{Global.TextEncode(r2.Chronology)}\t{Global.TextEncode(r2.Barcode)}\t{Global.TextEncode(r2.AccessionNumber)}\t{Global.TextEncode(r2.CallNumber)}\t{r2.DiscoverySuppress}\t{Global.TextEncode(r2.CopyNumber)}\t{Global.TextEncode(r2.ReceivingStatus)}\t{r2.Supplement}\t{r2.IsBound}\t{r2.ReceiptTime:M/d/yyyy HH:mm:ss}\t{r2.ReceiveTime:M/d/yyyy HH:mm:ss}\t{r2.StatusUpdatedDate:M/d/yyyy}\t{r2.ClaimingInterval}\t{Global.TextEncode(r2.InternalNote)}\t{Global.TextEncode(r2.ExternalNote)}\t{r2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.CreationUser?.Username)}\t{r2.CreationUserId}\t{r2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.LastWriteUser?.Username)}\t{r2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
