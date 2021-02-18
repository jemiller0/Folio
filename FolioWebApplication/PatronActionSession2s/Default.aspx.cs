using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.PatronActionSession2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PatronActionSession2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PatronActionSession2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PatronId", "patronId" }, { "LoanId", "loanId" }, { "ActionType", "actionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PatronActionSession2sRadGrid.DataSource = folioServiceContext.PatronActionSession2s(out var i, Global.GetCqlFilter(PatronActionSession2sRadGrid, d), PatronActionSession2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PatronActionSession2sRadGrid.PageSize * PatronActionSession2sRadGrid.CurrentPageIndex, PatronActionSession2sRadGrid.PageSize, true);
            PatronActionSession2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"PatronActionSession2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PatronId", "patronId" }, { "LoanId", "loanId" }, { "ActionType", "actionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tPatron\tPatronId\tLoan\tLoanId\tActionType\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var pas2 in folioServiceContext.PatronActionSession2s(Global.GetCqlFilter(PatronActionSession2sRadGrid, d), PatronActionSession2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{pas2.Id}\t{Global.TextEncode(pas2.Patron?.Username)}\t{pas2.PatronId}\t{pas2.Loan?.Id}\t{pas2.LoanId}\t{Global.TextEncode(pas2.ActionType)}\t{pas2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pas2.CreationUser?.Username)}\t{pas2.CreationUserId}\t{pas2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(pas2.LastWriteUser?.Username)}\t{pas2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
