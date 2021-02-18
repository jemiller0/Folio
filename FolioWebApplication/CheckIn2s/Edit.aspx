<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.CheckIn2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="CheckIn2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="CheckIn2HyperLink" runat="server" Text="Check In" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="CheckIn2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="CheckIn2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewCheckIn2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OccurredDateTime") != null %>'>
                                <td>
                                    <asp:Label ID="OccurredDateTimeLabel" runat="server" Text="Occurred Date Time:" AssociatedControlID="OccurredDateTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OccurredDateTimeLiteral" runat="server" Text='<%# Eval("OccurredDateTime", "{0:g}") %>' />
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
                            <tr runat="server" visible='<%# Eval("ItemStatusPriorToCheckIn") != null %>'>
                                <td>
                                    <asp:Label ID="ItemStatusPriorToCheckInLabel" runat="server" Text="Item Status Prior To Check In:" AssociatedControlID="ItemStatusPriorToCheckInLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemStatusPriorToCheckInLiteral" runat="server" Text='<%#: Eval("ItemStatusPriorToCheckIn") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RequestQueueSize") != null %>'>
                                <td>
                                    <asp:Label ID="RequestQueueSizeLabel" runat="server" Text="Request Queue Size:" AssociatedControlID="RequestQueueSizeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequestQueueSizeLiteral" runat="server" Text='<%#: Eval("RequestQueueSize") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemLocation") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLocationLabel" runat="server" Text="Item Location:" AssociatedControlID="ItemLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemLocationHyperLink" runat="server" Text='<%#: Eval("ItemLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ServicePoint") != null %>'>
                                <td>
                                    <asp:Label ID="ServicePointLabel" runat="server" Text="Service Point:" AssociatedControlID="ServicePointHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ServicePointHyperLink" runat="server" Text='<%#: Eval("ServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("ServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PerformedByUser") != null %>'>
                                <td>
                                    <asp:Label ID="PerformedByUserLabel" runat="server" Text="Performed By User:" AssociatedControlID="PerformedByUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PerformedByUserHyperLink" runat="server" Text='<%#: Eval("PerformedByUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("PerformedByUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
