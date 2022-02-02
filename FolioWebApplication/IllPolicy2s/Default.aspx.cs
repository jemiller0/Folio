using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.IllPolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IllPolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IllPolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(IllPolicy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(IllPolicy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            IllPolicy2sRadGrid.DataSource = folioServiceContext.IllPolicy2s(out var i, where, IllPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IllPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IllPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, IllPolicy2sRadGrid.PageSize * IllPolicy2sRadGrid.CurrentPageIndex, IllPolicy2sRadGrid.PageSize, true);
            IllPolicy2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"IllPolicy2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ip2 in folioServiceContext.IllPolicy2s(Global.GetCqlFilter(IllPolicy2sRadGrid, d), IllPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IllPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IllPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ip2.Id}\t{Global.TextEncode(ip2.Name)}\t{Global.TextEncode(ip2.Source)}\t{ip2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ip2.CreationUser?.Username)}\t{ip2.CreationUserId}\t{ip2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ip2.LastWriteUser?.Username)}\t{ip2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
