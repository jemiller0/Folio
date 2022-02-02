using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.StatisticalCodeType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StatisticalCodeType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StatisticalCodeType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(StatisticalCodeType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            StatisticalCodeType2sRadGrid.DataSource = folioServiceContext.StatisticalCodeType2s(out var i, where, StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StatisticalCodeType2sRadGrid.PageSize * StatisticalCodeType2sRadGrid.CurrentPageIndex, StatisticalCodeType2sRadGrid.PageSize, true);
            StatisticalCodeType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"StatisticalCodeType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var sct2 in folioServiceContext.StatisticalCodeType2s(Global.GetCqlFilter(StatisticalCodeType2sRadGrid, d), StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCodeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{sct2.Id}\t{Global.TextEncode(sct2.Name)}\t{Global.TextEncode(sct2.Source)}\t{sct2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sct2.CreationUser?.Username)}\t{sct2.CreationUserId}\t{sct2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sct2.LastWriteUser?.Username)}\t{sct2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
