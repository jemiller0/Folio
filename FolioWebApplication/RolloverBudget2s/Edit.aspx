<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.RolloverBudget2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="RolloverBudget2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudget2HyperLink" runat="server" Text="Rollover Budget" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="RolloverBudget2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="RolloverBudget2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewRolloverBudget2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Budget") != null %>'>
                                <td>
                                    <asp:Label ID="BudgetLabel" runat="server" Text="Budget:" AssociatedControlID="BudgetHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BudgetHyperLink" runat="server" Text='<%#: Eval("Budget.Name") %>' NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("BudgetId")}" %>' Enabled='<%# Session["Budget2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Rollover") != null %>'>
                                <td>
                                    <asp:Label ID="RolloverLabel" runat="server" Text="Rollover:" AssociatedControlID="RolloverHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RolloverHyperLink" runat="server" Text='<%# Eval("Rollover.Id") %>' NavigateUrl='<%# $"~/Rollover2s/Edit.aspx?Id={Eval("RolloverId")}" %>' Enabled='<%# Session["Rollover2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("FundDetailsName") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsNameLabel" runat="server" Text="Fund Details Name:" AssociatedControlID="FundDetailsNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsNameLiteral" runat="server" Text='<%#: Eval("FundDetailsName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsCode") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsCodeLabel" runat="server" Text="Fund Details Code:" AssociatedControlID="FundDetailsCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsCodeLiteral" runat="server" Text='<%#: Eval("FundDetailsCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsFundStatus") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsFundStatusLabel" runat="server" Text="Fund Details Fund Status:" AssociatedControlID="FundDetailsFundStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsFundStatusLiteral" runat="server" Text='<%#: Eval("FundDetailsFundStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsFundType") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsFundTypeLabel" runat="server" Text="Fund Details Fund Type:" AssociatedControlID="FundDetailsFundTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FundDetailsFundTypeHyperLink" runat="server" Text='<%#: Eval("FundDetailsFundType.Name") %>' NavigateUrl='<%# $"~/FundType2s/Edit.aspx?Id={Eval("FundDetailsFundTypeId")}" %>' Enabled='<%# Session["FundType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsFundTypeName") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsFundTypeNameLabel" runat="server" Text="Fund Details Fund Type Name:" AssociatedControlID="FundDetailsFundTypeNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsFundTypeNameLiteral" runat="server" Text='<%#: Eval("FundDetailsFundTypeName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsExternalAccountNo") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsExternalAccountNoLabel" runat="server" Text="Fund Details External Account No:" AssociatedControlID="FundDetailsExternalAccountNoLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsExternalAccountNoLiteral" runat="server" Text='<%#: Eval("FundDetailsExternalAccountNo") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsDescription") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsDescriptionLabel" runat="server" Text="Fund Details Description:" AssociatedControlID="FundDetailsDescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsDescriptionLiteral" runat="server" Text='<%#: Eval("FundDetailsDescription") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FundDetailsRestrictByLocations") != null %>'>
                                <td>
                                    <asp:Label ID="FundDetailsRestrictByLocationsLabel" runat="server" Text="Fund Details Restrict By Locations:" AssociatedControlID="FundDetailsRestrictByLocationsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FundDetailsRestrictByLocationsLiteral" runat="server" Text='<%#: Eval("FundDetailsRestrictByLocations") %>' />
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
                                    <asp:Literal ID="AllocatedLiteral" runat="server" Text='<%#: Eval("Allocated") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AwaitingPayment") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentLabel" runat="server" Text="Awaiting Payment:" AssociatedControlID="AwaitingPaymentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPaymentLiteral" runat="server" Text='<%#: Eval("AwaitingPayment") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Available") != null %>'>
                                <td>
                                    <asp:Label ID="AvailableLabel" runat="server" Text="Available:" AssociatedControlID="AvailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AvailableLiteral" runat="server" Text='<%#: Eval("Available") %>' />
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
                                    <asp:Literal ID="EncumberedLiteral" runat="server" Text='<%#: Eval("Encumbered") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Expenditures") != null %>'>
                                <td>
                                    <asp:Label ID="ExpendituresLabel" runat="server" Text="Expenditures:" AssociatedControlID="ExpendituresLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpendituresLiteral" runat="server" Text='<%#: Eval("Expenditures") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NetTransfers") != null %>'>
                                <td>
                                    <asp:Label ID="NetTransfersLabel" runat="server" Text="Net Transfers:" AssociatedControlID="NetTransfersLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NetTransfersLiteral" runat="server" Text='<%#: Eval("NetTransfers") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Unavailable") != null %>'>
                                <td>
                                    <asp:Label ID="UnavailableLabel" runat="server" Text="Unavailable:" AssociatedControlID="UnavailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UnavailableLiteral" runat="server" Text='<%#: Eval("Unavailable") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OverEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="OverEncumbranceLabel" runat="server" Text="Over Encumbrance:" AssociatedControlID="OverEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OverEncumbranceLiteral" runat="server" Text='<%#: Eval("OverEncumbrance") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OverExpended") != null %>'>
                                <td>
                                    <asp:Label ID="OverExpendedLabel" runat="server" Text="Over Expended:" AssociatedControlID="OverExpendedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OverExpendedLiteral" runat="server" Text='<%#: Eval("OverExpended") %>' />
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
                                    <asp:Literal ID="InitialAllocationLiteral" runat="server" Text='<%#: Eval("InitialAllocation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllocationTo") != null %>'>
                                <td>
                                    <asp:Label ID="AllocationToLabel" runat="server" Text="Allocation To:" AssociatedControlID="AllocationToLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllocationToLiteral" runat="server" Text='<%#: Eval("AllocationTo") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AllocationFrom") != null %>'>
                                <td>
                                    <asp:Label ID="AllocationFromLabel" runat="server" Text="Allocation From:" AssociatedControlID="AllocationFromLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AllocationFromLiteral" runat="server" Text='<%#: Eval("AllocationFrom") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TotalFunding") != null %>'>
                                <td>
                                    <asp:Label ID="TotalFundingLabel" runat="server" Text="Total Funding:" AssociatedControlID="TotalFundingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TotalFundingLiteral" runat="server" Text='<%#: Eval("TotalFunding") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CashBalance") != null %>'>
                                <td>
                                    <asp:Label ID="CashBalanceLabel" runat="server" Text="Cash Balance:" AssociatedControlID="CashBalanceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CashBalanceLiteral" runat="server" Text='<%#: Eval("CashBalance") %>' />
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
    <asp:Panel ID="RolloverBudgetAcquisitionsUnit2sPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetAcquisitionsUnit2sPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetAcquisitionsUnit2sHyperLink" runat="server" Text="Rollover Budget Acquisitions Units" NavigateUrl="~/RolloverBudgetAcquisitionsUnit2s/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetAcquisitionsUnit2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetAcquisitionsUnit2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget acquisitions units found">
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
    <asp:Panel ID="RolloverBudgetAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetAcquisitionsUnitsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetAcquisitionsUnitsHyperLink" runat="server" Text="Rollover Budget Acquisitions Units" NavigateUrl="~/RolloverBudgetAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget acquisitions units found">
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
    <asp:Panel ID="RolloverBudgetAllocatedFromNamesPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetAllocatedFromNamesPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetAllocatedFromNamesHyperLink" runat="server" Text="Rollover Budget Allocated From Names" NavigateUrl="~/RolloverBudgetAllocatedFromNames/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetAllocatedFromNamesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetAllocatedFromNamesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget allocated from names found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetAllocatedToNamesPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetAllocatedToNamesPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetAllocatedToNamesHyperLink" runat="server" Text="Rollover Budget Allocated To Names" NavigateUrl="~/RolloverBudgetAllocatedToNames/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetAllocatedToNamesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetAllocatedToNamesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget allocated to names found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetExpenseClassDetailsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetExpenseClassDetailsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetExpenseClassDetailsHyperLink" runat="server" Text="Rollover Budget Expense Class Details" NavigateUrl="~/RolloverBudgetExpenseClassDetails/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetExpenseClassDetailsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetExpenseClassDetailsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget expense class details found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Id 2" DataField="Id2" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridBoundColumn HeaderText="Expense Class Name" DataField="ExpenseClassName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Expense Class Code" DataField="ExpenseClassCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Expense Class Status" DataField="ExpenseClassStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Encumbered" DataField="Encumbered" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment" DataField="AwaitingPayment" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Credited" DataField="Credited" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Percentage Credited" DataField="PercentageCredited" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Expended" DataField="Expended" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Percentage Expended" DataField="PercentageExpended" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetFromFundsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetFromFundsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetFromFundsHyperLink" runat="server" Text="Rollover Budget From Funds" NavigateUrl="~/RolloverBudgetFromFunds/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetFromFundsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetFromFundsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget from funds found">
                    <Columns>
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
    <asp:Panel ID="RolloverBudgetLocationsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetLocationsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetLocationsHyperLink" runat="server" Text="Rollover Budget Locations" NavigateUrl="~/RolloverBudgetLocations/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetLocationsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetLocationsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget locations found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Location" DataField="Location.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Tenant Id" DataField="TenantId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetOrganizationsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetOrganizationsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetOrganizationsHyperLink" runat="server" Text="Rollover Budget Organizations" NavigateUrl="~/RolloverBudgetOrganizations/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetOrganizationsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetOrganizationsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget organizations found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Organization" DataField="Organization.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrganizationHyperLink" runat="server" Text='<%#: Eval("Organization.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("OrganizationId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetTagsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetTagsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetTagsHyperLink" runat="server" Text="Rollover Budget Tags" NavigateUrl="~/RolloverBudgetTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget tags found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RolloverBudgetToFundsPanel" runat="server" Visible='<%# (string)Session["RolloverBudgetToFundsPermission"] != null && RolloverBudget2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RolloverBudgetToFundsHyperLink" runat="server" Text="Rollover Budget To Funds" NavigateUrl="~/RolloverBudgetToFunds/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RolloverBudgetToFundsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RolloverBudgetToFundsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No rollover budget to funds found">
                    <Columns>
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetAcquisitionsUnit2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetAcquisitionsUnit2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetAllocatedFromNamesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetAllocatedFromNamesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetAllocatedToNamesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetAllocatedToNamesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetExpenseClassDetailsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetExpenseClassDetailsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetFromFundsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetFromFundsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetLocationsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetLocationsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetOrganizationsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetOrganizationsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RolloverBudgetToFundsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RolloverBudgetToFundsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
