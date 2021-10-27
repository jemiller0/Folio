using FolioLibrary;
using System.Collections.Generic;
using System.Web;

namespace FolioWebApplication
{
    public partial class Global : HttpApplication
    {
        private void AddPermissionsIfNecessary()
        {
            using (var fsc = new FolioServiceContext())
            {
                if (!fsc.AnyPermission2s("permissionName == \"uc.groups.edit\"")) fsc.Insert(new Permission2 { Code = "uc.groups.edit", Name = "uc.groups.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.holdings.edit\"")) fsc.Insert(new Permission2 { Code = "uc.holdings.edit", Name = "uc.holdings.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.instances.edit\"")) fsc.Insert(new Permission2 { Code = "uc.instances.edit", Name = "uc.instances.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.items.edit\"")) fsc.Insert(new Permission2 { Code = "uc.items.edit", Name = "uc.items.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.locationsettings.edit\"")) fsc.Insert(new Permission2 { Code = "uc.locationsettings.edit", Name = "uc.locationsettings.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.permissions.edit\"")) fsc.Insert(new Permission2 { Code = "uc.permissions.edit", Name = "uc.permissions.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.printers.edit\"")) fsc.Insert(new Permission2 { Code = "uc.printers.edit", Name = "uc.printers.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.settings.edit\"")) fsc.Insert(new Permission2 { Code = "uc.settings.edit", Name = "uc.settings.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.users.edit\"")) fsc.Insert(new Permission2 { Code = "uc.users.edit", Name = "uc.users.edit" });
                if (!fsc.AnyPermission2s("permissionName == \"uc.labels.edit\"")) fsc.Insert(new Permission2 { Code = "uc.labels.edit", Name = "uc.labels.edit" });
            }
        }

        private void SetPermissions(HashSet<string> roles)
        {
            SetCirculationPermissions(roles);
            SetConfigurationPermissions(roles);
            SetFeesPermissions(roles);
            SetFinancePermissions(roles);
            SetInventoryPermissions(roles);
            SetInvoicesPermissions(roles);
            SetOrdersPermissions(roles);
            SetOrganizationsPermissions(roles);
            SetPermissionsPermissions(roles);
            SetSourcePermissions(roles);
            SetTemplatesPermissions(roles);
            SetUsersPermissions(roles);
        }

        private void SetCirculationPermissions(HashSet<string> roles)
        {
            Session["CancellationReason2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.cancellationreasons.view") ? "View" : null;
            Session["CheckIn2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.checkins.view") ? "View" : null;
            Session["FixedDueDateSchedule2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.fixedduedateschedules.view") ? "View" : null;
            Session["Loan2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.loans.view") ? "View" : null;
            Session["LoanPolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.loanpolicies.view") ? "View" : null;
            Session["PatronActionSession2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronactionsessions.view") ? "View" : null;
            Session["PatronNoticePolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronnoticepolicies.view") ? "View" : null;
            Session["Request2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requests.view") ? "View" : null;
            Session["RequestPolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requestpolicies.view") ? "View" : null;
            Session["ScheduledNotice2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.schedulednotices.view") ? "View" : null;
            Session["StaffSlip2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.staffslips.view") ? "View" : null;
            Session["UserRequestPreference2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.userrequestpreferences.view") ? "View" : null;
        }

        private void SetConfigurationPermissions(HashSet<string> roles)
        {
            Session["Configuration2sPermission"] = roles.Contains("all") || roles.Contains("configuration.all") || roles.Contains("uc.configurations.view") ? "View" : null;
            Session["LocationSettingsPermission"] = roles.Contains("uc.locationsettings.edit") ? "Edit" : roles.Contains("all") || roles.Contains("configuration.all") || roles.Contains("uc.locationsettings.view") ? "View" : null;
            Session["PrintersPermission"] = roles.Contains("uc.printers.edit") ? "Edit" : roles.Contains("all") || roles.Contains("configuration.all") || roles.Contains("uc.printers.view") ? "View" : null;
            Session["SettingsPermission"] = roles.Contains("uc.settings.edit") ? "Edit" : roles.Contains("all") || roles.Contains("configuration.all") || roles.Contains("uc.settings.view") ? "View" : null;
        }

