using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Comment2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Comment2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Comment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Paid", "paid" }, { "Waived", "waived" }, { "Refunded", "refunded" }, { "TransferredManually", "transferredManually" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Comment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Comment2sRadGrid, "Paid", "paid"),
                Global.GetCqlFilter(Comment2sRadGrid, "Waived", "waived"),
                Global.GetCqlFilter(Comment2sRadGrid, "Refunded", "refunded"),
                Global.GetCqlFilter(Comment2sRadGrid, "TransferredManually", "transferredManually"),
                Global.GetCqlFilter(Comment2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Comment2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Comment2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Comment2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Comment2sRadGrid.DataSource = folioServiceContext.Comment2s(where, Comment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Comment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Comment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Comment2sRadGrid.PageSize * Comment2sRadGrid.CurrentPageIndex, Comment2sRadGrid.PageSize, true);
            Comment2sRadGrid.VirtualItemCount = folioServiceContext.CountComment2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Comment2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tPaid\tWaived\tRefunded\tTransferredManually\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Paid", "paid" }, { "Waived", "waived" }, { "Refunded", "refunded" }, { "TransferredManually", "transferredManually" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Comment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Comment2sRadGrid, "Paid", "paid"),
                Global.GetCqlFilter(Comment2sRadGrid, "Waived", "waived"),
                Global.GetCqlFilter(Comment2sRadGrid, "Refunded", "refunded"),
                Global.GetCqlFilter(Comment2sRadGrid, "TransferredManually", "transferredManually"),
                Global.GetCqlFilter(Comment2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Comment2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Comment2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Comment2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var c2 in folioServiceContext.Comment2s(where, Comment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Comment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Comment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{c2.Id}\t{c2.Paid}\t{c2.Waived}\t{c2.Refunded}\t{c2.TransferredManually}\t{c2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.CreationUser?.Username)}\t{c2.CreationUserId}\t{c2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.LastWriteUser?.Username)}\t{c2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
