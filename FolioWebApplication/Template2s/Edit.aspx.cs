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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"actionNoticeId == \"{id}\"",
                Global.GetCqlFilter(FeeType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Automatic", "automatic"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Name", "feeFineType"),
                Global.GetCqlFilter(FeeType2sRadGrid, "DefaultAmount", "defaultAmount"),
                Global.GetCqlFilter(FeeType2sRadGrid, "ChargeNotice.Name", "chargeNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FeeType2sRadGrid.DataSource = folioServiceContext.FeeType2s(out var i, where, FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2sRadGrid.PageSize * FeeType2sRadGrid.CurrentPageIndex, FeeType2sRadGrid.PageSize, true);
            FeeType2sRadGrid.VirtualItemCount = i;
            if (FeeType2sRadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2sRadGrid.AllowFilteringByColumn = FeeType2sRadGrid.VirtualItemCount > 10;
                FeeType2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void FeeType2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FeeType2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"chargeNoticeId == \"{id}\"",
                Global.GetCqlFilter(FeeType2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "Automatic", "automatic"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "Name", "feeFineType"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "DefaultAmount", "defaultAmount"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "ActionNotice.Name", "actionNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2s1RadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(FeeType2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FeeType2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FeeType2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FeeType2s1RadGrid.DataSource = folioServiceContext.FeeType2s(out var i, where, FeeType2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2s1RadGrid.PageSize * FeeType2s1RadGrid.CurrentPageIndex, FeeType2s1RadGrid.PageSize, true);
            FeeType2s1RadGrid.VirtualItemCount = i;
            if (FeeType2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2s1RadGrid.AllowFilteringByColumn = FeeType2s1RadGrid.VirtualItemCount > 10;
                FeeType2s1Panel.Visible = Template2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void Owner2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Owner2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"defaultActionNoticeId == \"{id}\"",
                Global.GetCqlFilter(Owner2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Owner2sRadGrid, "Name", "owner"),
                Global.GetCqlFilter(Owner2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(Owner2sRadGrid, "DefaultChargeNotice.Name", "defaultChargeNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(Owner2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Owner2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Owner2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Owner2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Owner2sRadGrid.DataSource = folioServiceContext.Owner2s(out var i, where, Owner2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Owner2sRadGrid.PageSize * Owner2sRadGrid.CurrentPageIndex, Owner2sRadGrid.PageSize, true);
            Owner2sRadGrid.VirtualItemCount = i;
            if (Owner2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Owner2sRadGrid.AllowFilteringByColumn = Owner2sRadGrid.VirtualItemCount > 10;
                Owner2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["Owner2sPermission"] != null && Owner2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void Owner2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Owner2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "owner" }, { "Description", "desc" }, { "DefaultChargeNoticeId", "defaultChargeNoticeId" }, { "DefaultActionNoticeId", "defaultActionNoticeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"defaultChargeNoticeId == \"{id}\"",
                Global.GetCqlFilter(Owner2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Owner2s1RadGrid, "Name", "owner"),
                Global.GetCqlFilter(Owner2s1RadGrid, "Description", "desc"),
                Global.GetCqlFilter(Owner2s1RadGrid, "DefaultActionNotice.Name", "defaultActionNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(Owner2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Owner2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Owner2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Owner2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Owner2s1RadGrid.DataSource = folioServiceContext.Owner2s(out var i, where, Owner2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Owner2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Owner2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Owner2s1RadGrid.PageSize * Owner2s1RadGrid.CurrentPageIndex, Owner2s1RadGrid.PageSize, true);
            Owner2s1RadGrid.VirtualItemCount = i;
            if (Owner2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Owner2s1RadGrid.AllowFilteringByColumn = Owner2s1RadGrid.VirtualItemCount > 10;
                Owner2s1Panel.Visible = Template2FormView.DataKey.Value != null && Session["Owner2sPermission"] != null && Owner2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"noticeConfig.templateId == \"{id}\"",
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Request.Id", "requestId", "id", folioServiceContext.FolioServiceClient.Requests),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Payment.Id", "feeFineActionId", "id", folioServiceContext.FolioServiceClient.Payments),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "RecipientUser.Username", "recipientUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NextRunTime", "nextRunTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "TriggeringEvent", "triggeringEvent"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTiming", "noticeConfig.timing"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigFormat", "noticeConfig.format"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(out var i, where, ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = i;
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = Template2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void TemplateOutputFormatsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TemplateOutputFormatsPermission"] == null) return;
            var id = (Guid?)Template2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTemplate2(id, true).TemplateOutputFormats ?? new TemplateOutputFormat[] { };
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
