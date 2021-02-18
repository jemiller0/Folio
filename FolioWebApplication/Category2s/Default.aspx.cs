using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Category2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Category2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Category2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Category2sRadGrid.DataSource = folioServiceContext.Category2s(out var i, Global.GetCqlFilter(Category2sRadGrid, d), Category2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Category2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Category2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Category2sRadGrid.PageSize * Category2sRadGrid.CurrentPageIndex, Category2sRadGrid.PageSize, true);
            Category2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Category2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var c2 in folioServiceContext.Category2s(Global.GetCqlFilter(Category2sRadGrid, d), Category2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Category2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Category2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{c2.Id}\t{Global.TextEncode(c2.Name)}\t{c2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.CreationUser?.Username)}\t{c2.CreationUserId}\t{c2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.LastWriteUser?.Username)}\t{c2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
