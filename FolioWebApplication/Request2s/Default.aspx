<%@ Page Title="Requests" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Request2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Request2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="Request2sHyperLink" runat="server" Text="Requests" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="Request2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="Request2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No requests found" CommandItemDisplay="Top">
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
                        <telerik:GridBoundColumn HeaderText="Request Level" DataField="RequestLevel" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Request Type" DataField="RequestType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Ecs Request Phase" DataField="EcsRequestPhase" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Request Date" DataField="RequestDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Patron Comments" DataField="PatronComments" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Requester" DataField="Requester.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RequesterHyperLink" runat="server" Text='<%#: Eval("RequesterId") != null ? Eval("Requester.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("RequesterId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proxy User" DataField="ProxyUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProxyUserHyperLink" runat="server" Text='<%#: Eval("ProxyUserId") != null ? Eval("ProxyUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ProxyUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Instance" DataField="Instance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Holding" DataField="Holding.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("HoldingId") != null ? Eval("Holding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Cancellation Reason" DataField="CancellationReason.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CancellationReasonHyperLink" runat="server" Text='<%#: Eval("CancellationReason.Name") %>' NavigateUrl='<%# $"~/CancellationReason2s/Edit.aspx?Id={Eval("CancellationReasonId")}" %>' Enabled='<%# Session["CancellationReason2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cancelled By User" DataField="CancelledByUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CancelledByUserHyperLink" runat="server" Text='<%#: Eval("CancelledByUserId") != null ? Eval("CancelledByUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CancelledByUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Cancellation Additional Information" DataField="CancellationAdditionalInformation" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Cancelled Date" DataField="CancelledDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Position" DataField="Position" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Instance Title" DataField="InstanceTitle" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Barcode" DataField="ItemBarcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester First Name" DataField="RequesterFirstName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester Last Name" DataField="RequesterLastName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester Middle Name" DataField="RequesterMiddleName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester Barcode" DataField="RequesterBarcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester Patron Group" DataField="RequesterPatronGroup" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Proxy First Name" DataField="ProxyFirstName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Proxy Last Name" DataField="ProxyLastName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Proxy Middle Name" DataField="ProxyMiddleName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Proxy Barcode" DataField="ProxyBarcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Proxy Patron Group" DataField="ProxyPatronGroup" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Fulfillment Preference" DataField="FulfillmentPreference" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Delivery Address Type" DataField="DeliveryAddressType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="DeliveryAddressTypeHyperLink" runat="server" Text='<%#: Eval("DeliveryAddressType.Name") %>' NavigateUrl='<%# $"~/AddressType2s/Edit.aspx?Id={Eval("DeliveryAddressTypeId")}" %>' Enabled='<%# Session["AddressType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Request Expiration Date" DataField="RequestExpirationDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Hold Shelf Expiration Date" DataField="HoldShelfExpirationDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Pickup Service Point" DataField="PickupServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PickupServicePointHyperLink" runat="server" Text='<%#: Eval("PickupServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("PickupServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Print Details Print Count" DataField="PrintDetailsPrintCount" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Print Details Requester" DataField="PrintDetailsRequester.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PrintDetailsRequesterHyperLink" runat="server" Text='<%#: Eval("PrintDetailsRequesterId") != null ? Eval("PrintDetailsRequester.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("PrintDetailsRequesterId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Print Details Is Printed" DataField="PrintDetailsIsPrinted" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Print Details Print Event Date" DataField="PrintDetailsPrintEventDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Pickup Request Closed Date" DataField="AwaitingPickupRequestClosedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Search Index Call Number Components Call Number" DataField="SearchIndexCallNumberComponentsCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Search Index Call Number Components Prefix" DataField="SearchIndexCallNumberComponentsPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Search Index Call Number Components Suffix" DataField="SearchIndexCallNumberComponentsSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Search Index Shelving Order" DataField="SearchIndexShelvingOrder" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Search Index Pickup Service Point Name" DataField="SearchIndexPickupServicePointName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Location Code" DataField="ItemLocationCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                    <telerik:AjaxUpdatedControl ControlID="Request2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Request2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Request2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
