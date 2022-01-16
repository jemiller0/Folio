using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Request2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Request2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Request2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var r2 = folioServiceContext.FindRequest2(id, true);
            if (r2 == null) Response.Redirect("Default.aspx");
            r2.Content = r2.Content != null ? JsonConvert.DeserializeObject<JToken>(r2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Request2FormView.DataSource = new[] { r2 };
            Title = $"Request {r2.Id}";
        }

        protected void RequestIdentifiersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RequestIdentifiersPermission"] == null) return;
            var id = (Guid?)Request2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRequest2(id).RequestIdentifiers ?? new RequestIdentifier[] { };
            RequestIdentifiersRadGrid.DataSource = l;
            RequestIdentifiersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RequestIdentifiersPanel.Visible = Request2FormView.DataKey.Value != null && ((string)Session["RequestIdentifiersPermission"] == "Edit" || Session["RequestIdentifiersPermission"] != null && l.Any());
        }

        protected void RequestTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["RequestTagsPermission"] == null) return;
            var id = (Guid?)Request2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindRequest2(id).RequestTags ?? new RequestTag[] { };
            RequestTagsRadGrid.DataSource = l;
            RequestTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            RequestTagsPanel.Visible = Request2FormView.DataKey.Value != null && ((string)Session["RequestTagsPermission"] == "Edit" || Session["RequestTagsPermission"] != null && l.Any());
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)Request2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(out var i, Global.GetCqlFilter(ScheduledNotice2sRadGrid, d, $"requestId == \"{id}\""), ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = i;
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = Request2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
