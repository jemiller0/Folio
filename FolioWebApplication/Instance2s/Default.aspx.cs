using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Instance2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Instance2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Instance2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors[0].name" }, { "PublicationStartYear", "publicationPeriod.start" }, { "PublicationEndYear", "publicationPeriod.end" }, { "DatesDateTypeId", "dates.dateTypeId" }, { "DatesDate1", "dates.date1" }, { "DatesDate2", "dates.date2" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Instance2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Instance2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Instance2sRadGrid, "MatchKey", "matchKey"),
                Global.GetCqlFilter(Instance2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Instance2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Instance2sRadGrid, "Author", "contributors[0].name"),
                Global.GetCqlFilter(Instance2sRadGrid, "PublicationStartYear", "publicationPeriod.start"),
                Global.GetCqlFilter(Instance2sRadGrid, "PublicationEndYear", "publicationPeriod.end"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDateTypeId", "dates.dateTypeId"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDate1", "dates.date1"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDate2", "dates.date2"),
                Global.GetCqlFilter(Instance2sRadGrid, "InstanceType.Name", "instanceTypeId", "name", folioServiceContext.FolioServiceClient.InstanceTypes),
                Global.GetCqlFilter(Instance2sRadGrid, "IssuanceMode.Name", "modeOfIssuanceId", "name", folioServiceContext.FolioServiceClient.ModeOfIssuances),
                Global.GetCqlFilter(Instance2sRadGrid, "CatalogedDate", "catalogedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "PreviouslyHeld", "previouslyHeld"),
                Global.GetCqlFilter(Instance2sRadGrid, "StaffSuppress", "staffSuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "SourceRecordFormat", "sourceRecordFormat"),
                Global.GetCqlFilter(Instance2sRadGrid, "Status.Name", "statusId", "name", folioServiceContext.FolioServiceClient.InstanceStatuses),
                Global.GetCqlFilter(Instance2sRadGrid, "StatusLastWriteTime", "statusUpdatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "CompletionTime", "")
            }.Where(s => s != null)));
            Instance2sRadGrid.DataSource = folioServiceContext.Instance2s(where, Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Instance2sRadGrid.PageSize * Instance2sRadGrid.CurrentPageIndex, Instance2sRadGrid.PageSize, true);
            Instance2sRadGrid.VirtualItemCount = folioServiceContext.CountInstance2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Instance2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tShortId\tMatchKey\tSource\tTitle\tAuthor\tPublicationStartYear\tPublicationEndYear\tDatesDateTypeId\tDatesDate1\tDatesDate2\tInstanceType\tInstanceTypeId\tIssuanceMode\tIssuanceModeId\tCatalogedDate\tPreviouslyHeld\tStaffSuppress\tDiscoverySuppress\tSourceRecordFormat\tStatus\tStatusId\tStatusLastWriteTime\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tCompletionTime\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors[0].name" }, { "PublicationStartYear", "publicationPeriod.start" }, { "PublicationEndYear", "publicationPeriod.end" }, { "DatesDateTypeId", "dates.dateTypeId" }, { "DatesDate1", "dates.date1" }, { "DatesDate2", "dates.date2" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Instance2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Instance2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Instance2sRadGrid, "MatchKey", "matchKey"),
                Global.GetCqlFilter(Instance2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Instance2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Instance2sRadGrid, "Author", "contributors[0].name"),
                Global.GetCqlFilter(Instance2sRadGrid, "PublicationStartYear", "publicationPeriod.start"),
                Global.GetCqlFilter(Instance2sRadGrid, "PublicationEndYear", "publicationPeriod.end"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDateTypeId", "dates.dateTypeId"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDate1", "dates.date1"),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDate2", "dates.date2"),
                Global.GetCqlFilter(Instance2sRadGrid, "InstanceType.Name", "instanceTypeId", "name", folioServiceContext.FolioServiceClient.InstanceTypes),
                Global.GetCqlFilter(Instance2sRadGrid, "IssuanceMode.Name", "modeOfIssuanceId", "name", folioServiceContext.FolioServiceClient.ModeOfIssuances),
                Global.GetCqlFilter(Instance2sRadGrid, "CatalogedDate", "catalogedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "PreviouslyHeld", "previouslyHeld"),
                Global.GetCqlFilter(Instance2sRadGrid, "StaffSuppress", "staffSuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "SourceRecordFormat", "sourceRecordFormat"),
                Global.GetCqlFilter(Instance2sRadGrid, "Status.Name", "statusId", "name", folioServiceContext.FolioServiceClient.InstanceStatuses),
                Global.GetCqlFilter(Instance2sRadGrid, "StatusLastWriteTime", "statusUpdatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "CompletionTime", "")
            }.Where(s => s != null)));
            foreach (var i2 in folioServiceContext.Instance2s(where, Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{i2.Id}\t{i2.ShortId}\t{Global.TextEncode(i2.MatchKey)}\t{Global.TextEncode(i2.Source)}\t{Global.TextEncode(i2.Title)}\t{Global.TextEncode(i2.Author)}\t{i2.PublicationStartYear}\t{i2.PublicationEndYear}\t{i2.DatesDateTypeId}\t{Global.TextEncode(i2.DatesDate1)}\t{Global.TextEncode(i2.DatesDate2)}\t{Global.TextEncode(i2.InstanceType?.Name)}\t{i2.InstanceTypeId}\t{Global.TextEncode(i2.IssuanceMode?.Name)}\t{i2.IssuanceModeId}\t{i2.CatalogedDate:M/d/yyyy}\t{i2.PreviouslyHeld}\t{i2.StaffSuppress}\t{i2.DiscoverySuppress}\t{Global.TextEncode(i2.SourceRecordFormat)}\t{Global.TextEncode(i2.Status?.Name)}\t{i2.StatusId}\t{i2.StatusLastWriteTime:M/d/yyyy HH:mm:ss}\t{i2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.CreationUser?.Username)}\t{i2.CreationUserId}\t{i2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastWriteUser?.Username)}\t{i2.LastWriteUserId}\t{i2.CompletionTime:M/d/yyyy HH:mm:ss}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
