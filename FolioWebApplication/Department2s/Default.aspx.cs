using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Department2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Department2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Department2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "UsageNumber", "usageNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Department2sRadGrid.DataSource = folioServiceContext.Department2s(out var i, Global.GetCqlFilter(Department2sRadGrid, d), Department2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Department2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Department2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Department2sRadGrid.PageSize * Department2sRadGrid.CurrentPageIndex, Department2sRadGrid.PageSize, true);
            Department2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Department2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "UsageNumber", "usageNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tUsageNumber\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var d2 in folioServiceContext.Department2s(Global.GetCqlFilter(Department2sRadGrid, d), Department2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Department2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Department2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{d2.Id}\t{Global.TextEncode(d2.Name)}\t{Global.TextEncode(d2.Code)}\t{d2.UsageNumber}\t{d2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(d2.CreationUser?.Username)}\t{d2.CreationUserId}\t{d2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(d2.LastWriteUser?.Username)}\t{d2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
