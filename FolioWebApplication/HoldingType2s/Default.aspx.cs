using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.HoldingType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["HoldingType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void HoldingType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(HoldingType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(HoldingType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            HoldingType2sRadGrid.DataSource = folioServiceContext.HoldingType2s(out var i, where, HoldingType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[HoldingType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(HoldingType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, HoldingType2sRadGrid.PageSize * HoldingType2sRadGrid.CurrentPageIndex, HoldingType2sRadGrid.PageSize, true);
            HoldingType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"HoldingType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(HoldingType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(HoldingType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(HoldingType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var ht2 in folioServiceContext.HoldingType2s(where, HoldingType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[HoldingType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(HoldingType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ht2.Id}\t{Global.TextEncode(ht2.Name)}\t{Global.TextEncode(ht2.Source)}\t{ht2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ht2.CreationUser?.Username)}\t{ht2.CreationUserId}\t{ht2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ht2.LastWriteUser?.Username)}\t{ht2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
