using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CallNumberType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CallNumberType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CallNumberType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            CallNumberType2sRadGrid.DataSource = folioServiceContext.CallNumberType2s(where, CallNumberType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CallNumberType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CallNumberType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CallNumberType2sRadGrid.PageSize * CallNumberType2sRadGrid.CurrentPageIndex, CallNumberType2sRadGrid.PageSize, true);
            CallNumberType2sRadGrid.VirtualItemCount = folioServiceContext.CountCallNumberType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"CallNumberType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(CallNumberType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var cnt2 in folioServiceContext.CallNumberType2s(where, CallNumberType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CallNumberType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CallNumberType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{cnt2.Id}\t{Global.TextEncode(cnt2.Name)}\t{Global.TextEncode(cnt2.Source)}\t{cnt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cnt2.CreationUser?.Username)}\t{cnt2.CreationUserId}\t{cnt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cnt2.LastWriteUser?.Username)}\t{cnt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