        private void SetFeesPermissions(HashSet<string> roles)
        {
            Session["Block2sPermission"] = roles.Contains("all") || roles.Contains("uc.blocks.view") ? "View" : null;
            Session["Comment2sPermission"] = roles.Contains("all") || roles.Contains("uc.comments.view") ? "View" : null;
            Session["Fee2sPermission"] = roles.Contains("all") || roles.Contains("uc.fees.view") ? "View" : null;
            Session["FeeType2sPermission"] = roles.Contains("all") || roles.Contains("uc.feetypes.view") ? "View" : null;
            Session["LostItemFeePolicy2sPermission"] = roles.Contains("all") || roles.Contains("uc.lostitemfeepolicies.view") ? "View" : null;
            Session["ManualBlockTemplate2sPermission"] = roles.Contains("all") || roles.Contains("uc.manualblocktemplates.view") ? "View" : null;
            Session["OverdueFinePolicy2sPermission"] = roles.Contains("all") || roles.Contains("uc.overduefinepolicies.view") ? "View" : null;
            Session["Owner2sPermission"] = roles.Contains("all") || roles.Contains("uc.owners.view") ? "View" : null;
            Session["Payment2sPermission"] = roles.Contains("all") || roles.Contains("uc.payments.view") ? "View" : null;
            Session["PaymentMethod2sPermission"] = roles.Contains("all") || roles.Contains("uc.paymentmethods.view") ? "View" : null;
            Session["RefundReason2sPermission"] = roles.Contains("all") || roles.Contains("uc.refundreasons.view") ? "View" : null;
            Session["TransferAccount2sPermission"] = roles.Contains("all") || roles.Contains("uc.transferaccounts.view") ? "View" : null;
            Session["TransferCriteria2sPermission"] = roles.Contains("all") || roles.Contains("uc.transfercriterias.view") ? "View" : null;
            Session["WaiveReason2sPermission"] = roles.Contains("all") || roles.Contains("uc.waivereasons.view") ? "View" : null;
        }

        private void SetFinancePermissions(HashSet<string> roles)
        {
            Session["Budget2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgets.view") ? "View" : null;
            Session["BudgetExpenseClass2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgetexpenseclasses.view") ? "View" : null;
            Session["BudgetGroup2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgetgroups.view") ? "View" : null;
            Session["ExpenseClass2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.expenseclasses.view") ? "View" : null;
            Session["FinanceGroup2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.financegroups.view") ? "View" : null;
            Session["FiscalYear2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fiscalyears.view") ? "View" : null;
            Session["Fund2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.funds.view") ? "View" : null;
            Session["FundType2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fundtypes.view") ? "View" : null;
            Session["InvoiceTransactionSummary2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.invoicetransactionsummaries.view") ? "View" : null;
            Session["Ledger2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgers.view") ? "View" : null;
            Session["LedgerRollover2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrollovers.view") ? "View" : null;
            Session["LedgerRolloverError2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrollovererrors.view") ? "View" : null;
            Session["LedgerRolloverProgress2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrolloverprogresses.view") ? "View" : null;
            Session["Transaction2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.transactions.view") ? "View" : null;
        }

        private void SetInventoryPermissions(HashSet<string> roles)
        {
            Session["AlternativeTitleType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.alternativetitletypes.view") ? "View" : null;
            Session["BoundWithPart2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.boundwithparts.view") ? "View" : null;
            Session["CallNumberType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.callnumbertypes.view") ? "View" : null;
            Session["Campus2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.campuses.view") ? "View" : null;
            Session["ClassificationType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.classificationtypes.view") ? "View" : null;
            Session["ContributorNameType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.contributornametypes.view") ? "View" : null;
            Session["ContributorType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.contributortypes.view") ? "View" : null;
            Session["ElectronicAccessRelationship2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.electronicaccessrelationships.view") ? "View" : null;
            Session["FormatsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.formats.view") ? "View" : null;
            Session["Holding2sPermission"] = roles.Contains("uc.holdings.edit") ? "Edit" : roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdings.view") ? "View" : null;
            Session["HoldingNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingnotetypes.view") ? "View" : null;
            Session["HoldingType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingtypes.view") ? "View" : null;
            Session["IdType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.idtypes.view") ? "View" : null;
            Session["IllPolicy2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.illpolicies.view") ? "View" : null;
            Session["Instance2sPermission"] = roles.Contains("uc.instances.edit") ? "Edit" : roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instances.view") ? "View" : null;
            Session["InstanceNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancenotetypes.view") ? "View" : null;
            Session["InstanceType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancetypes.view") ? "View" : null;
            Session["Institution2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.institutions.view") ? "View" : null;
            Session["IssuanceModesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.issuancemodes.view") ? "View" : null;
            Session["Item2sPermission"] = roles.Contains("uc.items.edit") ? "Edit" : roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.items.view") ? "View" : null;
            Session["ItemDamagedStatus2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemdamagedstatuses.view") ? "View" : null;
            Session["ItemNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemnotetypes.view") ? "View" : null;
            Session["Library2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.libraries.view") ? "View" : null;
            Session["LoanType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.loantypes.view") ? "View" : null;
            Session["Location2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.locations.view") ? "View" : null;
            Session["MaterialType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.materialtypes.view") ? "View" : null;
            Session["NatureOfContentTerm2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.natureofcontentterms.view") ? "View" : null;
            Session["PrecedingSucceedingTitle2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.precedingsucceedingtitles.view") ? "View" : null;
            Session["RelationshipsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.relationships.view") ? "View" : null;
            Session["RelationshipTypesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.relationshiptypes.view") ? "View" : null;
            Session["ServicePoint2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepoints.view") ? "View" : null;
            Session["ServicePointUser2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepointusers.view") ? "View" : null;
            Session["Source2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.sources.view") ? "View" : null;
            Session["StatisticalCode2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statisticalcodes.view") ? "View" : null;
            Session["StatisticalCodeType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statisticalcodetypes.view") ? "View" : null;
            Session["StatusesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statuses.view") ? "View" : null;
        }

