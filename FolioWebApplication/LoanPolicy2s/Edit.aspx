<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LoanPolicy2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LoanPolicy2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LoanPolicy2HyperLink" runat="server" Text="Loan Policy" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LoanPolicy2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="LoanPolicy2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLoanPolicy2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Name") != null %>'>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NameLiteral" runat="server" Text='<%#: Eval("Name") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Loanable") != null %>'>
                                <td>
                                    <asp:Label ID="LoanableLabel" runat="server" Text="Loanable:" AssociatedControlID="LoanableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoanableLiteral" runat="server" Text='<%#: Eval("Loanable") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyProfileId") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyProfileIdLabel" runat="server" Text="Loans Policy Profile Id:" AssociatedControlID="LoansPolicyProfileIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyProfileIdLiteral" runat="server" Text='<%#: Eval("LoansPolicyProfileId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyPeriodDurationLabel" runat="server" Text="Loans Policy Period Duration:" AssociatedControlID="LoansPolicyPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyPeriodDurationLiteral" runat="server" Text='<%#: Eval("LoansPolicyPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyPeriodIntervalLabel" runat="server" Text="Loans Policy Period Interval:" AssociatedControlID="LoansPolicyPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyPeriodIntervalLiteral" runat="server" Text='<%#: Eval("LoansPolicyPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyClosedLibraryDueDateManagementId") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyClosedLibraryDueDateManagementIdLabel" runat="server" Text="Loans Policy Closed Library Due Date Management Id:" AssociatedControlID="LoansPolicyClosedLibraryDueDateManagementIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyClosedLibraryDueDateManagementIdLiteral" runat="server" Text='<%#: Eval("LoansPolicyClosedLibraryDueDateManagementId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyGracePeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyGracePeriodDurationLabel" runat="server" Text="Loans Policy Grace Period Duration:" AssociatedControlID="LoansPolicyGracePeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyGracePeriodDurationLiteral" runat="server" Text='<%#: Eval("LoansPolicyGracePeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyGracePeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyGracePeriodIntervalLabel" runat="server" Text="Loans Policy Grace Period Interval:" AssociatedControlID="LoansPolicyGracePeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyGracePeriodIntervalLiteral" runat="server" Text='<%#: Eval("LoansPolicyGracePeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyOpeningTimeOffsetDuration") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyOpeningTimeOffsetDurationLabel" runat="server" Text="Loans Policy Opening Time Offset Duration:" AssociatedControlID="LoansPolicyOpeningTimeOffsetDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyOpeningTimeOffsetDurationLiteral" runat="server" Text='<%#: Eval("LoansPolicyOpeningTimeOffsetDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyOpeningTimeOffsetInterval") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyOpeningTimeOffsetIntervalLabel" runat="server" Text="Loans Policy Opening Time Offset Interval:" AssociatedControlID="LoansPolicyOpeningTimeOffsetIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyOpeningTimeOffsetIntervalLiteral" runat="server" Text='<%#: Eval("LoansPolicyOpeningTimeOffsetInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyFixedDueDateSchedule") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyFixedDueDateScheduleLabel" runat="server" Text="Loans Policy Fixed Due Date Schedule:" AssociatedControlID="LoansPolicyFixedDueDateScheduleHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LoansPolicyFixedDueDateScheduleHyperLink" runat="server" Text='<%#: Eval("LoansPolicyFixedDueDateSchedule.Name") %>' NavigateUrl='<%# $"~/FixedDueDateSchedule2s/Edit.aspx?Id={Eval("LoansPolicyFixedDueDateScheduleId")}" %>' Enabled='<%# Session["FixedDueDateSchedule2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LoansPolicyItemLimit") != null %>'>
                                <td>
                                    <asp:Label ID="LoansPolicyItemLimitLabel" runat="server" Text="Loans Policy Item Limit:" AssociatedControlID="LoansPolicyItemLimitLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LoansPolicyItemLimitLiteral" runat="server" Text='<%#: Eval("LoansPolicyItemLimit") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Renewable") != null %>'>
                                <td>
                                    <asp:Label ID="RenewableLabel" runat="server" Text="Renewable:" AssociatedControlID="RenewableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewableLiteral" runat="server" Text='<%#: Eval("Renewable") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyUnlimited") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyUnlimitedLabel" runat="server" Text="Renewals Policy Unlimited:" AssociatedControlID="RenewalsPolicyUnlimitedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyUnlimitedLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyUnlimited") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyNumberAllowed") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyNumberAllowedLabel" runat="server" Text="Renewals Policy Number Allowed:" AssociatedControlID="RenewalsPolicyNumberAllowedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyNumberAllowedLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyNumberAllowed") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyRenewFromId") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyRenewFromIdLabel" runat="server" Text="Renewals Policy Renew From Id:" AssociatedControlID="RenewalsPolicyRenewFromIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyRenewFromIdLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyRenewFromId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyDifferentPeriod") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyDifferentPeriodLabel" runat="server" Text="Renewals Policy Different Period:" AssociatedControlID="RenewalsPolicyDifferentPeriodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyDifferentPeriodLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyDifferentPeriod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyPeriodDurationLabel" runat="server" Text="Renewals Policy Period Duration:" AssociatedControlID="RenewalsPolicyPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyPeriodDurationLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyPeriodIntervalLabel" runat="server" Text="Renewals Policy Period Interval:" AssociatedControlID="RenewalsPolicyPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsPolicyPeriodIntervalLiteral" runat="server" Text='<%#: Eval("RenewalsPolicyPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalsPolicyAlternateFixedDueDateSchedule") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsPolicyAlternateFixedDueDateScheduleLabel" runat="server" Text="Renewals Policy Alternate Fixed Due Date Schedule:" AssociatedControlID="RenewalsPolicyAlternateFixedDueDateScheduleHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RenewalsPolicyAlternateFixedDueDateScheduleHyperLink" runat="server" Text='<%#: Eval("RenewalsPolicyAlternateFixedDueDateSchedule.Name") %>' NavigateUrl='<%# $"~/FixedDueDateSchedule2s/Edit.aspx?Id={Eval("RenewalsPolicyAlternateFixedDueDateScheduleId")}" %>' Enabled='<%# Session["FixedDueDateSchedule2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsAlternateGracePeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsAlternateGracePeriodDurationLabel" runat="server" Text="Recalls Alternate Grace Period Duration:" AssociatedControlID="RecallsAlternateGracePeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsAlternateGracePeriodDurationLiteral" runat="server" Text='<%#: Eval("RecallsAlternateGracePeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsAlternateGracePeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsAlternateGracePeriodIntervalLabel" runat="server" Text="Recalls Alternate Grace Period Interval:" AssociatedControlID="RecallsAlternateGracePeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsAlternateGracePeriodIntervalLiteral" runat="server" Text='<%#: Eval("RecallsAlternateGracePeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsMinimumGuaranteedLoanPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsMinimumGuaranteedLoanPeriodDurationLabel" runat="server" Text="Recalls Minimum Guaranteed Loan Period Duration:" AssociatedControlID="RecallsMinimumGuaranteedLoanPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsMinimumGuaranteedLoanPeriodDurationLiteral" runat="server" Text='<%#: Eval("RecallsMinimumGuaranteedLoanPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsMinimumGuaranteedLoanPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsMinimumGuaranteedLoanPeriodIntervalLabel" runat="server" Text="Recalls Minimum Guaranteed Loan Period Interval:" AssociatedControlID="RecallsMinimumGuaranteedLoanPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsMinimumGuaranteedLoanPeriodIntervalLiteral" runat="server" Text='<%#: Eval("RecallsMinimumGuaranteedLoanPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsRecallReturnIntervalDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsRecallReturnIntervalDurationLabel" runat="server" Text="Recalls Recall Return Interval Duration:" AssociatedControlID="RecallsRecallReturnIntervalDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsRecallReturnIntervalDurationLiteral" runat="server" Text='<%#: Eval("RecallsRecallReturnIntervalDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsRecallReturnIntervalInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsRecallReturnIntervalIntervalLabel" runat="server" Text="Recalls Recall Return Interval Interval:" AssociatedControlID="RecallsRecallReturnIntervalIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsRecallReturnIntervalIntervalLiteral" runat="server" Text='<%#: Eval("RecallsRecallReturnIntervalInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsAllowRecallsToExtendOverdueLoans") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsAllowRecallsToExtendOverdueLoansLabel" runat="server" Text="Recalls Allow Recalls To Extend Overdue Loans:" AssociatedControlID="RecallsAllowRecallsToExtendOverdueLoansLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsAllowRecallsToExtendOverdueLoansLiteral" runat="server" Text='<%#: Eval("RecallsAllowRecallsToExtendOverdueLoans") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsAlternateRecallReturnIntervalDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsAlternateRecallReturnIntervalDurationLabel" runat="server" Text="Recalls Alternate Recall Return Interval Duration:" AssociatedControlID="RecallsAlternateRecallReturnIntervalDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsAlternateRecallReturnIntervalDurationLiteral" runat="server" Text='<%#: Eval("RecallsAlternateRecallReturnIntervalDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecallsAlternateRecallReturnIntervalInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RecallsAlternateRecallReturnIntervalIntervalLabel" runat="server" Text="Recalls Alternate Recall Return Interval Interval:" AssociatedControlID="RecallsAlternateRecallReturnIntervalIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecallsAlternateRecallReturnIntervalIntervalLiteral" runat="server" Text='<%#: Eval("RecallsAlternateRecallReturnIntervalInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldsAlternateCheckoutLoanPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="HoldsAlternateCheckoutLoanPeriodDurationLabel" runat="server" Text="Holds Alternate Checkout Loan Period Duration:" AssociatedControlID="HoldsAlternateCheckoutLoanPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldsAlternateCheckoutLoanPeriodDurationLiteral" runat="server" Text='<%#: Eval("HoldsAlternateCheckoutLoanPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldsAlternateCheckoutLoanPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="HoldsAlternateCheckoutLoanPeriodIntervalLabel" runat="server" Text="Holds Alternate Checkout Loan Period Interval:" AssociatedControlID="HoldsAlternateCheckoutLoanPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldsAlternateCheckoutLoanPeriodIntervalLiteral" runat="server" Text='<%#: Eval("HoldsAlternateCheckoutLoanPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldsRenewItemsWithRequest") != null %>'>
                                <td>
                                    <asp:Label ID="HoldsRenewItemsWithRequestLabel" runat="server" Text="Holds Renew Items With Request:" AssociatedControlID="HoldsRenewItemsWithRequestLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldsRenewItemsWithRequestLiteral" runat="server" Text='<%#: Eval("HoldsRenewItemsWithRequest") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldsAlternateRenewalLoanPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="HoldsAlternateRenewalLoanPeriodDurationLabel" runat="server" Text="Holds Alternate Renewal Loan Period Duration:" AssociatedControlID="HoldsAlternateRenewalLoanPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldsAlternateRenewalLoanPeriodDurationLiteral" runat="server" Text='<%#: Eval("HoldsAlternateRenewalLoanPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldsAlternateRenewalLoanPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="HoldsAlternateRenewalLoanPeriodIntervalLabel" runat="server" Text="Holds Alternate Renewal Loan Period Interval:" AssociatedControlID="HoldsAlternateRenewalLoanPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldsAlternateRenewalLoanPeriodIntervalLiteral" runat="server" Text='<%#: Eval("HoldsAlternateRenewalLoanPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PagesAlternateCheckoutLoanPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="PagesAlternateCheckoutLoanPeriodDurationLabel" runat="server" Text="Pages Alternate Checkout Loan Period Duration:" AssociatedControlID="PagesAlternateCheckoutLoanPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PagesAlternateCheckoutLoanPeriodDurationLiteral" runat="server" Text='<%#: Eval("PagesAlternateCheckoutLoanPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PagesAlternateCheckoutLoanPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="PagesAlternateCheckoutLoanPeriodIntervalLabel" runat="server" Text="Pages Alternate Checkout Loan Period Interval:" AssociatedControlID="PagesAlternateCheckoutLoanPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PagesAlternateCheckoutLoanPeriodIntervalLiteral" runat="server" Text='<%#: Eval("PagesAlternateCheckoutLoanPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PagesRenewItemsWithRequest") != null %>'>
                                <td>
                                    <asp:Label ID="PagesRenewItemsWithRequestLabel" runat="server" Text="Pages Renew Items With Request:" AssociatedControlID="PagesRenewItemsWithRequestLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PagesRenewItemsWithRequestLiteral" runat="server" Text='<%#: Eval("PagesRenewItemsWithRequest") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PagesAlternateRenewalLoanPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="PagesAlternateRenewalLoanPeriodDurationLabel" runat="server" Text="Pages Alternate Renewal Loan Period Duration:" AssociatedControlID="PagesAlternateRenewalLoanPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PagesAlternateRenewalLoanPeriodDurationLiteral" runat="server" Text='<%#: Eval("PagesAlternateRenewalLoanPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PagesAlternateRenewalLoanPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="PagesAlternateRenewalLoanPeriodIntervalLabel" runat="server" Text="Pages Alternate Renewal Loan Period Interval:" AssociatedControlID="PagesAlternateRenewalLoanPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PagesAlternateRenewalLoanPeriodIntervalLiteral" runat="server" Text='<%#: Eval("PagesAlternateRenewalLoanPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationTime") != null %>'>
                                <td>
                                    <asp:Label ID="CreationTimeLabel" runat="server" Text="Creation Time:" AssociatedControlID="CreationTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreationTimeLiteral" runat="server" Text='<%# Eval("CreationTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationUser") != null %>'>
                                <td>
                                    <asp:Label ID="CreationUserLabel" runat="server" Text="Creation User:" AssociatedControlID="CreationUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteTimeLabel" runat="server" Text="Last Write Time:" AssociatedControlID="LastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastWriteTimeLiteral" runat="server" Text='<%# Eval("LastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteUser") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteUserLabel" runat="server" Text="Last Write User:" AssociatedControlID="LastWriteUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Content") != null %>'>
                                <td>
                                    <asp:Label ID="ContentLabel" runat="server" Text="Content:" AssociatedControlID="ContentLiteral" />
                                </td>
                                <td>
                                    <pre><asp:Literal ID="ContentLiteral" runat="server" Text='<%#: Eval("Content") %>' /></pre>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Loan2sPanel" runat="server" Visible='<%# (string)Session["Loan2sPermission"] != null && LoanPolicy2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Loan2sHyperLink" runat="server" Text="Loans" NavigateUrl="~/Loan2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Loan2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Loan2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loans found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proxy User" DataField="ProxyUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProxyUserHyperLink" runat="server" Text='<%#: Eval("ProxyUserId") != null ? Eval("ProxyUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ProxyUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Effective Location At Check Out" DataField="ItemEffectiveLocationAtCheckOut.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemEffectiveLocationAtCheckOutHyperLink" runat="server" Text='<%#: Eval("ItemEffectiveLocationAtCheckOut.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemEffectiveLocationAtCheckOutId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loan Time" DataField="LoanTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Return Time" DataField="ReturnTime" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="System Return Time" DataField="SystemReturnTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Action" DataField="Action" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Action Comment" DataField="ActionComment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status" DataField="ItemStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewal Count" DataField="RenewalCount" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Checkout Service Point" DataField="CheckoutServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckoutServicePointHyperLink" runat="server" Text='<%#: Eval("CheckoutServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckoutServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checkin Service Point" DataField="CheckinServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckinServicePointHyperLink" runat="server" Text='<%#: Eval("CheckinServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckinServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" DataField="Group.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="GroupHyperLink" runat="server" Text='<%#: Eval("Group.Name") %>' NavigateUrl='<%# $"~/Group2s/Edit.aspx?Id={Eval("GroupId")}" %>' Enabled='<%# Session["Group2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Due Date Changed By Recall" DataField="DueDateChangedByRecall" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Declared Lost Date" DataField="DeclaredLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Claimed Returned Date" DataField="ClaimedReturnedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Overdue Fine Policy" DataField="OverdueFinePolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OverdueFinePolicyHyperLink" runat="server" Text='<%#: Eval("OverdueFinePolicy.Name") %>' NavigateUrl='<%# $"~/OverdueFinePolicy2s/Edit.aspx?Id={Eval("OverdueFinePolicyId")}" %>' Enabled='<%# Session["OverdueFinePolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Lost Item Policy" DataField="LostItemPolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LostItemPolicyHyperLink" runat="server" Text='<%#: Eval("LostItemPolicy.Name") %>' NavigateUrl='<%# $"~/LostItemFeePolicy2s/Edit.aspx?Id={Eval("LostItemPolicyId")}" %>' Enabled='<%# Session["LostItemFeePolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Creation User" DataField="CreationUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Write User" DataField="LastWriteUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Lost Item Has Been Billed" DataField="AgedToLostDelayedBillingLostItemHasBeenBilled" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Date Lost Item Should Be Billed" DataField="AgedToLostDelayedBillingDateLostItemShouldBeBilled" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Aged To Lost Date" DataField="AgedToLostDelayedBillingAgedToLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Loan2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Loan2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
