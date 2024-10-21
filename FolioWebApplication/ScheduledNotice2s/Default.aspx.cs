using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ScheduledNotice2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "SessionId", "sessionId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Request.Id", "requestId", "id", folioServiceContext.FolioServiceClient.Requests),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Payment.Id", "feeFineActionId", "id", folioServiceContext.FolioServiceClient.Payments),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "RecipientUser.Username", "recipientUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "SessionId", "sessionId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NextRunTime", "nextRunTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "TriggeringEvent", "triggeringEvent"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTiming", "noticeConfig.timing"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTemplate.Name", "noticeConfig.templateId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigFormat", "noticeConfig.format"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(out var i, where, ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ScheduledNotice2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tLoan\tLoanId\tRequest\tRequestId\tPayment\tPaymentId\tRecipientUser\tRecipientUserId\tSessionId\tNextRunTime\tTriggeringEvent\tNoticeConfigTiming\tNoticeConfigRecurringPeriodDuration\tNoticeConfigRecurringPeriodInterval\tNoticeConfigTemplate\tNoticeConfigTemplateId\tNoticeConfigFormat\tNoticeConfigSendInRealTime\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "SessionId", "sessionId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Request.Id", "requestId", "id", folioServiceContext.FolioServiceClient.Requests),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Payment.Id", "feeFineActionId", "id", folioServiceContext.FolioServiceClient.Payments),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "RecipientUser.Username", "recipientUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "SessionId", "sessionId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NextRunTime", "nextRunTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "TriggeringEvent", "triggeringEvent"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTiming", "noticeConfig.timing"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTemplate.Name", "noticeConfig.templateId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigFormat", "noticeConfig.format"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var sn2 in folioServiceContext.ScheduledNotice2s(where, ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{sn2.Id}\t{sn2.Loan?.Id}\t{sn2.LoanId}\t{sn2.Request?.Id}\t{sn2.RequestId}\t{sn2.Payment?.Id}\t{sn2.PaymentId}\t{Global.TextEncode(sn2.RecipientUser?.Username)}\t{sn2.RecipientUserId}\t{sn2.SessionId}\t{sn2.NextRunTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sn2.TriggeringEvent)}\t{Global.TextEncode(sn2.NoticeConfigTiming)}\t{sn2.NoticeConfigRecurringPeriodDuration}\t{Global.TextEncode(sn2.NoticeConfigRecurringPeriodInterval)}\t{Global.TextEncode(sn2.NoticeConfigTemplate?.Name)}\t{sn2.NoticeConfigTemplateId}\t{Global.TextEncode(sn2.NoticeConfigFormat)}\t{sn2.NoticeConfigSendInRealTime}\t{sn2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sn2.CreationUser?.Username)}\t{sn2.CreationUserId}\t{sn2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sn2.LastWriteUser?.Username)}\t{sn2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
