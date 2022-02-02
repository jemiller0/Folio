using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Record2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Record2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Record2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SnapshotId", "snapshotId" }, { "MatchedId", "matchedId" }, { "Generation", "generation" }, { "RecordType", "recordType" }, { "InstanceId", "externalIdsHolder.instanceId" }, { "State", "state" }, { "LeaderRecordStatus", "leaderRecordStatus" }, { "Order", "order" }, { "SuppressDiscovery", "additionalInfo.suppressDiscovery" }, { "CreationUserId", "metadata.createdByUserId" }, { "CreationTime", "metadata.createdDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastWriteTime", "metadata.updatedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Record2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Record2sRadGrid, "Snapshot.Id", "snapshotId", "jobExecutionId", folioServiceContext.FolioServiceClient.Snapshots),
                Global.GetCqlFilter(Record2sRadGrid, "MatchedId", "matchedId"),
                Global.GetCqlFilter(Record2sRadGrid, "Generation", "generation"),
                Global.GetCqlFilter(Record2sRadGrid, "RecordType", "recordType"),
                Global.GetCqlFilter(Record2sRadGrid, "Instance.Title", "externalIdsHolder.instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Record2sRadGrid, "State", "state"),
                Global.GetCqlFilter(Record2sRadGrid, "LeaderRecordStatus", "leaderRecordStatus"),
                Global.GetCqlFilter(Record2sRadGrid, "Order", "order"),
                Global.GetCqlFilter(Record2sRadGrid, "SuppressDiscovery", "additionalInfo.suppressDiscovery"),
                Global.GetCqlFilter(Record2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Record2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Record2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Record2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Record2sRadGrid, "InstanceHrid", "")
            }.Where(s => s != null)));
            Record2sRadGrid.DataSource = folioServiceContext.Record2s(out var i, where, Record2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Record2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Record2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Record2sRadGrid.PageSize * Record2sRadGrid.CurrentPageIndex, Record2sRadGrid.PageSize, true);
            Record2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Record2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tSnapshot\tSnapshotId\tMatchedId\tGeneration\tRecordType\tInstance\tInstanceId\tState\tLeaderRecordStatus\tOrder\tSuppressDiscovery\tCreationUser\tCreationUserId\tCreationTime\tLastWriteUser\tLastWriteUserId\tLastWriteTime\tInstanceHrid\tErrorRecord2\tMarcRecord2\tRawRecord2\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SnapshotId", "snapshotId" }, { "MatchedId", "matchedId" }, { "Generation", "generation" }, { "RecordType", "recordType" }, { "InstanceId", "externalIdsHolder.instanceId" }, { "State", "state" }, { "LeaderRecordStatus", "leaderRecordStatus" }, { "Order", "order" }, { "SuppressDiscovery", "additionalInfo.suppressDiscovery" }, { "CreationUserId", "metadata.createdByUserId" }, { "CreationTime", "metadata.createdDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastWriteTime", "metadata.updatedDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Record2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Record2sRadGrid, "Snapshot.Id", "snapshotId", "jobExecutionId", folioServiceContext.FolioServiceClient.Snapshots),
                Global.GetCqlFilter(Record2sRadGrid, "MatchedId", "matchedId"),
                Global.GetCqlFilter(Record2sRadGrid, "Generation", "generation"),
                Global.GetCqlFilter(Record2sRadGrid, "RecordType", "recordType"),
                Global.GetCqlFilter(Record2sRadGrid, "Instance.Title", "externalIdsHolder.instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Record2sRadGrid, "State", "state"),
                Global.GetCqlFilter(Record2sRadGrid, "LeaderRecordStatus", "leaderRecordStatus"),
                Global.GetCqlFilter(Record2sRadGrid, "Order", "order"),
                Global.GetCqlFilter(Record2sRadGrid, "SuppressDiscovery", "additionalInfo.suppressDiscovery"),
                Global.GetCqlFilter(Record2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Record2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Record2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Record2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Record2sRadGrid, "InstanceHrid", "")
            }.Where(s => s != null)));
            foreach (var r2 in folioServiceContext.Record2s(where, Record2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Record2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Record2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{r2.Id}\t{r2.Snapshot?.Id}\t{r2.SnapshotId}\t{r2.MatchedId}\t{r2.Generation}\t{Global.TextEncode(r2.RecordType)}\t{Global.TextEncode(r2.Instance?.Title)}\t{r2.InstanceId}\t{Global.TextEncode(r2.State)}\t{Global.TextEncode(r2.LeaderRecordStatus)}\t{r2.Order}\t{r2.SuppressDiscovery}\t{Global.TextEncode(r2.CreationUser?.Username)}\t{r2.CreationUserId}\t{r2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.LastWriteUser?.Username)}\t{r2.LastWriteUserId}\t{r2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(r2.InstanceHrid)}\t{r2.ErrorRecord2?.Id}\t{r2.MarcRecord2?.Id}\t{r2.RawRecord2?.Id}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
