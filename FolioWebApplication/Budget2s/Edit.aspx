<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Budget2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Budget2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Budget2HyperLink" runat="server" Text="Budget" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Budget2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Budget2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewBudget2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("BudgetStatus") != null %>'>
                                <td>
                                    <asp:Label ID="BudgetStatusLabel" runat="server" Text="Budget Status:" AssociatedControlID="BudgetStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BudgetStatusLiteral" runat="server" Text='<%#: Eval("BudgetStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllowableEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="AllowableEncumbranceLabel" runat="server" Text="Allowable Encumbrance:" AssociatedControlID="AllowableEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllowableEncumbranceLiteral" runat="server" Text='<%#: Eval("AllowableEncumbrance") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllowableExpenditure") != null %>'>
                                <td>
                                    <asp:Label ID="AllowableExpenditureLabel" runat="server" Text="Allowable Expenditure:" AssociatedControlID="AllowableExpenditureLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllowableExpenditureLiteral" runat="server" Text='<%#: Eval("AllowableExpenditure") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Allocated") != null %>'>
                                <td>
                                    <asp:Label ID="AllocatedLabel" runat="server" Text="Allocated:" AssociatedControlID="AllocatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllocatedLiteral" runat="server" Text='<%# Eval("Allocated", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AwaitingPayment") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentLabel" runat="server" Text="Awaiting Payment:" AssociatedControlID="AwaitingPaymentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPaymentLiteral" runat="server" Text='<%# Eval("AwaitingPayment", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Available") != null %>'>
                                <td>
                                    <asp:Label ID="AvailableLabel" runat="server" Text="Available:" AssociatedControlID="AvailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AvailableLiteral" runat="server" Text='<%# Eval("Available", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Credits") != null %>'>
                                <td>
                                    <asp:Label ID="CreditsLabel" runat="server" Text="Credits:" AssociatedControlID="CreditsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreditsLiteral" runat="server" Text='<%#: Eval("Credits") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Encumbered") != null %>'>
                                <td>
                                    <asp:Label ID="EncumberedLabel" runat="server" Text="Encumbered:" AssociatedControlID="EncumberedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EncumberedLiteral" runat="server" Text='<%# Eval("Encumbered", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Expenditures") != null %>'>
                                <td>
                                    <asp:Label ID="ExpendituresLabel" runat="server" Text="Expenditures:" AssociatedControlID="ExpendituresLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpendituresLiteral" runat="server" Text='<%# Eval("Expenditures", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NetTransfers") != null %>'>
                                <td>
                                    <asp:Label ID="NetTransfersLabel" runat="server" Text="Net Transfers:" AssociatedControlID="NetTransfersLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NetTransfersLiteral" runat="server" Text='<%# Eval("NetTransfers", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Unavailable") != null %>'>
                                <td>
                                    <asp:Label ID="UnavailableLabel" runat="server" Text="Unavailable:" AssociatedControlID="UnavailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UnavailableLiteral" runat="server" Text='<%# Eval("Unavailable", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OverEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="OverEncumbranceLabel" runat="server" Text="Over Encumbrance:" AssociatedControlID="OverEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OverEncumbranceLiteral" runat="server" Text='<%# Eval("OverEncumbrance", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OverExpended") != null %>'>
                                <td>
                                    <asp:Label ID="OverExpendedLabel" runat="server" Text="Over Expended:" AssociatedControlID="OverExpendedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OverExpendedLiteral" runat="server" Text='<%# Eval("OverExpended", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Fund") != null %>'>
                                <td>
                                    <asp:Label ID="FundLabel" runat="server" Text="Fund:" AssociatedControlID="FundHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FundHyperLink" runat="server" Text='<%#: Eval("Fund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="FiscalYearLabel" runat="server" Text="Fiscal Year:" AssociatedControlID="FiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("InitialAllocation") != null %>'>
                                <td>
                                    <asp:Label ID="InitialAllocationLabel" runat="server" Text="Initial Allocation:" AssociatedControlID="InitialAllocationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InitialAllocationLiteral" runat="server" Text='<%# Eval("InitialAllocation", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllocationTo") != null %>'>
                                <td>
                                    <asp:Label ID="AllocationToLabel" runat="server" Text="Allocation To:" AssociatedControlID="AllocationToLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllocationToLiteral" runat="server" Text='<%# Eval("AllocationTo", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllocationFrom") != null %>'>
                                <td>
                                    <asp:Label ID="AllocationFromLabel" runat="server" Text="Allocation From:" AssociatedControlID="AllocationFromLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllocationFromLiteral" runat="server" Text='<%# Eval("AllocationFrom", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TotalFunding") != null %>'>
                                <td>
                                    <asp:Label ID="TotalFundingLabel" runat="server" Text="Total Funding:" AssociatedControlID="TotalFundingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TotalFundingLiteral" runat="server" Text='<%# Eval("TotalFunding", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CashBalance") != null %>'>
                                <td>
                                    <asp:Label ID="CashBalanceLabel" runat="server" Text="Cash Balance:" AssociatedControlID="CashBalanceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CashBalanceLiteral" runat="server" Text='<%# Eval("CashBalance", "{0:c}") %>' />
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
    <asp:Panel ID="BudgetAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["BudgetAcquisitionsUnitsPermission"] != null && Budget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BudgetAcquisitionsUnitsHyperLink" runat="server" Text="Budget Acquisitions Units" NavigateUrl="~/BudgetAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="BudgetAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BudgetAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No budget acquisitions units found">
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
    <asp:Panel ID="BudgetExpenseClass2sPanel" runat="server" Visible='<%# (string)Session["BudgetExpenseClass2sPermission"] != null && Budget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BudgetExpenseClass2sHyperLink" runat="server" Text="Budget Expense Classs" NavigateUrl="~/BudgetExpenseClass2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="BudgetExpenseClass2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BudgetExpenseClass2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No budget expense classes found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Id" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/BudgetExpenseClass2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/BudgetExpenseClass2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense Class" DataField="ExpenseClass.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="BudgetGroup2sPanel" runat="server" Visible='<%# (string)Session["BudgetGroup2sPermission"] != null && Budget2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn HeaderText="Group" DataField="Group.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="GroupHyperLink" runat="server" Text='<%#: Eval("Group.Name") %>' NavigateUrl='<%# $"~/FinanceGroup2s/Edit.aspx?Id={Eval("GroupId")}" %>' Enabled='<%# Session["FinanceGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fiscal Year" DataField="FiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
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
    <asp:Panel ID="BudgetTagsPanel" runat="server" Visible='<%# (string)Session["BudgetTagsPermission"] != null && Budget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BudgetTagsHyperLink" runat="server" Text="Budget Tags" NavigateUrl="~/BudgetTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="BudgetTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BudgetTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No budget tags found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Tag" DataField="Tag" AutoPostBackOnFilter="true" AllowSorting="false" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudget2sPanel" runat="server" Visible='<%# (string)Session["RolloverBudget2sPermission"] != null && Budget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudget2sHyperLink" runat="server" Text="Rollover Budgets" NavigateUrl="~/RolloverBudget2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="RolloverBudget2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudget2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budgets found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/RolloverBudget2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/RolloverBudget2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rollover" DataField="Rollover.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="RolloverHyperLink" runat="server" Text='<%# Eval("RolloverId") != null ? Eval("Rollover.Id") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Rollover2s/Edit.aspx?Id={Eval("RolloverId")}" %>' Enabled='<%# Session["Rollover2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/RolloverBudget2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Fund Details Name" DataField="FundDetailsName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Details Code" DataField="FundDetailsCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Details Fund Status" DataField="FundDetailsFundStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Fund Details Fund Type" DataField="FundDetailsFundType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundDetailsFundTypeHyperLink" runat="server" Text='<%#: Eval("FundDetailsFundType.Name") %>' NavigateUrl='<%# $"~/FundType2s/Edit.aspx?Id={Eval("FundDetailsFundTypeId")}" %>' Enabled='<%# Session["FundType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Fund Details Fund Type Name" DataField="FundDetailsFundTypeName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Details External Account No" DataField="FundDetailsExternalAccountNo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Details Description" DataField="FundDetailsDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Details Restrict By Locations" DataField="FundDetailsRestrictByLocations" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Budget Status" DataField="BudgetStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Allowable Encumbrance" DataField="AllowableEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allowable Expenditure" DataField="AllowableExpenditure" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allocated" DataField="Allocated" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment" DataField="AwaitingPayment" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Available" DataField="Available" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Credits" DataField="Credits" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Encumbered" DataField="Encumbered" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Expenditures" DataField="Expenditures" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Net Transfers" DataField="NetTransfers" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Unavailable" DataField="Unavailable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Over Encumbrance" DataField="OverEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Over Expended" DataField="OverExpended" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Fund" DataField="Fund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundHyperLink" runat="server" Text='<%#: Eval("Fund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fiscal Year" DataField="FiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Initial Allocation" DataField="InitialAllocation" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allocation To" DataField="AllocationTo" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Allocation From" DataField="AllocationFrom" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Total Funding" DataField="TotalFunding" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cash Balance" DataField="CashBalance" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BudgetAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BudgetExpenseClass2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetExpenseClass2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BudgetGroup2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetGroup2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BudgetTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudget2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudget2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
