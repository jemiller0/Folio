using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ContributorNameType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ContributorNameType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ContributorNameType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Ordering", "ordering" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Ordering", "ordering"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ContributorNameType2sRadGrid.DataSource = folioServiceContext.ContributorNameType2s(where, ContributorNameType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ContributorNameType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ContributorNameType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ContributorNameType2sRadGrid.PageSize * ContributorNameType2sRadGrid.CurrentPageIndex, ContributorNameType2sRadGrid.PageSize, true);
            ContributorNameType2sRadGrid.VirtualItemCount = folioServiceContext.CountContributorNameType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ContributorNameType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tOrdering\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Ordering", "ordering" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Ordering", "ordering"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ContributorNameType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var cnt2 in folioServiceContext.ContributorNameType2s(where, ContributorNameType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ContributorNameType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ContributorNameType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{cnt2.Id}\t{Global.TextEncode(cnt2.Name)}\t{Global.TextEncode(cnt2.Ordering)}\t{Global.TextEncode(cnt2.Source)}\t{cnt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cnt2.CreationUser?.Username)}\t{cnt2.CreationUserId}\t{cnt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cnt2.LastWriteUser?.Username)}\t{cnt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
