using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LoanPolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoanPolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LoanPolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
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
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyAlternateFixedDueDateSchedule.Name", "renewalsPolicy.alternateFixedDueDateScheduleId", "name", folioServiceContext.FolioServiceClient.FixedDueDateSchedules),
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
            LoanPolicy2sRadGrid.DataSource = folioServiceContext.LoanPolicy2s(out var i, where, LoanPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LoanPolicy2sRadGrid.PageSize * LoanPolicy2sRadGrid.CurrentPageIndex, LoanPolicy2sRadGrid.PageSize, true);
            LoanPolicy2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LoanPolicy2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tLoanable\tLoansPolicyProfileId\tLoansPolicyPeriodDuration\tLoansPolicyPeriodInterval\tLoansPolicyClosedLibraryDueDateManagementId\tLoansPolicyGracePeriodDuration\tLoansPolicyGracePeriodInterval\tLoansPolicyOpeningTimeOffsetDuration\tLoansPolicyOpeningTimeOffsetInterval\tLoansPolicyFixedDueDateSchedule\tLoansPolicyFixedDueDateScheduleId\tLoansPolicyItemLimit\tRenewable\tRenewalsPolicyUnlimited\tRenewalsPolicyNumberAllowed\tRenewalsPolicyRenewFromId\tRenewalsPolicyDifferentPeriod\tRenewalsPolicyPeriodDuration\tRenewalsPolicyPeriodInterval\tRenewalsPolicyAlternateFixedDueDateSchedule\tRenewalsPolicyAlternateFixedDueDateScheduleId\tRecallsAlternateGracePeriodDuration\tRecallsAlternateGracePeriodInterval\tRecallsMinimumGuaranteedLoanPeriodDuration\tRecallsMinimumGuaranteedLoanPeriodInterval\tRecallsRecallReturnIntervalDuration\tRecallsRecallReturnIntervalInterval\tRecallsAllowRecallsToExtendOverdueLoans\tRecallsAlternateRecallReturnIntervalDuration\tRecallsAlternateRecallReturnIntervalInterval\tHoldsAlternateCheckoutLoanPeriodDuration\tHoldsAlternateCheckoutLoanPeriodInterval\tHoldsRenewItemsWithRequest\tHoldsAlternateRenewalLoanPeriodDuration\tHoldsAlternateRenewalLoanPeriodInterval\tPagesAlternateCheckoutLoanPeriodDuration\tPagesAlternateCheckoutLoanPeriodInterval\tPagesRenewItemsWithRequest\tPagesAlternateRenewalLoanPeriodDuration\tPagesAlternateRenewalLoanPeriodInterval\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "Loanable", "loanable" }, { "LoansPolicyProfileId", "loansPolicy.profileId" }, { "LoansPolicyPeriodDuration", "loansPolicy.period.duration" }, { "LoansPolicyPeriodInterval", "loansPolicy.period.intervalId" }, { "LoansPolicyClosedLibraryDueDateManagementId", "loansPolicy.closedLibraryDueDateManagementId" }, { "LoansPolicyGracePeriodDuration", "loansPolicy.gracePeriod.duration" }, { "LoansPolicyGracePeriodInterval", "loansPolicy.gracePeriod.intervalId" }, { "LoansPolicyOpeningTimeOffsetDuration", "loansPolicy.openingTimeOffset.duration" }, { "LoansPolicyOpeningTimeOffsetInterval", "loansPolicy.openingTimeOffset.intervalId" }, { "LoansPolicyFixedDueDateScheduleId", "loansPolicy.fixedDueDateScheduleId" }, { "LoansPolicyItemLimit", "loansPolicy.itemLimit" }, { "Renewable", "renewable" }, { "RenewalsPolicyUnlimited", "renewalsPolicy.unlimited" }, { "RenewalsPolicyNumberAllowed", "renewalsPolicy.numberAllowed" }, { "RenewalsPolicyRenewFromId", "renewalsPolicy.renewFromId" }, { "RenewalsPolicyDifferentPeriod", "renewalsPolicy.differentPeriod" }, { "RenewalsPolicyPeriodDuration", "renewalsPolicy.period.duration" }, { "RenewalsPolicyPeriodInterval", "renewalsPolicy.period.intervalId" }, { "RenewalsPolicyAlternateFixedDueDateScheduleId", "renewalsPolicy.alternateFixedDueDateScheduleId" }, { "RecallsAlternateGracePeriodDuration", "requestManagement.recalls.alternateGracePeriod.duration" }, { "RecallsAlternateGracePeriodInterval", "requestManagement.recalls.alternateGracePeriod.intervalId" }, { "RecallsMinimumGuaranteedLoanPeriodDuration", "requestManagement.recalls.minimumGuaranteedLoanPeriod.duration" }, { "RecallsMinimumGuaranteedLoanPeriodInterval", "requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId" }, { "RecallsRecallReturnIntervalDuration", "requestManagement.recalls.recallReturnInterval.duration" }, { "RecallsRecallReturnIntervalInterval", "requestManagement.recalls.recallReturnInterval.intervalId" }, { "RecallsAllowRecallsToExtendOverdueLoans", "requestManagement.recalls.allowRecallsToExtendOverdueLoans" }, { "RecallsAlternateRecallReturnIntervalDuration", "requestManagement.recalls.alternateRecallReturnInterval.duration" }, { "RecallsAlternateRecallReturnIntervalInterval", "requestManagement.recalls.alternateRecallReturnInterval.intervalId" }, { "HoldsAlternateCheckoutLoanPeriodDuration", "requestManagement.holds.alternateCheckoutLoanPeriod.duration" }, { "HoldsAlternateCheckoutLoanPeriodInterval", "requestManagement.holds.alternateCheckoutLoanPeriod.intervalId" }, { "HoldsRenewItemsWithRequest", "requestManagement.holds.renewItemsWithRequest" }, { "HoldsAlternateRenewalLoanPeriodDuration", "requestManagement.holds.alternateRenewalLoanPeriod.duration" }, { "HoldsAlternateRenewalLoanPeriodInterval", "requestManagement.holds.alternateRenewalLoanPeriod.intervalId" }, { "PagesAlternateCheckoutLoanPeriodDuration", "requestManagement.pages.alternateCheckoutLoanPeriod.duration" }, { "PagesAlternateCheckoutLoanPeriodInterval", "requestManagement.pages.alternateCheckoutLoanPeriod.intervalId" }, { "PagesRenewItemsWithRequest", "requestManagement.pages.renewItemsWithRequest" }, { "PagesAlternateRenewalLoanPeriodDuration", "requestManagement.pages.alternateRenewalLoanPeriod.duration" }, { "PagesAlternateRenewalLoanPeriodInterval", "requestManagement.pages.alternateRenewalLoanPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
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
                Global.GetCqlFilter(LoanPolicy2sRadGrid, "RenewalsPolicyAlternateFixedDueDateSchedule.Name", "renewalsPolicy.alternateFixedDueDateScheduleId", "name", folioServiceContext.FolioServiceClient.FixedDueDateSchedules),
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
            foreach (var lp2 in folioServiceContext.LoanPolicy2s(where, LoanPolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LoanPolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lp2.Id}\t{Global.TextEncode(lp2.Name)}\t{Global.TextEncode(lp2.Description)}\t{lp2.Loanable}\t{Global.TextEncode(lp2.LoansPolicyProfileId)}\t{lp2.LoansPolicyPeriodDuration}\t{Global.TextEncode(lp2.LoansPolicyPeriodInterval)}\t{Global.TextEncode(lp2.LoansPolicyClosedLibraryDueDateManagementId)}\t{lp2.LoansPolicyGracePeriodDuration}\t{Global.TextEncode(lp2.LoansPolicyGracePeriodInterval)}\t{lp2.LoansPolicyOpeningTimeOffsetDuration}\t{Global.TextEncode(lp2.LoansPolicyOpeningTimeOffsetInterval)}\t{Global.TextEncode(lp2.LoansPolicyFixedDueDateSchedule?.Name)}\t{lp2.LoansPolicyFixedDueDateScheduleId}\t{lp2.LoansPolicyItemLimit}\t{lp2.Renewable}\t{lp2.RenewalsPolicyUnlimited}\t{lp2.RenewalsPolicyNumberAllowed}\t{Global.TextEncode(lp2.RenewalsPolicyRenewFromId)}\t{lp2.RenewalsPolicyDifferentPeriod}\t{lp2.RenewalsPolicyPeriodDuration}\t{Global.TextEncode(lp2.RenewalsPolicyPeriodInterval)}\t{Global.TextEncode(lp2.RenewalsPolicyAlternateFixedDueDateSchedule?.Name)}\t{lp2.RenewalsPolicyAlternateFixedDueDateScheduleId}\t{lp2.RecallsAlternateGracePeriodDuration}\t{Global.TextEncode(lp2.RecallsAlternateGracePeriodInterval)}\t{lp2.RecallsMinimumGuaranteedLoanPeriodDuration}\t{Global.TextEncode(lp2.RecallsMinimumGuaranteedLoanPeriodInterval)}\t{lp2.RecallsRecallReturnIntervalDuration}\t{Global.TextEncode(lp2.RecallsRecallReturnIntervalInterval)}\t{lp2.RecallsAllowRecallsToExtendOverdueLoans}\t{lp2.RecallsAlternateRecallReturnIntervalDuration}\t{Global.TextEncode(lp2.RecallsAlternateRecallReturnIntervalInterval)}\t{lp2.HoldsAlternateCheckoutLoanPeriodDuration}\t{Global.TextEncode(lp2.HoldsAlternateCheckoutLoanPeriodInterval)}\t{lp2.HoldsRenewItemsWithRequest}\t{lp2.HoldsAlternateRenewalLoanPeriodDuration}\t{Global.TextEncode(lp2.HoldsAlternateRenewalLoanPeriodInterval)}\t{lp2.PagesAlternateCheckoutLoanPeriodDuration}\t{Global.TextEncode(lp2.PagesAlternateCheckoutLoanPeriodInterval)}\t{lp2.PagesRenewItemsWithRequest}\t{lp2.PagesAlternateRenewalLoanPeriodDuration}\t{Global.TextEncode(lp2.PagesAlternateRenewalLoanPeriodInterval)}\t{lp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lp2.CreationUser?.Username)}\t{lp2.CreationUserId}\t{lp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lp2.LastWriteUser?.Username)}\t{lp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
