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
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "CancellationDeadlineDate", "cancellationDeadline" }, { "Status", "agreementStatus.label" }, { "IsPerpetual", "isPerpetual.label" }, { "RenewalPriority", "renewalPriority.label" }, { "Description", "description" }, { "CreationTime", "dateCreated" }, { "LastWriteTime", "lastUpdated" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Agreement2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Agreement2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CancellationDeadlineDate", "cancellationDeadline"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Status", "agreementStatus.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "IsPerpetual", "isPerpetual.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "RenewalPriority", "renewalPriority.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CreationTime", "dateCreated"),
                Global.GetCqlFilter(Agreement2sRadGrid, "LastWriteTime", "lastUpdated")
            }.Where(s => s != null)));
            Agreement2sRadGrid.DataSource = folioServiceContext.Agreement2s(where, Agreement2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Agreement2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Agreement2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Agreement2sRadGrid.PageSize * Agreement2sRadGrid.CurrentPageIndex, Agreement2sRadGrid.PageSize, true);
            Agreement2sRadGrid.VirtualItemCount = folioServiceContext.CountAgreement2s(where);
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
            Response.Write("Id\tName\tStartDate\tEndDate\tCancellationDeadlineDate\tStatus\tIsPerpetual\tRenewalPriority\tDescription\tCreationTime\tLastWriteTime\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "CancellationDeadlineDate", "cancellationDeadline" }, { "Status", "agreementStatus.label" }, { "IsPerpetual", "isPerpetual.label" }, { "RenewalPriority", "renewalPriority.label" }, { "Description", "description" }, { "CreationTime", "dateCreated" }, { "LastWriteTime", "lastUpdated" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Agreement2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Agreement2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CancellationDeadlineDate", "cancellationDeadline"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Status", "agreementStatus.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "IsPerpetual", "isPerpetual.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "RenewalPriority", "renewalPriority.label"),
                Global.GetCqlFilter(Agreement2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Agreement2sRadGrid, "CreationTime", "dateCreated"),
                Global.GetCqlFilter(Agreement2sRadGrid, "LastWriteTime", "lastUpdated")
            }.Where(s => s != null)));
            foreach (var a2 in folioServiceContext.Agreement2s(where, Agreement2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Agreement2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Agreement2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{a2.Id}\t{Global.TextEncode(a2.Name)}\t{a2.StartDate:M/d/yyyy}\t{a2.EndDate:M/d/yyyy}\t{a2.CancellationDeadlineDate:M/d/yyyy}\t{Global.TextEncode(a2.Status)}\t{Global.TextEncode(a2.IsPerpetual)}\t{Global.TextEncode(a2.RenewalPriority)}\t{Global.TextEncode(a2.Description)}\t{a2.CreationTime:M/d/yyyy HH:mm:ss}\t{a2.LastWriteTime:M/d/yyyy HH:mm:ss}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
