using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Formats
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FormatsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FormatsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(FormatsRadGrid, "Id", "id"),
                Global.GetCqlFilter(FormatsRadGrid, "Name", "name"),
                Global.GetCqlFilter(FormatsRadGrid, "Code", "code"),
                Global.GetCqlFilter(FormatsRadGrid, "Source", "source"),
                Global.GetCqlFilter(FormatsRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FormatsRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FormatsRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FormatsRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FormatsRadGrid.DataSource = folioServiceContext.Formats(out var i, where, FormatsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FormatsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FormatsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FormatsRadGrid.PageSize * FormatsRadGrid.CurrentPageIndex, FormatsRadGrid.PageSize, true);
            FormatsRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Formats.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var f in folioServiceContext.Formats(Global.GetCqlFilter(FormatsRadGrid, d), FormatsRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FormatsRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FormatsRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{f.Id}\t{Global.TextEncode(f.Name)}\t{Global.TextEncode(f.Code)}\t{Global.TextEncode(f.Source)}\t{f.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f.CreationUser?.Username)}\t{f.CreationUserId}\t{f.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f.LastWriteUser?.Username)}\t{f.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
