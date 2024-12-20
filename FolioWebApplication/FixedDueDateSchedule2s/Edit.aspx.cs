using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FixedDueDateSchedule2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FixedDueDateSchedule2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FixedDueDateSchedule2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var fdds2 = folioServiceContext.FindFixedDueDateSchedule2(id, true);
            if (fdds2 == null) Response.Redirect("Default.aspx");
            fdds2.Content = fdds2.Content != null ? JsonConvert.DeserializeObject<JToken>(fdds2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            FixedDueDateSchedule2FormView.DataSource = new[] { fdds2 };
            Title = $"Fixed Due Date Schedule {fdds2.Name}";
        }

        protected void FixedDueDateScheduleSchedulesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FixedDueDateScheduleSchedulesPermission"] == null) return;
            var id = (Guid?)FixedDueDateSchedule2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindFixedDueDateSchedule2(id, true).FixedDueDateScheduleSchedules ?? new FixedDueDateScheduleSchedule[] { };
            FixedDueDateScheduleSchedulesRadGrid.DataSource = l;
            FixedDueDateScheduleSchedulesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            FixedDueDateScheduleSchedulesPanel.Visible = FixedDueDateSchedule2FormView.DataKey.Value != null && ((string)Session["FixedDueDateScheduleSchedulesPermission"] == "Edit" || Session["FixedDueDateScheduleSchedulesPermission"] != null && l.Any());
        }

        protected void LoanPolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LoanPolicy2sPermission"] == null) return;
            var id = (Guid?)FixedDueDateSchedule2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"renewalsPolicy.alternateFixedDueDateScheduleId == \"{id}\"",
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "Loanable", "loanable"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyProfileId", "loansPolicy.profileId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyPeriodDuration", "loansPolicy.period.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyFixedDueDateSchedule.Name", "loansPolicy.fixedDueDateScheduleId", "name", folioServiceContext.FolioServiceClient.FixedDueDateSchedules),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LoansPolicyItemLimit", "loansPolicy.itemLimit"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "Renewable", "renewable"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LoanPolicy2sRadGrid.DataSource = folioServiceContext.LoanPolicy2s(where, LoanPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanPolicy2sRadGrid.PageSize * LoanPolicy2sRadGrid.CurrentPageIndex, LoanPolicy2sRadGrid.PageSize, true);
            LoanPolicy2sRadGrid.VirtualItemCount = folioServiceContext.CountLoanPolicy2s(where);
            if (LoanPolicy2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LoanPolicy2sRadGrid.AllowFilteringByColumn = LoanPolicy2sRadGrid.VirtualItemCount > 10;
                LoanPolicy2sPanel.Visible = FixedDueDateSchedule2FormView.DataKey.Value != null && Session["LoanPolicy2sPermission"] != null && LoanPolicy2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void LoanPolicy2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LoanPolicy2sPermission"] == null) return;
            var id = (Guid?)FixedDueDateSchedule2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"loansPolicy.fixedDueDateScheduleId == \"{id}\"",
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "Name", "name"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "Description", "description"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "Loanable", "loanable"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyProfileId", "loansPolicy.profileId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyPeriodDuration", "loansPolicy.period.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LoansPolicyItemLimit", "loansPolicy.itemLimit"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "Renewable", "renewable"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RenewalsPolicyAlternateFixedDueDateSchedule.Name", "renewalsPolicy.alternateFixedDueDateScheduleId", "name", folioServiceContext.FolioServiceClient.FixedDueDateSchedules),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LoanPolicy2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LoanPolicy2s1RadGrid.DataSource = folioServiceContext.LoanPolicy2s(where, LoanPolicy2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanPolicy2s1RadGrid.PageSize * LoanPolicy2s1RadGrid.CurrentPageIndex, LoanPolicy2s1RadGrid.PageSize, true);
            LoanPolicy2s1RadGrid.VirtualItemCount = folioServiceContext.CountLoanPolicy2s(where);
            if (LoanPolicy2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                LoanPolicy2s1RadGrid.AllowFilteringByColumn = LoanPolicy2s1RadGrid.VirtualItemCount > 10;
                LoanPolicy2s1Panel.Visible = FixedDueDateSchedule2FormView.DataKey.Value != null && Session["LoanPolicy2sPermission"] != null && LoanPolicy2s1RadGrid.VirtualItemCount > 0;
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
