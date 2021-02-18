using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PaymentMethod2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PaymentMethod2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PaymentMethod2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameMethod" }, { "AllowedRefundMethod", "allowedRefundMethod" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            PaymentMethod2sRadGrid.DataSource = folioServiceContext.PaymentMethod2s(out var i, Global.GetCqlFilter(PaymentMethod2sRadGrid, d), PaymentMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PaymentMethod2sRadGrid.PageSize * PaymentMethod2sRadGrid.CurrentPageIndex, PaymentMethod2sRadGrid.PageSize, true);
            PaymentMethod2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"PaymentMethod2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameMethod" }, { "AllowedRefundMethod", "allowedRefundMethod" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            Response.Write("Id\tName\tAllowedRefundMethod\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tOwner\tOwnerId\r\n");
            foreach (var pm2 in folioServiceContext.PaymentMethod2s(Global.GetCqlFilter(PaymentMethod2sRadGrid, d), PaymentMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{pm2.Id}\t{Global.TextEncode(pm2.Name)}\t{pm2.AllowedRefundMethod}\t{pm2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pm2.CreationUser?.Username)}\t{pm2.CreationUserId}\t{pm2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pm2.LastWriteUser?.Username)}\t{pm2.LastWriteUserId}\t{Global.TextEncode(pm2.Owner?.Name)}\t{pm2.OwnerId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
