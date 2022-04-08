<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Ledger2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Ledger2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Ledger2HyperLink" runat="server" Text="Ledger" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Ledger2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Ledger2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLedger2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FiscalYearOne") != null %>'>
                                <td>
                                    <asp:Label ID="FiscalYearOneLabel" runat="server" Text="Fiscal Year One:" AssociatedControlID="FiscalYearOneHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FiscalYearOneHyperLink" runat="server" Text='<%#: Eval("FiscalYearOne.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearOneId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LedgerStatus") != null %>'>
                                <td>
                                    <asp:Label ID="LedgerStatusLabel" runat="server" Text="Ledger Status:" AssociatedControlID="LedgerStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LedgerStatusLiteral" runat="server" Text='<%#: Eval("LedgerStatus") %>' />
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
                            <tr runat="server" visible='<%# Eval("Available") != null %>'>
                                <td>
                                    <asp:Label ID="AvailableLabel" runat="server" Text="Available:" AssociatedControlID="AvailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AvailableLiteral" runat="server" Text='<%# Eval("Available", "{0:c}") %>' />
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
                            <tr runat="server" visible='<%# Eval("Currency") != null %>'>
                                <td>
                                    <asp:Label ID="CurrencyLabel" runat="server" Text="Currency:" AssociatedControlID="CurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CurrencyLiteral" runat="server" Text='<%#: Eval("Currency") %>' />
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
                            <tr runat="server" visible='<%# Eval("AwaitingPayment") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentLabel" runat="server" Text="Awaiting Payment:" AssociatedControlID="AwaitingPaymentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPaymentLiteral" runat="server" Text='<%# Eval("AwaitingPayment", "{0:c}") %>' />
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
    <asp:Panel ID="Fund2sPanel" runat="server" Visible='<%# (string)Session["Fund2sPermission"] != null && Ledger2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Fund2sHyperLink" runat="server" Text="Funds" NavigateUrl="~/Fund2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Fund2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Fund2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No funds found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/finance/fund/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Code" DataField="Code" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fund Status" DataField="FundStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Fund Type" DataField="FundType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FundTypeHyperLink" runat="server" Text='<%#: Eval("FundType.Name") %>' NavigateUrl='<%# $"~/FundType2s/Edit.aspx?Id={Eval("FundTypeId")}" %>' Enabled='<%# Session["FundType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("Id")}" %>' />
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
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LedgerAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["LedgerAcquisitionsUnitsPermission"] != null && Ledger2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerAcquisitionsUnitsHyperLink" runat="server" Text="Ledger Acquisitions Units" NavigateUrl="~/LedgerAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="LedgerAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LedgerAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No ledger acquisitions units found">
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
    <asp:Panel ID="LedgerRollover2sPanel" runat="server" Visible='<%# (string)Session["LedgerRollover2sPermission"] != null && Ledger2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn HeaderText="From Fiscal Year" DataField="FromFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFiscalYearHyperLink" runat="server" Text='<%#: Eval("FromFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FromFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Fund2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Fund2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LedgerRollover2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LedgerRollover2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
