<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.AgreementItem2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="AgreementItem2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="AgreementItem2HyperLink" runat="server" Text="Agreement Item" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="AgreementItem2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="AgreementItem2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewAgreementItem2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SuppressFromDiscovery") != null %>'>
                                <td>
                                    <asp:Label ID="SuppressFromDiscoveryLabel" runat="server" Text="Suppress From Discovery:" AssociatedControlID="SuppressFromDiscoveryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SuppressFromDiscoveryLiteral" runat="server" Text='<%#: Eval("SuppressFromDiscovery") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Note") != null %>'>
                                <td>
                                    <asp:Label ID="NoteLabel" runat="server" Text="Note:" AssociatedControlID="NoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoteLiteral" runat="server" Text='<%#: Eval("Note") %>' />
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
                            <tr runat="server" visible='<%# Eval("CustomCoverage") != null %>'>
                                <td>
                                    <asp:Label ID="CustomCoverageLabel" runat="server" Text="Custom Coverage:" AssociatedControlID="CustomCoverageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CustomCoverageLiteral" runat="server" Text='<%#: Eval("CustomCoverage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StartDate") != null %>'>
                                <td>
                                    <asp:Label ID="StartDateLabel" runat="server" Text="Start Date:" AssociatedControlID="StartDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StartDateLiteral" runat="server" Text='<%# Eval("StartDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EndDate") != null %>'>
                                <td>
                                    <asp:Label ID="EndDateLabel" runat="server" Text="End Date:" AssociatedControlID="EndDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EndDateLiteral" runat="server" Text='<%# Eval("EndDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ActiveFromDate") != null %>'>
                                <td>
                                    <asp:Label ID="ActiveFromDateLabel" runat="server" Text="Active From Date:" AssociatedControlID="ActiveFromDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ActiveFromDateLiteral" runat="server" Text='<%# Eval("ActiveFromDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ActiveToDate") != null %>'>
                                <td>
                                    <asp:Label ID="ActiveToDateLabel" runat="server" Text="Active To Date:" AssociatedControlID="ActiveToDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ActiveToDateLiteral" runat="server" Text='<%# Eval("ActiveToDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ContentLastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="ContentLastWriteTimeLabel" runat="server" Text="Content Last Write Time:" AssociatedControlID="ContentLastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ContentLastWriteTimeLiteral" runat="server" Text='<%# Eval("ContentLastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("HaveAccess") != null %>'>
                                <td>
                                    <asp:Label ID="HaveAccessLabel" runat="server" Text="Have Access:" AssociatedControlID="HaveAccessLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HaveAccessLiteral" runat="server" Text='<%#: Eval("HaveAccess") %>' />
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
                            <tr runat="server" visible='<%# Eval("LastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteTimeLabel" runat="server" Text="Last Write Time:" AssociatedControlID="LastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastWriteTimeLiteral" runat="server" Text='<%# Eval("LastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Agreement") != null %>'>
                                <td>
                                    <asp:Label ID="AgreementLabel" runat="server" Text="Agreement:" AssociatedControlID="AgreementHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="AgreementHyperLink" runat="server" Text='<%#: Eval("Agreement.Name") %>' NavigateUrl='<%# $"~/Agreement2s/Edit.aspx?Id={Eval("AgreementId")}" %>' Enabled='<%# Session["Agreement2sPermission"] != null %>' />
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
    <asp:Panel ID="AgreementItemOrderItemsPanel" runat="server" Visible='<%# (string)Session["AgreementItemOrderItemsPermission"] != null && AgreementItem2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="AgreementItemOrderItemsHyperLink" runat="server" Text="Agreement Item Order Items" NavigateUrl="~/AgreementItemOrderItems/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="AgreementItemOrderItemsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="AgreementItemOrderItemsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No agreement item order items found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="AgreementItemOrderItemsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AgreementItemOrderItemsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
