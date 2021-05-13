<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.ManualBlockTemplate2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="ManualBlockTemplate2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="ManualBlockTemplate2HyperLink" runat="server" Text="Manual Block Template" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="ManualBlockTemplate2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="ManualBlockTemplate2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewManualBlockTemplate2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Name") != null %>'>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NameLiteral" runat="server" Text='<%#: Eval("Name") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Code") != null %>'>
                                <td>
                                    <asp:Label ID="CodeLabel" runat="server" Text="Code:" AssociatedControlID="CodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CodeLiteral" runat="server" Text='<%#: Eval("Code") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BlockTemplateDescription") != null %>'>
                                <td>
                                    <asp:Label ID="BlockTemplateDescriptionLabel" runat="server" Text="Block Template Description:" AssociatedControlID="BlockTemplateDescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BlockTemplateDescriptionLiteral" runat="server" Text='<%#: Eval("BlockTemplateDescription") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BlockTemplatePatronMessage") != null %>'>
                                <td>
                                    <asp:Label ID="BlockTemplatePatronMessageLabel" runat="server" Text="Block Template Patron Message:" AssociatedControlID="BlockTemplatePatronMessageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BlockTemplatePatronMessageLiteral" runat="server" Text='<%#: Eval("BlockTemplatePatronMessage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BlockTemplateBorrowing") != null %>'>
                                <td>
                                    <asp:Label ID="BlockTemplateBorrowingLabel" runat="server" Text="Block Template Borrowing:" AssociatedControlID="BlockTemplateBorrowingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BlockTemplateBorrowingLiteral" runat="server" Text='<%#: Eval("BlockTemplateBorrowing") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BlockTemplateRenewals") != null %>'>
                                <td>
                                    <asp:Label ID="BlockTemplateRenewalsLabel" runat="server" Text="Block Template Renewals:" AssociatedControlID="BlockTemplateRenewalsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BlockTemplateRenewalsLiteral" runat="server" Text='<%#: Eval("BlockTemplateRenewals") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BlockTemplateRequests") != null %>'>
                                <td>
                                    <asp:Label ID="BlockTemplateRequestsLabel" runat="server" Text="Block Template Requests:" AssociatedControlID="BlockTemplateRequestsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BlockTemplateRequestsLiteral" runat="server" Text='<%#: Eval("BlockTemplateRequests") %>' />
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
