using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CloseReason2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CloseReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CloseReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "reason" }, { "Source", "source" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(CloseReason2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CloseReason2sRadGrid, "Name", "reason"),
                Global.GetCqlFilter(CloseReason2sRadGrid, "Source", "source")
            }.Where(s => s != null)));
            CloseReason2sRadGrid.DataSource = folioServiceContext.CloseReason2s(out var i, where, CloseReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CloseReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CloseReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CloseReason2sRadGrid.PageSize * CloseReason2sRadGrid.CurrentPageIndex, CloseReason2sRadGrid.PageSize, true);
            CloseReason2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"CloseReason2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "reason" }, { "Source", "source" } };
            Response.Write("Id\tName\tSource\r\n");
            foreach (var cr2 in folioServiceContext.CloseReason2s(Global.GetCqlFilter(CloseReason2sRadGrid, d), CloseReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CloseReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CloseReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{cr2.Id}\t{Global.TextEncode(cr2.Name)}\t{Global.TextEncode(cr2.Source)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
