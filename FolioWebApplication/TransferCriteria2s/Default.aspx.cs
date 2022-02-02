using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.TransferCriteria2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TransferCriteria2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void TransferCriteria2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Criteria", "criteria" }, { "Type", "type" }, { "Value", "value" }, { "Interval", "interval" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(TransferCriteria2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(TransferCriteria2sRadGrid, "Criteria", "criteria"),
                Global.GetCqlFilter(TransferCriteria2sRadGrid, "Type", "type"),
                Global.GetCqlFilter(TransferCriteria2sRadGrid, "Value", "value"),
                Global.GetCqlFilter(TransferCriteria2sRadGrid, "Interval", "interval")
            }.Where(s => s != null)));
            TransferCriteria2sRadGrid.DataSource = folioServiceContext.TransferCriteria2s(out var i, where, TransferCriteria2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferCriteria2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferCriteria2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, TransferCriteria2sRadGrid.PageSize * TransferCriteria2sRadGrid.CurrentPageIndex, TransferCriteria2sRadGrid.PageSize, true);
            TransferCriteria2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"TransferCriteria2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Criteria", "criteria" }, { "Type", "type" }, { "Value", "value" }, { "Interval", "interval" } };
            Response.Write("Id\tCriteria\tType\tValue\tInterval\r\n");
            foreach (var tc2 in folioServiceContext.TransferCriteria2s(Global.GetCqlFilter(TransferCriteria2sRadGrid, d), TransferCriteria2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferCriteria2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferCriteria2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{tc2.Id}\t{Global.TextEncode(tc2.Criteria)}\t{Global.TextEncode(tc2.Type)}\t{tc2.Value}\t{Global.TextEncode(tc2.Interval)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
