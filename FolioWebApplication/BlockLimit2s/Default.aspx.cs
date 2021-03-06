using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BlockLimit2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BlockLimit2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BlockLimit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "GroupId", "patronGroupId" }, { "ConditionId", "conditionId" }, { "Value", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BlockLimit2sRadGrid.DataSource = folioServiceContext.BlockLimit2s(out var i, Global.GetCqlFilter(BlockLimit2sRadGrid, d), BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockLimit2sRadGrid.PageSize * BlockLimit2sRadGrid.CurrentPageIndex, BlockLimit2sRadGrid.PageSize, true);
            BlockLimit2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BlockLimit2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "GroupId", "patronGroupId" }, { "ConditionId", "conditionId" }, { "Value", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tGroup\tGroupId\tCondition\tConditionId\tValue\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var bl2 in folioServiceContext.BlockLimit2s(Global.GetCqlFilter(BlockLimit2sRadGrid, d), BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bl2.Id}\t{Global.TextEncode(bl2.Group?.Name)}\t{bl2.GroupId}\t{Global.TextEncode(bl2.Condition?.Name)}\t{bl2.ConditionId}\t{bl2.Value}\t{bl2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bl2.CreationUser?.Username)}\t{bl2.CreationUserId}\t{bl2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bl2.LastWriteUser?.Username)}\t{bl2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
