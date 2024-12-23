using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.BlockCondition2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BlockCondition2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void BlockCondition2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var bc2 = folioServiceContext.FindBlockCondition2(id, true);
            if (bc2 == null) Response.Redirect("Default.aspx");
            bc2.Content = bc2.Content != null ? JsonConvert.DeserializeObject<JToken>(bc2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            BlockCondition2FormView.DataSource = new[] { bc2 };
            Title = $"Block Condition {bc2.Name}";
        }

        protected void BlockLimit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BlockLimit2sPermission"] == null) return;
            var id = (Guid?)BlockCondition2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "GroupId", "patronGroupId" }, { "ConditionId", "conditionId" }, { "Value", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"conditionId == \"{id}\"",
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Group.Name", "patronGroupId", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Value", "value"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BlockLimit2sRadGrid.DataSource = folioServiceContext.BlockLimit2s(where, BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockLimit2sRadGrid.PageSize * BlockLimit2sRadGrid.CurrentPageIndex, BlockLimit2sRadGrid.PageSize, true);
            BlockLimit2sRadGrid.VirtualItemCount = folioServiceContext.CountBlockLimit2s(where);
            if (BlockLimit2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BlockLimit2sRadGrid.AllowFilteringByColumn = BlockLimit2sRadGrid.VirtualItemCount > 10;
                BlockLimit2sPanel.Visible = BlockCondition2FormView.DataKey.Value != null && Session["BlockLimit2sPermission"] != null && BlockLimit2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
