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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuppressFromDiscovery", "suppressFromDiscovery" }, { "Note", "note" }, { "Description", "description" }, { "CustomCoverage", "customCoverage" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "ActiveFromDate", "activeFrom" }, { "ActiveToDate", "activeTo" }, { "ContentLastWriteTime", "contentUpdated" }, { "HaveAccess", "haveAccess" }, { "CreationTime", "dateCreated" }, { "LastWriteTime", "lastUpdated" }, { "AgreementId", "owner.id" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "SuppressFromDiscovery", "suppressFromDiscovery"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Note", "note"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "CustomCoverage", "customCoverage"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ActiveFromDate", "activeFrom"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ActiveToDate", "activeTo"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ContentLastWriteTime", "contentUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "HaveAccess", "haveAccess"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "CreationTime", "dateCreated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "LastWriteTime", "lastUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Agreement.Name", "owner.id", "name", folioServiceContext.FolioServiceClient.Agreements)
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
            Response.Write("Id\tSuppressFromDiscovery\tNote\tDescription\tCustomCoverage\tStartDate\tEndDate\tActiveFromDate\tActiveToDate\tContentLastWriteTime\tHaveAccess\tCreationTime\tLastWriteTime\tAgreement\tAgreementId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SuppressFromDiscovery", "suppressFromDiscovery" }, { "Note", "note" }, { "Description", "description" }, { "CustomCoverage", "customCoverage" }, { "StartDate", "startDate" }, { "EndDate", "endDate" }, { "ActiveFromDate", "activeFrom" }, { "ActiveToDate", "activeTo" }, { "ContentLastWriteTime", "contentUpdated" }, { "HaveAccess", "haveAccess" }, { "CreationTime", "dateCreated" }, { "LastWriteTime", "lastUpdated" }, { "AgreementId", "owner.id" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "SuppressFromDiscovery", "suppressFromDiscovery"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Note", "note"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "CustomCoverage", "customCoverage"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "StartDate", "startDate"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "EndDate", "endDate"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ActiveFromDate", "activeFrom"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ActiveToDate", "activeTo"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "ContentLastWriteTime", "contentUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "HaveAccess", "haveAccess"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "CreationTime", "dateCreated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "LastWriteTime", "lastUpdated"),
                Global.GetCqlFilter(AgreementItem2sRadGrid, "Agreement.Name", "owner.id", "name", folioServiceContext.FolioServiceClient.Agreements)
            }.Where(s => s != null)));
            foreach (var ai2 in folioServiceContext.AgreementItem2s(where, AgreementItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AgreementItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ai2.Id}\t{ai2.SuppressFromDiscovery}\t{Global.TextEncode(ai2.Note)}\t{Global.TextEncode(ai2.Description)}\t{ai2.CustomCoverage}\t{ai2.StartDate:M/d/yyyy}\t{ai2.EndDate:M/d/yyyy}\t{ai2.ActiveFromDate:M/d/yyyy}\t{ai2.ActiveToDate:M/d/yyyy}\t{ai2.ContentLastWriteTime:M/d/yyyy HH:mm:ss}\t{ai2.HaveAccess}\t{ai2.CreationTime:M/d/yyyy HH:mm:ss}\t{ai2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ai2.Agreement?.Name)}\t{ai2.AgreementId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
