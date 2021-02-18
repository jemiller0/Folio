using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Snapshot2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Snapshot2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Snapshot2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var s2 = folioServiceContext.FindSnapshot2(id, true);
            if (s2 == null) Response.Redirect("Default.aspx");
            Snapshot2FormView.DataSource = new[] { s2 };
            Title = $"Snapshot {s2.Id}";
        }

        protected void Record2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Record2sPermission"] == null) return;
            var id = (Guid?)Snapshot2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SnapshotId", "snapshotId" }, { "MatchedId", "matchedId" }, { "Generation", "generation" }, { "RecordType", "recordType" }, { "InstanceId", "externalIdsHolder.instanceId" }, { "State", "state" }, { "LeaderRecordStatus", "leaderRecordStatus" }, { "Order", "order" }, { "SuppressDiscovery", "additionalInfo.suppressDiscovery" }, { "CreationUserId", "metadata.createdByUserId" }, { "CreationTime", "metadata.createdDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "LastWriteTime", "metadata.updatedDate" } };
            Record2sRadGrid.DataSource = folioServiceContext.Record2s(out var i, Global.GetCqlFilter(Record2sRadGrid, d, $"snapshotId == \"{id}\""), Record2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Record2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Record2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Record2sRadGrid.PageSize * Record2sRadGrid.CurrentPageIndex, Record2sRadGrid.PageSize, true);
            Record2sRadGrid.VirtualItemCount = i;
            if (Record2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Record2sRadGrid.AllowFilteringByColumn = Record2sRadGrid.VirtualItemCount > 10;
                Record2sPanel.Visible = Snapshot2FormView.DataKey.Value != null && Session["Record2sPermission"] != null && Record2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
