using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LoanType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoanType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LoanType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LoanType2sRadGrid.DataSource = folioServiceContext.LoanType2s(out var i, Global.GetCqlFilter(LoanType2sRadGrid, d), LoanType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanType2sRadGrid.PageSize * LoanType2sRadGrid.CurrentPageIndex, LoanType2sRadGrid.PageSize, true);
            LoanType2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LoanType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var lt2 in folioServiceContext.LoanType2s(Global.GetCqlFilter(LoanType2sRadGrid, d), LoanType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lt2.Id}\t{Global.TextEncode(lt2.Name)}\t{lt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lt2.CreationUser?.Username)}\t{lt2.CreationUserId}\t{lt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lt2.LastWriteUser?.Username)}\t{lt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
