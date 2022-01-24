<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Interface2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Interface2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Interface2HyperLink" runat="server" Text="Interface" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Interface2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Interface2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewInterface2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Uri") != null %>'>
                                <td>
                                    <asp:Label ID="UriLabel" runat="server" Text="URI:" AssociatedControlID="UriHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UriHyperLink" runat="server" Text='<%#: Eval("Uri") %>' NavigateUrl='<%#: Eval("Uri") %>' Target="_blank" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Notes") != null %>'>
                                <td>
                                    <asp:Label ID="NotesLabel" runat="server" Text="Notes:" AssociatedControlID="NotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NotesLiteral" runat="server" Text='<%#: Eval("Notes") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Available") != null %>'>
                                <td>
                                    <asp:Label ID="AvailableLabel" runat="server" Text="Available:" AssociatedControlID="AvailableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AvailableLiteral" runat="server" Text='<%#: Eval("Available") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DeliveryMethod") != null %>'>
                                <td>
                                    <asp:Label ID="DeliveryMethodLabel" runat="server" Text="Delivery Method:" AssociatedControlID="DeliveryMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DeliveryMethodLiteral" runat="server" Text='<%#: Eval("DeliveryMethod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatisticsFormat") != null %>'>
                                <td>
                                    <asp:Label ID="StatisticsFormatLabel" runat="server" Text="Statistics Format:" AssociatedControlID="StatisticsFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatisticsFormatLiteral" runat="server" Text='<%#: Eval("StatisticsFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LocallyStored") != null %>'>
                                <td>
                                    <asp:Label ID="LocallyStoredLabel" runat="server" Text="Locally Stored:" AssociatedControlID="LocallyStoredHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LocallyStoredHyperLink" runat="server" Text='<%#: Eval("LocallyStored") %>' NavigateUrl='<%#: Eval("LocallyStored") %>' Target="_blank" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OnlineLocation") != null %>'>
                                <td>
                                    <asp:Label ID="OnlineLocationLabel" runat="server" Text="Online Location:" AssociatedControlID="OnlineLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OnlineLocationHyperLink" runat="server" Text='<%#: Eval("OnlineLocation") %>' NavigateUrl='<%#: Eval("OnlineLocation") %>' Target="_blank" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatisticsNotes") != null %>'>
                                <td>
                                    <asp:Label ID="StatisticsNotesLabel" runat="server" Text="Statistics Notes:" AssociatedControlID="StatisticsNotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatisticsNotesLiteral" runat="server" Text='<%#: Eval("StatisticsNotes") %>' />
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
    <asp:Panel ID="InterfaceTypesPanel" runat="server" Visible='<%# (string)Session["InterfaceTypesPermission"] != null && Interface2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InterfaceTypesHyperLink" runat="server" Text="Interface Types" NavigateUrl="~/InterfaceTypes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InterfaceTypesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InterfaceTypesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No interface types found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="InterfaceTypesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InterfaceTypesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
