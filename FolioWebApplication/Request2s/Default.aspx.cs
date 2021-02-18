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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(out var i, Global.GetCqlFilter(Request2sRadGrid, d), Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Request2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Response.Write("Id\tRequestType\tRequestDate\tRequester\tRequesterId\tProxyUser\tProxyUserId\tItem\tItemId\tStatus\tCancellationReason\tCancellationReasonId\tCancelledByUser\tCancelledByUserId\tCancellationAdditionalInformation\tCancelledDate\tPosition\tItemTitle\tItemBarcode\tRequesterFirstName\tRequesterLastName\tRequesterMiddleName\tRequesterBarcode\tRequesterPatronGroup\tProxyFirstName\tProxyLastName\tProxyMiddleName\tProxyBarcode\tProxyPatronGroup\tFulfilmentPreference\tDeliveryAddressType\tDeliveryAddressTypeId\tRequestExpirationDate\tHoldShelfExpirationDate\tPickupServicePoint\tPickupServicePointId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tAwaitingPickupRequestClosedDate\r\n");
            foreach (var r2 in folioServiceContext.Request2s(Global.GetCqlFilter(Request2sRadGrid, d), Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{Global.TextEncode(r2.RequestType)}\t{r2.RequestDate:M/d/yyyy}\t{Global.TextEncode(r2.Requester?.Username)}\t{r2.RequesterId}\t{Global.TextEncode(r2.ProxyUser?.Username)}\t{r2.ProxyUserId}\t{r2.Item?.ShortId}\t{r2.ItemId}\t{Global.TextEncode(r2.Status)}\t{Global.TextEncode(r2.CancellationReason?.Name)}\t{r2.CancellationReasonId}\t{Global.TextEncode(r2.CancelledByUser?.Username)}\t{r2.CancelledByUserId}\t{Global.TextEncode(r2.CancellationAdditionalInformation)}\t{r2.CancelledDate:M/d/yyyy}\t{r2.Position}\t{Global.TextEncode(r2.ItemTitle)}\t{Global.TextEncode(r2.ItemBarcode)}\t{Global.TextEncode(r2.RequesterFirstName)}\t{Global.TextEncode(r2.RequesterLastName)}\t{Global.TextEncode(r2.RequesterMiddleName)}\t{Global.TextEncode(r2.RequesterBarcode)}\t{Global.TextEncode(r2.RequesterPatronGroup)}\t{Global.TextEncode(r2.ProxyFirstName)}\t{Global.TextEncode(r2.ProxyLastName)}\t{Global.TextEncode(r2.ProxyMiddleName)}\t{Global.TextEncode(r2.ProxyBarcode)}\t{Global.TextEncode(r2.ProxyPatronGroup)}\t{Global.TextEncode(r2.FulfilmentPreference)}\t{Global.TextEncode(r2.DeliveryAddressType?.Name)}\t{r2.DeliveryAddressTypeId}\t{r2.RequestExpirationDate:M/d/yyyy}\t{r2.HoldShelfExpirationDate:M/d/yyyy}\t{Global.TextEncode(r2.PickupServicePoint?.Name)}\t{r2.PickupServicePointId}\t{r2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.CreationUser?.Username)}\t{r2.CreationUserId}\t{r2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.LastWriteUser?.Username)}\t{r2.LastWriteUserId}\t{r2.AwaitingPickupRequestClosedDate:M/d/yyyy}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