        private void SetInvoicesPermissions(HashSet<string> roles)
        {
            Session["BatchGroup2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchgroups.view") ? "View" : null;
            Session["BatchVoucherExport2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherexports.view") ? "View" : null;
            Session["BatchVoucherExportConfig2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherexportconfigs.view") ? "View" : null;
            Session["Invoice2sPermission"] = roles.Contains("all") || roles.Contains("uc.invoices.view") ? "View" : null;
            Session["InvoiceItem2sPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitems.view") ? "View" : null;
            Session["Voucher2sPermission"] = roles.Contains("all") || roles.Contains("uc.vouchers.view") ? "View" : null;
            Session["VoucherItem2sPermission"] = roles.Contains("all") || roles.Contains("uc.voucheritems.view") ? "View" : null;
        }

        private void SetOrdersPermissions(HashSet<string> roles)
        {
            Session["AcquisitionsUnit2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.acquisitionsunits.view") ? "View" : null;
            Session["Alert2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.alerts.view") ? "View" : null;
            Session["CloseReason2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.closereasons.view") ? "View" : null;
            Session["Order2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orders.view") ? "View" : null;
            Session["OrderInvoice2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderinvoices.view") ? "View" : null;
            Session["OrderItem2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitems.view") ? "View" : null;
            Session["OrderTemplate2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.ordertemplates.view") ? "View" : null;
            Session["Prefix2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.prefixes.view") ? "View" : null;
            Session["Receiving2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.receivings.view") ? "View" : null;
            Session["ReportingCode2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.reportingcodes.view") ? "View" : null;
            Session["Suffix2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.suffixes.view") ? "View" : null;
            Session["Title2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.titles.view") ? "View" : null;
            Session["UserAcquisitionsUnit2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.useracquisitionsunits.view") ? "View" : null;
        }

        private void SetOrganizationsPermissions(HashSet<string> roles)
        {
            Session["Category2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.categories.view") ? "View" : null;
            Session["Contact2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contacts.view") ? "View" : null;
            Session["Interface2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.interfaces.view") ? "View" : null;
            Session["Organization2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizations.view") ? "View" : null;
        }

        private void SetPermissionsPermissions(HashSet<string> roles)
        {
            Session["Permission2sPermission"] = roles.Contains("uc.permissions.edit") ? "Edit" : roles.Contains("all") || roles.Contains("uc.permissions.view") ? "View" : null;
            Session["PermissionsUser2sPermission"] = roles.Contains("all") || roles.Contains("uc.permissionsusers.view") ? "View" : null;
        }

        private void SetSourcePermissions(HashSet<string> roles)
        {
            Session["Record2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.records.view") ? "View" : null;
            Session["Snapshot2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.snapshots.view") ? "View" : null;
        }

        private void SetTemplatesPermissions(HashSet<string> roles)
        {
            Session["Template2sPermission"] = roles.Contains("all") || roles.Contains("templates.all") || roles.Contains("uc.templates.view") ? "View" : null;
        }

