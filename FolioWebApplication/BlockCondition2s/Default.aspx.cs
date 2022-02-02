using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BlockCondition2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BlockCondition2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BlockCondition2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BlockBorrowing", "blockBorrowing" }, { "BlockRenewals", "blockRenewals" }, { "BlockRequests", "blockRequests" }, { "ValueType", "valueType" }, { "Message", "message" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockBorrowing", "blockBorrowing"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockRenewals", "blockRenewals"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockRequests", "blockRequests"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "ValueType", "valueType"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Message", "message"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BlockCondition2sRadGrid.DataSource = folioServiceContext.BlockCondition2s(out var i, where, BlockCondition2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockCondition2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockCondition2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockCondition2sRadGrid.PageSize * BlockCondition2sRadGrid.CurrentPageIndex, BlockCondition2sRadGrid.PageSize, true);
            BlockCondition2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"BlockCondition2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tBlockBorrowing\tBlockRenewals\tBlockRequests\tValueType\tMessage\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "BlockBorrowing", "blockBorrowing" }, { "BlockRenewals", "blockRenewals" }, { "BlockRequests", "blockRequests" }, { "ValueType", "valueType" }, { "Message", "message" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockBorrowing", "blockBorrowing"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockRenewals", "blockRenewals"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "BlockRequests", "blockRequests"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "ValueType", "valueType"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "Message", "message"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BlockCondition2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var bc2 in folioServiceContext.BlockCondition2s(where, BlockCondition2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockCondition2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockCondition2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{bc2.Id}\t{Global.TextEncode(bc2.Name)}\t{bc2.BlockBorrowing}\t{bc2.BlockRenewals}\t{bc2.BlockRequests}\t{Global.TextEncode(bc2.ValueType)}\t{Global.TextEncode(bc2.Message)}\t{bc2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bc2.CreationUser?.Username)}\t{bc2.CreationUserId}\t{bc2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(bc2.LastWriteUser?.Username)}\t{bc2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
