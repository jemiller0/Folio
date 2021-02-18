<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.UserRequestPreference2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="UserRequestPreference2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="UserRequestPreference2HyperLink" runat="server" Text="User Request Preference" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="UserRequestPreference2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="UserRequestPreference2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewUserRequestPreference2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
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
                            <tr runat="server" visible='<%# Eval("HoldShelf") != null %>'>
                                <td>
                                    <asp:Label ID="HoldShelfLabel" runat="server" Text="Hold Shelf:" AssociatedControlID="HoldShelfLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HoldShelfLiteral" runat="server" Text='<%#: Eval("HoldShelf") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Delivery") != null %>'>
                                <td>
                                    <asp:Label ID="DeliveryLabel" runat="server" Text="Delivery:" AssociatedControlID="DeliveryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DeliveryLiteral" runat="server" Text='<%#: Eval("Delivery") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DefaultServicePoint") != null %>'>
                                <td>
                                    <asp:Label ID="DefaultServicePointLabel" runat="server" Text="Default Service Point:" AssociatedControlID="DefaultServicePointHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="DefaultServicePointHyperLink" runat="server" Text='<%#: Eval("DefaultServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("DefaultServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DefaultDeliveryAddressType") != null %>'>
                                <td>
                                    <asp:Label ID="DefaultDeliveryAddressTypeLabel" runat="server" Text="Default Delivery Address Type:" AssociatedControlID="DefaultDeliveryAddressTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="DefaultDeliveryAddressTypeHyperLink" runat="server" Text='<%#: Eval("DefaultDeliveryAddressType.Name") %>' NavigateUrl='<%# $"~/AddressType2s/Edit.aspx?Id={Eval("DefaultDeliveryAddressTypeId")}" %>' Enabled='<%# Session["AddressType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Fulfillment") != null %>'>
                                <td>
                                    <asp:Label ID="FulfillmentLabel" runat="server" Text="Fulfillment:" AssociatedControlID="FulfillmentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FulfillmentLiteral" runat="server" Text='<%#: Eval("Fulfillment") %>' />
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
