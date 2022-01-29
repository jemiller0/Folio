using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FinanceGroup2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FinanceGroup2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FinanceGroup2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "Name", "name" }, { "Status", "status" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FinanceGroup2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FinanceGroup2sRadGrid.DataSource = folioServiceContext.FinanceGroup2s(out var i, where, FinanceGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FinanceGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FinanceGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FinanceGroup2sRadGrid.PageSize * FinanceGroup2sRadGrid.CurrentPageIndex, FinanceGroup2sRadGrid.PageSize, true);
            FinanceGroup2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"FinanceGroup2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "Name", "name" }, { "Status", "status" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tCode\tDescription\tName\tStatus\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var fg2 in folioServiceContext.FinanceGroup2s(Global.GetCqlFilter(FinanceGroup2sRadGrid, d), FinanceGroup2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FinanceGroup2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FinanceGroup2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{fg2.Id}\t{Global.TextEncode(fg2.Code)}\t{Global.TextEncode(fg2.Description)}\t{Global.TextEncode(fg2.Name)}\t{Global.TextEncode(fg2.Status)}\t{fg2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fg2.CreationUser?.Username)}\t{fg2.CreationUserId}\t{fg2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fg2.LastWriteUser?.Username)}\t{fg2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
