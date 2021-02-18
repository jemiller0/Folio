<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.GroupFundFiscalYear2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="GroupFundFiscalYear2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="GroupFundFiscalYear2HyperLink" runat="server" Text="Group Fund Fiscal Year" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="GroupFundFiscalYear2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="GroupFundFiscalYear2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewGroupFundFiscalYear2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Budget") != null %>'>
                                <td>
                                    <asp:Label ID="BudgetLabel" runat="server" Text="Budget:" AssociatedControlID="BudgetHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BudgetHyperLink" runat="server" Text='<%#: Eval("Budget.Name") %>' NavigateUrl='<%# $"~/Budget2s/Edit.aspx?Id={Eval("BudgetId")}" %>' Enabled='<%# Session["Budget2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Group") != null %>'>
                                <td>
                                    <asp:Label ID="GroupLabel" runat="server" Text="Group:" AssociatedControlID="GroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="GroupHyperLink" runat="server" Text='<%#: Eval("Group.Name") %>' NavigateUrl='<%# $"~/FinanceGroup2s/Edit.aspx?Id={Eval("GroupId")}" %>' Enabled='<%# Session["FinanceGroup2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="FiscalYearLabel" runat="server" Text="Fiscal Year:" AssociatedControlID="FiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Fund") != null %>'>
                                <td>
                                    <asp:Label ID="FundLabel" runat="server" Text="Fund:" AssociatedControlID="FundHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FundHyperLink" runat="server" Text='<%#: Eval("Fund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
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
