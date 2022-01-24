<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.BatchVoucherExportConfig2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="BatchVoucherExportConfig2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="BatchVoucherExportConfig2HyperLink" runat="server" Text="Batch Voucher Export Config" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="BatchVoucherExportConfig2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="BatchVoucherExportConfig2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewBatchVoucherExportConfig2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BatchGroup") != null %>'>
                                <td>
                                    <asp:Label ID="BatchGroupLabel" runat="server" Text="Batch Group:" AssociatedControlID="BatchGroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EnableScheduledExport") != null %>'>
                                <td>
                                    <asp:Label ID="EnableScheduledExportLabel" runat="server" Text="Enable Scheduled Export:" AssociatedControlID="EnableScheduledExportLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EnableScheduledExportLiteral" runat="server" Text='<%#: Eval("EnableScheduledExport") %>' />
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
                            <tr runat="server" visible='<%# Eval("StartTime") != null %>'>
                                <td>
                                    <asp:Label ID="StartTimeLabel" runat="server" Text="Start Time:" AssociatedControlID="StartTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StartTimeLiteral" runat="server" Text='<%#: Eval("StartTime") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UploadUri") != null %>'>
                                <td>
                                    <asp:Label ID="UploadUriLabel" runat="server" Text="Upload URI:" AssociatedControlID="UploadUriHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UploadUriHyperLink" runat="server" Text='<%#: Eval("UploadUri") %>' NavigateUrl='<%#: Eval("UploadUri") %>' Target="_blank" />
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
    <asp:Panel ID="BatchVoucherExportConfigWeekdaysPanel" runat="server" Visible='<%# (string)Session["BatchVoucherExportConfigWeekdaysPermission"] != null && BatchVoucherExportConfig2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BatchVoucherExportConfigWeekdaysHyperLink" runat="server" Text="Batch Voucher Export Config Weekdays" NavigateUrl="~/BatchVoucherExportConfigWeekdays/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="BatchVoucherExportConfigWeekdaysRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BatchVoucherExportConfigWeekdaysRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No batch voucher export config weekdays found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BatchVoucherExportConfigWeekdaysRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BatchVoucherExportConfigWeekdaysPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
