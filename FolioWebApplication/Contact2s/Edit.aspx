<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Contact2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Contact2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Contact2HyperLink" runat="server" Text="Contact" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Contact2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Contact2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewContact2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Prefix") != null %>'>
                                <td>
                                    <asp:Label ID="PrefixLabel" runat="server" Text="Prefix:" AssociatedControlID="PrefixLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PrefixLiteral" runat="server" Text='<%#: Eval("Prefix") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FirstName") != null %>'>
                                <td>
                                    <asp:Label ID="FirstNameLabel" runat="server" Text="First Name:" AssociatedControlID="FirstNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FirstNameLiteral" runat="server" Text='<%#: Eval("FirstName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastName") != null %>'>
                                <td>
                                    <asp:Label ID="LastNameLabel" runat="server" Text="Last Name:" AssociatedControlID="LastNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastNameLiteral" runat="server" Text='<%#: Eval("LastName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Language") != null %>'>
                                <td>
                                    <asp:Label ID="LanguageLabel" runat="server" Text="Language:" AssociatedControlID="LanguageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LanguageLiteral" runat="server" Text='<%#: Eval("Language") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Notes") != null %>'>
                                <td>
                                    <asp:Label ID="NotesLabel" runat="server" Text="Notes:" AssociatedControlID="NotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NotesLiteral" runat="server" Text='<%#: Eval("Notes") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Inactive") != null %>'>
                                <td>
                                    <asp:Label ID="InactiveLabel" runat="server" Text="Inactive:" AssociatedControlID="InactiveLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InactiveLiteral" runat="server" Text='<%#: Eval("Inactive") %>' />
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
