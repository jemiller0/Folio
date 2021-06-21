<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.VoucherItem2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="VoucherItem2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="VoucherItem2HyperLink" runat="server" Text="Voucher Item" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="VoucherItem2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="VoucherItem2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewVoucherItem2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Amount") != null %>'>
                                <td>
                                    <asp:Label ID="AmountLabel" runat="server" Text="Amount:" AssociatedControlID="AmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AmountLiteral" runat="server" Text='<%# Eval("Amount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AccountNumber") != null %>'>
                                <td>
                                    <asp:Label ID="AccountNumberLabel" runat="server" Text="Account Number:" AssociatedControlID="AccountNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccountNumberLiteral" runat="server" Text='<%#: Eval("AccountNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubTransaction") != null %>'>
                                <td>
                                    <asp:Label ID="SubTransactionLabel" runat="server" Text="Sub Transaction:" AssociatedControlID="SubTransactionHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SubTransactionHyperLink" runat="server" Text='<%# Eval("SubTransaction.Amount", "{0:c}") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("SubTransactionId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Voucher") != null %>'>
                                <td>
                                    <asp:Label ID="VoucherLabel" runat="server" Text="Voucher:" AssociatedControlID="VoucherHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="VoucherHyperLink" runat="server" Text='<%#: Eval("Voucher.Number") %>' NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("VoucherId")}" %>' Enabled='<%# Session["Voucher2sPermission"] != null %>' />
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
