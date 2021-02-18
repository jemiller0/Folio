<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Payment2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Payment2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Payment2HyperLink" runat="server" Text="Payment" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Payment2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Payment2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewPayment2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
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
                            <tr runat="server" visible='<%# Eval("TypeAction") != null %>'>
                                <td>
                                    <asp:Label ID="TypeActionLabel" runat="server" Text="Type Action:" AssociatedControlID="TypeActionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TypeActionLiteral" runat="server" Text='<%#: Eval("TypeAction") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Comments") != null %>'>
                                <td>
                                    <asp:Label ID="CommentsLabel" runat="server" Text="Comments:" AssociatedControlID="CommentsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CommentsLiteral" runat="server" Text='<%#: Eval("Comments") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Notify") != null %>'>
                                <td>
                                    <asp:Label ID="NotifyLabel" runat="server" Text="Notify:" AssociatedControlID="NotifyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NotifyLiteral" runat="server" Text='<%#: Eval("Notify") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Amount") != null %>'>
                                <td>
                                    <asp:Label ID="AmountLabel" runat="server" Text="Amount:" AssociatedControlID="AmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AmountLiteral" runat="server" Text='<%# Eval("Amount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RemainingAmount") != null %>'>
                                <td>
                                    <asp:Label ID="RemainingAmountLabel" runat="server" Text="Remaining Amount:" AssociatedControlID="RemainingAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RemainingAmountLiteral" runat="server" Text='<%# Eval("RemainingAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TransactionInformation") != null %>'>
                                <td>
                                    <asp:Label ID="TransactionInformationLabel" runat="server" Text="Transaction Information:" AssociatedControlID="TransactionInformationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TransactionInformationLiteral" runat="server" Text='<%#: Eval("TransactionInformation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreatedAt") != null %>'>
                                <td>
                                    <asp:Label ID="CreatedAtLabel" runat="server" Text="Created At:" AssociatedControlID="CreatedAtLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreatedAtLiteral" runat="server" Text='<%#: Eval("CreatedAt") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Source") != null %>'>
                                <td>
                                    <asp:Label ID="SourceLabel" runat="server" Text="Source:" AssociatedControlID="SourceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceLiteral" runat="server" Text='<%#: Eval("Source") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentMethod") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentMethodLabel" runat="server" Text="Payment Method:" AssociatedControlID="PaymentMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentMethodLiteral" runat="server" Text='<%#: Eval("PaymentMethod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Fee") != null %>'>
                                <td>
                                    <asp:Label ID="FeeLabel" runat="server" Text="Fee:" AssociatedControlID="FeeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FeeHyperLink" runat="server" Text='<%#: Eval("Fee.Title") %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("FeeId")}" %>' Enabled='<%# Session["Fee2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("User") != null %>'>
                                <td>
                                    <asp:Label ID="UserLabel" runat="server" Text="User:" AssociatedControlID="UserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("User.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
    <asp:Panel ID="ScheduledNotice2sPanel" runat="server" Visible='<%# (string)Session["ScheduledNotice2sPermission"] != null && Payment2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ScheduledNotice2sHyperLink" runat="server" Text="Scheduled Notices" NavigateUrl="~/ScheduledNotice2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="ScheduledNotice2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ScheduledNotice2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No scheduled notices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/ScheduledNotice2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/ScheduledNotice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Loan" DataField="Loan.Id" SortExpression="Loan.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Request" DataField="Request.Id" SortExpression="Request.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="RequestHyperLink" runat="server" Text='<%# Eval("Request.Id") %>' NavigateUrl='<%# $"~/Request2s/Edit.aspx?Id={Eval("RequestId")}" %>' Enabled='<%# Session["Request2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Recipient User" DataField="RecipientUser.Username" SortExpression="RecipientUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RecipientUserHyperLink" runat="server" Text='<%#: Eval("RecipientUserId") != null ? Eval("RecipientUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("RecipientUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Next Run Time" DataField="NextRunTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Triggering Event" DataField="TriggeringEvent" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Timing" DataField="NoticeConfigTiming" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Recurring Period Duration" DataField="NoticeConfigRecurringPeriodDuration" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Recurring Period Interval" DataField="NoticeConfigRecurringPeriodInterval" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Notice Config Template" DataField="NoticeConfigTemplate.Name" SortExpression="NoticeConfigTemplate.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NoticeConfigTemplateHyperLink" runat="server" Text='<%#: Eval("NoticeConfigTemplateId") != null ? Eval("NoticeConfigTemplate.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("NoticeConfigTemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Notice Config Format" DataField="NoticeConfigFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notice Config Send In Real Time" DataField="NoticeConfigSendInRealTime" AutoPostBackOnFilter="true" />
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ScheduledNotice2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ScheduledNotice2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
