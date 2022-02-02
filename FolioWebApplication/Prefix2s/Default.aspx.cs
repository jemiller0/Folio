using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Prefix2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Prefix2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Prefix2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Prefix2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Prefix2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Prefix2sRadGrid, "Description", "description")
            }.Where(s => s != null)));
            Prefix2sRadGrid.DataSource = folioServiceContext.Prefix2s(out var i, where, Prefix2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Prefix2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Prefix2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Prefix2sRadGrid.PageSize * Prefix2sRadGrid.CurrentPageIndex, Prefix2sRadGrid.PageSize, true);
            Prefix2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Prefix2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" } };
            Response.Write("Id\tName\tDescription\r\n");
            foreach (var p2 in folioServiceContext.Prefix2s(Global.GetCqlFilter(Prefix2sRadGrid, d), Prefix2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Prefix2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Prefix2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{p2.Id}\t{Global.TextEncode(p2.Name)}\t{Global.TextEncode(p2.Description)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
