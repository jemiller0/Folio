<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LedgerRolloverProgress2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LedgerRolloverProgress2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LedgerRolloverProgress2HyperLink" runat="server" Text="Ledger Rollover Progress" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LedgerRolloverProgress2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="LedgerRolloverProgress2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewLedgerRolloverProgress2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("OverallRolloverStatus") != null %>'>
                                <td>
                                    <asp:Label ID="OverallRolloverStatusLabel" runat="server" Text="Overall Rollover Status:" AssociatedControlID="OverallRolloverStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OverallRolloverStatusLiteral" runat="server" Text='<%#: Eval("OverallRolloverStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BudgetsClosingRolloverStatus") != null %>'>
                                <td>
                                    <asp:Label ID="BudgetsClosingRolloverStatusLabel" runat="server" Text="Budgets Closing Rollover Status:" AssociatedControlID="BudgetsClosingRolloverStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BudgetsClosingRolloverStatusLiteral" runat="server" Text='<%#: Eval("BudgetsClosingRolloverStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FinancialRolloverStatus") != null %>'>
                                <td>
                                    <asp:Label ID="FinancialRolloverStatusLabel" runat="server" Text="Financial Rollover Status:" AssociatedControlID="FinancialRolloverStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FinancialRolloverStatusLiteral" runat="server" Text='<%#: Eval("FinancialRolloverStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrdersRolloverStatus") != null %>'>
                                <td>
                                    <asp:Label ID="OrdersRolloverStatusLabel" runat="server" Text="Orders Rollover Status:" AssociatedControlID="OrdersRolloverStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrdersRolloverStatusLiteral" runat="server" Text='<%#: Eval("OrdersRolloverStatus") %>' />
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