        private void SetUsersPermissions(HashSet<string> roles)
        {
            Session["AddressType2sPermission"] = roles.Contains("all") || roles.Contains("uc.addresstypes.view") || roles.Contains("users.all") ? "View" : null;
            Session["BlockCondition2sPermission"] = roles.Contains("all") || roles.Contains("uc.blockconditions.view") || roles.Contains("users.all") ? "View" : null;
            Session["BlockLimit2sPermission"] = roles.Contains("all") || roles.Contains("uc.blocklimits.view") || roles.Contains("users.all") ? "View" : null;
            Session["CustomField2sPermission"] = roles.Contains("all") || roles.Contains("uc.customfields.view") || roles.Contains("users.all") ? "View" : null;
            Session["Department2sPermission"] = roles.Contains("all") || roles.Contains("uc.departments.view") || roles.Contains("users.all") ? "View" : null;
            Session["Group2sPermission"] = roles.Contains("uc.groups.edit") ? "Edit" : roles.Contains("all") || roles.Contains("uc.groups.view") || roles.Contains("users.all") ? "View" : null;
            Session["Proxy2sPermission"] = roles.Contains("all") || roles.Contains("uc.proxies.view") || roles.Contains("users.all") ? "View" : null;
            Session["User2sPermission"] = roles.Contains("uc.users.edit") ? "Edit" : roles.Contains("all") || roles.Contains("uc.users.view") || roles.Contains("users.all") ? "View" : null;
        }

        private void SetPermissions(string permission = null)
        {
            SetCirculationPermissions(permission);
            SetConfigurationPermissions(permission);
            SetFeesPermissions(permission);
            SetFinancePermissions(permission);
            SetInventoryPermissions(permission);
            SetInvoicesPermissions(permission);
            SetOrdersPermissions(permission);
            SetOrganizationsPermissions(permission);
            SetPermissionsPermissions(permission);
            SetSourcePermissions(permission);
            SetTemplatesPermissions(permission);
            SetUsersPermissions(permission);
        }

        private void SetCirculationPermissions(string permission = null)
        {
            Session["CancellationReason2sPermission"] = permission;
            Session["CheckIn2sPermission"] = permission;
            Session["FixedDueDateSchedule2sPermission"] = permission;
            Session["Loan2sPermission"] = permission;
            Session["LoanPolicy2sPermission"] = permission;
            Session["PatronActionSession2sPermission"] = permission;
            Session["PatronNoticePolicy2sPermission"] = permission;
            Session["Request2sPermission"] = permission;
            Session["RequestPolicy2sPermission"] = permission;
            Session["ScheduledNotice2sPermission"] = permission;
            Session["StaffSlip2sPermission"] = permission;
            Session["UserRequestPreference2sPermission"] = permission;
        }

        private void SetConfigurationPermissions(string permission = null)
        {
            Session["Configuration2sPermission"] = permission;
            Session["LocationSettingsPermission"] = permission;
            Session["PrintersPermission"] = permission;
            Session["SettingsPermission"] = permission;
        }

        private void SetFeesPermissions(string permission = null)
        {
            Session["Block2sPermission"] = permission;
            Session["Comment2sPermission"] = permission;
            Session["Fee2sPermission"] = permission;
            Session["FeeType2sPermission"] = permission;
            Session["LostItemFeePolicy2sPermission"] = permission;
            Session["ManualBlockTemplate2sPermission"] = permission;
            Session["OverdueFinePolicy2sPermission"] = permission;
            Session["Owner2sPermission"] = permission;
            Session["Payment2sPermission"] = permission;
            Session["PaymentMethod2sPermission"] = permission;
            Session["RefundReason2sPermission"] = permission;
            Session["TransferAccount2sPermission"] = permission;
            Session["TransferCriteria2sPermission"] = permission;
            Session["WaiveReason2sPermission"] = permission;
        }

        private void SetFinancePermissions(string permission = null)
        {
            Session["Budget2sPermission"] = permission;
            Session["BudgetExpenseClass2sPermission"] = permission;
            Session["BudgetGroup2sPermission"] = permission;
            Session["ExpenseClass2sPermission"] = permission;
            Session["FinanceGroup2sPermission"] = permission;
            Session["FiscalYear2sPermission"] = permission;
            Session["Fund2sPermission"] = permission;
            Session["FundType2sPermission"] = permission;
            Session["InvoiceTransactionSummary2sPermission"] = permission;
            Session["Ledger2sPermission"] = permission;
            Session["LedgerRollover2sPermission"] = permission;
            Session["LedgerRolloverError2sPermission"] = permission;
            Session["LedgerRolloverProgress2sPermission"] = permission;
            Session["Transaction2sPermission"] = permission;
        }

