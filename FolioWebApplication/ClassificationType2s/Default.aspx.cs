using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ClassificationType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ClassificationType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ClassificationType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ClassificationType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ClassificationType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ClassificationType2sRadGrid.DataSource = folioServiceContext.ClassificationType2s(out var i, where, ClassificationType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ClassificationType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ClassificationType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ClassificationType2sRadGrid.PageSize * ClassificationType2sRadGrid.CurrentPageIndex, ClassificationType2sRadGrid.PageSize, true);
            ClassificationType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ClassificationType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ct2 in folioServiceContext.ClassificationType2s(Global.GetCqlFilter(ClassificationType2sRadGrid, d), ClassificationType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ClassificationType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ClassificationType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ct2.Id}\t{Global.TextEncode(ct2.Name)}\t{Global.TextEncode(ct2.Source)}\t{ct2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ct2.CreationUser?.Username)}\t{ct2.CreationUserId}\t{ct2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ct2.LastWriteUser?.Username)}\t{ct2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
