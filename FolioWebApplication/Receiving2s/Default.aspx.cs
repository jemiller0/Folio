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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Receiving2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Response.Write("Id\tCaption\tComment\tFormat\tItem\tItemId\tLocation\tLocationId\tOrderItem\tOrderItemId\tTitle\tTitleId\tReceivingStatus\tSupplement\tReceiptTime\tReceiveTime\r\n");
            foreach (var r2 in folioServiceContext.Receiving2s(Global.GetCqlFilter(Receiving2sRadGrid, d), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{Global.TextEncode(r2.Caption)}\t{Global.TextEncode(r2.Comment)}\t{Global.TextEncode(r2.Format)}\t{r2.Item?.ShortId}\t{r2.ItemId}\t{Global.TextEncode(r2.Location?.Name)}\t{r2.LocationId}\t{Global.TextEncode(r2.OrderItem?.Number)}\t{r2.OrderItemId}\t{Global.TextEncode(r2.Title?.Title)}\t{r2.TitleId}\t{Global.TextEncode(r2.ReceivingStatus)}\t{r2.Supplement}\t{r2.ReceiptTime:M/d/yyyy HH:mm:ss}\t{r2.ReceiveTime:M/d/yyyy HH:mm:ss}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
