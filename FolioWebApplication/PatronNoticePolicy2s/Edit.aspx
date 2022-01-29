<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.PatronNoticePolicy2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PatronNoticePolicy2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="PatronNoticePolicy2HyperLink" runat="server" Text="Patron Notice Policy" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="PatronNoticePolicy2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="PatronNoticePolicy2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewPatronNoticePolicy2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Active") != null %>'>
                                <td>
                                    <asp:Label ID="ActiveLabel" runat="server" Text="Active:" AssociatedControlID="ActiveLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ActiveLiteral" runat="server" Text='<%#: Eval("Active") %>' />
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
    <asp:Panel ID="PatronNoticePolicyFeeFineNoticesPanel" runat="server" Visible='<%# (string)Session["PatronNoticePolicyFeeFineNoticesPermission"] != null && PatronNoticePolicy2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PatronNoticePolicyFeeFineNoticesHyperLink" runat="server" Text="Patron Notice Policy Fee Fine Notices" NavigateUrl="~/PatronNoticePolicyFeeFineNotices/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PatronNoticePolicyFeeFineNoticesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PatronNoticePolicyFeeFineNoticesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No patron notice policy fee fine notices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Name" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Template" DataField="Template.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemplateHyperLink" runat="server" Text='<%#: Eval("TemplateId") != null ? Eval("Template.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("TemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Template Name" DataField="TemplateName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Frequency" DataField="Frequency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Real Time" DataField="RealTime" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send How" DataField="SendOptionsSendHow" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send When" DataField="SendOptionsSendWhen" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Duration" DataField="SendOptionsSendByDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Interval" DataField="SendOptionsSendByInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Duration" DataField="SendOptionsSendEveryDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Interval" DataField="SendOptionsSendEveryInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PatronNoticePolicyLoanNoticesPanel" runat="server" Visible='<%# (string)Session["PatronNoticePolicyLoanNoticesPermission"] != null && PatronNoticePolicy2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PatronNoticePolicyLoanNoticesHyperLink" runat="server" Text="Patron Notice Policy Loan Notices" NavigateUrl="~/PatronNoticePolicyLoanNotices/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PatronNoticePolicyLoanNoticesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PatronNoticePolicyLoanNoticesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No patron notice policy loan notices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Name" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Template" DataField="Template.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemplateHyperLink" runat="server" Text='<%#: Eval("TemplateId") != null ? Eval("Template.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("TemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Template Name" DataField="TemplateName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Frequency" DataField="Frequency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Real Time" DataField="RealTime" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send How" DataField="SendOptionsSendHow" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send When" DataField="SendOptionsSendWhen" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Duration" DataField="SendOptionsSendByDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Interval" DataField="SendOptionsSendByInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Duration" DataField="SendOptionsSendEveryDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Interval" DataField="SendOptionsSendEveryInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PatronNoticePolicyRequestNoticesPanel" runat="server" Visible='<%# (string)Session["PatronNoticePolicyRequestNoticesPermission"] != null && PatronNoticePolicy2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PatronNoticePolicyRequestNoticesHyperLink" runat="server" Text="Patron Notice Policy Request Notices" NavigateUrl="~/PatronNoticePolicyRequestNotices/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PatronNoticePolicyRequestNoticesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PatronNoticePolicyRequestNoticesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No patron notice policy request notices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Name" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Template" DataField="Template.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemplateHyperLink" runat="server" Text='<%#: Eval("TemplateId") != null ? Eval("Template.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("TemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Template Name" DataField="TemplateName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Frequency" DataField="Frequency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Real Time" DataField="RealTime" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send How" DataField="SendOptionsSendHow" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send When" DataField="SendOptionsSendWhen" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Duration" DataField="SendOptionsSendByDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send By Interval" DataField="SendOptionsSendByInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Duration" DataField="SendOptionsSendEveryDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Send Options Send Every Interval" DataField="SendOptionsSendEveryInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="PatronNoticePolicyFeeFineNoticesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PatronNoticePolicyFeeFineNoticesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PatronNoticePolicyLoanNoticesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PatronNoticePolicyLoanNoticesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PatronNoticePolicyRequestNoticesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PatronNoticePolicyRequestNoticesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
