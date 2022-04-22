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
            SetLoginPermissions(roles);
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
            Session["CirculationRule2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.circulationrules.view") ? "View" : null;
            Session["FixedDueDateSchedule2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.fixedduedateschedules.view") ? "View" : null;
            Session["FixedDueDateScheduleSchedulesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.fixedduedatescheduleschedules.view") ? "View" : null;
            Session["Loan2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.loans.view") ? "View" : null;
            Session["LoanEvent2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.loanevents.view") ? "View" : null;
            Session["LoanPolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.loanpolicies.view") ? "View" : null;
            Session["PatronActionSession2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronactionsessions.view") ? "View" : null;
            Session["PatronNoticePolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronnoticepolicies.view") ? "View" : null;
            Session["PatronNoticePolicyFeeFineNoticesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronnoticepolicyfeefinenotices.view") ? "View" : null;
            Session["PatronNoticePolicyLoanNoticesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronnoticepolicyloannotices.view") ? "View" : null;
            Session["PatronNoticePolicyRequestNoticesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.patronnoticepolicyrequestnotices.view") ? "View" : null;
            Session["Request2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requests.view") ? "View" : null;
            Session["RequestIdentifiersPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requestidentifiers.view") ? "View" : null;
            Session["RequestNotesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requestnotes.view") ? "View" : null;
            Session["RequestPolicy2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requestpolicies.view") ? "View" : null;
            Session["RequestPolicyRequestTypesPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requestpolicyrequesttypes.view") ? "View" : null;
            Session["RequestTagsPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.requesttags.view") ? "View" : null;
            Session["ScheduledNotice2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.schedulednotices.view") ? "View" : null;
            Session["StaffSlip2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.staffslips.view") ? "View" : null;
            Session["UserRequestPreference2sPermission"] = roles.Contains("all") || roles.Contains("circulation.all") || roles.Contains("uc.userrequestpreferences.view") ? "View" : null;
        }

        private void SetConfigurationPermissions(HashSet<string> roles)
        {
            Session["AddressesPermission"] = roles.Contains("all") || roles.Contains("configuration.all") || roles.Contains("uc.addresses.view") ? "View" : null;
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
            Session["ServicePointOwnersPermission"] = roles.Contains("all") || roles.Contains("uc.servicepointowners.view") ? "View" : null;
            Session["TransferAccount2sPermission"] = roles.Contains("all") || roles.Contains("uc.transferaccounts.view") ? "View" : null;
            Session["TransferCriteria2sPermission"] = roles.Contains("all") || roles.Contains("uc.transfercriterias.view") ? "View" : null;
            Session["WaiveReason2sPermission"] = roles.Contains("all") || roles.Contains("uc.waivereasons.view") ? "View" : null;
        }

        private void SetFinancePermissions(HashSet<string> roles)
        {
            Session["AllocatedFromFundsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.allocatedfromfunds.view") ? "View" : null;
            Session["AllocatedToFundsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.allocatedtofunds.view") ? "View" : null;
            Session["Budget2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgets.view") ? "View" : null;
            Session["BudgetAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgetacquisitionsunits.view") ? "View" : null;
            Session["BudgetExpenseClass2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgetexpenseclasses.view") ? "View" : null;
            Session["BudgetGroup2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgetgroups.view") ? "View" : null;
            Session["BudgetTagsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.budgettags.view") ? "View" : null;
            Session["ExpenseClass2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.expenseclasses.view") ? "View" : null;
            Session["FinanceGroup2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.financegroups.view") ? "View" : null;
            Session["FinanceGroupAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.financegroupacquisitionsunits.view") ? "View" : null;
            Session["FiscalYear2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fiscalyears.view") ? "View" : null;
            Session["FiscalYearAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fiscalyearacquisitionsunits.view") ? "View" : null;
            Session["Fund2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.funds.view") ? "View" : null;
            Session["FundAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fundacquisitionsunits.view") ? "View" : null;
            Session["FundTagsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fundtags.view") ? "View" : null;
            Session["FundType2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.fundtypes.view") ? "View" : null;
            Session["Ledger2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgers.view") ? "View" : null;
            Session["LedgerAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgeracquisitionsunits.view") ? "View" : null;
            Session["LedgerRollover2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrollovers.view") ? "View" : null;
            Session["LedgerRolloverBudgetsRolloversPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrolloverbudgetsrollovers.view") ? "View" : null;
            Session["LedgerRolloverEncumbrancesRolloversPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrolloverencumbrancesrollovers.view") ? "View" : null;
            Session["LedgerRolloverError2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrollovererrors.view") ? "View" : null;
            Session["LedgerRolloverProgress2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.ledgerrolloverprogresses.view") ? "View" : null;
            Session["Transaction2sPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.transactions.view") ? "View" : null;
            Session["TransactionTagsPermission"] = roles.Contains("all") || roles.Contains("finance.all") || roles.Contains("uc.transactiontags.view") ? "View" : null;
        }

        private void SetInventoryPermissions(HashSet<string> roles)
        {
            Session["AlternativeTitlesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.alternativetitles.view") ? "View" : null;
            Session["AlternativeTitleType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.alternativetitletypes.view") ? "View" : null;
            Session["BoundWithPart2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.boundwithparts.view") ? "View" : null;
            Session["CallNumberType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.callnumbertypes.view") ? "View" : null;
            Session["Campus2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.campuses.view") ? "View" : null;
            Session["CirculationNotesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.circulationnotes.view") ? "View" : null;
            Session["ClassificationsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.classifications.view") ? "View" : null;
            Session["ClassificationType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.classificationtypes.view") ? "View" : null;
            Session["ContributorsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.contributors.view") ? "View" : null;
            Session["ContributorNameType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.contributornametypes.view") ? "View" : null;
            Session["ContributorType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.contributortypes.view") ? "View" : null;
            Session["EditionsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.editions.view") ? "View" : null;
            Session["ElectronicAccessesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.electronicaccesses.view") ? "View" : null;
            Session["ElectronicAccessRelationship2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.electronicaccessrelationships.view") ? "View" : null;
            Session["ExtentsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.extents.view") ? "View" : null;
            Session["FormatsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.formats.view") ? "View" : null;
            Session["Holding2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdings.view") ? "View" : null;
            Session["HoldingElectronicAccessesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingelectronicaccesses.view") ? "View" : null;
            Session["HoldingEntriesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingentries.view") ? "View" : null;
            Session["HoldingFormerIdsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingformerids.view") ? "View" : null;
            Session["HoldingNotesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingnotes.view") ? "View" : null;
            Session["HoldingNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingnotetypes.view") ? "View" : null;
            Session["HoldingStatisticalCodesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingstatisticalcodes.view") ? "View" : null;
            Session["HoldingTagsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingtags.view") ? "View" : null;
            Session["HoldingType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.holdingtypes.view") ? "View" : null;
            Session["HridSetting2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.hridsettings.view") ? "View" : null;
            Session["IdentifiersPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.identifiers.view") ? "View" : null;
            Session["IdType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.idtypes.view") ? "View" : null;
            Session["IllPolicy2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.illpolicies.view") ? "View" : null;
            Session["IndexStatementsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.indexstatements.view") ? "View" : null;
            Session["Instance2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instances.view") ? "View" : null;
            Session["InstanceFormat2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instanceformats.view") ? "View" : null;
            Session["InstanceNatureOfContentTermsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancenatureofcontentterms.view") ? "View" : null;
            Session["InstanceNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancenotetypes.view") ? "View" : null;
            Session["InstanceStatisticalCodesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancestatisticalcodes.view") ? "View" : null;
            Session["InstanceTagsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancetags.view") ? "View" : null;
            Session["InstanceType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.instancetypes.view") ? "View" : null;
            Session["Institution2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.institutions.view") ? "View" : null;
            Session["IssuanceModesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.issuancemodes.view") ? "View" : null;
            Session["Item2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.items.view") ? "View" : null;
            Session["ItemDamagedStatus2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemdamagedstatuses.view") ? "View" : null;
            Session["ItemElectronicAccessesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemelectronicaccesses.view") ? "View" : null;
            Session["ItemFormerIdsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemformerids.view") ? "View" : null;
            Session["ItemNotesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemnotes.view") ? "View" : null;
            Session["ItemNoteType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemnotetypes.view") ? "View" : null;
            Session["ItemStatisticalCodesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemstatisticalcodes.view") ? "View" : null;
            Session["ItemStatusesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemstatuses.view") ? "View" : null;
            Session["ItemTagsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemtags.view") ? "View" : null;
            Session["ItemYearCaptionsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.itemyearcaptions.view") ? "View" : null;
            Session["LanguagesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.languages.view") ? "View" : null;
            Session["Library2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.libraries.view") ? "View" : null;
            Session["LoanType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.loantypes.view") ? "View" : null;
            Session["Location2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.locations.view") ? "View" : null;
            Session["LocationServicePointsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.locationservicepoints.view") ? "View" : null;
            Session["MaterialType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.materialtypes.view") ? "View" : null;
            Session["NatureOfContentTerm2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.natureofcontentterms.view") ? "View" : null;
            Session["Note2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.notes.view") ? "View" : null;
            Session["PhysicalDescriptionsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.physicaldescriptions.view") ? "View" : null;
            Session["PrecedingSucceedingTitle2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.precedingsucceedingtitles.view") ? "View" : null;
            Session["PrecedingSucceedingTitleIdentifiersPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.precedingsucceedingtitleidentifiers.view") ? "View" : null;
            Session["PublicationsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.publications.view") ? "View" : null;
            Session["PublicationFrequenciesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.publicationfrequencies.view") ? "View" : null;
            Session["PublicationRangesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.publicationranges.view") ? "View" : null;
            Session["RelationshipsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.relationships.view") ? "View" : null;
            Session["RelationshipTypesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.relationshiptypes.view") ? "View" : null;
            Session["SeriesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.series.view") ? "View" : null;
            Session["ServicePoint2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepoints.view") ? "View" : null;
            Session["ServicePointStaffSlipsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepointstaffslips.view") ? "View" : null;
            Session["ServicePointUser2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepointusers.view") ? "View" : null;
            Session["ServicePointUserServicePointsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.servicepointuserservicepoints.view") ? "View" : null;
            Session["Source2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.sources.view") ? "View" : null;
            Session["SourceMarcsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.sourcemarcs.view") ? "View" : null;
            Session["SourceMarcFieldsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.sourcemarcfields.view") ? "View" : null;
            Session["StatisticalCode2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statisticalcodes.view") ? "View" : null;
            Session["StatisticalCodeType2sPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statisticalcodetypes.view") ? "View" : null;
            Session["StatusesPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.statuses.view") ? "View" : null;
            Session["SubjectsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.subjects.view") ? "View" : null;
            Session["SupplementStatementsPermission"] = roles.Contains("all") || roles.Contains("inventory.all") || roles.Contains("uc.supplementstatements.view") ? "View" : null;
        }

        private void SetInvoicesPermissions(HashSet<string> roles)
        {
            Session["BatchGroup2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchgroups.view") ? "View" : null;
            Session["BatchVoucher2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchvouchers.view") ? "View" : null;
            Session["BatchVoucherBatchedVouchersPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherbatchedvouchers.view") ? "View" : null;
            Session["BatchVoucherBatchedVoucherBatchedVoucherLinesPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherbatchedvoucherbatchedvoucherlines.view") ? "View" : null;
            Session["BatchVoucherBatchedVoucherBatchedVoucherLineFundCodesPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherbatchedvoucherbatchedvoucherlinefundcodes.view") ? "View" : null;
            Session["BatchVoucherExport2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherexports.view") ? "View" : null;
            Session["BatchVoucherExportConfig2sPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherexportconfigs.view") ? "View" : null;
            Session["BatchVoucherExportConfigWeekdaysPermission"] = roles.Contains("all") || roles.Contains("uc.batchvoucherexportconfigweekdays.view") ? "View" : null;
            Session["Document2sPermission"] = roles.Contains("all") || roles.Contains("uc.documents.view") ? "View" : null;
            Session["ExportConfigCredential2sPermission"] = roles.Contains("all") || roles.Contains("uc.exportconfigcredentials.view") ? "View" : null;
            Session["Invoice2sPermission"] = roles.Contains("all") || roles.Contains("uc.invoices.view") ? "View" : null;
            Session["InvoiceAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceacquisitionsunits.view") ? "View" : null;
            Session["InvoiceAdjustmentsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceadjustments.view") ? "View" : null;
            Session["InvoiceAdjustmentFundsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceadjustmentfunds.view") ? "View" : null;
            Session["InvoiceItem2sPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitems.view") ? "View" : null;
            Session["InvoiceItemAdjustmentsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitemadjustments.view") ? "View" : null;
            Session["InvoiceItemAdjustmentFundsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitemadjustmentfunds.view") ? "View" : null;
            Session["InvoiceItemFundsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitemfunds.view") ? "View" : null;
            Session["InvoiceItemReferenceNumbersPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitemreferencenumbers.view") ? "View" : null;
            Session["InvoiceItemTagsPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceitemtags.view") ? "View" : null;
            Session["InvoiceOrderNumbersPermission"] = roles.Contains("all") || roles.Contains("uc.invoiceordernumbers.view") ? "View" : null;
            Session["InvoiceStatusesPermission"] = roles.Contains("all") || roles.Contains("uc.invoicestatuses.view") ? "View" : null;
            Session["InvoiceTagsPermission"] = roles.Contains("all") || roles.Contains("uc.invoicetags.view") ? "View" : null;
            Session["PaymentTypesPermission"] = roles.Contains("all") || roles.Contains("uc.paymenttypes.view") ? "View" : null;
            Session["Voucher2sPermission"] = roles.Contains("all") || roles.Contains("uc.vouchers.view") ? "View" : null;
            Session["VoucherAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("uc.voucheracquisitionsunits.view") ? "View" : null;
            Session["VoucherItem2sPermission"] = roles.Contains("all") || roles.Contains("uc.voucheritems.view") ? "View" : null;
            Session["VoucherItemFundsPermission"] = roles.Contains("all") || roles.Contains("uc.voucheritemfunds.view") ? "View" : null;
            Session["VoucherItemInvoiceItemsPermission"] = roles.Contains("all") || roles.Contains("uc.voucheriteminvoiceitems.view") ? "View" : null;
            Session["VoucherStatusesPermission"] = roles.Contains("all") || roles.Contains("uc.voucherstatuses.view") ? "View" : null;
        }

        private void SetLoginPermissions(HashSet<string> roles)
        {
            Session["AuthAttempt2sPermission"] = roles.Contains("all") || roles.Contains("login.all") || roles.Contains("uc.authattempts.view") ? "View" : null;
            Session["AuthCredentialsHistory2sPermission"] = roles.Contains("all") || roles.Contains("login.all") || roles.Contains("uc.authcredentialshistories.view") ? "View" : null;
            Session["EventLog2sPermission"] = roles.Contains("all") || roles.Contains("login.all") || roles.Contains("uc.eventlogs.view") ? "View" : null;
            Session["Login2sPermission"] = roles.Contains("all") || roles.Contains("login.all") || roles.Contains("uc.logins.view") ? "View" : null;
        }

        private void SetOrdersPermissions(HashSet<string> roles)
        {
            Session["AcquisitionsUnit2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.acquisitionsunits.view") ? "View" : null;
            Session["Alert2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.alerts.view") ? "View" : null;
            Session["CloseReason2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.closereasons.view") ? "View" : null;
            Session["Order2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orders.view") ? "View" : null;
            Session["OrderAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderacquisitionsunits.view") ? "View" : null;
            Session["OrderInvoice2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderinvoices.view") ? "View" : null;
            Session["OrderItem2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitems.view") ? "View" : null;
            Session["OrderItemAlertsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemalerts.view") ? "View" : null;
            Session["OrderItemClaimsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemclaims.view") ? "View" : null;
            Session["OrderItemContributorsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemcontributors.view") ? "View" : null;
            Session["OrderItemFundsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemfunds.view") ? "View" : null;
            Session["OrderItemLocation2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemlocations.view") ? "View" : null;
            Session["OrderItemNotesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemnotes.view") ? "View" : null;
            Session["OrderItemProductIdsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemproductids.view") ? "View" : null;
            Session["OrderItemReferenceNumbersPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemreferencenumbers.view") ? "View" : null;
            Session["OrderItemReportingCodesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemreportingcodes.view") ? "View" : null;
            Session["OrderItemTagsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemtags.view") ? "View" : null;
            Session["OrderItemVolumesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderitemvolumes.view") ? "View" : null;
            Session["OrderNotesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.ordernotes.view") ? "View" : null;
            Session["OrderStatusesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.orderstatuses.view") ? "View" : null;
            Session["OrderTagsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.ordertags.view") ? "View" : null;
            Session["OrderTemplate2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.ordertemplates.view") ? "View" : null;
            Session["OrderTypesPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.ordertypes.view") ? "View" : null;
            Session["Prefix2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.prefixes.view") ? "View" : null;
            Session["Receiving2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.receivings.view") ? "View" : null;
            Session["ReportingCode2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.reportingcodes.view") ? "View" : null;
            Session["Suffix2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.suffixes.view") ? "View" : null;
            Session["Title2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.titles.view") ? "View" : null;
            Session["TitleContributorsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.titlecontributors.view") ? "View" : null;
            Session["TitleProductIdsPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.titleproductids.view") ? "View" : null;
            Session["UserAcquisitionsUnit2sPermission"] = roles.Contains("all") || roles.Contains("orders.all") || roles.Contains("uc.useracquisitionsunits.view") ? "View" : null;
        }

        private void SetOrganizationsPermissions(HashSet<string> roles)
        {
            Session["Category2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.categories.view") ? "View" : null;
            Session["Contact2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contacts.view") ? "View" : null;
            Session["ContactAddressesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactaddresses.view") ? "View" : null;
            Session["ContactAddressCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactaddresscategories.view") ? "View" : null;
            Session["ContactCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactcategories.view") ? "View" : null;
            Session["ContactEmailsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactemails.view") ? "View" : null;
            Session["ContactEmailCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactemailcategories.view") ? "View" : null;
            Session["ContactPhoneNumbersPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactphonenumbers.view") ? "View" : null;
            Session["ContactPhoneNumberCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contactphonenumbercategories.view") ? "View" : null;
            Session["ContactUrlsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contacturls.view") ? "View" : null;
            Session["ContactUrlCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.contacturlcategories.view") ? "View" : null;
            Session["CurrenciesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.currencies.view") ? "View" : null;
            Session["Interface2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.interfaces.view") ? "View" : null;
            Session["InterfaceCredential2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.interfacecredentials.view") ? "View" : null;
            Session["InterfaceTypesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.interfacetypes.view") ? "View" : null;
            Session["Organization2sPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizations.view") ? "View" : null;
            Session["OrganizationAccountsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationaccounts.view") ? "View" : null;
            Session["OrganizationAccountAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationaccountacquisitionsunits.view") ? "View" : null;
            Session["OrganizationAcquisitionsUnitsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationacquisitionsunits.view") ? "View" : null;
            Session["OrganizationAddressesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationaddresses.view") ? "View" : null;
            Session["OrganizationAddressCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationaddresscategories.view") ? "View" : null;
            Session["OrganizationAgreementsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationagreements.view") ? "View" : null;
            Session["OrganizationAliasesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationaliases.view") ? "View" : null;
            Session["OrganizationChangelogsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationchangelogs.view") ? "View" : null;
            Session["OrganizationContactsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationcontacts.view") ? "View" : null;
            Session["OrganizationEmailsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationemails.view") ? "View" : null;
            Session["OrganizationEmailCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationemailcategories.view") ? "View" : null;
            Session["OrganizationInterfacesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationinterfaces.view") ? "View" : null;
            Session["OrganizationNotesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationnotes.view") ? "View" : null;
            Session["OrganizationPhoneNumbersPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationphonenumbers.view") ? "View" : null;
            Session["OrganizationPhoneNumberCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationphonenumbercategories.view") ? "View" : null;
            Session["OrganizationTagsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationtags.view") ? "View" : null;
            Session["OrganizationUrlsPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationurls.view") ? "View" : null;
            Session["OrganizationUrlCategoriesPermission"] = roles.Contains("all") || roles.Contains("organizations.module.all") || roles.Contains("uc.organizationurlcategories.view") ? "View" : null;
        }

        private void SetPermissionsPermissions(HashSet<string> roles)
        {
            Session["Permission2sPermission"] = roles.Contains("uc.permissions.edit") ? "Edit" : roles.Contains("all") || roles.Contains("uc.permissions.view") ? "View" : null;
            Session["PermissionChildOfsPermission"] = roles.Contains("all") || roles.Contains("uc.permissionchildofs.view") ? "View" : null;
            Session["PermissionGrantedTosPermission"] = roles.Contains("all") || roles.Contains("uc.permissiongrantedtos.view") ? "View" : null;
            Session["PermissionSubPermissionsPermission"] = roles.Contains("all") || roles.Contains("uc.permissionsubpermissions.view") ? "View" : null;
            Session["PermissionsUser2sPermission"] = roles.Contains("all") || roles.Contains("uc.permissionsusers.view") ? "View" : null;
            Session["PermissionsUserPermissionsPermission"] = roles.Contains("all") || roles.Contains("uc.permissionsuserpermissions.view") ? "View" : null;
            Session["PermissionTagsPermission"] = roles.Contains("all") || roles.Contains("uc.permissiontags.view") ? "View" : null;
        }

        private void SetSourcePermissions(HashSet<string> roles)
        {
            Session["ErrorRecord2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.errorrecords.view") ? "View" : null;
            Session["JobExecution2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.jobexecutions.view") ? "View" : null;
            Session["JobExecutionProgress2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.jobexecutionprogresses.view") ? "View" : null;
            Session["JobExecutionSourceChunk2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.jobexecutionsourcechunks.view") ? "View" : null;
            Session["JobMonitoring2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.jobmonitorings.view") ? "View" : null;
            Session["JournalRecord2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.journalrecords.view") ? "View" : null;
            Session["MarcRecord2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.marcrecords.view") ? "View" : null;
            Session["RawRecord2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.rawrecords.view") ? "View" : null;
            Session["Record2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.records.view") ? "View" : null;
            Session["Snapshot2sPermission"] = roles.Contains("all") || roles.Contains("source-storage.all") || roles.Contains("uc.snapshots.view") ? "View" : null;
        }

        private void SetTemplatesPermissions(HashSet<string> roles)
        {
            Session["Template2sPermission"] = roles.Contains("all") || roles.Contains("templates.all") || roles.Contains("uc.templates.view") ? "View" : null;
            Session["TemplateOutputFormatsPermission"] = roles.Contains("all") || roles.Contains("templates.all") || roles.Contains("uc.templateoutputformats.view") ? "View" : null;
        }

        private void SetUsersPermissions(HashSet<string> roles)
        {
            Session["AddressType2sPermission"] = roles.Contains("all") || roles.Contains("uc.addresstypes.view") || roles.Contains("users.all") ? "View" : null;
            Session["BlockCondition2sPermission"] = roles.Contains("all") || roles.Contains("uc.blockconditions.view") || roles.Contains("users.all") ? "View" : null;
            Session["BlockLimit2sPermission"] = roles.Contains("all") || roles.Contains("uc.blocklimits.view") || roles.Contains("users.all") ? "View" : null;
            Session["ContactTypesPermission"] = roles.Contains("all") || roles.Contains("uc.contacttypes.view") || roles.Contains("users.all") ? "View" : null;
            Session["CountriesPermission"] = roles.Contains("all") || roles.Contains("uc.countries.view") || roles.Contains("users.all") ? "View" : null;
            Session["CustomField2sPermission"] = roles.Contains("all") || roles.Contains("uc.customfields.view") || roles.Contains("users.all") ? "View" : null;
            Session["CustomFieldValuesPermission"] = roles.Contains("all") || roles.Contains("uc.customfieldvalues.view") || roles.Contains("users.all") ? "View" : null;
            Session["Department2sPermission"] = roles.Contains("all") || roles.Contains("uc.departments.view") || roles.Contains("users.all") ? "View" : null;
            Session["Group2sPermission"] = roles.Contains("all") || roles.Contains("uc.groups.view") || roles.Contains("users.all") ? "View" : null;
            Session["Proxy2sPermission"] = roles.Contains("all") || roles.Contains("uc.proxies.view") || roles.Contains("users.all") ? "View" : null;
            Session["User2sPermission"] = roles.Contains("uc.users.edit") ? "Edit" : roles.Contains("all") || roles.Contains("uc.users.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserAddressesPermission"] = roles.Contains("all") || roles.Contains("uc.useraddresses.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserDepartmentsPermission"] = roles.Contains("all") || roles.Contains("uc.userdepartments.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserNotesPermission"] = roles.Contains("all") || roles.Contains("uc.usernotes.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserSummary2sPermission"] = roles.Contains("all") || roles.Contains("uc.usersummaries.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserSummaryOpenFeesFinesPermission"] = roles.Contains("all") || roles.Contains("uc.usersummaryopenfeesfines.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserSummaryOpenLoansPermission"] = roles.Contains("all") || roles.Contains("uc.usersummaryopenloans.view") || roles.Contains("users.all") ? "View" : null;
            Session["UserTagsPermission"] = roles.Contains("all") || roles.Contains("uc.usertags.view") || roles.Contains("users.all") ? "View" : null;
        }

        private void SetPermissions(string permission = null)
        {
            SetCirculationPermissions(permission);
            SetConfigurationPermissions(permission);
            SetFeesPermissions(permission);
            SetFinancePermissions(permission);
            SetInventoryPermissions(permission);
            SetInvoicesPermissions(permission);
            SetLoginPermissions(permission);
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
            Session["CirculationRule2sPermission"] = permission;
            Session["FixedDueDateSchedule2sPermission"] = permission;
            Session["FixedDueDateScheduleSchedulesPermission"] = permission;
            Session["Loan2sPermission"] = permission;
            Session["LoanEvent2sPermission"] = permission;
            Session["LoanPolicy2sPermission"] = permission;
            Session["PatronActionSession2sPermission"] = permission;
            Session["PatronNoticePolicy2sPermission"] = permission;
            Session["PatronNoticePolicyFeeFineNoticesPermission"] = permission;
            Session["PatronNoticePolicyLoanNoticesPermission"] = permission;
            Session["PatronNoticePolicyRequestNoticesPermission"] = permission;
            Session["Request2sPermission"] = permission;
            Session["RequestIdentifiersPermission"] = permission;
            Session["RequestNotesPermission"] = permission;
            Session["RequestPolicy2sPermission"] = permission;
            Session["RequestPolicyRequestTypesPermission"] = permission;
            Session["RequestTagsPermission"] = permission;
            Session["ScheduledNotice2sPermission"] = permission;
            Session["StaffSlip2sPermission"] = permission;
            Session["UserRequestPreference2sPermission"] = permission;
        }

        private void SetConfigurationPermissions(string permission = null)
        {
            Session["AddressesPermission"] = permission;
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
            Session["ServicePointOwnersPermission"] = permission;
            Session["TransferAccount2sPermission"] = permission;
            Session["TransferCriteria2sPermission"] = permission;
            Session["WaiveReason2sPermission"] = permission;
        }

        private void SetFinancePermissions(string permission = null)
        {
            Session["AllocatedFromFundsPermission"] = permission;
            Session["AllocatedToFundsPermission"] = permission;
            Session["Budget2sPermission"] = permission;
            Session["BudgetAcquisitionsUnitsPermission"] = permission;
            Session["BudgetExpenseClass2sPermission"] = permission;
            Session["BudgetGroup2sPermission"] = permission;
            Session["BudgetTagsPermission"] = permission;
            Session["ExpenseClass2sPermission"] = permission;
            Session["FinanceGroup2sPermission"] = permission;
            Session["FinanceGroupAcquisitionsUnitsPermission"] = permission;
            Session["FiscalYear2sPermission"] = permission;
            Session["FiscalYearAcquisitionsUnitsPermission"] = permission;
            Session["Fund2sPermission"] = permission;
            Session["FundAcquisitionsUnitsPermission"] = permission;
            Session["FundTagsPermission"] = permission;
            Session["FundType2sPermission"] = permission;
            Session["Ledger2sPermission"] = permission;
            Session["LedgerAcquisitionsUnitsPermission"] = permission;
            Session["LedgerRollover2sPermission"] = permission;
            Session["LedgerRolloverBudgetsRolloversPermission"] = permission;
            Session["LedgerRolloverEncumbrancesRolloversPermission"] = permission;
            Session["LedgerRolloverError2sPermission"] = permission;
            Session["LedgerRolloverProgress2sPermission"] = permission;
            Session["Transaction2sPermission"] = permission;
            Session["TransactionTagsPermission"] = permission;
        }

        private void SetInventoryPermissions(string permission = null)
        {
            Session["AlternativeTitlesPermission"] = permission;
            Session["AlternativeTitleType2sPermission"] = permission;
            Session["BoundWithPart2sPermission"] = permission;
            Session["CallNumberType2sPermission"] = permission;
            Session["Campus2sPermission"] = permission;
            Session["CirculationNotesPermission"] = permission;
            Session["ClassificationsPermission"] = permission;
            Session["ClassificationType2sPermission"] = permission;
            Session["ContributorsPermission"] = permission;
            Session["ContributorNameType2sPermission"] = permission;
            Session["ContributorType2sPermission"] = permission;
            Session["EditionsPermission"] = permission;
            Session["ElectronicAccessesPermission"] = permission;
            Session["ElectronicAccessRelationship2sPermission"] = permission;
            Session["ExtentsPermission"] = permission;
            Session["FormatsPermission"] = permission;
            Session["Holding2sPermission"] = permission;
            Session["HoldingElectronicAccessesPermission"] = permission;
            Session["HoldingEntriesPermission"] = permission;
            Session["HoldingFormerIdsPermission"] = permission;
            Session["HoldingNotesPermission"] = permission;
            Session["HoldingNoteType2sPermission"] = permission;
            Session["HoldingStatisticalCodesPermission"] = permission;
            Session["HoldingTagsPermission"] = permission;
            Session["HoldingType2sPermission"] = permission;
            Session["HridSetting2sPermission"] = permission;
            Session["IdentifiersPermission"] = permission;
            Session["IdType2sPermission"] = permission;
            Session["IllPolicy2sPermission"] = permission;
            Session["IndexStatementsPermission"] = permission;
            Session["Instance2sPermission"] = permission;
            Session["InstanceFormat2sPermission"] = permission;
            Session["InstanceNatureOfContentTermsPermission"] = permission;
            Session["InstanceNoteType2sPermission"] = permission;
            Session["InstanceStatisticalCodesPermission"] = permission;
            Session["InstanceTagsPermission"] = permission;
            Session["InstanceType2sPermission"] = permission;
            Session["Institution2sPermission"] = permission;
            Session["IssuanceModesPermission"] = permission;
            Session["Item2sPermission"] = permission;
            Session["ItemDamagedStatus2sPermission"] = permission;
            Session["ItemElectronicAccessesPermission"] = permission;
            Session["ItemFormerIdsPermission"] = permission;
            Session["ItemNotesPermission"] = permission;
            Session["ItemNoteType2sPermission"] = permission;
            Session["ItemStatisticalCodesPermission"] = permission;
            Session["ItemStatusesPermission"] = permission;
            Session["ItemTagsPermission"] = permission;
            Session["ItemYearCaptionsPermission"] = permission;
            Session["LanguagesPermission"] = permission;
            Session["Library2sPermission"] = permission;
            Session["LoanType2sPermission"] = permission;
            Session["Location2sPermission"] = permission;
            Session["LocationServicePointsPermission"] = permission;
            Session["MaterialType2sPermission"] = permission;
            Session["NatureOfContentTerm2sPermission"] = permission;
            Session["Note2sPermission"] = permission;
            Session["PhysicalDescriptionsPermission"] = permission;
            Session["PrecedingSucceedingTitle2sPermission"] = permission;
            Session["PrecedingSucceedingTitleIdentifiersPermission"] = permission;
            Session["PublicationsPermission"] = permission;
            Session["PublicationFrequenciesPermission"] = permission;
            Session["PublicationRangesPermission"] = permission;
            Session["RelationshipsPermission"] = permission;
            Session["RelationshipTypesPermission"] = permission;
            Session["SeriesPermission"] = permission;
            Session["ServicePoint2sPermission"] = permission;
            Session["ServicePointStaffSlipsPermission"] = permission;
            Session["ServicePointUser2sPermission"] = permission;
            Session["ServicePointUserServicePointsPermission"] = permission;
            Session["Source2sPermission"] = permission;
            Session["SourceMarcsPermission"] = permission;
            Session["SourceMarcFieldsPermission"] = permission;
            Session["StatisticalCode2sPermission"] = permission;
            Session["StatisticalCodeType2sPermission"] = permission;
            Session["StatusesPermission"] = permission;
            Session["SubjectsPermission"] = permission;
            Session["SupplementStatementsPermission"] = permission;
        }

        private void SetInvoicesPermissions(string permission = null)
        {
            Session["BatchGroup2sPermission"] = permission;
            Session["BatchVoucher2sPermission"] = permission;
            Session["BatchVoucherBatchedVouchersPermission"] = permission;
            Session["BatchVoucherBatchedVoucherBatchedVoucherLinesPermission"] = permission;
            Session["BatchVoucherBatchedVoucherBatchedVoucherLineFundCodesPermission"] = permission;
            Session["BatchVoucherExport2sPermission"] = permission;
            Session["BatchVoucherExportConfig2sPermission"] = permission;
            Session["BatchVoucherExportConfigWeekdaysPermission"] = permission;
            Session["Document2sPermission"] = permission;
            Session["ExportConfigCredential2sPermission"] = permission;
            Session["Invoice2sPermission"] = permission;
            Session["InvoiceAcquisitionsUnitsPermission"] = permission;
            Session["InvoiceAdjustmentsPermission"] = permission;
            Session["InvoiceAdjustmentFundsPermission"] = permission;
            Session["InvoiceItem2sPermission"] = permission;
            Session["InvoiceItemAdjustmentsPermission"] = permission;
            Session["InvoiceItemAdjustmentFundsPermission"] = permission;
            Session["InvoiceItemFundsPermission"] = permission;
            Session["InvoiceItemReferenceNumbersPermission"] = permission;
            Session["InvoiceItemTagsPermission"] = permission;
            Session["InvoiceOrderNumbersPermission"] = permission;
            Session["InvoiceStatusesPermission"] = permission;
            Session["InvoiceTagsPermission"] = permission;
            Session["PaymentTypesPermission"] = permission;
            Session["Voucher2sPermission"] = permission;
            Session["VoucherAcquisitionsUnitsPermission"] = permission;
            Session["VoucherItem2sPermission"] = permission;
            Session["VoucherItemFundsPermission"] = permission;
            Session["VoucherItemInvoiceItemsPermission"] = permission;
            Session["VoucherStatusesPermission"] = permission;
        }

        private void SetLoginPermissions(string permission = null)
        {
            Session["AuthAttempt2sPermission"] = permission;
            Session["AuthCredentialsHistory2sPermission"] = permission;
            Session["EventLog2sPermission"] = permission;
            Session["Login2sPermission"] = permission;
        }

        private void SetOrdersPermissions(string permission = null)
        {
            Session["AcquisitionsUnit2sPermission"] = permission;
            Session["Alert2sPermission"] = permission;
            Session["CloseReason2sPermission"] = permission;
            Session["Order2sPermission"] = permission;
            Session["OrderAcquisitionsUnitsPermission"] = permission;
            Session["OrderInvoice2sPermission"] = permission;
            Session["OrderItem2sPermission"] = permission;
            Session["OrderItemAlertsPermission"] = permission;
            Session["OrderItemClaimsPermission"] = permission;
            Session["OrderItemContributorsPermission"] = permission;
            Session["OrderItemFundsPermission"] = permission;
            Session["OrderItemLocation2sPermission"] = permission;
            Session["OrderItemNotesPermission"] = permission;
            Session["OrderItemProductIdsPermission"] = permission;
            Session["OrderItemReferenceNumbersPermission"] = permission;
            Session["OrderItemReportingCodesPermission"] = permission;
            Session["OrderItemTagsPermission"] = permission;
            Session["OrderItemVolumesPermission"] = permission;
            Session["OrderNotesPermission"] = permission;
            Session["OrderStatusesPermission"] = permission;
            Session["OrderTagsPermission"] = permission;
            Session["OrderTemplate2sPermission"] = permission;
            Session["OrderTypesPermission"] = permission;
            Session["Prefix2sPermission"] = permission;
            Session["Receiving2sPermission"] = permission;
            Session["ReportingCode2sPermission"] = permission;
            Session["Suffix2sPermission"] = permission;
            Session["Title2sPermission"] = permission;
            Session["TitleContributorsPermission"] = permission;
            Session["TitleProductIdsPermission"] = permission;
            Session["UserAcquisitionsUnit2sPermission"] = permission;
        }

        private void SetOrganizationsPermissions(string permission = null)
        {
            Session["Category2sPermission"] = permission;
            Session["Contact2sPermission"] = permission;
            Session["ContactAddressesPermission"] = permission;
            Session["ContactAddressCategoriesPermission"] = permission;
            Session["ContactCategoriesPermission"] = permission;
            Session["ContactEmailsPermission"] = permission;
            Session["ContactEmailCategoriesPermission"] = permission;
            Session["ContactPhoneNumbersPermission"] = permission;
            Session["ContactPhoneNumberCategoriesPermission"] = permission;
            Session["ContactUrlsPermission"] = permission;
            Session["ContactUrlCategoriesPermission"] = permission;
            Session["CurrenciesPermission"] = permission;
            Session["Interface2sPermission"] = permission;
            Session["InterfaceCredential2sPermission"] = permission;
            Session["InterfaceTypesPermission"] = permission;
            Session["Organization2sPermission"] = permission;
            Session["OrganizationAccountsPermission"] = permission;
            Session["OrganizationAccountAcquisitionsUnitsPermission"] = permission;
            Session["OrganizationAcquisitionsUnitsPermission"] = permission;
            Session["OrganizationAddressesPermission"] = permission;
            Session["OrganizationAddressCategoriesPermission"] = permission;
            Session["OrganizationAgreementsPermission"] = permission;
            Session["OrganizationAliasesPermission"] = permission;
            Session["OrganizationChangelogsPermission"] = permission;
            Session["OrganizationContactsPermission"] = permission;
            Session["OrganizationEmailsPermission"] = permission;
            Session["OrganizationEmailCategoriesPermission"] = permission;
            Session["OrganizationInterfacesPermission"] = permission;
            Session["OrganizationNotesPermission"] = permission;
            Session["OrganizationPhoneNumbersPermission"] = permission;
            Session["OrganizationPhoneNumberCategoriesPermission"] = permission;
            Session["OrganizationTagsPermission"] = permission;
            Session["OrganizationUrlsPermission"] = permission;
            Session["OrganizationUrlCategoriesPermission"] = permission;
        }

        private void SetPermissionsPermissions(string permission = null)
        {
            Session["Permission2sPermission"] = permission;
            Session["PermissionChildOfsPermission"] = permission;
            Session["PermissionGrantedTosPermission"] = permission;
            Session["PermissionSubPermissionsPermission"] = permission;
            Session["PermissionsUser2sPermission"] = permission;
            Session["PermissionsUserPermissionsPermission"] = permission;
            Session["PermissionTagsPermission"] = permission;
        }

        private void SetSourcePermissions(string permission = null)
        {
            Session["ErrorRecord2sPermission"] = permission;
            Session["JobExecution2sPermission"] = permission;
            Session["JobExecutionProgress2sPermission"] = permission;
            Session["JobExecutionSourceChunk2sPermission"] = permission;
            Session["JobMonitoring2sPermission"] = permission;
            Session["JournalRecord2sPermission"] = permission;
            Session["MarcRecord2sPermission"] = permission;
            Session["RawRecord2sPermission"] = permission;
            Session["Record2sPermission"] = permission;
            Session["Snapshot2sPermission"] = permission;
        }

        private void SetTemplatesPermissions(string permission = null)
        {
            Session["Template2sPermission"] = permission;
            Session["TemplateOutputFormatsPermission"] = permission;
        }

        private void SetUsersPermissions(string permission = null)
        {
            Session["AddressType2sPermission"] = permission;
            Session["BlockCondition2sPermission"] = permission;
            Session["BlockLimit2sPermission"] = permission;
            Session["ContactTypesPermission"] = permission;
            Session["CountriesPermission"] = permission;
            Session["CustomField2sPermission"] = permission;
            Session["CustomFieldValuesPermission"] = permission;
            Session["Department2sPermission"] = permission;
            Session["Group2sPermission"] = permission;
            Session["Proxy2sPermission"] = permission;
            Session["User2sPermission"] = permission;
            Session["UserAddressesPermission"] = permission;
            Session["UserDepartmentsPermission"] = permission;
            Session["UserNotesPermission"] = permission;
            Session["UserSummary2sPermission"] = permission;
            Session["UserSummaryOpenFeesFinesPermission"] = permission;
            Session["UserSummaryOpenLoansPermission"] = permission;
            Session["UserTagsPermission"] = permission;
        }

        public static string TextEncode(string value)
        {
            if (value == null) return value;
            return value.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }
    }
}
