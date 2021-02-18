<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.LocationSettings.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="LocationSettingPanel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="LocationSettingHyperLink" runat="server" Text="Location Setting" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="LocationSettingFormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["LocationSettingsPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="LocationSettingFormView_DataBinding" OnItemUpdating="LocationSettingFormView_ItemUpdating" OnItemDeleting="LocationSettingFormView_ItemDeleting"
                OnItemCommand="LocationSettingFormView_ItemCommand" Enabled='<%# true || (string)Session["LocationSettingsPermission"] == "Edit" %>'>
                <ItemTemplate>
                    <asp:Panel ID="ViewLocationSettingPanel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Location") != null %>'>
                                <td>
                                    <asp:Label ID="LocationLabel" runat="server" Text="Location:" AssociatedControlID="LocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Settings") != null %>'>
                                <td>
                                    <asp:Label ID="SettingsLabel" runat="server" Text="Settings:" AssociatedControlID="SettingsHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SettingsHyperLink" runat="server" Text='<%#: Eval("Settings.Name") %>' NavigateUrl='<%# $"~/Settings/Edit.aspx?Id={Eval("SettingsId")}" %>' Enabled='<%# Session["SettingsPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Enabled") != null %>'>
                                <td>
                                    <asp:Label ID="EnabledLabel" runat="server" Text="Enabled:" AssociatedControlID="EnabledLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EnabledLiteral" runat="server" Text='<%#: Eval("Enabled") %>' />
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
                        </table>
                    </asp:Panel>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Panel ID="EditLocationSettingPanel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="LocationSettingValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="LocationSetting" Visible="false" />
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="LocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("LocationId") %>' OnDataBinding="LocationRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="LocationRequiredFieldValidator" runat="server" ControlToValidate="LocationRadComboBox" ErrorMessage="The Location field is required." Display="Dynamic" CssClass="Error" ValidationGroup="LocationSetting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Settings:" NavigateUrl="~/Settings/Default.aspx" Enabled='<%# Session["SettingsPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="SettingsRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("SettingsId") %>' OnDataBinding="SettingsRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="SettingsRequiredFieldValidator" runat="server" ControlToValidate="SettingsRadComboBox" ErrorMessage="The Settings field is required." Display="Dynamic" CssClass="Error" ValidationGroup="LocationSetting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="EnabledRadCheckBox" runat="server" Text="Enabled" Checked='<%# Bind("Enabled") %>' AutoPostBack="false" />
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
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# LocationSettingFormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="LocationSetting" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this location setting?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# LocationSettingFormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Location Setting cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="LocationSetting" />
                                    <asp:CustomValidator ID="LocationSettingCustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="LocationSetting" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LocationSettingPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
