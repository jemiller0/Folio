using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AlternativeTitleType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AlternativeTitleType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AlternativeTitleType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            AlternativeTitleType2sRadGrid.DataSource = folioServiceContext.AlternativeTitleType2s(out var i, Global.GetCqlFilter(AlternativeTitleType2sRadGrid, d), AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, AlternativeTitleType2sRadGrid.PageSize * AlternativeTitleType2sRadGrid.CurrentPageIndex, AlternativeTitleType2sRadGrid.PageSize, true);
            AlternativeTitleType2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"AlternativeTitleType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var att2 in folioServiceContext.AlternativeTitleType2s(Global.GetCqlFilter(AlternativeTitleType2sRadGrid, d), AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AlternativeTitleType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{att2.Id}\t{Global.TextEncode(att2.Name)}\t{Global.TextEncode(att2.Source)}\t{att2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(att2.CreationUser?.Username)}\t{att2.CreationUserId}\t{att2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(att2.LastWriteUser?.Username)}\t{att2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
