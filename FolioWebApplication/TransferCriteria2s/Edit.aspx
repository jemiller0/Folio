<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.TransferCriteria2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="TransferCriteria2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="TransferCriteria2HyperLink" runat="server" Text="Transfer Criteria" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="TransferCriteria2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="TransferCriteria2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewTransferCriteria2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Criteria") != null %>'>
                                <td>
                                    <asp:Label ID="CriteriaLabel" runat="server" Text="Criteria:" AssociatedControlID="CriteriaLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CriteriaLiteral" runat="server" Text='<%#: Eval("Criteria") %>' />
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
                            <tr runat="server" visible='<%# Eval("Value") != null %>'>
                                <td>
                                    <asp:Label ID="ValueLabel" runat="server" Text="Value:" AssociatedControlID="ValueLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ValueLiteral" runat="server" Text='<%#: Eval("Value") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Interval") != null %>'>
                                <td>
                                    <asp:Label ID="IntervalLabel" runat="server" Text="Interval:" AssociatedControlID="IntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="IntervalLiteral" runat="server" Text='<%#: Eval("Interval") %>' />
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
