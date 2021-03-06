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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Template2sRadGrid.DataSource = folioServiceContext.Template2s(out var i, Global.GetCqlFilter(Template2sRadGrid, d), Template2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Template2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Template2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Template2sRadGrid.PageSize * Template2sRadGrid.CurrentPageIndex, Template2sRadGrid.PageSize, true);
            Template2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Template2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Active", "active" }, { "Category", "category" }, { "Description", "description" }, { "TemplateResolver", "templateResolver" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tActive\tCategory\tDescription\tTemplateResolver\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var t2 in folioServiceContext.Template2s(Global.GetCqlFilter(Template2sRadGrid, d), Template2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Template2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Template2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
