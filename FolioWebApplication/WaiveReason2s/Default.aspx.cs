using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.WaiveReason2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["WaiveReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void WaiveReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Name", "nameReason"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Account.Title", "accountId", "title", folioServiceContext.FolioServiceClient.Fees)
            }.Where(s => s != null)));
            WaiveReason2sRadGrid.DataSource = folioServiceContext.WaiveReason2s(where, WaiveReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, WaiveReason2sRadGrid.PageSize * WaiveReason2sRadGrid.CurrentPageIndex, WaiveReason2sRadGrid.PageSize, true);
            WaiveReason2sRadGrid.VirtualItemCount = folioServiceContext.CountWaiveReason2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"WaiveReason2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tAccount\tAccountId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameReason" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AccountId", "accountId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Name", "nameReason"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(WaiveReason2sRadGrid, "Account.Title", "accountId", "title", folioServiceContext.FolioServiceClient.Fees)
            }.Where(s => s != null)));
            foreach (var wr2 in folioServiceContext.WaiveReason2s(where, WaiveReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(WaiveReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{wr2.Id}\t{Global.TextEncode(wr2.Name)}\t{Global.TextEncode(wr2.Description)}\t{wr2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(wr2.CreationUser?.Username)}\t{wr2.CreationUserId}\t{wr2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(wr2.LastWriteUser?.Username)}\t{wr2.LastWriteUserId}\t{Global.TextEncode(wr2.Account?.Title)}\t{wr2.AccountId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
