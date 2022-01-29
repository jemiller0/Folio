using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Block2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Block2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Block2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Type", "type" }, { "Description", "desc" }, { "Code", "code" }, { "StaffInformation", "staffInformation" }, { "PatronMessage", "patronMessage" }, { "ExpirationDate", "expirationDate" }, { "Borrowing", "borrowing" }, { "Renewals", "renewals" }, { "Requests", "requests" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Block2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Block2sRadGrid, "Type", "type"),
                Global.GetCqlFilter(Block2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(Block2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Block2sRadGrid, "StaffInformation", "staffInformation"),
                Global.GetCqlFilter(Block2sRadGrid, "PatronMessage", "patronMessage"),
                Global.GetCqlFilter(Block2sRadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Block2sRadGrid, "Borrowing", "borrowing"),
                Global.GetCqlFilter(Block2sRadGrid, "Renewals", "renewals"),
                Global.GetCqlFilter(Block2sRadGrid, "Requests", "requests"),
                Global.GetCqlFilter(Block2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Block2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Block2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Block2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Block2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Block2sRadGrid.DataSource = folioServiceContext.Block2s(out var i, where, Block2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Block2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Block2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Block2sRadGrid.PageSize * Block2sRadGrid.CurrentPageIndex, Block2sRadGrid.PageSize, true);
            Block2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Block2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Type", "type" }, { "Description", "desc" }, { "Code", "code" }, { "StaffInformation", "staffInformation" }, { "PatronMessage", "patronMessage" }, { "ExpirationDate", "expirationDate" }, { "Borrowing", "borrowing" }, { "Renewals", "renewals" }, { "Requests", "requests" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tType\tDescription\tCode\tStaffInformation\tPatronMessage\tExpirationDate\tBorrowing\tRenewals\tRequests\tUser\tUserId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var b2 in folioServiceContext.Block2s(Global.GetCqlFilter(Block2sRadGrid, d), Block2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Block2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Block2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{b2.Id}\t{Global.TextEncode(b2.Type)}\t{Global.TextEncode(b2.Description)}\t{Global.TextEncode(b2.Code)}\t{Global.TextEncode(b2.StaffInformation)}\t{Global.TextEncode(b2.PatronMessage)}\t{b2.ExpirationDate:M/d/yyyy}\t{b2.Borrowing}\t{b2.Renewals}\t{b2.Requests}\t{Global.TextEncode(b2.User?.Username)}\t{b2.UserId}\t{b2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.CreationUser?.Username)}\t{b2.CreationUserId}\t{b2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(b2.LastWriteUser?.Username)}\t{b2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
