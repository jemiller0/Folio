<%@ Page Title="Vouchers" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Voucher2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Voucher2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="Voucher2sHyperLink" runat="server" Text="Vouchers" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="Voucher2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="Voucher2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No vouchers found" CommandItemDisplay="Top">
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
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Batch Group" DataField="BatchGroup.Name" SortExpression="BatchGroup.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Disbursement Number" DataField="DisbursementNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Date" DataField="DisbursementDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Amount" DataField="DisbursementAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Enclosure" DataField="Enclosure" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Invoice Currency" DataField="InvoiceCurrency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Invoice" DataField="Invoice.Number" SortExpression="Invoice.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Exchange Rate" DataField="ExchangeRate" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Export To Accounting" DataField="ExportToAccounting" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="System Currency" DataField="SystemCurrency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Type" DataField="Type" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Voucher Date" DataField="VoucherDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Vendor" DataField="Vendor.Name" SortExpression="Vendor.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="VendorHyperLink" runat="server" Text='<%#: Eval("Vendor.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("VendorId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Vendor Street Address 1" DataField="VendorStreetAddress1" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Street Address 2" DataField="VendorStreetAddress2" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor City" DataField="VendorCity" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor State" DataField="VendorState" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Postal Code" DataField="VendorPostalCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Country Code" DataField="VendorCountryCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    <telerik:AjaxUpdatedControl ControlID="Voucher2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Voucher2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Voucher2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
