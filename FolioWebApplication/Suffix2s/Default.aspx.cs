using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Suffix2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Suffix2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Suffix2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Suffix2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Suffix2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Suffix2sRadGrid, "Description", "description")
            }.Where(s => s != null)));
            Suffix2sRadGrid.DataSource = folioServiceContext.Suffix2s(where, Suffix2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Suffix2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Suffix2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Suffix2sRadGrid.PageSize * Suffix2sRadGrid.CurrentPageIndex, Suffix2sRadGrid.PageSize, true);
            Suffix2sRadGrid.VirtualItemCount = folioServiceContext.CountSuffix2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Suffix2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Suffix2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Suffix2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Suffix2sRadGrid, "Description", "description")
            }.Where(s => s != null)));
            foreach (var s2 in folioServiceContext.Suffix2s(where, Suffix2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Suffix2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Suffix2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{s2.Id}\t{Global.TextEncode(s2.Name)}\t{Global.TextEncode(s2.Description)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
