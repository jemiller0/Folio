<%@ Page Title="Loan Policies" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.LoanPolicy2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LoanPolicy2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="LoanPolicy2sHyperLink" runat="server" Text="Loan Policies" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="LoanPolicy2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="LoanPolicy2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loan policies found" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RadAjaxManager1_ClientEvents_OnRequestStart(ajaxManager, eventArgs) {
                if (eventArgs.get_eventTarget().indexOf("Export") != -1) eventArgs.set_enableAjax(false);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="RadAjaxManager1_ClientEvents_OnRequestStart">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ExportLinkButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LoanPolicy2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LoanPolicy2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LoanPolicy2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
