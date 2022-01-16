using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Template2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Template2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Template2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var t2 = folioServiceContext.FindTemplate2(id, true);
            if (t2 == null) Response.Redirect("Default.aspx");
            t2.Content = t2.Content != null ? JsonConvert.DeserializeObject<JToken>(t2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Template2FormView.DataSource = new[] { t2 };
            Title = $"Template {t2.Name}";
        }

        protected void FeeType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FeeType2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            FeeType2sRadGrid.DataSource = folioServiceContext.FeeType2s(out var i, Global.GetCqlFilter(FeeType2sRadGrid, d, $"actionNoticeId == \"{id}\""), FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2sRadGrid.PageSize * FeeType2sRadGrid.CurrentPageIndex, FeeType2sRadGrid.PageSize, true);
            FeeType2sRadGrid.VirtualItemCount = i;
            if (FeeType2sRadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2sRadGrid.AllowFilteringByColumn = FeeType2sRadGrid.VirtualItemCount > 10;
                FeeType2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void FeeType2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FeeType2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            FeeType2s1RadGrid.DataSource = folioServiceContext.FeeType2s(out var i, Global.GetCqlFilter(FeeType2s1RadGrid, d, $"chargeNoticeId == \"{id}\""), FeeType2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2s1RadGrid.PageSize * FeeType2s1RadGrid.CurrentPageIndex, FeeType2s1RadGrid.PageSize, true);
            FeeType2s1RadGrid.VirtualItemCount = i;
            if (FeeType2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2s1RadGrid.AllowFilteringByColumn = FeeType2s1RadGrid.VirtualItemCount > 10;
                FeeType2s1Panel.Visible = Template2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Owner2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Owner2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Owner2sRadGrid.DataSource = folioServiceContext.Owner2s(out var i, Global.GetCqlFilter(Owner2sRadGrid, d, $"defaultActionNoticeId == \"{id}\""), Owner2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Owner2sRadGrid.PageSize * Owner2sRadGrid.CurrentPageIndex, Owner2sRadGrid.PageSize, true);
            Owner2sRadGrid.VirtualItemCount = i;
            if (Owner2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Owner2sRadGrid.AllowFilteringByColumn = Owner2sRadGrid.VirtualItemCount > 10;
                Owner2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["Owner2sPermission"] != null && Owner2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Owner2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Owner2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Owner2s1RadGrid.DataSource = folioServiceContext.Owner2s(out var i, Global.GetCqlFilter(Owner2s1RadGrid, d, $"defaultChargeNoticeId == \"{id}\""), Owner2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Owner2s1RadGrid.PageSize * Owner2s1RadGrid.CurrentPageIndex, Owner2s1RadGrid.PageSize, true);
            Owner2s1RadGrid.VirtualItemCount = i;
            if (Owner2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Owner2s1RadGrid.AllowFilteringByColumn = Owner2s1RadGrid.VirtualItemCount > 10;
                Owner2s1Panel.Visible = Template2FormView.DataKey.Value != null && Session["Owner2sPermission"] != null && Owner2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(out var i, Global.GetCqlFilter(ScheduledNotice2sRadGrid, d, $"noticeConfig.templateId == \"{id}\""), ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = i;
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void TemplateOutputFormatsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TemplateOutputFormatsPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTemplate2(id).TemplateOutputFormats ?? new TemplateOutputFormat[] { };
            TemplateOutputFormatsRadGrid.DataSource = l;
            TemplateOutputFormatsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            TemplateOutputFormatsPanel.Visible = Template2FormView.DataKey.Value != null && ((string)Session["TemplateOutputFormatsPermission"] == "Edit" || Session["TemplateOutputFormatsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
