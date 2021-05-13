using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Owner2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Owner2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Owner2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Owner2sRadGrid.DataSource = folioServiceContext.Owner2s(out var i, Global.GetCqlFilter(Owner2sRadGrid, d), Owner2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Owner2sRadGrid.PageSize * Owner2sRadGrid.CurrentPageIndex, Owner2sRadGrid.PageSize, true);
            Owner2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Owner2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tDefaultChargeNotice\tDefaultChargeNoticeId\tDefaultActionNotice\tDefaultActionNoticeId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var o2 in folioServiceContext.Owner2s(Global.GetCqlFilter(Owner2sRadGrid, d), Owner2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{o2.Id}\t{Global.TextEncode(o2.Name)}\t{Global.TextEncode(o2.Description)}\t{Global.TextEncode(o2.DefaultChargeNotice?.Name)}\t{o2.DefaultChargeNoticeId}\t{Global.TextEncode(o2.DefaultActionNotice?.Name)}\t{o2.DefaultActionNoticeId}\t{o2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.CreationUser?.Username)}\t{o2.CreationUserId}\t{o2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.LastWriteUser?.Username)}\t{o2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
