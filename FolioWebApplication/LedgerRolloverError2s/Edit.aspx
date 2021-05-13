<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LedgerRolloverError2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LedgerRolloverError2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverError2HyperLink" runat="server" Text="Ledger Rollover Error" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LedgerRolloverError2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="LedgerRolloverError2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLedgerRolloverError2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LedgerRollover") != null %>'>
                                <td>
                                    <asp:Label ID="LedgerRolloverLabel" runat="server" Text="Ledger Rollover:" AssociatedControlID="LedgerRolloverHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LedgerRolloverHyperLink" runat="server" Text='<%# Eval("LedgerRollover.Id") %>' NavigateUrl='<%# $"~/LedgerRollover2s/Edit.aspx?Id={Eval("LedgerRolloverId")}" %>' Enabled='<%# Session["LedgerRollover2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ErrorType") != null %>'>
                                <td>
                                    <asp:Label ID="ErrorTypeLabel" runat="server" Text="Error Type:" AssociatedControlID="ErrorTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ErrorTypeLiteral" runat="server" Text='<%#: Eval("ErrorType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FailedAction") != null %>'>
                                <td>
                                    <asp:Label ID="FailedActionLabel" runat="server" Text="Failed Action:" AssociatedControlID="FailedActionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FailedActionLiteral" runat="server" Text='<%#: Eval("FailedAction") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ErrorMessage") != null %>'>
                                <td>
                                    <asp:Label ID="ErrorMessageLabel" runat="server" Text="Error Message:" AssociatedControlID="ErrorMessageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ErrorMessageLiteral" runat="server" Text='<%#: Eval("ErrorMessage") %>' />
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
