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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Comment2sRadGrid.DataSource = folioServiceContext.Comment2s(out var i, Global.GetCqlFilter(Comment2sRadGrid, d), Comment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Comment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Comment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Comment2sRadGrid.PageSize * Comment2sRadGrid.CurrentPageIndex, Comment2sRadGrid.PageSize, true);
            Comment2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Comment2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Paid", "paid" }, { "Waived", "waived" }, { "Refunded", "refunded" }, { "TransferredManually", "transferredManually" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tPaid\tWaived\tRefunded\tTransferredManually\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var c2 in folioServiceContext.Comment2s(Global.GetCqlFilter(Comment2sRadGrid, d), Comment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Comment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Comment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
