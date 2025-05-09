using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Contact2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Contact2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Contact2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Prefix", "prefix" }, { "FirstName", "firstName" }, { "LastName", "lastName" }, { "Language", "language" }, { "Notes", "notes" }, { "Inactive", "inactive" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Contact2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Contact2sRadGrid, "Name", ""),
                Global.GetCqlFilter(Contact2sRadGrid, "Prefix", "prefix"),
                Global.GetCqlFilter(Contact2sRadGrid, "FirstName", "firstName"),
                Global.GetCqlFilter(Contact2sRadGrid, "LastName", "lastName"),
                Global.GetCqlFilter(Contact2sRadGrid, "Language", "language"),
                Global.GetCqlFilter(Contact2sRadGrid, "Notes", "notes"),
                Global.GetCqlFilter(Contact2sRadGrid, "Inactive", "inactive"),
                Global.GetCqlFilter(Contact2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Contact2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Contact2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Contact2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Contact2sRadGrid.DataSource = folioServiceContext.Contact2s(where, Contact2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Contact2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Contact2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Contact2sRadGrid.PageSize * Contact2sRadGrid.CurrentPageIndex, Contact2sRadGrid.PageSize, true);
            Contact2sRadGrid.VirtualItemCount = folioServiceContext.CountContact2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Contact2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tPrefix\tFirstName\tLastName\tLanguage\tNotes\tInactive\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Prefix", "prefix" }, { "FirstName", "firstName" }, { "LastName", "lastName" }, { "Language", "language" }, { "Notes", "notes" }, { "Inactive", "inactive" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Contact2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Contact2sRadGrid, "Name", ""),
                Global.GetCqlFilter(Contact2sRadGrid, "Prefix", "prefix"),
                Global.GetCqlFilter(Contact2sRadGrid, "FirstName", "firstName"),
                Global.GetCqlFilter(Contact2sRadGrid, "LastName", "lastName"),
                Global.GetCqlFilter(Contact2sRadGrid, "Language", "language"),
                Global.GetCqlFilter(Contact2sRadGrid, "Notes", "notes"),
                Global.GetCqlFilter(Contact2sRadGrid, "Inactive", "inactive"),
                Global.GetCqlFilter(Contact2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Contact2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Contact2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Contact2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var c2 in folioServiceContext.Contact2s(where, Contact2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Contact2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Contact2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{c2.Id}\t{Global.TextEncode(c2.Name)}\t{Global.TextEncode(c2.Prefix)}\t{Global.TextEncode(c2.FirstName)}\t{Global.TextEncode(c2.LastName)}\t{Global.TextEncode(c2.Language)}\t{Global.TextEncode(c2.Notes)}\t{c2.Inactive}\t{c2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.CreationUser?.Username)}\t{c2.CreationUserId}\t{c2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.LastWriteUser?.Username)}\t{c2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
