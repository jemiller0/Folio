<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Instance2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Instance2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Instance2HyperLink" runat="server" Text="Instance" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Instance2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Instance2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewInstance2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                <td>
                                    <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%#: Eval("ShortId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MatchKey") != null %>'>
                                <td>
                                    <asp:Label ID="MatchKeyLabel" runat="server" Text="Match Key:" AssociatedControlID="MatchKeyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MatchKeyLiteral" runat="server" Text='<%#: Eval("MatchKey") %>' />
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
                            <tr runat="server" visible='<%# Eval("Title") != null %>'>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TitleLiteral" runat="server" Text='<%#: Eval("Title") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Author") != null %>'>
                                <td>
                                    <asp:Label ID="AuthorLabel" runat="server" Text="Author:" AssociatedControlID="AuthorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AuthorLiteral" runat="server" Text='<%#: Eval("Author") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationStartYear") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationStartYearLabel" runat="server" Text="Publication Start Year:" AssociatedControlID="PublicationStartYearLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationStartYearLiteral" runat="server" Text='<%#: Eval("PublicationStartYear") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationEndYear") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationEndYearLabel" runat="server" Text="Publication End Year:" AssociatedControlID="PublicationEndYearLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationEndYearLiteral" runat="server" Text='<%#: Eval("PublicationEndYear") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InstanceType") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceTypeLabel" runat="server" Text="Instance Type:" AssociatedControlID="InstanceTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceTypeHyperLink" runat="server" Text='<%#: Eval("InstanceType.Name") %>' NavigateUrl='<%# $"~/InstanceType2s/Edit.aspx?Id={Eval("InstanceTypeId")}" %>' Enabled='<%# Session["InstanceType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IssuanceMode") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Issuance Mode:" NavigateUrl="~/IssuanceModes/Default.aspx" Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IssuanceModeHyperLink" runat="server" Text='<%#: Eval("IssuanceMode.Name") %>' NavigateUrl='<%# $"~/IssuanceModes/Edit.aspx?Id={Eval("IssuanceModeId")}" %>' Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CatalogedDate") != null %>'>
                                <td>
                                    <asp:Label ID="CatalogedDateLabel" runat="server" Text="Cataloged Date:" AssociatedControlID="CatalogedDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CatalogedDateLiteral" runat="server" Text='<%# Eval("CatalogedDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PreviouslyHeld") != null %>'>
                                <td>
                                    <asp:Label ID="PreviouslyHeldLabel" runat="server" Text="Previously Held:" AssociatedControlID="PreviouslyHeldLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PreviouslyHeldLiteral" runat="server" Text='<%#: Eval("PreviouslyHeld") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StaffSuppress") != null %>'>
                                <td>
                                    <asp:Label ID="StaffSuppressLabel" runat="server" Text="Staff Suppress:" AssociatedControlID="StaffSuppressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StaffSuppressLiteral" runat="server" Text='<%#: Eval("StaffSuppress") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DiscoverySuppress") != null %>'>
                                <td>
                                    <asp:Label ID="DiscoverySuppressLabel" runat="server" Text="Discovery Suppress:" AssociatedControlID="DiscoverySuppressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscoverySuppressLiteral" runat="server" Text='<%#: Eval("DiscoverySuppress") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SourceRecordFormat") != null %>'>
                                <td>
                                    <asp:Label ID="SourceRecordFormatLabel" runat="server" Text="Source Record Format:" AssociatedControlID="SourceRecordFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceRecordFormatLiteral" runat="server" Text='<%#: Eval("SourceRecordFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Status:" NavigateUrl="~/Statuses/Default.aspx" Enabled='<%# Session["StatusesPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="StatusHyperLink" runat="server" Text='<%#: Eval("Status.Name") %>' NavigateUrl='<%# $"~/Statuses/Edit.aspx?Id={Eval("StatusId")}" %>' Enabled='<%# Session["StatusesPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatusLastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLastWriteTimeLabel" runat="server" Text="Status Last Write Time:" AssociatedControlID="StatusLastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLastWriteTimeLiteral" runat="server" Text='<%# Eval("StatusLastWriteTime", "{0:g}") %>' />
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
                            <tr runat="server" visible='<%# Eval("CompletionTime") != null %>'>
                                <td>
                                    <asp:Label ID="CompletionTimeLabel" runat="server" Text="Completion Time:" AssociatedControlID="CompletionTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CompletionTimeLiteral" runat="server" Text='<%# Eval("CompletionTime", "{0:g}") %>' />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="AdministrativeNotesPanel" runat="server" Visible='<%# (string)Session["AdministrativeNotesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="AdministrativeNotesHyperLink" runat="server" Text="Administrative Notes" NavigateUrl="~/AdministrativeNotes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="AdministrativeNotesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="AdministrativeNotesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No administrative notes found">
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
    <asp:Panel ID="AlternativeTitlesPanel" runat="server" Visible='<%# (string)Session["AlternativeTitlesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="AlternativeTitlesHyperLink" runat="server" Text="Alternative Titles" NavigateUrl="~/AlternativeTitles/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="AlternativeTitlesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="AlternativeTitlesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No alternative titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Alternative Title Type" DataField="AlternativeTitleType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AlternativeTitleTypeHyperLink" runat="server" Text='<%#: Eval("AlternativeTitleType.Name") %>' NavigateUrl='<%# $"~/AlternativeTitleType2s/Edit.aspx?Id={Eval("AlternativeTitleTypeId")}" %>' Enabled='<%# Session["AlternativeTitleType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="ClassificationsPanel" runat="server" Visible='<%# (string)Session["ClassificationsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ClassificationsHyperLink" runat="server" Text="Classifications" NavigateUrl="~/Classifications/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ClassificationsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ClassificationsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No classifications found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Number" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Number" DataField="Number" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Classification Type" DataField="ClassificationType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ClassificationTypeHyperLink" runat="server" Text='<%#: Eval("ClassificationType.Name") %>' NavigateUrl='<%# $"~/ClassificationType2s/Edit.aspx?Id={Eval("ClassificationTypeId")}" %>' Enabled='<%# Session["ClassificationType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="ContributorsPanel" runat="server" Visible='<%# (string)Session["ContributorsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ContributorsHyperLink" runat="server" Text="Contributors" NavigateUrl="~/Contributors/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ContributorsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ContributorsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No contributors found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Name" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Contributor Type" DataField="ContributorType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ContributorTypeHyperLink" runat="server" Text='<%#: Eval("ContributorType.Name") %>' NavigateUrl='<%# $"~/ContributorType2s/Edit.aspx?Id={Eval("ContributorTypeId")}" %>' Enabled='<%# Session["ContributorType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Contributor Type Text" DataField="ContributorTypeText" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Contributor Name Type" DataField="ContributorNameType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ContributorNameTypeHyperLink" runat="server" Text='<%#: Eval("ContributorNameType.Name") %>' NavigateUrl='<%# $"~/ContributorNameType2s/Edit.aspx?Id={Eval("ContributorNameTypeId")}" %>' Enabled='<%# Session["ContributorNameType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Primary" DataField="Primary" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="EditionsPanel" runat="server" Visible='<%# (string)Session["EditionsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="EditionsHyperLink" runat="server" Text="Editions" NavigateUrl="~/Editions/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="EditionsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="EditionsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No editions found">
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
    <asp:Panel ID="ElectronicAccessesPanel" runat="server" Visible='<%# (string)Session["ElectronicAccessesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ElectronicAccessesHyperLink" runat="server" Text="Electronic Accesses" NavigateUrl="~/ElectronicAccesses/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ElectronicAccessesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ElectronicAccessesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No electronic accesses found">
                    <Columns>
                        <telerik:GridHyperLinkColumn HeaderText="URI" DataTextField="Uri" DataNavigateUrlFields="Uri" Target="_blank" SortExpression="Uri" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Link Text" DataField="LinkText" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Materials Specification" DataField="MaterialsSpecification" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Public Note" DataField="PublicNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Relationship" DataField="Relationship.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="RelationshipHyperLink" runat="server" Text='<%#: Eval("Relationship.Name") %>' NavigateUrl='<%# $"~/ElectronicAccessRelationship2s/Edit.aspx?Id={Eval("RelationshipId")}" %>' Enabled='<%# Session["ElectronicAccessRelationship2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Fee2sPanel" runat="server" Visible='<%# (string)Session["Fee2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Fee2sHyperLink" runat="server" Text="Fees" NavigateUrl="~/Fee2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Fee2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Fee2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No fees found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/users/{Eval("UserId")}/accounts/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Remaining Amount" DataField="RemainingAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Status Name" DataField="PaymentStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Material Type" DataField="MaterialType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status Name" DataField="ItemStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Location" DataField="Location" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Returned Time" DataField="ReturnedTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Loan" DataField="Loan.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material Type 1" DataField="MaterialType1.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialType1HyperLink" runat="server" Text='<%#: Eval("MaterialType1.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fee Type" DataField="FeeType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner" DataField="Owner.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("OwnerId") != null ? Eval("Owner.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Holding" DataField="Holding.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("HoldingId") != null ? Eval("Holding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Holding2sPanel" runat="server" Visible='<%# (string)Session["Holding2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Holding2sHyperLink" runat="server" Text="Holdings" NavigateUrl="~/Holding2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Holding2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Holding2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holdings found">
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/inventory/view/{Eval("InstanceId")}/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Holding Type" DataField="HoldingType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingTypeHyperLink" runat="server" Text='<%#: Eval("HoldingType.Name") %>' NavigateUrl='<%# $"~/HoldingType2s/Edit.aspx?Id={Eval("HoldingTypeId")}" %>' Enabled='<%# Session["HoldingType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" DataField="Location.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Temporary Location" DataField="TemporaryLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Location" DataField="EffectiveLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Call Number Type" DataField="CallNumberType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Shelving Title" DataField="ShelvingTitle" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Format" DataField="AcquisitionFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Method" DataField="AcquisitionMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receipt Status" DataField="ReceiptStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Ill Policy" DataField="IllPolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IllPolicyHyperLink" runat="server" Text='<%#: Eval("IllPolicy.Name") %>' NavigateUrl='<%# $"~/IllPolicy2s/Edit.aspx?Id={Eval("IllPolicyId")}" %>' Enabled='<%# Session["IllPolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Retention Policy" DataField="RetentionPolicy" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Digitization Policy" DataField="DigitizationPolicy" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Count" DataField="ItemCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving History Display Type" DataField="ReceivingHistoryDisplayType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
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
                        <telerik:GridTemplateColumn HeaderText="Source" DataField="Source.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceHyperLink" runat="server" Text='<%#: Eval("SourceId") != null ? Eval("Source.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Source2s/Edit.aspx?Id={Eval("SourceId")}" %>' Enabled='<%# Session["Source2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Items">
                            <ItemTemplate>
                                <telerik:RadGrid ID="Holding2sItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Holding2sItem2sRadGrid_NeedDataSource">
                                    <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No items found">
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="NewLabelHyperLink" runat="server" Text="New Label" NavigateUrl='<%# $"~/Labels/Edit.aspx?Barcode={Eval("Barcode")}" %>' Visible='<%# !string.IsNullOrWhiteSpace((string)Eval("Barcode")) && Session["LabelsPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                                            <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridTemplateColumn HeaderText="Barcode" DataField="Barcode" SortExpression="Barcode" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="BarcodeHyperLink" runat="server" Text='<%#: Eval("Barcode") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Effective Shelving Order" DataField="EffectiveShelvingOrder" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridTemplateColumn HeaderText="Call Number Type" DataField="CallNumberType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number" DataField="EffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number Prefix" DataField="EffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number Suffix" DataField="EffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridTemplateColumn HeaderText="Effective Call Number Type" DataField="EffectiveCallNumberType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="EffectiveCallNumberTypeHyperLink" runat="server" Text='<%#: Eval("EffectiveCallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("EffectiveCallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Volume" DataField="Volume" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Item Identifier" DataField="ItemIdentifier" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Pieces Count" DataField="PiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Pieces Description" DataField="PiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Count" DataField="MissingPiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Description" DataField="MissingPiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Time" DataField="MissingPiecesTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn HeaderText="Damaged Status" DataField="DamagedStatus.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="DamagedStatusHyperLink" runat="server" Text='<%#: Eval("DamagedStatus.Name") %>' NavigateUrl='<%# $"~/ItemDamagedStatus2s/Edit.aspx?Id={Eval("DamagedStatusId")}" %>' Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Damaged Status Time" DataField="DamagedStatusTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Status Last Write Time" DataField="StatusLastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn HeaderText="Material Type" DataField="MaterialType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="MaterialTypeHyperLink" runat="server" Text='<%#: Eval("MaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Permanent Loan Type" DataField="PermanentLoanType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="PermanentLoanTypeHyperLink" runat="server" Text='<%#: Eval("PermanentLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("PermanentLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Temporary Loan Type" DataField="TemporaryLoanType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="TemporaryLoanTypeHyperLink" runat="server" Text='<%#: Eval("TemporaryLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("TemporaryLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Permanent Location" DataField="PermanentLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="PermanentLocationHyperLink" runat="server" Text='<%#: Eval("PermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("PermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Temporary Location" DataField="TemporaryLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Effective Location" DataField="EffectiveLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="In Transit Destination Service Point" DataField="InTransitDestinationServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="InTransitDestinationServicePointHyperLink" runat="server" Text='<%#: Eval("InTransitDestinationServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("InTransitDestinationServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
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
                                            <telerik:GridBoundColumn HeaderText="Last Check In Date Time" DataField="LastCheckInDateTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn HeaderText="Last Check In Service Point" DataField="LastCheckInServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="LastCheckInServicePointHyperLink" runat="server" Text='<%#: Eval("LastCheckInServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("LastCheckInServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Last Check In Staff Member" DataField="LastCheckInStaffMember.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%#: Eval("LastCheckInStaffMemberId") != null ? Eval("LastCheckInStaffMember.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastCheckInStaffMemberId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="IdentifiersPanel" runat="server" Visible='<%# (string)Session["IdentifiersPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="IdentifiersHyperLink" runat="server" Text="Identifiers" NavigateUrl="~/Identifiers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="IdentifiersRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="IdentifiersRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No identifiers found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Identifier Type" DataField="IdentifierType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdentifierTypeHyperLink" runat="server" Text='<%#: Eval("IdentifierType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("IdentifierTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InstanceFormat2sPanel" runat="server" Visible='<%# (string)Session["InstanceFormat2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InstanceFormat2sHyperLink" runat="server" Text="Instance Formats" NavigateUrl="~/InstanceFormat2s/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InstanceFormat2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InstanceFormat2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instance formats found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Format" DataField="Format.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FormatHyperLink" runat="server" Text='<%#: Eval("Format.Name") %>' NavigateUrl='<%# $"~/Formats/Edit.aspx?Id={Eval("FormatId")}" %>' Enabled='<%# Session["FormatsPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InstanceNatureOfContentTermsPanel" runat="server" Visible='<%# (string)Session["InstanceNatureOfContentTermsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InstanceNatureOfContentTermsHyperLink" runat="server" Text="Instance Nature Of Content Terms" NavigateUrl="~/InstanceNatureOfContentTerms/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InstanceNatureOfContentTermsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InstanceNatureOfContentTermsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instance nature of content terms found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nature Of Content Term" DataField="NatureOfContentTerm.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NatureOfContentTermHyperLink" runat="server" Text='<%#: Eval("NatureOfContentTerm.Name") %>' NavigateUrl='<%# $"~/NatureOfContentTerm2s/Edit.aspx?Id={Eval("NatureOfContentTermId")}" %>' Enabled='<%# Session["NatureOfContentTerm2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InstanceNotesPanel" runat="server" Visible='<%# (string)Session["InstanceNotesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InstanceNotesHyperLink" runat="server" Text="Instance Notes" NavigateUrl="~/InstanceNotes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InstanceNotesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InstanceNotesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instance notes found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Instance Note Type" DataField="InstanceNoteType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceNoteTypeHyperLink" runat="server" Text='<%#: Eval("InstanceNoteType.Name") %>' NavigateUrl='<%# $"~/InstanceNoteType2s/Edit.aspx?Id={Eval("InstanceNoteTypeId")}" %>' Enabled='<%# Session["InstanceNoteType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Only" DataField="StaffOnly" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InstanceStatisticalCodesPanel" runat="server" Visible='<%# (string)Session["InstanceStatisticalCodesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InstanceStatisticalCodesHyperLink" runat="server" Text="Instance Statistical Codes" NavigateUrl="~/InstanceStatisticalCodes/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InstanceStatisticalCodesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InstanceStatisticalCodesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instance statistical codes found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Statistical Code" DataField="StatisticalCode.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="StatisticalCodeHyperLink" runat="server" Text='<%#: Eval("StatisticalCode.Name") %>' NavigateUrl='<%# $"~/StatisticalCode2s/Edit.aspx?Id={Eval("StatisticalCodeId")}" %>' Enabled='<%# Session["StatisticalCode2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InstanceTagsPanel" runat="server" Visible='<%# (string)Session["InstanceTagsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InstanceTagsHyperLink" runat="server" Text="Instance Tags" NavigateUrl="~/InstanceTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InstanceTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InstanceTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No instance tags found">
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
    <asp:Panel ID="LanguagesPanel" runat="server" Visible='<%# (string)Session["LanguagesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LanguagesHyperLink" runat="server" Text="Languages" NavigateUrl="~/Languages/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="LanguagesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LanguagesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No languages found">
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
    <asp:Panel ID="OrderItem2sPanel" runat="server" Visible='<%# (string)Session["OrderItem2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderItem2sHyperLink" runat="server" Text="Order Items" NavigateUrl="~/OrderItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="OrderItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="OrderItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No order items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/orders/lines/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Edition" DataField="Edition" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Checkin Items" DataField="CheckinItems" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Agreement" DataField="Agreement.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AgreementHyperLink" runat="server" Text='<%#: Eval("AgreementId") != null ? Eval("Agreement.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Agreement2s/Edit.aspx?Id={Eval("AgreementId")}" %>' Enabled='<%# Session["Agreement2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Acquisition Method" DataField="AcquisitionMethod.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AcquisitionMethodHyperLink" runat="server" Text='<%#: Eval("AcquisitionMethodId") != null ? Eval("AcquisitionMethod.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/AcquisitionMethod2s/Edit.aspx?Id={Eval("AcquisitionMethodId")}" %>' Enabled='<%# Session["AcquisitionMethod2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Automatic Export" DataField="AutomaticExport" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction" DataField="CancellationRestriction" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction Note" DataField="CancellationRestrictionNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Collection" DataField="Collection" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Physical Unit List Price" DataField="PhysicalUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Electronic Unit List Price" DataField="ElectronicUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Additional Cost" DataField="AdditionalCost" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Discount" DataField="Discount" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Discount Type" DataField="DiscountType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Exchange Rate" DataField="ExchangeRate" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Physical Quantity" DataField="PhysicalQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Electronic Quantity" DataField="ElectronicQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Estimated Price" DataField="EstimatedPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Fiscal Year Rollover Adjustment Amount" DataField="FiscalYearRolloverAdjustmentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Internal Note" DataField="InternalNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Note" DataField="ReceivingNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription From" DataField="SubscriptionFrom" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription Interval" DataField="SubscriptionInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Subscription To" DataField="SubscriptionTo" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Donor" DataField="Donor" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource Activated" DataField="EresourceActivated" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Activation Due" DataField="EresourceActivationDue" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Create Inventory" DataField="EresourceCreateInventory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource Trial" DataField="EresourceTrial" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Expected Activation Date" DataField="EresourceExpectedActivationDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Eresource User Limit" DataField="EresourceUserLimit" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Eresource Access Provider" DataField="EresourceAccessProvider.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceAccessProviderHyperLink" runat="server" Text='<%#: Eval("EresourceAccessProvider.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("EresourceAccessProviderId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Eresource License Code" DataField="EresourceLicenseCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Description" DataField="EresourceLicenseDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Reference" DataField="EresourceLicenseReference" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Eresource Material Type" DataField="EresourceMaterialType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceMaterialTypeHyperLink" runat="server" Text='<%#: Eval("EresourceMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("EresourceMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridHyperLinkColumn HeaderText="Eresource Resource URL" DataTextField="EresourceResourceUrl" DataNavigateUrlFields="EresourceResourceUrl" Target="_blank" SortExpression="EresourceResourceUrl" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Is Package" DataField="IsPackage" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Last EDI Export Date" DataField="LastEdiExportDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Order Format" DataField="OrderFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Package Order Item" DataField="PackageOrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="PackageOrderItemHyperLink" runat="server" Text='<%#: Eval("PackageOrderItemId") != null ? Eval("PackageOrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("PackageOrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Payment Status" DataField="PaymentStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Physical Create Inventory" DataField="PhysicalCreateInventory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Physical Material Type" DataField="PhysicalMaterialType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialTypeHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("PhysicalMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Physical Material Supplier" DataField="PhysicalMaterialSupplier.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialSupplierHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialSupplier.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("PhysicalMaterialSupplierId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Physical Expected Receipt Date" DataField="PhysicalExpectedReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Physical Receipt Due" DataField="PhysicalReceiptDue" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Publication Year" DataField="PublicationYear" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Receipt Date" DataField="ReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Receipt Status" DataField="ReceiptStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewal Note" DataField="RenewalNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester" DataField="Requester" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Rush" DataField="Rush" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Selector" DataField="Selector" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Title Or Package" DataField="TitleOrPackage" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Instructions" DataField="VendorInstructions" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Note" DataField="VendorNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Customer Id" DataField="VendorCustomerId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="PhysicalDescriptionsPanel" runat="server" Visible='<%# (string)Session["PhysicalDescriptionsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PhysicalDescriptionsHyperLink" runat="server" Text="Physical Descriptions" NavigateUrl="~/PhysicalDescriptions/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PhysicalDescriptionsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PhysicalDescriptionsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No physical descriptions found">
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
    <asp:Panel ID="PrecedingSucceedingTitle2sPanel" runat="server" Visible='<%# (string)Session["PrecedingSucceedingTitle2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitle2sHyperLink" runat="server" Text="Preceding Succeeding Titles" NavigateUrl="~/PrecedingSucceedingTitle2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="PrecedingSucceedingTitle2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PrecedingSucceedingTitle2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No preceding succeeding titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Succeeding Instance" DataField="SucceedingInstance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SucceedingInstanceHyperLink" runat="server" Text='<%#: Eval("SucceedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SucceedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Short Id" DataField="ShortId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="PrecedingSucceedingTitle2s1Panel" runat="server" Visible='<%# (string)Session["PrecedingSucceedingTitle2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitle2s1HyperLink" runat="server" Text="Preceding Succeeding Titles 1" NavigateUrl="~/PrecedingSucceedingTitle2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="PrecedingSucceedingTitle2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PrecedingSucceedingTitle2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No preceding succeeding titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Preceding Instance" DataField="PrecedingInstance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PrecedingInstanceHyperLink" runat="server" Text='<%#: Eval("PrecedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("PrecedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Short Id" DataField="ShortId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="PublicationFrequenciesPanel" runat="server" Visible='<%# (string)Session["PublicationFrequenciesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PublicationFrequenciesHyperLink" runat="server" Text="Publication Frequencies" NavigateUrl="~/PublicationFrequencies/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PublicationFrequenciesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PublicationFrequenciesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No publication frequencies found">
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
    <asp:Panel ID="PublicationRangesPanel" runat="server" Visible='<%# (string)Session["PublicationRangesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PublicationRangesHyperLink" runat="server" Text="Publication Ranges" NavigateUrl="~/PublicationRanges/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PublicationRangesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PublicationRangesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No publication ranges found">
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
    <asp:Panel ID="PublicationsPanel" runat="server" Visible='<%# (string)Session["PublicationsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PublicationsHyperLink" runat="server" Text="Publications" NavigateUrl="~/Publications/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PublicationsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PublicationsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No publications found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Place" DataField="Place" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Publication Year" DataField="PublicationYear" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Role" DataField="Role" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RelationshipsPanel" runat="server" Visible='<%# (string)Session["RelationshipsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RelationshipsHyperLink" runat="server" Text="Relationships" NavigateUrl="~/Relationships/Default.aspx" /></legend>
            <telerik:RadGrid ID="RelationshipsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RelationshipsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No relationships found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Super Instance" DataField="SuperInstance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SuperInstanceHyperLink" runat="server" Text='<%#: Eval("SuperInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SuperInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Instance Relationship Type" DataField="InstanceRelationshipType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceRelationshipTypeHyperLink" runat="server" Text='<%#: Eval("InstanceRelationshipType.Name") %>' NavigateUrl='<%# $"~/RelationshipTypes/Edit.aspx?Id={Eval("InstanceRelationshipTypeId")}" %>' Enabled='<%# Session["RelationshipTypesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    <asp:Panel ID="Relationships1Panel" runat="server" Visible='<%# (string)Session["RelationshipsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Relationships1HyperLink" runat="server" Text="Relationships 1" NavigateUrl="~/Relationships/Default.aspx" /></legend>
            <telerik:RadGrid ID="Relationships1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Relationships1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No relationships found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Instance" DataField="SubInstance.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SubInstanceHyperLink" runat="server" Text='<%#: Eval("SubInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SubInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Instance Relationship Type" DataField="InstanceRelationshipType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceRelationshipTypeHyperLink" runat="server" Text='<%#: Eval("InstanceRelationshipType.Name") %>' NavigateUrl='<%# $"~/RelationshipTypes/Edit.aspx?Id={Eval("InstanceRelationshipTypeId")}" %>' Enabled='<%# Session["RelationshipTypesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    <asp:Panel ID="SeriesPanel" runat="server" Visible='<%# (string)Session["SeriesPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="SeriesHyperLink" runat="server" Text="Series" NavigateUrl="~/Series/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="SeriesRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="SeriesRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No series found">
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
    <asp:Panel ID="SubjectsPanel" runat="server" Visible='<%# (string)Session["SubjectsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="SubjectsHyperLink" runat="server" Text="Subjects" NavigateUrl="~/Subjects/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="SubjectsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="SubjectsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id, InstanceId" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No subjects found">
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
    <asp:Panel ID="Title2sPanel" runat="server" Visible='<%# (string)Session["Title2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Title2sHyperLink" runat="server" Text="Titles" NavigateUrl="~/Title2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Title2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Title2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Expected Receipt Date" DataField="ExpectedReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Edition" DataField="Edition" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Package Name" DataField="PackageName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Item Number" DataField="OrderItemNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Published Date" DataField="PublishedDate" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Note" DataField="ReceivingNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription From" DataField="SubscriptionFrom" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription To" DataField="SubscriptionTo" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription Interval" DataField="SubscriptionInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Is Acknowledged" DataField="IsAcknowledged" AutoPostBackOnFilter="true" />
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="AdministrativeNotesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AdministrativeNotesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="AlternativeTitlesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AlternativeTitlesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ClassificationsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ClassificationsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ContributorsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ContributorsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="EditionsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="EditionsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ElectronicAccessesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ElectronicAccessesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Fee2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Fee2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Holding2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Holding2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="IdentifiersRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="IdentifiersPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InstanceFormat2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InstanceFormat2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InstanceNatureOfContentTermsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InstanceNatureOfContentTermsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InstanceNotesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InstanceNotesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InstanceStatisticalCodesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InstanceStatisticalCodesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InstanceTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InstanceTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LanguagesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LanguagesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PhysicalDescriptionsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PhysicalDescriptionsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PrecedingSucceedingTitle2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrecedingSucceedingTitle2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PrecedingSucceedingTitle2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrecedingSucceedingTitle2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PublicationFrequenciesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PublicationFrequenciesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PublicationRangesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PublicationRangesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PublicationsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PublicationsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RelationshipsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RelationshipsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Relationships1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Relationships1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="SeriesRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="SeriesPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="SubjectsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="SubjectsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Title2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Title2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
