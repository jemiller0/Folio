using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PrecedingSucceedingTitle2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PrecedingSucceedingTitle2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PrecedingSucceedingTitle2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "Hrid", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PrecedingSucceedingTitle2sRadGrid.DataSource = folioServiceContext.PrecedingSucceedingTitle2s(out var i, Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, d), PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PrecedingSucceedingTitle2sRadGrid.PageSize * PrecedingSucceedingTitle2sRadGrid.CurrentPageIndex, PrecedingSucceedingTitle2sRadGrid.PageSize, true);
            PrecedingSucceedingTitle2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"PrecedingSucceedingTitle2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PrecedingInstanceId", "precedingInstanceId" }, { "SucceedingInstanceId", "succeedingInstanceId" }, { "Title", "title" }, { "Hrid", "hrid" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tPrecedingInstance\tPrecedingInstanceId\tSucceedingInstance\tSucceedingInstanceId\tTitle\tHrid\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var pst2 in folioServiceContext.PrecedingSucceedingTitle2s(Global.GetCqlFilter(PrecedingSucceedingTitle2sRadGrid, d), PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrecedingSucceedingTitle2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{pst2.Id}\t{Global.TextEncode(pst2.PrecedingInstance?.Title)}\t{pst2.PrecedingInstanceId}\t{Global.TextEncode(pst2.SucceedingInstance?.Title)}\t{pst2.SucceedingInstanceId}\t{Global.TextEncode(pst2.Title)}\t{Global.TextEncode(pst2.Hrid)}\t{pst2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pst2.CreationUser?.Username)}\t{pst2.CreationUserId}\t{pst2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pst2.LastWriteUser?.Username)}\t{pst2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
