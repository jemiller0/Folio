<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.BatchVoucherExport2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="BatchVoucherExport2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="BatchVoucherExport2HyperLink" runat="server" Text="Batch Voucher Export" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="BatchVoucherExport2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="BatchVoucherExport2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewBatchVoucherExport2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLabel" runat="server" Text="Status:" AssociatedControlID="StatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLiteral" runat="server" Text='<%#: Eval("Status") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Message") != null %>'>
                                <td>
                                    <asp:Label ID="MessageLabel" runat="server" Text="Message:" AssociatedControlID="MessageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MessageLiteral" runat="server" Text='<%#: Eval("Message") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BatchGroup") != null %>'>
                                <td>
                                    <asp:Label ID="BatchGroupLabel" runat="server" Text="Batch Group:" AssociatedControlID="BatchGroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Start") != null %>'>
                                <td>
                                    <asp:Label ID="StartLabel" runat="server" Text="Start:" AssociatedControlID="StartLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StartLiteral" runat="server" Text='<%# Eval("Start", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("End") != null %>'>
                                <td>
                                    <asp:Label ID="EndLabel" runat="server" Text="End:" AssociatedControlID="EndLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EndLiteral" runat="server" Text='<%# Eval("End", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BatchVoucher") != null %>'>
                                <td>
                                    <asp:Label ID="BatchVoucherLabel" runat="server" Text="Batch Voucher:" AssociatedControlID="BatchVoucherHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BatchVoucherHyperLink" runat="server" Text='<%# Eval("BatchVoucher.Id") %>' NavigateUrl='<%# $"~/BatchVoucher2s/Edit.aspx?Id={Eval("BatchVoucherId")}" %>' Enabled='<%# Session["BatchVoucher2sPermission"] != null %>' />
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
