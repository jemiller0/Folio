<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Permission2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Permission2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Permission2HyperLink" runat="server" Text="Permission" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Permission2FormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["Permission2sPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="Permission2FormView_DataBinding" OnItemUpdating="Permission2FormView_ItemUpdating" OnItemDeleting="Permission2FormView_ItemDeleting"
                OnItemCommand="Permission2FormView_ItemCommand" Enabled='<%# true || (string)Session["Permission2sPermission"] == "Edit" %>'>
                <ItemTemplate>
                    <asp:Panel ID="ViewPermission2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Code") != null %>'>
                                <td>
                                    <asp:Label ID="CodeLabel" runat="server" Text="Code:" AssociatedControlID="CodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CodeLiteral" runat="server" Text='<%#: Eval("Code") %>' />
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
                            <tr runat="server" visible='<%# Eval("Editable") != null %>'>
                                <td>
                                    <asp:Label ID="EditableLabel" runat="server" Text="Editable:" AssociatedControlID="EditableLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EditableLiteral" runat="server" Text='<%#: Eval("Editable") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Visible") != null %>'>
                                <td>
                                    <asp:Label ID="VisibleLabel" runat="server" Text="Visible:" AssociatedControlID="VisibleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VisibleLiteral" runat="server" Text='<%#: Eval("Visible") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Dummy") != null %>'>
                                <td>
                                    <asp:Label ID="DummyLabel" runat="server" Text="Dummy:" AssociatedControlID="DummyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DummyLiteral" runat="server" Text='<%#: Eval("Dummy") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Deprecated") != null %>'>
                                <td>
                                    <asp:Label ID="DeprecatedLabel" runat="server" Text="Deprecated:" AssociatedControlID="DeprecatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DeprecatedLiteral" runat="server" Text='<%#: Eval("Deprecated") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ModuleName") != null %>'>
                                <td>
                                    <asp:Label ID="ModuleNameLabel" runat="server" Text="Module Name:" AssociatedControlID="ModuleNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ModuleNameLiteral" runat="server" Text='<%#: Eval("ModuleName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ModuleVersion") != null %>'>
                                <td>
                                    <asp:Label ID="ModuleVersionLabel" runat="server" Text="Module Version:" AssociatedControlID="ModuleVersionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ModuleVersionLiteral" runat="server" Text='<%#: Eval("ModuleVersion") %>' />
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
                <EditItemTemplate>
                    <asp:Panel ID="EditPermission2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="Permission2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Permission2" Visible="false" />
                        </div>
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Permission2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CodeLabel" runat="server" Text="Code:" AssociatedControlID="CodeRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CodeRadTextBox" runat="server" Text='<%# Bind("Code") %>' MaxLength="1024" Width="500px" />
                                    <asp:RequiredFieldValidator ID="CodeRequiredFieldValidator" runat="server" ControlToValidate="CodeRadTextBox" ErrorMessage="The Code field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Permission2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="NameRadTextBox" runat="server" Text='<%# Bind("Name") %>' MaxLength="1024" Width="500px" />
                                    <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameRadTextBox" ErrorMessage="The Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Permission2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="DescriptionRadTextBox" runat="server" Text='<%# Bind("Description") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="EditableRadCheckBox" runat="server" Text="Editable" Checked='<%# Bind("Editable") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="VisibleRadCheckBox" runat="server" Text="Visible" Checked='<%# Bind("Visible") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="DummyRadCheckBox" runat="server" Text="Dummy" Checked='<%# Bind("Dummy") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="DeprecatedRadCheckBox" runat="server" Text="Deprecated" Checked='<%# Bind("Deprecated") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ModuleNameLabel" runat="server" Text="Module Name:" AssociatedControlID="ModuleNameRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ModuleNameRadTextBox" runat="server" Text='<%# Bind("ModuleName") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ModuleVersionLabel" runat="server" Text="Module Version:" AssociatedControlID="ModuleVersionRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ModuleVersionRadTextBox" runat="server" Text='<%# Bind("ModuleVersion") %>' MaxLength="1024" Width="500px" />
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
                                    <asp:HyperLink runat="server" Text="Creation User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%# Eval("CreationUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("CreationUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
                                    <asp:HyperLink runat="server" Text="Last Write User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%# Eval("LastWriteUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastWriteUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
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
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# Permission2FormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="Permission2" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this permission?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# Permission2FormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Permission cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="Permission2" />
                                    <asp:CustomValidator ID="Permission2CustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Permission2" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PermissionChildOfsPanel" runat="server" Visible='<%# (string)Session["PermissionChildOfsPermission"] != null && Permission2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PermissionChildOfsHyperLink" runat="server" Text="Permission Child Ofs" NavigateUrl="~/PermissionChildOfs/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PermissionChildOfsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PermissionChildOfsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No permission child ofs found">
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
    <asp:Panel ID="PermissionGrantedTosPanel" runat="server" Visible='<%# (string)Session["PermissionGrantedTosPermission"] != null && Permission2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PermissionGrantedTosHyperLink" runat="server" Text="Permission Granted Tos" NavigateUrl="~/PermissionGrantedTos/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PermissionGrantedTosRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PermissionGrantedTosRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No permission granted tos found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Permissions User" DataField="PermissionsUser.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermissionsUserHyperLink" runat="server" Text='<%# Eval("PermissionsUser.Id") %>' NavigateUrl='<%# $"~/PermissionsUser2s/Edit.aspx?Id={Eval("PermissionsUserId")}" %>' Enabled='<%# Session["PermissionsUser2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PermissionSubPermissionsPanel" runat="server" Visible='<%# (string)Session["PermissionSubPermissionsPermission"] != null && Permission2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PermissionSubPermissionsHyperLink" runat="server" Text="Permission Sub Permissions" NavigateUrl="~/PermissionSubPermissions/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PermissionSubPermissionsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PermissionSubPermissionsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No permission sub permissions found">
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
    <asp:Panel ID="PermissionTagsPanel" runat="server" Visible='<%# (string)Session["PermissionTagsPermission"] != null && Permission2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PermissionTagsHyperLink" runat="server" Text="Permission Tags" NavigateUrl="~/PermissionTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="PermissionTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PermissionTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No permission tags found">
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Permission2Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PermissionChildOfsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PermissionChildOfsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PermissionGrantedTosRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PermissionGrantedTosPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PermissionSubPermissionsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PermissionSubPermissionsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PermissionTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PermissionTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
