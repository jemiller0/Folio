using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CheckIn2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CheckIn2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "OccurredDateTime", "occurredDateTime"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "RequestQueueSize", "requestQueueSize"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemLocation.Name", "itemLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ServicePoint.Name", "servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(CheckIn2sRadGrid, "PerformedByUser.Username", "performedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(where, CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = folioServiceContext.CountCheckIn2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"CheckIn2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tOccurredDateTime\tItem\tItemId\tItemStatusPriorToCheckIn\tRequestQueueSize\tItemLocation\tItemLocationId\tServicePoint\tServicePointId\tPerformedByUser\tPerformedByUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CheckIn2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "OccurredDateTime", "occurredDateTime"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "RequestQueueSize", "requestQueueSize"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemLocation.Name", "itemLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ServicePoint.Name", "servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(CheckIn2sRadGrid, "PerformedByUser.Username", "performedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var ci2 in folioServiceContext.CheckIn2s(where, CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ci2.Id}\t{ci2.OccurredDateTime:M/d/yyyy HH:mm:ss}\t{ci2.Item?.ShortId}\t{ci2.ItemId}\t{Global.TextEncode(ci2.ItemStatusPriorToCheckIn)}\t{ci2.RequestQueueSize}\t{Global.TextEncode(ci2.ItemLocation?.Name)}\t{ci2.ItemLocationId}\t{Global.TextEncode(ci2.ServicePoint?.Name)}\t{ci2.ServicePointId}\t{Global.TextEncode(ci2.PerformedByUser?.Username)}\t{ci2.PerformedByUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
