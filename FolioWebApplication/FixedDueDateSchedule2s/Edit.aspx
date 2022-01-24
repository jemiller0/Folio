<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.FixedDueDateSchedule2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="FixedDueDateSchedule2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="FixedDueDateSchedule2HyperLink" runat="server" Text="Fixed Due Date Schedule" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="FixedDueDateSchedule2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="FixedDueDateSchedule2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewFixedDueDateSchedule2Panel" runat="server">
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
    <asp:Panel ID="FixedDueDateScheduleSchedulesPanel" runat="server" Visible='<%# (string)Session["FixedDueDateScheduleSchedulesPermission"] != null && FixedDueDateSchedule2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="FixedDueDateScheduleSchedulesHyperLink" runat="server" Text="Fixed Due Date Schedule Schedules" NavigateUrl="~/FixedDueDateScheduleSchedules/Default.aspx" /></legend>
            <telerik:RadGrid ID="FixedDueDateScheduleSchedulesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="FixedDueDateScheduleSchedulesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No fixed due date schedule schedules found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="From" DataField="From" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="To" DataField="To" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Due" DataField="Due" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LoanPolicy2sPanel" runat="server" Visible='<%# (string)Session["LoanPolicy2sPermission"] != null && FixedDueDateSchedule2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LoanPolicy2sHyperLink" runat="server" Text="Loan Policys" NavigateUrl="~/LoanPolicy2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LoanPolicy2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LoanPolicy2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loan policies found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loanable" DataField="Loanable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Profile Id" DataField="LoansPolicyProfileId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Period Duration" DataField="LoansPolicyPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Period Interval" DataField="LoansPolicyPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Closed Library Due Date Management Id" DataField="LoansPolicyClosedLibraryDueDateManagementId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Grace Period Duration" DataField="LoansPolicyGracePeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Grace Period Interval" DataField="LoansPolicyGracePeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Opening Time Offset Duration" DataField="LoansPolicyOpeningTimeOffsetDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Opening Time Offset Interval" DataField="LoansPolicyOpeningTimeOffsetInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Loans Policy Fixed Due Date Schedule" DataField="LoansPolicyFixedDueDateSchedule.Name" SortExpression="LoansPolicyFixedDueDateSchedule.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoansPolicyFixedDueDateScheduleHyperLink" runat="server" Text='<%#: Eval("LoansPolicyFixedDueDateSchedule.Name") %>' NavigateUrl='<%# $"~/FixedDueDateSchedule2s/Edit.aspx?Id={Eval("LoansPolicyFixedDueDateScheduleId")}" %>' Enabled='<%# Session["FixedDueDateSchedule2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Loans Policy Item Limit" DataField="LoansPolicyItemLimit" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewable" DataField="Renewable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Unlimited" DataField="RenewalsPolicyUnlimited" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Number Allowed" DataField="RenewalsPolicyNumberAllowed" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Renew From Id" DataField="RenewalsPolicyRenewFromId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Different Period" DataField="RenewalsPolicyDifferentPeriod" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Period Duration" DataField="RenewalsPolicyPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Period Interval" DataField="RenewalsPolicyPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Grace Period Duration" DataField="RecallsAlternateGracePeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Grace Period Interval" DataField="RecallsAlternateGracePeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Minimum Guaranteed Loan Period Duration" DataField="RecallsMinimumGuaranteedLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Minimum Guaranteed Loan Period Interval" DataField="RecallsMinimumGuaranteedLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Recall Return Interval Duration" DataField="RecallsRecallReturnIntervalDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Recall Return Interval Interval" DataField="RecallsRecallReturnIntervalInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Allow Recalls To Extend Overdue Loans" DataField="RecallsAllowRecallsToExtendOverdueLoans" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Recall Return Interval Duration" DataField="RecallsAlternateRecallReturnIntervalDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Recall Return Interval Interval" DataField="RecallsAlternateRecallReturnIntervalInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Checkout Loan Period Duration" DataField="HoldsAlternateCheckoutLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Checkout Loan Period Interval" DataField="HoldsAlternateCheckoutLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Holds Renew Items With Request" DataField="HoldsRenewItemsWithRequest" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Renewal Loan Period Duration" DataField="HoldsAlternateRenewalLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Renewal Loan Period Interval" DataField="HoldsAlternateRenewalLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Checkout Loan Period Duration" DataField="PagesAlternateCheckoutLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Checkout Loan Period Interval" DataField="PagesAlternateCheckoutLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pages Renew Items With Request" DataField="PagesRenewItemsWithRequest" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Renewal Loan Period Duration" DataField="PagesAlternateRenewalLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Renewal Loan Period Interval" DataField="PagesAlternateRenewalLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LoanPolicy2s1Panel" runat="server" Visible='<%# (string)Session["LoanPolicy2sPermission"] != null && FixedDueDateSchedule2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LoanPolicy2s1HyperLink" runat="server" Text="Loan Policys 1" NavigateUrl="~/LoanPolicy2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LoanPolicy2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LoanPolicy2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loan policies found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loanable" DataField="Loanable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Profile Id" DataField="LoansPolicyProfileId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Period Duration" DataField="LoansPolicyPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Period Interval" DataField="LoansPolicyPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Closed Library Due Date Management Id" DataField="LoansPolicyClosedLibraryDueDateManagementId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Grace Period Duration" DataField="LoansPolicyGracePeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Grace Period Interval" DataField="LoansPolicyGracePeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Opening Time Offset Duration" DataField="LoansPolicyOpeningTimeOffsetDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Opening Time Offset Interval" DataField="LoansPolicyOpeningTimeOffsetInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loans Policy Item Limit" DataField="LoansPolicyItemLimit" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewable" DataField="Renewable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Unlimited" DataField="RenewalsPolicyUnlimited" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Number Allowed" DataField="RenewalsPolicyNumberAllowed" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Renew From Id" DataField="RenewalsPolicyRenewFromId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Different Period" DataField="RenewalsPolicyDifferentPeriod" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Period Duration" DataField="RenewalsPolicyPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewals Policy Period Interval" DataField="RenewalsPolicyPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Renewals Policy Alternate Fixed Due Date Schedule" DataField="RenewalsPolicyAlternateFixedDueDateSchedule.Name" SortExpression="RenewalsPolicyAlternateFixedDueDateSchedule.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RenewalsPolicyAlternateFixedDueDateScheduleHyperLink" runat="server" Text='<%#: Eval("RenewalsPolicyAlternateFixedDueDateSchedule.Name") %>' NavigateUrl='<%# $"~/FixedDueDateSchedule2s/Edit.aspx?Id={Eval("RenewalsPolicyAlternateFixedDueDateScheduleId")}" %>' Enabled='<%# Session["FixedDueDateSchedule2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Grace Period Duration" DataField="RecallsAlternateGracePeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Grace Period Interval" DataField="RecallsAlternateGracePeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Minimum Guaranteed Loan Period Duration" DataField="RecallsMinimumGuaranteedLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Minimum Guaranteed Loan Period Interval" DataField="RecallsMinimumGuaranteedLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Recall Return Interval Duration" DataField="RecallsRecallReturnIntervalDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Recall Return Interval Interval" DataField="RecallsRecallReturnIntervalInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalls Allow Recalls To Extend Overdue Loans" DataField="RecallsAllowRecallsToExtendOverdueLoans" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Recall Return Interval Duration" DataField="RecallsAlternateRecallReturnIntervalDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalls Alternate Recall Return Interval Interval" DataField="RecallsAlternateRecallReturnIntervalInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Checkout Loan Period Duration" DataField="HoldsAlternateCheckoutLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Checkout Loan Period Interval" DataField="HoldsAlternateCheckoutLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Holds Renew Items With Request" DataField="HoldsRenewItemsWithRequest" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Renewal Loan Period Duration" DataField="HoldsAlternateRenewalLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Holds Alternate Renewal Loan Period Interval" DataField="HoldsAlternateRenewalLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Checkout Loan Period Duration" DataField="PagesAlternateCheckoutLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Checkout Loan Period Interval" DataField="PagesAlternateCheckoutLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pages Renew Items With Request" DataField="PagesRenewItemsWithRequest" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Renewal Loan Period Duration" DataField="PagesAlternateRenewalLoanPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Pages Alternate Renewal Loan Period Interval" DataField="PagesAlternateRenewalLoanPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="FixedDueDateScheduleSchedulesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="FixedDueDateScheduleSchedulesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LoanPolicy2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LoanPolicy2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LoanPolicy2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LoanPolicy2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
