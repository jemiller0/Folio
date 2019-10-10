using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FolioConsoleApplication
{
    partial class Program4
    {
        private static bool api;
        private static bool compress;
        private static bool force;
        private static int? take;
        private readonly static TraceSource traceSource = new TraceSource("FolioConsoleApplication", SourceLevels.Information);
        private static bool validate;
        private static bool whatIf;

        static int Main(string[] args)
        {
            var s = Stopwatch.StartNew();
            try
            {
                var tracePath = args.SkipWhile(s3 => !s3.Equals("-TracePath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var verbose = args.Any(s3 => s3.Equals("-Verbose", StringComparison.OrdinalIgnoreCase));
                traceSource.Listeners.Add(new TextWriterTraceListener(Console.Out) { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId });
                if (tracePath != null) traceSource.Listeners.Add(new DefaultTraceListener() { LogFileName = tracePath, TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId });
                FolioBulkCopyContext.traceSource.Listeners.AddRange(traceSource.Listeners);
                FolioDapperContext.traceSource.Listeners.AddRange(traceSource.Listeners);
                FolioServiceClient.traceSource.Listeners.AddRange(traceSource.Listeners);
                traceSource.Switch.Level = FolioBulkCopyContext.traceSource.Switch.Level = FolioDapperContext.traceSource.Switch.Level = FolioServiceClient.traceSource.Switch.Level = verbose ? SourceLevels.Verbose : SourceLevels.Information;
                traceSource.TraceEvent(TraceEventType.Information, 0, "Starting");
                Initialize();
                if (args.Length == 0)
                {
                    traceSource.TraceEvent(TraceEventType.Critical, 0, "Usage: dotnet FolioConsoleApplication.dll [-All] [-Api] [-Compress] [-Delete] [-Force] [-Load] [-Path <string>] [-Save] [-Validate] [-Verbose] [-WhatIf] [-AllOrders] [-AllUsers] [-AllInventory] [-AllFinance] [-AllCirculation] [-AllOrganizations] [-AllInvoices] [-AllSource] [-AllLogin] [-AllPermissions] [-AcquisitionsUnitsPath <string>] [-AcquisitionsUnitsWhere <string>] [-AddressTypesPath <string>] [-AddressTypesWhere <string>] [-AlertsPath <string>] [-AlertsWhere <string>] [-AlternativeTitleTypesPath <string>] [-AlternativeTitleTypesWhere <string>] [-BudgetsPath <string>] [-BudgetsWhere <string>] [-CallNumberTypesPath <string>] [-CallNumberTypesWhere <string>] [-CampusesPath <string>] [-CampusesWhere <string>] [-CancellationReasonsPath <string>] [-CancellationReasonsWhere <string>] [-CategoriesPath <string>] [-CategoriesWhere <string>] [-ClassificationTypesPath <string>] [-ClassificationTypesWhere <string>] [-ContactsPath <string>] [-ContactsWhere <string>] [-ContributorNameTypesPath <string>] [-ContributorNameTypesWhere <string>] [-ContributorTypesPath <string>] [-ContributorTypesWhere <string>] [-DocumentsPath <string>] [-DocumentsWhere <string>] [-ElectronicAccessRelationshipsPath <string>] [-ElectronicAccessRelationshipsWhere <string>] [-EncumbrancesPath <string>] [-EncumbrancesWhere <string>] [-ErrorRecordsPath <string>] [-ErrorRecordsWhere <string>] [-FiscalYearsPath <string>] [-FiscalYearsWhere <string>] [-FixedDueDateSchedulesPath <string>] [-FixedDueDateSchedulesWhere <string>] [-FundsPath <string>] [-FundsWhere <string>] [-FundDistributionsPath <string>] [-FundDistributionsWhere <string>] [-GroupsPath <string>] [-GroupsWhere <string>] [-HoldingsPath <string>] [-HoldingsWhere <string>] [-HoldingNoteTypesPath <string>] [-HoldingNoteTypesWhere <string>] [-HoldingTypesPath <string>] [-HoldingTypesWhere <string>] [-IdTypesPath <string>] [-IdTypesWhere <string>] [-IllPoliciesPath <string>] [-IllPoliciesWhere <string>] [-InstancesPath <string>] [-InstancesWhere <string>] [-InstanceFormatsPath <string>] [-InstanceFormatsWhere <string>] [-InstanceNoteTypesPath <string>] [-InstanceNoteTypesWhere <string>] [-InstanceRelationshipsPath <string>] [-InstanceRelationshipsWhere <string>] [-InstanceRelationshipTypesPath <string>] [-InstanceRelationshipTypesWhere <string>] [-InstanceStatusesPath <string>] [-InstanceStatusesWhere <string>] [-InstanceTypesPath <string>] [-InstanceTypesWhere <string>] [-InstitutionsPath <string>] [-InstitutionsWhere <string>] [-InterfacesPath <string>] [-InterfacesWhere <string>] [-InterfaceCredentialsPath <string>] [-InterfaceCredentialsWhere <string>] [-InvoicesPath <string>] [-InvoicesWhere <string>] [-InvoiceItemsPath <string>] [-InvoiceItemsWhere <string>] [-ItemsPath <string>] [-ItemsWhere <string>] [-ItemDamagedStatusesPath <string>] [-ItemDamagedStatusesWhere <string>] [-ItemNoteTypesPath <string>] [-ItemNoteTypesWhere <string>] [-LedgersPath <string>] [-LedgersWhere <string>] [-LibrariesPath <string>] [-LibrariesWhere <string>] [-LoansPath <string>] [-LoansWhere <string>] [-LoanPoliciesPath <string>] [-LoanPoliciesWhere <string>] [-LoanTypesPath <string>] [-LoanTypesWhere <string>] [-LocationsPath <string>] [-LocationsWhere <string>] [-LoginsPath <string>] [-LoginsWhere <string>] [-MarcRecordsPath <string>] [-MarcRecordsWhere <string>] [-MaterialTypesPath <string>] [-MaterialTypesWhere <string>] [-ModeOfIssuancesPath <string>] [-ModeOfIssuancesWhere <string>] [-NatureOfContentTermsPath <string>] [-NatureOfContentTermsWhere <string>] [-OrdersPath <string>] [-OrdersWhere <string>] [-OrderItemsPath <string>] [-OrderItemsWhere <string>] [-OrganizationsPath <string>] [-OrganizationsWhere <string>] [-PatronNoticePoliciesPath <string>] [-PatronNoticePoliciesWhere <string>] [-PermissionsPath <string>] [-PermissionsWhere <string>] [-PermissionsUsersPath <string>] [-PermissionsUsersWhere <string>] [-PiecesPath <string>] [-PiecesWhere <string>] [-ProxiesPath <string>] [-ProxiesWhere <string>] [-RawRecordsPath <string>] [-RawRecordsWhere <string>] [-RecordsPath <string>] [-RecordsWhere <string>] [-ReportingCodesPath <string>] [-ReportingCodesWhere <string>] [-RequestsPath <string>] [-RequestsWhere <string>] [-RequestPoliciesPath <string>] [-RequestPoliciesWhere <string>] [-ScheduledNoticesPath <string>] [-ScheduledNoticesWhere <string>] [-ServicePointsPath <string>] [-ServicePointsWhere <string>] [-ServicePointUsersPath <string>] [-ServicePointUsersWhere <string>] [-SnapshotsPath <string>] [-SnapshotsWhere <string>] [-StaffSlipsPath <string>] [-StaffSlipsWhere <string>] [-StatisticalCodesPath <string>] [-StatisticalCodesWhere <string>] [-StatisticalCodeTypesPath <string>] [-StatisticalCodeTypesWhere <string>] [-UsersPath <string>] [-UsersWhere <string>] [-UserAcquisitionsUnitsPath <string>] [-UserAcquisitionsUnitsWhere <string>] [-VouchersPath <string>] [-VouchersWhere <string>] [-VoucherItemsPath <string>] [-VoucherItemsWhere <string>]");
                    return -1;
                }
                var all = args.Any(s3 => s3.Equals("-All", StringComparison.OrdinalIgnoreCase));
                api = args.Any(s3 => s3.Equals("-Api", StringComparison.OrdinalIgnoreCase));
                compress = args.Any(s3 => s3.Equals("-Compress", StringComparison.OrdinalIgnoreCase));
                var delete = args.Any(s3 => s3.Equals("-Delete", StringComparison.OrdinalIgnoreCase));
                force = args.Any(s3 => s3.Equals("-Force", StringComparison.OrdinalIgnoreCase));
                var load = args.Any(s3 => s3.Equals("-Load", StringComparison.OrdinalIgnoreCase));
                var path = args.SkipWhile(s3 => !s3.Equals("-Path", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault() ?? ".";
                var save = args.Any(s3 => s3.Equals("-Save", StringComparison.OrdinalIgnoreCase));
                take = int.TryParse(args.SkipWhile(s3 => !s3.Equals("-Take", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault(), out int i) ? (int?)i : null;
                var threads = int.TryParse(args.SkipWhile(s3 => !s3.Equals("-Threads", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault(), out i) ? (int?)i : null;
                validate = args.Any(s3 => s3.Equals("-Validate", StringComparison.OrdinalIgnoreCase));
                whatIf = args.Any(s3 => s3.Equals("-WhatIf", StringComparison.OrdinalIgnoreCase));
                var allCirculation = args.Any(s3 => s3.Equals("-AllCirculation", StringComparison.OrdinalIgnoreCase));
                var allFinance = args.Any(s3 => s3.Equals("-AllFinance", StringComparison.OrdinalIgnoreCase));
                var allInventory = args.Any(s3 => s3.Equals("-AllInventory", StringComparison.OrdinalIgnoreCase));
                var allInvoices = args.Any(s3 => s3.Equals("-AllInvoices", StringComparison.OrdinalIgnoreCase));
                var allLogin = args.Any(s3 => s3.Equals("-AllLogin", StringComparison.OrdinalIgnoreCase));
                var allOrders = args.Any(s3 => s3.Equals("-AllOrders", StringComparison.OrdinalIgnoreCase));
                var allOrganizations = args.Any(s3 => s3.Equals("-AllOrganizations", StringComparison.OrdinalIgnoreCase));
                var allPermissions = args.Any(s3 => s3.Equals("-AllPermissions", StringComparison.OrdinalIgnoreCase));
                var allSource = args.Any(s3 => s3.Equals("-AllSource", StringComparison.OrdinalIgnoreCase));
                var allUsers = args.Any(s3 => s3.Equals("-AllUsers", StringComparison.OrdinalIgnoreCase));
                var acquisitionsUnitsPath = args.SkipWhile(s3 => !s3.Equals("-AcquisitionsUnitsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var addressTypesPath = args.SkipWhile(s3 => !s3.Equals("-AddressTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alertsPath = args.SkipWhile(s3 => !s3.Equals("-AlertsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alternativeTitleTypesPath = args.SkipWhile(s3 => !s3.Equals("-AlternativeTitleTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var budgetsPath = args.SkipWhile(s3 => !s3.Equals("-BudgetsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var callNumberTypesPath = args.SkipWhile(s3 => !s3.Equals("-CallNumberTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var campusesPath = args.SkipWhile(s3 => !s3.Equals("-CampusesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var cancellationReasonsPath = args.SkipWhile(s3 => !s3.Equals("-CancellationReasonsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var categoriesPath = args.SkipWhile(s3 => !s3.Equals("-CategoriesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var classificationTypesPath = args.SkipWhile(s3 => !s3.Equals("-ClassificationTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contactsPath = args.SkipWhile(s3 => !s3.Equals("-ContactsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorNameTypesPath = args.SkipWhile(s3 => !s3.Equals("-ContributorNameTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorTypesPath = args.SkipWhile(s3 => !s3.Equals("-ContributorTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var documentsPath = args.SkipWhile(s3 => !s3.Equals("-DocumentsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var electronicAccessRelationshipsPath = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var encumbrancesPath = args.SkipWhile(s3 => !s3.Equals("-EncumbrancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var errorRecordsPath = args.SkipWhile(s3 => !s3.Equals("-ErrorRecordsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fiscalYearsPath = args.SkipWhile(s3 => !s3.Equals("-FiscalYearsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fixedDueDateSchedulesPath = args.SkipWhile(s3 => !s3.Equals("-FixedDueDateSchedulesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fundsPath = args.SkipWhile(s3 => !s3.Equals("-FundsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fundDistributionsPath = args.SkipWhile(s3 => !s3.Equals("-FundDistributionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var groupsPath = args.SkipWhile(s3 => !s3.Equals("-GroupsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingsPath = args.SkipWhile(s3 => !s3.Equals("-HoldingsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingNoteTypesPath = args.SkipWhile(s3 => !s3.Equals("-HoldingNoteTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingTypesPath = args.SkipWhile(s3 => !s3.Equals("-HoldingTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var idTypesPath = args.SkipWhile(s3 => !s3.Equals("-IdTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var illPoliciesPath = args.SkipWhile(s3 => !s3.Equals("-IllPoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instancesPath = args.SkipWhile(s3 => !s3.Equals("-InstancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceFormatsPath = args.SkipWhile(s3 => !s3.Equals("-InstanceFormatsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceNoteTypesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceNoteTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipsPath = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipTypesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceStatusesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceStatusesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceTypesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var institutionsPath = args.SkipWhile(s3 => !s3.Equals("-InstitutionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfacesPath = args.SkipWhile(s3 => !s3.Equals("-InterfacesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfaceCredentialsPath = args.SkipWhile(s3 => !s3.Equals("-InterfaceCredentialsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoicesPath = args.SkipWhile(s3 => !s3.Equals("-InvoicesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoiceItemsPath = args.SkipWhile(s3 => !s3.Equals("-InvoiceItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemsPath = args.SkipWhile(s3 => !s3.Equals("-ItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemDamagedStatusesPath = args.SkipWhile(s3 => !s3.Equals("-ItemDamagedStatusesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesPath = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ledgersPath = args.SkipWhile(s3 => !s3.Equals("-LedgersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesPath = args.SkipWhile(s3 => !s3.Equals("-LibrariesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loansPath = args.SkipWhile(s3 => !s3.Equals("-LoansPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanPoliciesPath = args.SkipWhile(s3 => !s3.Equals("-LoanPoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesPath = args.SkipWhile(s3 => !s3.Equals("-LoanTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsPath = args.SkipWhile(s3 => !s3.Equals("-LocationsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsPath = args.SkipWhile(s3 => !s3.Equals("-LoginsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var marcRecordsPath = args.SkipWhile(s3 => !s3.Equals("-MarcRecordsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesPath = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesPath = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var natureOfContentTermsPath = args.SkipWhile(s3 => !s3.Equals("-NatureOfContentTermsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ordersPath = args.SkipWhile(s3 => !s3.Equals("-OrdersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var orderItemsPath = args.SkipWhile(s3 => !s3.Equals("-OrderItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var organizationsPath = args.SkipWhile(s3 => !s3.Equals("-OrganizationsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var patronNoticePoliciesPath = args.SkipWhile(s3 => !s3.Equals("-PatronNoticePoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var piecesPath = args.SkipWhile(s3 => !s3.Equals("-PiecesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesPath = args.SkipWhile(s3 => !s3.Equals("-ProxiesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var rawRecordsPath = args.SkipWhile(s3 => !s3.Equals("-RawRecordsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var recordsPath = args.SkipWhile(s3 => !s3.Equals("-RecordsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var reportingCodesPath = args.SkipWhile(s3 => !s3.Equals("-ReportingCodesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestsPath = args.SkipWhile(s3 => !s3.Equals("-RequestsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestPoliciesPath = args.SkipWhile(s3 => !s3.Equals("-RequestPoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var scheduledNoticesPath = args.SkipWhile(s3 => !s3.Equals("-ScheduledNoticesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var snapshotsPath = args.SkipWhile(s3 => !s3.Equals("-SnapshotsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var staffSlipsPath = args.SkipWhile(s3 => !s3.Equals("-StaffSlipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersPath = args.SkipWhile(s3 => !s3.Equals("-UsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var userAcquisitionsUnitsPath = args.SkipWhile(s3 => !s3.Equals("-UserAcquisitionsUnitsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var vouchersPath = args.SkipWhile(s3 => !s3.Equals("-VouchersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var voucherItemsPath = args.SkipWhile(s3 => !s3.Equals("-VoucherItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var acquisitionsUnitsWhere = args.SkipWhile(s3 => !s3.Equals("-AcquisitionsUnitsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var addressTypesWhere = args.SkipWhile(s3 => !s3.Equals("-AddressTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alertsWhere = args.SkipWhile(s3 => !s3.Equals("-AlertsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alternativeTitleTypesWhere = args.SkipWhile(s3 => !s3.Equals("-AlternativeTitleTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var budgetsWhere = args.SkipWhile(s3 => !s3.Equals("-BudgetsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var callNumberTypesWhere = args.SkipWhile(s3 => !s3.Equals("-CallNumberTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var campusesWhere = args.SkipWhile(s3 => !s3.Equals("-CampusesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var cancellationReasonsWhere = args.SkipWhile(s3 => !s3.Equals("-CancellationReasonsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var categoriesWhere = args.SkipWhile(s3 => !s3.Equals("-CategoriesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var classificationTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ClassificationTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contactsWhere = args.SkipWhile(s3 => !s3.Equals("-ContactsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorNameTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ContributorNameTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ContributorTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var documentsWhere = args.SkipWhile(s3 => !s3.Equals("-DocumentsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var electronicAccessRelationshipsWhere = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var encumbrancesWhere = args.SkipWhile(s3 => !s3.Equals("-EncumbrancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var errorRecordsWhere = args.SkipWhile(s3 => !s3.Equals("-ErrorRecordsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fiscalYearsWhere = args.SkipWhile(s3 => !s3.Equals("-FiscalYearsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fixedDueDateSchedulesWhere = args.SkipWhile(s3 => !s3.Equals("-FixedDueDateSchedulesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fundsWhere = args.SkipWhile(s3 => !s3.Equals("-FundsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var fundDistributionsWhere = args.SkipWhile(s3 => !s3.Equals("-FundDistributionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var groupsWhere = args.SkipWhile(s3 => !s3.Equals("-GroupsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingsWhere = args.SkipWhile(s3 => !s3.Equals("-HoldingsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingNoteTypesWhere = args.SkipWhile(s3 => !s3.Equals("-HoldingNoteTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var holdingTypesWhere = args.SkipWhile(s3 => !s3.Equals("-HoldingTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var idTypesWhere = args.SkipWhile(s3 => !s3.Equals("-IdTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var illPoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-IllPoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instancesWhere = args.SkipWhile(s3 => !s3.Equals("-InstancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceFormatsWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceFormatsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceNoteTypesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceNoteTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipsWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipTypesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceStatusesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceStatusesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceTypesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var institutionsWhere = args.SkipWhile(s3 => !s3.Equals("-InstitutionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfacesWhere = args.SkipWhile(s3 => !s3.Equals("-InterfacesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfaceCredentialsWhere = args.SkipWhile(s3 => !s3.Equals("-InterfaceCredentialsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoicesWhere = args.SkipWhile(s3 => !s3.Equals("-InvoicesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoiceItemsWhere = args.SkipWhile(s3 => !s3.Equals("-InvoiceItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemsWhere = args.SkipWhile(s3 => !s3.Equals("-ItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemDamagedStatusesWhere = args.SkipWhile(s3 => !s3.Equals("-ItemDamagedStatusesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ledgersWhere = args.SkipWhile(s3 => !s3.Equals("-LedgersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesWhere = args.SkipWhile(s3 => !s3.Equals("-LibrariesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loansWhere = args.SkipWhile(s3 => !s3.Equals("-LoansWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanPoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-LoanPoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesWhere = args.SkipWhile(s3 => !s3.Equals("-LoanTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsWhere = args.SkipWhile(s3 => !s3.Equals("-LocationsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsWhere = args.SkipWhile(s3 => !s3.Equals("-LoginsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var marcRecordsWhere = args.SkipWhile(s3 => !s3.Equals("-MarcRecordsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesWhere = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesWhere = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var natureOfContentTermsWhere = args.SkipWhile(s3 => !s3.Equals("-NatureOfContentTermsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ordersWhere = args.SkipWhile(s3 => !s3.Equals("-OrdersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var orderItemsWhere = args.SkipWhile(s3 => !s3.Equals("-OrderItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var organizationsWhere = args.SkipWhile(s3 => !s3.Equals("-OrganizationsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var patronNoticePoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-PatronNoticePoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var piecesWhere = args.SkipWhile(s3 => !s3.Equals("-PiecesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesWhere = args.SkipWhile(s3 => !s3.Equals("-ProxiesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var rawRecordsWhere = args.SkipWhile(s3 => !s3.Equals("-RawRecordsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var recordsWhere = args.SkipWhile(s3 => !s3.Equals("-RecordsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var reportingCodesWhere = args.SkipWhile(s3 => !s3.Equals("-ReportingCodesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestsWhere = args.SkipWhile(s3 => !s3.Equals("-RequestsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestPoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-RequestPoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var scheduledNoticesWhere = args.SkipWhile(s3 => !s3.Equals("-ScheduledNoticesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var snapshotsWhere = args.SkipWhile(s3 => !s3.Equals("-SnapshotsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var staffSlipsWhere = args.SkipWhile(s3 => !s3.Equals("-StaffSlipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersWhere = args.SkipWhile(s3 => !s3.Equals("-UsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var userAcquisitionsUnitsWhere = args.SkipWhile(s3 => !s3.Equals("-UserAcquisitionsUnitsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var vouchersWhere = args.SkipWhile(s3 => !s3.Equals("-VouchersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var voucherItemsWhere = args.SkipWhile(s3 => !s3.Equals("-VoucherItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                if (all)
                {
                    acquisitionsUnitsPath = $"{path}/acquisitionsunits.json";
                    addressTypesPath = $"{path}/addresstypes.json";
                    alertsPath = $"{path}/alerts.json";
                    alternativeTitleTypesPath = $"{path}/alternativetitletypes.json";
                    budgetsPath = $"{path}/budgets.json";
                    callNumberTypesPath = $"{path}/callnumbertypes.json";
                    campusesPath = $"{path}/campuses.json";
                    cancellationReasonsPath = $"{path}/cancellationreasons.json";
                    categoriesPath = $"{path}/categories.json";
                    classificationTypesPath = $"{path}/classificationtypes.json";
                    contactsPath = $"{path}/contacts.json";
                    contributorNameTypesPath = $"{path}/contributornametypes.json";
                    contributorTypesPath = $"{path}/contributortypes.json";
                    if (!api) documentsPath = $"{path}/documents.json";
                    electronicAccessRelationshipsPath = $"{path}/electronicaccessrelationships.json";
                    if (!api) encumbrancesPath = $"{path}/encumbrances.json";
                    if (!api) errorRecordsPath = $"{path}/errorrecords.json";
                    fiscalYearsPath = $"{path}/fiscalyears.json";
                    fixedDueDateSchedulesPath = $"{path}/fixedduedateschedules.json";
                    fundsPath = $"{path}/funds.json";
                    fundDistributionsPath = $"{path}/funddistributions.json";
                    groupsPath = $"{path}/groups.json";
                    holdingsPath = $"{path}/holdings.json";
                    holdingNoteTypesPath = $"{path}/holdingnotetypes.json";
                    holdingTypesPath = $"{path}/holdingtypes.json";
                    idTypesPath = $"{path}/idtypes.json";
                    illPoliciesPath = $"{path}/illpolicies.json";
                    instancesPath = $"{path}/instances.json";
                    instanceFormatsPath = $"{path}/instanceformats.json";
                    instanceNoteTypesPath = $"{path}/instancenotetypes.json";
                    instanceRelationshipsPath = $"{path}/instancerelationships.json";
                    instanceRelationshipTypesPath = $"{path}/instancerelationshiptypes.json";
                    instanceStatusesPath = $"{path}/instancestatuses.json";
                    instanceTypesPath = $"{path}/instancetypes.json";
                    institutionsPath = $"{path}/institutions.json";
                    interfacesPath = $"{path}/interfaces.json";
                    if (!api) interfaceCredentialsPath = $"{path}/interfacecredentials.json";
                    invoicesPath = $"{path}/invoices.json";
                    invoiceItemsPath = $"{path}/invoiceitems.json";
                    itemsPath = $"{path}/items.json";
                    itemDamagedStatusesPath = $"{path}/itemdamagedstatuses.json";
                    itemNoteTypesPath = $"{path}/itemnotetypes.json";
                    ledgersPath = $"{path}/ledgers.json";
                    librariesPath = $"{path}/libraries.json";
                    loansPath = $"{path}/loans.json";
                    loanPoliciesPath = $"{path}/loanpolicies.json";
                    loanTypesPath = $"{path}/loantypes.json";
                    locationsPath = $"{path}/locations.json";
                    loginsPath = $"{path}/logins.json";
                    if (!api) marcRecordsPath = $"{path}/marcrecords.json";
                    materialTypesPath = $"{path}/materialtypes.json";
                    modeOfIssuancesPath = $"{path}/modeofissuances.json";
                    natureOfContentTermsPath = $"{path}/natureofcontentterms.json";
                    ordersPath = $"{path}/orders.json";
                    orderItemsPath = $"{path}/orderitems.json";
                    organizationsPath = $"{path}/organizations.json";
                    patronNoticePoliciesPath = $"{path}/patronnoticepolicies.json";
                    permissionsPath = $"{path}/permissions.json";
                    permissionsUsersPath = $"{path}/permissionsusers.json";
                    piecesPath = $"{path}/pieces.json";
                    proxiesPath = $"{path}/proxies.json";
                    if (!api) rawRecordsPath = $"{path}/rawrecords.json";
                    recordsPath = $"{path}/records.json";
                    reportingCodesPath = $"{path}/reportingcodes.json";
                    requestsPath = $"{path}/requests.json";
                    requestPoliciesPath = $"{path}/requestpolicies.json";
                    scheduledNoticesPath = $"{path}/schedulednotices.json";
                    servicePointsPath = $"{path}/servicepoints.json";
                    servicePointUsersPath = $"{path}/servicepointusers.json";
                    snapshotsPath = $"{path}/snapshots.json";
                    staffSlipsPath = $"{path}/staffslips.json";
                    statisticalCodesPath = $"{path}/statisticalcodes.json";
                    statisticalCodeTypesPath = $"{path}/statisticalcodetypes.json";
                    usersPath = $"{path}/users.json";
                    userAcquisitionsUnitsPath = $"{path}/useracquisitionsunits.json";
                    vouchersPath = $"{path}/vouchers.json";
                    voucherItemsPath = $"{path}/voucheritems.json";
                }
                if (allCirculation)
                {
                    cancellationReasonsPath = $"{path}/cancellationreasons.json";
                    fixedDueDateSchedulesPath = $"{path}/fixedduedateschedules.json";
                    loansPath = $"{path}/loans.json";
                    loanPoliciesPath = $"{path}/loanpolicies.json";
                    patronNoticePoliciesPath = $"{path}/patronnoticepolicies.json";
                    requestsPath = $"{path}/requests.json";
                    requestPoliciesPath = $"{path}/requestpolicies.json";
                    scheduledNoticesPath = $"{path}/schedulednotices.json";
                    staffSlipsPath = $"{path}/staffslips.json";
                }
                if (allFinance)
                {
                    budgetsPath = $"{path}/budgets.json";
                    if (!api) encumbrancesPath = $"{path}/encumbrances.json";
                    fiscalYearsPath = $"{path}/fiscalyears.json";
                    fundsPath = $"{path}/funds.json";
                    fundDistributionsPath = $"{path}/funddistributions.json";
                    ledgersPath = $"{path}/ledgers.json";
                }
                if (allInventory)
                {
                    alternativeTitleTypesPath = $"{path}/alternativetitletypes.json";
                    callNumberTypesPath = $"{path}/callnumbertypes.json";
                    campusesPath = $"{path}/campuses.json";
                    classificationTypesPath = $"{path}/classificationtypes.json";
                    contributorNameTypesPath = $"{path}/contributornametypes.json";
                    contributorTypesPath = $"{path}/contributortypes.json";
                    electronicAccessRelationshipsPath = $"{path}/electronicaccessrelationships.json";
                    holdingsPath = $"{path}/holdings.json";
                    holdingNoteTypesPath = $"{path}/holdingnotetypes.json";
                    holdingTypesPath = $"{path}/holdingtypes.json";
                    idTypesPath = $"{path}/idtypes.json";
                    illPoliciesPath = $"{path}/illpolicies.json";
                    instancesPath = $"{path}/instances.json";
                    instanceFormatsPath = $"{path}/instanceformats.json";
                    instanceNoteTypesPath = $"{path}/instancenotetypes.json";
                    instanceRelationshipsPath = $"{path}/instancerelationships.json";
                    instanceRelationshipTypesPath = $"{path}/instancerelationshiptypes.json";
                    instanceStatusesPath = $"{path}/instancestatuses.json";
                    instanceTypesPath = $"{path}/instancetypes.json";
                    institutionsPath = $"{path}/institutions.json";
                    itemsPath = $"{path}/items.json";
                    itemDamagedStatusesPath = $"{path}/itemdamagedstatuses.json";
                    itemNoteTypesPath = $"{path}/itemnotetypes.json";
                    librariesPath = $"{path}/libraries.json";
                    loanTypesPath = $"{path}/loantypes.json";
                    locationsPath = $"{path}/locations.json";
                    materialTypesPath = $"{path}/materialtypes.json";
                    modeOfIssuancesPath = $"{path}/modeofissuances.json";
                    natureOfContentTermsPath = $"{path}/natureofcontentterms.json";
                    servicePointsPath = $"{path}/servicepoints.json";
                    servicePointUsersPath = $"{path}/servicepointusers.json";
                    statisticalCodesPath = $"{path}/statisticalcodes.json";
                    statisticalCodeTypesPath = $"{path}/statisticalcodetypes.json";
                }
                if (allInvoices)
                {
                    if (!api) documentsPath = $"{path}/documents.json";
                    invoicesPath = $"{path}/invoices.json";
                    invoiceItemsPath = $"{path}/invoiceitems.json";
                    vouchersPath = $"{path}/vouchers.json";
                    voucherItemsPath = $"{path}/voucheritems.json";
                }
                if (allLogin)
                {
                    loginsPath = $"{path}/logins.json";
                }
                if (allOrders)
                {
                    acquisitionsUnitsPath = $"{path}/acquisitionsunits.json";
                    alertsPath = $"{path}/alerts.json";
                    ordersPath = $"{path}/orders.json";
                    orderItemsPath = $"{path}/orderitems.json";
                    piecesPath = $"{path}/pieces.json";
                    reportingCodesPath = $"{path}/reportingcodes.json";
                    userAcquisitionsUnitsPath = $"{path}/useracquisitionsunits.json";
                }
                if (allOrganizations)
                {
                    categoriesPath = $"{path}/categories.json";
                    contactsPath = $"{path}/contacts.json";
                    interfacesPath = $"{path}/interfaces.json";
                    if (!api) interfaceCredentialsPath = $"{path}/interfacecredentials.json";
                    organizationsPath = $"{path}/organizations.json";
                }
                if (allPermissions)
                {
                    permissionsPath = $"{path}/permissions.json";
                    permissionsUsersPath = $"{path}/permissionsusers.json";
                }
                if (allSource)
                {
                    if (!api) errorRecordsPath = $"{path}/errorrecords.json";
                    if (!api) marcRecordsPath = $"{path}/marcrecords.json";
                    if (!api) rawRecordsPath = $"{path}/rawrecords.json";
                    recordsPath = $"{path}/records.json";
                    snapshotsPath = $"{path}/snapshots.json";
                }
                if (allUsers)
                {
                    addressTypesPath = $"{path}/addresstypes.json";
                    groupsPath = $"{path}/groups.json";
                    proxiesPath = $"{path}/proxies.json";
                    usersPath = $"{path}/users.json";
                }
                var l = new List<Action>();
                if (save && addressTypesPath != null) l.Add(() => SaveAddressTypes(addressTypesPath, addressTypesWhere));
                if (save && groupsPath != null) l.Add(() => SaveGroups(groupsPath, groupsWhere));
                if (save && usersPath != null) l.Add(() => SaveUsers(usersPath, usersWhere));
                if (save && proxiesPath != null) l.Add(() => SaveProxies(proxiesPath, proxiesWhere));
                if (save && loginsPath != null) l.Add(() => SaveLogins(loginsPath, loginsWhere));
                if (save && permissionsPath != null) l.Add(() => SavePermissions(permissionsPath, permissionsWhere));
                if (save && permissionsUsersPath != null) l.Add(() => SavePermissionsUsers(permissionsUsersPath, permissionsUsersWhere));
                if (save && instanceNoteTypesPath != null) l.Add(() => SaveInstanceNoteTypes(instanceNoteTypesPath, instanceNoteTypesWhere));
                if (save && itemDamagedStatusesPath != null) l.Add(() => SaveItemDamagedStatuses(itemDamagedStatusesPath, itemDamagedStatusesWhere));
                if (save && natureOfContentTermsPath != null) l.Add(() => SaveNatureOfContentTerms(natureOfContentTermsPath, natureOfContentTermsWhere));
                if (save && institutionsPath != null) l.Add(() => SaveInstitutions(institutionsPath, institutionsWhere));
                if (save && campusesPath != null) l.Add(() => SaveCampuses(campusesPath, campusesWhere));
                if (save && librariesPath != null) l.Add(() => SaveLibraries(librariesPath, librariesWhere));
                if (save && servicePointsPath != null) l.Add(() => SaveServicePoints(servicePointsPath, servicePointsWhere));
                if (save && servicePointUsersPath != null) l.Add(() => SaveServicePointUsers(servicePointUsersPath, servicePointUsersWhere));
                if (save && locationsPath != null) l.Add(() => SaveLocations(locationsPath, locationsWhere));
                if (save && alternativeTitleTypesPath != null) l.Add(() => SaveAlternativeTitleTypes(alternativeTitleTypesPath, alternativeTitleTypesWhere));
                if (save && callNumberTypesPath != null) l.Add(() => SaveCallNumberTypes(callNumberTypesPath, callNumberTypesWhere));
                if (save && classificationTypesPath != null) l.Add(() => SaveClassificationTypes(classificationTypesPath, classificationTypesWhere));
                if (save && contributorNameTypesPath != null) l.Add(() => SaveContributorNameTypes(contributorNameTypesPath, contributorNameTypesWhere));
                if (save && contributorTypesPath != null) l.Add(() => SaveContributorTypes(contributorTypesPath, contributorTypesWhere));
                if (save && electronicAccessRelationshipsPath != null) l.Add(() => SaveElectronicAccessRelationships(electronicAccessRelationshipsPath, electronicAccessRelationshipsWhere));
                if (save && holdingNoteTypesPath != null) l.Add(() => SaveHoldingNoteTypes(holdingNoteTypesPath, holdingNoteTypesWhere));
                if (save && holdingTypesPath != null) l.Add(() => SaveHoldingTypes(holdingTypesPath, holdingTypesWhere));
                if (save && idTypesPath != null) l.Add(() => SaveIdTypes(idTypesPath, idTypesWhere));
                if (save && illPoliciesPath != null) l.Add(() => SaveIllPolicies(illPoliciesPath, illPoliciesWhere));
                if (save && instanceFormatsPath != null) l.Add(() => SaveInstanceFormats(instanceFormatsPath, instanceFormatsWhere));
                if (save && instanceRelationshipTypesPath != null) l.Add(() => SaveInstanceRelationshipTypes(instanceRelationshipTypesPath, instanceRelationshipTypesWhere));
                if (save && instanceRelationshipsPath != null) l.Add(() => SaveInstanceRelationships(instanceRelationshipsPath, instanceRelationshipsWhere));
                if (save && instanceStatusesPath != null) l.Add(() => SaveInstanceStatuses(instanceStatusesPath, instanceStatusesWhere));
                if (save && instanceTypesPath != null) l.Add(() => SaveInstanceTypes(instanceTypesPath, instanceTypesWhere));
                if (save && itemNoteTypesPath != null) l.Add(() => SaveItemNoteTypes(itemNoteTypesPath, itemNoteTypesWhere));
                if (save && loanTypesPath != null) l.Add(() => SaveLoanTypes(loanTypesPath, loanTypesWhere));
                if (save && materialTypesPath != null) l.Add(() => SaveMaterialTypes(materialTypesPath, materialTypesWhere));
                if (save && modeOfIssuancesPath != null) l.Add(() => SaveModeOfIssuances(modeOfIssuancesPath, modeOfIssuancesWhere));
                if (save && statisticalCodeTypesPath != null) l.Add(() => SaveStatisticalCodeTypes(statisticalCodeTypesPath, statisticalCodeTypesWhere));
                if (save && statisticalCodesPath != null) l.Add(() => SaveStatisticalCodes(statisticalCodesPath, statisticalCodesWhere));
                if (save && instancesPath != null) l.Add(() => SaveInstances(instancesPath, instancesWhere));
                if (save && holdingsPath != null) l.Add(() => SaveHoldings(holdingsPath, holdingsWhere));
                if (save && itemsPath != null) l.Add(() => SaveItems(itemsPath, itemsWhere));
                if (save && categoriesPath != null) l.Add(() => SaveCategories(categoriesPath, categoriesWhere));
                if (save && contactsPath != null) l.Add(() => SaveContacts(contactsPath, contactsWhere));
                if (save && interfacesPath != null) l.Add(() => SaveInterfaces(interfacesPath, interfacesWhere));
                if (save && interfaceCredentialsPath != null) l.Add(() => SaveInterfaceCredentials(interfaceCredentialsPath, interfaceCredentialsWhere));
                if (save && organizationsPath != null) l.Add(() => SaveOrganizations(organizationsPath, organizationsWhere));
                if (save && acquisitionsUnitsPath != null) l.Add(() => SaveAcquisitionsUnits(acquisitionsUnitsPath, acquisitionsUnitsWhere));
                if (save && reportingCodesPath != null) l.Add(() => SaveReportingCodes(reportingCodesPath, reportingCodesWhere));
                if (save && alertsPath != null) l.Add(() => SaveAlerts(alertsPath, alertsWhere));
                if (save && ordersPath != null) l.Add(() => SaveOrders(ordersPath, ordersWhere));
                if (save && orderItemsPath != null) l.Add(() => SaveOrderItems(orderItemsPath, orderItemsWhere));
                if (save && piecesPath != null) l.Add(() => SavePieces(piecesPath, piecesWhere));
                if (save && userAcquisitionsUnitsPath != null) l.Add(() => SaveUserAcquisitionsUnits(userAcquisitionsUnitsPath, userAcquisitionsUnitsWhere));
                if (save && fiscalYearsPath != null) l.Add(() => SaveFiscalYears(fiscalYearsPath, fiscalYearsWhere));
                if (save && ledgersPath != null) l.Add(() => SaveLedgers(ledgersPath, ledgersWhere));
                if (save && fundsPath != null) l.Add(() => SaveFunds(fundsPath, fundsWhere));
                if (save && budgetsPath != null) l.Add(() => SaveBudgets(budgetsPath, budgetsWhere));
                if (save && encumbrancesPath != null) l.Add(() => SaveEncumbrances(encumbrancesPath, encumbrancesWhere));
                if (save && fundDistributionsPath != null) l.Add(() => SaveFundDistributions(fundDistributionsPath, fundDistributionsWhere));
                if (save && invoicesPath != null) l.Add(() => SaveInvoices(invoicesPath, invoicesWhere));
                if (save && documentsPath != null) l.Add(() => SaveDocuments(documentsPath, documentsWhere));
                if (save && invoiceItemsPath != null) l.Add(() => SaveInvoiceItems(invoiceItemsPath, invoiceItemsWhere));
                if (save && vouchersPath != null) l.Add(() => SaveVouchers(vouchersPath, vouchersWhere));
                if (save && voucherItemsPath != null) l.Add(() => SaveVoucherItems(voucherItemsPath, voucherItemsWhere));
                if (save && cancellationReasonsPath != null) l.Add(() => SaveCancellationReasons(cancellationReasonsPath, cancellationReasonsWhere));
                if (save && fixedDueDateSchedulesPath != null) l.Add(() => SaveFixedDueDateSchedules(fixedDueDateSchedulesPath, fixedDueDateSchedulesWhere));
                if (save && loanPoliciesPath != null) l.Add(() => SaveLoanPolicies(loanPoliciesPath, loanPoliciesWhere));
                if (save && patronNoticePoliciesPath != null) l.Add(() => SavePatronNoticePolicies(patronNoticePoliciesPath, patronNoticePoliciesWhere));
                if (save && requestPoliciesPath != null) l.Add(() => SaveRequestPolicies(requestPoliciesPath, requestPoliciesWhere));
                if (save && loansPath != null) l.Add(() => SaveLoans(loansPath, loansWhere));
                if (save && requestsPath != null) l.Add(() => SaveRequests(requestsPath, requestsWhere));
                if (save && scheduledNoticesPath != null) l.Add(() => SaveScheduledNotices(scheduledNoticesPath, scheduledNoticesWhere));
                if (save && staffSlipsPath != null) l.Add(() => SaveStaffSlips(staffSlipsPath, staffSlipsWhere));
                if (save && snapshotsPath != null) l.Add(() => SaveSnapshots(snapshotsPath, snapshotsWhere));
                if (save && recordsPath != null) l.Add(() => SaveRecords(recordsPath, recordsWhere));
                if (save && rawRecordsPath != null) l.Add(() => SaveRawRecords(rawRecordsPath, rawRecordsWhere));
                if (save && marcRecordsPath != null) l.Add(() => SaveMarcRecords(marcRecordsPath, marcRecordsWhere));
                if (save && errorRecordsPath != null) l.Add(() => SaveErrorRecords(errorRecordsPath, errorRecordsWhere));
                if (threads == 1) foreach (var a in l) a(); else l.AsParallel().WithDegreeOfParallelism(threads ?? Environment.ProcessorCount).ForAll(a => a());
                if (delete && errorRecordsPath != null) DeleteErrorRecords(errorRecordsWhere);
                if (delete && marcRecordsPath != null) DeleteMarcRecords(marcRecordsWhere);
                if (delete && rawRecordsPath != null) DeleteRawRecords(rawRecordsWhere);
                if (delete && recordsPath != null) DeleteRecords(recordsWhere);
                if (delete && snapshotsPath != null) DeleteSnapshots(snapshotsWhere);
                if (delete && staffSlipsPath != null) DeleteStaffSlips(staffSlipsWhere);
                if (delete && scheduledNoticesPath != null) DeleteScheduledNotices(scheduledNoticesWhere);
                if (delete && requestsPath != null) DeleteRequests(requestsWhere);
                if (delete && loansPath != null) DeleteLoans(loansWhere);
                if (delete && requestPoliciesPath != null) DeleteRequestPolicies(requestPoliciesWhere);
                if (delete && patronNoticePoliciesPath != null) DeletePatronNoticePolicies(patronNoticePoliciesWhere);
                if (delete && loanPoliciesPath != null) DeleteLoanPolicies(loanPoliciesWhere);
                if (delete && fixedDueDateSchedulesPath != null) DeleteFixedDueDateSchedules(fixedDueDateSchedulesWhere);
                if (delete && cancellationReasonsPath != null) DeleteCancellationReasons(cancellationReasonsWhere);
                if (delete && voucherItemsPath != null) DeleteVoucherItems(voucherItemsWhere);
                if (delete && vouchersPath != null) DeleteVouchers(vouchersWhere);
                if (delete && invoiceItemsPath != null) DeleteInvoiceItems(invoiceItemsWhere);
                if (delete && documentsPath != null) DeleteDocuments(documentsWhere);
                if (delete && invoicesPath != null) DeleteInvoices(invoicesWhere);
                if (delete && fundDistributionsPath != null) DeleteFundDistributions(fundDistributionsWhere);
                if (delete && encumbrancesPath != null) DeleteEncumbrances(encumbrancesWhere);
                if (delete && budgetsPath != null) DeleteBudgets(budgetsWhere);
                if (delete && fundsPath != null) DeleteFunds(fundsWhere);
                if (delete && ledgersPath != null) DeleteLedgers(ledgersWhere);
                if (delete && fiscalYearsPath != null) DeleteFiscalYears(fiscalYearsWhere);
                if (delete && userAcquisitionsUnitsPath != null) DeleteUserAcquisitionsUnits(userAcquisitionsUnitsWhere);
                if (delete && piecesPath != null) DeletePieces(piecesWhere);
                if (delete && orderItemsPath != null) DeleteOrderItems(orderItemsWhere);
                if (delete && ordersPath != null) DeleteOrders(ordersWhere);
                if (delete && alertsPath != null) DeleteAlerts(alertsWhere);
                if (delete && reportingCodesPath != null) DeleteReportingCodes(reportingCodesWhere);
                if (delete && acquisitionsUnitsPath != null) DeleteAcquisitionsUnits(acquisitionsUnitsWhere);
                if (delete && organizationsPath != null) DeleteOrganizations(organizationsWhere);
                if (delete && interfaceCredentialsPath != null) DeleteInterfaceCredentials(interfaceCredentialsWhere);
                if (delete && interfacesPath != null) DeleteInterfaces(interfacesWhere);
                if (delete && contactsPath != null) DeleteContacts(contactsWhere);
                if (delete && categoriesPath != null) DeleteCategories(categoriesWhere);
                if (delete && itemsPath != null) DeleteItems(itemsWhere);
                if (delete && holdingsPath != null) DeleteHoldings(holdingsWhere);
                if (delete && instancesPath != null) DeleteInstances(instancesWhere);
                if (delete && statisticalCodesPath != null) DeleteStatisticalCodes(statisticalCodesWhere);
                if (delete && statisticalCodeTypesPath != null) DeleteStatisticalCodeTypes(statisticalCodeTypesWhere);
                if (delete && modeOfIssuancesPath != null) DeleteModeOfIssuances(modeOfIssuancesWhere);
                if (delete && materialTypesPath != null) DeleteMaterialTypes(materialTypesWhere);
                if (delete && loanTypesPath != null) DeleteLoanTypes(loanTypesWhere);
                if (delete && itemNoteTypesPath != null) DeleteItemNoteTypes(itemNoteTypesWhere);
                if (delete && instanceTypesPath != null) DeleteInstanceTypes(instanceTypesWhere);
                if (delete && instanceStatusesPath != null) DeleteInstanceStatuses(instanceStatusesWhere);
                if (delete && instanceRelationshipsPath != null) DeleteInstanceRelationships(instanceRelationshipsWhere);
                if (delete && instanceRelationshipTypesPath != null) DeleteInstanceRelationshipTypes(instanceRelationshipTypesWhere);
                if (delete && instanceFormatsPath != null) DeleteInstanceFormats(instanceFormatsWhere);
                if (delete && illPoliciesPath != null) DeleteIllPolicies(illPoliciesWhere);
                if (delete && idTypesPath != null) DeleteIdTypes(idTypesWhere);
                if (delete && holdingTypesPath != null) DeleteHoldingTypes(holdingTypesWhere);
                if (delete && holdingNoteTypesPath != null) DeleteHoldingNoteTypes(holdingNoteTypesWhere);
                if (delete && electronicAccessRelationshipsPath != null) DeleteElectronicAccessRelationships(electronicAccessRelationshipsWhere);
                if (delete && contributorTypesPath != null) DeleteContributorTypes(contributorTypesWhere);
                if (delete && contributorNameTypesPath != null) DeleteContributorNameTypes(contributorNameTypesWhere);
                if (delete && classificationTypesPath != null) DeleteClassificationTypes(classificationTypesWhere);
                if (delete && callNumberTypesPath != null) DeleteCallNumberTypes(callNumberTypesWhere);
                if (delete && alternativeTitleTypesPath != null) DeleteAlternativeTitleTypes(alternativeTitleTypesWhere);
                if (delete && locationsPath != null) DeleteLocations(locationsWhere);
                if (delete && servicePointUsersPath != null) DeleteServicePointUsers(servicePointUsersWhere);
                if (delete && servicePointsPath != null) DeleteServicePoints(servicePointsWhere);
                if (delete && librariesPath != null) DeleteLibraries(librariesWhere);
                if (delete && campusesPath != null) DeleteCampuses(campusesWhere);
                if (delete && institutionsPath != null) DeleteInstitutions(institutionsWhere);
                if (delete && instanceNoteTypesPath != null) DeleteInstanceNoteTypes(instanceNoteTypesWhere);
                if (delete && itemDamagedStatusesPath != null) DeleteItemDamagedStatuses(itemDamagedStatusesWhere);
                if (delete && natureOfContentTermsPath != null) DeleteNatureOfContentTerms(natureOfContentTermsWhere);
                if (delete && permissionsUsersPath != null) DeletePermissionsUsers(permissionsUsersWhere);
                if (delete && permissionsPath != null) DeletePermissions(permissionsWhere);
                if (delete && loginsPath != null) DeleteLogins(loginsWhere);
                if (delete && proxiesPath != null) DeleteProxies(proxiesWhere);
                if (delete && usersPath != null) DeleteUsers(usersWhere);
                if (delete && groupsPath != null) DeleteGroups(groupsWhere);
                if (delete && addressTypesPath != null) DeleteAddressTypes(addressTypesWhere);
                if (load && addressTypesPath != null) LoadAddressTypes(addressTypesPath);
                if (load && groupsPath != null) LoadGroups(groupsPath);
                if (load && usersPath != null) LoadUsers(usersPath);
                if (load && proxiesPath != null) LoadProxies(proxiesPath);
                if (load && loginsPath != null) LoadLogins(loginsPath);
                if (load && permissionsPath != null) LoadPermissions(permissionsPath);
                if (load && permissionsUsersPath != null) LoadPermissionsUsers(permissionsUsersPath);
                if (load && instanceNoteTypesPath != null) LoadInstanceNoteTypes(instanceNoteTypesPath);
                if (load && itemDamagedStatusesPath != null) LoadItemDamagedStatuses(itemDamagedStatusesPath);
                if (load && natureOfContentTermsPath != null) LoadNatureOfContentTerms(natureOfContentTermsPath);
                if (load && institutionsPath != null) LoadInstitutions(institutionsPath);
                if (load && campusesPath != null) LoadCampuses(campusesPath);
                if (load && librariesPath != null) LoadLibraries(librariesPath);
                if (load && servicePointsPath != null) LoadServicePoints(servicePointsPath);
                if (load && servicePointUsersPath != null) LoadServicePointUsers(servicePointUsersPath);
                if (load && locationsPath != null) LoadLocations(locationsPath);
                if (load && alternativeTitleTypesPath != null) LoadAlternativeTitleTypes(alternativeTitleTypesPath);
                if (load && callNumberTypesPath != null) LoadCallNumberTypes(callNumberTypesPath);
                if (load && classificationTypesPath != null) LoadClassificationTypes(classificationTypesPath);
                if (load && contributorNameTypesPath != null) LoadContributorNameTypes(contributorNameTypesPath);
                if (load && contributorTypesPath != null) LoadContributorTypes(contributorTypesPath);
                if (load && electronicAccessRelationshipsPath != null) LoadElectronicAccessRelationships(electronicAccessRelationshipsPath);
                if (load && holdingNoteTypesPath != null) LoadHoldingNoteTypes(holdingNoteTypesPath);
                if (load && holdingTypesPath != null) LoadHoldingTypes(holdingTypesPath);
                if (load && idTypesPath != null) LoadIdTypes(idTypesPath);
                if (load && illPoliciesPath != null) LoadIllPolicies(illPoliciesPath);
                if (load && instanceFormatsPath != null) LoadInstanceFormats(instanceFormatsPath);
                if (load && instanceRelationshipTypesPath != null) LoadInstanceRelationshipTypes(instanceRelationshipTypesPath);
                if (load && instanceRelationshipsPath != null) LoadInstanceRelationships(instanceRelationshipsPath);
                if (load && instanceStatusesPath != null) LoadInstanceStatuses(instanceStatusesPath);
                if (load && instanceTypesPath != null) LoadInstanceTypes(instanceTypesPath);
                if (load && itemNoteTypesPath != null) LoadItemNoteTypes(itemNoteTypesPath);
                if (load && loanTypesPath != null) LoadLoanTypes(loanTypesPath);
                if (load && materialTypesPath != null) LoadMaterialTypes(materialTypesPath);
                if (load && modeOfIssuancesPath != null) LoadModeOfIssuances(modeOfIssuancesPath);
                if (load && statisticalCodeTypesPath != null) LoadStatisticalCodeTypes(statisticalCodeTypesPath);
                if (load && statisticalCodesPath != null) LoadStatisticalCodes(statisticalCodesPath);
                if (load && instancesPath != null) LoadInstances(instancesPath);
                if (load && holdingsPath != null) LoadHoldings(holdingsPath);
                if (load && itemsPath != null) LoadItems(itemsPath);
                if (load && categoriesPath != null) LoadCategories(categoriesPath);
                if (load && contactsPath != null) LoadContacts(contactsPath);
                if (load && interfacesPath != null) LoadInterfaces(interfacesPath);
                if (load && interfaceCredentialsPath != null) LoadInterfaceCredentials(interfaceCredentialsPath);
                if (load && organizationsPath != null) LoadOrganizations(organizationsPath);
                if (load && acquisitionsUnitsPath != null) LoadAcquisitionsUnits(acquisitionsUnitsPath);
                if (load && reportingCodesPath != null) LoadReportingCodes(reportingCodesPath);
                if (load && alertsPath != null) LoadAlerts(alertsPath);
                if (load && ordersPath != null) LoadOrders(ordersPath);
                if (load && orderItemsPath != null) LoadOrderItems(orderItemsPath);
                if (load && piecesPath != null) LoadPieces(piecesPath);
                if (load && userAcquisitionsUnitsPath != null) LoadUserAcquisitionsUnits(userAcquisitionsUnitsPath);
                if (load && fiscalYearsPath != null) LoadFiscalYears(fiscalYearsPath);
                if (load && ledgersPath != null) LoadLedgers(ledgersPath);
                if (load && fundsPath != null) LoadFunds(fundsPath);
                if (load && budgetsPath != null) LoadBudgets(budgetsPath);
                if (load && encumbrancesPath != null) LoadEncumbrances(encumbrancesPath);
                if (load && fundDistributionsPath != null) LoadFundDistributions(fundDistributionsPath);
                if (load && invoicesPath != null) LoadInvoices(invoicesPath);
                if (load && documentsPath != null) LoadDocuments(documentsPath);
                if (load && invoiceItemsPath != null) LoadInvoiceItems(invoiceItemsPath);
                if (load && vouchersPath != null) LoadVouchers(vouchersPath);
                if (load && voucherItemsPath != null) LoadVoucherItems(voucherItemsPath);
                if (load && cancellationReasonsPath != null) LoadCancellationReasons(cancellationReasonsPath);
                if (load && fixedDueDateSchedulesPath != null) LoadFixedDueDateSchedules(fixedDueDateSchedulesPath);
                if (load && loanPoliciesPath != null) LoadLoanPolicies(loanPoliciesPath);
                if (load && patronNoticePoliciesPath != null) LoadPatronNoticePolicies(patronNoticePoliciesPath);
                if (load && requestPoliciesPath != null) LoadRequestPolicies(requestPoliciesPath);
                if (load && loansPath != null) LoadLoans(loansPath);
                if (load && requestsPath != null) LoadRequests(requestsPath);
                if (load && scheduledNoticesPath != null) LoadScheduledNotices(scheduledNoticesPath);
                if (load && staffSlipsPath != null) LoadStaffSlips(staffSlipsPath);
                if (load && snapshotsPath != null) LoadSnapshots(snapshotsPath);
                if (load && recordsPath != null) LoadRecords(recordsPath);
                if (load && rawRecordsPath != null) LoadRawRecords(rawRecordsPath);
                if (load && marcRecordsPath != null) LoadMarcRecords(marcRecordsPath);
                if (load && errorRecordsPath != null) LoadErrorRecords(errorRecordsPath);
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Critical, 0, e.ToString());
                return -1;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, "Ending");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
            }
            return 0;
        }

        public static void Initialize()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectionStrings.config");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "<connectionStrings>\r\n  <add name=\"FolioContext\" providerName=\"Npgsql\" connectionString=\"Host=localhost;Username=postgres;Password=;Database=folio\" />\r\n</connectionStrings>");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Created {path}");
            }
            path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AppSettings.config");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "<appSettings>\r\n  <add key=\"url\" value=\"http://localhost:9130\"/>\r\n  <add key=\"tenant\" value=\"diku\"/>\r\n  <add key=\"username\" value=\"diku_admin\"/>\r\n  <add key=\"password\" value=\"\"/>\r\n</appSettings>");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Created {path}");
            }
        }

        public static void DeleteAcquisitionsUnits(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting acquisitions units");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.AcquisitionsUnits(where))
                    {
                        if (!whatIf) fsc.DeleteAcquisitionsUnit((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.acquisitions_unit{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadAcquisitionsUnits(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading acquisitions units");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AcquisitionsUnit.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertAcquisitionsUnit(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new AcquisitionsUnit
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveAcquisitionsUnits(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving acquisitions units");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AcquisitionsUnit.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.AcquisitionsUnits(where, take: take) : fdc.AcquisitionsUnits(where, take: take).Select(au => JObject.Parse(au.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteAddressTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting address types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.AddressTypes(where))
                    {
                        if (!whatIf) fsc.DeleteAddressType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_users.addresstype{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} address types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadAddressTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading address types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AddressType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AddressType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AddressType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertAddressType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new AddressType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} address types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveAddressTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving address types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AddressType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.AddressTypes(where, take: take) : fdc.AddressTypes(where, take: take).Select(at => JObject.Parse(at.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AddressType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AddressType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} address types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteAlerts(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting alerts");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Alerts(where))
                    {
                        if (!whatIf) fsc.DeleteAlert((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.alert{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} alerts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadAlerts(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading alerts");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Alert.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Alert {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Alert {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertAlert(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Alert
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} alerts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveAlerts(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving alerts");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Alert.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Alerts(where, take: take) : fdc.Alerts(where, take: take).Select(a => JObject.Parse(a.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Alert {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Alert {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} alerts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteAlternativeTitleTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting alternative title types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.AlternativeTitleTypes(where))
                    {
                        if (!whatIf) fsc.DeleteAlternativeTitleType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.alternative_title_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} alternative title types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadAlternativeTitleTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading alternative title types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AlternativeTitleType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AlternativeTitleType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AlternativeTitleType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertAlternativeTitleType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new AlternativeTitleType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} alternative title types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveAlternativeTitleTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving alternative title types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.AlternativeTitleType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.AlternativeTitleTypes(where, take: take) : fdc.AlternativeTitleTypes(where, take: take).Select(att => JObject.Parse(att.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"AlternativeTitleType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"AlternativeTitleType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} alternative title types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteBudgets(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting budgets");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Budgets(where))
                    {
                        if (!whatIf) fsc.DeleteBudget((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.budget{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} budgets");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadBudgets(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading budgets");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Budget.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Budget {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Budget {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertBudget(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Budget
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            FundId = (Guid?)jo.SelectToken("fundId"),
                            FiscalYearId = (Guid?)jo.SelectToken("fiscalYearId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} budgets");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveBudgets(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving budgets");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Budget.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Budgets(where, take: take) : fdc.Budgets(where, take: take).Select(b => JObject.Parse(b.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Budget {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Budget {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} budgets");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCallNumberTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting call number types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.CallNumberTypes(where))
                    {
                        if (!whatIf) fsc.DeleteCallNumberType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.call_number_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} call number types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadCallNumberTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading call number types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.CallNumberType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"CallNumberType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"CallNumberType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertCallNumberType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new CallNumberType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} call number types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveCallNumberTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving call number types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.CallNumberType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.CallNumberTypes(where, take: take) : fdc.CallNumberTypes(where, take: take).Select(cnt => JObject.Parse(cnt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"CallNumberType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"CallNumberType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} call number types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCampuses(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting campuses");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Campuses(where))
                    {
                        if (!whatIf) fsc.DeleteCampus((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.loccampus{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} campuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadCampuses(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading campuses");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Campus.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Campus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Campus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertCampus(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Campus
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Institutionid = (Guid?)jo.SelectToken("institutionId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} campuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveCampuses(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving campuses");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Campus.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Campuses(where, take: take) : fdc.Campuses(where, take: take).Select(c => JObject.Parse(c.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Campus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Campus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} campuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCancellationReasons(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting cancellation reasons");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.CancellationReasons(where))
                    {
                        if (!whatIf) fsc.DeleteCancellationReason((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.cancellation_reason{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} cancellation reasons");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadCancellationReasons(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading cancellation reasons");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.CancellationReason.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"CancellationReason {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"CancellationReason {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertCancellationReason(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new CancellationReason
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} cancellation reasons");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveCancellationReasons(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving cancellation reasons");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.CancellationReason.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.CancellationReasons(where, take: take) : fdc.CancellationReasons(where, take: take).Select(cr => JObject.Parse(cr.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"CancellationReason {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"CancellationReason {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} cancellation reasons");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCategories(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting categories");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Categories(where))
                    {
                        if (!whatIf) fsc.DeleteCategory((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_organizations_storage.categories{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} categories");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadCategories(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading categories");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Category.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Category {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Category {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertCategory(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Category
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} categories");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveCategories(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving categories");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Category.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Categories(where, take: take) : fdc.Categories(where, take: take).Select(c => JObject.Parse(c.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Category {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Category {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} categories");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteClassificationTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting classification types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ClassificationTypes(where))
                    {
                        if (!whatIf) fsc.DeleteClassificationType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.classification_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} classification types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadClassificationTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading classification types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ClassificationType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ClassificationType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ClassificationType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertClassificationType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ClassificationType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} classification types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveClassificationTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving classification types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ClassificationType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ClassificationTypes(where, take: take) : fdc.ClassificationTypes(where, take: take).Select(ct => JObject.Parse(ct.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ClassificationType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ClassificationType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} classification types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteContacts(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting contacts");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Contacts(where))
                    {
                        if (!whatIf) fsc.DeleteContact((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_organizations_storage.contacts{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} contacts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadContacts(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading contacts");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Contact.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Contact {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Contact {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertContact(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Contact
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} contacts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveContacts(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving contacts");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Contact.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Contacts(where, take: take) : fdc.Contacts(where, take: take).Select(c => JObject.Parse(c.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Contact {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Contact {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} contacts");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteContributorNameTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting contributor name types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ContributorNameTypes(where))
                    {
                        if (!whatIf) fsc.DeleteContributorNameType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.contributor_name_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} contributor name types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadContributorNameTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading contributor name types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ContributorNameType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ContributorNameType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ContributorNameType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertContributorNameType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ContributorNameType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} contributor name types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveContributorNameTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving contributor name types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ContributorNameType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ContributorNameTypes(where, take: take) : fdc.ContributorNameTypes(where, take: take).Select(cnt => JObject.Parse(cnt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ContributorNameType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ContributorNameType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} contributor name types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteContributorTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting contributor types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ContributorTypes(where))
                    {
                        if (!whatIf) fsc.DeleteContributorType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.contributor_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} contributor types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadContributorTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading contributor types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ContributorType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ContributorType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ContributorType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertContributorType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ContributorType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} contributor types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveContributorTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving contributor types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ContributorType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ContributorTypes(where, take: take) : fdc.ContributorTypes(where, take: take).Select(ct => JObject.Parse(ct.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ContributorType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ContributorType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} contributor types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteDocuments(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting documents");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_invoice_storage.documents{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} documents");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadDocuments(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading documents");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Document.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Document {jo["documentMetadata.id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Document {jo["documentMetadata.id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Document
                        {
                            Id = (Guid?)jo.SelectToken("documentMetadata.id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("documentMetadata.metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("documentMetadata.metadata.createdByUserId"),
                            Invoiceid = (Guid?)jo.SelectToken("documentMetadata.invoiceId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} documents");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveDocuments(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving documents");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Document.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.Documents(where, take: take).Select(d => JObject.Parse(d.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Document {jo["documentMetadata.id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Document {jo["documentMetadata.id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} documents");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteElectronicAccessRelationships(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting electronic access relationships");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ElectronicAccessRelationships(where))
                    {
                        if (!whatIf) fsc.DeleteElectronicAccessRelationship((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.electronic_access_relationship{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} electronic access relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadElectronicAccessRelationships(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading electronic access relationships");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ElectronicAccessRelationship.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ElectronicAccessRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ElectronicAccessRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertElectronicAccessRelationship(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ElectronicAccessRelationship
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} electronic access relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveElectronicAccessRelationships(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving electronic access relationships");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ElectronicAccessRelationship.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ElectronicAccessRelationships(where, take: take) : fdc.ElectronicAccessRelationships(where, take: take).Select(ear => JObject.Parse(ear.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ElectronicAccessRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ElectronicAccessRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} electronic access relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteEncumbrances(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting encumbrances");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.encumbrance{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} encumbrances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadEncumbrances(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading encumbrances");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Encumbrance.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Encumbrance {jo[""]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Encumbrance {jo[""]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Encumbrance
                        {
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} encumbrances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveEncumbrances(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving encumbrances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Encumbrance.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.Encumbrances(where, take: take).Select(e2 => JObject.Parse(e2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Encumbrance {jo[""]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Encumbrance {jo[""]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} encumbrances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteErrorRecords(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting error records");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_source_record_storage.error_records{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} error records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadErrorRecords(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading error records");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ErrorRecord.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ErrorRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ErrorRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ErrorRecord
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} error records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveErrorRecords(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving error records");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ErrorRecord.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.ErrorRecords(where, take: take).Select(er => JObject.Parse(er.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ErrorRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ErrorRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} error records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteFiscalYears(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting fiscal years");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.FiscalYears(where))
                    {
                        if (!whatIf) fsc.DeleteFiscalYear((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.fiscal_year{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} fiscal years");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadFiscalYears(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading fiscal years");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FiscalYear.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FiscalYear {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FiscalYear {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertFiscalYear(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new FiscalYear
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} fiscal years");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveFiscalYears(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving fiscal years");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FiscalYear.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FiscalYears(where, take: take) : fdc.FiscalYears(where, take: take).Select(fy => JObject.Parse(fy.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FiscalYear {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FiscalYear {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} fiscal years");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteFixedDueDateSchedules(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting fixed due date schedules");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.FixedDueDateSchedules(where))
                    {
                        if (!whatIf) fsc.DeleteFixedDueDateSchedule((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.fixed_due_date_schedule{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} fixed due date schedules");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadFixedDueDateSchedules(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading fixed due date schedules");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FixedDueDateSchedule.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FixedDueDateSchedule {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FixedDueDateSchedule {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertFixedDueDateSchedule(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new FixedDueDateSchedule
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} fixed due date schedules");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveFixedDueDateSchedules(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving fixed due date schedules");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FixedDueDateSchedule.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FixedDueDateSchedules(where, take: take) : fdc.FixedDueDateSchedules(where, take: take).Select(fdds => JObject.Parse(fdds.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FixedDueDateSchedule {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FixedDueDateSchedule {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} fixed due date schedules");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteFunds(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting funds");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Funds(where))
                    {
                        if (!whatIf) fsc.DeleteFund((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.fund{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} funds");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadFunds(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading funds");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Fund.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Fund {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Fund {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertFund(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Fund
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            LedgerId = (Guid?)jo.SelectToken("ledgerId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} funds");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveFunds(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving funds");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Fund.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Funds(where, take: take) : fdc.Funds(where, take: take).Select(f => JObject.Parse(f.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Fund {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Fund {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} funds");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteFundDistributions(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting fund distributions");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.FundDistributions(where))
                    {
                        if (!whatIf) fsc.DeleteFundDistribution((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.fund_distribution{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} fund distributions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadFundDistributions(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading fund distributions");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FundDistribution.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FundDistribution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FundDistribution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertFundDistribution(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new FundDistribution
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            Budgetid = (Guid?)jo.SelectToken("budgetId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} fund distributions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveFundDistributions(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving fund distributions");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.FundDistribution.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FundDistributions(where, take: take) : fdc.FundDistributions(where, take: take).Select(fd => JObject.Parse(fd.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"FundDistribution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"FundDistribution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} fund distributions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteGroups(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting groups");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Groups(where))
                    {
                        if (!whatIf) fsc.DeleteGroup((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_users.groups{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} groups");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadGroups(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading groups");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Group.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Group {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Group {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertGroup(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Group
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} groups");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveGroups(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving groups");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Group.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Groups(where, take: take) : fdc.Groups(where, take: take).Select(g => JObject.Parse(g.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Group {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Group {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} groups");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldings(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holdings");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Holdings(where))
                    {
                        if (!whatIf) fsc.DeleteHolding((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.holdings_record{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} holdings");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadHoldings(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading holdings");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Holding.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Holding {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Holding {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertHolding(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Holding
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Instanceid = (Guid?)jo.SelectToken("instanceId"),
                            Permanentlocationid = (Guid?)jo.SelectToken("permanentLocationId"),
                            Temporarylocationid = (Guid?)jo.SelectToken("temporaryLocationId"),
                            Holdingstypeid = (Guid?)jo.SelectToken("holdingsTypeId"),
                            Callnumbertypeid = (Guid?)jo.SelectToken("callNumberTypeId"),
                            Illpolicyid = (Guid?)jo.SelectToken("illPolicyId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} holdings");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveHoldings(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving holdings");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Holding.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Holdings(where, take: take) : fdc.Holdings(where, take: take).Select(h => JObject.Parse(h.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Holding {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Holding {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} holdings");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldingNoteTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holding note types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.HoldingNoteTypes(where))
                    {
                        if (!whatIf) fsc.DeleteHoldingNoteType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.holdings_note_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} holding note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadHoldingNoteTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading holding note types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.HoldingNoteType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"HoldingNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"HoldingNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertHoldingNoteType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new HoldingNoteType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} holding note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveHoldingNoteTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving holding note types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.HoldingNoteType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.HoldingNoteTypes(where, take: take) : fdc.HoldingNoteTypes(where, take: take).Select(hnt => JObject.Parse(hnt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"HoldingNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"HoldingNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} holding note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldingTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holding types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.HoldingTypes(where))
                    {
                        if (!whatIf) fsc.DeleteHoldingType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.holdings_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} holding types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadHoldingTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading holding types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.HoldingType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"HoldingType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"HoldingType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertHoldingType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new HoldingType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} holding types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveHoldingTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving holding types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.HoldingType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.HoldingTypes(where, take: take) : fdc.HoldingTypes(where, take: take).Select(ht => JObject.Parse(ht.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"HoldingType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"HoldingType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} holding types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteIdTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting id types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.IdTypes(where))
                    {
                        if (!whatIf) fsc.DeleteIdType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.identifier_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} id types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadIdTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading id types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.IdType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"IdType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"IdType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertIdType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new IdType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} id types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveIdTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving id types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.IdType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.IdTypes(where, take: take) : fdc.IdTypes(where, take: take).Select(it => JObject.Parse(it.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"IdType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"IdType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} id types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteIllPolicies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting ill policies");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.IllPolicies(where))
                    {
                        if (!whatIf) fsc.DeleteIllPolicy((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.ill_policy{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} ill policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadIllPolicies(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading ill policies");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.IllPolicy.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"IllPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"IllPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertIllPolicy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new IllPolicy
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} ill policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveIllPolicies(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving ill policies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.IllPolicy.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.IllPolicies(where, take: take) : fdc.IllPolicies(where, take: take).Select(ip => JObject.Parse(ip.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"IllPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"IllPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} ill policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstances(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instances");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Instances(where))
                    {
                        if (!whatIf) fsc.DeleteInstance((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstances(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instances");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Instance.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Instance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Instance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstance(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Instance
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Instancestatusid = (Guid?)jo.SelectToken("statusId"),
                            Modeofissuanceid = (Guid?)jo.SelectToken("modeOfIssuanceId"),
                            Instancetypeid = (Guid?)jo.SelectToken("instanceTypeId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstances(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Instance.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Instances(where, take: take) : fdc.Instances(where, take: take).Select(i2 => JObject.Parse(i2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Instance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Instance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceFormats(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance formats");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceFormats(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceFormat((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_format{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance formats");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceFormats(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance formats");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceFormat.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceFormat {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceFormat {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceFormat(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceFormat
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance formats");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceFormats(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance formats");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceFormat.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceFormats(where, take: take) : fdc.InstanceFormats(where, take: take).Select(@if => JObject.Parse(@if.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceFormat {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceFormat {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance formats");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceNoteTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance note types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceNoteTypes(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceNoteType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_note_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceNoteTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance note types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceNoteType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceNoteType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceNoteType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceNoteTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance note types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceNoteType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceNoteTypes(where, take: take) : fdc.InstanceNoteTypes(where, take: take).Select(@int => JObject.Parse(@int.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceRelationships(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance relationships");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceRelationships(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceRelationship((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_relationship{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceRelationships(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance relationships");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceRelationship.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceRelationship(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceRelationship
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Superinstanceid = (Guid?)jo.SelectToken("superInstanceId"),
                            Subinstanceid = (Guid?)jo.SelectToken("subInstanceId"),
                            Instancerelationshiptypeid = (Guid?)jo.SelectToken("instanceRelationshipTypeId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceRelationships(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance relationships");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceRelationship.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceRelationships(where, take: take) : fdc.InstanceRelationships(where, take: take).Select(ir => JObject.Parse(ir.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceRelationship {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance relationships");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceRelationshipTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance relationship types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceRelationshipTypes(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceRelationshipType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_relationship_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance relationship types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceRelationshipTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance relationship types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceRelationshipType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceRelationshipType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceRelationshipType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceRelationshipType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceRelationshipType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance relationship types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceRelationshipTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance relationship types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceRelationshipType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceRelationshipTypes(where, take: take) : fdc.InstanceRelationshipTypes(where, take: take).Select(irt => JObject.Parse(irt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceRelationshipType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceRelationshipType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance relationship types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceStatuses(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance statuses");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceStatuses(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceStatus((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_status{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceStatuses(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance statuses");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceStatus.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceStatus(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceStatus
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceStatuses(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance statuses");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceStatus.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceStatuses(where, take: take) : fdc.InstanceStatuses(where, take: take).Select(@is => JObject.Parse(@is.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceTypes(where))
                    {
                        if (!whatIf) fsc.DeleteInstanceType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.instance_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} instance types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstanceTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading instance types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstanceType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InstanceType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} instance types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstanceTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving instance types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InstanceType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceTypes(where, take: take) : fdc.InstanceTypes(where, take: take).Select(it => JObject.Parse(it.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InstanceType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InstanceType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} instance types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstitutions(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting institutions");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Institutions(where))
                    {
                        if (!whatIf) fsc.DeleteInstitution((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.locinstitution{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} institutions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInstitutions(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading institutions");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Institution.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Institution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Institution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInstitution(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Institution
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} institutions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInstitutions(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving institutions");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Institution.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Institutions(where, take: take) : fdc.Institutions(where, take: take).Select(i2 => JObject.Parse(i2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Institution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Institution {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} institutions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInterfaces(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting interfaces");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Interfaces(where))
                    {
                        if (!whatIf) fsc.DeleteInterface((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_organizations_storage.interfaces{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} interfaces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInterfaces(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading interfaces");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Interface.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Interface {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Interface {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInterface(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Interface
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} interfaces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInterfaces(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving interfaces");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Interface.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Interfaces(where, take: take) : fdc.Interfaces(where, take: take).Select(i2 => JObject.Parse(i2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Interface {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Interface {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} interfaces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInterfaceCredentials(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting interface credentials");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_organizations_storage.interface_credentials{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} interface credentials");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInterfaceCredentials(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading interface credentials");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InterfaceCredential.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InterfaceCredential {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InterfaceCredential {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InterfaceCredential
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            Interfaceid = (Guid?)jo.SelectToken("interfaceId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} interface credentials");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInterfaceCredentials(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving interface credentials");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InterfaceCredential.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.InterfaceCredentials(where, take: take).Select(ic => JObject.Parse(ic.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InterfaceCredential {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InterfaceCredential {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} interface credentials");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInvoices(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting invoices");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Invoices(where))
                    {
                        if (!whatIf) fsc.DeleteInvoice((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_invoice_storage.invoices{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} invoices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInvoices(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading invoices");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Invoice.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Invoice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Invoice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInvoice(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Invoice
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} invoices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInvoices(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving invoices");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Invoice.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Invoices(where, take: take) : fdc.Invoices(where, take: take).Select(i2 => JObject.Parse(i2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Invoice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Invoice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} invoices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInvoiceItems(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting invoice items");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InvoiceItems(where))
                    {
                        if (!whatIf) fsc.DeleteInvoiceItem((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_invoice_storage.invoice_lines{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} invoice items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadInvoiceItems(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading invoice items");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InvoiceItem.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InvoiceItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InvoiceItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertInvoiceItem(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new InvoiceItem
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Invoiceid = (Guid?)jo.SelectToken("invoiceId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} invoice items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveInvoiceItems(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving invoice items");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.InvoiceItem.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InvoiceItems(where, take: take) : fdc.InvoiceItems(where, take: take).Select(ii => JObject.Parse(ii.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"InvoiceItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"InvoiceItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} invoice items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteItems(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting items");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Items(where))
                    {
                        if (!whatIf) fsc.DeleteItem((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.item{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadItems(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading items");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Item.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Item {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Item {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertItem(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Item
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Holdingsrecordid = (Guid?)jo.SelectToken("holdingsRecordId"),
                            Permanentloantypeid = (Guid?)jo.SelectToken("permanentLoanTypeId"),
                            Temporaryloantypeid = (Guid?)jo.SelectToken("temporaryLoanTypeId"),
                            Materialtypeid = (Guid?)jo.SelectToken("materialTypeId"),
                            Permanentlocationid = (Guid?)jo.SelectToken("permanentLocationId"),
                            Temporarylocationid = (Guid?)jo.SelectToken("temporaryLocationId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveItems(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving items");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Item.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Items(where, take: take) : fdc.Items(where, take: take).Select(i2 => JObject.Parse(i2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Item {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Item {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteItemDamagedStatuses(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting item damaged statuses");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ItemDamagedStatuses(where))
                    {
                        if (!whatIf) fsc.DeleteItemDamagedStatus((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.item_damaged_status{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} item damaged statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadItemDamagedStatuses(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading item damaged statuses");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ItemDamagedStatus.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ItemDamagedStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ItemDamagedStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertItemDamagedStatus(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ItemDamagedStatus
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} item damaged statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveItemDamagedStatuses(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving item damaged statuses");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ItemDamagedStatus.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ItemDamagedStatuses(where, take: take) : fdc.ItemDamagedStatuses(where, take: take).Select(ids => JObject.Parse(ids.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ItemDamagedStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ItemDamagedStatus {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} item damaged statuses");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteItemNoteTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting item note types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ItemNoteTypes(where))
                    {
                        if (!whatIf) fsc.DeleteItemNoteType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.item_note_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} item note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadItemNoteTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading item note types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ItemNoteType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ItemNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ItemNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertItemNoteType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ItemNoteType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} item note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveItemNoteTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving item note types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ItemNoteType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ItemNoteTypes(where, take: take) : fdc.ItemNoteTypes(where, take: take).Select(@int => JObject.Parse(@int.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ItemNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ItemNoteType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} item note types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLedgers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting ledgers");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Ledgers(where))
                    {
                        if (!whatIf) fsc.DeleteLedger((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_finance_storage.ledger{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} ledgers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLedgers(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading ledgers");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Ledger.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Ledger {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Ledger {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLedger(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Ledger
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} ledgers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLedgers(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving ledgers");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Ledger.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Ledgers(where, take: take) : fdc.Ledgers(where, take: take).Select(l => JObject.Parse(l.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Ledger {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Ledger {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} ledgers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLibraries(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting libraries");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Libraries(where))
                    {
                        if (!whatIf) fsc.DeleteLibrary((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.loclibrary{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} libraries");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLibraries(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading libraries");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Library.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Library {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Library {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLibrary(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Library
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Campusid = (Guid?)jo.SelectToken("campusId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} libraries");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLibraries(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving libraries");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Library.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Libraries(where, take: take) : fdc.Libraries(where, take: take).Select(l => JObject.Parse(l.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Library {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Library {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} libraries");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLoans(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting loans");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Loans(where))
                    {
                        if (!whatIf) fsc.DeleteLoan((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.loan{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} loans");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLoans(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading loans");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Loan.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Loan {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Loan {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLoan(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Loan
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} loans");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLoans(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving loans");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Loan.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Loans(where, take: take) : fdc.Loans(where, take: take).Select(l => JObject.Parse(l.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Loan {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Loan {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} loans");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLoanPolicies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting loan policies");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.LoanPolicies(where))
                    {
                        if (!whatIf) fsc.DeleteLoanPolicy((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.loan_policy{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} loan policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLoanPolicies(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading loan policies");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.LoanPolicy.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"LoanPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"LoanPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLoanPolicy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new LoanPolicy
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            LoanspolicyFixedduedatescheduleid = (Guid?)jo.SelectToken("loansPolicy.fixedDueDateScheduleId"),
                            RenewalspolicyAlternatefixedduedatescheduleid = (Guid?)jo.SelectToken("renewalsPolicy.alternateFixedDueDateScheduleId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} loan policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLoanPolicies(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving loan policies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.LoanPolicy.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.LoanPolicies(where, take: take) : fdc.LoanPolicies(where, take: take).Select(lp => JObject.Parse(lp.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"LoanPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"LoanPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} loan policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLoanTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting loan types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.LoanTypes(where))
                    {
                        if (!whatIf) fsc.DeleteLoanType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.loan_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} loan types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLoanTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading loan types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.LoanType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"LoanType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"LoanType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLoanType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new LoanType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} loan types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLoanTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving loan types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.LoanType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.LoanTypes(where, take: take) : fdc.LoanTypes(where, take: take).Select(lt => JObject.Parse(lt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"LoanType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"LoanType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} loan types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLocations(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting locations");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Locations(where))
                    {
                        if (!whatIf) fsc.DeleteLocation((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.location{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} locations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLocations(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading locations");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Location.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Location {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Location {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLocation(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Location
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Institutionid = (Guid?)jo.SelectToken("institutionId"),
                            Campusid = (Guid?)jo.SelectToken("campusId"),
                            Libraryid = (Guid?)jo.SelectToken("libraryId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} locations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLocations(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving locations");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Location.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Locations(where, take: take) : fdc.Locations(where, take: take).Select(l => JObject.Parse(l.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Location {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Location {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} locations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLogins(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting logins");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Logins(where))
                    {
                        if (!whatIf) fsc.DeleteLogin((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_login.auth_credentials{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} logins");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadLogins(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading logins");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Login.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Login {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Login {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertLogin(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Login
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} logins");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveLogins(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving logins");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Login.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Logins(where, take: take) : fdc.Logins(where, take: take).Select(l => JObject.Parse(l.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Login {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Login {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} logins");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteMarcRecords(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting marc records");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_source_record_storage.marc_records{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} marc records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadMarcRecords(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading marc records");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.MarcRecord.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"MarcRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"MarcRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new MarcRecord
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} marc records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveMarcRecords(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving marc records");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.MarcRecord.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.MarcRecords(where, take: take).Select(mr => JObject.Parse(mr.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"MarcRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"MarcRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} marc records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteMaterialTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting material types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.MaterialTypes(where))
                    {
                        if (!whatIf) fsc.DeleteMaterialType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.material_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} material types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadMaterialTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading material types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.MaterialType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"MaterialType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"MaterialType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertMaterialType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new MaterialType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} material types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveMaterialTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving material types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.MaterialType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.MaterialTypes(where, take: take) : fdc.MaterialTypes(where, take: take).Select(mt => JObject.Parse(mt.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"MaterialType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"MaterialType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} material types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteModeOfIssuances(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting mode of issuances");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ModeOfIssuances(where))
                    {
                        if (!whatIf) fsc.DeleteModeOfIssuance((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.mode_of_issuance{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} mode of issuances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadModeOfIssuances(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading mode of issuances");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ModeOfIssuance.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ModeOfIssuance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ModeOfIssuance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertModeOfIssuance(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ModeOfIssuance
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} mode of issuances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveModeOfIssuances(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving mode of issuances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ModeOfIssuance.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ModeOfIssuances(where, take: take) : fdc.ModeOfIssuances(where, take: take).Select(moi => JObject.Parse(moi.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ModeOfIssuance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ModeOfIssuance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} mode of issuances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteNatureOfContentTerms(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting nature of content terms");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.NatureOfContentTerms(where))
                    {
                        if (!whatIf) fsc.DeleteNatureOfContentTerm((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.nature_of_content_term{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} nature of content terms");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadNatureOfContentTerms(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading nature of content terms");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.NatureOfContentTerm.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"NatureOfContentTerm {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"NatureOfContentTerm {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertNatureOfContentTerm(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new NatureOfContentTerm
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} nature of content terms");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveNatureOfContentTerms(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving nature of content terms");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.NatureOfContentTerm.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.NatureOfContentTerms(where, take: take) : fdc.NatureOfContentTerms(where, take: take).Select(noct => JObject.Parse(noct.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"NatureOfContentTerm {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"NatureOfContentTerm {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} nature of content terms");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteOrders(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting orders");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Orders(where))
                    {
                        if (!whatIf) fsc.DeleteOrder((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.purchase_order{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} orders");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadOrders(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading orders");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Order.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Order {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Order {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertOrder(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Order
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} orders");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveOrders(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving orders");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Order.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Orders(where, take: take) : fdc.Orders(where, take: take).Select(o => JObject.Parse(o.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Order {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Order {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} orders");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteOrderItems(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting order items");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.OrderItems(where))
                    {
                        if (!whatIf) fsc.DeleteOrderItem((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.po_line{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} order items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadOrderItems(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading order items");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.OrderItem.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"OrderItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"OrderItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertOrderItem(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new OrderItem
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Purchaseorderid = (Guid?)jo.SelectToken("purchaseOrderId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} order items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveOrderItems(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving order items");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.OrderItem.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.OrderItems(where, take: take) : fdc.OrderItems(where, take: take).Select(oi => JObject.Parse(oi.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"OrderItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"OrderItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} order items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteOrganizations(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting organizations");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Organizations(where))
                    {
                        if (!whatIf) fsc.DeleteOrganization((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_organizations_storage.organizations{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} organizations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadOrganizations(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading organizations");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Organization.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Organization {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Organization {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertOrganization(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Organization
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} organizations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveOrganizations(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving organizations");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Organization.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Organizations(where, take: take) : fdc.Organizations(where, take: take).Select(o => JObject.Parse(o.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Organization {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Organization {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} organizations");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePatronNoticePolicies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting patron notice policies");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.PatronNoticePolicies(where))
                    {
                        if (!whatIf) fsc.DeletePatronNoticePolicy((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.patron_notice_policy{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} patron notice policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadPatronNoticePolicies(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading patron notice policies");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.PatronNoticePolicy.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"PatronNoticePolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"PatronNoticePolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertPatronNoticePolicy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new PatronNoticePolicy
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} patron notice policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SavePatronNoticePolicies(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving patron notice policies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.PatronNoticePolicy.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.PatronNoticePolicies(where, take: take) : fdc.PatronNoticePolicies(where, take: take).Select(pnp => JObject.Parse(pnp.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"PatronNoticePolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"PatronNoticePolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} patron notice policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePermissions(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting permissions");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Permissions(where))
                    {
                        if (!whatIf) fsc.DeletePermission((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_permissions.permissions{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} permissions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadPermissions(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading permissions");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Permission.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Permission {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Permission {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertPermission(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Permission
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} permissions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SavePermissions(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving permissions");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Permission.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Permissions(where, take: take) : fdc.Permissions(where, take: take).Select(p => JObject.Parse(p.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Permission {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Permission {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} permissions");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePermissionsUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting permissions users");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.PermissionsUsers(where))
                    {
                        if (!whatIf) fsc.DeletePermissionsUser((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_permissions.permissions_users{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} permissions users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadPermissionsUsers(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading permissions users");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.PermissionsUser.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"PermissionsUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"PermissionsUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertPermissionsUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new PermissionsUser
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} permissions users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SavePermissionsUsers(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving permissions users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.PermissionsUser.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.PermissionsUsers(where, take: take) : fdc.PermissionsUsers(where, take: take).Select(pu => JObject.Parse(pu.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"PermissionsUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"PermissionsUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} permissions users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePieces(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting pieces");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Pieces(where))
                    {
                        if (!whatIf) fsc.DeletePiece((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.pieces{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} pieces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadPieces(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading pieces");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Piece.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Piece {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Piece {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertPiece(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Piece
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            Polineid = (Guid?)jo.SelectToken("poLineId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} pieces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SavePieces(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving pieces");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Piece.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Pieces(where, take: take) : fdc.Pieces(where, take: take).Select(p => JObject.Parse(p.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Piece {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Piece {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} pieces");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteProxies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting proxies");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Proxies(where))
                    {
                        if (!whatIf) fsc.DeleteProxy((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_users.proxyfor{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} proxies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadProxies(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading proxies");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Proxy.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Proxy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Proxy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertProxy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Proxy
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} proxies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveProxies(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving proxies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Proxy.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Proxies(where, take: take) : fdc.Proxies(where, take: take).Select(p => JObject.Parse(p.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Proxy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Proxy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} proxies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteRawRecords(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting raw records");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            {
                var i = 0;
                if (api)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_source_record_storage.raw_records{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} raw records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadRawRecords(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading raw records");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.RawRecord.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"RawRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"RawRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        throw new NotSupportedException();
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new RawRecord
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} raw records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveRawRecords(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving raw records");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.RawRecord.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? throw new NotSupportedException() : fdc.RawRecords(where, take: take).Select(rr => JObject.Parse(rr.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"RawRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"RawRecord {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} raw records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteRecords(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting records");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Records(where))
                    {
                        if (!whatIf) fsc.DeleteRecord((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_source_record_storage.records{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadRecords(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading records");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Record.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                var l2 = new List<JObject>(1000);
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Record {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Record {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        l2.Add(jo);
                        if (i % 1000 == 0)
                        {
                            if (!whatIf) fsc.InsertRecords(l2);
                            l2.Clear();
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Record
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                if (api && !whatIf && l2.Any()) fsc.InsertRecords(l2);
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveRecords(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving records");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Record.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Records(where, take: take) : fdc.Records(where, take: take).Select(r => JObject.Parse(r.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Record {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Record {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} records");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteReportingCodes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting reporting codes");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ReportingCodes(where))
                    {
                        if (!whatIf) fsc.DeleteReportingCode((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.reporting_code{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} reporting codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadReportingCodes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading reporting codes");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ReportingCode.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ReportingCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ReportingCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertReportingCode(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ReportingCode
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString()
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} reporting codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveReportingCodes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving reporting codes");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ReportingCode.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ReportingCodes(where, take: take) : fdc.ReportingCodes(where, take: take).Select(rc => JObject.Parse(rc.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ReportingCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ReportingCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} reporting codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteRequests(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting requests");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Requests(where))
                    {
                        if (!whatIf) fsc.DeleteRequest((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.request{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} requests");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadRequests(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading requests");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Request.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Request {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Request {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertRequest(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Request
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Cancellationreasonid = (Guid?)jo.SelectToken("cancellationReasonId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} requests");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveRequests(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving requests");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Request.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Requests(where, take: take) : fdc.Requests(where, take: take).Select(r => JObject.Parse(r.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Request {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Request {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} requests");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteRequestPolicies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting request policies");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.RequestPolicies(where))
                    {
                        if (!whatIf) fsc.DeleteRequestPolicy((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.request_policy{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} request policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadRequestPolicies(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading request policies");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.RequestPolicy.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"RequestPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"RequestPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertRequestPolicy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new RequestPolicy
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} request policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveRequestPolicies(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving request policies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.RequestPolicy.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.RequestPolicies(where, take: take) : fdc.RequestPolicies(where, take: take).Select(rp => JObject.Parse(rp.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"RequestPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"RequestPolicy {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} request policies");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteScheduledNotices(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting scheduled notices");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ScheduledNotices(where))
                    {
                        if (!whatIf) fsc.DeleteScheduledNotice((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.scheduled_notice{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} scheduled notices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadScheduledNotices(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading scheduled notices");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ScheduledNotice.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ScheduledNotice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ScheduledNotice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertScheduledNotice(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ScheduledNotice
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} scheduled notices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveScheduledNotices(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving scheduled notices");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ScheduledNotice.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ScheduledNotices(where, take: take) : fdc.ScheduledNotices(where, take: take).Select(sn => JObject.Parse(sn.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ScheduledNotice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ScheduledNotice {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} scheduled notices");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteServicePoints(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting service points");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ServicePoints(where))
                    {
                        if (!whatIf) fsc.DeleteServicePoint((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.service_point{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} service points");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadServicePoints(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading service points");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ServicePoint.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ServicePoint {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ServicePoint {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertServicePoint(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ServicePoint
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} service points");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveServicePoints(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving service points");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ServicePoint.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ServicePoints(where, take: take) : fdc.ServicePoints(where, take: take).Select(sp => JObject.Parse(sp.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ServicePoint {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ServicePoint {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} service points");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteServicePointUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting service point users");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ServicePointUsers(where))
                    {
                        if (!whatIf) fsc.DeleteServicePointUser((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.service_point_user{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} service point users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadServicePointUsers(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading service point users");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ServicePointUser.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ServicePointUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ServicePointUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertServicePointUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new ServicePointUser
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Defaultservicepointid = (Guid?)jo.SelectToken("defaultServicePointId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} service point users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveServicePointUsers(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving service point users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.ServicePointUser.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ServicePointUsers(where, take: take) : fdc.ServicePointUsers(where, take: take).Select(spu => JObject.Parse(spu.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"ServicePointUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"ServicePointUser {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} service point users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteSnapshots(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting snapshots");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Snapshots(where))
                    {
                        if (!whatIf) fsc.DeleteSnapshot((string)jo["jobExecutionId"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_source_record_storage.snapshots{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} snapshots");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadSnapshots(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading snapshots");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Snapshot.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Snapshot {jo["jobExecutionId"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Snapshot {jo["jobExecutionId"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertSnapshot(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Snapshot
                        {
                            Id = (Guid?)jo.SelectToken("jobExecutionId"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} snapshots");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveSnapshots(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving snapshots");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Snapshot.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Snapshots(where, take: take) : fdc.Snapshots(where, take: take).Select(s3 => JObject.Parse(s3.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Snapshot {jo["jobExecutionId"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Snapshot {jo["jobExecutionId"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} snapshots");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteStaffSlips(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting staff slips");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.StaffSlips(where))
                    {
                        if (!whatIf) fsc.DeleteStaffSlip((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_circulation_storage.staff_slips{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} staff slips");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadStaffSlips(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading staff slips");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StaffSlip.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StaffSlip {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StaffSlip {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertStaffSlip(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new StaffSlip
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} staff slips");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveStaffSlips(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving staff slips");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StaffSlip.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StaffSlips(where, take: take) : fdc.StaffSlips(where, take: take).Select(ss => JObject.Parse(ss.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StaffSlip {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StaffSlip {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} staff slips");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteStatisticalCodes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting statistical codes");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.StatisticalCodes(where))
                    {
                        if (!whatIf) fsc.DeleteStatisticalCode((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.statistical_code{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} statistical codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadStatisticalCodes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading statistical codes");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StatisticalCode.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StatisticalCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StatisticalCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertStatisticalCode(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new StatisticalCode
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Statisticalcodetypeid = (Guid?)jo.SelectToken("statisticalCodeTypeId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} statistical codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveStatisticalCodes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving statistical codes");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StatisticalCode.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StatisticalCodes(where, take: take) : fdc.StatisticalCodes(where, take: take).Select(sc => JObject.Parse(sc.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StatisticalCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StatisticalCode {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} statistical codes");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteStatisticalCodeTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting statistical code types");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.StatisticalCodeTypes(where))
                    {
                        if (!whatIf) fsc.DeleteStatisticalCodeType((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_inventory_storage.statistical_code_type{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} statistical code types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadStatisticalCodeTypes(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading statistical code types");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StatisticalCodeType.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StatisticalCodeType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StatisticalCodeType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertStatisticalCodeType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new StatisticalCodeType
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} statistical code types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveStatisticalCodeTypes(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving statistical code types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.StatisticalCodeType.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StatisticalCodeTypes(where, take: take) : fdc.StatisticalCodeTypes(where, take: take).Select(sct => JObject.Parse(sct.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"StatisticalCodeType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"StatisticalCodeType {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} statistical code types");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting users");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Users(where))
                    {
                        if (!whatIf) fsc.DeleteUser((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_users.users{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadUsers(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading users");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.User.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"User {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"User {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new User
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Patrongroup = (Guid?)jo.SelectToken("patronGroup")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveUsers(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.User.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Users(where, take: take) : fdc.Users(where, take: take).Select(u => JObject.Parse(u.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"User {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"User {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} users");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteUserAcquisitionsUnits(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting user acquisitions units");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.UserAcquisitionsUnits(where))
                    {
                        if (!whatIf) fsc.DeleteUserAcquisitionsUnit((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_orders_storage.acquisitions_unit_membership{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} user acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadUserAcquisitionsUnits(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading user acquisitions units");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.UserAcquisitionsUnit.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"UserAcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"UserAcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertUserAcquisitionsUnit(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new UserAcquisitionsUnit
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Acquisitionsunitid = (Guid?)jo.SelectToken("acquisitionsUnitId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} user acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveUserAcquisitionsUnits(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving user acquisitions units");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.UserAcquisitionsUnit.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.UserAcquisitionsUnits(where, take: take) : fdc.UserAcquisitionsUnits(where, take: take).Select(uau => JObject.Parse(uau.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"UserAcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"UserAcquisitionsUnit {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} user acquisitions units");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteVouchers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting vouchers");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Vouchers(where))
                    {
                        if (!whatIf) fsc.DeleteVoucher((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_invoice_storage.vouchers{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} vouchers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadVouchers(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading vouchers");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Voucher.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Voucher {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Voucher {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertVoucher(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Voucher
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Invoiceid = (Guid?)jo.SelectToken("invoiceId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} vouchers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveVouchers(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving vouchers");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Voucher.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Vouchers(where, take: take) : fdc.Vouchers(where, take: take).Select(v => JObject.Parse(v.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Voucher {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Voucher {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} vouchers");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteVoucherItems(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting voucher items");
            var s = Stopwatch.StartNew();
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.VoucherItems(where))
                    {
                        if (!whatIf) fsc.DeleteVoucherItem((string)jo["id"]);
                        if (++i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                }
                else
                {
                    if (!whatIf) i = fbcc.ExecuteNonQuery($"DELETE FROM diku_mod_invoice_storage.voucher_lines{(where != null ? $" WHERE {where}" : "")}");
                }
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Deleted {i} voucher items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void LoadVoucherItems(string path)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Loading voucher items");
            var s = Stopwatch.StartNew();
            using (var sr = new StreamReader(compress || path.EndsWith(".gz") ? (Stream)new GZipStream(File.OpenRead($"{path}{(path.EndsWith(".gz") ? "" : ".gz")}"), CompressionMode.Decompress) : File.OpenRead(path)))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.VoucherItem.json")))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr2.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    if (take != null && take <= i) break;
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"VoucherItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"VoucherItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertVoucherItem(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new VoucherItem
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
                            Voucherid = (Guid?)jo.SelectToken("voucherId")
                        });
                        if (i % 1000 == 0)
                        {
                            fbcc.Commit();
                            if (i % 10000 == 0)
                            {
                                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                                s2.Restart();
                            }
                        }
                    }
                }
                fbcc.Commit();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} voucher items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveVoucherItems(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving voucher items");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.VoucherItem.json")))
            using (var sw = new StreamWriter(whatIf ? new MemoryStream() : compress ? (Stream)new GZipStream(new FileStream($"{path}.gz", FileMode.Create), CompressionMode.Compress) : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.VoucherItems(where, take: take) : fdc.VoucherItems(where, take: take).Select(vi => JObject.Parse(vi.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"VoucherItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"VoucherItem {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (!whatIf) js.Serialize(jtw, jo);
                    if (++i % 10000 == 0)
                    {
                        traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                        s2.Restart();
                    }
                }
                jtw.WriteEndArray();
                traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Saved {i} voucher items");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }
    }
}
