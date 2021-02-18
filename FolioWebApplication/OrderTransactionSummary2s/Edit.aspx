<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.OrderTransactionSummary2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="OrderTransactionSummary2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderTransactionSummary2HyperLink" runat="server" Text="Order Transaction Summary" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="OrderTransactionSummary2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="OrderTransactionSummary2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewOrderTransactionSummary2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Order2") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Order:" NavigateUrl="~/Order2s/Default.aspx" Enabled='<%# Session["Order2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="Order2HyperLink" runat="server" Text='<%#: Eval("Order2.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("Id")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NumTransactions") != null %>'>
                                <td>
                                    <asp:Label ID="NumTransactionsLabel" runat="server" Text="Num Transactions:" AssociatedControlID="NumTransactionsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NumTransactionsLiteral" runat="server" Text='<%#: Eval("NumTransactions") %>' />
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
