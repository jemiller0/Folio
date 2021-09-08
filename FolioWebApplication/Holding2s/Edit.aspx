<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Holding2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Holding2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Holding2HyperLink" runat="server" Text="Holding" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Holding2FormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["Holding2sPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="Holding2FormView_DataBinding" OnItemUpdating="Holding2FormView_ItemUpdating" OnItemDeleting="Holding2FormView_ItemDeleting"
                OnItemCommand="Holding2FormView_ItemCommand" Enabled='<%# true || (string)Session["Holding2sPermission"] == "Edit" %>'>
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
                            <tr runat="server" visible='<%# Eval("Version") != null %>'>
                                <td>
                                    <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VersionLiteral" runat="server" Text='<%#: Eval("Version") %>' />
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
                <EditItemTemplate>
                    <asp:Panel ID="EditHolding2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="Holding2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Holding2" Visible="false" />
                        </div>
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="VersionRadNumericTextBox" runat="server" DbValue='<%# Bind("Version") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                <td>
                                    <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%# Eval("ShortId") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Holding Type:" NavigateUrl="~/HoldingType2s/Default.aspx" Enabled='<%# Session["HoldingType2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="HoldingTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("HoldingTypeId") %>' OnDataBinding="HoldingTypeRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Instance:" NavigateUrl="~/Instance2s/Default.aspx" Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="InstanceRadComboBox" runat="server" MaxHeight="500px" Width="500px" EnableLoadOnDemand="true" SelectedValue='<%# Bind("InstanceId") %>' OnDataBinding="InstanceRadComboBox_DataBinding" OnItemsRequested="InstanceRadComboBox_ItemsRequested" />
                                    <asp:RequiredFieldValidator ID="InstanceRequiredFieldValidator" runat="server" ControlToValidate="InstanceRadComboBox" ErrorMessage="The Instance field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2" />
                                    <asp:CustomValidator ID="InstanceCustomValidator" runat="server" ErrorMessage="The Instance field is invalid." ControlToValidate="InstanceRadComboBox" Display="Dynamic" CssClass="Error" OnServerValidate="InstanceCustomValidator_ServerValidate" ValidationGroup="Holding2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="LocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("LocationId") %>' OnDataBinding="LocationRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="LocationRequiredFieldValidator" runat="server" ControlToValidate="LocationRadComboBox" ErrorMessage="The Location field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Temporary Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="TemporaryLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLocationId") %>' OnDataBinding="TemporaryLocationRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Effective Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="EffectiveLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("EffectiveLocationId") %>' OnDataBinding="EffectiveLocationRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="CallNumberTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("CallNumberTypeId") %>' OnDataBinding="CallNumberTypeRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CallNumberPrefixRadTextBox" runat="server" Text='<%# Bind("CallNumberPrefix") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CallNumberRadTextBox" runat="server" Text='<%# Bind("CallNumber") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CallNumberSuffixLabel" runat="server" Text="Call Number Suffix:" AssociatedControlID="CallNumberSuffixRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CallNumberSuffixRadTextBox" runat="server" Text='<%# Bind("CallNumberSuffix") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ShelvingTitleLabel" runat="server" Text="Shelving Title:" AssociatedControlID="ShelvingTitleRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ShelvingTitleRadTextBox" runat="server" Text='<%# Bind("ShelvingTitle") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="AcquisitionFormatLabel" runat="server" Text="Acquisition Format:" AssociatedControlID="AcquisitionFormatRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="AcquisitionFormatRadTextBox" runat="server" Text='<%# Bind("AcquisitionFormat") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="AcquisitionMethodLabel" runat="server" Text="Acquisition Method:" AssociatedControlID="AcquisitionMethodRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="AcquisitionMethodRadTextBox" runat="server" Text='<%# Bind("AcquisitionMethod") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ReceiptStatusLabel" runat="server" Text="Receipt Status:" AssociatedControlID="ReceiptStatusRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ReceiptStatusRadTextBox" runat="server" Text='<%# Bind("ReceiptStatus") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Ill Policy:" NavigateUrl="~/IllPolicy2s/Default.aspx" Enabled='<%# Session["IllPolicy2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="IllPolicyRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("IllPolicyId") %>' OnDataBinding="IllPolicyRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="RetentionPolicyLabel" runat="server" Text="Retention Policy:" AssociatedControlID="RetentionPolicyRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RetentionPolicyRadTextBox" runat="server" Text='<%# Bind("RetentionPolicy") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="DigitizationPolicyLabel" runat="server" Text="Digitization Policy:" AssociatedControlID="DigitizationPolicyRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="DigitizationPolicyRadTextBox" runat="server" Text='<%# Bind("DigitizationPolicy") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CopyNumberRadTextBox" runat="server" Text='<%# Bind("CopyNumber") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ItemCountLabel" runat="server" Text="Item Count:" AssociatedControlID="ItemCountRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ItemCountRadTextBox" runat="server" Text='<%# Bind("ItemCount") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ReceivingHistoryDisplayTypeLabel" runat="server" Text="Receiving History Display Type:" AssociatedControlID="ReceivingHistoryDisplayTypeRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ReceivingHistoryDisplayTypeRadTextBox" runat="server" Text='<%# Bind("ReceivingHistoryDisplayType") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="DiscoverySuppressRadCheckBox" runat="server" Text="Discovery Suppress" Checked='<%# Bind("DiscoverySuppress") %>' AutoPostBack="false" />
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
                                    <asp:HyperLink runat="server" Text="Creation User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%# Eval("CreationUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("CreationUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
                                    <asp:HyperLink runat="server" Text="Last Write User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%# Eval("LastWriteUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastWriteUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Source:" NavigateUrl="~/Source2s/Default.aspx" Enabled='<%# Session["Source2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="SourceRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("SourceId") %>' OnDataBinding="SourceRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
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
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# Holding2FormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="Holding2" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this holding?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# Holding2FormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Holding cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="Holding2" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Item" DataField="Item.ShortId" SortExpression="Item.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Returned Time" DataField="ReturnedTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Loan" DataField="Loan.Id" SortExpression="Loan.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="User" DataField="User.Username" SortExpression="User.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Item" DataField="Item.ShortId" SortExpression="Item.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Material Type 1" DataField="MaterialType1.Name" SortExpression="MaterialType1.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialType1HyperLink" runat="server" Text='<%#: Eval("MaterialType1.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Fee Type" DataField="FeeType.Name" SortExpression="FeeType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Owner" DataField="Owner.Name" SortExpression="Owner.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("OwnerId") != null ? Eval("Owner.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance" DataField="Instance.Title" SortExpression="Instance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Item2sPanel" runat="server" Visible='<%# (string)Session["Item2sPermission"] != null && Holding2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Item2sHyperLink" runat="server" Text="Items" NavigateUrl="~/Item2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Item2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Item2sRadGrid_NeedDataSource" OnItemCommand="Item2sRadGrid_ItemCommand" OnInsertCommand="Item2sRadGrid_UpdateCommand" OnUpdateCommand="Item2sRadGrid_UpdateCommand" OnDeleteCommand="Item2sRadGrid_DeleteCommand">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No items found" CommandItemDisplay='<%# (string)Session["Item2sPermission"] == "Edit" ? GridCommandItemDisplay.Top : GridCommandItemDisplay.None %>' CommandItemSettings-ShowAddNewRecordButton='<%# (string)Session["Item2sPermission"] == "Edit" %>' InsertItemPageIndexAction="ShowItemOnFirstPage">
                    <CommandItemSettings AddNewRecordText="New Item" />
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditLinkButton" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false" Visible='<%# (string)Session["Item2sPermission"] == "Edit" %>' />
                                <asp:HyperLink ID="ViewHyperLink" runat="server" Text="View" NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' Visible='<%# (string)Session["Item2sPermission"] == "View" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteLinkButton" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?")' Visible='<%# (string)Session["Item2sPermission"] == "Edit" %>' />
                                <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Item cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" />
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
                        <telerik:GridBoundColumn HeaderText="Version" DataField="Version" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Shelving Order" DataField="EffectiveShelvingOrder" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Call Number Type" DataField="CallNumberType.Name" SortExpression="CallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Effective Call Number" DataField="EffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Prefix" DataField="EffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Suffix" DataField="EffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Call Number Type" DataField="EffectiveCallNumberType.Name" SortExpression="EffectiveCallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Damaged Status" DataField="DamagedStatus.Name" SortExpression="DamagedStatus.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="DamagedStatusHyperLink" runat="server" Text='<%#: Eval("DamagedStatus.Name") %>' NavigateUrl='<%# $"~/ItemDamagedStatus2s/Edit.aspx?Id={Eval("DamagedStatusId")}" %>' Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Damaged Status Time" DataField="DamagedStatusTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Status Date" DataField="StatusDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Material Type" DataField="MaterialType.Name" SortExpression="MaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialTypeHyperLink" runat="server" Text='<%#: Eval("MaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Loan Type" DataField="PermanentLoanType.Name" SortExpression="PermanentLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLoanTypeHyperLink" runat="server" Text='<%#: Eval("PermanentLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("PermanentLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Loan Type" DataField="TemporaryLoanType.Name" SortExpression="TemporaryLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLoanTypeHyperLink" runat="server" Text='<%#: Eval("TemporaryLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("TemporaryLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Location" DataField="PermanentLocation.Name" SortExpression="PermanentLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLocationHyperLink" runat="server" Text='<%#: Eval("PermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("PermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Location" DataField="TemporaryLocation.Name" SortExpression="TemporaryLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Location" DataField="EffectiveLocation.Name" SortExpression="EffectiveLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="In Transit Destination Service Point" DataField="InTransitDestinationServicePoint.Name" SortExpression="InTransitDestinationServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InTransitDestinationServicePointHyperLink" runat="server" Text='<%#: Eval("InTransitDestinationServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("InTransitDestinationServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order Item" DataField="OrderItem.Number" SortExpression="OrderItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Last Check In Date Time" DataField="LastCheckInDateTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Service Point" DataField="LastCheckInServicePoint.Name" SortExpression="LastCheckInServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInServicePointHyperLink" runat="server" Text='<%#: Eval("LastCheckInServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("LastCheckInServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Staff Member" DataField="LastCheckInStaffMember.Username" SortExpression="LastCheckInStaffMember.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%#: Eval("LastCheckInStaffMemberId") != null ? Eval("LastCheckInStaffMember.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastCheckInStaffMemberId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <asp:Panel ID="EditItem2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                                <div>
                                    <asp:ValidationSummary ID="Item2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Item2s" Visible="false" />
                                </div>
                                <table>
                                    <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                        <td>
                                            <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionRadNumericTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="VersionRadNumericTextBox" runat="server" DbValue='<%# Bind("Version") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                        <td>
                                            <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%# Eval("ShortId") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="DiscoverySuppressRadCheckBox" runat="server" Text="Discovery Suppress" Checked='<%# Bind("DiscoverySuppress") %>' AutoPostBack="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="AccessionNumberLabel" runat="server" Text="Accession Number:" AssociatedControlID="AccessionNumberRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="AccessionNumberRadTextBox" runat="server" Text='<%# Bind("AccessionNumber") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="BarcodeLabel" runat="server" Text="Barcode:" AssociatedControlID="BarcodeRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="BarcodeRadTextBox" runat="server" Text='<%# Bind("Barcode") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveShelvingOrder") != null %>'>
                                        <td>
                                            <asp:Label ID="EffectiveShelvingOrderLabel" runat="server" Text="Effective Shelving Order:" AssociatedControlID="EffectiveShelvingOrderLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="EffectiveShelvingOrderLiteral" runat="server" Text='<%#: Eval("EffectiveShelvingOrder") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberRadTextBox" runat="server" Text='<%# Bind("CallNumber") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberPrefixRadTextBox" runat="server" Text='<%# Bind("CallNumberPrefix") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberSuffixLabel" runat="server" Text="Call Number Suffix:" AssociatedControlID="CallNumberSuffixRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberSuffixRadTextBox" runat="server" Text='<%# Bind("CallNumberSuffix") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="CallNumberTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("CallNumberTypeId") %>' OnDataBinding="Item2sCallNumberTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveCallNumber") != null %>'>
                                        <td>
                                            <asp:Label ID="EffectiveCallNumberLabel" runat="server" Text="Effective Call Number:" AssociatedControlID="EffectiveCallNumberLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="EffectiveCallNumberLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumber") %>' />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveCallNumberPrefix") != null %>'>
                                        <td>
                                            <asp:Label ID="EffectiveCallNumberPrefixLabel" runat="server" Text="Effective Call Number Prefix:" AssociatedControlID="EffectiveCallNumberPrefixLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="EffectiveCallNumberPrefixLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumberPrefix") %>' />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveCallNumberSuffix") != null %>'>
                                        <td>
                                            <asp:Label ID="EffectiveCallNumberSuffixLabel" runat="server" Text="Effective Call Number Suffix:" AssociatedControlID="EffectiveCallNumberSuffixLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="EffectiveCallNumberSuffixLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumberSuffix") %>' />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveCallNumberType") != null %>'>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Effective Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="EffectiveCallNumberTypeHyperLink" runat="server" Text='<%# Eval("EffectiveCallNumberType.Name") %>' NavigateUrl='<%# "~/CallNumberType2s/Edit.aspx?Id=" + Eval("EffectiveCallNumberTypeId") %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="VolumeLabel" runat="server" Text="Volume:" AssociatedControlID="VolumeRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="VolumeRadTextBox" runat="server" Text='<%# Bind("Volume") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="EnumerationLabel" runat="server" Text="Enumeration:" AssociatedControlID="EnumerationRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="EnumerationRadTextBox" runat="server" Text='<%# Bind("Enumeration") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ChronologyLabel" runat="server" Text="Chronology:" AssociatedControlID="ChronologyRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ChronologyRadTextBox" runat="server" Text='<%# Bind("Chronology") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ItemIdentifierLabel" runat="server" Text="Item Identifier:" AssociatedControlID="ItemIdentifierRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ItemIdentifierRadTextBox" runat="server" Text='<%# Bind("ItemIdentifier") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CopyNumberRadTextBox" runat="server" Text='<%# Bind("CopyNumber") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="PiecesCountLabel" runat="server" Text="Pieces Count:" AssociatedControlID="PiecesCountRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="PiecesCountRadTextBox" runat="server" Text='<%# Bind("PiecesCount") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="PiecesDescriptionLabel" runat="server" Text="Pieces Description:" AssociatedControlID="PiecesDescriptionRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="PiecesDescriptionRadTextBox" runat="server" Text='<%# Bind("PiecesDescription") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="MissingPiecesCountLabel" runat="server" Text="Missing Pieces Count:" AssociatedControlID="MissingPiecesCountRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="MissingPiecesCountRadTextBox" runat="server" Text='<%# Bind("MissingPiecesCount") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="MissingPiecesDescriptionLabel" runat="server" Text="Missing Pieces Description:" AssociatedControlID="MissingPiecesDescriptionRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="MissingPiecesDescriptionRadTextBox" runat="server" Text='<%# Bind("MissingPiecesDescription") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="MissingPiecesTimeLabel" runat="server" Text="Missing Pieces Time:" AssociatedControlID="MissingPiecesTimeRadDateTimePicker" />
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="MissingPiecesTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("MissingPiecesTime") %>' Width="500px" MinDate="1/1/1900" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Damaged Status:" NavigateUrl="~/ItemDamagedStatus2s/Default.aspx" Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="DamagedStatusRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("DamagedStatusId") %>' OnDataBinding="Item2sDamagedStatusRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="DamagedStatusTimeLabel" runat="server" Text="Damaged Status Time:" AssociatedControlID="DamagedStatusTimeRadDateTimePicker" />
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="DamagedStatusTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("DamagedStatusTime") %>' Width="500px" MinDate="1/1/1900" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="StatusNameLabel" runat="server" Text="Status Name:" AssociatedControlID="StatusNameRadComboBox" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="StatusNameRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" SelectedValue='<%# Bind("StatusName") %>' OnDataBinding="Item2sStatusNameRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="StatusNameRequiredFieldValidator" runat="server" ControlToValidate="StatusNameRadComboBox" ErrorMessage="The Status Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Item2s" />
                                            <asp:RegularExpressionValidator ID="StatusNameRegularExpressionValidator" runat="server" ErrorMessage="The Status Name field must match the regular expression '^(Aged to lost|Available|Awaiting pickup|Awaiting delivery|Checked out|Claimed returned|Declared lost|In process|In process (non-requestable)|In transit|Intellectual item|Long missing|Lost and paid|Missing|On order|Paged|Restricted|Order closed|Unavailable|Unknown|Withdrawn)$'." ControlToValidate="StatusNameRadComboBox" Display="Dynamic" CssClass="Error" ValidationExpression="^(Aged to lost|Available|Awaiting pickup|Awaiting delivery|Checked out|Claimed returned|Declared lost|In process|In process (non-requestable)|In transit|Intellectual item|Long missing|Lost and paid|Missing|On order|Paged|Restricted|Order closed|Unavailable|Unknown|Withdrawn)$" ValidationGroup="Item2s" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("StatusDate") != null %>'>
                                        <td>
                                            <asp:Label ID="StatusDateLabel" runat="server" Text="Status Date:" AssociatedControlID="StatusDateLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="StatusDateLiteral" runat="server" Text='<%# Eval("StatusDate", "{0:d}") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Material Type:" NavigateUrl="~/MaterialType2s/Default.aspx" Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="MaterialTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("MaterialTypeId") %>' OnDataBinding="Item2sMaterialTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="MaterialTypeRequiredFieldValidator" runat="server" ControlToValidate="MaterialTypeRadComboBox" ErrorMessage="The Material Type field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Item2s" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Permanent Loan Type:" NavigateUrl="~/LoanType2s/Default.aspx" Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="PermanentLoanTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("PermanentLoanTypeId") %>' OnDataBinding="Item2sPermanentLoanTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="PermanentLoanTypeRequiredFieldValidator" runat="server" ControlToValidate="PermanentLoanTypeRadComboBox" ErrorMessage="The Permanent Loan Type field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Item2s" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Temporary Loan Type:" NavigateUrl="~/LoanType2s/Default.aspx" Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TemporaryLoanTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLoanTypeId") %>' OnDataBinding="Item2sTemporaryLoanTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Permanent Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="PermanentLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("PermanentLocationId") %>' OnDataBinding="Item2sPermanentLocationRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Temporary Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TemporaryLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLocationId") %>' OnDataBinding="Item2sTemporaryLocationRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("EffectiveLocation") != null %>'>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Effective Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%# Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# "~/Location2s/Edit.aspx?Id=" + Eval("EffectiveLocationId") %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="In Transit Destination Service Point:" NavigateUrl="~/ServicePoint2s/Default.aspx" Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="InTransitDestinationServicePointRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("InTransitDestinationServicePointId") %>' OnDataBinding="Item2sInTransitDestinationServicePointRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Order Item:" NavigateUrl="~/OrderItem2s/Default.aspx" Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="OrderItemRadComboBox" runat="server" MaxHeight="500px" Width="500px" EnableLoadOnDemand="true" SelectedValue='<%# Bind("OrderItemId") %>' OnDataBinding="Item2sOrderItemRadComboBox_DataBinding" OnItemsRequested="Item2sOrderItemRadComboBox_ItemsRequested" />
                                            <asp:CustomValidator ID="OrderItemCustomValidator" runat="server" ErrorMessage="The Order Item field is invalid." ControlToValidate="OrderItemRadComboBox" Display="Dynamic" CssClass="Error" OnServerValidate="Item2sOrderItemCustomValidator_ServerValidate" ValidationGroup="Item2s" />
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
                                            <asp:HyperLink runat="server" Text="Creation User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%# Eval("CreationUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("CreationUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
                                            <asp:HyperLink runat="server" Text="Last Write User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%# Eval("LastWriteUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastWriteUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LastCheckInDateTimeLabel" runat="server" Text="Last Check In Date Time:" AssociatedControlID="LastCheckInDateTimeRadDateTimePicker" />
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="LastCheckInDateTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("LastCheckInDateTime") %>' Width="500px" MinDate="1/1/1900" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Last Check In Service Point:" NavigateUrl="~/ServicePoint2s/Default.aspx" Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="LastCheckInServicePointRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("LastCheckInServicePointId") %>' OnDataBinding="Item2sLastCheckInServicePointRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("LastCheckInStaffMember") != null %>'>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Last Check In Staff Member:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%# Eval("LastCheckInStaffMember.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastCheckInStaffMemberId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
                                    <tr>
                                        <td></td>
                                        <td>
                                            <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="Item2s" />
                                            <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                            <asp:CustomValidator ID="Item2CustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Item2s" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </FormTemplate>
                    </EditFormSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Receiving2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Caption" DataField="Caption" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Item" DataField="Item.ShortId" SortExpression="Item.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Location" DataField="Location.Name" SortExpression="Location.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order Item" DataField="OrderItem.Number" SortExpression="OrderItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Title" DataField="Title.Title" SortExpression="Title.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title.Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("TitleId")}" %>' Enabled='<%# Session["Title2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Display On Holding" DataField="DisplayOnHolding" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Receiving Status" DataField="ReceivingStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Supplement" DataField="Supplement" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Receipt Time" DataField="ReceiptTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Receive Time" DataField="ReceiveTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Holding2Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BoundWithPart2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BoundWithPart2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Fee2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Fee2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
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
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
