<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Group2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Group2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Group2HyperLink" runat="server" Text="Group" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Group2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Group2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewGroup2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("ExpirationOffsetInDays") != null %>'>
                                <td>
                                    <asp:Label ID="ExpirationOffsetInDaysLabel" runat="server" Text="Expiration Offset In Days:" AssociatedControlID="ExpirationOffsetInDaysLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpirationOffsetInDaysLiteral" runat="server" Text='<%#: Eval("ExpirationOffsetInDays") %>' />
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
    <asp:Panel ID="ActualCostRecord2sPanel" runat="server" Visible='<%# (string)Session["ActualCostRecord2sPermission"] != null && Group2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="ActualCostRecord2sHyperLink" runat="server" Text="Actual Cost Records" NavigateUrl="~/ActualCostRecord2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="ActualCostRecord2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="ActualCostRecord2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No actual cost records found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/ActualCostRecord2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/ActualCostRecord2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Loss Type" DataField="LossType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loss Date" DataField="LossDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Expiration Date" DataField="ExpirationDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="User Barcode" DataField="UserBarcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="User First Name" DataField="UserFirstName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="User Last Name" DataField="UserLastName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="User Middle Name" DataField="UserMiddleName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="User Group Name" DataField="UserGroupName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Barcode" DataField="ItemBarcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item Material Type" DataField="ItemMaterialType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemMaterialTypeHyperLink" runat="server" Text='<%#: Eval("ItemMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("ItemMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Item Material Type Name" DataField="ItemMaterialTypeName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item Permanent Location" DataField="ItemPermanentLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemPermanentLocationHyperLink" runat="server" Text='<%#: Eval("ItemPermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemPermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Item Permanent Location Name" DataField="ItemPermanentLocationName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item Effective Location" DataField="ItemEffectiveLocation.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemEffectiveLocationHyperLink" runat="server" Text='<%#: Eval("ItemEffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemEffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Item Effective Location Name" DataField="ItemEffectiveLocationName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item Loan Type" DataField="ItemLoanType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemLoanTypeHyperLink" runat="server" Text='<%#: Eval("ItemLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("ItemLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Item Loan Type Name" DataField="ItemLoanTypeName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Item Holding" DataField="ItemHolding.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHoldingHyperLink" runat="server" Text='<%# Eval("ItemHoldingId") != null ? Eval("ItemHolding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("ItemHoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Item Effective Call Number" DataField="ItemEffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Effective Call Number Prefix" DataField="ItemEffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Effective Call Number Suffix" DataField="ItemEffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Volume" DataField="ItemVolume" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Enumeration" DataField="ItemEnumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Chronology" DataField="ItemChronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Display Summary" DataField="ItemDisplaySummary" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Copy Number" DataField="ItemCopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Instance Title" DataField="InstanceTitle" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Fee" DataField="Fee.Title" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeHyperLink" runat="server" Text='<%#: Eval("FeeId") != null ? Eval("Fee.Title") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("FeeId")}" %>' Enabled='<%# Session["Fee2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Fee Billed Amount" DataField="FeeBilledAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn HeaderText="Owner" DataField="Owner.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("OwnerId") != null ? Eval("Owner.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Owner Name" DataField="OwnerName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Fee Type" DataField="FeeType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Fee Type Name" DataField="FeeTypeName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Additional Info For Staff" DataField="AdditionalInfoForStaff" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Additional Info For Patron" DataField="AdditionalInfoForPatron" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="BlockLimit2sPanel" runat="server" Visible='<%# (string)Session["BlockLimit2sPermission"] != null && Group2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="BlockLimit2sHyperLink" runat="server" Text="Block Limits" NavigateUrl="~/BlockLimit2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="BlockLimit2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="BlockLimit2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No block limits found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/BlockLimit2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/settings/users/limits/{Eval("GroupId")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/BlockLimit2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Condition" DataField="Condition.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ConditionHyperLink" runat="server" Text='<%#: Eval("Condition.Name") %>' NavigateUrl='<%# $"~/BlockCondition2s/Edit.aspx?Id={Eval("ConditionId")}" %>' Enabled='<%# Session["BlockCondition2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" AutoPostBackOnFilter="true" />
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
    <asp:Panel ID="Loan2sPanel" runat="server" Visible='<%# (string)Session["Loan2sPermission"] != null && Group2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Loan2sHyperLink" runat="server" Text="Loans" NavigateUrl="~/Loan2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Loan2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Loan2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No loans found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/users/{Eval("UserId")}/loans/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proxy User" DataField="ProxyUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProxyUserHyperLink" runat="server" Text='<%#: Eval("ProxyUserId") != null ? Eval("ProxyUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ProxyUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" DataField="Item.ShortId" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Int32">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Effective Location At Check Out" DataField="ItemEffectiveLocationAtCheckOut.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemEffectiveLocationAtCheckOutHyperLink" runat="server" Text='<%#: Eval("ItemEffectiveLocationAtCheckOut.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("ItemEffectiveLocationAtCheckOutId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Loan Time" DataField="LoanTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Return Time" DataField="ReturnTime" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="System Return Time" DataField="SystemReturnTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Action" DataField="Action" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Action Comment" DataField="ActionComment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status" DataField="ItemStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Renewal Count" DataField="RenewalCount" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Loan Policy" DataField="LoanPolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanPolicyHyperLink" runat="server" Text='<%#: Eval("LoanPolicy.Name") %>' NavigateUrl='<%# $"~/LoanPolicy2s/Edit.aspx?Id={Eval("LoanPolicyId")}" %>' Enabled='<%# Session["LoanPolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checkout Service Point" DataField="CheckoutServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckoutServicePointHyperLink" runat="server" Text='<%#: Eval("CheckoutServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckoutServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checkin Service Point" DataField="CheckinServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CheckinServicePointHyperLink" runat="server" Text='<%#: Eval("CheckinServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("CheckinServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Due Date Changed By Recall" DataField="DueDateChangedByRecall" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Is Dcb" DataField="IsDcb" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Declared Lost Date" DataField="DeclaredLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Claimed Returned Date" DataField="ClaimedReturnedDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Overdue Fine Policy" DataField="OverdueFinePolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OverdueFinePolicyHyperLink" runat="server" Text='<%#: Eval("OverdueFinePolicy.Name") %>' NavigateUrl='<%# $"~/OverdueFinePolicy2s/Edit.aspx?Id={Eval("OverdueFinePolicyId")}" %>' Enabled='<%# Session["OverdueFinePolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Lost Item Policy" DataField="LostItemPolicy.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LostItemPolicyHyperLink" runat="server" Text='<%#: Eval("LostItemPolicy.Name") %>' NavigateUrl='<%# $"~/LostItemFeePolicy2s/Edit.aspx?Id={Eval("LostItemPolicyId")}" %>' Enabled='<%# Session["LostItemFeePolicy2sPermission"] != null %>' />
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
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Lost Item Has Been Billed" DataField="AgedToLostDelayedBillingLostItemHasBeenBilled" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Date Lost Item Should Be Billed" DataField="AgedToLostDelayedBillingDateLostItemShouldBeBilled" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Aged To Lost Delayed Billing Aged To Lost Date" DataField="AgedToLostDelayedBillingAgedToLostDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Reminders Last Fee Billed Number" DataField="RemindersLastFeeBilledNumber" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Reminders Last Fee Billed Date" DataField="RemindersLastFeeBilledDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="User2sPanel" runat="server" Visible='<%# (string)Session["User2sPermission"] != null && Group2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="User2sHyperLink" runat="server" Text="Users" NavigateUrl="~/User2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="User2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="User2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No users found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditViewHyperLink" Text='<%# Session["User2sPermission"] %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/users/preview/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Username" DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UsernameHyperLink" runat="server" Text='<%#: Eval("Username") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="External System Id" DataField="ExternalSystemId" SortExpression="ExternalSystemId" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExternalSystemIdHyperLink" runat="server" Text='<%#: Eval("ExternalSystemId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Barcode" DataField="Barcode" SortExpression="Barcode" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BarcodeHyperLink" runat="server" Text='<%#: Eval("Barcode") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Active" DataField="Active" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" AllowFiltering="false" AllowSorting="false" HtmlEncode="true" />
                        <telerik:GridBoundColumn HeaderText="Last Name" DataField="LastName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="First Name" DataField="FirstName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Middle Name" DataField="MiddleName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Preferred First Name" DataField="PreferredFirstName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridHyperLinkColumn HeaderText="Email Address" DataTextField="EmailAddress" DataNavigateUrlFormatString="mailto:{0}" DataNavigateUrlFields="EmailAddress" SortExpression="EmailAddress" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridHyperLinkColumn HeaderText="Phone Number" DataTextField="PhoneNumber" DataNavigateUrlFormatString="tel:{0}" DataNavigateUrlFields="PhoneNumber" SortExpression="PhoneNumber" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridHyperLinkColumn HeaderText="Mobile Phone Number" DataTextField="MobilePhoneNumber" DataNavigateUrlFormatString="tel:{0}" DataNavigateUrlFields="MobilePhoneNumber" SortExpression="MobilePhoneNumber" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Birth Date" DataField="BirthDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Preferred Contact Type" DataField="PreferredContactType" AutoPostBackOnFilter="true" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Profile Picture Link" DataField="ProfilePictureLink" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Start Date" DataField="StartDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="End Date" DataField="EndDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Category Code" DataField="CategoryCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Category" DataField="Category" AutoPostBackOnFilter="true" AllowSorting="false" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Statuses" DataField="Statuses" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Status" DataField="StaffStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Privileges" DataField="StaffPrivileges" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Division" DataField="StaffDivision" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Staff Department" DataField="StaffDepartment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Student Id" DataField="StudentId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Student Status" DataField="StudentStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Student Restriction" DataField="StudentRestriction" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Student Division" DataField="StudentDivision" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Student Department" DataField="StudentDepartment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Deceased" DataField="Deceased" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Collections" DataField="Collections" AutoPostBackOnFilter="true" />
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
            <telerik:AjaxSetting AjaxControlID="ActualCostRecord2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ActualCostRecord2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BlockLimit2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BlockLimit2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Loan2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Loan2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="User2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="User2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
