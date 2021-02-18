using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.NatureOfContentTerm2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NatureOfContentTerm2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void NatureOfContentTerm2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            NatureOfContentTerm2sRadGrid.DataSource = folioServiceContext.NatureOfContentTerm2s(out var i, Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, d), NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, NatureOfContentTerm2sRadGrid.PageSize * NatureOfContentTerm2sRadGrid.CurrentPageIndex, NatureOfContentTerm2sRadGrid.PageSize, true);
            NatureOfContentTerm2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"NatureOfContentTerm2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var noct2 in folioServiceContext.NatureOfContentTerm2s(Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, d), NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{noct2.Id}\t{Global.TextEncode(noct2.Name)}\t{Global.TextEncode(noct2.Source)}\t{noct2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(noct2.CreationUser?.Username)}\t{noct2.CreationUserId}\t{noct2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(noct2.LastWriteUser?.Username)}\t{noct2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
