<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.ActualCostRecord2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="ActualCostRecord2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="ActualCostRecord2HyperLink" runat="server" Text="Actual Cost Record" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="ActualCostRecord2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="ActualCostRecord2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewActualCostRecord2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LossType") != null %>'>
                                <td>
                                    <asp:Label ID="LossTypeLabel" runat="server" Text="Loss Type:" AssociatedControlID="LossTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LossTypeLiteral" runat="server" Text='<%#: Eval("LossType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LossDate") != null %>'>
                                <td>
                                    <asp:Label ID="LossDateLabel" runat="server" Text="Loss Date:" AssociatedControlID="LossDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LossDateLiteral" runat="server" Text='<%# Eval("LossDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpirationDate") != null %>'>
                                <td>
                                    <asp:Label ID="ExpirationDateLabel" runat="server" Text="Expiration Date:" AssociatedControlID="ExpirationDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpirationDateLiteral" runat="server" Text='<%# Eval("ExpirationDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserBarcode") != null %>'>
                                <td>
                                    <asp:Label ID="UserBarcodeLabel" runat="server" Text="User Barcode:" AssociatedControlID="UserBarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UserBarcodeLiteral" runat="server" Text='<%#: Eval("UserBarcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserFirstName") != null %>'>
                                <td>
                                    <asp:Label ID="UserFirstNameLabel" runat="server" Text="User First Name:" AssociatedControlID="UserFirstNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UserFirstNameLiteral" runat="server" Text='<%#: Eval("UserFirstName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserLastName") != null %>'>
                                <td>
                                    <asp:Label ID="UserLastNameLabel" runat="server" Text="User Last Name:" AssociatedControlID="UserLastNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UserLastNameLiteral" runat="server" Text='<%#: Eval("UserLastName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserMiddleName") != null %>'>
                                <td>
                                    <asp:Label ID="UserMiddleNameLabel" runat="server" Text="User Middle Name:" AssociatedControlID="UserMiddleNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UserMiddleNameLiteral" runat="server" Text='<%#: Eval("UserMiddleName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserGroup") != null %>'>
                                <td>
                                    <asp:Label ID="UserGroupLabel" runat="server" Text="User Group:" AssociatedControlID="UserGroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UserGroupHyperLink" runat="server" Text='<%#: Eval("UserGroup.Name") %>' NavigateUrl='<%# $"~/Group2s/Edit.aspx?Id={Eval("UserGroupId")}" %>' Enabled='<%# Session["Group2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("UserGroupName") != null %>'>
                                <td>
                                    <asp:Label ID="UserGroupNameLabel" runat="server" Text="User Group Name:" AssociatedControlID="UserGroupNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="UserGroupNameLiteral" runat="server" Text='<%#: Eval("UserGroupName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemBarcode") != null %>'>
                                <td>
                                    <asp:Label ID="ItemBarcodeLabel" runat="server" Text="Item Barcode:" AssociatedControlID="ItemBarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemBarcodeLiteral" runat="server" Text='<%#: Eval("ItemBarcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemMaterialType") != null %>'>
                                <td>
                                    <asp:Label ID="ItemMaterialTypeLabel" runat="server" Text="Item Material Type:" AssociatedControlID="ItemMaterialTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemMaterialTypeHyperLink" runat="server" Text='<%#: Eval("ItemMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("ItemMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemMaterialTypeName") != null %>'>
                                <td>
                                    <asp:Label ID="ItemMaterialTypeNameLabel" runat="server" Text="Item Material Type Name:" AssociatedControlID="ItemMaterialTypeNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemMaterialTypeNameLiteral" runat="server" Text='<%#: Eval("ItemMaterialTypeName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemPermanentLocation") != null %>'>
                                <td>
                                    <asp:Label ID="ItemPermanentLocationLabel" runat="server" Text="Item Permanent Location:" AssociatedControlID="ItemPermanentLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemPermanentLocationHyperLink" runat="server" Text='<%#: Eval("ItemPermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemPermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemPermanentLocationName") != null %>'>
                                <td>
                                    <asp:Label ID="ItemPermanentLocationNameLabel" runat="server" Text="Item Permanent Location Name:" AssociatedControlID="ItemPermanentLocationNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemPermanentLocationNameLiteral" runat="server" Text='<%#: Eval("ItemPermanentLocationName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEffectiveLocation") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEffectiveLocationLabel" runat="server" Text="Item Effective Location:" AssociatedControlID="ItemEffectiveLocationHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemEffectiveLocationHyperLink" runat="server" Text='<%#: Eval("ItemEffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemEffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEffectiveLocationName") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEffectiveLocationNameLabel" runat="server" Text="Item Effective Location Name:" AssociatedControlID="ItemEffectiveLocationNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemEffectiveLocationNameLiteral" runat="server" Text='<%#: Eval("ItemEffectiveLocationName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemLoanType") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLoanTypeLabel" runat="server" Text="Item Loan Type:" AssociatedControlID="ItemLoanTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemLoanTypeHyperLink" runat="server" Text='<%#: Eval("ItemLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("ItemLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemLoanTypeName") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLoanTypeNameLabel" runat="server" Text="Item Loan Type Name:" AssociatedControlID="ItemLoanTypeNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemLoanTypeNameLiteral" runat="server" Text='<%#: Eval("ItemLoanTypeName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemHolding") != null %>'>
                                <td>
                                    <asp:Label ID="ItemHoldingLabel" runat="server" Text="Item Holding:" AssociatedControlID="ItemHoldingHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemHoldingHyperLink" runat="server" Text='<%# Eval("ItemHolding.ShortId") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("ItemHoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEffectiveCallNumber") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEffectiveCallNumberLabel" runat="server" Text="Item Effective Call Number:" AssociatedControlID="ItemEffectiveCallNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemEffectiveCallNumberLiteral" runat="server" Text='<%#: Eval("ItemEffectiveCallNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEffectiveCallNumberPrefix") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEffectiveCallNumberPrefixLabel" runat="server" Text="Item Effective Call Number Prefix:" AssociatedControlID="ItemEffectiveCallNumberPrefixLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemEffectiveCallNumberPrefixLiteral" runat="server" Text='<%#: Eval("ItemEffectiveCallNumberPrefix") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEffectiveCallNumberSuffix") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEffectiveCallNumberSuffixLabel" runat="server" Text="Item Effective Call Number Suffix:" AssociatedControlID="ItemEffectiveCallNumberSuffixLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemEffectiveCallNumberSuffixLiteral" runat="server" Text='<%#: Eval("ItemEffectiveCallNumberSuffix") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemVolume") != null %>'>
                                <td>
                                    <asp:Label ID="ItemVolumeLabel" runat="server" Text="Item Volume:" AssociatedControlID="ItemVolumeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemVolumeLiteral" runat="server" Text='<%#: Eval("ItemVolume") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemEnumeration") != null %>'>
                                <td>
                                    <asp:Label ID="ItemEnumerationLabel" runat="server" Text="Item Enumeration:" AssociatedControlID="ItemEnumerationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemEnumerationLiteral" runat="server" Text='<%#: Eval("ItemEnumeration") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemChronology") != null %>'>
                                <td>
                                    <asp:Label ID="ItemChronologyLabel" runat="server" Text="Item Chronology:" AssociatedControlID="ItemChronologyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemChronologyLiteral" runat="server" Text='<%#: Eval("ItemChronology") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemDisplaySummary") != null %>'>
                                <td>
                                    <asp:Label ID="ItemDisplaySummaryLabel" runat="server" Text="Item Display Summary:" AssociatedControlID="ItemDisplaySummaryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemDisplaySummaryLiteral" runat="server" Text='<%#: Eval("ItemDisplaySummary") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemCopyNumber") != null %>'>
                                <td>
                                    <asp:Label ID="ItemCopyNumberLabel" runat="server" Text="Item Copy Number:" AssociatedControlID="ItemCopyNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemCopyNumberLiteral" runat="server" Text='<%#: Eval("ItemCopyNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InstanceTitle") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceTitleLabel" runat="server" Text="Instance Title:" AssociatedControlID="InstanceTitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InstanceTitleLiteral" runat="server" Text='<%#: Eval("InstanceTitle") %>' />
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
                            <tr runat="server" visible='<%# Eval("FeeBilledAmount") != null %>'>
                                <td>
                                    <asp:Label ID="FeeBilledAmountLabel" runat="server" Text="Fee Billed Amount:" AssociatedControlID="FeeBilledAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FeeBilledAmountLiteral" runat="server" Text='<%# Eval("FeeBilledAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Owner") != null %>'>
                                <td>
                                    <asp:Label ID="OwnerLabel" runat="server" Text="Owner:" AssociatedControlID="OwnerHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("Owner.Name") %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OwnerName") != null %>'>
                                <td>
                                    <asp:Label ID="OwnerNameLabel" runat="server" Text="Owner Name:" AssociatedControlID="OwnerNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OwnerNameLiteral" runat="server" Text='<%#: Eval("OwnerName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FeeType") != null %>'>
                                <td>
                                    <asp:Label ID="FeeTypeLabel" runat="server" Text="Fee Type:" AssociatedControlID="FeeTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FeeTypeName") != null %>'>
                                <td>
                                    <asp:Label ID="FeeTypeNameLabel" runat="server" Text="Fee Type Name:" AssociatedControlID="FeeTypeNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="FeeTypeNameLiteral" runat="server" Text='<%#: Eval("FeeTypeName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLabel" runat="server" Text="Status:" AssociatedControlID="StatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLiteral" runat="server" Text='<%#: Eval("Status") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AdditionalInfoForStaff") != null %>'>
                                <td>
                                    <asp:Label ID="AdditionalInfoForStaffLabel" runat="server" Text="Additional Info For Staff:" AssociatedControlID="AdditionalInfoForStaffLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AdditionalInfoForStaffLiteral" runat="server" Text='<%#: Eval("AdditionalInfoForStaff") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AdditionalInfoForPatron") != null %>'>
                                <td>
                                    <asp:Label ID="AdditionalInfoForPatronLabel" runat="server" Text="Additional Info For Patron:" AssociatedControlID="AdditionalInfoForPatronLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AdditionalInfoForPatronLiteral" runat="server" Text='<%#: Eval("AdditionalInfoForPatron") %>' />
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
    <asp:Panel ID="ActualCostRecordContributorsPanel" runat="server" Visible='<%# (string)Session["ActualCostRecordContributorsPermission"] != null && ActualCostRecord2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ActualCostRecordContributorsHyperLink" runat="server" Text="Actual Cost Record Contributors" NavigateUrl="~/ActualCostRecordContributors/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ActualCostRecordContributorsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ActualCostRecordContributorsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No actual cost record contributors found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Name" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="ActualCostRecordIdentifiersPanel" runat="server" Visible='<%# (string)Session["ActualCostRecordIdentifiersPermission"] != null && ActualCostRecord2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ActualCostRecordIdentifiersHyperLink" runat="server" Text="Actual Cost Record Identifiers" NavigateUrl="~/ActualCostRecordIdentifiers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="ActualCostRecordIdentifiersRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ActualCostRecordIdentifiersRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No actual cost record identifiers found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Identifier Type" DataField="IdentifierType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Identifier Type 1" DataField="IdentifierType1.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdentifierType1HyperLink" runat="server" Text='<%#: Eval("IdentifierType1.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("IdentifierTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ActualCostRecordContributorsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ActualCostRecordContributorsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ActualCostRecordIdentifiersRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ActualCostRecordIdentifiersPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
