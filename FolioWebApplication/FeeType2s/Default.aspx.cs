using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FeeType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FeeType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FeeType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(FeeType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Automatic", "automatic"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Name", "feeFineType"),
                Global.GetCqlFilter(FeeType2sRadGrid, "DefaultAmount", "defaultAmount"),
                Global.GetCqlFilter(FeeType2sRadGrid, "ChargeNotice.Name", "chargeNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2sRadGrid, "ActionNotice.Name", "actionNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FeeType2sRadGrid.DataSource = folioServiceContext.FeeType2s(out var i, where, FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2sRadGrid.PageSize * FeeType2sRadGrid.CurrentPageIndex, FeeType2sRadGrid.PageSize, true);
            FeeType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"FeeType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tAutomatic\tName\tDefaultAmount\tChargeNotice\tChargeNoticeId\tActionNotice\tActionNoticeId\tOwner\tOwnerId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ft2 in folioServiceContext.FeeType2s(Global.GetCqlFilter(FeeType2sRadGrid, d), FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ft2.Id}\t{ft2.Automatic}\t{Global.TextEncode(ft2.Name)}\t{ft2.DefaultAmount}\t{Global.TextEncode(ft2.ChargeNotice?.Name)}\t{ft2.ChargeNoticeId}\t{Global.TextEncode(ft2.ActionNotice?.Name)}\t{ft2.ActionNoticeId}\t{Global.TextEncode(ft2.Owner?.Name)}\t{ft2.OwnerId}\t{ft2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ft2.CreationUser?.Username)}\t{ft2.CreationUserId}\t{ft2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ft2.LastWriteUser?.Username)}\t{ft2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
