using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Statuses
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StatusesPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StatusFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var s = folioServiceContext.FindStatus(id, true);
            if (s == null) Response.Redirect("Default.aspx");
            s.Content = s.Content != null ? JsonConvert.DeserializeObject<JToken>(s.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            StatusFormView.DataSource = new[] { s };
            Title = $"Status {s.Name}";
        }

        protected void Instance2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Instance2sPermission"] == null) return;
            var id = (Guid?)StatusFormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "SourceUri", "sourceUri" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors[0].name" }, { "DateTypeId", "dates.dateTypeId" }, { "Date1", "dates.date1" }, { "Date2", "dates.date2" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "Deleted", "deleted" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"statusId == \"{id}\"",
                Global.GetCqlFilter(Instance2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Instance2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Instance2sRadGrid, "MatchKey", "matchKey"),
                Global.GetCqlFilter(Instance2sRadGrid, "SourceUri", "sourceUri"),
                Global.GetCqlFilter(Instance2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Instance2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Instance2sRadGrid, "Author", "contributors[0].name"),
                Global.GetCqlFilter(Instance2sRadGrid, "DateType.Name", "dates.dateTypeId", "name", folioServiceContext.FolioServiceClient.DateTypes),
                Global.GetCqlFilter(Instance2sRadGrid, "Date1", "dates.date1"),
                Global.GetCqlFilter(Instance2sRadGrid, "Date2", "dates.date2"),
                Global.GetCqlFilter(Instance2sRadGrid, "InstanceType.Name", "instanceTypeId", "name", folioServiceContext.FolioServiceClient.InstanceTypes),
                Global.GetCqlFilter(Instance2sRadGrid, "IssuanceMode.Name", "modeOfIssuanceId", "name", folioServiceContext.FolioServiceClient.ModeOfIssuances),
                Global.GetCqlFilter(Instance2sRadGrid, "CatalogedDate", "catalogedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "PreviouslyHeld", "previouslyHeld"),
                Global.GetCqlFilter(Instance2sRadGrid, "StaffSuppress", "staffSuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Instance2sRadGrid, "Deleted", "deleted"),
                Global.GetCqlFilter(Instance2sRadGrid, "SourceRecordFormat", "sourceRecordFormat"),
                Global.GetCqlFilter(Instance2sRadGrid, "StatusLastWriteTime", "statusUpdatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Instance2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Instance2sRadGrid, "CompletionTime", ""),
                Global.GetCqlFilter(Instance2sRadGrid, "DatesDatetypeid", "")
            }.Where(s => s != null)));
            Instance2sRadGrid.DataSource = folioServiceContext.Instance2s(where, Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Instance2sRadGrid.PageSize * Instance2sRadGrid.CurrentPageIndex, Instance2sRadGrid.PageSize, true);
            Instance2sRadGrid.VirtualItemCount = folioServiceContext.CountInstance2s(where);
            if (Instance2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Instance2sRadGrid.AllowFilteringByColumn = Instance2sRadGrid.VirtualItemCount > 10;
                Instance2sPanel.Visible = StatusFormView.DataKey.Value != null && Session["Instance2sPermission"] != null && Instance2sRadGrid.VirtualItemCount > 0;
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
