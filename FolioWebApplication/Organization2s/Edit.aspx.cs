using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Organization2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Organization2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Organization2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var o2 = folioServiceContext.FindOrganization2(id, true);
            if (o2 == null) Response.Redirect("Default.aspx");
            o2.Content = o2.Content != null ? JsonConvert.DeserializeObject<JToken>(o2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Organization2FormView.DataSource = new[] { o2 };
            Title = $"Organization {o2.Name}";
        }

        protected void CurrenciesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CurrenciesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).Currencies ?? new Currency[] { };
            CurrenciesRadGrid.DataSource = l;
            CurrenciesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            CurrenciesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["CurrenciesPermission"] == "Edit" || Session["CurrenciesPermission"] != null && l.Any());
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(out var i, Global.GetCqlFilter(Invoice2sRadGrid, d, $"vendorId == \"{id}\""), Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = i;
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2sRadGrid, d, $"vendor == \"{id}\""), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2sRadGrid, d, $"eresource.accessProvider == \"{id}\""), OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrderItem2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethod", "acquisitionMethod" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OrderItem2s1RadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, Global.GetCqlFilter(OrderItem2s1RadGrid, d, $"physical.materialSupplier == \"{id}\""), OrderItem2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2s1RadGrid.PageSize * OrderItem2s1RadGrid.CurrentPageIndex, OrderItem2s1RadGrid.PageSize, true);
            OrderItem2s1RadGrid.VirtualItemCount = i;
            if (OrderItem2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2s1RadGrid.AllowFilteringByColumn = OrderItem2s1RadGrid.VirtualItemCount > 10;
                OrderItem2s1Panel.Visible = Organization2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void OrganizationAccountsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAccountsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationAccounts ?? new OrganizationAccount[] { };
            OrganizationAccountsRadGrid.DataSource = l;
            OrganizationAccountsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAccountsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAccountsPermission"] == "Edit" || Session["OrganizationAccountsPermission"] != null && l.Any());
        }

        protected void OrganizationAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationAcquisitionsUnits ?? new OrganizationAcquisitionsUnit[] { };
            OrganizationAcquisitionsUnitsRadGrid.DataSource = l;
            OrganizationAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAcquisitionsUnitsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAcquisitionsUnitsPermission"] == "Edit" || Session["OrganizationAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void OrganizationAddressesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAddressesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationAddresses ?? new OrganizationAddress[] { };
            OrganizationAddressesRadGrid.DataSource = l;
            OrganizationAddressesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAddressesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAddressesPermission"] == "Edit" || Session["OrganizationAddressesPermission"] != null && l.Any());
        }

        protected void OrganizationAgreementsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAgreementsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationAgreements ?? new OrganizationAgreement[] { };
            OrganizationAgreementsRadGrid.DataSource = l;
            OrganizationAgreementsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAgreementsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAgreementsPermission"] == "Edit" || Session["OrganizationAgreementsPermission"] != null && l.Any());
        }

        protected void OrganizationAliasesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationAliasesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationAliases ?? new OrganizationAlias[] { };
            OrganizationAliasesRadGrid.DataSource = l;
            OrganizationAliasesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationAliasesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationAliasesPermission"] == "Edit" || Session["OrganizationAliasesPermission"] != null && l.Any());
        }

        protected void OrganizationChangelogsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationChangelogsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationChangelogs ?? new OrganizationChangelog[] { };
            OrganizationChangelogsRadGrid.DataSource = l;
            OrganizationChangelogsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationChangelogsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationChangelogsPermission"] == "Edit" || Session["OrganizationChangelogsPermission"] != null && l.Any());
        }

        protected void OrganizationContactsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationContactsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationContacts ?? new OrganizationContact[] { };
            OrganizationContactsRadGrid.DataSource = l;
            OrganizationContactsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationContactsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationContactsPermission"] == "Edit" || Session["OrganizationContactsPermission"] != null && l.Any());
        }

        protected void OrganizationEmailsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationEmailsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationEmails ?? new OrganizationEmail[] { };
            OrganizationEmailsRadGrid.DataSource = l;
            OrganizationEmailsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationEmailsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationEmailsPermission"] == "Edit" || Session["OrganizationEmailsPermission"] != null && l.Any());
        }

        protected void OrganizationInterfacesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationInterfacesPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationInterfaces ?? new OrganizationInterface[] { };
            OrganizationInterfacesRadGrid.DataSource = l;
            OrganizationInterfacesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationInterfacesPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationInterfacesPermission"] == "Edit" || Session["OrganizationInterfacesPermission"] != null && l.Any());
        }

        protected void OrganizationPhoneNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationPhoneNumbersPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationPhoneNumbers ?? new OrganizationPhoneNumber[] { };
            OrganizationPhoneNumbersRadGrid.DataSource = l;
            OrganizationPhoneNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationPhoneNumbersPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationPhoneNumbersPermission"] == "Edit" || Session["OrganizationPhoneNumbersPermission"] != null && l.Any());
        }

        protected void OrganizationTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationTagsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationTags ?? new OrganizationTag[] { };
            OrganizationTagsRadGrid.DataSource = l;
            OrganizationTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationTagsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationTagsPermission"] == "Edit" || Session["OrganizationTagsPermission"] != null && l.Any());
        }

        protected void OrganizationUrlsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrganizationUrlsPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOrganization2(id).OrganizationUrls ?? new OrganizationUrl[] { };
            OrganizationUrlsRadGrid.DataSource = l;
            OrganizationUrlsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            OrganizationUrlsPanel.Visible = Organization2FormView.DataKey.Value != null && ((string)Session["OrganizationUrlsPermission"] == "Edit" || Session["OrganizationUrlsPermission"] != null && l.Any());
        }

        protected void Voucher2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Voucher2sPermission"] == null) return;
            var id = (Guid?)Organization2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AccountNumber", "accountNo" }, { "Amount", "amount" }, { "BatchGroupId", "batchGroupId" }, { "DisbursementNumber", "disbursementNumber" }, { "DisbursementDate", "disbursementDate" }, { "DisbursementAmount", "disbursementAmount" }, { "Enclosure", "enclosureNeeded" }, { "InvoiceCurrency", "invoiceCurrency" }, { "InvoiceId", "invoiceId" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "SystemCurrency", "systemCurrency" }, { "Type", "type" }, { "VoucherDate", "voucherDate" }, { "Number", "voucherNumber" }, { "VendorId", "vendorId" }, { "VendorStreetAddress1", "vendorAddress.addressLine1" }, { "VendorStreetAddress2", "vendorAddress.addressLine2" }, { "VendorCity", "vendorAddress.city" }, { "VendorState", "vendorAddress.stateRegion" }, { "VendorPostalCode", "vendorAddress.zipCode" }, { "VendorCountryCode", "vendorAddress.country" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Voucher2sRadGrid.DataSource = folioServiceContext.Voucher2s(out var i, Global.GetCqlFilter(Voucher2sRadGrid, d, $"vendorId == \"{id}\""), Voucher2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Voucher2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Voucher2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Voucher2sRadGrid.PageSize * Voucher2sRadGrid.CurrentPageIndex, Voucher2sRadGrid.PageSize, true);
            Voucher2sRadGrid.VirtualItemCount = i;
            if (Voucher2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Voucher2sRadGrid.AllowFilteringByColumn = Voucher2sRadGrid.VirtualItemCount > 10;
                Voucher2sPanel.Visible = Organization2FormView.DataKey.Value != null && Session["Voucher2sPermission"] != null && Voucher2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
