<%@ Page Title="Agreement Items" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.AgreementItem2s.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="AgreementItem2sPanel" runat="server">
        <fieldset>
            <legend><asp:HyperLink ID="AgreementItem2sHyperLink" runat="server" Text="Agreement Items" NavigateUrl="Default.aspx" /></legend>
            <asp:LinkButton ID="ExportLinkButton" runat="server" Text="Export" OnClick="ExportLinkButton_Click" />
            <telerik:RadGrid ID="AgreementItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="100" EnableLinqExpressions="false" OnNeedDataSource="AgreementItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No agreement items found" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Suppress From Discovery" DataField="SuppressFromDiscovery" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Custom Coverage" DataField="CustomCoverage" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Start Date" DataField="StartDate" SortExpression="StartDate" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="StartDateHyperLink" runat="server" Text='<%# $"{Eval("StartDate"):d}" ?? "&nbsp;" %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="End Date" DataField="EndDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Active From Date" DataField="ActiveFromDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Active To Date" DataField="ActiveToDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Content Last Write Time" DataField="ContentLastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Have Access" DataField="HaveAccess" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Agreement" DataField="Agreement.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AgreementHyperLink" runat="server" Text='<%#: Eval("AgreementId") != null ? Eval("Agreement.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Agreement2s/Edit.aspx?Id={Eval("AgreementId")}" %>' Enabled='<%# Session["Agreement2sPermission"] != null %>' />
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
                    <telerik:AjaxUpdatedControl ControlID="AgreementItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="AgreementItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AgreementItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
