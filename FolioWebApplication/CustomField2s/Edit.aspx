<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.CustomField2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="CustomField2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="CustomField2HyperLink" runat="server" Text="Custom Field" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="CustomField2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="CustomField2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewCustomField2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("RefId") != null %>'>
                                <td>
                                    <asp:Label ID="RefIdLabel" runat="server" Text="Ref Id:" AssociatedControlID="RefIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RefIdLiteral" runat="server" Text='<%#: Eval("RefId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Type") != null %>'>
                                <td>
                                    <asp:Label ID="TypeLabel" runat="server" Text="Type:" AssociatedControlID="TypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TypeLiteral" runat="server" Text='<%#: Eval("Type") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EntityType") != null %>'>
                                <td>
                                    <asp:Label ID="EntityTypeLabel" runat="server" Text="Entity Type:" AssociatedControlID="EntityTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EntityTypeLiteral" runat="server" Text='<%#: Eval("EntityType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Visible") != null %>'>
                                <td>
                                    <asp:Label ID="VisibleLabel" runat="server" Text="Visible:" AssociatedControlID="VisibleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VisibleLiteral" runat="server" Text='<%#: Eval("Visible") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Required") != null %>'>
                                <td>
                                    <asp:Label ID="RequiredLabel" runat="server" Text="Required:" AssociatedControlID="RequiredLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequiredLiteral" runat="server" Text='<%#: Eval("Required") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IsRepeatable") != null %>'>
                                <td>
                                    <asp:Label ID="IsRepeatableLabel" runat="server" Text="Is Repeatable:" AssociatedControlID="IsRepeatableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="IsRepeatableLiteral" runat="server" Text='<%#: Eval("IsRepeatable") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Order") != null %>'>
                                <td>
                                    <asp:Label ID="OrderLabel" runat="server" Text="Order:" AssociatedControlID="OrderLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderLiteral" runat="server" Text='<%#: Eval("Order") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HelpText") != null %>'>
                                <td>
                                    <asp:Label ID="HelpTextLabel" runat="server" Text="Help Text:" AssociatedControlID="HelpTextLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HelpTextLiteral" runat="server" Text='<%#: Eval("HelpText") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CheckboxFieldDefault") != null %>'>
                                <td>
                                    <asp:Label ID="CheckboxFieldDefaultLabel" runat="server" Text="Checkbox Field Default:" AssociatedControlID="CheckboxFieldDefaultLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CheckboxFieldDefaultLiteral" runat="server" Text='<%#: Eval("CheckboxFieldDefault") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SelectFieldMultiSelect") != null %>'>
                                <td>
                                    <asp:Label ID="SelectFieldMultiSelectLabel" runat="server" Text="Select Field Multi Select:" AssociatedControlID="SelectFieldMultiSelectLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SelectFieldMultiSelectLiteral" runat="server" Text='<%#: Eval("SelectFieldMultiSelect") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SelectFieldOptionsSortingOrder") != null %>'>
                                <td>
                                    <asp:Label ID="SelectFieldOptionsSortingOrderLabel" runat="server" Text="Select Field Options Sorting Order:" AssociatedControlID="SelectFieldOptionsSortingOrderLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SelectFieldOptionsSortingOrderLiteral" runat="server" Text='<%#: Eval("SelectFieldOptionsSortingOrder") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TextFieldFieldFormat") != null %>'>
                                <td>
                                    <asp:Label ID="TextFieldFieldFormatLabel" runat="server" Text="Text Field Field Format:" AssociatedControlID="TextFieldFieldFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TextFieldFieldFormatLiteral" runat="server" Text='<%#: Eval("TextFieldFieldFormat") %>' />
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
</asp:Content>
