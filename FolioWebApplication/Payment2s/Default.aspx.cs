using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Payment2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Payment2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePointId", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Payment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Payment2sRadGrid, "CreationTime", "dateAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "TypeAction", "typeAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "Comments", "comments"),
                Global.GetCqlFilter(Payment2sRadGrid, "Notify", "notify"),
                Global.GetCqlFilter(Payment2sRadGrid, "Amount", "amountAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "RemainingAmount", "balance"),
                Global.GetCqlFilter(Payment2sRadGrid, "TransactionInformation", "transactionInformation"),
                Global.GetCqlFilter(Payment2sRadGrid, "ServicePoint.Name", "createdAt", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Payment2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Payment2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Payment2sRadGrid, "Fee.Title", "accountId", "title", folioServiceContext.FolioServiceClient.Fees),
                Global.GetCqlFilter(Payment2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Payment2sRadGrid.DataSource = folioServiceContext.Payment2s(out var i, where, Payment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Payment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Payment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Payment2sRadGrid.PageSize * Payment2sRadGrid.CurrentPageIndex, Payment2sRadGrid.PageSize, true);
            Payment2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Payment2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tCreationTime\tTypeAction\tComments\tNotify\tAmount\tRemainingAmount\tTransactionInformation\tServicePoint\tServicePointId\tSource\tPaymentMethod\tFee\tFeeId\tUser\tUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePointId", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Payment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Payment2sRadGrid, "CreationTime", "dateAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "TypeAction", "typeAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "Comments", "comments"),
                Global.GetCqlFilter(Payment2sRadGrid, "Notify", "notify"),
                Global.GetCqlFilter(Payment2sRadGrid, "Amount", "amountAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "RemainingAmount", "balance"),
                Global.GetCqlFilter(Payment2sRadGrid, "TransactionInformation", "transactionInformation"),
                Global.GetCqlFilter(Payment2sRadGrid, "ServicePoint.Name", "createdAt", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Payment2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Payment2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Payment2sRadGrid, "Fee.Title", "accountId", "title", folioServiceContext.FolioServiceClient.Fees),
                Global.GetCqlFilter(Payment2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var p2 in folioServiceContext.Payment2s(where, Payment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Payment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Payment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{p2.Id}\t{p2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p2.TypeAction)}\t{Global.TextEncode(p2.Comments)}\t{p2.Notify}\t{p2.Amount}\t{p2.RemainingAmount}\t{Global.TextEncode(p2.TransactionInformation)}\t{Global.TextEncode(p2.ServicePoint?.Name)}\t{p2.ServicePointId}\t{Global.TextEncode(p2.Source)}\t{Global.TextEncode(p2.PaymentMethod)}\t{Global.TextEncode(p2.Fee?.Title)}\t{p2.FeeId}\t{Global.TextEncode(p2.User?.Username)}\t{p2.UserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
