using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FixedDueDateSchedule2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FixedDueDateSchedule2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FixedDueDateSchedule2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            FixedDueDateSchedule2sRadGrid.DataSource = folioServiceContext.FixedDueDateSchedule2s(out var i, Global.GetCqlFilter(FixedDueDateSchedule2sRadGrid, d), FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FixedDueDateSchedule2sRadGrid.PageSize * FixedDueDateSchedule2sRadGrid.CurrentPageIndex, FixedDueDateSchedule2sRadGrid.PageSize, true);
            FixedDueDateSchedule2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"FixedDueDateSchedule2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var fdds2 in folioServiceContext.FixedDueDateSchedule2s(Global.GetCqlFilter(FixedDueDateSchedule2sRadGrid, d), FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FixedDueDateSchedule2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{fdds2.Id}\t{Global.TextEncode(fdds2.Name)}\t{Global.TextEncode(fdds2.Description)}\t{fdds2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fdds2.CreationUser?.Username)}\t{fdds2.CreationUserId}\t{fdds2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fdds2.LastWriteUser?.Username)}\t{fdds2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
