using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Loan2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Loan2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Loan2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var l2 = folioServiceContext.FindLoan2(id, true);
            if (l2 == null) Response.Redirect("Default.aspx");
            l2.Content = l2.Content != null ? JsonConvert.DeserializeObject<JToken>(l2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Loan2FormView.DataSource = new[] { l2 };
            Title = $"Loan {l2.Id}";
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Loan2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"loanId == \"{id}\"",
                Global.GetCqlFilter(Fee2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Fee2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Fee2sRadGrid, "RemainingAmount", "remaining"),
                Global.GetCqlFilter(Fee2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "PaymentStatusName", "paymentStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Fee2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Fee2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType", "materialType"),
                Global.GetCqlFilter(Fee2sRadGrid, "ItemStatusName", "itemStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Location", "location"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "ReturnedTime", "returnedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType1.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "FeeType.Name", "feeFineId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(Fee2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Fee2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances)
            }.Where(s => s != null)));
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, where, Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Loan2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PatronActionSession2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronActionSession2sPermission"] == null) return;
            var id = (Guid?)Loan2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SessionId", "sessionId" }, { "PatronId", "patronId" }, { "LoanId", "loanId" }, { "ActionType", "actionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"loanId == \"{id}\"",
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "SessionId", "sessionId"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "Patron.Username", "patronId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "ActionType", "actionType"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PatronActionSession2sRadGrid.DataSource = folioServiceContext.PatronActionSession2s(out var i, where, PatronActionSession2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PatronActionSession2sRadGrid.PageSize * PatronActionSession2sRadGrid.CurrentPageIndex, PatronActionSession2sRadGrid.PageSize, true);
            PatronActionSession2sRadGrid.VirtualItemCount = i;
            if (PatronActionSession2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PatronActionSession2sRadGrid.AllowFilteringByColumn = PatronActionSession2sRadGrid.VirtualItemCount > 10;
                PatronActionSession2sPanel.Visible = Loan2FormView.DataKey.Value != null && Session["PatronActionSession2sPermission"] != null && PatronActionSession2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)Loan2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "SessionId", "sessionId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"loanId == \"{id}\"",
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Id", "id"),
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
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = Loan2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
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
