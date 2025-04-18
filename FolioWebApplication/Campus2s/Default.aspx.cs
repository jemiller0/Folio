using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Campus2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Campus2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Campus2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "InstitutionId", "institutionId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Campus2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Campus2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Campus2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Campus2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Campus2sRadGrid.DataSource = folioServiceContext.Campus2s(where, Campus2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Campus2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Campus2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Campus2sRadGrid.PageSize * Campus2sRadGrid.CurrentPageIndex, Campus2sRadGrid.PageSize, true);
            Campus2sRadGrid.VirtualItemCount = folioServiceContext.CountCampus2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Campus2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCode\tInstitution\tInstitutionId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "InstitutionId", "institutionId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Campus2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Campus2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Campus2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Campus2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Campus2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var c2 in folioServiceContext.Campus2s(where, Campus2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Campus2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Campus2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{c2.Id}\t{Global.TextEncode(c2.Name)}\t{Global.TextEncode(c2.Code)}\t{Global.TextEncode(c2.Institution?.Name)}\t{c2.InstitutionId}\t{c2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.CreationUser?.Username)}\t{c2.CreationUserId}\t{c2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(c2.LastWriteUser?.Username)}\t{c2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
