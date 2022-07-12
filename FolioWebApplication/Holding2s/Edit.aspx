<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Holding2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Holding2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Holding2HyperLink" runat="server" Text="Holding" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Holding2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Holding2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewHolding2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                <td>
                                    <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%#: Eval("ShortId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HoldingType") != null %>'>
                                <td>
                                    <asp:Label ID="HoldingTypeLabel" runat="server" Text="Holding Type:" AssociatedControlID="HoldingTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="HoldingTypeHyperLink" runat="server" Text='<%#: Eval("HoldingType.Name") %>' NavigateUrl='<%# $"~/HoldingType2s/Edit.aspx?Id={Eval("HoldingTypeId")}" %>' Enabled='<%# Session["HoldingType2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("Location") != null %>'>
                                <td>
                                    <asp:Label ID="LocationLabel" runat="server" Text="Location:" AssociatedControlID="LocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TemporaryLocation") != null %>'>
                                <td>
                                    <asp:Label ID="TemporaryLocationLabel" runat="server" Text="Temporary Location:" AssociatedControlID="TemporaryLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EffectiveLocation") != null %>'>
                                <td>
                                    <asp:Label ID="EffectiveLocationLabel" runat="server" Text="Effective Location:" AssociatedControlID="EffectiveLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CallNumberType") != null %>'>
                                <td>
                                    <asp:Label ID="CallNumberTypeLabel" runat="server" Text="Call Number Type:" AssociatedControlID="CallNumberTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CallNumberPrefix") != null %>'>
                                <td>
                                    <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberPrefixLiteral" runat="server" Text='<%#: Eval("CallNumberPrefix") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CallNumber") != null %>'>
                                <td>
                                    <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberLiteral" runat="server" Text='<%#: Eval("CallNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CallNumberSuffix") != null %>'>
                                <td>
                                    <asp:Label ID="CallNumberSuffixLabel" runat="server" Text="Call Number Suffix:" AssociatedControlID="CallNumberSuffixLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberSuffixLiteral" runat="server" Text='<%#: Eval("CallNumberSuffix") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShelvingTitle") != null %>'>
                                <td>
                                    <asp:Label ID="ShelvingTitleLabel" runat="server" Text="Shelving Title:" AssociatedControlID="ShelvingTitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShelvingTitleLiteral" runat="server" Text='<%#: Eval("ShelvingTitle") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AcquisitionFormat") != null %>'>
                                <td>
                                    <asp:Label ID="AcquisitionFormatLabel" runat="server" Text="Acquisition Format:" AssociatedControlID="AcquisitionFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AcquisitionFormatLiteral" runat="server" Text='<%#: Eval("AcquisitionFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AcquisitionMethod") != null %>'>
                                <td>
                                    <asp:Label ID="AcquisitionMethodLabel" runat="server" Text="Acquisition Method:" AssociatedControlID="AcquisitionMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AcquisitionMethodLiteral" runat="server" Text='<%#: Eval("AcquisitionMethod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceiptStatus") != null %>'>
                                <td>
                                    <asp:Label ID="ReceiptStatusLabel" runat="server" Text="Receipt Status:" AssociatedControlID="ReceiptStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceiptStatusLiteral" runat="server" Text='<%#: Eval("ReceiptStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IllPolicy") != null %>'>
                                <td>
                                    <asp:Label ID="IllPolicyLabel" runat="server" Text="Ill Policy:" AssociatedControlID="IllPolicyHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IllPolicyHyperLink" runat="server" Text='<%#: Eval("IllPolicy.Name") %>' NavigateUrl='<%# $"~/IllPolicy2s/Edit.aspx?Id={Eval("IllPolicyId")}" %>' Enabled='<%# Session["IllPolicy2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RetentionPolicy") != null %>'>
                                <td>
                                    <asp:Label ID="RetentionPolicyLabel" runat="server" Text="Retention Policy:" AssociatedControlID="RetentionPolicyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RetentionPolicyLiteral" runat="server" Text='<%#: Eval("RetentionPolicy") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DigitizationPolicy") != null %>'>
                                <td>
                                    <asp:Label ID="DigitizationPolicyLabel" runat="server" Text="Digitization Policy:" AssociatedControlID="DigitizationPolicyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DigitizationPolicyLiteral" runat="server" Text='<%#: Eval("DigitizationPolicy") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CopyNumber") != null %>'>
                                <td>
                                    <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CopyNumberLiteral" runat="server" Text='<%#: Eval("CopyNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemCount") != null %>'>
                                <td>
                                    <asp:Label ID="ItemCountLabel" runat="server" Text="Item Count:" AssociatedControlID="ItemCountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemCountLiteral" runat="server" Text='<%#: Eval("ItemCount") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceivingHistoryDisplayType") != null %>'>
                                <td>
                                    <asp:Label ID="ReceivingHistoryDisplayTypeLabel" runat="server" Text="Receiving History Display Type:" AssociatedControlID="ReceivingHistoryDisplayTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceivingHistoryDisplayTypeLiteral" runat="server" Text='<%#: Eval("ReceivingHistoryDisplayType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DiscoverySuppress") != null %>'>
                                <td>
                                    <asp:Label ID="DiscoverySuppressLabel" runat="server" Text="Discovery Suppress:" AssociatedControlID="DiscoverySuppressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscoverySuppressLiteral" runat="server" Text='<%#: Eval("DiscoverySuppress") %>' />
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
                            <tr runat="server" visible='<%# Eval("Source") != null %>'>
                                <td>
                                    <asp:Label ID="SourceLabel" runat="server" Text="Source:" AssociatedControlID="SourceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SourceHyperLink" runat="server" Text='<%#: Eval("Source.Name") %>' NavigateUrl='<%# $"~/Source2s/Edit.aspx?Id={Eval("SourceId")}" %>' Enabled='<%# Session["Source2sPermission"] != null %>' />
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
    <asp:Panel ID="BoundWithPart2sPanel" runat="server" Visible='<%# (string)Session["BoundWithPart2sPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BoundWithPart2sHyperLink" runat="server" Text="Bound With Parts" NavigateUrl="~/BoundWithPart2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="BoundWithPart2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BoundWithPart2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No bound with parts found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/BoundWithPart2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/BoundWithPart2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
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
    <asp:Panel ID="ExtentsPanel" runat="server" Visible='<%# (string)Session["ExtentsPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ExtentsHyperLink" runat="server" Text="Extents" NavigateUrl="~/Extents/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ExtentsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ExtentsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No extents found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Note" DataField="StaffNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Fee2sPanel" runat="server" Visible='<%# (string)Session["Fee2sPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Fee2sHyperLink" runat="server" Text="Fees" NavigateUrl="~/Fee2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Fee2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Fee2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No fees found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/users/{Eval("UserId")}/accounts/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Remaining Amount" DataField="RemainingAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Status Name" DataField="PaymentStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Material Type" DataField="MaterialType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status Name" DataField="ItemStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Location" DataField="Location" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Returned Time" DataField="ReturnedTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Loan" DataField="Loan.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material Type 1" DataField="MaterialType1.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialType1HyperLink" runat="server" Text='<%#: Eval("MaterialType1.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fee Type" DataField="FeeType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner" DataField="Owner.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("OwnerId") != null ? Eval("Owner.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Instance" DataField="Instance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="HoldingAdministrativeNotesPanel" runat="server" Visible='<%# (string)Session["HoldingAdministrativeNotesPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingAdministrativeNotesHyperLink" runat="server" Text="Holding Administrative Notes" NavigateUrl="~/HoldingAdministrativeNotes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingAdministrativeNotesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingAdministrativeNotesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding administrative notes found">
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
    <asp:Panel ID="HoldingElectronicAccessesPanel" runat="server" Visible='<%# (string)Session["HoldingElectronicAccessesPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingElectronicAccessesHyperLink" runat="server" Text="Holding Electronic Accesses" NavigateUrl="~/HoldingElectronicAccesses/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingElectronicAccessesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingElectronicAccessesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding electronic accesses found">
                    <Columns>
                        <telerik:GridHyperLinkColumn HeaderText="URI" DataTextField="Uri" DataNavigateUrlFields="Uri" Target="_blank" SortExpression="Uri" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Link Text" DataField="LinkText" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Materials Specification" DataField="MaterialsSpecification" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Public Note" DataField="PublicNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Relationship" DataField="Relationship.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RelationshipHyperLink" runat="server" Text='<%#: Eval("Relationship.Name") %>' NavigateUrl='<%# $"~/ElectronicAccessRelationship2s/Edit.aspx?Id={Eval("RelationshipId")}" %>' Enabled='<%# Session["ElectronicAccessRelationship2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="HoldingEntriesPanel" runat="server" Visible='<%# (string)Session["HoldingEntriesPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingEntriesHyperLink" runat="server" Text="Holding Entries" NavigateUrl="~/HoldingEntries/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingEntriesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingEntriesRadGrid_NeedDataSource" OnItemCommand="HoldingEntriesRadGrid_ItemCommand">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding entries found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Enumeration" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="PrintLinkButton" runat="server" CommandName="Print">Print Label</asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Public Display" DataField="PublicDisplay" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="HoldingFormerIdsPanel" runat="server" Visible='<%# (string)Session["HoldingFormerIdsPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingFormerIdsHyperLink" runat="server" Text="Holding Former Ids" NavigateUrl="~/HoldingFormerIds/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingFormerIdsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingFormerIdsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding former ids found">
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
    <asp:Panel ID="HoldingNotesPanel" runat="server" Visible='<%# (string)Session["HoldingNotesPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingNotesHyperLink" runat="server" Text="Holding Notes" NavigateUrl="~/HoldingNotes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingNotesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingNotesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding notes found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Holding Note Type" DataField="HoldingNoteType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingNoteTypeHyperLink" runat="server" Text='<%#: Eval("HoldingNoteType.Name") %>' NavigateUrl='<%# $"~/HoldingNoteType2s/Edit.aspx?Id={Eval("HoldingNoteTypeId")}" %>' Enabled='<%# Session["HoldingNoteType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Only" DataField="StaffOnly" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="HoldingStatisticalCodesPanel" runat="server" Visible='<%# (string)Session["HoldingStatisticalCodesPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingStatisticalCodesHyperLink" runat="server" Text="Holding Statistical Codes" NavigateUrl="~/HoldingStatisticalCodes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingStatisticalCodesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingStatisticalCodesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding statistical codes found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Statistical Code" DataField="StatisticalCode.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="StatisticalCodeHyperLink" runat="server" Text='<%#: Eval("StatisticalCode.Name") %>' NavigateUrl='<%# $"~/StatisticalCode2s/Edit.aspx?Id={Eval("StatisticalCodeId")}" %>' Enabled='<%# Session["StatisticalCode2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="HoldingTagsPanel" runat="server" Visible='<%# (string)Session["HoldingTagsPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="HoldingTagsHyperLink" runat="server" Text="Holding Tags" NavigateUrl="~/HoldingTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="HoldingTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="HoldingTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holding tags found">
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
    <asp:Panel ID="IndexStatementsPanel" runat="server" Visible='<%# (string)Session["IndexStatementsPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="IndexStatementsHyperLink" runat="server" Text="Index Statements" NavigateUrl="~/IndexStatements/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="IndexStatementsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="IndexStatementsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No index statements found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Statement" DataField="Statement" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Note" DataField="StaffNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Item2sPanel" runat="server" Visible='<%# (string)Session["Item2sPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Item2sHyperLink" runat="server" Text="Items" NavigateUrl="~/Item2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Item2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Item2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No items found">
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="NewLabelHyperLink" runat="server" Text="New Label" NavigateUrl='<%# $"~/Labels/Edit.aspx?Barcode={Eval("Barcode")}" %>' Visible='<%# !string.IsNullOrWhiteSpace((string)Eval("Barcode")) && Session["LabelsPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Barcode" DataField="Barcode" SortExpression="Barcode" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BarcodeHyperLink" runat="server" Text='<%#: Eval("Barcode") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Effective Shelving Order" DataField="EffectiveShelvingOrder" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Call Number Type" DataField="CallNumberType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Effective Call Number" DataField="EffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Prefix" DataField="EffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Suffix" DataField="EffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Effective Call Number Type" DataField="EffectiveCallNumberType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveCallNumberTypeHyperLink" runat="server" Text='<%#: Eval("EffectiveCallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("EffectiveCallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Volume" DataField="Volume" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Identifier" DataField="ItemIdentifier" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pieces Count" DataField="PiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Pieces Description" DataField="PiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Missing Pieces Count" DataField="MissingPiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Missing Pieces Description" DataField="MissingPiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Missing Pieces Time" DataField="MissingPiecesTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Damaged Status" DataField="DamagedStatus.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="DamagedStatusHyperLink" runat="server" Text='<%#: Eval("DamagedStatus.Name") %>' NavigateUrl='<%# $"~/ItemDamagedStatus2s/Edit.aspx?Id={Eval("DamagedStatusId")}" %>' Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Damaged Status Time" DataField="DamagedStatusTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Status Last Write Time" DataField="StatusLastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Material Type" DataField="MaterialType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialTypeHyperLink" runat="server" Text='<%#: Eval("MaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Permanent Loan Type" DataField="PermanentLoanType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLoanTypeHyperLink" runat="server" Text='<%#: Eval("PermanentLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("PermanentLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Temporary Loan Type" DataField="TemporaryLoanType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLoanTypeHyperLink" runat="server" Text='<%#: Eval("TemporaryLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("TemporaryLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Permanent Location" DataField="PermanentLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLocationHyperLink" runat="server" Text='<%#: Eval("PermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("PermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Temporary Location" DataField="TemporaryLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Location" DataField="EffectiveLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="In Transit Destination Service Point" DataField="InTransitDestinationServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InTransitDestinationServicePointHyperLink" runat="server" Text='<%#: Eval("InTransitDestinationServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("InTransitDestinationServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Last Check In Date Time" DataField="LastCheckInDateTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Check In Service Point" DataField="LastCheckInServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInServicePointHyperLink" runat="server" Text='<%#: Eval("LastCheckInServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("LastCheckInServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Check In Staff Member" DataField="LastCheckInStaffMember.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%#: Eval("LastCheckInStaffMemberId") != null ? Eval("LastCheckInStaffMember.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastCheckInStaffMemberId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Receiving2sPanel" runat="server" Visible='<%# (string)Session["Receiving2sPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridBoundColumn HeaderText="Caption" DataField="Caption" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title.Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("TitleId")}" %>' Enabled='<%# Session["Title2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Display On Holding" DataField="DisplayOnHolding" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Status" DataField="ReceivingStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Supplement" DataField="Supplement" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Receipt Time" DataField="ReceiptTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Receive Time" DataField="ReceiveTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="SupplementStatementsPanel" runat="server" Visible='<%# (string)Session["SupplementStatementsPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="SupplementStatementsHyperLink" runat="server" Text="Supplement Statements" NavigateUrl="~/SupplementStatements/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="SupplementStatementsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="SupplementStatementsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, HoldingId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No supplement statements found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Statement" DataField="Statement" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Note" DataField="StaffNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BoundWithPart2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BoundWithPart2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ExtentsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ExtentsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Fee2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Fee2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingAdministrativeNotesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingAdministrativeNotesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingElectronicAccessesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingElectronicAccessesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingEntriesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingEntriesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingFormerIdsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingFormerIdsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingNotesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingNotesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingStatisticalCodesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingStatisticalCodesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="HoldingTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HoldingTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="IndexStatementsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="IndexStatementsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Item2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Item2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Receiving2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Receiving2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="SupplementStatementsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="SupplementStatementsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
