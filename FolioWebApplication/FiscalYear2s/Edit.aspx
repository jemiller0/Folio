<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.FiscalYear2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="FiscalYear2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="FiscalYear2HyperLink" runat="server" Text="Fiscal Year" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="FiscalYear2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="FiscalYear2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewFiscalYear2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Code") != null %>'>
                                <td>
                                    <asp:Label ID="CodeLabel" runat="server" Text="Code:" AssociatedControlID="CodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CodeLiteral" runat="server" Text='<%#: Eval("Code") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Currency") != null %>'>
                                <td>
                                    <asp:Label ID="CurrencyLabel" runat="server" Text="Currency:" AssociatedControlID="CurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CurrencyLiteral" runat="server" Text='<%#: Eval("Currency") %>' />
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
                            <tr runat="server" visible='<%# Eval("StartDate") != null %>'>
                                <td>
                                    <asp:Label ID="StartDateLabel" runat="server" Text="Start Date:" AssociatedControlID="StartDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StartDateLiteral" runat="server" Text='<%# Eval("StartDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EndDate") != null %>'>
                                <td>
                                    <asp:Label ID="EndDateLabel" runat="server" Text="End Date:" AssociatedControlID="EndDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EndDateLiteral" runat="server" Text='<%# Eval("EndDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Series") != null %>'>
                                <td>
                                    <asp:Label ID="SeriesLabel" runat="server" Text="Series:" AssociatedControlID="SeriesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SeriesLiteral" runat="server" Text='<%#: Eval("Series") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryAllocated") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryAllocatedLabel" runat="server" Text="Financial Summary Allocated:" AssociatedControlID="FinancialSummaryAllocatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryAllocatedLiteral" runat="server" Text='<%# Eval("FinancialSummaryAllocated", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryAvailable") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryAvailableLabel" runat="server" Text="Financial Summary Available:" AssociatedControlID="FinancialSummaryAvailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryAvailableLiteral" runat="server" Text='<%# Eval("FinancialSummaryAvailable", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryUnavailable") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryUnavailableLabel" runat="server" Text="Financial Summary Unavailable:" AssociatedControlID="FinancialSummaryUnavailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryUnavailableLiteral" runat="server" Text='<%# Eval("FinancialSummaryUnavailable", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryInitialAllocation") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryInitialAllocationLabel" runat="server" Text="Financial Summary Initial Allocation:" AssociatedControlID="FinancialSummaryInitialAllocationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryInitialAllocationLiteral" runat="server" Text='<%# Eval("FinancialSummaryInitialAllocation", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryAllocationTo") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryAllocationToLabel" runat="server" Text="Financial Summary Allocation To:" AssociatedControlID="FinancialSummaryAllocationToLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryAllocationToLiteral" runat="server" Text='<%# Eval("FinancialSummaryAllocationTo", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryAllocationFrom") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryAllocationFromLabel" runat="server" Text="Financial Summary Allocation From:" AssociatedControlID="FinancialSummaryAllocationFromLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryAllocationFromLiteral" runat="server" Text='<%# Eval("FinancialSummaryAllocationFrom", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryTotalFunding") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryTotalFundingLabel" runat="server" Text="Financial Summary Total Funding:" AssociatedControlID="FinancialSummaryTotalFundingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryTotalFundingLiteral" runat="server" Text='<%# Eval("FinancialSummaryTotalFunding", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryCashBalance") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryCashBalanceLabel" runat="server" Text="Financial Summary Cash Balance:" AssociatedControlID="FinancialSummaryCashBalanceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryCashBalanceLiteral" runat="server" Text='<%# Eval("FinancialSummaryCashBalance", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryAwaitingPayment") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryAwaitingPaymentLabel" runat="server" Text="Financial Summary Awaiting Payment:" AssociatedControlID="FinancialSummaryAwaitingPaymentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryAwaitingPaymentLiteral" runat="server" Text='<%# Eval("FinancialSummaryAwaitingPayment", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryEncumbered") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryEncumberedLabel" runat="server" Text="Financial Summary Encumbered:" AssociatedControlID="FinancialSummaryEncumberedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryEncumberedLiteral" runat="server" Text='<%# Eval("FinancialSummaryEncumbered", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryExpenditures") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryExpendituresLabel" runat="server" Text="Financial Summary Expenditures:" AssociatedControlID="FinancialSummaryExpendituresLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryExpendituresLiteral" runat="server" Text='<%# Eval("FinancialSummaryExpenditures", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryOverEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryOverEncumbranceLabel" runat="server" Text="Financial Summary Over Encumbrance:" AssociatedControlID="FinancialSummaryOverEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryOverEncumbranceLiteral" runat="server" Text='<%# Eval("FinancialSummaryOverEncumbrance", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialSummaryOverExpended") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialSummaryOverExpendedLabel" runat="server" Text="Financial Summary Over Expended:" AssociatedControlID="FinancialSummaryOverExpendedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialSummaryOverExpendedLiteral" runat="server" Text='<%# Eval("FinancialSummaryOverExpended", "{0:c}") %>' />
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
    <asp:Panel ID="Budget2sPanel" runat="server" Visible='<%# (string)Session["Budget2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Budget2sHyperLink" runat="server" Text="Budgets" NavigateUrl="~/Budget2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Budget2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Budget2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No budgets found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Budget Status" DataField="BudgetStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Allowable Encumbrance" DataField="AllowableEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allowable Expenditure" DataField="AllowableExpenditure" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allocated" DataField="Allocated" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment" DataField="AwaitingPayment" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Available" DataField="Available" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Encumbered" DataField="Encumbered" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Expenditures" DataField="Expenditures" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Net Transfers" DataField="NetTransfers" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Unavailable" DataField="Unavailable" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Over Encumbrance" DataField="OverEncumbrance" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Over Expended" DataField="OverExpended" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn HeaderText="Fund" DataField="Fund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundHyperLink" runat="server" Text='<%#: Eval("Fund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Initial Allocation" DataField="InitialAllocation" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Allocation To" DataField="AllocationTo" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Allocation From" DataField="AllocationFrom" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Total Funding" DataField="TotalFunding" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Cash Balance" DataField="CashBalance" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="BudgetGroup2sPanel" runat="server" Visible='<%# (string)Session["BudgetGroup2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BudgetGroup2sHyperLink" runat="server" Text="Budget Groups" NavigateUrl="~/BudgetGroup2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="BudgetGroup2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BudgetGroup2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No budget groups found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Id" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/BudgetGroup2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/BudgetGroup2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget" DataField="Budget.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BudgetHyperLink" runat="server" Text='<%#: Eval("Budget.Name") %>' NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("BudgetId")}" %>' Enabled='<%# Session["Budget2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" DataField="Group.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="GroupHyperLink" runat="server" Text='<%#: Eval("Group.Name") %>' NavigateUrl='<%# $"~/FinanceGroup2s/Edit.aspx?Id={Eval("GroupId")}" %>' Enabled='<%# Session["FinanceGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fund" DataField="Fund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundHyperLink" runat="server" Text='<%#: Eval("Fund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="FiscalYearAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["FiscalYearAcquisitionsUnitsPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="FiscalYearAcquisitionsUnitsHyperLink" runat="server" Text="Fiscal Year Acquisitions Units" NavigateUrl="~/FiscalYearAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="FiscalYearAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="FiscalYearAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No fiscal year acquisitions units found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Acquisitions Unit" DataField="AcquisitionsUnit.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AcquisitionsUnitHyperLink" runat="server" Text='<%#: Eval("AcquisitionsUnit.Name") %>' NavigateUrl='<%# $"~/AcquisitionsUnit2s/Edit.aspx?Id={Eval("AcquisitionsUnitId")}" %>' Enabled='<%# Session["AcquisitionsUnit2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Ledger2sPanel" runat="server" Visible='<%# (string)Session["Ledger2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Ledger2sHyperLink" runat="server" Text="Ledgers" NavigateUrl="~/Ledger2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Ledger2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Ledger2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledgers found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Code" DataField="Code" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Ledger Status" DataField="LedgerStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Allocated" DataField="Allocated" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Available" DataField="Available" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Net Transfers" DataField="NetTransfers" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Unavailable" DataField="Unavailable" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Restrict Encumbrance" DataField="RestrictEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Restrict Expenditures" DataField="RestrictExpenditures" AutoPostBackOnFilter="true" />
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
                        <telerik:GridBoundColumn HeaderText="Initial Allocation" DataField="InitialAllocation" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Allocation To" DataField="AllocationTo" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Allocation From" DataField="AllocationFrom" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Total Funding" DataField="TotalFunding" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Cash Balance" DataField="CashBalance" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment" DataField="AwaitingPayment" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Encumbered" DataField="Encumbered" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Expenditures" DataField="Expenditures" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Over Encumbrance" DataField="OverEncumbrance" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Over Expended" DataField="OverExpended" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LedgerRollover2sPanel" runat="server" Visible='<%# (string)Session["LedgerRollover2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRollover2sHyperLink" runat="server" Text="Ledger Rollovers" NavigateUrl="~/LedgerRollover2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LedgerRollover2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRollover2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollovers found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LedgerRollover2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/LedgerRollover2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ledger" DataField="Ledger.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LedgerHyperLink" runat="server" Text='<%#: Eval("Ledger.Name") %>' NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("LedgerId")}" %>' Enabled='<%# Session["Ledger2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Fiscal Year" DataField="ToFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFiscalYearHyperLink" runat="server" Text='<%#: Eval("ToFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("ToFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Restrict Encumbrance" DataField="RestrictEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Restrict Expenditures" DataField="RestrictExpenditures" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Need Close Budgets" DataField="NeedCloseBudgets" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency Factor" DataField="CurrencyFactor" AutoPostBackOnFilter="true" />
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
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LedgerRollover2s1Panel" runat="server" Visible='<%# (string)Session["LedgerRollover2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRollover2s1HyperLink" runat="server" Text="Ledger Rollovers 1" NavigateUrl="~/LedgerRollover2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LedgerRollover2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRollover2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollovers found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LedgerRollover2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/LedgerRollover2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ledger" DataField="Ledger.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LedgerHyperLink" runat="server" Text='<%#: Eval("Ledger.Name") %>' NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("LedgerId")}" %>' Enabled='<%# Session["Ledger2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Fiscal Year" DataField="FromFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFiscalYearHyperLink" runat="server" Text='<%#: Eval("FromFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FromFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Restrict Encumbrance" DataField="RestrictEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Restrict Expenditures" DataField="RestrictExpenditures" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Need Close Budgets" DataField="NeedCloseBudgets" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency Factor" DataField="CurrencyFactor" AutoPostBackOnFilter="true" />
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
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Transaction2sPanel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2sHyperLink" runat="server" Text="Transactions" NavigateUrl="~/Transaction2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Transaction2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="Transaction2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transactions found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true" Aggregate="Sum" FooterAggregateFormatString="{0:c}">
                            <ItemTemplate>
                                <asp:HyperLink ID="AmountHyperLink" runat="server" Text='<%# $"{Eval("Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Awaiting Payment Encumbrance" DataField="AwaitingPaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="AwaitingPaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("AwaitingPaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("AwaitingPaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Release Encumbrance" DataField="AwaitingPaymentReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Amount" DataField="AwaitingPaymentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Expended Amount" DataField="ExpendedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Initial Encumbered Amount" DataField="InitialEncumberedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Status" DataField="OrderStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription" DataField="Subscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Re Encumber" DataField="ReEncumber" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense Class" DataField="ExpenseClass.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fiscal Year" DataField="FiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Fund" DataField="FromFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Cancelled" DataField="InvoiceCancelled" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Payment Encumbrance" DataField="PaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("PaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Invoice" DataField="Invoice.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Item" DataField="InvoiceItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItemId") != null ? Eval("InvoiceItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Fund" DataField="ToFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Transaction Type" DataField="TransactionType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Transaction2s1Panel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && FiscalYear2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2s1HyperLink" runat="server" Text="Transactions 1" NavigateUrl="~/Transaction2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Transaction2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="Transaction2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transactions found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true" Aggregate="Sum" FooterAggregateFormatString="{0:c}">
                            <ItemTemplate>
                                <asp:HyperLink ID="AmountHyperLink" runat="server" Text='<%# $"{Eval("Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Awaiting Payment Encumbrance" DataField="AwaitingPaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="AwaitingPaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("AwaitingPaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("AwaitingPaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Release Encumbrance" DataField="AwaitingPaymentReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Amount" DataField="AwaitingPaymentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Expended Amount" DataField="ExpendedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Initial Encumbered Amount" DataField="InitialEncumberedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Status" DataField="OrderStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription" DataField="Subscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Re Encumber" DataField="ReEncumber" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense Class" DataField="ExpenseClass.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Fund" DataField="FromFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Cancelled" DataField="InvoiceCancelled" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Payment Encumbrance" DataField="PaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("PaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Source Fiscal Year" DataField="SourceFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceFiscalYearHyperLink" runat="server" Text='<%#: Eval("SourceFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("SourceFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" DataField="Invoice.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Item" DataField="InvoiceItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItemId") != null ? Eval("InvoiceItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Fund" DataField="ToFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Transaction Type" DataField="TransactionType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Budget2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Budget2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BudgetGroup2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetGroup2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="FiscalYearAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="FiscalYearAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Ledger2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Ledger2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRollover2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRollover2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRollover2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRollover2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
