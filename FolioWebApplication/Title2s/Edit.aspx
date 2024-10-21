<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Title2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Title2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Title2HyperLink" runat="server" Text="Title" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Title2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Title2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewTitle2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpectedReceiptDate") != null %>'>
                                <td>
                                    <asp:Label ID="ExpectedReceiptDateLabel" runat="server" Text="Expected Receipt Date:" AssociatedControlID="ExpectedReceiptDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpectedReceiptDateLiteral" runat="server" Text='<%# Eval("ExpectedReceiptDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Title") != null %>'>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TitleLiteral" runat="server" Text='<%#: Eval("Title") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderItem") != null %>'>
                                <td>
                                    <asp:Label ID="OrderItemLabel" runat="server" Text="Order Item:" AssociatedControlID="OrderItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItem.Number") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Instance") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceLabel" runat="server" Text="Instance:" AssociatedControlID="InstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Publisher") != null %>'>
                                <td>
                                    <asp:Label ID="PublisherLabel" runat="server" Text="Publisher:" AssociatedControlID="PublisherLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublisherLiteral" runat="server" Text='<%#: Eval("Publisher") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Edition") != null %>'>
                                <td>
                                    <asp:Label ID="EditionLabel" runat="server" Text="Edition:" AssociatedControlID="EditionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EditionLiteral" runat="server" Text='<%#: Eval("Edition") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PackageName") != null %>'>
                                <td>
                                    <asp:Label ID="PackageNameLabel" runat="server" Text="Package Name:" AssociatedControlID="PackageNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PackageNameLiteral" runat="server" Text='<%#: Eval("PackageName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderItemNumber") != null %>'>
                                <td>
                                    <asp:Label ID="OrderItemNumberLabel" runat="server" Text="Order Item Number:" AssociatedControlID="OrderItemNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderItemNumberLiteral" runat="server" Text='<%#: Eval("OrderItemNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublishedDate") != null %>'>
                                <td>
                                    <asp:Label ID="PublishedDateLabel" runat="server" Text="Published Date:" AssociatedControlID="PublishedDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublishedDateLiteral" runat="server" Text='<%#: Eval("PublishedDate") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceivingNote") != null %>'>
                                <td>
                                    <asp:Label ID="ReceivingNoteLabel" runat="server" Text="Receiving Note:" AssociatedControlID="ReceivingNoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceivingNoteLiteral" runat="server" Text='<%#: Eval("ReceivingNote") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionFrom") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionFromLabel" runat="server" Text="Subscription From:" AssociatedControlID="SubscriptionFromLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionFromLiteral" runat="server" Text='<%# Eval("SubscriptionFrom", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionTo") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionToLabel" runat="server" Text="Subscription To:" AssociatedControlID="SubscriptionToLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionToLiteral" runat="server" Text='<%# Eval("SubscriptionTo", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionInterval") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionIntervalLabel" runat="server" Text="Subscription Interval:" AssociatedControlID="SubscriptionIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionIntervalLiteral" runat="server" Text='<%#: Eval("SubscriptionInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ClaimingActive") != null %>'>
                                <td>
                                    <asp:Label ID="ClaimingActiveLabel" runat="server" Text="Claiming Active:" AssociatedControlID="ClaimingActiveLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ClaimingActiveLiteral" runat="server" Text='<%#: Eval("ClaimingActive") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ClaimingInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ClaimingIntervalLabel" runat="server" Text="Claiming Interval:" AssociatedControlID="ClaimingIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ClaimingIntervalLiteral" runat="server" Text='<%#: Eval("ClaimingInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IsAcknowledged") != null %>'>
                                <td>
                                    <asp:Label ID="IsAcknowledgedLabel" runat="server" Text="Is Acknowledged:" AssociatedControlID="IsAcknowledgedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="IsAcknowledgedLiteral" runat="server" Text='<%#: Eval("IsAcknowledged") %>' />
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
    <asp:Panel ID="Receiving2sPanel" runat="server" Visible='<%# (string)Session["Receiving2sPermission"] != null && Title2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Receiving2sHyperLink" runat="server" Text="Receivings" NavigateUrl="~/Receiving2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Receiving2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Receiving2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No receivings found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="ReceiveTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Receiving2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/receiving/{Eval("TitleId")}/view" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Receiving2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Display Summary" DataField="DisplaySummary" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Bind Item Id" DataField="BindItemId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridBoundColumn HeaderText="Bind Item Tenant Id" DataField="BindItemTenantId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridTemplateColumn HeaderText="Location" DataField="Location.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Holding" DataField="Holding.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("HoldingId") != null ? Eval("Holding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Receiving Tenant Id" DataField="ReceivingTenantId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridBoundColumn HeaderText="Display On Holding" DataField="DisplayOnHolding" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Display To Public" DataField="DisplayToPublic" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Status" DataField="ReceivingStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Supplement" DataField="Supplement" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Is Bound" DataField="IsBound" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Receipt Time" DataField="ReceiptTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Receive Time" DataField="ReceiveTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Status Updated Date" DataField="StatusUpdatedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Claiming Interval" DataField="ClaimingInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Internal Note" DataField="InternalNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="External Note" DataField="ExternalNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="TitleAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["TitleAcquisitionsUnitsPermission"] != null && Title2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="TitleAcquisitionsUnitsHyperLink" runat="server" Text="Title Acquisitions Units" NavigateUrl="~/TitleAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="TitleAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="TitleAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No title acquisitions units found">
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
    <asp:Panel ID="TitleBindItemIdsPanel" runat="server" Visible='<%# (string)Session["TitleBindItemIdsPermission"] != null && Title2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="TitleBindItemIdsHyperLink" runat="server" Text="Title Bind Item Ids" NavigateUrl="~/TitleBindItemIds/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="TitleBindItemIdsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="TitleBindItemIdsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No title bind item ids found">
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
    <asp:Panel ID="TitleContributorsPanel" runat="server" Visible='<%# (string)Session["TitleContributorsPermission"] != null && Title2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="TitleContributorsHyperLink" runat="server" Text="Title Contributors" NavigateUrl="~/TitleContributors/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="TitleContributorsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="TitleContributorsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No title contributors found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Contributor" DataField="Contributor" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Contributor Name Type" DataField="ContributorNameType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ContributorNameTypeHyperLink" runat="server" Text='<%#: Eval("ContributorNameType.Name") %>' NavigateUrl='<%# $"~/ContributorNameType2s/Edit.aspx?Id={Eval("ContributorNameTypeId")}" %>' Enabled='<%# Session["ContributorNameType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="TitleProductIdsPanel" runat="server" Visible='<%# (string)Session["TitleProductIdsPermission"] != null && Title2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="TitleProductIdsHyperLink" runat="server" Text="Title Product Ids" NavigateUrl="~/TitleProductIds/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="TitleProductIdsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="TitleProductIdsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No title product ids found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Product Id" DataField="ProductId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Product Id Type" DataField="ProductIdType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProductIdTypeHyperLink" runat="server" Text='<%#: Eval("ProductIdType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("ProductIdTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Qualifier" DataField="Qualifier" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Receiving2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Receiving2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TitleAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TitleAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TitleBindItemIdsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TitleBindItemIdsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TitleContributorsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TitleContributorsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TitleProductIdsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TitleProductIdsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
