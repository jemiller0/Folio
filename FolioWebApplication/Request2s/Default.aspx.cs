using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Request2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Request2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
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
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(out var i, where, Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Request2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tRequestLevel\tRequestType\tEcsRequestPhase\tRequestDate\tPatronComments\tRequester\tRequesterId\tProxyUser\tProxyUserId\tInstance\tInstanceId\tHolding\tHoldingId\tItem\tItemId\tStatus\tCancellationReason\tCancellationReasonId\tCancelledByUser\tCancelledByUserId\tCancellationAdditionalInformation\tCancelledDate\tPosition\tInstanceTitle\tItemBarcode\tRequesterFirstName\tRequesterLastName\tRequesterMiddleName\tRequesterBarcode\tRequesterPatronGroup\tProxyFirstName\tProxyLastName\tProxyMiddleName\tProxyBarcode\tProxyPatronGroup\tFulfillmentPreference\tDeliveryAddressType\tDeliveryAddressTypeId\tRequestExpirationDate\tHoldShelfExpirationDate\tPickupServicePoint\tPickupServicePointId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tPrintDetailsPrintCount\tPrintDetailsRequester\tPrintDetailsRequesterId\tPrintDetailsIsPrinted\tPrintDetailsPrintEventDate\tAwaitingPickupRequestClosedDate\tSearchIndexCallNumberComponentsCallNumber\tSearchIndexCallNumberComponentsPrefix\tSearchIndexCallNumberComponentsSuffix\tSearchIndexShelvingOrder\tSearchIndexPickupServicePointName\tItemLocationCode\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
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
            foreach (var r2 in folioServiceContext.Request2s(where, Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{Global.TextEncode(r2.RequestLevel)}\t{Global.TextEncode(r2.RequestType)}\t{Global.TextEncode(r2.EcsRequestPhase)}\t{r2.RequestDate:M/d/yyyy}\t{Global.TextEncode(r2.PatronComments)}\t{Global.TextEncode(r2.Requester?.Username)}\t{r2.RequesterId}\t{Global.TextEncode(r2.ProxyUser?.Username)}\t{r2.ProxyUserId}\t{Global.TextEncode(r2.Instance?.Title)}\t{r2.InstanceId}\t{r2.Holding?.ShortId}\t{r2.HoldingId}\t{r2.Item?.ShortId}\t{r2.ItemId}\t{Global.TextEncode(r2.Status)}\t{Global.TextEncode(r2.CancellationReason?.Name)}\t{r2.CancellationReasonId}\t{Global.TextEncode(r2.CancelledByUser?.Username)}\t{r2.CancelledByUserId}\t{Global.TextEncode(r2.CancellationAdditionalInformation)}\t{r2.CancelledDate:M/d/yyyy}\t{r2.Position}\t{Global.TextEncode(r2.InstanceTitle)}\t{Global.TextEncode(r2.ItemBarcode)}\t{Global.TextEncode(r2.RequesterFirstName)}\t{Global.TextEncode(r2.RequesterLastName)}\t{Global.TextEncode(r2.RequesterMiddleName)}\t{Global.TextEncode(r2.RequesterBarcode)}\t{Global.TextEncode(r2.RequesterPatronGroup)}\t{Global.TextEncode(r2.ProxyFirstName)}\t{Global.TextEncode(r2.ProxyLastName)}\t{Global.TextEncode(r2.ProxyMiddleName)}\t{Global.TextEncode(r2.ProxyBarcode)}\t{Global.TextEncode(r2.ProxyPatronGroup)}\t{Global.TextEncode(r2.FulfillmentPreference)}\t{Global.TextEncode(r2.DeliveryAddressType?.Name)}\t{r2.DeliveryAddressTypeId}\t{r2.RequestExpirationDate:M/d/yyyy}\t{r2.HoldShelfExpirationDate:M/d/yyyy}\t{Global.TextEncode(r2.PickupServicePoint?.Name)}\t{r2.PickupServicePointId}\t{r2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.CreationUser?.Username)}\t{r2.CreationUserId}\t{r2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.LastWriteUser?.Username)}\t{r2.LastWriteUserId}\t{r2.PrintDetailsPrintCount}\t{Global.TextEncode(r2.PrintDetailsRequester?.Username)}\t{r2.PrintDetailsRequesterId}\t{r2.PrintDetailsIsPrinted}\t{r2.PrintDetailsPrintEventDate:M/d/yyyy}\t{r2.AwaitingPickupRequestClosedDate:M/d/yyyy}\t{Global.TextEncode(r2.SearchIndexCallNumberComponentsCallNumber)}\t{Global.TextEncode(r2.SearchIndexCallNumberComponentsPrefix)}\t{Global.TextEncode(r2.SearchIndexCallNumberComponentsSuffix)}\t{Global.TextEncode(r2.SearchIndexShelvingOrder)}\t{Global.TextEncode(r2.SearchIndexPickupServicePointName)}\t{Global.TextEncode(r2.ItemLocationCode)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
