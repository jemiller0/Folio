<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.AgreementItem2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="AgreementItem2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="AgreementItem2HyperLink" runat="server" Text="Agreement Item" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="AgreementItem2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="AgreementItem2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewAgreementItem2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DateCreated") != null %>'>
                                <td>
                                    <asp:Label ID="DateCreatedLabel" runat="server" Text="Date Created:" AssociatedControlID="DateCreatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DateCreatedLiteral" runat="server" Text='<%# Eval("DateCreated", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastUpdated") != null %>'>
                                <td>
                                    <asp:Label ID="LastUpdatedLabel" runat="server" Text="Last Updated:" AssociatedControlID="LastUpdatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastUpdatedLiteral" runat="server" Text='<%# Eval("LastUpdated", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AgreementId") != null %>'>
                                <td>
                                    <asp:Label ID="AgreementIdLabel" runat="server" Text="Agreement Id:" AssociatedControlID="AgreementIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AgreementIdLiteral" runat="server" Text='<%#: Eval("AgreementId") %>' />
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
