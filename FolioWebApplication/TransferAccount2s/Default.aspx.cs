using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.TransferAccount2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TransferAccount2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void TransferAccount2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "accountName" }, { "Description", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Name", "accountName"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners)
            }.Where(s => s != null)));
            TransferAccount2sRadGrid.DataSource = folioServiceContext.TransferAccount2s(out var i, where, TransferAccount2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, TransferAccount2sRadGrid.PageSize * TransferAccount2sRadGrid.CurrentPageIndex, TransferAccount2sRadGrid.PageSize, true);
            TransferAccount2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"TransferAccount2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tOwner\tOwnerId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "accountName" }, { "Description", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Name", "accountName"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners)
            }.Where(s => s != null)));
            foreach (var ta2 in folioServiceContext.TransferAccount2s(where, TransferAccount2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ta2.Id}\t{Global.TextEncode(ta2.Name)}\t{Global.TextEncode(ta2.Description)}\t{ta2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ta2.CreationUser?.Username)}\t{ta2.CreationUserId}\t{ta2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ta2.LastWriteUser?.Username)}\t{ta2.LastWriteUserId}\t{Global.TextEncode(ta2.Owner?.Name)}\t{ta2.OwnerId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
