using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.FixedDueDateSchedule2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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

        protected void LoanPolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LoanPolicy2sPermission"] == null) return;
            var id = (Guid?)FixedDueDateSchedule2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LoanPolicy2sRadGrid.DataSource = folioServiceContext.LoanPolicy2s(out var i, Global.GetCqlFilter(LoanPolicy2sRadGrid, d, $"renewalsPolicy.alternateFixedDueDateScheduleId == \"{id}\""), LoanPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanPolicy2sRadGrid.PageSize * LoanPolicy2sRadGrid.CurrentPageIndex, LoanPolicy2sRadGrid.PageSize, true);
            LoanPolicy2sRadGrid.VirtualItemCount = i;
            if (LoanPolicy2sRadGrid.MasterTableView.FilterExpression == "")
            {
                LoanPolicy2sRadGrid.AllowFilteringByColumn = LoanPolicy2sRadGrid.VirtualItemCount > 10;
                LoanPolicy2sPanel.Visible = FixedDueDateSchedule2FormView.DataKey.Value != null && Session["LoanPolicy2sPermission"] != null && LoanPolicy2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void LoanPolicy2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LoanPolicy2sPermission"] == null) return;
            var id = (Guid?)FixedDueDateSchedule2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LoanPolicy2s1RadGrid.DataSource = folioServiceContext.LoanPolicy2s(out var i, Global.GetCqlFilter(LoanPolicy2s1RadGrid, d, $"loansPolicy.fixedDueDateScheduleId == \"{id}\""), LoanPolicy2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanPolicy2s1RadGrid.PageSize * LoanPolicy2s1RadGrid.CurrentPageIndex, LoanPolicy2s1RadGrid.PageSize, true);
            LoanPolicy2s1RadGrid.VirtualItemCount = i;
            if (LoanPolicy2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                LoanPolicy2s1RadGrid.AllowFilteringByColumn = LoanPolicy2s1RadGrid.VirtualItemCount > 10;
                LoanPolicy2s1Panel.Visible = FixedDueDateSchedule2FormView.DataKey.Value != null && Session["LoanPolicy2sPermission"] != null && LoanPolicy2s1RadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
