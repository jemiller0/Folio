using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Agreement2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Agreement2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Agreement2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "CancellationDeadline", "cancellationDeadline" }, { "DateCreated", "dateCreated" }, { "LastUpdated", "lastUpdated" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Agreement2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Agreement2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CancellationDeadline", "cancellationDeadline"),
                Global.GetCqlFilter(Agreement2sRadGrid, "DateCreated", "dateCreated"),
                Global.GetCqlFilter(Agreement2sRadGrid, "LastUpdated", "lastUpdated")
            }.Where(s => s != null)));
            Agreement2sRadGrid.DataSource = folioServiceContext.Agreement2s(out var i, where, Agreement2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Agreement2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Agreement2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Agreement2sRadGrid.PageSize * Agreement2sRadGrid.CurrentPageIndex, Agreement2sRadGrid.PageSize, true);
            Agreement2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Agreement2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tStartDate\tEndDate\tCancellationDeadline\tDateCreated\tLastUpdated\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "CancellationDeadline", "cancellationDeadline" }, { "DateCreated", "dateCreated" }, { "LastUpdated", "lastUpdated" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Agreement2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Agreement2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CancellationDeadline", "cancellationDeadline"),
                Global.GetCqlFilter(Agreement2sRadGrid, "DateCreated", "dateCreated"),
                Global.GetCqlFilter(Agreement2sRadGrid, "LastUpdated", "lastUpdated")
            }.Where(s => s != null)));
            foreach (var a2 in folioServiceContext.Agreement2s(where, Agreement2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Agreement2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Agreement2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{a2.Id}\t{Global.TextEncode(a2.Name)}\t{Global.TextEncode(a2.Description)}\t{a2.StartDate:M/d/yyyy}\t{a2.EndDate:M/d/yyyy}\t{a2.CancellationDeadline:M/d/yyyy}\t{a2.DateCreated:M/d/yyyy}\t{a2.LastUpdated:M/d/yyyy}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
