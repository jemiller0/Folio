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
        private DataTable acquisitionMethodsDataTable, acquisitionsUnitsDataTable, actualCostRecordsDataTable, addressTypesDataTable, agreementsDataTable, agreementItemsDataTable, alertsDataTable, alternativeTitleTypesDataTable, authAttemptsDataTable, authCredentialsHistoriesDataTable, authPasswordActionsDataTable, batchGroupsDataTable, blocksDataTable, blockConditionsDataTable, blockLimitsDataTable, boundWithPartsDataTable, budgetsDataTable, budgetExpenseClassesDataTable, budgetGroupsDataTable, callNumberTypesDataTable, campusesDataTable, cancellationReasonsDataTable, categoriesDataTable, checkInsDataTable, circulationRulesDataTable, classificationTypesDataTable, closeReasonsDataTable, commentsDataTable, configurationsDataTable, contactsDataTable, contactTypesDataTable, contributorNameTypesDataTable, contributorTypesDataTable, countriesDataTable, customFieldsDataTable, departmentsDataTable, department3sDataTable, divisionsDataTable, documentsDataTable, donorsDataTable, donorFiscalYearsDataTable, electronicAccessRelationshipsDataTable, errorRecordsDataTable, eventLogsDataTable, expenseClassesDataTable, feesDataTable, feeTypesDataTable, financeGroupsDataTable, fiscalYearsDataTable, fixedDueDateSchedulesDataTable, fundsDataTable, fund3sDataTable, fundTypesDataTable, groupsDataTable, holdingsDataTable, holdingDonor2sDataTable, holdingNoteTypesDataTable, holdingTypesDataTable, hridSettingsDataTable, idTypesDataTable, illPoliciesDataTable, instancesDataTable, instanceFormatsDataTable, instanceNoteTypesDataTable, instanceRelationshipsDataTable, instanceRelationshipTypesDataTable, instanceSourceMarcsDataTable, instanceStatusesDataTable, instanceTypesDataTable, institutionsDataTable, interfacesDataTable, interfaceCredentialsDataTable, invoicesDataTable, invoice3sDataTable, invoiceItemsDataTable, invoiceItem3sDataTable, invoiceItemDonorsDataTable, invoiceItemFund2sDataTable, invoiceStatusesDataTable, itemsDataTable, itemDamagedStatusesDataTable, itemDonor2sDataTable, itemNoteTypesDataTable, itemOrderItemsDataTable, itemStatusesDataTable, ledgersDataTable, librariesDataTable, linksDataTable, loansDataTable, loanEventsDataTable, loanPoliciesDataTable, loanTypesDataTable, locationsDataTable, loginsDataTable, lostItemFeePoliciesDataTable, manualBlockTemplatesDataTable, marcRecordsDataTable, materialTypesDataTable, modeOfIssuancesDataTable, natureOfContentTermsDataTable, notesDataTable, noteLinksDataTable, noteTypesDataTable, oclcHoldingsDataTable, oclcHolding2sDataTable, oclcNumber2sDataTable, ordersDataTable, order3sDataTable, orderInvoicesDataTable, orderItemsDataTable, orderItem3sDataTable, orderItemDonorsDataTable, orderItemFund3sDataTable, orderStatusesDataTable, orderStatus2sDataTable, orderTemplatesDataTable, orderTypesDataTable, orderType2sDataTable, organizationsDataTable, organizationType2sDataTable, overdueFinePoliciesDataTable, ownersDataTable, patronActionSessionsDataTable, patronNoticePoliciesDataTable, paymentsDataTable, paymentMethodsDataTable, paymentTypesDataTable, permissionsDataTable, permissionsUsersDataTable, personDonorsDataTable, precedingSucceedingTitlesDataTable, prefixesDataTable, proxiesDataTable, rawRecordsDataTable, receiptStatusesDataTable, receivingsDataTable, recordsDataTable, referenceDatasDataTable, refundReasonsDataTable, relatedInstanceTypesDataTable, reportingCodesDataTable, requestsDataTable, requestPoliciesDataTable, rolloversDataTable, rolloverBudgetsDataTable, rolloverErrorsDataTable, rolloverProgressesDataTable, scheduledNoticesDataTable, servicePointsDataTable, servicePointUsersDataTable, snapshotsDataTable, sourcesDataTable, staffSlipsDataTable, statisticalCodesDataTable, statisticalCodeTypesDataTable, suffixesDataTable, tagsDataTable, templatesDataTable, titlesDataTable, transactionsDataTable, transferAccountsDataTable, transferCriteriasDataTable, usersDataTable, userAcquisitionsUnitsDataTable, userCategoriesDataTable, userRequestPreferencesDataTable, userSummariesDataTable, vendorsDataTable, vouchersDataTable, voucher3sDataTable, voucherItemsDataTable, voucherItem3sDataTable, voucherStatusesDataTable, waiveReasonsDataTable;
        private bool checkConstraints;
        private string connectionString;
        private bool identityInsert;
        private string providerName;
        private dynamic sqlBulkCopy;
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";
        public string ProviderName => providerName;

        public FolioBulkCopyContext(string name = "FolioContext", bool identityInsert = true, bool checkConstraints = false, string providerName = "Npgsql")
        {
            connectionString = ConfigurationManager.ConnectionStrings[name]?.ConnectionString ?? name;
            if (connectionString.Contains("http")) return;
            this.providerName = ConfigurationManager.ConnectionStrings[name]?.ProviderName ?? providerName;
            this.identityInsert = identityInsert;
            this.checkConstraints = checkConstraints;
            if (IsSqlServer) throw new NotImplementedException();
            else if (IsPostgreSql) sqlBulkCopy = new PostgreSqlBulkCopy(name, checkConstraints);
            else
            {
                throw new NotImplementedException();
            }
            PostgreSqlBulkCopy.traceSource = traceSource;
        }

        public int ExecuteNonQuery(string commandText) => sqlBulkCopy.ExecuteNonQuery(commandText);

        public void Insert(AcquisitionMethod acquisitionMethod)
        {
            if (acquisitionMethodsDataTable == null)
            {
                acquisitionMethodsDataTable = new DataTable();
                acquisitionMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                acquisitionMethodsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                acquisitionMethodsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = acquisitionMethodsDataTable.NewRow();
            dr["id"] = (object)acquisitionMethod.Id ?? DBNull.Value;
            dr["jsonb"] = (object)acquisitionMethod.Content ?? DBNull.Value;
            acquisitionMethodsDataTable.Rows.Add(dr);
        }

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

        public void Insert(ActualCostRecord actualCostRecord)
        {
            if (actualCostRecordsDataTable == null)
            {
                actualCostRecordsDataTable = new DataTable();
                actualCostRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                actualCostRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                actualCostRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                actualCostRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                actualCostRecordsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = actualCostRecordsDataTable.NewRow();
            dr["id"] = (object)actualCostRecord.Id ?? DBNull.Value;
            dr["jsonb"] = (object)actualCostRecord.Content ?? DBNull.Value;
            dr["creation_date"] = (object)actualCostRecord.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)actualCostRecord.CreationUserId ?? DBNull.Value;
            actualCostRecordsDataTable.Rows.Add(dr);
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

        public void Insert(Agreement agreement)
        {
            if (agreementsDataTable == null)
            {
                agreementsDataTable = new DataTable();
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                agreementsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                agreementsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = agreementsDataTable.NewRow();
            dr["id"] = (object)agreement.Id ?? DBNull.Value;
            dr["jsonb"] = (object)agreement.Content ?? DBNull.Value;
            agreementsDataTable.Rows.Add(dr);
        }

        public void Insert(AgreementItem agreementItem)
        {
            if (agreementItemsDataTable == null)
            {
                agreementItemsDataTable = new DataTable();
                agreementItemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                agreementItemsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                agreementItemsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = agreementItemsDataTable.NewRow();
            dr["id"] = (object)agreementItem.Id ?? DBNull.Value;
            dr["jsonb"] = (object)agreementItem.Content ?? DBNull.Value;
            agreementItemsDataTable.Rows.Add(dr);
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

        public void Insert(BlockCondition blockCondition)
        {
            if (blockConditionsDataTable == null)
            {
                blockConditionsDataTable = new DataTable();
                blockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                blockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                blockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                blockConditionsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                blockConditionsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = blockConditionsDataTable.NewRow();
            dr["id"] = (object)blockCondition.Id ?? DBNull.Value;
            dr["jsonb"] = (object)blockCondition.Content ?? DBNull.Value;
            dr["creation_date"] = (object)blockCondition.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)blockCondition.CreationUserId ?? DBNull.Value;
            blockConditionsDataTable.Rows.Add(dr);
        }

        public void Insert(BlockLimit blockLimit)
        {
            if (blockLimitsDataTable == null)
            {
                blockLimitsDataTable = new DataTable();
                blockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                blockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                blockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                blockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                blockLimitsDataTable.Columns.Add(new DataColumn { ColumnName = "conditionid", DataType = typeof(Guid) });
                blockLimitsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = blockLimitsDataTable.NewRow();
            dr["id"] = (object)blockLimit.Id ?? DBNull.Value;
            dr["jsonb"] = (object)blockLimit.Content ?? DBNull.Value;
            dr["creation_date"] = (object)blockLimit.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)blockLimit.CreationUserId ?? DBNull.Value;
            dr["conditionid"] = (object)blockLimit.Conditionid ?? DBNull.Value;
            blockLimitsDataTable.Rows.Add(dr);
        }

        public void Insert(BoundWithPart boundWithPart)
        {
            if (boundWithPartsDataTable == null)
            {
                boundWithPartsDataTable = new DataTable();
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "itemid", DataType = typeof(Guid) });
                boundWithPartsDataTable.Columns.Add(new DataColumn { ColumnName = "holdingsrecordid", DataType = typeof(Guid) });
                boundWithPartsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = boundWithPartsDataTable.NewRow();
            dr["id"] = (object)boundWithPart.Id ?? DBNull.Value;
            dr["jsonb"] = (object)boundWithPart.Content ?? DBNull.Value;
            dr["creation_date"] = (object)boundWithPart.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)boundWithPart.CreationUserId ?? DBNull.Value;
            dr["itemid"] = (object)boundWithPart.Itemid ?? DBNull.Value;
            dr["holdingsrecordid"] = (object)boundWithPart.Holdingsrecordid ?? DBNull.Value;
            boundWithPartsDataTable.Rows.Add(dr);
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

        public void Insert(BudgetExpenseClass budgetExpenseClass)
        {
            if (budgetExpenseClassesDataTable == null)
            {
                budgetExpenseClassesDataTable = new DataTable();
                budgetExpenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                budgetExpenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                budgetExpenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "budgetid", DataType = typeof(Guid) });
                budgetExpenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "expenseclassid", DataType = typeof(Guid) });
                budgetExpenseClassesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = budgetExpenseClassesDataTable.NewRow();
            dr["id"] = (object)budgetExpenseClass.Id ?? DBNull.Value;
            dr["jsonb"] = (object)budgetExpenseClass.Content ?? DBNull.Value;
            dr["budgetid"] = (object)budgetExpenseClass.Budgetid ?? DBNull.Value;
            dr["expenseclassid"] = (object)budgetExpenseClass.Expenseclassid ?? DBNull.Value;
            budgetExpenseClassesDataTable.Rows.Add(dr);
        }

        public void Insert(BudgetGroup budgetGroup)
        {
            if (budgetGroupsDataTable == null)
            {
                budgetGroupsDataTable = new DataTable();
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "budgetid", DataType = typeof(Guid) });
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "groupid", DataType = typeof(Guid) });
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "fundid", DataType = typeof(Guid) });
                budgetGroupsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                budgetGroupsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = budgetGroupsDataTable.NewRow();
            dr["id"] = (object)budgetGroup.Id ?? DBNull.Value;
            dr["jsonb"] = (object)budgetGroup.Content ?? DBNull.Value;
            dr["budgetid"] = (object)budgetGroup.Budgetid ?? DBNull.Value;
            dr["groupid"] = (object)budgetGroup.Groupid ?? DBNull.Value;
            dr["fundid"] = (object)budgetGroup.Fundid ?? DBNull.Value;
            dr["fiscalyearid"] = (object)budgetGroup.Fiscalyearid ?? DBNull.Value;
            budgetGroupsDataTable.Rows.Add(dr);
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
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                circulationRulesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                circulationRulesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = circulationRulesDataTable.NewRow();
            dr["id"] = (object)circulationRule.Id ?? DBNull.Value;
            dr["jsonb"] = (object)circulationRule.Content ?? DBNull.Value;
            dr["lock"] = (object)circulationRule.Lock ?? DBNull.Value;
            dr["creation_date"] = (object)circulationRule.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)circulationRule.CreationUserId ?? DBNull.Value;
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

        public void Insert(CloseReason closeReason)
        {
            if (closeReasonsDataTable == null)
            {
                closeReasonsDataTable = new DataTable();
                closeReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                closeReasonsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                closeReasonsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = closeReasonsDataTable.NewRow();
            dr["id"] = (object)closeReason.Id ?? DBNull.Value;
            dr["jsonb"] = (object)closeReason.Content ?? DBNull.Value;
            closeReasonsDataTable.Rows.Add(dr);
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

        public void Insert(ContactType contactType)
        {
            if (contactTypesDataTable == null)
            {
                contactTypesDataTable = new DataTable();
                contactTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(string) });
                contactTypesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                
            }
            var dr = contactTypesDataTable.NewRow();
            dr["id"] = (object)contactType.Id ?? DBNull.Value;
            dr["name"] = (object)contactType.Name ?? DBNull.Value;
            contactTypesDataTable.Rows.Add(dr);
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
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "alpha2_code", DataType = typeof(string) });
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "alpha3_code", DataType = typeof(string) });
                countriesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                
            }
            var dr = countriesDataTable.NewRow();
            dr["id"] = (object)country.Id ?? DBNull.Value;
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

        public void Insert(Department department)
        {
            if (departmentsDataTable == null)
            {
                departmentsDataTable = new DataTable();
                departmentsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                departmentsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                departmentsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                departmentsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                departmentsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = departmentsDataTable.NewRow();
            dr["id"] = (object)department.Id ?? DBNull.Value;
            dr["jsonb"] = (object)department.Content ?? DBNull.Value;
            dr["creation_date"] = (object)department.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)department.CreationUserId ?? DBNull.Value;
            departmentsDataTable.Rows.Add(dr);
        }

        public void Insert(Department3 department3)
        {
            if (department3sDataTable == null)
            {
                department3sDataTable = new DataTable();
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                department3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = department3sDataTable.NewRow();
            dr["id"] = (object)department3.Id ?? DBNull.Value;
            dr["name"] = (object)department3.Name ?? DBNull.Value;
            dr["creation_time"] = (object)department3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)department3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)department3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)department3.LastWriteUsername ?? DBNull.Value;
            department3sDataTable.Rows.Add(dr);
        }

        public void Insert(Division division)
        {
            if (divisionsDataTable == null)
            {
                divisionsDataTable = new DataTable();
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                divisionsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = divisionsDataTable.NewRow();
            dr["id"] = (object)division.Id ?? DBNull.Value;
            dr["name"] = (object)division.Name ?? DBNull.Value;
            dr["creation_time"] = (object)division.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)division.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)division.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)division.LastWriteUsername ?? DBNull.Value;
            divisionsDataTable.Rows.Add(dr);
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

        public void Insert(Donor donor)
        {
            if (donorsDataTable == null)
            {
                donorsDataTable = new DataTable();
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "code", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "report", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "enabled", DataType = typeof(bool) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "public_display", DataType = typeof(string) });
                donorsDataTable.Columns.Add(new DataColumn { ColumnName = "notes", DataType = typeof(string) });
                
            }
            var dr = donorsDataTable.NewRow();
            dr["id"] = (object)donor.Id ?? DBNull.Value;
            dr["name"] = (object)donor.Name ?? DBNull.Value;
            dr["code"] = (object)donor.Code ?? DBNull.Value;
            dr["amount"] = (object)donor.Amount ?? DBNull.Value;
            dr["report"] = (object)donor.Report ?? DBNull.Value;
            dr["creation_time"] = (object)donor.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)donor.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)donor.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)donor.LastWriteUsername ?? DBNull.Value;
            dr["enabled"] = (object)donor.Enabled ?? DBNull.Value;
            dr["public_display"] = (object)donor.PublicDisplay ?? DBNull.Value;
            dr["notes"] = (object)donor.Notes ?? DBNull.Value;
            donorsDataTable.Rows.Add(dr);
        }

        public void Insert(DonorFiscalYear donorFiscalYear)
        {
            if (donorFiscalYearsDataTable == null)
            {
                donorFiscalYearsDataTable = new DataTable();
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscal_year", DataType = typeof(int) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "start_amount", DataType = typeof(decimal) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                donorFiscalYearsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = donorFiscalYearsDataTable.NewRow();
            dr["id"] = (object)donorFiscalYear.Id ?? DBNull.Value;
            dr["donor_id"] = (object)donorFiscalYear.DonorId ?? DBNull.Value;
            dr["fiscal_year"] = (object)donorFiscalYear.FiscalYear ?? DBNull.Value;
            dr["start_amount"] = (object)donorFiscalYear.StartAmount ?? DBNull.Value;
            dr["creation_time"] = (object)donorFiscalYear.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)donorFiscalYear.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)donorFiscalYear.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)donorFiscalYear.LastWriteUsername ?? DBNull.Value;
            donorFiscalYearsDataTable.Rows.Add(dr);
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
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "content", DataType = typeof(string) });
                errorRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "description", DataType = typeof(string) });
                
            }
            var dr = errorRecordsDataTable.NewRow();
            dr["id"] = (object)errorRecord.Id ?? DBNull.Value;
            dr["content"] = (object)errorRecord.Content ?? DBNull.Value;
            dr["description"] = (object)errorRecord.Description ?? DBNull.Value;
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

        public void Insert(ExpenseClass expenseClass)
        {
            if (expenseClassesDataTable == null)
            {
                expenseClassesDataTable = new DataTable();
                expenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                expenseClassesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                expenseClassesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = expenseClassesDataTable.NewRow();
            dr["id"] = (object)expenseClass.Id ?? DBNull.Value;
            dr["jsonb"] = (object)expenseClass.Content ?? DBNull.Value;
            expenseClassesDataTable.Rows.Add(dr);
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

        public void Insert(Fund3 fund3)
        {
            if (fund3sDataTable == null)
            {
                fund3sDataTable = new DataTable();
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "number", DataType = typeof(string) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "code", DataType = typeof(string) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                fund3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = fund3sDataTable.NewRow();
            dr["id"] = (object)fund3.Id ?? DBNull.Value;
            dr["name"] = (object)fund3.Name ?? DBNull.Value;
            dr["number"] = (object)fund3.Number ?? DBNull.Value;
            dr["code"] = (object)fund3.Code ?? DBNull.Value;
            dr["creation_time"] = (object)fund3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)fund3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)fund3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)fund3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)fund3.LongId ?? DBNull.Value;
            fund3sDataTable.Rows.Add(dr);
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
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "effectivelocationid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "holdingstypeid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "callnumbertypeid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "illpolicyid", DataType = typeof(Guid) });
                holdingsDataTable.Columns.Add(new DataColumn { ColumnName = "sourceid", DataType = typeof(Guid) });
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
            dr["effectivelocationid"] = (object)holding.Effectivelocationid ?? DBNull.Value;
            dr["holdingstypeid"] = (object)holding.Holdingstypeid ?? DBNull.Value;
            dr["callnumbertypeid"] = (object)holding.Callnumbertypeid ?? DBNull.Value;
            dr["illpolicyid"] = (object)holding.Illpolicyid ?? DBNull.Value;
            dr["sourceid"] = (object)holding.Sourceid ?? DBNull.Value;
            holdingsDataTable.Rows.Add(dr);
        }

        public void Insert(HoldingDonor2 holdingDonor2)
        {
            if (holdingDonor2sDataTable == null)
            {
                holdingDonor2sDataTable = new DataTable();
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "holding_id", DataType = typeof(Guid) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "report", DataType = typeof(bool) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                holdingDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                
            }
            var dr = holdingDonor2sDataTable.NewRow();
            dr["holding_id"] = (object)holdingDonor2.HoldingId ?? DBNull.Value;
            dr["donor_id"] = (object)holdingDonor2.DonorId ?? DBNull.Value;
            dr["report"] = (object)holdingDonor2.Report ?? DBNull.Value;
            dr["creation_time"] = (object)holdingDonor2.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)holdingDonor2.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)holdingDonor2.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)holdingDonor2.LastWriteUsername ?? DBNull.Value;
            dr["id"] = (object)holdingDonor2.Id ?? DBNull.Value;
            holdingDonor2sDataTable.Rows.Add(dr);
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
                hridSettingsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                hridSettingsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                hridSettingsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = hridSettingsDataTable.NewRow();
            dr["id"] = (object)hridSetting.Id ?? DBNull.Value;
            dr["jsonb"] = (object)hridSetting.Content ?? DBNull.Value;
            dr["lock"] = (object)hridSetting.Lock ?? DBNull.Value;
            dr["creation_date"] = (object)hridSetting.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)hridSetting.CreationUserId ?? DBNull.Value;
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
                instancesDataTable.Columns.Add(new DataColumn { ColumnName = "complete_updated_date", DataType = typeof(DateTime) });
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
            dr["complete_updated_date"] = (object)instance.CompletionTime ?? DBNull.Value;
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

        public void Insert(Invoice3 invoice3)
        {
            if (invoice3sDataTable == null)
            {
                invoice3sDataTable = new DataTable();
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_id", DataType = typeof(int) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_date", DataType = typeof(DateTime) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_invoice_id", DataType = typeof(string) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_invoice_amount", DataType = typeof(decimal) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_status_id", DataType = typeof(int) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "fiscal_year", DataType = typeof(int) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_time", DataType = typeof(DateTime) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_username", DataType = typeof(string) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_time", DataType = typeof(DateTime) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_username", DataType = typeof(string) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                invoice3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = invoice3sDataTable.NewRow();
            dr["id"] = (object)invoice3.Id ?? DBNull.Value;
            dr["vendor_id"] = (object)invoice3.VendorId ?? DBNull.Value;
            dr["invoice_date"] = (object)invoice3.InvoiceDate ?? DBNull.Value;
            dr["vendor_invoice_id"] = (object)invoice3.VendorInvoiceId ?? DBNull.Value;
            dr["vendor_invoice_amount"] = (object)invoice3.VendorInvoiceAmount ?? DBNull.Value;
            dr["invoice_status_id"] = (object)invoice3.InvoiceStatusId ?? DBNull.Value;
            dr["fiscal_year"] = (object)invoice3.FiscalYear ?? DBNull.Value;
            dr["approve_time"] = (object)invoice3.ApproveTime ?? DBNull.Value;
            dr["approve_username"] = (object)invoice3.ApproveUsername ?? DBNull.Value;
            dr["cancel_time"] = (object)invoice3.CancelTime ?? DBNull.Value;
            dr["cancel_username"] = (object)invoice3.CancelUsername ?? DBNull.Value;
            dr["creation_time"] = (object)invoice3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)invoice3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)invoice3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)invoice3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)invoice3.LongId ?? DBNull.Value;
            invoice3sDataTable.Rows.Add(dr);
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

        public void Insert(InvoiceItem3 invoiceItem3)
        {
            if (invoiceItem3sDataTable == null)
            {
                invoiceItem3sDataTable = new DataTable();
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_id", DataType = typeof(int) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "quantity", DataType = typeof(int) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_item_id", DataType = typeof(int) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                invoiceItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = invoiceItem3sDataTable.NewRow();
            dr["id"] = (object)invoiceItem3.Id ?? DBNull.Value;
            dr["invoice_id"] = (object)invoiceItem3.InvoiceId ?? DBNull.Value;
            dr["name"] = (object)invoiceItem3.Name ?? DBNull.Value;
            dr["quantity"] = (object)invoiceItem3.Quantity ?? DBNull.Value;
            dr["order_item_id"] = (object)invoiceItem3.OrderItemId ?? DBNull.Value;
            dr["creation_time"] = (object)invoiceItem3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)invoiceItem3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)invoiceItem3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)invoiceItem3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)invoiceItem3.LongId ?? DBNull.Value;
            invoiceItem3sDataTable.Rows.Add(dr);
        }

        public void Insert(InvoiceItemDonor invoiceItemDonor)
        {
            if (invoiceItemDonorsDataTable == null)
            {
                invoiceItemDonorsDataTable = new DataTable();
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_item_id", DataType = typeof(string) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_item_short_id", DataType = typeof(int) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "shipping_amount", DataType = typeof(decimal) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                invoiceItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = invoiceItemDonorsDataTable.NewRow();
            dr["id"] = (object)invoiceItemDonor.Id ?? DBNull.Value;
            dr["invoice_item_id"] = (object)invoiceItemDonor.InvoiceItemId ?? DBNull.Value;
            dr["invoice_item_short_id"] = (object)invoiceItemDonor.InvoiceItemShortId ?? DBNull.Value;
            dr["donor_id"] = (object)invoiceItemDonor.DonorId ?? DBNull.Value;
            dr["amount"] = (object)invoiceItemDonor.Amount ?? DBNull.Value;
            dr["shipping_amount"] = (object)invoiceItemDonor.ShippingAmount ?? DBNull.Value;
            dr["creation_time"] = (object)invoiceItemDonor.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)invoiceItemDonor.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)invoiceItemDonor.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)invoiceItemDonor.LastWriteUsername ?? DBNull.Value;
            invoiceItemDonorsDataTable.Rows.Add(dr);
        }

        public void Insert(InvoiceItemFund2 invoiceItemFund2)
        {
            if (invoiceItemFund2sDataTable == null)
            {
                invoiceItemFund2sDataTable = new DataTable();
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_item_id", DataType = typeof(int) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "fund_id", DataType = typeof(int) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "shipping_amount", DataType = typeof(decimal) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                invoiceItemFund2sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = invoiceItemFund2sDataTable.NewRow();
            dr["id"] = (object)invoiceItemFund2.Id ?? DBNull.Value;
            dr["invoice_item_id"] = (object)invoiceItemFund2.InvoiceItemId ?? DBNull.Value;
            dr["fund_id"] = (object)invoiceItemFund2.FundId ?? DBNull.Value;
            dr["amount"] = (object)invoiceItemFund2.Amount ?? DBNull.Value;
            dr["shipping_amount"] = (object)invoiceItemFund2.ShippingAmount ?? DBNull.Value;
            dr["creation_time"] = (object)invoiceItemFund2.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)invoiceItemFund2.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)invoiceItemFund2.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)invoiceItemFund2.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)invoiceItemFund2.LongId ?? DBNull.Value;
            invoiceItemFund2sDataTable.Rows.Add(dr);
        }

        public void Insert(InvoiceStatus invoiceStatus)
        {
            if (invoiceStatusesDataTable == null)
            {
                invoiceStatusesDataTable = new DataTable();
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                invoiceStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = invoiceStatusesDataTable.NewRow();
            dr["id"] = (object)invoiceStatus.Id ?? DBNull.Value;
            dr["name"] = (object)invoiceStatus.Name ?? DBNull.Value;
            dr["creation_time"] = (object)invoiceStatus.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)invoiceStatus.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)invoiceStatus.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)invoiceStatus.LastWriteUsername ?? DBNull.Value;
            invoiceStatusesDataTable.Rows.Add(dr);
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

        public void Insert(ItemDonor2 itemDonor2)
        {
            if (itemDonor2sDataTable == null)
            {
                itemDonor2sDataTable = new DataTable();
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "item_id", DataType = typeof(Guid) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "item_short_id", DataType = typeof(int) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "report", DataType = typeof(bool) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                itemDonor2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = itemDonor2sDataTable.NewRow();
            dr["id"] = (object)itemDonor2.Id ?? DBNull.Value;
            dr["item_id"] = (object)itemDonor2.ItemId ?? DBNull.Value;
            dr["item_short_id"] = (object)itemDonor2.ItemShortId ?? DBNull.Value;
            dr["donor_id"] = (object)itemDonor2.DonorId ?? DBNull.Value;
            dr["report"] = (object)itemDonor2.Report ?? DBNull.Value;
            dr["creation_time"] = (object)itemDonor2.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)itemDonor2.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)itemDonor2.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)itemDonor2.LastWriteUsername ?? DBNull.Value;
            itemDonor2sDataTable.Rows.Add(dr);
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

        public void Insert(ItemOrderItem itemOrderItem)
        {
            if (itemOrderItemsDataTable == null)
            {
                itemOrderItemsDataTable = new DataTable();
                itemOrderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                itemOrderItemsDataTable.Columns.Add(new DataColumn { ColumnName = "order_item_id", DataType = typeof(int) });
                
            }
            var dr = itemOrderItemsDataTable.NewRow();
            dr["id"] = (object)itemOrderItem.Id ?? DBNull.Value;
            dr["order_item_id"] = (object)itemOrderItem.OrderItemId ?? DBNull.Value;
            itemOrderItemsDataTable.Rows.Add(dr);
        }

        public void Insert(ItemStatus itemStatus)
        {
            if (itemStatusesDataTable == null)
            {
                itemStatusesDataTable = new DataTable();
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                itemStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = itemStatusesDataTable.NewRow();
            dr["id"] = (object)itemStatus.Id ?? DBNull.Value;
            dr["name"] = (object)itemStatus.Name ?? DBNull.Value;
            dr["creation_time"] = (object)itemStatus.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)itemStatus.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)itemStatus.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)itemStatus.LastWriteUsername ?? DBNull.Value;
            itemStatusesDataTable.Rows.Add(dr);
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

        public void Insert(Link link)
        {
            if (linksDataTable == null)
            {
                linksDataTable = new DataTable();
                linksDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                linksDataTable.Columns.Add(new DataColumn { ColumnName = "object_id", DataType = typeof(string) });
                linksDataTable.Columns.Add(new DataColumn { ColumnName = "object_type", DataType = typeof(string) });
                
            }
            var dr = linksDataTable.NewRow();
            dr["id"] = (object)link.Id ?? DBNull.Value;
            dr["object_id"] = (object)link.ObjectId ?? DBNull.Value;
            dr["object_type"] = (object)link.ObjectType ?? DBNull.Value;
            linksDataTable.Rows.Add(dr);
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

        public void Insert(LoanEvent loanEvent)
        {
            if (loanEventsDataTable == null)
            {
                loanEventsDataTable = new DataTable();
                loanEventsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                loanEventsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                loanEventsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = loanEventsDataTable.NewRow();
            dr["id"] = (object)loanEvent.Id ?? DBNull.Value;
            dr["jsonb"] = (object)loanEvent.Content ?? DBNull.Value;
            loanEventsDataTable.Rows.Add(dr);
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

        public void Insert(ManualBlockTemplate manualBlockTemplate)
        {
            if (manualBlockTemplatesDataTable == null)
            {
                manualBlockTemplatesDataTable = new DataTable();
                manualBlockTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                manualBlockTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                manualBlockTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                manualBlockTemplatesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                manualBlockTemplatesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = manualBlockTemplatesDataTable.NewRow();
            dr["id"] = (object)manualBlockTemplate.Id ?? DBNull.Value;
            dr["jsonb"] = (object)manualBlockTemplate.Content ?? DBNull.Value;
            dr["creation_date"] = (object)manualBlockTemplate.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)manualBlockTemplate.CreationUserId ?? DBNull.Value;
            manualBlockTemplatesDataTable.Rows.Add(dr);
        }

        public void Insert(MarcRecord marcRecord)
        {
            if (marcRecordsDataTable == null)
            {
                marcRecordsDataTable = new DataTable();
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                marcRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "content", DataType = typeof(string) });
                marcRecordsDataTable.Columns["content"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = marcRecordsDataTable.NewRow();
            dr["id"] = (object)marcRecord.Id ?? DBNull.Value;
            dr["content"] = (object)marcRecord.Content ?? DBNull.Value;
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
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "title", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "content", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "indexed_content", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "domain", DataType = typeof(string) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "type_id", DataType = typeof(Guid) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "pop_up_on_user", DataType = typeof(bool) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "pop_up_on_check_out", DataType = typeof(bool) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(Guid) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "updated_by", DataType = typeof(Guid) });
                notesDataTable.Columns.Add(new DataColumn { ColumnName = "updated_date", DataType = typeof(DateTime) });
                
            }
            var dr = notesDataTable.NewRow();
            dr["id"] = (object)note.Id ?? DBNull.Value;
            dr["title"] = (object)note.Title ?? DBNull.Value;
            dr["content"] = (object)note.Content ?? DBNull.Value;
            dr["indexed_content"] = (object)note.IndexedContent ?? DBNull.Value;
            dr["domain"] = (object)note.Domain ?? DBNull.Value;
            dr["type_id"] = (object)note.TypeId ?? DBNull.Value;
            dr["pop_up_on_user"] = (object)note.PopUpOnUser ?? DBNull.Value;
            dr["pop_up_on_check_out"] = (object)note.PopUpOnCheckOut ?? DBNull.Value;
            dr["created_by"] = (object)note.CreationUserId ?? DBNull.Value;
            dr["created_date"] = (object)note.CreationTime ?? DBNull.Value;
            dr["updated_by"] = (object)note.UpdatedBy ?? DBNull.Value;
            dr["updated_date"] = (object)note.LastWriteTime ?? DBNull.Value;
            notesDataTable.Rows.Add(dr);
        }

        public void Insert(NoteLink noteLink)
        {
            if (noteLinksDataTable == null)
            {
                noteLinksDataTable = new DataTable();
                noteLinksDataTable.Columns.Add(new DataColumn { ColumnName = "note_id", DataType = typeof(Guid) });
                noteLinksDataTable.Columns.Add(new DataColumn { ColumnName = "link_id", DataType = typeof(Guid) });
                
            }
            var dr = noteLinksDataTable.NewRow();
            dr["note_id"] = (object)noteLink.NoteId ?? DBNull.Value;
            dr["link_id"] = (object)noteLink.LinkId ?? DBNull.Value;
            noteLinksDataTable.Rows.Add(dr);
        }

        public void Insert(NoteType noteType)
        {
            if (noteTypesDataTable == null)
            {
                noteTypesDataTable = new DataTable();
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(Guid) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "updated_by", DataType = typeof(Guid) });
                noteTypesDataTable.Columns.Add(new DataColumn { ColumnName = "updated_date", DataType = typeof(DateTime) });
                
            }
            var dr = noteTypesDataTable.NewRow();
            dr["id"] = (object)noteType.Id ?? DBNull.Value;
            dr["name"] = (object)noteType.Name ?? DBNull.Value;
            dr["created_by"] = (object)noteType.CreationUserId ?? DBNull.Value;
            dr["created_date"] = (object)noteType.CreationTime ?? DBNull.Value;
            dr["updated_by"] = (object)noteType.UpdatedBy ?? DBNull.Value;
            dr["updated_date"] = (object)noteType.LastWriteTime ?? DBNull.Value;
            noteTypesDataTable.Rows.Add(dr);
        }

        public void Insert(OclcHolding oclcHolding)
        {
            if (oclcHoldingsDataTable == null)
            {
                oclcHoldingsDataTable = new DataTable();
                oclcHoldingsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                oclcHoldingsDataTable.Columns.Add(new DataColumn { ColumnName = "number", DataType = typeof(int) });
                oclcHoldingsDataTable.Columns.Add(new DataColumn { ColumnName = "instance_id", DataType = typeof(int) });
                oclcHoldingsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                
            }
            var dr = oclcHoldingsDataTable.NewRow();
            dr["id"] = (object)oclcHolding.Id ?? DBNull.Value;
            dr["number"] = (object)oclcHolding.Number ?? DBNull.Value;
            dr["instance_id"] = (object)oclcHolding.InstanceId ?? DBNull.Value;
            dr["last_write_time"] = (object)oclcHolding.LastWriteTime ?? DBNull.Value;
            oclcHoldingsDataTable.Rows.Add(dr);
        }

        public void Insert(OclcHolding2 oclcHolding2)
        {
            if (oclcHolding2sDataTable == null)
            {
                oclcHolding2sDataTable = new DataTable();
                oclcHolding2sDataTable.Columns.Add(new DataColumn { ColumnName = "number", DataType = typeof(int) });
                oclcHolding2sDataTable.Columns.Add(new DataColumn { ColumnName = "instance_id", DataType = typeof(int) });
                oclcHolding2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                
            }
            var dr = oclcHolding2sDataTable.NewRow();
            dr["number"] = (object)oclcHolding2.Number ?? DBNull.Value;
            dr["instance_id"] = (object)oclcHolding2.InstanceId ?? DBNull.Value;
            dr["last_write_time"] = (object)oclcHolding2.LastWriteTime ?? DBNull.Value;
            oclcHolding2sDataTable.Rows.Add(dr);
        }

        public void Insert(OclcNumber2 oclcNumber2)
        {
            if (oclcNumber2sDataTable == null)
            {
                oclcNumber2sDataTable = new DataTable();
                oclcNumber2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                oclcNumber2sDataTable.Columns.Add(new DataColumn { ColumnName = "instance_id", DataType = typeof(int) });
                oclcNumber2sDataTable.Columns.Add(new DataColumn { ColumnName = "content", DataType = typeof(int) });
                oclcNumber2sDataTable.Columns.Add(new DataColumn { ColumnName = "holding_creation_time", DataType = typeof(DateTime) });
                oclcNumber2sDataTable.Columns.Add(new DataColumn { ColumnName = "invalid_time", DataType = typeof(DateTime) });
                
            }
            var dr = oclcNumber2sDataTable.NewRow();
            dr["id"] = (object)oclcNumber2.Id ?? DBNull.Value;
            dr["instance_id"] = (object)oclcNumber2.InstanceId ?? DBNull.Value;
            dr["content"] = (object)oclcNumber2.Content ?? DBNull.Value;
            dr["holding_creation_time"] = (object)oclcNumber2.HoldingCreationTime ?? DBNull.Value;
            dr["invalid_time"] = (object)oclcNumber2.InvalidTime ?? DBNull.Value;
            oclcNumber2sDataTable.Rows.Add(dr);
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

        public void Insert(Order3 order3)
        {
            if (order3sDataTable == null)
            {
                order3sDataTable = new DataTable();
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "document_id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_date", DataType = typeof(DateTime) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_type_id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_status_id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "fiscal_year", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_customer_id", DataType = typeof(string) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "delivery_room_id", DataType = typeof(int) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_time", DataType = typeof(DateTime) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_username", DataType = typeof(string) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_time", DataType = typeof(DateTime) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_username", DataType = typeof(string) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "freight_amount", DataType = typeof(decimal) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "shipping_amount", DataType = typeof(decimal) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                order3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = order3sDataTable.NewRow();
            dr["id"] = (object)order3.Id ?? DBNull.Value;
            dr["document_id"] = (object)order3.DocumentId ?? DBNull.Value;
            dr["vendor_id"] = (object)order3.VendorId ?? DBNull.Value;
            dr["order_date"] = (object)order3.OrderDate ?? DBNull.Value;
            dr["order_type_id"] = (object)order3.OrderTypeId ?? DBNull.Value;
            dr["order_status_id"] = (object)order3.OrderStatusId ?? DBNull.Value;
            dr["fiscal_year"] = (object)order3.FiscalYear ?? DBNull.Value;
            dr["vendor_customer_id"] = (object)order3.VendorCustomerId ?? DBNull.Value;
            dr["delivery_room_id"] = (object)order3.DeliveryRoomId ?? DBNull.Value;
            dr["approve_time"] = (object)order3.ApproveTime ?? DBNull.Value;
            dr["approve_username"] = (object)order3.ApproveUsername ?? DBNull.Value;
            dr["cancel_time"] = (object)order3.CancelTime ?? DBNull.Value;
            dr["cancel_username"] = (object)order3.CancelUsername ?? DBNull.Value;
            dr["freight_amount"] = (object)order3.FreightAmount ?? DBNull.Value;
            dr["shipping_amount"] = (object)order3.ShippingAmount ?? DBNull.Value;
            dr["creation_time"] = (object)order3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)order3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)order3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)order3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)order3.LongId ?? DBNull.Value;
            order3sDataTable.Rows.Add(dr);
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

        public void Insert(OrderItem3 orderItem3)
        {
            if (orderItem3sDataTable == null)
            {
                orderItem3sDataTable = new DataTable();
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_id", DataType = typeof(int) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "unit_price", DataType = typeof(decimal) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "quantity", DataType = typeof(int) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_item_id", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_notes", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "special_notes", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "miscellaneous_notes", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "selector_notes", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "instance_id", DataType = typeof(int) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "holding_id", DataType = typeof(int) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                orderItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "receipt_status_id", DataType = typeof(int) });
                
            }
            var dr = orderItem3sDataTable.NewRow();
            dr["id"] = (object)orderItem3.Id ?? DBNull.Value;
            dr["order_id"] = (object)orderItem3.OrderId ?? DBNull.Value;
            dr["name"] = (object)orderItem3.Name ?? DBNull.Value;
            dr["unit_price"] = (object)orderItem3.UnitPrice ?? DBNull.Value;
            dr["quantity"] = (object)orderItem3.Quantity ?? DBNull.Value;
            dr["vendor_item_id"] = (object)orderItem3.VendorItemId ?? DBNull.Value;
            dr["vendor_notes"] = (object)orderItem3.VendorNotes ?? DBNull.Value;
            dr["special_notes"] = (object)orderItem3.SpecialNotes ?? DBNull.Value;
            dr["miscellaneous_notes"] = (object)orderItem3.MiscellaneousNotes ?? DBNull.Value;
            dr["selector_notes"] = (object)orderItem3.SelectorNotes ?? DBNull.Value;
            dr["instance_id"] = (object)orderItem3.InstanceId ?? DBNull.Value;
            dr["holding_id"] = (object)orderItem3.HoldingId ?? DBNull.Value;
            dr["creation_time"] = (object)orderItem3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderItem3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderItem3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderItem3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)orderItem3.LongId ?? DBNull.Value;
            dr["receipt_status_id"] = (object)orderItem3.ReceiptStatusId ?? DBNull.Value;
            orderItem3sDataTable.Rows.Add(dr);
        }

        public void Insert(OrderItemDonor orderItemDonor)
        {
            if (orderItemDonorsDataTable == null)
            {
                orderItemDonorsDataTable = new DataTable();
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "order_item_id", DataType = typeof(string) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "order_item_short_id", DataType = typeof(int) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "report", DataType = typeof(bool) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderItemDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = orderItemDonorsDataTable.NewRow();
            dr["id"] = (object)orderItemDonor.Id ?? DBNull.Value;
            dr["order_item_id"] = (object)orderItemDonor.OrderItemId ?? DBNull.Value;
            dr["order_item_short_id"] = (object)orderItemDonor.OrderItemShortId ?? DBNull.Value;
            dr["donor_id"] = (object)orderItemDonor.DonorId ?? DBNull.Value;
            dr["report"] = (object)orderItemDonor.Report ?? DBNull.Value;
            dr["amount"] = (object)orderItemDonor.Amount ?? DBNull.Value;
            dr["creation_time"] = (object)orderItemDonor.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderItemDonor.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderItemDonor.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderItemDonor.LastWriteUsername ?? DBNull.Value;
            orderItemDonorsDataTable.Rows.Add(dr);
        }

        public void Insert(OrderItemFund3 orderItemFund3)
        {
            if (orderItemFund3sDataTable == null)
            {
                orderItemFund3sDataTable = new DataTable();
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "order_item_id", DataType = typeof(int) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "fund_id", DataType = typeof(int) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                orderItemFund3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = orderItemFund3sDataTable.NewRow();
            dr["id"] = (object)orderItemFund3.Id ?? DBNull.Value;
            dr["order_item_id"] = (object)orderItemFund3.OrderItemId ?? DBNull.Value;
            dr["fund_id"] = (object)orderItemFund3.FundId ?? DBNull.Value;
            dr["amount"] = (object)orderItemFund3.Amount ?? DBNull.Value;
            dr["creation_time"] = (object)orderItemFund3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderItemFund3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderItemFund3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderItemFund3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)orderItemFund3.LongId ?? DBNull.Value;
            orderItemFund3sDataTable.Rows.Add(dr);
        }

        public void Insert(OrderStatus orderStatus)
        {
            if (orderStatusesDataTable == null)
            {
                orderStatusesDataTable = new DataTable();
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = orderStatusesDataTable.NewRow();
            dr["id"] = (object)orderStatus.Id ?? DBNull.Value;
            dr["name"] = (object)orderStatus.Name ?? DBNull.Value;
            dr["creation_time"] = (object)orderStatus.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderStatus.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderStatus.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderStatus.LastWriteUsername ?? DBNull.Value;
            orderStatusesDataTable.Rows.Add(dr);
        }

        public void Insert(OrderStatus2 orderStatus2)
        {
            if (orderStatus2sDataTable == null)
            {
                orderStatus2sDataTable = new DataTable();
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderStatus2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = orderStatus2sDataTable.NewRow();
            dr["id"] = (object)orderStatus2.Id ?? DBNull.Value;
            dr["name"] = (object)orderStatus2.Name ?? DBNull.Value;
            dr["creation_time"] = (object)orderStatus2.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderStatus2.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderStatus2.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderStatus2.LastWriteUsername ?? DBNull.Value;
            orderStatus2sDataTable.Rows.Add(dr);
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

        public void Insert(OrderType orderType)
        {
            if (orderTypesDataTable == null)
            {
                orderTypesDataTable = new DataTable();
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderTypesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = orderTypesDataTable.NewRow();
            dr["id"] = (object)orderType.Id ?? DBNull.Value;
            dr["name"] = (object)orderType.Name ?? DBNull.Value;
            dr["creation_time"] = (object)orderType.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderType.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderType.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderType.LastWriteUsername ?? DBNull.Value;
            orderTypesDataTable.Rows.Add(dr);
        }

        public void Insert(OrderType2 orderType2)
        {
            if (orderType2sDataTable == null)
            {
                orderType2sDataTable = new DataTable();
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                orderType2sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = orderType2sDataTable.NewRow();
            dr["id"] = (object)orderType2.Id ?? DBNull.Value;
            dr["name"] = (object)orderType2.Name ?? DBNull.Value;
            dr["creation_time"] = (object)orderType2.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)orderType2.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)orderType2.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)orderType2.LastWriteUsername ?? DBNull.Value;
            orderType2sDataTable.Rows.Add(dr);
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

        public void Insert(OrganizationType2 organizationType2)
        {
            if (organizationType2sDataTable == null)
            {
                organizationType2sDataTable = new DataTable();
                organizationType2sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                organizationType2sDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                organizationType2sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                organizationType2sDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                organizationType2sDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = organizationType2sDataTable.NewRow();
            dr["id"] = (object)organizationType2.Id ?? DBNull.Value;
            dr["jsonb"] = (object)organizationType2.Content ?? DBNull.Value;
            dr["creation_date"] = (object)organizationType2.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)organizationType2.CreationUserId ?? DBNull.Value;
            organizationType2sDataTable.Rows.Add(dr);
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

        public void Insert(PaymentType paymentType)
        {
            if (paymentTypesDataTable == null)
            {
                paymentTypesDataTable = new DataTable();
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                paymentTypesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = paymentTypesDataTable.NewRow();
            dr["id"] = (object)paymentType.Id ?? DBNull.Value;
            dr["name"] = (object)paymentType.Name ?? DBNull.Value;
            dr["creation_time"] = (object)paymentType.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)paymentType.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)paymentType.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)paymentType.LastWriteUsername ?? DBNull.Value;
            paymentTypesDataTable.Rows.Add(dr);
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

        public void Insert(PersonDonor personDonor)
        {
            if (personDonorsDataTable == null)
            {
                personDonorsDataTable = new DataTable();
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "person_id", DataType = typeof(Guid) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "donor_id", DataType = typeof(int) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                personDonorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = personDonorsDataTable.NewRow();
            dr["id"] = (object)personDonor.Id ?? DBNull.Value;
            dr["person_id"] = (object)personDonor.PersonId ?? DBNull.Value;
            dr["donor_id"] = (object)personDonor.DonorId ?? DBNull.Value;
            dr["creation_time"] = (object)personDonor.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)personDonor.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)personDonor.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)personDonor.LastWriteUsername ?? DBNull.Value;
            personDonorsDataTable.Rows.Add(dr);
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
                rawRecordsDataTable.Columns.Add(new DataColumn { ColumnName = "content", DataType = typeof(string) });
                
            }
            var dr = rawRecordsDataTable.NewRow();
            dr["id"] = (object)rawRecord.Id ?? DBNull.Value;
            dr["content"] = (object)rawRecord.Content ?? DBNull.Value;
            rawRecordsDataTable.Rows.Add(dr);
        }

        public void Insert(ReceiptStatus receiptStatus)
        {
            if (receiptStatusesDataTable == null)
            {
                receiptStatusesDataTable = new DataTable();
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                receiptStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = receiptStatusesDataTable.NewRow();
            dr["id"] = (object)receiptStatus.Id ?? DBNull.Value;
            dr["name"] = (object)receiptStatus.Name ?? DBNull.Value;
            dr["creation_time"] = (object)receiptStatus.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)receiptStatus.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)receiptStatus.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)receiptStatus.LastWriteUsername ?? DBNull.Value;
            receiptStatusesDataTable.Rows.Add(dr);
        }

        public void Insert(Receiving receiving)
        {
            if (receivingsDataTable == null)
            {
                receivingsDataTable = new DataTable();
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "polineid", DataType = typeof(Guid) });
                receivingsDataTable.Columns.Add(new DataColumn { ColumnName = "titleid", DataType = typeof(Guid) });
                receivingsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = receivingsDataTable.NewRow();
            dr["id"] = (object)receiving.Id ?? DBNull.Value;
            dr["jsonb"] = (object)receiving.Content ?? DBNull.Value;
            dr["creation_date"] = (object)receiving.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)receiving.CreationUserId ?? DBNull.Value;
            dr["polineid"] = (object)receiving.Polineid ?? DBNull.Value;
            dr["titleid"] = (object)receiving.Titleid ?? DBNull.Value;
            receivingsDataTable.Rows.Add(dr);
        }

        public void Insert(Record record)
        {
            if (recordsDataTable == null)
            {
                recordsDataTable = new DataTable();
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "snapshot_id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "matched_id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "generation", DataType = typeof(int) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "record_type", DataType = typeof(string) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "external_id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "state", DataType = typeof(string) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "leader_record_status", DataType = typeof(string) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "order", DataType = typeof(int) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "suppress_discovery", DataType = typeof(bool) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by_user_id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_by_user_id", DataType = typeof(Guid) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_date", DataType = typeof(DateTime) });
                recordsDataTable.Columns.Add(new DataColumn { ColumnName = "external_hrid", DataType = typeof(string) });
                
            }
            var dr = recordsDataTable.NewRow();
            dr["id"] = (object)record.Id ?? DBNull.Value;
            dr["snapshot_id"] = (object)record.SnapshotId ?? DBNull.Value;
            dr["matched_id"] = (object)record.MatchedId ?? DBNull.Value;
            dr["generation"] = (object)record.Generation ?? DBNull.Value;
            dr["record_type"] = (object)record.RecordType ?? DBNull.Value;
            dr["external_id"] = (object)record.InstanceId ?? DBNull.Value;
            dr["state"] = (object)record.State ?? DBNull.Value;
            dr["leader_record_status"] = (object)record.LeaderRecordStatus ?? DBNull.Value;
            dr["order"] = (object)record.Order ?? DBNull.Value;
            dr["suppress_discovery"] = (object)record.SuppressDiscovery ?? DBNull.Value;
            dr["created_by_user_id"] = (object)record.CreationUserId ?? DBNull.Value;
            dr["created_date"] = (object)record.CreationTime ?? DBNull.Value;
            dr["updated_by_user_id"] = (object)record.LastWriteUserId ?? DBNull.Value;
            dr["updated_date"] = (object)record.LastWriteTime ?? DBNull.Value;
            dr["external_hrid"] = (object)record.InstanceShortId ?? DBNull.Value;
            recordsDataTable.Rows.Add(dr);
        }

        public void Insert(ReferenceData referenceData)
        {
            if (referenceDatasDataTable == null)
            {
                referenceDatasDataTable = new DataTable();
                referenceDatasDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                referenceDatasDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                referenceDatasDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = referenceDatasDataTable.NewRow();
            dr["id"] = (object)referenceData.Id ?? DBNull.Value;
            dr["jsonb"] = (object)referenceData.Content ?? DBNull.Value;
            referenceDatasDataTable.Rows.Add(dr);
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

        public void Insert(RelatedInstanceType relatedInstanceType)
        {
            if (relatedInstanceTypesDataTable == null)
            {
                relatedInstanceTypesDataTable = new DataTable();
                relatedInstanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                relatedInstanceTypesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                relatedInstanceTypesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = relatedInstanceTypesDataTable.NewRow();
            dr["id"] = (object)relatedInstanceType.Id ?? DBNull.Value;
            dr["jsonb"] = (object)relatedInstanceType.Content ?? DBNull.Value;
            relatedInstanceTypesDataTable.Rows.Add(dr);
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

        public void Insert(Rollover rollover)
        {
            if (rolloversDataTable == null)
            {
                rolloversDataTable = new DataTable();
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerid", DataType = typeof(Guid) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "fromfiscalyearid", DataType = typeof(Guid) });
                rolloversDataTable.Columns.Add(new DataColumn { ColumnName = "tofiscalyearid", DataType = typeof(Guid) });
                rolloversDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = rolloversDataTable.NewRow();
            dr["id"] = (object)rollover.Id ?? DBNull.Value;
            dr["jsonb"] = (object)rollover.Content ?? DBNull.Value;
            dr["creation_date"] = (object)rollover.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)rollover.CreationUserId ?? DBNull.Value;
            dr["ledgerid"] = (object)rollover.Ledgerid ?? DBNull.Value;
            dr["fromfiscalyearid"] = (object)rollover.Fromfiscalyearid ?? DBNull.Value;
            dr["tofiscalyearid"] = (object)rollover.Tofiscalyearid ?? DBNull.Value;
            rolloversDataTable.Rows.Add(dr);
        }

        public void Insert(RolloverBudget rolloverBudget)
        {
            if (rolloverBudgetsDataTable == null)
            {
                rolloverBudgetsDataTable = new DataTable();
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerrolloverid", DataType = typeof(Guid) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fundid", DataType = typeof(Guid) });
                rolloverBudgetsDataTable.Columns.Add(new DataColumn { ColumnName = "fiscalyearid", DataType = typeof(Guid) });
                rolloverBudgetsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = rolloverBudgetsDataTable.NewRow();
            dr["id"] = (object)rolloverBudget.Id ?? DBNull.Value;
            dr["jsonb"] = (object)rolloverBudget.Content ?? DBNull.Value;
            dr["creation_date"] = (object)rolloverBudget.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)rolloverBudget.CreationUserId ?? DBNull.Value;
            dr["ledgerrolloverid"] = (object)rolloverBudget.Ledgerrolloverid ?? DBNull.Value;
            dr["fundid"] = (object)rolloverBudget.Fundid ?? DBNull.Value;
            dr["fiscalyearid"] = (object)rolloverBudget.Fiscalyearid ?? DBNull.Value;
            rolloverBudgetsDataTable.Rows.Add(dr);
        }

        public void Insert(RolloverError rolloverError)
        {
            if (rolloverErrorsDataTable == null)
            {
                rolloverErrorsDataTable = new DataTable();
                rolloverErrorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                rolloverErrorsDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                rolloverErrorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                rolloverErrorsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                rolloverErrorsDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerrolloverid", DataType = typeof(Guid) });
                rolloverErrorsDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = rolloverErrorsDataTable.NewRow();
            dr["id"] = (object)rolloverError.Id ?? DBNull.Value;
            dr["jsonb"] = (object)rolloverError.Content ?? DBNull.Value;
            dr["creation_date"] = (object)rolloverError.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)rolloverError.CreationUserId ?? DBNull.Value;
            dr["ledgerrolloverid"] = (object)rolloverError.Ledgerrolloverid ?? DBNull.Value;
            rolloverErrorsDataTable.Rows.Add(dr);
        }

        public void Insert(RolloverProgress rolloverProgress)
        {
            if (rolloverProgressesDataTable == null)
            {
                rolloverProgressesDataTable = new DataTable();
                rolloverProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                rolloverProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                rolloverProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                rolloverProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                rolloverProgressesDataTable.Columns.Add(new DataColumn { ColumnName = "ledgerrolloverid", DataType = typeof(Guid) });
                rolloverProgressesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = rolloverProgressesDataTable.NewRow();
            dr["id"] = (object)rolloverProgress.Id ?? DBNull.Value;
            dr["jsonb"] = (object)rolloverProgress.Content ?? DBNull.Value;
            dr["creation_date"] = (object)rolloverProgress.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)rolloverProgress.CreationUserId ?? DBNull.Value;
            dr["ledgerrolloverid"] = (object)rolloverProgress.Ledgerrolloverid ?? DBNull.Value;
            rolloverProgressesDataTable.Rows.Add(dr);
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
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "status", DataType = typeof(string) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "processing_started_date", DataType = typeof(DateTime) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by_user_id", DataType = typeof(Guid) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_by_user_id", DataType = typeof(Guid) });
                snapshotsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_date", DataType = typeof(DateTime) });
                
            }
            var dr = snapshotsDataTable.NewRow();
            dr["id"] = (object)snapshot.Id ?? DBNull.Value;
            dr["status"] = (object)snapshot.Status ?? DBNull.Value;
            dr["processing_started_date"] = (object)snapshot.ProcessingStartedDate ?? DBNull.Value;
            dr["created_by_user_id"] = (object)snapshot.CreationUserId ?? DBNull.Value;
            dr["created_date"] = (object)snapshot.CreationTime ?? DBNull.Value;
            dr["updated_by_user_id"] = (object)snapshot.LastWriteUserId ?? DBNull.Value;
            dr["updated_date"] = (object)snapshot.LastWriteTime ?? DBNull.Value;
            snapshotsDataTable.Rows.Add(dr);
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
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(Guid) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "label", DataType = typeof(string) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "description", DataType = typeof(string) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "created_date", DataType = typeof(DateTime) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_date", DataType = typeof(DateTime) });
                tagsDataTable.Columns.Add(new DataColumn { ColumnName = "updated_by", DataType = typeof(Guid) });
                
            }
            var dr = tagsDataTable.NewRow();
            dr["id"] = (object)tag.Id ?? DBNull.Value;
            dr["created_by"] = (object)tag.CreationUserId ?? DBNull.Value;
            dr["label"] = (object)tag.Label ?? DBNull.Value;
            dr["description"] = (object)tag.Description ?? DBNull.Value;
            dr["created_date"] = (object)tag.CreationTime ?? DBNull.Value;
            dr["updated_date"] = (object)tag.LastWriteTime ?? DBNull.Value;
            dr["updated_by"] = (object)tag.UpdatedBy ?? DBNull.Value;
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
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                titlesDataTable.Columns.Add(new DataColumn { ColumnName = "polineid", DataType = typeof(Guid) });
                titlesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = titlesDataTable.NewRow();
            dr["id"] = (object)title.Id ?? DBNull.Value;
            dr["jsonb"] = (object)title.Content ?? DBNull.Value;
            dr["creation_date"] = (object)title.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)title.CreationUserId ?? DBNull.Value;
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
                transactionsDataTable.Columns.Add(new DataColumn { ColumnName = "expenseclassid", DataType = typeof(Guid) });
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
            dr["expenseclassid"] = (object)transaction.Expenseclassid ?? DBNull.Value;
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

        public void Insert(UserCategory userCategory)
        {
            if (userCategoriesDataTable == null)
            {
                userCategoriesDataTable = new DataTable();
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(string) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "code", DataType = typeof(string) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                userCategoriesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = userCategoriesDataTable.NewRow();
            dr["id"] = (object)userCategory.Id ?? DBNull.Value;
            dr["name"] = (object)userCategory.Name ?? DBNull.Value;
            dr["code"] = (object)userCategory.Code ?? DBNull.Value;
            dr["creation_time"] = (object)userCategory.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)userCategory.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)userCategory.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)userCategory.LastWriteUsername ?? DBNull.Value;
            userCategoriesDataTable.Rows.Add(dr);
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

        public void Insert(UserSummary userSummary)
        {
            if (userSummariesDataTable == null)
            {
                userSummariesDataTable = new DataTable();
                userSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(Guid) });
                userSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "jsonb", DataType = typeof(string) });
                userSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_date", DataType = typeof(DateTime) });
                userSummariesDataTable.Columns.Add(new DataColumn { ColumnName = "created_by", DataType = typeof(string) });
                userSummariesDataTable.Columns["jsonb"].ExtendedProperties["NpgsqlDbType"] = NpgsqlDbType.Jsonb;
            }
            var dr = userSummariesDataTable.NewRow();
            dr["id"] = (object)userSummary.Id ?? DBNull.Value;
            dr["jsonb"] = (object)userSummary.Content ?? DBNull.Value;
            dr["creation_date"] = (object)userSummary.CreationTime ?? DBNull.Value;
            dr["created_by"] = (object)userSummary.CreationUserId ?? DBNull.Value;
            userSummariesDataTable.Rows.Add(dr);
        }

        public void Insert(Vendor vendor)
        {
            if (vendorsDataTable == null)
            {
                vendorsDataTable = new DataTable();
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "id2", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "number", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "code", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "email_address", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                vendorsDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = vendorsDataTable.NewRow();
            dr["id"] = (object)vendor.Id ?? DBNull.Value;
            dr["id2"] = (object)vendor.Id2 ?? DBNull.Value;
            dr["name"] = (object)vendor.Name ?? DBNull.Value;
            dr["number"] = (object)vendor.Number ?? DBNull.Value;
            dr["code"] = (object)vendor.Code ?? DBNull.Value;
            dr["email_address"] = (object)vendor.EmailAddress ?? DBNull.Value;
            dr["creation_time"] = (object)vendor.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)vendor.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)vendor.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)vendor.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)vendor.LongId ?? DBNull.Value;
            vendorsDataTable.Rows.Add(dr);
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

        public void Insert(Voucher3 voucher3)
        {
            if (voucher3sDataTable == null)
            {
                voucher3sDataTable = new DataTable();
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "invoice_date", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_invoice_id", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_invoice_amount", DataType = typeof(decimal) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "vendor_number", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "number", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "payment_type_id", DataType = typeof(int) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "enclosure", DataType = typeof(bool) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "manual", DataType = typeof(bool) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "voucher_status_id", DataType = typeof(int) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "notes", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "review_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "review_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "approve_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "cancel_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "export_id", DataType = typeof(int) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "export_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "check_number", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "completion_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "completion_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "mail_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                voucher3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = voucher3sDataTable.NewRow();
            dr["id"] = (object)voucher3.Id ?? DBNull.Value;
            dr["invoice_date"] = (object)voucher3.InvoiceDate ?? DBNull.Value;
            dr["vendor_invoice_id"] = (object)voucher3.VendorInvoiceId ?? DBNull.Value;
            dr["vendor_invoice_amount"] = (object)voucher3.VendorInvoiceAmount ?? DBNull.Value;
            dr["vendor_number"] = (object)voucher3.VendorNumber ?? DBNull.Value;
            dr["number"] = (object)voucher3.Number ?? DBNull.Value;
            dr["payment_type_id"] = (object)voucher3.PaymentTypeId ?? DBNull.Value;
            dr["enclosure"] = (object)voucher3.Enclosure ?? DBNull.Value;
            dr["manual"] = (object)voucher3.Manual ?? DBNull.Value;
            dr["voucher_status_id"] = (object)voucher3.VoucherStatusId ?? DBNull.Value;
            dr["notes"] = (object)voucher3.Notes ?? DBNull.Value;
            dr["review_time"] = (object)voucher3.ReviewTime ?? DBNull.Value;
            dr["review_username"] = (object)voucher3.ReviewUsername ?? DBNull.Value;
            dr["approve_time"] = (object)voucher3.ApproveTime ?? DBNull.Value;
            dr["approve_username"] = (object)voucher3.ApproveUsername ?? DBNull.Value;
            dr["cancel_time"] = (object)voucher3.CancelTime ?? DBNull.Value;
            dr["cancel_username"] = (object)voucher3.CancelUsername ?? DBNull.Value;
            dr["export_id"] = (object)voucher3.ExportId ?? DBNull.Value;
            dr["export_time"] = (object)voucher3.ExportTime ?? DBNull.Value;
            dr["check_number"] = (object)voucher3.CheckNumber ?? DBNull.Value;
            dr["completion_time"] = (object)voucher3.CompletionTime ?? DBNull.Value;
            dr["completion_username"] = (object)voucher3.CompletionUsername ?? DBNull.Value;
            dr["mail_time"] = (object)voucher3.MailTime ?? DBNull.Value;
            dr["creation_time"] = (object)voucher3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)voucher3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)voucher3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)voucher3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)voucher3.LongId ?? DBNull.Value;
            voucher3sDataTable.Rows.Add(dr);
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

        public void Insert(VoucherItem3 voucherItem3)
        {
            if (voucherItem3sDataTable == null)
            {
                voucherItem3sDataTable = new DataTable();
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "voucher_id", DataType = typeof(int) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "account_number", DataType = typeof(string) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "amount", DataType = typeof(decimal) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                voucherItem3sDataTable.Columns.Add(new DataColumn { ColumnName = "long_id", DataType = typeof(Guid) });
                
            }
            var dr = voucherItem3sDataTable.NewRow();
            dr["id"] = (object)voucherItem3.Id ?? DBNull.Value;
            dr["voucher_id"] = (object)voucherItem3.VoucherId ?? DBNull.Value;
            dr["account_number"] = (object)voucherItem3.AccountNumber ?? DBNull.Value;
            dr["amount"] = (object)voucherItem3.Amount ?? DBNull.Value;
            dr["creation_time"] = (object)voucherItem3.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)voucherItem3.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)voucherItem3.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)voucherItem3.LastWriteUsername ?? DBNull.Value;
            dr["long_id"] = (object)voucherItem3.LongId ?? DBNull.Value;
            voucherItem3sDataTable.Rows.Add(dr);
        }

        public void Insert(VoucherStatus voucherStatus)
        {
            if (voucherStatusesDataTable == null)
            {
                voucherStatusesDataTable = new DataTable();
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "id", DataType = typeof(int) });
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "name", DataType = typeof(string) });
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_time", DataType = typeof(DateTime) });
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "creation_username", DataType = typeof(string) });
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_time", DataType = typeof(DateTime) });
                voucherStatusesDataTable.Columns.Add(new DataColumn { ColumnName = "last_write_username", DataType = typeof(string) });
                
            }
            var dr = voucherStatusesDataTable.NewRow();
            dr["id"] = (object)voucherStatus.Id ?? DBNull.Value;
            dr["name"] = (object)voucherStatus.Name ?? DBNull.Value;
            dr["creation_time"] = (object)voucherStatus.CreationTime ?? DBNull.Value;
            dr["creation_username"] = (object)voucherStatus.CreationUsername ?? DBNull.Value;
            dr["last_write_time"] = (object)voucherStatus.LastWriteTime ?? DBNull.Value;
            dr["last_write_username"] = (object)voucherStatus.LastWriteUsername ?? DBNull.Value;
            voucherStatusesDataTable.Rows.Add(dr);
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
            if (acquisitionMethodsDataTable != null && acquisitionMethodsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}acquisition_method";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(acquisitionMethodsDataTable);
                acquisitionMethodsDataTable.Clear();
            }
            if (acquisitionsUnitsDataTable != null && acquisitionsUnitsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(acquisitionsUnitsDataTable);
                acquisitionsUnitsDataTable.Clear();
            }
            if (actualCostRecordsDataTable != null && actualCostRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}actual_cost_record";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(actualCostRecordsDataTable);
                actualCostRecordsDataTable.Clear();
            }
            if (addressTypesDataTable != null && addressTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}addresstype";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(addressTypesDataTable);
                addressTypesDataTable.Clear();
            }
            if (agreementsDataTable != null && agreementsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc_agreements{(IsMySql ? "_" : ".")}agreements";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(agreementsDataTable);
                agreementsDataTable.Clear();
            }
            if (agreementItemsDataTable != null && agreementItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc_agreements{(IsMySql ? "_" : ".")}agreement_items";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(agreementItemsDataTable);
                agreementItemsDataTable.Clear();
            }
            if (alertsDataTable != null && alertsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}alert";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(alternativeTitleTypesDataTable);
                alternativeTitleTypesDataTable.Clear();
            }
            if (authAttemptsDataTable != null && authAttemptsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_login{(IsMySql ? "_" : ".")}auth_attempts";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_login{(IsMySql ? "_" : ".")}auth_password_action";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(batchGroupsDataTable);
                batchGroupsDataTable.Clear();
            }
            if (blocksDataTable != null && blocksDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(blocksDataTable);
                blocksDataTable.Clear();
            }
            if (blockConditionsDataTable != null && blockConditionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(blockConditionsDataTable);
                blockConditionsDataTable.Clear();
            }
            if (blockLimitsDataTable != null && blockLimitsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("conditionid", "conditionid");
                sqlBulkCopy.WriteToServer(blockLimitsDataTable);
                blockLimitsDataTable.Clear();
            }
            if (boundWithPartsDataTable != null && boundWithPartsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}bound_with_part";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("itemid", "itemid");
                sqlBulkCopy.ColumnMappings.Add("holdingsrecordid", "holdingsrecordid");
                sqlBulkCopy.WriteToServer(boundWithPartsDataTable);
                boundWithPartsDataTable.Clear();
            }
            if (budgetsDataTable != null && budgetsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}budget";
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
            if (budgetExpenseClassesDataTable != null && budgetExpenseClassesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("budgetid", "budgetid");
                sqlBulkCopy.ColumnMappings.Add("expenseclassid", "expenseclassid");
                sqlBulkCopy.WriteToServer(budgetExpenseClassesDataTable);
                budgetExpenseClassesDataTable.Clear();
            }
            if (budgetGroupsDataTable != null && budgetGroupsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("budgetid", "budgetid");
                sqlBulkCopy.ColumnMappings.Add("groupid", "groupid");
                sqlBulkCopy.ColumnMappings.Add("fundid", "fundid");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.WriteToServer(budgetGroupsDataTable);
                budgetGroupsDataTable.Clear();
            }
            if (callNumberTypesDataTable != null && callNumberTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}categories";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("lock", "lock");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(circulationRulesDataTable);
                circulationRulesDataTable.Clear();
            }
            if (classificationTypesDataTable != null && classificationTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(classificationTypesDataTable);
                classificationTypesDataTable.Clear();
            }
            if (closeReasonsDataTable != null && closeReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(closeReasonsDataTable);
                closeReasonsDataTable.Clear();
            }
            if (commentsDataTable != null && commentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}comments";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_configuration{(IsMySql ? "_" : ".")}config_data";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(contactsDataTable);
                contactsDataTable.Clear();
            }
            if (contactTypesDataTable != null && contactTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}contact_types";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.WriteToServer(contactTypesDataTable);
                contactTypesDataTable.Clear();
            }
            if (contributorNameTypesDataTable != null && contributorNameTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type";
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
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("alpha2_code", "alpha2_code");
                sqlBulkCopy.ColumnMappings.Add("alpha3_code", "alpha3_code");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.WriteToServer(countriesDataTable);
                countriesDataTable.Clear();
            }
            if (customFieldsDataTable != null && customFieldsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}custom_fields";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(customFieldsDataTable);
                customFieldsDataTable.Clear();
            }
            if (departmentsDataTable != null && departmentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}departments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(departmentsDataTable);
                departmentsDataTable.Clear();
            }
            if (department3sDataTable != null && department3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}departments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(department3sDataTable);
                department3sDataTable.Clear();
            }
            if (divisionsDataTable != null && divisionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}divisions";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(divisionsDataTable);
                divisionsDataTable.Clear();
            }
            if (documentsDataTable != null && documentsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}documents";
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
            if (donorsDataTable != null && donorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("code", "code");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("report", "report");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("enabled", "enabled");
                sqlBulkCopy.ColumnMappings.Add("public_display", "public_display");
                sqlBulkCopy.ColumnMappings.Add("notes", "notes");
                sqlBulkCopy.WriteToServer(donorsDataTable);
                donorsDataTable.Clear();
            }
            if (donorFiscalYearsDataTable != null && donorFiscalYearsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}donor_fiscal_years";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("fiscal_year", "fiscal_year");
                sqlBulkCopy.ColumnMappings.Add("start_amount", "start_amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(donorFiscalYearsDataTable);
                donorFiscalYearsDataTable.Clear();
            }
            if (electronicAccessRelationshipsDataTable != null && electronicAccessRelationshipsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("content", "content");
                sqlBulkCopy.ColumnMappings.Add("description", "description");
                sqlBulkCopy.WriteToServer(errorRecordsDataTable);
                errorRecordsDataTable.Clear();
            }
            if (eventLogsDataTable != null && eventLogsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_login{(IsMySql ? "_" : ".")}event_logs";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(eventLogsDataTable);
                eventLogsDataTable.Clear();
            }
            if (expenseClassesDataTable != null && expenseClassesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(expenseClassesDataTable);
                expenseClassesDataTable.Clear();
            }
            if (feesDataTable != null && feesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}accounts";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}feefines";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}groups";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(fixedDueDateSchedulesDataTable);
                fixedDueDateSchedulesDataTable.Clear();
            }
            if (fundsDataTable != null && fundsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}fund";
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
            if (fund3sDataTable != null && fund3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}funds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("number", "number");
                sqlBulkCopy.ColumnMappings.Add("code", "code");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(fund3sDataTable);
                fund3sDataTable.Clear();
            }
            if (fundTypesDataTable != null && fundTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(fundTypesDataTable);
                fundTypesDataTable.Clear();
            }
            if (groupsDataTable != null && groupsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}groups";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("instanceid", "instanceid");
                sqlBulkCopy.ColumnMappings.Add("permanentlocationid", "permanentlocationid");
                sqlBulkCopy.ColumnMappings.Add("temporarylocationid", "temporarylocationid");
                sqlBulkCopy.ColumnMappings.Add("effectivelocationid", "effectivelocationid");
                sqlBulkCopy.ColumnMappings.Add("holdingstypeid", "holdingstypeid");
                sqlBulkCopy.ColumnMappings.Add("callnumbertypeid", "callnumbertypeid");
                sqlBulkCopy.ColumnMappings.Add("illpolicyid", "illpolicyid");
                sqlBulkCopy.ColumnMappings.Add("sourceid", "sourceid");
                sqlBulkCopy.WriteToServer(holdingsDataTable);
                holdingsDataTable.Clear();
            }
            if (holdingDonor2sDataTable != null && holdingDonor2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}holding_donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("holding_id", "holding_id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("report", "report");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.WriteToServer(holdingDonor2sDataTable);
                holdingDonor2sDataTable.Clear();
            }
            if (holdingNoteTypesDataTable != null && holdingNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("lock", "lock");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(hridSettingsDataTable);
                hridSettingsDataTable.Clear();
            }
            if (idTypesDataTable != null && idTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("instancestatusid", "instancestatusid");
                sqlBulkCopy.ColumnMappings.Add("modeofissuanceid", "modeofissuanceid");
                sqlBulkCopy.ColumnMappings.Add("instancetypeid", "instancetypeid");
                sqlBulkCopy.ColumnMappings.Add("complete_updated_date", "complete_updated_date");
                sqlBulkCopy.WriteToServer(instancesDataTable);
                instancesDataTable.Clear();
            }
            if (instanceFormatsDataTable != null && instanceFormatsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceFormatsDataTable);
                instanceFormatsDataTable.Clear();
            }
            if (instanceNoteTypesDataTable != null && instanceNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(instanceTypesDataTable);
                instanceTypesDataTable.Clear();
            }
            if (institutionsDataTable != null && institutionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("interfaceid", "interfaceid");
                sqlBulkCopy.WriteToServer(interfaceCredentialsDataTable);
                interfaceCredentialsDataTable.Clear();
            }
            if (invoicesDataTable != null && invoicesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("batchgroupid", "batchgroupid");
                sqlBulkCopy.WriteToServer(invoicesDataTable);
                invoicesDataTable.Clear();
            }
            if (invoice3sDataTable != null && invoice3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}invoices";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("vendor_id", "vendor_id");
                sqlBulkCopy.ColumnMappings.Add("invoice_date", "invoice_date");
                sqlBulkCopy.ColumnMappings.Add("vendor_invoice_id", "vendor_invoice_id");
                sqlBulkCopy.ColumnMappings.Add("vendor_invoice_amount", "vendor_invoice_amount");
                sqlBulkCopy.ColumnMappings.Add("invoice_status_id", "invoice_status_id");
                sqlBulkCopy.ColumnMappings.Add("fiscal_year", "fiscal_year");
                sqlBulkCopy.ColumnMappings.Add("approve_time", "approve_time");
                sqlBulkCopy.ColumnMappings.Add("approve_username", "approve_username");
                sqlBulkCopy.ColumnMappings.Add("cancel_time", "cancel_time");
                sqlBulkCopy.ColumnMappings.Add("cancel_username", "cancel_username");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(invoice3sDataTable);
                invoice3sDataTable.Clear();
            }
            if (invoiceItemsDataTable != null && invoiceItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("invoiceid", "invoiceid");
                sqlBulkCopy.WriteToServer(invoiceItemsDataTable);
                invoiceItemsDataTable.Clear();
            }
            if (invoiceItem3sDataTable != null && invoiceItem3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}invoice_items";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("invoice_id", "invoice_id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("quantity", "quantity");
                sqlBulkCopy.ColumnMappings.Add("order_item_id", "order_item_id");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(invoiceItem3sDataTable);
                invoiceItem3sDataTable.Clear();
            }
            if (invoiceItemDonorsDataTable != null && invoiceItemDonorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}invoice_item_donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("invoice_item_id", "invoice_item_id");
                sqlBulkCopy.ColumnMappings.Add("invoice_item_short_id", "invoice_item_short_id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("shipping_amount", "shipping_amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(invoiceItemDonorsDataTable);
                invoiceItemDonorsDataTable.Clear();
            }
            if (invoiceItemFund2sDataTable != null && invoiceItemFund2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}invoice_item_funds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("invoice_item_id", "invoice_item_id");
                sqlBulkCopy.ColumnMappings.Add("fund_id", "fund_id");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("shipping_amount", "shipping_amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(invoiceItemFund2sDataTable);
                invoiceItemFund2sDataTable.Clear();
            }
            if (invoiceStatusesDataTable != null && invoiceStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}invoice_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(invoiceStatusesDataTable);
                invoiceStatusesDataTable.Clear();
            }
            if (itemsDataTable != null && itemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}item";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(itemDamagedStatusesDataTable);
                itemDamagedStatusesDataTable.Clear();
            }
            if (itemDonor2sDataTable != null && itemDonor2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}item_donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("item_id", "item_id");
                sqlBulkCopy.ColumnMappings.Add("item_short_id", "item_short_id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("report", "report");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(itemDonor2sDataTable);
                itemDonor2sDataTable.Clear();
            }
            if (itemNoteTypesDataTable != null && itemNoteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(itemNoteTypesDataTable);
                itemNoteTypesDataTable.Clear();
            }
            if (itemOrderItemsDataTable != null && itemOrderItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}item_order_items";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("order_item_id", "order_item_id");
                sqlBulkCopy.WriteToServer(itemOrderItemsDataTable);
                itemOrderItemsDataTable.Clear();
            }
            if (itemStatusesDataTable != null && itemStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}item_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(itemStatusesDataTable);
                itemStatusesDataTable.Clear();
            }
            if (ledgersDataTable != null && ledgersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}ledger";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearoneid", "fiscalyearoneid");
                sqlBulkCopy.WriteToServer(ledgersDataTable);
                ledgersDataTable.Clear();
            }
            if (librariesDataTable != null && librariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("campusid", "campusid");
                sqlBulkCopy.WriteToServer(librariesDataTable);
                librariesDataTable.Clear();
            }
            if (linksDataTable != null && linksDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_notes{(IsMySql ? "_" : ".")}link";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("object_id", "object_id");
                sqlBulkCopy.ColumnMappings.Add("object_type", "object_type");
                sqlBulkCopy.WriteToServer(linksDataTable);
                linksDataTable.Clear();
            }
            if (loansDataTable != null && loansDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}loan";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(loansDataTable);
                loansDataTable.Clear();
            }
            if (loanEventsDataTable != null && loanEventsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(loanEventsDataTable);
                loanEventsDataTable.Clear();
            }
            if (loanPoliciesDataTable != null && loanPoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}location";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_login{(IsMySql ? "_" : ".")}auth_credentials";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(lostItemFeePoliciesDataTable);
                lostItemFeePoliciesDataTable.Clear();
            }
            if (manualBlockTemplatesDataTable != null && manualBlockTemplatesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}manual_block_templates";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(manualBlockTemplatesDataTable);
                manualBlockTemplatesDataTable.Clear();
            }
            if (marcRecordsDataTable != null && marcRecordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("content", "content");
                sqlBulkCopy.WriteToServer(marcRecordsDataTable);
                marcRecordsDataTable.Clear();
            }
            if (materialTypesDataTable != null && materialTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_notes{(IsMySql ? "_" : ".")}note";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("title", "title");
                sqlBulkCopy.ColumnMappings.Add("content", "content");
                sqlBulkCopy.ColumnMappings.Add("indexed_content", "indexed_content");
                sqlBulkCopy.ColumnMappings.Add("domain", "domain");
                sqlBulkCopy.ColumnMappings.Add("type_id", "type_id");
                sqlBulkCopy.ColumnMappings.Add("pop_up_on_user", "pop_up_on_user");
                sqlBulkCopy.ColumnMappings.Add("pop_up_on_check_out", "pop_up_on_check_out");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.ColumnMappings.Add("updated_by", "updated_by");
                sqlBulkCopy.ColumnMappings.Add("updated_date", "updated_date");
                sqlBulkCopy.WriteToServer(notesDataTable);
                notesDataTable.Clear();
            }
            if (noteLinksDataTable != null && noteLinksDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_notes{(IsMySql ? "_" : ".")}note_link";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("note_id", "note_id");
                sqlBulkCopy.ColumnMappings.Add("link_id", "link_id");
                sqlBulkCopy.WriteToServer(noteLinksDataTable);
                noteLinksDataTable.Clear();
            }
            if (noteTypesDataTable != null && noteTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_notes{(IsMySql ? "_" : ".")}type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.ColumnMappings.Add("updated_by", "updated_by");
                sqlBulkCopy.ColumnMappings.Add("updated_date", "updated_date");
                sqlBulkCopy.WriteToServer(noteTypesDataTable);
                noteTypesDataTable.Clear();
            }
            if (oclcHoldingsDataTable != null && oclcHoldingsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}oclc_holdings";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("number", "number");
                sqlBulkCopy.ColumnMappings.Add("instance_id", "instance_id");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.WriteToServer(oclcHoldingsDataTable);
                oclcHoldingsDataTable.Clear();
            }
            if (oclcHolding2sDataTable != null && oclcHolding2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}oclc_holdings2";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("number", "number");
                sqlBulkCopy.ColumnMappings.Add("instance_id", "instance_id");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.WriteToServer(oclcHolding2sDataTable);
                oclcHolding2sDataTable.Clear();
            }
            if (oclcNumber2sDataTable != null && oclcNumber2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}oclc_numbers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("instance_id", "instance_id");
                sqlBulkCopy.ColumnMappings.Add("content", "content");
                sqlBulkCopy.ColumnMappings.Add("holding_creation_time", "holding_creation_time");
                sqlBulkCopy.ColumnMappings.Add("invalid_time", "invalid_time");
                sqlBulkCopy.WriteToServer(oclcNumber2sDataTable);
                oclcNumber2sDataTable.Clear();
            }
            if (ordersDataTable != null && ordersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(ordersDataTable);
                ordersDataTable.Clear();
            }
            if (order3sDataTable != null && order3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}orders";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("document_id", "document_id");
                sqlBulkCopy.ColumnMappings.Add("vendor_id", "vendor_id");
                sqlBulkCopy.ColumnMappings.Add("order_date", "order_date");
                sqlBulkCopy.ColumnMappings.Add("order_type_id", "order_type_id");
                sqlBulkCopy.ColumnMappings.Add("order_status_id", "order_status_id");
                sqlBulkCopy.ColumnMappings.Add("fiscal_year", "fiscal_year");
                sqlBulkCopy.ColumnMappings.Add("vendor_customer_id", "vendor_customer_id");
                sqlBulkCopy.ColumnMappings.Add("delivery_room_id", "delivery_room_id");
                sqlBulkCopy.ColumnMappings.Add("approve_time", "approve_time");
                sqlBulkCopy.ColumnMappings.Add("approve_username", "approve_username");
                sqlBulkCopy.ColumnMappings.Add("cancel_time", "cancel_time");
                sqlBulkCopy.ColumnMappings.Add("cancel_username", "cancel_username");
                sqlBulkCopy.ColumnMappings.Add("freight_amount", "freight_amount");
                sqlBulkCopy.ColumnMappings.Add("shipping_amount", "shipping_amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(order3sDataTable);
                order3sDataTable.Clear();
            }
            if (orderInvoicesDataTable != null && orderInvoicesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("purchaseorderid", "purchaseorderid");
                sqlBulkCopy.WriteToServer(orderInvoicesDataTable);
                orderInvoicesDataTable.Clear();
            }
            if (orderItemsDataTable != null && orderItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}po_line";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("purchaseorderid", "purchaseorderid");
                sqlBulkCopy.WriteToServer(orderItemsDataTable);
                orderItemsDataTable.Clear();
            }
            if (orderItem3sDataTable != null && orderItem3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}order_items";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("order_id", "order_id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("unit_price", "unit_price");
                sqlBulkCopy.ColumnMappings.Add("quantity", "quantity");
                sqlBulkCopy.ColumnMappings.Add("vendor_item_id", "vendor_item_id");
                sqlBulkCopy.ColumnMappings.Add("vendor_notes", "vendor_notes");
                sqlBulkCopy.ColumnMappings.Add("special_notes", "special_notes");
                sqlBulkCopy.ColumnMappings.Add("miscellaneous_notes", "miscellaneous_notes");
                sqlBulkCopy.ColumnMappings.Add("selector_notes", "selector_notes");
                sqlBulkCopy.ColumnMappings.Add("instance_id", "instance_id");
                sqlBulkCopy.ColumnMappings.Add("holding_id", "holding_id");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.ColumnMappings.Add("receipt_status_id", "receipt_status_id");
                sqlBulkCopy.WriteToServer(orderItem3sDataTable);
                orderItem3sDataTable.Clear();
            }
            if (orderItemDonorsDataTable != null && orderItemDonorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}order_item_donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("order_item_id", "order_item_id");
                sqlBulkCopy.ColumnMappings.Add("order_item_short_id", "order_item_short_id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("report", "report");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(orderItemDonorsDataTable);
                orderItemDonorsDataTable.Clear();
            }
            if (orderItemFund3sDataTable != null && orderItemFund3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}order_item_funds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("order_item_id", "order_item_id");
                sqlBulkCopy.ColumnMappings.Add("fund_id", "fund_id");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(orderItemFund3sDataTable);
                orderItemFund3sDataTable.Clear();
            }
            if (orderStatusesDataTable != null && orderStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}order_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(orderStatusesDataTable);
                orderStatusesDataTable.Clear();
            }
            if (orderStatus2sDataTable != null && orderStatus2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}order_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(orderStatus2sDataTable);
                orderStatus2sDataTable.Clear();
            }
            if (orderTemplatesDataTable != null && orderTemplatesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(orderTemplatesDataTable);
                orderTemplatesDataTable.Clear();
            }
            if (orderTypesDataTable != null && orderTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}order_types";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(orderTypesDataTable);
                orderTypesDataTable.Clear();
            }
            if (orderType2sDataTable != null && orderType2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}order_types";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(orderType2sDataTable);
                orderType2sDataTable.Clear();
            }
            if (organizationsDataTable != null && organizationsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(organizationsDataTable);
                organizationsDataTable.Clear();
            }
            if (organizationType2sDataTable != null && organizationType2sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_organizations_storage{(IsMySql ? "_" : ".")}organization_types";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(organizationType2sDataTable);
                organizationType2sDataTable.Clear();
            }
            if (overdueFinePoliciesDataTable != null && overdueFinePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}owners";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(patronActionSessionsDataTable);
                patronActionSessionsDataTable.Clear();
            }
            if (patronNoticePoliciesDataTable != null && patronNoticePoliciesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}payments";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(paymentMethodsDataTable);
                paymentMethodsDataTable.Clear();
            }
            if (paymentTypesDataTable != null && paymentTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}payment_types";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(paymentTypesDataTable);
                paymentTypesDataTable.Clear();
            }
            if (permissionsDataTable != null && permissionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_permissions{(IsMySql ? "_" : ".")}permissions";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_permissions{(IsMySql ? "_" : ".")}permissions_users";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(permissionsUsersDataTable);
                permissionsUsersDataTable.Clear();
            }
            if (personDonorsDataTable != null && personDonorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}person_donors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("person_id", "person_id");
                sqlBulkCopy.ColumnMappings.Add("donor_id", "donor_id");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(personDonorsDataTable);
                personDonorsDataTable.Clear();
            }
            if (precedingSucceedingTitlesDataTable != null && precedingSucceedingTitlesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(prefixesDataTable);
                prefixesDataTable.Clear();
            }
            if (proxiesDataTable != null && proxiesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}proxyfor";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("content", "content");
                sqlBulkCopy.WriteToServer(rawRecordsDataTable);
                rawRecordsDataTable.Clear();
            }
            if (receiptStatusesDataTable != null && receiptStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}receipt_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(receiptStatusesDataTable);
                receiptStatusesDataTable.Clear();
            }
            if (receivingsDataTable != null && receivingsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}pieces";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("polineid", "polineid");
                sqlBulkCopy.ColumnMappings.Add("titleid", "titleid");
                sqlBulkCopy.WriteToServer(receivingsDataTable);
                receivingsDataTable.Clear();
            }
            if (recordsDataTable != null && recordsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("snapshot_id", "snapshot_id");
                sqlBulkCopy.ColumnMappings.Add("matched_id", "matched_id");
                sqlBulkCopy.ColumnMappings.Add("generation", "generation");
                sqlBulkCopy.ColumnMappings.Add("record_type", "record_type");
                sqlBulkCopy.ColumnMappings.Add("external_id", "external_id");
                sqlBulkCopy.ColumnMappings.Add("state", "state");
                sqlBulkCopy.ColumnMappings.Add("leader_record_status", "leader_record_status");
                sqlBulkCopy.ColumnMappings.Add("order", "order");
                sqlBulkCopy.ColumnMappings.Add("suppress_discovery", "suppress_discovery");
                sqlBulkCopy.ColumnMappings.Add("created_by_user_id", "created_by_user_id");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.ColumnMappings.Add("updated_by_user_id", "updated_by_user_id");
                sqlBulkCopy.ColumnMappings.Add("updated_date", "updated_date");
                sqlBulkCopy.ColumnMappings.Add("external_hrid", "external_hrid");
                sqlBulkCopy.WriteToServer(recordsDataTable);
                recordsDataTable.Clear();
            }
            if (referenceDatasDataTable != null && referenceDatasDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc_agreements{(IsMySql ? "_" : ".")}reference_datas";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(referenceDatasDataTable);
                referenceDatasDataTable.Clear();
            }
            if (refundReasonsDataTable != null && refundReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}refunds";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(refundReasonsDataTable);
                refundReasonsDataTable.Clear();
            }
            if (relatedInstanceTypesDataTable != null && relatedInstanceTypesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}related_instance_type";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(relatedInstanceTypesDataTable);
                relatedInstanceTypesDataTable.Clear();
            }
            if (reportingCodesDataTable != null && reportingCodesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}request";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(requestPoliciesDataTable);
                requestPoliciesDataTable.Clear();
            }
            if (rolloversDataTable != null && rolloversDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}ledger_fiscal_year_rollover";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ledgerid", "ledgerid");
                sqlBulkCopy.ColumnMappings.Add("fromfiscalyearid", "fromfiscalyearid");
                sqlBulkCopy.ColumnMappings.Add("tofiscalyearid", "tofiscalyearid");
                sqlBulkCopy.WriteToServer(rolloversDataTable);
                rolloversDataTable.Clear();
            }
            if (rolloverBudgetsDataTable != null && rolloverBudgetsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}ledger_fiscal_year_rollover_budget";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ledgerrolloverid", "ledgerrolloverid");
                sqlBulkCopy.ColumnMappings.Add("fundid", "fundid");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.WriteToServer(rolloverBudgetsDataTable);
                rolloverBudgetsDataTable.Clear();
            }
            if (rolloverErrorsDataTable != null && rolloverErrorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}ledger_fiscal_year_rollover_error";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ledgerrolloverid", "ledgerrolloverid");
                sqlBulkCopy.WriteToServer(rolloverErrorsDataTable);
                rolloverErrorsDataTable.Clear();
            }
            if (rolloverProgressesDataTable != null && rolloverProgressesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}ledger_fiscal_year_rollover_progress";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("ledgerrolloverid", "ledgerrolloverid");
                sqlBulkCopy.WriteToServer(rolloverProgressesDataTable);
                rolloverProgressesDataTable.Clear();
            }
            if (scheduledNoticesDataTable != null && scheduledNoticesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("status", "status");
                sqlBulkCopy.ColumnMappings.Add("processing_started_date", "processing_started_date");
                sqlBulkCopy.ColumnMappings.Add("created_by_user_id", "created_by_user_id");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.ColumnMappings.Add("updated_by_user_id", "updated_by_user_id");
                sqlBulkCopy.ColumnMappings.Add("updated_date", "updated_date");
                sqlBulkCopy.WriteToServer(snapshotsDataTable);
                snapshotsDataTable.Clear();
            }
            if (sourcesDataTable != null && sourcesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.WriteToServer(suffixesDataTable);
                suffixesDataTable.Clear();
            }
            if (tagsDataTable != null && tagsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_tags{(IsMySql ? "_" : ".")}tags";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("label", "label");
                sqlBulkCopy.ColumnMappings.Add("description", "description");
                sqlBulkCopy.ColumnMappings.Add("created_date", "created_date");
                sqlBulkCopy.ColumnMappings.Add("updated_date", "updated_date");
                sqlBulkCopy.ColumnMappings.Add("updated_by", "updated_by");
                sqlBulkCopy.WriteToServer(tagsDataTable);
                tagsDataTable.Clear();
            }
            if (templatesDataTable != null && templatesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_template_engine{(IsMySql ? "_" : ".")}template";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}titles";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("polineid", "polineid");
                sqlBulkCopy.WriteToServer(titlesDataTable);
                titlesDataTable.Clear();
            }
            if (transactionsDataTable != null && transactionsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_finance_storage{(IsMySql ? "_" : ".")}transaction";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("fiscalyearid", "fiscalyearid");
                sqlBulkCopy.ColumnMappings.Add("fromfundid", "fromfundid");
                sqlBulkCopy.ColumnMappings.Add("sourcefiscalyearid", "sourcefiscalyearid");
                sqlBulkCopy.ColumnMappings.Add("tofundid", "tofundid");
                sqlBulkCopy.ColumnMappings.Add("expenseclassid", "expenseclassid");
                sqlBulkCopy.WriteToServer(transactionsDataTable);
                transactionsDataTable.Clear();
            }
            if (transferAccountsDataTable != null && transferAccountsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}transfers";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_users{(IsMySql ? "_" : ".")}users";
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
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("acquisitionsunitid", "acquisitionsunitid");
                sqlBulkCopy.WriteToServer(userAcquisitionsUnitsDataTable);
                userAcquisitionsUnitsDataTable.Clear();
            }
            if (userCategoriesDataTable != null && userCategoriesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}user_categories";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("code", "code");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(userCategoriesDataTable);
                userCategoriesDataTable.Clear();
            }
            if (userRequestPreferencesDataTable != null && userRequestPreferencesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(userRequestPreferencesDataTable);
                userRequestPreferencesDataTable.Clear();
            }
            if (userSummariesDataTable != null && userSummariesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.WriteToServer(userSummariesDataTable);
                userSummariesDataTable.Clear();
            }
            if (vendorsDataTable != null && vendorsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}vendors";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("id2", "id2");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("number", "number");
                sqlBulkCopy.ColumnMappings.Add("code", "code");
                sqlBulkCopy.ColumnMappings.Add("email_address", "email_address");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(vendorsDataTable);
                vendorsDataTable.Clear();
            }
            if (vouchersDataTable != null && vouchersDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers";
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
            if (voucher3sDataTable != null && voucher3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}vouchers";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("invoice_date", "invoice_date");
                sqlBulkCopy.ColumnMappings.Add("vendor_invoice_id", "vendor_invoice_id");
                sqlBulkCopy.ColumnMappings.Add("vendor_invoice_amount", "vendor_invoice_amount");
                sqlBulkCopy.ColumnMappings.Add("vendor_number", "vendor_number");
                sqlBulkCopy.ColumnMappings.Add("number", "number");
                sqlBulkCopy.ColumnMappings.Add("payment_type_id", "payment_type_id");
                sqlBulkCopy.ColumnMappings.Add("enclosure", "enclosure");
                sqlBulkCopy.ColumnMappings.Add("manual", "manual");
                sqlBulkCopy.ColumnMappings.Add("voucher_status_id", "voucher_status_id");
                sqlBulkCopy.ColumnMappings.Add("notes", "notes");
                sqlBulkCopy.ColumnMappings.Add("review_time", "review_time");
                sqlBulkCopy.ColumnMappings.Add("review_username", "review_username");
                sqlBulkCopy.ColumnMappings.Add("approve_time", "approve_time");
                sqlBulkCopy.ColumnMappings.Add("approve_username", "approve_username");
                sqlBulkCopy.ColumnMappings.Add("cancel_time", "cancel_time");
                sqlBulkCopy.ColumnMappings.Add("cancel_username", "cancel_username");
                sqlBulkCopy.ColumnMappings.Add("export_id", "export_id");
                sqlBulkCopy.ColumnMappings.Add("export_time", "export_time");
                sqlBulkCopy.ColumnMappings.Add("check_number", "check_number");
                sqlBulkCopy.ColumnMappings.Add("completion_time", "completion_time");
                sqlBulkCopy.ColumnMappings.Add("completion_username", "completion_username");
                sqlBulkCopy.ColumnMappings.Add("mail_time", "mail_time");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(voucher3sDataTable);
                voucher3sDataTable.Clear();
            }
            if (voucherItemsDataTable != null && voucherItemsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("jsonb", "jsonb");
                sqlBulkCopy.ColumnMappings.Add("creation_date", "creation_date");
                sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                sqlBulkCopy.ColumnMappings.Add("voucherid", "voucherid");
                sqlBulkCopy.WriteToServer(voucherItemsDataTable);
                voucherItemsDataTable.Clear();
            }
            if (voucherItem3sDataTable != null && voucherItem3sDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"local{(IsMySql ? "_" : ".")}voucher_items";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("voucher_id", "voucher_id");
                sqlBulkCopy.ColumnMappings.Add("account_number", "account_number");
                sqlBulkCopy.ColumnMappings.Add("amount", "amount");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.ColumnMappings.Add("long_id", "long_id");
                sqlBulkCopy.WriteToServer(voucherItem3sDataTable);
                voucherItem3sDataTable.Clear();
            }
            if (voucherStatusesDataTable != null && voucherStatusesDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uc{(IsMySql ? "_" : ".")}voucher_statuses";
                sqlBulkCopy.ColumnMappings.Clear();
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("name", "name");
                sqlBulkCopy.ColumnMappings.Add("creation_time", "creation_time");
                sqlBulkCopy.ColumnMappings.Add("creation_username", "creation_username");
                sqlBulkCopy.ColumnMappings.Add("last_write_time", "last_write_time");
                sqlBulkCopy.ColumnMappings.Add("last_write_username", "last_write_username");
                sqlBulkCopy.WriteToServer(voucherStatusesDataTable);
                voucherStatusesDataTable.Clear();
            }
            if (waiveReasonsDataTable != null && waiveReasonsDataTable.Rows.Count > 0)
            {
                sqlBulkCopy.DestinationTableName = $"uchicago_mod_feesfines{(IsMySql ? "_" : ".")}waives";
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
            if (acquisitionMethodsDataTable != null) acquisitionMethodsDataTable.Dispose();
            if (acquisitionsUnitsDataTable != null) acquisitionsUnitsDataTable.Dispose();
            if (actualCostRecordsDataTable != null) actualCostRecordsDataTable.Dispose();
            if (addressTypesDataTable != null) addressTypesDataTable.Dispose();
            if (agreementsDataTable != null) agreementsDataTable.Dispose();
            if (agreementItemsDataTable != null) agreementItemsDataTable.Dispose();
            if (alertsDataTable != null) alertsDataTable.Dispose();
            if (alternativeTitleTypesDataTable != null) alternativeTitleTypesDataTable.Dispose();
            if (authAttemptsDataTable != null) authAttemptsDataTable.Dispose();
            if (authCredentialsHistoriesDataTable != null) authCredentialsHistoriesDataTable.Dispose();
            if (authPasswordActionsDataTable != null) authPasswordActionsDataTable.Dispose();
            if (batchGroupsDataTable != null) batchGroupsDataTable.Dispose();
            if (blocksDataTable != null) blocksDataTable.Dispose();
            if (blockConditionsDataTable != null) blockConditionsDataTable.Dispose();
            if (blockLimitsDataTable != null) blockLimitsDataTable.Dispose();
            if (boundWithPartsDataTable != null) boundWithPartsDataTable.Dispose();
            if (budgetsDataTable != null) budgetsDataTable.Dispose();
            if (budgetExpenseClassesDataTable != null) budgetExpenseClassesDataTable.Dispose();
            if (budgetGroupsDataTable != null) budgetGroupsDataTable.Dispose();
            if (callNumberTypesDataTable != null) callNumberTypesDataTable.Dispose();
            if (campusesDataTable != null) campusesDataTable.Dispose();
            if (cancellationReasonsDataTable != null) cancellationReasonsDataTable.Dispose();
            if (categoriesDataTable != null) categoriesDataTable.Dispose();
            if (checkInsDataTable != null) checkInsDataTable.Dispose();
            if (circulationRulesDataTable != null) circulationRulesDataTable.Dispose();
            if (classificationTypesDataTable != null) classificationTypesDataTable.Dispose();
            if (closeReasonsDataTable != null) closeReasonsDataTable.Dispose();
            if (commentsDataTable != null) commentsDataTable.Dispose();
            if (configurationsDataTable != null) configurationsDataTable.Dispose();
            if (contactsDataTable != null) contactsDataTable.Dispose();
            if (contactTypesDataTable != null) contactTypesDataTable.Dispose();
            if (contributorNameTypesDataTable != null) contributorNameTypesDataTable.Dispose();
            if (contributorTypesDataTable != null) contributorTypesDataTable.Dispose();
            if (countriesDataTable != null) countriesDataTable.Dispose();
            if (customFieldsDataTable != null) customFieldsDataTable.Dispose();
            if (departmentsDataTable != null) departmentsDataTable.Dispose();
            if (department3sDataTable != null) department3sDataTable.Dispose();
            if (divisionsDataTable != null) divisionsDataTable.Dispose();
            if (documentsDataTable != null) documentsDataTable.Dispose();
            if (donorsDataTable != null) donorsDataTable.Dispose();
            if (donorFiscalYearsDataTable != null) donorFiscalYearsDataTable.Dispose();
            if (electronicAccessRelationshipsDataTable != null) electronicAccessRelationshipsDataTable.Dispose();
            if (errorRecordsDataTable != null) errorRecordsDataTable.Dispose();
            if (eventLogsDataTable != null) eventLogsDataTable.Dispose();
            if (expenseClassesDataTable != null) expenseClassesDataTable.Dispose();
            if (feesDataTable != null) feesDataTable.Dispose();
            if (feeTypesDataTable != null) feeTypesDataTable.Dispose();
            if (financeGroupsDataTable != null) financeGroupsDataTable.Dispose();
            if (fiscalYearsDataTable != null) fiscalYearsDataTable.Dispose();
            if (fixedDueDateSchedulesDataTable != null) fixedDueDateSchedulesDataTable.Dispose();
            if (fundsDataTable != null) fundsDataTable.Dispose();
            if (fund3sDataTable != null) fund3sDataTable.Dispose();
            if (fundTypesDataTable != null) fundTypesDataTable.Dispose();
            if (groupsDataTable != null) groupsDataTable.Dispose();
            if (holdingsDataTable != null) holdingsDataTable.Dispose();
            if (holdingDonor2sDataTable != null) holdingDonor2sDataTable.Dispose();
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
            if (invoice3sDataTable != null) invoice3sDataTable.Dispose();
            if (invoiceItemsDataTable != null) invoiceItemsDataTable.Dispose();
            if (invoiceItem3sDataTable != null) invoiceItem3sDataTable.Dispose();
            if (invoiceItemDonorsDataTable != null) invoiceItemDonorsDataTable.Dispose();
            if (invoiceItemFund2sDataTable != null) invoiceItemFund2sDataTable.Dispose();
            if (invoiceStatusesDataTable != null) invoiceStatusesDataTable.Dispose();
            if (itemsDataTable != null) itemsDataTable.Dispose();
            if (itemDamagedStatusesDataTable != null) itemDamagedStatusesDataTable.Dispose();
            if (itemDonor2sDataTable != null) itemDonor2sDataTable.Dispose();
            if (itemNoteTypesDataTable != null) itemNoteTypesDataTable.Dispose();
            if (itemOrderItemsDataTable != null) itemOrderItemsDataTable.Dispose();
            if (itemStatusesDataTable != null) itemStatusesDataTable.Dispose();
            if (ledgersDataTable != null) ledgersDataTable.Dispose();
            if (librariesDataTable != null) librariesDataTable.Dispose();
            if (linksDataTable != null) linksDataTable.Dispose();
            if (loansDataTable != null) loansDataTable.Dispose();
            if (loanEventsDataTable != null) loanEventsDataTable.Dispose();
            if (loanPoliciesDataTable != null) loanPoliciesDataTable.Dispose();
            if (loanTypesDataTable != null) loanTypesDataTable.Dispose();
            if (locationsDataTable != null) locationsDataTable.Dispose();
            if (loginsDataTable != null) loginsDataTable.Dispose();
            if (lostItemFeePoliciesDataTable != null) lostItemFeePoliciesDataTable.Dispose();
            if (manualBlockTemplatesDataTable != null) manualBlockTemplatesDataTable.Dispose();
            if (marcRecordsDataTable != null) marcRecordsDataTable.Dispose();
            if (materialTypesDataTable != null) materialTypesDataTable.Dispose();
            if (modeOfIssuancesDataTable != null) modeOfIssuancesDataTable.Dispose();
            if (natureOfContentTermsDataTable != null) natureOfContentTermsDataTable.Dispose();
            if (notesDataTable != null) notesDataTable.Dispose();
            if (noteLinksDataTable != null) noteLinksDataTable.Dispose();
            if (noteTypesDataTable != null) noteTypesDataTable.Dispose();
            if (oclcHoldingsDataTable != null) oclcHoldingsDataTable.Dispose();
            if (oclcHolding2sDataTable != null) oclcHolding2sDataTable.Dispose();
            if (oclcNumber2sDataTable != null) oclcNumber2sDataTable.Dispose();
            if (ordersDataTable != null) ordersDataTable.Dispose();
            if (order3sDataTable != null) order3sDataTable.Dispose();
            if (orderInvoicesDataTable != null) orderInvoicesDataTable.Dispose();
            if (orderItemsDataTable != null) orderItemsDataTable.Dispose();
            if (orderItem3sDataTable != null) orderItem3sDataTable.Dispose();
            if (orderItemDonorsDataTable != null) orderItemDonorsDataTable.Dispose();
            if (orderItemFund3sDataTable != null) orderItemFund3sDataTable.Dispose();
            if (orderStatusesDataTable != null) orderStatusesDataTable.Dispose();
            if (orderStatus2sDataTable != null) orderStatus2sDataTable.Dispose();
            if (orderTemplatesDataTable != null) orderTemplatesDataTable.Dispose();
            if (orderTypesDataTable != null) orderTypesDataTable.Dispose();
            if (orderType2sDataTable != null) orderType2sDataTable.Dispose();
            if (organizationsDataTable != null) organizationsDataTable.Dispose();
            if (organizationType2sDataTable != null) organizationType2sDataTable.Dispose();
            if (overdueFinePoliciesDataTable != null) overdueFinePoliciesDataTable.Dispose();
            if (ownersDataTable != null) ownersDataTable.Dispose();
            if (patronActionSessionsDataTable != null) patronActionSessionsDataTable.Dispose();
            if (patronNoticePoliciesDataTable != null) patronNoticePoliciesDataTable.Dispose();
            if (paymentsDataTable != null) paymentsDataTable.Dispose();
            if (paymentMethodsDataTable != null) paymentMethodsDataTable.Dispose();
            if (paymentTypesDataTable != null) paymentTypesDataTable.Dispose();
            if (permissionsDataTable != null) permissionsDataTable.Dispose();
            if (permissionsUsersDataTable != null) permissionsUsersDataTable.Dispose();
            if (personDonorsDataTable != null) personDonorsDataTable.Dispose();
            if (precedingSucceedingTitlesDataTable != null) precedingSucceedingTitlesDataTable.Dispose();
            if (prefixesDataTable != null) prefixesDataTable.Dispose();
            if (proxiesDataTable != null) proxiesDataTable.Dispose();
            if (rawRecordsDataTable != null) rawRecordsDataTable.Dispose();
            if (receiptStatusesDataTable != null) receiptStatusesDataTable.Dispose();
            if (receivingsDataTable != null) receivingsDataTable.Dispose();
            if (recordsDataTable != null) recordsDataTable.Dispose();
            if (referenceDatasDataTable != null) referenceDatasDataTable.Dispose();
            if (refundReasonsDataTable != null) refundReasonsDataTable.Dispose();
            if (relatedInstanceTypesDataTable != null) relatedInstanceTypesDataTable.Dispose();
            if (reportingCodesDataTable != null) reportingCodesDataTable.Dispose();
            if (requestsDataTable != null) requestsDataTable.Dispose();
            if (requestPoliciesDataTable != null) requestPoliciesDataTable.Dispose();
            if (rolloversDataTable != null) rolloversDataTable.Dispose();
            if (rolloverBudgetsDataTable != null) rolloverBudgetsDataTable.Dispose();
            if (rolloverErrorsDataTable != null) rolloverErrorsDataTable.Dispose();
            if (rolloverProgressesDataTable != null) rolloverProgressesDataTable.Dispose();
            if (scheduledNoticesDataTable != null) scheduledNoticesDataTable.Dispose();
            if (servicePointsDataTable != null) servicePointsDataTable.Dispose();
            if (servicePointUsersDataTable != null) servicePointUsersDataTable.Dispose();
            if (snapshotsDataTable != null) snapshotsDataTable.Dispose();
            if (sourcesDataTable != null) sourcesDataTable.Dispose();
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
            if (userCategoriesDataTable != null) userCategoriesDataTable.Dispose();
            if (userRequestPreferencesDataTable != null) userRequestPreferencesDataTable.Dispose();
            if (userSummariesDataTable != null) userSummariesDataTable.Dispose();
            if (vendorsDataTable != null) vendorsDataTable.Dispose();
            if (vouchersDataTable != null) vouchersDataTable.Dispose();
            if (voucher3sDataTable != null) voucher3sDataTable.Dispose();
            if (voucherItemsDataTable != null) voucherItemsDataTable.Dispose();
            if (voucherItem3sDataTable != null) voucherItem3sDataTable.Dispose();
            if (voucherStatusesDataTable != null) voucherStatusesDataTable.Dispose();
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
    
    public class PostgreSqlBulkCopy : IDisposable
    {
        private string connectionString;
        private DbConnection dbConnection;
        private DbTransaction dbTransaction;
        private string providerName;
        public static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public PostgreSqlBulkCopy(string name, bool checkConstraints)
        {
            providerName = "Npgsql";
            connectionString = ConfigurationManager.ConnectionStrings[name]?.ConnectionString ?? name;
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
