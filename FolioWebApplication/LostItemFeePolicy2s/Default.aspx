<%@ Page Title="Lost Item Fee Policies" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.LostItemFeePolicy2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LostItemFeePolicy2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="LostItemFeePolicy2sHyperLink" runat="server" Text="Lost Item Fee Policies" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="LostItemFeePolicy2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="LostItemFeePolicy2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No lost item fee policies found" CommandItemDisplay="Top">
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
                        <telerik:GridBoundColumn HeaderText="Item Aged Lost Overdue Duration" DataField="ItemAgedLostOverdueDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Item Aged Lost Overdue Interval" DataField="ItemAgedLostOverdueInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Patron Billed After Aged Lost Duration" DataField="PatronBilledAfterAgedLostDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Patron Billed After Aged Lost Interval" DataField="PatronBilledAfterAgedLostInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Recalled Item Aged Lost Overdue Duration" DataField="RecalledItemAgedLostOverdueDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Recalled Item Aged Lost Overdue Interval" DataField="RecalledItemAgedLostOverdueInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Patron Billed After Recalled Item Aged Lost Duration" DataField="PatronBilledAfterRecalledItemAgedLostDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Patron Billed After Recalled Item Aged Lost Interval" DataField="PatronBilledAfterRecalledItemAgedLostInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Charge Amount Item Charge Type" DataField="ChargeAmountItemChargeType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Charge Amount Item Amount" DataField="ChargeAmountItemAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Lost Item Processing Fee" DataField="LostItemProcessingFee" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Charge Amount Item Patron" DataField="ChargeAmountItemPatron" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Charge Amount Item System" DataField="ChargeAmountItemSystem" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Lost Item Charge Fee Fine Duration" DataField="LostItemChargeFeeFineDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Lost Item Charge Fee Fine Interval" DataField="LostItemChargeFeeFineInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Returned Lost Item Processing Fee" DataField="ReturnedLostItemProcessingFee" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Replaced Lost Item Processing Fee" DataField="ReplacedLostItemProcessingFee" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Replacement Processing Fee" DataField="ReplacementProcessingFee" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Replacement Allowed" DataField="ReplacementAllowed" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Lost Item Returned" DataField="LostItemReturned" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fees Fines Shall Refunded Duration" DataField="FeesFinesShallRefundedDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Fees Fines Shall Refunded Interval" DataField="FeesFinesShallRefundedInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    <telerik:AjaxUpdatedControl ControlID="LostItemFeePolicy2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LostItemFeePolicy2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LostItemFeePolicy2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
