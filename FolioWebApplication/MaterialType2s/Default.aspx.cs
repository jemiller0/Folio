using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.MaterialType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MaterialType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void MaterialType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(MaterialType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(MaterialType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(MaterialType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(MaterialType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(MaterialType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(MaterialType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(MaterialType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            MaterialType2sRadGrid.DataSource = folioServiceContext.MaterialType2s(out var i, where, MaterialType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[MaterialType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(MaterialType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, MaterialType2sRadGrid.PageSize * MaterialType2sRadGrid.CurrentPageIndex, MaterialType2sRadGrid.PageSize, true);
            MaterialType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"MaterialType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var mt2 in folioServiceContext.MaterialType2s(Global.GetCqlFilter(MaterialType2sRadGrid, d), MaterialType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[MaterialType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(MaterialType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{mt2.Id}\t{Global.TextEncode(mt2.Name)}\t{Global.TextEncode(mt2.Source)}\t{mt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(mt2.CreationUser?.Username)}\t{mt2.CreationUserId}\t{mt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(mt2.LastWriteUser?.Username)}\t{mt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
