<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Relationships.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="RelationshipPanel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="RelationshipHyperLink" runat="server" Text="Relationship" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="RelationshipFormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="RelationshipFormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewRelationshipPanel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SuperInstance") != null %>'>
                                <td>
                                    <asp:Label ID="SuperInstanceLabel" runat="server" Text="Super Instance:" AssociatedControlID="SuperInstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SuperInstanceHyperLink" runat="server" Text='<%#: Eval("SuperInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SuperInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubInstance") != null %>'>
                                <td>
                                    <asp:Label ID="SubInstanceLabel" runat="server" Text="Sub Instance:" AssociatedControlID="SubInstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SubInstanceHyperLink" runat="server" Text='<%#: Eval("SubInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SubInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InstanceRelationshipType") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceRelationshipTypeLabel" runat="server" Text="Instance Relationship Type:" AssociatedControlID="InstanceRelationshipTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceRelationshipTypeHyperLink" runat="server" Text='<%#: Eval("InstanceRelationshipType.Name") %>' NavigateUrl='<%# $"~/RelationshipTypes/Edit.aspx?Id={Eval("InstanceRelationshipTypeId")}" %>' Enabled='<%# Session["RelationshipTypesPermission"] != null %>' />
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
