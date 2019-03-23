using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolioConsoleApplication
{
    partial class Program4
    {
        private static bool api;
        private readonly static TraceSource traceSource = new TraceSource("FolioConsoleApplication", SourceLevels.Information);
        private static bool validate;

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
                if (args.Length == 0)
                {
                    traceSource.TraceEvent(TraceEventType.Critical, 0, "Usage: dotnet FolioConsoleApplication.dll [-All] [-Api] [-Delete] [-Load] [-Save] [-Validate] [-Verbose] [-AllUsers] [-AllInventory] [-AllLogin] [-AllPermissions] [-AddressTypesPath <string>] [-AddressTypesWhere <string>] [-AlternativeTitleTypesPath <string>] [-AlternativeTitleTypesWhere <string>] [-CallNumberTypesPath <string>] [-CallNumberTypesWhere <string>] [-CampusesPath <string>] [-CampusesWhere <string>] [-ClassificationTypesPath <string>] [-ClassificationTypesWhere <string>] [-ContributorNameTypesPath <string>] [-ContributorNameTypesWhere <string>] [-ContributorTypesPath <string>] [-ContributorTypesWhere <string>] [-ElectronicAccessRelationshipsPath <string>] [-ElectronicAccessRelationshipsWhere <string>] [-GroupsPath <string>] [-GroupsWhere <string>] [-HoldingsPath <string>] [-HoldingsWhere <string>] [-HoldingNoteTypesPath <string>] [-HoldingNoteTypesWhere <string>] [-HoldingTypesPath <string>] [-HoldingTypesWhere <string>] [-IdTypesPath <string>] [-IdTypesWhere <string>] [-IllPoliciesPath <string>] [-IllPoliciesWhere <string>] [-InstancesPath <string>] [-InstancesWhere <string>] [-InstanceFormatsPath <string>] [-InstanceFormatsWhere <string>] [-InstanceRelationshipsPath <string>] [-InstanceRelationshipsWhere <string>] [-InstanceRelationshipTypesPath <string>] [-InstanceRelationshipTypesWhere <string>] [-InstanceStatusesPath <string>] [-InstanceStatusesWhere <string>] [-InstanceTypesPath <string>] [-InstanceTypesWhere <string>] [-InstitutionsPath <string>] [-InstitutionsWhere <string>] [-ItemsPath <string>] [-ItemsWhere <string>] [-ItemNoteTypesPath <string>] [-ItemNoteTypesWhere <string>] [-LibrariesPath <string>] [-LibrariesWhere <string>] [-LoanTypesPath <string>] [-LoanTypesWhere <string>] [-LocationsPath <string>] [-LocationsWhere <string>] [-LoginsPath <string>] [-LoginsWhere <string>] [-MaterialTypesPath <string>] [-MaterialTypesWhere <string>] [-ModeOfIssuancesPath <string>] [-ModeOfIssuancesWhere <string>] [-PermissionsPath <string>] [-PermissionsWhere <string>] [-PermissionsUsersPath <string>] [-PermissionsUsersWhere <string>] [-ProxiesPath <string>] [-ProxiesWhere <string>] [-ServicePointsPath <string>] [-ServicePointsWhere <string>] [-ServicePointUsersPath <string>] [-ServicePointUsersWhere <string>] [-StatisticalCodesPath <string>] [-StatisticalCodesWhere <string>] [-StatisticalCodeTypesPath <string>] [-StatisticalCodeTypesWhere <string>] [-UsersPath <string>] [-UsersWhere <string>]");
                    return -1;
                }
                var all = args.Any(s3 => s3.Equals("-All", StringComparison.OrdinalIgnoreCase));
                api = args.Any(s3 => s3.Equals("-Api", StringComparison.OrdinalIgnoreCase));
                var delete = args.Any(s3 => s3.Equals("-Delete", StringComparison.OrdinalIgnoreCase));
                var load = args.Any(s3 => s3.Equals("-Load", StringComparison.OrdinalIgnoreCase));
                var save = args.Any(s3 => s3.Equals("-Save", StringComparison.OrdinalIgnoreCase));
                validate = args.Any(s3 => s3.Equals("-Validate", StringComparison.OrdinalIgnoreCase));
                var allUsers = args.Any(s3 => s3.Equals("-AllUsers", StringComparison.OrdinalIgnoreCase));
                var allInventory = args.Any(s3 => s3.Equals("-AllInventory", StringComparison.OrdinalIgnoreCase));
                var allLogin = args.Any(s3 => s3.Equals("-AllLogin", StringComparison.OrdinalIgnoreCase));
                var allPermissions = args.Any(s3 => s3.Equals("-AllPermissions", StringComparison.OrdinalIgnoreCase));
                var addressTypesPath = args.SkipWhile(s3 => !s3.Equals("-AddressTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alternativeTitleTypesPath = args.SkipWhile(s3 => !s3.Equals("-AlternativeTitleTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var callNumberTypesPath = args.SkipWhile(s3 => !s3.Equals("-CallNumberTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var campusesPath = args.SkipWhile(s3 => !s3.Equals("-CampusesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var classificationTypesPath = args.SkipWhile(s3 => !s3.Equals("-ClassificationTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorNameTypesPath = args.SkipWhile(s3 => !s3.Equals("-ContributorNameTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorTypesPath = args.SkipWhile(s3 => !s3.Equals("-ContributorTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var electronicAccessRelationshipsPath = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
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
                var itemsPath = args.SkipWhile(s3 => !s3.Equals("-ItemsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesPath = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesPath = args.SkipWhile(s3 => !s3.Equals("-LibrariesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesPath = args.SkipWhile(s3 => !s3.Equals("-LoanTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsPath = args.SkipWhile(s3 => !s3.Equals("-LocationsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsPath = args.SkipWhile(s3 => !s3.Equals("-LoginsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesPath = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesPath = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersPath = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesPath = args.SkipWhile(s3 => !s3.Equals("-ProxiesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointsPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersPath = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesPath = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersPath = args.SkipWhile(s3 => !s3.Equals("-UsersPath", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var addressTypesWhere = args.SkipWhile(s3 => !s3.Equals("-AddressTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var alternativeTitleTypesWhere = args.SkipWhile(s3 => !s3.Equals("-AlternativeTitleTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var callNumberTypesWhere = args.SkipWhile(s3 => !s3.Equals("-CallNumberTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var campusesWhere = args.SkipWhile(s3 => !s3.Equals("-CampusesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var classificationTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ClassificationTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorNameTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ContributorNameTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var contributorTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ContributorTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var electronicAccessRelationshipsWhere = args.SkipWhile(s3 => !s3.Equals("-ElectronicAccessRelationshipsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
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
                var itemsWhere = args.SkipWhile(s3 => !s3.Equals("-ItemsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var itemNoteTypesWhere = args.SkipWhile(s3 => !s3.Equals("-ItemNoteTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var librariesWhere = args.SkipWhile(s3 => !s3.Equals("-LibrariesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loanTypesWhere = args.SkipWhile(s3 => !s3.Equals("-LoanTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var locationsWhere = args.SkipWhile(s3 => !s3.Equals("-LocationsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var loginsWhere = args.SkipWhile(s3 => !s3.Equals("-LoginsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var materialTypesWhere = args.SkipWhile(s3 => !s3.Equals("-MaterialTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var modeOfIssuancesWhere = args.SkipWhile(s3 => !s3.Equals("-ModeOfIssuancesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var permissionsUsersWhere = args.SkipWhile(s3 => !s3.Equals("-PermissionsUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var proxiesWhere = args.SkipWhile(s3 => !s3.Equals("-ProxiesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointsWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointsWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var servicePointUsersWhere = args.SkipWhile(s3 => !s3.Equals("-ServicePointUsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var statisticalCodeTypesWhere = args.SkipWhile(s3 => !s3.Equals("-StatisticalCodeTypesWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                var usersWhere = args.SkipWhile(s3 => !s3.Equals("-UsersWhere", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
                if (all)
                {
                    addressTypesPath = "addresstypes.json";
                    alternativeTitleTypesPath = "alternativetitletypes.json";
                    callNumberTypesPath = "callnumbertypes.json";
                    campusesPath = "campuses.json";
                    classificationTypesPath = "classificationtypes.json";
                    contributorNameTypesPath = "contributornametypes.json";
                    contributorTypesPath = "contributortypes.json";
                    electronicAccessRelationshipsPath = "electronicaccessrelationships.json";
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
                    itemsPath = "items.json";
                    itemNoteTypesPath = "itemnotetypes.json";
                    librariesPath = "libraries.json";
                    loanTypesPath = "loantypes.json";
                    locationsPath = "locations.json";
                    loginsPath = "logins.json";
                    materialTypesPath = "materialtypes.json";
                    modeOfIssuancesPath = "modeofissuances.json";
                    permissionsPath = "permissions.json";
                    permissionsUsersPath = "permissionsusers.json";
                    proxiesPath = "proxies.json";
                    servicePointsPath = "servicepoints.json";
                    servicePointUsersPath = "servicepointusers.json";
                    statisticalCodesPath = "statisticalcodes.json";
                    statisticalCodeTypesPath = "statisticalcodetypes.json";
                    usersPath = "users.json";
                }
                if (allUsers)
                {
                    addressTypesPath = "addresstypes.json";
                    groupsPath = "groups.json";
                    proxiesPath = "proxies.json";
                    usersPath = "users.json";
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
                if (save && librariesPath != null) SaveLibraries(librariesPath, librariesWhere);
                if (save && institutionsPath != null) SaveInstitutions(institutionsPath, institutionsWhere);
                if (save && campusesPath != null) SaveCampuses(campusesPath, campusesWhere);
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
                if (delete && campusesPath != null) DeleteCampuses(campusesWhere);
                if (delete && institutionsPath != null) DeleteInstitutions(institutionsWhere);
                if (delete && librariesPath != null) DeleteLibraries(librariesWhere);
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
                if (load && librariesPath != null) LoadLibraries(librariesPath);
                if (load && institutionsPath != null) LoadInstitutions(institutionsPath);
                if (load && campusesPath != null) LoadCampuses(campusesPath);
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

        public static void DeleteAddressTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting address types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.AddressTypes(where))
                    {
                        fsc.DeleteAddressType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE addresstype" : $"DELETE FROM addresstype WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = AddressType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"AddressType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertAddressType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new AddressType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.AddressTypes(where) : fdc.AddressTypes(where).Select(at => JObject.Parse(at.Content)))
                    {
                        if (validate)
                        {
                            var vr = AddressType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"AddressType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteAlternativeTitleTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting alternative title types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.AlternativeTitleTypes(where))
                    {
                        fsc.DeleteAlternativeTitleType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE alternative_title_type" : $"DELETE FROM alternative_title_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = AlternativeTitleType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"AlternativeTitleType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertAlternativeTitleType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new AlternativeTitleType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.AlternativeTitleTypes(where) : fdc.AlternativeTitleTypes(where).Select(att => JObject.Parse(att.Content)))
                    {
                        if (validate)
                        {
                            var vr = AlternativeTitleType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"AlternativeTitleType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCallNumberTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting call number types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.CallNumberTypes(where))
                    {
                        fsc.DeleteCallNumberType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE call_number_type" : $"DELETE FROM call_number_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = CallNumberType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"CallNumberType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertCallNumberType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new CallNumberType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.CallNumberTypes(where) : fdc.CallNumberTypes(where).Select(cnt => JObject.Parse(cnt.Content)))
                    {
                        if (validate)
                        {
                            var vr = CallNumberType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"CallNumberType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteCampuses(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting campuses");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Campuses(where))
                    {
                        fsc.DeleteCampus((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE loccampus" : $"DELETE FROM loccampus WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Campus.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Campus {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertCampus(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Campus
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Campuses(where) : fdc.Campuses(where).Select(c => JObject.Parse(c.Content)))
                    {
                        if (validate)
                        {
                            var vr = Campus.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Campus {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteClassificationTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting classification types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ClassificationTypes(where))
                    {
                        fsc.DeleteClassificationType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE classification_type" : $"DELETE FROM classification_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ClassificationType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ClassificationType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertClassificationType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ClassificationType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ClassificationTypes(where) : fdc.ClassificationTypes(where).Select(ct => JObject.Parse(ct.Content)))
                    {
                        if (validate)
                        {
                            var vr = ClassificationType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ClassificationType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteContributorNameTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting contributor name types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ContributorNameTypes(where))
                    {
                        fsc.DeleteContributorNameType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE contributor_name_type" : $"DELETE FROM contributor_name_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ContributorNameType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ContributorNameType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertContributorNameType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ContributorNameType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ContributorNameTypes(where) : fdc.ContributorNameTypes(where).Select(cnt => JObject.Parse(cnt.Content)))
                    {
                        if (validate)
                        {
                            var vr = ContributorNameType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ContributorNameType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteContributorTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting contributor types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ContributorTypes(where))
                    {
                        fsc.DeleteContributorType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE contributor_type" : $"DELETE FROM contributor_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ContributorType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ContributorType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertContributorType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ContributorType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ContributorTypes(where) : fdc.ContributorTypes(where).Select(ct => JObject.Parse(ct.Content)))
                    {
                        if (validate)
                        {
                            var vr = ContributorType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ContributorType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteElectronicAccessRelationships(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting electronic access relationships");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ElectronicAccessRelationships(where))
                    {
                        fsc.DeleteElectronicAccessRelationship((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE electronic_access_relationship" : $"DELETE FROM electronic_access_relationship WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ElectronicAccessRelationship.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ElectronicAccessRelationship {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertElectronicAccessRelationship(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ElectronicAccessRelationship
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ElectronicAccessRelationships(where) : fdc.ElectronicAccessRelationships(where).Select(ear => JObject.Parse(ear.Content)))
                    {
                        if (validate)
                        {
                            var vr = ElectronicAccessRelationship.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ElectronicAccessRelationship {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteGroups(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting groups");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Groups(where))
                    {
                        fsc.DeleteGroup((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE groups" : $"DELETE FROM groups WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Group.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Group {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertGroup(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Group
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Groups(where) : fdc.Groups(where).Select(g => JObject.Parse(g.Content)))
                    {
                        if (validate)
                        {
                            var vr = Group.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Group {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldings(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holdings");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Holdings(where))
                    {
                        fsc.DeleteHolding((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE holdings_record" : $"DELETE FROM holdings_record WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Holding.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Holding {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertHolding(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Holding
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Holdings(where) : fdc.Holdings(where).Select(h => JObject.Parse(h.Content)))
                    {
                        if (validate)
                        {
                            var vr = Holding.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Holding {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldingNoteTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holding note types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.HoldingNoteTypes(where))
                    {
                        fsc.DeleteHoldingNoteType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE holdings_note_type" : $"DELETE FROM holdings_note_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = HoldingNoteType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"HoldingNoteType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertHoldingNoteType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new HoldingNoteType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.HoldingNoteTypes(where) : fdc.HoldingNoteTypes(where).Select(hnt => JObject.Parse(hnt.Content)))
                    {
                        if (validate)
                        {
                            var vr = HoldingNoteType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"HoldingNoteType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteHoldingTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting holding types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.HoldingTypes(where))
                    {
                        fsc.DeleteHoldingType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE holdings_type" : $"DELETE FROM holdings_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = HoldingType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"HoldingType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertHoldingType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new HoldingType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.HoldingTypes(where) : fdc.HoldingTypes(where).Select(ht => JObject.Parse(ht.Content)))
                    {
                        if (validate)
                        {
                            var vr = HoldingType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"HoldingType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteIdTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting id types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.IdTypes(where))
                    {
                        fsc.DeleteIdType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE identifier_type" : $"DELETE FROM identifier_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = IdType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"IdType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertIdType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new IdType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.IdTypes(where) : fdc.IdTypes(where).Select(it => JObject.Parse(it.Content)))
                    {
                        if (validate)
                        {
                            var vr = IdType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"IdType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteIllPolicies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting ill policies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.IllPolicies(where))
                    {
                        fsc.DeleteIllPolicy((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE ill_policy" : $"DELETE FROM ill_policy WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = IllPolicy.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"IllPolicy {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertIllPolicy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new IllPolicy
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.IllPolicies(where) : fdc.IllPolicies(where).Select(ip => JObject.Parse(ip.Content)))
                    {
                        if (validate)
                        {
                            var vr = IllPolicy.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"IllPolicy {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstances(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Instances(where))
                    {
                        fsc.DeleteInstance((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance" : $"DELETE FROM instance WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Instance.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Instance {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstance(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Instance
                        {
                            Id = (Guid?)jo.SelectToken("id"),
                            Content = jo.ToString(),
                            CreationTime = (DateTime?)jo.SelectToken("metadata.createdDate"),
                            CreationUserId = (string)jo.SelectToken("metadata.createdByUserId"),
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Instances(where) : fdc.Instances(where).Select(i2 => JObject.Parse(i2.Content)))
                    {
                        if (validate)
                        {
                            var vr = Instance.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Instance {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceFormats(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance formats");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceFormats(where))
                    {
                        fsc.DeleteInstanceFormat((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance_format" : $"DELETE FROM instance_format WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = InstanceFormat.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"InstanceFormat {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstanceFormat(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new InstanceFormat
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.InstanceFormats(where) : fdc.InstanceFormats(where).Select(@if => JObject.Parse(@if.Content)))
                    {
                        if (validate)
                        {
                            var vr = InstanceFormat.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"InstanceFormat {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceRelationships(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance relationships");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceRelationships(where))
                    {
                        fsc.DeleteInstanceRelationship((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance_relationship" : $"DELETE FROM instance_relationship WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = InstanceRelationship.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"InstanceRelationship {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstanceRelationship(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new InstanceRelationship
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.InstanceRelationships(where) : fdc.InstanceRelationships(where).Select(ir => JObject.Parse(ir.Content)))
                    {
                        if (validate)
                        {
                            var vr = InstanceRelationship.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"InstanceRelationship {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceRelationshipTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance relationship types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceRelationshipTypes(where))
                    {
                        fsc.DeleteInstanceRelationshipType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance_relationship_type" : $"DELETE FROM instance_relationship_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = InstanceRelationshipType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"InstanceRelationshipType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstanceRelationshipType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new InstanceRelationshipType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.InstanceRelationshipTypes(where) : fdc.InstanceRelationshipTypes(where).Select(irt => JObject.Parse(irt.Content)))
                    {
                        if (validate)
                        {
                            var vr = InstanceRelationshipType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"InstanceRelationshipType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceStatuses(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance statuses");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceStatuses(where))
                    {
                        fsc.DeleteInstanceStatus((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance_status" : $"DELETE FROM instance_status WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = InstanceStatus.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"InstanceStatus {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstanceStatus(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new InstanceStatus
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.InstanceStatuses(where) : fdc.InstanceStatuses(where).Select(@is => JObject.Parse(@is.Content)))
                    {
                        if (validate)
                        {
                            var vr = InstanceStatus.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"InstanceStatus {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstanceTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting instance types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.InstanceTypes(where))
                    {
                        fsc.DeleteInstanceType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE instance_type" : $"DELETE FROM instance_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = InstanceType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"InstanceType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstanceType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new InstanceType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.InstanceTypes(where) : fdc.InstanceTypes(where).Select(it => JObject.Parse(it.Content)))
                    {
                        if (validate)
                        {
                            var vr = InstanceType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"InstanceType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteInstitutions(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting institutions");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Institutions(where))
                    {
                        fsc.DeleteInstitution((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE locinstitution" : $"DELETE FROM locinstitution WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Institution.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Institution {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertInstitution(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Institution
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Institutions(where) : fdc.Institutions(where).Select(i2 => JObject.Parse(i2.Content)))
                    {
                        if (validate)
                        {
                            var vr = Institution.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Institution {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteItems(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting items");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Items(where))
                    {
                        fsc.DeleteItem((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE item" : $"DELETE FROM item WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Item.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Item {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertItem(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Item
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Items(where) : fdc.Items(where).Select(i2 => JObject.Parse(i2.Content)))
                    {
                        if (validate)
                        {
                            var vr = Item.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Item {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteItemNoteTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting item note types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ItemNoteTypes(where))
                    {
                        fsc.DeleteItemNoteType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE item_note_type" : $"DELETE FROM item_note_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ItemNoteType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ItemNoteType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertItemNoteType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ItemNoteType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ItemNoteTypes(where) : fdc.ItemNoteTypes(where).Select(@int => JObject.Parse(@int.Content)))
                    {
                        if (validate)
                        {
                            var vr = ItemNoteType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ItemNoteType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLibraries(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting libraries");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Libraries(where))
                    {
                        fsc.DeleteLibrary((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE loclibrary" : $"DELETE FROM loclibrary WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Library.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Library {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertLibrary(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Library
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Libraries(where) : fdc.Libraries(where).Select(l => JObject.Parse(l.Content)))
                    {
                        if (validate)
                        {
                            var vr = Library.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Library {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLoanTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting loan types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.LoanTypes(where))
                    {
                        fsc.DeleteLoanType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE loan_type" : $"DELETE FROM loan_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = LoanType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"LoanType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertLoanType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new LoanType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.LoanTypes(where) : fdc.LoanTypes(where).Select(lt => JObject.Parse(lt.Content)))
                    {
                        if (validate)
                        {
                            var vr = LoanType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"LoanType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLocations(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting locations");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Locations(where))
                    {
                        fsc.DeleteLocation((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE location" : $"DELETE FROM location WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Location.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Location {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertLocation(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Location
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Locations(where) : fdc.Locations(where).Select(l => JObject.Parse(l.Content)))
                    {
                        if (validate)
                        {
                            var vr = Location.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Location {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteLogins(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting logins");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Logins(where))
                    {
                        fsc.DeleteLogin((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE auth_credentials" : $"DELETE FROM auth_credentials WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Login.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Login {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertLogin(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Login
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Logins(where) : fdc.Logins(where).Select(l => JObject.Parse(l.Content)))
                    {
                        if (validate)
                        {
                            var vr = Login.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Login {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteMaterialTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting material types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.MaterialTypes(where))
                    {
                        fsc.DeleteMaterialType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE material_type" : $"DELETE FROM material_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = MaterialType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"MaterialType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertMaterialType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new MaterialType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.MaterialTypes(where) : fdc.MaterialTypes(where).Select(mt => JObject.Parse(mt.Content)))
                    {
                        if (validate)
                        {
                            var vr = MaterialType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"MaterialType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteModeOfIssuances(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting mode of issuances");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ModeOfIssuances(where))
                    {
                        fsc.DeleteModeOfIssuance((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE mode_of_issuance" : $"DELETE FROM mode_of_issuance WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ModeOfIssuance.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ModeOfIssuance {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertModeOfIssuance(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ModeOfIssuance
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ModeOfIssuances(where) : fdc.ModeOfIssuances(where).Select(moi => JObject.Parse(moi.Content)))
                    {
                        if (validate)
                        {
                            var vr = ModeOfIssuance.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ModeOfIssuance {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePermissions(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting permissions");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Permissions(where))
                    {
                        fsc.DeletePermission((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE permissions" : $"DELETE FROM permissions WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Permission.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Permission {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertPermission(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Permission
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Permissions(where) : fdc.Permissions(where).Select(p => JObject.Parse(p.Content)))
                    {
                        if (validate)
                        {
                            var vr = Permission.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Permission {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeletePermissionsUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting permissions users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.PermissionsUsers(where))
                    {
                        fsc.DeletePermissionsUser((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE permissions_users" : $"DELETE FROM permissions_users WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = PermissionsUser.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"PermissionsUser {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertPermissionsUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new PermissionsUser
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.PermissionsUsers(where) : fdc.PermissionsUsers(where).Select(pu => JObject.Parse(pu.Content)))
                    {
                        if (validate)
                        {
                            var vr = PermissionsUser.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"PermissionsUser {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteProxies(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting proxies");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Proxies(where))
                    {
                        fsc.DeleteProxy((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE proxyfor" : $"DELETE FROM proxyfor WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = Proxy.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"Proxy {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertProxy(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new Proxy
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Proxies(where) : fdc.Proxies(where).Select(p => JObject.Parse(p.Content)))
                    {
                        if (validate)
                        {
                            var vr = Proxy.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"Proxy {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteServicePoints(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting service points");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ServicePoints(where))
                    {
                        fsc.DeleteServicePoint((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE service_point" : $"DELETE FROM service_point WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ServicePoint.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ServicePoint {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertServicePoint(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ServicePoint
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ServicePoints(where) : fdc.ServicePoints(where).Select(sp => JObject.Parse(sp.Content)))
                    {
                        if (validate)
                        {
                            var vr = ServicePoint.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ServicePoint {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteServicePointUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting service point users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.ServicePointUsers(where))
                    {
                        fsc.DeleteServicePointUser((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE service_point_user" : $"DELETE FROM service_point_user WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = ServicePointUser.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"ServicePointUser {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertServicePointUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new ServicePointUser
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.ServicePointUsers(where) : fdc.ServicePointUsers(where).Select(spu => JObject.Parse(spu.Content)))
                    {
                        if (validate)
                        {
                            var vr = ServicePointUser.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"ServicePointUser {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteStatisticalCodes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting statistical codes");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.StatisticalCodes(where))
                    {
                        fsc.DeleteStatisticalCode((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE statistical_code" : $"DELETE FROM statistical_code WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = StatisticalCode.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"StatisticalCode {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertStatisticalCode(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new StatisticalCode
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.StatisticalCodes(where) : fdc.StatisticalCodes(where).Select(sc => JObject.Parse(sc.Content)))
                    {
                        if (validate)
                        {
                            var vr = StatisticalCode.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"StatisticalCode {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteStatisticalCodeTypes(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting statistical code types");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.StatisticalCodeTypes(where))
                    {
                        fsc.DeleteStatisticalCodeType((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE statistical_code_type" : $"DELETE FROM statistical_code_type WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = StatisticalCodeType.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"StatisticalCodeType {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertStatisticalCodeType(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new StatisticalCodeType
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.StatisticalCodeTypes(where) : fdc.StatisticalCodeTypes(where).Select(sct => JObject.Parse(sct.Content)))
                    {
                        if (validate)
                        {
                            var vr = StatisticalCodeType.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"StatisticalCodeType {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }

        public static void DeleteUsers(string where = null)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Deleting users");
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
            using (var fsc = new FolioServiceClient())
            {
                var i = 0;
                if (api)
                {
                    var s2 = Stopwatch.StartNew();
                    foreach (var jo in fsc.Users(where))
                    {
                        fsc.DeleteUser((string)jo["id"]);
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
                    i = fdc.Execute(where == null ? "TRUNCATE TABLE users" : $"DELETE FROM users WHERE {where}");
                    fdc.Commit();
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
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            using (var fbcc = new FolioBulkCopyContext())
            using (var fsc = new FolioServiceClient())
            {
                var s2 = Stopwatch.StartNew();
                var js = new JsonSerializer();
                jtr.Read();
                var i = 0;
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    ++i;
                    var jo = (JObject)js.Deserialize(jtr);
                    if (validate)
                    {
                        var vr = User.ValidateContent(jo.ToString());
                        if (vr != ValidationResult.Success) throw new ValidationException($"User {jo["id"]}: {vr.ErrorMessage}");
                    }
                    if (api)
                    {
                        fsc.InsertUser(jo);
                        if (i % 100 == 0)
                        {
                            traceSource.TraceEvent(TraceEventType.Information, 0, $"{i} {s2.Elapsed} {s.Elapsed}");
                            s2.Restart();
                        }
                    }
                    else
                    {
                        fbcc.Insert(new User
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
            {
                using (var sw = new StreamWriter(path))
                using (var jtw = new JsonTextWriter(sw))
                {
                    var js = new JsonSerializer { Formatting = Formatting.Indented };
                    var s2 = Stopwatch.StartNew();
                    jtw.WriteStartArray();
                    var i = 0;
                    foreach (var jo in api ? fsc.Users(where) : fdc.Users(where).Select(u => JObject.Parse(u.Content)))
                    {
                        if (validate)
                        {
                            var vr = User.ValidateContent(jo.ToString());
                            if (vr != ValidationResult.Success) throw new ValidationException($"User {jo["id"]}: {vr.ErrorMessage}");
                        }
                        js.Serialize(jtw, jo);
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
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"{s.Elapsed} elapsed");
        }
    }
}
