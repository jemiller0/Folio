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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            NatureOfContentTerm2sRadGrid.DataSource = folioServiceContext.NatureOfContentTerm2s(out var i, where, NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, NatureOfContentTerm2sRadGrid.PageSize * NatureOfContentTerm2sRadGrid.CurrentPageIndex, NatureOfContentTerm2sRadGrid.PageSize, true);
            NatureOfContentTerm2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"NatureOfContentTerm2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(NatureOfContentTerm2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var noct2 in folioServiceContext.NatureOfContentTerm2s(where, NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(NatureOfContentTerm2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
