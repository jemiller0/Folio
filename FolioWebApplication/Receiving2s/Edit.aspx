<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Receiving2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Receiving2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Receiving2HyperLink" runat="server" Text="Receiving" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Receiving2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Receiving2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewReceiving2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Caption") != null %>'>
                                <td>
                                    <asp:Label ID="CaptionLabel" runat="server" Text="Caption:" AssociatedControlID="CaptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CaptionLiteral" runat="server" Text='<%#: Eval("Caption") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Comment") != null %>'>
                                <td>
                                    <asp:Label ID="CommentLabel" runat="server" Text="Comment:" AssociatedControlID="CommentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CommentLiteral" runat="server" Text='<%#: Eval("Comment") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Format") != null %>'>
                                <td>
                                    <asp:Label ID="FormatLabel" runat="server" Text="Format:" AssociatedControlID="FormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FormatLiteral" runat="server" Text='<%#: Eval("Format") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Item") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLabel" runat="server" Text="Item:" AssociatedControlID="ItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("Item.ShortId") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Location") != null %>'>
                                <td>
                                    <asp:Label ID="LocationLabel" runat="server" Text="Location:" AssociatedControlID="LocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderItem") != null %>'>
                                <td>
                                    <asp:Label ID="OrderItemLabel" runat="server" Text="Order Item:" AssociatedControlID="OrderItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItem.Number") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Title") != null %>'>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title.Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("TitleId")}" %>' Enabled='<%# Session["Title2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceivingStatus") != null %>'>
                                <td>
                                    <asp:Label ID="ReceivingStatusLabel" runat="server" Text="Receiving Status:" AssociatedControlID="ReceivingStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceivingStatusLiteral" runat="server" Text='<%#: Eval("ReceivingStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Supplement") != null %>'>
                                <td>
                                    <asp:Label ID="SupplementLabel" runat="server" Text="Supplement:" AssociatedControlID="SupplementLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SupplementLiteral" runat="server" Text='<%#: Eval("Supplement") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceiptTime") != null %>'>
                                <td>
                                    <asp:Label ID="ReceiptTimeLabel" runat="server" Text="Receipt Time:" AssociatedControlID="ReceiptTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceiptTimeLiteral" runat="server" Text='<%# Eval("ReceiptTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceiveTime") != null %>'>
                                <td>
                                    <asp:Label ID="ReceiveTimeLabel" runat="server" Text="Receive Time:" AssociatedControlID="ReceiveTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceiveTimeLiteral" runat="server" Text='<%# Eval("ReceiveTime", "{0:g}") %>' />
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
