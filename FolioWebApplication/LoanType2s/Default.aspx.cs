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
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(LoanType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LoanType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(LoanType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(LoanType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LoanType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LoanType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LoanType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LoanType2sRadGrid.DataSource = folioServiceContext.LoanType2s(where, LoanType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanType2sRadGrid.PageSize * LoanType2sRadGrid.CurrentPageIndex, LoanType2sRadGrid.PageSize, true);
            LoanType2sRadGrid.VirtualItemCount = folioServiceContext.CountLoanType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LoanType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(LoanType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LoanType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(LoanType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(LoanType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LoanType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LoanType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LoanType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var lt2 in folioServiceContext.LoanType2s(where, LoanType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lt2.Id}\t{Global.TextEncode(lt2.Name)}\t{Global.TextEncode(lt2.Source)}\t{lt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lt2.CreationUser?.Username)}\t{lt2.CreationUserId}\t{lt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lt2.LastWriteUser?.Username)}\t{lt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
