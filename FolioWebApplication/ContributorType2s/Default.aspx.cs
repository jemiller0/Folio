using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ContributorType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ContributorType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ContributorType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ContributorType2sRadGrid.DataSource = folioServiceContext.ContributorType2s(out var i, Global.GetCqlFilter(ContributorType2sRadGrid, d), ContributorType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ContributorType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ContributorType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ContributorType2sRadGrid.PageSize * ContributorType2sRadGrid.CurrentPageIndex, ContributorType2sRadGrid.PageSize, true);
            ContributorType2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ContributorType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ct2 in folioServiceContext.ContributorType2s(Global.GetCqlFilter(ContributorType2sRadGrid, d), ContributorType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ContributorType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ContributorType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ct2.Id}\t{Global.TextEncode(ct2.Name)}\t{Global.TextEncode(ct2.Code)}\t{ct2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ct2.CreationUser?.Username)}\t{ct2.CreationUserId}\t{ct2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ct2.LastWriteUser?.Username)}\t{ct2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
