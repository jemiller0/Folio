using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.IdType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IdType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(IdType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(IdType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(IdType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(IdType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(IdType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(IdType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(IdType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            IdType2sRadGrid.DataSource = folioServiceContext.IdType2s(out var i, where, IdType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IdType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IdType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, IdType2sRadGrid.PageSize * IdType2sRadGrid.CurrentPageIndex, IdType2sRadGrid.PageSize, true);
            IdType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"IdType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var it2 in folioServiceContext.IdType2s(Global.GetCqlFilter(IdType2sRadGrid, d), IdType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IdType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IdType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{it2.Id}\t{Global.TextEncode(it2.Name)}\t{Global.TextEncode(it2.Source)}\t{it2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(it2.CreationUser?.Username)}\t{it2.CreationUserId}\t{it2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(it2.LastWriteUser?.Username)}\t{it2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
