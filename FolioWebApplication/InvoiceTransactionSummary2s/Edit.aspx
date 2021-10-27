<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.InvoiceTransactionSummary2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="InvoiceTransactionSummary2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceTransactionSummary2HyperLink" runat="server" Text="Invoice Transaction Summary" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="InvoiceTransactionSummary2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="InvoiceTransactionSummary2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewInvoiceTransactionSummary2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Invoice2") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Invoice:" NavigateUrl="~/Invoice2s/Default.aspx" Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="Invoice2HyperLink" runat="server" Text='<%#: Eval("Invoice2.Number") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("Id")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NumPendingPayments") != null %>'>
                                <td>
                                    <asp:Label ID="NumPendingPaymentsLabel" runat="server" Text="Num Pending Payments:" AssociatedControlID="NumPendingPaymentsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NumPendingPaymentsLiteral" runat="server" Text='<%#: Eval("NumPendingPayments") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NumPaymentsCredits") != null %>'>
                                <td>
                                    <asp:Label ID="NumPaymentsCreditsLabel" runat="server" Text="Num Payments Credits:" AssociatedControlID="NumPaymentsCreditsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NumPaymentsCreditsLiteral" runat="server" Text='<%#: Eval("NumPaymentsCredits") %>' />
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
