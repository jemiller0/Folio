using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioConsoleApplication
{
    partial class Program4
    {
        private static bool api;
        private static bool force;
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
                    traceSource.TraceEvent(TraceEventType.Critical, 0, "Usage: dotnet FolioConsoleApplication.dll [-All] [-Api] [-Delete] [-Force] [-Load] [-Save] [-Validate] [-Verbose] [-WhatIf] [-AllUsers] [-AllOrders] [-AllInventory] [-AllFinance] [-AllCirculation] [-AllOrganizations] [-AllInvoices] [-AllLogin] [-AllPermissions] [-AddressTypesPath <string>] [-AddressTypesWhere <string>] [-AlertsPath <string>] [-AlertsWhere <string>] [-AlternativeTitleTypesPath <string>] [-AlternativeTitleTypesWhere <string>] [-BudgetsPath <string>] [-BudgetsWhere <string>] [-CallNumberTypesPath <string>] [-CallNumberTypesWhere <string>] [-CampusesPath <string>] [-CampusesWhere <string>] [-CancellationReasonsPath <string>] [-CancellationReasonsWhere <string>] [-CategoriesPath <string>] [-CategoriesWhere <string>] [-ClassificationTypesPath <string>] [-ClassificationTypesWhere <string>] [-ContactsPath <string>] [-ContactsWhere <string>] [-ContributorNameTypesPath <string>] [-ContributorNameTypesWhere <string>] [-ContributorTypesPath <string>] [-ContributorTypesWhere <string>] [-ElectronicAccessRelationshipsPath <string>] [-ElectronicAccessRelationshipsWhere <string>] [-EncumbrancesPath <string>] [-EncumbrancesWhere <string>] [-FiscalYearsPath <string>] [-FiscalYearsWhere <string>] [-FixedDueDateSchedulesPath <string>] [-FixedDueDateSchedulesWhere <string>] [-FundsPath <string>] [-FundsWhere <string>] [-FundDistributionsPath <string>] [-FundDistributionsWhere <string>] [-GroupsPath <string>] [-GroupsWhere <string>] [-HoldingsPath <string>] [-HoldingsWhere <string>] [-HoldingNoteTypesPath <string>] [-HoldingNoteTypesWhere <string>] [-HoldingTypesPath <string>] [-HoldingTypesWhere <string>] [-IdTypesPath <string>] [-IdTypesWhere <string>] [-IllPoliciesPath <string>] [-IllPoliciesWhere <string>] [-InstancesPath <string>] [-InstancesWhere <string>] [-InstanceFormatsPath <string>] [-InstanceFormatsWhere <string>] [-InstanceRelationshipsPath <string>] [-InstanceRelationshipsWhere <string>] [-InstanceRelationshipTypesPath <string>] [-InstanceRelationshipTypesWhere <string>] [-InstanceStatusesPath <string>] [-InstanceStatusesWhere <string>] [-InstanceTypesPath <string>] [-InstanceTypesWhere <string>] [-InstitutionsPath <string>] [-InstitutionsWhere <string>] [-InterfacesPath <string>] [-InterfacesWhere <string>] [-InvoicesPath <string>] [-InvoicesWhere <string>] [-InvoiceItemsPath <string>] [-InvoiceItemsWhere <string>] [-ItemsPath <string>] [-ItemsWhere <string>] [-ItemNoteTypesPath <string>] [-ItemNoteTypesWhere <string>] [-LedgersPath <string>] [-LedgersWhere <string>] [-LibrariesPath <string>] [-LibrariesWhere <string>] [-LoansPath <string>] [-LoansWhere <string>] [-LoanPoliciesPath <string>] [-LoanPoliciesWhere <string>] [-LoanTypesPath <string>] [-LoanTypesWhere <string>] [-LocationsPath <string>] [-LocationsWhere <string>] [-LoginsPath <string>] [-LoginsWhere <string>] [-MaterialTypesPath <string>] [-MaterialTypesWhere <string>] [-ModeOfIssuancesPath <string>] [-ModeOfIssuancesWhere <string>] [-OrdersPath <string>] [-OrdersWhere <string>] [-OrderItemsPath <string>] [-OrderItemsWhere <string>] [-OrganizationsPath <string>] [-OrganizationsWhere <string>] [-PatronNoticePoliciesPath <string>] [-PatronNoticePoliciesWhere <string>] [-PermissionsPath <string>] [-PermissionsWhere <string>] [-PermissionsUsersPath <string>] [-PermissionsUsersWhere <string>] [-PiecesPath <string>] [-PiecesWhere <string>] [-ProxiesPath <string>] [-ProxiesWhere <string>] [-ReportingCodesPath <string>] [-ReportingCodesWhere <string>] [-RequestsPath <string>] [-RequestsWhere <string>] [-RequestPoliciesPath <string>] [-RequestPoliciesWhere <string>] [-ScheduledNoticesPath <string>] [-ScheduledNoticesWhere <string>] [-ServicePointsPath <string>] [-ServicePointsWhere <string>] [-ServicePointUsersPath <string>] [-ServicePointUsersWhere <string>] [-StaffSlipsPath <string>] [-StaffSlipsWhere <string>] [-StatisticalCodesPath <string>] [-StatisticalCodesWhere <string>] [-StatisticalCodeTypesPath <string>] [-StatisticalCodeTypesWhere <string>] [-UsersPath <string>] [-UsersWhere <string>] [-VouchersPath <string>] [-VouchersWhere <string>] [-VoucherItemsPath <string>] [-VoucherItemsWhere <string>]");
                    return -1;
                }
                var all = args.Any(s3 => s3.Equals("-All", StringComparison.OrdinalIgnoreCase));
                api = args.Any(s3 => s3.Equals("-Api", StringComparison.OrdinalIgnoreCase));
                var delete = args.Any(s3 => s3.Equals("-Delete", StringComparison.OrdinalIgnoreCase));
                force = args.Any(s3 => s3.Equals("-Force", StringComparison.OrdinalIgnoreCase));
                var load = args.Any(s3 => s3.Equals("-Load", StringComparison.OrdinalIgnoreCase));
                var save = args.Any(s3 => s3.Equals("-Save", StringComparison.OrdinalIgnoreCase));
                validate = args.Any(s3 => s3.Equals("-Validate", StringComparison.OrdinalIgnoreCase));
                whatIf = args.Any(s3 => s3.Equals("-WhatIf", StringComparison.OrdinalIgnoreCase));
                var allUsers = args.Any(s3 => s3.Equals("-AllUsers", StringComparison.OrdinalIgnoreCase));
                var allOrders = args.Any(s3 => s3.Equals("-AllOrders", StringComparison.OrdinalIgnoreCase));
                var allInventory = args.Any(s3 => s3.Equals("-AllInventory", StringComparison.OrdinalIgnoreCase));
                var allFinance = args.Any(s3 => s3.Equals("-AllFinance", StringComparison.OrdinalIgnoreCase));
                var allCirculation = args.Any(s3 => s3.Equals("-AllCirculation", StringComparison.OrdinalIgnoreCase));
                var allOrganizations = args.Any(s3 => s3.Equals("-AllOrganizations", StringComparison.OrdinalIgnoreCase));
                var allInvoices = args.Any(s3 => s3.Equals("-AllInvoices", StringComparison.OrdinalIgnoreCase));
                var allLogin = args.Any(s3 => s3.Equals("-AllLogin", StringComparison.OrdinalIgnoreCase));
                var allPermissions = args.Any(s3 => s3.Equals("-AllPermissions", StringComparison.OrdinalIgnoreCase));
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
                var electronicAccessRelationshipsPath = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var encumbrancesPath = args.SkipWhile(s3 => !s3.Equals("-EncumbrancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
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
                var instanceRelationshipsPath = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipTypesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceStatusesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceStatusesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceTypesPath = args.SkipWhile(s3 => !s3.Equals("-InstanceTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var institutionsPath = args.SkipWhile(s3 => !s3.Equals("-InstitutionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfacesPath = args.SkipWhile(s3 => !s3.Equals("-InterfacesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoicesPath = args.SkipWhile(s3 => !s3.Equals("-InvoicesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoiceItemsPath = args.SkipWhile(s3 => !s3.Equals("-InvoiceItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemsPath = args.SkipWhile(s3 => !s3.Equals("-ItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesPath = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ledgersPath = args.SkipWhile(s3 => !s3.Equals("-LedgersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesPath = args.SkipWhile(s3 => !s3.Equals("-LibrariesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loansPath = args.SkipWhile(s3 => !s3.Equals("-LoansPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanPoliciesPath = args.SkipWhile(s3 => !s3.Equals("-LoanPoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesPath = args.SkipWhile(s3 => !s3.Equals("-LoanTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsPath = args.SkipWhile(s3 => !s3.Equals("-LocationsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsPath = args.SkipWhile(s3 => !s3.Equals("-LoginsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesPath = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesPath = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ordersPath = args.SkipWhile(s3 => !s3.Equals("-OrdersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var orderItemsPath = args.SkipWhile(s3 => !s3.Equals("-OrderItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var organizationsPath = args.SkipWhile(s3 => !s3.Equals("-OrganizationsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var patronNoticePoliciesPath = args.SkipWhile(s3 => !s3.Equals("-PatronNoticePoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var piecesPath = args.SkipWhile(s3 => !s3.Equals("-PiecesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesPath = args.SkipWhile(s3 => !s3.Equals("-ProxiesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var reportingCodesPath = args.SkipWhile(s3 => !s3.Equals("-ReportingCodesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestsPath = args.SkipWhile(s3 => !s3.Equals("-RequestsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestPoliciesPath = args.SkipWhile(s3 => !s3.Equals("-RequestPoliciesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var scheduledNoticesPath = args.SkipWhile(s3 => !s3.Equals("-ScheduledNoticesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var staffSlipsPath = args.SkipWhile(s3 => !s3.Equals("-StaffSlipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersPath = args.SkipWhile(s3 => !s3.Equals("-UsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var vouchersPath = args.SkipWhile(s3 => !s3.Equals("-VouchersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var voucherItemsPath = args.SkipWhile(s3 => !s3.Equals("-VoucherItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
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
                var electronicAccessRelationshipsWhere = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var encumbrancesWhere = args.SkipWhile(s3 => !s3.Equals("-EncumbrancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
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
                var instanceRelationshipsWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceRelationshipTypesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceRelationshipTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceStatusesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceStatusesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var instanceTypesWhere = args.SkipWhile(s3 => !s3.Equals("-InstanceTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var institutionsWhere = args.SkipWhile(s3 => !s3.Equals("-InstitutionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var interfacesWhere = args.SkipWhile(s3 => !s3.Equals("-InterfacesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoicesWhere = args.SkipWhile(s3 => !s3.Equals("-InvoicesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var invoiceItemsWhere = args.SkipWhile(s3 => !s3.Equals("-InvoiceItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemsWhere = args.SkipWhile(s3 => !s3.Equals("-ItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ledgersWhere = args.SkipWhile(s3 => !s3.Equals("-LedgersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesWhere = args.SkipWhile(s3 => !s3.Equals("-LibrariesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loansWhere = args.SkipWhile(s3 => !s3.Equals("-LoansWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanPoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-LoanPoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesWhere = args.SkipWhile(s3 => !s3.Equals("-LoanTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsWhere = args.SkipWhile(s3 => !s3.Equals("-LocationsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsWhere = args.SkipWhile(s3 => !s3.Equals("-LoginsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesWhere = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesWhere = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var ordersWhere = args.SkipWhile(s3 => !s3.Equals("-OrdersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var orderItemsWhere = args.SkipWhile(s3 => !s3.Equals("-OrderItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var organizationsWhere = args.SkipWhile(s3 => !s3.Equals("-OrganizationsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var patronNoticePoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-PatronNoticePoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var piecesWhere = args.SkipWhile(s3 => !s3.Equals("-PiecesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesWhere = args.SkipWhile(s3 => !s3.Equals("-ProxiesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var reportingCodesWhere = args.SkipWhile(s3 => !s3.Equals("-ReportingCodesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestsWhere = args.SkipWhile(s3 => !s3.Equals("-RequestsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var requestPoliciesWhere = args.SkipWhile(s3 => !s3.Equals("-RequestPoliciesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var scheduledNoticesWhere = args.SkipWhile(s3 => !s3.Equals("-ScheduledNoticesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var staffSlipsWhere = args.SkipWhile(s3 => !s3.Equals("-StaffSlipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersWhere = args.SkipWhile(s3 => !s3.Equals("-UsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var vouchersWhere = args.SkipWhile(s3 => !s3.Equals("-VouchersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var voucherItemsWhere = args.SkipWhile(s3 => !s3.Equals("-VoucherItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                if (all)
                {
                    addressTypesPath = "addresstypes.json";
                    alertsPath = "alerts.json";
                    alternativeTitleTypesPath = "alternativetitletypes.json";
                    budgetsPath = "budgets.json";
                    callNumberTypesPath = "callnumbertypes.json";
                    campusesPath = "campuses.json";
                    cancellationReasonsPath = "cancellationreasons.json";
                    categoriesPath = "categories.json";
                    classificationTypesPath = "classificationtypes.json";
                    contactsPath = "contacts.json";
                    contributorNameTypesPath = "contributornametypes.json";
                    contributorTypesPath = "contributortypes.json";
                    electronicAccessRelationshipsPath = "electronicaccessrelationships.json";
                    encumbrancesPath = "encumbrances.json";
                    fiscalYearsPath = "fiscalyears.json";
                    fixedDueDateSchedulesPath = "fixedduedateschedules.json";
                    fundsPath = "funds.json";
                    fundDistributionsPath = "funddistributions.json";
                    groupsPath = "groups.json";
                    holdingsPath = "holdings.json";
                    holdingNoteTypesPath = "holdingnotetypes.json";
                    holdingTypesPath = "holdingtypes.json";
                    idTypesPath = "idtypes.json";
                    illPoliciesPath = "illpolicies.json";
                    instancesPath = "instances.json";
                    instanceFormatsPath = "instanceformats.json";
                    instanceRelationshipsPath = "instancerelationships.json";
                    instanceRelationshipTypesPath = "instancerelationshiptypes.json";
                    instanceStatusesPath = "instancestatuses.json";
                    instanceTypesPath = "instancetypes.json";
                    institutionsPath = "institutions.json";
                    interfacesPath = "interfaces.json";
                    invoicesPath = "invoices.json";
                    invoiceItemsPath = "invoiceitems.json";
                    itemsPath = "items.json";
                    itemNoteTypesPath = "itemnotetypes.json";
                    ledgersPath = "ledgers.json";
                    librariesPath = "libraries.json";
                    loansPath = "loans.json";
                    loanPoliciesPath = "loanpolicies.json";
                    loanTypesPath = "loantypes.json";
                    locationsPath = "locations.json";
                    loginsPath = "logins.json";
                    materialTypesPath = "materialtypes.json";
                    modeOfIssuancesPath = "modeofissuances.json";
                    ordersPath = "orders.json";
                    orderItemsPath = "orderitems.json";
                    organizationsPath = "organizations.json";
                    patronNoticePoliciesPath = "patronnoticepolicies.json";
                    permissionsPath = "permissions.json";
                    permissionsUsersPath = "permissionsusers.json";
                    piecesPath = "pieces.json";
                    proxiesPath = "proxies.json";
                    reportingCodesPath = "reportingcodes.json";
                    requestsPath = "requests.json";
                    requestPoliciesPath = "requestpolicies.json";
                    scheduledNoticesPath = "schedulednotices.json";
                    servicePointsPath = "servicepoints.json";
                    servicePointUsersPath = "servicepointusers.json";
                    staffSlipsPath = "staffslips.json";
                    statisticalCodesPath = "statisticalcodes.json";
                    statisticalCodeTypesPath = "statisticalcodetypes.json";
                    usersPath = "users.json";
                    vouchersPath = "vouchers.json";
                    voucherItemsPath = "voucheritems.json";
                }
                if (allUsers)
                {
                    addressTypesPath = "addresstypes.json";
                    groupsPath = "groups.json";
                    proxiesPath = "proxies.json";
                    usersPath = "users.json";
                }
                if (allOrders)
                {
                    alertsPath = "alerts.json";
                    ordersPath = "orders.json";
                    orderItemsPath = "orderitems.json";
                    piecesPath = "pieces.json";
                    reportingCodesPath = "reportingcodes.json";
                }
                if (allInventory)
                {
                    alternativeTitleTypesPath = "alternativetitletypes.json";
                    callNumberTypesPath = "callnumbertypes.json";
                    campusesPath = "campuses.json";
                    classificationTypesPath = "classificationtypes.json";
                    contributorNameTypesPath = "contributornametypes.json";
                    contributorTypesPath = "contributortypes.json";
                    electronicAccessRelationshipsPath = "electronicaccessrelationships.json";
                    holdingsPath = "holdings.json";
                    holdingNoteTypesPath = "holdingnotetypes.json";
                    holdingTypesPath = "holdingtypes.json";
                    idTypesPath = "idtypes.json";
                    illPoliciesPath = "illpolicies.json";
                    instancesPath = "instances.json";
                    instanceFormatsPath = "instanceformats.json";
                    instanceRelationshipsPath = "instancerelationships.json";
                    instanceRelationshipTypesPath = "instancerelationshiptypes.json";
                    instanceStatusesPath = "instancestatuses.json";
                    instanceTypesPath = "instancetypes.json";
                    institutionsPath = "institutions.json";
                    itemsPath = "items.json";
                    itemNoteTypesPath = "itemnotetypes.json";
                    librariesPath = "libraries.json";
                    loanTypesPath = "loantypes.json";
                    locationsPath = "locations.json";
                    materialTypesPath = "materialtypes.json";
                    modeOfIssuancesPath = "modeofissuances.json";
                    servicePointsPath = "servicepoints.json";
                    servicePointUsersPath = "servicepointusers.json";
                    statisticalCodesPath = "statisticalcodes.json";
                    statisticalCodeTypesPath = "statisticalcodetypes.json";
                }
                if (allFinance)
                {
                    budgetsPath = "budgets.json";
                    encumbrancesPath = "encumbrances.json";
                    fiscalYearsPath = "fiscalyears.json";
                    fundsPath = "funds.json";
                    fundDistributionsPath = "funddistributions.json";
                    ledgersPath = "ledgers.json";
                }
                if (allCirculation)
                {
                    cancellationReasonsPath = "cancellationreasons.json";
                    fixedDueDateSchedulesPath = "fixedduedateschedules.json";
                    loansPath = "loans.json";
                    loanPoliciesPath = "loanpolicies.json";
                    patronNoticePoliciesPath = "patronnoticepolicies.json";
                    requestsPath = "requests.json";
                    requestPoliciesPath = "requestpolicies.json";
                    scheduledNoticesPath = "schedulednotices.json";
                    staffSlipsPath = "staffslips.json";
                }
                if (allOrganizations)
                {
                    categoriesPath = "categories.json";
                    contactsPath = "contacts.json";
                    interfacesPath = "interfaces.json";
                    organizationsPath = "organizations.json";
                }
                if (allInvoices)
                {
                    invoicesPath = "invoices.json";
                    invoiceItemsPath = "invoiceitems.json";
                    vouchersPath = "vouchers.json";
                    voucherItemsPath = "voucheritems.json";
                }
                if (allLogin)
                {
                    loginsPath = "logins.json";
                }
                if (allPermissions)
                {
                    permissionsPath = "permissions.json";
                    permissionsUsersPath = "permissionsusers.json";
                }
                if (save && addressTypesPath != null) SaveAddressTypes(addressTypesPath, addressTypesWhere);
                if (save && groupsPath != null) SaveGroups(groupsPath, groupsWhere);
                if (save && usersPath != null) SaveUsers(usersPath, usersWhere);
                if (save && proxiesPath != null) SaveProxies(proxiesPath, proxiesWhere);
                if (save && loginsPath != null) SaveLogins(loginsPath, loginsWhere);
                if (save && permissionsPath != null) SavePermissions(permissionsPath, permissionsWhere);
                if (save && permissionsUsersPath != null) SavePermissionsUsers(permissionsUsersPath, permissionsUsersWhere);
                if (save && institutionsPath != null) SaveInstitutions(institutionsPath, institutionsWhere);
                if (save && campusesPath != null) SaveCampuses(campusesPath, campusesWhere);
                if (save && librariesPath != null) SaveLibraries(librariesPath, librariesWhere);
                if (save && servicePointsPath != null) SaveServicePoints(servicePointsPath, servicePointsWhere);
                if (save && servicePointUsersPath != null) SaveServicePointUsers(servicePointUsersPath, servicePointUsersWhere);
                if (save && locationsPath != null) SaveLocations(locationsPath, locationsWhere);
                if (save && alternativeTitleTypesPath != null) SaveAlternativeTitleTypes(alternativeTitleTypesPath, alternativeTitleTypesWhere);
                if (save && callNumberTypesPath != null) SaveCallNumberTypes(callNumberTypesPath, callNumberTypesWhere);
                if (save && classificationTypesPath != null) SaveClassificationTypes(classificationTypesPath, classificationTypesWhere);
                if (save && contributorNameTypesPath != null) SaveContributorNameTypes(contributorNameTypesPath, contributorNameTypesWhere);
                if (save && contributorTypesPath != null) SaveContributorTypes(contributorTypesPath, contributorTypesWhere);
                if (save && electronicAccessRelationshipsPath != null) SaveElectronicAccessRelationships(electronicAccessRelationshipsPath, electronicAccessRelationshipsWhere);
                if (save && holdingNoteTypesPath != null) SaveHoldingNoteTypes(holdingNoteTypesPath, holdingNoteTypesWhere);
                if (save && holdingTypesPath != null) SaveHoldingTypes(holdingTypesPath, holdingTypesWhere);
                if (save && idTypesPath != null) SaveIdTypes(idTypesPath, idTypesWhere);
                if (save && illPoliciesPath != null) SaveIllPolicies(illPoliciesPath, illPoliciesWhere);
                if (save && instanceFormatsPath != null) SaveInstanceFormats(instanceFormatsPath, instanceFormatsWhere);
                if (save && instanceRelationshipTypesPath != null) SaveInstanceRelationshipTypes(instanceRelationshipTypesPath, instanceRelationshipTypesWhere);
                if (save && instanceRelationshipsPath != null) SaveInstanceRelationships(instanceRelationshipsPath, instanceRelationshipsWhere);
                if (save && instanceStatusesPath != null) SaveInstanceStatuses(instanceStatusesPath, instanceStatusesWhere);
                if (save && instanceTypesPath != null) SaveInstanceTypes(instanceTypesPath, instanceTypesWhere);
                if (save && itemNoteTypesPath != null) SaveItemNoteTypes(itemNoteTypesPath, itemNoteTypesWhere);
                if (save && loanTypesPath != null) SaveLoanTypes(loanTypesPath, loanTypesWhere);
                if (save && materialTypesPath != null) SaveMaterialTypes(materialTypesPath, materialTypesWhere);
                if (save && modeOfIssuancesPath != null) SaveModeOfIssuances(modeOfIssuancesPath, modeOfIssuancesWhere);
                if (save && statisticalCodeTypesPath != null) SaveStatisticalCodeTypes(statisticalCodeTypesPath, statisticalCodeTypesWhere);
                if (save && statisticalCodesPath != null) SaveStatisticalCodes(statisticalCodesPath, statisticalCodesWhere);
                if (save && instancesPath != null) SaveInstances(instancesPath, instancesWhere);
                if (save && holdingsPath != null) SaveHoldings(holdingsPath, holdingsWhere);
                if (save && itemsPath != null) SaveItems(itemsPath, itemsWhere);
                if (save && categoriesPath != null) SaveCategories(categoriesPath, categoriesWhere);
                if (save && organizationsPath != null) SaveOrganizations(organizationsPath, organizationsWhere);
                if (save && contactsPath != null) SaveContacts(contactsPath, contactsWhere);
                if (save && interfacesPath != null) SaveInterfaces(interfacesPath, interfacesWhere);
                if (save && alertsPath != null) SaveAlerts(alertsPath, alertsWhere);
                if (save && ordersPath != null) SaveOrders(ordersPath, ordersWhere);
                if (save && orderItemsPath != null) SaveOrderItems(orderItemsPath, orderItemsWhere);
                if (save && piecesPath != null) SavePieces(piecesPath, piecesWhere);
                if (save && reportingCodesPath != null) SaveReportingCodes(reportingCodesPath, reportingCodesWhere);
                if (save && fiscalYearsPath != null) SaveFiscalYears(fiscalYearsPath, fiscalYearsWhere);
                if (save && ledgersPath != null) SaveLedgers(ledgersPath, ledgersWhere);
                if (save && fundsPath != null) SaveFunds(fundsPath, fundsWhere);
                if (save && budgetsPath != null) SaveBudgets(budgetsPath, budgetsWhere);
                if (save && encumbrancesPath != null) SaveEncumbrances(encumbrancesPath, encumbrancesWhere);
                if (save && fundDistributionsPath != null) SaveFundDistributions(fundDistributionsPath, fundDistributionsWhere);
                if (save && invoicesPath != null) SaveInvoices(invoicesPath, invoicesWhere);
                if (save && invoiceItemsPath != null) SaveInvoiceItems(invoiceItemsPath, invoiceItemsWhere);
                if (save && vouchersPath != null) SaveVouchers(vouchersPath, vouchersWhere);
                if (save && voucherItemsPath != null) SaveVoucherItems(voucherItemsPath, voucherItemsWhere);
                if (save && cancellationReasonsPath != null) SaveCancellationReasons(cancellationReasonsPath, cancellationReasonsWhere);
                if (save && fixedDueDateSchedulesPath != null) SaveFixedDueDateSchedules(fixedDueDateSchedulesPath, fixedDueDateSchedulesWhere);
                if (save && loanPoliciesPath != null) SaveLoanPolicies(loanPoliciesPath, loanPoliciesWhere);
                if (save && patronNoticePoliciesPath != null) SavePatronNoticePolicies(patronNoticePoliciesPath, patronNoticePoliciesWhere);
                if (save && requestPoliciesPath != null) SaveRequestPolicies(requestPoliciesPath, requestPoliciesWhere);
                if (save && loansPath != null) SaveLoans(loansPath, loansWhere);
                if (save && requestsPath != null) SaveRequests(requestsPath, requestsWhere);
                if (save && scheduledNoticesPath != null) SaveScheduledNotices(scheduledNoticesPath, scheduledNoticesWhere);
                if (save && staffSlipsPath != null) SaveStaffSlips(staffSlipsPath, staffSlipsWhere);
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
                if (delete && invoicesPath != null) DeleteInvoices(invoicesWhere);
                if (delete && fundDistributionsPath != null) DeleteFundDistributions(fundDistributionsWhere);
                if (delete && encumbrancesPath != null) DeleteEncumbrances(encumbrancesWhere);
                if (delete && budgetsPath != null) DeleteBudgets(budgetsWhere);
                if (delete && fundsPath != null) DeleteFunds(fundsWhere);
                if (delete && ledgersPath != null) DeleteLedgers(ledgersWhere);
                if (delete && fiscalYearsPath != null) DeleteFiscalYears(fiscalYearsWhere);
                if (delete && reportingCodesPath != null) DeleteReportingCodes(reportingCodesWhere);
                if (delete && piecesPath != null) DeletePieces(piecesWhere);
                if (delete && orderItemsPath != null) DeleteOrderItems(orderItemsWhere);
                if (delete && ordersPath != null) DeleteOrders(ordersWhere);
                if (delete && alertsPath != null) DeleteAlerts(alertsWhere);
                if (delete && interfacesPath != null) DeleteInterfaces(interfacesWhere);
                if (delete && contactsPath != null) DeleteContacts(contactsWhere);
                if (delete && organizationsPath != null) DeleteOrganizations(organizationsWhere);
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
                if (load && organizationsPath != null) LoadOrganizations(organizationsPath);
                if (load && contactsPath != null) LoadContacts(contactsPath);
                if (load && interfacesPath != null) LoadInterfaces(interfacesPath);
                if (load && alertsPath != null) LoadAlerts(alertsPath);
                if (load && ordersPath != null) LoadOrders(ordersPath);
                if (load && orderItemsPath != null) LoadOrderItems(orderItemsPath);
                if (load && piecesPath != null) LoadPieces(piecesPath);
                if (load && reportingCodesPath != null) LoadReportingCodes(reportingCodesPath);
                if (load && fiscalYearsPath != null) LoadFiscalYears(fiscalYearsPath);
                if (load && ledgersPath != null) LoadLedgers(ledgersPath);
                if (load && fundsPath != null) LoadFunds(fundsPath);
                if (load && budgetsPath != null) LoadBudgets(budgetsPath);
                if (load && encumbrancesPath != null) LoadEncumbrances(encumbrancesPath);
                if (load && fundDistributionsPath != null) LoadFundDistributions(fundDistributionsPath);
                if (load && invoicesPath != null) LoadInvoices(invoicesPath);
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.AddressTypes(where) : fdc.AddressTypes(where).Select(at => JObject.Parse(at.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Alerts(where) : fdc.Alerts(where).Select(a => JObject.Parse(a.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.AlternativeTitleTypes(where) : fdc.AlternativeTitleTypes(where).Select(att => JObject.Parse(att.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Budgets(where) : fdc.Budgets(where).Select(b => JObject.Parse(b.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.CallNumberTypes(where) : fdc.CallNumberTypes(where).Select(cnt => JObject.Parse(cnt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Campuses(where) : fdc.Campuses(where).Select(c => JObject.Parse(c.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.CancellationReasons(where) : fdc.CancellationReasons(where).Select(cr => JObject.Parse(cr.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Categories(where) : fdc.Categories(where).Select(c => JObject.Parse(c.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ClassificationTypes(where) : fdc.ClassificationTypes(where).Select(ct => JObject.Parse(ct.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Contacts(where) : fdc.Contacts(where).Select(c => JObject.Parse(c.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ContributorNameTypes(where) : fdc.ContributorNameTypes(where).Select(cnt => JObject.Parse(cnt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ContributorTypes(where) : fdc.ContributorTypes(where).Select(ct => JObject.Parse(ct.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ElectronicAccessRelationships(where) : fdc.ElectronicAccessRelationships(where).Select(ear => JObject.Parse(ear.Content)))
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
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Encumbrances(where))
                    {
                        if (!whatIf) fsc.DeleteEncumbrance((string)jo["id"]);
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
            using (var sr = new StreamReader(path))
            using (var sr2 = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Encumbrance.json")))
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
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Encumbrance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Encumbrance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
                    }
                    if (api)
                    {
                        if (!whatIf) fsc.InsertEncumbrance(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        if (!whatIf) fbcc.Insert(new Encumbrance
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            Budgetid = (Guid?)jo.SelectToken("budgetId"),
                            Fundid = (Guid?)jo.SelectToken("fundId")
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Added {i} encumbrances");
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void SaveEncumbrances(string path, string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Saving encumbrances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            using (var sr = new StreamReader(Assembly.GetAssembly(typeof(FolioContext)).GetManifestResourceStream("FolioLibrary.Encumbrance.json")))
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Encumbrances(where) : fdc.Encumbrances(where).Select(e2 => JObject.Parse(e2.Content)))
                {
                    if (validate)
                    {
                        var l = js4.Validate(jo);
                        if (l.Any()) if (force) traceSource.TraceEvent(TraceEventType.Error, 0, $"Encumbrance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}"); else throw new ValidationException($"Encumbrance {jo["id"]}: {string.Join(" ", l.Select(ve => ve.ToString()))}");
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FiscalYears(where) : fdc.FiscalYears(where).Select(fy => JObject.Parse(fy.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FixedDueDateSchedules(where) : fdc.FixedDueDateSchedules(where).Select(fdds => JObject.Parse(fdds.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Funds(where) : fdc.Funds(where).Select(f => JObject.Parse(f.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.FundDistributions(where) : fdc.FundDistributions(where).Select(fd => JObject.Parse(fd.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Groups(where) : fdc.Groups(where).Select(g => JObject.Parse(g.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Holdings(where) : fdc.Holdings(where).Select(h => JObject.Parse(h.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.HoldingNoteTypes(where) : fdc.HoldingNoteTypes(where).Select(hnt => JObject.Parse(hnt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.HoldingTypes(where) : fdc.HoldingTypes(where).Select(ht => JObject.Parse(ht.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.IdTypes(where) : fdc.IdTypes(where).Select(it => JObject.Parse(it.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.IllPolicies(where) : fdc.IllPolicies(where).Select(ip => JObject.Parse(ip.Content)))
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
            using (var sr = new StreamReader(path))
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
                            Modeofissuanceid = (Guid?)jo.SelectToken("modeOfIssuanceId")
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Instances(where) : fdc.Instances(where).Select(i2 => JObject.Parse(i2.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceFormats(where) : fdc.InstanceFormats(where).Select(@if => JObject.Parse(@if.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceRelationships(where) : fdc.InstanceRelationships(where).Select(ir => JObject.Parse(ir.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceRelationshipTypes(where) : fdc.InstanceRelationshipTypes(where).Select(irt => JObject.Parse(irt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceStatuses(where) : fdc.InstanceStatuses(where).Select(@is => JObject.Parse(@is.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InstanceTypes(where) : fdc.InstanceTypes(where).Select(it => JObject.Parse(it.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Institutions(where) : fdc.Institutions(where).Select(i2 => JObject.Parse(i2.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Interfaces(where) : fdc.Interfaces(where).Select(i2 => JObject.Parse(i2.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Invoices(where) : fdc.Invoices(where).Select(i2 => JObject.Parse(i2.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.InvoiceItems(where) : fdc.InvoiceItems(where).Select(ii => JObject.Parse(ii.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Items(where) : fdc.Items(where).Select(i2 => JObject.Parse(i2.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ItemNoteTypes(where) : fdc.ItemNoteTypes(where).Select(@int => JObject.Parse(@int.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Ledgers(where) : fdc.Ledgers(where).Select(l => JObject.Parse(l.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Libraries(where) : fdc.Libraries(where).Select(l => JObject.Parse(l.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Loans(where) : fdc.Loans(where).Select(l => JObject.Parse(l.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.LoanPolicies(where) : fdc.LoanPolicies(where).Select(lp => JObject.Parse(lp.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.LoanTypes(where) : fdc.LoanTypes(where).Select(lt => JObject.Parse(lt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Locations(where) : fdc.Locations(where).Select(l => JObject.Parse(l.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Logins(where) : fdc.Logins(where).Select(l => JObject.Parse(l.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.MaterialTypes(where) : fdc.MaterialTypes(where).Select(mt => JObject.Parse(mt.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ModeOfIssuances(where) : fdc.ModeOfIssuances(where).Select(moi => JObject.Parse(moi.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Orders(where) : fdc.Orders(where).Select(o => JObject.Parse(o.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.OrderItems(where) : fdc.OrderItems(where).Select(oi => JObject.Parse(oi.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Organizations(where) : fdc.Organizations(where).Select(o => JObject.Parse(o.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.PatronNoticePolicies(where) : fdc.PatronNoticePolicies(where).Select(pnp => JObject.Parse(pnp.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Permissions(where) : fdc.Permissions(where).Select(p => JObject.Parse(p.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.PermissionsUsers(where) : fdc.PermissionsUsers(where).Select(pu => JObject.Parse(pu.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Pieces(where) : fdc.Pieces(where).Select(p => JObject.Parse(p.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Proxies(where) : fdc.Proxies(where).Select(p => JObject.Parse(p.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ReportingCodes(where) : fdc.ReportingCodes(where).Select(rc => JObject.Parse(rc.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Requests(where) : fdc.Requests(where).Select(r => JObject.Parse(r.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.RequestPolicies(where) : fdc.RequestPolicies(where).Select(rp => JObject.Parse(rp.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ScheduledNotices(where) : fdc.ScheduledNotices(where).Select(sn => JObject.Parse(sn.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ServicePoints(where) : fdc.ServicePoints(where).Select(sp => JObject.Parse(sp.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.ServicePointUsers(where) : fdc.ServicePointUsers(where).Select(spu => JObject.Parse(spu.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StaffSlips(where) : fdc.StaffSlips(where).Select(ss => JObject.Parse(ss.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StatisticalCodes(where) : fdc.StatisticalCodes(where).Select(sc => JObject.Parse(sc.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.StatisticalCodeTypes(where) : fdc.StatisticalCodeTypes(where).Select(sct => JObject.Parse(sct.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Users(where) : fdc.Users(where).Select(u => JObject.Parse(u.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.Vouchers(where) : fdc.Vouchers(where).Select(v => JObject.Parse(v.Content)))
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
            using (var sr = new StreamReader(path))
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
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
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
            using (var sw = new StreamWriter(whatIf ? (Stream)new MemoryStream() : new FileStream(path, FileMode.Create)))
            using (var jtw = new JsonTextWriter(sw))
            {
                var s2 = Stopwatch.StartNew();
                var js4 = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var js = new JsonSerializer { Formatting = Formatting.Indented };
                jtw.WriteStartArray();
                var i = 0;
                foreach (var jo in api ? fsc.VoucherItems(where) : fdc.VoucherItems(where).Select(vi => JObject.Parse(vi.Content)))
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
