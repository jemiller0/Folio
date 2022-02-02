using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Alert2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Alert2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Alert2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Alert", "alert" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Alert2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Alert2sRadGrid, "Alert", "alert")
            }.Where(s => s != null)));
            Alert2sRadGrid.DataSource = folioServiceContext.Alert2s(out var i, where, Alert2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Alert2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Alert2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Alert2sRadGrid.PageSize * Alert2sRadGrid.CurrentPageIndex, Alert2sRadGrid.PageSize, true);
            Alert2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Alert2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tAlert\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Alert", "alert" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Alert2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Alert2sRadGrid, "Alert", "alert")
            }.Where(s => s != null)));
            foreach (var a2 in folioServiceContext.Alert2s(where, Alert2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Alert2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Alert2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{a2.Id}\t{Global.TextEncode(a2.Alert)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
