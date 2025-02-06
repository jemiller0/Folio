using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Permission2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Permission2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Permission2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void Permission2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "permissionName" }, { "Name", "displayName" }, { "Description", "description" }, { "Editable", "mutable" }, { "Visible", "visible" }, { "Dummy", "dummy" }, { "Deprecated", "deprecated" }, { "ModuleName", "moduleName" }, { "ModuleVersion", "moduleVersion" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Permission2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Permission2sRadGrid, "Code", "permissionName"),
                Global.GetCqlFilter(Permission2sRadGrid, "Name", "displayName"),
                Global.GetCqlFilter(Permission2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Permission2sRadGrid, "Editable", "mutable"),
                Global.GetCqlFilter(Permission2sRadGrid, "Visible", "visible"),
                Global.GetCqlFilter(Permission2sRadGrid, "Dummy", "dummy"),
                Global.GetCqlFilter(Permission2sRadGrid, "Deprecated", "deprecated"),
                Global.GetCqlFilter(Permission2sRadGrid, "ModuleName", "moduleName"),
                Global.GetCqlFilter(Permission2sRadGrid, "ModuleVersion", "moduleVersion"),
                Global.GetCqlFilter(Permission2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Permission2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Permission2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Permission2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Permission2sRadGrid.DataSource = folioServiceContext.Permission2s(where, Permission2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Permission2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Permission2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Permission2sRadGrid.PageSize * Permission2sRadGrid.CurrentPageIndex, Permission2sRadGrid.PageSize, true);
            Permission2sRadGrid.VirtualItemCount = folioServiceContext.CountPermission2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Permission2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tCode\tName\tDescription\tEditable\tVisible\tDummy\tDeprecated\tModuleName\tModuleVersion\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "permissionName" }, { "Name", "displayName" }, { "Description", "description" }, { "Editable", "mutable" }, { "Visible", "visible" }, { "Dummy", "dummy" }, { "Deprecated", "deprecated" }, { "ModuleName", "moduleName" }, { "ModuleVersion", "moduleVersion" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Permission2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Permission2sRadGrid, "Code", "permissionName"),
                Global.GetCqlFilter(Permission2sRadGrid, "Name", "displayName"),
                Global.GetCqlFilter(Permission2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Permission2sRadGrid, "Editable", "mutable"),
                Global.GetCqlFilter(Permission2sRadGrid, "Visible", "visible"),
                Global.GetCqlFilter(Permission2sRadGrid, "Dummy", "dummy"),
                Global.GetCqlFilter(Permission2sRadGrid, "Deprecated", "deprecated"),
                Global.GetCqlFilter(Permission2sRadGrid, "ModuleName", "moduleName"),
                Global.GetCqlFilter(Permission2sRadGrid, "ModuleVersion", "moduleVersion"),
                Global.GetCqlFilter(Permission2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Permission2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Permission2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Permission2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var p2 in folioServiceContext.Permission2s(where, Permission2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Permission2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Permission2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{p2.Id}\t{Global.TextEncode(p2.Code)}\t{Global.TextEncode(p2.Name)}\t{Global.TextEncode(p2.Description)}\t{p2.Editable}\t{p2.Visible}\t{p2.Dummy}\t{p2.Deprecated}\t{Global.TextEncode(p2.ModuleName)}\t{Global.TextEncode(p2.ModuleVersion)}\t{p2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p2.CreationUser?.Username)}\t{p2.CreationUserId}\t{p2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p2.LastWriteUser?.Username)}\t{p2.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void Permission2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                folioServiceContext.DeletePermission2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)gei.FindControl("DeleteCustomValidator");
                cv.IsValid = false;
                e.Canceled = true;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
