using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CustomField2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomField2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CustomField2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "RefId", "refId" }, { "Type", "type" }, { "EntityType", "entityType" }, { "Visible", "visible" }, { "Required", "required" }, { "IsRepeatable", "isRepeatable" }, { "Order", "order" }, { "HelpText", "helpText" }, { "CheckboxFieldDefault", "checkboxField.default" }, { "SelectFieldMultiSelect", "selectField.multiSelect" }, { "SelectFieldOptionsSortingOrder", "selectField.options.sortingOrder" }, { "TextFieldFieldFormat", "textField.fieldFormat" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CustomField2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CustomField2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(CustomField2sRadGrid, "RefId", "refId"),
                Global.GetCqlFilter(CustomField2sRadGrid, "Type", "type"),
                Global.GetCqlFilter(CustomField2sRadGrid, "EntityType", "entityType"),
                Global.GetCqlFilter(CustomField2sRadGrid, "Visible", "visible"),
                Global.GetCqlFilter(CustomField2sRadGrid, "Required", "required"),
                Global.GetCqlFilter(CustomField2sRadGrid, "IsRepeatable", "isRepeatable"),
                Global.GetCqlFilter(CustomField2sRadGrid, "Order", "order"),
                Global.GetCqlFilter(CustomField2sRadGrid, "HelpText", "helpText"),
                Global.GetCqlFilter(CustomField2sRadGrid, "CheckboxFieldDefault", "checkboxField.default"),
                Global.GetCqlFilter(CustomField2sRadGrid, "SelectFieldMultiSelect", "selectField.multiSelect"),
                Global.GetCqlFilter(CustomField2sRadGrid, "SelectFieldOptionsSortingOrder", "selectField.options.sortingOrder"),
                Global.GetCqlFilter(CustomField2sRadGrid, "TextFieldFieldFormat", "textField.fieldFormat"),
                Global.GetCqlFilter(CustomField2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(CustomField2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(CustomField2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(CustomField2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            CustomField2sRadGrid.DataSource = folioServiceContext.CustomField2s(out var i, where, CustomField2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CustomField2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CustomField2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CustomField2sRadGrid.PageSize * CustomField2sRadGrid.CurrentPageIndex, CustomField2sRadGrid.PageSize, true);
            CustomField2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"CustomField2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "RefId", "refId" }, { "Type", "type" }, { "EntityType", "entityType" }, { "Visible", "visible" }, { "Required", "required" }, { "IsRepeatable", "isRepeatable" }, { "Order", "order" }, { "HelpText", "helpText" }, { "CheckboxFieldDefault", "checkboxField.default" }, { "SelectFieldMultiSelect", "selectField.multiSelect" }, { "SelectFieldOptionsSortingOrder", "selectField.options.sortingOrder" }, { "TextFieldFieldFormat", "textField.fieldFormat" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tRefId\tType\tEntityType\tVisible\tRequired\tIsRepeatable\tOrder\tHelpText\tCheckboxFieldDefault\tSelectFieldMultiSelect\tSelectFieldOptionsSortingOrder\tTextFieldFieldFormat\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var cf2 in folioServiceContext.CustomField2s(Global.GetCqlFilter(CustomField2sRadGrid, d), CustomField2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CustomField2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CustomField2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{cf2.Id}\t{Global.TextEncode(cf2.Name)}\t{Global.TextEncode(cf2.RefId)}\t{Global.TextEncode(cf2.Type)}\t{Global.TextEncode(cf2.EntityType)}\t{cf2.Visible}\t{cf2.Required}\t{cf2.IsRepeatable}\t{cf2.Order}\t{Global.TextEncode(cf2.HelpText)}\t{cf2.CheckboxFieldDefault}\t{cf2.SelectFieldMultiSelect}\t{Global.TextEncode(cf2.SelectFieldOptionsSortingOrder)}\t{Global.TextEncode(cf2.TextFieldFieldFormat)}\t{cf2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cf2.CreationUser?.Username)}\t{cf2.CreationUserId}\t{cf2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cf2.LastWriteUser?.Username)}\t{cf2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
