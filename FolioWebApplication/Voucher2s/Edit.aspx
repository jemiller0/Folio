<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Voucher2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Voucher2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Voucher2HyperLink" runat="server" Text="Voucher" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Voucher2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Voucher2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewVoucher2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AccountingCode") != null %>'>
                                <td>
                                    <asp:Label ID="AccountingCodeLabel" runat="server" Text="Accounting Code:" AssociatedControlID="AccountingCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccountingCodeLiteral" runat="server" Text='<%#: Eval("AccountingCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AccountNumber") != null %>'>
                                <td>
                                    <asp:Label ID="AccountNumberLabel" runat="server" Text="Account Number:" AssociatedControlID="AccountNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccountNumberLiteral" runat="server" Text='<%#: Eval("AccountNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Amount") != null %>'>
                                <td>
                                    <asp:Label ID="AmountLabel" runat="server" Text="Amount:" AssociatedControlID="AmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AmountLiteral" runat="server" Text='<%# Eval("Amount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BatchGroup") != null %>'>
                                <td>
                                    <asp:Label ID="BatchGroupLabel" runat="server" Text="Batch Group:" AssociatedControlID="BatchGroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DisbursementNumber") != null %>'>
                                <td>
                                    <asp:Label ID="DisbursementNumberLabel" runat="server" Text="Disbursement Number:" AssociatedControlID="DisbursementNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DisbursementNumberLiteral" runat="server" Text='<%#: Eval("DisbursementNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DisbursementDate") != null %>'>
                                <td>
                                    <asp:Label ID="DisbursementDateLabel" runat="server" Text="Disbursement Date:" AssociatedControlID="DisbursementDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DisbursementDateLiteral" runat="server" Text='<%# Eval("DisbursementDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DisbursementAmount") != null %>'>
                                <td>
                                    <asp:Label ID="DisbursementAmountLabel" runat="server" Text="Disbursement Amount:" AssociatedControlID="DisbursementAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DisbursementAmountLiteral" runat="server" Text='<%# Eval("DisbursementAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EnclosureNeeded") != null %>'>
                                <td>
                                    <asp:Label ID="EnclosureNeededLabel" runat="server" Text="Enclosure Needed:" AssociatedControlID="EnclosureNeededLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EnclosureNeededLiteral" runat="server" Text='<%#: Eval("EnclosureNeeded") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InvoiceCurrency") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceCurrencyLabel" runat="server" Text="Invoice Currency:" AssociatedControlID="InvoiceCurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InvoiceCurrencyLiteral" runat="server" Text='<%#: Eval("InvoiceCurrency") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Invoice") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceLabel" runat="server" Text="Invoice:" AssociatedControlID="InvoiceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("Invoice.Number") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExchangeRate") != null %>'>
                                <td>
                                    <asp:Label ID="ExchangeRateLabel" runat="server" Text="Exchange Rate:" AssociatedControlID="ExchangeRateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExchangeRateLiteral" runat="server" Text='<%#: Eval("ExchangeRate") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExportToAccounting") != null %>'>
                                <td>
                                    <asp:Label ID="ExportToAccountingLabel" runat="server" Text="Export To Accounting:" AssociatedControlID="ExportToAccountingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExportToAccountingLiteral" runat="server" Text='<%#: Eval("ExportToAccounting") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLabel" runat="server" Text="Status:" AssociatedControlID="StatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLiteral" runat="server" Text='<%#: Eval("Status") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SystemCurrency") != null %>'>
                                <td>
                                    <asp:Label ID="SystemCurrencyLabel" runat="server" Text="System Currency:" AssociatedControlID="SystemCurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SystemCurrencyLiteral" runat="server" Text='<%#: Eval("SystemCurrency") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Type") != null %>'>
                                <td>
                                    <asp:Label ID="TypeLabel" runat="server" Text="Type:" AssociatedControlID="TypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TypeLiteral" runat="server" Text='<%#: Eval("Type") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VoucherDate") != null %>'>
                                <td>
                                    <asp:Label ID="VoucherDateLabel" runat="server" Text="Voucher Date:" AssociatedControlID="VoucherDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VoucherDateLiteral" runat="server" Text='<%# Eval("VoucherDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VoucherNumber") != null %>'>
                                <td>
                                    <asp:Label ID="VoucherNumberLabel" runat="server" Text="Voucher Number:" AssociatedControlID="VoucherNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VoucherNumberLiteral" runat="server" Text='<%#: Eval("VoucherNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Vendor") != null %>'>
                                <td>
                                    <asp:Label ID="VendorLabel" runat="server" Text="Vendor:" AssociatedControlID="VendorHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="VendorHyperLink" runat="server" Text='<%#: Eval("Vendor.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("VendorId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorStreetAddress1") != null %>'>
                                <td>
                                    <asp:Label ID="VendorStreetAddress1Label" runat="server" Text="Vendor Street Address 1:" AssociatedControlID="VendorStreetAddress1Literal" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorStreetAddress1Literal" runat="server" Text='<%#: Eval("VendorStreetAddress1") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorStreetAddress2") != null %>'>
                                <td>
                                    <asp:Label ID="VendorStreetAddress2Label" runat="server" Text="Vendor Street Address 2:" AssociatedControlID="VendorStreetAddress2Literal" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorStreetAddress2Literal" runat="server" Text='<%#: Eval("VendorStreetAddress2") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorCity") != null %>'>
                                <td>
                                    <asp:Label ID="VendorCityLabel" runat="server" Text="Vendor City:" AssociatedControlID="VendorCityLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorCityLiteral" runat="server" Text='<%#: Eval("VendorCity") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorState") != null %>'>
                                <td>
                                    <asp:Label ID="VendorStateLabel" runat="server" Text="Vendor State:" AssociatedControlID="VendorStateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorStateLiteral" runat="server" Text='<%#: Eval("VendorState") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorPostalCode") != null %>'>
                                <td>
                                    <asp:Label ID="VendorPostalCodeLabel" runat="server" Text="Vendor Postal Code:" AssociatedControlID="VendorPostalCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorPostalCodeLiteral" runat="server" Text='<%#: Eval("VendorPostalCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorCountryCode") != null %>'>
                                <td>
                                    <asp:Label ID="VendorCountryCodeLabel" runat="server" Text="Vendor Country Code:" AssociatedControlID="VendorCountryCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorCountryCodeLiteral" runat="server" Text='<%#: Eval("VendorCountryCode") %>' />
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
    <asp:Panel ID="VoucherItem2sPanel" runat="server" Visible='<%# (string)Session["VoucherItem2sPermission"] != null && Voucher2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="VoucherItem2sHyperLink" runat="server" Text="Voucher Items" NavigateUrl="~/VoucherItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="VoucherItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="VoucherItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No voucher items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/VoucherItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/VoucherItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="External Account Number" DataField="ExternalAccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Sub Transaction" DataField="SubTransaction.Amount" SortExpression="SubTransaction.Amount" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="SubTransactionHyperLink" runat="server" Text='<%# $"{Eval("SubTransaction.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("SubTransactionId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
            <telerik:AjaxSetting AjaxControlID="VoucherItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="VoucherItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
