<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.ScheduledNotice2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="ScheduledNotice2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="ScheduledNotice2HyperLink" runat="server" Text="Scheduled Notice" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="ScheduledNotice2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="ScheduledNotice2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewScheduledNotice2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Loan") != null %>'>
                                <td>
                                    <asp:Label ID="LoanLabel" runat="server" Text="Loan:" AssociatedControlID="LoanHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Request") != null %>'>
                                <td>
                                    <asp:Label ID="RequestLabel" runat="server" Text="Request:" AssociatedControlID="RequestHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RequestHyperLink" runat="server" Text='<%# Eval("Request.Id") %>' NavigateUrl='<%# $"~/Request2s/Edit.aspx?Id={Eval("RequestId")}" %>' Enabled='<%# Session["Request2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Payment") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentLabel" runat="server" Text="Payment:" AssociatedControlID="PaymentHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PaymentHyperLink" runat="server" Text='<%# Eval("Payment.Id") %>' NavigateUrl='<%# $"~/Payment2s/Edit.aspx?Id={Eval("PaymentId")}" %>' Enabled='<%# Session["Payment2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RecipientUser") != null %>'>
                                <td>
                                    <asp:Label ID="RecipientUserLabel" runat="server" Text="Recipient User:" AssociatedControlID="RecipientUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="RecipientUserHyperLink" runat="server" Text='<%#: Eval("RecipientUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("RecipientUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NextRunTime") != null %>'>
                                <td>
                                    <asp:Label ID="NextRunTimeLabel" runat="server" Text="Next Run Time:" AssociatedControlID="NextRunTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NextRunTimeLiteral" runat="server" Text='<%# Eval("NextRunTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TriggeringEvent") != null %>'>
                                <td>
                                    <asp:Label ID="TriggeringEventLabel" runat="server" Text="Triggering Event:" AssociatedControlID="TriggeringEventLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TriggeringEventLiteral" runat="server" Text='<%#: Eval("TriggeringEvent") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigTiming") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigTimingLabel" runat="server" Text="Notice Config Timing:" AssociatedControlID="NoticeConfigTimingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoticeConfigTimingLiteral" runat="server" Text='<%#: Eval("NoticeConfigTiming") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigRecurringPeriodDuration") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigRecurringPeriodDurationLabel" runat="server" Text="Notice Config Recurring Period Duration:" AssociatedControlID="NoticeConfigRecurringPeriodDurationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoticeConfigRecurringPeriodDurationLiteral" runat="server" Text='<%#: Eval("NoticeConfigRecurringPeriodDuration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigRecurringPeriodInterval") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigRecurringPeriodIntervalLabel" runat="server" Text="Notice Config Recurring Period Interval:" AssociatedControlID="NoticeConfigRecurringPeriodIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoticeConfigRecurringPeriodIntervalLiteral" runat="server" Text='<%#: Eval("NoticeConfigRecurringPeriodInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigTemplate") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigTemplateLabel" runat="server" Text="Notice Config Template:" AssociatedControlID="NoticeConfigTemplateHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="NoticeConfigTemplateHyperLink" runat="server" Text='<%#: Eval("NoticeConfigTemplate.Name") %>' NavigateUrl='<%# $"~/Template2s/Edit.aspx?Id={Eval("NoticeConfigTemplateId")}" %>' Enabled='<%# Session["Template2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigFormat") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigFormatLabel" runat="server" Text="Notice Config Format:" AssociatedControlID="NoticeConfigFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoticeConfigFormatLiteral" runat="server" Text='<%#: Eval("NoticeConfigFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NoticeConfigSendInRealTime") != null %>'>
                                <td>
                                    <asp:Label ID="NoticeConfigSendInRealTimeLabel" runat="server" Text="Notice Config Send In Real Time:" AssociatedControlID="NoticeConfigSendInRealTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoticeConfigSendInRealTimeLiteral" runat="server" Text='<%#: Eval("NoticeConfigSendInRealTime") %>' />
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
