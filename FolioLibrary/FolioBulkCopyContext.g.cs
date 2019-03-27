using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace FolioLibrary
{
    public partial class FolioBulkCopyContext : IDisposable
    {
        private DataTable accountsDataTable, account2sDataTable, addressesDataTable, addressTypesDataTable, adjustmentsDataTable, agreementsDataTable, alertsDataTable, aliasesDataTable, alternativeTitleTypesDataTable, auditLoansDataTable, authAttemptsDataTable, authCredentialsHistoriesDataTable, authPasswordActionsDataTable, blocksDataTable, budgetsDataTable, callNumberTypesDataTable, campusesDataTable, cancellationReasonsDataTable, categoriesDataTable, circulationRulesDataTable, claimsDataTable, classificationTypesDataTable, commentsDataTable, contactsDataTable, contactCategoriesDataTable, contributorNameTypesDataTable, contributorTypesDataTable, costsDataTable, detailsDataTable, electronicAccessRelationshipsDataTable, emailsDataTable, eresourcesDataTable, eventLogsDataTable, feesDataTable, feeActionsDataTable, fiscalYearsDataTable, fixedDueDateSchedulesDataTable, fundsDataTable, fundDistributionsDataTable, fundDistribution2sDataTable, groupsDataTable, holdingsDataTable, holdingNoteTypesDataTable, holdingTypesDataTable, idTypesDataTable, illPoliciesDataTable, instancesDataTable, instanceFormatsDataTable, instanceRelationshipsDataTable, instanceRelationshipTypesDataTable, instanceSourceMarcsDataTable, instanceStatusesDataTable, instanceTypesDataTable, institutionsDataTable, interfacesDataTable, itemsDataTable, itemNoteTypesDataTable, ledgersDataTable, librariesDataTable, loansDataTable, loanPoliciesDataTable, loanTypesDataTable, locationsDataTable, loginsDataTable, materialTypesDataTable, modeOfIssuancesDataTable, notesDataTable, ordersDataTable, orderItemsDataTable, orderItemLocationsDataTable, ownersDataTable, patronNoticePoliciesDataTable, paymentsDataTable, permissionsDataTable, permissionsUsersDataTable, phoneNumbersDataTable, physicalsDataTable, piecesDataTable, proxiesDataTable, refundsDataTable, reportingCodesDataTable, requestsDataTable, requestPoliciesDataTable, servicePointsDataTable, servicePointUsersDataTable, sourcesDataTable, staffSlipsDataTable, statisticalCodesDataTable, statisticalCodeTypesDataTable, tagsDataTable, transactionsDataTable, transfersDataTable, transferCriteriasDataTable, urlsDataTable, usersDataTable, vendorsDataTable, vendorCategoriesDataTable, vendorDetailsDataTable, vendorTypesDataTable, waivesDataTable;
        private bool checkConstraints;
        private string connectionString;
        private bool identityInsert;
        private string name;
        private string providerName;
        private dynamic sqlBulkCopy;
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";
        public string ProviderName => providerName;

        public FolioBulkCopyContext(bool identityInsert = true, bool checkConstraints = false) : this("FolioContext", identityInsert, checkConstraints) { }

        public FolioBulkCopyContext(string name, bool identityInsert = true, bool checkConstraints = false)
        {
            this.name = name;
            if (ConfigurationManager.ConnectionStrings[name] == null) return;
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            this.identityInsert = identityInsert;
            this.checkConstraints = checkConstraints;
            if (IsSqlServer) throw new NotImplementedException();
            else if (IsPostgreSql) sqlBulkCopy = new PostgreSqlBulkCopy(name, checkConstraints);
            else
            {
                sqlBulkCopy = new MySqlBulkCopy(name, checkConstraints);
            }
        }

        public int ExecuteNonQuery(string commandText) => sqlBulkCopy.ExecuteNonQuery(commandText);

        public void Insert(Account account)
        {
            if (accountsDataTable == null)
            {
                accountsDataTable = new DataTable();
                accountsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                accountsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                accountsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                accountsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                accountsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = accountsDataTable.NewRow();
            dr["id"] = (object)account.Id ?? DBNull.Value;
            dr["jsonb"] = (object)account.Content ?? DBNull.Value;
            dr["creation_date"] = (object)account.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)account.CreationUserId ?? DBNull.Value;
            accountsDataTable.Rows.Add(dr);
        }

        public void Insert(Account2 account2)
        {
            if (account2sDataTable == null)
            {
                account2sDataTable = new DataTable();
                account2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                account2sDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                account2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                account2sDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                account2sDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = account2sDataTable.NewRow();
            dr["id"] = (object)account2.Id ?? DBNull.Value;
            dr["jsonb"] = (object)account2.Content ?? DBNull.Value;
            dr["creation_date"] = (object)account2.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)account2.CreationUserId ?? DBNull.Value;
            account2sDataTable.Rows.Add(dr);
        }

        public void Insert(Address address)
        {
            if (addressesDataTable == null)
            {
                addressesDataTable = new DataTable();
                addressesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                addressesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                addressesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                addressesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                addressesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = addressesDataTable.NewRow();
            dr["id"] = (object)address.Id ?? DBNull.Value;
            dr["jsonb"] = (object)address.Content ?? DBNull.Value;
            dr["creation_date"] = (object)address.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)address.CreationUserId ?? DBNull.Value;
            addressesDataTable.Rows.Add(dr);
        }

        public void Insert(AddressType addressType)
        {
            if (addressTypesDataTable == null)
            {
                addressTypesDataTable = new DataTable();
                addressTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                addressTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                addressTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                addressTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                addressTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = addressTypesDataTable.NewRow();
            dr["id"] = (object)addressType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)addressType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)addressType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)addressType.CreationUserId ?? DBNull.Value;
            addressTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Adjustment adjustment)
        {
            if (adjustmentsDataTable == null)
            {
                adjustmentsDataTable = new DataTable();
                adjustmentsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                adjustmentsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                adjustmentsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                adjustmentsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                adjustmentsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = adjustmentsDataTable.NewRow();
            dr["id"] = (object)adjustment.Id ?? DBNull.Value;
            dr["jsonb"] = (object)adjustment.Content ?? DBNull.Value;
            dr["creation_date"] = (object)adjustment.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)adjustment.CreationUserId ?? DBNull.Value;
            adjustmentsDataTable.Rows.Add(dr);
        }

        public void Insert(Agreement agreement)
        {
            if (agreementsDataTable == null)
            {
                agreementsDataTable = new DataTable();
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                agreementsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = agreementsDataTable.NewRow();
            dr["id"] = (object)agreement.Id ?? DBNull.Value;
            dr["jsonb"] = (object)agreement.Content ?? DBNull.Value;
            dr["creation_date"] = (object)agreement.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)agreement.CreationUserId ?? DBNull.Value;
            agreementsDataTable.Rows.Add(dr);
        }

        public void Insert(Alert alert)
        {
            if (alertsDataTable == null)
            {
                alertsDataTable = new DataTable();
                alertsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                alertsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                alertsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                alertsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                alertsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = alertsDataTable.NewRow();
            dr["id"] = (object)alert.Id ?? DBNull.Value;
            dr["jsonb"] = (object)alert.Content ?? DBNull.Value;
            dr["creation_date"] = (object)alert.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)alert.CreationUserId ?? DBNull.Value;
            alertsDataTable.Rows.Add(dr);
        }

        public void Insert(Alias alias)
        {
            if (aliasesDataTable == null)
            {
                aliasesDataTable = new DataTable();
                aliasesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                aliasesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                aliasesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                aliasesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                aliasesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = aliasesDataTable.NewRow();
            dr["id"] = (object)alias.Id ?? DBNull.Value;
            dr["jsonb"] = (object)alias.Content ?? DBNull.Value;
            dr["creation_date"] = (object)alias.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)alias.CreationUserId ?? DBNull.Value;
            aliasesDataTable.Rows.Add(dr);
        }

        public void Insert(AlternativeTitleType alternativeTitleType)
        {
            if (alternativeTitleTypesDataTable == null)
            {
                alternativeTitleTypesDataTable = new DataTable();
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                alternativeTitleTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = alternativeTitleTypesDataTable.NewRow();
            dr["_id"] = (object)alternativeTitleType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)alternativeTitleType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)alternativeTitleType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)alternativeTitleType.CreationUserId ?? DBNull.Value;
            alternativeTitleTypesDataTable.Rows.Add(dr);
        }

        public void Insert(AuditLoan auditLoan)
        {
            if (auditLoansDataTable == null)
            {
                auditLoansDataTable = new DataTable();
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "orig_id", DataType = typeof(Guid) });
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "operation", DataType = typeof(string) });
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                auditLoansDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = auditLoansDataTable.NewRow();
            dr["_id"] = (object)auditLoan.Id ?? DBNull.Value;
            dr["orig_id"] = (object)auditLoan.OrigId ?? DBNull.Value;
            dr["operation"] = (object)auditLoan.Operation ?? DBNull.Value;
            dr["jsonb"] = (object)auditLoan.Content ?? DBNull.Value;
            dr["created_date"] = (object)auditLoan.CreationTime ?? DBNull.Value;
            auditLoansDataTable.Rows.Add(dr);
        }

        public void Insert(AuthAttempt authAttempt)
        {
            if (authAttemptsDataTable == null)
            {
                authAttemptsDataTable = new DataTable();
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authAttemptsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authAttemptsDataTable.NewRow();
            dr["_id"] = (object)authAttempt.Id ?? DBNull.Value;
            dr["jsonb"] = (object)authAttempt.Content ?? DBNull.Value;
            dr["creation_date"] = (object)authAttempt.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)authAttempt.CreationUserId ?? DBNull.Value;
            authAttemptsDataTable.Rows.Add(dr);
        }

        public void Insert(AuthCredentialsHistory authCredentialsHistory)
        {
            if (authCredentialsHistoriesDataTable == null)
            {
                authCredentialsHistoriesDataTable = new DataTable();
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authCredentialsHistoriesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authCredentialsHistoriesDataTable.NewRow();
            dr["_id"] = (object)authCredentialsHistory.Id ?? DBNull.Value;
            dr["jsonb"] = (object)authCredentialsHistory.Content ?? DBNull.Value;
            dr["creation_date"] = (object)authCredentialsHistory.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)authCredentialsHistory.CreationUserId ?? DBNull.Value;
            authCredentialsHistoriesDataTable.Rows.Add(dr);
        }

        public void Insert(AuthPasswordAction authPasswordAction)
        {
            if (authPasswordActionsDataTable == null)
            {
                authPasswordActionsDataTable = new DataTable();
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authPasswordActionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authPasswordActionsDataTable.NewRow();
            dr["_id"] = (object)authPasswordAction.Id ?? DBNull.Value;
            dr["jsonb"] = (object)authPasswordAction.Content ?? DBNull.Value;
            dr["creation_date"] = (object)authPasswordAction.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)authPasswordAction.CreationUserId ?? DBNull.Value;
            authPasswordActionsDataTable.Rows.Add(dr);
        }

        public void Insert(Block block)
        {
            if (blocksDataTable == null)
            {
                blocksDataTable = new DataTable();
                blocksDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                blocksDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                blocksDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                blocksDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                blocksDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = blocksDataTable.NewRow();
            dr["id"] = (object)block.Id ?? DBNull.Value;
            dr["jsonb"] = (object)block.Content ?? DBNull.Value;
            dr["creation_date"] = (object)block.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)block.CreationUserId ?? DBNull.Value;
            blocksDataTable.Rows.Add(dr);
        }

        public void Insert(Budget budget)
        {
            if (budgetsDataTable == null)
            {
                budgetsDataTable = new DataTable();
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fund_id", DataType = typeof(Guid) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscal_year_id", DataType = typeof(Guid) });
                budgetsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = budgetsDataTable.NewRow();
            dr["id"] = (object)budget.Id ?? DBNull.Value;
            dr["jsonb"] = (object)budget.Content ?? DBNull.Value;
            dr["creation_date"] = (object)budget.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)budget.CreationUserId ?? DBNull.Value;
            dr["fund_id"] = (object)budget.FundId ?? DBNull.Value;
            dr["fiscal_year_id"] = (object)budget.FiscalYearId ?? DBNull.Value;
            budgetsDataTable.Rows.Add(dr);
        }

        public void Insert(CallNumberType callNumberType)
        {
            if (callNumberTypesDataTable == null)
            {
                callNumberTypesDataTable = new DataTable();
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                callNumberTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = callNumberTypesDataTable.NewRow();
            dr["_id"] = (object)callNumberType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)callNumberType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)callNumberType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)callNumberType.CreationUserId ?? DBNull.Value;
            callNumberTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Campus campus)
        {
            if (campusesDataTable == null)
            {
                campusesDataTable = new DataTable();
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "institutionid", DataType = typeof(Guid) });
                campusesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = campusesDataTable.NewRow();
            dr["_id"] = (object)campus.Id ?? DBNull.Value;
            dr["jsonb"] = (object)campus.Content ?? DBNull.Value;
            dr["creation_date"] = (object)campus.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)campus.CreationUserId ?? DBNull.Value;
            dr["institutionid"] = (object)campus.Institutionid ?? DBNull.Value;
            campusesDataTable.Rows.Add(dr);
        }

        public void Insert(CancellationReason cancellationReason)
        {
            if (cancellationReasonsDataTable == null)
            {
                cancellationReasonsDataTable = new DataTable();
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                cancellationReasonsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = cancellationReasonsDataTable.NewRow();
            dr["_id"] = (object)cancellationReason.Id ?? DBNull.Value;
            dr["jsonb"] = (object)cancellationReason.Content ?? DBNull.Value;
            dr["creation_date"] = (object)cancellationReason.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)cancellationReason.CreationUserId ?? DBNull.Value;
            cancellationReasonsDataTable.Rows.Add(dr);
        }

        public void Insert(Category category)
        {
            if (categoriesDataTable == null)
            {
                categoriesDataTable = new DataTable();
                categoriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                categoriesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                categoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                categoriesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                categoriesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = categoriesDataTable.NewRow();
            dr["id"] = (object)category.Id ?? DBNull.Value;
            dr["jsonb"] = (object)category.Content ?? DBNull.Value;
            dr["creation_date"] = (object)category.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)category.CreationUserId ?? DBNull.Value;
            categoriesDataTable.Rows.Add(dr);
        }

        public void Insert(CirculationRule circulationRule)
        {
            if (circulationRulesDataTable == null)
            {
                circulationRulesDataTable = new DataTable();
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "lock", DataType = typeof(bool) });
                circulationRulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = circulationRulesDataTable.NewRow();
            dr["_id"] = (object)circulationRule.Id ?? DBNull.Value;
            dr["jsonb"] = (object)circulationRule.Content ?? DBNull.Value;
            dr["lock"] = (object)circulationRule.Lock ?? DBNull.Value;
            circulationRulesDataTable.Rows.Add(dr);
        }

        public void Insert(Claim claim)
        {
            if (claimsDataTable == null)
            {
                claimsDataTable = new DataTable();
                claimsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                claimsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                claimsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                claimsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                claimsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = claimsDataTable.NewRow();
            dr["id"] = (object)claim.Id ?? DBNull.Value;
            dr["jsonb"] = (object)claim.Content ?? DBNull.Value;
            dr["creation_date"] = (object)claim.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)claim.CreationUserId ?? DBNull.Value;
            claimsDataTable.Rows.Add(dr);
        }

        public void Insert(ClassificationType classificationType)
        {
            if (classificationTypesDataTable == null)
            {
                classificationTypesDataTable = new DataTable();
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                classificationTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = classificationTypesDataTable.NewRow();
            dr["_id"] = (object)classificationType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)classificationType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)classificationType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)classificationType.CreationUserId ?? DBNull.Value;
            classificationTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Comment comment)
        {
            if (commentsDataTable == null)
            {
                commentsDataTable = new DataTable();
                commentsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                commentsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                commentsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                commentsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                commentsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = commentsDataTable.NewRow();
            dr["id"] = (object)comment.Id ?? DBNull.Value;
            dr["jsonb"] = (object)comment.Content ?? DBNull.Value;
            dr["creation_date"] = (object)comment.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)comment.CreationUserId ?? DBNull.Value;
            commentsDataTable.Rows.Add(dr);
        }

        public void Insert(Contact contact)
        {
            if (contactsDataTable == null)
            {
                contactsDataTable = new DataTable();
                contactsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                contactsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contactsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                contactsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                contactsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contactsDataTable.NewRow();
            dr["id"] = (object)contact.Id ?? DBNull.Value;
            dr["jsonb"] = (object)contact.Content ?? DBNull.Value;
            dr["creation_date"] = (object)contact.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)contact.CreationUserId ?? DBNull.Value;
            contactsDataTable.Rows.Add(dr);
        }

        public void Insert(ContactCategory contactCategory)
        {
            if (contactCategoriesDataTable == null)
            {
                contactCategoriesDataTable = new DataTable();
                contactCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                contactCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contactCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                contactCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                contactCategoriesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contactCategoriesDataTable.NewRow();
            dr["id"] = (object)contactCategory.Id ?? DBNull.Value;
            dr["jsonb"] = (object)contactCategory.Content ?? DBNull.Value;
            dr["creation_date"] = (object)contactCategory.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)contactCategory.CreationUserId ?? DBNull.Value;
            contactCategoriesDataTable.Rows.Add(dr);
        }

        public void Insert(ContributorNameType contributorNameType)
        {
            if (contributorNameTypesDataTable == null)
            {
                contributorNameTypesDataTable = new DataTable();
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                contributorNameTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contributorNameTypesDataTable.NewRow();
            dr["_id"] = (object)contributorNameType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)contributorNameType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)contributorNameType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)contributorNameType.CreationUserId ?? DBNull.Value;
            contributorNameTypesDataTable.Rows.Add(dr);
        }

        public void Insert(ContributorType contributorType)
        {
            if (contributorTypesDataTable == null)
            {
                contributorTypesDataTable = new DataTable();
                contributorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                contributorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contributorTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contributorTypesDataTable.NewRow();
            dr["_id"] = (object)contributorType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)contributorType.Content ?? DBNull.Value;
            contributorTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Cost cost)
        {
            if (costsDataTable == null)
            {
                costsDataTable = new DataTable();
                costsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                costsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                costsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                costsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                costsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = costsDataTable.NewRow();
            dr["id"] = (object)cost.Id ?? DBNull.Value;
            dr["jsonb"] = (object)cost.Content ?? DBNull.Value;
            dr["creation_date"] = (object)cost.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)cost.CreationUserId ?? DBNull.Value;
            costsDataTable.Rows.Add(dr);
        }

        public void Insert(Detail detail)
        {
            if (detailsDataTable == null)
            {
                detailsDataTable = new DataTable();
                detailsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                detailsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                detailsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                detailsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                detailsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = detailsDataTable.NewRow();
            dr["id"] = (object)detail.Id ?? DBNull.Value;
            dr["jsonb"] = (object)detail.Content ?? DBNull.Value;
            dr["creation_date"] = (object)detail.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)detail.CreationUserId ?? DBNull.Value;
            detailsDataTable.Rows.Add(dr);
        }

        public void Insert(ElectronicAccessRelationship electronicAccessRelationship)
        {
            if (electronicAccessRelationshipsDataTable == null)
            {
                electronicAccessRelationshipsDataTable = new DataTable();
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                electronicAccessRelationshipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = electronicAccessRelationshipsDataTable.NewRow();
            dr["_id"] = (object)electronicAccessRelationship.Id ?? DBNull.Value;
            dr["jsonb"] = (object)electronicAccessRelationship.Content ?? DBNull.Value;
            dr["creation_date"] = (object)electronicAccessRelationship.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)electronicAccessRelationship.CreationUserId ?? DBNull.Value;
            electronicAccessRelationshipsDataTable.Rows.Add(dr);
        }

        public void Insert(Email email)
        {
            if (emailsDataTable == null)
            {
                emailsDataTable = new DataTable();
                emailsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                emailsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                emailsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                emailsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                emailsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = emailsDataTable.NewRow();
            dr["id"] = (object)email.Id ?? DBNull.Value;
            dr["jsonb"] = (object)email.Content ?? DBNull.Value;
            dr["creation_date"] = (object)email.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)email.CreationUserId ?? DBNull.Value;
            emailsDataTable.Rows.Add(dr);
        }

        public void Insert(Eresource eresource)
        {
            if (eresourcesDataTable == null)
            {
                eresourcesDataTable = new DataTable();
                eresourcesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                eresourcesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                eresourcesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                eresourcesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                eresourcesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = eresourcesDataTable.NewRow();
            dr["id"] = (object)eresource.Id ?? DBNull.Value;
            dr["jsonb"] = (object)eresource.Content ?? DBNull.Value;
            dr["creation_date"] = (object)eresource.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)eresource.CreationUserId ?? DBNull.Value;
            eresourcesDataTable.Rows.Add(dr);
        }

        public void Insert(EventLog eventLog)
        {
            if (eventLogsDataTable == null)
            {
                eventLogsDataTable = new DataTable();
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                eventLogsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = eventLogsDataTable.NewRow();
            dr["_id"] = (object)eventLog.Id ?? DBNull.Value;
            dr["jsonb"] = (object)eventLog.Content ?? DBNull.Value;
            dr["creation_date"] = (object)eventLog.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)eventLog.CreationUserId ?? DBNull.Value;
            eventLogsDataTable.Rows.Add(dr);
        }

        public void Insert(Fee fee)
        {
            if (feesDataTable == null)
            {
                feesDataTable = new DataTable();
                feesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                feesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                feesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                feesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                feesDataTable.Columns.Add(new DataColumn { ColumnName = "ownerid", DataType = typeof(Guid) });
                feesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = feesDataTable.NewRow();
            dr["id"] = (object)fee.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fee.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fee.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fee.CreationUserId ?? DBNull.Value;
            dr["ownerid"] = (object)fee.Ownerid ?? DBNull.Value;
            feesDataTable.Rows.Add(dr);
        }

        public void Insert(FeeAction feeAction)
        {
            if (feeActionsDataTable == null)
            {
                feeActionsDataTable = new DataTable();
                feeActionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                feeActionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                feeActionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                feeActionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                feeActionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = feeActionsDataTable.NewRow();
            dr["id"] = (object)feeAction.Id ?? DBNull.Value;
            dr["jsonb"] = (object)feeAction.Content ?? DBNull.Value;
            dr["creation_date"] = (object)feeAction.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)feeAction.CreationUserId ?? DBNull.Value;
            feeActionsDataTable.Rows.Add(dr);
        }

        public void Insert(FiscalYear fiscalYear)
        {
            if (fiscalYearsDataTable == null)
            {
                fiscalYearsDataTable = new DataTable();
                fiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                fiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                fiscalYearsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fiscalYearsDataTable.NewRow();
            dr["id"] = (object)fiscalYear.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fiscalYear.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fiscalYear.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fiscalYear.CreationUserId ?? DBNull.Value;
            fiscalYearsDataTable.Rows.Add(dr);
        }

        public void Insert(FixedDueDateSchedule fixedDueDateSchedule)
        {
            if (fixedDueDateSchedulesDataTable == null)
            {
                fixedDueDateSchedulesDataTable = new DataTable();
                fixedDueDateSchedulesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                fixedDueDateSchedulesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fixedDueDateSchedulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fixedDueDateSchedulesDataTable.NewRow();
            dr["_id"] = (object)fixedDueDateSchedule.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fixedDueDateSchedule.Content ?? DBNull.Value;
            fixedDueDateSchedulesDataTable.Rows.Add(dr);
        }

        public void Insert(Fund fund)
        {
            if (fundsDataTable == null)
            {
                fundsDataTable = new DataTable();
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "ledger_id", DataType = typeof(Guid) });
                fundsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fundsDataTable.NewRow();
            dr["id"] = (object)fund.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fund.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fund.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fund.CreationUserId ?? DBNull.Value;
            dr["ledger_id"] = (object)fund.LedgerId ?? DBNull.Value;
            fundsDataTable.Rows.Add(dr);
        }

        public void Insert(FundDistribution fundDistribution)
        {
            if (fundDistributionsDataTable == null)
            {
                fundDistributionsDataTable = new DataTable();
                fundDistributionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fundDistributionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fundDistributionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                fundDistributionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                fundDistributionsDataTable.Columns.Add(new DataColumn { ColumnName = "budget_id", DataType = typeof(Guid) });
                fundDistributionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fundDistributionsDataTable.NewRow();
            dr["id"] = (object)fundDistribution.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fundDistribution.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fundDistribution.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fundDistribution.CreationUserId ?? DBNull.Value;
            dr["budget_id"] = (object)fundDistribution.BudgetId ?? DBNull.Value;
            fundDistributionsDataTable.Rows.Add(dr);
        }

        public void Insert(FundDistribution2 fundDistribution2)
        {
            if (fundDistribution2sDataTable == null)
            {
                fundDistribution2sDataTable = new DataTable();
                fundDistribution2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fundDistribution2sDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fundDistribution2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                fundDistribution2sDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                fundDistribution2sDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fundDistribution2sDataTable.NewRow();
            dr["id"] = (object)fundDistribution2.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fundDistribution2.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fundDistribution2.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fundDistribution2.CreationUserId ?? DBNull.Value;
            fundDistribution2sDataTable.Rows.Add(dr);
        }

        public void Insert(Group group)
        {
            if (groupsDataTable == null)
            {
                groupsDataTable = new DataTable();
                groupsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                groupsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                groupsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                groupsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                groupsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = groupsDataTable.NewRow();
            dr["id"] = (object)group.Id ?? DBNull.Value;
            dr["jsonb"] = (object)group.Content ?? DBNull.Value;
            dr["creation_date"] = (object)group.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)group.CreationUserId ?? DBNull.Value;
            groupsDataTable.Rows.Add(dr);
        }

        public void Insert(Holding holding)
        {
            if (holdingsDataTable == null)
            {
                holdingsDataTable = new DataTable();
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "instanceid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "permanentlocationid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "temporarylocationid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "holdingstypeid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "callnumbertypeid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "illpolicyid", DataType = typeof(Guid) });
                holdingsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = holdingsDataTable.NewRow();
            dr["_id"] = (object)holding.Id ?? DBNull.Value;
            dr["jsonb"] = (object)holding.Content ?? DBNull.Value;
            dr["creation_date"] = (object)holding.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)holding.CreationUserId ?? DBNull.Value;
            dr["instanceid"] = (object)holding.Instanceid ?? DBNull.Value;
            dr["permanentlocationid"] = (object)holding.Permanentlocationid ?? DBNull.Value;
            dr["temporarylocationid"] = (object)holding.Temporarylocationid ?? DBNull.Value;
            dr["holdingstypeid"] = (object)holding.Holdingstypeid ?? DBNull.Value;
            dr["callnumbertypeid"] = (object)holding.Callnumbertypeid ?? DBNull.Value;
            dr["illpolicyid"] = (object)holding.Illpolicyid ?? DBNull.Value;
            holdingsDataTable.Rows.Add(dr);
        }

        public void Insert(HoldingNoteType holdingNoteType)
        {
            if (holdingNoteTypesDataTable == null)
            {
                holdingNoteTypesDataTable = new DataTable();
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                holdingNoteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = holdingNoteTypesDataTable.NewRow();
            dr["_id"] = (object)holdingNoteType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)holdingNoteType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)holdingNoteType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)holdingNoteType.CreationUserId ?? DBNull.Value;
            holdingNoteTypesDataTable.Rows.Add(dr);
        }

        public void Insert(HoldingType holdingType)
        {
            if (holdingTypesDataTable == null)
            {
                holdingTypesDataTable = new DataTable();
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                holdingTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = holdingTypesDataTable.NewRow();
            dr["_id"] = (object)holdingType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)holdingType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)holdingType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)holdingType.CreationUserId ?? DBNull.Value;
            holdingTypesDataTable.Rows.Add(dr);
        }

        public void Insert(IdType idType)
        {
            if (idTypesDataTable == null)
            {
                idTypesDataTable = new DataTable();
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                idTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = idTypesDataTable.NewRow();
            dr["_id"] = (object)idType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)idType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)idType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)idType.CreationUserId ?? DBNull.Value;
            idTypesDataTable.Rows.Add(dr);
        }

        public void Insert(IllPolicy illPolicy)
        {
            if (illPoliciesDataTable == null)
            {
                illPoliciesDataTable = new DataTable();
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                illPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = illPoliciesDataTable.NewRow();
            dr["_id"] = (object)illPolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)illPolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)illPolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)illPolicy.CreationUserId ?? DBNull.Value;
            illPoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(Instance instance)
        {
            if (instancesDataTable == null)
            {
                instancesDataTable = new DataTable();
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "instancestatusid", DataType = typeof(Guid) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "modeofissuanceid", DataType = typeof(Guid) });
                instancesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instancesDataTable.NewRow();
            dr["_id"] = (object)instance.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instance.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instance.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instance.CreationUserId ?? DBNull.Value;
            dr["instancestatusid"] = (object)instance.Instancestatusid ?? DBNull.Value;
            dr["modeofissuanceid"] = (object)instance.Modeofissuanceid ?? DBNull.Value;
            instancesDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceFormat instanceFormat)
        {
            if (instanceFormatsDataTable == null)
            {
                instanceFormatsDataTable = new DataTable();
                instanceFormatsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceFormatsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceFormatsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceFormatsDataTable.NewRow();
            dr["_id"] = (object)instanceFormat.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceFormat.Content ?? DBNull.Value;
            instanceFormatsDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceRelationship instanceRelationship)
        {
            if (instanceRelationshipsDataTable == null)
            {
                instanceRelationshipsDataTable = new DataTable();
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "superinstanceid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "subinstanceid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "instancerelationshiptypeid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceRelationshipsDataTable.NewRow();
            dr["_id"] = (object)instanceRelationship.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceRelationship.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instanceRelationship.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instanceRelationship.CreationUserId ?? DBNull.Value;
            dr["superinstanceid"] = (object)instanceRelationship.Superinstanceid ?? DBNull.Value;
            dr["subinstanceid"] = (object)instanceRelationship.Subinstanceid ?? DBNull.Value;
            dr["instancerelationshiptypeid"] = (object)instanceRelationship.Instancerelationshiptypeid ?? DBNull.Value;
            instanceRelationshipsDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceRelationshipType instanceRelationshipType)
        {
            if (instanceRelationshipTypesDataTable == null)
            {
                instanceRelationshipTypesDataTable = new DataTable();
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceRelationshipTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceRelationshipTypesDataTable.NewRow();
            dr["_id"] = (object)instanceRelationshipType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceRelationshipType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instanceRelationshipType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instanceRelationshipType.CreationUserId ?? DBNull.Value;
            instanceRelationshipTypesDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceSourceMarc instanceSourceMarc)
        {
            if (instanceSourceMarcsDataTable == null)
            {
                instanceSourceMarcsDataTable = new DataTable();
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceSourceMarcsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceSourceMarcsDataTable.NewRow();
            dr["_id"] = (object)instanceSourceMarc.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceSourceMarc.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instanceSourceMarc.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instanceSourceMarc.CreationUserId ?? DBNull.Value;
            instanceSourceMarcsDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceStatus instanceStatus)
        {
            if (instanceStatusesDataTable == null)
            {
                instanceStatusesDataTable = new DataTable();
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceStatusesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceStatusesDataTable.NewRow();
            dr["_id"] = (object)instanceStatus.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceStatus.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instanceStatus.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instanceStatus.CreationUserId ?? DBNull.Value;
            instanceStatusesDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceType instanceType)
        {
            if (instanceTypesDataTable == null)
            {
                instanceTypesDataTable = new DataTable();
                instanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                instanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceTypesDataTable.NewRow();
            dr["_id"] = (object)instanceType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceType.Content ?? DBNull.Value;
            instanceTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Institution institution)
        {
            if (institutionsDataTable == null)
            {
                institutionsDataTable = new DataTable();
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                institutionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = institutionsDataTable.NewRow();
            dr["_id"] = (object)institution.Id ?? DBNull.Value;
            dr["jsonb"] = (object)institution.Content ?? DBNull.Value;
            dr["creation_date"] = (object)institution.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)institution.CreationUserId ?? DBNull.Value;
            institutionsDataTable.Rows.Add(dr);
        }

        public void Insert(Interface @interface)
        {
            if (interfacesDataTable == null)
            {
                interfacesDataTable = new DataTable();
                interfacesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                interfacesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                interfacesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                interfacesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                interfacesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = interfacesDataTable.NewRow();
            dr["id"] = (object)@interface.Id ?? DBNull.Value;
            dr["jsonb"] = (object)@interface.Content ?? DBNull.Value;
            dr["creation_date"] = (object)@interface.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)@interface.CreationUserId ?? DBNull.Value;
            interfacesDataTable.Rows.Add(dr);
        }

        public void Insert(Item item)
        {
            if (itemsDataTable == null)
            {
                itemsDataTable = new DataTable();
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "holdingsrecordid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "permanentloantypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "temporaryloantypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "materialtypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "permanentlocationid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "temporarylocationid", DataType = typeof(Guid) });
                itemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = itemsDataTable.NewRow();
            dr["_id"] = (object)item.Id ?? DBNull.Value;
            dr["jsonb"] = (object)item.Content ?? DBNull.Value;
            dr["creation_date"] = (object)item.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)item.CreationUserId ?? DBNull.Value;
            dr["holdingsrecordid"] = (object)item.Holdingsrecordid ?? DBNull.Value;
            dr["permanentloantypeid"] = (object)item.Permanentloantypeid ?? DBNull.Value;
            dr["temporaryloantypeid"] = (object)item.Temporaryloantypeid ?? DBNull.Value;
            dr["materialtypeid"] = (object)item.Materialtypeid ?? DBNull.Value;
            dr["permanentlocationid"] = (object)item.Permanentlocationid ?? DBNull.Value;
            dr["temporarylocationid"] = (object)item.Temporarylocationid ?? DBNull.Value;
            itemsDataTable.Rows.Add(dr);
        }

        public void Insert(ItemNoteType itemNoteType)
        {
            if (itemNoteTypesDataTable == null)
            {
                itemNoteTypesDataTable = new DataTable();
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                itemNoteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = itemNoteTypesDataTable.NewRow();
            dr["_id"] = (object)itemNoteType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)itemNoteType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)itemNoteType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)itemNoteType.CreationUserId ?? DBNull.Value;
            itemNoteTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Ledger ledger)
        {
            if (ledgersDataTable == null)
            {
                ledgersDataTable = new DataTable();
                ledgersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                ledgersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                ledgersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                ledgersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                ledgersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = ledgersDataTable.NewRow();
            dr["id"] = (object)ledger.Id ?? DBNull.Value;
            dr["jsonb"] = (object)ledger.Content ?? DBNull.Value;
            dr["creation_date"] = (object)ledger.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)ledger.CreationUserId ?? DBNull.Value;
            ledgersDataTable.Rows.Add(dr);
        }

        public void Insert(Library library)
        {
            if (librariesDataTable == null)
            {
                librariesDataTable = new DataTable();
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "campusid", DataType = typeof(Guid) });
                librariesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = librariesDataTable.NewRow();
            dr["_id"] = (object)library.Id ?? DBNull.Value;
            dr["jsonb"] = (object)library.Content ?? DBNull.Value;
            dr["creation_date"] = (object)library.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)library.CreationUserId ?? DBNull.Value;
            dr["campusid"] = (object)library.Campusid ?? DBNull.Value;
            librariesDataTable.Rows.Add(dr);
        }

        public void Insert(Loan loan)
        {
            if (loansDataTable == null)
            {
                loansDataTable = new DataTable();
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loansDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loansDataTable.NewRow();
            dr["_id"] = (object)loan.Id ?? DBNull.Value;
            dr["jsonb"] = (object)loan.Content ?? DBNull.Value;
            dr["creation_date"] = (object)loan.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)loan.CreationUserId ?? DBNull.Value;
            loansDataTable.Rows.Add(dr);
        }

        public void Insert(LoanPolicy loanPolicy)
        {
            if (loanPoliciesDataTable == null)
            {
                loanPoliciesDataTable = new DataTable();
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "loanspolicy_fixedduedatescheduleid", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "renewalspolicy_alternatefixedduedatescheduleid", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loanPoliciesDataTable.NewRow();
            dr["_id"] = (object)loanPolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)loanPolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)loanPolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)loanPolicy.CreationUserId ?? DBNull.Value;
            dr["loanspolicy_fixedduedatescheduleid"] = (object)loanPolicy.LoanspolicyFixedduedatescheduleid ?? DBNull.Value;
            dr["renewalspolicy_alternatefixedduedatescheduleid"] = (object)loanPolicy.RenewalspolicyAlternatefixedduedatescheduleid ?? DBNull.Value;
            loanPoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(LoanType loanType)
        {
            if (loanTypesDataTable == null)
            {
                loanTypesDataTable = new DataTable();
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loanTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loanTypesDataTable.NewRow();
            dr["_id"] = (object)loanType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)loanType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)loanType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)loanType.CreationUserId ?? DBNull.Value;
            loanTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Location location)
        {
            if (locationsDataTable == null)
            {
                locationsDataTable = new DataTable();
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "institutionid", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "campusid", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "libraryid", DataType = typeof(Guid) });
                locationsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = locationsDataTable.NewRow();
            dr["_id"] = (object)location.Id ?? DBNull.Value;
            dr["jsonb"] = (object)location.Content ?? DBNull.Value;
            dr["creation_date"] = (object)location.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)location.CreationUserId ?? DBNull.Value;
            dr["institutionid"] = (object)location.Institutionid ?? DBNull.Value;
            dr["campusid"] = (object)location.Campusid ?? DBNull.Value;
            dr["libraryid"] = (object)location.Libraryid ?? DBNull.Value;
            locationsDataTable.Rows.Add(dr);
        }

        public void Insert(Login login)
        {
            if (loginsDataTable == null)
            {
                loginsDataTable = new DataTable();
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loginsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loginsDataTable.NewRow();
            dr["_id"] = (object)login.Id ?? DBNull.Value;
            dr["jsonb"] = (object)login.Content ?? DBNull.Value;
            dr["creation_date"] = (object)login.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)login.CreationUserId ?? DBNull.Value;
            loginsDataTable.Rows.Add(dr);
        }

        public void Insert(MaterialType materialType)
        {
            if (materialTypesDataTable == null)
            {
                materialTypesDataTable = new DataTable();
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                materialTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = materialTypesDataTable.NewRow();
            dr["_id"] = (object)materialType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)materialType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)materialType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)materialType.CreationUserId ?? DBNull.Value;
            materialTypesDataTable.Rows.Add(dr);
        }

        public void Insert(ModeOfIssuance modeOfIssuance)
        {
            if (modeOfIssuancesDataTable == null)
            {
                modeOfIssuancesDataTable = new DataTable();
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                modeOfIssuancesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = modeOfIssuancesDataTable.NewRow();
            dr["_id"] = (object)modeOfIssuance.Id ?? DBNull.Value;
            dr["jsonb"] = (object)modeOfIssuance.Content ?? DBNull.Value;
            dr["creation_date"] = (object)modeOfIssuance.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)modeOfIssuance.CreationUserId ?? DBNull.Value;
            modeOfIssuancesDataTable.Rows.Add(dr);
        }

        public void Insert(Note note)
        {
            if (notesDataTable == null)
            {
                notesDataTable = new DataTable();
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                notesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = notesDataTable.NewRow();
            dr["id"] = (object)note.Id ?? DBNull.Value;
            dr["jsonb"] = (object)note.Content ?? DBNull.Value;
            dr["creation_date"] = (object)note.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)note.CreationUserId ?? DBNull.Value;
            notesDataTable.Rows.Add(dr);
        }

        public void Insert(Order order)
        {
            if (ordersDataTable == null)
            {
                ordersDataTable = new DataTable();
                ordersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                ordersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                ordersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                ordersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                ordersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = ordersDataTable.NewRow();
            dr["id"] = (object)order.Id ?? DBNull.Value;
            dr["jsonb"] = (object)order.Content ?? DBNull.Value;
            dr["creation_date"] = (object)order.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)order.CreationUserId ?? DBNull.Value;
            ordersDataTable.Rows.Add(dr);
        }

        public void Insert(OrderItem orderItem)
        {
            if (orderItemsDataTable == null)
            {
                orderItemsDataTable = new DataTable();
                orderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                orderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                orderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                orderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                orderItemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderItemsDataTable.NewRow();
            dr["id"] = (object)orderItem.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderItem.Content ?? DBNull.Value;
            dr["creation_date"] = (object)orderItem.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)orderItem.CreationUserId ?? DBNull.Value;
            orderItemsDataTable.Rows.Add(dr);
        }

        public void Insert(OrderItemLocation orderItemLocation)
        {
            if (orderItemLocationsDataTable == null)
            {
                orderItemLocationsDataTable = new DataTable();
                orderItemLocationsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                orderItemLocationsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                orderItemLocationsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                orderItemLocationsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                orderItemLocationsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderItemLocationsDataTable.NewRow();
            dr["id"] = (object)orderItemLocation.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderItemLocation.Content ?? DBNull.Value;
            dr["creation_date"] = (object)orderItemLocation.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)orderItemLocation.CreationUserId ?? DBNull.Value;
            orderItemLocationsDataTable.Rows.Add(dr);
        }

        public void Insert(Owner owner)
        {
            if (ownersDataTable == null)
            {
                ownersDataTable = new DataTable();
                ownersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                ownersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                ownersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                ownersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                ownersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = ownersDataTable.NewRow();
            dr["id"] = (object)owner.Id ?? DBNull.Value;
            dr["jsonb"] = (object)owner.Content ?? DBNull.Value;
            dr["creation_date"] = (object)owner.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)owner.CreationUserId ?? DBNull.Value;
            ownersDataTable.Rows.Add(dr);
        }

        public void Insert(PatronNoticePolicy patronNoticePolicy)
        {
            if (patronNoticePoliciesDataTable == null)
            {
                patronNoticePoliciesDataTable = new DataTable();
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                patronNoticePoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = patronNoticePoliciesDataTable.NewRow();
            dr["_id"] = (object)patronNoticePolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)patronNoticePolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)patronNoticePolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)patronNoticePolicy.CreationUserId ?? DBNull.Value;
            patronNoticePoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(Payment payment)
        {
            if (paymentsDataTable == null)
            {
                paymentsDataTable = new DataTable();
                paymentsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                paymentsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                paymentsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                paymentsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                paymentsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = paymentsDataTable.NewRow();
            dr["id"] = (object)payment.Id ?? DBNull.Value;
            dr["jsonb"] = (object)payment.Content ?? DBNull.Value;
            dr["creation_date"] = (object)payment.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)payment.CreationUserId ?? DBNull.Value;
            paymentsDataTable.Rows.Add(dr);
        }

        public void Insert(Permission permission)
        {
            if (permissionsDataTable == null)
            {
                permissionsDataTable = new DataTable();
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                permissionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = permissionsDataTable.NewRow();
            dr["_id"] = (object)permission.Id ?? DBNull.Value;
            dr["jsonb"] = (object)permission.Content ?? DBNull.Value;
            dr["creation_date"] = (object)permission.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)permission.CreationUserId ?? DBNull.Value;
            permissionsDataTable.Rows.Add(dr);
        }

        public void Insert(PermissionsUser permissionsUser)
        {
            if (permissionsUsersDataTable == null)
            {
                permissionsUsersDataTable = new DataTable();
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                permissionsUsersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = permissionsUsersDataTable.NewRow();
            dr["_id"] = (object)permissionsUser.Id ?? DBNull.Value;
            dr["jsonb"] = (object)permissionsUser.Content ?? DBNull.Value;
            dr["creation_date"] = (object)permissionsUser.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)permissionsUser.CreationUserId ?? DBNull.Value;
            permissionsUsersDataTable.Rows.Add(dr);
        }

        public void Insert(PhoneNumber phoneNumber)
        {
            if (phoneNumbersDataTable == null)
            {
                phoneNumbersDataTable = new DataTable();
                phoneNumbersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                phoneNumbersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                phoneNumbersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                phoneNumbersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                phoneNumbersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = phoneNumbersDataTable.NewRow();
            dr["id"] = (object)phoneNumber.Id ?? DBNull.Value;
            dr["jsonb"] = (object)phoneNumber.Content ?? DBNull.Value;
            dr["creation_date"] = (object)phoneNumber.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)phoneNumber.CreationUserId ?? DBNull.Value;
            phoneNumbersDataTable.Rows.Add(dr);
        }

        public void Insert(Physical physical)
        {
            if (physicalsDataTable == null)
            {
                physicalsDataTable = new DataTable();
                physicalsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                physicalsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                physicalsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                physicalsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                physicalsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = physicalsDataTable.NewRow();
            dr["id"] = (object)physical.Id ?? DBNull.Value;
            dr["jsonb"] = (object)physical.Content ?? DBNull.Value;
            dr["creation_date"] = (object)physical.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)physical.CreationUserId ?? DBNull.Value;
            physicalsDataTable.Rows.Add(dr);
        }

        public void Insert(Piece piece)
        {
            if (piecesDataTable == null)
            {
                piecesDataTable = new DataTable();
                piecesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                piecesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                piecesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                piecesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                piecesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = piecesDataTable.NewRow();
            dr["id"] = (object)piece.Id ?? DBNull.Value;
            dr["jsonb"] = (object)piece.Content ?? DBNull.Value;
            dr["creation_date"] = (object)piece.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)piece.CreationUserId ?? DBNull.Value;
            piecesDataTable.Rows.Add(dr);
        }

        public void Insert(Proxy proxy)
        {
            if (proxiesDataTable == null)
            {
                proxiesDataTable = new DataTable();
                proxiesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                proxiesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                proxiesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                proxiesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                proxiesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = proxiesDataTable.NewRow();
            dr["id"] = (object)proxy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)proxy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)proxy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)proxy.CreationUserId ?? DBNull.Value;
            proxiesDataTable.Rows.Add(dr);
        }

        public void Insert(Refund refund)
        {
            if (refundsDataTable == null)
            {
                refundsDataTable = new DataTable();
                refundsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                refundsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                refundsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                refundsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                refundsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = refundsDataTable.NewRow();
            dr["id"] = (object)refund.Id ?? DBNull.Value;
            dr["jsonb"] = (object)refund.Content ?? DBNull.Value;
            dr["creation_date"] = (object)refund.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)refund.CreationUserId ?? DBNull.Value;
            refundsDataTable.Rows.Add(dr);
        }

        public void Insert(ReportingCode reportingCode)
        {
            if (reportingCodesDataTable == null)
            {
                reportingCodesDataTable = new DataTable();
                reportingCodesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                reportingCodesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                reportingCodesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                reportingCodesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                reportingCodesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = reportingCodesDataTable.NewRow();
            dr["id"] = (object)reportingCode.Id ?? DBNull.Value;
            dr["jsonb"] = (object)reportingCode.Content ?? DBNull.Value;
            dr["creation_date"] = (object)reportingCode.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)reportingCode.CreationUserId ?? DBNull.Value;
            reportingCodesDataTable.Rows.Add(dr);
        }

        public void Insert(Request request)
        {
            if (requestsDataTable == null)
            {
                requestsDataTable = new DataTable();
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "cancellationreasonid", DataType = typeof(Guid) });
                requestsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = requestsDataTable.NewRow();
            dr["_id"] = (object)request.Id ?? DBNull.Value;
            dr["jsonb"] = (object)request.Content ?? DBNull.Value;
            dr["creation_date"] = (object)request.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)request.CreationUserId ?? DBNull.Value;
            dr["cancellationreasonid"] = (object)request.Cancellationreasonid ?? DBNull.Value;
            requestsDataTable.Rows.Add(dr);
        }

        public void Insert(RequestPolicy requestPolicy)
        {
            if (requestPoliciesDataTable == null)
            {
                requestPoliciesDataTable = new DataTable();
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                requestPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = requestPoliciesDataTable.NewRow();
            dr["_id"] = (object)requestPolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)requestPolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)requestPolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)requestPolicy.CreationUserId ?? DBNull.Value;
            requestPoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(ServicePoint servicePoint)
        {
            if (servicePointsDataTable == null)
            {
                servicePointsDataTable = new DataTable();
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                servicePointsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = servicePointsDataTable.NewRow();
            dr["_id"] = (object)servicePoint.Id ?? DBNull.Value;
            dr["jsonb"] = (object)servicePoint.Content ?? DBNull.Value;
            dr["creation_date"] = (object)servicePoint.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)servicePoint.CreationUserId ?? DBNull.Value;
            servicePointsDataTable.Rows.Add(dr);
        }

        public void Insert(ServicePointUser servicePointUser)
        {
            if (servicePointUsersDataTable == null)
            {
                servicePointUsersDataTable = new DataTable();
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "defaultservicepointid", DataType = typeof(Guid) });
                servicePointUsersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = servicePointUsersDataTable.NewRow();
            dr["_id"] = (object)servicePointUser.Id ?? DBNull.Value;
            dr["jsonb"] = (object)servicePointUser.Content ?? DBNull.Value;
            dr["creation_date"] = (object)servicePointUser.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)servicePointUser.CreationUserId ?? DBNull.Value;
            dr["defaultservicepointid"] = (object)servicePointUser.Defaultservicepointid ?? DBNull.Value;
            servicePointUsersDataTable.Rows.Add(dr);
        }

        public void Insert(Source source)
        {
            if (sourcesDataTable == null)
            {
                sourcesDataTable = new DataTable();
                sourcesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                sourcesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                sourcesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                sourcesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                sourcesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = sourcesDataTable.NewRow();
            dr["id"] = (object)source.Id ?? DBNull.Value;
            dr["jsonb"] = (object)source.Content ?? DBNull.Value;
            dr["creation_date"] = (object)source.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)source.CreationUserId ?? DBNull.Value;
            sourcesDataTable.Rows.Add(dr);
        }

        public void Insert(StaffSlip staffSlip)
        {
            if (staffSlipsDataTable == null)
            {
                staffSlipsDataTable = new DataTable();
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                staffSlipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = staffSlipsDataTable.NewRow();
            dr["_id"] = (object)staffSlip.Id ?? DBNull.Value;
            dr["jsonb"] = (object)staffSlip.Content ?? DBNull.Value;
            dr["creation_date"] = (object)staffSlip.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)staffSlip.CreationUserId ?? DBNull.Value;
            staffSlipsDataTable.Rows.Add(dr);
        }

        public void Insert(StatisticalCode statisticalCode)
        {
            if (statisticalCodesDataTable == null)
            {
                statisticalCodesDataTable = new DataTable();
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "statisticalcodetypeid", DataType = typeof(Guid) });
                statisticalCodesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = statisticalCodesDataTable.NewRow();
            dr["_id"] = (object)statisticalCode.Id ?? DBNull.Value;
            dr["jsonb"] = (object)statisticalCode.Content ?? DBNull.Value;
            dr["creation_date"] = (object)statisticalCode.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)statisticalCode.CreationUserId ?? DBNull.Value;
            dr["statisticalcodetypeid"] = (object)statisticalCode.Statisticalcodetypeid ?? DBNull.Value;
            statisticalCodesDataTable.Rows.Add(dr);
        }

        public void Insert(StatisticalCodeType statisticalCodeType)
        {
            if (statisticalCodeTypesDataTable == null)
            {
                statisticalCodeTypesDataTable = new DataTable();
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                statisticalCodeTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = statisticalCodeTypesDataTable.NewRow();
            dr["_id"] = (object)statisticalCodeType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)statisticalCodeType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)statisticalCodeType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)statisticalCodeType.CreationUserId ?? DBNull.Value;
            statisticalCodeTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Tag tag)
        {
            if (tagsDataTable == null)
            {
                tagsDataTable = new DataTable();
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "_id", DataType = typeof(Guid) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                tagsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = tagsDataTable.NewRow();
            dr["_id"] = (object)tag.Id ?? DBNull.Value;
            dr["jsonb"] = (object)tag.Content ?? DBNull.Value;
            dr["creation_date"] = (object)tag.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)tag.CreationUserId ?? DBNull.Value;
            tagsDataTable.Rows.Add(dr);
        }

        public void Insert(Transaction transaction)
        {
            if (transactionsDataTable == null)
            {
                transactionsDataTable = new DataTable();
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "budget_id", DataType = typeof(Guid) });
                transactionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = transactionsDataTable.NewRow();
            dr["id"] = (object)transaction.Id ?? DBNull.Value;
            dr["jsonb"] = (object)transaction.Content ?? DBNull.Value;
            dr["creation_date"] = (object)transaction.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)transaction.CreationUserId ?? DBNull.Value;
            dr["budget_id"] = (object)transaction.BudgetId ?? DBNull.Value;
            transactionsDataTable.Rows.Add(dr);
        }

        public void Insert(Transfer transfer)
        {
            if (transfersDataTable == null)
            {
                transfersDataTable = new DataTable();
                transfersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                transfersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                transfersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                transfersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                transfersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = transfersDataTable.NewRow();
            dr["id"] = (object)transfer.Id ?? DBNull.Value;
            dr["jsonb"] = (object)transfer.Content ?? DBNull.Value;
            dr["creation_date"] = (object)transfer.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)transfer.CreationUserId ?? DBNull.Value;
            transfersDataTable.Rows.Add(dr);
        }

        public void Insert(TransferCriteria transferCriteria)
        {
            if (transferCriteriasDataTable == null)
            {
                transferCriteriasDataTable = new DataTable();
                transferCriteriasDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                transferCriteriasDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                transferCriteriasDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                transferCriteriasDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                transferCriteriasDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = transferCriteriasDataTable.NewRow();
            dr["id"] = (object)transferCriteria.Id ?? DBNull.Value;
            dr["jsonb"] = (object)transferCriteria.Content ?? DBNull.Value;
            dr["creation_date"] = (object)transferCriteria.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)transferCriteria.CreationUserId ?? DBNull.Value;
            transferCriteriasDataTable.Rows.Add(dr);
        }

        public void Insert(Url url)
        {
            if (urlsDataTable == null)
            {
                urlsDataTable = new DataTable();
                urlsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                urlsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                urlsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                urlsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                urlsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = urlsDataTable.NewRow();
            dr["id"] = (object)url.Id ?? DBNull.Value;
            dr["jsonb"] = (object)url.Content ?? DBNull.Value;
            dr["creation_date"] = (object)url.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)url.CreationUserId ?? DBNull.Value;
            urlsDataTable.Rows.Add(dr);
        }

        public void Insert(User user)
        {
            if (usersDataTable == null)
            {
                usersDataTable = new DataTable();
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                usersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = usersDataTable.NewRow();
            dr["id"] = (object)user.Id ?? DBNull.Value;
            dr["jsonb"] = (object)user.Content ?? DBNull.Value;
            dr["creation_date"] = (object)user.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)user.CreationUserId ?? DBNull.Value;
            usersDataTable.Rows.Add(dr);
        }

        public void Insert(Vendor vendor)
        {
            if (vendorsDataTable == null)
            {
                vendorsDataTable = new DataTable();
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                vendorsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = vendorsDataTable.NewRow();
            dr["id"] = (object)vendor.Id ?? DBNull.Value;
            dr["jsonb"] = (object)vendor.Content ?? DBNull.Value;
            dr["creation_date"] = (object)vendor.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)vendor.CreationUserId ?? DBNull.Value;
            vendorsDataTable.Rows.Add(dr);
        }

        public void Insert(VendorCategory vendorCategory)
        {
            if (vendorCategoriesDataTable == null)
            {
                vendorCategoriesDataTable = new DataTable();
                vendorCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                vendorCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                vendorCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                vendorCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                vendorCategoriesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = vendorCategoriesDataTable.NewRow();
            dr["id"] = (object)vendorCategory.Id ?? DBNull.Value;
            dr["jsonb"] = (object)vendorCategory.Content ?? DBNull.Value;
            dr["creation_date"] = (object)vendorCategory.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)vendorCategory.CreationUserId ?? DBNull.Value;
            vendorCategoriesDataTable.Rows.Add(dr);
        }

        public void Insert(VendorDetail vendorDetail)
        {
            if (vendorDetailsDataTable == null)
            {
                vendorDetailsDataTable = new DataTable();
                vendorDetailsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                vendorDetailsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                vendorDetailsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                vendorDetailsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                vendorDetailsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = vendorDetailsDataTable.NewRow();
            dr["id"] = (object)vendorDetail.Id ?? DBNull.Value;
            dr["jsonb"] = (object)vendorDetail.Content ?? DBNull.Value;
            dr["creation_date"] = (object)vendorDetail.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)vendorDetail.CreationUserId ?? DBNull.Value;
            vendorDetailsDataTable.Rows.Add(dr);
        }

        public void Insert(VendorType vendorType)
        {
            if (vendorTypesDataTable == null)
            {
                vendorTypesDataTable = new DataTable();
                vendorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                vendorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                vendorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                vendorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                vendorTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = vendorTypesDataTable.NewRow();
            dr["id"] = (object)vendorType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)vendorType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)vendorType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)vendorType.CreationUserId ?? DBNull.Value;
            vendorTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Waive waive)
        {
            if (waivesDataTable == null)
            {
                waivesDataTable = new DataTable();
                waivesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                waivesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                waivesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                waivesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                waivesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = waivesDataTable.NewRow();
            dr["id"] = (object)waive.Id ?? DBNull.Value;
            dr["jsonb"] = (object)waive.Content ?? DBNull.Value;
            dr["creation_date"] = (object)waive.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)waive.CreationUserId ?? DBNull.Value;
            waivesDataTable.Rows.Add(dr);
        }

        public void Commit()
        {
            if (accountsDataTable != null && accountsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(accountsDataTable);
                accountsDataTable.Clear();
            }
            if (account2sDataTable != null && account2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}account";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(account2sDataTable);
                account2sDataTable.Clear();
            }
            if (addressesDataTable != null && addressesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}address";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(addressesDataTable);
                addressesDataTable.Clear();
            }
            if (addressTypesDataTable != null && addressTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}addresstype";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(addressTypesDataTable);
                addressTypesDataTable.Clear();
            }
            if (adjustmentsDataTable != null && adjustmentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}adjustment";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(adjustmentsDataTable);
                adjustmentsDataTable.Clear();
            }
            if (agreementsDataTable != null && agreementsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}agreement";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(agreementsDataTable);
                agreementsDataTable.Clear();
            }
            if (alertsDataTable != null && alertsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(alertsDataTable);
                alertsDataTable.Clear();
            }
            if (aliasesDataTable != null && aliasesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}alias";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(aliasesDataTable);
                aliasesDataTable.Clear();
            }
            if (alternativeTitleTypesDataTable != null && alternativeTitleTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(alternativeTitleTypesDataTable);
                alternativeTitleTypesDataTable.Clear();
            }
            if (auditLoansDataTable != null && auditLoansDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("orig_id", "orig_id");
                sqlBulkCopy.ColumnMappings.Add("operation", "operation");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.WriteToServer(auditLoansDataTable);
                auditLoansDataTable.Clear();
            }
            if (authAttemptsDataTable != null && authAttemptsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(authAttemptsDataTable);
                authAttemptsDataTable.Clear();
            }
            if (authCredentialsHistoriesDataTable != null && authCredentialsHistoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(authCredentialsHistoriesDataTable);
                authCredentialsHistoriesDataTable.Clear();
            }
            if (authPasswordActionsDataTable != null && authPasswordActionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(authPasswordActionsDataTable);
                authPasswordActionsDataTable.Clear();
            }
            if (blocksDataTable != null && blocksDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(blocksDataTable);
                blocksDataTable.Clear();
            }
            if (budgetsDataTable != null && budgetsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("fund_id", "fund_id");
                sqlBulkCopy.ColumnMappings.Add("fiscal_year_id", "fiscal_year_id");
                sqlBulkCopy.WriteToServer(budgetsDataTable);
                budgetsDataTable.Clear();
            }
            if (callNumberTypesDataTable != null && callNumberTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(callNumberTypesDataTable);
                callNumberTypesDataTable.Clear();
            }
            if (campusesDataTable != null && campusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("institutionid", "institutionid");
                sqlBulkCopy.WriteToServer(campusesDataTable);
                campusesDataTable.Clear();
            }
            if (cancellationReasonsDataTable != null && cancellationReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(cancellationReasonsDataTable);
                cancellationReasonsDataTable.Clear();
            }
            if (categoriesDataTable != null && categoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}category";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(categoriesDataTable);
                categoriesDataTable.Clear();
            }
            if (circulationRulesDataTable != null && circulationRulesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("lock", "lock");
                sqlBulkCopy.WriteToServer(circulationRulesDataTable);
                circulationRulesDataTable.Clear();
            }
            if (claimsDataTable != null && claimsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}claim";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(claimsDataTable);
                claimsDataTable.Clear();
            }
            if (classificationTypesDataTable != null && classificationTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(classificationTypesDataTable);
                classificationTypesDataTable.Clear();
            }
            if (commentsDataTable != null && commentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}comments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(commentsDataTable);
                commentsDataTable.Clear();
            }
            if (contactsDataTable != null && contactsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}contact";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(contactsDataTable);
                contactsDataTable.Clear();
            }
            if (contactCategoriesDataTable != null && contactCategoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}contact_category";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(contactCategoriesDataTable);
                contactCategoriesDataTable.Clear();
            }
            if (contributorNameTypesDataTable != null && contributorNameTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(contributorNameTypesDataTable);
                contributorNameTypesDataTable.Clear();
            }
            if (contributorTypesDataTable != null && contributorTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(contributorTypesDataTable);
                contributorTypesDataTable.Clear();
            }
            if (costsDataTable != null && costsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}cost";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(costsDataTable);
                costsDataTable.Clear();
            }
            if (detailsDataTable != null && detailsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}details";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(detailsDataTable);
                detailsDataTable.Clear();
            }
            if (electronicAccessRelationshipsDataTable != null && electronicAccessRelationshipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(electronicAccessRelationshipsDataTable);
                electronicAccessRelationshipsDataTable.Clear();
            }
            if (emailsDataTable != null && emailsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}email";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(emailsDataTable);
                emailsDataTable.Clear();
            }
            if (eresourcesDataTable != null && eresourcesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}eresource";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(eresourcesDataTable);
                eresourcesDataTable.Clear();
            }
            if (eventLogsDataTable != null && eventLogsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}event_logs";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(eventLogsDataTable);
                eventLogsDataTable.Clear();
            }
            if (feesDataTable != null && feesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ownerid", "ownerid");
                sqlBulkCopy.WriteToServer(feesDataTable);
                feesDataTable.Clear();
            }
            if (feeActionsDataTable != null && feeActionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(feeActionsDataTable);
                feeActionsDataTable.Clear();
            }
            if (fiscalYearsDataTable != null && fiscalYearsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(fiscalYearsDataTable);
                fiscalYearsDataTable.Clear();
            }
            if (fixedDueDateSchedulesDataTable != null && fixedDueDateSchedulesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(fixedDueDateSchedulesDataTable);
                fixedDueDateSchedulesDataTable.Clear();
            }
            if (fundsDataTable != null && fundsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ledger_id", "ledger_id");
                sqlBulkCopy.WriteToServer(fundsDataTable);
                fundsDataTable.Clear();
            }
            if (fundDistributionsDataTable != null && fundDistributionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_distribution";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("budget_id", "budget_id");
                sqlBulkCopy.WriteToServer(fundDistributionsDataTable);
                fundDistributionsDataTable.Clear();
            }
            if (fundDistribution2sDataTable != null && fundDistribution2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}fund_distribution";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(fundDistribution2sDataTable);
                fundDistribution2sDataTable.Clear();
            }
            if (groupsDataTable != null && groupsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}groups";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(groupsDataTable);
                groupsDataTable.Clear();
            }
            if (holdingsDataTable != null && holdingsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("instanceid", "instanceid");
                sqlBulkCopy.ColumnMappings.Add("permanentlocationid", "permanentlocationid");
                sqlBulkCopy.ColumnMappings.Add("temporarylocationid", "temporarylocationid");
                sqlBulkCopy.ColumnMappings.Add("holdingstypeid", "holdingstypeid");
                sqlBulkCopy.ColumnMappings.Add("callnumbertypeid", "callnumbertypeid");
                sqlBulkCopy.ColumnMappings.Add("illpolicyid", "illpolicyid");
                sqlBulkCopy.WriteToServer(holdingsDataTable);
                holdingsDataTable.Clear();
            }
            if (holdingNoteTypesDataTable != null && holdingNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(holdingNoteTypesDataTable);
                holdingNoteTypesDataTable.Clear();
            }
            if (holdingTypesDataTable != null && holdingTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(holdingTypesDataTable);
                holdingTypesDataTable.Clear();
            }
            if (idTypesDataTable != null && idTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(idTypesDataTable);
                idTypesDataTable.Clear();
            }
            if (illPoliciesDataTable != null && illPoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(illPoliciesDataTable);
                illPoliciesDataTable.Clear();
            }
            if (instancesDataTable != null && instancesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("instancestatusid", "instancestatusid");
                sqlBulkCopy.ColumnMappings.Add("modeofissuanceid", "modeofissuanceid");
                sqlBulkCopy.WriteToServer(instancesDataTable);
                instancesDataTable.Clear();
            }
            if (instanceFormatsDataTable != null && instanceFormatsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceFormatsDataTable);
                instanceFormatsDataTable.Clear();
            }
            if (instanceRelationshipsDataTable != null && instanceRelationshipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("superinstanceid", "superinstanceid");
                sqlBulkCopy.ColumnMappings.Add("subinstanceid", "subinstanceid");
                sqlBulkCopy.ColumnMappings.Add("instancerelationshiptypeid", "instancerelationshiptypeid");
                sqlBulkCopy.WriteToServer(instanceRelationshipsDataTable);
                instanceRelationshipsDataTable.Clear();
            }
            if (instanceRelationshipTypesDataTable != null && instanceRelationshipTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(instanceRelationshipTypesDataTable);
                instanceRelationshipTypesDataTable.Clear();
            }
            if (instanceSourceMarcsDataTable != null && instanceSourceMarcsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(instanceSourceMarcsDataTable);
                instanceSourceMarcsDataTable.Clear();
            }
            if (instanceStatusesDataTable != null && instanceStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(instanceStatusesDataTable);
                instanceStatusesDataTable.Clear();
            }
            if (instanceTypesDataTable != null && instanceTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceTypesDataTable);
                instanceTypesDataTable.Clear();
            }
            if (institutionsDataTable != null && institutionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(institutionsDataTable);
                institutionsDataTable.Clear();
            }
            if (interfacesDataTable != null && interfacesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}interface";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(interfacesDataTable);
                interfacesDataTable.Clear();
            }
            if (itemsDataTable != null && itemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("holdingsrecordid", "holdingsrecordid");
                sqlBulkCopy.ColumnMappings.Add("permanentloantypeid", "permanentloantypeid");
                sqlBulkCopy.ColumnMappings.Add("temporaryloantypeid", "temporaryloantypeid");
                sqlBulkCopy.ColumnMappings.Add("materialtypeid", "materialtypeid");
                sqlBulkCopy.ColumnMappings.Add("permanentlocationid", "permanentlocationid");
                sqlBulkCopy.ColumnMappings.Add("temporarylocationid", "temporarylocationid");
                sqlBulkCopy.WriteToServer(itemsDataTable);
                itemsDataTable.Clear();
            }
            if (itemNoteTypesDataTable != null && itemNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(itemNoteTypesDataTable);
                itemNoteTypesDataTable.Clear();
            }
            if (ledgersDataTable != null && ledgersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(ledgersDataTable);
                ledgersDataTable.Clear();
            }
            if (librariesDataTable != null && librariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("campusid", "campusid");
                sqlBulkCopy.WriteToServer(librariesDataTable);
                librariesDataTable.Clear();
            }
            if (loansDataTable != null && loansDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(loansDataTable);
                loansDataTable.Clear();
            }
            if (loanPoliciesDataTable != null && loanPoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("loanspolicy_fixedduedatescheduleid", "loanspolicy_fixedduedatescheduleid");
                sqlBulkCopy.ColumnMappings.Add("renewalspolicy_alternatefixedduedatescheduleid", "renewalspolicy_alternatefixedduedatescheduleid");
                sqlBulkCopy.WriteToServer(loanPoliciesDataTable);
                loanPoliciesDataTable.Clear();
            }
            if (loanTypesDataTable != null && loanTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(loanTypesDataTable);
                loanTypesDataTable.Clear();
            }
            if (locationsDataTable != null && locationsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("institutionid", "institutionid");
                sqlBulkCopy.ColumnMappings.Add("campusid", "campusid");
                sqlBulkCopy.ColumnMappings.Add("libraryid", "libraryid");
                sqlBulkCopy.WriteToServer(locationsDataTable);
                locationsDataTable.Clear();
            }
            if (loginsDataTable != null && loginsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(loginsDataTable);
                loginsDataTable.Clear();
            }
            if (materialTypesDataTable != null && materialTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(materialTypesDataTable);
                materialTypesDataTable.Clear();
            }
            if (modeOfIssuancesDataTable != null && modeOfIssuancesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(modeOfIssuancesDataTable);
                modeOfIssuancesDataTable.Clear();
            }
            if (notesDataTable != null && notesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_notes{(IsMySql ? "_" : ".")}note_data";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(notesDataTable);
                notesDataTable.Clear();
            }
            if (ordersDataTable != null && ordersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(ordersDataTable);
                ordersDataTable.Clear();
            }
            if (orderItemsDataTable != null && orderItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(orderItemsDataTable);
                orderItemsDataTable.Clear();
            }
            if (orderItemLocationsDataTable != null && orderItemLocationsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}location";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(orderItemLocationsDataTable);
                orderItemLocationsDataTable.Clear();
            }
            if (ownersDataTable != null && ownersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}owners";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(ownersDataTable);
                ownersDataTable.Clear();
            }
            if (patronNoticePoliciesDataTable != null && patronNoticePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronNoticePoliciesDataTable);
                patronNoticePoliciesDataTable.Clear();
            }
            if (paymentsDataTable != null && paymentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}payments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(paymentsDataTable);
                paymentsDataTable.Clear();
            }
            if (permissionsDataTable != null && permissionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_permissions{(IsMySql ? "_" : ".")}permissions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(permissionsDataTable);
                permissionsDataTable.Clear();
            }
            if (permissionsUsersDataTable != null && permissionsUsersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(permissionsUsersDataTable);
                permissionsUsersDataTable.Clear();
            }
            if (phoneNumbersDataTable != null && phoneNumbersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}phone_number";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(phoneNumbersDataTable);
                phoneNumbersDataTable.Clear();
            }
            if (physicalsDataTable != null && physicalsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}physical";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(physicalsDataTable);
                physicalsDataTable.Clear();
            }
            if (piecesDataTable != null && piecesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(piecesDataTable);
                piecesDataTable.Clear();
            }
            if (proxiesDataTable != null && proxiesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}proxyfor";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(proxiesDataTable);
                proxiesDataTable.Clear();
            }
            if (refundsDataTable != null && refundsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(refundsDataTable);
                refundsDataTable.Clear();
            }
            if (reportingCodesDataTable != null && reportingCodesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(reportingCodesDataTable);
                reportingCodesDataTable.Clear();
            }
            if (requestsDataTable != null && requestsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("cancellationreasonid", "cancellationreasonid");
                sqlBulkCopy.WriteToServer(requestsDataTable);
                requestsDataTable.Clear();
            }
            if (requestPoliciesDataTable != null && requestPoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(requestPoliciesDataTable);
                requestPoliciesDataTable.Clear();
            }
            if (servicePointsDataTable != null && servicePointsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(servicePointsDataTable);
                servicePointsDataTable.Clear();
            }
            if (servicePointUsersDataTable != null && servicePointUsersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("defaultservicepointid", "defaultservicepointid");
                sqlBulkCopy.WriteToServer(servicePointUsersDataTable);
                servicePointUsersDataTable.Clear();
            }
            if (sourcesDataTable != null && sourcesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}source";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(sourcesDataTable);
                sourcesDataTable.Clear();
            }
            if (staffSlipsDataTable != null && staffSlipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(staffSlipsDataTable);
                staffSlipsDataTable.Clear();
            }
            if (statisticalCodesDataTable != null && statisticalCodesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("statisticalcodetypeid", "statisticalcodetypeid");
                sqlBulkCopy.WriteToServer(statisticalCodesDataTable);
                statisticalCodesDataTable.Clear();
            }
            if (statisticalCodeTypesDataTable != null && statisticalCodeTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(statisticalCodeTypesDataTable);
                statisticalCodeTypesDataTable.Clear();
            }
            if (tagsDataTable != null && tagsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_tags{(IsMySql ? "_" : ".")}tags";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("_id", "_id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(tagsDataTable);
                tagsDataTable.Clear();
            }
            if (transactionsDataTable != null && transactionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("budget_id", "budget_id");
                sqlBulkCopy.WriteToServer(transactionsDataTable);
                transactionsDataTable.Clear();
            }
            if (transfersDataTable != null && transfersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(transfersDataTable);
                transfersDataTable.Clear();
            }
            if (transferCriteriasDataTable != null && transferCriteriasDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(transferCriteriasDataTable);
                transferCriteriasDataTable.Clear();
            }
            if (urlsDataTable != null && urlsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}url";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(urlsDataTable);
                urlsDataTable.Clear();
            }
            if (usersDataTable != null && usersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}users";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(usersDataTable);
                usersDataTable.Clear();
            }
            if (vendorsDataTable != null && vendorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}vendor";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(vendorsDataTable);
                vendorsDataTable.Clear();
            }
            if (vendorCategoriesDataTable != null && vendorCategoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}vendor_category";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(vendorCategoriesDataTable);
                vendorCategoriesDataTable.Clear();
            }
            if (vendorDetailsDataTable != null && vendorDetailsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}vendor_detail";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(vendorDetailsDataTable);
                vendorDetailsDataTable.Clear();
            }
            if (vendorTypesDataTable != null && vendorTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_vendors{(IsMySql ? "_" : ".")}vendor_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(vendorTypesDataTable);
                vendorTypesDataTable.Clear();
            }
            if (waivesDataTable != null && waivesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}waives";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(waivesDataTable);
                waivesDataTable.Clear();
            }
        }

        public void Dispose()
        {
            if (sqlBulkCopy != null) sqlBulkCopy.Close();
            if (accountsDataTable != null) accountsDataTable.Dispose();
            if (account2sDataTable != null) account2sDataTable.Dispose();
            if (addressesDataTable != null) addressesDataTable.Dispose();
            if (addressTypesDataTable != null) addressTypesDataTable.Dispose();
            if (adjustmentsDataTable != null) adjustmentsDataTable.Dispose();
            if (agreementsDataTable != null) agreementsDataTable.Dispose();
            if (alertsDataTable != null) alertsDataTable.Dispose();
            if (aliasesDataTable != null) aliasesDataTable.Dispose();
            if (alternativeTitleTypesDataTable != null) alternativeTitleTypesDataTable.Dispose();
            if (auditLoansDataTable != null) auditLoansDataTable.Dispose();
            if (authAttemptsDataTable != null) authAttemptsDataTable.Dispose();
            if (authCredentialsHistoriesDataTable != null) authCredentialsHistoriesDataTable.Dispose();
            if (authPasswordActionsDataTable != null) authPasswordActionsDataTable.Dispose();
            if (blocksDataTable != null) blocksDataTable.Dispose();
            if (budgetsDataTable != null) budgetsDataTable.Dispose();
            if (callNumberTypesDataTable != null) callNumberTypesDataTable.Dispose();
            if (campusesDataTable != null) campusesDataTable.Dispose();
            if (cancellationReasonsDataTable != null) cancellationReasonsDataTable.Dispose();
            if (categoriesDataTable != null) categoriesDataTable.Dispose();
            if (circulationRulesDataTable != null) circulationRulesDataTable.Dispose();
            if (claimsDataTable != null) claimsDataTable.Dispose();
            if (classificationTypesDataTable != null) classificationTypesDataTable.Dispose();
            if (commentsDataTable != null) commentsDataTable.Dispose();
            if (contactsDataTable != null) contactsDataTable.Dispose();
            if (contactCategoriesDataTable != null) contactCategoriesDataTable.Dispose();
            if (contributorNameTypesDataTable != null) contributorNameTypesDataTable.Dispose();
            if (contributorTypesDataTable != null) contributorTypesDataTable.Dispose();
            if (costsDataTable != null) costsDataTable.Dispose();
            if (detailsDataTable != null) detailsDataTable.Dispose();
            if (electronicAccessRelationshipsDataTable != null) electronicAccessRelationshipsDataTable.Dispose();
            if (emailsDataTable != null) emailsDataTable.Dispose();
            if (eresourcesDataTable != null) eresourcesDataTable.Dispose();
            if (eventLogsDataTable != null) eventLogsDataTable.Dispose();
            if (feesDataTable != null) feesDataTable.Dispose();
            if (feeActionsDataTable != null) feeActionsDataTable.Dispose();
            if (fiscalYearsDataTable != null) fiscalYearsDataTable.Dispose();
            if (fixedDueDateSchedulesDataTable != null) fixedDueDateSchedulesDataTable.Dispose();
            if (fundsDataTable != null) fundsDataTable.Dispose();
            if (fundDistributionsDataTable != null) fundDistributionsDataTable.Dispose();
            if (fundDistribution2sDataTable != null) fundDistribution2sDataTable.Dispose();
            if (groupsDataTable != null) groupsDataTable.Dispose();
            if (holdingsDataTable != null) holdingsDataTable.Dispose();
            if (holdingNoteTypesDataTable != null) holdingNoteTypesDataTable.Dispose();
            if (holdingTypesDataTable != null) holdingTypesDataTable.Dispose();
            if (idTypesDataTable != null) idTypesDataTable.Dispose();
            if (illPoliciesDataTable != null) illPoliciesDataTable.Dispose();
            if (instancesDataTable != null) instancesDataTable.Dispose();
            if (instanceFormatsDataTable != null) instanceFormatsDataTable.Dispose();
            if (instanceRelationshipsDataTable != null) instanceRelationshipsDataTable.Dispose();
            if (instanceRelationshipTypesDataTable != null) instanceRelationshipTypesDataTable.Dispose();
            if (instanceSourceMarcsDataTable != null) instanceSourceMarcsDataTable.Dispose();
            if (instanceStatusesDataTable != null) instanceStatusesDataTable.Dispose();
            if (instanceTypesDataTable != null) instanceTypesDataTable.Dispose();
            if (institutionsDataTable != null) institutionsDataTable.Dispose();
            if (interfacesDataTable != null) interfacesDataTable.Dispose();
            if (itemsDataTable != null) itemsDataTable.Dispose();
            if (itemNoteTypesDataTable != null) itemNoteTypesDataTable.Dispose();
            if (ledgersDataTable != null) ledgersDataTable.Dispose();
            if (librariesDataTable != null) librariesDataTable.Dispose();
            if (loansDataTable != null) loansDataTable.Dispose();
            if (loanPoliciesDataTable != null) loanPoliciesDataTable.Dispose();
            if (loanTypesDataTable != null) loanTypesDataTable.Dispose();
            if (locationsDataTable != null) locationsDataTable.Dispose();
            if (loginsDataTable != null) loginsDataTable.Dispose();
            if (materialTypesDataTable != null) materialTypesDataTable.Dispose();
            if (modeOfIssuancesDataTable != null) modeOfIssuancesDataTable.Dispose();
            if (notesDataTable != null) notesDataTable.Dispose();
            if (ordersDataTable != null) ordersDataTable.Dispose();
            if (orderItemsDataTable != null) orderItemsDataTable.Dispose();
            if (orderItemLocationsDataTable != null) orderItemLocationsDataTable.Dispose();
            if (ownersDataTable != null) ownersDataTable.Dispose();
            if (patronNoticePoliciesDataTable != null) patronNoticePoliciesDataTable.Dispose();
            if (paymentsDataTable != null) paymentsDataTable.Dispose();
            if (permissionsDataTable != null) permissionsDataTable.Dispose();
            if (permissionsUsersDataTable != null) permissionsUsersDataTable.Dispose();
            if (phoneNumbersDataTable != null) phoneNumbersDataTable.Dispose();
            if (physicalsDataTable != null) physicalsDataTable.Dispose();
            if (piecesDataTable != null) piecesDataTable.Dispose();
            if (proxiesDataTable != null) proxiesDataTable.Dispose();
            if (refundsDataTable != null) refundsDataTable.Dispose();
            if (reportingCodesDataTable != null) reportingCodesDataTable.Dispose();
            if (requestsDataTable != null) requestsDataTable.Dispose();
            if (requestPoliciesDataTable != null) requestPoliciesDataTable.Dispose();
            if (servicePointsDataTable != null) servicePointsDataTable.Dispose();
            if (servicePointUsersDataTable != null) servicePointUsersDataTable.Dispose();
            if (sourcesDataTable != null) sourcesDataTable.Dispose();
            if (staffSlipsDataTable != null) staffSlipsDataTable.Dispose();
            if (statisticalCodesDataTable != null) statisticalCodesDataTable.Dispose();
            if (statisticalCodeTypesDataTable != null) statisticalCodeTypesDataTable.Dispose();
            if (tagsDataTable != null) tagsDataTable.Dispose();
            if (transactionsDataTable != null) transactionsDataTable.Dispose();
            if (transfersDataTable != null) transfersDataTable.Dispose();
            if (transferCriteriasDataTable != null) transferCriteriasDataTable.Dispose();
            if (urlsDataTable != null) urlsDataTable.Dispose();
            if (usersDataTable != null) usersDataTable.Dispose();
            if (vendorsDataTable != null) vendorsDataTable.Dispose();
            if (vendorCategoriesDataTable != null) vendorCategoriesDataTable.Dispose();
            if (vendorDetailsDataTable != null) vendorDetailsDataTable.Dispose();
            if (vendorTypesDataTable != null) vendorTypesDataTable.Dispose();
            if (waivesDataTable != null) waivesDataTable.Dispose();
        }

        public static string Trim(string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Length == 0) value = null;
            }
            return value;
        }
    }

    public class MySqlBulkCopy : IDisposable
    {
        private string connectionString;
        private DbConnection dbConnection;
        private DbTransaction dbTransaction;
        private string providerName;
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";

        public MySqlBulkCopy(string name, bool checkConstraints)
        {
            providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            if (!checkConstraints) DisableConstraints();
        }

        public Dictionary<string, string> ColumnMappings { get; } = new Dictionary<string, string>();
        public string DestinationTableName { get; set; }

        private DbConnection Connection
        {
            get
            {
                if (dbConnection == null)
                {
                    dbConnection = DbProviderFactories.GetFactory(IsMySql ? "MySql.Data.MySqlClient2" : providerName).CreateConnection();
                    dbConnection.ConnectionString = connectionString;
                    dbConnection.Open();
                }
                return dbConnection;
            }
        }

        private DbTransaction Transaction
        {
            get
            {
                if (dbTransaction == null) dbTransaction = Connection.BeginTransaction();
                return dbTransaction;
            }
        }

        public void WriteToServer(DataTable table) => ExecuteNonQuery($"{(false && IsMySql ? $"LOCK TABLES {DestinationTableName} WRITE; ALTER TABLE {DestinationTableName} DISABLE KEYS; " : "")}{(IsSqlServer ? $"SET IDENTITY_INSERT {DestinationTableName} ON; " : "")}INSERT INTO {DestinationTableName} ({string.Join(", ", table.Columns.Cast<DataColumn>().Select(dc2 => dc2.ColumnName))}) VALUES {string.Join(",", table.Rows.Cast<DataRow>().Select(dr => $"({string.Join(",", table.Columns.Cast<DataColumn>().Select(dc2 => dr[dc2] == DBNull.Value ? "NULL" : dc2.DataType == typeof(string) ? $"'{SqlEncode((string)dr[dc2])}'" : dc2.DataType == typeof(Guid) ? $"'{dr[dc2]}'" : dc2.DataType == typeof(DateTime) ? $"'{(DateTime)dr[dc2]:yyyy-MM-dd HH:mm:ss.FFFFFFF}'" : dc2.DataType == typeof(bool) ? IsPostgreSql ? (bool)dr[dc2] ? "true" : "false" : (bool)dr[dc2] ? "1" : "0" : dc2.DataType == typeof(byte[]) ? IsMySql ? $"X'{string.Join("", ((byte[])dr[dc2]).Select(b => b.ToString("X2")))}'" : IsSqlServer ? $"0x{string.Join("", ((byte[])dr[dc2]).Select(b => b.ToString("X2")))}" : throw new NotImplementedException() : dr[dc2]))})"))}{(IsSqlServer ? $"; SET IDENTITY_INSERT {DestinationTableName} OFF" : "")}{(false && IsMySql ? $"; ALTER TABLE {DestinationTableName} ENABLE KEYS; UNLOCK TABLES" : "")}");

        public int ExecuteNonQuery(string commandText)
        {
            using (var dc = Connection.CreateCommand())
            {
                dc.CommandText = commandText;
                dc.Transaction = Transaction;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, dc.CommandText);
                var i = dc.ExecuteNonQuery();
                Commit();
                return i;
            }
        }

        public void DisableConstraints() => ExecuteNonQuery(IsMySql ? "SET FOREIGN_KEY_CHECKS = 0" : IsPostgreSql ? "SET SESSION session_replication_role = replica" : throw new NotImplementedException());

        private string SqlEncode(string value) => value.Replace("'", "''").Replace(@"\", IsMySql ? @"\\" : @"\");

        private string TextEncode(string value) => value.Replace(@"\", @"\\").Replace("\t", "\\t").Replace("\n", "\\n");

        public void Commit()
        {
            if (dbTransaction != null)
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "Committing transaction");
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = dbConnection.BeginTransaction();
            }
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Dispose();
            }
        }
    }
    
    public class PostgreSqlBulkCopy : IDisposable
    {
        private string connectionString;
        private DbConnection dbConnection;
        private DbTransaction dbTransaction;
        private string providerName;
        private readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public PostgreSqlBulkCopy(string name, bool checkConstraints)
        {
            providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            if (!checkConstraints) DisableConstraints();
        }

        public Dictionary<string, string> ColumnMappings { get; } = new Dictionary<string, string>();
        public string DestinationTableName { get; set; }

        private DbConnection Connection
        {
            get
            {
                if (dbConnection == null)
                {
                    dbConnection = DbProviderFactories.GetFactory(providerName).CreateConnection();
                    dbConnection.ConnectionString = connectionString;
                    dbConnection.Open();
                }
                return dbConnection;
            }
        }

        private DbTransaction Transaction
        {
            get
            {
                if (dbTransaction == null) dbTransaction = Connection.BeginTransaction();
                return dbTransaction;
            }
        }

        public void WriteToServer(DataTable table)
        {
            using (var nbi = ((NpgsqlConnection)Connection).BeginBinaryImport($"COPY {DestinationTableName} ({string.Join(", ", table.Columns.Cast<DataColumn>().Select(dc2 => dc2.ColumnName))}) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (var dr in table.Rows.Cast<DataRow>())
                {
                    nbi.StartRow();
                    for (int i = 0; i < dr.ItemArray.Count(); i++)
                        if (dr.Table.Columns[i].ExtendedProperties["NpgsqlDbType"] != null)
                            nbi.Write(dr.ItemArray[i], (NpgsqlDbType)dr.Table.Columns[i].ExtendedProperties["NpgsqlDbType"]);
                        else
                            nbi.Write(dr.ItemArray[i]);
                }
                nbi.Complete();
            }
            Commit();
        }

        public int ExecuteNonQuery(string commandText)
        {
            using (var dc = Connection.CreateCommand())
            {
                dc.CommandText = commandText;
                dc.Transaction = Transaction;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, dc.CommandText);
                var i = dc.ExecuteNonQuery();
                Commit();
                return i;
            }
        }

        public void DisableConstraints() => ExecuteNonQuery("SET SESSION session_replication_role = replica");

        public void Commit()
        {
            if (dbTransaction != null)
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "Committing transaction");
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = dbConnection.BeginTransaction();
            }
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Dispose();
            }
        }
    }
}
