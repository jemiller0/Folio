using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RequestPolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RequestPolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RequestPolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            RequestPolicy2sRadGrid.DataSource = folioServiceContext.RequestPolicy2s(out var i, Global.GetCqlFilter(RequestPolicy2sRadGrid, d), RequestPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RequestPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RequestPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RequestPolicy2sRadGrid.PageSize * RequestPolicy2sRadGrid.CurrentPageIndex, RequestPolicy2sRadGrid.PageSize, true);
            RequestPolicy2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"RequestPolicy2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var rp2 in folioServiceContext.RequestPolicy2s(Global.GetCqlFilter(RequestPolicy2sRadGrid, d), RequestPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RequestPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RequestPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rp2.Id}\t{Global.TextEncode(rp2.Name)}\t{Global.TextEncode(rp2.Description)}\t{rp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rp2.CreationUser?.Username)}\t{rp2.CreationUserId}\t{rp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(rp2.LastWriteUser?.Username)}\t{rp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
