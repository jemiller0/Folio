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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Title2sRadGrid.DataSource = folioServiceContext.Title2s(out var i, Global.GetCqlFilter(Title2sRadGrid, d), Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Title2sRadGrid.PageSize * Title2sRadGrid.CurrentPageIndex, Title2sRadGrid.PageSize, true);
            Title2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Title2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ExpectedReceiptDate", "expectedReceiptDate" }, { "Title", "title" }, { "OrderItemId", "poLineId" }, { "InstanceId", "instanceId" }, { "Publisher", "publisher" }, { "Edition", "edition" }, { "PackageName", "packageName" }, { "OrderItemNumber", "poLineNumber" }, { "PublishedDate", "publishedDate" }, { "ReceivingNote", "receivingNote" }, { "SubscriptionFrom", "subscriptionFrom" }, { "SubscriptionTo", "subscriptionTo" }, { "SubscriptionInterval", "subscriptionInterval" }, { "IsAcknowledged", "isAcknowledged" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tExpectedReceiptDate\tTitle\tOrderItem\tOrderItemId\tInstance\tInstanceId\tPublisher\tEdition\tPackageName\tOrderItemNumber\tPublishedDate\tReceivingNote\tSubscriptionFrom\tSubscriptionTo\tSubscriptionInterval\tIsAcknowledged\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var t2 in folioServiceContext.Title2s(Global.GetCqlFilter(Title2sRadGrid, d), Title2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Title2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Title2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{t2.Id}\t{t2.ExpectedReceiptDate:M/d/yyyy}\t{Global.TextEncode(t2.Title)}\t{Global.TextEncode(t2.OrderItem?.Number)}\t{t2.OrderItemId}\t{Global.TextEncode(t2.Instance?.Title)}\t{t2.InstanceId}\t{Global.TextEncode(t2.Publisher)}\t{Global.TextEncode(t2.Edition)}\t{Global.TextEncode(t2.PackageName)}\t{Global.TextEncode(t2.OrderItemNumber)}\t{Global.TextEncode(t2.PublishedDate)}\t{Global.TextEncode(t2.ReceivingNote)}\t{t2.SubscriptionFrom:M/d/yyyy}\t{t2.SubscriptionTo:M/d/yyyy}\t{t2.SubscriptionInterval}\t{t2.IsAcknowledged}\t{t2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.CreationUser?.Username)}\t{t2.CreationUserId}\t{t2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.LastWriteUser?.Username)}\t{t2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
