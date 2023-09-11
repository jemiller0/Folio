using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ReferenceData2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ReferenceData2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ReferenceData2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Label", "label" }, { "Value", "value" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Label", "label"),
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Value", "value")
            }.Where(s => s != null)));
            ReferenceData2sRadGrid.DataSource = folioServiceContext.ReferenceData2s(out var i, where, ReferenceData2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ReferenceData2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ReferenceData2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ReferenceData2sRadGrid.PageSize * ReferenceData2sRadGrid.CurrentPageIndex, ReferenceData2sRadGrid.PageSize, true);
            ReferenceData2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ReferenceData2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tLabel\tValue\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Label", "label" }, { "Value", "value" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Label", "label"),
                Global.GetCqlFilter(ReferenceData2sRadGrid, "Value", "value")
            }.Where(s => s != null)));
            foreach (var rd2 in folioServiceContext.ReferenceData2s(where, ReferenceData2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ReferenceData2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ReferenceData2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{rd2.Id}\t{Global.TextEncode(rd2.Label)}\t{Global.TextEncode(rd2.Value)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
