<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LedgerRollover2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LedgerRollover2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRollover2HyperLink" runat="server" Text="Ledger Rollover" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LedgerRollover2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="LedgerRollover2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLedgerRollover2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Ledger") != null %>'>
                                <td>
                                    <asp:Label ID="LedgerLabel" runat="server" Text="Ledger:" AssociatedControlID="LedgerHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LedgerHyperLink" runat="server" Text='<%#: Eval("Ledger.Name") %>' NavigateUrl='<%# $"~/Ledger2s/Edit.aspx?Id={Eval("LedgerId")}" %>' Enabled='<%# Session["Ledger2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FromFiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="FromFiscalYearLabel" runat="server" Text="From Fiscal Year:" AssociatedControlID="FromFiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FromFiscalYearHyperLink" runat="server" Text='<%#: Eval("FromFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FromFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ToFiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="ToFiscalYearLabel" runat="server" Text="To Fiscal Year:" AssociatedControlID="ToFiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ToFiscalYearHyperLink" runat="server" Text='<%#: Eval("ToFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("ToFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RestrictEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="RestrictEncumbranceLabel" runat="server" Text="Restrict Encumbrance:" AssociatedControlID="RestrictEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RestrictEncumbranceLiteral" runat="server" Text='<%#: Eval("RestrictEncumbrance") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RestrictExpenditures") != null %>'>
                                <td>
                                    <asp:Label ID="RestrictExpendituresLabel" runat="server" Text="Restrict Expenditures:" AssociatedControlID="RestrictExpendituresLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RestrictExpendituresLiteral" runat="server" Text='<%#: Eval("RestrictExpenditures") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NeedCloseBudgets") != null %>'>
                                <td>
                                    <asp:Label ID="NeedCloseBudgetsLabel" runat="server" Text="Need Close Budgets:" AssociatedControlID="NeedCloseBudgetsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NeedCloseBudgetsLiteral" runat="server" Text='<%#: Eval("NeedCloseBudgets") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CurrencyFactor") != null %>'>
                                <td>
                                    <asp:Label ID="CurrencyFactorLabel" runat="server" Text="Currency Factor:" AssociatedControlID="CurrencyFactorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CurrencyFactorLiteral" runat="server" Text='<%#: Eval("CurrencyFactor") %>' />
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
    <asp:Panel ID="LedgerRolloverBudgetsRolloversPanel" runat="server" Visible='<%# (string)Session["LedgerRolloverBudgetsRolloversPermission"] != null && LedgerRollover2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverBudgetsRolloversHyperLink" runat="server" Text="Ledger Rollover Budgets Rollovers" NavigateUrl="~/LedgerRolloverBudgetsRollovers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="LedgerRolloverBudgetsRolloversRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRolloverBudgetsRolloversRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollover budgets rollovers found">
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Fund Type" DataField="FundType.Name" SortExpression="FundType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundTypeHyperLink" runat="server" Text='<%#: Eval("FundType.Name") %>' NavigateUrl='<%# $"~/FundType2s/Edit.aspx?Id={Eval("FundTypeId")}" %>' Enabled='<%# Session["FundType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Rollover Allocation" DataField="RolloverAllocation" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Rollover Available" DataField="RolloverAvailable" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Set Allowances" DataField="SetAllowances" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Adjust Allocation" DataField="AdjustAllocation" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Add Available To" DataField="AddAvailableTo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Allowable Encumbrance" DataField="AllowableEncumbrance" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Allowable Expenditure" DataField="AllowableExpenditure" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LedgerRolloverEncumbrancesRolloversPanel" runat="server" Visible='<%# (string)Session["LedgerRolloverEncumbrancesRolloversPermission"] != null && LedgerRollover2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverEncumbrancesRolloversHyperLink" runat="server" Text="Ledger Rollover Encumbrances Rollovers" NavigateUrl="~/LedgerRolloverEncumbrancesRollovers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="LedgerRolloverEncumbrancesRolloversRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRolloverEncumbrancesRolloversRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollover encumbrances rollovers found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Based On" DataField="BasedOn" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Increase By" DataField="IncreaseBy" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LedgerRolloverError2sPanel" runat="server" Visible='<%# (string)Session["LedgerRolloverError2sPermission"] != null && LedgerRollover2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverError2sHyperLink" runat="server" Text="Ledger Rollover Errors" NavigateUrl="~/LedgerRolloverError2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LedgerRolloverError2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRolloverError2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollover errors found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LedgerRolloverError2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/LedgerRolloverError2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Error Type" DataField="ErrorType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Failed Action" DataField="FailedAction" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Error Message" DataField="ErrorMessage" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="LedgerRolloverProgress2sPanel" runat="server" Visible='<%# (string)Session["LedgerRolloverProgress2sPermission"] != null && LedgerRollover2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverProgress2sHyperLink" runat="server" Text="Ledger Rollover Progresss" NavigateUrl="~/LedgerRolloverProgress2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="LedgerRolloverProgress2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerRolloverProgress2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger rollover progresses found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/LedgerRolloverProgress2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/LedgerRolloverProgress2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Overall Rollover Status" DataField="OverallRolloverStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Budgets Closing Rollover Status" DataField="BudgetsClosingRolloverStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Financial Rollover Status" DataField="FinancialRolloverStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Orders Rollover Status" DataField="OrdersRolloverStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
            <telerik:AjaxSetting AjaxControlID="LedgerRolloverBudgetsRolloversRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRolloverBudgetsRolloversPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRolloverEncumbrancesRolloversRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRolloverEncumbrancesRolloversPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRolloverError2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRolloverError2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRolloverProgress2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRolloverProgress2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
