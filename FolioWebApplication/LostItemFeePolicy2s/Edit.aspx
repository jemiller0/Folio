<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LostItemFeePolicy2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LostItemFeePolicy2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LostItemFeePolicy2HyperLink" runat="server" Text="Lost Item Fee Policy" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LostItemFeePolicy2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="LostItemFeePolicy2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLostItemFeePolicy2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemAgedLostOverdueDuration") != null %>'>
                                <td>
                                    <asp:Label ID="ItemAgedLostOverdueDurationLabel" runat="server" Text="Item Aged Lost Overdue Duration:" AssociatedControlID="ItemAgedLostOverdueDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemAgedLostOverdueDurationLiteral" runat="server" Text='<%#: Eval("ItemAgedLostOverdueDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemAgedLostOverdueInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ItemAgedLostOverdueIntervalLabel" runat="server" Text="Item Aged Lost Overdue Interval:" AssociatedControlID="ItemAgedLostOverdueIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemAgedLostOverdueIntervalLiteral" runat="server" Text='<%#: Eval("ItemAgedLostOverdueInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronBilledAfterAgedLostDuration") != null %>'>
                                <td>
                                    <asp:Label ID="PatronBilledAfterAgedLostDurationLabel" runat="server" Text="Patron Billed After Aged Lost Duration:" AssociatedControlID="PatronBilledAfterAgedLostDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronBilledAfterAgedLostDurationLiteral" runat="server" Text='<%#: Eval("PatronBilledAfterAgedLostDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronBilledAfterAgedLostInterval") != null %>'>
                                <td>
                                    <asp:Label ID="PatronBilledAfterAgedLostIntervalLabel" runat="server" Text="Patron Billed After Aged Lost Interval:" AssociatedControlID="PatronBilledAfterAgedLostIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronBilledAfterAgedLostIntervalLiteral" runat="server" Text='<%#: Eval("PatronBilledAfterAgedLostInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecalledItemAgedLostOverdueDuration") != null %>'>
                                <td>
                                    <asp:Label ID="RecalledItemAgedLostOverdueDurationLabel" runat="server" Text="Recalled Item Aged Lost Overdue Duration:" AssociatedControlID="RecalledItemAgedLostOverdueDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecalledItemAgedLostOverdueDurationLiteral" runat="server" Text='<%#: Eval("RecalledItemAgedLostOverdueDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecalledItemAgedLostOverdueInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RecalledItemAgedLostOverdueIntervalLabel" runat="server" Text="Recalled Item Aged Lost Overdue Interval:" AssociatedControlID="RecalledItemAgedLostOverdueIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecalledItemAgedLostOverdueIntervalLiteral" runat="server" Text='<%#: Eval("RecalledItemAgedLostOverdueInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronBilledAfterRecalledItemAgedLostDuration") != null %>'>
                                <td>
                                    <asp:Label ID="PatronBilledAfterRecalledItemAgedLostDurationLabel" runat="server" Text="Patron Billed After Recalled Item Aged Lost Duration:" AssociatedControlID="PatronBilledAfterRecalledItemAgedLostDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronBilledAfterRecalledItemAgedLostDurationLiteral" runat="server" Text='<%#: Eval("PatronBilledAfterRecalledItemAgedLostDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronBilledAfterRecalledItemAgedLostInterval") != null %>'>
                                <td>
                                    <asp:Label ID="PatronBilledAfterRecalledItemAgedLostIntervalLabel" runat="server" Text="Patron Billed After Recalled Item Aged Lost Interval:" AssociatedControlID="PatronBilledAfterRecalledItemAgedLostIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronBilledAfterRecalledItemAgedLostIntervalLiteral" runat="server" Text='<%#: Eval("PatronBilledAfterRecalledItemAgedLostInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ChargeAmountItemChargeType") != null %>'>
                                <td>
                                    <asp:Label ID="ChargeAmountItemChargeTypeLabel" runat="server" Text="Charge Amount Item Charge Type:" AssociatedControlID="ChargeAmountItemChargeTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ChargeAmountItemChargeTypeLiteral" runat="server" Text='<%#: Eval("ChargeAmountItemChargeType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ChargeAmountItemAmount") != null %>'>
                                <td>
                                    <asp:Label ID="ChargeAmountItemAmountLabel" runat="server" Text="Charge Amount Item Amount:" AssociatedControlID="ChargeAmountItemAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ChargeAmountItemAmountLiteral" runat="server" Text='<%# Eval("ChargeAmountItemAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LostItemProcessingFee") != null %>'>
                                <td>
                                    <asp:Label ID="LostItemProcessingFeeLabel" runat="server" Text="Lost Item Processing Fee:" AssociatedControlID="LostItemProcessingFeeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LostItemProcessingFeeLiteral" runat="server" Text='<%#: Eval("LostItemProcessingFee") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ChargeAmountItemPatron") != null %>'>
                                <td>
                                    <asp:Label ID="ChargeAmountItemPatronLabel" runat="server" Text="Charge Amount Item Patron:" AssociatedControlID="ChargeAmountItemPatronLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ChargeAmountItemPatronLiteral" runat="server" Text='<%#: Eval("ChargeAmountItemPatron") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ChargeAmountItemSystem") != null %>'>
                                <td>
                                    <asp:Label ID="ChargeAmountItemSystemLabel" runat="server" Text="Charge Amount Item System:" AssociatedControlID="ChargeAmountItemSystemLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ChargeAmountItemSystemLiteral" runat="server" Text='<%#: Eval("ChargeAmountItemSystem") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LostItemChargeFeeFineDuration") != null %>'>
                                <td>
                                    <asp:Label ID="LostItemChargeFeeFineDurationLabel" runat="server" Text="Lost Item Charge Fee Fine Duration:" AssociatedControlID="LostItemChargeFeeFineDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LostItemChargeFeeFineDurationLiteral" runat="server" Text='<%#: Eval("LostItemChargeFeeFineDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LostItemChargeFeeFineInterval") != null %>'>
                                <td>
                                    <asp:Label ID="LostItemChargeFeeFineIntervalLabel" runat="server" Text="Lost Item Charge Fee Fine Interval:" AssociatedControlID="LostItemChargeFeeFineIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LostItemChargeFeeFineIntervalLiteral" runat="server" Text='<%#: Eval("LostItemChargeFeeFineInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReturnedLostItemProcessingFee") != null %>'>
                                <td>
                                    <asp:Label ID="ReturnedLostItemProcessingFeeLabel" runat="server" Text="Returned Lost Item Processing Fee:" AssociatedControlID="ReturnedLostItemProcessingFeeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReturnedLostItemProcessingFeeLiteral" runat="server" Text='<%#: Eval("ReturnedLostItemProcessingFee") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReplacedLostItemProcessingFee") != null %>'>
                                <td>
                                    <asp:Label ID="ReplacedLostItemProcessingFeeLabel" runat="server" Text="Replaced Lost Item Processing Fee:" AssociatedControlID="ReplacedLostItemProcessingFeeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReplacedLostItemProcessingFeeLiteral" runat="server" Text='<%#: Eval("ReplacedLostItemProcessingFee") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReplacementProcessingFee") != null %>'>
                                <td>
                                    <asp:Label ID="ReplacementProcessingFeeLabel" runat="server" Text="Replacement Processing Fee:" AssociatedControlID="ReplacementProcessingFeeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReplacementProcessingFeeLiteral" runat="server" Text='<%#: Eval("ReplacementProcessingFee") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReplacementAllowed") != null %>'>
                                <td>
                                    <asp:Label ID="ReplacementAllowedLabel" runat="server" Text="Replacement Allowed:" AssociatedControlID="ReplacementAllowedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReplacementAllowedLiteral" runat="server" Text='<%#: Eval("ReplacementAllowed") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LostItemReturned") != null %>'>
                                <td>
                                    <asp:Label ID="LostItemReturnedLabel" runat="server" Text="Lost Item Returned:" AssociatedControlID="LostItemReturnedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LostItemReturnedLiteral" runat="server" Text='<%#: Eval("LostItemReturned") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FeesFinesShallRefundedDuration") != null %>'>
                                <td>
                                    <asp:Label ID="FeesFinesShallRefundedDurationLabel" runat="server" Text="Fees Fines Shall Refunded Duration:" AssociatedControlID="FeesFinesShallRefundedDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FeesFinesShallRefundedDurationLiteral" runat="server" Text='<%#: Eval("FeesFinesShallRefundedDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FeesFinesShallRefundedInterval") != null %>'>
                                <td>
                                    <asp:Label ID="FeesFinesShallRefundedIntervalLabel" runat="server" Text="Fees Fines Shall Refunded Interval:" AssociatedControlID="FeesFinesShallRefundedIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FeesFinesShallRefundedIntervalLiteral" runat="server" Text='<%#: Eval("FeesFinesShallRefundedInterval") %>' />
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
    <asp:Panel ID="Loan2sPanel" runat="server" Visible='<%# (string)Session["Loan2sPermission"] != null && LostItemFeePolicy2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Loan2sHyperLink" runat="server" Text="Loans" NavigateUrl="~/Loan2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Loan2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Loan2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loans found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/users/{Eval("UserId")}/loans/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proxy User" DataField="ProxyUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProxyUserHyperLink" runat="server" Text='<%#: Eval("ProxyUserId") != null ? Eval("ProxyUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ProxyUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Effective Location At Check Out" DataField="ItemEffectiveLocationAtCheckOut.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemEffectiveLocationAtCheckOutHyperLink" runat="server" Text='<%#: Eval("ItemEffectiveLocationAtCheckOut.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemEffectiveLocationAtCheckOutId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loan Time" DataField="LoanTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Return Time" DataField="ReturnTime" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="System Return Time" DataField="SystemReturnTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Action" DataField="Action" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Action Comment" DataField="ActionComment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status" DataField="ItemStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewal Count" DataField="RenewalCount" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Loan Policy" DataField="LoanPolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanPolicyHyperLink" runat="server" Text='<%#: Eval("LoanPolicy.Name") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("LoanPolicyId")}" %>' Enabled='<%# Session["LoanPolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checkout Service Point" DataField="CheckoutServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckoutServicePointHyperLink" runat="server" Text='<%#: Eval("CheckoutServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckoutServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checkin Service Point" DataField="CheckinServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckinServicePointHyperLink" runat="server" Text='<%#: Eval("CheckinServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckinServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" DataField="Group.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="GroupHyperLink" runat="server" Text='<%#: Eval("Group.Name") %>' NavigateUrl='<%# $"~/Group2s/Edit.aspx?Id={Eval("GroupId")}" %>' Enabled='<%# Session["Group2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Due Date Changed By Recall" DataField="DueDateChangedByRecall" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Declared Lost Date" DataField="DeclaredLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Claimed Returned Date" DataField="ClaimedReturnedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Overdue Fine Policy" DataField="OverdueFinePolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OverdueFinePolicyHyperLink" runat="server" Text='<%#: Eval("OverdueFinePolicy.Name") %>' NavigateUrl='<%# $"~/OverdueFinePolicy2s/Edit.aspx?Id={Eval("OverdueFinePolicyId")}" %>' Enabled='<%# Session["OverdueFinePolicy2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Lost Item Has Been Billed" DataField="AgedToLostDelayedBillingLostItemHasBeenBilled" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Date Lost Item Should Be Billed" DataField="AgedToLostDelayedBillingDateLostItemShouldBeBilled" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Aged To Lost Date" DataField="AgedToLostDelayedBillingAgedToLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Loan2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Loan2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
