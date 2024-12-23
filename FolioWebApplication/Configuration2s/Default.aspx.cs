using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Configuration2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Configuration2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Configuration2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Module", "module" }, { "ConfigName", "configName" }, { "Code", "code" }, { "Description", "description" }, { "Default", "default" }, { "Enabled", "enabled" }, { "Value", "value" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Configuration2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Module", "module"),
                Global.GetCqlFilter(Configuration2sRadGrid, "ConfigName", "configName"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Default", "default"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Enabled", "enabled"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Value", "value"),
                Global.GetCqlFilter(Configuration2sRadGrid, "UserId", "userId"),
                Global.GetCqlFilter(Configuration2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Configuration2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Configuration2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Configuration2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Configuration2sRadGrid.DataSource = folioServiceContext.Configuration2s(where, Configuration2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Configuration2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Configuration2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Configuration2sRadGrid.PageSize * Configuration2sRadGrid.CurrentPageIndex, Configuration2sRadGrid.PageSize, true);
            Configuration2sRadGrid.VirtualItemCount = folioServiceContext.CountConfiguration2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Configuration2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tModule\tConfigName\tCode\tDescription\tDefault\tEnabled\tValue\tUserId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Module", "module" }, { "ConfigName", "configName" }, { "Code", "code" }, { "Description", "description" }, { "Default", "default" }, { "Enabled", "enabled" }, { "Value", "value" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Configuration2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Module", "module"),
                Global.GetCqlFilter(Configuration2sRadGrid, "ConfigName", "configName"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Default", "default"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Enabled", "enabled"),
                Global.GetCqlFilter(Configuration2sRadGrid, "Value", "value"),
                Global.GetCqlFilter(Configuration2sRadGrid, "UserId", "userId"),
                Global.GetCqlFilter(Configuration2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Configuration2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Configuration2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Configuration2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var c2 in folioServiceContext.Configuration2s(where, Configuration2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Configuration2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Configuration2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{c2.Id}\t{Global.TextEncode(c2.Module)}\t{Global.TextEncode(c2.ConfigName)}\t{Global.TextEncode(c2.Code)}\t{Global.TextEncode(c2.Description)}\t{c2.Default}\t{c2.Enabled}\t{Global.TextEncode(c2.Value)}\t{Global.TextEncode(c2.UserId)}\t{c2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.CreationUser?.Username)}\t{c2.CreationUserId}\t{c2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.LastWriteUser?.Username)}\t{c2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
