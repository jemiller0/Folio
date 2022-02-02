using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FundType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FundType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FundType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(FundType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(FundType2sRadGrid, "Name", "name")
            }.Where(s => s != null)));
            FundType2sRadGrid.DataSource = folioServiceContext.FundType2s(out var i, where, FundType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FundType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FundType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FundType2sRadGrid.PageSize * FundType2sRadGrid.CurrentPageIndex, FundType2sRadGrid.PageSize, true);
            FundType2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"FundType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" } };
            Response.Write("Id\tName\r\n");
            foreach (var ft2 in folioServiceContext.FundType2s(Global.GetCqlFilter(FundType2sRadGrid, d), FundType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FundType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FundType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ft2.Id}\t{Global.TextEncode(ft2.Name)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
