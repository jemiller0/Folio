<%@ Page Title="Instances" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Instance2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Instance2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="Instance2sHyperLink" runat="server" Text="Instances" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="Instance2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="Instance2sRadGrid_NeedDataSource" OnItemCommand="Instance2sRadGrid_ItemCommand" OnDeleteCommand="Instance2sRadGrid_DeleteCommand">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instances found" CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton='<%# (string)Session["Instance2sPermission"] == "Edit" %>'>
                    <CommandItemSettings AddNewRecordText="New Instance" />
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditViewHyperLink" Text='<%# Session["Instance2sPermission"] %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteLinkButton" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this instance?")' Visible='<%# (string)Session["Instance2sPermission"] == "Edit" %>' />
                                <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Instance cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Version" DataField="Version" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Short Id" DataField="ShortId" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Match Key" DataField="MatchKey" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Author" DataField="Author" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Publication Year" DataField="PublicationYear" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Publication Period Start" DataField="PublicationPeriodStart" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Publication Period End" DataField="PublicationPeriodEnd" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance Type" DataField="InstanceType.Name" SortExpression="InstanceType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceTypeHyperLink" runat="server" Text='<%#: Eval("InstanceType.Name") %>' NavigateUrl='<%# $"~/InstanceType2s/Edit.aspx?Id={Eval("InstanceTypeId")}" %>' Enabled='<%# Session["InstanceType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Issuance Mode" DataField="IssuanceMode.Name" SortExpression="IssuanceMode.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IssuanceModeHyperLink" runat="server" Text='<%#: Eval("IssuanceMode.Name") %>' NavigateUrl='<%# $"~/IssuanceModes/Edit.aspx?Id={Eval("IssuanceModeId")}" %>' Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Cataloged Date" DataField="CatalogedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Previously Held" DataField="PreviouslyHeld" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Staff Suppress" DataField="StaffSuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Source Record Format" DataField="SourceRecordFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Status" DataField="Status.Name" SortExpression="Status.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="StatusHyperLink" runat="server" Text='<%#: Eval("Status.Name") %>' NavigateUrl='<%# $"~/Statuses/Edit.aspx?Id={Eval("StatusId")}" %>' Enabled='<%# Session["StatusesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status Last Write Time" DataField="StatusLastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
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
                    <telerik:AjaxUpdatedControl ControlID="Instance2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Instance2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Instance2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
