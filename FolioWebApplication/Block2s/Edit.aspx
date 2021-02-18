<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Block2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Block2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Block2HyperLink" runat="server" Text="Block" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Block2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Block2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewBlock2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Type") != null %>'>
                                <td>
                                    <asp:Label ID="TypeLabel" runat="server" Text="Type:" AssociatedControlID="TypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TypeLiteral" runat="server" Text='<%#: Eval("Type") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Desc") != null %>'>
                                <td>
                                    <asp:Label ID="DescLabel" runat="server" Text="Desc:" AssociatedControlID="DescLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescLiteral" runat="server" Text='<%#: Eval("Desc") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StaffInformation") != null %>'>
                                <td>
                                    <asp:Label ID="StaffInformationLabel" runat="server" Text="Staff Information:" AssociatedControlID="StaffInformationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StaffInformationLiteral" runat="server" Text='<%#: Eval("StaffInformation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PatronMessage") != null %>'>
                                <td>
                                    <asp:Label ID="PatronMessageLabel" runat="server" Text="Patron Message:" AssociatedControlID="PatronMessageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PatronMessageLiteral" runat="server" Text='<%#: Eval("PatronMessage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpirationDate") != null %>'>
                                <td>
                                    <asp:Label ID="ExpirationDateLabel" runat="server" Text="Expiration Date:" AssociatedControlID="ExpirationDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpirationDateLiteral" runat="server" Text='<%# Eval("ExpirationDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Borrowing") != null %>'>
                                <td>
                                    <asp:Label ID="BorrowingLabel" runat="server" Text="Borrowing:" AssociatedControlID="BorrowingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BorrowingLiteral" runat="server" Text='<%#: Eval("Borrowing") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Renewals") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalsLabel" runat="server" Text="Renewals:" AssociatedControlID="RenewalsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalsLiteral" runat="server" Text='<%#: Eval("Renewals") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Requests") != null %>'>
                                <td>
                                    <asp:Label ID="RequestsLabel" runat="server" Text="Requests:" AssociatedControlID="RequestsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequestsLiteral" runat="server" Text='<%#: Eval("Requests") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("User") != null %>'>
                                <td>
                                    <asp:Label ID="UserLabel" runat="server" Text="User:" AssociatedControlID="UserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("User.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
