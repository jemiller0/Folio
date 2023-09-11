<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.ReferenceData2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="ReferenceData2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="ReferenceData2HyperLink" runat="server" Text="Reference Data" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="ReferenceData2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="ReferenceData2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewReferenceData2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Label") != null %>'>
                                <td>
                                    <asp:Label ID="LabelLabel" runat="server" Text="Label:" AssociatedControlID="LabelLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LabelLiteral" runat="server" Text='<%#: Eval("Label") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Value") != null %>'>
                                <td>
                                    <asp:Label ID="ValueLabel" runat="server" Text="Value:" AssociatedControlID="ValueLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ValueLiteral" runat="server" Text='<%#: Eval("Value") %>' />
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
