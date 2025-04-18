using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ItemDamagedStatus2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ItemDamagedStatus2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ItemDamagedStatus2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ItemDamagedStatus2sRadGrid.DataSource = folioServiceContext.ItemDamagedStatus2s(where, ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ItemDamagedStatus2sRadGrid.PageSize * ItemDamagedStatus2sRadGrid.CurrentPageIndex, ItemDamagedStatus2sRadGrid.PageSize, true);
            ItemDamagedStatus2sRadGrid.VirtualItemCount = folioServiceContext.CountItemDamagedStatus2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ItemDamagedStatus2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ItemDamagedStatus2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var ids2 in folioServiceContext.ItemDamagedStatus2s(where, ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ItemDamagedStatus2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ids2.Id}\t{Global.TextEncode(ids2.Name)}\t{Global.TextEncode(ids2.Source)}\t{ids2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ids2.CreationUser?.Username)}\t{ids2.CreationUserId}\t{ids2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ids2.LastWriteUser?.Username)}\t{ids2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
