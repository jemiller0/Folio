<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Record2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Record2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Record2HyperLink" runat="server" Text="Record" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Record2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Record2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewRecord2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Snapshot") != null %>'>
                                <td>
                                    <asp:Label ID="SnapshotLabel" runat="server" Text="Snapshot:" AssociatedControlID="SnapshotHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SnapshotHyperLink" runat="server" Text='<%# Eval("Snapshot.Id") %>' NavigateUrl='<%# $"~/Snapshot2s/Edit.aspx?Id={Eval("SnapshotId")}" %>' Enabled='<%# Session["Snapshot2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MatchedId") != null %>'>
                                <td>
                                    <asp:Label ID="MatchedIdLabel" runat="server" Text="Matched Id:" AssociatedControlID="MatchedIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MatchedIdLiteral" runat="server" Text='<%#: Eval("MatchedId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Generation") != null %>'>
                                <td>
                                    <asp:Label ID="GenerationLabel" runat="server" Text="Generation:" AssociatedControlID="GenerationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="GenerationLiteral" runat="server" Text='<%#: Eval("Generation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecordType") != null %>'>
                                <td>
                                    <asp:Label ID="RecordTypeLabel" runat="server" Text="Record Type:" AssociatedControlID="RecordTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RecordTypeLiteral" runat="server" Text='<%#: Eval("RecordType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Instance") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceLabel" runat="server" Text="Instance:" AssociatedControlID="InstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("State") != null %>'>
                                <td>
                                    <asp:Label ID="StateLabel" runat="server" Text="State:" AssociatedControlID="StateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StateLiteral" runat="server" Text='<%#: Eval("State") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LeaderRecordStatus") != null %>'>
                                <td>
                                    <asp:Label ID="LeaderRecordStatusLabel" runat="server" Text="Leader Record Status:" AssociatedControlID="LeaderRecordStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LeaderRecordStatusLiteral" runat="server" Text='<%#: Eval("LeaderRecordStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Order") != null %>'>
                                <td>
                                    <asp:Label ID="OrderLabel" runat="server" Text="Order:" AssociatedControlID="OrderLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderLiteral" runat="server" Text='<%#: Eval("Order") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SuppressDiscovery") != null %>'>
                                <td>
                                    <asp:Label ID="SuppressDiscoveryLabel" runat="server" Text="Suppress Discovery:" AssociatedControlID="SuppressDiscoveryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SuppressDiscoveryLiteral" runat="server" Text='<%#: Eval("SuppressDiscovery") %>' />
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
                            <tr runat="server" visible='<%# Eval("CreationTime") != null %>'>
                                <td>
                                    <asp:Label ID="CreationTimeLabel" runat="server" Text="Creation Time:" AssociatedControlID="CreationTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreationTimeLiteral" runat="server" Text='<%# Eval("CreationTime", "{0:g}") %>' />
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
                            <tr runat="server" visible='<%# Eval("LastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteTimeLabel" runat="server" Text="Last Write Time:" AssociatedControlID="LastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastWriteTimeLiteral" runat="server" Text='<%# Eval("LastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InstanceHrid") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceHridLabel" runat="server" Text="Instance Hrid:" AssociatedControlID="InstanceHridLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InstanceHridLiteral" runat="server" Text='<%#: Eval("InstanceHrid") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ErrorRecord2") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Error Record:" NavigateUrl="~/ErrorRecord2s/Default.aspx" Enabled='<%# Session["ErrorRecord2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ErrorRecord2HyperLink" runat="server" Text='<%# Eval("ErrorRecord2.Id") %>' NavigateUrl='<%# $"~/ErrorRecord2s/Edit.aspx?Id={Eval("Id")}" %>' Enabled='<%# Session["ErrorRecord2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MarcRecord2") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Marc Record:" NavigateUrl="~/MarcRecord2s/Default.aspx" Enabled='<%# Session["MarcRecord2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="MarcRecord2HyperLink" runat="server" Text='<%# Eval("MarcRecord2.Id") %>' NavigateUrl='<%# $"~/MarcRecord2s/Edit.aspx?Id={Eval("Id")}" %>' Enabled='<%# Session["MarcRecord2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RawRecord2") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Raw Record:" NavigateUrl="~/RawRecord2s/Default.aspx" Enabled='<%# Session["RawRecord2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RawRecord2HyperLink" runat="server" Text='<%# Eval("RawRecord2.Id") %>' NavigateUrl='<%# $"~/RawRecord2s/Edit.aspx?Id={Eval("Id")}" %>' Enabled='<%# Session["RawRecord2sPermission"] != null %>' />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
</asp:Content>
