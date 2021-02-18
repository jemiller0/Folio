<%@ Page Title="Label" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Labels.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="FindPanel" runat="server" DefaultButton="FindRadButton">
        <%--<div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="Error" ValidationGroup="Find" />
        </div>--%>
        <asp:Label ID="BarcodeLabel" runat="server" Text="Barcode:" AssociatedControlID="BarcodeRadTextBox" />
        <telerik:RadTextBox ID="BarcodeRadTextBox" runat="server" ValidationGroup="Find" AccessKey="B" />
        <asp:RequiredFieldValidator ID="BarcodeRadTextBoxRequiredFieldValidator" runat="server" CssClass="Error" ErrorMessage="The Barcode field is required." ControlToValidate="BarcodeRadTextBox" Display="Dynamic" ValidationGroup="Find" />
        <telerik:RadButton ID="FindRadButton" runat="server" Text="Find" OnClick="FindRadButton_Click" ValidationGroup="Find" AccessKey="F" />
        <asp:CustomValidator ID="FindCustomValidator" runat="server" ErrorMessage="The item does not exist." CssClass="Error" Display="Dynamic" ValidationGroup="Find" />
    </asp:Panel>
    <asp:Panel ID="LabelPanel" runat="server">
        <asp:FormView ID="LabelFormView" ItemType="FolioLibrary.Label" runat="server" DataKeyNames="Id" DefaultMode="Edit" RenderOuterTable="false" SelectMethod="LabelFormView_GetItem" UpdateMethod="LabelFormView_UpdateItem">
            <EditItemTemplate>
                <asp:Panel ID="LabelPanel" runat="server" DefaultButton="ViewRadButton"> 
                    <fieldset>
                        <legend>Label</legend>
                        <div>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Label" />
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="OrientationLabel" runat="server" Text="Orientation:" AssociatedControlID="OrientationRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="OrientationRadComboBox" runat="server" SelectMethod="OrientationRadComboBox_GetItems" SelectedValue="<%# BindItem.Orientation %>" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="OrientationRadComboBox_SelectedIndexChanged" ValidationGroup="Label" AccessKey="O" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontFamilyLabel" runat="server" Text="Font Family:" AssociatedControlID="FontFamilyRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="FontFamilyRadComboBox" runat="server" SelectMethod="FontFamilyRadComboBox_GetItems" SelectedValue="<%# BindItem.Font.Family %>" MarkFirstMatch="true" ValidationGroup="Label" AccessKey="A" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontSizeLabel" runat="server" Text="Font Size:" AssociatedControlID="FontSizeRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="FontSizeRadComboBox" runat="server" SelectMethod="FontSizeRadComboBox_GetItems" Text="<%# BindItem.Font.Size %>" MarkFirstMatch="true" ValidationGroup="Label" AllowCustomText="true" AccessKey="S" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontWeightLabel1" runat="server" Text="Font Weight:" AssociatedControlID="FontWeightRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="FontWeightRadComboBox" runat="server" SelectMethod="FontWeightRadComboBox_GetItems" SelectedValue="<%# BindItem.Font.Weight %>" MarkFirstMatch="true" ValidationGroup="Label" AccessKey="W" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="TextLabel" runat="server" Text="Text:" AssociatedControlID="TextRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="TextRadTextBox" runat="server" TextMode="MultiLine" Rows="10" Columns="80" Width="500px" Text="<%# BindItem.Text %>" ValidationGroup="Label" AccessKey="T" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <telerik:RadButton ID="ViewRadButton" runat="server" Text="Print" CommandName="Update" AccessKey="P" />
                                </td>
                            </tr>
                            <%--<tr>
                                <td></td>
                                <td>
                                    <telerik:RadButton ID="PrintRadButton" runat="server" Text="Print" CommandName="Update1" AccessKey="P" OnClientClicking="alert('OnClientClicking() called!')" OnClientClicked="alert('OnClientClicked() called!')">
                                    </telerik:RadButton>
                                </td>
                            </tr>--%>
                        </table>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="ItemPanel" runat="server" Visible="false">
                    <fieldset>
                        <legend>Item</legend>
                        <table>
                            <%--<tr runat="server" visible="<%# Item.Item != null && Item.Item.CallNumberType != null %>">
                                <td>
                                    <asp:Label ID="CallNumberTypeLabel" runat="server" Text="Call Number Type:" AssociatedControlID="CallNumberTypeLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberTypeLiteral" runat="server" Text="<%#: Item.Item.CallNumberType %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.CallNumberPrefix != null %>">
                                <td>
                                    <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberPrefixLiteral" runat="server" Text="<%#: Item.Item.CallNumberPrefix %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.CallNumber != null %>">
                                <td>
                                    <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberLiteral" runat="server" Text="<%#: Item.Item.CallNumber %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.Enumeration != null %>">
                                <td>
                                    <asp:Label ID="EnumerationLabel" runat="server" Text="Enumeration:" AssociatedControlID="EnumerationLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="EnumerationLiteral" runat="server" Text="<%#: Item.Item.Enumeration %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.Chronology != null %>">
                                <td>
                                    <asp:Label ID="ChronologyLabel" runat="server" Text="Chronology:" AssociatedControlID="ChronologyLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="ChronologyLiteral" runat="server" Text="<%#: Item.Item.Chronology %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.CopyNumber != null %>">
                                <td>
                                    <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="CopyNumberLiteral" runat="server" Text="<%#: Item.Item.CopyNumber %>"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" visible="<%# Item.Item.LocationCode != null %>">
                                <td>
                                    <asp:Label ID="LocationCodeLabel" runat="server" Text="Location Code:" AssociatedControlID="LocationCodeLiteral"/>
                                </td>
                                <td>
                                    <asp:Literal ID="LocationCodeLiteral" runat="server" Text="<%#: Item.Item.LocationCode %>"></asp:Literal>
                                </td>
                            </tr>
                        </table>--%>
                    </fieldset>
                </asp:Panel>
            </EditItemTemplate>
        </asp:FormView>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="TypeRadComboBox">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TextRadTextBox" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="ViewRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LabelPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
