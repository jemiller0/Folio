<%@ Page Title="Organizations" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Organization2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Organization2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="Organization2sHyperLink" runat="server" Text="Organizations" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="Organization2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="Organization2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No organizations found" CommandItemDisplay="Top">
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
                        <telerik:GridBoundColumn HeaderText="Code" DataField="Code" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Export To Accounting" DataField="ExportToAccounting" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Language" DataField="Language" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Method" DataField="PaymentMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Access Provider" DataField="AccessProvider" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Governmental" DataField="Governmental" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Licensor" DataField="Licensor" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Material Supplier" DataField="MaterialSupplier" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Claiming Interval" DataField="ClaimingInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Discount Percent" DataField="DiscountPercent" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Expected Activation Interval" DataField="ExpectedActivationInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Expected Invoice Interval" DataField="ExpectedInvoiceInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Renewal Activation Interval" DataField="RenewalActivationInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Subscription Interval" DataField="SubscriptionInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Expected Receipt Interval" DataField="ExpectedReceiptInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Tax Id" DataField="TaxId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Liable For Vat" DataField="LiableForVat" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Tax Percentage" DataField="TaxPercentage" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Vendor EDI Code" DataField="EdiVendorEdiCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Vendor EDI Type" DataField="EdiVendorEdiType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Lib EDI Code" DataField="EdiLibEdiCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Lib EDI Type" DataField="EdiLibEdiType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Prorate Tax" DataField="EdiProrateTax" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Prorate Fees" DataField="EdiProrateFees" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Naming Convention" DataField="EdiNamingConvention" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Send Acct Num" DataField="EdiSendAcctNum" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Support Order" DataField="EdiSupportOrder" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Support Invoice" DataField="EdiSupportInvoice" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Notes" DataField="EdiNotes" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP FTP Format" DataField="EdiFtpFtpFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Server Address" DataField="EdiFtpServerAddress" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Username" DataField="EdiFtpUsername" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Password" DataField="EdiFtpPassword" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP FTP Mode" DataField="EdiFtpFtpMode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP FTP Conn Mode" DataField="EdiFtpFtpConnMode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP FTP Port" DataField="EdiFtpFtpPort" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Order Directory" DataField="EdiFtpOrderDirectory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Invoice Directory" DataField="EdiFtpInvoiceDirectory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI FTP Notes" DataField="EdiFtpNotes" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Schedule Edi" DataField="EdiJobScheduleEdi" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Scheduling Date" DataField="EdiJobSchedulingDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Time" DataField="EdiJobTime" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Monday" DataField="EdiJobIsMonday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Tuesday" DataField="EdiJobIsTuesday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Wednesday" DataField="EdiJobIsWednesday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Thursday" DataField="EdiJobIsThursday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Friday" DataField="EdiJobIsFriday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Saturday" DataField="EdiJobIsSaturday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Is Sunday" DataField="EdiJobIsSunday" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Send To Emails" DataField="EdiJobSendToEmails" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Notify All Edi" DataField="EdiJobNotifyAllEdi" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Notify Invoice Only" DataField="EdiJobNotifyInvoiceOnly" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Notify Error Only" DataField="EdiJobNotifyErrorOnly" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="EDI Job Scheduling Notes" DataField="EdiJobSchedulingNotes" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Is Vendor" DataField="IsVendor" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="San Code" DataField="SanCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    <telerik:AjaxUpdatedControl ControlID="Organization2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Organization2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Organization2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
