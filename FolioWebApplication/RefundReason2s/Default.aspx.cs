using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RefundReason2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RefundReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RefundReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            RefundReason2sRadGrid.DataSource = folioServiceContext.RefundReason2s(out var i, Global.GetCqlFilter(RefundReason2sRadGrid, d), RefundReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RefundReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RefundReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RefundReason2sRadGrid.PageSize * RefundReason2sRadGrid.CurrentPageIndex, RefundReason2sRadGrid.PageSize, true);
            RefundReason2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"RefundReason2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tAccount\tAccountId\r\n");
            foreach (var rr2 in folioServiceContext.RefundReason2s(Global.GetCqlFilter(RefundReason2sRadGrid, d), RefundReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RefundReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RefundReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rr2.Id}\t{Global.TextEncode(rr2.Name)}\t{Global.TextEncode(rr2.Description)}\t{rr2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rr2.CreationUser?.Username)}\t{rr2.CreationUserId}\t{rr2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rr2.LastWriteUser?.Username)}\t{rr2.LastWriteUserId}\t{Global.TextEncode(rr2.Account?.Title)}\t{rr2.AccountId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
