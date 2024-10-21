using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.RolloverError2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RolloverError2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void RolloverError2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RolloverError2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "Rollover.Id", "ledgerRolloverId", "id", folioServiceContext.FolioServiceClient.Rollovers),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorType", "errorType"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "FailedAction", "failedAction"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorMessage", "errorMessage"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            RolloverError2sRadGrid.DataSource = folioServiceContext.RolloverError2s(out var i, where, RolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, RolloverError2sRadGrid.PageSize * RolloverError2sRadGrid.CurrentPageIndex, RolloverError2sRadGrid.PageSize, true);
            RolloverError2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"RolloverError2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tRollover\tRolloverId\tErrorType\tFailedAction\tErrorMessage\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(RolloverError2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "Rollover.Id", "ledgerRolloverId", "id", folioServiceContext.FolioServiceClient.Rollovers),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorType", "errorType"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "FailedAction", "failedAction"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "ErrorMessage", "errorMessage"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(RolloverError2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var re2 in folioServiceContext.RolloverError2s(where, RolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[RolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(RolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{re2.Id}\t{re2.Rollover?.Id}\t{re2.RolloverId}\t{Global.TextEncode(re2.ErrorType)}\t{Global.TextEncode(re2.FailedAction)}\t{Global.TextEncode(re2.ErrorMessage)}\t{re2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(re2.CreationUser?.Username)}\t{re2.CreationUserId}\t{re2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(re2.LastWriteUser?.Username)}\t{re2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
