<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Comment2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Comment2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Comment2HyperLink" runat="server" Text="Comment" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Comment2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Comment2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewComment2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Paid") != null %>'>
                                <td>
                                    <asp:Label ID="PaidLabel" runat="server" Text="Paid:" AssociatedControlID="PaidLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaidLiteral" runat="server" Text='<%#: Eval("Paid") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Waived") != null %>'>
                                <td>
                                    <asp:Label ID="WaivedLabel" runat="server" Text="Waived:" AssociatedControlID="WaivedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="WaivedLiteral" runat="server" Text='<%#: Eval("Waived") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Refunded") != null %>'>
                                <td>
                                    <asp:Label ID="RefundedLabel" runat="server" Text="Refunded:" AssociatedControlID="RefundedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RefundedLiteral" runat="server" Text='<%#: Eval("Refunded") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TransferredManually") != null %>'>
                                <td>
                                    <asp:Label ID="TransferredManuallyLabel" runat="server" Text="Transferred Manually:" AssociatedControlID="TransferredManuallyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TransferredManuallyLiteral" runat="server" Text='<%#: Eval("TransferredManually") %>' />
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
