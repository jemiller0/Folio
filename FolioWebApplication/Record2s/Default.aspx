<%@ Page Title="Records" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Record2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Record2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="Record2sHyperLink" runat="server" Text="Records" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="Record2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="Record2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No records found" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" AllowSorting="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Snapshot" DataField="Snapshot.Id" AllowSorting="false" AllowFiltering="false" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="SnapshotHyperLink" runat="server" Text='<%# Eval("Snapshot.Id") %>' NavigateUrl='<%# $"~/Snapshot2s/Edit.aspx?Id={Eval("SnapshotId")}" %>' Enabled='<%# Session["Snapshot2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Matched Id" DataField="MatchedId" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Generation" DataField="Generation" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Record Type" DataField="RecordType" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridTemplateColumn HeaderText="Instance" DataField="Instance.Title" AllowSorting="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="State" DataField="State" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Leader Record Status" DataField="LeaderRecordStatus" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Order" DataField="Order" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Suppress Discovery" DataField="SuppressDiscovery" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridTemplateColumn HeaderText="Creation User" DataField="CreationUser.Username" AllowSorting="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AllowFiltering="false" AllowSorting="false" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Write User" DataField="LastWriteUser.Username" AllowSorting="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AllowFiltering="false" AllowSorting="false" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Instance Hrid" DataField="InstanceHrid" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Error Record 2" DataField="ErrorRecord2" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Marc Record 2" DataField="MarcRecord2" AllowFiltering="false" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Raw Record 2" DataField="RawRecord2" AllowFiltering="false" AllowSorting="false" />
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
                    <telerik:AjaxUpdatedControl ControlID="Record2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Record2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Record2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
