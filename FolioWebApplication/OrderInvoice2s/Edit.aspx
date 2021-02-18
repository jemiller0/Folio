<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.OrderInvoice2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="OrderInvoice2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderInvoice2HyperLink" runat="server" Text="Order Invoice" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="OrderInvoice2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="OrderInvoice2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewOrderInvoice2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Order") != null %>'>
                                <td>
                                    <asp:Label ID="OrderLabel" runat="server" Text="Order:" AssociatedControlID="OrderHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Invoice") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceLabel" runat="server" Text="Invoice:" AssociatedControlID="InvoiceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("Invoice.Number") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
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
