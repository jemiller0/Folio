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
        private DataTable acquisitionsUnitsDataTable, addressTypesDataTable, alertsDataTable, alternativeTitleTypesDataTable, auditLoansDataTable, authAttemptsDataTable, authCredentialsHistoriesDataTable, authPasswordActionsDataTable, batchGroupsDataTable, batchVouchersDataTable, batchVoucherExportsDataTable, batchVoucherExportConfigsDataTable, blocksDataTable, budgetsDataTable, callNumberTypesDataTable, campusesDataTable, cancellationReasonsDataTable, categoriesDataTable, checkInsDataTable, circulationRulesDataTable, classificationTypesDataTable, commentsDataTable, configurationsDataTable, contactsDataTable, contributorNameTypesDataTable, contributorTypesDataTable, countriesDataTable, customFieldsDataTable, documentsDataTable, electronicAccessRelationshipsDataTable, errorRecordsDataTable, eventLogsDataTable, exportConfigCredentialsDataTable, feesDataTable, feeTypesDataTable, financeGroupsDataTable, fiscalYearsDataTable, fixedDueDateSchedulesDataTable, fundsDataTable, fundTypesDataTable, groupsDataTable, groupFundFiscalYearsDataTable, holdingsDataTable, holdingNoteTypesDataTable, holdingTypesDataTable, hridSettingsDataTable, idTypesDataTable, illPoliciesDataTable, instancesDataTable, instanceFormatsDataTable, instanceNoteTypesDataTable, instanceRelationshipsDataTable, instanceRelationshipTypesDataTable, instanceSourceMarcsDataTable, instanceStatusesDataTable, instanceTypesDataTable, institutionsDataTable, interfacesDataTable, interfaceCredentialsDataTable, invoicesDataTable, invoiceItemsDataTable, invoiceTransactionSummariesDataTable, itemsDataTable, itemDamagedStatusesDataTable, itemNoteTypesDataTable, jobExecutionsDataTable, jobExecutionProgressesDataTable, jobExecutionSourceChunksDataTable, journalRecordsDataTable, ledgersDataTable, ledgerFiscalYearsDataTable, librariesDataTable, loansDataTable, loanPoliciesDataTable, loanTypesDataTable, locationsDataTable, loginsDataTable, lostItemFeePoliciesDataTable, mappingRulesDataTable, marcRecordsDataTable, materialTypesDataTable, modeOfIssuancesDataTable, natureOfContentTermsDataTable, notesDataTable, noteTypesDataTable, ordersDataTable, orderInvoicesDataTable, orderItemsDataTable, orderTemplatesDataTable, orderTransactionSummariesDataTable, organizationsDataTable, overdueFinePoliciesDataTable, ownersDataTable, patronActionSessionsDataTable, patronBlockConditionsDataTable, patronBlockLimitsDataTable, patronNoticePoliciesDataTable, paymentsDataTable, paymentMethodsDataTable, permissionsDataTable, permissionsUsersDataTable, piecesDataTable, precedingSucceedingTitlesDataTable, prefixesDataTable, proxiesDataTable, rawRecordsDataTable, reasonsForClosuresDataTable, recordsDataTable, refundReasonsDataTable, reportingCodesDataTable, requestsDataTable, requestPoliciesDataTable, scheduledNoticesDataTable, servicePointsDataTable, servicePointUsersDataTable, snapshotsDataTable, staffSlipsDataTable, statisticalCodesDataTable, statisticalCodeTypesDataTable, suffixesDataTable, tagsDataTable, templatesDataTable, titlesDataTable, transactionsDataTable, transferAccountsDataTable, transferCriteriasDataTable, usersDataTable, userAcquisitionsUnitsDataTable, userRequestPreferencesDataTable, vouchersDataTable, voucherItemsDataTable, waiveReasonsDataTable;
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
            MySqlBulkCopy.traceSource = traceSource;
            PostgreSqlBulkCopy.traceSource = traceSource;
        }

        public int ExecuteNonQuery(string commandText) => sqlBulkCopy.ExecuteNonQuery(commandText);

        public void Insert(AcquisitionsUnit acquisitionsUnit)
        {
            if (acquisitionsUnitsDataTable == null)
            {
                acquisitionsUnitsDataTable = new DataTable();
                acquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                acquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                acquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                acquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                acquisitionsUnitsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = acquisitionsUnitsDataTable.NewRow();
            dr["id"] = (object)acquisitionsUnit.Id ?? DBNull.Value;
            dr["jsonb"] = (object)acquisitionsUnit.Content ?? DBNull.Value;
            dr["creation_date"] = (object)acquisitionsUnit.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)acquisitionsUnit.CreationUserId ?? DBNull.Value;
            acquisitionsUnitsDataTable.Rows.Add(dr);
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

        public void Insert(AlternativeTitleType alternativeTitleType)
        {
            if (alternativeTitleTypesDataTable == null)
            {
                alternativeTitleTypesDataTable = new DataTable();
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                alternativeTitleTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                alternativeTitleTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = alternativeTitleTypesDataTable.NewRow();
            dr["id"] = (object)alternativeTitleType.Id ?? DBNull.Value;
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
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                auditLoansDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                auditLoansDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = auditLoansDataTable.NewRow();
            dr["id"] = (object)auditLoan.Id ?? DBNull.Value;
            dr["jsonb"] = (object)auditLoan.Content ?? DBNull.Value;
            auditLoansDataTable.Rows.Add(dr);
        }

        public void Insert(AuthAttempt authAttempt)
        {
            if (authAttemptsDataTable == null)
            {
                authAttemptsDataTable = new DataTable();
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authAttemptsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authAttemptsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authAttemptsDataTable.NewRow();
            dr["id"] = (object)authAttempt.Id ?? DBNull.Value;
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
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authCredentialsHistoriesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authCredentialsHistoriesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authCredentialsHistoriesDataTable.NewRow();
            dr["id"] = (object)authCredentialsHistory.Id ?? DBNull.Value;
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
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                authPasswordActionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                authPasswordActionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = authPasswordActionsDataTable.NewRow();
            dr["id"] = (object)authPasswordAction.Id ?? DBNull.Value;
            dr["jsonb"] = (object)authPasswordAction.Content ?? DBNull.Value;
            dr["creation_date"] = (object)authPasswordAction.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)authPasswordAction.CreationUserId ?? DBNull.Value;
            authPasswordActionsDataTable.Rows.Add(dr);
        }

        public void Insert(BatchGroup batchGroup)
        {
            if (batchGroupsDataTable == null)
            {
                batchGroupsDataTable = new DataTable();
                batchGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                batchGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                batchGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                batchGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                batchGroupsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = batchGroupsDataTable.NewRow();
            dr["id"] = (object)batchGroup.Id ?? DBNull.Value;
            dr["jsonb"] = (object)batchGroup.Content ?? DBNull.Value;
            dr["creation_date"] = (object)batchGroup.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)batchGroup.CreationUserId ?? DBNull.Value;
            batchGroupsDataTable.Rows.Add(dr);
        }

        public void Insert(BatchVoucher batchVoucher)
        {
            if (batchVouchersDataTable == null)
            {
                batchVouchersDataTable = new DataTable();
                batchVouchersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                batchVouchersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                batchVouchersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = batchVouchersDataTable.NewRow();
            dr["id"] = (object)batchVoucher.Id ?? DBNull.Value;
            dr["jsonb"] = (object)batchVoucher.Content ?? DBNull.Value;
            batchVouchersDataTable.Rows.Add(dr);
        }

        public void Insert(BatchVoucherExport batchVoucherExport)
        {
            if (batchVoucherExportsDataTable == null)
            {
                batchVoucherExportsDataTable = new DataTable();
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "batchgroupid", DataType = typeof(Guid) });
                batchVoucherExportsDataTable.Columns.Add(new DataColumn { ColumnName = "batchvoucherid", DataType = typeof(Guid) });
                batchVoucherExportsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = batchVoucherExportsDataTable.NewRow();
            dr["id"] = (object)batchVoucherExport.Id ?? DBNull.Value;
            dr["jsonb"] = (object)batchVoucherExport.Content ?? DBNull.Value;
            dr["creation_date"] = (object)batchVoucherExport.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)batchVoucherExport.CreationUserId ?? DBNull.Value;
            dr["batchgroupid"] = (object)batchVoucherExport.Batchgroupid ?? DBNull.Value;
            dr["batchvoucherid"] = (object)batchVoucherExport.Batchvoucherid ?? DBNull.Value;
            batchVoucherExportsDataTable.Rows.Add(dr);
        }

        public void Insert(BatchVoucherExportConfig batchVoucherExportConfig)
        {
            if (batchVoucherExportConfigsDataTable == null)
            {
                batchVoucherExportConfigsDataTable = new DataTable();
                batchVoucherExportConfigsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                batchVoucherExportConfigsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                batchVoucherExportConfigsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                batchVoucherExportConfigsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                batchVoucherExportConfigsDataTable.Columns.Add(new DataColumn { ColumnName = "batchgroupid", DataType = typeof(Guid) });
                batchVoucherExportConfigsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = batchVoucherExportConfigsDataTable.NewRow();
            dr["id"] = (object)batchVoucherExportConfig.Id ?? DBNull.Value;
            dr["jsonb"] = (object)batchVoucherExportConfig.Content ?? DBNull.Value;
            dr["creation_date"] = (object)batchVoucherExportConfig.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)batchVoucherExportConfig.CreationUserId ?? DBNull.Value;
            dr["batchgroupid"] = (object)batchVoucherExportConfig.Batchgroupid ?? DBNull.Value;
            batchVoucherExportConfigsDataTable.Rows.Add(dr);
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
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fundid", DataType = typeof(Guid) });
                budgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                budgetsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = budgetsDataTable.NewRow();
            dr["id"] = (object)budget.Id ?? DBNull.Value;
            dr["jsonb"] = (object)budget.Content ?? DBNull.Value;
            dr["creation_date"] = (object)budget.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)budget.CreationUserId ?? DBNull.Value;
            dr["fundid"] = (object)budget.FundId ?? DBNull.Value;
            dr["fiscalyearid"] = (object)budget.FiscalYearId ?? DBNull.Value;
            budgetsDataTable.Rows.Add(dr);
        }

        public void Insert(CallNumberType callNumberType)
        {
            if (callNumberTypesDataTable == null)
            {
                callNumberTypesDataTable = new DataTable();
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                callNumberTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                callNumberTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = callNumberTypesDataTable.NewRow();
            dr["id"] = (object)callNumberType.Id ?? DBNull.Value;
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
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                campusesDataTable.Columns.Add(new DataColumn { ColumnName = "institutionid", DataType = typeof(Guid) });
                campusesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = campusesDataTable.NewRow();
            dr["id"] = (object)campus.Id ?? DBNull.Value;
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
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                cancellationReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                cancellationReasonsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = cancellationReasonsDataTable.NewRow();
            dr["id"] = (object)cancellationReason.Id ?? DBNull.Value;
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

        public void Insert(CheckIn checkIn)
        {
            if (checkInsDataTable == null)
            {
                checkInsDataTable = new DataTable();
                checkInsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                checkInsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                checkInsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                checkInsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                checkInsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = checkInsDataTable.NewRow();
            dr["id"] = (object)checkIn.Id ?? DBNull.Value;
            dr["jsonb"] = (object)checkIn.Content ?? DBNull.Value;
            dr["creation_date"] = (object)checkIn.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)checkIn.CreationUserId ?? DBNull.Value;
            checkInsDataTable.Rows.Add(dr);
        }

        public void Insert(CirculationRule circulationRule)
        {
            if (circulationRulesDataTable == null)
            {
                circulationRulesDataTable = new DataTable();
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "lock", DataType = typeof(bool) });
                circulationRulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = circulationRulesDataTable.NewRow();
            dr["id"] = (object)circulationRule.Id ?? DBNull.Value;
            dr["jsonb"] = (object)circulationRule.Content ?? DBNull.Value;
            dr["lock"] = (object)circulationRule.Lock ?? DBNull.Value;
            circulationRulesDataTable.Rows.Add(dr);
        }

        public void Insert(ClassificationType classificationType)
        {
            if (classificationTypesDataTable == null)
            {
                classificationTypesDataTable = new DataTable();
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                classificationTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                classificationTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = classificationTypesDataTable.NewRow();
            dr["id"] = (object)classificationType.Id ?? DBNull.Value;
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

        public void Insert(Configuration configuration)
        {
            if (configurationsDataTable == null)
            {
                configurationsDataTable = new DataTable();
                configurationsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                configurationsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                configurationsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                configurationsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                configurationsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = configurationsDataTable.NewRow();
            dr["id"] = (object)configuration.Id ?? DBNull.Value;
            dr["jsonb"] = (object)configuration.Content ?? DBNull.Value;
            dr["creation_date"] = (object)configuration.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)configuration.CreationUserId ?? DBNull.Value;
            configurationsDataTable.Rows.Add(dr);
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

        public void Insert(ContributorNameType contributorNameType)
        {
            if (contributorNameTypesDataTable == null)
            {
                contributorNameTypesDataTable = new DataTable();
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                contributorNameTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                contributorNameTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contributorNameTypesDataTable.NewRow();
            dr["id"] = (object)contributorNameType.Id ?? DBNull.Value;
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
                contributorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                contributorTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                contributorTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = contributorTypesDataTable.NewRow();
            dr["id"] = (object)contributorType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)contributorType.Content ?? DBNull.Value;
            contributorTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Country country)
        {
            if (countriesDataTable == null)
            {
                countriesDataTable = new DataTable();
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "alpha2_code", DataType = typeof(string) });
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "alpha3_code", DataType = typeof(string) });
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                
            }
            var dr = countriesDataTable.NewRow();
            dr["alpha2_code"] = (object)country.Alpha2Code ?? DBNull.Value;
            dr["alpha3_code"] = (object)country.Alpha3Code ?? DBNull.Value;
            dr["name"] = (object)country.Name ?? DBNull.Value;
            countriesDataTable.Rows.Add(dr);
        }

        public void Insert(CustomField customField)
        {
            if (customFieldsDataTable == null)
            {
                customFieldsDataTable = new DataTable();
                customFieldsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                customFieldsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                customFieldsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = customFieldsDataTable.NewRow();
            dr["id"] = (object)customField.Id ?? DBNull.Value;
            dr["jsonb"] = (object)customField.Content ?? DBNull.Value;
            customFieldsDataTable.Rows.Add(dr);
        }

        public void Insert(Document document)
        {
            if (documentsDataTable == null)
            {
                documentsDataTable = new DataTable();
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "invoiceid", DataType = typeof(Guid) });
                documentsDataTable.Columns.Add(new DataColumn { ColumnName = "document_data", DataType = typeof(string) });
                documentsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = documentsDataTable.NewRow();
            dr["id"] = (object)document.Id ?? DBNull.Value;
            dr["jsonb"] = (object)document.Content ?? DBNull.Value;
            dr["creation_date"] = (object)document.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)document.CreationUserId ?? DBNull.Value;
            dr["invoiceid"] = (object)document.Invoiceid ?? DBNull.Value;
            dr["document_data"] = (object)document.DocumentData ?? DBNull.Value;
            documentsDataTable.Rows.Add(dr);
        }

        public void Insert(ElectronicAccessRelationship electronicAccessRelationship)
        {
            if (electronicAccessRelationshipsDataTable == null)
            {
                electronicAccessRelationshipsDataTable = new DataTable();
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                electronicAccessRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                electronicAccessRelationshipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = electronicAccessRelationshipsDataTable.NewRow();
            dr["id"] = (object)electronicAccessRelationship.Id ?? DBNull.Value;
            dr["jsonb"] = (object)electronicAccessRelationship.Content ?? DBNull.Value;
            dr["creation_date"] = (object)electronicAccessRelationship.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)electronicAccessRelationship.CreationUserId ?? DBNull.Value;
            electronicAccessRelationshipsDataTable.Rows.Add(dr);
        }

        public void Insert(ErrorRecord errorRecord)
        {
            if (errorRecordsDataTable == null)
            {
                errorRecordsDataTable = new DataTable();
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                errorRecordsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = errorRecordsDataTable.NewRow();
            dr["id"] = (object)errorRecord.Id ?? DBNull.Value;
            dr["jsonb"] = (object)errorRecord.Content ?? DBNull.Value;
            dr["creation_date"] = (object)errorRecord.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)errorRecord.CreationUserId ?? DBNull.Value;
            errorRecordsDataTable.Rows.Add(dr);
        }

        public void Insert(EventLog eventLog)
        {
            if (eventLogsDataTable == null)
            {
                eventLogsDataTable = new DataTable();
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                eventLogsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                eventLogsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = eventLogsDataTable.NewRow();
            dr["id"] = (object)eventLog.Id ?? DBNull.Value;
            dr["jsonb"] = (object)eventLog.Content ?? DBNull.Value;
            dr["creation_date"] = (object)eventLog.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)eventLog.CreationUserId ?? DBNull.Value;
            eventLogsDataTable.Rows.Add(dr);
        }

        public void Insert(ExportConfigCredential exportConfigCredential)
        {
            if (exportConfigCredentialsDataTable == null)
            {
                exportConfigCredentialsDataTable = new DataTable();
                exportConfigCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                exportConfigCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                exportConfigCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                exportConfigCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                exportConfigCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "exportconfigid", DataType = typeof(Guid) });
                exportConfigCredentialsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = exportConfigCredentialsDataTable.NewRow();
            dr["id"] = (object)exportConfigCredential.Id ?? DBNull.Value;
            dr["jsonb"] = (object)exportConfigCredential.Content ?? DBNull.Value;
            dr["creation_date"] = (object)exportConfigCredential.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)exportConfigCredential.CreationUserId ?? DBNull.Value;
            dr["exportconfigid"] = (object)exportConfigCredential.Exportconfigid ?? DBNull.Value;
            exportConfigCredentialsDataTable.Rows.Add(dr);
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
                feesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = feesDataTable.NewRow();
            dr["id"] = (object)fee.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fee.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fee.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fee.CreationUserId ?? DBNull.Value;
            feesDataTable.Rows.Add(dr);
        }

        public void Insert(FeeType feeType)
        {
            if (feeTypesDataTable == null)
            {
                feeTypesDataTable = new DataTable();
                feeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                feeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                feeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                feeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                feeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "ownerid", DataType = typeof(Guid) });
                feeTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = feeTypesDataTable.NewRow();
            dr["id"] = (object)feeType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)feeType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)feeType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)feeType.CreationUserId ?? DBNull.Value;
            dr["ownerid"] = (object)feeType.Ownerid ?? DBNull.Value;
            feeTypesDataTable.Rows.Add(dr);
        }

        public void Insert(FinanceGroup financeGroup)
        {
            if (financeGroupsDataTable == null)
            {
                financeGroupsDataTable = new DataTable();
                financeGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                financeGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                financeGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                financeGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                financeGroupsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = financeGroupsDataTable.NewRow();
            dr["id"] = (object)financeGroup.Id ?? DBNull.Value;
            dr["jsonb"] = (object)financeGroup.Content ?? DBNull.Value;
            dr["creation_date"] = (object)financeGroup.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)financeGroup.CreationUserId ?? DBNull.Value;
            financeGroupsDataTable.Rows.Add(dr);
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
                fixedDueDateSchedulesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fixedDueDateSchedulesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fixedDueDateSchedulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fixedDueDateSchedulesDataTable.NewRow();
            dr["id"] = (object)fixedDueDateSchedule.Id ?? DBNull.Value;
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
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerid", DataType = typeof(Guid) });
                fundsDataTable.Columns.Add(new DataColumn { ColumnName = "fundtypeid", DataType = typeof(Guid) });
                fundsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fundsDataTable.NewRow();
            dr["id"] = (object)fund.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fund.Content ?? DBNull.Value;
            dr["creation_date"] = (object)fund.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)fund.CreationUserId ?? DBNull.Value;
            dr["ledgerid"] = (object)fund.LedgerId ?? DBNull.Value;
            dr["fundtypeid"] = (object)fund.Fundtypeid ?? DBNull.Value;
            fundsDataTable.Rows.Add(dr);
        }

        public void Insert(FundType fundType)
        {
            if (fundTypesDataTable == null)
            {
                fundTypesDataTable = new DataTable();
                fundTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                fundTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                fundTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = fundTypesDataTable.NewRow();
            dr["id"] = (object)fundType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)fundType.Content ?? DBNull.Value;
            fundTypesDataTable.Rows.Add(dr);
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

        public void Insert(GroupFundFiscalYear groupFundFiscalYear)
        {
            if (groupFundFiscalYearsDataTable == null)
            {
                groupFundFiscalYearsDataTable = new DataTable();
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "budgetid", DataType = typeof(Guid) });
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "groupid", DataType = typeof(Guid) });
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "fundid", DataType = typeof(Guid) });
                groupFundFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                groupFundFiscalYearsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = groupFundFiscalYearsDataTable.NewRow();
            dr["id"] = (object)groupFundFiscalYear.Id ?? DBNull.Value;
            dr["jsonb"] = (object)groupFundFiscalYear.Content ?? DBNull.Value;
            dr["budgetid"] = (object)groupFundFiscalYear.Budgetid ?? DBNull.Value;
            dr["groupid"] = (object)groupFundFiscalYear.Groupid ?? DBNull.Value;
            dr["fundid"] = (object)groupFundFiscalYear.Fundid ?? DBNull.Value;
            dr["fiscalyearid"] = (object)groupFundFiscalYear.Fiscalyearid ?? DBNull.Value;
            groupFundFiscalYearsDataTable.Rows.Add(dr);
        }

        public void Insert(Holding holding)
        {
            if (holdingsDataTable == null)
            {
                holdingsDataTable = new DataTable();
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
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
            dr["id"] = (object)holding.Id ?? DBNull.Value;
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
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                holdingNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                holdingNoteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = holdingNoteTypesDataTable.NewRow();
            dr["id"] = (object)holdingNoteType.Id ?? DBNull.Value;
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
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                holdingTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                holdingTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = holdingTypesDataTable.NewRow();
            dr["id"] = (object)holdingType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)holdingType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)holdingType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)holdingType.CreationUserId ?? DBNull.Value;
            holdingTypesDataTable.Rows.Add(dr);
        }

        public void Insert(HridSetting hridSetting)
        {
            if (hridSettingsDataTable == null)
            {
                hridSettingsDataTable = new DataTable();
                hridSettingsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                hridSettingsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                hridSettingsDataTable.Columns.Add(new DataColumn { ColumnName = "lock", DataType = typeof(bool) });
                hridSettingsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = hridSettingsDataTable.NewRow();
            dr["id"] = (object)hridSetting.Id ?? DBNull.Value;
            dr["jsonb"] = (object)hridSetting.Content ?? DBNull.Value;
            dr["lock"] = (object)hridSetting.Lock ?? DBNull.Value;
            hridSettingsDataTable.Rows.Add(dr);
        }

        public void Insert(IdType idType)
        {
            if (idTypesDataTable == null)
            {
                idTypesDataTable = new DataTable();
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                idTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                idTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = idTypesDataTable.NewRow();
            dr["id"] = (object)idType.Id ?? DBNull.Value;
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
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                illPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                illPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = illPoliciesDataTable.NewRow();
            dr["id"] = (object)illPolicy.Id ?? DBNull.Value;
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
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "instancestatusid", DataType = typeof(Guid) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "modeofissuanceid", DataType = typeof(Guid) });
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "instancetypeid", DataType = typeof(Guid) });
                instancesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instancesDataTable.NewRow();
            dr["id"] = (object)instance.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instance.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instance.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instance.CreationUserId ?? DBNull.Value;
            dr["instancestatusid"] = (object)instance.Instancestatusid ?? DBNull.Value;
            dr["modeofissuanceid"] = (object)instance.Modeofissuanceid ?? DBNull.Value;
            dr["instancetypeid"] = (object)instance.Instancetypeid ?? DBNull.Value;
            instancesDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceFormat instanceFormat)
        {
            if (instanceFormatsDataTable == null)
            {
                instanceFormatsDataTable = new DataTable();
                instanceFormatsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceFormatsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceFormatsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceFormatsDataTable.NewRow();
            dr["id"] = (object)instanceFormat.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceFormat.Content ?? DBNull.Value;
            instanceFormatsDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceNoteType instanceNoteType)
        {
            if (instanceNoteTypesDataTable == null)
            {
                instanceNoteTypesDataTable = new DataTable();
                instanceNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceNoteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceNoteTypesDataTable.NewRow();
            dr["id"] = (object)instanceNoteType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceNoteType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)instanceNoteType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)instanceNoteType.CreationUserId ?? DBNull.Value;
            instanceNoteTypesDataTable.Rows.Add(dr);
        }

        public void Insert(InstanceRelationship instanceRelationship)
        {
            if (instanceRelationshipsDataTable == null)
            {
                instanceRelationshipsDataTable = new DataTable();
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "superinstanceid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "subinstanceid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns.Add(new DataColumn { ColumnName = "instancerelationshiptypeid", DataType = typeof(Guid) });
                instanceRelationshipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceRelationshipsDataTable.NewRow();
            dr["id"] = (object)instanceRelationship.Id ?? DBNull.Value;
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
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceRelationshipTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceRelationshipTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceRelationshipTypesDataTable.NewRow();
            dr["id"] = (object)instanceRelationshipType.Id ?? DBNull.Value;
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
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceSourceMarcsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceSourceMarcsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceSourceMarcsDataTable.NewRow();
            dr["id"] = (object)instanceSourceMarc.Id ?? DBNull.Value;
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
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                instanceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                instanceStatusesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceStatusesDataTable.NewRow();
            dr["id"] = (object)instanceStatus.Id ?? DBNull.Value;
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
                instanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                instanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                instanceTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = instanceTypesDataTable.NewRow();
            dr["id"] = (object)instanceType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)instanceType.Content ?? DBNull.Value;
            instanceTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Institution institution)
        {
            if (institutionsDataTable == null)
            {
                institutionsDataTable = new DataTable();
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                institutionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                institutionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = institutionsDataTable.NewRow();
            dr["id"] = (object)institution.Id ?? DBNull.Value;
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

        public void Insert(InterfaceCredential interfaceCredential)
        {
            if (interfaceCredentialsDataTable == null)
            {
                interfaceCredentialsDataTable = new DataTable();
                interfaceCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                interfaceCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                interfaceCredentialsDataTable.Columns.Add(new DataColumn { ColumnName = "interfaceid", DataType = typeof(Guid) });
                interfaceCredentialsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = interfaceCredentialsDataTable.NewRow();
            dr["id"] = (object)interfaceCredential.Id ?? DBNull.Value;
            dr["jsonb"] = (object)interfaceCredential.Content ?? DBNull.Value;
            dr["interfaceid"] = (object)interfaceCredential.Interfaceid ?? DBNull.Value;
            interfaceCredentialsDataTable.Rows.Add(dr);
        }

        public void Insert(Invoice invoice)
        {
            if (invoicesDataTable == null)
            {
                invoicesDataTable = new DataTable();
                invoicesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                invoicesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                invoicesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                invoicesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                invoicesDataTable.Columns.Add(new DataColumn { ColumnName = "batchgroupid", DataType = typeof(Guid) });
                invoicesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = invoicesDataTable.NewRow();
            dr["id"] = (object)invoice.Id ?? DBNull.Value;
            dr["jsonb"] = (object)invoice.Content ?? DBNull.Value;
            dr["creation_date"] = (object)invoice.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)invoice.CreationUserId ?? DBNull.Value;
            dr["batchgroupid"] = (object)invoice.Batchgroupid ?? DBNull.Value;
            invoicesDataTable.Rows.Add(dr);
        }

        public void Insert(InvoiceItem invoiceItem)
        {
            if (invoiceItemsDataTable == null)
            {
                invoiceItemsDataTable = new DataTable();
                invoiceItemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                invoiceItemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                invoiceItemsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                invoiceItemsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                invoiceItemsDataTable.Columns.Add(new DataColumn { ColumnName = "invoiceid", DataType = typeof(Guid) });
                invoiceItemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = invoiceItemsDataTable.NewRow();
            dr["id"] = (object)invoiceItem.Id ?? DBNull.Value;
            dr["jsonb"] = (object)invoiceItem.Content ?? DBNull.Value;
            dr["creation_date"] = (object)invoiceItem.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)invoiceItem.CreationUserId ?? DBNull.Value;
            dr["invoiceid"] = (object)invoiceItem.Invoiceid ?? DBNull.Value;
            invoiceItemsDataTable.Rows.Add(dr);
        }

        public void Insert(InvoiceTransactionSummary invoiceTransactionSummary)
        {
            if (invoiceTransactionSummariesDataTable == null)
            {
                invoiceTransactionSummariesDataTable = new DataTable();
                invoiceTransactionSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                invoiceTransactionSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                invoiceTransactionSummariesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = invoiceTransactionSummariesDataTable.NewRow();
            dr["id"] = (object)invoiceTransactionSummary.Id ?? DBNull.Value;
            dr["jsonb"] = (object)invoiceTransactionSummary.Content ?? DBNull.Value;
            invoiceTransactionSummariesDataTable.Rows.Add(dr);
        }

        public void Insert(Item item)
        {
            if (itemsDataTable == null)
            {
                itemsDataTable = new DataTable();
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "holdingsrecordid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "permanentloantypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "temporaryloantypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "materialtypeid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "permanentlocationid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "temporarylocationid", DataType = typeof(Guid) });
                itemsDataTable.Columns.Add(new DataColumn { ColumnName = "effectivelocationid", DataType = typeof(Guid) });
                itemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = itemsDataTable.NewRow();
            dr["id"] = (object)item.Id ?? DBNull.Value;
            dr["jsonb"] = (object)item.Content ?? DBNull.Value;
            dr["creation_date"] = (object)item.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)item.CreationUserId ?? DBNull.Value;
            dr["holdingsrecordid"] = (object)item.Holdingsrecordid ?? DBNull.Value;
            dr["permanentloantypeid"] = (object)item.Permanentloantypeid ?? DBNull.Value;
            dr["temporaryloantypeid"] = (object)item.Temporaryloantypeid ?? DBNull.Value;
            dr["materialtypeid"] = (object)item.Materialtypeid ?? DBNull.Value;
            dr["permanentlocationid"] = (object)item.Permanentlocationid ?? DBNull.Value;
            dr["temporarylocationid"] = (object)item.Temporarylocationid ?? DBNull.Value;
            dr["effectivelocationid"] = (object)item.Effectivelocationid ?? DBNull.Value;
            itemsDataTable.Rows.Add(dr);
        }

        public void Insert(ItemDamagedStatus itemDamagedStatus)
        {
            if (itemDamagedStatusesDataTable == null)
            {
                itemDamagedStatusesDataTable = new DataTable();
                itemDamagedStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                itemDamagedStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                itemDamagedStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                itemDamagedStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                itemDamagedStatusesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = itemDamagedStatusesDataTable.NewRow();
            dr["id"] = (object)itemDamagedStatus.Id ?? DBNull.Value;
            dr["jsonb"] = (object)itemDamagedStatus.Content ?? DBNull.Value;
            dr["creation_date"] = (object)itemDamagedStatus.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)itemDamagedStatus.CreationUserId ?? DBNull.Value;
            itemDamagedStatusesDataTable.Rows.Add(dr);
        }

        public void Insert(ItemNoteType itemNoteType)
        {
            if (itemNoteTypesDataTable == null)
            {
                itemNoteTypesDataTable = new DataTable();
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                itemNoteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                itemNoteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = itemNoteTypesDataTable.NewRow();
            dr["id"] = (object)itemNoteType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)itemNoteType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)itemNoteType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)itemNoteType.CreationUserId ?? DBNull.Value;
            itemNoteTypesDataTable.Rows.Add(dr);
        }

        public void Insert(JobExecution jobExecution)
        {
            if (jobExecutionsDataTable == null)
            {
                jobExecutionsDataTable = new DataTable();
                jobExecutionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                jobExecutionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                jobExecutionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = jobExecutionsDataTable.NewRow();
            dr["id"] = (object)jobExecution.Id ?? DBNull.Value;
            dr["jsonb"] = (object)jobExecution.Content ?? DBNull.Value;
            jobExecutionsDataTable.Rows.Add(dr);
        }

        public void Insert(JobExecutionProgress jobExecutionProgress)
        {
            if (jobExecutionProgressesDataTable == null)
            {
                jobExecutionProgressesDataTable = new DataTable();
                jobExecutionProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                jobExecutionProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                jobExecutionProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "jobexecutionid", DataType = typeof(Guid) });
                jobExecutionProgressesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = jobExecutionProgressesDataTable.NewRow();
            dr["id"] = (object)jobExecutionProgress.Id ?? DBNull.Value;
            dr["jsonb"] = (object)jobExecutionProgress.Content ?? DBNull.Value;
            dr["jobexecutionid"] = (object)jobExecutionProgress.Jobexecutionid ?? DBNull.Value;
            jobExecutionProgressesDataTable.Rows.Add(dr);
        }

        public void Insert(JobExecutionSourceChunk jobExecutionSourceChunk)
        {
            if (jobExecutionSourceChunksDataTable == null)
            {
                jobExecutionSourceChunksDataTable = new DataTable();
                jobExecutionSourceChunksDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                jobExecutionSourceChunksDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                jobExecutionSourceChunksDataTable.Columns.Add(new DataColumn { ColumnName = "jobexecutionid", DataType = typeof(Guid) });
                jobExecutionSourceChunksDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = jobExecutionSourceChunksDataTable.NewRow();
            dr["id"] = (object)jobExecutionSourceChunk.Id ?? DBNull.Value;
            dr["jsonb"] = (object)jobExecutionSourceChunk.Content ?? DBNull.Value;
            dr["jobexecutionid"] = (object)jobExecutionSourceChunk.Jobexecutionid ?? DBNull.Value;
            jobExecutionSourceChunksDataTable.Rows.Add(dr);
        }

        public void Insert(JournalRecord journalRecord)
        {
            if (journalRecordsDataTable == null)
            {
                journalRecordsDataTable = new DataTable();
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "job_execution_id", DataType = typeof(Guid) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "source_id", DataType = typeof(Guid) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "source_record_order", DataType = typeof(int) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "entity_type", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "entity_id", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "entity_hrid", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "action_type", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "action_status", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "error", DataType = typeof(string) });
                journalRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "action_date", DataType = typeof(DateTime) });
                
            }
            var dr = journalRecordsDataTable.NewRow();
            dr["id"] = (object)journalRecord.Id ?? DBNull.Value;
            dr["job_execution_id"] = (object)journalRecord.JobExecutionId ?? DBNull.Value;
            dr["source_id"] = (object)journalRecord.SourceId ?? DBNull.Value;
            dr["source_record_order"] = (object)journalRecord.SourceRecordOrder ?? DBNull.Value;
            dr["entity_type"] = (object)journalRecord.EntityType ?? DBNull.Value;
            dr["entity_id"] = (object)journalRecord.EntityId ?? DBNull.Value;
            dr["entity_hrid"] = (object)journalRecord.EntityHrid ?? DBNull.Value;
            dr["action_type"] = (object)journalRecord.ActionType ?? DBNull.Value;
            dr["action_status"] = (object)journalRecord.ActionStatus ?? DBNull.Value;
            dr["error"] = (object)journalRecord.Error ?? DBNull.Value;
            dr["action_date"] = (object)journalRecord.ActionDate ?? DBNull.Value;
            journalRecordsDataTable.Rows.Add(dr);
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
                ledgersDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearoneid", DataType = typeof(Guid) });
                ledgersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = ledgersDataTable.NewRow();
            dr["id"] = (object)ledger.Id ?? DBNull.Value;
            dr["jsonb"] = (object)ledger.Content ?? DBNull.Value;
            dr["creation_date"] = (object)ledger.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)ledger.CreationUserId ?? DBNull.Value;
            dr["fiscalyearoneid"] = (object)ledger.Fiscalyearoneid ?? DBNull.Value;
            ledgersDataTable.Rows.Add(dr);
        }

        public void Insert(LedgerFiscalYear ledgerFiscalYear)
        {
            if (ledgerFiscalYearsDataTable == null)
            {
                ledgerFiscalYearsDataTable = new DataTable();
                ledgerFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                ledgerFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                ledgerFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerid", DataType = typeof(Guid) });
                ledgerFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                ledgerFiscalYearsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = ledgerFiscalYearsDataTable.NewRow();
            dr["id"] = (object)ledgerFiscalYear.Id ?? DBNull.Value;
            dr["jsonb"] = (object)ledgerFiscalYear.Content ?? DBNull.Value;
            dr["ledgerid"] = (object)ledgerFiscalYear.Ledgerid ?? DBNull.Value;
            dr["fiscalyearid"] = (object)ledgerFiscalYear.Fiscalyearid ?? DBNull.Value;
            ledgerFiscalYearsDataTable.Rows.Add(dr);
        }

        public void Insert(Library library)
        {
            if (librariesDataTable == null)
            {
                librariesDataTable = new DataTable();
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                librariesDataTable.Columns.Add(new DataColumn { ColumnName = "campusid", DataType = typeof(Guid) });
                librariesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = librariesDataTable.NewRow();
            dr["id"] = (object)library.Id ?? DBNull.Value;
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
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loansDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loansDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loansDataTable.NewRow();
            dr["id"] = (object)loan.Id ?? DBNull.Value;
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
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "loanspolicy_fixedduedatescheduleid", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "renewalspolicy_alternatefixedduedatescheduleid", DataType = typeof(Guid) });
                loanPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loanPoliciesDataTable.NewRow();
            dr["id"] = (object)loanPolicy.Id ?? DBNull.Value;
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
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loanTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loanTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loanTypesDataTable.NewRow();
            dr["id"] = (object)loanType.Id ?? DBNull.Value;
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
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "institutionid", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "campusid", DataType = typeof(Guid) });
                locationsDataTable.Columns.Add(new DataColumn { ColumnName = "libraryid", DataType = typeof(Guid) });
                locationsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = locationsDataTable.NewRow();
            dr["id"] = (object)location.Id ?? DBNull.Value;
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
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                loginsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                loginsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loginsDataTable.NewRow();
            dr["id"] = (object)login.Id ?? DBNull.Value;
            dr["jsonb"] = (object)login.Content ?? DBNull.Value;
            dr["creation_date"] = (object)login.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)login.CreationUserId ?? DBNull.Value;
            loginsDataTable.Rows.Add(dr);
        }

        public void Insert(LostItemFeePolicy lostItemFeePolicy)
        {
            if (lostItemFeePoliciesDataTable == null)
            {
                lostItemFeePoliciesDataTable = new DataTable();
                lostItemFeePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                lostItemFeePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                lostItemFeePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                lostItemFeePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                lostItemFeePoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = lostItemFeePoliciesDataTable.NewRow();
            dr["id"] = (object)lostItemFeePolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)lostItemFeePolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)lostItemFeePolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)lostItemFeePolicy.CreationUserId ?? DBNull.Value;
            lostItemFeePoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(MappingRule mappingRule)
        {
            if (mappingRulesDataTable == null)
            {
                mappingRulesDataTable = new DataTable();
                mappingRulesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                mappingRulesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                mappingRulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = mappingRulesDataTable.NewRow();
            dr["id"] = (object)mappingRule.Id ?? DBNull.Value;
            dr["jsonb"] = (object)mappingRule.Content ?? DBNull.Value;
            mappingRulesDataTable.Rows.Add(dr);
        }

        public void Insert(MarcRecord marcRecord)
        {
            if (marcRecordsDataTable == null)
            {
                marcRecordsDataTable = new DataTable();
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                marcRecordsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = marcRecordsDataTable.NewRow();
            dr["id"] = (object)marcRecord.Id ?? DBNull.Value;
            dr["jsonb"] = (object)marcRecord.Content ?? DBNull.Value;
            dr["creation_date"] = (object)marcRecord.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)marcRecord.CreationUserId ?? DBNull.Value;
            marcRecordsDataTable.Rows.Add(dr);
        }

        public void Insert(MaterialType materialType)
        {
            if (materialTypesDataTable == null)
            {
                materialTypesDataTable = new DataTable();
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                materialTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                materialTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = materialTypesDataTable.NewRow();
            dr["id"] = (object)materialType.Id ?? DBNull.Value;
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
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                modeOfIssuancesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                modeOfIssuancesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = modeOfIssuancesDataTable.NewRow();
            dr["id"] = (object)modeOfIssuance.Id ?? DBNull.Value;
            dr["jsonb"] = (object)modeOfIssuance.Content ?? DBNull.Value;
            dr["creation_date"] = (object)modeOfIssuance.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)modeOfIssuance.CreationUserId ?? DBNull.Value;
            modeOfIssuancesDataTable.Rows.Add(dr);
        }

        public void Insert(NatureOfContentTerm natureOfContentTerm)
        {
            if (natureOfContentTermsDataTable == null)
            {
                natureOfContentTermsDataTable = new DataTable();
                natureOfContentTermsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                natureOfContentTermsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                natureOfContentTermsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                natureOfContentTermsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                natureOfContentTermsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = natureOfContentTermsDataTable.NewRow();
            dr["id"] = (object)natureOfContentTerm.Id ?? DBNull.Value;
            dr["jsonb"] = (object)natureOfContentTerm.Content ?? DBNull.Value;
            dr["creation_date"] = (object)natureOfContentTerm.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)natureOfContentTerm.CreationUserId ?? DBNull.Value;
            natureOfContentTermsDataTable.Rows.Add(dr);
        }

        public void Insert(Note note)
        {
            if (notesDataTable == null)
            {
                notesDataTable = new DataTable();
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "temporary_type_id", DataType = typeof(Guid) });
                notesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = notesDataTable.NewRow();
            dr["id"] = (object)note.Id ?? DBNull.Value;
            dr["jsonb"] = (object)note.Content ?? DBNull.Value;
            dr["temporary_type_id"] = (object)note.TemporaryTypeId ?? DBNull.Value;
            notesDataTable.Rows.Add(dr);
        }

        public void Insert(NoteType noteType)
        {
            if (noteTypesDataTable == null)
            {
                noteTypesDataTable = new DataTable();
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                noteTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = noteTypesDataTable.NewRow();
            dr["id"] = (object)noteType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)noteType.Content ?? DBNull.Value;
            noteTypesDataTable.Rows.Add(dr);
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

        public void Insert(OrderInvoice orderInvoice)
        {
            if (orderInvoicesDataTable == null)
            {
                orderInvoicesDataTable = new DataTable();
                orderInvoicesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                orderInvoicesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                orderInvoicesDataTable.Columns.Add(new DataColumn { ColumnName = "purchaseorderid", DataType = typeof(Guid) });
                orderInvoicesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderInvoicesDataTable.NewRow();
            dr["id"] = (object)orderInvoice.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderInvoice.Content ?? DBNull.Value;
            dr["purchaseorderid"] = (object)orderInvoice.Purchaseorderid ?? DBNull.Value;
            orderInvoicesDataTable.Rows.Add(dr);
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
                orderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "purchaseorderid", DataType = typeof(Guid) });
                orderItemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderItemsDataTable.NewRow();
            dr["id"] = (object)orderItem.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderItem.Content ?? DBNull.Value;
            dr["creation_date"] = (object)orderItem.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)orderItem.CreationUserId ?? DBNull.Value;
            dr["purchaseorderid"] = (object)orderItem.Purchaseorderid ?? DBNull.Value;
            orderItemsDataTable.Rows.Add(dr);
        }

        public void Insert(OrderTemplate orderTemplate)
        {
            if (orderTemplatesDataTable == null)
            {
                orderTemplatesDataTable = new DataTable();
                orderTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                orderTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                orderTemplatesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderTemplatesDataTable.NewRow();
            dr["id"] = (object)orderTemplate.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderTemplate.Content ?? DBNull.Value;
            orderTemplatesDataTable.Rows.Add(dr);
        }

        public void Insert(OrderTransactionSummary orderTransactionSummary)
        {
            if (orderTransactionSummariesDataTable == null)
            {
                orderTransactionSummariesDataTable = new DataTable();
                orderTransactionSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                orderTransactionSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                orderTransactionSummariesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = orderTransactionSummariesDataTable.NewRow();
            dr["id"] = (object)orderTransactionSummary.Id ?? DBNull.Value;
            dr["jsonb"] = (object)orderTransactionSummary.Content ?? DBNull.Value;
            orderTransactionSummariesDataTable.Rows.Add(dr);
        }

        public void Insert(Organization organization)
        {
            if (organizationsDataTable == null)
            {
                organizationsDataTable = new DataTable();
                organizationsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                organizationsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                organizationsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                organizationsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                organizationsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = organizationsDataTable.NewRow();
            dr["id"] = (object)organization.Id ?? DBNull.Value;
            dr["jsonb"] = (object)organization.Content ?? DBNull.Value;
            dr["creation_date"] = (object)organization.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)organization.CreationUserId ?? DBNull.Value;
            organizationsDataTable.Rows.Add(dr);
        }

        public void Insert(OverdueFinePolicy overdueFinePolicy)
        {
            if (overdueFinePoliciesDataTable == null)
            {
                overdueFinePoliciesDataTable = new DataTable();
                overdueFinePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                overdueFinePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                overdueFinePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                overdueFinePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                overdueFinePoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = overdueFinePoliciesDataTable.NewRow();
            dr["id"] = (object)overdueFinePolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)overdueFinePolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)overdueFinePolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)overdueFinePolicy.CreationUserId ?? DBNull.Value;
            overdueFinePoliciesDataTable.Rows.Add(dr);
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

        public void Insert(PatronActionSession patronActionSession)
        {
            if (patronActionSessionsDataTable == null)
            {
                patronActionSessionsDataTable = new DataTable();
                patronActionSessionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                patronActionSessionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                patronActionSessionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                patronActionSessionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                patronActionSessionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = patronActionSessionsDataTable.NewRow();
            dr["id"] = (object)patronActionSession.Id ?? DBNull.Value;
            dr["jsonb"] = (object)patronActionSession.Content ?? DBNull.Value;
            dr["creation_date"] = (object)patronActionSession.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)patronActionSession.CreationUserId ?? DBNull.Value;
            patronActionSessionsDataTable.Rows.Add(dr);
        }

        public void Insert(PatronBlockCondition patronBlockCondition)
        {
            if (patronBlockConditionsDataTable == null)
            {
                patronBlockConditionsDataTable = new DataTable();
                patronBlockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                patronBlockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                patronBlockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                patronBlockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                patronBlockConditionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = patronBlockConditionsDataTable.NewRow();
            dr["id"] = (object)patronBlockCondition.Id ?? DBNull.Value;
            dr["jsonb"] = (object)patronBlockCondition.Content ?? DBNull.Value;
            dr["creation_date"] = (object)patronBlockCondition.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)patronBlockCondition.CreationUserId ?? DBNull.Value;
            patronBlockConditionsDataTable.Rows.Add(dr);
        }

        public void Insert(PatronBlockLimit patronBlockLimit)
        {
            if (patronBlockLimitsDataTable == null)
            {
                patronBlockLimitsDataTable = new DataTable();
                patronBlockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                patronBlockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                patronBlockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                patronBlockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                patronBlockLimitsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = patronBlockLimitsDataTable.NewRow();
            dr["id"] = (object)patronBlockLimit.Id ?? DBNull.Value;
            dr["jsonb"] = (object)patronBlockLimit.Content ?? DBNull.Value;
            dr["creation_date"] = (object)patronBlockLimit.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)patronBlockLimit.CreationUserId ?? DBNull.Value;
            patronBlockLimitsDataTable.Rows.Add(dr);
        }

        public void Insert(PatronNoticePolicy patronNoticePolicy)
        {
            if (patronNoticePoliciesDataTable == null)
            {
                patronNoticePoliciesDataTable = new DataTable();
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                patronNoticePoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                patronNoticePoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = patronNoticePoliciesDataTable.NewRow();
            dr["id"] = (object)patronNoticePolicy.Id ?? DBNull.Value;
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

        public void Insert(PaymentMethod paymentMethod)
        {
            if (paymentMethodsDataTable == null)
            {
                paymentMethodsDataTable = new DataTable();
                paymentMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                paymentMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                paymentMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                paymentMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                paymentMethodsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = paymentMethodsDataTable.NewRow();
            dr["id"] = (object)paymentMethod.Id ?? DBNull.Value;
            dr["jsonb"] = (object)paymentMethod.Content ?? DBNull.Value;
            dr["creation_date"] = (object)paymentMethod.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)paymentMethod.CreationUserId ?? DBNull.Value;
            paymentMethodsDataTable.Rows.Add(dr);
        }

        public void Insert(Permission permission)
        {
            if (permissionsDataTable == null)
            {
                permissionsDataTable = new DataTable();
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                permissionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                permissionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = permissionsDataTable.NewRow();
            dr["id"] = (object)permission.Id ?? DBNull.Value;
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
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                permissionsUsersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                permissionsUsersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = permissionsUsersDataTable.NewRow();
            dr["id"] = (object)permissionsUser.Id ?? DBNull.Value;
            dr["jsonb"] = (object)permissionsUser.Content ?? DBNull.Value;
            dr["creation_date"] = (object)permissionsUser.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)permissionsUser.CreationUserId ?? DBNull.Value;
            permissionsUsersDataTable.Rows.Add(dr);
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
                piecesDataTable.Columns.Add(new DataColumn { ColumnName = "polineid", DataType = typeof(Guid) });
                piecesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = piecesDataTable.NewRow();
            dr["id"] = (object)piece.Id ?? DBNull.Value;
            dr["jsonb"] = (object)piece.Content ?? DBNull.Value;
            dr["creation_date"] = (object)piece.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)piece.CreationUserId ?? DBNull.Value;
            dr["polineid"] = (object)piece.Polineid ?? DBNull.Value;
            piecesDataTable.Rows.Add(dr);
        }

        public void Insert(PrecedingSucceedingTitle precedingSucceedingTitle)
        {
            if (precedingSucceedingTitlesDataTable == null)
            {
                precedingSucceedingTitlesDataTable = new DataTable();
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "precedinginstanceid", DataType = typeof(Guid) });
                precedingSucceedingTitlesDataTable.Columns.Add(new DataColumn { ColumnName = "succeedinginstanceid", DataType = typeof(Guid) });
                precedingSucceedingTitlesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = precedingSucceedingTitlesDataTable.NewRow();
            dr["id"] = (object)precedingSucceedingTitle.Id ?? DBNull.Value;
            dr["jsonb"] = (object)precedingSucceedingTitle.Content ?? DBNull.Value;
            dr["creation_date"] = (object)precedingSucceedingTitle.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)precedingSucceedingTitle.CreationUserId ?? DBNull.Value;
            dr["precedinginstanceid"] = (object)precedingSucceedingTitle.Precedinginstanceid ?? DBNull.Value;
            dr["succeedinginstanceid"] = (object)precedingSucceedingTitle.Succeedinginstanceid ?? DBNull.Value;
            precedingSucceedingTitlesDataTable.Rows.Add(dr);
        }

        public void Insert(Prefix prefix)
        {
            if (prefixesDataTable == null)
            {
                prefixesDataTable = new DataTable();
                prefixesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                prefixesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                prefixesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = prefixesDataTable.NewRow();
            dr["id"] = (object)prefix.Id ?? DBNull.Value;
            dr["jsonb"] = (object)prefix.Content ?? DBNull.Value;
            prefixesDataTable.Rows.Add(dr);
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

        public void Insert(RawRecord rawRecord)
        {
            if (rawRecordsDataTable == null)
            {
                rawRecordsDataTable = new DataTable();
                rawRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                rawRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                rawRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                rawRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                rawRecordsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = rawRecordsDataTable.NewRow();
            dr["id"] = (object)rawRecord.Id ?? DBNull.Value;
            dr["jsonb"] = (object)rawRecord.Content ?? DBNull.Value;
            dr["creation_date"] = (object)rawRecord.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)rawRecord.CreationUserId ?? DBNull.Value;
            rawRecordsDataTable.Rows.Add(dr);
        }

        public void Insert(ReasonsForClosure reasonsForClosure)
        {
            if (reasonsForClosuresDataTable == null)
            {
                reasonsForClosuresDataTable = new DataTable();
                reasonsForClosuresDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                reasonsForClosuresDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                reasonsForClosuresDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = reasonsForClosuresDataTable.NewRow();
            dr["id"] = (object)reasonsForClosure.Id ?? DBNull.Value;
            dr["jsonb"] = (object)reasonsForClosure.Content ?? DBNull.Value;
            reasonsForClosuresDataTable.Rows.Add(dr);
        }

        public void Insert(Record record)
        {
            if (recordsDataTable == null)
            {
                recordsDataTable = new DataTable();
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "jobexecutionid", DataType = typeof(Guid) });
                recordsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = recordsDataTable.NewRow();
            dr["id"] = (object)record.Id ?? DBNull.Value;
            dr["jsonb"] = (object)record.Content ?? DBNull.Value;
            dr["creation_date"] = (object)record.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)record.CreationUserId ?? DBNull.Value;
            dr["jobexecutionid"] = (object)record.Jobexecutionid ?? DBNull.Value;
            recordsDataTable.Rows.Add(dr);
        }

        public void Insert(RefundReason refundReason)
        {
            if (refundReasonsDataTable == null)
            {
                refundReasonsDataTable = new DataTable();
                refundReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                refundReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                refundReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                refundReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                refundReasonsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = refundReasonsDataTable.NewRow();
            dr["id"] = (object)refundReason.Id ?? DBNull.Value;
            dr["jsonb"] = (object)refundReason.Content ?? DBNull.Value;
            dr["creation_date"] = (object)refundReason.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)refundReason.CreationUserId ?? DBNull.Value;
            refundReasonsDataTable.Rows.Add(dr);
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
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                requestsDataTable.Columns.Add(new DataColumn { ColumnName = "cancellationreasonid", DataType = typeof(Guid) });
                requestsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = requestsDataTable.NewRow();
            dr["id"] = (object)request.Id ?? DBNull.Value;
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
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                requestPoliciesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                requestPoliciesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = requestPoliciesDataTable.NewRow();
            dr["id"] = (object)requestPolicy.Id ?? DBNull.Value;
            dr["jsonb"] = (object)requestPolicy.Content ?? DBNull.Value;
            dr["creation_date"] = (object)requestPolicy.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)requestPolicy.CreationUserId ?? DBNull.Value;
            requestPoliciesDataTable.Rows.Add(dr);
        }

        public void Insert(ScheduledNotice scheduledNotice)
        {
            if (scheduledNoticesDataTable == null)
            {
                scheduledNoticesDataTable = new DataTable();
                scheduledNoticesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                scheduledNoticesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                scheduledNoticesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                scheduledNoticesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                scheduledNoticesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = scheduledNoticesDataTable.NewRow();
            dr["id"] = (object)scheduledNotice.Id ?? DBNull.Value;
            dr["jsonb"] = (object)scheduledNotice.Content ?? DBNull.Value;
            dr["creation_date"] = (object)scheduledNotice.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)scheduledNotice.CreationUserId ?? DBNull.Value;
            scheduledNoticesDataTable.Rows.Add(dr);
        }

        public void Insert(ServicePoint servicePoint)
        {
            if (servicePointsDataTable == null)
            {
                servicePointsDataTable = new DataTable();
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                servicePointsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                servicePointsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = servicePointsDataTable.NewRow();
            dr["id"] = (object)servicePoint.Id ?? DBNull.Value;
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
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                servicePointUsersDataTable.Columns.Add(new DataColumn { ColumnName = "defaultservicepointid", DataType = typeof(Guid) });
                servicePointUsersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = servicePointUsersDataTable.NewRow();
            dr["id"] = (object)servicePointUser.Id ?? DBNull.Value;
            dr["jsonb"] = (object)servicePointUser.Content ?? DBNull.Value;
            dr["creation_date"] = (object)servicePointUser.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)servicePointUser.CreationUserId ?? DBNull.Value;
            dr["defaultservicepointid"] = (object)servicePointUser.Defaultservicepointid ?? DBNull.Value;
            servicePointUsersDataTable.Rows.Add(dr);
        }

        public void Insert(Snapshot snapshot)
        {
            if (snapshotsDataTable == null)
            {
                snapshotsDataTable = new DataTable();
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                snapshotsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = snapshotsDataTable.NewRow();
            dr["id"] = (object)snapshot.Id ?? DBNull.Value;
            dr["jsonb"] = (object)snapshot.Content ?? DBNull.Value;
            dr["creation_date"] = (object)snapshot.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)snapshot.CreationUserId ?? DBNull.Value;
            snapshotsDataTable.Rows.Add(dr);
        }

        public void Insert(StaffSlip staffSlip)
        {
            if (staffSlipsDataTable == null)
            {
                staffSlipsDataTable = new DataTable();
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                staffSlipsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                staffSlipsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = staffSlipsDataTable.NewRow();
            dr["id"] = (object)staffSlip.Id ?? DBNull.Value;
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
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                statisticalCodesDataTable.Columns.Add(new DataColumn { ColumnName = "statisticalcodetypeid", DataType = typeof(Guid) });
                statisticalCodesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = statisticalCodesDataTable.NewRow();
            dr["id"] = (object)statisticalCode.Id ?? DBNull.Value;
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
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                statisticalCodeTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                statisticalCodeTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = statisticalCodeTypesDataTable.NewRow();
            dr["id"] = (object)statisticalCodeType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)statisticalCodeType.Content ?? DBNull.Value;
            dr["creation_date"] = (object)statisticalCodeType.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)statisticalCodeType.CreationUserId ?? DBNull.Value;
            statisticalCodeTypesDataTable.Rows.Add(dr);
        }

        public void Insert(Suffix suffix)
        {
            if (suffixesDataTable == null)
            {
                suffixesDataTable = new DataTable();
                suffixesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                suffixesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                suffixesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = suffixesDataTable.NewRow();
            dr["id"] = (object)suffix.Id ?? DBNull.Value;
            dr["jsonb"] = (object)suffix.Content ?? DBNull.Value;
            suffixesDataTable.Rows.Add(dr);
        }

        public void Insert(Tag tag)
        {
            if (tagsDataTable == null)
            {
                tagsDataTable = new DataTable();
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                tagsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = tagsDataTable.NewRow();
            dr["id"] = (object)tag.Id ?? DBNull.Value;
            dr["jsonb"] = (object)tag.Content ?? DBNull.Value;
            dr["creation_date"] = (object)tag.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)tag.CreationUserId ?? DBNull.Value;
            tagsDataTable.Rows.Add(dr);
        }

        public void Insert(Template template)
        {
            if (templatesDataTable == null)
            {
                templatesDataTable = new DataTable();
                templatesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                templatesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                templatesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                templatesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                templatesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = templatesDataTable.NewRow();
            dr["id"] = (object)template.Id ?? DBNull.Value;
            dr["jsonb"] = (object)template.Content ?? DBNull.Value;
            dr["creation_date"] = (object)template.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)template.CreationUserId ?? DBNull.Value;
            templatesDataTable.Rows.Add(dr);
        }

        public void Insert(Title title)
        {
            if (titlesDataTable == null)
            {
                titlesDataTable = new DataTable();
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "polineid", DataType = typeof(Guid) });
                titlesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = titlesDataTable.NewRow();
            dr["id"] = (object)title.Id ?? DBNull.Value;
            dr["jsonb"] = (object)title.Content ?? DBNull.Value;
            dr["polineid"] = (object)title.Polineid ?? DBNull.Value;
            titlesDataTable.Rows.Add(dr);
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
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "fromfundid", DataType = typeof(Guid) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "sourcefiscalyearid", DataType = typeof(Guid) });
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "tofundid", DataType = typeof(Guid) });
                transactionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = transactionsDataTable.NewRow();
            dr["id"] = (object)transaction.Id ?? DBNull.Value;
            dr["jsonb"] = (object)transaction.Content ?? DBNull.Value;
            dr["creation_date"] = (object)transaction.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)transaction.CreationUserId ?? DBNull.Value;
            dr["fiscalyearid"] = (object)transaction.Fiscalyearid ?? DBNull.Value;
            dr["fromfundid"] = (object)transaction.Fromfundid ?? DBNull.Value;
            dr["sourcefiscalyearid"] = (object)transaction.Sourcefiscalyearid ?? DBNull.Value;
            dr["tofundid"] = (object)transaction.Tofundid ?? DBNull.Value;
            transactionsDataTable.Rows.Add(dr);
        }

        public void Insert(TransferAccount transferAccount)
        {
            if (transferAccountsDataTable == null)
            {
                transferAccountsDataTable = new DataTable();
                transferAccountsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                transferAccountsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                transferAccountsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                transferAccountsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                transferAccountsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = transferAccountsDataTable.NewRow();
            dr["id"] = (object)transferAccount.Id ?? DBNull.Value;
            dr["jsonb"] = (object)transferAccount.Content ?? DBNull.Value;
            dr["creation_date"] = (object)transferAccount.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)transferAccount.CreationUserId ?? DBNull.Value;
            transferAccountsDataTable.Rows.Add(dr);
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

        public void Insert(User user)
        {
            if (usersDataTable == null)
            {
                usersDataTable = new DataTable();
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                usersDataTable.Columns.Add(new DataColumn { ColumnName = "patrongroup", DataType = typeof(Guid) });
                usersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = usersDataTable.NewRow();
            dr["id"] = (object)user.Id ?? DBNull.Value;
            dr["jsonb"] = (object)user.Content ?? DBNull.Value;
            dr["creation_date"] = (object)user.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)user.CreationUserId ?? DBNull.Value;
            dr["patrongroup"] = (object)user.Patrongroup ?? DBNull.Value;
            usersDataTable.Rows.Add(dr);
        }

        public void Insert(UserAcquisitionsUnit userAcquisitionsUnit)
        {
            if (userAcquisitionsUnitsDataTable == null)
            {
                userAcquisitionsUnitsDataTable = new DataTable();
                userAcquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                userAcquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                userAcquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                userAcquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                userAcquisitionsUnitsDataTable.Columns.Add(new DataColumn { ColumnName = "acquisitionsunitid", DataType = typeof(Guid) });
                userAcquisitionsUnitsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = userAcquisitionsUnitsDataTable.NewRow();
            dr["id"] = (object)userAcquisitionsUnit.Id ?? DBNull.Value;
            dr["jsonb"] = (object)userAcquisitionsUnit.Content ?? DBNull.Value;
            dr["creation_date"] = (object)userAcquisitionsUnit.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)userAcquisitionsUnit.CreationUserId ?? DBNull.Value;
            dr["acquisitionsunitid"] = (object)userAcquisitionsUnit.Acquisitionsunitid ?? DBNull.Value;
            userAcquisitionsUnitsDataTable.Rows.Add(dr);
        }

        public void Insert(UserRequestPreference userRequestPreference)
        {
            if (userRequestPreferencesDataTable == null)
            {
                userRequestPreferencesDataTable = new DataTable();
                userRequestPreferencesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                userRequestPreferencesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                userRequestPreferencesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                userRequestPreferencesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                userRequestPreferencesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = userRequestPreferencesDataTable.NewRow();
            dr["id"] = (object)userRequestPreference.Id ?? DBNull.Value;
            dr["jsonb"] = (object)userRequestPreference.Content ?? DBNull.Value;
            dr["creation_date"] = (object)userRequestPreference.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)userRequestPreference.CreationUserId ?? DBNull.Value;
            userRequestPreferencesDataTable.Rows.Add(dr);
        }

        public void Insert(Voucher voucher)
        {
            if (vouchersDataTable == null)
            {
                vouchersDataTable = new DataTable();
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "invoiceid", DataType = typeof(Guid) });
                vouchersDataTable.Columns.Add(new DataColumn { ColumnName = "batchgroupid", DataType = typeof(Guid) });
                vouchersDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = vouchersDataTable.NewRow();
            dr["id"] = (object)voucher.Id ?? DBNull.Value;
            dr["jsonb"] = (object)voucher.Content ?? DBNull.Value;
            dr["creation_date"] = (object)voucher.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)voucher.CreationUserId ?? DBNull.Value;
            dr["invoiceid"] = (object)voucher.Invoiceid ?? DBNull.Value;
            dr["batchgroupid"] = (object)voucher.Batchgroupid ?? DBNull.Value;
            vouchersDataTable.Rows.Add(dr);
        }

        public void Insert(VoucherItem voucherItem)
        {
            if (voucherItemsDataTable == null)
            {
                voucherItemsDataTable = new DataTable();
                voucherItemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                voucherItemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                voucherItemsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                voucherItemsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                voucherItemsDataTable.Columns.Add(new DataColumn { ColumnName = "voucherid", DataType = typeof(Guid) });
                voucherItemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = voucherItemsDataTable.NewRow();
            dr["id"] = (object)voucherItem.Id ?? DBNull.Value;
            dr["jsonb"] = (object)voucherItem.Content ?? DBNull.Value;
            dr["creation_date"] = (object)voucherItem.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)voucherItem.CreationUserId ?? DBNull.Value;
            dr["voucherid"] = (object)voucherItem.Voucherid ?? DBNull.Value;
            voucherItemsDataTable.Rows.Add(dr);
        }

        public void Insert(WaiveReason waiveReason)
        {
            if (waiveReasonsDataTable == null)
            {
                waiveReasonsDataTable = new DataTable();
                waiveReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                waiveReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                waiveReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                waiveReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                waiveReasonsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = waiveReasonsDataTable.NewRow();
            dr["id"] = (object)waiveReason.Id ?? DBNull.Value;
            dr["jsonb"] = (object)waiveReason.Content ?? DBNull.Value;
            dr["creation_date"] = (object)waiveReason.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)waiveReason.CreationUserId ?? DBNull.Value;
            waiveReasonsDataTable.Rows.Add(dr);
        }

        public void Commit()
        {
            if (acquisitionsUnitsDataTable != null && acquisitionsUnitsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(acquisitionsUnitsDataTable);
                acquisitionsUnitsDataTable.Clear();
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
            if (alternativeTitleTypesDataTable != null && alternativeTitleTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(auditLoansDataTable);
                auditLoansDataTable.Clear();
            }
            if (authAttemptsDataTable != null && authAttemptsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(authPasswordActionsDataTable);
                authPasswordActionsDataTable.Clear();
            }
            if (batchGroupsDataTable != null && batchGroupsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(batchGroupsDataTable);
                batchGroupsDataTable.Clear();
            }
            if (batchVouchersDataTable != null && batchVouchersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(batchVouchersDataTable);
                batchVouchersDataTable.Clear();
            }
            if (batchVoucherExportsDataTable != null && batchVoucherExportsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("batchgroupid", "batchgroupid");
                sqlBulkCopy.ColumnMappings.Add("batchvoucherid", "batchvoucherid");
                sqlBulkCopy.WriteToServer(batchVoucherExportsDataTable);
                batchVoucherExportsDataTable.Clear();
            }
            if (batchVoucherExportConfigsDataTable != null && batchVoucherExportConfigsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("batchgroupid", "batchgroupid");
                sqlBulkCopy.WriteToServer(batchVoucherExportConfigsDataTable);
                batchVoucherExportConfigsDataTable.Clear();
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
                sqlBulkCopy.ColumnMappings.Add("fundid", "fundid");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.WriteToServer(budgetsDataTable);
                budgetsDataTable.Clear();
            }
            if (callNumberTypesDataTable != null && callNumberTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(cancellationReasonsDataTable);
                cancellationReasonsDataTable.Clear();
            }
            if (categoriesDataTable != null && categoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(categoriesDataTable);
                categoriesDataTable.Clear();
            }
            if (checkInsDataTable != null && checkInsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(checkInsDataTable);
                checkInsDataTable.Clear();
            }
            if (circulationRulesDataTable != null && circulationRulesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("lock", "lock");
                sqlBulkCopy.WriteToServer(circulationRulesDataTable);
                circulationRulesDataTable.Clear();
            }
            if (classificationTypesDataTable != null && classificationTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
            if (configurationsDataTable != null && configurationsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_configuration{(IsMySql ? "_" : ".")}config_data";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(configurationsDataTable);
                configurationsDataTable.Clear();
            }
            if (contactsDataTable != null && contactsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(contactsDataTable);
                contactsDataTable.Clear();
            }
            if (contributorNameTypesDataTable != null && contributorNameTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(contributorTypesDataTable);
                contributorTypesDataTable.Clear();
            }
            if (countriesDataTable != null && countriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}countries";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("alpha2_code", "alpha2_code");
                sqlBulkCopy.ColumnMappings.Add("alpha3_code", "alpha3_code");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.WriteToServer(countriesDataTable);
                countriesDataTable.Clear();
            }
            if (customFieldsDataTable != null && customFieldsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}custom_fields";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(customFieldsDataTable);
                customFieldsDataTable.Clear();
            }
            if (documentsDataTable != null && documentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("invoiceid", "invoiceid");
                sqlBulkCopy.ColumnMappings.Add("document_data", "document_data");
                sqlBulkCopy.WriteToServer(documentsDataTable);
                documentsDataTable.Clear();
            }
            if (electronicAccessRelationshipsDataTable != null && electronicAccessRelationshipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(electronicAccessRelationshipsDataTable);
                electronicAccessRelationshipsDataTable.Clear();
            }
            if (errorRecordsDataTable != null && errorRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(errorRecordsDataTable);
                errorRecordsDataTable.Clear();
            }
            if (eventLogsDataTable != null && eventLogsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_login{(IsMySql ? "_" : ".")}event_logs";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(eventLogsDataTable);
                eventLogsDataTable.Clear();
            }
            if (exportConfigCredentialsDataTable != null && exportConfigCredentialsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("exportconfigid", "exportconfigid");
                sqlBulkCopy.WriteToServer(exportConfigCredentialsDataTable);
                exportConfigCredentialsDataTable.Clear();
            }
            if (feesDataTable != null && feesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(feesDataTable);
                feesDataTable.Clear();
            }
            if (feeTypesDataTable != null && feeTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ownerid", "ownerid");
                sqlBulkCopy.WriteToServer(feeTypesDataTable);
                feeTypesDataTable.Clear();
            }
            if (financeGroupsDataTable != null && financeGroupsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(financeGroupsDataTable);
                financeGroupsDataTable.Clear();
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("ledgerid", "ledgerid");
                sqlBulkCopy.ColumnMappings.Add("fundtypeid", "fundtypeid");
                sqlBulkCopy.WriteToServer(fundsDataTable);
                fundsDataTable.Clear();
            }
            if (fundTypesDataTable != null && fundTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(fundTypesDataTable);
                fundTypesDataTable.Clear();
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
            if (groupFundFiscalYearsDataTable != null && groupFundFiscalYearsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("budgetid", "budgetid");
                sqlBulkCopy.ColumnMappings.Add("groupid", "groupid");
                sqlBulkCopy.ColumnMappings.Add("fundid", "fundid");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.WriteToServer(groupFundFiscalYearsDataTable);
                groupFundFiscalYearsDataTable.Clear();
            }
            if (holdingsDataTable != null && holdingsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(holdingTypesDataTable);
                holdingTypesDataTable.Clear();
            }
            if (hridSettingsDataTable != null && hridSettingsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("lock", "lock");
                sqlBulkCopy.WriteToServer(hridSettingsDataTable);
                hridSettingsDataTable.Clear();
            }
            if (idTypesDataTable != null && idTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("instancestatusid", "instancestatusid");
                sqlBulkCopy.ColumnMappings.Add("modeofissuanceid", "modeofissuanceid");
                sqlBulkCopy.ColumnMappings.Add("instancetypeid", "instancetypeid");
                sqlBulkCopy.WriteToServer(instancesDataTable);
                instancesDataTable.Clear();
            }
            if (instanceFormatsDataTable != null && instanceFormatsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceFormatsDataTable);
                instanceFormatsDataTable.Clear();
            }
            if (instanceNoteTypesDataTable != null && instanceNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(instanceNoteTypesDataTable);
                instanceNoteTypesDataTable.Clear();
            }
            if (instanceRelationshipsDataTable != null && instanceRelationshipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceTypesDataTable);
                instanceTypesDataTable.Clear();
            }
            if (institutionsDataTable != null && institutionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(institutionsDataTable);
                institutionsDataTable.Clear();
            }
            if (interfacesDataTable != null && interfacesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(interfacesDataTable);
                interfacesDataTable.Clear();
            }
            if (interfaceCredentialsDataTable != null && interfaceCredentialsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("interfaceid", "interfaceid");
                sqlBulkCopy.WriteToServer(interfaceCredentialsDataTable);
                interfaceCredentialsDataTable.Clear();
            }
            if (invoicesDataTable != null && invoicesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("batchgroupid", "batchgroupid");
                sqlBulkCopy.WriteToServer(invoicesDataTable);
                invoicesDataTable.Clear();
            }
            if (invoiceItemsDataTable != null && invoiceItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("invoiceid", "invoiceid");
                sqlBulkCopy.WriteToServer(invoiceItemsDataTable);
                invoiceItemsDataTable.Clear();
            }
            if (invoiceTransactionSummariesDataTable != null && invoiceTransactionSummariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(invoiceTransactionSummariesDataTable);
                invoiceTransactionSummariesDataTable.Clear();
            }
            if (itemsDataTable != null && itemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("holdingsrecordid", "holdingsrecordid");
                sqlBulkCopy.ColumnMappings.Add("permanentloantypeid", "permanentloantypeid");
                sqlBulkCopy.ColumnMappings.Add("temporaryloantypeid", "temporaryloantypeid");
                sqlBulkCopy.ColumnMappings.Add("materialtypeid", "materialtypeid");
                sqlBulkCopy.ColumnMappings.Add("permanentlocationid", "permanentlocationid");
                sqlBulkCopy.ColumnMappings.Add("temporarylocationid", "temporarylocationid");
                sqlBulkCopy.ColumnMappings.Add("effectivelocationid", "effectivelocationid");
                sqlBulkCopy.WriteToServer(itemsDataTable);
                itemsDataTable.Clear();
            }
            if (itemDamagedStatusesDataTable != null && itemDamagedStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(itemDamagedStatusesDataTable);
                itemDamagedStatusesDataTable.Clear();
            }
            if (itemNoteTypesDataTable != null && itemNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(itemNoteTypesDataTable);
                itemNoteTypesDataTable.Clear();
            }
            if (jobExecutionsDataTable != null && jobExecutionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(jobExecutionsDataTable);
                jobExecutionsDataTable.Clear();
            }
            if (jobExecutionProgressesDataTable != null && jobExecutionProgressesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("jobexecutionid", "jobexecutionid");
                sqlBulkCopy.WriteToServer(jobExecutionProgressesDataTable);
                jobExecutionProgressesDataTable.Clear();
            }
            if (jobExecutionSourceChunksDataTable != null && jobExecutionSourceChunksDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("jobexecutionid", "jobexecutionid");
                sqlBulkCopy.WriteToServer(jobExecutionSourceChunksDataTable);
                jobExecutionSourceChunksDataTable.Clear();
            }
            if (journalRecordsDataTable != null && journalRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("job_execution_id", "job_execution_id");
                sqlBulkCopy.ColumnMappings.Add("source_id", "source_id");
                sqlBulkCopy.ColumnMappings.Add("source_record_order", "source_record_order");
                sqlBulkCopy.ColumnMappings.Add("entity_type", "entity_type");
                sqlBulkCopy.ColumnMappings.Add("entity_id", "entity_id");
                sqlBulkCopy.ColumnMappings.Add("entity_hrid", "entity_hrid");
                sqlBulkCopy.ColumnMappings.Add("action_type", "action_type");
                sqlBulkCopy.ColumnMappings.Add("action_status", "action_status");
                sqlBulkCopy.ColumnMappings.Add("error", "error");
                sqlBulkCopy.ColumnMappings.Add("action_date", "action_date");
                sqlBulkCopy.WriteToServer(journalRecordsDataTable);
                journalRecordsDataTable.Clear();
            }
            if (ledgersDataTable != null && ledgersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearoneid", "fiscalyearoneid");
                sqlBulkCopy.WriteToServer(ledgersDataTable);
                ledgersDataTable.Clear();
            }
            if (ledgerFiscalYearsDataTable != null && ledgerFiscalYearsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledgerfy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("ledgerid", "ledgerid");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.WriteToServer(ledgerFiscalYearsDataTable);
                ledgerFiscalYearsDataTable.Clear();
            }
            if (librariesDataTable != null && librariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(loginsDataTable);
                loginsDataTable.Clear();
            }
            if (lostItemFeePoliciesDataTable != null && lostItemFeePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(lostItemFeePoliciesDataTable);
                lostItemFeePoliciesDataTable.Clear();
            }
            if (mappingRulesDataTable != null && mappingRulesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(mappingRulesDataTable);
                mappingRulesDataTable.Clear();
            }
            if (marcRecordsDataTable != null && marcRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(marcRecordsDataTable);
                marcRecordsDataTable.Clear();
            }
            if (materialTypesDataTable != null && materialTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(modeOfIssuancesDataTable);
                modeOfIssuancesDataTable.Clear();
            }
            if (natureOfContentTermsDataTable != null && natureOfContentTermsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(natureOfContentTermsDataTable);
                natureOfContentTermsDataTable.Clear();
            }
            if (notesDataTable != null && notesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_notes{(IsMySql ? "_" : ".")}note_data";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("temporary_type_id", "temporary_type_id");
                sqlBulkCopy.WriteToServer(notesDataTable);
                notesDataTable.Clear();
            }
            if (noteTypesDataTable != null && noteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_notes{(IsMySql ? "_" : ".")}note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(noteTypesDataTable);
                noteTypesDataTable.Clear();
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
            if (orderInvoicesDataTable != null && orderInvoicesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("purchaseorderid", "purchaseorderid");
                sqlBulkCopy.WriteToServer(orderInvoicesDataTable);
                orderInvoicesDataTable.Clear();
            }
            if (orderItemsDataTable != null && orderItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("purchaseorderid", "purchaseorderid");
                sqlBulkCopy.WriteToServer(orderItemsDataTable);
                orderItemsDataTable.Clear();
            }
            if (orderTemplatesDataTable != null && orderTemplatesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(orderTemplatesDataTable);
                orderTemplatesDataTable.Clear();
            }
            if (orderTransactionSummariesDataTable != null && orderTransactionSummariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(orderTransactionSummariesDataTable);
                orderTransactionSummariesDataTable.Clear();
            }
            if (organizationsDataTable != null && organizationsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(organizationsDataTable);
                organizationsDataTable.Clear();
            }
            if (overdueFinePoliciesDataTable != null && overdueFinePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(overdueFinePoliciesDataTable);
                overdueFinePoliciesDataTable.Clear();
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
            if (patronActionSessionsDataTable != null && patronActionSessionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronActionSessionsDataTable);
                patronActionSessionsDataTable.Clear();
            }
            if (patronBlockConditionsDataTable != null && patronBlockConditionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}patron_block_conditions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronBlockConditionsDataTable);
                patronBlockConditionsDataTable.Clear();
            }
            if (patronBlockLimitsDataTable != null && patronBlockLimitsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}patron_block_limits";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronBlockLimitsDataTable);
                patronBlockLimitsDataTable.Clear();
            }
            if (patronNoticePoliciesDataTable != null && patronNoticePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronNoticePoliciesDataTable);
                patronNoticePoliciesDataTable.Clear();
            }
            if (paymentsDataTable != null && paymentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(paymentsDataTable);
                paymentsDataTable.Clear();
            }
            if (paymentMethodsDataTable != null && paymentMethodsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}payments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(paymentMethodsDataTable);
                paymentMethodsDataTable.Clear();
            }
            if (permissionsDataTable != null && permissionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_permissions{(IsMySql ? "_" : ".")}permissions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(permissionsUsersDataTable);
                permissionsUsersDataTable.Clear();
            }
            if (piecesDataTable != null && piecesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("polineid", "polineid");
                sqlBulkCopy.WriteToServer(piecesDataTable);
                piecesDataTable.Clear();
            }
            if (precedingSucceedingTitlesDataTable != null && precedingSucceedingTitlesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("precedinginstanceid", "precedinginstanceid");
                sqlBulkCopy.ColumnMappings.Add("succeedinginstanceid", "succeedinginstanceid");
                sqlBulkCopy.WriteToServer(precedingSucceedingTitlesDataTable);
                precedingSucceedingTitlesDataTable.Clear();
            }
            if (prefixesDataTable != null && prefixesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(prefixesDataTable);
                prefixesDataTable.Clear();
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
            if (rawRecordsDataTable != null && rawRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(rawRecordsDataTable);
                rawRecordsDataTable.Clear();
            }
            if (reasonsForClosuresDataTable != null && reasonsForClosuresDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(reasonsForClosuresDataTable);
                reasonsForClosuresDataTable.Clear();
            }
            if (recordsDataTable != null && recordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("jobexecutionid", "jobexecutionid");
                sqlBulkCopy.WriteToServer(recordsDataTable);
                recordsDataTable.Clear();
            }
            if (refundReasonsDataTable != null && refundReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(refundReasonsDataTable);
                refundReasonsDataTable.Clear();
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(requestPoliciesDataTable);
                requestPoliciesDataTable.Clear();
            }
            if (scheduledNoticesDataTable != null && scheduledNoticesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(scheduledNoticesDataTable);
                scheduledNoticesDataTable.Clear();
            }
            if (servicePointsDataTable != null && servicePointsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("defaultservicepointid", "defaultservicepointid");
                sqlBulkCopy.WriteToServer(servicePointUsersDataTable);
                servicePointUsersDataTable.Clear();
            }
            if (snapshotsDataTable != null && snapshotsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(snapshotsDataTable);
                snapshotsDataTable.Clear();
            }
            if (staffSlipsDataTable != null && staffSlipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(statisticalCodeTypesDataTable);
                statisticalCodeTypesDataTable.Clear();
            }
            if (suffixesDataTable != null && suffixesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(suffixesDataTable);
                suffixesDataTable.Clear();
            }
            if (tagsDataTable != null && tagsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_tags{(IsMySql ? "_" : ".")}tags";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(tagsDataTable);
                tagsDataTable.Clear();
            }
            if (templatesDataTable != null && templatesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_template_engine{(IsMySql ? "_" : ".")}template";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(templatesDataTable);
                templatesDataTable.Clear();
            }
            if (titlesDataTable != null && titlesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("polineid", "polineid");
                sqlBulkCopy.WriteToServer(titlesDataTable);
                titlesDataTable.Clear();
            }
            if (transactionsDataTable != null && transactionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.ColumnMappings.Add("fromfundid", "fromfundid");
                sqlBulkCopy.ColumnMappings.Add("sourcefiscalyearid", "sourcefiscalyearid");
                sqlBulkCopy.ColumnMappings.Add("tofundid", "tofundid");
                sqlBulkCopy.WriteToServer(transactionsDataTable);
                transactionsDataTable.Clear();
            }
            if (transferAccountsDataTable != null && transferAccountsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(transferAccountsDataTable);
                transferAccountsDataTable.Clear();
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
            if (usersDataTable != null && usersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_users{(IsMySql ? "_" : ".")}users";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("patrongroup", "patrongroup");
                sqlBulkCopy.WriteToServer(usersDataTable);
                usersDataTable.Clear();
            }
            if (userAcquisitionsUnitsDataTable != null && userAcquisitionsUnitsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("acquisitionsunitid", "acquisitionsunitid");
                sqlBulkCopy.WriteToServer(userAcquisitionsUnitsDataTable);
                userAcquisitionsUnitsDataTable.Clear();
            }
            if (userRequestPreferencesDataTable != null && userRequestPreferencesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(userRequestPreferencesDataTable);
                userRequestPreferencesDataTable.Clear();
            }
            if (vouchersDataTable != null && vouchersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("invoiceid", "invoiceid");
                sqlBulkCopy.ColumnMappings.Add("batchgroupid", "batchgroupid");
                sqlBulkCopy.WriteToServer(vouchersDataTable);
                vouchersDataTable.Clear();
            }
            if (voucherItemsDataTable != null && voucherItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("voucherid", "voucherid");
                sqlBulkCopy.WriteToServer(voucherItemsDataTable);
                voucherItemsDataTable.Clear();
            }
            if (waiveReasonsDataTable != null && waiveReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"diku_mod_feesfines{(IsMySql ? "_" : ".")}waives";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(waiveReasonsDataTable);
                waiveReasonsDataTable.Clear();
            }
        }

        public void Dispose()
        {
            if (sqlBulkCopy != null) sqlBulkCopy.Close();
            if (acquisitionsUnitsDataTable != null) acquisitionsUnitsDataTable.Dispose();
            if (addressTypesDataTable != null) addressTypesDataTable.Dispose();
            if (alertsDataTable != null) alertsDataTable.Dispose();
            if (alternativeTitleTypesDataTable != null) alternativeTitleTypesDataTable.Dispose();
            if (auditLoansDataTable != null) auditLoansDataTable.Dispose();
            if (authAttemptsDataTable != null) authAttemptsDataTable.Dispose();
            if (authCredentialsHistoriesDataTable != null) authCredentialsHistoriesDataTable.Dispose();
            if (authPasswordActionsDataTable != null) authPasswordActionsDataTable.Dispose();
            if (batchGroupsDataTable != null) batchGroupsDataTable.Dispose();
            if (batchVouchersDataTable != null) batchVouchersDataTable.Dispose();
            if (batchVoucherExportsDataTable != null) batchVoucherExportsDataTable.Dispose();
            if (batchVoucherExportConfigsDataTable != null) batchVoucherExportConfigsDataTable.Dispose();
            if (blocksDataTable != null) blocksDataTable.Dispose();
            if (budgetsDataTable != null) budgetsDataTable.Dispose();
            if (callNumberTypesDataTable != null) callNumberTypesDataTable.Dispose();
            if (campusesDataTable != null) campusesDataTable.Dispose();
            if (cancellationReasonsDataTable != null) cancellationReasonsDataTable.Dispose();
            if (categoriesDataTable != null) categoriesDataTable.Dispose();
            if (checkInsDataTable != null) checkInsDataTable.Dispose();
            if (circulationRulesDataTable != null) circulationRulesDataTable.Dispose();
            if (classificationTypesDataTable != null) classificationTypesDataTable.Dispose();
            if (commentsDataTable != null) commentsDataTable.Dispose();
            if (configurationsDataTable != null) configurationsDataTable.Dispose();
            if (contactsDataTable != null) contactsDataTable.Dispose();
            if (contributorNameTypesDataTable != null) contributorNameTypesDataTable.Dispose();
            if (contributorTypesDataTable != null) contributorTypesDataTable.Dispose();
            if (countriesDataTable != null) countriesDataTable.Dispose();
            if (customFieldsDataTable != null) customFieldsDataTable.Dispose();
            if (documentsDataTable != null) documentsDataTable.Dispose();
            if (electronicAccessRelationshipsDataTable != null) electronicAccessRelationshipsDataTable.Dispose();
            if (errorRecordsDataTable != null) errorRecordsDataTable.Dispose();
            if (eventLogsDataTable != null) eventLogsDataTable.Dispose();
            if (exportConfigCredentialsDataTable != null) exportConfigCredentialsDataTable.Dispose();
            if (feesDataTable != null) feesDataTable.Dispose();
            if (feeTypesDataTable != null) feeTypesDataTable.Dispose();
            if (financeGroupsDataTable != null) financeGroupsDataTable.Dispose();
            if (fiscalYearsDataTable != null) fiscalYearsDataTable.Dispose();
            if (fixedDueDateSchedulesDataTable != null) fixedDueDateSchedulesDataTable.Dispose();
            if (fundsDataTable != null) fundsDataTable.Dispose();
            if (fundTypesDataTable != null) fundTypesDataTable.Dispose();
            if (groupsDataTable != null) groupsDataTable.Dispose();
            if (groupFundFiscalYearsDataTable != null) groupFundFiscalYearsDataTable.Dispose();
            if (holdingsDataTable != null) holdingsDataTable.Dispose();
            if (holdingNoteTypesDataTable != null) holdingNoteTypesDataTable.Dispose();
            if (holdingTypesDataTable != null) holdingTypesDataTable.Dispose();
            if (hridSettingsDataTable != null) hridSettingsDataTable.Dispose();
            if (idTypesDataTable != null) idTypesDataTable.Dispose();
            if (illPoliciesDataTable != null) illPoliciesDataTable.Dispose();
            if (instancesDataTable != null) instancesDataTable.Dispose();
            if (instanceFormatsDataTable != null) instanceFormatsDataTable.Dispose();
            if (instanceNoteTypesDataTable != null) instanceNoteTypesDataTable.Dispose();
            if (instanceRelationshipsDataTable != null) instanceRelationshipsDataTable.Dispose();
            if (instanceRelationshipTypesDataTable != null) instanceRelationshipTypesDataTable.Dispose();
            if (instanceSourceMarcsDataTable != null) instanceSourceMarcsDataTable.Dispose();
            if (instanceStatusesDataTable != null) instanceStatusesDataTable.Dispose();
            if (instanceTypesDataTable != null) instanceTypesDataTable.Dispose();
            if (institutionsDataTable != null) institutionsDataTable.Dispose();
            if (interfacesDataTable != null) interfacesDataTable.Dispose();
            if (interfaceCredentialsDataTable != null) interfaceCredentialsDataTable.Dispose();
            if (invoicesDataTable != null) invoicesDataTable.Dispose();
            if (invoiceItemsDataTable != null) invoiceItemsDataTable.Dispose();
            if (invoiceTransactionSummariesDataTable != null) invoiceTransactionSummariesDataTable.Dispose();
            if (itemsDataTable != null) itemsDataTable.Dispose();
            if (itemDamagedStatusesDataTable != null) itemDamagedStatusesDataTable.Dispose();
            if (itemNoteTypesDataTable != null) itemNoteTypesDataTable.Dispose();
            if (jobExecutionsDataTable != null) jobExecutionsDataTable.Dispose();
            if (jobExecutionProgressesDataTable != null) jobExecutionProgressesDataTable.Dispose();
            if (jobExecutionSourceChunksDataTable != null) jobExecutionSourceChunksDataTable.Dispose();
            if (journalRecordsDataTable != null) journalRecordsDataTable.Dispose();
            if (ledgersDataTable != null) ledgersDataTable.Dispose();
            if (ledgerFiscalYearsDataTable != null) ledgerFiscalYearsDataTable.Dispose();
            if (librariesDataTable != null) librariesDataTable.Dispose();
            if (loansDataTable != null) loansDataTable.Dispose();
            if (loanPoliciesDataTable != null) loanPoliciesDataTable.Dispose();
            if (loanTypesDataTable != null) loanTypesDataTable.Dispose();
            if (locationsDataTable != null) locationsDataTable.Dispose();
            if (loginsDataTable != null) loginsDataTable.Dispose();
            if (lostItemFeePoliciesDataTable != null) lostItemFeePoliciesDataTable.Dispose();
            if (mappingRulesDataTable != null) mappingRulesDataTable.Dispose();
            if (marcRecordsDataTable != null) marcRecordsDataTable.Dispose();
            if (materialTypesDataTable != null) materialTypesDataTable.Dispose();
            if (modeOfIssuancesDataTable != null) modeOfIssuancesDataTable.Dispose();
            if (natureOfContentTermsDataTable != null) natureOfContentTermsDataTable.Dispose();
            if (notesDataTable != null) notesDataTable.Dispose();
            if (noteTypesDataTable != null) noteTypesDataTable.Dispose();
            if (ordersDataTable != null) ordersDataTable.Dispose();
            if (orderInvoicesDataTable != null) orderInvoicesDataTable.Dispose();
            if (orderItemsDataTable != null) orderItemsDataTable.Dispose();
            if (orderTemplatesDataTable != null) orderTemplatesDataTable.Dispose();
            if (orderTransactionSummariesDataTable != null) orderTransactionSummariesDataTable.Dispose();
            if (organizationsDataTable != null) organizationsDataTable.Dispose();
            if (overdueFinePoliciesDataTable != null) overdueFinePoliciesDataTable.Dispose();
            if (ownersDataTable != null) ownersDataTable.Dispose();
            if (patronActionSessionsDataTable != null) patronActionSessionsDataTable.Dispose();
            if (patronBlockConditionsDataTable != null) patronBlockConditionsDataTable.Dispose();
            if (patronBlockLimitsDataTable != null) patronBlockLimitsDataTable.Dispose();
            if (patronNoticePoliciesDataTable != null) patronNoticePoliciesDataTable.Dispose();
            if (paymentsDataTable != null) paymentsDataTable.Dispose();
            if (paymentMethodsDataTable != null) paymentMethodsDataTable.Dispose();
            if (permissionsDataTable != null) permissionsDataTable.Dispose();
            if (permissionsUsersDataTable != null) permissionsUsersDataTable.Dispose();
            if (piecesDataTable != null) piecesDataTable.Dispose();
            if (precedingSucceedingTitlesDataTable != null) precedingSucceedingTitlesDataTable.Dispose();
            if (prefixesDataTable != null) prefixesDataTable.Dispose();
            if (proxiesDataTable != null) proxiesDataTable.Dispose();
            if (rawRecordsDataTable != null) rawRecordsDataTable.Dispose();
            if (reasonsForClosuresDataTable != null) reasonsForClosuresDataTable.Dispose();
            if (recordsDataTable != null) recordsDataTable.Dispose();
            if (refundReasonsDataTable != null) refundReasonsDataTable.Dispose();
            if (reportingCodesDataTable != null) reportingCodesDataTable.Dispose();
            if (requestsDataTable != null) requestsDataTable.Dispose();
            if (requestPoliciesDataTable != null) requestPoliciesDataTable.Dispose();
            if (scheduledNoticesDataTable != null) scheduledNoticesDataTable.Dispose();
            if (servicePointsDataTable != null) servicePointsDataTable.Dispose();
            if (servicePointUsersDataTable != null) servicePointUsersDataTable.Dispose();
            if (snapshotsDataTable != null) snapshotsDataTable.Dispose();
            if (staffSlipsDataTable != null) staffSlipsDataTable.Dispose();
            if (statisticalCodesDataTable != null) statisticalCodesDataTable.Dispose();
            if (statisticalCodeTypesDataTable != null) statisticalCodeTypesDataTable.Dispose();
            if (suffixesDataTable != null) suffixesDataTable.Dispose();
            if (tagsDataTable != null) tagsDataTable.Dispose();
            if (templatesDataTable != null) templatesDataTable.Dispose();
            if (titlesDataTable != null) titlesDataTable.Dispose();
            if (transactionsDataTable != null) transactionsDataTable.Dispose();
            if (transferAccountsDataTable != null) transferAccountsDataTable.Dispose();
            if (transferCriteriasDataTable != null) transferCriteriasDataTable.Dispose();
            if (usersDataTable != null) usersDataTable.Dispose();
            if (userAcquisitionsUnitsDataTable != null) userAcquisitionsUnitsDataTable.Dispose();
            if (userRequestPreferencesDataTable != null) userRequestPreferencesDataTable.Dispose();
            if (vouchersDataTable != null) vouchersDataTable.Dispose();
            if (voucherItemsDataTable != null) voucherItemsDataTable.Dispose();
            if (waiveReasonsDataTable != null) waiveReasonsDataTable.Dispose();
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
        public static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

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
        public static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

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
