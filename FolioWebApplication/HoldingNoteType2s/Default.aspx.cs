using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.HoldingNoteType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["HoldingNoteType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void HoldingNoteType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            HoldingNoteType2sRadGrid.DataSource = folioServiceContext.HoldingNoteType2s(out var i, where, HoldingNoteType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[HoldingNoteType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(HoldingNoteType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, HoldingNoteType2sRadGrid.PageSize * HoldingNoteType2sRadGrid.CurrentPageIndex, HoldingNoteType2sRadGrid.PageSize, true);
            HoldingNoteType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"HoldingNoteType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(HoldingNoteType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var hnt2 in folioServiceContext.HoldingNoteType2s(where, HoldingNoteType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[HoldingNoteType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(HoldingNoteType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{hnt2.Id}\t{Global.TextEncode(hnt2.Name)}\t{Global.TextEncode(hnt2.Source)}\t{hnt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(hnt2.CreationUser?.Username)}\t{hnt2.CreationUserId}\t{hnt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(hnt2.LastWriteUser?.Username)}\t{hnt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
