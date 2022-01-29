using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ReportingCode2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ReportingCode2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ReportingCode2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ReportingCode2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ReportingCode2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(ReportingCode2sRadGrid, "Description", "description")
            }.Where(s => s != null)));
            ReportingCode2sRadGrid.DataSource = folioServiceContext.ReportingCode2s(out var i, where, ReportingCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ReportingCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ReportingCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ReportingCode2sRadGrid.PageSize * ReportingCode2sRadGrid.CurrentPageIndex, ReportingCode2sRadGrid.PageSize, true);
            ReportingCode2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ReportingCode2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" } };
            Response.Write("Id\tCode\tDescription\r\n");
            foreach (var rc2 in folioServiceContext.ReportingCode2s(Global.GetCqlFilter(ReportingCode2sRadGrid, d), ReportingCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ReportingCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ReportingCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rc2.Id}\t{Global.TextEncode(rc2.Code)}\t{Global.TextEncode(rc2.Description)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
