using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Title2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Title2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Title2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Title2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Title2sRadGrid, "ExpectedReceiptDate", "expectedReceiptDate"),
                Global.GetCqlFilter(Title2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Title2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Title2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
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
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(out var i, where, Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Title2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tExpectedReceiptDate\tTitle\tOrderItem\tOrderItemId\tInstance\tInstanceId\tPublisher\tEdition\tPackageName\tOrderItemNumber\tPublishedDate\tReceivingNote\tSubscriptionFrom\tSubscriptionTo\tSubscriptionInterval\tClaimingActive\tClaimingInterval\tIsAcknowledged\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "ClaimingActive", "claimingActive" }, { "ClaimingInterval", "claimingInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Title2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Title2sRadGrid, "ExpectedReceiptDate", "expectedReceiptDate"),
                Global.GetCqlFilter(Title2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Title2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Title2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
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
            foreach (var t2 in folioServiceContext.Title2s(where, Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{t2.Id}\t{t2.ExpectedReceiptDate:M/d/yyyy}\t{Global.TextEncode(t2.Title)}\t{Global.TextEncode(t2.OrderItem?.Number)}\t{t2.OrderItemId}\t{Global.TextEncode(t2.Instance?.Title)}\t{t2.InstanceId}\t{Global.TextEncode(t2.Publisher)}\t{Global.TextEncode(t2.Edition)}\t{Global.TextEncode(t2.PackageName)}\t{Global.TextEncode(t2.OrderItemNumber)}\t{Global.TextEncode(t2.PublishedDate)}\t{Global.TextEncode(t2.ReceivingNote)}\t{t2.SubscriptionFrom:M/d/yyyy}\t{t2.SubscriptionTo:M/d/yyyy}\t{t2.SubscriptionInterval}\t{t2.ClaimingActive}\t{t2.ClaimingInterval}\t{t2.IsAcknowledged}\t{t2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.CreationUser?.Username)}\t{t2.CreationUserId}\t{t2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.LastWriteUser?.Username)}\t{t2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
