<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Settings.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="SettingPanel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="SettingHyperLink" runat="server" Text="Setting" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="SettingFormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["SettingsPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="SettingFormView_DataBinding" OnItemUpdating="SettingFormView_ItemUpdating" OnItemDeleting="SettingFormView_ItemDeleting"
                OnItemCommand="SettingFormView_ItemCommand" Enabled='<%# true || (string)Session["SettingsPermission"] == "Edit" %>'>
                <ItemTemplate>
                    <asp:Panel ID="ViewSettingPanel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Name") != null %>'>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NameLiteral" runat="server" Text='<%#: Eval("Name") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Orientation") != null %>'>
                                <td>
                                    <asp:Label ID="OrientationLabel" runat="server" Text="Orientation:" AssociatedControlID="OrientationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrientationLiteral" runat="server" Text='<%#: Eval("Orientation") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FontFamily") != null %>'>
                                <td>
                                    <asp:Label ID="FontFamilyLabel" runat="server" Text="Font Family:" AssociatedControlID="FontFamilyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FontFamilyLiteral" runat="server" Text='<%#: Eval("FontFamily") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FontSize") != null %>'>
                                <td>
                                    <asp:Label ID="FontSizeLabel" runat="server" Text="Font Size:" AssociatedControlID="FontSizeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FontSizeLiteral" runat="server" Text='<%#: Eval("FontSize") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FontWeight") != null %>'>
                                <td>
                                    <asp:Label ID="FontWeightLabel" runat="server" Text="Font Weight:" AssociatedControlID="FontWeightLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FontWeightLiteral" runat="server" Text='<%#: Eval("FontWeight") %>' />
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
                    <asp:Panel ID="EditSettingPanel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="SettingValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Setting" Visible="false" />
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="NameRadTextBox" runat="server" Text='<%# Bind("Name") %>' MaxLength="1024" Width="500px" />
                                    <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameRadTextBox" ErrorMessage="The Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="OrientationLabel" runat="server" Text="Orientation:" AssociatedControlID="OrientationRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="OrientationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" AllowCustomText="true" Text='<%# Bind("Orientation") %>' OnDataBinding="OrientationRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="OrientationRequiredFieldValidator" runat="server" ControlToValidate="OrientationRadComboBox" ErrorMessage="The Orientation field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontFamilyLabel" runat="server" Text="Font Family:" AssociatedControlID="FontFamilyRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="FontFamilyRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" SelectedValue='<%# Bind("FontFamily") %>' OnDataBinding="FontFamilyRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="FontFamilyRequiredFieldValidator" runat="server" ControlToValidate="FontFamilyRadComboBox" ErrorMessage="The Font Family field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                    <asp:RegularExpressionValidator ID="FontFamilyRegularExpressionValidator" runat="server" ErrorMessage="The Font Family field must match the regular expression '^(Agency FB|Algerian|Arial|Arial Black|Arial Narrow|Arial Rounded MT Bold|Arial Unicode MS|Bahnschrift|Bahnschrift Condensed|Bahnschrift Light|Bahnschrift Light Condensed|Bahnschrift Light SemiCondensed|Bahnschrift SemiBold|Bahnschrift SemiBold Condensed|Bahnschrift SemiBold SemiConden|Bahnschrift SemiCondensed|Bahnschrift SemiLight|Bahnschrift SemiLight Condensed|Bahnschrift SemiLight SemiConde|BarCode|Baskerville Old Face|Bauhaus 93|Bell MT|Berlin Sans FB|Berlin Sans FB Demi|Bernard MT Condensed|Blackadder ITC|Bodoni MT|Bodoni MT Black|Bodoni MT Condensed|Bodoni MT Poster Compressed|Book Antiqua|Bookman Old Style|Bookshelf Symbol 7|Bradley Hand ITC|Britannic Bold|Broadway|Brush Script MT|Calibri|Calibri Light|Californian FB|Calisto MT|Cambria|Cambria Math|Candara|Candara Light|Cascadia Code|Cascadia Code ExtraLight|Cascadia Code Light|Cascadia Code SemiBold|Cascadia Code SemiLight|Cascadia Mono|Cascadia Mono ExtraLight|Cascadia Mono Light|Cascadia Mono SemiBold|Cascadia Mono SemiLight|Castellar|Centaur|Century|Century Gothic|Century Schoolbook|Chiller|Colonna MT|Comic Sans MS|Consolas|Constantia|Cooper Black|Copperplate Gothic Bold|Copperplate Gothic Light|Corbel|Corbel Light|Courier New|Curlz MT|Dubai|Dubai Light|Dubai Medium|Ebrima|Edwardian Script ITC|Elephant|Engravers MT|Eras Bold ITC|Eras Demi ITC|Eras Light ITC|Eras Medium ITC|Felix Titling|Footlight MT Light|Forte|Franklin Gothic Book|Franklin Gothic Demi|Franklin Gothic Demi Cond|Franklin Gothic Heavy|Franklin Gothic Medium|Franklin Gothic Medium Cond|Freestyle Script|French Script MT|Gabriola|Gadugi|Garamond|Georgia|Gigi|Gill Sans MT|Gill Sans MT Condensed|Gill Sans MT Ext Condensed Bold|Gill Sans Ultra Bold|Gill Sans Ultra Bold Condensed|Gloucester MT Extra Condensed|Goudy Old Style|Goudy Stout|Haettenschweiler|Harlow Solid Italic|Harrington|High Tower Text|HoloLens MDL2 Assets|Impact|Imprint MT Shadow|Informal Roman|Ink Free|Javanese Text|Jokerman|Juice ITC|Kristen ITC|Kunstler Script|Leelawadee|Leelawadee UI|Leelawadee UI Semilight|Lucida Bright|Lucida Calligraphy|Lucida Console|Lucida Fax|Lucida Handwriting|Lucida Sans|Lucida Sans Typewriter|Lucida Sans Unicode|Magneto|Maiandra GD|Malgun Gothic|Malgun Gothic Semilight|Marlett|Matura MT Script Capitals|Microsoft Himalaya|Microsoft JhengHei|Microsoft JhengHei Light|Microsoft JhengHei UI|Microsoft JhengHei UI Light|Microsoft New Tai Lue|Microsoft PhagsPa|Microsoft Sans Serif|Microsoft Tai Le|Microsoft Uighur|Microsoft YaHei|Microsoft YaHei Light|Microsoft YaHei UI|Microsoft YaHei UI Light|Microsoft Yi Baiti|MingLiU-ExtB|MingLiU_HKSCS-ExtB|Mistral|Modern No. 20|Mongolian Baiti|Monotype Corsiva|MS Gothic|MS Outlook|MS PGothic|MS Reference Sans Serif|MS Reference Specialty|MS UI Gothic|MT Extra|MV Boli|Myanmar Text|Niagara Engraved|Niagara Solid|Nirmala UI|Nirmala UI Semilight|NSimSun|OCR A Extended|Old English Text MT|Onyx|Palace Script MT|Palatino Linotype|Papyrus|Parchment|Perpetua|Perpetua Titling MT|Playbill|PMingLiU-ExtB|Poor Richard|Pristina|Rage Italic|Ravie|Rockwell|Rockwell Condensed|Rockwell Extra Bold|Script MT Bold|Segoe MDL2 Assets|Segoe Print|Segoe Script|Segoe UI|Segoe UI Black|Segoe UI Emoji|Segoe UI Historic|Segoe UI Light|Segoe UI Semibold|Segoe UI Semilight|Segoe UI Symbol|Showcard Gothic|SimSun|SimSun-ExtB|Sitka Banner|Sitka Display|Sitka Heading|Sitka Small|Sitka Subheading|Sitka Text|Snap ITC|Stencil|Sylfaen|Symbol|Tahoma|Tempus Sans ITC|Times New Roman|Trebuchet MS|Tw Cen MT|Tw Cen MT Condensed|Tw Cen MT Condensed Extra Bold|Verdana|Viner Hand ITC|Vivaldi|Vladimir Script|Webdings|Wide Latin|Wingdings|Wingdings 2|Wingdings 3|Yu Gothic|Yu Gothic Light|Yu Gothic Medium|Yu Gothic UI|Yu Gothic UI Light|Yu Gothic UI Semibold|Yu Gothic UI Semilight)$'." ControlToValidate="FontFamilyRadComboBox" Display="Dynamic" CssClass="Error" ValidationExpression="^(Agency FB|Algerian|Arial|Arial Black|Arial Narrow|Arial Rounded MT Bold|Arial Unicode MS|Bahnschrift|Bahnschrift Condensed|Bahnschrift Light|Bahnschrift Light Condensed|Bahnschrift Light SemiCondensed|Bahnschrift SemiBold|Bahnschrift SemiBold Condensed|Bahnschrift SemiBold SemiConden|Bahnschrift SemiCondensed|Bahnschrift SemiLight|Bahnschrift SemiLight Condensed|Bahnschrift SemiLight SemiConde|BarCode|Baskerville Old Face|Bauhaus 93|Bell MT|Berlin Sans FB|Berlin Sans FB Demi|Bernard MT Condensed|Blackadder ITC|Bodoni MT|Bodoni MT Black|Bodoni MT Condensed|Bodoni MT Poster Compressed|Book Antiqua|Bookman Old Style|Bookshelf Symbol 7|Bradley Hand ITC|Britannic Bold|Broadway|Brush Script MT|Calibri|Calibri Light|Californian FB|Calisto MT|Cambria|Cambria Math|Candara|Candara Light|Cascadia Code|Cascadia Code ExtraLight|Cascadia Code Light|Cascadia Code SemiBold|Cascadia Code SemiLight|Cascadia Mono|Cascadia Mono ExtraLight|Cascadia Mono Light|Cascadia Mono SemiBold|Cascadia Mono SemiLight|Castellar|Centaur|Century|Century Gothic|Century Schoolbook|Chiller|Colonna MT|Comic Sans MS|Consolas|Constantia|Cooper Black|Copperplate Gothic Bold|Copperplate Gothic Light|Corbel|Corbel Light|Courier New|Curlz MT|Dubai|Dubai Light|Dubai Medium|Ebrima|Edwardian Script ITC|Elephant|Engravers MT|Eras Bold ITC|Eras Demi ITC|Eras Light ITC|Eras Medium ITC|Felix Titling|Footlight MT Light|Forte|Franklin Gothic Book|Franklin Gothic Demi|Franklin Gothic Demi Cond|Franklin Gothic Heavy|Franklin Gothic Medium|Franklin Gothic Medium Cond|Freestyle Script|French Script MT|Gabriola|Gadugi|Garamond|Georgia|Gigi|Gill Sans MT|Gill Sans MT Condensed|Gill Sans MT Ext Condensed Bold|Gill Sans Ultra Bold|Gill Sans Ultra Bold Condensed|Gloucester MT Extra Condensed|Goudy Old Style|Goudy Stout|Haettenschweiler|Harlow Solid Italic|Harrington|High Tower Text|HoloLens MDL2 Assets|Impact|Imprint MT Shadow|Informal Roman|Ink Free|Javanese Text|Jokerman|Juice ITC|Kristen ITC|Kunstler Script|Leelawadee|Leelawadee UI|Leelawadee UI Semilight|Lucida Bright|Lucida Calligraphy|Lucida Console|Lucida Fax|Lucida Handwriting|Lucida Sans|Lucida Sans Typewriter|Lucida Sans Unicode|Magneto|Maiandra GD|Malgun Gothic|Malgun Gothic Semilight|Marlett|Matura MT Script Capitals|Microsoft Himalaya|Microsoft JhengHei|Microsoft JhengHei Light|Microsoft JhengHei UI|Microsoft JhengHei UI Light|Microsoft New Tai Lue|Microsoft PhagsPa|Microsoft Sans Serif|Microsoft Tai Le|Microsoft Uighur|Microsoft YaHei|Microsoft YaHei Light|Microsoft YaHei UI|Microsoft YaHei UI Light|Microsoft Yi Baiti|MingLiU-ExtB|MingLiU_HKSCS-ExtB|Mistral|Modern No. 20|Mongolian Baiti|Monotype Corsiva|MS Gothic|MS Outlook|MS PGothic|MS Reference Sans Serif|MS Reference Specialty|MS UI Gothic|MT Extra|MV Boli|Myanmar Text|Niagara Engraved|Niagara Solid|Nirmala UI|Nirmala UI Semilight|NSimSun|OCR A Extended|Old English Text MT|Onyx|Palace Script MT|Palatino Linotype|Papyrus|Parchment|Perpetua|Perpetua Titling MT|Playbill|PMingLiU-ExtB|Poor Richard|Pristina|Rage Italic|Ravie|Rockwell|Rockwell Condensed|Rockwell Extra Bold|Script MT Bold|Segoe MDL2 Assets|Segoe Print|Segoe Script|Segoe UI|Segoe UI Black|Segoe UI Emoji|Segoe UI Historic|Segoe UI Light|Segoe UI Semibold|Segoe UI Semilight|Segoe UI Symbol|Showcard Gothic|SimSun|SimSun-ExtB|Sitka Banner|Sitka Display|Sitka Heading|Sitka Small|Sitka Subheading|Sitka Text|Snap ITC|Stencil|Sylfaen|Symbol|Tahoma|Tempus Sans ITC|Times New Roman|Trebuchet MS|Tw Cen MT|Tw Cen MT Condensed|Tw Cen MT Condensed Extra Bold|Verdana|Viner Hand ITC|Vivaldi|Vladimir Script|Webdings|Wide Latin|Wingdings|Wingdings 2|Wingdings 3|Yu Gothic|Yu Gothic Light|Yu Gothic Medium|Yu Gothic UI|Yu Gothic UI Light|Yu Gothic UI Semibold|Yu Gothic UI Semilight)$" ValidationGroup="Setting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontSizeLabel" runat="server" Text="Font Size:" AssociatedControlID="FontSizeRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="FontSizeRadNumericTextBox" runat="server" DbValue='<%# Bind("FontSize") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                    <asp:RequiredFieldValidator ID="FontSizeRequiredFieldValidator" runat="server" ControlToValidate="FontSizeRadNumericTextBox" ErrorMessage="The Font Size field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FontWeightLabel" runat="server" Text="Font Weight:" AssociatedControlID="FontWeightRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="FontWeightRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" AllowCustomText="true" Text='<%# Bind("FontWeight") %>' OnDataBinding="FontWeightRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="FontWeightRequiredFieldValidator" runat="server" ControlToValidate="FontWeightRadComboBox" ErrorMessage="The Font Weight field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
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
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# SettingFormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="Setting" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this setting?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# SettingFormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Setting cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                    <asp:CustomValidator ID="SettingCustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Setting" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="LocationSettingsPanel" runat="server" Visible='<%# (string)Session["LocationSettingsPermission"] != null && SettingFormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="LocationSettingsHyperLink" runat="server" Text="Location Settings" NavigateUrl="~/LocationSettings/Default.aspx" /></legend>
            <telerik:RadGrid ID="LocationSettingsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="LocationSettingsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No location settings found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditViewHyperLink" Text='<%# Session["LocationSettingsPermission"] %>' NavigateUrl='<%# $"~/LocationSettings/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" DataField="Location.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Enabled" DataField="Enabled" AutoPostBackOnFilter="true" />
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
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="SettingPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="LocationSettingsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LocationSettingsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
