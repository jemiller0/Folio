using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Proxy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Proxy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Proxy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Proxy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Proxy2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "RequestForSponsor", "requestForSponsor"),
                Global.GetCqlFilter(Proxy2sRadGrid, "NotificationsTo", "notificationsTo"),
                Global.GetCqlFilter(Proxy2sRadGrid, "AccrueTo", "accrueTo"),
                Global.GetCqlFilter(Proxy2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Proxy2sRadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Proxy2sRadGrid.DataSource = folioServiceContext.Proxy2s(where, Proxy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Proxy2sRadGrid.PageSize * Proxy2sRadGrid.CurrentPageIndex, Proxy2sRadGrid.PageSize, true);
            Proxy2sRadGrid.VirtualItemCount = folioServiceContext.CountProxy2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Proxy2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tUser\tUserId\tProxyUser\tProxyUserId\tRequestForSponsor\tNotificationsTo\tAccrueTo\tStatus\tExpirationDate\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Proxy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Proxy2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "RequestForSponsor", "requestForSponsor"),
                Global.GetCqlFilter(Proxy2sRadGrid, "NotificationsTo", "notificationsTo"),
                Global.GetCqlFilter(Proxy2sRadGrid, "AccrueTo", "accrueTo"),
                Global.GetCqlFilter(Proxy2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Proxy2sRadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Proxy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var p2 in folioServiceContext.Proxy2s(where, Proxy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{p2.Id}\t{Global.TextEncode(p2.User?.Username)}\t{p2.UserId}\t{Global.TextEncode(p2.ProxyUser?.Username)}\t{p2.ProxyUserId}\t{Global.TextEncode(p2.RequestForSponsor)}\t{Global.TextEncode(p2.NotificationsTo)}\t{Global.TextEncode(p2.AccrueTo)}\t{Global.TextEncode(p2.Status)}\t{p2.ExpirationDate:M/d/yyyy}\t{p2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p2.CreationUser?.Username)}\t{p2.CreationUserId}\t{p2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p2.LastWriteUser?.Username)}\t{p2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
