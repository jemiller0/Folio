using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AgreementItem2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AgreementItem2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AgreementItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DateCreated", "dateCreated" }, { "LastUpdated", "lastUpdated" }, { "AgreementId", "owner.id" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "DateCreated", "dateCreated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "LastUpdated", "lastUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "AgreementId", "owner.id")
            }.Where(s => s != null)));
            AgreementItem2sRadGrid.DataSource = folioServiceContext.AgreementItem2s(out var i, where, AgreementItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, AgreementItem2sRadGrid.PageSize * AgreementItem2sRadGrid.CurrentPageIndex, AgreementItem2sRadGrid.PageSize, true);
            AgreementItem2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"AgreementItem2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tDateCreated\tLastUpdated\tAgreementId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DateCreated", "dateCreated" }, { "LastUpdated", "lastUpdated" }, { "AgreementId", "owner.id" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "DateCreated", "dateCreated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "LastUpdated", "lastUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "AgreementId", "owner.id")
            }.Where(s => s != null)));
            foreach (var ai2 in folioServiceContext.AgreementItem2s(where, AgreementItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ai2.Id}\t{ai2.DateCreated:M/d/yyyy}\t{ai2.LastUpdated:M/d/yyyy}\t{ai2.AgreementId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
