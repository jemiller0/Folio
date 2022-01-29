using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.StaffSlip2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StaffSlip2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StaffSlip2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Active", "active" }, { "Template", "template" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(StaffSlip2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "Template", "template"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(StaffSlip2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            StaffSlip2sRadGrid.DataSource = folioServiceContext.StaffSlip2s(out var i, where, StaffSlip2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StaffSlip2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StaffSlip2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StaffSlip2sRadGrid.PageSize * StaffSlip2sRadGrid.CurrentPageIndex, StaffSlip2sRadGrid.PageSize, true);
            StaffSlip2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"StaffSlip2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Active", "active" }, { "Template", "template" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tActive\tTemplate\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ss2 in folioServiceContext.StaffSlip2s(Global.GetCqlFilter(StaffSlip2sRadGrid, d), StaffSlip2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StaffSlip2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StaffSlip2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ss2.Id}\t{Global.TextEncode(ss2.Name)}\t{Global.TextEncode(ss2.Description)}\t{ss2.Active}\t{Global.TextEncode(ss2.Template)}\t{ss2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ss2.CreationUser?.Username)}\t{ss2.CreationUserId}\t{ss2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ss2.LastWriteUser?.Username)}\t{ss2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
