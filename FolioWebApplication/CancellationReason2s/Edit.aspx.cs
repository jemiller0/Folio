using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CancellationReason2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CancellationReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CancellationReason2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var cr2 = folioServiceContext.FindCancellationReason2(id, true);
            if (cr2 == null) Response.Redirect("Default.aspx");
            cr2.Content = cr2.Content != null ? JsonConvert.DeserializeObject<JToken>(cr2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            CancellationReason2FormView.DataSource = new[] { cr2 };
            Title = $"Cancellation Reason {cr2.Name}";
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)CancellationReason2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"cancellationReasonId == \"{id}\"",
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Request2sRadGrid, "Status", "status"),
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
                Request2sPanel.Visible = CancellationReason2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
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
