<%@ Page Title="Location Settings" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.LocationSettings.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LocationSettingsPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="LocationSettingsHyperLink" runat="server" Text="Location Settings" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="LocationSettingsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="LocationSettingsRadGrid_NeedDataSource" OnItemCommand="LocationSettingsRadGrid_ItemCommand" OnDeleteCommand="LocationSettingsRadGrid_DeleteCommand">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No location settings found" CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton='<%# (string)Session["LocationSettingsPermission"] == "Edit" %>'>
                    <CommandItemSettings AddNewRecordText="New Location Setting" />
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditViewHyperLink" Text='<%# Session["LocationSettingsPermission"] %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteLinkButton" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this location setting?")' Visible='<%# (string)Session["LocationSettingsPermission"] == "Edit" %>' />
                                <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Location Setting cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" DataField="Location.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Settings" DataField="Settings.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SettingsHyperLink" runat="server" Text='<%#: Eval("Settings.Name") %>' NavigateUrl='<%# $"~/Settings/Edit.aspx?Id={Eval("SettingsId")}" %>' Enabled='<%# Session["SettingsPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Enabled" DataField="Enabled" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Creation User" DataField="CreationUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Write User" DataField="LastWriteUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RadAjaxManager1_ClientEvents_OnRequestStart(ajaxManager, eventArgs) {
                if (eventArgs.get_eventTarget().indexOf("Export") != -1) eventArgs.set_enableAjax(false);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="RadAjaxManager1_ClientEvents_OnRequestStart">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ExportLinkButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LocationSettingsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LocationSettingsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LocationSettingsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
