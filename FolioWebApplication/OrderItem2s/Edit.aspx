<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.OrderItem2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="OrderItem2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderItem2HyperLink" runat="server" Text="Order Item" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="OrderItem2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="OrderItem2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewOrderItem2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Edition") != null %>'>
                                <td>
                                    <asp:Label ID="EditionLabel" runat="server" Text="Edition:" AssociatedControlID="EditionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EditionLiteral" runat="server" Text='<%#: Eval("Edition") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CheckinItems") != null %>'>
                                <td>
                                    <asp:Label ID="CheckinItemsLabel" runat="server" Text="Checkin Items:" AssociatedControlID="CheckinItemsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CheckinItemsLiteral" runat="server" Text='<%#: Eval("CheckinItems") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AgreementId") != null %>'>
                                <td>
                                    <asp:Label ID="AgreementIdLabel" runat="server" Text="Agreement Id:" AssociatedControlID="AgreementIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AgreementIdLiteral" runat="server" Text='<%#: Eval("AgreementId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AcquisitionMethod") != null %>'>
                                <td>
                                    <asp:Label ID="AcquisitionMethodLabel" runat="server" Text="Acquisition Method:" AssociatedControlID="AcquisitionMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AcquisitionMethodLiteral" runat="server" Text='<%#: Eval("AcquisitionMethod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancellationRestriction") != null %>'>
                                <td>
                                    <asp:Label ID="CancellationRestrictionLabel" runat="server" Text="Cancellation Restriction:" AssociatedControlID="CancellationRestrictionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CancellationRestrictionLiteral" runat="server" Text='<%#: Eval("CancellationRestriction") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancellationRestrictionNote") != null %>'>
                                <td>
                                    <asp:Label ID="CancellationRestrictionNoteLabel" runat="server" Text="Cancellation Restriction Note:" AssociatedControlID="CancellationRestrictionNoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CancellationRestrictionNoteLiteral" runat="server" Text='<%#: Eval("CancellationRestrictionNote") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Collection") != null %>'>
                                <td>
                                    <asp:Label ID="CollectionLabel" runat="server" Text="Collection:" AssociatedControlID="CollectionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CollectionLiteral" runat="server" Text='<%#: Eval("Collection") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalUnitListPrice") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalUnitListPriceLabel" runat="server" Text="Physical Unit List Price:" AssociatedControlID="PhysicalUnitListPriceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PhysicalUnitListPriceLiteral" runat="server" Text='<%# Eval("PhysicalUnitListPrice", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ElectronicUnitListPrice") != null %>'>
                                <td>
                                    <asp:Label ID="ElectronicUnitListPriceLabel" runat="server" Text="Electronic Unit List Price:" AssociatedControlID="ElectronicUnitListPriceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ElectronicUnitListPriceLiteral" runat="server" Text='<%# Eval("ElectronicUnitListPrice", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Currency") != null %>'>
                                <td>
                                    <asp:Label ID="CurrencyLabel" runat="server" Text="Currency:" AssociatedControlID="CurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CurrencyLiteral" runat="server" Text='<%#: Eval("Currency") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AdditionalCost") != null %>'>
                                <td>
                                    <asp:Label ID="AdditionalCostLabel" runat="server" Text="Additional Cost:" AssociatedControlID="AdditionalCostLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AdditionalCostLiteral" runat="server" Text='<%# Eval("AdditionalCost", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Discount") != null %>'>
                                <td>
                                    <asp:Label ID="DiscountLabel" runat="server" Text="Discount:" AssociatedControlID="DiscountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscountLiteral" runat="server" Text='<%#: Eval("Discount") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DiscountType") != null %>'>
                                <td>
                                    <asp:Label ID="DiscountTypeLabel" runat="server" Text="Discount Type:" AssociatedControlID="DiscountTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscountTypeLiteral" runat="server" Text='<%#: Eval("DiscountType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalQuantity") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalQuantityLabel" runat="server" Text="Physical Quantity:" AssociatedControlID="PhysicalQuantityLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PhysicalQuantityLiteral" runat="server" Text='<%#: Eval("PhysicalQuantity") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ElectronicQuantity") != null %>'>
                                <td>
                                    <asp:Label ID="ElectronicQuantityLabel" runat="server" Text="Electronic Quantity:" AssociatedControlID="ElectronicQuantityLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ElectronicQuantityLiteral" runat="server" Text='<%#: Eval("ElectronicQuantity") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EstimatedPrice") != null %>'>
                                <td>
                                    <asp:Label ID="EstimatedPriceLabel" runat="server" Text="Estimated Price:" AssociatedControlID="EstimatedPriceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EstimatedPriceLiteral" runat="server" Text='<%# Eval("EstimatedPrice", "{0:c}") %>' />
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
                            <tr runat="server" visible='<%# Eval("ReceivingNote") != null %>'>
                                <td>
                                    <asp:Label ID="ReceivingNoteLabel" runat="server" Text="Receiving Note:" AssociatedControlID="ReceivingNoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceivingNoteLiteral" runat="server" Text='<%#: Eval("ReceivingNote") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionFrom") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionFromLabel" runat="server" Text="Subscription From:" AssociatedControlID="SubscriptionFromLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionFromLiteral" runat="server" Text='<%# Eval("SubscriptionFrom", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionInterval") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionIntervalLabel" runat="server" Text="Subscription Interval:" AssociatedControlID="SubscriptionIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionIntervalLiteral" runat="server" Text='<%#: Eval("SubscriptionInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionTo") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionToLabel" runat="server" Text="Subscription To:" AssociatedControlID="SubscriptionToLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionToLiteral" runat="server" Text='<%# Eval("SubscriptionTo", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Donor") != null %>'>
                                <td>
                                    <asp:Label ID="DonorLabel" runat="server" Text="Donor:" AssociatedControlID="DonorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DonorLiteral" runat="server" Text='<%#: Eval("Donor") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceActivated") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceActivatedLabel" runat="server" Text="Eresource Activated:" AssociatedControlID="EresourceActivatedLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceActivatedLiteral" runat="server" Text='<%#: Eval("EresourceActivated") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceActivationDue") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceActivationDueLabel" runat="server" Text="Eresource Activation Due:" AssociatedControlID="EresourceActivationDueLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceActivationDueLiteral" runat="server" Text='<%#: Eval("EresourceActivationDue") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceCreateInventory") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceCreateInventoryLabel" runat="server" Text="Eresource Create Inventory:" AssociatedControlID="EresourceCreateInventoryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceCreateInventoryLiteral" runat="server" Text='<%#: Eval("EresourceCreateInventory") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceTrial") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceTrialLabel" runat="server" Text="Eresource Trial:" AssociatedControlID="EresourceTrialLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceTrialLiteral" runat="server" Text='<%#: Eval("EresourceTrial") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceExpectedActivationDate") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceExpectedActivationDateLabel" runat="server" Text="Eresource Expected Activation Date:" AssociatedControlID="EresourceExpectedActivationDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceExpectedActivationDateLiteral" runat="server" Text='<%# Eval("EresourceExpectedActivationDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceUserLimit") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceUserLimitLabel" runat="server" Text="Eresource User Limit:" AssociatedControlID="EresourceUserLimitLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceUserLimitLiteral" runat="server" Text='<%#: Eval("EresourceUserLimit") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceAccessProvider") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceAccessProviderLabel" runat="server" Text="Eresource Access Provider:" AssociatedControlID="EresourceAccessProviderHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="EresourceAccessProviderHyperLink" runat="server" Text='<%#: Eval("EresourceAccessProvider.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("EresourceAccessProviderId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceLicenseCode") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceLicenseCodeLabel" runat="server" Text="Eresource License Code:" AssociatedControlID="EresourceLicenseCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceLicenseCodeLiteral" runat="server" Text='<%#: Eval("EresourceLicenseCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceLicenseDescription") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceLicenseDescriptionLabel" runat="server" Text="Eresource License Description:" AssociatedControlID="EresourceLicenseDescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceLicenseDescriptionLiteral" runat="server" Text='<%#: Eval("EresourceLicenseDescription") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceLicenseReference") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceLicenseReferenceLabel" runat="server" Text="Eresource License Reference:" AssociatedControlID="EresourceLicenseReferenceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EresourceLicenseReferenceLiteral" runat="server" Text='<%#: Eval("EresourceLicenseReference") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceMaterialType") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceMaterialTypeLabel" runat="server" Text="Eresource Material Type:" AssociatedControlID="EresourceMaterialTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="EresourceMaterialTypeHyperLink" runat="server" Text='<%#: Eval("EresourceMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("EresourceMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EresourceResourceUrl") != null %>'>
                                <td>
                                    <asp:Label ID="EresourceResourceUrlLabel" runat="server" Text="Eresource Resource URL:" AssociatedControlID="EresourceResourceUrlHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="EresourceResourceUrlHyperLink" runat="server" Text='<%#: Eval("EresourceResourceUrl") %>' NavigateUrl='<%#: Eval("EresourceResourceUrl") %>' Target="_blank" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Instance") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceLabel" runat="server" Text="Instance:" AssociatedControlID="InstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IsPackage") != null %>'>
                                <td>
                                    <asp:Label ID="IsPackageLabel" runat="server" Text="Is Package:" AssociatedControlID="IsPackageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="IsPackageLiteral" runat="server" Text='<%#: Eval("IsPackage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderFormat") != null %>'>
                                <td>
                                    <asp:Label ID="OrderFormatLabel" runat="server" Text="Order Format:" AssociatedControlID="OrderFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderFormatLiteral" runat="server" Text='<%#: Eval("OrderFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PackageOrderItem") != null %>'>
                                <td>
                                    <asp:Label ID="PackageOrderItemLabel" runat="server" Text="Package Order Item:" AssociatedControlID="PackageOrderItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PackageOrderItemHyperLink" runat="server" Text='<%#: Eval("PackageOrderItem.Number") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("PackageOrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentStatus") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentStatusLabel" runat="server" Text="Payment Status:" AssociatedControlID="PaymentStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentStatusLiteral" runat="server" Text='<%#: Eval("PaymentStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalCreateInventory") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalCreateInventoryLabel" runat="server" Text="Physical Create Inventory:" AssociatedControlID="PhysicalCreateInventoryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PhysicalCreateInventoryLiteral" runat="server" Text='<%#: Eval("PhysicalCreateInventory") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalMaterialType") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalMaterialTypeLabel" runat="server" Text="Physical Material Type:" AssociatedControlID="PhysicalMaterialTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PhysicalMaterialTypeHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("PhysicalMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalMaterialSupplier") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalMaterialSupplierLabel" runat="server" Text="Physical Material Supplier:" AssociatedControlID="PhysicalMaterialSupplierHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PhysicalMaterialSupplierHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialSupplier.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("PhysicalMaterialSupplierId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalExpectedReceiptDate") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalExpectedReceiptDateLabel" runat="server" Text="Physical Expected Receipt Date:" AssociatedControlID="PhysicalExpectedReceiptDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PhysicalExpectedReceiptDateLiteral" runat="server" Text='<%# Eval("PhysicalExpectedReceiptDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PhysicalReceiptDue") != null %>'>
                                <td>
                                    <asp:Label ID="PhysicalReceiptDueLabel" runat="server" Text="Physical Receipt Due:" AssociatedControlID="PhysicalReceiptDueLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PhysicalReceiptDueLiteral" runat="server" Text='<%# Eval("PhysicalReceiptDue", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Description2") != null %>'>
                                <td>
                                    <asp:Label ID="Description2Label" runat="server" Text="Description 2:" AssociatedControlID="Description2Literal" />
                                </td>
                                <td>
                                    <asp:Literal ID="Description2Literal" runat="server" Text='<%#: Eval("Description2") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Number") != null %>'>
                                <td>
                                    <asp:Label ID="NumberLabel" runat="server" Text="Number:" AssociatedControlID="NumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NumberLiteral" runat="server" Text='<%#: Eval("Number") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationYear") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationYearLabel" runat="server" Text="Publication Year:" AssociatedControlID="PublicationYearLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationYearLiteral" runat="server" Text='<%#: Eval("PublicationYear") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Publisher") != null %>'>
                                <td>
                                    <asp:Label ID="PublisherLabel" runat="server" Text="Publisher:" AssociatedControlID="PublisherLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublisherLiteral" runat="server" Text='<%#: Eval("Publisher") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Order") != null %>'>
                                <td>
                                    <asp:Label ID="OrderLabel" runat="server" Text="Order:" AssociatedControlID="OrderHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceiptDate") != null %>'>
                                <td>
                                    <asp:Label ID="ReceiptDateLabel" runat="server" Text="Receipt Date:" AssociatedControlID="ReceiptDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceiptDateLiteral" runat="server" Text='<%# Eval("ReceiptDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReceiptStatus") != null %>'>
                                <td>
                                    <asp:Label ID="ReceiptStatusLabel" runat="server" Text="Receipt Status:" AssociatedControlID="ReceiptStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReceiptStatusLiteral" runat="server" Text='<%#: Eval("ReceiptStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Requester") != null %>'>
                                <td>
                                    <asp:Label ID="RequesterLabel" runat="server" Text="Requester:" AssociatedControlID="RequesterLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RequesterLiteral" runat="server" Text='<%#: Eval("Requester") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Rush") != null %>'>
                                <td>
                                    <asp:Label ID="RushLabel" runat="server" Text="Rush:" AssociatedControlID="RushLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RushLiteral" runat="server" Text='<%#: Eval("Rush") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Selector") != null %>'>
                                <td>
                                    <asp:Label ID="SelectorLabel" runat="server" Text="Selector:" AssociatedControlID="SelectorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SelectorLiteral" runat="server" Text='<%#: Eval("Selector") %>' />
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
                            <tr runat="server" visible='<%# Eval("TitleOrPackage") != null %>'>
                                <td>
                                    <asp:Label ID="TitleOrPackageLabel" runat="server" Text="Title Or Package:" AssociatedControlID="TitleOrPackageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TitleOrPackageLiteral" runat="server" Text='<%#: Eval("TitleOrPackage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorInstructions") != null %>'>
                                <td>
                                    <asp:Label ID="VendorInstructionsLabel" runat="server" Text="Vendor Instructions:" AssociatedControlID="VendorInstructionsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorInstructionsLiteral" runat="server" Text='<%#: Eval("VendorInstructions") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorNote") != null %>'>
                                <td>
                                    <asp:Label ID="VendorNoteLabel" runat="server" Text="Vendor Note:" AssociatedControlID="VendorNoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorNoteLiteral" runat="server" Text='<%#: Eval("VendorNote") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorReferenceNumber") != null %>'>
                                <td>
                                    <asp:Label ID="VendorReferenceNumberLabel" runat="server" Text="Vendor Reference Number:" AssociatedControlID="VendorReferenceNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorReferenceNumberLiteral" runat="server" Text='<%#: Eval("VendorReferenceNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorReferenceNumberType") != null %>'>
                                <td>
                                    <asp:Label ID="VendorReferenceNumberTypeLabel" runat="server" Text="Vendor Reference Number Type:" AssociatedControlID="VendorReferenceNumberTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorReferenceNumberTypeLiteral" runat="server" Text='<%#: Eval("VendorReferenceNumberType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorAccount") != null %>'>
                                <td>
                                    <asp:Label ID="VendorAccountLabel" runat="server" Text="Vendor Account:" AssociatedControlID="VendorAccountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorAccountLiteral" runat="server" Text='<%#: Eval("VendorAccount") %>' />
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
    <asp:Panel ID="InvoiceItem2sPanel" runat="server" Visible='<%# (string)Session["InvoiceItem2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceItem2sHyperLink" runat="server" Text="Invoice Items" NavigateUrl="~/InvoiceItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="InvoiceItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InvoiceItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Adjustments Total" DataField="AdjustmentsTotal" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Invoice" DataField="Invoice.Number" SortExpression="Invoice.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Line Status" DataField="InvoiceLineStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Product Id" DataField="ProductId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Product Id Type" DataField="ProductIdType.Name" SortExpression="ProductIdType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProductIdTypeHyperLink" runat="server" Text='<%#: Eval("ProductIdType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("ProductIdTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Quantity" DataField="Quantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Release Encumbrance" DataField="ReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Subscription Info" DataField="SubscriptionInfo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription Start" DataField="SubscriptionStart" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription End" DataField="SubscriptionEnd" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Sub Total" DataField="SubTotal" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Total" DataField="Total" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Vendor Ref No" DataField="VendorRefNo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="Item2sPanel" runat="server" Visible='<%# (string)Session["Item2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Item2sHyperLink" runat="server" Text="Items" NavigateUrl="~/Item2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Item2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Item2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No items found">
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditViewHyperLink" Text='<%# Session["Item2sPermission"] %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Holding" DataField="Holding.ShortId" SortExpression="Holding.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("HoldingId") != null ? Eval("Holding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Call Number Type" DataField="CallNumberType.Name" SortExpression="CallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Effective Call Number" DataField="EffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Prefix" DataField="EffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Effective Call Number Suffix" DataField="EffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Call Number Type" DataField="EffectiveCallNumberType.Name" SortExpression="EffectiveCallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Damaged Status" DataField="DamagedStatus.Name" SortExpression="DamagedStatus.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="DamagedStatusHyperLink" runat="server" Text='<%#: Eval("DamagedStatus.Name") %>' NavigateUrl='<%# $"~/ItemDamagedStatus2s/Edit.aspx?Id={Eval("DamagedStatusId")}" %>' Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Damaged Status Time" DataField="DamagedStatusTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Status Date" DataField="StatusDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Material Type" DataField="MaterialType.Name" SortExpression="MaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialTypeHyperLink" runat="server" Text='<%#: Eval("MaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Loan Type" DataField="PermanentLoanType.Name" SortExpression="PermanentLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLoanTypeHyperLink" runat="server" Text='<%#: Eval("PermanentLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("PermanentLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Loan Type" DataField="TemporaryLoanType.Name" SortExpression="TemporaryLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLoanTypeHyperLink" runat="server" Text='<%#: Eval("TemporaryLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("TemporaryLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Location" DataField="PermanentLocation.Name" SortExpression="PermanentLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PermanentLocationHyperLink" runat="server" Text='<%#: Eval("PermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("PermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Location" DataField="TemporaryLocation.Name" SortExpression="TemporaryLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Location" DataField="EffectiveLocation.Name" SortExpression="EffectiveLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="In Transit Destination Service Point" DataField="InTransitDestinationServicePoint.Name" SortExpression="InTransitDestinationServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InTransitDestinationServicePointHyperLink" runat="server" Text='<%#: Eval("InTransitDestinationServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("InTransitDestinationServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                        <telerik:GridBoundColumn HeaderText="Last Check In Date Time" DataField="LastCheckInDateTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Service Point" DataField="LastCheckInServicePoint.Name" SortExpression="LastCheckInServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInServicePointHyperLink" runat="server" Text='<%#: Eval("LastCheckInServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("LastCheckInServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Staff Member" DataField="LastCheckInStaffMember.Username" SortExpression="LastCheckInStaffMember.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%#: Eval("LastCheckInStaffMemberId") != null ? Eval("LastCheckInStaffMember.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastCheckInStaffMemberId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="OrderItem2sPanel" runat="server" Visible='<%# (string)Session["OrderItem2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Edition" DataField="Edition" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Checkin Items" DataField="CheckinItems" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Agreement Id" DataField="AgreementId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Method" DataField="AcquisitionMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction" DataField="CancellationRestriction" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction Note" DataField="CancellationRestrictionNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Collection" DataField="Collection" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Physical Unit List Price" DataField="PhysicalUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Electronic Unit List Price" DataField="ElectronicUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Additional Cost" DataField="AdditionalCost" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Discount" DataField="Discount" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Discount Type" DataField="DiscountType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Physical Quantity" DataField="PhysicalQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Electronic Quantity" DataField="ElectronicQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Estimated Price" DataField="EstimatedPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Eresource Access Provider" DataField="EresourceAccessProvider.Name" SortExpression="EresourceAccessProvider.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceAccessProviderHyperLink" runat="server" Text='<%#: Eval("EresourceAccessProvider.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("EresourceAccessProviderId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Eresource License Code" DataField="EresourceLicenseCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Description" DataField="EresourceLicenseDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Reference" DataField="EresourceLicenseReference" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Eresource Material Type" DataField="EresourceMaterialType.Name" SortExpression="EresourceMaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceMaterialTypeHyperLink" runat="server" Text='<%#: Eval("EresourceMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("EresourceMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridHyperLinkColumn HeaderText="Eresource Resource URL" DataTextField="EresourceResourceUrl" DataNavigateUrlFields="EresourceResourceUrl" Target="_blank" SortExpression="EresourceResourceUrl" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance" DataField="Instance.Title" SortExpression="Instance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Is Package" DataField="IsPackage" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Order Format" DataField="OrderFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Status" DataField="PaymentStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Physical Create Inventory" DataField="PhysicalCreateInventory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Physical Material Type" DataField="PhysicalMaterialType.Name" SortExpression="PhysicalMaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialTypeHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("PhysicalMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Physical Material Supplier" DataField="PhysicalMaterialSupplier.Name" SortExpression="PhysicalMaterialSupplier.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialSupplierHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialSupplier.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("PhysicalMaterialSupplierId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Physical Expected Receipt Date" DataField="PhysicalExpectedReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Physical Receipt Due" DataField="PhysicalReceiptDue" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Description 2" DataField="Description2" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Publication Year" DataField="PublicationYear" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order" DataField="Order.Number" SortExpression="Order.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Receipt Date" DataField="ReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Receipt Status" DataField="ReceiptStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester" DataField="Requester" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Rush" DataField="Rush" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Selector" DataField="Selector" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Title Or Package" DataField="TitleOrPackage" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Instructions" DataField="VendorInstructions" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Note" DataField="VendorNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Reference Number" DataField="VendorReferenceNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Reference Number Type" DataField="VendorReferenceNumberType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Account" DataField="VendorAccount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="Receiving2sPanel" runat="server" Visible='<%# (string)Session["Receiving2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Receiving2sHyperLink" runat="server" Text="Receivings" NavigateUrl="~/Receiving2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Receiving2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Receiving2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No receivings found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="ReceiveTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Receiving2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Receiving2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Caption" DataField="Caption" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Format" DataField="Format" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Item" DataField="Item.ShortId" SortExpression="Item.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Location" DataField="Location.Name" SortExpression="Location.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Title" DataField="Title.Title" SortExpression="Title.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title.Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("TitleId")}" %>' Enabled='<%# Session["Title2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Receiving Status" DataField="ReceivingStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Supplement" DataField="Supplement" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Receipt Time" DataField="ReceiptTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Receive Time" DataField="ReceiveTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Title2sPanel" runat="server" Visible='<%# (string)Session["Title2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance" DataField="Instance.Title" SortExpression="Instance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
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
    <asp:Panel ID="Transaction2sPanel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && OrderItem2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2sHyperLink" runat="server" Text="Transactions" NavigateUrl="~/Transaction2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Transaction2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Transaction2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transactions found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="AmountHyperLink" runat="server" Text='<%# $"{Eval("Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Awaiting Payment Encumbrance" DataField="AwaitingPaymentEncumbrance.Amount" SortExpression="AwaitingPaymentEncumbrance.Amount" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="AwaitingPaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("AwaitingPaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("AwaitingPaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Release Encumbrance" DataField="AwaitingPaymentReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Amount" DataField="AwaitingPaymentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Expended Amount" DataField="ExpendedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Initial Encumbered Amount" DataField="InitialEncumberedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription" DataField="Subscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Re Encumber" DataField="ReEncumber" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order" DataField="Order.Number" SortExpression="Order.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Expense Class" DataField="ExpenseClass.Name" SortExpression="ExpenseClass.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Fiscal Year" DataField="FiscalYear.Name" SortExpression="FiscalYear.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="From Fund" DataField="FromFund.Name" SortExpression="FromFund.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Payment Encumbrance" DataField="PaymentEncumbrance.Amount" SortExpression="PaymentEncumbrance.Amount" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("PaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Source Fiscal Year" DataField="SourceFiscalYear.Name" SortExpression="SourceFiscalYear.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceFiscalYearHyperLink" runat="server" Text='<%#: Eval("SourceFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("SourceFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Invoice" DataField="Invoice.Number" SortExpression="Invoice.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Invoice Item" DataField="InvoiceItem.Number" SortExpression="InvoiceItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItemId") != null ? Eval("InvoiceItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="To Fund" DataField="ToFund.Name" SortExpression="ToFund.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Transaction Type" DataField="TransactionType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
            <telerik:AjaxSetting AjaxControlID="InvoiceItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Item2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Item2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Receiving2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Receiving2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Title2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Title2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
