<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Request2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Request2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Request2HyperLink" runat="server" Text="Request" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Request2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Request2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewRequest2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequestType") != null %>'>
                                <td>
                                    <asp:Label ID="RequestTypeLabel" runat="server" Text="Request Type:" AssociatedControlID="RequestTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequestTypeLiteral" runat="server" Text='<%#: Eval("RequestType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequestDate") != null %>'>
                                <td>
                                    <asp:Label ID="RequestDateLabel" runat="server" Text="Request Date:" AssociatedControlID="RequestDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequestDateLiteral" runat="server" Text='<%# Eval("RequestDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronComments") != null %>'>
                                <td>
                                    <asp:Label ID="PatronCommentsLabel" runat="server" Text="Patron Comments:" AssociatedControlID="PatronCommentsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronCommentsLiteral" runat="server" Text='<%#: Eval("PatronComments") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Requester") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterLabel" runat="server" Text="Requester:" AssociatedControlID="RequesterHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RequesterHyperLink" runat="server" Text='<%#: Eval("Requester.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("RequesterId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyUser") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyUserLabel" runat="server" Text="Proxy User:" AssociatedControlID="ProxyUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ProxyUserHyperLink" runat="server" Text='<%#: Eval("ProxyUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ProxyUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Item") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLabel" runat="server" Text="Item:" AssociatedControlID="ItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("Item.ShortId") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("CancellationReason") != null %>'>
                                <td>
                                    <asp:Label ID="CancellationReasonLabel" runat="server" Text="Cancellation Reason:" AssociatedControlID="CancellationReasonHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CancellationReasonHyperLink" runat="server" Text='<%#: Eval("CancellationReason.Name") %>' NavigateUrl='<%# $"~/CancellationReason2s/Edit.aspx?Id={Eval("CancellationReasonId")}" %>' Enabled='<%# Session["CancellationReason2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancelledByUser") != null %>'>
                                <td>
                                    <asp:Label ID="CancelledByUserLabel" runat="server" Text="Cancelled By User:" AssociatedControlID="CancelledByUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CancelledByUserHyperLink" runat="server" Text='<%#: Eval("CancelledByUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CancelledByUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancellationAdditionalInformation") != null %>'>
                                <td>
                                    <asp:Label ID="CancellationAdditionalInformationLabel" runat="server" Text="Cancellation Additional Information:" AssociatedControlID="CancellationAdditionalInformationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CancellationAdditionalInformationLiteral" runat="server" Text='<%#: Eval("CancellationAdditionalInformation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancelledDate") != null %>'>
                                <td>
                                    <asp:Label ID="CancelledDateLabel" runat="server" Text="Cancelled Date:" AssociatedControlID="CancelledDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CancelledDateLiteral" runat="server" Text='<%# Eval("CancelledDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Position") != null %>'>
                                <td>
                                    <asp:Label ID="PositionLabel" runat="server" Text="Position:" AssociatedControlID="PositionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PositionLiteral" runat="server" Text='<%#: Eval("Position") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemTitle") != null %>'>
                                <td>
                                    <asp:Label ID="ItemTitleLabel" runat="server" Text="Item Title:" AssociatedControlID="ItemTitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemTitleLiteral" runat="server" Text='<%#: Eval("ItemTitle") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemBarcode") != null %>'>
                                <td>
                                    <asp:Label ID="ItemBarcodeLabel" runat="server" Text="Item Barcode:" AssociatedControlID="ItemBarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemBarcodeLiteral" runat="server" Text='<%#: Eval("ItemBarcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequesterFirstName") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterFirstNameLabel" runat="server" Text="Requester First Name:" AssociatedControlID="RequesterFirstNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterFirstNameLiteral" runat="server" Text='<%#: Eval("RequesterFirstName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequesterLastName") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterLastNameLabel" runat="server" Text="Requester Last Name:" AssociatedControlID="RequesterLastNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterLastNameLiteral" runat="server" Text='<%#: Eval("RequesterLastName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequesterMiddleName") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterMiddleNameLabel" runat="server" Text="Requester Middle Name:" AssociatedControlID="RequesterMiddleNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterMiddleNameLiteral" runat="server" Text='<%#: Eval("RequesterMiddleName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequesterBarcode") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterBarcodeLabel" runat="server" Text="Requester Barcode:" AssociatedControlID="RequesterBarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterBarcodeLiteral" runat="server" Text='<%#: Eval("RequesterBarcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequesterPatronGroup") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterPatronGroupLabel" runat="server" Text="Requester Patron Group:" AssociatedControlID="RequesterPatronGroupLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterPatronGroupLiteral" runat="server" Text='<%#: Eval("RequesterPatronGroup") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyFirstName") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyFirstNameLabel" runat="server" Text="Proxy First Name:" AssociatedControlID="ProxyFirstNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ProxyFirstNameLiteral" runat="server" Text='<%#: Eval("ProxyFirstName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyLastName") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyLastNameLabel" runat="server" Text="Proxy Last Name:" AssociatedControlID="ProxyLastNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ProxyLastNameLiteral" runat="server" Text='<%#: Eval("ProxyLastName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyMiddleName") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyMiddleNameLabel" runat="server" Text="Proxy Middle Name:" AssociatedControlID="ProxyMiddleNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ProxyMiddleNameLiteral" runat="server" Text='<%#: Eval("ProxyMiddleName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyBarcode") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyBarcodeLabel" runat="server" Text="Proxy Barcode:" AssociatedControlID="ProxyBarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ProxyBarcodeLiteral" runat="server" Text='<%#: Eval("ProxyBarcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ProxyPatronGroup") != null %>'>
                                <td>
                                    <asp:Label ID="ProxyPatronGroupLabel" runat="server" Text="Proxy Patron Group:" AssociatedControlID="ProxyPatronGroupLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ProxyPatronGroupLiteral" runat="server" Text='<%#: Eval("ProxyPatronGroup") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FulfilmentPreference") != null %>'>
                                <td>
                                    <asp:Label ID="FulfilmentPreferenceLabel" runat="server" Text="Fulfilment Preference:" AssociatedControlID="FulfilmentPreferenceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FulfilmentPreferenceLiteral" runat="server" Text='<%#: Eval("FulfilmentPreference") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DeliveryAddressType") != null %>'>
                                <td>
                                    <asp:Label ID="DeliveryAddressTypeLabel" runat="server" Text="Delivery Address Type:" AssociatedControlID="DeliveryAddressTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="DeliveryAddressTypeHyperLink" runat="server" Text='<%#: Eval("DeliveryAddressType.Name") %>' NavigateUrl='<%# $"~/AddressType2s/Edit.aspx?Id={Eval("DeliveryAddressTypeId")}" %>' Enabled='<%# Session["AddressType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequestExpirationDate") != null %>'>
                                <td>
                                    <asp:Label ID="RequestExpirationDateLabel" runat="server" Text="Request Expiration Date:" AssociatedControlID="RequestExpirationDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequestExpirationDateLiteral" runat="server" Text='<%# Eval("RequestExpirationDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldShelfExpirationDate") != null %>'>
                                <td>
                                    <asp:Label ID="HoldShelfExpirationDateLabel" runat="server" Text="Hold Shelf Expiration Date:" AssociatedControlID="HoldShelfExpirationDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldShelfExpirationDateLiteral" runat="server" Text='<%# Eval("HoldShelfExpirationDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PickupServicePoint") != null %>'>
                                <td>
                                    <asp:Label ID="PickupServicePointLabel" runat="server" Text="Pickup Service Point:" AssociatedControlID="PickupServicePointHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PickupServicePointHyperLink" runat="server" Text='<%#: Eval("PickupServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("PickupServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("AwaitingPickupRequestClosedDate") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPickupRequestClosedDateLabel" runat="server" Text="Awaiting Pickup Request Closed Date:" AssociatedControlID="AwaitingPickupRequestClosedDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPickupRequestClosedDateLiteral" runat="server" Text='<%# Eval("AwaitingPickupRequestClosedDate", "{0:d}") %>' />
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
    <asp:Panel ID="RequestIdentifiersPanel" runat="server" Visible='<%# (string)Session["RequestIdentifiersPermission"] != null && Request2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RequestIdentifiersHyperLink" runat="server" Text="Request Identifiers" NavigateUrl="~/RequestIdentifiers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RequestIdentifiersRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RequestIdentifiersRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No request identifiers found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Identifier Type" DataField="IdentifierType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdentifierTypeHyperLink" runat="server" Text='<%#: Eval("IdentifierType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("IdentifierTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RequestTagsPanel" runat="server" Visible='<%# (string)Session["RequestTagsPermission"] != null && Request2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RequestTagsHyperLink" runat="server" Text="Request Tags" NavigateUrl="~/RequestTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="RequestTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RequestTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No request tags found">
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
    <asp:Panel ID="ScheduledNotice2sPanel" runat="server" Visible='<%# (string)Session["ScheduledNotice2sPermission"] != null && Request2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ScheduledNotice2sHyperLink" runat="server" Text="Scheduled Notices" NavigateUrl="~/ScheduledNotice2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="ScheduledNotice2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ScheduledNotice2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No scheduled notices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/ScheduledNotice2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/ScheduledNotice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Loan" DataField="Loan.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment" DataField="Payment.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentHyperLink" runat="server" Text='<%# Eval("Payment.Id") %>' NavigateUrl='<%# $"~/Payment2s/Edit.aspx?Id={Eval("PaymentId")}" %>' Enabled='<%# Session["Payment2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Recipient User" DataField="RecipientUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RecipientUserHyperLink" runat="server" Text='<%#: Eval("RecipientUserId") != null ? Eval("RecipientUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("RecipientUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Next Run Time" DataField="NextRunTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Triggering Event" DataField="TriggeringEvent" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Timing" DataField="NoticeConfigTiming" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Recurring Period Duration" DataField="NoticeConfigRecurringPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Recurring Period Interval" DataField="NoticeConfigRecurringPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Notice Config Template" DataField="NoticeConfigTemplate.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NoticeConfigTemplateHyperLink" runat="server" Text='<%#: Eval("NoticeConfigTemplateId") != null ? Eval("NoticeConfigTemplate.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("NoticeConfigTemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Notice Config Format" DataField="NoticeConfigFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Send In Real Time" DataField="NoticeConfigSendInRealTime" AutoPostBackOnFilter="true" />
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
            <telerik:AjaxSetting AjaxControlID="RequestIdentifiersRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RequestIdentifiersPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RequestTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RequestTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ScheduledNotice2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ScheduledNotice2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
