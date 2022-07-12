using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AcquisitionMethod2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AcquisitionMethod2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AcquisitionMethod2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "value" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "Name", "value"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            AcquisitionMethod2sRadGrid.DataSource = folioServiceContext.AcquisitionMethod2s(out var i, where, AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, AcquisitionMethod2sRadGrid.PageSize * AcquisitionMethod2sRadGrid.CurrentPageIndex, AcquisitionMethod2sRadGrid.PageSize, true);
            AcquisitionMethod2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"AcquisitionMethod2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "value" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "Name", "value"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(AcquisitionMethod2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var am2 in folioServiceContext.AcquisitionMethod2s(where, AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AcquisitionMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{am2.Id}\t{Global.TextEncode(am2.Name)}\t{Global.TextEncode(am2.Source)}\t{am2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(am2.CreationUser?.Username)}\t{am2.CreationUserId}\t{am2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(am2.LastWriteUser?.Username)}\t{am2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
