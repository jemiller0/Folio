using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LedgerRolloverError2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LedgerRolloverError2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LedgerRolloverError2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LedgerRolloverError2sRadGrid.DataSource = folioServiceContext.LedgerRolloverError2s(out var i, Global.GetCqlFilter(LedgerRolloverError2sRadGrid, d), LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LedgerRolloverError2sRadGrid.PageSize * LedgerRolloverError2sRadGrid.CurrentPageIndex, LedgerRolloverError2sRadGrid.PageSize, true);
            LedgerRolloverError2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LedgerRolloverError2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LedgerRolloverId", "ledgerRolloverId" }, { "ErrorType", "errorType" }, { "FailedAction", "failedAction" }, { "ErrorMessage", "errorMessage" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tLedgerRollover\tLedgerRolloverId\tErrorType\tFailedAction\tErrorMessage\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var lre2 in folioServiceContext.LedgerRolloverError2s(Global.GetCqlFilter(LedgerRolloverError2sRadGrid, d), LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LedgerRolloverError2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lre2.Id}\t{lre2.LedgerRollover?.Id}\t{lre2.LedgerRolloverId}\t{Global.TextEncode(lre2.ErrorType)}\t{Global.TextEncode(lre2.FailedAction)}\t{Global.TextEncode(lre2.ErrorMessage)}\t{lre2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lre2.CreationUser?.Username)}\t{lre2.CreationUserId}\t{lre2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lre2.LastWriteUser?.Username)}\t{lre2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