        private void SetInventoryPermissions(string permission = null)
        {
            Session["AlternativeTitleType2sPermission"] = permission;
            Session["BoundWithPart2sPermission"] = permission;
            Session["CallNumberType2sPermission"] = permission;
            Session["Campus2sPermission"] = permission;
            Session["ClassificationType2sPermission"] = permission;
            Session["ContributorNameType2sPermission"] = permission;
            Session["ContributorType2sPermission"] = permission;
            Session["ElectronicAccessRelationship2sPermission"] = permission;
            Session["FormatsPermission"] = permission;
            Session["Holding2sPermission"] = permission;
            Session["HoldingNoteType2sPermission"] = permission;
            Session["HoldingType2sPermission"] = permission;
            Session["IdType2sPermission"] = permission;
            Session["IllPolicy2sPermission"] = permission;
            Session["Instance2sPermission"] = permission;
            Session["InstanceNoteType2sPermission"] = permission;
            Session["InstanceType2sPermission"] = permission;
            Session["Institution2sPermission"] = permission;
            Session["IssuanceModesPermission"] = permission;
            Session["Item2sPermission"] = permission;
            Session["ItemDamagedStatus2sPermission"] = permission;
            Session["ItemNoteType2sPermission"] = permission;
            Session["Library2sPermission"] = permission;
            Session["LoanType2sPermission"] = permission;
            Session["Location2sPermission"] = permission;
            Session["MaterialType2sPermission"] = permission;
            Session["NatureOfContentTerm2sPermission"] = permission;
            Session["PrecedingSucceedingTitle2sPermission"] = permission;
            Session["RelationshipsPermission"] = permission;
            Session["RelationshipTypesPermission"] = permission;
            Session["ServicePoint2sPermission"] = permission;
            Session["ServicePointUser2sPermission"] = permission;
            Session["Source2sPermission"] = permission;
            Session["StatisticalCode2sPermission"] = permission;
            Session["StatisticalCodeType2sPermission"] = permission;
            Session["StatusesPermission"] = permission;
        }

        private void SetInvoicesPermissions(string permission = null)
        {
            Session["BatchGroup2sPermission"] = permission;
            Session["BatchVoucherExport2sPermission"] = permission;
            Session["BatchVoucherExportConfig2sPermission"] = permission;
            Session["Invoice2sPermission"] = permission;
            Session["InvoiceItem2sPermission"] = permission;
            Session["Voucher2sPermission"] = permission;
            Session["VoucherItem2sPermission"] = permission;
        }

        private void SetOrdersPermissions(string permission = null)
        {
            Session["AcquisitionsUnit2sPermission"] = permission;
            Session["Alert2sPermission"] = permission;
            Session["CloseReason2sPermission"] = permission;
            Session["Order2sPermission"] = permission;
            Session["OrderInvoice2sPermission"] = permission;
            Session["OrderItem2sPermission"] = permission;
            Session["OrderTemplate2sPermission"] = permission;
            Session["Prefix2sPermission"] = permission;
            Session["Receiving2sPermission"] = permission;
            Session["ReportingCode2sPermission"] = permission;
            Session["Suffix2sPermission"] = permission;
            Session["Title2sPermission"] = permission;
            Session["UserAcquisitionsUnit2sPermission"] = permission;
        }

        private void SetOrganizationsPermissions(string permission = null)
        {
            Session["Category2sPermission"] = permission;
            Session["Contact2sPermission"] = permission;
            Session["Interface2sPermission"] = permission;
            Session["Organization2sPermission"] = permission;
        }

        private void SetPermissionsPermissions(string permission = null)
        {
            Session["Permission2sPermission"] = permission;
            Session["PermissionsUser2sPermission"] = permission;
        }

        private void SetSourcePermissions(string permission = null)
        {
            Session["Record2sPermission"] = permission;
            Session["Snapshot2sPermission"] = permission;
        }

        private void SetTemplatesPermissions(string permission = null)
        {
            Session["Template2sPermission"] = permission;
        }

        private void SetUsersPermissions(string permission = null)
        {
            Session["AddressType2sPermission"] = permission;
            Session["BlockCondition2sPermission"] = permission;
            Session["BlockLimit2sPermission"] = permission;
            Session["CustomField2sPermission"] = permission;
            Session["Department2sPermission"] = permission;
            Session["Group2sPermission"] = permission;
            Session["Proxy2sPermission"] = permission;
            Session["User2sPermission"] = permission;
        }

        public static string TextEncode(string value)
        {
            if (value == null) return value;
            return value.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }
    }
}
