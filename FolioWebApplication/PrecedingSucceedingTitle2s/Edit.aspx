<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.PrecedingSucceedingTitle2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PrecedingSucceedingTitle2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitle2HyperLink" runat="server" Text="Preceding Succeeding Title" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="PrecedingSucceedingTitle2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="PrecedingSucceedingTitle2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewPrecedingSucceedingTitle2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PrecedingInstance") != null %>'>
                                <td>
                                    <asp:Label ID="PrecedingInstanceLabel" runat="server" Text="Preceding Instance:" AssociatedControlID="PrecedingInstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PrecedingInstanceHyperLink" runat="server" Text='<%#: Eval("PrecedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("PrecedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SucceedingInstance") != null %>'>
                                <td>
                                    <asp:Label ID="SucceedingInstanceLabel" runat="server" Text="Succeeding Instance:" AssociatedControlID="SucceedingInstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SucceedingInstanceHyperLink" runat="server" Text='<%#: Eval("SucceedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SucceedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Title") != null %>'>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TitleLiteral" runat="server" Text='<%#: Eval("Title") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Hrid") != null %>'>
                                <td>
                                    <asp:Label ID="HridLabel" runat="server" Text="Hrid:" AssociatedControlID="HridLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HridLiteral" runat="server" Text='<%#: Eval("Hrid") %>' />
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
    <asp:Panel ID="PrecedingSucceedingTitleIdentifiersPanel" runat="server" Visible='<%# (string)Session["PrecedingSucceedingTitleIdentifiersPermission"] != null && PrecedingSucceedingTitle2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitleIdentifiersHyperLink" runat="server" Text="Preceding Succeeding Title Identifiers" NavigateUrl="~/PrecedingSucceedingTitleIdentifiers/Default.aspx" /></legend>
            <telerik:RadGrid ID="PrecedingSucceedingTitleIdentifiersRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PrecedingSucceedingTitleIdentifiersRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No preceding succeeding title identifiers found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Identifier Type" DataField="IdentifierType.Name" SortExpression="IdentifierType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdentifierTypeHyperLink" runat="server" Text='<%#: Eval("IdentifierType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("IdentifierTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="PrecedingSucceedingTitleIdentifiersRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrecedingSucceedingTitleIdentifiersPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
