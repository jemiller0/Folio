using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Template2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Template2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Template2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Active", "active" }, { "Category", "category" }, { "Description", "description" }, { "TemplateResolver", "templateResolver" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Template2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Template2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Template2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(Template2sRadGrid, "Category", "category"),
                Global.GetCqlFilter(Template2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Template2sRadGrid, "TemplateResolver", "templateResolver"),
                Global.GetCqlFilter(Template2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Template2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Template2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Template2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Template2sRadGrid.DataSource = folioServiceContext.Template2s(out var i, where, Template2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Template2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Template2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Template2sRadGrid.PageSize * Template2sRadGrid.CurrentPageIndex, Template2sRadGrid.PageSize, true);
            Template2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Template2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tActive\tCategory\tDescription\tTemplateResolver\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Active", "active" }, { "Category", "category" }, { "Description", "description" }, { "TemplateResolver", "templateResolver" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Template2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Template2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Template2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(Template2sRadGrid, "Category", "category"),
                Global.GetCqlFilter(Template2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Template2sRadGrid, "TemplateResolver", "templateResolver"),
                Global.GetCqlFilter(Template2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Template2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Template2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Template2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var t2 in folioServiceContext.Template2s(where, Template2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Template2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Template2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{t2.Id}\t{Global.TextEncode(t2.Name)}\t{t2.Active}\t{Global.TextEncode(t2.Category)}\t{Global.TextEncode(t2.Description)}\t{Global.TextEncode(t2.TemplateResolver)}\t{t2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.CreationUser?.Username)}\t{t2.CreationUserId}\t{t2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(t2.LastWriteUser?.Username)}\t{t2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
