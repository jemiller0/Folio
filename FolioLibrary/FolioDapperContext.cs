using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FolioLibrary
{
    public class FolioDapperContext : IDisposable
    {
        private string connectionString;
        public int? CommandTimeout { get; set; } = 30;
        private DbConnection dbConnection;
        private DbTransaction dbTransaction;
        private string providerName;
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";

        public FolioDapperContext(string nameOrConnectionString = "FolioContext", string providerName = "Npgsql")
        {
            this.providerName = ConfigurationManager.ConnectionStrings[nameOrConnectionString]?.ProviderName ?? providerName ?? "Npgsql";
            connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString]?.ConnectionString ?? nameOrConnectionString;
        }

        private DbConnection Connection
        {
            get
            {
                if (dbConnection == null)
                {
                    dbConnection = DbProviderFactories.GetFactory(IsMySql ? "MySql.Data.MySqlClient2" : providerName).CreateConnection();
                    dbConnection.ConnectionString = connectionString;
                    dbConnection.Open();
                    if (IsMySql) dbConnection.Execute("SET SQL_MODE = 'ANSI'");
                }
                return dbConnection;
            }
        }

        private DbTransaction Transaction
        {
            get
            {
                if (dbTransaction == null) dbTransaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                return dbTransaction;
            }
        }

        public AcquisitionsUnit FindAcquisitionsUnit(Guid? id, bool load = false) => Query<AcquisitionsUnit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit WHERE id = @id", new { id }).SingleOrDefault();
        public AcquisitionsUnit2 FindAcquisitionsUnit2(Guid? id, bool load = false)
        {
            var au2 = Query<AcquisitionsUnit2>($"SELECT id AS \"Id\", name AS \"Name\", is_deleted AS \"IsDeleted\", protect_create AS \"ProtectCreate\", protect_read AS \"ProtectRead\", protect_update AS \"ProtectUpdate\", protect_delete AS \"ProtectDelete\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (au2 == null) return null;
            if (load && au2.CreationUserId != null) au2.CreationUser = FindUser2(au2.CreationUserId);
            if (load && au2.LastWriteUserId != null) au2.LastWriteUser = FindUser2(au2.LastWriteUserId);
            return au2;
        }
        public Address FindAddress(Guid? id, bool load = false)
        {
            var a = Query<Address>($"SELECT id AS \"Id\", name AS \"Name\", content AS \"Content\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}addresses WHERE id = @id", new { id }).SingleOrDefault();
            if (a == null) return null;
            if (load && a.CreationUserId != null) a.CreationUser = FindUser2(a.CreationUserId);
            if (load && a.LastWriteUserId != null) a.LastWriteUser = FindUser2(a.LastWriteUserId);
            return a;
        }
        public AddressType FindAddressType(Guid? id, bool load = false) => Query<AddressType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}addresstype WHERE id = @id", new { id }).SingleOrDefault();
        public AddressType2 FindAddressType2(Guid? id, bool load = false)
        {
            var at2 = Query<AddressType2>($"SELECT id AS \"Id\", address_type AS \"Name\", \"desc\" AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}address_types WHERE id = @id", new { id }).SingleOrDefault();
            if (at2 == null) return null;
            if (load && at2.CreationUserId != null) at2.CreationUser = FindUser2(at2.CreationUserId);
            if (load && at2.LastWriteUserId != null) at2.LastWriteUser = FindUser2(at2.LastWriteUserId);
            return at2;
        }
        public Alert FindAlert(Guid? id, bool load = false) => Query<Alert>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert WHERE id = @id", new { id }).SingleOrDefault();
        public Alert2 FindAlert2(Guid? id, bool load = false) => Query<Alert2>($"SELECT id AS \"Id\", alert AS \"Alert\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alerts WHERE id = @id", new { id }).SingleOrDefault();
        public AllocatedFromFund FindAllocatedFromFund(string id, bool load = false)
        {
            var aff = Query<AllocatedFromFund>($"SELECT id AS \"Id\", fund_id AS \"FundId\", from_fund_id AS \"FromFundId\" FROM uc{(IsMySql ? "_" : ".")}allocated_from_funds WHERE id = @id", new { id }).SingleOrDefault();
            if (aff == null) return null;
            if (load && aff.FundId != null) aff.Fund = FindFund2(aff.FundId);
            if (load && aff.FromFundId != null) aff.FromFund = FindFund2(aff.FromFundId);
            return aff;
        }
        public AllocatedToFund FindAllocatedToFund(string id, bool load = false)
        {
            var atf = Query<AllocatedToFund>($"SELECT id AS \"Id\", fund_id AS \"FundId\", to_fund_id AS \"ToFundId\" FROM uc{(IsMySql ? "_" : ".")}allocated_to_funds WHERE id = @id", new { id }).SingleOrDefault();
            if (atf == null) return null;
            if (load && atf.FundId != null) atf.Fund = FindFund2(atf.FundId);
            if (load && atf.ToFundId != null) atf.ToFund = FindFund2(atf.ToFundId);
            return atf;
        }
        public AlternativeTitle FindAlternativeTitle(string id, Guid? instanceId, bool load = false)
        {
            var at = Query<AlternativeTitle>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", alternative_title_type_id AS \"AlternativeTitleTypeId\", alternative_title AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alternative_titles WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (at == null) return null;
            if (load && at.InstanceId != null) at.Instance = FindInstance2(at.InstanceId);
            if (load && at.AlternativeTitleTypeId != null) at.AlternativeTitleType = FindAlternativeTitleType2(at.AlternativeTitleTypeId);
            return at;
        }
        public AlternativeTitleType FindAlternativeTitleType(Guid? id, bool load = false) => Query<AlternativeTitleType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type WHERE id = @id", new { id }).SingleOrDefault();
        public AlternativeTitleType2 FindAlternativeTitleType2(Guid? id, bool load = false)
        {
            var att2 = Query<AlternativeTitleType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alternative_title_types WHERE id = @id", new { id }).SingleOrDefault();
            if (att2 == null) return null;
            if (load && att2.CreationUserId != null) att2.CreationUser = FindUser2(att2.CreationUserId);
            if (load && att2.LastWriteUserId != null) att2.LastWriteUser = FindUser2(att2.LastWriteUserId);
            return att2;
        }
        public AuditLoan FindAuditLoan(Guid? id, bool load = false) => Query<AuditLoan>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan WHERE id = @id", new { id }).SingleOrDefault();
        public AuthAttempt FindAuthAttempt(Guid? id, bool load = false) => Query<AuthAttempt>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts WHERE id = @id", new { id }).SingleOrDefault();
        public AuthAttempt2 FindAuthAttempt2(Guid? id, bool load = false)
        {
            var aa2 = Query<AuthAttempt2>($"SELECT id AS \"Id\", user_id AS \"UserId\", last_attempt AS \"LastAttempt\", attempt_count AS \"AttemptCount\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}auth_attempts WHERE id = @id", new { id }).SingleOrDefault();
            if (aa2 == null) return null;
            if (load && aa2.UserId != null) aa2.User = FindUser2(aa2.UserId);
            return aa2;
        }
        public AuthCredentialsHistory FindAuthCredentialsHistory(Guid? id, bool load = false) => Query<AuthCredentialsHistory>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history WHERE id = @id", new { id }).SingleOrDefault();
        public AuthCredentialsHistory2 FindAuthCredentialsHistory2(Guid? id, bool load = false)
        {
            var ach2 = Query<AuthCredentialsHistory2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hash AS \"Hash\", salt AS \"Salt\", date AS \"Date\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}auth_credentials_histories WHERE id = @id", new { id }).SingleOrDefault();
            if (ach2 == null) return null;
            if (load && ach2.UserId != null) ach2.User = FindUser2(ach2.UserId);
            if (load && ach2.CreationUserId != null) ach2.CreationUser = FindUser2(ach2.CreationUserId);
            if (load && ach2.LastWriteUserId != null) ach2.LastWriteUser = FindUser2(ach2.LastWriteUserId);
            return ach2;
        }
        public AuthPasswordAction FindAuthPasswordAction(Guid? id, bool load = false) => Query<AuthPasswordAction>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action WHERE id = @id", new { id }).SingleOrDefault();
        public BatchGroup FindBatchGroup(Guid? id, bool load = false) => Query<BatchGroup>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups WHERE id = @id", new { id }).SingleOrDefault();
        public BatchGroup2 FindBatchGroup2(Guid? id, bool load = false)
        {
            var bg2 = Query<BatchGroup2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_groups WHERE id = @id", new { id }).SingleOrDefault();
            if (bg2 == null) return null;
            if (load && bg2.CreationUserId != null) bg2.CreationUser = FindUser2(bg2.CreationUserId);
            if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = FindUser2(bg2.LastWriteUserId);
            return bg2;
        }
        public BatchVoucher FindBatchVoucher(Guid? id, bool load = false) => Query<BatchVoucher>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers WHERE id = @id", new { id }).SingleOrDefault();
        public BatchVoucher2 FindBatchVoucher2(Guid? id, bool load = false) => Query<BatchVoucher2>($"SELECT id AS \"Id\", batch_group AS \"BatchGroup\", created AS \"Created\", start AS \"Start\", \"end\" AS \"End\", total_records AS \"TotalRecords\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_vouchers WHERE id = @id", new { id }).SingleOrDefault();
        public BatchVoucherBatchedVoucher FindBatchVoucherBatchedVoucher(string id, bool load = false)
        {
            var bvbv = Query<BatchVoucherBatchedVoucher>($"SELECT id AS \"Id\", batch_voucher_id AS \"BatchVoucherId\", accounting_code AS \"AccountingCode\", amount AS \"Amount\", disbursement_number AS \"DisbursementNumber\", disbursement_date AS \"DisbursementDate\", disbursement_amount AS \"DisbursementAmount\", enclosure_needed AS \"EnclosureNeeded\", exchange_rate AS \"ExchangeRate\", folio_invoice_no AS \"FolioInvoiceNo\", invoice_currency AS \"InvoiceCurrency\", invoice_note AS \"InvoiceNote\", status AS \"Status\", system_currency AS \"SystemCurrency\", type AS \"Type\", vendor_invoice_no AS \"VendorInvoiceNo\", vendor_name AS \"VendorName\", voucher_date AS \"VoucherDate\", voucher_number AS \"VoucherNumber\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_vouchers WHERE id = @id", new { id }).SingleOrDefault();
            if (bvbv == null) return null;
            if (load && bvbv.BatchVoucherId != null) bvbv.BatchVoucher = FindBatchVoucher2(bvbv.BatchVoucherId);
            return bvbv;
        }
        public BatchVoucherBatchedVoucherBatchedVoucherLine FindBatchVoucherBatchedVoucherBatchedVoucherLine(string id, bool load = false)
        {
            var bvbvbvl = Query<BatchVoucherBatchedVoucherBatchedVoucherLine>($"SELECT id AS \"Id\", batch_voucher_batched_voucher_id AS \"BatchVoucherBatchedVoucherId\", amount AS \"Amount\", external_account_number AS \"ExternalAccountNumber\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_lines WHERE id = @id", new { id }).SingleOrDefault();
            if (bvbvbvl == null) return null;
            if (load && bvbvbvl.BatchVoucherBatchedVoucherId != null) bvbvbvl.BatchVoucherBatchedVoucher = FindBatchVoucherBatchedVoucher(bvbvbvl.BatchVoucherBatchedVoucherId);
            return bvbvbvl;
        }
        public BatchVoucherBatchedVoucherBatchedVoucherLineFundCode FindBatchVoucherBatchedVoucherBatchedVoucherLineFundCode(string id, bool load = false)
        {
            var bvbvbvlfc = Query<BatchVoucherBatchedVoucherBatchedVoucherLineFundCode>($"SELECT id AS \"Id\", batch_voucher_batched_voucher_batched_voucher_line_id AS \"BatchVoucherBatchedVoucherBatchedVoucherLineId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_line_fund_codes WHERE id = @id", new { id }).SingleOrDefault();
            if (bvbvbvlfc == null) return null;
            if (load && bvbvbvlfc.BatchVoucherBatchedVoucherBatchedVoucherLineId != null) bvbvbvlfc.BatchVoucherBatchedVoucherBatchedVoucherLine = FindBatchVoucherBatchedVoucherBatchedVoucherLine(bvbvbvlfc.BatchVoucherBatchedVoucherBatchedVoucherLineId);
            return bvbvbvlfc;
        }
        public BatchVoucherExport FindBatchVoucherExport(Guid? id, bool load = false)
        {
            var bve = Query<BatchVoucherExport>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\", batchvoucherid AS \"Batchvoucherid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports WHERE id = @id", new { id }).SingleOrDefault();
            if (bve == null) return null;
            if (load && bve.Batchgroupid != null) bve.BatchGroup = FindBatchGroup(bve.Batchgroupid);
            if (load && bve.Batchvoucherid != null) bve.BatchVoucher = FindBatchVoucher(bve.Batchvoucherid);
            return bve;
        }
        public BatchVoucherExport2 FindBatchVoucherExport2(Guid? id, bool load = false)
        {
            var bve2 = Query<BatchVoucherExport2>($"SELECT id AS \"Id\", status AS \"Status\", message AS \"Message\", batch_group_id AS \"BatchGroupId\", start AS \"Start\", \"end\" AS \"End\", batch_voucher_id AS \"BatchVoucherId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_exports WHERE id = @id", new { id }).SingleOrDefault();
            if (bve2 == null) return null;
            if (load && bve2.BatchGroupId != null) bve2.BatchGroup = FindBatchGroup2(bve2.BatchGroupId);
            if (load && bve2.BatchVoucherId != null) bve2.BatchVoucher = FindBatchVoucher2(bve2.BatchVoucherId);
            if (load && bve2.CreationUserId != null) bve2.CreationUser = FindUser2(bve2.CreationUserId);
            if (load && bve2.LastWriteUserId != null) bve2.LastWriteUser = FindUser2(bve2.LastWriteUserId);
            return bve2;
        }
        public BatchVoucherExportConfig FindBatchVoucherExportConfig(Guid? id, bool load = false)
        {
            var bvec = Query<BatchVoucherExportConfig>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs WHERE id = @id", new { id }).SingleOrDefault();
            if (bvec == null) return null;
            if (load && bvec.Batchgroupid != null) bvec.BatchGroup = FindBatchGroup(bvec.Batchgroupid);
            return bvec;
        }
        public BatchVoucherExportConfig2 FindBatchVoucherExportConfig2(Guid? id, bool load = false)
        {
            var bvec2 = Query<BatchVoucherExportConfig2>($"SELECT id AS \"Id\", batch_group_id AS \"BatchGroupId\", enable_scheduled_export AS \"EnableScheduledExport\", format AS \"Format\", start_time AS \"StartTime\", upload_uri AS \"UploadUri\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_configs WHERE id = @id", new { id }).SingleOrDefault();
            if (bvec2 == null) return null;
            if (load && bvec2.BatchGroupId != null) bvec2.BatchGroup = FindBatchGroup2(bvec2.BatchGroupId);
            if (load && bvec2.CreationUserId != null) bvec2.CreationUser = FindUser2(bvec2.CreationUserId);
            if (load && bvec2.LastWriteUserId != null) bvec2.LastWriteUser = FindUser2(bvec2.LastWriteUserId);
            return bvec2;
        }
        public BatchVoucherExportConfigWeekday FindBatchVoucherExportConfigWeekday(string id, bool load = false)
        {
            var bvecw = Query<BatchVoucherExportConfigWeekday>($"SELECT id AS \"Id\", batch_voucher_export_config_id AS \"BatchVoucherExportConfigId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_config_weekdays WHERE id = @id", new { id }).SingleOrDefault();
            if (bvecw == null) return null;
            if (load && bvecw.BatchVoucherExportConfigId != null) bvecw.BatchVoucherExportConfig = FindBatchVoucherExportConfig2(bvecw.BatchVoucherExportConfigId);
            return bvecw;
        }
        public Block FindBlock(Guid? id, bool load = false) => Query<Block>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks WHERE id = @id", new { id }).SingleOrDefault();
        public Block2 FindBlock2(Guid? id, bool load = false)
        {
            var b2 = Query<Block2>($"SELECT id AS \"Id\", type AS \"Type\", \"desc\" AS \"Desc\", staff_information AS \"StaffInformation\", patron_message AS \"PatronMessage\", expiration_date AS \"ExpirationDate\", borrowing AS \"Borrowing\", renewals AS \"Renewals\", requests AS \"Requests\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}blocks WHERE id = @id", new { id }).SingleOrDefault();
            if (b2 == null) return null;
            if (load && b2.UserId != null) b2.User = FindUser2(b2.UserId);
            if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId);
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId);
            return b2;
        }
        public BlockCondition FindBlockCondition(Guid? id, bool load = false) => Query<BlockCondition>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions WHERE id = @id", new { id }).SingleOrDefault();
        public BlockCondition2 FindBlockCondition2(Guid? id, bool load = false)
        {
            var bc2 = Query<BlockCondition2>($"SELECT id AS \"Id\", name AS \"Name\", block_borrowing AS \"BlockBorrowing\", block_renewals AS \"BlockRenewals\", block_requests AS \"BlockRequests\", value_type AS \"ValueType\", message AS \"Message\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}block_conditions WHERE id = @id", new { id }).SingleOrDefault();
            if (bc2 == null) return null;
            if (load && bc2.CreationUserId != null) bc2.CreationUser = FindUser2(bc2.CreationUserId);
            if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = FindUser2(bc2.LastWriteUserId);
            return bc2;
        }
        public BlockLimit FindBlockLimit(Guid? id, bool load = false)
        {
            var bl = Query<BlockLimit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", conditionid AS \"Conditionid\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits WHERE id = @id", new { id }).SingleOrDefault();
            if (bl == null) return null;
            if (load && bl.Conditionid != null) bl.BlockCondition = FindBlockCondition(bl.Conditionid);
            return bl;
        }
        public BlockLimit2 FindBlockLimit2(Guid? id, bool load = false)
        {
            var bl2 = Query<BlockLimit2>($"SELECT id AS \"Id\", group_id AS \"GroupId\", condition_id AS \"ConditionId\", value AS \"Value\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", conditionid AS \"Conditionid\" FROM uc{(IsMySql ? "_" : ".")}block_limits WHERE id = @id", new { id }).SingleOrDefault();
            if (bl2 == null) return null;
            if (load && bl2.GroupId != null) bl2.Group = FindGroup2(bl2.GroupId);
            if (load && bl2.ConditionId != null) bl2.Condition = FindBlockCondition2(bl2.ConditionId);
            if (load && bl2.CreationUserId != null) bl2.CreationUser = FindUser2(bl2.CreationUserId);
            if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = FindUser2(bl2.LastWriteUserId);
            return bl2;
        }
        public Budget FindBudget(Guid? id, bool load = false)
        {
            var b = Query<Budget>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fundid AS \"FundId\", fiscalyearid AS \"FiscalYearId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget WHERE id = @id", new { id }).SingleOrDefault();
            if (b == null) return null;
            if (load && b.FundId != null) b.Fund = FindFund(b.FundId);
            if (load && b.FiscalYearId != null) b.FiscalYear = FindFiscalYear(b.FiscalYearId);
            return b;
        }
        public Budget2 FindBudget2(Guid? id, bool load = false)
        {
            var b2 = Query<Budget2>($"SELECT id AS \"Id\", name AS \"Name\", budget_status AS \"BudgetStatus\", allowable_encumbrance AS \"AllowableEncumbrance\", allowable_expenditure AS \"AllowableExpenditure\", allocated AS \"Allocated\", awaiting_payment AS \"AwaitingPayment\", available AS \"Available\", encumbered AS \"Encumbered\", expenditures AS \"Expenditures\", net_transfers AS \"NetTransfers\", unavailable AS \"Unavailable\", over_encumbrance AS \"OverEncumbrance\", over_expended AS \"OverExpended\", fund_id AS \"FundId\", fiscal_year_id AS \"FiscalYearId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}budgets WHERE id = @id", new { id }).SingleOrDefault();
            if (b2 == null) return null;
            if (load && b2.FundId != null) b2.Fund = FindFund2(b2.FundId);
            if (load && b2.FiscalYearId != null) b2.FiscalYear = FindFiscalYear2(b2.FiscalYearId);
            if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId);
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId);
            return b2;
        }
        public BudgetAcquisitionsUnit FindBudgetAcquisitionsUnit(string id, bool load = false)
        {
            var bau = Query<BudgetAcquisitionsUnit>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}budget_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (bau == null) return null;
            if (load && bau.BudgetId != null) bau.Budget = FindBudget2(bau.BudgetId);
            if (load && bau.AcquisitionsUnitId != null) bau.AcquisitionsUnit = FindAcquisitionsUnit2(bau.AcquisitionsUnitId);
            return bau;
        }
        public BudgetExpenseClass FindBudgetExpenseClass(Guid? id, bool load = false)
        {
            var bec = Query<BudgetExpenseClass>($"SELECT id AS \"Id\", jsonb AS \"Content\", budgetid AS \"Budgetid\", expenseclassid AS \"Expenseclassid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class WHERE id = @id", new { id }).SingleOrDefault();
            if (bec == null) return null;
            if (load && bec.Budgetid != null) bec.Budget = FindBudget(bec.Budgetid);
            if (load && bec.Expenseclassid != null) bec.ExpenseClass = FindExpenseClass(bec.Expenseclassid);
            return bec;
        }
        public BudgetExpenseClass2 FindBudgetExpenseClass2(Guid? id, bool load = false)
        {
            var bec2 = Query<BudgetExpenseClass2>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", expense_class_id AS \"ExpenseClassId\", status AS \"Status\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}budget_expense_classes WHERE id = @id", new { id }).SingleOrDefault();
            if (bec2 == null) return null;
            if (load && bec2.BudgetId != null) bec2.Budget = FindBudget2(bec2.BudgetId);
            if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = FindExpenseClass2(bec2.ExpenseClassId);
            return bec2;
        }
        public BudgetTag FindBudgetTag(string id, bool load = false)
        {
            var bt = Query<BudgetTag>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", tag_id AS \"TagId\" FROM uc{(IsMySql ? "_" : ".")}budget_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (bt == null) return null;
            if (load && bt.BudgetId != null) bt.Budget = FindBudget2(bt.BudgetId);
            if (load && bt.TagId != null) bt.Tag = FindTag2(bt.TagId);
            return bt;
        }
        public CallNumberType FindCallNumberType(Guid? id, bool load = false) => Query<CallNumberType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type WHERE id = @id", new { id }).SingleOrDefault();
        public CallNumberType2 FindCallNumberType2(Guid? id, bool load = false)
        {
            var cnt2 = Query<CallNumberType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}call_number_types WHERE id = @id", new { id }).SingleOrDefault();
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId);
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId);
            return cnt2;
        }
        public Campus FindCampus(Guid? id, bool load = false)
        {
            var c = Query<Campus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", institutionid AS \"Institutionid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus WHERE id = @id", new { id }).SingleOrDefault();
            if (c == null) return null;
            if (load && c.Institutionid != null) c.Institution = FindInstitution(c.Institutionid);
            return c;
        }
        public Campus2 FindCampus2(Guid? id, bool load = false)
        {
            var c2 = Query<Campus2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", institution_id AS \"InstitutionId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}campuses WHERE id = @id", new { id }).SingleOrDefault();
            if (c2 == null) return null;
            if (load && c2.InstitutionId != null) c2.Institution = FindInstitution2(c2.InstitutionId);
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId);
            return c2;
        }
        public CancellationReason FindCancellationReason(Guid? id, bool load = false) => Query<CancellationReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason WHERE id = @id", new { id }).SingleOrDefault();
        public CancellationReason2 FindCancellationReason2(Guid? id, bool load = false)
        {
            var cr2 = Query<CancellationReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", public_description AS \"PublicDescription\", requires_additional_information AS \"RequiresAdditionalInformation\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}cancellation_reasons WHERE id = @id", new { id }).SingleOrDefault();
            if (cr2 == null) return null;
            if (load && cr2.CreationUserId != null) cr2.CreationUser = FindUser2(cr2.CreationUserId);
            if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = FindUser2(cr2.LastWriteUserId);
            return cr2;
        }
        public Category FindCategory(Guid? id, bool load = false) => Query<Category>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories WHERE id = @id", new { id }).SingleOrDefault();
        public Category2 FindCategory2(Guid? id, bool load = false)
        {
            var c2 = Query<Category2>($"SELECT id AS \"Id\", value AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}categories WHERE id = @id", new { id }).SingleOrDefault();
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId);
            return c2;
        }
        public CheckIn FindCheckIn(Guid? id, bool load = false) => Query<CheckIn>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in WHERE id = @id", new { id }).SingleOrDefault();
        public CheckIn2 FindCheckIn2(Guid? id, bool load = false)
        {
            var ci2 = Query<CheckIn2>($"SELECT id AS \"Id\", occurred_date_time AS \"OccurredDateTime\", item_id AS \"ItemId\", item_status_prior_to_check_in AS \"ItemStatusPriorToCheckIn\", request_queue_size AS \"RequestQueueSize\", item_location_id AS \"ItemLocationId\", service_point_id AS \"ServicePointId\", performed_by_user_id AS \"PerformedByUserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}check_ins WHERE id = @id", new { id }).SingleOrDefault();
            if (ci2 == null) return null;
            if (load && ci2.ItemId != null) ci2.Item = FindItem2(ci2.ItemId);
            if (load && ci2.ItemLocationId != null) ci2.ItemLocation = FindLocation2(ci2.ItemLocationId);
            if (load && ci2.ServicePointId != null) ci2.ServicePoint = FindServicePoint2(ci2.ServicePointId);
            if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = FindUser2(ci2.PerformedByUserId);
            return ci2;
        }
        public CirculationNote FindCirculationNote(string id, Guid? itemId, bool load = false)
        {
            var cn = Query<CirculationNote>($"SELECT id AS \"Id\", item_id AS \"ItemId\", id2 AS \"Id2\", note_type AS \"NoteType\", note AS \"Note\", source_id AS \"SourceId\", source_personal_last_name AS \"SourcePersonalLastName\", source_personal_first_name AS \"SourcePersonalFirstName\", date AS \"Date\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}circulation_notes WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (cn == null) return null;
            if (load && cn.ItemId != null) cn.Item = FindItem2(cn.ItemId);
            return cn;
        }
        public CirculationRule FindCirculationRule(Guid? id, bool load = false) => Query<CirculationRule>($"SELECT id AS \"Id\", jsonb AS \"Content\", lock AS \"Lock\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules WHERE id = @id", new { id }).SingleOrDefault();
        public CirculationRule2 FindCirculationRule2(Guid? id, bool load = false) => Query<CirculationRule2>($"SELECT id AS \"Id\", rules_as_text AS \"RulesAsText\", content AS \"Content\", lock AS \"Lock\" FROM uc{(IsMySql ? "_" : ".")}circulation_rules WHERE id = @id", new { id }).SingleOrDefault();
        public Classification FindClassification(string id, Guid? instanceId, bool load = false)
        {
            var c = Query<Classification>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", classification_number AS \"Number\", classification_type_id AS \"ClassificationTypeId\" FROM uc{(IsMySql ? "_" : ".")}classifications WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (c == null) return null;
            if (load && c.InstanceId != null) c.Instance = FindInstance2(c.InstanceId);
            if (load && c.ClassificationTypeId != null) c.ClassificationType = FindClassificationType2(c.ClassificationTypeId);
            return c;
        }
        public ClassificationType FindClassificationType(Guid? id, bool load = false) => Query<ClassificationType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type WHERE id = @id", new { id }).SingleOrDefault();
        public ClassificationType2 FindClassificationType2(Guid? id, bool load = false)
        {
            var ct2 = Query<ClassificationType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}classification_types WHERE id = @id", new { id }).SingleOrDefault();
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId);
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId);
            return ct2;
        }
        public CloseReason FindCloseReason(Guid? id, bool load = false) => Query<CloseReason>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure WHERE id = @id", new { id }).SingleOrDefault();
        public CloseReason2 FindCloseReason2(Guid? id, bool load = false) => Query<CloseReason2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}close_reasons WHERE id = @id", new { id }).SingleOrDefault();
        public Comment FindComment(Guid? id, bool load = false) => Query<Comment>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}comments WHERE id = @id", new { id }).SingleOrDefault();
        public Comment2 FindComment2(Guid? id, bool load = false)
        {
            var c2 = Query<Comment2>($"SELECT id AS \"Id\", paid AS \"Paid\", waived AS \"Waived\", refunded AS \"Refunded\", transferred_manually AS \"TransferredManually\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}comments WHERE id = @id", new { id }).SingleOrDefault();
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId);
            return c2;
        }
        public Configuration FindConfiguration(Guid? id, bool load = false) => Query<Configuration>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_configuration{(IsMySql ? "_" : ".")}config_data WHERE id = @id", new { id }).SingleOrDefault();
        public Configuration2 FindConfiguration2(Guid? id, bool load = false)
        {
            var c2 = Query<Configuration2>($"SELECT id AS \"Id\", module AS \"Module\", config_name AS \"ConfigName\", code AS \"Code\", description AS \"Description\", \"default\" AS \"Default\", enabled AS \"Enabled\", value AS \"Value\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}configurations WHERE id = @id", new { id }).SingleOrDefault();
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId);
            return c2;
        }
        public Contact FindContact(Guid? id, bool load = false) => Query<Contact>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts WHERE id = @id", new { id }).SingleOrDefault();
        public Contact2 FindContact2(Guid? id, bool load = false)
        {
            var c2 = Query<Contact2>($"SELECT id AS \"Id\", name AS \"Name\", prefix AS \"Prefix\", first_name AS \"FirstName\", last_name AS \"LastName\", language AS \"Language\", notes AS \"Notes\", inactive AS \"Inactive\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contacts WHERE id = @id", new { id }).SingleOrDefault();
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId);
            return c2;
        }
        public ContactAddress FindContactAddress(string id, bool load = false)
        {
            var ca = Query<ContactAddress>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", state_region AS \"StateRegion\", zip_code AS \"ZipCode\", country AS \"Country\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_addresses WHERE id = @id", new { id }).SingleOrDefault();
            if (ca == null) return null;
            if (load && ca.ContactId != null) ca.Contact = FindContact2(ca.ContactId);
            if (load && ca.CreationUserId != null) ca.CreationUser = FindUser2(ca.CreationUserId);
            if (load && ca.LastWriteUserId != null) ca.LastWriteUser = FindUser2(ca.LastWriteUserId);
            return ca;
        }
        public ContactAddressCategory FindContactAddressCategory(string id, bool load = false)
        {
            var cac = Query<ContactAddressCategory>($"SELECT id AS \"Id\", contact_address_id AS \"ContactAddressId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_address_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (cac == null) return null;
            if (load && cac.ContactAddressId != null) cac.ContactAddress = FindContactAddress(cac.ContactAddressId);
            if (load && cac.CategoryId != null) cac.Category = FindCategory2(cac.CategoryId);
            return cac;
        }
        public ContactCategory FindContactCategory(string id, bool load = false)
        {
            var cc = Query<ContactCategory>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (cc == null) return null;
            if (load && cc.ContactId != null) cc.Contact = FindContact2(cc.ContactId);
            if (load && cc.CategoryId != null) cc.Category = FindCategory2(cc.CategoryId);
            return cc;
        }
        public ContactEmail FindContactEmail(string id, bool load = false)
        {
            var ce = Query<ContactEmail>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_emails WHERE id = @id", new { id }).SingleOrDefault();
            if (ce == null) return null;
            if (load && ce.ContactId != null) ce.Contact = FindContact2(ce.ContactId);
            if (load && ce.CreationUserId != null) ce.CreationUser = FindUser2(ce.CreationUserId);
            if (load && ce.LastWriteUserId != null) ce.LastWriteUser = FindUser2(ce.LastWriteUserId);
            return ce;
        }
        public ContactEmailCategory FindContactEmailCategory(string id, bool load = false)
        {
            var cec = Query<ContactEmailCategory>($"SELECT id AS \"Id\", contact_email_id AS \"ContactEmailId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_email_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (cec == null) return null;
            if (load && cec.ContactEmailId != null) cec.ContactEmail = FindContactEmail(cec.ContactEmailId);
            if (load && cec.CategoryId != null) cec.Category = FindCategory2(cec.CategoryId);
            return cec;
        }
        public ContactPhoneNumber FindContactPhoneNumber(string id, bool load = false)
        {
            var cpn = Query<ContactPhoneNumber>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", phone_number AS \"PhoneNumber\", type AS \"Type\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_phone_numbers WHERE id = @id", new { id }).SingleOrDefault();
            if (cpn == null) return null;
            if (load && cpn.ContactId != null) cpn.Contact = FindContact2(cpn.ContactId);
            if (load && cpn.CreationUserId != null) cpn.CreationUser = FindUser2(cpn.CreationUserId);
            if (load && cpn.LastWriteUserId != null) cpn.LastWriteUser = FindUser2(cpn.LastWriteUserId);
            return cpn;
        }
        public ContactPhoneNumberCategory FindContactPhoneNumberCategory(string id, bool load = false)
        {
            var cpnc = Query<ContactPhoneNumberCategory>($"SELECT id AS \"Id\", contact_phone_number_id AS \"ContactPhoneNumberId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_phone_number_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (cpnc == null) return null;
            if (load && cpnc.ContactPhoneNumberId != null) cpnc.ContactPhoneNumber = FindContactPhoneNumber(cpnc.ContactPhoneNumberId);
            if (load && cpnc.CategoryId != null) cpnc.Category = FindCategory2(cpnc.CategoryId);
            return cpnc;
        }
        public ContactType FindContactType(string id, bool load = false) => Query<ContactType>($"SELECT id AS \"Id\", name AS \"Name\" FROM uc{(IsMySql ? "_" : ".")}contact_types WHERE id = @id", new { id }).SingleOrDefault();
        public ContactUrl FindContactUrl(string id, bool load = false)
        {
            var cu = Query<ContactUrl>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", language AS \"Language\", is_primary AS \"IsPrimary\", notes AS \"Notes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_urls WHERE id = @id", new { id }).SingleOrDefault();
            if (cu == null) return null;
            if (load && cu.ContactId != null) cu.Contact = FindContact2(cu.ContactId);
            if (load && cu.CreationUserId != null) cu.CreationUser = FindUser2(cu.CreationUserId);
            if (load && cu.LastWriteUserId != null) cu.LastWriteUser = FindUser2(cu.LastWriteUserId);
            return cu;
        }
        public ContactUrlCategory FindContactUrlCategory(string id, bool load = false)
        {
            var cuc = Query<ContactUrlCategory>($"SELECT id AS \"Id\", contact_url_id AS \"ContactUrlId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_url_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (cuc == null) return null;
            if (load && cuc.ContactUrlId != null) cuc.ContactUrl = FindContactUrl(cuc.ContactUrlId);
            if (load && cuc.CategoryId != null) cuc.Category = FindCategory2(cuc.CategoryId);
            return cuc;
        }
        public Contributor FindContributor(string id, Guid? instanceId, bool load = false)
        {
            var c = Query<Contributor>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", name AS \"Name\", contributor_type_id AS \"ContributorTypeId\", contributor_type_text AS \"ContributorTypeText\", contributor_name_type_id AS \"ContributorNameTypeId\", primary AS \"Primary\" FROM uc{(IsMySql ? "_" : ".")}contributors WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (c == null) return null;
            if (load && c.InstanceId != null) c.Instance = FindInstance2(c.InstanceId);
            if (load && c.ContributorTypeId != null) c.ContributorType = FindContributorType2(c.ContributorTypeId);
            if (load && c.ContributorNameTypeId != null) c.ContributorNameType = FindContributorNameType2(c.ContributorNameTypeId);
            return c;
        }
        public ContributorNameType FindContributorNameType(Guid? id, bool load = false) => Query<ContributorNameType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type WHERE id = @id", new { id }).SingleOrDefault();
        public ContributorNameType2 FindContributorNameType2(Guid? id, bool load = false)
        {
            var cnt2 = Query<ContributorNameType2>($"SELECT id AS \"Id\", name AS \"Name\", ordering AS \"Ordering\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contributor_name_types WHERE id = @id", new { id }).SingleOrDefault();
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId);
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId);
            return cnt2;
        }
        public ContributorType FindContributorType(Guid? id, bool load = false) => Query<ContributorType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type WHERE id = @id", new { id }).SingleOrDefault();
        public ContributorType2 FindContributorType2(Guid? id, bool load = false)
        {
            var ct2 = Query<ContributorType2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contributor_types WHERE id = @id", new { id }).SingleOrDefault();
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId);
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId);
            return ct2;
        }
        public Country FindCountry(string alpha2Code, bool load = false) => Query<Country>($"SELECT alpha2_code AS \"Alpha2Code\", alpha3_code AS \"Alpha3Code\", name AS \"Name\" FROM uc{(IsMySql ? "_" : ".")}countries WHERE alpha2_code = @alpha2Code", new { alpha2Code }).SingleOrDefault();
        public Currency FindCurrency(string id, bool load = false)
        {
            var c = Query<Currency>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}currencies WHERE id = @id", new { id }).SingleOrDefault();
            if (c == null) return null;
            if (load && c.OrganizationId != null) c.Organization = FindOrganization2(c.OrganizationId);
            return c;
        }
        public CustomField FindCustomField(Guid? id, bool load = false) => Query<CustomField>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_users{(IsMySql ? "_" : ".")}custom_fields WHERE id = @id", new { id }).SingleOrDefault();
        public CustomField2 FindCustomField2(Guid? id, bool load = false)
        {
            var cf2 = Query<CustomField2>($"SELECT id AS \"Id\", name AS \"Name\", ref_id AS \"RefId\", type AS \"Type\", entity_type AS \"EntityType\", visible AS \"Visible\", required AS \"Required\", is_repeatable AS \"IsRepeatable\", \"order\" AS \"Order\", help_text AS \"HelpText\", checkbox_field_default AS \"CheckboxFieldDefault\", select_field_multi_select AS \"SelectFieldMultiSelect\", select_field_options_sorting_order AS \"SelectFieldOptionsSortingOrder\", text_field_field_format AS \"TextFieldFieldFormat\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}custom_fields WHERE id = @id", new { id }).SingleOrDefault();
            if (cf2 == null) return null;
            if (load && cf2.CreationUserId != null) cf2.CreationUser = FindUser2(cf2.CreationUserId);
            if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = FindUser2(cf2.LastWriteUserId);
            return cf2;
        }
        public CustomFieldValue FindCustomFieldValue(string id, bool load = false)
        {
            var cfv = Query<CustomFieldValue>($"SELECT id AS \"Id\", custom_field_id AS \"CustomFieldId\", id2 AS \"Id2\", value AS \"Value\", \"default\" AS \"Default\" FROM uc{(IsMySql ? "_" : ".")}custom_field_values WHERE id = @id", new { id }).SingleOrDefault();
            if (cfv == null) return null;
            if (load && cfv.CustomFieldId != null) cfv.CustomField = FindCustomField2(cfv.CustomFieldId);
            return cfv;
        }
        public Department FindDepartment(Guid? id, bool load = false) => Query<Department>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}departments WHERE id = @id", new { id }).SingleOrDefault();
        public Department2 FindDepartment2(Guid? id, bool load = false)
        {
            var d2 = Query<Department2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", usage_number AS \"UsageNumber\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}departments WHERE id = @id", new { id }).SingleOrDefault();
            if (d2 == null) return null;
            if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId);
            if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId);
            return d2;
        }
        public Document FindDocument(Guid? id, bool load = false)
        {
            var d = Query<Document>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\", document_data AS \"DocumentData\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents WHERE id = @id", new { id }).SingleOrDefault();
            if (d == null) return null;
            if (load && d.Invoiceid != null) d.Invoice = FindInvoice(d.Invoiceid);
            return d;
        }
        public Document2 FindDocument2(Guid? id, bool load = false)
        {
            var d2 = Query<Document2>($"SELECT id AS \"Id\", document_metadata_name AS \"DocumentMetadataName\", document_metadata_invoice_id AS \"DocumentMetadataInvoiceId\", document_metadata_url AS \"DocumentMetadataUrl\", document_metadata_metadata_created_date AS \"DocumentMetadataMetadataCreatedDate\", document_metadata_metadata_created_by_user_id AS \"DocumentMetadataMetadataCreatedByUserId\", document_metadata_metadata_created_by_username AS \"DocumentMetadataMetadataCreatedByUsername\", document_metadata_metadata_updated_date AS \"DocumentMetadataMetadataUpdatedDate\", document_metadata_metadata_updated_by_user_id AS \"DocumentMetadataMetadataUpdatedByUserId\", document_metadata_metadata_updated_by_username AS \"DocumentMetadataMetadataUpdatedByUsername\", contents_data AS \"ContentsData\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", invoiceid AS \"Invoiceid\", document_data AS \"DocumentData\" FROM uc{(IsMySql ? "_" : ".")}documents WHERE id = @id", new { id }).SingleOrDefault();
            if (d2 == null) return null;
            if (load && d2.DocumentMetadataInvoiceId != null) d2.DocumentMetadataInvoice = FindInvoice2(d2.DocumentMetadataInvoiceId);
            if (load && d2.DocumentMetadataMetadataCreatedByUserId != null) d2.DocumentMetadataMetadataCreatedByUser = FindUser2(d2.DocumentMetadataMetadataCreatedByUserId);
            if (load && d2.DocumentMetadataMetadataUpdatedByUserId != null) d2.DocumentMetadataMetadataUpdatedByUser = FindUser2(d2.DocumentMetadataMetadataUpdatedByUserId);
            if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId);
            if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId);
            return d2;
        }
        public Edition FindEdition(string id, Guid? instanceId, bool load = false)
        {
            var e2 = Query<Edition>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}editions WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (e2 == null) return null;
            if (load && e2.InstanceId != null) e2.Instance = FindInstance2(e2.InstanceId);
            return e2;
        }
        public ElectronicAccess FindElectronicAccess(string id, Guid? instanceId, bool load = false)
        {
            var ea = Query<ElectronicAccess>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}electronic_accesses WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (ea == null) return null;
            if (load && ea.InstanceId != null) ea.Instance = FindInstance2(ea.InstanceId);
            if (load && ea.RelationshipId != null) ea.Relationship = FindRelationship(ea.RelationshipId);
            return ea;
        }
        public ElectronicAccessRelationship FindElectronicAccessRelationship(Guid? id, bool load = false) => Query<ElectronicAccessRelationship>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship WHERE id = @id", new { id }).SingleOrDefault();
        public ElectronicAccessRelationship2 FindElectronicAccessRelationship2(Guid? id, bool load = false)
        {
            var ear2 = Query<ElectronicAccessRelationship2>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}electronic_access_relationships WHERE id = @id", new { id }).SingleOrDefault();
            if (ear2 == null) return null;
            if (load && ear2.CreationUserId != null) ear2.CreationUser = FindUser2(ear2.CreationUserId);
            if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = FindUser2(ear2.LastWriteUserId);
            return ear2;
        }
        public ErrorRecord FindErrorRecord(Guid? id, bool load = false)
        {
            var er = Query<ErrorRecord>($"SELECT id AS \"Id\", content AS \"Content\", description AS \"Description\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb WHERE id = @id", new { id }).SingleOrDefault();
            if (er == null) return null;
            if (load && er.Id != null) er.Record = FindRecord(er.Id);
            return er;
        }
        public ErrorRecord2 FindErrorRecord2(Guid? id, bool load = false)
        {
            var er2 = Query<ErrorRecord2>($"SELECT id AS \"Id\", content AS \"Content\", description AS \"Description\" FROM uc{(IsMySql ? "_" : ".")}error_records WHERE id = @id", new { id }).SingleOrDefault();
            if (er2 == null) return null;
            if (load && er2.Id != null) er2.Record2 = FindRecord2(er2.Id);
            return er2;
        }
        public EventLog FindEventLog(Guid? id, bool load = false) => Query<EventLog>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}event_logs WHERE id = @id", new { id }).SingleOrDefault();
        public EventLog2 FindEventLog2(Guid? id, bool load = false)
        {
            var el2 = Query<EventLog2>($"SELECT id AS \"Id\", tenant AS \"Tenant\", user_id AS \"UserId\", ip AS \"Ip\", browser_information AS \"BrowserInformation\", timestamp AS \"Timestamp\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}event_logs WHERE id = @id", new { id }).SingleOrDefault();
            if (el2 == null) return null;
            if (load && el2.UserId != null) el2.User = FindUser2(el2.UserId);
            if (load && el2.CreationUserId != null) el2.CreationUser = FindUser2(el2.CreationUserId);
            if (load && el2.LastWriteUserId != null) el2.LastWriteUser = FindUser2(el2.LastWriteUserId);
            return el2;
        }
        public ExpenseClass FindExpenseClass(Guid? id, bool load = false) => Query<ExpenseClass>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class WHERE id = @id", new { id }).SingleOrDefault();
        public ExpenseClass2 FindExpenseClass2(Guid? id, bool load = false)
        {
            var ec2 = Query<ExpenseClass2>($"SELECT id AS \"Id\", code AS \"Code\", external_account_number_ext AS \"ExternalAccountNumberExt\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}expense_classes WHERE id = @id", new { id }).SingleOrDefault();
            if (ec2 == null) return null;
            if (load && ec2.CreationUserId != null) ec2.CreationUser = FindUser2(ec2.CreationUserId);
            if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = FindUser2(ec2.LastWriteUserId);
            return ec2;
        }
        public ExportConfigCredential FindExportConfigCredential(Guid? id, bool load = false)
        {
            var ecc = Query<ExportConfigCredential>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", exportconfigid AS \"Exportconfigid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials WHERE id = @id", new { id }).SingleOrDefault();
            if (ecc == null) return null;
            if (load && ecc.Exportconfigid != null) ecc.BatchVoucherExportConfig = FindBatchVoucherExportConfig(ecc.Exportconfigid);
            return ecc;
        }
        public ExportConfigCredential2 FindExportConfigCredential2(Guid? id, bool load = false)
        {
            var ecc2 = Query<ExportConfigCredential2>($"SELECT id AS \"Id\", username AS \"Username\", password AS \"Password\", export_config_id AS \"ExportConfigId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}export_config_credentials WHERE id = @id", new { id }).SingleOrDefault();
            if (ecc2 == null) return null;
            if (load && ecc2.ExportConfigId != null) ecc2.ExportConfig = FindBatchVoucherExportConfig2(ecc2.ExportConfigId);
            if (load && ecc2.CreationUserId != null) ecc2.CreationUser = FindUser2(ecc2.CreationUserId);
            if (load && ecc2.LastWriteUserId != null) ecc2.LastWriteUser = FindUser2(ecc2.LastWriteUserId);
            return ecc2;
        }
        public Extent FindExtent(string id, Guid? holdingId, bool load = false)
        {
            var e2 = Query<Extent>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Content\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}extents WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (e2 == null) return null;
            if (load && e2.HoldingId != null) e2.Holding = FindHolding2(e2.HoldingId);
            return e2;
        }
        public Fee FindFee(Guid? id, bool load = false) => Query<Fee>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts WHERE id = @id", new { id }).SingleOrDefault();
        public Fee2 FindFee2(Guid? id, bool load = false)
        {
            var f2 = Query<Fee2>($"SELECT id AS \"Id\", amount AS \"Amount\", remaining AS \"RemainingAmount\", date_created AS \"DateCreated\", date_updated AS \"DateUpdated\", status_name AS \"StatusName\", payment_status_name AS \"PaymentStatusName\", fee_fine_type AS \"FeeFineType\", fee_fine_owner AS \"FeeFineOwner\", title AS \"Title\", call_number AS \"CallNumber\", barcode AS \"Barcode\", material_type AS \"MaterialType\", item_status_name AS \"ItemStatusName\", location AS \"Location\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", due_date AS \"DueTime\", returned_date AS \"ReturnedTime\", loan_id AS \"LoanId\", user_id AS \"UserId\", item_id AS \"ItemId\", material_type_id AS \"MaterialTypeId\", fee_type_id AS \"FeeTypeId\", owner_id AS \"OwnerId\", holding_id AS \"HoldingId\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fees WHERE id = @id", new { id }).SingleOrDefault();
            if (f2 == null) return null;
            if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId);
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId);
            if (load && f2.LoanId != null) f2.Loan = FindLoan2(f2.LoanId);
            if (load && f2.UserId != null) f2.User = FindUser2(f2.UserId);
            if (load && f2.ItemId != null) f2.Item = FindItem2(f2.ItemId);
            if (load && f2.MaterialTypeId != null) f2.MaterialType1 = FindMaterialType2(f2.MaterialTypeId);
            if (load && f2.FeeTypeId != null) f2.FeeType = FindFeeType2(f2.FeeTypeId);
            if (load && f2.OwnerId != null) f2.Owner = FindOwner2(f2.OwnerId);
            if (load && f2.HoldingId != null) f2.Holding = FindHolding2(f2.HoldingId);
            if (load && f2.InstanceId != null) f2.Instance = FindInstance2(f2.InstanceId);
            return f2;
        }
        public FeeType FindFeeType(Guid? id, bool load = false)
        {
            var ft = Query<FeeType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", ownerid AS \"Ownerid\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines WHERE id = @id", new { id }).SingleOrDefault();
            if (ft == null) return null;
            if (load && ft.Ownerid != null) ft.Owner = FindOwner(ft.Ownerid);
            return ft;
        }
        public FeeType2 FindFeeType2(Guid? id, bool load = false)
        {
            var ft2 = Query<FeeType2>($"SELECT id AS \"Id\", automatic AS \"Automatic\", fee_fine_type AS \"Name\", default_amount AS \"DefaultAmount\", charge_notice_id AS \"ChargeNoticeId\", action_notice_id AS \"ActionNoticeId\", owner_id AS \"OwnerId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fee_types WHERE id = @id", new { id }).SingleOrDefault();
            if (ft2 == null) return null;
            if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = FindTemplate2(ft2.ChargeNoticeId);
            if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = FindTemplate2(ft2.ActionNoticeId);
            if (load && ft2.OwnerId != null) ft2.Owner = FindOwner2(ft2.OwnerId);
            if (load && ft2.CreationUserId != null) ft2.CreationUser = FindUser2(ft2.CreationUserId);
            if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = FindUser2(ft2.LastWriteUserId);
            return ft2;
        }
        public FinanceGroup FindFinanceGroup(Guid? id, bool load = false) => Query<FinanceGroup>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id }).SingleOrDefault();
        public FinanceGroup2 FindFinanceGroup2(Guid? id, bool load = false)
        {
            var fg2 = Query<FinanceGroup2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", name AS \"Name\", status AS \"Status\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}finance_groups WHERE id = @id", new { id }).SingleOrDefault();
            if (fg2 == null) return null;
            if (load && fg2.CreationUserId != null) fg2.CreationUser = FindUser2(fg2.CreationUserId);
            if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = FindUser2(fg2.LastWriteUserId);
            return fg2;
        }
        public FinanceGroupAcquisitionsUnit FindFinanceGroupAcquisitionsUnit(string id, bool load = false)
        {
            var fgau = Query<FinanceGroupAcquisitionsUnit>($"SELECT id AS \"Id\", finance_group_id AS \"FinanceGroupId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}finance_group_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (fgau == null) return null;
            if (load && fgau.FinanceGroupId != null) fgau.FinanceGroup = FindFinanceGroup2(fgau.FinanceGroupId);
            if (load && fgau.AcquisitionsUnitId != null) fgau.AcquisitionsUnit = FindAcquisitionsUnit2(fgau.AcquisitionsUnitId);
            return fgau;
        }
        public FiscalYear FindFiscalYear(Guid? id, bool load = false) => Query<FiscalYear>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year WHERE id = @id", new { id }).SingleOrDefault();
        public FiscalYear2 FindFiscalYear2(Guid? id, bool load = false)
        {
            var fy2 = Query<FiscalYear2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", currency AS \"Currency\", description AS \"Description\", period_start AS \"StartDate\", period_end AS \"EndDate\", series AS \"Series\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fiscal_years WHERE id = @id", new { id }).SingleOrDefault();
            if (fy2 == null) return null;
            if (load && fy2.CreationUserId != null) fy2.CreationUser = FindUser2(fy2.CreationUserId);
            if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = FindUser2(fy2.LastWriteUserId);
            return fy2;
        }
        public FiscalYearAcquisitionsUnit FindFiscalYearAcquisitionsUnit(string id, bool load = false)
        {
            var fyau = Query<FiscalYearAcquisitionsUnit>($"SELECT id AS \"Id\", fiscal_year_id AS \"FiscalYearId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}fiscal_year_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (fyau == null) return null;
            if (load && fyau.FiscalYearId != null) fyau.FiscalYear = FindFiscalYear2(fyau.FiscalYearId);
            if (load && fyau.AcquisitionsUnitId != null) fyau.AcquisitionsUnit = FindAcquisitionsUnit2(fyau.AcquisitionsUnitId);
            return fyau;
        }
        public FixedDueDateSchedule FindFixedDueDateSchedule(Guid? id, bool load = false) => Query<FixedDueDateSchedule>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule WHERE id = @id", new { id }).SingleOrDefault();
        public FixedDueDateSchedule2 FindFixedDueDateSchedule2(Guid? id, bool load = false)
        {
            var fdds2 = Query<FixedDueDateSchedule2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedules WHERE id = @id", new { id }).SingleOrDefault();
            if (fdds2 == null) return null;
            if (load && fdds2.CreationUserId != null) fdds2.CreationUser = FindUser2(fdds2.CreationUserId);
            if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = FindUser2(fdds2.LastWriteUserId);
            return fdds2;
        }
        public FixedDueDateScheduleSchedule FindFixedDueDateScheduleSchedule(string id, bool load = false)
        {
            var fddss = Query<FixedDueDateScheduleSchedule>($"SELECT id AS \"Id\", fixed_due_date_schedule_id AS \"FixedDueDateScheduleId\", from AS \"From\", \"to\" AS \"To\", due AS \"Due\" FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedule_schedules WHERE id = @id", new { id }).SingleOrDefault();
            if (fddss == null) return null;
            if (load && fddss.FixedDueDateScheduleId != null) fddss.FixedDueDateSchedule = FindFixedDueDateSchedule2(fddss.FixedDueDateScheduleId);
            return fddss;
        }
        public Format FindFormat(Guid? id, bool load = false)
        {
            var f = Query<Format>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}formats WHERE id = @id", new { id }).SingleOrDefault();
            if (f == null) return null;
            if (load && f.CreationUserId != null) f.CreationUser = FindUser2(f.CreationUserId);
            if (load && f.LastWriteUserId != null) f.LastWriteUser = FindUser2(f.LastWriteUserId);
            return f;
        }
        public Fund FindFund(Guid? id, bool load = false)
        {
            var f = Query<Fund>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", ledgerid AS \"LedgerId\", fundtypeid AS \"Fundtypeid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund WHERE id = @id", new { id }).SingleOrDefault();
            if (f == null) return null;
            if (load && f.LedgerId != null) f.Ledger = FindLedger(f.LedgerId);
            if (load && f.Fundtypeid != null) f.FundType = FindFundType(f.Fundtypeid);
            return f;
        }
        public Fund2 FindFund2(Guid? id, bool load = false)
        {
            var f2 = Query<Fund2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", external_account_no AS \"ExternalAccountNo\", fund_status AS \"FundStatus\", fund_type_id AS \"FundTypeId\", ledger_id AS \"LedgerId\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}funds WHERE id = @id", new { id }).SingleOrDefault();
            if (f2 == null) return null;
            if (load && f2.FundTypeId != null) f2.FundType = FindFundType2(f2.FundTypeId);
            if (load && f2.LedgerId != null) f2.Ledger = FindLedger2(f2.LedgerId);
            if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId);
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId);
            return f2;
        }
        public FundAcquisitionsUnit FindFundAcquisitionsUnit(string id, bool load = false)
        {
            var fau = Query<FundAcquisitionsUnit>($"SELECT id AS \"Id\", fund_id AS \"FundId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}fund_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (fau == null) return null;
            if (load && fau.FundId != null) fau.Fund = FindFund2(fau.FundId);
            if (load && fau.AcquisitionsUnitId != null) fau.AcquisitionsUnit = FindAcquisitionsUnit2(fau.AcquisitionsUnitId);
            return fau;
        }
        public FundTag FindFundTag(string id, bool load = false)
        {
            var ft = Query<FundTag>($"SELECT id AS \"Id\", fund_id AS \"FundId\", tag_id AS \"TagId\" FROM uc{(IsMySql ? "_" : ".")}fund_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (ft == null) return null;
            if (load && ft.FundId != null) ft.Fund = FindFund2(ft.FundId);
            if (load && ft.TagId != null) ft.Tag = FindTag2(ft.TagId);
            return ft;
        }
        public FundType FindFundType(Guid? id, bool load = false) => Query<FundType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type WHERE id = @id", new { id }).SingleOrDefault();
        public FundType2 FindFundType2(Guid? id, bool load = false) => Query<FundType2>($"SELECT id AS \"Id\", name AS \"Name\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fund_types WHERE id = @id", new { id }).SingleOrDefault();
        public Group FindGroup(Guid? id, bool load = false) => Query<Group>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id }).SingleOrDefault();
        public Group2 FindGroup2(Guid? id, bool load = false)
        {
            var g2 = Query<Group2>($"SELECT id AS \"Id\", \"group\" AS \"Name\", \"desc\" AS \"Description\", expiration_offset_in_days AS \"ExpirationOffsetInDays\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id }).SingleOrDefault();
            if (g2 == null) return null;
            if (load && g2.CreationUserId != null) g2.CreationUser = FindUser2(g2.CreationUserId);
            if (load && g2.LastWriteUserId != null) g2.LastWriteUser = FindUser2(g2.LastWriteUserId);
            return g2;
        }
        public GroupFundFiscalYear FindGroupFundFiscalYear(Guid? id, bool load = false)
        {
            var gffy = Query<GroupFundFiscalYear>($"SELECT id AS \"Id\", jsonb AS \"Content\", budgetid AS \"Budgetid\", groupid AS \"Groupid\", fundid AS \"Fundid\", fiscalyearid AS \"Fiscalyearid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year WHERE id = @id", new { id }).SingleOrDefault();
            if (gffy == null) return null;
            if (load && gffy.Budgetid != null) gffy.Budget = FindBudget(gffy.Budgetid);
            if (load && gffy.Groupid != null) gffy.FinanceGroup = FindFinanceGroup(gffy.Groupid);
            if (load && gffy.Fundid != null) gffy.Fund = FindFund(gffy.Fundid);
            if (load && gffy.Fiscalyearid != null) gffy.FiscalYear = FindFiscalYear(gffy.Fiscalyearid);
            return gffy;
        }
        public GroupFundFiscalYear2 FindGroupFundFiscalYear2(Guid? id, bool load = false)
        {
            var gffy2 = Query<GroupFundFiscalYear2>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", group_id AS \"GroupId\", fiscal_year_id AS \"FiscalYearId\", fund_id AS \"FundId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}group_fund_fiscal_years WHERE id = @id", new { id }).SingleOrDefault();
            if (gffy2 == null) return null;
            if (load && gffy2.BudgetId != null) gffy2.Budget = FindBudget2(gffy2.BudgetId);
            if (load && gffy2.GroupId != null) gffy2.Group = FindFinanceGroup2(gffy2.GroupId);
            if (load && gffy2.FiscalYearId != null) gffy2.FiscalYear = FindFiscalYear2(gffy2.FiscalYearId);
            if (load && gffy2.FundId != null) gffy2.Fund = FindFund2(gffy2.FundId);
            return gffy2;
        }
        public Holding FindHolding(Guid? id, bool load = false)
        {
            var h = Query<Holding>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", instanceid AS \"Instanceid\", permanentlocationid AS \"Permanentlocationid\", temporarylocationid AS \"Temporarylocationid\", holdingstypeid AS \"Holdingstypeid\", callnumbertypeid AS \"Callnumbertypeid\", illpolicyid AS \"Illpolicyid\", sourceid AS \"Sourceid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record WHERE id = @id", new { id }).SingleOrDefault();
            if (h == null) return null;
            if (load && h.Instanceid != null) h.Instance = FindInstance(h.Instanceid);
            if (load && h.Permanentlocationid != null) h.Location = FindLocation(h.Permanentlocationid);
            if (load && h.Temporarylocationid != null) h.Location1 = FindLocation(h.Temporarylocationid);
            if (load && h.Holdingstypeid != null) h.HoldingType = FindHoldingType(h.Holdingstypeid);
            if (load && h.Callnumbertypeid != null) h.CallNumberType = FindCallNumberType(h.Callnumbertypeid);
            if (load && h.Illpolicyid != null) h.IllPolicy = FindIllPolicy(h.Illpolicyid);
            if (load && h.Sourceid != null) h.Source = FindSource(h.Sourceid);
            return h;
        }
        public Holding2 FindHolding2(Guid? id, bool load = false)
        {
            var h2 = Query<Holding2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", holding_type_id AS \"HoldingTypeId\", instance_id AS \"InstanceId\", permanent_location_id AS \"LocationId\", temporary_location_id AS \"TemporaryLocationId\", call_number_type_id AS \"CallNumberTypeId\", call_number_prefix AS \"CallNumberPrefix\", call_number AS \"CallNumber\", call_number_suffix AS \"CallNumberSuffix\", shelving_title AS \"ShelvingTitle\", acquisition_format AS \"AcquisitionFormat\", acquisition_method AS \"AcquisitionMethod\", receipt_status AS \"ReceiptStatus\", ill_policy_id AS \"IllPolicyId\", retention_policy AS \"RetentionPolicy\", digitization_policy AS \"DigitizationPolicy\", copy_number AS \"CopyNumber\", number_of_items AS \"ItemCount\", receiving_history_display_type AS \"ReceivingHistoryDisplayType\", discovery_suppress AS \"DiscoverySuppress\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", source_id AS \"SourceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holdings WHERE id = @id", new { id }).SingleOrDefault();
            if (h2 == null) return null;
            if (load && h2.HoldingTypeId != null) h2.HoldingType = FindHoldingType2(h2.HoldingTypeId);
            if (load && h2.InstanceId != null) h2.Instance = FindInstance2(h2.InstanceId);
            if (load && h2.LocationId != null) h2.Location = FindLocation2(h2.LocationId);
            if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = FindLocation2(h2.TemporaryLocationId);
            if (load && h2.CallNumberTypeId != null) h2.CallNumberType = FindCallNumberType2(h2.CallNumberTypeId);
            if (load && h2.IllPolicyId != null) h2.IllPolicy = FindIllPolicy2(h2.IllPolicyId);
            if (load && h2.CreationUserId != null) h2.CreationUser = FindUser2(h2.CreationUserId);
            if (load && h2.LastWriteUserId != null) h2.LastWriteUser = FindUser2(h2.LastWriteUserId);
            if (load && h2.SourceId != null) h2.Source = FindSource2(h2.SourceId);
            return h2;
        }
        public HoldingElectronicAccess FindHoldingElectronicAccess(string id, Guid? holdingId, bool load = false)
        {
            var hea = Query<HoldingElectronicAccess>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}holding_electronic_accesses WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (hea == null) return null;
            if (load && hea.HoldingId != null) hea.Holding = FindHolding2(hea.HoldingId);
            if (load && hea.RelationshipId != null) hea.Relationship = FindRelationship(hea.RelationshipId);
            return hea;
        }
        public HoldingEntry FindHoldingEntry(string id, Guid? holdingId, bool load = false)
        {
            var he = Query<HoldingEntry>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", public_display AS \"PublicDisplay\", enumeration AS \"Enumeration\", chronology AS \"Chronology\" FROM uc{(IsMySql ? "_" : ".")}holding_entries WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (he == null) return null;
            if (load && he.HoldingId != null) he.Holding = FindHolding2(he.HoldingId);
            return he;
        }
        public HoldingFormerId FindHoldingFormerId(string id, Guid? holdingId, bool load = false)
        {
            var hfi = Query<HoldingFormerId>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_former_ids WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (hfi == null) return null;
            if (load && hfi.HoldingId != null) hfi.Holding = FindHolding2(hfi.HoldingId);
            return hfi;
        }
        public HoldingNote FindHoldingNote(string id, Guid? holdingId, bool load = false)
        {
            var hn = Query<HoldingNote>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", holding_note_type_id AS \"HoldingNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}holding_notes WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (hn == null) return null;
            if (load && hn.HoldingId != null) hn.Holding = FindHolding2(hn.HoldingId);
            if (load && hn.HoldingNoteTypeId != null) hn.HoldingNoteType = FindHoldingNoteType2(hn.HoldingNoteTypeId);
            return hn;
        }
        public HoldingNoteType FindHoldingNoteType(Guid? id, bool load = false) => Query<HoldingNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type WHERE id = @id", new { id }).SingleOrDefault();
        public HoldingNoteType2 FindHoldingNoteType2(Guid? id, bool load = false)
        {
            var hnt2 = Query<HoldingNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_note_types WHERE id = @id", new { id }).SingleOrDefault();
            if (hnt2 == null) return null;
            if (load && hnt2.CreationUserId != null) hnt2.CreationUser = FindUser2(hnt2.CreationUserId);
            if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = FindUser2(hnt2.LastWriteUserId);
            return hnt2;
        }
        public HoldingStatisticalCode FindHoldingStatisticalCode(string id, Guid? holdingId, bool load = false)
        {
            var hsc = Query<HoldingStatisticalCode>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}holding_statistical_codes WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (hsc == null) return null;
            if (load && hsc.HoldingId != null) hsc.Holding = FindHolding2(hsc.HoldingId);
            if (load && hsc.StatisticalCodeId != null) hsc.StatisticalCode = FindStatisticalCode2(hsc.StatisticalCodeId);
            return hsc;
        }
        public HoldingTag FindHoldingTag(string id, Guid? holdingId, bool load = false)
        {
            var ht = Query<HoldingTag>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_tags WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (ht == null) return null;
            if (load && ht.HoldingId != null) ht.Holding = FindHolding2(ht.HoldingId);
            return ht;
        }
        public HoldingType FindHoldingType(Guid? id, bool load = false) => Query<HoldingType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type WHERE id = @id", new { id }).SingleOrDefault();
        public HoldingType2 FindHoldingType2(Guid? id, bool load = false)
        {
            var ht2 = Query<HoldingType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_types WHERE id = @id", new { id }).SingleOrDefault();
            if (ht2 == null) return null;
            if (load && ht2.CreationUserId != null) ht2.CreationUser = FindUser2(ht2.CreationUserId);
            if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = FindUser2(ht2.LastWriteUserId);
            return ht2;
        }
        public HridSetting FindHridSetting(Guid? id, bool load = false) => Query<HridSetting>($"SELECT id AS \"Id\", jsonb AS \"Content\", lock AS \"Lock\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings WHERE id = @id", new { id }).SingleOrDefault();
        public HridSetting2 FindHridSetting2(Guid? id, bool load = false) => Query<HridSetting2>($"SELECT id AS \"Id\", instances_prefix AS \"InstancesPrefix\", instances_start_number AS \"InstancesStartNumber\", holdings_prefix AS \"HoldingsPrefix\", holdings_start_number AS \"HoldingsStartNumber\", items_prefix AS \"ItemsPrefix\", items_start_number AS \"ItemsStartNumber\", content AS \"Content\", lock AS \"Lock\" FROM uc{(IsMySql ? "_" : ".")}hrid_settings WHERE id = @id", new { id }).SingleOrDefault();
        public Identifier FindIdentifier(string id, Guid? instanceId, bool load = false)
        {
            var i = Query<Identifier>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", value AS \"Content\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}identifiers WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (i == null) return null;
            if (load && i.InstanceId != null) i.Instance = FindInstance2(i.InstanceId);
            if (load && i.IdentifierTypeId != null) i.IdentifierType = FindIdType2(i.IdentifierTypeId);
            return i;
        }
        public IdType FindIdType(Guid? id, bool load = false) => Query<IdType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type WHERE id = @id", new { id }).SingleOrDefault();
        public IdType2 FindIdType2(Guid? id, bool load = false)
        {
            var it2 = Query<IdType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}id_types WHERE id = @id", new { id }).SingleOrDefault();
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId);
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId);
            return it2;
        }
        public IllPolicy FindIllPolicy(Guid? id, bool load = false) => Query<IllPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy WHERE id = @id", new { id }).SingleOrDefault();
        public IllPolicy2 FindIllPolicy2(Guid? id, bool load = false)
        {
            var ip2 = Query<IllPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}ill_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (ip2 == null) return null;
            if (load && ip2.CreationUserId != null) ip2.CreationUser = FindUser2(ip2.CreationUserId);
            if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = FindUser2(ip2.LastWriteUserId);
            return ip2;
        }
        public IndexStatement FindIndexStatement(string id, Guid? holdingId, bool load = false)
        {
            var @is = Query<IndexStatement>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Statement\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}index_statements WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (@is == null) return null;
            if (load && @is.HoldingId != null) @is.Holding = FindHolding2(@is.HoldingId);
            return @is;
        }
        public Instance FindInstance(Guid? id, bool load = false)
        {
            var i = Query<Instance>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", instancestatusid AS \"Instancestatusid\", modeofissuanceid AS \"Modeofissuanceid\", instancetypeid AS \"Instancetypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance WHERE id = @id", new { id }).SingleOrDefault();
            if (i == null) return null;
            if (load && i.Instancestatusid != null) i.InstanceStatus = FindInstanceStatus(i.Instancestatusid);
            if (load && i.Modeofissuanceid != null) i.ModeOfIssuance = FindModeOfIssuance(i.Modeofissuanceid);
            if (load && i.Instancetypeid != null) i.InstanceType = FindInstanceType(i.Instancetypeid);
            return i;
        }
        public Instance2 FindInstance2(Guid? id, bool load = false)
        {
            var i2 = Query<Instance2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", match_key AS \"MatchKey\", source AS \"Source\", title AS \"Title\", author AS \"Author\", publication_year AS \"PublicationYear\", index_title AS \"IndexTitle\", instance_type_id AS \"InstanceTypeId\", mode_of_issuance_id AS \"IssuanceModeId\", cataloged_date AS \"CatalogedDate\", previously_held AS \"PreviouslyHeld\", staff_suppress AS \"StaffSuppress\", discovery_suppress AS \"DiscoverySuppress\", source_record_format AS \"SourceRecordFormat\", status_id AS \"StatusId\", status_updated_date AS \"StatusLastWriteTime\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instances WHERE id = @id", new { id }).SingleOrDefault();
            if (i2 == null) return null;
            if (load && i2.InstanceTypeId != null) i2.InstanceType = FindInstanceType2(i2.InstanceTypeId);
            if (load && i2.IssuanceModeId != null) i2.IssuanceMode = FindIssuanceMode(i2.IssuanceModeId);
            if (load && i2.StatusId != null) i2.Status = FindStatus(i2.StatusId);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId);
            return i2;
        }
        public InstanceFormat FindInstanceFormat(Guid? id, bool load = false) => Query<InstanceFormat>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format WHERE id = @id", new { id }).SingleOrDefault();
        public InstanceFormat2 FindInstanceFormat2(string id, Guid? instanceId, bool load = false)
        {
            var if2 = Query<InstanceFormat2>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", format_id AS \"FormatId\" FROM uc{(IsMySql ? "_" : ".")}instance_formats WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (if2 == null) return null;
            if (load && if2.InstanceId != null) if2.Instance = FindInstance2(if2.InstanceId);
            if (load && if2.FormatId != null) if2.Format = FindFormat(if2.FormatId);
            return if2;
        }
        public InstanceNatureOfContentTerm FindInstanceNatureOfContentTerm(string id, Guid? instanceId, bool load = false)
        {
            var inoct = Query<InstanceNatureOfContentTerm>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", nature_of_content_term_id AS \"NatureOfContentTermId\" FROM uc{(IsMySql ? "_" : ".")}instance_nature_of_content_terms WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (inoct == null) return null;
            if (load && inoct.InstanceId != null) inoct.Instance = FindInstance2(inoct.InstanceId);
            if (load && inoct.NatureOfContentTermId != null) inoct.NatureOfContentTerm = FindNatureOfContentTerm2(inoct.NatureOfContentTermId);
            return inoct;
        }
        public InstanceNoteType FindInstanceNoteType(Guid? id, bool load = false) => Query<InstanceNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type WHERE id = @id", new { id }).SingleOrDefault();
        public InstanceNoteType2 FindInstanceNoteType2(Guid? id, bool load = false)
        {
            var int2 = Query<InstanceNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_note_types WHERE id = @id", new { id }).SingleOrDefault();
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId);
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId);
            return int2;
        }
        public InstanceRelationship FindInstanceRelationship(Guid? id, bool load = false)
        {
            var ir = Query<InstanceRelationship>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", superinstanceid AS \"Superinstanceid\", subinstanceid AS \"Subinstanceid\", instancerelationshiptypeid AS \"Instancerelationshiptypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship WHERE id = @id", new { id }).SingleOrDefault();
            if (ir == null) return null;
            if (load && ir.Superinstanceid != null) ir.Instance1 = FindInstance(ir.Superinstanceid);
            if (load && ir.Subinstanceid != null) ir.Instance = FindInstance(ir.Subinstanceid);
            if (load && ir.Instancerelationshiptypeid != null) ir.InstanceRelationshipType = FindInstanceRelationshipType(ir.Instancerelationshiptypeid);
            return ir;
        }
        public InstanceRelationshipType FindInstanceRelationshipType(Guid? id, bool load = false) => Query<InstanceRelationshipType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type WHERE id = @id", new { id }).SingleOrDefault();
        public InstanceSourceMarc FindInstanceSourceMarc(Guid? id, bool load = false)
        {
            var ism = Query<InstanceSourceMarc>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc WHERE id = @id", new { id }).SingleOrDefault();
            if (ism == null) return null;
            if (load && ism.Id != null) ism.Instance = FindInstance(ism.Id);
            return ism;
        }
        public InstanceStatisticalCode FindInstanceStatisticalCode(string id, Guid? instanceId, bool load = false)
        {
            var isc = Query<InstanceStatisticalCode>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}instance_statistical_codes WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (isc == null) return null;
            if (load && isc.InstanceId != null) isc.Instance = FindInstance2(isc.InstanceId);
            if (load && isc.StatisticalCodeId != null) isc.StatisticalCode = FindStatisticalCode2(isc.StatisticalCodeId);
            return isc;
        }
        public InstanceStatus FindInstanceStatus(Guid? id, bool load = false) => Query<InstanceStatus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status WHERE id = @id", new { id }).SingleOrDefault();
        public InstanceTag FindInstanceTag(string id, Guid? instanceId, bool load = false)
        {
            var it = Query<InstanceTag>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_tags WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (it == null) return null;
            if (load && it.InstanceId != null) it.Instance = FindInstance2(it.InstanceId);
            return it;
        }
        public InstanceType FindInstanceType(Guid? id, bool load = false) => Query<InstanceType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type WHERE id = @id", new { id }).SingleOrDefault();
        public InstanceType2 FindInstanceType2(Guid? id, bool load = false)
        {
            var it2 = Query<InstanceType2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_types WHERE id = @id", new { id }).SingleOrDefault();
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId);
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId);
            return it2;
        }
        public Institution FindInstitution(Guid? id, bool load = false) => Query<Institution>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution WHERE id = @id", new { id }).SingleOrDefault();
        public Institution2 FindInstitution2(Guid? id, bool load = false)
        {
            var i2 = Query<Institution2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}institutions WHERE id = @id", new { id }).SingleOrDefault();
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId);
            return i2;
        }
        public Interface FindInterface(Guid? id, bool load = false) => Query<Interface>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces WHERE id = @id", new { id }).SingleOrDefault();
        public Interface2 FindInterface2(Guid? id, bool load = false)
        {
            var i2 = Query<Interface2>($"SELECT id AS \"Id\", name AS \"Name\", uri AS \"Uri\", notes AS \"Notes\", available AS \"Available\", delivery_method AS \"DeliveryMethod\", statistics_format AS \"StatisticsFormat\", locally_stored AS \"LocallyStored\", online_location AS \"OnlineLocation\", statistics_notes AS \"StatisticsNotes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interfaces WHERE id = @id", new { id }).SingleOrDefault();
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId);
            return i2;
        }
        public InterfaceCredential FindInterfaceCredential(Guid? id, bool load = false)
        {
            var ic = Query<InterfaceCredential>($"SELECT id AS \"Id\", jsonb AS \"Content\", interfaceid AS \"Interfaceid\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials WHERE id = @id", new { id }).SingleOrDefault();
            if (ic == null) return null;
            if (load && ic.Interfaceid != null) ic.Interface = FindInterface(ic.Interfaceid);
            return ic;
        }
        public InterfaceCredential2 FindInterfaceCredential2(Guid? id, bool load = false)
        {
            var ic2 = Query<InterfaceCredential2>($"SELECT id AS \"Id\", username AS \"Username\", password AS \"Password\", interface_id AS \"InterfaceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interface_credentials WHERE id = @id", new { id }).SingleOrDefault();
            if (ic2 == null) return null;
            if (load && ic2.InterfaceId != null) ic2.Interface = FindInterface2(ic2.InterfaceId);
            return ic2;
        }
        public InterfaceType FindInterfaceType(string id, bool load = false)
        {
            var it = Query<InterfaceType>($"SELECT id AS \"Id\", interface_id AS \"InterfaceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interface_type WHERE id = @id", new { id }).SingleOrDefault();
            if (it == null) return null;
            if (load && it.InterfaceId != null) it.Interface = FindInterface2(it.InterfaceId);
            return it;
        }
        public Invoice FindInvoice(Guid? id, bool load = false)
        {
            var i = Query<Invoice>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices WHERE id = @id", new { id }).SingleOrDefault();
            if (i == null) return null;
            if (load && i.Batchgroupid != null) i.BatchGroup = FindBatchGroup(i.Batchgroupid);
            return i;
        }
        public Invoice2 FindInvoice2(Guid? id, bool load = false)
        {
            var i2 = Query<Invoice2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", adjustments_total AS \"AdjustmentsTotal\", approved_by_id AS \"ApprovedById\", approval_date AS \"ApprovalDate\", batch_group_id AS \"BatchGroupId\", bill_to_id AS \"BillToId\", chk_subscription_overlap AS \"ChkSubscriptionOverlap\", currency AS \"Currency\", enclosure_needed AS \"EnclosureNeeded\", exchange_rate AS \"ExchangeRate\", export_to_accounting AS \"ExportToAccounting\", folio_invoice_no AS \"Number\", invoice_date AS \"InvoiceDate\", lock_total AS \"LockTotal\", note AS \"Note\", payment_due AS \"PaymentDue\", payment_terms AS \"PaymentTerms\", payment_method AS \"PaymentMethod\", status AS \"Status\", source AS \"Source\", sub_total AS \"SubTotal\", total AS \"Total\", vendor_invoice_no AS \"VendorInvoiceNo\", disbursement_number AS \"DisbursementNumber\", voucher_number AS \"VoucherNumber\", payment_id AS \"PaymentId\", disbursement_date AS \"DisbursementDate\", vendor_id AS \"VendorId\", manual_payment AS \"ManualPayment\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoices WHERE id = @id", new { id }).SingleOrDefault();
            if (i2 == null) return null;
            if (load && i2.ApprovedById != null) i2.ApprovedBy = FindUser2(i2.ApprovedById);
            if (load && i2.BatchGroupId != null) i2.BatchGroup = FindBatchGroup2(i2.BatchGroupId);
            if (load && i2.BillToId != null) i2.BillTo = FindConfiguration2(i2.BillToId);
            if (load && i2.PaymentId != null) i2.Payment = FindTransaction2(i2.PaymentId);
            if (load && i2.VendorId != null) i2.Vendor = FindOrganization2(i2.VendorId);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId);
            return i2;
        }
        public InvoiceAcquisitionsUnit FindInvoiceAcquisitionsUnit(string id, bool load = false)
        {
            var iau = Query<InvoiceAcquisitionsUnit>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}invoice_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (iau == null) return null;
            if (load && iau.InvoiceId != null) iau.Invoice = FindInvoice2(iau.InvoiceId);
            if (load && iau.AcquisitionsUnitId != null) iau.AcquisitionsUnit = FindAcquisitionsUnit2(iau.AcquisitionsUnitId);
            return iau;
        }
        public InvoiceAdjustment FindInvoiceAdjustment(string id, bool load = false)
        {
            var ia = Query<InvoiceAdjustment>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", id2 AS \"Id2\", adjustment_id AS \"AdjustmentId\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", prorate AS \"Prorate\", relation_to_total AS \"RelationToTotal\", type AS \"Type\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_adjustments WHERE id = @id", new { id }).SingleOrDefault();
            if (ia == null) return null;
            if (load && ia.InvoiceId != null) ia.Invoice = FindInvoice2(ia.InvoiceId);
            return ia;
        }
        public InvoiceAdjustmentFund FindInvoiceAdjustmentFund(string id, bool load = false)
        {
            var iaf = Query<InvoiceAdjustmentFund>($"SELECT id AS \"Id\", invoice_adjustment_id AS \"InvoiceAdjustmentId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_adjustment_fund_distributions WHERE id = @id", new { id }).SingleOrDefault();
            if (iaf == null) return null;
            if (load && iaf.EncumbranceId != null) iaf.Encumbrance = FindTransaction2(iaf.EncumbranceId);
            if (load && iaf.FundId != null) iaf.Fund = FindFund2(iaf.FundId);
            if (load && iaf.InvoiceItemId != null) iaf.InvoiceItem = FindInvoiceItem2(iaf.InvoiceItemId);
            if (load && iaf.ExpenseClassId != null) iaf.ExpenseClass = FindExpenseClass2(iaf.ExpenseClassId);
            return iaf;
        }
        public InvoiceItem FindInvoiceItem(Guid? id, bool load = false)
        {
            var ii = Query<InvoiceItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines WHERE id = @id", new { id }).SingleOrDefault();
            if (ii == null) return null;
            if (load && ii.Invoiceid != null) ii.Invoice = FindInvoice(ii.Invoiceid);
            return ii;
        }
        public InvoiceItem2 FindInvoiceItem2(Guid? id, bool load = false)
        {
            var ii2 = Query<InvoiceItem2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", account_number AS \"AccountNumber\", adjustments_total AS \"AdjustmentsTotal\", comment AS \"Comment\", description AS \"Description\", invoice_id AS \"InvoiceId\", invoice_line_number AS \"Number\", invoice_line_status AS \"InvoiceLineStatus\", po_line_id AS \"OrderItemId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", quantity AS \"Quantity\", release_encumbrance AS \"ReleaseEncumbrance\", subscription_info AS \"SubscriptionInfo\", subscription_start AS \"SubscriptionStart\", subscription_end AS \"SubscriptionEnd\", sub_total AS \"SubTotal\", total AS \"Total\", vendor_ref_no AS \"VendorRefNo\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_items WHERE id = @id", new { id }).SingleOrDefault();
            if (ii2 == null) return null;
            if (load && ii2.InvoiceId != null) ii2.Invoice = FindInvoice2(ii2.InvoiceId);
            if (load && ii2.OrderItemId != null) ii2.OrderItem = FindOrderItem2(ii2.OrderItemId);
            if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = FindIdType2(ii2.ProductIdTypeId);
            if (load && ii2.CreationUserId != null) ii2.CreationUser = FindUser2(ii2.CreationUserId);
            if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = FindUser2(ii2.LastWriteUserId);
            return ii2;
        }
        public InvoiceItemAdjustment FindInvoiceItemAdjustment(string id, bool load = false)
        {
            var iia = Query<InvoiceItemAdjustment>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", id2 AS \"Id2\", adjustment_id AS \"AdjustmentId\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", prorate AS \"Prorate\", relation_to_total AS \"RelationToTotal\", type AS \"Type\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustments WHERE id = @id", new { id }).SingleOrDefault();
            if (iia == null) return null;
            if (load && iia.InvoiceItemId != null) iia.InvoiceItem = FindInvoiceItem2(iia.InvoiceItemId);
            return iia;
        }
        public InvoiceItemAdjustmentFund FindInvoiceItemAdjustmentFund(string id, bool load = false)
        {
            var iiaf = Query<InvoiceItemAdjustmentFund>($"SELECT id AS \"Id\", invoice_item_adjustment_id AS \"InvoiceItemAdjustmentId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustment_fund_distributions WHERE id = @id", new { id }).SingleOrDefault();
            if (iiaf == null) return null;
            if (load && iiaf.EncumbranceId != null) iiaf.Encumbrance = FindTransaction2(iiaf.EncumbranceId);
            if (load && iiaf.FundId != null) iiaf.Fund = FindFund2(iiaf.FundId);
            if (load && iiaf.InvoiceItemId != null) iiaf.InvoiceItem = FindInvoiceItem2(iiaf.InvoiceItemId);
            if (load && iiaf.ExpenseClassId != null) iiaf.ExpenseClass = FindExpenseClass2(iiaf.ExpenseClassId);
            return iiaf;
        }
        public InvoiceItemFund FindInvoiceItemFund(string id, bool load = false)
        {
            var iif = Query<InvoiceItemFund>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_fund_distributions WHERE id = @id", new { id }).SingleOrDefault();
            if (iif == null) return null;
            if (load && iif.InvoiceItemId != null) iif.InvoiceItem = FindInvoiceItem2(iif.InvoiceItemId);
            if (load && iif.EncumbranceId != null) iif.Encumbrance = FindTransaction2(iif.EncumbranceId);
            if (load && iif.FundId != null) iif.Fund = FindFund2(iif.FundId);
            if (load && iif.ExpenseClassId != null) iif.ExpenseClass = FindExpenseClass2(iif.ExpenseClassId);
            return iif;
        }
        public InvoiceItemTag FindInvoiceItemTag(string id, bool load = false)
        {
            var iit = Query<InvoiceItemTag>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (iit == null) return null;
            if (load && iit.InvoiceItemId != null) iit.InvoiceItem = FindInvoiceItem2(iit.InvoiceItemId);
            return iit;
        }
        public InvoiceOrderNumber FindInvoiceOrderNumber(string id, bool load = false)
        {
            var ion = Query<InvoiceOrderNumber>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_order_numbers WHERE id = @id", new { id }).SingleOrDefault();
            if (ion == null) return null;
            if (load && ion.InvoiceId != null) ion.Invoice = FindInvoice2(ion.InvoiceId);
            return ion;
        }
        public InvoiceTag FindInvoiceTag(string id, bool load = false)
        {
            var it = Query<InvoiceTag>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (it == null) return null;
            if (load && it.InvoiceId != null) it.Invoice = FindInvoice2(it.InvoiceId);
            return it;
        }
        public InvoiceTransactionSummary FindInvoiceTransactionSummary(Guid? id, bool load = false) => Query<InvoiceTransactionSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries WHERE id = @id", new { id }).SingleOrDefault();
        public InvoiceTransactionSummary2 FindInvoiceTransactionSummary2(Guid? id, bool load = false)
        {
            var its2 = Query<InvoiceTransactionSummary2>($"SELECT id AS \"Id\", num_pending_payments AS \"NumPendingPayments\", num_payments_credits AS \"NumPaymentsCredits\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_transaction_summaries WHERE id = @id", new { id }).SingleOrDefault();
            if (its2 == null) return null;
            if (load && its2.Id != null) its2.Invoice2 = FindInvoice2(its2.Id);
            return its2;
        }
        public IssuanceMode FindIssuanceMode(Guid? id, bool load = false)
        {
            var im = Query<IssuanceMode>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}mode_of_issuances WHERE id = @id", new { id }).SingleOrDefault();
            if (im == null) return null;
            if (load && im.CreationUserId != null) im.CreationUser = FindUser2(im.CreationUserId);
            if (load && im.LastWriteUserId != null) im.LastWriteUser = FindUser2(im.LastWriteUserId);
            return im;
        }
        public Item FindItem(Guid? id, bool load = false)
        {
            var i = Query<Item>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", holdingsrecordid AS \"Holdingsrecordid\", permanentloantypeid AS \"Permanentloantypeid\", temporaryloantypeid AS \"Temporaryloantypeid\", materialtypeid AS \"Materialtypeid\", permanentlocationid AS \"Permanentlocationid\", temporarylocationid AS \"Temporarylocationid\", effectivelocationid AS \"Effectivelocationid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item WHERE id = @id", new { id }).SingleOrDefault();
            if (i == null) return null;
            if (load && i.Holdingsrecordid != null) i.Holding = FindHolding(i.Holdingsrecordid);
            if (load && i.Permanentloantypeid != null) i.LoanType = FindLoanType(i.Permanentloantypeid);
            if (load && i.Temporaryloantypeid != null) i.LoanType1 = FindLoanType(i.Temporaryloantypeid);
            if (load && i.Materialtypeid != null) i.MaterialType = FindMaterialType(i.Materialtypeid);
            if (load && i.Permanentlocationid != null) i.Location1 = FindLocation(i.Permanentlocationid);
            if (load && i.Temporarylocationid != null) i.Location2 = FindLocation(i.Temporarylocationid);
            if (load && i.Effectivelocationid != null) i.Location = FindLocation(i.Effectivelocationid);
            return i;
        }
        public Item2 FindItem2(Guid? id, bool load = false)
        {
            var i2 = Query<Item2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", holding_id AS \"HoldingId\", discovery_suppress AS \"DiscoverySuppress\", accession_number AS \"AccessionNumber\", barcode AS \"Barcode\", call_number AS \"CallNumber\", call_number_prefix AS \"CallNumberPrefix\", call_number_suffix AS \"CallNumberSuffix\", call_number_type_id AS \"CallNumberTypeId\", effective_call_number AS \"EffectiveCallNumber\", effective_call_number_prefix AS \"EffectiveCallNumberPrefix\", effective_call_number_suffix AS \"EffectiveCallNumberSuffix\", effective_call_number_type_id AS \"EffectiveCallNumberTypeId\", volume AS \"Volume\", enumeration AS \"Enumeration\", chronology AS \"Chronology\", item_identifier AS \"ItemIdentifier\", copy_number AS \"CopyNumber\", number_of_pieces AS \"PiecesCount\", description_of_pieces AS \"PiecesDescription\", number_of_missing_pieces AS \"MissingPiecesCount\", missing_pieces AS \"MissingPiecesDescription\", missing_pieces_date AS \"MissingPiecesTime\", item_damaged_status_id AS \"DamagedStatusId\", item_damaged_status_date AS \"DamagedStatusTime\", status_name AS \"StatusName\", status_date AS \"StatusDate\", material_type_id AS \"MaterialTypeId\", permanent_loan_type_id AS \"PermanentLoanTypeId\", temporary_loan_type_id AS \"TemporaryLoanTypeId\", permanent_location_id AS \"PermanentLocationId\", temporary_location_id AS \"TemporaryLocationId\", effective_location_id AS \"EffectiveLocationId\", in_transit_destination_service_point_id AS \"InTransitDestinationServicePointId\", order_item_id AS \"OrderItemId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", last_check_in_date_time AS \"LastCheckInDateTime\", last_check_in_service_point_id AS \"LastCheckInServicePointId\", last_check_in_staff_member_id AS \"LastCheckInStaffMemberId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}items WHERE id = @id", new { id }).SingleOrDefault();
            if (i2 == null) return null;
            if (load && i2.HoldingId != null) i2.Holding = FindHolding2(i2.HoldingId);
            if (load && i2.CallNumberTypeId != null) i2.CallNumberType = FindCallNumberType2(i2.CallNumberTypeId);
            if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = FindCallNumberType2(i2.EffectiveCallNumberTypeId);
            if (load && i2.DamagedStatusId != null) i2.DamagedStatus = FindItemDamagedStatus2(i2.DamagedStatusId);
            if (load && i2.MaterialTypeId != null) i2.MaterialType = FindMaterialType2(i2.MaterialTypeId);
            if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = FindLoanType2(i2.PermanentLoanTypeId);
            if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = FindLoanType2(i2.TemporaryLoanTypeId);
            if (load && i2.PermanentLocationId != null) i2.PermanentLocation = FindLocation2(i2.PermanentLocationId);
            if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = FindLocation2(i2.TemporaryLocationId);
            if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = FindLocation2(i2.EffectiveLocationId);
            if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = FindServicePoint2(i2.InTransitDestinationServicePointId);
            if (load && i2.OrderItemId != null) i2.OrderItem = FindOrderItem2(i2.OrderItemId);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId);
            if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = FindServicePoint2(i2.LastCheckInServicePointId);
            if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = FindUser2(i2.LastCheckInStaffMemberId);
            return i2;
        }
        public ItemDamagedStatus FindItemDamagedStatus(Guid? id, bool load = false) => Query<ItemDamagedStatus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status WHERE id = @id", new { id }).SingleOrDefault();
        public ItemDamagedStatus2 FindItemDamagedStatus2(Guid? id, bool load = false)
        {
            var ids2 = Query<ItemDamagedStatus2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_damaged_statuses WHERE id = @id", new { id }).SingleOrDefault();
            if (ids2 == null) return null;
            if (load && ids2.CreationUserId != null) ids2.CreationUser = FindUser2(ids2.CreationUserId);
            if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = FindUser2(ids2.LastWriteUserId);
            return ids2;
        }
        public ItemElectronicAccess FindItemElectronicAccess(string id, Guid? itemId, bool load = false)
        {
            var iea = Query<ItemElectronicAccess>($"SELECT id AS \"Id\", item_id AS \"ItemId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}item_electronic_accesses WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (iea == null) return null;
            if (load && iea.ItemId != null) iea.Item = FindItem2(iea.ItemId);
            if (load && iea.RelationshipId != null) iea.Relationship = FindRelationship(iea.RelationshipId);
            return iea;
        }
        public ItemFormerId FindItemFormerId(string id, Guid? itemId, bool load = false)
        {
            var ifi = Query<ItemFormerId>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_former_ids WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (ifi == null) return null;
            if (load && ifi.ItemId != null) ifi.Item = FindItem2(ifi.ItemId);
            return ifi;
        }
        public ItemNote FindItemNote(string id, Guid? itemId, bool load = false)
        {
            var @in = Query<ItemNote>($"SELECT id AS \"Id\", item_id AS \"ItemId\", item_note_type_id AS \"ItemNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}item_notes WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (@in == null) return null;
            if (load && @in.ItemId != null) @in.Item = FindItem2(@in.ItemId);
            if (load && @in.ItemNoteTypeId != null) @in.ItemNoteType = FindItemNoteType2(@in.ItemNoteTypeId);
            return @in;
        }
        public ItemNoteType FindItemNoteType(Guid? id, bool load = false) => Query<ItemNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type WHERE id = @id", new { id }).SingleOrDefault();
        public ItemNoteType2 FindItemNoteType2(Guid? id, bool load = false)
        {
            var int2 = Query<ItemNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_note_types WHERE id = @id", new { id }).SingleOrDefault();
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId);
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId);
            return int2;
        }
        public ItemStatisticalCode FindItemStatisticalCode(string id, Guid? itemId, bool load = false)
        {
            var isc = Query<ItemStatisticalCode>($"SELECT id AS \"Id\", item_id AS \"ItemId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}item_statistical_codes WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (isc == null) return null;
            if (load && isc.ItemId != null) isc.Item = FindItem2(isc.ItemId);
            if (load && isc.StatisticalCodeId != null) isc.StatisticalCode = FindStatisticalCode2(isc.StatisticalCodeId);
            return isc;
        }
        public ItemTag FindItemTag(string id, Guid? itemId, bool load = false)
        {
            var it = Query<ItemTag>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_tags WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (it == null) return null;
            if (load && it.ItemId != null) it.Item = FindItem2(it.ItemId);
            return it;
        }
        public ItemYearCaption FindItemYearCaption(string id, Guid? itemId, bool load = false)
        {
            var iyc = Query<ItemYearCaption>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_year_caption WHERE id = @id AND item_id = @itemId", new { id, itemId }).SingleOrDefault();
            if (iyc == null) return null;
            if (load && iyc.ItemId != null) iyc.Item = FindItem2(iyc.ItemId);
            return iyc;
        }
        public JobExecution FindJobExecution(Guid? id, bool load = false) => Query<JobExecution>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions WHERE id = @id", new { id }).SingleOrDefault();
        public JobExecution2 FindJobExecution2(Guid? id, bool load = false)
        {
            var je2 = Query<JobExecution2>($"SELECT id AS \"Id\", hr_id AS \"HrId\", parent_job_id AS \"ParentJobId\", subordination_type AS \"SubordinationType\", job_profile_info_name AS \"JobProfileInfoName\", job_profile_info_data_type AS \"JobProfileInfoDataType\", job_profile_snapshot_wrapper_profile_id AS \"JobProfileSnapshotWrapperProfileId\", job_profile_snapshot_wrapper_content_type AS \"JobProfileSnapshotWrapperContentType\", job_profile_snapshot_wrapper_react_to AS \"JobProfileSnapshotWrapperReactTo\", job_profile_snapshot_wrapper_order AS \"JobProfileSnapshotWrapperOrder\", source_path AS \"SourcePath\", file_name AS \"FileName\", run_by_first_name AS \"RunByFirstName\", run_by_last_name AS \"RunByLastName\", progress_job_execution_id AS \"ProgressJobExecutionId\", progress_current AS \"ProgressCurrent\", progress_total AS \"ProgressTotal\", started_date AS \"StartedDate\", completed_date AS \"CompletedDate\", status AS \"Status\", ui_status AS \"UiStatus\", error_status AS \"ErrorStatus\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}job_executions WHERE id = @id", new { id }).SingleOrDefault();
            if (je2 == null) return null;
            if (load && je2.ParentJobId != null) je2.ParentJob = FindJobExecution2(je2.ParentJobId);
            if (load && je2.ProgressJobExecutionId != null) je2.ProgressJobExecution = FindJobExecution2(je2.ProgressJobExecutionId);
            if (load && je2.UserId != null) je2.User = FindUser2(je2.UserId);
            return je2;
        }
        public JobExecutionProgress FindJobExecutionProgress(Guid? id, bool load = false)
        {
            var jep = Query<JobExecutionProgress>($"SELECT id AS \"Id\", jsonb AS \"Content\", jobexecutionid AS \"Jobexecutionid\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress WHERE id = @id", new { id }).SingleOrDefault();
            if (jep == null) return null;
            if (load && jep.Jobexecutionid != null) jep.JobExecution = FindJobExecution(jep.Jobexecutionid);
            return jep;
        }
        public JobExecutionSourceChunk FindJobExecutionSourceChunk(Guid? id, bool load = false)
        {
            var jesc = Query<JobExecutionSourceChunk>($"SELECT id AS \"Id\", jsonb AS \"Content\", jobexecutionid AS \"Jobexecutionid\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks WHERE id = @id", new { id }).SingleOrDefault();
            if (jesc == null) return null;
            if (load && jesc.Jobexecutionid != null) jesc.JobExecution = FindJobExecution(jesc.Jobexecutionid);
            return jesc;
        }
        public JobExecutionSourceChunk2 FindJobExecutionSourceChunk2(Guid? id, bool load = false)
        {
            var jesc2 = Query<JobExecutionSourceChunk2>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", last AS \"Last\", state AS \"State\", chunk_size AS \"ChunkSize\", processed_amount AS \"ProcessedAmount\", completed_date AS \"CompletedDate\", error AS \"Error\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}job_execution_source_chunks WHERE id = @id", new { id }).SingleOrDefault();
            if (jesc2 == null) return null;
            if (load && jesc2.JobExecutionId != null) jesc2.JobExecution = FindJobExecution2(jesc2.JobExecutionId);
            return jesc2;
        }
        public JournalRecord FindJournalRecord(Guid? id, bool load = false)
        {
            var jr = Query<JournalRecord>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", source_id AS \"SourceId\", entity_type AS \"EntityType\", entity_id AS \"EntityId\", entity_hrid AS \"EntityHrid\", action_type AS \"ActionType\", action_status AS \"ActionStatus\", action_date AS \"ActionDate\", source_record_order AS \"SourceRecordOrder\", error AS \"Error\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records WHERE id = @id", new { id }).SingleOrDefault();
            if (jr == null) return null;
            if (load && jr.JobExecutionId != null) jr.JobExecution = FindJobExecution(jr.JobExecutionId);
            return jr;
        }
        public JournalRecord2 FindJournalRecord2(Guid? id, bool load = false)
        {
            var jr2 = Query<JournalRecord2>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", source_id AS \"SourceId\", entity_type AS \"EntityType\", entity_id AS \"EntityId\", entity_hrid AS \"EntityHrid\", action_type AS \"ActionType\", action_status AS \"ActionStatus\", action_date AS \"ActionDate\", source_record_order AS \"SourceRecordOrder\", error AS \"Error\" FROM uc{(IsMySql ? "_" : ".")}journal_records WHERE id = @id", new { id }).SingleOrDefault();
            if (jr2 == null) return null;
            if (load && jr2.JobExecutionId != null) jr2.JobExecution = FindJobExecution2(jr2.JobExecutionId);
            return jr2;
        }
        public Language FindLanguage(string id, Guid? instanceId, bool load = false)
        {
            var l = Query<Language>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}languages WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (l == null) return null;
            if (load && l.InstanceId != null) l.Instance = FindInstance2(l.InstanceId);
            return l;
        }
        public Ledger FindLedger(Guid? id, bool load = false)
        {
            var l = Query<Ledger>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fiscalyearoneid AS \"Fiscalyearoneid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger WHERE id = @id", new { id }).SingleOrDefault();
            if (l == null) return null;
            if (load && l.Fiscalyearoneid != null) l.FiscalYear = FindFiscalYear(l.Fiscalyearoneid);
            return l;
        }
        public Ledger2 FindLedger2(Guid? id, bool load = false)
        {
            var l2 = Query<Ledger2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", fiscal_year_one_id AS \"FiscalYearOneId\", ledger_status AS \"LedgerStatus\", allocated AS \"Allocated\", available AS \"Available\", net_transfers AS \"NetTransfers\", unavailable AS \"Unavailable\", currency AS \"Currency\", restrict_encumbrance AS \"RestrictEncumbrance\", restrict_expenditures AS \"RestrictExpenditures\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}ledgers WHERE id = @id", new { id }).SingleOrDefault();
            if (l2 == null) return null;
            if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = FindFiscalYear2(l2.FiscalYearOneId);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId);
            return l2;
        }
        public LedgerAcquisitionsUnit FindLedgerAcquisitionsUnit(string id, bool load = false)
        {
            var lau = Query<LedgerAcquisitionsUnit>($"SELECT id AS \"Id\", ledger_id AS \"LedgerId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}ledger_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (lau == null) return null;
            if (load && lau.LedgerId != null) lau.Ledger = FindLedger2(lau.LedgerId);
            if (load && lau.AcquisitionsUnitId != null) lau.AcquisitionsUnit = FindAcquisitionsUnit2(lau.AcquisitionsUnitId);
            return lau;
        }
        public Library FindLibrary(Guid? id, bool load = false)
        {
            var l = Query<Library>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", campusid AS \"Campusid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary WHERE id = @id", new { id }).SingleOrDefault();
            if (l == null) return null;
            if (load && l.Campusid != null) l.Campus = FindCampus(l.Campusid);
            return l;
        }
        public Library2 FindLibrary2(Guid? id, bool load = false)
        {
            var l2 = Query<Library2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", campus_id AS \"CampusId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}libraries WHERE id = @id", new { id }).SingleOrDefault();
            if (l2 == null) return null;
            if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId);
            return l2;
        }
        public Loan FindLoan(Guid? id, bool load = false) => Query<Loan>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan WHERE id = @id", new { id }).SingleOrDefault();
        public Loan2 FindLoan2(Guid? id, bool load = false)
        {
            var l2 = Query<Loan2>($"SELECT id AS \"Id\", user_id AS \"UserId\", proxy_user_id AS \"ProxyUserId\", item_id AS \"ItemId\", item_effective_location_at_check_out_id AS \"ItemEffectiveLocationAtCheckOutId\", status_name AS \"StatusName\", loan_date AS \"LoanTime\", due_date AS \"DueTime\", return_date AS \"ReturnTime\", system_return_date AS \"SystemReturnTime\", action AS \"Action\", action_comment AS \"ActionComment\", item_status AS \"ItemStatus\", renewal_count AS \"RenewalCount\", loan_policy_id AS \"LoanPolicyId\", checkout_service_point_id AS \"CheckoutServicePointId\", checkin_service_point_id AS \"CheckinServicePointId\", group_id AS \"GroupId\", due_date_changed_by_recall AS \"DueDateChangedByRecall\", declared_lost_date AS \"DeclaredLostDate\", claimed_returned_date AS \"ClaimedReturnedDate\", overdue_fine_policy_id AS \"OverdueFinePolicyId\", lost_item_policy_id AS \"LostItemPolicyId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", aged_to_lost_delayed_billing_lost_item_has_been_billed AS \"AgedToLostDelayedBillingLostItemHasBeenBilled\", aged_to_lost_delayed_billing_date_lost_item_should_be_billed AS \"AgedToLostDelayedBillingDateLostItemShouldBeBilled\", aged_to_lost_delayed_billing_aged_to_lost_date AS \"AgedToLostDelayedBillingAgedToLostDate\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loans WHERE id = @id", new { id }).SingleOrDefault();
            if (l2 == null) return null;
            if (load && l2.UserId != null) l2.User = FindUser2(l2.UserId);
            if (load && l2.ProxyUserId != null) l2.ProxyUser = FindUser2(l2.ProxyUserId);
            if (load && l2.ItemId != null) l2.Item = FindItem2(l2.ItemId);
            if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId);
            if (load && l2.LoanPolicyId != null) l2.LoanPolicy = FindLoanPolicy2(l2.LoanPolicyId);
            if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = FindServicePoint2(l2.CheckoutServicePointId);
            if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = FindServicePoint2(l2.CheckinServicePointId);
            if (load && l2.GroupId != null) l2.Group = FindGroup2(l2.GroupId);
            if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = FindOverdueFinePolicy2(l2.OverdueFinePolicyId);
            if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = FindLostItemFeePolicy2(l2.LostItemPolicyId);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId);
            return l2;
        }
        public LoanPolicy FindLoanPolicy(Guid? id, bool load = false)
        {
            var lp = Query<LoanPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", loanspolicy_fixedduedatescheduleid AS \"LoanspolicyFixedduedatescheduleid\", renewalspolicy_alternatefixedduedatescheduleid AS \"RenewalspolicyAlternatefixedduedatescheduleid\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy WHERE id = @id", new { id }).SingleOrDefault();
            if (lp == null) return null;
            if (load && lp.LoanspolicyFixedduedatescheduleid != null) lp.FixedDueDateSchedule = FindFixedDueDateSchedule(lp.LoanspolicyFixedduedatescheduleid);
            if (load && lp.RenewalspolicyAlternatefixedduedatescheduleid != null) lp.FixedDueDateSchedule1 = FindFixedDueDateSchedule(lp.RenewalspolicyAlternatefixedduedatescheduleid);
            return lp;
        }
        public LoanPolicy2 FindLoanPolicy2(Guid? id, bool load = false)
        {
            var lp2 = Query<LoanPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", loanable AS \"Loanable\", loans_policy_profile_id AS \"LoansPolicyProfileId\", loans_policy_period_duration AS \"LoansPolicyPeriodDuration\", loans_policy_period_interval_id AS \"LoansPolicyPeriodInterval\", loans_policy_closed_library_due_date_management_id AS \"LoansPolicyClosedLibraryDueDateManagementId\", loans_policy_grace_period_duration AS \"LoansPolicyGracePeriodDuration\", loans_policy_grace_period_interval_id AS \"LoansPolicyGracePeriodInterval\", loans_policy_opening_time_offset_duration AS \"LoansPolicyOpeningTimeOffsetDuration\", loans_policy_opening_time_offset_interval_id AS \"LoansPolicyOpeningTimeOffsetInterval\", loans_policy_fixed_due_date_schedule_id AS \"LoansPolicyFixedDueDateScheduleId\", loans_policy_item_limit AS \"LoansPolicyItemLimit\", renewable AS \"Renewable\", renewals_policy_unlimited AS \"RenewalsPolicyUnlimited\", renewals_policy_number_allowed AS \"RenewalsPolicyNumberAllowed\", renewals_policy_renew_from_id AS \"RenewalsPolicyRenewFromId\", renewals_policy_different_period AS \"RenewalsPolicyDifferentPeriod\", renewals_policy_period_duration AS \"RenewalsPolicyPeriodDuration\", renewals_policy_period_interval_id AS \"RenewalsPolicyPeriodInterval\", renewals_policy_alternate_fixed_due_date_schedule_id AS \"RenewalsPolicyAlternateFixedDueDateScheduleId\", recalls_alternate_grace_period_duration AS \"RecallsAlternateGracePeriodDuration\", recalls_alternate_grace_period_interval_id AS \"RecallsAlternateGracePeriodInterval\", recalls_minimum_guaranteed_loan_period_duration AS \"RecallsMinimumGuaranteedLoanPeriodDuration\", recalls_minimum_guaranteed_loan_period_interval_id AS \"RecallsMinimumGuaranteedLoanPeriodInterval\", recalls_recall_return_interval_duration AS \"RecallsRecallReturnIntervalDuration\", recalls_recall_return_interval_interval_id AS \"RecallsRecallReturnIntervalInterval\", holds_alternate_checkout_loan_period_duration AS \"HoldsAlternateCheckoutLoanPeriodDuration\", holds_alternate_checkout_loan_period_interval_id AS \"HoldsAlternateCheckoutLoanPeriodInterval\", holds_renew_items_with_request AS \"HoldsRenewItemsWithRequest\", holds_alternate_renewal_loan_period_duration AS \"HoldsAlternateRenewalLoanPeriodDuration\", holds_alternate_renewal_loan_period_interval_id AS \"HoldsAlternateRenewalLoanPeriodInterval\", pages_alternate_checkout_loan_period_duration AS \"PagesAlternateCheckoutLoanPeriodDuration\", pages_alternate_checkout_loan_period_interval_id AS \"PagesAlternateCheckoutLoanPeriodInterval\", pages_renew_items_with_request AS \"PagesRenewItemsWithRequest\", pages_alternate_renewal_loan_period_duration AS \"PagesAlternateRenewalLoanPeriodDuration\", pages_alternate_renewal_loan_period_interval_id AS \"PagesAlternateRenewalLoanPeriodInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loan_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (lp2 == null) return null;
            if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId);
            if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId);
            if (load && lp2.CreationUserId != null) lp2.CreationUser = FindUser2(lp2.CreationUserId);
            if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = FindUser2(lp2.LastWriteUserId);
            return lp2;
        }
        public LoanType FindLoanType(Guid? id, bool load = false) => Query<LoanType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type WHERE id = @id", new { id }).SingleOrDefault();
        public LoanType2 FindLoanType2(Guid? id, bool load = false)
        {
            var lt2 = Query<LoanType2>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loan_types WHERE id = @id", new { id }).SingleOrDefault();
            if (lt2 == null) return null;
            if (load && lt2.CreationUserId != null) lt2.CreationUser = FindUser2(lt2.CreationUserId);
            if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = FindUser2(lt2.LastWriteUserId);
            return lt2;
        }
        public Location FindLocation(Guid? id, bool load = false)
        {
            var l = Query<Location>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", institutionid AS \"Institutionid\", campusid AS \"Campusid\", libraryid AS \"Libraryid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location WHERE id = @id", new { id }).SingleOrDefault();
            if (l == null) return null;
            if (load && l.Institutionid != null) l.Institution = FindInstitution(l.Institutionid);
            if (load && l.Campusid != null) l.Campus = FindCampus(l.Campusid);
            if (load && l.Libraryid != null) l.Library = FindLibrary(l.Libraryid);
            return l;
        }
        public Location2 FindLocation2(Guid? id, bool load = false)
        {
            var l2 = Query<Location2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", discovery_display_name AS \"DiscoveryDisplayName\", is_active AS \"IsActive\", institution_id AS \"InstitutionId\", campus_id AS \"CampusId\", library_id AS \"LibraryId\", primary_service_point_id AS \"PrimaryServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}locations WHERE id = @id", new { id }).SingleOrDefault();
            if (l2 == null) return null;
            if (load && l2.InstitutionId != null) l2.Institution = FindInstitution2(l2.InstitutionId);
            if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId);
            if (load && l2.LibraryId != null) l2.Library = FindLibrary2(l2.LibraryId);
            if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = FindServicePoint2(l2.PrimaryServicePointId);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId);
            return l2;
        }
        public LocationServicePoint FindLocationServicePoint(string id, bool load = false)
        {
            var lsp = Query<LocationServicePoint>($"SELECT id AS \"Id\", location_id AS \"LocationId\", service_point_id AS \"ServicePointId\" FROM uc{(IsMySql ? "_" : ".")}location_service_points WHERE id = @id", new { id }).SingleOrDefault();
            if (lsp == null) return null;
            if (load && lsp.LocationId != null) lsp.Location = FindLocation2(lsp.LocationId);
            if (load && lsp.ServicePointId != null) lsp.ServicePoint = FindServicePoint2(lsp.ServicePointId);
            return lsp;
        }
        public LocationSetting FindLocationSetting(Guid? id, bool load = false)
        {
            var ls = Query<LocationSetting>($"SELECT id AS \"Id\", location_id AS \"LocationId\", settings_id AS \"SettingsId\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}location_settings WHERE id = @id", new { id }).SingleOrDefault();
            if (ls == null) return null;
            if (load && ls.LocationId != null) ls.Location = FindLocation2(ls.LocationId);
            if (load && ls.SettingsId != null) ls.Settings = FindSetting(ls.SettingsId);
            if (load && ls.CreationUserId != null) ls.CreationUser = FindUser2(ls.CreationUserId);
            if (load && ls.LastWriteUserId != null) ls.LastWriteUser = FindUser2(ls.LastWriteUserId);
            return ls;
        }
        public Login FindLogin(Guid? id, bool load = false) => Query<Login>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials WHERE id = @id", new { id }).SingleOrDefault();
        public Login2 FindLogin2(Guid? id, bool load = false)
        {
            var l2 = Query<Login2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hash AS \"Hash\", salt AS \"Salt\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}logins WHERE id = @id", new { id }).SingleOrDefault();
            if (l2 == null) return null;
            if (load && l2.UserId != null) l2.User = FindUser2(l2.UserId);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId);
            return l2;
        }
        public LostItemFeePolicy FindLostItemFeePolicy(Guid? id, bool load = false) => Query<LostItemFeePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy WHERE id = @id", new { id }).SingleOrDefault();
        public LostItemFeePolicy2 FindLostItemFeePolicy2(Guid? id, bool load = false)
        {
            var lifp2 = Query<LostItemFeePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", item_aged_lost_overdue_duration AS \"ItemAgedLostOverdueDuration\", item_aged_lost_overdue_interval_id AS \"ItemAgedLostOverdueInterval\", patron_billed_after_aged_lost_duration AS \"PatronBilledAfterAgedLostDuration\", patron_billed_after_aged_lost_interval_id AS \"PatronBilledAfterAgedLostInterval\", charge_amount_item_charge_type AS \"ChargeAmountItemChargeType\", charge_amount_item_amount AS \"ChargeAmountItemAmount\", lost_item_processing_fee AS \"LostItemProcessingFee\", charge_amount_item_patron AS \"ChargeAmountItemPatron\", charge_amount_item_system AS \"ChargeAmountItemSystem\", lost_item_charge_fee_fine_duration AS \"LostItemChargeFeeFineDuration\", lost_item_charge_fee_fine_interval_id AS \"LostItemChargeFeeFineInterval\", returned_lost_item_processing_fee AS \"ReturnedLostItemProcessingFee\", replaced_lost_item_processing_fee AS \"ReplacedLostItemProcessingFee\", replacement_processing_fee AS \"ReplacementProcessingFee\", replacement_allowed AS \"ReplacementAllowed\", lost_item_returned AS \"LostItemReturned\", fees_fines_shall_refunded_duration AS \"FeesFinesShallRefundedDuration\", fees_fines_shall_refunded_interval_id AS \"FeesFinesShallRefundedInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}lost_item_fee_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (lifp2 == null) return null;
            if (load && lifp2.CreationUserId != null) lifp2.CreationUser = FindUser2(lifp2.CreationUserId);
            if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = FindUser2(lifp2.LastWriteUserId);
            return lifp2;
        }
        public MappingRule FindMappingRule(Guid? id, bool load = false) => Query<MappingRule>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules WHERE id = @id", new { id }).SingleOrDefault();
        public MarcRecord FindMarcRecord(Guid? id, bool load = false)
        {
            var mr = Query<MarcRecord>($"SELECT id AS \"Id\", content AS \"Content\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb WHERE id = @id", new { id }).SingleOrDefault();
            if (mr == null) return null;
            if (load && mr.Id != null) mr.Record = FindRecord(mr.Id);
            return mr;
        }
        public MarcRecord2 FindMarcRecord2(Guid? id, bool load = false)
        {
            var mr2 = Query<MarcRecord2>($"SELECT id AS \"Id\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}marc_records WHERE id = @id", new { id }).SingleOrDefault();
            if (mr2 == null) return null;
            if (load && mr2.Id != null) mr2.Record2 = FindRecord2(mr2.Id);
            return mr2;
        }
        public MaterialType FindMaterialType(Guid? id, bool load = false) => Query<MaterialType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type WHERE id = @id", new { id }).SingleOrDefault();
        public MaterialType2 FindMaterialType2(Guid? id, bool load = false)
        {
            var mt2 = Query<MaterialType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}material_types WHERE id = @id", new { id }).SingleOrDefault();
            if (mt2 == null) return null;
            if (load && mt2.CreationUserId != null) mt2.CreationUser = FindUser2(mt2.CreationUserId);
            if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = FindUser2(mt2.LastWriteUserId);
            return mt2;
        }
        public ModeOfIssuance FindModeOfIssuance(Guid? id, bool load = false) => Query<ModeOfIssuance>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance WHERE id = @id", new { id }).SingleOrDefault();
        public NatureOfContentTerm FindNatureOfContentTerm(Guid? id, bool load = false) => Query<NatureOfContentTerm>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term WHERE id = @id", new { id }).SingleOrDefault();
        public NatureOfContentTerm2 FindNatureOfContentTerm2(Guid? id, bool load = false)
        {
            var noct2 = Query<NatureOfContentTerm2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}nature_of_content_terms WHERE id = @id", new { id }).SingleOrDefault();
            if (noct2 == null) return null;
            if (load && noct2.CreationUserId != null) noct2.CreationUser = FindUser2(noct2.CreationUserId);
            if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = FindUser2(noct2.LastWriteUserId);
            return noct2;
        }
        public Note FindNote(Guid? id, bool load = false)
        {
            var n = Query<Note>($"SELECT id AS \"Id\", jsonb AS \"Content\", temporary_type_id AS \"TemporaryTypeId\" FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_data WHERE id = @id", new { id }).SingleOrDefault();
            if (n == null) return null;
            if (load && n.TemporaryTypeId != null) n.TemporaryType = FindNoteType(n.TemporaryTypeId);
            return n;
        }
        public Note2 FindNote2(string id, Guid? instanceId, bool load = false)
        {
            var n2 = Query<Note2>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", instance_note_type_id AS \"InstanceNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}notes WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (n2 == null) return null;
            if (load && n2.InstanceId != null) n2.Instance = FindInstance2(n2.InstanceId);
            if (load && n2.InstanceNoteTypeId != null) n2.InstanceNoteType = FindInstanceNoteType2(n2.InstanceNoteTypeId);
            return n2;
        }
        public Note3 FindNote3(Guid? id, bool load = false)
        {
            var n3 = Query<Note3>($"SELECT id AS \"Id\", type_id AS \"TypeId\", type AS \"Type\", domain AS \"Domain\", title AS \"Title\", content2 AS \"Content2\", status AS \"Status\", creator_last_name AS \"CreatorLastName\", creator_first_name AS \"CreatorFirstName\", creator_middle_name AS \"CreatorMiddleName\", updater_last_name AS \"UpdaterLastName\", updater_first_name AS \"UpdaterFirstName\", updater_middle_name AS \"UpdaterMiddleName\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", temporary_type_id AS \"TemporaryTypeId\" FROM uc{(IsMySql ? "_" : ".")}notes2 WHERE id = @id", new { id }).SingleOrDefault();
            if (n3 == null) return null;
            if (load && n3.TypeId != null) n3.Type1 = FindNoteType2(n3.TypeId);
            if (load && n3.CreationUserId != null) n3.CreationUser = FindUser2(n3.CreationUserId);
            if (load && n3.LastWriteUserId != null) n3.LastWriteUser = FindUser2(n3.LastWriteUserId);
            if (load && n3.TemporaryTypeId != null) n3.TemporaryType = FindNoteType2(n3.TemporaryTypeId);
            return n3;
        }
        public NoteLink FindNoteLink(string id, bool load = false)
        {
            var nl = Query<NoteLink>($"SELECT id AS \"Id\", note_id AS \"NoteId\", id2 AS \"Id2\", type AS \"Type\" FROM uc{(IsMySql ? "_" : ".")}note_links WHERE id = @id", new { id }).SingleOrDefault();
            if (nl == null) return null;
            if (load && nl.NoteId != null) nl.Note = FindNote3(nl.NoteId);
            return nl;
        }
        public NoteType FindNoteType(Guid? id, bool load = false) => Query<NoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_type WHERE id = @id", new { id }).SingleOrDefault();
        public NoteType2 FindNoteType2(Guid? id, bool load = false)
        {
            var nt2 = Query<NoteType2>($"SELECT id AS \"Id\", name AS \"Name\", usage_note_total AS \"UsageNoteTotal\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}note_types WHERE id = @id", new { id }).SingleOrDefault();
            if (nt2 == null) return null;
            if (load && nt2.CreationUserId != null) nt2.CreationUser = FindUser2(nt2.CreationUserId);
            if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = FindUser2(nt2.LastWriteUserId);
            return nt2;
        }
        public Order FindOrder(Guid? id, bool load = false) => Query<Order>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order WHERE id = @id", new { id }).SingleOrDefault();
        public Order2 FindOrder2(Guid? id, bool load = false)
        {
            var o2 = Query<Order2>($"SELECT id AS \"Id\", approved AS \"Approved\", approved_by_id AS \"ApprovedById\", approval_date AS \"ApprovalDate\", assigned_to_id AS \"AssignedToId\", bill_to_id AS \"BillToId\", close_reason_reason AS \"CloseReasonReason\", close_reason_note AS \"CloseReasonNote\", date_ordered AS \"OrderDate\", manual_po AS \"Manual\", po_number AS \"Number\", po_number_prefix AS \"NumberPrefix\", po_number_suffix AS \"NumberSuffix\", order_type AS \"OrderType\", re_encumber AS \"Reencumber\", ongoing_interval AS \"OngoingInterval\", ongoing_is_subscription AS \"OngoingIsSubscription\", ongoing_manual_renewal AS \"OngoingManualRenewal\", ongoing_notes AS \"OngoingNotes\", ongoing_review_period AS \"OngoingReviewPeriod\", ongoing_renewal_date AS \"OngoingRenewalDate\", ongoing_review_date AS \"OngoingReviewDate\", ship_to_id AS \"ShipToId\", template_id AS \"TemplateId\", vendor_id AS \"VendorId\", workflow_status AS \"WorkflowStatus\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}orders WHERE id = @id", new { id }).SingleOrDefault();
            if (o2 == null) return null;
            if (load && o2.ApprovedById != null) o2.ApprovedBy = FindUser2(o2.ApprovedById);
            if (load && o2.AssignedToId != null) o2.AssignedTo = FindUser2(o2.AssignedToId);
            if (load && o2.BillToId != null) o2.BillTo = FindAddress(o2.BillToId);
            if (load && o2.ShipToId != null) o2.ShipTo = FindAddress(o2.ShipToId);
            if (load && o2.TemplateId != null) o2.Template = FindOrderTemplate2(o2.TemplateId);
            if (load && o2.VendorId != null) o2.Vendor = FindOrganization2(o2.VendorId);
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId);
            return o2;
        }
        public OrderAcquisitionsUnit FindOrderAcquisitionsUnit(string id, bool load = false)
        {
            var oau = Query<OrderAcquisitionsUnit>($"SELECT id AS \"Id\", order_id AS \"OrderId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}order_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (oau == null) return null;
            if (load && oau.OrderId != null) oau.Order = FindOrder2(oau.OrderId);
            if (load && oau.AcquisitionsUnitId != null) oau.AcquisitionsUnit = FindAcquisitionsUnit2(oau.AcquisitionsUnitId);
            return oau;
        }
        public OrderInvoice FindOrderInvoice(Guid? id, bool load = false)
        {
            var oi = Query<OrderInvoice>($"SELECT id AS \"Id\", jsonb AS \"Content\", purchaseorderid AS \"Purchaseorderid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship WHERE id = @id", new { id }).SingleOrDefault();
            if (oi == null) return null;
            if (load && oi.Purchaseorderid != null) oi.Order = FindOrder(oi.Purchaseorderid);
            return oi;
        }
        public OrderInvoice2 FindOrderInvoice2(Guid? id, bool load = false)
        {
            var oi2 = Query<OrderInvoice2>($"SELECT id AS \"Id\", order_id AS \"OrderId\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_invoices WHERE id = @id", new { id }).SingleOrDefault();
            if (oi2 == null) return null;
            if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId);
            if (load && oi2.InvoiceId != null) oi2.Invoice = FindInvoice2(oi2.InvoiceId);
            return oi2;
        }
        public OrderItem FindOrderItem(Guid? id, bool load = false)
        {
            var oi = Query<OrderItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", purchaseorderid AS \"Purchaseorderid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line WHERE id = @id", new { id }).SingleOrDefault();
            if (oi == null) return null;
            if (load && oi.Purchaseorderid != null) oi.Order = FindOrder(oi.Purchaseorderid);
            return oi;
        }
        public OrderItem2 FindOrderItem2(Guid? id, bool load = false)
        {
            var oi2 = Query<OrderItem2>($"SELECT id AS \"Id\", edition AS \"Edition\", checkin_items AS \"CheckinItems\", agreement_id AS \"AgreementId\", acquisition_method AS \"AcquisitionMethod\", cancellation_restriction AS \"CancellationRestriction\", cancellation_restriction_note AS \"CancellationRestrictionNote\", collection AS \"Collection\", cost_list_unit_price AS \"PhysicalUnitListPrice\", cost_list_unit_price_electronic AS \"ElectronicUnitListPrice\", cost_currency AS \"Currency\", cost_additional_cost AS \"AdditionalCost\", cost_discount AS \"Discount\", cost_discount_type AS \"DiscountType\", cost_quantity_physical AS \"PhysicalQuantity\", cost_quantity_electronic AS \"ElectronicQuantity\", cost_po_line_estimated_price AS \"EstimatedPrice\", description AS \"Description\", details_receiving_note AS \"ReceivingNote\", details_subscription_from AS \"SubscriptionFrom\", details_subscription_interval AS \"SubscriptionInterval\", details_subscription_to AS \"SubscriptionTo\", donor AS \"Donor\", eresource_activated AS \"EresourceActivated\", eresource_activation_due AS \"EresourceActivationDue\", eresource_create_inventory AS \"EresourceCreateInventory\", eresource_trial AS \"EresourceTrial\", eresource_expected_activation AS \"EresourceExpectedActivationDate\", eresource_user_limit AS \"EresourceUserLimit\", eresource_access_provider_id AS \"EresourceAccessProviderId\", eresource_license_code AS \"EresourceLicenseCode\", eresource_license_description AS \"EresourceLicenseDescription\", eresource_license_reference AS \"EresourceLicenseReference\", eresource_material_type_id AS \"EresourceMaterialTypeId\", eresource_resource_url AS \"EresourceResourceUrl\", instance_id AS \"InstanceId\", is_package AS \"IsPackage\", order_format AS \"OrderFormat\", package_po_line_id AS \"PackageOrderItemId\", payment_status AS \"PaymentStatus\", physical_create_inventory AS \"PhysicalCreateInventory\", physical_material_type_id AS \"PhysicalMaterialTypeId\", physical_material_supplier_id AS \"PhysicalMaterialSupplierId\", physical_expected_receipt_date AS \"PhysicalExpectedReceiptDate\", physical_receipt_due AS \"PhysicalReceiptDue\", po_line_description AS \"Description2\", po_line_number AS \"Number\", publication_year AS \"PublicationYear\", publisher AS \"Publisher\", order_id AS \"OrderId\", receipt_date AS \"ReceiptDate\", receipt_status AS \"ReceiptStatus\", requester AS \"Requester\", rush AS \"Rush\", selector AS \"Selector\", source AS \"Source\", title_or_package AS \"TitleOrPackage\", vendor_detail_instructions AS \"VendorInstructions\", vendor_detail_note_from_vendor AS \"VendorNote\", vendor_detail_ref_number AS \"VendorReferenceNumber\", vendor_detail_ref_number_type AS \"VendorReferenceNumberType\", vendor_detail_vendor_account AS \"VendorAccount\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_items WHERE id = @id", new { id }).SingleOrDefault();
            if (oi2 == null) return null;
            if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = FindOrganization2(oi2.EresourceAccessProviderId);
            if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = FindMaterialType2(oi2.EresourceMaterialTypeId);
            if (load && oi2.InstanceId != null) oi2.Instance = FindInstance2(oi2.InstanceId);
            if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = FindOrderItem2(oi2.PackageOrderItemId);
            if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = FindMaterialType2(oi2.PhysicalMaterialTypeId);
            if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = FindOrganization2(oi2.PhysicalMaterialSupplierId);
            if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId);
            if (load && oi2.CreationUserId != null) oi2.CreationUser = FindUser2(oi2.CreationUserId);
            if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = FindUser2(oi2.LastWriteUserId);
            return oi2;
        }
        public OrderItemAlert FindOrderItemAlert(string id, bool load = false)
        {
            var oia = Query<OrderItemAlert>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", alert_id AS \"AlertId\" FROM uc{(IsMySql ? "_" : ".")}order_item_alerts WHERE id = @id", new { id }).SingleOrDefault();
            if (oia == null) return null;
            if (load && oia.OrderItemId != null) oia.OrderItem = FindOrderItem2(oia.OrderItemId);
            if (load && oia.AlertId != null) oia.Alert = FindAlert2(oia.AlertId);
            return oia;
        }
        public OrderItemClaim FindOrderItemClaim(string id, bool load = false)
        {
            var oic = Query<OrderItemClaim>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", claimed AS \"Claimed\", sent AS \"Sent\", grace AS \"Grace\" FROM uc{(IsMySql ? "_" : ".")}order_item_claims WHERE id = @id", new { id }).SingleOrDefault();
            if (oic == null) return null;
            if (load && oic.OrderItemId != null) oic.OrderItem = FindOrderItem2(oic.OrderItemId);
            return oic;
        }
        public OrderItemContributor FindOrderItemContributor(string id, bool load = false)
        {
            var oic = Query<OrderItemContributor>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", contributor AS \"Contributor\", contributor_name_type_id AS \"ContributorNameTypeId\" FROM uc{(IsMySql ? "_" : ".")}order_item_contributors WHERE id = @id", new { id }).SingleOrDefault();
            if (oic == null) return null;
            if (load && oic.OrderItemId != null) oic.OrderItem = FindOrderItem2(oic.OrderItemId);
            if (load && oic.ContributorNameTypeId != null) oic.ContributorNameType = FindContributorNameType2(oic.ContributorNameTypeId);
            return oic;
        }
        public OrderItemFund FindOrderItemFund(string id, bool load = false)
        {
            var oif = Query<OrderItemFund>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", expense_class_id AS \"ExpenseClassId\", distribution_type AS \"DistributionType\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}order_item_fund_distributions WHERE id = @id", new { id }).SingleOrDefault();
            if (oif == null) return null;
            if (load && oif.OrderItemId != null) oif.OrderItem = FindOrderItem2(oif.OrderItemId);
            if (load && oif.EncumbranceId != null) oif.Encumbrance = FindTransaction2(oif.EncumbranceId);
            if (load && oif.FundId != null) oif.Fund = FindFund2(oif.FundId);
            if (load && oif.ExpenseClassId != null) oif.ExpenseClass = FindExpenseClass2(oif.ExpenseClassId);
            return oif;
        }
        public OrderItemLocation2 FindOrderItemLocation2(string id, bool load = false)
        {
            var oil2 = Query<OrderItemLocation2>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", location_id AS \"LocationId\", quantity AS \"Quantity\", quantity_electronic AS \"ElectronicQuantity\", quantity_physical AS \"PhysicalQuantity\" FROM uc{(IsMySql ? "_" : ".")}order_item_locations WHERE id = @id", new { id }).SingleOrDefault();
            if (oil2 == null) return null;
            if (load && oil2.OrderItemId != null) oil2.OrderItem = FindOrderItem2(oil2.OrderItemId);
            if (load && oil2.LocationId != null) oil2.Location = FindLocation2(oil2.LocationId);
            return oil2;
        }
        public OrderItemProductId FindOrderItemProductId(string id, bool load = false)
        {
            var oipi = Query<OrderItemProductId>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", qualifier AS \"Qualifier\" FROM uc{(IsMySql ? "_" : ".")}order_item_product_ids WHERE id = @id", new { id }).SingleOrDefault();
            if (oipi == null) return null;
            if (load && oipi.OrderItemId != null) oipi.OrderItem = FindOrderItem2(oipi.OrderItemId);
            if (load && oipi.ProductIdTypeId != null) oipi.ProductIdType = FindIdType2(oipi.ProductIdTypeId);
            return oipi;
        }
        public OrderItemReportingCode FindOrderItemReportingCode(string id, bool load = false)
        {
            var oirc = Query<OrderItemReportingCode>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", reporting_code_id AS \"ReportingCodeId\" FROM uc{(IsMySql ? "_" : ".")}order_item_reporting_codes WHERE id = @id", new { id }).SingleOrDefault();
            if (oirc == null) return null;
            if (load && oirc.OrderItemId != null) oirc.OrderItem = FindOrderItem2(oirc.OrderItemId);
            if (load && oirc.ReportingCodeId != null) oirc.ReportingCode = FindReportingCode2(oirc.ReportingCodeId);
            return oirc;
        }
        public OrderItemTag FindOrderItemTag(string id, bool load = false)
        {
            var oit = Query<OrderItemTag>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_item_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (oit == null) return null;
            if (load && oit.OrderItemId != null) oit.OrderItem = FindOrderItem2(oit.OrderItemId);
            return oit;
        }
        public OrderItemVolume FindOrderItemVolume(string id, bool load = false)
        {
            var oiv = Query<OrderItemVolume>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_item_volumes WHERE id = @id", new { id }).SingleOrDefault();
            if (oiv == null) return null;
            if (load && oiv.OrderItemId != null) oiv.OrderItem = FindOrderItem2(oiv.OrderItemId);
            return oiv;
        }
        public OrderNote FindOrderNote(string id, bool load = false)
        {
            var @on = Query<OrderNote>($"SELECT id AS \"Id\", order_id AS \"OrderId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_notes WHERE id = @id", new { id }).SingleOrDefault();
            if (@on == null) return null;
            if (load && @on.OrderId != null) @on.Order = FindOrder2(@on.OrderId);
            return @on;
        }
        public OrderTag FindOrderTag(string id, bool load = false)
        {
            var ot = Query<OrderTag>($"SELECT id AS \"Id\", order_id AS \"OrderId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (ot == null) return null;
            if (load && ot.OrderId != null) ot.Order = FindOrder2(ot.OrderId);
            return ot;
        }
        public OrderTemplate FindOrderTemplate(Guid? id, bool load = false) => Query<OrderTemplate>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates WHERE id = @id", new { id }).SingleOrDefault();
        public OrderTemplate2 FindOrderTemplate2(Guid? id, bool load = false) => Query<OrderTemplate2>($"SELECT id AS \"Id\", template_name AS \"TemplateName\", template_code AS \"TemplateCode\", template_description AS \"TemplateDescription\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_templates WHERE id = @id", new { id }).SingleOrDefault();
        public OrderTransactionSummary FindOrderTransactionSummary(Guid? id, bool load = false) => Query<OrderTransactionSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries WHERE id = @id", new { id }).SingleOrDefault();
        public OrderTransactionSummary2 FindOrderTransactionSummary2(Guid? id, bool load = false)
        {
            var ots2 = Query<OrderTransactionSummary2>($"SELECT id AS \"Id\", num_transactions AS \"NumTransactions\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_transaction_summaries WHERE id = @id", new { id }).SingleOrDefault();
            if (ots2 == null) return null;
            if (load && ots2.Id != null) ots2.Order2 = FindOrder2(ots2.Id);
            return ots2;
        }
        public Organization FindOrganization(Guid? id, bool load = false) => Query<Organization>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations WHERE id = @id", new { id }).SingleOrDefault();
        public Organization2 FindOrganization2(Guid? id, bool load = false)
        {
            var o2 = Query<Organization2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", status AS \"Status\", language AS \"Language\", erp_code AS \"ErpCode\", payment_method AS \"PaymentMethod\", access_provider AS \"AccessProvider\", governmental AS \"Governmental\", licensor AS \"Licensor\", material_supplier AS \"MaterialSupplier\", claiming_interval AS \"ClaimingInterval\", discount_percent AS \"DiscountPercent\", expected_activation_interval AS \"ExpectedActivationInterval\", expected_invoice_interval AS \"ExpectedInvoiceInterval\", renewal_activation_interval AS \"RenewalActivationInterval\", subscription_interval AS \"SubscriptionInterval\", expected_receipt_interval AS \"ExpectedReceiptInterval\", tax_id AS \"TaxId\", liable_for_vat AS \"LiableForVat\", tax_percentage AS \"TaxPercentage\", edi_vendor_edi_code AS \"EdiVendorEdiCode\", edi_vendor_edi_type AS \"EdiVendorEdiType\", edi_lib_edi_code AS \"EdiLibEdiCode\", edi_lib_edi_type AS \"EdiLibEdiType\", edi_prorate_tax AS \"EdiProrateTax\", edi_prorate_fees AS \"EdiProrateFees\", edi_naming_convention AS \"EdiNamingConvention\", edi_send_acct_num AS \"EdiSendAcctNum\", edi_support_order AS \"EdiSupportOrder\", edi_support_invoice AS \"EdiSupportInvoice\", edi_notes AS \"EdiNotes\", edi_ftp_ftp_format AS \"EdiFtpFtpFormat\", edi_ftp_server_address AS \"EdiFtpServerAddress\", edi_ftp_username AS \"EdiFtpUsername\", edi_ftp_password AS \"EdiFtpPassword\", edi_ftp_ftp_mode AS \"EdiFtpFtpMode\", edi_ftp_ftp_conn_mode AS \"EdiFtpFtpConnMode\", edi_ftp_ftp_port AS \"EdiFtpFtpPort\", edi_ftp_order_directory AS \"EdiFtpOrderDirectory\", edi_ftp_invoice_directory AS \"EdiFtpInvoiceDirectory\", edi_ftp_notes AS \"EdiFtpNotes\", edi_job_schedule_edi AS \"EdiJobScheduleEdi\", edi_job_scheduling_date AS \"EdiJobSchedulingDate\", edi_job_time AS \"EdiJobTime\", edi_job_is_monday AS \"EdiJobIsMonday\", edi_job_is_tuesday AS \"EdiJobIsTuesday\", edi_job_is_wednesday AS \"EdiJobIsWednesday\", edi_job_is_thursday AS \"EdiJobIsThursday\", edi_job_is_friday AS \"EdiJobIsFriday\", edi_job_is_saturday AS \"EdiJobIsSaturday\", edi_job_is_sunday AS \"EdiJobIsSunday\", edi_job_send_to_emails AS \"EdiJobSendToEmails\", edi_job_notify_all_edi AS \"EdiJobNotifyAllEdi\", edi_job_notify_invoice_only AS \"EdiJobNotifyInvoiceOnly\", edi_job_notify_error_only AS \"EdiJobNotifyErrorOnly\", edi_job_scheduling_notes AS \"EdiJobSchedulingNotes\", is_vendor AS \"IsVendor\", san_code AS \"SanCode\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}organizations WHERE id = @id", new { id }).SingleOrDefault();
            if (o2 == null) return null;
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId);
            return o2;
        }
        public OrganizationAccount FindOrganizationAccount(string id, bool load = false)
        {
            var oa = Query<OrganizationAccount>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", name AS \"Name\", account_no AS \"AccountNo\", description AS \"Description\", app_system_no AS \"AppSystemNo\", payment_method AS \"PaymentMethod\", account_status AS \"AccountStatus\", contact_info AS \"ContactInfo\", library_code AS \"LibraryCode\", library_edi_code AS \"LibraryEdiCode\", notes AS \"Notes\" FROM uc{(IsMySql ? "_" : ".")}organization_accounts WHERE id = @id", new { id }).SingleOrDefault();
            if (oa == null) return null;
            if (load && oa.OrganizationId != null) oa.Organization = FindOrganization2(oa.OrganizationId);
            return oa;
        }
        public OrganizationAccountAcquisitionsUnit FindOrganizationAccountAcquisitionsUnit(string id, bool load = false)
        {
            var oaau = Query<OrganizationAccountAcquisitionsUnit>($"SELECT id AS \"Id\", organization_account_id AS \"OrganizationAccountId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}organization_account_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (oaau == null) return null;
            if (load && oaau.OrganizationAccountId != null) oaau.OrganizationAccount = FindOrganizationAccount(oaau.OrganizationAccountId);
            if (load && oaau.AcquisitionsUnitId != null) oaau.AcquisitionsUnit = FindAcquisitionsUnit2(oaau.AcquisitionsUnitId);
            return oaau;
        }
        public OrganizationAcquisitionsUnit FindOrganizationAcquisitionsUnit(string id, bool load = false)
        {
            var oau = Query<OrganizationAcquisitionsUnit>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}organization_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (oau == null) return null;
            if (load && oau.OrganizationId != null) oau.Organization = FindOrganization2(oau.OrganizationId);
            if (load && oau.AcquisitionsUnitId != null) oau.AcquisitionsUnit = FindAcquisitionsUnit2(oau.AcquisitionsUnitId);
            return oau;
        }
        public OrganizationAddress FindOrganizationAddress(string id, bool load = false)
        {
            var oa = Query<OrganizationAddress>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", state_region AS \"StateRegion\", zip_code AS \"ZipCode\", country AS \"Country\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_addresses WHERE id = @id", new { id }).SingleOrDefault();
            if (oa == null) return null;
            if (load && oa.OrganizationId != null) oa.Organization = FindOrganization2(oa.OrganizationId);
            if (load && oa.CreationUserId != null) oa.CreationUser = FindUser2(oa.CreationUserId);
            if (load && oa.LastWriteUserId != null) oa.LastWriteUser = FindUser2(oa.LastWriteUserId);
            return oa;
        }
        public OrganizationAddressCategory FindOrganizationAddressCategory(string id, bool load = false)
        {
            var oac = Query<OrganizationAddressCategory>($"SELECT id AS \"Id\", organization_address_id AS \"OrganizationAddressId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_address_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (oac == null) return null;
            if (load && oac.OrganizationAddressId != null) oac.OrganizationAddress = FindOrganizationAddress(oac.OrganizationAddressId);
            if (load && oac.CategoryId != null) oac.Category = FindCategory2(oac.CategoryId);
            return oac;
        }
        public OrganizationAgreement FindOrganizationAgreement(string id, bool load = false)
        {
            var oa = Query<OrganizationAgreement>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", name AS \"Name\", discount AS \"Discount\", reference_url AS \"ReferenceUrl\", notes AS \"Notes\" FROM uc{(IsMySql ? "_" : ".")}organization_agreements WHERE id = @id", new { id }).SingleOrDefault();
            if (oa == null) return null;
            if (load && oa.OrganizationId != null) oa.Organization = FindOrganization2(oa.OrganizationId);
            return oa;
        }
        public OrganizationAlias FindOrganizationAlias(string id, bool load = false)
        {
            var oa = Query<OrganizationAlias>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", value AS \"Value\", description AS \"Description\" FROM uc{(IsMySql ? "_" : ".")}organization_aliases WHERE id = @id", new { id }).SingleOrDefault();
            if (oa == null) return null;
            if (load && oa.OrganizationId != null) oa.Organization = FindOrganization2(oa.OrganizationId);
            return oa;
        }
        public OrganizationChangelog FindOrganizationChangelog(string id, bool load = false)
        {
            var oc = Query<OrganizationChangelog>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", description AS \"Description\", timestamp AS \"Timestamp\" FROM uc{(IsMySql ? "_" : ".")}organization_changelogs WHERE id = @id", new { id }).SingleOrDefault();
            if (oc == null) return null;
            if (load && oc.OrganizationId != null) oc.Organization = FindOrganization2(oc.OrganizationId);
            return oc;
        }
        public OrganizationContact FindOrganizationContact(string id, bool load = false)
        {
            var oc = Query<OrganizationContact>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", contact_id AS \"ContactId\" FROM uc{(IsMySql ? "_" : ".")}organization_contacts WHERE id = @id", new { id }).SingleOrDefault();
            if (oc == null) return null;
            if (load && oc.OrganizationId != null) oc.Organization = FindOrganization2(oc.OrganizationId);
            if (load && oc.ContactId != null) oc.Contact = FindContact2(oc.ContactId);
            return oc;
        }
        public OrganizationEmail FindOrganizationEmail(string id, bool load = false)
        {
            var oe = Query<OrganizationEmail>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_emails WHERE id = @id", new { id }).SingleOrDefault();
            if (oe == null) return null;
            if (load && oe.OrganizationId != null) oe.Organization = FindOrganization2(oe.OrganizationId);
            if (load && oe.CreationUserId != null) oe.CreationUser = FindUser2(oe.CreationUserId);
            if (load && oe.LastWriteUserId != null) oe.LastWriteUser = FindUser2(oe.LastWriteUserId);
            return oe;
        }
        public OrganizationEmailCategory FindOrganizationEmailCategory(string id, bool load = false)
        {
            var oec = Query<OrganizationEmailCategory>($"SELECT id AS \"Id\", organization_email_id AS \"OrganizationEmailId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_email_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (oec == null) return null;
            if (load && oec.OrganizationEmailId != null) oec.OrganizationEmail = FindOrganizationEmail(oec.OrganizationEmailId);
            if (load && oec.CategoryId != null) oec.Category = FindCategory2(oec.CategoryId);
            return oec;
        }
        public OrganizationInterface FindOrganizationInterface(string id, bool load = false)
        {
            var oi = Query<OrganizationInterface>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", interface_id AS \"InterfaceId\" FROM uc{(IsMySql ? "_" : ".")}organization_interfaces WHERE id = @id", new { id }).SingleOrDefault();
            if (oi == null) return null;
            if (load && oi.OrganizationId != null) oi.Organization = FindOrganization2(oi.OrganizationId);
            if (load && oi.InterfaceId != null) oi.Interface = FindInterface2(oi.InterfaceId);
            return oi;
        }
        public OrganizationPhoneNumber FindOrganizationPhoneNumber(string id, bool load = false)
        {
            var opn = Query<OrganizationPhoneNumber>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", phone_number AS \"PhoneNumber\", type AS \"Type\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_phone_numbers WHERE id = @id", new { id }).SingleOrDefault();
            if (opn == null) return null;
            if (load && opn.OrganizationId != null) opn.Organization = FindOrganization2(opn.OrganizationId);
            if (load && opn.CreationUserId != null) opn.CreationUser = FindUser2(opn.CreationUserId);
            if (load && opn.LastWriteUserId != null) opn.LastWriteUser = FindUser2(opn.LastWriteUserId);
            return opn;
        }
        public OrganizationPhoneNumberCategory FindOrganizationPhoneNumberCategory(string id, bool load = false)
        {
            var opnc = Query<OrganizationPhoneNumberCategory>($"SELECT id AS \"Id\", organization_phone_number_id AS \"OrganizationPhoneNumberId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_phone_number_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (opnc == null) return null;
            if (load && opnc.OrganizationPhoneNumberId != null) opnc.OrganizationPhoneNumber = FindOrganizationPhoneNumber(opnc.OrganizationPhoneNumberId);
            if (load && opnc.CategoryId != null) opnc.Category = FindCategory2(opnc.CategoryId);
            return opnc;
        }
        public OrganizationTag FindOrganizationTag(string id, bool load = false)
        {
            var ot = Query<OrganizationTag>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}organization_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (ot == null) return null;
            if (load && ot.OrganizationId != null) ot.Organization = FindOrganization2(ot.OrganizationId);
            return ot;
        }
        public OrganizationUrl FindOrganizationUrl(string id, bool load = false)
        {
            var ou = Query<OrganizationUrl>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", language AS \"Language\", is_primary AS \"IsPrimary\", notes AS \"Notes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_urls WHERE id = @id", new { id }).SingleOrDefault();
            if (ou == null) return null;
            if (load && ou.OrganizationId != null) ou.Organization = FindOrganization2(ou.OrganizationId);
            if (load && ou.CreationUserId != null) ou.CreationUser = FindUser2(ou.CreationUserId);
            if (load && ou.LastWriteUserId != null) ou.LastWriteUser = FindUser2(ou.LastWriteUserId);
            return ou;
        }
        public OrganizationUrlCategory FindOrganizationUrlCategory(string id, bool load = false)
        {
            var ouc = Query<OrganizationUrlCategory>($"SELECT id AS \"Id\", organization_url_id AS \"OrganizationUrlId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_url_categories WHERE id = @id", new { id }).SingleOrDefault();
            if (ouc == null) return null;
            if (load && ouc.OrganizationUrlId != null) ouc.OrganizationUrl = FindOrganizationUrl(ouc.OrganizationUrlId);
            if (load && ouc.CategoryId != null) ouc.Category = FindCategory2(ouc.CategoryId);
            return ouc;
        }
        public OverdueFinePolicy FindOverdueFinePolicy(Guid? id, bool load = false) => Query<OverdueFinePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy WHERE id = @id", new { id }).SingleOrDefault();
        public OverdueFinePolicy2 FindOverdueFinePolicy2(Guid? id, bool load = false)
        {
            var ofp2 = Query<OverdueFinePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", overdue_fine_quantity AS \"OverdueFineQuantity\", overdue_fine_interval_id AS \"OverdueFineInterval\", count_closed AS \"CountClosed\", max_overdue_fine AS \"MaxOverdueFine\", forgive_overdue_fine AS \"ForgiveOverdueFine\", overdue_recall_fine_quantity AS \"OverdueRecallFineQuantity\", overdue_recall_fine_interval_id AS \"OverdueRecallFineInterval\", grace_period_recall AS \"GracePeriodRecall\", max_overdue_recall_fine AS \"MaxOverdueRecallFine\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}overdue_fine_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (ofp2 == null) return null;
            if (load && ofp2.CreationUserId != null) ofp2.CreationUser = FindUser2(ofp2.CreationUserId);
            if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = FindUser2(ofp2.LastWriteUserId);
            return ofp2;
        }
        public Owner FindOwner(Guid? id, bool load = false) => Query<Owner>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}owners WHERE id = @id", new { id }).SingleOrDefault();
        public Owner2 FindOwner2(Guid? id, bool load = false)
        {
            var o2 = Query<Owner2>($"SELECT id AS \"Id\", owner AS \"Name\", \"desc\" AS \"Desc\", default_charge_notice_id AS \"DefaultChargeNoticeId\", default_action_notice_id AS \"DefaultActionNoticeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}owners WHERE id = @id", new { id }).SingleOrDefault();
            if (o2 == null) return null;
            if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = FindTemplate2(o2.DefaultChargeNoticeId);
            if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = FindTemplate2(o2.DefaultActionNoticeId);
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId);
            return o2;
        }
        public PatronActionSession FindPatronActionSession(Guid? id, bool load = false) => Query<PatronActionSession>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session WHERE id = @id", new { id }).SingleOrDefault();
        public PatronActionSession2 FindPatronActionSession2(Guid? id, bool load = false)
        {
            var pas2 = Query<PatronActionSession2>($"SELECT id AS \"Id\", patron_id AS \"PatronId\", loan_id AS \"LoanId\", action_type AS \"ActionType\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}patron_action_sessions WHERE id = @id", new { id }).SingleOrDefault();
            if (pas2 == null) return null;
            if (load && pas2.PatronId != null) pas2.Patron = FindUser2(pas2.PatronId);
            if (load && pas2.LoanId != null) pas2.Loan = FindLoan2(pas2.LoanId);
            if (load && pas2.CreationUserId != null) pas2.CreationUser = FindUser2(pas2.CreationUserId);
            if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = FindUser2(pas2.LastWriteUserId);
            return pas2;
        }
        public PatronNoticePolicy FindPatronNoticePolicy(Guid? id, bool load = false) => Query<PatronNoticePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy WHERE id = @id", new { id }).SingleOrDefault();
        public PatronNoticePolicy2 FindPatronNoticePolicy2(Guid? id, bool load = false)
        {
            var pnp2 = Query<PatronNoticePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", active AS \"Active\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (pnp2 == null) return null;
            if (load && pnp2.CreationUserId != null) pnp2.CreationUser = FindUser2(pnp2.CreationUserId);
            if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = FindUser2(pnp2.LastWriteUserId);
            return pnp2;
        }
        public PatronNoticePolicyFeeFineNotice FindPatronNoticePolicyFeeFineNotice(string id, bool load = false)
        {
            var pnpffn = Query<PatronNoticePolicyFeeFineNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_fee_fine_notices WHERE id = @id", new { id }).SingleOrDefault();
            if (pnpffn == null) return null;
            if (load && pnpffn.PatronNoticePolicyId != null) pnpffn.PatronNoticePolicy = FindPatronNoticePolicy2(pnpffn.PatronNoticePolicyId);
            if (load && pnpffn.TemplateId != null) pnpffn.Template = FindTemplate2(pnpffn.TemplateId);
            return pnpffn;
        }
        public PatronNoticePolicyLoanNotice FindPatronNoticePolicyLoanNotice(string id, bool load = false)
        {
            var pnpln = Query<PatronNoticePolicyLoanNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_loan_notices WHERE id = @id", new { id }).SingleOrDefault();
            if (pnpln == null) return null;
            if (load && pnpln.PatronNoticePolicyId != null) pnpln.PatronNoticePolicy = FindPatronNoticePolicy2(pnpln.PatronNoticePolicyId);
            if (load && pnpln.TemplateId != null) pnpln.Template = FindTemplate2(pnpln.TemplateId);
            return pnpln;
        }
        public PatronNoticePolicyRequestNotice FindPatronNoticePolicyRequestNotice(string id, bool load = false)
        {
            var pnprn = Query<PatronNoticePolicyRequestNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_request_notices WHERE id = @id", new { id }).SingleOrDefault();
            if (pnprn == null) return null;
            if (load && pnprn.PatronNoticePolicyId != null) pnprn.PatronNoticePolicy = FindPatronNoticePolicy2(pnprn.PatronNoticePolicyId);
            if (load && pnprn.TemplateId != null) pnprn.Template = FindTemplate2(pnprn.TemplateId);
            return pnprn;
        }
        public Payment FindPayment(Guid? id, bool load = false) => Query<Payment>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions WHERE id = @id", new { id }).SingleOrDefault();
        public Payment2 FindPayment2(Guid? id, bool load = false)
        {
            var p2 = Query<Payment2>($"SELECT id AS \"Id\", date_action AS \"CreationTime\", type_action AS \"TypeAction\", comments AS \"Comments\", notify AS \"Notify\", amount_action AS \"Amount\", balance AS \"RemainingAmount\", transaction_information AS \"TransactionInformation\", created_at AS \"CreatedAt\", source AS \"Source\", payment_method AS \"PaymentMethod\", fee_id AS \"FeeId\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}payments WHERE id = @id", new { id }).SingleOrDefault();
            if (p2 == null) return null;
            if (load && p2.FeeId != null) p2.Fee = FindFee2(p2.FeeId);
            if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId);
            return p2;
        }
        public PaymentMethod FindPaymentMethod(Guid? id, bool load = false) => Query<PaymentMethod>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}payments WHERE id = @id", new { id }).SingleOrDefault();
        public PaymentMethod2 FindPaymentMethod2(Guid? id, bool load = false)
        {
            var pm2 = Query<PaymentMethod2>($"SELECT id AS \"Id\", name AS \"Name\", allowed_refund_method AS \"AllowedRefundMethod\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", owner_id AS \"OwnerId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}payment_methods WHERE id = @id", new { id }).SingleOrDefault();
            if (pm2 == null) return null;
            if (load && pm2.CreationUserId != null) pm2.CreationUser = FindUser2(pm2.CreationUserId);
            if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = FindUser2(pm2.LastWriteUserId);
            if (load && pm2.OwnerId != null) pm2.Owner = FindOwner2(pm2.OwnerId);
            return pm2;
        }
        public Permission FindPermission(Guid? id, bool load = false) => Query<Permission>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions WHERE id = @id", new { id }).SingleOrDefault();
        public Permission2 FindPermission2(Guid? id, bool load = false)
        {
            var p2 = Query<Permission2>($"SELECT id AS \"Id\", permission_name AS \"Code\", display_name AS \"Name\", description AS \"Description\", mutable AS \"Editable\", visible AS \"Visible\", dummy AS \"Dummy\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions WHERE id = @id", new { id }).SingleOrDefault();
            if (p2 == null) return null;
            if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId);
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId);
            return p2;
        }
        public PermissionChildOf FindPermissionChildOf(string id, bool load = false)
        {
            var pco = Query<PermissionChildOf>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_child_of WHERE id = @id", new { id }).SingleOrDefault();
            if (pco == null) return null;
            if (load && pco.PermissionId != null) pco.Permission = FindPermission2(pco.PermissionId);
            return pco;
        }
        public PermissionGrantedTo FindPermissionGrantedTo(string id, bool load = false)
        {
            var pgt = Query<PermissionGrantedTo>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", permissions_user_id AS \"PermissionsUserId\" FROM uc{(IsMySql ? "_" : ".")}permission_granted_to WHERE id = @id", new { id }).SingleOrDefault();
            if (pgt == null) return null;
            if (load && pgt.PermissionId != null) pgt.Permission = FindPermission2(pgt.PermissionId);
            if (load && pgt.PermissionsUserId != null) pgt.PermissionsUser = FindPermissionsUser2(pgt.PermissionsUserId);
            return pgt;
        }
        public PermissionSubPermission FindPermissionSubPermission(string id, bool load = false)
        {
            var psp = Query<PermissionSubPermission>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_sub_permissions WHERE id = @id", new { id }).SingleOrDefault();
            if (psp == null) return null;
            if (load && psp.PermissionId != null) psp.Permission = FindPermission2(psp.PermissionId);
            return psp;
        }
        public PermissionsUser FindPermissionsUser(Guid? id, bool load = false) => Query<PermissionsUser>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users WHERE id = @id", new { id }).SingleOrDefault();
        public PermissionsUser2 FindPermissionsUser2(Guid? id, bool load = false)
        {
            var pu2 = Query<PermissionsUser2>($"SELECT id AS \"Id\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions_users WHERE id = @id", new { id }).SingleOrDefault();
            if (pu2 == null) return null;
            if (load && pu2.UserId != null) pu2.User = FindUser2(pu2.UserId);
            if (load && pu2.CreationUserId != null) pu2.CreationUser = FindUser2(pu2.CreationUserId);
            if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = FindUser2(pu2.LastWriteUserId);
            return pu2;
        }
        public PermissionsUserPermission FindPermissionsUserPermission(string id, bool load = false)
        {
            var pup = Query<PermissionsUserPermission>($"SELECT id AS \"Id\", permissions_user_id AS \"PermissionsUserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions_user_permissions WHERE id = @id", new { id }).SingleOrDefault();
            if (pup == null) return null;
            if (load && pup.PermissionsUserId != null) pup.PermissionsUser = FindPermissionsUser2(pup.PermissionsUserId);
            return pup;
        }
        public PermissionTag FindPermissionTag(string id, bool load = false)
        {
            var pt = Query<PermissionTag>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (pt == null) return null;
            if (load && pt.PermissionId != null) pt.Permission = FindPermission2(pt.PermissionId);
            return pt;
        }
        public PhysicalDescription FindPhysicalDescription(string id, Guid? instanceId, bool load = false)
        {
            var pd = Query<PhysicalDescription>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}physical_descriptions WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (pd == null) return null;
            if (load && pd.InstanceId != null) pd.Instance = FindInstance2(pd.InstanceId);
            return pd;
        }
        public PrecedingSucceedingTitle FindPrecedingSucceedingTitle(Guid? id, bool load = false)
        {
            var pst = Query<PrecedingSucceedingTitle>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", precedinginstanceid AS \"Precedinginstanceid\", succeedinginstanceid AS \"Succeedinginstanceid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title WHERE id = @id", new { id }).SingleOrDefault();
            if (pst == null) return null;
            if (load && pst.Precedinginstanceid != null) pst.Instance = FindInstance(pst.Precedinginstanceid);
            if (load && pst.Succeedinginstanceid != null) pst.Instance1 = FindInstance(pst.Succeedinginstanceid);
            return pst;
        }
        public PrecedingSucceedingTitle2 FindPrecedingSucceedingTitle2(Guid? id, bool load = false)
        {
            var pst2 = Query<PrecedingSucceedingTitle2>($"SELECT id AS \"Id\", preceding_instance_id AS \"PrecedingInstanceId\", succeeding_instance_id AS \"SucceedingInstanceId\", title AS \"Title\", hrid AS \"Hrid\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_titles WHERE id = @id", new { id }).SingleOrDefault();
            if (pst2 == null) return null;
            if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = FindInstance2(pst2.PrecedingInstanceId);
            if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = FindInstance2(pst2.SucceedingInstanceId);
            if (load && pst2.CreationUserId != null) pst2.CreationUser = FindUser2(pst2.CreationUserId);
            if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = FindUser2(pst2.LastWriteUserId);
            return pst2;
        }
        public PrecedingSucceedingTitleIdentifier FindPrecedingSucceedingTitleIdentifier(string id, bool load = false)
        {
            var psti = Query<PrecedingSucceedingTitleIdentifier>($"SELECT id AS \"Id\", preceding_succeeding_title_id AS \"PrecedingSucceedingTitleId\", value AS \"Value\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_title_identifiers WHERE id = @id", new { id }).SingleOrDefault();
            if (psti == null) return null;
            if (load && psti.PrecedingSucceedingTitleId != null) psti.PrecedingSucceedingTitle = FindPrecedingSucceedingTitle2(psti.PrecedingSucceedingTitleId);
            if (load && psti.IdentifierTypeId != null) psti.IdentifierType = FindIdType2(psti.IdentifierTypeId);
            return psti;
        }
        public Prefix FindPrefix(Guid? id, bool load = false) => Query<Prefix>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes WHERE id = @id", new { id }).SingleOrDefault();
        public Prefix2 FindPrefix2(Guid? id, bool load = false) => Query<Prefix2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}prefixes WHERE id = @id", new { id }).SingleOrDefault();
        public Printer FindPrinter(Guid? id, bool load = false)
        {
            var p = Query<Printer>($"SELECT id AS \"Id\", computer_name AS \"ComputerName\", name AS \"Name\", \"left\" AS \"Left\", top AS \"Top\", width AS \"Width\", height AS \"Height\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}printers WHERE id = @id", new { id }).SingleOrDefault();
            if (p == null) return null;
            if (load && p.CreationUserId != null) p.CreationUser = FindUser2(p.CreationUserId);
            if (load && p.LastWriteUserId != null) p.LastWriteUser = FindUser2(p.LastWriteUserId);
            return p;
        }
        public Proxy FindProxy(Guid? id, bool load = false) => Query<Proxy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}proxyfor WHERE id = @id", new { id }).SingleOrDefault();
        public Proxy2 FindProxy2(Guid? id, bool load = false)
        {
            var p2 = Query<Proxy2>($"SELECT id AS \"Id\", user_id AS \"UserId\", proxy_user_id AS \"ProxyUserId\", request_for_sponsor AS \"RequestForSponsor\", notifications_to AS \"NotificationsTo\", accrue_to AS \"AccrueTo\", status AS \"Status\", expiration_date AS \"ExpirationDate\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}proxies WHERE id = @id", new { id }).SingleOrDefault();
            if (p2 == null) return null;
            if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId);
            if (load && p2.ProxyUserId != null) p2.ProxyUser = FindUser2(p2.ProxyUserId);
            if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId);
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId);
            return p2;
        }
        public Publication FindPublication(string id, Guid? instanceId, bool load = false)
        {
            var p = Query<Publication>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", publisher AS \"Publisher\", place AS \"Place\", date_of_publication AS \"PublicationYear\", role AS \"Role\" FROM uc{(IsMySql ? "_" : ".")}publications WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (p == null) return null;
            if (load && p.InstanceId != null) p.Instance = FindInstance2(p.InstanceId);
            return p;
        }
        public PublicationFrequency FindPublicationFrequency(string id, Guid? instanceId, bool load = false)
        {
            var pf = Query<PublicationFrequency>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}publication_frequency WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (pf == null) return null;
            if (load && pf.InstanceId != null) pf.Instance = FindInstance2(pf.InstanceId);
            return pf;
        }
        public PublicationRange FindPublicationRange(string id, Guid? instanceId, bool load = false)
        {
            var pr = Query<PublicationRange>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}publication_range WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (pr == null) return null;
            if (load && pr.InstanceId != null) pr.Instance = FindInstance2(pr.InstanceId);
            return pr;
        }
        public RawRecord FindRawRecord(Guid? id, bool load = false)
        {
            var rr = Query<RawRecord>($"SELECT id AS \"Id\", content AS \"Content\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb WHERE id = @id", new { id }).SingleOrDefault();
            if (rr == null) return null;
            if (load && rr.Id != null) rr.Record = FindRecord(rr.Id);
            return rr;
        }
        public RawRecord2 FindRawRecord2(Guid? id, bool load = false)
        {
            var rr2 = Query<RawRecord2>($"SELECT id AS \"Id\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}raw_records WHERE id = @id", new { id }).SingleOrDefault();
            if (rr2 == null) return null;
            if (load && rr2.Id != null) rr2.Record2 = FindRecord2(rr2.Id);
            return rr2;
        }
        public Receiving FindReceiving(Guid? id, bool load = false)
        {
            var r = Query<Receiving>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", polineid AS \"Polineid\", titleid AS \"Titleid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces WHERE id = @id", new { id }).SingleOrDefault();
            if (r == null) return null;
            if (load && r.Polineid != null) r.OrderItem = FindOrderItem(r.Polineid);
            if (load && r.Titleid != null) r.Title = FindTitle(r.Titleid);
            return r;
        }
        public Receiving2 FindReceiving2(Guid? id, bool load = false)
        {
            var r2 = Query<Receiving2>($"SELECT id AS \"Id\", caption AS \"Caption\", comment AS \"Comment\", format AS \"Format\", item_id AS \"ItemId\", location_id AS \"LocationId\", po_line_id AS \"OrderItemId\", title_id AS \"TitleId\", receiving_status AS \"ReceivingStatus\", supplement AS \"Supplement\", receipt_date AS \"ReceiptTime\", received_date AS \"ReceiveTime\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}receivings WHERE id = @id", new { id }).SingleOrDefault();
            if (r2 == null) return null;
            if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId);
            if (load && r2.LocationId != null) r2.Location = FindLocation2(r2.LocationId);
            if (load && r2.OrderItemId != null) r2.OrderItem = FindOrderItem2(r2.OrderItemId);
            if (load && r2.TitleId != null) r2.Title = FindTitle2(r2.TitleId);
            return r2;
        }
        public Record FindRecord(Guid? id, bool load = false)
        {
            var r = Query<Record>($"SELECT id AS \"Id\", snapshot_id AS \"SnapshotId\", matched_id AS \"MatchedId\", generation AS \"Generation\", record_type AS \"RecordType\", instance_id AS \"InstanceId\", state AS \"State\", leader_record_status AS \"LeaderRecordStatus\", \"order\" AS \"Order\", suppress_discovery AS \"SuppressDiscovery\", created_by_user_id AS \"CreationUserId\", created_date AS \"CreationTime\", updated_by_user_id AS \"LastWriteUserId\", updated_date AS \"LastWriteTime\", instance_hrid AS \"InstanceHrid\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb WHERE id = @id", new { id }).SingleOrDefault();
            if (r == null) return null;
            if (load && r.SnapshotId != null) r.Snapshot = FindSnapshot(r.SnapshotId);
            return r;
        }
        public Record2 FindRecord2(Guid? id, bool load = false)
        {
            var r2 = Query<Record2>($"SELECT id AS \"Id\", snapshot_id AS \"SnapshotId\", matched_id AS \"MatchedId\", generation AS \"Generation\", record_type AS \"RecordType\", instance_id AS \"InstanceId\", state AS \"State\", leader_record_status AS \"LeaderRecordStatus\", \"order\" AS \"Order\", suppress_discovery AS \"SuppressDiscovery\", creation_user_id AS \"CreationUserId\", creation_time AS \"CreationTime\", last_write_user_id AS \"LastWriteUserId\", last_write_time AS \"LastWriteTime\", instance_hrid AS \"InstanceHrid\" FROM uc{(IsMySql ? "_" : ".")}records WHERE id = @id", new { id }).SingleOrDefault();
            if (r2 == null) return null;
            if (load && r2.SnapshotId != null) r2.Snapshot = FindSnapshot2(r2.SnapshotId);
            if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId);
            return r2;
        }
        public RefundReason FindRefundReason(Guid? id, bool load = false) => Query<RefundReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds WHERE id = @id", new { id }).SingleOrDefault();
        public RefundReason2 FindRefundReason2(Guid? id, bool load = false)
        {
            var rr2 = Query<RefundReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", account_id AS \"AccountId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}refund_reasons WHERE id = @id", new { id }).SingleOrDefault();
            if (rr2 == null) return null;
            if (load && rr2.CreationUserId != null) rr2.CreationUser = FindUser2(rr2.CreationUserId);
            if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = FindUser2(rr2.LastWriteUserId);
            if (load && rr2.AccountId != null) rr2.Account = FindFee2(rr2.AccountId);
            return rr2;
        }
        public Relationship FindRelationship(Guid? id, bool load = false)
        {
            var r = Query<Relationship>($"SELECT id AS \"Id\", super_instance_id AS \"SuperInstanceId\", sub_instance_id AS \"SubInstanceId\", instance_relationship_type_id AS \"InstanceRelationshipTypeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}relationships WHERE id = @id", new { id }).SingleOrDefault();
            if (r == null) return null;
            if (load && r.SuperInstanceId != null) r.SuperInstance = FindInstance2(r.SuperInstanceId);
            if (load && r.SubInstanceId != null) r.SubInstance = FindInstance2(r.SubInstanceId);
            if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = FindRelationshipType(r.InstanceRelationshipTypeId);
            if (load && r.CreationUserId != null) r.CreationUser = FindUser2(r.CreationUserId);
            if (load && r.LastWriteUserId != null) r.LastWriteUser = FindUser2(r.LastWriteUserId);
            return r;
        }
        public RelationshipType FindRelationshipType(Guid? id, bool load = false)
        {
            var rt = Query<RelationshipType>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}relationship_types WHERE id = @id", new { id }).SingleOrDefault();
            if (rt == null) return null;
            if (load && rt.CreationUserId != null) rt.CreationUser = FindUser2(rt.CreationUserId);
            if (load && rt.LastWriteUserId != null) rt.LastWriteUser = FindUser2(rt.LastWriteUserId);
            return rt;
        }
        public ReportingCode FindReportingCode(Guid? id, bool load = false) => Query<ReportingCode>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code WHERE id = @id", new { id }).SingleOrDefault();
        public ReportingCode2 FindReportingCode2(Guid? id, bool load = false) => Query<ReportingCode2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}reporting_codes WHERE id = @id", new { id }).SingleOrDefault();
        public Request FindRequest(Guid? id, bool load = false)
        {
            var r = Query<Request>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", cancellationreasonid AS \"Cancellationreasonid\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request WHERE id = @id", new { id }).SingleOrDefault();
            if (r == null) return null;
            if (load && r.Cancellationreasonid != null) r.CancellationReason = FindCancellationReason(r.Cancellationreasonid);
            return r;
        }
        public Request2 FindRequest2(Guid? id, bool load = false)
        {
            var r2 = Query<Request2>($"SELECT id AS \"Id\", request_type AS \"RequestType\", request_date AS \"RequestDate\", requester_id AS \"RequesterId\", proxy_user_id AS \"ProxyUserId\", item_id AS \"ItemId\", status AS \"Status\", cancellation_reason_id AS \"CancellationReasonId\", cancelled_by_user_id AS \"CancelledByUserId\", cancellation_additional_information AS \"CancellationAdditionalInformation\", cancelled_date AS \"CancelledDate\", position AS \"Position\", item_title AS \"ItemTitle\", item_barcode AS \"ItemBarcode\", requester_first_name AS \"RequesterFirstName\", requester_last_name AS \"RequesterLastName\", requester_middle_name AS \"RequesterMiddleName\", requester_barcode AS \"RequesterBarcode\", requester_patron_group AS \"RequesterPatronGroup\", proxy_first_name AS \"ProxyFirstName\", proxy_last_name AS \"ProxyLastName\", proxy_middle_name AS \"ProxyMiddleName\", proxy_barcode AS \"ProxyBarcode\", proxy_patron_group AS \"ProxyPatronGroup\", fulfilment_preference AS \"FulfilmentPreference\", delivery_address_type_id AS \"DeliveryAddressTypeId\", request_expiration_date AS \"RequestExpirationDate\", hold_shelf_expiration_date AS \"HoldShelfExpirationDate\", pickup_service_point_id AS \"PickupServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", awaiting_pickup_request_closed_date AS \"AwaitingPickupRequestClosedDate\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}requests WHERE id = @id", new { id }).SingleOrDefault();
            if (r2 == null) return null;
            if (load && r2.RequesterId != null) r2.Requester = FindUser2(r2.RequesterId);
            if (load && r2.ProxyUserId != null) r2.ProxyUser = FindUser2(r2.ProxyUserId);
            if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId);
            if (load && r2.CancellationReasonId != null) r2.CancellationReason = FindCancellationReason2(r2.CancellationReasonId);
            if (load && r2.CancelledByUserId != null) r2.CancelledByUser = FindUser2(r2.CancelledByUserId);
            if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = FindAddressType2(r2.DeliveryAddressTypeId);
            if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = FindServicePoint2(r2.PickupServicePointId);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId);
            return r2;
        }
        public RequestIdentifier FindRequestIdentifier(string id, bool load = false)
        {
            var ri = Query<RequestIdentifier>($"SELECT id AS \"Id\", request_id AS \"RequestId\", value AS \"Value\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}request_identifiers WHERE id = @id", new { id }).SingleOrDefault();
            if (ri == null) return null;
            if (load && ri.RequestId != null) ri.Request = FindRequest2(ri.RequestId);
            if (load && ri.IdentifierTypeId != null) ri.IdentifierType = FindIdType2(ri.IdentifierTypeId);
            return ri;
        }
        public RequestPolicy FindRequestPolicy(Guid? id, bool load = false) => Query<RequestPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy WHERE id = @id", new { id }).SingleOrDefault();
        public RequestPolicy2 FindRequestPolicy2(Guid? id, bool load = false)
        {
            var rp2 = Query<RequestPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_policies WHERE id = @id", new { id }).SingleOrDefault();
            if (rp2 == null) return null;
            if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId);
            if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId);
            return rp2;
        }
        public RequestPolicyRequestType FindRequestPolicyRequestType(string id, bool load = false)
        {
            var rprt = Query<RequestPolicyRequestType>($"SELECT id AS \"Id\", request_policy_id AS \"RequestPolicyId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_policy_request_types WHERE id = @id", new { id }).SingleOrDefault();
            if (rprt == null) return null;
            if (load && rprt.RequestPolicyId != null) rprt.RequestPolicy = FindRequestPolicy2(rprt.RequestPolicyId);
            return rprt;
        }
        public RequestTag FindRequestTag(string id, bool load = false)
        {
            var rt = Query<RequestTag>($"SELECT id AS \"Id\", request_id AS \"RequestId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (rt == null) return null;
            if (load && rt.RequestId != null) rt.Request = FindRequest2(rt.RequestId);
            return rt;
        }
        public ScheduledNotice FindScheduledNotice(Guid? id, bool load = false) => Query<ScheduledNotice>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice WHERE id = @id", new { id }).SingleOrDefault();
        public ScheduledNotice2 FindScheduledNotice2(Guid? id, bool load = false)
        {
            var sn2 = Query<ScheduledNotice2>($"SELECT id AS \"Id\", loan_id AS \"LoanId\", request_id AS \"RequestId\", payment_id AS \"PaymentId\", recipient_user_id AS \"RecipientUserId\", next_run_time AS \"NextRunTime\", triggering_event AS \"TriggeringEvent\", notice_config_timing AS \"NoticeConfigTiming\", notice_config_recurring_period_duration AS \"NoticeConfigRecurringPeriodDuration\", notice_config_recurring_period_interval_id AS \"NoticeConfigRecurringPeriodInterval\", notice_config_template_id AS \"NoticeConfigTemplateId\", notice_config_format AS \"NoticeConfigFormat\", notice_config_send_in_real_time AS \"NoticeConfigSendInRealTime\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}scheduled_notices WHERE id = @id", new { id }).SingleOrDefault();
            if (sn2 == null) return null;
            if (load && sn2.LoanId != null) sn2.Loan = FindLoan2(sn2.LoanId);
            if (load && sn2.RequestId != null) sn2.Request = FindRequest2(sn2.RequestId);
            if (load && sn2.PaymentId != null) sn2.Payment = FindPayment2(sn2.PaymentId);
            if (load && sn2.RecipientUserId != null) sn2.RecipientUser = FindUser2(sn2.RecipientUserId);
            if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = FindTemplate2(sn2.NoticeConfigTemplateId);
            if (load && sn2.CreationUserId != null) sn2.CreationUser = FindUser2(sn2.CreationUserId);
            if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = FindUser2(sn2.LastWriteUserId);
            return sn2;
        }
        public Series FindSeries(string id, Guid? instanceId, bool load = false)
        {
            var s = Query<Series>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}series WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (s == null) return null;
            if (load && s.InstanceId != null) s.Instance = FindInstance2(s.InstanceId);
            return s;
        }
        public ServicePoint FindServicePoint(Guid? id, bool load = false) => Query<ServicePoint>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point WHERE id = @id", new { id }).SingleOrDefault();
        public ServicePoint2 FindServicePoint2(Guid? id, bool load = false)
        {
            var sp2 = Query<ServicePoint2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", discovery_display_name AS \"DiscoveryDisplayName\", description AS \"Description\", shelving_lag_time AS \"ShelvingLagTime\", pickup_location AS \"PickupLocation\", hold_shelf_expiry_period_duration AS \"HoldShelfExpiryPeriodDuration\", hold_shelf_expiry_period_interval_id AS \"HoldShelfExpiryPeriodInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}service_points WHERE id = @id", new { id }).SingleOrDefault();
            if (sp2 == null) return null;
            if (load && sp2.CreationUserId != null) sp2.CreationUser = FindUser2(sp2.CreationUserId);
            if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = FindUser2(sp2.LastWriteUserId);
            return sp2;
        }
        public ServicePointOwner FindServicePointOwner(string id, bool load = false)
        {
            var spo = Query<ServicePointOwner>($"SELECT id AS \"Id\", owner_id AS \"OwnerId\", value AS \"Value\", label AS \"Label\" FROM uc{(IsMySql ? "_" : ".")}service_point_owners WHERE id = @id", new { id }).SingleOrDefault();
            if (spo == null) return null;
            if (load && spo.OwnerId != null) spo.Owner = FindOwner2(spo.OwnerId);
            return spo;
        }
        public ServicePointStaffSlip FindServicePointStaffSlip(string id, bool load = false)
        {
            var spss = Query<ServicePointStaffSlip>($"SELECT id AS \"Id\", service_point_id AS \"ServicePointId\", staff_slip_id AS \"StaffSlipId\", print_by_default AS \"PrintByDefault\" FROM uc{(IsMySql ? "_" : ".")}service_point_staff_slips WHERE id = @id", new { id }).SingleOrDefault();
            if (spss == null) return null;
            if (load && spss.ServicePointId != null) spss.ServicePoint = FindServicePoint2(spss.ServicePointId);
            if (load && spss.StaffSlipId != null) spss.StaffSlip = FindStaffSlip2(spss.StaffSlipId);
            return spss;
        }
        public ServicePointUser FindServicePointUser(Guid? id, bool load = false)
        {
            var spu = Query<ServicePointUser>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", defaultservicepointid AS \"Defaultservicepointid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user WHERE id = @id", new { id }).SingleOrDefault();
            if (spu == null) return null;
            if (load && spu.Defaultservicepointid != null) spu.ServicePoint = FindServicePoint(spu.Defaultservicepointid);
            return spu;
        }
        public ServicePointUser2 FindServicePointUser2(Guid? id, bool load = false)
        {
            var spu2 = Query<ServicePointUser2>($"SELECT id AS \"Id\", user_id AS \"UserId\", default_service_point_id AS \"DefaultServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}service_point_users WHERE id = @id", new { id }).SingleOrDefault();
            if (spu2 == null) return null;
            if (load && spu2.UserId != null) spu2.User = FindUser2(spu2.UserId);
            if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = FindServicePoint2(spu2.DefaultServicePointId);
            if (load && spu2.CreationUserId != null) spu2.CreationUser = FindUser2(spu2.CreationUserId);
            if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = FindUser2(spu2.LastWriteUserId);
            return spu2;
        }
        public ServicePointUserServicePoint FindServicePointUserServicePoint(string id, bool load = false)
        {
            var spusp = Query<ServicePointUserServicePoint>($"SELECT id AS \"Id\", service_point_user_id AS \"ServicePointUserId\", service_point_id AS \"ServicePointId\" FROM uc{(IsMySql ? "_" : ".")}service_point_user_service_points WHERE id = @id", new { id }).SingleOrDefault();
            if (spusp == null) return null;
            if (load && spusp.ServicePointUserId != null) spusp.ServicePointUser = FindServicePointUser2(spusp.ServicePointUserId);
            if (load && spusp.ServicePointId != null) spusp.ServicePoint = FindServicePoint2(spusp.ServicePointId);
            return spusp;
        }
        public Setting FindSetting(Guid? id, bool load = false)
        {
            var s = Query<Setting>($"SELECT id AS \"Id\", name AS \"Name\", orientation AS \"Orientation\", font_family AS \"FontFamily\", font_size AS \"FontSize\", font_weight AS \"FontWeight\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}settings WHERE id = @id", new { id }).SingleOrDefault();
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId);
            if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId);
            return s;
        }
        public Snapshot FindSnapshot(Guid? id, bool load = false) => Query<Snapshot>($"SELECT id AS \"Id\", status AS \"Status\", processing_started_date AS \"ProcessingStartedDate\", created_by_user_id AS \"CreationUserId\", created_date AS \"CreationTime\", updated_by_user_id AS \"LastWriteUserId\", updated_date AS \"LastWriteTime\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb WHERE id = @id", new { id }).SingleOrDefault();
        public Snapshot2 FindSnapshot2(Guid? id, bool load = false)
        {
            var s2 = Query<Snapshot2>($"SELECT id AS \"Id\", status AS \"Status\", processing_started_date AS \"ProcessingStartedDate\", creation_user_id AS \"CreationUserId\", creation_time AS \"CreationTime\", last_write_user_id AS \"LastWriteUserId\", last_write_time AS \"LastWriteTime\" FROM uc{(IsMySql ? "_" : ".")}snapshots WHERE id = @id", new { id }).SingleOrDefault();
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId);
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId);
            return s2;
        }
        public Source FindSource(Guid? id, bool load = false) => Query<Source>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source WHERE id = @id", new { id }).SingleOrDefault();
        public Source2 FindSource2(Guid? id, bool load = false)
        {
            var s2 = Query<Source2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}sources WHERE id = @id", new { id }).SingleOrDefault();
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId);
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId);
            return s2;
        }
        public SourceMarc FindSourceMarc(Guid? id, bool load = false) => Query<SourceMarc>($"SELECT id AS \"Id\", leader AS \"Leader\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}source_marcs WHERE id = @id", new { id }).SingleOrDefault();
        public SourceMarcField FindSourceMarcField(string id, bool load = false)
        {
            var smf = Query<SourceMarcField>($"SELECT id AS \"Id\", source_marc_id AS \"SourceMarcId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}source_marc_fields WHERE id = @id", new { id }).SingleOrDefault();
            if (smf == null) return null;
            if (load && smf.SourceMarcId != null) smf.SourceMarc = FindSourceMarc(smf.SourceMarcId);
            return smf;
        }
        public StaffSlip FindStaffSlip(Guid? id, bool load = false) => Query<StaffSlip>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips WHERE id = @id", new { id }).SingleOrDefault();
        public StaffSlip2 FindStaffSlip2(Guid? id, bool load = false)
        {
            var ss2 = Query<StaffSlip2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", active AS \"Active\", template AS \"Template\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}staff_slips WHERE id = @id", new { id }).SingleOrDefault();
            if (ss2 == null) return null;
            if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId);
            if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId);
            return ss2;
        }
        public StatisticalCode FindStatisticalCode(Guid? id, bool load = false)
        {
            var sc = Query<StatisticalCode>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", statisticalcodetypeid AS \"Statisticalcodetypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code WHERE id = @id", new { id }).SingleOrDefault();
            if (sc == null) return null;
            if (load && sc.Statisticalcodetypeid != null) sc.StatisticalCodeType = FindStatisticalCodeType(sc.Statisticalcodetypeid);
            return sc;
        }
        public StatisticalCode2 FindStatisticalCode2(Guid? id, bool load = false)
        {
            var sc2 = Query<StatisticalCode2>($"SELECT id AS \"Id\", code AS \"Code\", name AS \"Name\", statistical_code_type_id AS \"StatisticalCodeTypeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statistical_codes WHERE id = @id", new { id }).SingleOrDefault();
            if (sc2 == null) return null;
            if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId);
            if (load && sc2.CreationUserId != null) sc2.CreationUser = FindUser2(sc2.CreationUserId);
            if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = FindUser2(sc2.LastWriteUserId);
            return sc2;
        }
        public StatisticalCodeType FindStatisticalCodeType(Guid? id, bool load = false) => Query<StatisticalCodeType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type WHERE id = @id", new { id }).SingleOrDefault();
        public StatisticalCodeType2 FindStatisticalCodeType2(Guid? id, bool load = false)
        {
            var sct2 = Query<StatisticalCodeType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statistical_code_types WHERE id = @id", new { id }).SingleOrDefault();
            if (sct2 == null) return null;
            if (load && sct2.CreationUserId != null) sct2.CreationUser = FindUser2(sct2.CreationUserId);
            if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = FindUser2(sct2.LastWriteUserId);
            return sct2;
        }
        public Status FindStatus(Guid? id, bool load = false)
        {
            var s = Query<Status>($"SELECT id AS \"Id\", code AS \"Code\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statuses WHERE id = @id", new { id }).SingleOrDefault();
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId);
            if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId);
            return s;
        }
        public Subject FindSubject(string id, Guid? instanceId, bool load = false)
        {
            var s = Query<Subject>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}subjects WHERE id = @id AND instance_id = @instanceId", new { id, instanceId }).SingleOrDefault();
            if (s == null) return null;
            if (load && s.InstanceId != null) s.Instance = FindInstance2(s.InstanceId);
            return s;
        }
        public Suffix FindSuffix(Guid? id, bool load = false) => Query<Suffix>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes WHERE id = @id", new { id }).SingleOrDefault();
        public Suffix2 FindSuffix2(Guid? id, bool load = false) => Query<Suffix2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}suffixes WHERE id = @id", new { id }).SingleOrDefault();
        public SupplementStatement FindSupplementStatement(string id, Guid? holdingId, bool load = false)
        {
            var ss = Query<SupplementStatement>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Statement\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}supplement_statements WHERE id = @id AND holding_id = @holdingId", new { id, holdingId }).SingleOrDefault();
            if (ss == null) return null;
            if (load && ss.HoldingId != null) ss.Holding = FindHolding2(ss.HoldingId);
            return ss;
        }
        public Tag FindTag(Guid? id, bool load = false) => Query<Tag>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_tags{(IsMySql ? "_" : ".")}tags WHERE id = @id", new { id }).SingleOrDefault();
        public Tag2 FindTag2(Guid? id, bool load = false)
        {
            var t2 = Query<Tag2>($"SELECT id AS \"Id\", label AS \"Label\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}tags WHERE id = @id", new { id }).SingleOrDefault();
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId);
            return t2;
        }
        public Template FindTemplate(Guid? id, bool load = false) => Query<Template>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_template_engine{(IsMySql ? "_" : ".")}template WHERE id = @id", new { id }).SingleOrDefault();
        public Template2 FindTemplate2(Guid? id, bool load = false)
        {
            var t2 = Query<Template2>($"SELECT id AS \"Id\", name AS \"Name\", active AS \"Active\", category AS \"Category\", description AS \"Description\", template_resolver AS \"TemplateResolver\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}templates WHERE id = @id", new { id }).SingleOrDefault();
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId);
            return t2;
        }
        public TemplateOutputFormat FindTemplateOutputFormat(string id, bool load = false)
        {
            var tof = Query<TemplateOutputFormat>($"SELECT id AS \"Id\", template_id AS \"TemplateId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}template_output_formats WHERE id = @id", new { id }).SingleOrDefault();
            if (tof == null) return null;
            if (load && tof.TemplateId != null) tof.Template = FindTemplate2(tof.TemplateId);
            return tof;
        }
        public Title FindTitle(Guid? id, bool load = false)
        {
            var t = Query<Title>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", polineid AS \"Polineid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles WHERE id = @id", new { id }).SingleOrDefault();
            if (t == null) return null;
            if (load && t.Polineid != null) t.OrderItem = FindOrderItem(t.Polineid);
            return t;
        }
        public Title2 FindTitle2(Guid? id, bool load = false)
        {
            var t2 = Query<Title2>($"SELECT id AS \"Id\", expected_receipt_date AS \"ExpectedReceiptDate\", title AS \"Title\", po_line_id AS \"OrderItemId\", instance_id AS \"InstanceId\", publisher AS \"Publisher\", edition AS \"Edition\", package_name AS \"PackageName\", po_line_number AS \"OrderItemNumber\", published_date AS \"PublishedDate\", receiving_note AS \"ReceivingNote\", subscription_from AS \"SubscriptionFrom\", subscription_to AS \"SubscriptionTo\", subscription_interval AS \"SubscriptionInterval\", is_acknowledged AS \"IsAcknowledged\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}titles WHERE id = @id", new { id }).SingleOrDefault();
            if (t2 == null) return null;
            if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId);
            if (load && t2.InstanceId != null) t2.Instance = FindInstance2(t2.InstanceId);
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId);
            return t2;
        }
        public TitleContributor FindTitleContributor(string id, bool load = false)
        {
            var tc = Query<TitleContributor>($"SELECT id AS \"Id\", title_id AS \"TitleId\", contributor AS \"Contributor\", contributor_name_type_id AS \"ContributorNameTypeId\" FROM uc{(IsMySql ? "_" : ".")}title_contributors WHERE id = @id", new { id }).SingleOrDefault();
            if (tc == null) return null;
            if (load && tc.TitleId != null) tc.Title = FindTitle2(tc.TitleId);
            if (load && tc.ContributorNameTypeId != null) tc.ContributorNameType = FindContributorNameType2(tc.ContributorNameTypeId);
            return tc;
        }
        public TitleProductId FindTitleProductId(string id, bool load = false)
        {
            var tpi = Query<TitleProductId>($"SELECT id AS \"Id\", title_id AS \"TitleId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", qualifier AS \"Qualifier\" FROM uc{(IsMySql ? "_" : ".")}title_product_ids WHERE id = @id", new { id }).SingleOrDefault();
            if (tpi == null) return null;
            if (load && tpi.TitleId != null) tpi.Title = FindTitle2(tpi.TitleId);
            if (load && tpi.ProductIdTypeId != null) tpi.ProductIdType = FindIdType2(tpi.ProductIdTypeId);
            return tpi;
        }
        public Transaction FindTransaction(Guid? id, bool load = false)
        {
            var t = Query<Transaction>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fiscalyearid AS \"Fiscalyearid\", fromfundid AS \"Fromfundid\", sourcefiscalyearid AS \"Sourcefiscalyearid\", tofundid AS \"Tofundid\", expenseclassid AS \"Expenseclassid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction WHERE id = @id", new { id }).SingleOrDefault();
            if (t == null) return null;
            if (load && t.Fiscalyearid != null) t.FiscalYear = FindFiscalYear(t.Fiscalyearid);
            if (load && t.Fromfundid != null) t.Fund = FindFund(t.Fromfundid);
            if (load && t.Sourcefiscalyearid != null) t.FiscalYear1 = FindFiscalYear(t.Sourcefiscalyearid);
            if (load && t.Tofundid != null) t.Fund1 = FindFund(t.Tofundid);
            if (load && t.Expenseclassid != null) t.ExpenseClass = FindExpenseClass(t.Expenseclassid);
            return t;
        }
        public Transaction2 FindTransaction2(Guid? id, bool load = false)
        {
            var t2 = Query<Transaction2>($"SELECT id AS \"Id\", amount AS \"Amount\", awaiting_payment_encumbrance_id AS \"AwaitingPaymentEncumbranceId\", awaiting_payment_release_encumbrance AS \"AwaitingPaymentReleaseEncumbrance\", currency AS \"Currency\", description AS \"Description\", encumbrance_amount_awaiting_payment AS \"AwaitingPaymentAmount\", encumbrance_amount_expended AS \"ExpendedAmount\", encumbrance_initial_amount_encumbered AS \"InitialEncumberedAmount\", encumbrance_status AS \"Status\", encumbrance_order_type AS \"OrderType\", encumbrance_subscription AS \"Subscription\", encumbrance_re_encumber AS \"ReEncumber\", encumbrance_source_purchase_order_id AS \"OrderId\", encumbrance_source_po_line_id AS \"OrderItemId\", expense_class_id AS \"ExpenseClassId\", fiscal_year_id AS \"FiscalYearId\", from_fund_id AS \"FromFundId\", payment_encumbrance_id AS \"PaymentEncumbranceId\", source AS \"Source\", source_fiscal_year_id AS \"SourceFiscalYearId\", source_invoice_id AS \"InvoiceId\", source_invoice_line_id AS \"InvoiceItemId\", to_fund_id AS \"ToFundId\", transaction_type AS \"TransactionType\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transactions WHERE id = @id", new { id }).SingleOrDefault();
            if (t2 == null) return null;
            if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = FindTransaction2(t2.AwaitingPaymentEncumbranceId);
            if (load && t2.OrderId != null) t2.Order = FindOrder2(t2.OrderId);
            if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId);
            if (load && t2.ExpenseClassId != null) t2.ExpenseClass = FindExpenseClass2(t2.ExpenseClassId);
            if (load && t2.FiscalYearId != null) t2.FiscalYear = FindFiscalYear2(t2.FiscalYearId);
            if (load && t2.FromFundId != null) t2.FromFund = FindFund2(t2.FromFundId);
            if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = FindTransaction2(t2.PaymentEncumbranceId);
            if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = FindFiscalYear2(t2.SourceFiscalYearId);
            if (load && t2.InvoiceId != null) t2.Invoice = FindInvoice2(t2.InvoiceId);
            if (load && t2.InvoiceItemId != null) t2.InvoiceItem = FindInvoiceItem2(t2.InvoiceItemId);
            if (load && t2.ToFundId != null) t2.ToFund = FindFund2(t2.ToFundId);
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId);
            return t2;
        }
        public TransactionTag FindTransactionTag(string id, bool load = false)
        {
            var tt = Query<TransactionTag>($"SELECT id AS \"Id\", transaction_id AS \"TransactionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transaction_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (tt == null) return null;
            if (load && tt.TransactionId != null) tt.Transaction = FindTransaction2(tt.TransactionId);
            return tt;
        }
        public TransferAccount FindTransferAccount(Guid? id, bool load = false) => Query<TransferAccount>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers WHERE id = @id", new { id }).SingleOrDefault();
        public TransferAccount2 FindTransferAccount2(Guid? id, bool load = false)
        {
            var ta2 = Query<TransferAccount2>($"SELECT id AS \"Id\", name AS \"Name\", \"desc\" AS \"Desc\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", owner_id AS \"OwnerId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transfer_accounts WHERE id = @id", new { id }).SingleOrDefault();
            if (ta2 == null) return null;
            if (load && ta2.CreationUserId != null) ta2.CreationUser = FindUser2(ta2.CreationUserId);
            if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = FindUser2(ta2.LastWriteUserId);
            if (load && ta2.OwnerId != null) ta2.Owner = FindOwner2(ta2.OwnerId);
            return ta2;
        }
        public TransferCriteria FindTransferCriteria(Guid? id, bool load = false) => Query<TransferCriteria>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria WHERE id = @id", new { id }).SingleOrDefault();
        public TransferCriteria2 FindTransferCriteria2(Guid? id, bool load = false) => Query<TransferCriteria2>($"SELECT id AS \"Id\", criteria AS \"Criteria\", type AS \"Type\", value AS \"Value\", interval AS \"Interval\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transfer_criterias WHERE id = @id", new { id }).SingleOrDefault();
        public User FindUser(Guid? id, bool load = false)
        {
            var u = Query<User>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", patrongroup AS \"Patrongroup\" FROM diku_mod_users{(IsMySql ? "_" : ".")}users WHERE id = @id", new { id }).SingleOrDefault();
            if (u == null) return null;
            if (load && u.Patrongroup != null) u.Group = FindGroup(u.Patrongroup);
            return u;
        }
        public User2 FindUser2(Guid? id, bool load = false)
        {
            var u2 = Query<User2>($"SELECT id AS \"Id\", username AS \"Username\", external_system_id AS \"ExternalSystemId\", barcode AS \"Barcode\", active AS \"Active\", type AS \"Type\", group_id AS \"GroupId\", name AS \"Name\", last_name AS \"LastName\", first_name AS \"FirstName\", middle_name AS \"MiddleName\", preferred_first_name AS \"PreferredFirstName\", email AS \"EmailAddress\", phone AS \"PhoneNumber\", mobile_phone AS \"MobilePhoneNumber\", date_of_birth AS \"BirthDate\", preferred_contact_type_id AS \"PreferredContactTypeId\", enrollment_date AS \"StartDate\", expiration_date AS \"EndDate\", source AS \"Source\", category AS \"Category\", status AS \"Status\", statuses AS \"Statuses\", staff_status AS \"StaffStatus\", staff_privileges AS \"StaffPrivileges\", staff_division AS \"StaffDivision\", staff_department AS \"StaffDepartment\", student_id AS \"StudentId\", student_status AS \"StudentStatus\", student_restriction AS \"StudentRestriction\", student_division AS \"StudentDivision\", student_department AS \"StudentDepartment\", deceased AS \"Deceased\", collections AS \"Collections\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}users WHERE id = @id", new { id }).SingleOrDefault();
            if (u2 == null) return null;
            if (load && u2.GroupId != null) u2.Group = FindGroup2(u2.GroupId);
            if (load && u2.PreferredContactTypeId != null) u2.PreferredContactType = FindContactType(u2.PreferredContactTypeId);
            if (load && u2.CreationUserId != null) u2.CreationUser = FindUser2(u2.CreationUserId);
            if (load && u2.LastWriteUserId != null) u2.LastWriteUser = FindUser2(u2.LastWriteUserId);
            return u2;
        }
        public UserAcquisitionsUnit FindUserAcquisitionsUnit(Guid? id, bool load = false)
        {
            var uau = Query<UserAcquisitionsUnit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", acquisitionsunitid AS \"Acquisitionsunitid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership WHERE id = @id", new { id }).SingleOrDefault();
            if (uau == null) return null;
            if (load && uau.Acquisitionsunitid != null) uau.AcquisitionsUnit = FindAcquisitionsUnit(uau.Acquisitionsunitid);
            return uau;
        }
        public UserAcquisitionsUnit2 FindUserAcquisitionsUnit2(Guid? id, bool load = false)
        {
            var uau2 = Query<UserAcquisitionsUnit2>($"SELECT id AS \"Id\", user_id AS \"UserId\", acquisitions_unit_id AS \"AcquisitionsUnitId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (uau2 == null) return null;
            if (load && uau2.UserId != null) uau2.User = FindUser2(uau2.UserId);
            if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId);
            if (load && uau2.CreationUserId != null) uau2.CreationUser = FindUser2(uau2.CreationUserId);
            if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = FindUser2(uau2.LastWriteUserId);
            return uau2;
        }
        public UserAddress FindUserAddress(string id, bool load = false)
        {
            var ua = Query<UserAddress>($"SELECT id AS \"Id\", user_id AS \"UserId\", id2 AS \"Id2\", country_id AS \"CountryCode\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", region AS \"State\", postal_code AS \"PostalCode\", address_type_id AS \"AddressTypeId\", primary_address AS \"Default\" FROM uc{(IsMySql ? "_" : ".")}user_addresses WHERE id = @id", new { id }).SingleOrDefault();
            if (ua == null) return null;
            if (load && ua.UserId != null) ua.User = FindUser2(ua.UserId);
            if (load && ua.AddressTypeId != null) ua.AddressType = FindAddressType2(ua.AddressTypeId);
            return ua;
        }
        public UserDepartment FindUserDepartment(string id, bool load = false)
        {
            var ud = Query<UserDepartment>($"SELECT id AS \"Id\", user_id AS \"UserId\", department_id AS \"DepartmentId\" FROM uc{(IsMySql ? "_" : ".")}user_departments WHERE id = @id", new { id }).SingleOrDefault();
            if (ud == null) return null;
            if (load && ud.UserId != null) ud.User = FindUser2(ud.UserId);
            if (load && ud.DepartmentId != null) ud.Department = FindDepartment2(ud.DepartmentId);
            return ud;
        }
        public UserRequestPreference FindUserRequestPreference(Guid? id, bool load = false) => Query<UserRequestPreference>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference WHERE id = @id", new { id }).SingleOrDefault();
        public UserRequestPreference2 FindUserRequestPreference2(Guid? id, bool load = false)
        {
            var urp2 = Query<UserRequestPreference2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hold_shelf AS \"HoldShelf\", delivery AS \"Delivery\", default_service_point_id AS \"DefaultServicePointId\", default_delivery_address_type_id AS \"DefaultDeliveryAddressTypeId\", fulfillment AS \"Fulfillment\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_request_preferences WHERE id = @id", new { id }).SingleOrDefault();
            if (urp2 == null) return null;
            if (load && urp2.UserId != null) urp2.User = FindUser2(urp2.UserId);
            if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = FindServicePoint2(urp2.DefaultServicePointId);
            if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = FindAddressType2(urp2.DefaultDeliveryAddressTypeId);
            if (load && urp2.CreationUserId != null) urp2.CreationUser = FindUser2(urp2.CreationUserId);
            if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = FindUser2(urp2.LastWriteUserId);
            return urp2;
        }
        public UserSummary FindUserSummary(Guid? id, bool load = false) => Query<UserSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary WHERE id = @id", new { id }).SingleOrDefault();
        public UserSummary2 FindUserSummary2(Guid? id, bool load = false)
        {
            var us2 = Query<UserSummary2>($"SELECT id AS \"Id\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_summaries WHERE id = @id", new { id }).SingleOrDefault();
            if (us2 == null) return null;
            if (load && us2.UserId != null) us2.User = FindUser2(us2.UserId);
            if (load && us2.CreationUserId != null) us2.CreationUser = FindUser2(us2.CreationUserId);
            if (load && us2.LastWriteUserId != null) us2.LastWriteUser = FindUser2(us2.LastWriteUserId);
            return us2;
        }
        public UserSummaryOpenFeesFine FindUserSummaryOpenFeesFine(string id, bool load = false)
        {
            var usoff = Query<UserSummaryOpenFeesFine>($"SELECT id AS \"Id\", user_summary_id AS \"UserSummaryId\", fee_fine_id AS \"FeeFineId\", fee_fine_type_id AS \"FeeFineTypeId\", loan_id AS \"LoanId\", balance AS \"Balance\" FROM uc{(IsMySql ? "_" : ".")}user_summary_open_fees_fines WHERE id = @id", new { id }).SingleOrDefault();
            if (usoff == null) return null;
            if (load && usoff.UserSummaryId != null) usoff.UserSummary = FindUserSummary2(usoff.UserSummaryId);
            if (load && usoff.FeeFineId != null) usoff.FeeFine = FindFee2(usoff.FeeFineId);
            if (load && usoff.FeeFineTypeId != null) usoff.FeeFineType = FindFeeType2(usoff.FeeFineTypeId);
            if (load && usoff.LoanId != null) usoff.Loan = FindLoan2(usoff.LoanId);
            return usoff;
        }
        public UserSummaryOpenLoan FindUserSummaryOpenLoan(string id, bool load = false)
        {
            var usol = Query<UserSummaryOpenLoan>($"SELECT id AS \"Id\", user_summary_id AS \"UserSummaryId\", loan_id AS \"LoanId\", due_date AS \"DueDate\", recall AS \"Recall\", item_lost AS \"ItemLost\", item_claimed_returned AS \"ItemClaimedReturned\" FROM uc{(IsMySql ? "_" : ".")}user_summary_open_loans WHERE id = @id", new { id }).SingleOrDefault();
            if (usol == null) return null;
            if (load && usol.UserSummaryId != null) usol.UserSummary = FindUserSummary2(usol.UserSummaryId);
            if (load && usol.LoanId != null) usol.Loan = FindLoan2(usol.LoanId);
            return usol;
        }
        public UserTag FindUserTag(string id, bool load = false)
        {
            var ut = Query<UserTag>($"SELECT id AS \"Id\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_tags WHERE id = @id", new { id }).SingleOrDefault();
            if (ut == null) return null;
            if (load && ut.UserId != null) ut.User = FindUser2(ut.UserId);
            return ut;
        }
        public Voucher FindVoucher(Guid? id, bool load = false)
        {
            var v = Query<Voucher>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers WHERE id = @id", new { id }).SingleOrDefault();
            if (v == null) return null;
            if (load && v.Invoiceid != null) v.Invoice = FindInvoice(v.Invoiceid);
            if (load && v.Batchgroupid != null) v.BatchGroup = FindBatchGroup(v.Batchgroupid);
            return v;
        }
        public Voucher2 FindVoucher2(Guid? id, bool load = false)
        {
            var v2 = Query<Voucher2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", amount AS \"Amount\", batch_group_id AS \"BatchGroupId\", disbursement_number AS \"DisbursementNumber\", disbursement_date AS \"DisbursementDate\", disbursement_amount AS \"DisbursementAmount\", invoice_currency AS \"InvoiceCurrency\", invoice_id AS \"InvoiceId\", exchange_rate AS \"ExchangeRate\", export_to_accounting AS \"ExportToAccounting\", status AS \"Status\", system_currency AS \"SystemCurrency\", type AS \"Type\", voucher_date AS \"VoucherDate\", voucher_number AS \"VoucherNumber\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}vouchers WHERE id = @id", new { id }).SingleOrDefault();
            if (v2 == null) return null;
            if (load && v2.BatchGroupId != null) v2.BatchGroup = FindBatchGroup2(v2.BatchGroupId);
            if (load && v2.InvoiceId != null) v2.Invoice = FindInvoice2(v2.InvoiceId);
            if (load && v2.CreationUserId != null) v2.CreationUser = FindUser2(v2.CreationUserId);
            if (load && v2.LastWriteUserId != null) v2.LastWriteUser = FindUser2(v2.LastWriteUserId);
            return v2;
        }
        public VoucherAcquisitionsUnit FindVoucherAcquisitionsUnit(string id, bool load = false)
        {
            var vau = Query<VoucherAcquisitionsUnit>($"SELECT id AS \"Id\", voucher_id AS \"VoucherId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}voucher_acquisitions_units WHERE id = @id", new { id }).SingleOrDefault();
            if (vau == null) return null;
            if (load && vau.VoucherId != null) vau.Voucher = FindVoucher2(vau.VoucherId);
            if (load && vau.AcquisitionsUnitId != null) vau.AcquisitionsUnit = FindAcquisitionsUnit2(vau.AcquisitionsUnitId);
            return vau;
        }
        public VoucherItem FindVoucherItem(Guid? id, bool load = false)
        {
            var vi = Query<VoucherItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", voucherid AS \"Voucherid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines WHERE id = @id", new { id }).SingleOrDefault();
            if (vi == null) return null;
            if (load && vi.Voucherid != null) vi.Voucher = FindVoucher(vi.Voucherid);
            return vi;
        }
        public VoucherItem2 FindVoucherItem2(Guid? id, bool load = false)
        {
            var vi2 = Query<VoucherItem2>($"SELECT id AS \"Id\", amount AS \"Amount\", external_account_number AS \"ExternalAccountNumber\", sub_transaction_id AS \"SubTransactionId\", voucher_id AS \"VoucherId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}voucher_items WHERE id = @id", new { id }).SingleOrDefault();
            if (vi2 == null) return null;
            if (load && vi2.SubTransactionId != null) vi2.SubTransaction = FindTransaction2(vi2.SubTransactionId);
            if (load && vi2.VoucherId != null) vi2.Voucher = FindVoucher2(vi2.VoucherId);
            if (load && vi2.CreationUserId != null) vi2.CreationUser = FindUser2(vi2.CreationUserId);
            if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = FindUser2(vi2.LastWriteUserId);
            return vi2;
        }
        public VoucherItemFund FindVoucherItemFund(string id, bool load = false)
        {
            var vif = Query<VoucherItemFund>($"SELECT id AS \"Id\", voucher_item_id AS \"VoucherItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}voucher_item_fund_distributions WHERE id = @id", new { id }).SingleOrDefault();
            if (vif == null) return null;
            if (load && vif.VoucherItemId != null) vif.VoucherItem = FindVoucherItem2(vif.VoucherItemId);
            if (load && vif.EncumbranceId != null) vif.Encumbrance = FindTransaction2(vif.EncumbranceId);
            if (load && vif.FundId != null) vif.Fund = FindFund2(vif.FundId);
            if (load && vif.InvoiceItemId != null) vif.InvoiceItem = FindInvoiceItem2(vif.InvoiceItemId);
            if (load && vif.ExpenseClassId != null) vif.ExpenseClass = FindExpenseClass2(vif.ExpenseClassId);
            return vif;
        }
        public VoucherItemSourceId FindVoucherItemSourceId(string id, bool load = false)
        {
            var visi = Query<VoucherItemSourceId>($"SELECT id AS \"Id\", voucher_item_id AS \"VoucherItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}voucher_item_source_ids WHERE id = @id", new { id }).SingleOrDefault();
            if (visi == null) return null;
            if (load && visi.VoucherItemId != null) visi.VoucherItem = FindVoucherItem2(visi.VoucherItemId);
            return visi;
        }
        public WaiveReason FindWaiveReason(Guid? id, bool load = false) => Query<WaiveReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}waives WHERE id = @id", new { id }).SingleOrDefault();
        public WaiveReason2 FindWaiveReason2(Guid? id, bool load = false)
        {
            var wr2 = Query<WaiveReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", account_id AS \"AccountId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}waive_reasons WHERE id = @id", new { id }).SingleOrDefault();
            if (wr2 == null) return null;
            if (load && wr2.CreationUserId != null) wr2.CreationUser = FindUser2(wr2.CreationUserId);
            if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = FindUser2(wr2.LastWriteUserId);
            if (load && wr2.AccountId != null) wr2.Account = FindFee2(wr2.AccountId);
            return wr2;
        }

        public IEnumerable<AcquisitionsUnit> AcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AcquisitionsUnit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AcquisitionsUnit2> AcquisitionsUnit2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AcquisitionsUnit2>($"SELECT id AS \"Id\", name AS \"Name\", is_deleted AS \"IsDeleted\", protect_create AS \"ProtectCreate\", protect_read AS \"ProtectRead\", protect_update AS \"ProtectUpdate\", protect_delete AS \"ProtectDelete\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Address> Addresses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Address>($"SELECT id AS \"Id\", name AS \"Name\", content AS \"Content\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}addresses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AddressType> AddressTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AddressType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}addresstype{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AddressType2> AddressType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AddressType2>($"SELECT id AS \"Id\", address_type AS \"Name\", \"desc\" AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}address_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Alert> Alerts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Alert>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Alert2> Alert2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Alert2>($"SELECT id AS \"Id\", alert AS \"Alert\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alerts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AllocatedFromFund> AllocatedFromFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AllocatedFromFund>($"SELECT id AS \"Id\", fund_id AS \"FundId\", from_fund_id AS \"FromFundId\" FROM uc{(IsMySql ? "_" : ".")}allocated_from_funds{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AllocatedToFund> AllocatedToFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AllocatedToFund>($"SELECT id AS \"Id\", fund_id AS \"FundId\", to_fund_id AS \"ToFundId\" FROM uc{(IsMySql ? "_" : ".")}allocated_to_funds{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AlternativeTitle> AlternativeTitles(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AlternativeTitle>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", alternative_title_type_id AS \"AlternativeTitleTypeId\", alternative_title AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alternative_titles{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<AlternativeTitleType> AlternativeTitleTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AlternativeTitleType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AlternativeTitleType2> AlternativeTitleType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AlternativeTitleType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}alternative_title_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuditLoan> AuditLoans(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuditLoan>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuthAttempt> AuthAttempts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuthAttempt>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuthAttempt2> AuthAttempt2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuthAttempt2>($"SELECT id AS \"Id\", user_id AS \"UserId\", last_attempt AS \"LastAttempt\", attempt_count AS \"AttemptCount\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuthCredentialsHistory> AuthCredentialsHistories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuthCredentialsHistory>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuthCredentialsHistory2> AuthCredentialsHistory2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuthCredentialsHistory2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hash AS \"Hash\", salt AS \"Salt\", date AS \"Date\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}auth_credentials_histories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<AuthPasswordAction> AuthPasswordActions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<AuthPasswordAction>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchGroup> BatchGroups(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchGroup>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchGroup2> BatchGroup2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchGroup2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucher> BatchVouchers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucher>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucher2> BatchVoucher2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucher2>($"SELECT id AS \"Id\", batch_group AS \"BatchGroup\", created AS \"Created\", start AS \"Start\", \"end\" AS \"End\", total_records AS \"TotalRecords\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherBatchedVoucher> BatchVoucherBatchedVouchers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherBatchedVoucher>($"SELECT id AS \"Id\", batch_voucher_id AS \"BatchVoucherId\", accounting_code AS \"AccountingCode\", amount AS \"Amount\", disbursement_number AS \"DisbursementNumber\", disbursement_date AS \"DisbursementDate\", disbursement_amount AS \"DisbursementAmount\", enclosure_needed AS \"EnclosureNeeded\", exchange_rate AS \"ExchangeRate\", folio_invoice_no AS \"FolioInvoiceNo\", invoice_currency AS \"InvoiceCurrency\", invoice_note AS \"InvoiceNote\", status AS \"Status\", system_currency AS \"SystemCurrency\", type AS \"Type\", vendor_invoice_no AS \"VendorInvoiceNo\", vendor_name AS \"VendorName\", voucher_date AS \"VoucherDate\", voucher_number AS \"VoucherNumber\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_vouchers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherBatchedVoucherBatchedVoucherLine> BatchVoucherBatchedVoucherBatchedVoucherLines(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherBatchedVoucherBatchedVoucherLine>($"SELECT id AS \"Id\", batch_voucher_batched_voucher_id AS \"BatchVoucherBatchedVoucherId\", amount AS \"Amount\", external_account_number AS \"ExternalAccountNumber\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_lines{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherBatchedVoucherBatchedVoucherLineFundCode> BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherBatchedVoucherBatchedVoucherLineFundCode>($"SELECT id AS \"Id\", batch_voucher_batched_voucher_batched_voucher_line_id AS \"BatchVoucherBatchedVoucherBatchedVoucherLineId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_line_fund_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherExport> BatchVoucherExports(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherExport>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\", batchvoucherid AS \"Batchvoucherid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherExport2> BatchVoucherExport2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherExport2>($"SELECT id AS \"Id\", status AS \"Status\", message AS \"Message\", batch_group_id AS \"BatchGroupId\", start AS \"Start\", \"end\" AS \"End\", batch_voucher_id AS \"BatchVoucherId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherExportConfig> BatchVoucherExportConfigs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherExportConfig>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherExportConfig2> BatchVoucherExportConfig2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherExportConfig2>($"SELECT id AS \"Id\", batch_group_id AS \"BatchGroupId\", enable_scheduled_export AS \"EnableScheduledExport\", format AS \"Format\", start_time AS \"StartTime\", upload_uri AS \"UploadUri\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BatchVoucherExportConfigWeekday> BatchVoucherExportConfigWeekdays(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BatchVoucherExportConfigWeekday>($"SELECT id AS \"Id\", batch_voucher_export_config_id AS \"BatchVoucherExportConfigId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_config_weekdays{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Block> Blocks(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Block>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Block2> Block2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Block2>($"SELECT id AS \"Id\", type AS \"Type\", \"desc\" AS \"Desc\", staff_information AS \"StaffInformation\", patron_message AS \"PatronMessage\", expiration_date AS \"ExpirationDate\", borrowing AS \"Borrowing\", renewals AS \"Renewals\", requests AS \"Requests\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}blocks{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BlockCondition> BlockConditions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BlockCondition>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BlockCondition2> BlockCondition2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BlockCondition2>($"SELECT id AS \"Id\", name AS \"Name\", block_borrowing AS \"BlockBorrowing\", block_renewals AS \"BlockRenewals\", block_requests AS \"BlockRequests\", value_type AS \"ValueType\", message AS \"Message\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}block_conditions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BlockLimit> BlockLimits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BlockLimit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", conditionid AS \"Conditionid\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BlockLimit2> BlockLimit2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BlockLimit2>($"SELECT id AS \"Id\", group_id AS \"GroupId\", condition_id AS \"ConditionId\", value AS \"Value\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", conditionid AS \"Conditionid\" FROM uc{(IsMySql ? "_" : ".")}block_limits{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Budget> Budgets(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Budget>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fundid AS \"FundId\", fiscalyearid AS \"FiscalYearId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Budget2> Budget2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Budget2>($"SELECT id AS \"Id\", name AS \"Name\", budget_status AS \"BudgetStatus\", allowable_encumbrance AS \"AllowableEncumbrance\", allowable_expenditure AS \"AllowableExpenditure\", allocated AS \"Allocated\", awaiting_payment AS \"AwaitingPayment\", available AS \"Available\", encumbered AS \"Encumbered\", expenditures AS \"Expenditures\", net_transfers AS \"NetTransfers\", unavailable AS \"Unavailable\", over_encumbrance AS \"OverEncumbrance\", over_expended AS \"OverExpended\", fund_id AS \"FundId\", fiscal_year_id AS \"FiscalYearId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}budgets{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BudgetAcquisitionsUnit> BudgetAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BudgetAcquisitionsUnit>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}budget_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BudgetExpenseClass> BudgetExpenseClasses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BudgetExpenseClass>($"SELECT id AS \"Id\", jsonb AS \"Content\", budgetid AS \"Budgetid\", expenseclassid AS \"Expenseclassid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BudgetExpenseClass2> BudgetExpenseClass2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BudgetExpenseClass2>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", expense_class_id AS \"ExpenseClassId\", status AS \"Status\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}budget_expense_classes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<BudgetTag> BudgetTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<BudgetTag>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", tag_id AS \"TagId\" FROM uc{(IsMySql ? "_" : ".")}budget_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CallNumberType> CallNumberTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CallNumberType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CallNumberType2> CallNumberType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CallNumberType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}call_number_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Campus> Campuses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Campus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", institutionid AS \"Institutionid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Campus2> Campus2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Campus2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", institution_id AS \"InstitutionId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}campuses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CancellationReason> CancellationReasons(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CancellationReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CancellationReason2> CancellationReason2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CancellationReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", public_description AS \"PublicDescription\", requires_additional_information AS \"RequiresAdditionalInformation\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}cancellation_reasons{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Category> Categories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Category>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Category2> Category2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Category2>($"SELECT id AS \"Id\", value AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CheckIn> CheckIns(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CheckIn>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CheckIn2> CheckIn2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CheckIn2>($"SELECT id AS \"Id\", occurred_date_time AS \"OccurredDateTime\", item_id AS \"ItemId\", item_status_prior_to_check_in AS \"ItemStatusPriorToCheckIn\", request_queue_size AS \"RequestQueueSize\", item_location_id AS \"ItemLocationId\", service_point_id AS \"ServicePointId\", performed_by_user_id AS \"PerformedByUserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}check_ins{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CirculationNote> CirculationNotes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CirculationNote>($"SELECT id AS \"Id\", item_id AS \"ItemId\", id2 AS \"Id2\", note_type AS \"NoteType\", note AS \"Note\", source_id AS \"SourceId\", source_personal_last_name AS \"SourcePersonalLastName\", source_personal_first_name AS \"SourcePersonalFirstName\", date AS \"Date\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}circulation_notes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<CirculationRule> CirculationRules(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CirculationRule>($"SELECT id AS \"Id\", jsonb AS \"Content\", lock AS \"Lock\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CirculationRule2> CirculationRule2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CirculationRule2>($"SELECT id AS \"Id\", rules_as_text AS \"RulesAsText\", content AS \"Content\", lock AS \"Lock\" FROM uc{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Classification> Classifications(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Classification>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", classification_number AS \"Number\", classification_type_id AS \"ClassificationTypeId\" FROM uc{(IsMySql ? "_" : ".")}classifications{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<ClassificationType> ClassificationTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ClassificationType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ClassificationType2> ClassificationType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ClassificationType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}classification_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CloseReason> CloseReasons(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CloseReason>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CloseReason2> CloseReason2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CloseReason2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}close_reasons{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Comment> Comments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Comment>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Comment2> Comment2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Comment2>($"SELECT id AS \"Id\", paid AS \"Paid\", waived AS \"Waived\", refunded AS \"Refunded\", transferred_manually AS \"TransferredManually\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Configuration> Configurations(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Configuration>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_configuration{(IsMySql ? "_" : ".")}config_data{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Configuration2> Configuration2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Configuration2>($"SELECT id AS \"Id\", module AS \"Module\", config_name AS \"ConfigName\", code AS \"Code\", description AS \"Description\", \"default\" AS \"Default\", enabled AS \"Enabled\", value AS \"Value\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}configurations{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Contact> Contacts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Contact>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Contact2> Contact2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Contact2>($"SELECT id AS \"Id\", name AS \"Name\", prefix AS \"Prefix\", first_name AS \"FirstName\", last_name AS \"LastName\", language AS \"Language\", notes AS \"Notes\", inactive AS \"Inactive\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactAddress> ContactAddresses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactAddress>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", state_region AS \"StateRegion\", zip_code AS \"ZipCode\", country AS \"Country\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_addresses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactAddressCategory> ContactAddressCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactAddressCategory>($"SELECT id AS \"Id\", contact_address_id AS \"ContactAddressId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_address_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactCategory> ContactCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactCategory>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactEmail> ContactEmails(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactEmail>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_emails{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactEmailCategory> ContactEmailCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactEmailCategory>($"SELECT id AS \"Id\", contact_email_id AS \"ContactEmailId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_email_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactPhoneNumber> ContactPhoneNumbers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactPhoneNumber>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", phone_number AS \"PhoneNumber\", type AS \"Type\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_phone_numbers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactPhoneNumberCategory> ContactPhoneNumberCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactPhoneNumberCategory>($"SELECT id AS \"Id\", contact_phone_number_id AS \"ContactPhoneNumberId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_phone_number_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactType> ContactTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactType>($"SELECT id AS \"Id\", name AS \"Name\" FROM uc{(IsMySql ? "_" : ".")}contact_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactUrl> ContactUrls(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactUrl>($"SELECT id AS \"Id\", contact_id AS \"ContactId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", language AS \"Language\", is_primary AS \"IsPrimary\", notes AS \"Notes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}contact_urls{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContactUrlCategory> ContactUrlCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContactUrlCategory>($"SELECT id AS \"Id\", contact_url_id AS \"ContactUrlId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}contact_url_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Contributor> Contributors(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Contributor>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", name AS \"Name\", contributor_type_id AS \"ContributorTypeId\", contributor_type_text AS \"ContributorTypeText\", contributor_name_type_id AS \"ContributorNameTypeId\", primary AS \"Primary\" FROM uc{(IsMySql ? "_" : ".")}contributors{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<ContributorNameType> ContributorNameTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContributorNameType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContributorNameType2> ContributorNameType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContributorNameType2>($"SELECT id AS \"Id\", name AS \"Name\", ordering AS \"Ordering\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contributor_name_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContributorType> ContributorTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContributorType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ContributorType2> ContributorType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ContributorType2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}contributor_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Country> Countries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Country>($"SELECT alpha2_code AS \"Alpha2Code\", alpha3_code AS \"Alpha3Code\", name AS \"Name\" FROM uc{(IsMySql ? "_" : ".")}countries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "alpha2_code"}" : "")}", param, skip, take);
        public IEnumerable<Currency> Currencies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Currency>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}currencies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CustomField> CustomFields(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CustomField>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_users{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CustomField2> CustomField2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CustomField2>($"SELECT id AS \"Id\", name AS \"Name\", ref_id AS \"RefId\", type AS \"Type\", entity_type AS \"EntityType\", visible AS \"Visible\", required AS \"Required\", is_repeatable AS \"IsRepeatable\", \"order\" AS \"Order\", help_text AS \"HelpText\", checkbox_field_default AS \"CheckboxFieldDefault\", select_field_multi_select AS \"SelectFieldMultiSelect\", select_field_options_sorting_order AS \"SelectFieldOptionsSortingOrder\", text_field_field_format AS \"TextFieldFieldFormat\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<CustomFieldValue> CustomFieldValues(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<CustomFieldValue>($"SELECT id AS \"Id\", custom_field_id AS \"CustomFieldId\", id2 AS \"Id2\", value AS \"Value\", \"default\" AS \"Default\" FROM uc{(IsMySql ? "_" : ".")}custom_field_values{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Department> Departments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Department>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Department2> Department2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Department2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", usage_number AS \"UsageNumber\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Document> Documents(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Document>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\", document_data AS \"DocumentData\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Document2> Document2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Document2>($"SELECT id AS \"Id\", document_metadata_name AS \"DocumentMetadataName\", document_metadata_invoice_id AS \"DocumentMetadataInvoiceId\", document_metadata_url AS \"DocumentMetadataUrl\", document_metadata_metadata_created_date AS \"DocumentMetadataMetadataCreatedDate\", document_metadata_metadata_created_by_user_id AS \"DocumentMetadataMetadataCreatedByUserId\", document_metadata_metadata_created_by_username AS \"DocumentMetadataMetadataCreatedByUsername\", document_metadata_metadata_updated_date AS \"DocumentMetadataMetadataUpdatedDate\", document_metadata_metadata_updated_by_user_id AS \"DocumentMetadataMetadataUpdatedByUserId\", document_metadata_metadata_updated_by_username AS \"DocumentMetadataMetadataUpdatedByUsername\", contents_data AS \"ContentsData\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", invoiceid AS \"Invoiceid\", document_data AS \"DocumentData\" FROM uc{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Edition> Editions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Edition>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}editions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<ElectronicAccess> ElectronicAccesses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ElectronicAccess>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<ElectronicAccessRelationship> ElectronicAccessRelationships(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ElectronicAccessRelationship>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ElectronicAccessRelationship2> ElectronicAccessRelationship2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ElectronicAccessRelationship2>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}electronic_access_relationships{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ErrorRecord> ErrorRecords(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ErrorRecord>($"SELECT id AS \"Id\", content AS \"Content\", description AS \"Description\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ErrorRecord2> ErrorRecord2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ErrorRecord2>($"SELECT id AS \"Id\", content AS \"Content\", description AS \"Description\" FROM uc{(IsMySql ? "_" : ".")}error_records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<EventLog> EventLogs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<EventLog>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<EventLog2> EventLog2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<EventLog2>($"SELECT id AS \"Id\", tenant AS \"Tenant\", user_id AS \"UserId\", ip AS \"Ip\", browser_information AS \"BrowserInformation\", timestamp AS \"Timestamp\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ExpenseClass> ExpenseClasses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ExpenseClass>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ExpenseClass2> ExpenseClass2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ExpenseClass2>($"SELECT id AS \"Id\", code AS \"Code\", external_account_number_ext AS \"ExternalAccountNumberExt\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}expense_classes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ExportConfigCredential> ExportConfigCredentials(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ExportConfigCredential>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", exportconfigid AS \"Exportconfigid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ExportConfigCredential2> ExportConfigCredential2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ExportConfigCredential2>($"SELECT id AS \"Id\", username AS \"Username\", password AS \"Password\", export_config_id AS \"ExportConfigId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Extent> Extents(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Extent>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Content\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}extents{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<Fee> Fees(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Fee>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Fee2> Fee2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Fee2>($"SELECT id AS \"Id\", amount AS \"Amount\", remaining AS \"RemainingAmount\", date_created AS \"DateCreated\", date_updated AS \"DateUpdated\", status_name AS \"StatusName\", payment_status_name AS \"PaymentStatusName\", fee_fine_type AS \"FeeFineType\", fee_fine_owner AS \"FeeFineOwner\", title AS \"Title\", call_number AS \"CallNumber\", barcode AS \"Barcode\", material_type AS \"MaterialType\", item_status_name AS \"ItemStatusName\", location AS \"Location\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", due_date AS \"DueTime\", returned_date AS \"ReturnedTime\", loan_id AS \"LoanId\", user_id AS \"UserId\", item_id AS \"ItemId\", material_type_id AS \"MaterialTypeId\", fee_type_id AS \"FeeTypeId\", owner_id AS \"OwnerId\", holding_id AS \"HoldingId\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fees{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FeeType> FeeTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FeeType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", ownerid AS \"Ownerid\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FeeType2> FeeType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FeeType2>($"SELECT id AS \"Id\", automatic AS \"Automatic\", fee_fine_type AS \"Name\", default_amount AS \"DefaultAmount\", charge_notice_id AS \"ChargeNoticeId\", action_notice_id AS \"ActionNoticeId\", owner_id AS \"OwnerId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fee_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FinanceGroup> FinanceGroups(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FinanceGroup>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FinanceGroup2> FinanceGroup2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FinanceGroup2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", name AS \"Name\", status AS \"Status\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}finance_groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FinanceGroupAcquisitionsUnit> FinanceGroupAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FinanceGroupAcquisitionsUnit>($"SELECT id AS \"Id\", finance_group_id AS \"FinanceGroupId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}finance_group_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FiscalYear> FiscalYears(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FiscalYear>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FiscalYear2> FiscalYear2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FiscalYear2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", currency AS \"Currency\", description AS \"Description\", period_start AS \"StartDate\", period_end AS \"EndDate\", series AS \"Series\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fiscal_years{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FiscalYearAcquisitionsUnit> FiscalYearAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FiscalYearAcquisitionsUnit>($"SELECT id AS \"Id\", fiscal_year_id AS \"FiscalYearId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}fiscal_year_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FixedDueDateSchedule> FixedDueDateSchedules(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FixedDueDateSchedule>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FixedDueDateSchedule2> FixedDueDateSchedule2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FixedDueDateSchedule2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedules{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FixedDueDateScheduleSchedule> FixedDueDateScheduleSchedules(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FixedDueDateScheduleSchedule>($"SELECT id AS \"Id\", fixed_due_date_schedule_id AS \"FixedDueDateScheduleId\", from AS \"From\", \"to\" AS \"To\", due AS \"Due\" FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedule_schedules{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Format> Formats(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Format>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}formats{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Fund> Funds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Fund>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", ledgerid AS \"LedgerId\", fundtypeid AS \"Fundtypeid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Fund2> Fund2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Fund2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", external_account_no AS \"ExternalAccountNo\", fund_status AS \"FundStatus\", fund_type_id AS \"FundTypeId\", ledger_id AS \"LedgerId\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}funds{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FundAcquisitionsUnit> FundAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FundAcquisitionsUnit>($"SELECT id AS \"Id\", fund_id AS \"FundId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}fund_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FundTag> FundTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FundTag>($"SELECT id AS \"Id\", fund_id AS \"FundId\", tag_id AS \"TagId\" FROM uc{(IsMySql ? "_" : ".")}fund_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FundType> FundTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FundType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<FundType2> FundType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<FundType2>($"SELECT id AS \"Id\", name AS \"Name\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}fund_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Group> Groups(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Group>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Group2> Group2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Group2>($"SELECT id AS \"Id\", \"group\" AS \"Name\", \"desc\" AS \"Description\", expiration_offset_in_days AS \"ExpirationOffsetInDays\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<GroupFundFiscalYear> GroupFundFiscalYears(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<GroupFundFiscalYear>($"SELECT id AS \"Id\", jsonb AS \"Content\", budgetid AS \"Budgetid\", groupid AS \"Groupid\", fundid AS \"Fundid\", fiscalyearid AS \"Fiscalyearid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<GroupFundFiscalYear2> GroupFundFiscalYear2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<GroupFundFiscalYear2>($"SELECT id AS \"Id\", budget_id AS \"BudgetId\", group_id AS \"GroupId\", fiscal_year_id AS \"FiscalYearId\", fund_id AS \"FundId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}group_fund_fiscal_years{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Holding> Holdings(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Holding>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", instanceid AS \"Instanceid\", permanentlocationid AS \"Permanentlocationid\", temporarylocationid AS \"Temporarylocationid\", holdingstypeid AS \"Holdingstypeid\", callnumbertypeid AS \"Callnumbertypeid\", illpolicyid AS \"Illpolicyid\", sourceid AS \"Sourceid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Holding2> Holding2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Holding2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", holding_type_id AS \"HoldingTypeId\", instance_id AS \"InstanceId\", permanent_location_id AS \"LocationId\", temporary_location_id AS \"TemporaryLocationId\", call_number_type_id AS \"CallNumberTypeId\", call_number_prefix AS \"CallNumberPrefix\", call_number AS \"CallNumber\", call_number_suffix AS \"CallNumberSuffix\", shelving_title AS \"ShelvingTitle\", acquisition_format AS \"AcquisitionFormat\", acquisition_method AS \"AcquisitionMethod\", receipt_status AS \"ReceiptStatus\", ill_policy_id AS \"IllPolicyId\", retention_policy AS \"RetentionPolicy\", digitization_policy AS \"DigitizationPolicy\", copy_number AS \"CopyNumber\", number_of_items AS \"ItemCount\", receiving_history_display_type AS \"ReceivingHistoryDisplayType\", discovery_suppress AS \"DiscoverySuppress\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", source_id AS \"SourceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holdings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingElectronicAccess> HoldingElectronicAccesses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingElectronicAccess>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}holding_electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingEntry> HoldingEntries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingEntry>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", public_display AS \"PublicDisplay\", enumeration AS \"Enumeration\", chronology AS \"Chronology\" FROM uc{(IsMySql ? "_" : ".")}holding_entries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingFormerId> HoldingFormerIds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingFormerId>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_former_ids{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingNote> HoldingNotes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingNote>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", holding_note_type_id AS \"HoldingNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}holding_notes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingNoteType> HoldingNoteTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingNoteType2> HoldingNoteType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_note_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingStatisticalCode> HoldingStatisticalCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingStatisticalCode>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}holding_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingTag> HoldingTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingTag>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingType> HoldingTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HoldingType2> HoldingType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HoldingType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}holding_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HridSetting> HridSettings(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HridSetting>($"SELECT id AS \"Id\", jsonb AS \"Content\", lock AS \"Lock\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<HridSetting2> HridSetting2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<HridSetting2>($"SELECT id AS \"Id\", instances_prefix AS \"InstancesPrefix\", instances_start_number AS \"InstancesStartNumber\", holdings_prefix AS \"HoldingsPrefix\", holdings_start_number AS \"HoldingsStartNumber\", items_prefix AS \"ItemsPrefix\", items_start_number AS \"ItemsStartNumber\", content AS \"Content\", lock AS \"Lock\" FROM uc{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Identifier> Identifiers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Identifier>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", value AS \"Content\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}identifiers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<IdType> IdTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IdType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<IdType2> IdType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IdType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}id_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<IllPolicy> IllPolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IllPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<IllPolicy2> IllPolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IllPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}ill_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<IndexStatement> IndexStatements(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IndexStatement>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Statement\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}index_statements{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<Instance> Instances(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Instance>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", instancestatusid AS \"Instancestatusid\", modeofissuanceid AS \"Modeofissuanceid\", instancetypeid AS \"Instancetypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Instance2> Instance2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Instance2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", match_key AS \"MatchKey\", source AS \"Source\", title AS \"Title\", author AS \"Author\", publication_year AS \"PublicationYear\", index_title AS \"IndexTitle\", instance_type_id AS \"InstanceTypeId\", mode_of_issuance_id AS \"IssuanceModeId\", cataloged_date AS \"CatalogedDate\", previously_held AS \"PreviouslyHeld\", staff_suppress AS \"StaffSuppress\", discovery_suppress AS \"DiscoverySuppress\", source_record_format AS \"SourceRecordFormat\", status_id AS \"StatusId\", status_updated_date AS \"StatusLastWriteTime\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instances{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceFormat> InstanceFormats(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceFormat>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceFormat2> InstanceFormat2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceFormat2>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", format_id AS \"FormatId\" FROM uc{(IsMySql ? "_" : ".")}instance_formats{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceNatureOfContentTerm> InstanceNatureOfContentTerms(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceNatureOfContentTerm>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", nature_of_content_term_id AS \"NatureOfContentTermId\" FROM uc{(IsMySql ? "_" : ".")}instance_nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceNoteType> InstanceNoteTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceNoteType2> InstanceNoteType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_note_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceRelationship> InstanceRelationships(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceRelationship>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", superinstanceid AS \"Superinstanceid\", subinstanceid AS \"Subinstanceid\", instancerelationshiptypeid AS \"Instancerelationshiptypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceRelationshipType> InstanceRelationshipTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceRelationshipType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceSourceMarc> InstanceSourceMarcs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceSourceMarc>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceStatisticalCode> InstanceStatisticalCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceStatisticalCode>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}instance_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceStatus> InstanceStatuses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceStatus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceTag> InstanceTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceTag>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceType> InstanceTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InstanceType2> InstanceType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InstanceType2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}instance_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Institution> Institutions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Institution>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Institution2> Institution2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Institution2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}institutions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Interface> Interfaces(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Interface>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Interface2> Interface2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Interface2>($"SELECT id AS \"Id\", name AS \"Name\", uri AS \"Uri\", notes AS \"Notes\", available AS \"Available\", delivery_method AS \"DeliveryMethod\", statistics_format AS \"StatisticsFormat\", locally_stored AS \"LocallyStored\", online_location AS \"OnlineLocation\", statistics_notes AS \"StatisticsNotes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InterfaceCredential> InterfaceCredentials(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InterfaceCredential>($"SELECT id AS \"Id\", jsonb AS \"Content\", interfaceid AS \"Interfaceid\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InterfaceCredential2> InterfaceCredential2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InterfaceCredential2>($"SELECT id AS \"Id\", username AS \"Username\", password AS \"Password\", interface_id AS \"InterfaceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InterfaceType> InterfaceTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InterfaceType>($"SELECT id AS \"Id\", interface_id AS \"InterfaceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}interface_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Invoice> Invoices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Invoice>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Invoice2> Invoice2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Invoice2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", adjustments_total AS \"AdjustmentsTotal\", approved_by_id AS \"ApprovedById\", approval_date AS \"ApprovalDate\", batch_group_id AS \"BatchGroupId\", bill_to_id AS \"BillToId\", chk_subscription_overlap AS \"ChkSubscriptionOverlap\", currency AS \"Currency\", enclosure_needed AS \"EnclosureNeeded\", exchange_rate AS \"ExchangeRate\", export_to_accounting AS \"ExportToAccounting\", folio_invoice_no AS \"Number\", invoice_date AS \"InvoiceDate\", lock_total AS \"LockTotal\", note AS \"Note\", payment_due AS \"PaymentDue\", payment_terms AS \"PaymentTerms\", payment_method AS \"PaymentMethod\", status AS \"Status\", source AS \"Source\", sub_total AS \"SubTotal\", total AS \"Total\", vendor_invoice_no AS \"VendorInvoiceNo\", disbursement_number AS \"DisbursementNumber\", voucher_number AS \"VoucherNumber\", payment_id AS \"PaymentId\", disbursement_date AS \"DisbursementDate\", vendor_id AS \"VendorId\", manual_payment AS \"ManualPayment\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceAcquisitionsUnit> InvoiceAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceAcquisitionsUnit>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}invoice_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceAdjustment> InvoiceAdjustments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceAdjustment>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", id2 AS \"Id2\", adjustment_id AS \"AdjustmentId\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", prorate AS \"Prorate\", relation_to_total AS \"RelationToTotal\", type AS \"Type\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_adjustments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceAdjustmentFund> InvoiceAdjustmentFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceAdjustmentFund>($"SELECT id AS \"Id\", invoice_adjustment_id AS \"InvoiceAdjustmentId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItem> InvoiceItems(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItem2> InvoiceItem2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItem2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", account_number AS \"AccountNumber\", adjustments_total AS \"AdjustmentsTotal\", comment AS \"Comment\", description AS \"Description\", invoice_id AS \"InvoiceId\", invoice_line_number AS \"Number\", invoice_line_status AS \"InvoiceLineStatus\", po_line_id AS \"OrderItemId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", quantity AS \"Quantity\", release_encumbrance AS \"ReleaseEncumbrance\", subscription_info AS \"SubscriptionInfo\", subscription_start AS \"SubscriptionStart\", subscription_end AS \"SubscriptionEnd\", sub_total AS \"SubTotal\", total AS \"Total\", vendor_ref_no AS \"VendorRefNo\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_items{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItemAdjustment> InvoiceItemAdjustments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItemAdjustment>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", id2 AS \"Id2\", adjustment_id AS \"AdjustmentId\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", prorate AS \"Prorate\", relation_to_total AS \"RelationToTotal\", type AS \"Type\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItemAdjustmentFund>($"SELECT id AS \"Id\", invoice_item_adjustment_id AS \"InvoiceItemAdjustmentId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItemFund> InvoiceItemFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItemFund>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceItemTag> InvoiceItemTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceItemTag>($"SELECT id AS \"Id\", invoice_item_id AS \"InvoiceItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_item_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceOrderNumber> InvoiceOrderNumbers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceOrderNumber>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_order_numbers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceTag> InvoiceTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceTag>($"SELECT id AS \"Id\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceTransactionSummary> InvoiceTransactionSummaries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceTransactionSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<InvoiceTransactionSummary2> InvoiceTransactionSummary2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<InvoiceTransactionSummary2>($"SELECT id AS \"Id\", num_pending_payments AS \"NumPendingPayments\", num_payments_credits AS \"NumPaymentsCredits\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<IssuanceMode> IssuanceModes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<IssuanceMode>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}mode_of_issuances{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Item> Items(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Item>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", holdingsrecordid AS \"Holdingsrecordid\", permanentloantypeid AS \"Permanentloantypeid\", temporaryloantypeid AS \"Temporaryloantypeid\", materialtypeid AS \"Materialtypeid\", permanentlocationid AS \"Permanentlocationid\", temporarylocationid AS \"Temporarylocationid\", effectivelocationid AS \"Effectivelocationid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Item2> Item2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Item2>($"SELECT id AS \"Id\", hrid AS \"ShortId\", holding_id AS \"HoldingId\", discovery_suppress AS \"DiscoverySuppress\", accession_number AS \"AccessionNumber\", barcode AS \"Barcode\", call_number AS \"CallNumber\", call_number_prefix AS \"CallNumberPrefix\", call_number_suffix AS \"CallNumberSuffix\", call_number_type_id AS \"CallNumberTypeId\", effective_call_number AS \"EffectiveCallNumber\", effective_call_number_prefix AS \"EffectiveCallNumberPrefix\", effective_call_number_suffix AS \"EffectiveCallNumberSuffix\", effective_call_number_type_id AS \"EffectiveCallNumberTypeId\", volume AS \"Volume\", enumeration AS \"Enumeration\", chronology AS \"Chronology\", item_identifier AS \"ItemIdentifier\", copy_number AS \"CopyNumber\", number_of_pieces AS \"PiecesCount\", description_of_pieces AS \"PiecesDescription\", number_of_missing_pieces AS \"MissingPiecesCount\", missing_pieces AS \"MissingPiecesDescription\", missing_pieces_date AS \"MissingPiecesTime\", item_damaged_status_id AS \"DamagedStatusId\", item_damaged_status_date AS \"DamagedStatusTime\", status_name AS \"StatusName\", status_date AS \"StatusDate\", material_type_id AS \"MaterialTypeId\", permanent_loan_type_id AS \"PermanentLoanTypeId\", temporary_loan_type_id AS \"TemporaryLoanTypeId\", permanent_location_id AS \"PermanentLocationId\", temporary_location_id AS \"TemporaryLocationId\", effective_location_id AS \"EffectiveLocationId\", in_transit_destination_service_point_id AS \"InTransitDestinationServicePointId\", order_item_id AS \"OrderItemId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", last_check_in_date_time AS \"LastCheckInDateTime\", last_check_in_service_point_id AS \"LastCheckInServicePointId\", last_check_in_staff_member_id AS \"LastCheckInStaffMemberId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}items{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ItemDamagedStatus> ItemDamagedStatuses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemDamagedStatus>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ItemDamagedStatus2> ItemDamagedStatus2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemDamagedStatus2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_damaged_statuses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ItemElectronicAccess> ItemElectronicAccesses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemElectronicAccess>($"SELECT id AS \"Id\", item_id AS \"ItemId\", uri AS \"Uri\", link_text AS \"LinkText\", materials_specification AS \"MaterialsSpecification\", public_note AS \"PublicNote\", relationship_id AS \"RelationshipId\" FROM uc{(IsMySql ? "_" : ".")}item_electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<ItemFormerId> ItemFormerIds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemFormerId>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_former_ids{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<ItemNote> ItemNotes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemNote>($"SELECT id AS \"Id\", item_id AS \"ItemId\", item_note_type_id AS \"ItemNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}item_notes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<ItemNoteType> ItemNoteTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemNoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ItemNoteType2> ItemNoteType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemNoteType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_note_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ItemStatisticalCode> ItemStatisticalCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemStatisticalCode>($"SELECT id AS \"Id\", item_id AS \"ItemId\", statistical_code_id AS \"StatisticalCodeId\" FROM uc{(IsMySql ? "_" : ".")}item_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<ItemTag> ItemTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemTag>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<ItemYearCaption> ItemYearCaptions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ItemYearCaption>($"SELECT id AS \"Id\", item_id AS \"ItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}item_year_caption{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, item_id"}" : "")}", param, skip, take);
        public IEnumerable<JobExecution> JobExecutions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JobExecution>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JobExecution2> JobExecution2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JobExecution2>($"SELECT id AS \"Id\", hr_id AS \"HrId\", parent_job_id AS \"ParentJobId\", subordination_type AS \"SubordinationType\", job_profile_info_name AS \"JobProfileInfoName\", job_profile_info_data_type AS \"JobProfileInfoDataType\", job_profile_snapshot_wrapper_profile_id AS \"JobProfileSnapshotWrapperProfileId\", job_profile_snapshot_wrapper_content_type AS \"JobProfileSnapshotWrapperContentType\", job_profile_snapshot_wrapper_react_to AS \"JobProfileSnapshotWrapperReactTo\", job_profile_snapshot_wrapper_order AS \"JobProfileSnapshotWrapperOrder\", source_path AS \"SourcePath\", file_name AS \"FileName\", run_by_first_name AS \"RunByFirstName\", run_by_last_name AS \"RunByLastName\", progress_job_execution_id AS \"ProgressJobExecutionId\", progress_current AS \"ProgressCurrent\", progress_total AS \"ProgressTotal\", started_date AS \"StartedDate\", completed_date AS \"CompletedDate\", status AS \"Status\", ui_status AS \"UiStatus\", error_status AS \"ErrorStatus\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JobExecutionProgress> JobExecutionProgresses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JobExecutionProgress>($"SELECT id AS \"Id\", jsonb AS \"Content\", jobexecutionid AS \"Jobexecutionid\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JobExecutionSourceChunk> JobExecutionSourceChunks(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JobExecutionSourceChunk>($"SELECT id AS \"Id\", jsonb AS \"Content\", jobexecutionid AS \"Jobexecutionid\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JobExecutionSourceChunk2> JobExecutionSourceChunk2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JobExecutionSourceChunk2>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", last AS \"Last\", state AS \"State\", chunk_size AS \"ChunkSize\", processed_amount AS \"ProcessedAmount\", completed_date AS \"CompletedDate\", error AS \"Error\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JournalRecord> JournalRecords(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JournalRecord>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", source_id AS \"SourceId\", entity_type AS \"EntityType\", entity_id AS \"EntityId\", entity_hrid AS \"EntityHrid\", action_type AS \"ActionType\", action_status AS \"ActionStatus\", action_date AS \"ActionDate\", source_record_order AS \"SourceRecordOrder\", error AS \"Error\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<JournalRecord2> JournalRecord2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<JournalRecord2>($"SELECT id AS \"Id\", job_execution_id AS \"JobExecutionId\", source_id AS \"SourceId\", entity_type AS \"EntityType\", entity_id AS \"EntityId\", entity_hrid AS \"EntityHrid\", action_type AS \"ActionType\", action_status AS \"ActionStatus\", action_date AS \"ActionDate\", source_record_order AS \"SourceRecordOrder\", error AS \"Error\" FROM uc{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Language> Languages(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Language>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}languages{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<Ledger> Ledgers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Ledger>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fiscalyearoneid AS \"Fiscalyearoneid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Ledger2> Ledger2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Ledger2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", fiscal_year_one_id AS \"FiscalYearOneId\", ledger_status AS \"LedgerStatus\", allocated AS \"Allocated\", available AS \"Available\", net_transfers AS \"NetTransfers\", unavailable AS \"Unavailable\", currency AS \"Currency\", restrict_encumbrance AS \"RestrictEncumbrance\", restrict_expenditures AS \"RestrictExpenditures\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}ledgers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LedgerAcquisitionsUnit> LedgerAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LedgerAcquisitionsUnit>($"SELECT id AS \"Id\", ledger_id AS \"LedgerId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}ledger_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Library> Libraries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Library>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", campusid AS \"Campusid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Library2> Library2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Library2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", campus_id AS \"CampusId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}libraries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Loan> Loans(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Loan>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Loan2> Loan2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Loan2>($"SELECT id AS \"Id\", user_id AS \"UserId\", proxy_user_id AS \"ProxyUserId\", item_id AS \"ItemId\", item_effective_location_at_check_out_id AS \"ItemEffectiveLocationAtCheckOutId\", status_name AS \"StatusName\", loan_date AS \"LoanTime\", due_date AS \"DueTime\", return_date AS \"ReturnTime\", system_return_date AS \"SystemReturnTime\", action AS \"Action\", action_comment AS \"ActionComment\", item_status AS \"ItemStatus\", renewal_count AS \"RenewalCount\", loan_policy_id AS \"LoanPolicyId\", checkout_service_point_id AS \"CheckoutServicePointId\", checkin_service_point_id AS \"CheckinServicePointId\", group_id AS \"GroupId\", due_date_changed_by_recall AS \"DueDateChangedByRecall\", declared_lost_date AS \"DeclaredLostDate\", claimed_returned_date AS \"ClaimedReturnedDate\", overdue_fine_policy_id AS \"OverdueFinePolicyId\", lost_item_policy_id AS \"LostItemPolicyId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", aged_to_lost_delayed_billing_lost_item_has_been_billed AS \"AgedToLostDelayedBillingLostItemHasBeenBilled\", aged_to_lost_delayed_billing_date_lost_item_should_be_billed AS \"AgedToLostDelayedBillingDateLostItemShouldBeBilled\", aged_to_lost_delayed_billing_aged_to_lost_date AS \"AgedToLostDelayedBillingAgedToLostDate\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loans{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LoanPolicy> LoanPolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LoanPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", loanspolicy_fixedduedatescheduleid AS \"LoanspolicyFixedduedatescheduleid\", renewalspolicy_alternatefixedduedatescheduleid AS \"RenewalspolicyAlternatefixedduedatescheduleid\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LoanPolicy2> LoanPolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LoanPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", loanable AS \"Loanable\", loans_policy_profile_id AS \"LoansPolicyProfileId\", loans_policy_period_duration AS \"LoansPolicyPeriodDuration\", loans_policy_period_interval_id AS \"LoansPolicyPeriodInterval\", loans_policy_closed_library_due_date_management_id AS \"LoansPolicyClosedLibraryDueDateManagementId\", loans_policy_grace_period_duration AS \"LoansPolicyGracePeriodDuration\", loans_policy_grace_period_interval_id AS \"LoansPolicyGracePeriodInterval\", loans_policy_opening_time_offset_duration AS \"LoansPolicyOpeningTimeOffsetDuration\", loans_policy_opening_time_offset_interval_id AS \"LoansPolicyOpeningTimeOffsetInterval\", loans_policy_fixed_due_date_schedule_id AS \"LoansPolicyFixedDueDateScheduleId\", loans_policy_item_limit AS \"LoansPolicyItemLimit\", renewable AS \"Renewable\", renewals_policy_unlimited AS \"RenewalsPolicyUnlimited\", renewals_policy_number_allowed AS \"RenewalsPolicyNumberAllowed\", renewals_policy_renew_from_id AS \"RenewalsPolicyRenewFromId\", renewals_policy_different_period AS \"RenewalsPolicyDifferentPeriod\", renewals_policy_period_duration AS \"RenewalsPolicyPeriodDuration\", renewals_policy_period_interval_id AS \"RenewalsPolicyPeriodInterval\", renewals_policy_alternate_fixed_due_date_schedule_id AS \"RenewalsPolicyAlternateFixedDueDateScheduleId\", recalls_alternate_grace_period_duration AS \"RecallsAlternateGracePeriodDuration\", recalls_alternate_grace_period_interval_id AS \"RecallsAlternateGracePeriodInterval\", recalls_minimum_guaranteed_loan_period_duration AS \"RecallsMinimumGuaranteedLoanPeriodDuration\", recalls_minimum_guaranteed_loan_period_interval_id AS \"RecallsMinimumGuaranteedLoanPeriodInterval\", recalls_recall_return_interval_duration AS \"RecallsRecallReturnIntervalDuration\", recalls_recall_return_interval_interval_id AS \"RecallsRecallReturnIntervalInterval\", holds_alternate_checkout_loan_period_duration AS \"HoldsAlternateCheckoutLoanPeriodDuration\", holds_alternate_checkout_loan_period_interval_id AS \"HoldsAlternateCheckoutLoanPeriodInterval\", holds_renew_items_with_request AS \"HoldsRenewItemsWithRequest\", holds_alternate_renewal_loan_period_duration AS \"HoldsAlternateRenewalLoanPeriodDuration\", holds_alternate_renewal_loan_period_interval_id AS \"HoldsAlternateRenewalLoanPeriodInterval\", pages_alternate_checkout_loan_period_duration AS \"PagesAlternateCheckoutLoanPeriodDuration\", pages_alternate_checkout_loan_period_interval_id AS \"PagesAlternateCheckoutLoanPeriodInterval\", pages_renew_items_with_request AS \"PagesRenewItemsWithRequest\", pages_alternate_renewal_loan_period_duration AS \"PagesAlternateRenewalLoanPeriodDuration\", pages_alternate_renewal_loan_period_interval_id AS \"PagesAlternateRenewalLoanPeriodInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loan_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LoanType> LoanTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LoanType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LoanType2> LoanType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LoanType2>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}loan_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Location> Locations(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Location>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", institutionid AS \"Institutionid\", campusid AS \"Campusid\", libraryid AS \"Libraryid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Location2> Location2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Location2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", discovery_display_name AS \"DiscoveryDisplayName\", is_active AS \"IsActive\", institution_id AS \"InstitutionId\", campus_id AS \"CampusId\", library_id AS \"LibraryId\", primary_service_point_id AS \"PrimaryServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}locations{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LocationServicePoint> LocationServicePoints(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LocationServicePoint>($"SELECT id AS \"Id\", location_id AS \"LocationId\", service_point_id AS \"ServicePointId\" FROM uc{(IsMySql ? "_" : ".")}location_service_points{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LocationSetting> LocationSettings(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LocationSetting>($"SELECT id AS \"Id\", location_id AS \"LocationId\", settings_id AS \"SettingsId\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}location_settings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Login> Logins(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Login>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Login2> Login2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Login2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hash AS \"Hash\", salt AS \"Salt\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}logins{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LostItemFeePolicy> LostItemFeePolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LostItemFeePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<LostItemFeePolicy2> LostItemFeePolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<LostItemFeePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", item_aged_lost_overdue_duration AS \"ItemAgedLostOverdueDuration\", item_aged_lost_overdue_interval_id AS \"ItemAgedLostOverdueInterval\", patron_billed_after_aged_lost_duration AS \"PatronBilledAfterAgedLostDuration\", patron_billed_after_aged_lost_interval_id AS \"PatronBilledAfterAgedLostInterval\", charge_amount_item_charge_type AS \"ChargeAmountItemChargeType\", charge_amount_item_amount AS \"ChargeAmountItemAmount\", lost_item_processing_fee AS \"LostItemProcessingFee\", charge_amount_item_patron AS \"ChargeAmountItemPatron\", charge_amount_item_system AS \"ChargeAmountItemSystem\", lost_item_charge_fee_fine_duration AS \"LostItemChargeFeeFineDuration\", lost_item_charge_fee_fine_interval_id AS \"LostItemChargeFeeFineInterval\", returned_lost_item_processing_fee AS \"ReturnedLostItemProcessingFee\", replaced_lost_item_processing_fee AS \"ReplacedLostItemProcessingFee\", replacement_processing_fee AS \"ReplacementProcessingFee\", replacement_allowed AS \"ReplacementAllowed\", lost_item_returned AS \"LostItemReturned\", fees_fines_shall_refunded_duration AS \"FeesFinesShallRefundedDuration\", fees_fines_shall_refunded_interval_id AS \"FeesFinesShallRefundedInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}lost_item_fee_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<MappingRule> MappingRules(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<MappingRule>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<MarcRecord> MarcRecords(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<MarcRecord>($"SELECT id AS \"Id\", content AS \"Content\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<MarcRecord2> MarcRecord2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<MarcRecord2>($"SELECT id AS \"Id\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}marc_records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<MaterialType> MaterialTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<MaterialType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<MaterialType2> MaterialType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<MaterialType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}material_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ModeOfIssuance> ModeOfIssuances(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ModeOfIssuance>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<NatureOfContentTerm> NatureOfContentTerms(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<NatureOfContentTerm>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<NatureOfContentTerm2> NatureOfContentTerm2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<NatureOfContentTerm2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Note> Notes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Note>($"SELECT id AS \"Id\", jsonb AS \"Content\", temporary_type_id AS \"TemporaryTypeId\" FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_data{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Note2> Note2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Note2>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", instance_note_type_id AS \"InstanceNoteTypeId\", note AS \"Note\", staff_only AS \"StaffOnly\" FROM uc{(IsMySql ? "_" : ".")}notes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<Note3> Note3s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Note3>($"SELECT id AS \"Id\", type_id AS \"TypeId\", type AS \"Type\", domain AS \"Domain\", title AS \"Title\", content2 AS \"Content2\", status AS \"Status\", creator_last_name AS \"CreatorLastName\", creator_first_name AS \"CreatorFirstName\", creator_middle_name AS \"CreatorMiddleName\", updater_last_name AS \"UpdaterLastName\", updater_first_name AS \"UpdaterFirstName\", updater_middle_name AS \"UpdaterMiddleName\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\", temporary_type_id AS \"TemporaryTypeId\" FROM uc{(IsMySql ? "_" : ".")}notes2{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<NoteLink> NoteLinks(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<NoteLink>($"SELECT id AS \"Id\", note_id AS \"NoteId\", id2 AS \"Id2\", type AS \"Type\" FROM uc{(IsMySql ? "_" : ".")}note_links{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<NoteType> NoteTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<NoteType>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<NoteType2> NoteType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<NoteType2>($"SELECT id AS \"Id\", name AS \"Name\", usage_note_total AS \"UsageNoteTotal\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}note_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Order> Orders(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Order>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Order2> Order2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Order2>($"SELECT id AS \"Id\", approved AS \"Approved\", approved_by_id AS \"ApprovedById\", approval_date AS \"ApprovalDate\", assigned_to_id AS \"AssignedToId\", bill_to_id AS \"BillToId\", close_reason_reason AS \"CloseReasonReason\", close_reason_note AS \"CloseReasonNote\", date_ordered AS \"OrderDate\", manual_po AS \"Manual\", po_number AS \"Number\", po_number_prefix AS \"NumberPrefix\", po_number_suffix AS \"NumberSuffix\", order_type AS \"OrderType\", re_encumber AS \"Reencumber\", ongoing_interval AS \"OngoingInterval\", ongoing_is_subscription AS \"OngoingIsSubscription\", ongoing_manual_renewal AS \"OngoingManualRenewal\", ongoing_notes AS \"OngoingNotes\", ongoing_review_period AS \"OngoingReviewPeriod\", ongoing_renewal_date AS \"OngoingRenewalDate\", ongoing_review_date AS \"OngoingReviewDate\", ship_to_id AS \"ShipToId\", template_id AS \"TemplateId\", vendor_id AS \"VendorId\", workflow_status AS \"WorkflowStatus\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}orders{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderAcquisitionsUnit> OrderAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderAcquisitionsUnit>($"SELECT id AS \"Id\", order_id AS \"OrderId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}order_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderInvoice> OrderInvoices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderInvoice>($"SELECT id AS \"Id\", jsonb AS \"Content\", purchaseorderid AS \"Purchaseorderid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderInvoice2> OrderInvoice2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderInvoice2>($"SELECT id AS \"Id\", order_id AS \"OrderId\", invoice_id AS \"InvoiceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_invoices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItem> OrderItems(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", purchaseorderid AS \"Purchaseorderid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItem2> OrderItem2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItem2>($"SELECT id AS \"Id\", edition AS \"Edition\", checkin_items AS \"CheckinItems\", agreement_id AS \"AgreementId\", acquisition_method AS \"AcquisitionMethod\", cancellation_restriction AS \"CancellationRestriction\", cancellation_restriction_note AS \"CancellationRestrictionNote\", collection AS \"Collection\", cost_list_unit_price AS \"PhysicalUnitListPrice\", cost_list_unit_price_electronic AS \"ElectronicUnitListPrice\", cost_currency AS \"Currency\", cost_additional_cost AS \"AdditionalCost\", cost_discount AS \"Discount\", cost_discount_type AS \"DiscountType\", cost_quantity_physical AS \"PhysicalQuantity\", cost_quantity_electronic AS \"ElectronicQuantity\", cost_po_line_estimated_price AS \"EstimatedPrice\", description AS \"Description\", details_receiving_note AS \"ReceivingNote\", details_subscription_from AS \"SubscriptionFrom\", details_subscription_interval AS \"SubscriptionInterval\", details_subscription_to AS \"SubscriptionTo\", donor AS \"Donor\", eresource_activated AS \"EresourceActivated\", eresource_activation_due AS \"EresourceActivationDue\", eresource_create_inventory AS \"EresourceCreateInventory\", eresource_trial AS \"EresourceTrial\", eresource_expected_activation AS \"EresourceExpectedActivationDate\", eresource_user_limit AS \"EresourceUserLimit\", eresource_access_provider_id AS \"EresourceAccessProviderId\", eresource_license_code AS \"EresourceLicenseCode\", eresource_license_description AS \"EresourceLicenseDescription\", eresource_license_reference AS \"EresourceLicenseReference\", eresource_material_type_id AS \"EresourceMaterialTypeId\", eresource_resource_url AS \"EresourceResourceUrl\", instance_id AS \"InstanceId\", is_package AS \"IsPackage\", order_format AS \"OrderFormat\", package_po_line_id AS \"PackageOrderItemId\", payment_status AS \"PaymentStatus\", physical_create_inventory AS \"PhysicalCreateInventory\", physical_material_type_id AS \"PhysicalMaterialTypeId\", physical_material_supplier_id AS \"PhysicalMaterialSupplierId\", physical_expected_receipt_date AS \"PhysicalExpectedReceiptDate\", physical_receipt_due AS \"PhysicalReceiptDue\", po_line_description AS \"Description2\", po_line_number AS \"Number\", publication_year AS \"PublicationYear\", publisher AS \"Publisher\", order_id AS \"OrderId\", receipt_date AS \"ReceiptDate\", receipt_status AS \"ReceiptStatus\", requester AS \"Requester\", rush AS \"Rush\", selector AS \"Selector\", source AS \"Source\", title_or_package AS \"TitleOrPackage\", vendor_detail_instructions AS \"VendorInstructions\", vendor_detail_note_from_vendor AS \"VendorNote\", vendor_detail_ref_number AS \"VendorReferenceNumber\", vendor_detail_ref_number_type AS \"VendorReferenceNumberType\", vendor_detail_vendor_account AS \"VendorAccount\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_items{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemAlert> OrderItemAlerts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemAlert>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", alert_id AS \"AlertId\" FROM uc{(IsMySql ? "_" : ".")}order_item_alerts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemClaim> OrderItemClaims(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemClaim>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", claimed AS \"Claimed\", sent AS \"Sent\", grace AS \"Grace\" FROM uc{(IsMySql ? "_" : ".")}order_item_claims{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemContributor> OrderItemContributors(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemContributor>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", contributor AS \"Contributor\", contributor_name_type_id AS \"ContributorNameTypeId\" FROM uc{(IsMySql ? "_" : ".")}order_item_contributors{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemFund> OrderItemFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemFund>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", expense_class_id AS \"ExpenseClassId\", distribution_type AS \"DistributionType\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}order_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemLocation2> OrderItemLocation2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemLocation2>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", location_id AS \"LocationId\", quantity AS \"Quantity\", quantity_electronic AS \"ElectronicQuantity\", quantity_physical AS \"PhysicalQuantity\" FROM uc{(IsMySql ? "_" : ".")}order_item_locations{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemProductId> OrderItemProductIds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemProductId>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", qualifier AS \"Qualifier\" FROM uc{(IsMySql ? "_" : ".")}order_item_product_ids{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemReportingCode> OrderItemReportingCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemReportingCode>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", reporting_code_id AS \"ReportingCodeId\" FROM uc{(IsMySql ? "_" : ".")}order_item_reporting_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemTag> OrderItemTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemTag>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_item_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderItemVolume> OrderItemVolumes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderItemVolume>($"SELECT id AS \"Id\", order_item_id AS \"OrderItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_item_volumes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderNote> OrderNotes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderNote>($"SELECT id AS \"Id\", order_id AS \"OrderId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_notes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderTag> OrderTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderTag>($"SELECT id AS \"Id\", order_id AS \"OrderId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderTemplate> OrderTemplates(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderTemplate>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderTemplate2> OrderTemplate2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderTemplate2>($"SELECT id AS \"Id\", template_name AS \"TemplateName\", template_code AS \"TemplateCode\", template_description AS \"TemplateDescription\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderTransactionSummary> OrderTransactionSummaries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderTransactionSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrderTransactionSummary2> OrderTransactionSummary2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrderTransactionSummary2>($"SELECT id AS \"Id\", num_transactions AS \"NumTransactions\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Organization> Organizations(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Organization>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Organization2> Organization2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Organization2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", description AS \"Description\", export_to_accounting AS \"ExportToAccounting\", status AS \"Status\", language AS \"Language\", erp_code AS \"ErpCode\", payment_method AS \"PaymentMethod\", access_provider AS \"AccessProvider\", governmental AS \"Governmental\", licensor AS \"Licensor\", material_supplier AS \"MaterialSupplier\", claiming_interval AS \"ClaimingInterval\", discount_percent AS \"DiscountPercent\", expected_activation_interval AS \"ExpectedActivationInterval\", expected_invoice_interval AS \"ExpectedInvoiceInterval\", renewal_activation_interval AS \"RenewalActivationInterval\", subscription_interval AS \"SubscriptionInterval\", expected_receipt_interval AS \"ExpectedReceiptInterval\", tax_id AS \"TaxId\", liable_for_vat AS \"LiableForVat\", tax_percentage AS \"TaxPercentage\", edi_vendor_edi_code AS \"EdiVendorEdiCode\", edi_vendor_edi_type AS \"EdiVendorEdiType\", edi_lib_edi_code AS \"EdiLibEdiCode\", edi_lib_edi_type AS \"EdiLibEdiType\", edi_prorate_tax AS \"EdiProrateTax\", edi_prorate_fees AS \"EdiProrateFees\", edi_naming_convention AS \"EdiNamingConvention\", edi_send_acct_num AS \"EdiSendAcctNum\", edi_support_order AS \"EdiSupportOrder\", edi_support_invoice AS \"EdiSupportInvoice\", edi_notes AS \"EdiNotes\", edi_ftp_ftp_format AS \"EdiFtpFtpFormat\", edi_ftp_server_address AS \"EdiFtpServerAddress\", edi_ftp_username AS \"EdiFtpUsername\", edi_ftp_password AS \"EdiFtpPassword\", edi_ftp_ftp_mode AS \"EdiFtpFtpMode\", edi_ftp_ftp_conn_mode AS \"EdiFtpFtpConnMode\", edi_ftp_ftp_port AS \"EdiFtpFtpPort\", edi_ftp_order_directory AS \"EdiFtpOrderDirectory\", edi_ftp_invoice_directory AS \"EdiFtpInvoiceDirectory\", edi_ftp_notes AS \"EdiFtpNotes\", edi_job_schedule_edi AS \"EdiJobScheduleEdi\", edi_job_scheduling_date AS \"EdiJobSchedulingDate\", edi_job_time AS \"EdiJobTime\", edi_job_is_monday AS \"EdiJobIsMonday\", edi_job_is_tuesday AS \"EdiJobIsTuesday\", edi_job_is_wednesday AS \"EdiJobIsWednesday\", edi_job_is_thursday AS \"EdiJobIsThursday\", edi_job_is_friday AS \"EdiJobIsFriday\", edi_job_is_saturday AS \"EdiJobIsSaturday\", edi_job_is_sunday AS \"EdiJobIsSunday\", edi_job_send_to_emails AS \"EdiJobSendToEmails\", edi_job_notify_all_edi AS \"EdiJobNotifyAllEdi\", edi_job_notify_invoice_only AS \"EdiJobNotifyInvoiceOnly\", edi_job_notify_error_only AS \"EdiJobNotifyErrorOnly\", edi_job_scheduling_notes AS \"EdiJobSchedulingNotes\", is_vendor AS \"IsVendor\", san_code AS \"SanCode\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAccount> OrganizationAccounts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAccount>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", name AS \"Name\", account_no AS \"AccountNo\", description AS \"Description\", app_system_no AS \"AppSystemNo\", payment_method AS \"PaymentMethod\", account_status AS \"AccountStatus\", contact_info AS \"ContactInfo\", library_code AS \"LibraryCode\", library_edi_code AS \"LibraryEdiCode\", notes AS \"Notes\" FROM uc{(IsMySql ? "_" : ".")}organization_accounts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAccountAcquisitionsUnit> OrganizationAccountAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAccountAcquisitionsUnit>($"SELECT id AS \"Id\", organization_account_id AS \"OrganizationAccountId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}organization_account_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAcquisitionsUnit> OrganizationAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAcquisitionsUnit>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}organization_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAddress> OrganizationAddresses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAddress>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", state_region AS \"StateRegion\", zip_code AS \"ZipCode\", country AS \"Country\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_addresses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAddressCategory> OrganizationAddressCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAddressCategory>($"SELECT id AS \"Id\", organization_address_id AS \"OrganizationAddressId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_address_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAgreement> OrganizationAgreements(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAgreement>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", name AS \"Name\", discount AS \"Discount\", reference_url AS \"ReferenceUrl\", notes AS \"Notes\" FROM uc{(IsMySql ? "_" : ".")}organization_agreements{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationAlias> OrganizationAliases(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationAlias>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", value AS \"Value\", description AS \"Description\" FROM uc{(IsMySql ? "_" : ".")}organization_aliases{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationChangelog> OrganizationChangelogs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationChangelog>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", description AS \"Description\", timestamp AS \"Timestamp\" FROM uc{(IsMySql ? "_" : ".")}organization_changelogs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationContact> OrganizationContacts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationContact>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", contact_id AS \"ContactId\" FROM uc{(IsMySql ? "_" : ".")}organization_contacts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationEmail> OrganizationEmails(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationEmail>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_emails{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationEmailCategory> OrganizationEmailCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationEmailCategory>($"SELECT id AS \"Id\", organization_email_id AS \"OrganizationEmailId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_email_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationInterface> OrganizationInterfaces(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationInterface>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", interface_id AS \"InterfaceId\" FROM uc{(IsMySql ? "_" : ".")}organization_interfaces{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationPhoneNumber> OrganizationPhoneNumbers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationPhoneNumber>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", phone_number AS \"PhoneNumber\", type AS \"Type\", is_primary AS \"IsPrimary\", language AS \"Language\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_phone_numbers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationPhoneNumberCategory> OrganizationPhoneNumberCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationPhoneNumberCategory>($"SELECT id AS \"Id\", organization_phone_number_id AS \"OrganizationPhoneNumberId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_phone_number_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationTag> OrganizationTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationTag>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}organization_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationUrl> OrganizationUrls(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationUrl>($"SELECT id AS \"Id\", organization_id AS \"OrganizationId\", id2 AS \"Id2\", value AS \"Value\", description AS \"Description\", language AS \"Language\", is_primary AS \"IsPrimary\", notes AS \"Notes\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\" FROM uc{(IsMySql ? "_" : ".")}organization_urls{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OrganizationUrlCategory> OrganizationUrlCategories(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OrganizationUrlCategory>($"SELECT id AS \"Id\", organization_url_id AS \"OrganizationUrlId\", category_id AS \"CategoryId\" FROM uc{(IsMySql ? "_" : ".")}organization_url_categories{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OverdueFinePolicy> OverdueFinePolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OverdueFinePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<OverdueFinePolicy2> OverdueFinePolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<OverdueFinePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", overdue_fine_quantity AS \"OverdueFineQuantity\", overdue_fine_interval_id AS \"OverdueFineInterval\", count_closed AS \"CountClosed\", max_overdue_fine AS \"MaxOverdueFine\", forgive_overdue_fine AS \"ForgiveOverdueFine\", overdue_recall_fine_quantity AS \"OverdueRecallFineQuantity\", overdue_recall_fine_interval_id AS \"OverdueRecallFineInterval\", grace_period_recall AS \"GracePeriodRecall\", max_overdue_recall_fine AS \"MaxOverdueRecallFine\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}overdue_fine_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Owner> Owners(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Owner>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Owner2> Owner2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Owner2>($"SELECT id AS \"Id\", owner AS \"Name\", \"desc\" AS \"Desc\", default_charge_notice_id AS \"DefaultChargeNoticeId\", default_action_notice_id AS \"DefaultActionNoticeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronActionSession> PatronActionSessions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronActionSession>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronActionSession2> PatronActionSession2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronActionSession2>($"SELECT id AS \"Id\", patron_id AS \"PatronId\", loan_id AS \"LoanId\", action_type AS \"ActionType\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}patron_action_sessions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronNoticePolicy> PatronNoticePolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronNoticePolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronNoticePolicy2> PatronNoticePolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronNoticePolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", active AS \"Active\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronNoticePolicyFeeFineNotice> PatronNoticePolicyFeeFineNotices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronNoticePolicyFeeFineNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_fee_fine_notices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronNoticePolicyLoanNotice> PatronNoticePolicyLoanNotices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronNoticePolicyLoanNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_loan_notices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PatronNoticePolicyRequestNotice> PatronNoticePolicyRequestNotices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PatronNoticePolicyRequestNotice>($"SELECT id AS \"Id\", patron_notice_policy_id AS \"PatronNoticePolicyId\", name AS \"Name\", template_id AS \"TemplateId\", template_name AS \"TemplateName\", format AS \"Format\", frequency AS \"Frequency\", real_time AS \"RealTime\", send_options_send_how AS \"SendOptionsSendHow\", send_options_send_when AS \"SendOptionsSendWhen\", send_options_send_by_duration AS \"SendOptionsSendByDuration\", send_options_send_by_interval_id AS \"SendOptionsSendByInterval\", send_options_send_every_duration AS \"SendOptionsSendEveryDuration\", send_options_send_every_interval_id AS \"SendOptionsSendEveryInterval\" FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_request_notices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Payment> Payments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Payment>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Payment2> Payment2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Payment2>($"SELECT id AS \"Id\", date_action AS \"CreationTime\", type_action AS \"TypeAction\", comments AS \"Comments\", notify AS \"Notify\", amount_action AS \"Amount\", balance AS \"RemainingAmount\", transaction_information AS \"TransactionInformation\", created_at AS \"CreatedAt\", source AS \"Source\", payment_method AS \"PaymentMethod\", fee_id AS \"FeeId\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PaymentMethod> PaymentMethods(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PaymentMethod>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PaymentMethod2> PaymentMethod2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PaymentMethod2>($"SELECT id AS \"Id\", name AS \"Name\", allowed_refund_method AS \"AllowedRefundMethod\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", owner_id AS \"OwnerId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}payment_methods{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Permission> Permissions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Permission>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Permission2> Permission2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Permission2>($"SELECT id AS \"Id\", permission_name AS \"Code\", display_name AS \"Name\", description AS \"Description\", mutable AS \"Editable\", visible AS \"Visible\", dummy AS \"Dummy\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionChildOf> PermissionChildOfs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionChildOf>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_child_of{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionGrantedTo> PermissionGrantedTos(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionGrantedTo>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", permissions_user_id AS \"PermissionsUserId\" FROM uc{(IsMySql ? "_" : ".")}permission_granted_to{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionSubPermission> PermissionSubPermissions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionSubPermission>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_sub_permissions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionsUser> PermissionsUsers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionsUser>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionsUser2> PermissionsUser2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionsUser2>($"SELECT id AS \"Id\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionsUserPermission> PermissionsUserPermissions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionsUserPermission>($"SELECT id AS \"Id\", permissions_user_id AS \"PermissionsUserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permissions_user_permissions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PermissionTag> PermissionTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PermissionTag>($"SELECT id AS \"Id\", permission_id AS \"PermissionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}permission_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PhysicalDescription> PhysicalDescriptions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PhysicalDescription>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}physical_descriptions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<PrecedingSucceedingTitle> PrecedingSucceedingTitles(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PrecedingSucceedingTitle>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", precedinginstanceid AS \"Precedinginstanceid\", succeedinginstanceid AS \"Succeedinginstanceid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PrecedingSucceedingTitle2>($"SELECT id AS \"Id\", preceding_instance_id AS \"PrecedingInstanceId\", succeeding_instance_id AS \"SucceedingInstanceId\", title AS \"Title\", hrid AS \"Hrid\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_titles{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<PrecedingSucceedingTitleIdentifier> PrecedingSucceedingTitleIdentifiers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PrecedingSucceedingTitleIdentifier>($"SELECT id AS \"Id\", preceding_succeeding_title_id AS \"PrecedingSucceedingTitleId\", value AS \"Value\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_title_identifiers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Prefix> Prefixes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Prefix>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Prefix2> Prefix2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Prefix2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Printer> Printers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Printer>($"SELECT id AS \"Id\", computer_name AS \"ComputerName\", name AS \"Name\", \"left\" AS \"Left\", top AS \"Top\", width AS \"Width\", height AS \"Height\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}printers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Proxy> Proxies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Proxy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_users{(IsMySql ? "_" : ".")}proxyfor{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Proxy2> Proxy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Proxy2>($"SELECT id AS \"Id\", user_id AS \"UserId\", proxy_user_id AS \"ProxyUserId\", request_for_sponsor AS \"RequestForSponsor\", notifications_to AS \"NotificationsTo\", accrue_to AS \"AccrueTo\", status AS \"Status\", expiration_date AS \"ExpirationDate\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}proxies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Publication> Publications(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Publication>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", publisher AS \"Publisher\", place AS \"Place\", date_of_publication AS \"PublicationYear\", role AS \"Role\" FROM uc{(IsMySql ? "_" : ".")}publications{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<PublicationFrequency> PublicationFrequencies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PublicationFrequency>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}publication_frequency{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<PublicationRange> PublicationRanges(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<PublicationRange>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}publication_range{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<RawRecord> RawRecords(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RawRecord>($"SELECT id AS \"Id\", content AS \"Content\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RawRecord2> RawRecord2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RawRecord2>($"SELECT id AS \"Id\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}raw_records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Receiving> Receivings(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Receiving>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", polineid AS \"Polineid\", titleid AS \"Titleid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Receiving2> Receiving2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Receiving2>($"SELECT id AS \"Id\", caption AS \"Caption\", comment AS \"Comment\", format AS \"Format\", item_id AS \"ItemId\", location_id AS \"LocationId\", po_line_id AS \"OrderItemId\", title_id AS \"TitleId\", receiving_status AS \"ReceivingStatus\", supplement AS \"Supplement\", receipt_date AS \"ReceiptTime\", received_date AS \"ReceiveTime\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}receivings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Record> Records(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Record>($"SELECT id AS \"Id\", snapshot_id AS \"SnapshotId\", matched_id AS \"MatchedId\", generation AS \"Generation\", record_type AS \"RecordType\", instance_id AS \"InstanceId\", state AS \"State\", leader_record_status AS \"LeaderRecordStatus\", \"order\" AS \"Order\", suppress_discovery AS \"SuppressDiscovery\", created_by_user_id AS \"CreationUserId\", created_date AS \"CreationTime\", updated_by_user_id AS \"LastWriteUserId\", updated_date AS \"LastWriteTime\", instance_hrid AS \"InstanceHrid\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Record2> Record2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Record2>($"SELECT id AS \"Id\", snapshot_id AS \"SnapshotId\", matched_id AS \"MatchedId\", generation AS \"Generation\", record_type AS \"RecordType\", instance_id AS \"InstanceId\", state AS \"State\", leader_record_status AS \"LeaderRecordStatus\", \"order\" AS \"Order\", suppress_discovery AS \"SuppressDiscovery\", creation_user_id AS \"CreationUserId\", creation_time AS \"CreationTime\", last_write_user_id AS \"LastWriteUserId\", last_write_time AS \"LastWriteTime\", instance_hrid AS \"InstanceHrid\" FROM uc{(IsMySql ? "_" : ".")}records{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RefundReason> RefundReasons(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RefundReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RefundReason2> RefundReason2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RefundReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", account_id AS \"AccountId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}refund_reasons{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Relationship> Relationships(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Relationship>($"SELECT id AS \"Id\", super_instance_id AS \"SuperInstanceId\", sub_instance_id AS \"SubInstanceId\", instance_relationship_type_id AS \"InstanceRelationshipTypeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}relationships{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RelationshipType> RelationshipTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RelationshipType>($"SELECT id AS \"Id\", name AS \"Name\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}relationship_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ReportingCode> ReportingCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ReportingCode>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ReportingCode2> ReportingCode2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ReportingCode2>($"SELECT id AS \"Id\", code AS \"Code\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}reporting_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Request> Requests(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Request>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", cancellationreasonid AS \"Cancellationreasonid\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Request2> Request2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Request2>($"SELECT id AS \"Id\", request_type AS \"RequestType\", request_date AS \"RequestDate\", requester_id AS \"RequesterId\", proxy_user_id AS \"ProxyUserId\", item_id AS \"ItemId\", status AS \"Status\", cancellation_reason_id AS \"CancellationReasonId\", cancelled_by_user_id AS \"CancelledByUserId\", cancellation_additional_information AS \"CancellationAdditionalInformation\", cancelled_date AS \"CancelledDate\", position AS \"Position\", item_title AS \"ItemTitle\", item_barcode AS \"ItemBarcode\", requester_first_name AS \"RequesterFirstName\", requester_last_name AS \"RequesterLastName\", requester_middle_name AS \"RequesterMiddleName\", requester_barcode AS \"RequesterBarcode\", requester_patron_group AS \"RequesterPatronGroup\", proxy_first_name AS \"ProxyFirstName\", proxy_last_name AS \"ProxyLastName\", proxy_middle_name AS \"ProxyMiddleName\", proxy_barcode AS \"ProxyBarcode\", proxy_patron_group AS \"ProxyPatronGroup\", fulfilment_preference AS \"FulfilmentPreference\", delivery_address_type_id AS \"DeliveryAddressTypeId\", request_expiration_date AS \"RequestExpirationDate\", hold_shelf_expiration_date AS \"HoldShelfExpirationDate\", pickup_service_point_id AS \"PickupServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", awaiting_pickup_request_closed_date AS \"AwaitingPickupRequestClosedDate\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}requests{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RequestIdentifier> RequestIdentifiers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RequestIdentifier>($"SELECT id AS \"Id\", request_id AS \"RequestId\", value AS \"Value\", identifier_type_id AS \"IdentifierTypeId\" FROM uc{(IsMySql ? "_" : ".")}request_identifiers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RequestPolicy> RequestPolicies(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RequestPolicy>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RequestPolicy2> RequestPolicy2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RequestPolicy2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_policies{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RequestPolicyRequestType> RequestPolicyRequestTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RequestPolicyRequestType>($"SELECT id AS \"Id\", request_policy_id AS \"RequestPolicyId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_policy_request_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<RequestTag> RequestTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<RequestTag>($"SELECT id AS \"Id\", request_id AS \"RequestId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}request_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ScheduledNotice> ScheduledNotices(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ScheduledNotice>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ScheduledNotice2> ScheduledNotice2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ScheduledNotice2>($"SELECT id AS \"Id\", loan_id AS \"LoanId\", request_id AS \"RequestId\", payment_id AS \"PaymentId\", recipient_user_id AS \"RecipientUserId\", next_run_time AS \"NextRunTime\", triggering_event AS \"TriggeringEvent\", notice_config_timing AS \"NoticeConfigTiming\", notice_config_recurring_period_duration AS \"NoticeConfigRecurringPeriodDuration\", notice_config_recurring_period_interval_id AS \"NoticeConfigRecurringPeriodInterval\", notice_config_template_id AS \"NoticeConfigTemplateId\", notice_config_format AS \"NoticeConfigFormat\", notice_config_send_in_real_time AS \"NoticeConfigSendInRealTime\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}scheduled_notices{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Series> Series(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Series>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}series{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePoint> ServicePoints(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePoint>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePoint2> ServicePoint2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePoint2>($"SELECT id AS \"Id\", name AS \"Name\", code AS \"Code\", discovery_display_name AS \"DiscoveryDisplayName\", description AS \"Description\", shelving_lag_time AS \"ShelvingLagTime\", pickup_location AS \"PickupLocation\", hold_shelf_expiry_period_duration AS \"HoldShelfExpiryPeriodDuration\", hold_shelf_expiry_period_interval_id AS \"HoldShelfExpiryPeriodInterval\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}service_points{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePointOwner> ServicePointOwners(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePointOwner>($"SELECT id AS \"Id\", owner_id AS \"OwnerId\", value AS \"Value\", label AS \"Label\" FROM uc{(IsMySql ? "_" : ".")}service_point_owners{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePointStaffSlip> ServicePointStaffSlips(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePointStaffSlip>($"SELECT id AS \"Id\", service_point_id AS \"ServicePointId\", staff_slip_id AS \"StaffSlipId\", print_by_default AS \"PrintByDefault\" FROM uc{(IsMySql ? "_" : ".")}service_point_staff_slips{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePointUser> ServicePointUsers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePointUser>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", defaultservicepointid AS \"Defaultservicepointid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePointUser2> ServicePointUser2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePointUser2>($"SELECT id AS \"Id\", user_id AS \"UserId\", default_service_point_id AS \"DefaultServicePointId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}service_point_users{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<ServicePointUserServicePoint> ServicePointUserServicePoints(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<ServicePointUserServicePoint>($"SELECT id AS \"Id\", service_point_user_id AS \"ServicePointUserId\", service_point_id AS \"ServicePointId\" FROM uc{(IsMySql ? "_" : ".")}service_point_user_service_points{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Setting> Settings(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Setting>($"SELECT id AS \"Id\", name AS \"Name\", orientation AS \"Orientation\", font_family AS \"FontFamily\", font_size AS \"FontSize\", font_weight AS \"FontWeight\", enabled AS \"Enabled\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\" FROM uc{(IsMySql ? "_" : ".")}settings{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Snapshot> Snapshots(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Snapshot>($"SELECT id AS \"Id\", status AS \"Status\", processing_started_date AS \"ProcessingStartedDate\", created_by_user_id AS \"CreationUserId\", created_date AS \"CreationTime\", updated_by_user_id AS \"LastWriteUserId\", updated_date AS \"LastWriteTime\" FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Snapshot2> Snapshot2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Snapshot2>($"SELECT id AS \"Id\", status AS \"Status\", processing_started_date AS \"ProcessingStartedDate\", creation_user_id AS \"CreationUserId\", creation_time AS \"CreationTime\", last_write_user_id AS \"LastWriteUserId\", last_write_time AS \"LastWriteTime\" FROM uc{(IsMySql ? "_" : ".")}snapshots{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Source> Sources(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Source>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Source2> Source2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Source2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}sources{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<SourceMarc> SourceMarcs(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<SourceMarc>($"SELECT id AS \"Id\", leader AS \"Leader\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}source_marcs{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<SourceMarcField> SourceMarcFields(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<SourceMarcField>($"SELECT id AS \"Id\", source_marc_id AS \"SourceMarcId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}source_marc_fields{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StaffSlip> StaffSlips(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StaffSlip>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StaffSlip2> StaffSlip2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StaffSlip2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", active AS \"Active\", template AS \"Template\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StatisticalCode> StatisticalCodes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StatisticalCode>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", statisticalcodetypeid AS \"Statisticalcodetypeid\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StatisticalCode2> StatisticalCode2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StatisticalCode2>($"SELECT id AS \"Id\", code AS \"Code\", name AS \"Name\", statistical_code_type_id AS \"StatisticalCodeTypeId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statistical_codes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StatisticalCodeType> StatisticalCodeTypes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StatisticalCodeType>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<StatisticalCodeType2> StatisticalCodeType2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<StatisticalCodeType2>($"SELECT id AS \"Id\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statistical_code_types{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Status> Statuses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Status>($"SELECT id AS \"Id\", code AS \"Code\", name AS \"Name\", source AS \"Source\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}statuses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Subject> Subjects(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Subject>($"SELECT id AS \"Id\", instance_id AS \"InstanceId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}subjects{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, instance_id"}" : "")}", param, skip, take);
        public IEnumerable<Suffix> Suffixes(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Suffix>($"SELECT id AS \"Id\", jsonb AS \"Content\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Suffix2> Suffix2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Suffix2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<SupplementStatement> SupplementStatements(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<SupplementStatement>($"SELECT id AS \"Id\", holding_id AS \"HoldingId\", statement AS \"Statement\", note AS \"Note\", staff_note AS \"StaffNote\" FROM uc{(IsMySql ? "_" : ".")}supplement_statements{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id, holding_id"}" : "")}", param, skip, take);
        public IEnumerable<Tag> Tags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Tag>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_tags{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Tag2> Tag2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Tag2>($"SELECT id AS \"Id\", label AS \"Label\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Template> Templates(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Template>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_template_engine{(IsMySql ? "_" : ".")}template{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Template2> Template2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Template2>($"SELECT id AS \"Id\", name AS \"Name\", active AS \"Active\", category AS \"Category\", description AS \"Description\", template_resolver AS \"TemplateResolver\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}templates{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TemplateOutputFormat> TemplateOutputFormats(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TemplateOutputFormat>($"SELECT id AS \"Id\", template_id AS \"TemplateId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}template_output_formats{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Title> Titles(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Title>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", polineid AS \"Polineid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Title2> Title2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Title2>($"SELECT id AS \"Id\", expected_receipt_date AS \"ExpectedReceiptDate\", title AS \"Title\", po_line_id AS \"OrderItemId\", instance_id AS \"InstanceId\", publisher AS \"Publisher\", edition AS \"Edition\", package_name AS \"PackageName\", po_line_number AS \"OrderItemNumber\", published_date AS \"PublishedDate\", receiving_note AS \"ReceivingNote\", subscription_from AS \"SubscriptionFrom\", subscription_to AS \"SubscriptionTo\", subscription_interval AS \"SubscriptionInterval\", is_acknowledged AS \"IsAcknowledged\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TitleContributor> TitleContributors(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TitleContributor>($"SELECT id AS \"Id\", title_id AS \"TitleId\", contributor AS \"Contributor\", contributor_name_type_id AS \"ContributorNameTypeId\" FROM uc{(IsMySql ? "_" : ".")}title_contributors{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TitleProductId> TitleProductIds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TitleProductId>($"SELECT id AS \"Id\", title_id AS \"TitleId\", product_id AS \"ProductId\", product_id_type_id AS \"ProductIdTypeId\", qualifier AS \"Qualifier\" FROM uc{(IsMySql ? "_" : ".")}title_product_ids{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Transaction> Transactions(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Transaction>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", fiscalyearid AS \"Fiscalyearid\", fromfundid AS \"Fromfundid\", sourcefiscalyearid AS \"Sourcefiscalyearid\", tofundid AS \"Tofundid\", expenseclassid AS \"Expenseclassid\" FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Transaction2> Transaction2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Transaction2>($"SELECT id AS \"Id\", amount AS \"Amount\", awaiting_payment_encumbrance_id AS \"AwaitingPaymentEncumbranceId\", awaiting_payment_release_encumbrance AS \"AwaitingPaymentReleaseEncumbrance\", currency AS \"Currency\", description AS \"Description\", encumbrance_amount_awaiting_payment AS \"AwaitingPaymentAmount\", encumbrance_amount_expended AS \"ExpendedAmount\", encumbrance_initial_amount_encumbered AS \"InitialEncumberedAmount\", encumbrance_status AS \"Status\", encumbrance_order_type AS \"OrderType\", encumbrance_subscription AS \"Subscription\", encumbrance_re_encumber AS \"ReEncumber\", encumbrance_source_purchase_order_id AS \"OrderId\", encumbrance_source_po_line_id AS \"OrderItemId\", expense_class_id AS \"ExpenseClassId\", fiscal_year_id AS \"FiscalYearId\", from_fund_id AS \"FromFundId\", payment_encumbrance_id AS \"PaymentEncumbranceId\", source AS \"Source\", source_fiscal_year_id AS \"SourceFiscalYearId\", source_invoice_id AS \"InvoiceId\", source_invoice_line_id AS \"InvoiceItemId\", to_fund_id AS \"ToFundId\", transaction_type AS \"TransactionType\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transactions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TransactionTag> TransactionTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TransactionTag>($"SELECT id AS \"Id\", transaction_id AS \"TransactionId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transaction_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TransferAccount> TransferAccounts(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TransferAccount>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TransferAccount2> TransferAccount2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TransferAccount2>($"SELECT id AS \"Id\", name AS \"Name\", \"desc\" AS \"Desc\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", owner_id AS \"OwnerId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transfer_accounts{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TransferCriteria> TransferCriterias(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TransferCriteria>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<TransferCriteria2> TransferCriteria2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<TransferCriteria2>($"SELECT id AS \"Id\", criteria AS \"Criteria\", type AS \"Type\", value AS \"Value\", interval AS \"Interval\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}transfer_criterias{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<User> Users(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<User>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", patrongroup AS \"Patrongroup\" FROM diku_mod_users{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<User2> User2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<User2>($"SELECT id AS \"Id\", username AS \"Username\", external_system_id AS \"ExternalSystemId\", barcode AS \"Barcode\", active AS \"Active\", type AS \"Type\", group_id AS \"GroupId\", name AS \"Name\", last_name AS \"LastName\", first_name AS \"FirstName\", middle_name AS \"MiddleName\", preferred_first_name AS \"PreferredFirstName\", email AS \"EmailAddress\", phone AS \"PhoneNumber\", mobile_phone AS \"MobilePhoneNumber\", date_of_birth AS \"BirthDate\", preferred_contact_type_id AS \"PreferredContactTypeId\", enrollment_date AS \"StartDate\", expiration_date AS \"EndDate\", source AS \"Source\", category AS \"Category\", status AS \"Status\", statuses AS \"Statuses\", staff_status AS \"StaffStatus\", staff_privileges AS \"StaffPrivileges\", staff_division AS \"StaffDivision\", staff_department AS \"StaffDepartment\", student_id AS \"StudentId\", student_status AS \"StudentStatus\", student_restriction AS \"StudentRestriction\", student_division AS \"StudentDivision\", student_department AS \"StudentDepartment\", deceased AS \"Deceased\", collections AS \"Collections\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserAcquisitionsUnit> UserAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserAcquisitionsUnit>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", acquisitionsunitid AS \"Acquisitionsunitid\" FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserAcquisitionsUnit2> UserAcquisitionsUnit2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserAcquisitionsUnit2>($"SELECT id AS \"Id\", user_id AS \"UserId\", acquisitions_unit_id AS \"AcquisitionsUnitId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserAddress> UserAddresses(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserAddress>($"SELECT id AS \"Id\", user_id AS \"UserId\", id2 AS \"Id2\", country_id AS \"CountryCode\", address_line1 AS \"StreetAddress1\", address_line2 AS \"StreetAddress2\", city AS \"City\", region AS \"State\", postal_code AS \"PostalCode\", address_type_id AS \"AddressTypeId\", primary_address AS \"Default\" FROM uc{(IsMySql ? "_" : ".")}user_addresses{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserDepartment> UserDepartments(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserDepartment>($"SELECT id AS \"Id\", user_id AS \"UserId\", department_id AS \"DepartmentId\" FROM uc{(IsMySql ? "_" : ".")}user_departments{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserRequestPreference> UserRequestPreferences(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserRequestPreference>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserRequestPreference2> UserRequestPreference2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserRequestPreference2>($"SELECT id AS \"Id\", user_id AS \"UserId\", hold_shelf AS \"HoldShelf\", delivery AS \"Delivery\", default_service_point_id AS \"DefaultServicePointId\", default_delivery_address_type_id AS \"DefaultDeliveryAddressTypeId\", fulfillment AS \"Fulfillment\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_request_preferences{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserSummary> UserSummaries(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserSummary>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserSummary2> UserSummary2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserSummary2>($"SELECT id AS \"Id\", user_id AS \"UserId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_summaries{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserSummaryOpenFeesFine> UserSummaryOpenFeesFines(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserSummaryOpenFeesFine>($"SELECT id AS \"Id\", user_summary_id AS \"UserSummaryId\", fee_fine_id AS \"FeeFineId\", fee_fine_type_id AS \"FeeFineTypeId\", loan_id AS \"LoanId\", balance AS \"Balance\" FROM uc{(IsMySql ? "_" : ".")}user_summary_open_fees_fines{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserSummaryOpenLoan> UserSummaryOpenLoans(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserSummaryOpenLoan>($"SELECT id AS \"Id\", user_summary_id AS \"UserSummaryId\", loan_id AS \"LoanId\", due_date AS \"DueDate\", recall AS \"Recall\", item_lost AS \"ItemLost\", item_claimed_returned AS \"ItemClaimedReturned\" FROM uc{(IsMySql ? "_" : ".")}user_summary_open_loans{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<UserTag> UserTags(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<UserTag>($"SELECT id AS \"Id\", user_id AS \"UserId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}user_tags{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Voucher> Vouchers(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Voucher>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", invoiceid AS \"Invoiceid\", batchgroupid AS \"Batchgroupid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<Voucher2> Voucher2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<Voucher2>($"SELECT id AS \"Id\", accounting_code AS \"AccountingCode\", amount AS \"Amount\", batch_group_id AS \"BatchGroupId\", disbursement_number AS \"DisbursementNumber\", disbursement_date AS \"DisbursementDate\", disbursement_amount AS \"DisbursementAmount\", invoice_currency AS \"InvoiceCurrency\", invoice_id AS \"InvoiceId\", exchange_rate AS \"ExchangeRate\", export_to_accounting AS \"ExportToAccounting\", status AS \"Status\", system_currency AS \"SystemCurrency\", type AS \"Type\", voucher_date AS \"VoucherDate\", voucher_number AS \"VoucherNumber\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<VoucherAcquisitionsUnit> VoucherAcquisitionsUnits(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<VoucherAcquisitionsUnit>($"SELECT id AS \"Id\", voucher_id AS \"VoucherId\", acquisitions_unit_id AS \"AcquisitionsUnitId\" FROM uc{(IsMySql ? "_" : ".")}voucher_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<VoucherItem> VoucherItems(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<VoucherItem>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\", voucherid AS \"Voucherid\" FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<VoucherItem2> VoucherItem2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<VoucherItem2>($"SELECT id AS \"Id\", amount AS \"Amount\", external_account_number AS \"ExternalAccountNumber\", sub_transaction_id AS \"SubTransactionId\", voucher_id AS \"VoucherId\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}voucher_items{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<VoucherItemFund> VoucherItemFunds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<VoucherItemFund>($"SELECT id AS \"Id\", voucher_item_id AS \"VoucherItemId\", code AS \"FundCode\", encumbrance_id AS \"EncumbranceId\", fund_id AS \"FundId\", invoice_item_id AS \"InvoiceItemId\", distribution_type AS \"DistributionType\", expense_class_id AS \"ExpenseClassId\", value AS \"Value\" FROM uc{(IsMySql ? "_" : ".")}voucher_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<VoucherItemSourceId> VoucherItemSourceIds(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<VoucherItemSourceId>($"SELECT id AS \"Id\", voucher_item_id AS \"VoucherItemId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}voucher_item_source_ids{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<WaiveReason> WaiveReasons(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<WaiveReason>($"SELECT id AS \"Id\", jsonb AS \"Content\", creation_date AS \"CreationTime\", created_by AS \"CreationUserId\" FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}waives{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);
        public IEnumerable<WaiveReason2> WaiveReason2s(string where = null, object param = null, string orderBy = null, int? skip = null, int? take = null) => Query<WaiveReason2>($"SELECT id AS \"Id\", name AS \"Name\", description AS \"Description\", created_date AS \"CreationTime\", created_by_user_id AS \"CreationUserId\", created_by_username AS \"CreationUserUsername\", updated_date AS \"LastWriteTime\", updated_by_user_id AS \"LastWriteUserId\", updated_by_username AS \"LastWriteUserUsername\", account_id AS \"AccountId\", content AS \"Content\" FROM uc{(IsMySql ? "_" : ".")}waive_reasons{(where != null ? $" WHERE {where}" : "")}{(orderBy != null || skip != null || take != null ? $" ORDER BY {orderBy ?? "id"}" : "")}", param, skip, take);

        public int CountAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAcquisitionsUnit2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAddresses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}addresses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAddressTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}addresstype{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAddressType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}address_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAlerts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAlert2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alerts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAllocatedFromFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}allocated_from_funds{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAllocatedToFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}allocated_to_funds{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAlternativeTitles(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alternative_titles{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAlternativeTitleTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAlternativeTitleType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alternative_title_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuditLoans(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuthAttempts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuthAttempt2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuthCredentialsHistories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuthCredentialsHistory2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}auth_credentials_histories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountAuthPasswordActions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchGroups(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchGroup2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVouchers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucher2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherBatchedVouchers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_vouchers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherBatchedVoucherBatchedVoucherLines(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_lines{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherBatchedVoucherBatchedVoucherLineFundCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_line_fund_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherExports(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherExport2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherExportConfigs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherExportConfig2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBatchVoucherExportConfigWeekdays(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_config_weekdays{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlocks(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlock2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}blocks{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlockConditions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlockCondition2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}block_conditions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlockLimits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBlockLimit2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}block_limits{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudgets(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudget2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budgets{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudgetAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudgetExpenseClasses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudgetExpenseClass2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_expense_classes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountBudgetTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCallNumberTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCallNumberType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}call_number_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCampuses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCampus2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}campuses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCancellationReasons(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCancellationReason2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}cancellation_reasons{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCategory2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCheckIns(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCheckIn2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}check_ins{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCirculationNotes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}circulation_notes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCirculationRules(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCirculationRule2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountClassifications(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}classifications{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountClassificationTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountClassificationType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}classification_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCloseReasons(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCloseReason2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}close_reasons{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountComments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountComment2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountConfigurations(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_configuration{(IsMySql ? "_" : ".")}config_data{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountConfiguration2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}configurations{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContacts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContact2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactAddresses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_addresses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactAddressCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_address_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactEmails(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_emails{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactEmailCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_email_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactPhoneNumbers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_phone_numbers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactPhoneNumberCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_phone_number_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactUrls(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_urls{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContactUrlCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_url_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContributors(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributors{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContributorNameTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContributorNameType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributor_name_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContributorTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountContributorType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributor_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCountries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}countries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCurrencies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}currencies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCustomFields(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCustomField2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountCustomFieldValues(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}custom_field_values{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountDepartments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountDepartment2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountDocuments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountDocument2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountEditions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}editions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountElectronicAccesses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}electronic_accesses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountElectronicAccessRelationships(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountElectronicAccessRelationship2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}electronic_access_relationships{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountErrorRecords(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountErrorRecord2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}error_records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountEventLogs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountEventLog2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountExpenseClasses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountExpenseClass2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}expense_classes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountExportConfigCredentials(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountExportConfigCredential2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountExtents(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}extents{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFees(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFee2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fees{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFeeTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFeeType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fee_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFinanceGroups(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFinanceGroup2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}finance_groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFinanceGroupAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}finance_group_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFiscalYears(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFiscalYear2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fiscal_years{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFiscalYearAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fiscal_year_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFixedDueDateSchedules(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFixedDueDateSchedule2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedules{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFixedDueDateScheduleSchedules(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedule_schedules{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFormats(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}formats{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFund2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}funds{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFundAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFundTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFundTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountFundType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountGroups(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountGroup2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountGroupFundFiscalYears(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountGroupFundFiscalYear2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}group_fund_fiscal_years{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldings(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHolding2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holdings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingElectronicAccesses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_electronic_accesses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingEntries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_entries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingFormerIds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_former_ids{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingNotes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_notes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingNoteTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingNoteType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_note_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingStatisticalCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_statistical_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHoldingType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHridSettings(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountHridSetting2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIdentifiers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}identifiers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIdTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIdType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}id_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIllPolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIllPolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ill_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIndexStatements(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}index_statements{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstances(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstance2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instances{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceFormats(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceFormat2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_formats{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceNatureOfContentTerms(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceNoteTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceNoteType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_note_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceRelationships(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceRelationshipTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceSourceMarcs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceStatisticalCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_statistical_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceStatuses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstanceType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstitutions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInstitution2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}institutions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInterfaces(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInterface2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInterfaceCredentials(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInterfaceCredential2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInterfaceTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interface_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoice2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceAdjustments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_adjustments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceAdjustmentFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItems(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItem2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_items{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItemAdjustments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItemAdjustmentFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItemFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceItemTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceOrderNumbers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_order_numbers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceTransactionSummaries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountInvoiceTransactionSummary2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountIssuanceModes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}mode_of_issuances{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItems(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItem2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}items{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemDamagedStatuses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemDamagedStatus2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_damaged_statuses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemElectronicAccesses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_electronic_accesses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemFormerIds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_former_ids{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemNotes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_notes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemNoteTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemNoteType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_note_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemStatisticalCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_statistical_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountItemYearCaptions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_year_caption{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJobExecutions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJobExecution2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJobExecutionProgresses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJobExecutionSourceChunks(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJobExecutionSourceChunk2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJournalRecords(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountJournalRecord2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLanguages(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}languages{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLedgers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLedger2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ledgers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLedgerAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ledger_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLibraries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLibrary2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}libraries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoans(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoan2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loans{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoanPolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoanPolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loan_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoanTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLoanType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loan_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLocations(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLocation2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}locations{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLocationServicePoints(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}location_service_points{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLocationSettings(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}location_settings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLogins(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLogin2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}logins{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLostItemFeePolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountLostItemFeePolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}lost_item_fee_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountMappingRules(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountMarcRecords(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountMarcRecord2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}marc_records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountMaterialTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountMaterialType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}material_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountModeOfIssuances(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNatureOfContentTerms(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNatureOfContentTerm2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNotes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_data{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNote2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}notes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNote3s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}notes2{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNoteLinks(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}note_links{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNoteTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountNoteType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}note_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrders(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrder2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}orders{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderInvoices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderInvoice2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_invoices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItems(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItem2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_items{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemAlerts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_alerts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemClaims(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_claims{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemContributors(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_contributors{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemLocation2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_locations{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemProductIds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_product_ids{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemReportingCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_reporting_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderItemVolumes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_volumes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderNotes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_notes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderTemplates(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderTemplate2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderTransactionSummaries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrderTransactionSummary2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizations(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganization2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAccounts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_accounts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAccountAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_account_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAddresses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_addresses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAddressCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_address_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAgreements(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_agreements{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationAliases(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_aliases{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationChangelogs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_changelogs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationContacts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_contacts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationEmails(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_emails{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationEmailCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_email_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationInterfaces(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_interfaces{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationPhoneNumbers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_phone_numbers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationPhoneNumberCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_phone_number_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationUrls(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_urls{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOrganizationUrlCategories(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_url_categories{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOverdueFinePolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOverdueFinePolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}overdue_fine_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOwners(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountOwner2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronActionSessions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronActionSession2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_action_sessions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronNoticePolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronNoticePolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronNoticePolicyFeeFineNotices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_fee_fine_notices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronNoticePolicyLoanNotices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_loan_notices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPatronNoticePolicyRequestNotices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_request_notices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPayments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPayment2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPaymentMethods(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPaymentMethod2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}payment_methods{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermission2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionChildOfs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_child_of{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionGrantedTos(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_granted_to{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionSubPermissions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_sub_permissions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionsUsers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionsUser2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionsUserPermissions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions_user_permissions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPermissionTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPhysicalDescriptions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}physical_descriptions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrecedingSucceedingTitles(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrecedingSucceedingTitle2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_titles{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrecedingSucceedingTitleIdentifiers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_title_identifiers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrefixes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrefix2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPrinters(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}printers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountProxies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}proxyfor{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountProxy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}proxies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPublications(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publications{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPublicationFrequencies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publication_frequency{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountPublicationRanges(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publication_range{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRawRecords(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRawRecord2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}raw_records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountReceivings(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountReceiving2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}receivings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRecords(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRecord2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}records{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRefundReasons(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRefundReason2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}refund_reasons{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRelationships(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}relationships{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRelationshipTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}relationship_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountReportingCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountReportingCode2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}reporting_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequests(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequest2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}requests{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequestIdentifiers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_identifiers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequestPolicies(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequestPolicy2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_policies{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequestPolicyRequestTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_policy_request_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountRequestTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountScheduledNotices(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountScheduledNotice2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}scheduled_notices{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSeries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}series{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePoints(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePoint2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_points{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePointOwners(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_owners{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePointStaffSlips(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_staff_slips{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePointUsers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePointUser2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_users{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountServicePointUserServicePoints(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_user_service_points{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSettings(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}settings{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSnapshots(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSnapshot2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}snapshots{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSources(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSource2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}sources{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSourceMarcs(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}source_marcs{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSourceMarcFields(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}source_marc_fields{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStaffSlips(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStaffSlip2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStatisticalCodes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStatisticalCode2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statistical_codes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStatisticalCodeTypes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStatisticalCodeType2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statistical_code_types{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountStatuses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statuses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSubjects(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}subjects{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSuffixes(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSuffix2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountSupplementStatements(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}supplement_statements{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_tags{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTag2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTemplates(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_template_engine{(IsMySql ? "_" : ".")}template{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTemplate2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}templates{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTemplateOutputFormats(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}template_output_formats{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTitles(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTitle2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTitleContributors(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}title_contributors{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTitleProductIds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}title_product_ids{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransactions(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransaction2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transactions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransactionTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transaction_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransferAccounts(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransferAccount2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transfer_accounts{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransferCriterias(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountTransferCriteria2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transfer_criterias{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUsers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUser2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserAcquisitionsUnit2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserAddresses(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_addresses{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserDepartments(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_departments{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserRequestPreferences(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserRequestPreference2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_request_preferences{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserSummaries(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserSummary2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summaries{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserSummaryOpenFeesFines(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summary_open_fees_fines{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserSummaryOpenLoans(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summary_open_loans{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountUserTags(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_tags{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVouchers(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucher2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucherAcquisitionsUnits(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_acquisitions_units{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucherItems(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucherItem2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_items{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucherItemFunds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountVoucherItemSourceIds(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_item_source_ids{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountWaiveReasons(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}waives{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);
        public int CountWaiveReason2s(string where = null, object param = null, int? take = null) => Count($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}waive_reasons{(where != null ? $" WHERE {where}" : "")}", param: param, take: take);

        public bool AnyAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAcquisitionsUnit2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAddresses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}addresses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAddressTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}addresstype{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAddressType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}address_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAlerts(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAlert2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alerts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAllocatedFromFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}allocated_from_funds{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAllocatedToFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}allocated_to_funds{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAlternativeTitles(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alternative_titles{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyAlternativeTitleTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAlternativeTitleType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}alternative_title_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuditLoans(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuthAttempts(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuthAttempt2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}auth_attempts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuthCredentialsHistories(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuthCredentialsHistory2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}auth_credentials_histories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyAuthPasswordActions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchGroups(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchGroup2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVouchers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucher2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_vouchers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherBatchedVouchers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_vouchers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherBatchedVoucherBatchedVoucherLines(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_lines{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherBatchedVoucherBatchedVoucherLineFundCodes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_line_fund_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherExports(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherExport2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_exports{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherExportConfigs(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherExportConfig2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_configs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBatchVoucherExportConfigWeekdays(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_config_weekdays{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlocks(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlock2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}blocks{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlockConditions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlockCondition2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}block_conditions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlockLimits(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBlockLimit2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}block_limits{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudgets(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudget2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budgets{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudgetAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudgetExpenseClasses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudgetExpenseClass2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_expense_classes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyBudgetTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}budget_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCallNumberTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCallNumberType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}call_number_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCampuses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCampus2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}campuses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCancellationReasons(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCancellationReason2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}cancellation_reasons{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCategories(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCategory2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCheckIns(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCheckIn2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}check_ins{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCirculationNotes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}circulation_notes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyCirculationRules(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCirculationRule2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}circulation_rules{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyClassifications(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}classifications{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyClassificationTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyClassificationType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}classification_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCloseReasons(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCloseReason2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}close_reasons{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyComments(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyComment2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}comments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyConfigurations(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_configuration{(IsMySql ? "_" : ".")}config_data{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyConfiguration2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}configurations{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContacts(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContact2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contacts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactAddresses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_addresses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactAddressCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_address_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactEmails(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_emails{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactEmailCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_email_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactPhoneNumbers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_phone_numbers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactPhoneNumberCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_phone_number_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactTypes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactUrls(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_urls{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContactUrlCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contact_url_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContributors(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributors{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyContributorNameTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContributorNameType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributor_name_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContributorTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyContributorType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}contributor_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCountries(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}countries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY alpha2_code" : "")}", param: param, take: 1).Any();
        public bool AnyCurrencies(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}currencies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCustomFields(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCustomField2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}custom_fields{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyCustomFieldValues(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}custom_field_values{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyDepartments(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyDepartment2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}departments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyDocuments(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyDocument2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}documents{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyEditions(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}editions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyElectronicAccesses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyElectronicAccessRelationships(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyElectronicAccessRelationship2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}electronic_access_relationships{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyErrorRecords(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyErrorRecord2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}error_records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyEventLogs(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyEventLog2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}event_logs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyExpenseClasses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyExpenseClass2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}expense_classes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyExportConfigCredentials(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyExportConfigCredential2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}export_config_credentials{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyExtents(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}extents{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyFees(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFee2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fees{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFeeTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFeeType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fee_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFinanceGroups(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFinanceGroup2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}finance_groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFinanceGroupAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}finance_group_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFiscalYears(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFiscalYear2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fiscal_years{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFiscalYearAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fiscal_year_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFixedDueDateSchedules(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFixedDueDateSchedule2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedules{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFixedDueDateScheduleSchedules(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedule_schedules{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFormats(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}formats{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFunds(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFund2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}funds{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFundAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFundTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFundTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyFundType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}fund_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyGroups(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyGroup2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}groups{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyGroupFundFiscalYears(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyGroupFundFiscalYear2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}group_fund_fiscal_years{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldings(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHolding2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holdings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingElectronicAccesses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingEntries(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_entries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingFormerIds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_former_ids{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingNotes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_notes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingNoteTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingNoteType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_note_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingStatisticalCodes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHoldingType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}holding_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHridSettings(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyHridSetting2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}hrid_settings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIdentifiers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}identifiers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyIdTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIdType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}id_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIllPolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIllPolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ill_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIndexStatements(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}index_statements{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyInstances(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstance2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instances{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceFormats(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceFormat2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_formats{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceNatureOfContentTerms(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceNoteTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceNoteType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_note_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceRelationships(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceRelationshipTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceSourceMarcs(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceStatisticalCodes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceStatuses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstanceType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}instance_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstitutions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInstitution2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}institutions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInterfaces(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInterface2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interfaces{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInterfaceCredentials(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInterfaceCredential2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interface_credentials{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInterfaceTypes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}interface_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoices(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoice2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceAdjustments(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_adjustments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceAdjustmentFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItems(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItem2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_items{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItemAdjustments(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItemAdjustmentFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustment_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItemFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceItemTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_item_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceOrderNumbers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_order_numbers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceTransactionSummaries(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyInvoiceTransactionSummary2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}invoice_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyIssuanceModes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}mode_of_issuances{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItems(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItem2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}items{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItemDamagedStatuses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItemDamagedStatus2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_damaged_statuses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItemElectronicAccesses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_electronic_accesses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyItemFormerIds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_former_ids{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyItemNotes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_notes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyItemNoteTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItemNoteType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_note_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyItemStatisticalCodes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_statistical_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyItemTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyItemYearCaptions(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}item_year_caption{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, item_id" : "")}", param: param, take: 1).Any();
        public bool AnyJobExecutions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJobExecution2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}job_executions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJobExecutionProgresses(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJobExecutionSourceChunks(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJobExecutionSourceChunk2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}job_execution_source_chunks{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJournalRecords(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyJournalRecord2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}journal_records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLanguages(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}languages{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyLedgers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLedger2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ledgers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLedgerAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}ledger_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLibraries(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLibrary2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}libraries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoans(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoan2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loans{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoanPolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoanPolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loan_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoanTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLoanType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}loan_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLocations(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLocation2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}locations{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLocationServicePoints(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}location_service_points{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLocationSettings(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}location_settings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLogins(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLogin2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}logins{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLostItemFeePolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyLostItemFeePolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}lost_item_fee_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyMappingRules(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyMarcRecords(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyMarcRecord2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}marc_records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyMaterialTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyMaterialType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}material_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyModeOfIssuances(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNatureOfContentTerms(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNatureOfContentTerm2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}nature_of_content_terms{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNotes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_data{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNote2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}notes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyNote3s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}notes2{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNoteLinks(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}note_links{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNoteTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyNoteType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}note_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrders(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrder2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}orders{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderInvoices(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderInvoice2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_invoices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItems(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItem2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_items{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemAlerts(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_alerts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemClaims(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_claims{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemContributors(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_contributors{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemLocation2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_locations{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemProductIds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_product_ids{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemReportingCodes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_reporting_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderItemVolumes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_item_volumes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderNotes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_notes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderTemplates(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderTemplate2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_templates{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderTransactionSummaries(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrderTransactionSummary2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}order_transaction_summaries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizations(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganization2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organizations{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAccounts(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_accounts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAccountAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_account_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAddresses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_addresses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAddressCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_address_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAgreements(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_agreements{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationAliases(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_aliases{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationChangelogs(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_changelogs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationContacts(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_contacts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationEmails(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_emails{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationEmailCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_email_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationInterfaces(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_interfaces{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationPhoneNumbers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_phone_numbers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationPhoneNumberCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_phone_number_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationUrls(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_urls{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOrganizationUrlCategories(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}organization_url_categories{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOverdueFinePolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOverdueFinePolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}overdue_fine_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOwners(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyOwner2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}owners{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronActionSessions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronActionSession2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_action_sessions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronNoticePolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronNoticePolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronNoticePolicyFeeFineNotices(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_fee_fine_notices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronNoticePolicyLoanNotices(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_loan_notices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPatronNoticePolicyRequestNotices(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_request_notices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPayments(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPayment2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPaymentMethods(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}payments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPaymentMethod2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}payment_methods{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermission2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionChildOfs(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_child_of{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionGrantedTos(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_granted_to{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionSubPermissions(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_sub_permissions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionsUsers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionsUser2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions_users{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionsUserPermissions(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permissions_user_permissions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPermissionTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}permission_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPhysicalDescriptions(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}physical_descriptions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyPrecedingSucceedingTitles(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPrecedingSucceedingTitle2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_titles{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPrecedingSucceedingTitleIdentifiers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_title_identifiers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPrefixes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPrefix2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}prefixes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPrinters(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}printers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyProxies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}proxyfor{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyProxy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}proxies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyPublications(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publications{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyPublicationFrequencies(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publication_frequency{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyPublicationRanges(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}publication_range{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyRawRecords(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRawRecord2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}raw_records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyReceivings(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyReceiving2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}receivings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRecords(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRecord2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}records{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRefundReasons(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRefundReason2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}refund_reasons{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRelationships(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}relationships{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRelationshipTypes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}relationship_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyReportingCodes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyReportingCode2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}reporting_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequests(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequest2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}requests{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequestIdentifiers(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_identifiers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequestPolicies(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequestPolicy2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_policies{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequestPolicyRequestTypes(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_policy_request_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyRequestTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}request_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyScheduledNotices(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyScheduledNotice2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}scheduled_notices{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySeries(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}series{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePoints(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePoint2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_points{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePointOwners(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_owners{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePointStaffSlips(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_staff_slips{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePointUsers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePointUser2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_users{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyServicePointUserServicePoints(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}service_point_user_service_points{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySettings(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}settings{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySnapshots(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySnapshot2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}snapshots{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySources(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySource2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}sources{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySourceMarcs(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}source_marcs{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySourceMarcFields(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}source_marc_fields{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStaffSlips(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStaffSlip2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}staff_slips{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStatisticalCodes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStatisticalCode2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statistical_codes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStatisticalCodeTypes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStatisticalCodeType2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statistical_code_types{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyStatuses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}statuses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySubjects(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}subjects{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, instance_id" : "")}", param: param, take: 1).Any();
        public bool AnySuffixes(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySuffix2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}suffixes{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnySupplementStatements(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}supplement_statements{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id, holding_id" : "")}", param: param, take: 1).Any();
        public bool AnyTags(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_tags{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTag2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTemplates(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_template_engine{(IsMySql ? "_" : ".")}template{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTemplate2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}templates{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTemplateOutputFormats(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}template_output_formats{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTitles(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTitle2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}titles{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTitleContributors(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}title_contributors{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTitleProductIds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}title_product_ids{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransactions(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransaction2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transactions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransactionTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transaction_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransferAccounts(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransferAccount2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transfer_accounts{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransferCriterias(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyTransferCriteria2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}transfer_criterias{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUsers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_users{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUser2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}users{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserAcquisitionsUnit2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserAddresses(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_addresses{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserDepartments(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_departments{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserRequestPreferences(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserRequestPreference2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_request_preferences{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserSummaries(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserSummary2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summaries{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserSummaryOpenFeesFines(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summary_open_fees_fines{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserSummaryOpenLoans(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_summary_open_loans{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyUserTags(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}user_tags{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVouchers(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucher2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}vouchers{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucherAcquisitionsUnits(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_acquisitions_units{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucherItems(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucherItem2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_items{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucherItemFunds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_item_fund_distributions{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyVoucherItemSourceIds(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}voucher_item_source_ids{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyWaiveReasons(string where = null, object param = null) => Query($"SELECT 1 FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}waives{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();
        public bool AnyWaiveReason2s(string where = null, object param = null) => Query($"SELECT 1 FROM uc{(IsMySql ? "_" : ".")}waive_reasons{(where != null ? $" WHERE {where}" : "")}{(IsSqlServer ? $" ORDER BY id" : "")}", param: param, take: 1).Any();

        public void Insert(AcquisitionsUnit acquisitionsUnit) => Execute("INSERT INTO diku_mod_orders_storage.acquisitions_unit (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", acquisitionsUnit);
        public void Insert(AddressType addressType) => Execute("INSERT INTO diku_mod_users.addresstype (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", addressType);
        public void Insert(Alert alert) => Execute("INSERT INTO diku_mod_orders_storage.alert (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", alert);
        public void Insert(AlternativeTitleType alternativeTitleType) => Execute("INSERT INTO diku_mod_inventory_storage.alternative_title_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", alternativeTitleType);
        public void Insert(AuditLoan auditLoan) => Execute("INSERT INTO diku_mod_circulation_storage.audit_loan (id, jsonb) VALUES (@Id, @Content::jsonb)", auditLoan);
        public void Insert(AuthAttempt authAttempt) => Execute("INSERT INTO diku_mod_login.auth_attempts (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", authAttempt);
        public void Insert(AuthCredentialsHistory authCredentialsHistory) => Execute("INSERT INTO diku_mod_login.auth_credentials_history (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", authCredentialsHistory);
        public void Insert(AuthPasswordAction authPasswordAction) => Execute("INSERT INTO diku_mod_login.auth_password_action (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", authPasswordAction);
        public void Insert(BatchGroup batchGroup) => Execute("INSERT INTO diku_mod_invoice_storage.batch_groups (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", batchGroup);
        public void Insert(BatchVoucher batchVoucher) => Execute("INSERT INTO diku_mod_invoice_storage.batch_vouchers (id, jsonb) VALUES (@Id, @Content::jsonb)", batchVoucher);
        public void Insert(BatchVoucherExport batchVoucherExport) => Execute("INSERT INTO diku_mod_invoice_storage.batch_voucher_exports (id, jsonb, creation_date, created_by, batchgroupid, batchvoucherid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Batchgroupid, @Batchvoucherid)", batchVoucherExport);
        public void Insert(BatchVoucherExportConfig batchVoucherExportConfig) => Execute("INSERT INTO diku_mod_invoice_storage.batch_voucher_export_configs (id, jsonb, creation_date, created_by, batchgroupid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Batchgroupid)", batchVoucherExportConfig);
        public void Insert(Block block) => Execute("INSERT INTO diku_mod_feesfines.manualblocks (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", block);
        public void Insert(BlockCondition blockCondition) => Execute("INSERT INTO diku_mod_patron_blocks.patron_block_conditions (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", blockCondition);
        public void Insert(BlockLimit blockLimit) => Execute("INSERT INTO diku_mod_patron_blocks.patron_block_limits (id, jsonb, creation_date, created_by, conditionid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Conditionid)", blockLimit);
        public void Insert(Budget budget) => Execute("INSERT INTO diku_mod_finance_storage.budget (id, jsonb, creation_date, created_by, fundid, fiscalyearid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @FundId, @FiscalYearId)", budget);
        public void Insert(BudgetExpenseClass budgetExpenseClass) => Execute("INSERT INTO diku_mod_finance_storage.budget_expense_class (id, jsonb, budgetid, expenseclassid) VALUES (@Id, @Content::jsonb, @Budgetid, @Expenseclassid)", budgetExpenseClass);
        public void Insert(CallNumberType callNumberType) => Execute("INSERT INTO diku_mod_inventory_storage.call_number_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", callNumberType);
        public void Insert(Campus campus) => Execute("INSERT INTO diku_mod_inventory_storage.loccampus (id, jsonb, creation_date, created_by, institutionid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Institutionid)", campus);
        public void Insert(CancellationReason cancellationReason) => Execute("INSERT INTO diku_mod_circulation_storage.cancellation_reason (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", cancellationReason);
        public void Insert(Category category) => Execute("INSERT INTO diku_mod_organizations_storage.categories (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", category);
        public void Insert(CheckIn checkIn) => Execute("INSERT INTO diku_mod_circulation_storage.check_in (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", checkIn);
        public void Insert(CirculationRule circulationRule) => Execute("INSERT INTO diku_mod_circulation_storage.circulation_rules (id, jsonb, lock) VALUES (@Id, @Content::jsonb, @Lock)", circulationRule);
        public void Insert(ClassificationType classificationType) => Execute("INSERT INTO diku_mod_inventory_storage.classification_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", classificationType);
        public void Insert(CloseReason closeReason) => Execute("INSERT INTO diku_mod_orders_storage.reasons_for_closure (id, jsonb) VALUES (@Id, @Content::jsonb)", closeReason);
        public void Insert(Comment comment) => Execute("INSERT INTO diku_mod_feesfines.comments (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", comment);
        public void Insert(Configuration configuration) => Execute("INSERT INTO diku_mod_configuration.config_data (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", configuration);
        public void Insert(Contact contact) => Execute("INSERT INTO diku_mod_organizations_storage.contacts (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", contact);
        public void Insert(ContactType contactType) => Execute("INSERT INTO uc.contact_types (id, name) VALUES (@Id, @Name)", contactType);
        public void Insert(ContributorNameType contributorNameType) => Execute("INSERT INTO diku_mod_inventory_storage.contributor_name_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", contributorNameType);
        public void Insert(ContributorType contributorType) => Execute("INSERT INTO diku_mod_inventory_storage.contributor_type (id, jsonb) VALUES (@Id, @Content::jsonb)", contributorType);
        public void Insert(Country country) => Execute("INSERT INTO uc.countries (alpha2_code, alpha3_code, name) VALUES (@Alpha2Code, @Alpha3Code, @Name)", country);
        public void Insert(CustomField customField) => Execute("INSERT INTO diku_mod_users.custom_fields (id, jsonb) VALUES (@Id, @Content::jsonb)", customField);
        public void Insert(Department department) => Execute("INSERT INTO diku_mod_users.departments (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", department);
        public void Insert(Document document) => Execute("INSERT INTO diku_mod_invoice_storage.documents (id, jsonb, creation_date, created_by, invoiceid, document_data) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Invoiceid, @DocumentData)", document);
        public void Insert(ElectronicAccessRelationship electronicAccessRelationship) => Execute("INSERT INTO diku_mod_inventory_storage.electronic_access_relationship (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", electronicAccessRelationship);
        public void Insert(ErrorRecord errorRecord) => Execute("INSERT INTO diku_mod_source_record_storage.error_records_lb (id, content, description) VALUES (@Id, @Content, @Description)", errorRecord);
        public void Insert(EventLog eventLog) => Execute("INSERT INTO diku_mod_login.event_logs (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", eventLog);
        public void Insert(ExpenseClass expenseClass) => Execute("INSERT INTO diku_mod_finance_storage.expense_class (id, jsonb) VALUES (@Id, @Content::jsonb)", expenseClass);
        public void Insert(ExportConfigCredential exportConfigCredential) => Execute("INSERT INTO diku_mod_invoice_storage.export_config_credentials (id, jsonb, creation_date, created_by, exportconfigid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Exportconfigid)", exportConfigCredential);
        public void Insert(Fee fee) => Execute("INSERT INTO diku_mod_feesfines.accounts (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", fee);
        public void Insert(FeeType feeType) => Execute("INSERT INTO diku_mod_feesfines.feefines (id, jsonb, creation_date, created_by, ownerid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Ownerid)", feeType);
        public void Insert(FinanceGroup financeGroup) => Execute("INSERT INTO diku_mod_finance_storage.groups (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", financeGroup);
        public void Insert(FiscalYear fiscalYear) => Execute("INSERT INTO diku_mod_finance_storage.fiscal_year (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", fiscalYear);
        public void Insert(FixedDueDateSchedule fixedDueDateSchedule) => Execute("INSERT INTO diku_mod_circulation_storage.fixed_due_date_schedule (id, jsonb) VALUES (@Id, @Content::jsonb)", fixedDueDateSchedule);
        public void Insert(Fund fund) => Execute("INSERT INTO diku_mod_finance_storage.fund (id, jsonb, creation_date, created_by, ledgerid, fundtypeid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @LedgerId, @Fundtypeid)", fund);
        public void Insert(FundType fundType) => Execute("INSERT INTO diku_mod_finance_storage.fund_type (id, jsonb) VALUES (@Id, @Content::jsonb)", fundType);
        public void Insert(Group group) => Execute("INSERT INTO diku_mod_users.groups (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", group);
        public void Insert(GroupFundFiscalYear groupFundFiscalYear) => Execute("INSERT INTO diku_mod_finance_storage.group_fund_fiscal_year (id, jsonb, budgetid, groupid, fundid, fiscalyearid) VALUES (@Id, @Content::jsonb, @Budgetid, @Groupid, @Fundid, @Fiscalyearid)", groupFundFiscalYear);
        public void Insert(Holding holding) => Execute("INSERT INTO diku_mod_inventory_storage.holdings_record (id, jsonb, creation_date, created_by, instanceid, permanentlocationid, temporarylocationid, holdingstypeid, callnumbertypeid, illpolicyid, sourceid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Instanceid, @Permanentlocationid, @Temporarylocationid, @Holdingstypeid, @Callnumbertypeid, @Illpolicyid, @Sourceid)", holding);
        public void Insert(HoldingNoteType holdingNoteType) => Execute("INSERT INTO diku_mod_inventory_storage.holdings_note_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", holdingNoteType);
        public void Insert(HoldingType holdingType) => Execute("INSERT INTO diku_mod_inventory_storage.holdings_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", holdingType);
        public void Insert(HridSetting hridSetting) => Execute("INSERT INTO diku_mod_inventory_storage.hrid_settings (id, jsonb, lock) VALUES (@Id, @Content::jsonb, @Lock)", hridSetting);
        public void Insert(IdType idType) => Execute("INSERT INTO diku_mod_inventory_storage.identifier_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", idType);
        public void Insert(IllPolicy illPolicy) => Execute("INSERT INTO diku_mod_inventory_storage.ill_policy (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", illPolicy);
        public void Insert(Instance instance) => Execute("INSERT INTO diku_mod_inventory_storage.instance (id, jsonb, creation_date, created_by, instancestatusid, modeofissuanceid, instancetypeid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Instancestatusid, @Modeofissuanceid, @Instancetypeid)", instance);
        public void Insert(InstanceFormat instanceFormat) => Execute("INSERT INTO diku_mod_inventory_storage.instance_format (id, jsonb) VALUES (@Id, @Content::jsonb)", instanceFormat);
        public void Insert(InstanceNoteType instanceNoteType) => Execute("INSERT INTO diku_mod_inventory_storage.instance_note_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", instanceNoteType);
        public void Insert(InstanceRelationship instanceRelationship) => Execute("INSERT INTO diku_mod_inventory_storage.instance_relationship (id, jsonb, creation_date, created_by, superinstanceid, subinstanceid, instancerelationshiptypeid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Superinstanceid, @Subinstanceid, @Instancerelationshiptypeid)", instanceRelationship);
        public void Insert(InstanceRelationshipType instanceRelationshipType) => Execute("INSERT INTO diku_mod_inventory_storage.instance_relationship_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", instanceRelationshipType);
        public void Insert(InstanceSourceMarc instanceSourceMarc) => Execute("INSERT INTO diku_mod_inventory_storage.instance_source_marc (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", instanceSourceMarc);
        public void Insert(InstanceStatus instanceStatus) => Execute("INSERT INTO diku_mod_inventory_storage.instance_status (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", instanceStatus);
        public void Insert(InstanceType instanceType) => Execute("INSERT INTO diku_mod_inventory_storage.instance_type (id, jsonb) VALUES (@Id, @Content::jsonb)", instanceType);
        public void Insert(Institution institution) => Execute("INSERT INTO diku_mod_inventory_storage.locinstitution (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", institution);
        public void Insert(Interface @interface) => Execute("INSERT INTO diku_mod_organizations_storage.interfaces (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", @interface);
        public void Insert(InterfaceCredential interfaceCredential) => Execute("INSERT INTO diku_mod_organizations_storage.interface_credentials (id, jsonb, interfaceid) VALUES (@Id, @Content::jsonb, @Interfaceid)", interfaceCredential);
        public void Insert(Invoice invoice) => Execute("INSERT INTO diku_mod_invoice_storage.invoices (id, jsonb, creation_date, created_by, batchgroupid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Batchgroupid)", invoice);
        public void Insert(InvoiceItem invoiceItem) => Execute("INSERT INTO diku_mod_invoice_storage.invoice_lines (id, jsonb, creation_date, created_by, invoiceid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Invoiceid)", invoiceItem);
        public void Insert(InvoiceTransactionSummary invoiceTransactionSummary) => Execute("INSERT INTO diku_mod_finance_storage.invoice_transaction_summaries (id, jsonb) VALUES (@Id, @Content::jsonb)", invoiceTransactionSummary);
        public void Insert(Item item) => Execute("INSERT INTO diku_mod_inventory_storage.item (id, jsonb, creation_date, created_by, holdingsrecordid, permanentloantypeid, temporaryloantypeid, materialtypeid, permanentlocationid, temporarylocationid, effectivelocationid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Holdingsrecordid, @Permanentloantypeid, @Temporaryloantypeid, @Materialtypeid, @Permanentlocationid, @Temporarylocationid, @Effectivelocationid)", item);
        public void Insert(ItemDamagedStatus itemDamagedStatus) => Execute("INSERT INTO diku_mod_inventory_storage.item_damaged_status (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", itemDamagedStatus);
        public void Insert(ItemNoteType itemNoteType) => Execute("INSERT INTO diku_mod_inventory_storage.item_note_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", itemNoteType);
        public void Insert(JobExecution jobExecution) => Execute("INSERT INTO diku_mod_source_record_manager.job_executions (id, jsonb) VALUES (@Id, @Content::jsonb)", jobExecution);
        public void Insert(JobExecutionProgress jobExecutionProgress) => Execute("INSERT INTO diku_mod_source_record_manager.job_execution_progress (id, jsonb, jobexecutionid) VALUES (@Id, @Content::jsonb, @Jobexecutionid)", jobExecutionProgress);
        public void Insert(JobExecutionSourceChunk jobExecutionSourceChunk) => Execute("INSERT INTO diku_mod_source_record_manager.job_execution_source_chunks (id, jsonb, jobexecutionid) VALUES (@Id, @Content::jsonb, @Jobexecutionid)", jobExecutionSourceChunk);
        public void Insert(JournalRecord journalRecord) => Execute("INSERT INTO diku_mod_source_record_manager.journal_records (id, job_execution_id, source_id, entity_type, entity_id, entity_hrid, action_type, action_status, action_date, source_record_order, error) VALUES (@Id, @JobExecutionId, @SourceId, @EntityType, @EntityId, @EntityHrid, @ActionType, @ActionStatus, @ActionDate, @SourceRecordOrder, @Error)", journalRecord);
        public void Insert(Ledger ledger) => Execute("INSERT INTO diku_mod_finance_storage.ledger (id, jsonb, creation_date, created_by, fiscalyearoneid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Fiscalyearoneid)", ledger);
        public void Insert(Library library) => Execute("INSERT INTO diku_mod_inventory_storage.loclibrary (id, jsonb, creation_date, created_by, campusid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Campusid)", library);
        public void Insert(Loan loan) => Execute("INSERT INTO diku_mod_circulation_storage.loan (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", loan);
        public void Insert(LoanPolicy loanPolicy) => Execute("INSERT INTO diku_mod_circulation_storage.loan_policy (id, jsonb, creation_date, created_by, loanspolicy_fixedduedatescheduleid, renewalspolicy_alternatefixedduedatescheduleid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @LoanspolicyFixedduedatescheduleid, @RenewalspolicyAlternatefixedduedatescheduleid)", loanPolicy);
        public void Insert(LoanType loanType) => Execute("INSERT INTO diku_mod_inventory_storage.loan_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", loanType);
        public void Insert(Location location) => Execute("INSERT INTO diku_mod_inventory_storage.location (id, jsonb, creation_date, created_by, institutionid, campusid, libraryid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Institutionid, @Campusid, @Libraryid)", location);
        public void Insert(Login login) => Execute("INSERT INTO diku_mod_login.auth_credentials (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", login);
        public void Insert(LostItemFeePolicy lostItemFeePolicy) => Execute("INSERT INTO diku_mod_feesfines.lost_item_fee_policy (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", lostItemFeePolicy);
        public void Insert(MappingRule mappingRule) => Execute("INSERT INTO diku_mod_source_record_manager.mapping_rules (id, jsonb) VALUES (@Id, @Content::jsonb)", mappingRule);
        public void Insert(MarcRecord marcRecord) => Execute("INSERT INTO diku_mod_source_record_storage.marc_records_lb (id, content) VALUES (@Id, @Content::jsonb)", marcRecord);
        public void Insert(MaterialType materialType) => Execute("INSERT INTO diku_mod_inventory_storage.material_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", materialType);
        public void Insert(ModeOfIssuance modeOfIssuance) => Execute("INSERT INTO diku_mod_inventory_storage.mode_of_issuance (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", modeOfIssuance);
        public void Insert(NatureOfContentTerm natureOfContentTerm) => Execute("INSERT INTO diku_mod_inventory_storage.nature_of_content_term (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", natureOfContentTerm);
        public void Insert(Note note) => Execute("INSERT INTO diku_mod_notes.note_data (id, jsonb, temporary_type_id) VALUES (@Id, @Content::jsonb, @TemporaryTypeId)", note);
        public void Insert(NoteType noteType) => Execute("INSERT INTO diku_mod_notes.note_type (id, jsonb) VALUES (@Id, @Content::jsonb)", noteType);
        public void Insert(Order order) => Execute("INSERT INTO diku_mod_orders_storage.purchase_order (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", order);
        public void Insert(OrderInvoice orderInvoice) => Execute("INSERT INTO diku_mod_orders_storage.order_invoice_relationship (id, jsonb, purchaseorderid) VALUES (@Id, @Content::jsonb, @Purchaseorderid)", orderInvoice);
        public void Insert(OrderItem orderItem) => Execute("INSERT INTO diku_mod_orders_storage.po_line (id, jsonb, creation_date, created_by, purchaseorderid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Purchaseorderid)", orderItem);
        public void Insert(OrderTemplate orderTemplate) => Execute("INSERT INTO diku_mod_orders_storage.order_templates (id, jsonb) VALUES (@Id, @Content::jsonb)", orderTemplate);
        public void Insert(OrderTransactionSummary orderTransactionSummary) => Execute("INSERT INTO diku_mod_finance_storage.order_transaction_summaries (id, jsonb) VALUES (@Id, @Content::jsonb)", orderTransactionSummary);
        public void Insert(Organization organization) => Execute("INSERT INTO diku_mod_organizations_storage.organizations (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", organization);
        public void Insert(OverdueFinePolicy overdueFinePolicy) => Execute("INSERT INTO diku_mod_feesfines.overdue_fine_policy (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", overdueFinePolicy);
        public void Insert(Owner owner) => Execute("INSERT INTO diku_mod_feesfines.owners (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", owner);
        public void Insert(PatronActionSession patronActionSession) => Execute("INSERT INTO diku_mod_circulation_storage.patron_action_session (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", patronActionSession);
        public void Insert(PatronNoticePolicy patronNoticePolicy) => Execute("INSERT INTO diku_mod_circulation_storage.patron_notice_policy (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", patronNoticePolicy);
        public void Insert(Payment payment) => Execute("INSERT INTO diku_mod_feesfines.feefineactions (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", payment);
        public void Insert(PaymentMethod paymentMethod) => Execute("INSERT INTO diku_mod_feesfines.payments (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", paymentMethod);
        public void Insert(Permission permission) => Execute("INSERT INTO diku_mod_permissions.permissions (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", permission);
        public void Insert(PermissionsUser permissionsUser) => Execute("INSERT INTO diku_mod_permissions.permissions_users (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", permissionsUser);
        public void Insert(PrecedingSucceedingTitle precedingSucceedingTitle) => Execute("INSERT INTO diku_mod_inventory_storage.preceding_succeeding_title (id, jsonb, creation_date, created_by, precedinginstanceid, succeedinginstanceid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Precedinginstanceid, @Succeedinginstanceid)", precedingSucceedingTitle);
        public void Insert(Prefix prefix) => Execute("INSERT INTO diku_mod_orders_storage.prefixes (id, jsonb) VALUES (@Id, @Content::jsonb)", prefix);
        public void Insert(Proxy proxy) => Execute("INSERT INTO diku_mod_users.proxyfor (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", proxy);
        public void Insert(RawRecord rawRecord) => Execute("INSERT INTO diku_mod_source_record_storage.raw_records_lb (id, content) VALUES (@Id, @Content)", rawRecord);
        public void Insert(Receiving receiving) => Execute("INSERT INTO diku_mod_orders_storage.pieces (id, jsonb, creation_date, created_by, polineid, titleid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Polineid, @Titleid)", receiving);
        public void Insert(Record record) => Execute("INSERT INTO diku_mod_source_record_storage.records_lb (id, snapshot_id, matched_id, generation, record_type, instance_id, state, leader_record_status, order, suppress_discovery, created_by_user_id, created_date, updated_by_user_id, updated_date, instance_hrid) VALUES (@Id, @SnapshotId, @MatchedId, @Generation, @RecordType, @InstanceId, @State, @LeaderRecordStatus, @Order, @SuppressDiscovery, @CreationUserId, @CreationTime, @LastWriteUserId, @LastWriteTime, @InstanceHrid)", record);
        public void Insert(RefundReason refundReason) => Execute("INSERT INTO diku_mod_feesfines.refunds (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", refundReason);
        public void Insert(ReportingCode reportingCode) => Execute("INSERT INTO diku_mod_orders_storage.reporting_code (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", reportingCode);
        public void Insert(Request request) => Execute("INSERT INTO diku_mod_circulation_storage.request (id, jsonb, creation_date, created_by, cancellationreasonid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Cancellationreasonid)", request);
        public void Insert(RequestPolicy requestPolicy) => Execute("INSERT INTO diku_mod_circulation_storage.request_policy (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", requestPolicy);
        public void Insert(ScheduledNotice scheduledNotice) => Execute("INSERT INTO diku_mod_circulation_storage.scheduled_notice (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", scheduledNotice);
        public void Insert(ServicePoint servicePoint) => Execute("INSERT INTO diku_mod_inventory_storage.service_point (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", servicePoint);
        public void Insert(ServicePointUser servicePointUser) => Execute("INSERT INTO diku_mod_inventory_storage.service_point_user (id, jsonb, creation_date, created_by, defaultservicepointid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Defaultservicepointid)", servicePointUser);
        public void Insert(Snapshot snapshot) => Execute("INSERT INTO diku_mod_source_record_storage.snapshots_lb (id, status, processing_started_date, created_by_user_id, created_date, updated_by_user_id, updated_date) VALUES (@Id, @Status, @ProcessingStartedDate, @CreationUserId, @CreationTime, @LastWriteUserId, @LastWriteTime)", snapshot);
        public void Insert(Source source) => Execute("INSERT INTO diku_mod_inventory_storage.holdings_records_source (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", source);
        public void Insert(StaffSlip staffSlip) => Execute("INSERT INTO diku_mod_circulation_storage.staff_slips (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", staffSlip);
        public void Insert(StatisticalCode statisticalCode) => Execute("INSERT INTO diku_mod_inventory_storage.statistical_code (id, jsonb, creation_date, created_by, statisticalcodetypeid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Statisticalcodetypeid)", statisticalCode);
        public void Insert(StatisticalCodeType statisticalCodeType) => Execute("INSERT INTO diku_mod_inventory_storage.statistical_code_type (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", statisticalCodeType);
        public void Insert(Suffix suffix) => Execute("INSERT INTO diku_mod_orders_storage.suffixes (id, jsonb) VALUES (@Id, @Content::jsonb)", suffix);
        public void Insert(Tag tag) => Execute("INSERT INTO diku_mod_tags.tags (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", tag);
        public void Insert(Template template) => Execute("INSERT INTO diku_mod_template_engine.template (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", template);
        public void Insert(Title title) => Execute("INSERT INTO diku_mod_orders_storage.titles (id, jsonb, creation_date, created_by, polineid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Polineid)", title);
        public void Insert(Transaction transaction) => Execute("INSERT INTO diku_mod_finance_storage.transaction (id, jsonb, creation_date, created_by, fiscalyearid, fromfundid, sourcefiscalyearid, tofundid, expenseclassid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Fiscalyearid, @Fromfundid, @Sourcefiscalyearid, @Tofundid, @Expenseclassid)", transaction);
        public void Insert(TransferAccount transferAccount) => Execute("INSERT INTO diku_mod_feesfines.transfers (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", transferAccount);
        public void Insert(TransferCriteria transferCriteria) => Execute("INSERT INTO diku_mod_feesfines.transfer_criteria (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", transferCriteria);
        public void Insert(User user) => Execute("INSERT INTO diku_mod_users.users (id, jsonb, creation_date, created_by, patrongroup) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Patrongroup)", user);
        public void Insert(UserAcquisitionsUnit userAcquisitionsUnit) => Execute("INSERT INTO diku_mod_orders_storage.acquisitions_unit_membership (id, jsonb, creation_date, created_by, acquisitionsunitid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Acquisitionsunitid)", userAcquisitionsUnit);
        public void Insert(UserRequestPreference userRequestPreference) => Execute("INSERT INTO diku_mod_circulation_storage.user_request_preference (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", userRequestPreference);
        public void Insert(UserSummary userSummary) => Execute("INSERT INTO diku_mod_patron_blocks.user_summary (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", userSummary);
        public void Insert(Voucher voucher) => Execute("INSERT INTO diku_mod_invoice_storage.vouchers (id, jsonb, creation_date, created_by, invoiceid, batchgroupid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Invoiceid, @Batchgroupid)", voucher);
        public void Insert(VoucherItem voucherItem) => Execute("INSERT INTO diku_mod_invoice_storage.voucher_lines (id, jsonb, creation_date, created_by, voucherid) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId, @Voucherid)", voucherItem);
        public void Insert(WaiveReason waiveReason) => Execute("INSERT INTO diku_mod_feesfines.waives (id, jsonb, creation_date, created_by) VALUES (@Id, @Content::jsonb, @CreationTime, @CreationUserId)", waiveReason);

        public int Update(AcquisitionsUnit acquisitionsUnit) => Execute($"UPDATE diku_mod_orders_storage.acquisitions_unit SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", acquisitionsUnit);
        public int Update(AddressType addressType) => Execute($"UPDATE diku_mod_users.addresstype SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", addressType);
        public int Update(Alert alert) => Execute($"UPDATE diku_mod_orders_storage.alert SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", alert);
        public int Update(AlternativeTitleType alternativeTitleType) => Execute($"UPDATE diku_mod_inventory_storage.alternative_title_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", alternativeTitleType);
        public int Update(AuditLoan auditLoan) => Execute($"UPDATE diku_mod_circulation_storage.audit_loan SET jsonb = @Content::jsonb WHERE id = @Id", auditLoan);
        public int Update(AuthAttempt authAttempt) => Execute($"UPDATE diku_mod_login.auth_attempts SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", authAttempt);
        public int Update(AuthCredentialsHistory authCredentialsHistory) => Execute($"UPDATE diku_mod_login.auth_credentials_history SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", authCredentialsHistory);
        public int Update(AuthPasswordAction authPasswordAction) => Execute($"UPDATE diku_mod_login.auth_password_action SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", authPasswordAction);
        public int Update(BatchGroup batchGroup) => Execute($"UPDATE diku_mod_invoice_storage.batch_groups SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", batchGroup);
        public int Update(BatchVoucher batchVoucher) => Execute($"UPDATE diku_mod_invoice_storage.batch_vouchers SET jsonb = @Content::jsonb WHERE id = @Id", batchVoucher);
        public int Update(BatchVoucherExport batchVoucherExport) => Execute($"UPDATE diku_mod_invoice_storage.batch_voucher_exports SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, batchgroupid = @Batchgroupid, batchvoucherid = @Batchvoucherid WHERE id = @Id", batchVoucherExport);
        public int Update(BatchVoucherExportConfig batchVoucherExportConfig) => Execute($"UPDATE diku_mod_invoice_storage.batch_voucher_export_configs SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, batchgroupid = @Batchgroupid WHERE id = @Id", batchVoucherExportConfig);
        public int Update(Block block) => Execute($"UPDATE diku_mod_feesfines.manualblocks SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", block);
        public int Update(BlockCondition blockCondition) => Execute($"UPDATE diku_mod_patron_blocks.patron_block_conditions SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", blockCondition);
        public int Update(BlockLimit blockLimit) => Execute($"UPDATE diku_mod_patron_blocks.patron_block_limits SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, conditionid = @Conditionid WHERE id = @Id", blockLimit);
        public int Update(Budget budget) => Execute($"UPDATE diku_mod_finance_storage.budget SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, fundid = @FundId, fiscalyearid = @FiscalYearId WHERE id = @Id", budget);
        public int Update(BudgetExpenseClass budgetExpenseClass) => Execute($"UPDATE diku_mod_finance_storage.budget_expense_class SET jsonb = @Content::jsonb, budgetid = @Budgetid, expenseclassid = @Expenseclassid WHERE id = @Id", budgetExpenseClass);
        public int Update(CallNumberType callNumberType) => Execute($"UPDATE diku_mod_inventory_storage.call_number_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", callNumberType);
        public int Update(Campus campus) => Execute($"UPDATE diku_mod_inventory_storage.loccampus SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, institutionid = @Institutionid WHERE id = @Id", campus);
        public int Update(CancellationReason cancellationReason) => Execute($"UPDATE diku_mod_circulation_storage.cancellation_reason SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", cancellationReason);
        public int Update(Category category) => Execute($"UPDATE diku_mod_organizations_storage.categories SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", category);
        public int Update(CheckIn checkIn) => Execute($"UPDATE diku_mod_circulation_storage.check_in SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", checkIn);
        public int Update(CirculationRule circulationRule) => Execute($"UPDATE diku_mod_circulation_storage.circulation_rules SET jsonb = @Content::jsonb, lock = @Lock WHERE id = @Id", circulationRule);
        public int Update(ClassificationType classificationType) => Execute($"UPDATE diku_mod_inventory_storage.classification_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", classificationType);
        public int Update(CloseReason closeReason) => Execute($"UPDATE diku_mod_orders_storage.reasons_for_closure SET jsonb = @Content::jsonb WHERE id = @Id", closeReason);
        public int Update(Comment comment) => Execute($"UPDATE diku_mod_feesfines.comments SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", comment);
        public int Update(Configuration configuration) => Execute($"UPDATE diku_mod_configuration.config_data SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", configuration);
        public int Update(Contact contact) => Execute($"UPDATE diku_mod_organizations_storage.contacts SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", contact);
        public int Update(ContactType contactType) => Execute($"UPDATE uc.contact_types SET name = @Name WHERE id = @Id", contactType);
        public int Update(ContributorNameType contributorNameType) => Execute($"UPDATE diku_mod_inventory_storage.contributor_name_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", contributorNameType);
        public int Update(ContributorType contributorType) => Execute($"UPDATE diku_mod_inventory_storage.contributor_type SET jsonb = @Content::jsonb WHERE id = @Id", contributorType);
        public int Update(Country country) => Execute($"UPDATE uc.countries SET alpha3_code = @Alpha3Code, name = @Name WHERE alpha2_code = @Alpha2Code", country);
        public int Update(CustomField customField) => Execute($"UPDATE diku_mod_users.custom_fields SET jsonb = @Content::jsonb WHERE id = @Id", customField);
        public int Update(Department department) => Execute($"UPDATE diku_mod_users.departments SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", department);
        public int Update(Document document) => Execute($"UPDATE diku_mod_invoice_storage.documents SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, invoiceid = @Invoiceid, document_data = @DocumentData WHERE id = @Id", document);
        public int Update(ElectronicAccessRelationship electronicAccessRelationship) => Execute($"UPDATE diku_mod_inventory_storage.electronic_access_relationship SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", electronicAccessRelationship);
        public int Update(ErrorRecord errorRecord) => Execute($"UPDATE diku_mod_source_record_storage.error_records_lb SET content = @Content, description = @Description WHERE id = @Id", errorRecord);
        public int Update(EventLog eventLog) => Execute($"UPDATE diku_mod_login.event_logs SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", eventLog);
        public int Update(ExpenseClass expenseClass) => Execute($"UPDATE diku_mod_finance_storage.expense_class SET jsonb = @Content::jsonb WHERE id = @Id", expenseClass);
        public int Update(ExportConfigCredential exportConfigCredential) => Execute($"UPDATE diku_mod_invoice_storage.export_config_credentials SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, exportconfigid = @Exportconfigid WHERE id = @Id", exportConfigCredential);
        public int Update(Fee fee) => Execute($"UPDATE diku_mod_feesfines.accounts SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", fee);
        public int Update(FeeType feeType) => Execute($"UPDATE diku_mod_feesfines.feefines SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, ownerid = @Ownerid WHERE id = @Id", feeType);
        public int Update(FinanceGroup financeGroup) => Execute($"UPDATE diku_mod_finance_storage.groups SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", financeGroup);
        public int Update(FiscalYear fiscalYear) => Execute($"UPDATE diku_mod_finance_storage.fiscal_year SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", fiscalYear);
        public int Update(FixedDueDateSchedule fixedDueDateSchedule) => Execute($"UPDATE diku_mod_circulation_storage.fixed_due_date_schedule SET jsonb = @Content::jsonb WHERE id = @Id", fixedDueDateSchedule);
        public int Update(Fund fund) => Execute($"UPDATE diku_mod_finance_storage.fund SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, ledgerid = @LedgerId, fundtypeid = @Fundtypeid WHERE id = @Id", fund);
        public int Update(FundType fundType) => Execute($"UPDATE diku_mod_finance_storage.fund_type SET jsonb = @Content::jsonb WHERE id = @Id", fundType);
        public int Update(Group group) => Execute($"UPDATE diku_mod_users.groups SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", group);
        public int Update(GroupFundFiscalYear groupFundFiscalYear) => Execute($"UPDATE diku_mod_finance_storage.group_fund_fiscal_year SET jsonb = @Content::jsonb, budgetid = @Budgetid, groupid = @Groupid, fundid = @Fundid, fiscalyearid = @Fiscalyearid WHERE id = @Id", groupFundFiscalYear);
        public int Update(Holding holding) => Execute($"UPDATE diku_mod_inventory_storage.holdings_record SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, instanceid = @Instanceid, permanentlocationid = @Permanentlocationid, temporarylocationid = @Temporarylocationid, holdingstypeid = @Holdingstypeid, callnumbertypeid = @Callnumbertypeid, illpolicyid = @Illpolicyid, sourceid = @Sourceid WHERE id = @Id", holding);
        public int Update(HoldingNoteType holdingNoteType) => Execute($"UPDATE diku_mod_inventory_storage.holdings_note_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", holdingNoteType);
        public int Update(HoldingType holdingType) => Execute($"UPDATE diku_mod_inventory_storage.holdings_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", holdingType);
        public int Update(HridSetting hridSetting) => Execute($"UPDATE diku_mod_inventory_storage.hrid_settings SET jsonb = @Content::jsonb, lock = @Lock WHERE id = @Id", hridSetting);
        public int Update(IdType idType) => Execute($"UPDATE diku_mod_inventory_storage.identifier_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", idType);
        public int Update(IllPolicy illPolicy) => Execute($"UPDATE diku_mod_inventory_storage.ill_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", illPolicy);
        public int Update(Instance instance) => Execute($"UPDATE diku_mod_inventory_storage.instance SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, instancestatusid = @Instancestatusid, modeofissuanceid = @Modeofissuanceid, instancetypeid = @Instancetypeid WHERE id = @Id", instance);
        public int Update(InstanceFormat instanceFormat) => Execute($"UPDATE diku_mod_inventory_storage.instance_format SET jsonb = @Content::jsonb WHERE id = @Id", instanceFormat);
        public int Update(InstanceNoteType instanceNoteType) => Execute($"UPDATE diku_mod_inventory_storage.instance_note_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", instanceNoteType);
        public int Update(InstanceRelationship instanceRelationship) => Execute($"UPDATE diku_mod_inventory_storage.instance_relationship SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, superinstanceid = @Superinstanceid, subinstanceid = @Subinstanceid, instancerelationshiptypeid = @Instancerelationshiptypeid WHERE id = @Id", instanceRelationship);
        public int Update(InstanceRelationshipType instanceRelationshipType) => Execute($"UPDATE diku_mod_inventory_storage.instance_relationship_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", instanceRelationshipType);
        public int Update(InstanceSourceMarc instanceSourceMarc) => Execute($"UPDATE diku_mod_inventory_storage.instance_source_marc SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", instanceSourceMarc);
        public int Update(InstanceStatus instanceStatus) => Execute($"UPDATE diku_mod_inventory_storage.instance_status SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", instanceStatus);
        public int Update(InstanceType instanceType) => Execute($"UPDATE diku_mod_inventory_storage.instance_type SET jsonb = @Content::jsonb WHERE id = @Id", instanceType);
        public int Update(Institution institution) => Execute($"UPDATE diku_mod_inventory_storage.locinstitution SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", institution);
        public int Update(Interface @interface) => Execute($"UPDATE diku_mod_organizations_storage.interfaces SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", @interface);
        public int Update(InterfaceCredential interfaceCredential) => Execute($"UPDATE diku_mod_organizations_storage.interface_credentials SET jsonb = @Content::jsonb, interfaceid = @Interfaceid WHERE id = @Id", interfaceCredential);
        public int Update(Invoice invoice) => Execute($"UPDATE diku_mod_invoice_storage.invoices SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, batchgroupid = @Batchgroupid WHERE id = @Id", invoice);
        public int Update(InvoiceItem invoiceItem) => Execute($"UPDATE diku_mod_invoice_storage.invoice_lines SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, invoiceid = @Invoiceid WHERE id = @Id", invoiceItem);
        public int Update(InvoiceTransactionSummary invoiceTransactionSummary) => Execute($"UPDATE diku_mod_finance_storage.invoice_transaction_summaries SET jsonb = @Content::jsonb WHERE id = @Id", invoiceTransactionSummary);
        public int Update(Item item) => Execute($"UPDATE diku_mod_inventory_storage.item SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, holdingsrecordid = @Holdingsrecordid, permanentloantypeid = @Permanentloantypeid, temporaryloantypeid = @Temporaryloantypeid, materialtypeid = @Materialtypeid, permanentlocationid = @Permanentlocationid, temporarylocationid = @Temporarylocationid, effectivelocationid = @Effectivelocationid WHERE id = @Id", item);
        public int Update(ItemDamagedStatus itemDamagedStatus) => Execute($"UPDATE diku_mod_inventory_storage.item_damaged_status SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", itemDamagedStatus);
        public int Update(ItemNoteType itemNoteType) => Execute($"UPDATE diku_mod_inventory_storage.item_note_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", itemNoteType);
        public int Update(JobExecution jobExecution) => Execute($"UPDATE diku_mod_source_record_manager.job_executions SET jsonb = @Content::jsonb WHERE id = @Id", jobExecution);
        public int Update(JobExecutionProgress jobExecutionProgress) => Execute($"UPDATE diku_mod_source_record_manager.job_execution_progress SET jsonb = @Content::jsonb, jobexecutionid = @Jobexecutionid WHERE id = @Id", jobExecutionProgress);
        public int Update(JobExecutionSourceChunk jobExecutionSourceChunk) => Execute($"UPDATE diku_mod_source_record_manager.job_execution_source_chunks SET jsonb = @Content::jsonb, jobexecutionid = @Jobexecutionid WHERE id = @Id", jobExecutionSourceChunk);
        public int Update(JournalRecord journalRecord) => Execute($"UPDATE diku_mod_source_record_manager.journal_records SET job_execution_id = @JobExecutionId, source_id = @SourceId, entity_type = @EntityType, entity_id = @EntityId, entity_hrid = @EntityHrid, action_type = @ActionType, action_status = @ActionStatus, action_date = @ActionDate, source_record_order = @SourceRecordOrder, error = @Error WHERE id = @Id", journalRecord);
        public int Update(Ledger ledger) => Execute($"UPDATE diku_mod_finance_storage.ledger SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, fiscalyearoneid = @Fiscalyearoneid WHERE id = @Id", ledger);
        public int Update(Library library) => Execute($"UPDATE diku_mod_inventory_storage.loclibrary SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, campusid = @Campusid WHERE id = @Id", library);
        public int Update(Loan loan) => Execute($"UPDATE diku_mod_circulation_storage.loan SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", loan);
        public int Update(LoanPolicy loanPolicy) => Execute($"UPDATE diku_mod_circulation_storage.loan_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, loanspolicy_fixedduedatescheduleid = @LoanspolicyFixedduedatescheduleid, renewalspolicy_alternatefixedduedatescheduleid = @RenewalspolicyAlternatefixedduedatescheduleid WHERE id = @Id", loanPolicy);
        public int Update(LoanType loanType) => Execute($"UPDATE diku_mod_inventory_storage.loan_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", loanType);
        public int Update(Location location) => Execute($"UPDATE diku_mod_inventory_storage.location SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, institutionid = @Institutionid, campusid = @Campusid, libraryid = @Libraryid WHERE id = @Id", location);
        public int Update(Login login) => Execute($"UPDATE diku_mod_login.auth_credentials SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", login);
        public int Update(LostItemFeePolicy lostItemFeePolicy) => Execute($"UPDATE diku_mod_feesfines.lost_item_fee_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", lostItemFeePolicy);
        public int Update(MappingRule mappingRule) => Execute($"UPDATE diku_mod_source_record_manager.mapping_rules SET jsonb = @Content::jsonb WHERE id = @Id", mappingRule);
        public int Update(MarcRecord marcRecord) => Execute($"UPDATE diku_mod_source_record_storage.marc_records_lb SET content = @Content::jsonb WHERE id = @Id", marcRecord);
        public int Update(MaterialType materialType) => Execute($"UPDATE diku_mod_inventory_storage.material_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", materialType);
        public int Update(ModeOfIssuance modeOfIssuance) => Execute($"UPDATE diku_mod_inventory_storage.mode_of_issuance SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", modeOfIssuance);
        public int Update(NatureOfContentTerm natureOfContentTerm) => Execute($"UPDATE diku_mod_inventory_storage.nature_of_content_term SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", natureOfContentTerm);
        public int Update(Note note) => Execute($"UPDATE diku_mod_notes.note_data SET jsonb = @Content::jsonb, temporary_type_id = @TemporaryTypeId WHERE id = @Id", note);
        public int Update(NoteType noteType) => Execute($"UPDATE diku_mod_notes.note_type SET jsonb = @Content::jsonb WHERE id = @Id", noteType);
        public int Update(Order order) => Execute($"UPDATE diku_mod_orders_storage.purchase_order SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", order);
        public int Update(OrderInvoice orderInvoice) => Execute($"UPDATE diku_mod_orders_storage.order_invoice_relationship SET jsonb = @Content::jsonb, purchaseorderid = @Purchaseorderid WHERE id = @Id", orderInvoice);
        public int Update(OrderItem orderItem) => Execute($"UPDATE diku_mod_orders_storage.po_line SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, purchaseorderid = @Purchaseorderid WHERE id = @Id", orderItem);
        public int Update(OrderTemplate orderTemplate) => Execute($"UPDATE diku_mod_orders_storage.order_templates SET jsonb = @Content::jsonb WHERE id = @Id", orderTemplate);
        public int Update(OrderTransactionSummary orderTransactionSummary) => Execute($"UPDATE diku_mod_finance_storage.order_transaction_summaries SET jsonb = @Content::jsonb WHERE id = @Id", orderTransactionSummary);
        public int Update(Organization organization) => Execute($"UPDATE diku_mod_organizations_storage.organizations SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", organization);
        public int Update(OverdueFinePolicy overdueFinePolicy) => Execute($"UPDATE diku_mod_feesfines.overdue_fine_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", overdueFinePolicy);
        public int Update(Owner owner) => Execute($"UPDATE diku_mod_feesfines.owners SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", owner);
        public int Update(PatronActionSession patronActionSession) => Execute($"UPDATE diku_mod_circulation_storage.patron_action_session SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", patronActionSession);
        public int Update(PatronNoticePolicy patronNoticePolicy) => Execute($"UPDATE diku_mod_circulation_storage.patron_notice_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", patronNoticePolicy);
        public int Update(Payment payment) => Execute($"UPDATE diku_mod_feesfines.feefineactions SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", payment);
        public int Update(PaymentMethod paymentMethod) => Execute($"UPDATE diku_mod_feesfines.payments SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", paymentMethod);
        public int Update(Permission permission) => Execute($"UPDATE diku_mod_permissions.permissions SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", permission);
        public int Update(PermissionsUser permissionsUser) => Execute($"UPDATE diku_mod_permissions.permissions_users SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", permissionsUser);
        public int Update(PrecedingSucceedingTitle precedingSucceedingTitle) => Execute($"UPDATE diku_mod_inventory_storage.preceding_succeeding_title SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, precedinginstanceid = @Precedinginstanceid, succeedinginstanceid = @Succeedinginstanceid WHERE id = @Id", precedingSucceedingTitle);
        public int Update(Prefix prefix) => Execute($"UPDATE diku_mod_orders_storage.prefixes SET jsonb = @Content::jsonb WHERE id = @Id", prefix);
        public int Update(Proxy proxy) => Execute($"UPDATE diku_mod_users.proxyfor SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", proxy);
        public int Update(RawRecord rawRecord) => Execute($"UPDATE diku_mod_source_record_storage.raw_records_lb SET content = @Content WHERE id = @Id", rawRecord);
        public int Update(Receiving receiving) => Execute($"UPDATE diku_mod_orders_storage.pieces SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, polineid = @Polineid, titleid = @Titleid WHERE id = @Id", receiving);
        public int Update(Record record, DateTime? lastWriteTime = null) => Execute($"UPDATE diku_mod_source_record_storage.records_lb SET snapshot_id = @SnapshotId, matched_id = @MatchedId, generation = @Generation, record_type = @RecordType, instance_id = @InstanceId, state = @State, leader_record_status = @LeaderRecordStatus, order = @Order, suppress_discovery = @SuppressDiscovery, created_by_user_id = @CreationUserId, created_date = @CreationTime, updated_by_user_id = @LastWriteUserId, updated_date = @LastWriteTime, instance_hrid = @InstanceHrid WHERE id = @Id{(lastWriteTime != null ? " AND (updated_date IS NULL OR updated_date = @lastWriteTime)" : "")}", new { record.SnapshotId, record.MatchedId, record.Generation, record.RecordType, record.InstanceId, record.State, record.LeaderRecordStatus, record.Order, record.SuppressDiscovery, record.CreationUserId, record.CreationTime, record.LastWriteUserId, record.LastWriteTime, record.InstanceHrid, record.Id, lastWriteTime });
        public int Update(RefundReason refundReason) => Execute($"UPDATE diku_mod_feesfines.refunds SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", refundReason);
        public int Update(ReportingCode reportingCode) => Execute($"UPDATE diku_mod_orders_storage.reporting_code SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", reportingCode);
        public int Update(Request request) => Execute($"UPDATE diku_mod_circulation_storage.request SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, cancellationreasonid = @Cancellationreasonid WHERE id = @Id", request);
        public int Update(RequestPolicy requestPolicy) => Execute($"UPDATE diku_mod_circulation_storage.request_policy SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", requestPolicy);
        public int Update(ScheduledNotice scheduledNotice) => Execute($"UPDATE diku_mod_circulation_storage.scheduled_notice SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", scheduledNotice);
        public int Update(ServicePoint servicePoint) => Execute($"UPDATE diku_mod_inventory_storage.service_point SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", servicePoint);
        public int Update(ServicePointUser servicePointUser) => Execute($"UPDATE diku_mod_inventory_storage.service_point_user SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, defaultservicepointid = @Defaultservicepointid WHERE id = @Id", servicePointUser);
        public int Update(Snapshot snapshot, DateTime? lastWriteTime = null) => Execute($"UPDATE diku_mod_source_record_storage.snapshots_lb SET status = @Status, processing_started_date = @ProcessingStartedDate, created_by_user_id = @CreationUserId, created_date = @CreationTime, updated_by_user_id = @LastWriteUserId, updated_date = @LastWriteTime WHERE id = @Id{(lastWriteTime != null ? " AND (updated_date IS NULL OR updated_date = @lastWriteTime)" : "")}", new { snapshot.Status, snapshot.ProcessingStartedDate, snapshot.CreationUserId, snapshot.CreationTime, snapshot.LastWriteUserId, snapshot.LastWriteTime, snapshot.Id, lastWriteTime });
        public int Update(Source source) => Execute($"UPDATE diku_mod_inventory_storage.holdings_records_source SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", source);
        public int Update(StaffSlip staffSlip) => Execute($"UPDATE diku_mod_circulation_storage.staff_slips SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", staffSlip);
        public int Update(StatisticalCode statisticalCode) => Execute($"UPDATE diku_mod_inventory_storage.statistical_code SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, statisticalcodetypeid = @Statisticalcodetypeid WHERE id = @Id", statisticalCode);
        public int Update(StatisticalCodeType statisticalCodeType) => Execute($"UPDATE diku_mod_inventory_storage.statistical_code_type SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", statisticalCodeType);
        public int Update(Suffix suffix) => Execute($"UPDATE diku_mod_orders_storage.suffixes SET jsonb = @Content::jsonb WHERE id = @Id", suffix);
        public int Update(Tag tag) => Execute($"UPDATE diku_mod_tags.tags SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", tag);
        public int Update(Template template) => Execute($"UPDATE diku_mod_template_engine.template SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", template);
        public int Update(Title title) => Execute($"UPDATE diku_mod_orders_storage.titles SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, polineid = @Polineid WHERE id = @Id", title);
        public int Update(Transaction transaction) => Execute($"UPDATE diku_mod_finance_storage.transaction SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, fiscalyearid = @Fiscalyearid, fromfundid = @Fromfundid, sourcefiscalyearid = @Sourcefiscalyearid, tofundid = @Tofundid, expenseclassid = @Expenseclassid WHERE id = @Id", transaction);
        public int Update(TransferAccount transferAccount) => Execute($"UPDATE diku_mod_feesfines.transfers SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", transferAccount);
        public int Update(TransferCriteria transferCriteria) => Execute($"UPDATE diku_mod_feesfines.transfer_criteria SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", transferCriteria);
        public int Update(User user) => Execute($"UPDATE diku_mod_users.users SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, patrongroup = @Patrongroup WHERE id = @Id", user);
        public int Update(UserAcquisitionsUnit userAcquisitionsUnit) => Execute($"UPDATE diku_mod_orders_storage.acquisitions_unit_membership SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, acquisitionsunitid = @Acquisitionsunitid WHERE id = @Id", userAcquisitionsUnit);
        public int Update(UserRequestPreference userRequestPreference) => Execute($"UPDATE diku_mod_circulation_storage.user_request_preference SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", userRequestPreference);
        public int Update(UserSummary userSummary) => Execute($"UPDATE diku_mod_patron_blocks.user_summary SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", userSummary);
        public int Update(Voucher voucher) => Execute($"UPDATE diku_mod_invoice_storage.vouchers SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, invoiceid = @Invoiceid, batchgroupid = @Batchgroupid WHERE id = @Id", voucher);
        public int Update(VoucherItem voucherItem) => Execute($"UPDATE diku_mod_invoice_storage.voucher_lines SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId, voucherid = @Voucherid WHERE id = @Id", voucherItem);
        public int Update(WaiveReason waiveReason) => Execute($"UPDATE diku_mod_feesfines.waives SET jsonb = @Content::jsonb, creation_date = @CreationTime, created_by = @CreationUserId WHERE id = @Id", waiveReason);

        public int DeleteAcquisitionsUnit(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit WHERE id = @id", new { id });
        public int DeleteAcquisitionsUnit2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}acquisitions_units WHERE id = @id", new { id });
        public int DeleteAddress(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}addresses WHERE id = @id", new { id });
        public int DeleteAddressType(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}addresstype WHERE id = @id", new { id });
        public int DeleteAddressType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}address_types WHERE id = @id", new { id });
        public int DeleteAlert(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}alert WHERE id = @id", new { id });
        public int DeleteAlert2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}alerts WHERE id = @id", new { id });
        public int DeleteAllocatedFromFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}allocated_from_funds WHERE id = @id", new { id });
        public int DeleteAllocatedToFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}allocated_to_funds WHERE id = @id", new { id });
        public int DeleteAlternativeTitle(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}alternative_titles WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteAlternativeTitleType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}alternative_title_type WHERE id = @id", new { id });
        public int DeleteAlternativeTitleType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}alternative_title_types WHERE id = @id", new { id });
        public int DeleteAuditLoan(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}audit_loan WHERE id = @id", new { id });
        public int DeleteAuthAttempt(Guid? id) => Execute($"DELETE FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_attempts WHERE id = @id", new { id });
        public int DeleteAuthAttempt2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}auth_attempts WHERE id = @id", new { id });
        public int DeleteAuthCredentialsHistory(Guid? id) => Execute($"DELETE FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials_history WHERE id = @id", new { id });
        public int DeleteAuthCredentialsHistory2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}auth_credentials_histories WHERE id = @id", new { id });
        public int DeleteAuthPasswordAction(Guid? id) => Execute($"DELETE FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_password_action WHERE id = @id", new { id });
        public int DeleteBatchGroup(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_groups WHERE id = @id", new { id });
        public int DeleteBatchGroup2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_groups WHERE id = @id", new { id });
        public int DeleteBatchVoucher(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_vouchers WHERE id = @id", new { id });
        public int DeleteBatchVoucher2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_vouchers WHERE id = @id", new { id });
        public int DeleteBatchVoucherBatchedVoucher(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_vouchers WHERE id = @id", new { id });
        public int DeleteBatchVoucherBatchedVoucherBatchedVoucherLine(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_lines WHERE id = @id", new { id });
        public int DeleteBatchVoucherBatchedVoucherBatchedVoucherLineFundCode(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_batched_voucher_batched_voucher_line_fund_codes WHERE id = @id", new { id });
        public int DeleteBatchVoucherExport(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_exports WHERE id = @id", new { id });
        public int DeleteBatchVoucherExport2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_exports WHERE id = @id", new { id });
        public int DeleteBatchVoucherExportConfig(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}batch_voucher_export_configs WHERE id = @id", new { id });
        public int DeleteBatchVoucherExportConfig2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_configs WHERE id = @id", new { id });
        public int DeleteBatchVoucherExportConfigWeekday(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}batch_voucher_export_config_weekdays WHERE id = @id", new { id });
        public int DeleteBlock(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}manualblocks WHERE id = @id", new { id });
        public int DeleteBlock2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}blocks WHERE id = @id", new { id });
        public int DeleteBlockCondition(Guid? id) => Execute($"DELETE FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_conditions WHERE id = @id", new { id });
        public int DeleteBlockCondition2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}block_conditions WHERE id = @id", new { id });
        public int DeleteBlockLimit(Guid? id) => Execute($"DELETE FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}patron_block_limits WHERE id = @id", new { id });
        public int DeleteBlockLimit2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}block_limits WHERE id = @id", new { id });
        public int DeleteBudget(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget WHERE id = @id", new { id });
        public int DeleteBudget2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}budgets WHERE id = @id", new { id });
        public int DeleteBudgetAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}budget_acquisitions_units WHERE id = @id", new { id });
        public int DeleteBudgetExpenseClass(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}budget_expense_class WHERE id = @id", new { id });
        public int DeleteBudgetExpenseClass2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}budget_expense_classes WHERE id = @id", new { id });
        public int DeleteBudgetTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}budget_tags WHERE id = @id", new { id });
        public int DeleteCallNumberType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}call_number_type WHERE id = @id", new { id });
        public int DeleteCallNumberType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}call_number_types WHERE id = @id", new { id });
        public int DeleteCampus(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loccampus WHERE id = @id", new { id });
        public int DeleteCampus2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}campuses WHERE id = @id", new { id });
        public int DeleteCancellationReason(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}cancellation_reason WHERE id = @id", new { id });
        public int DeleteCancellationReason2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}cancellation_reasons WHERE id = @id", new { id });
        public int DeleteCategory(Guid? id) => Execute($"DELETE FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}categories WHERE id = @id", new { id });
        public int DeleteCategory2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}categories WHERE id = @id", new { id });
        public int DeleteCheckIn(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}check_in WHERE id = @id", new { id });
        public int DeleteCheckIn2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}check_ins WHERE id = @id", new { id });
        public int DeleteCirculationNote(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}circulation_notes WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteCirculationRule(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}circulation_rules WHERE id = @id", new { id });
        public int DeleteCirculationRule2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}circulation_rules WHERE id = @id", new { id });
        public int DeleteClassification(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}classifications WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteClassificationType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}classification_type WHERE id = @id", new { id });
        public int DeleteClassificationType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}classification_types WHERE id = @id", new { id });
        public int DeleteCloseReason(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reasons_for_closure WHERE id = @id", new { id });
        public int DeleteCloseReason2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}close_reasons WHERE id = @id", new { id });
        public int DeleteComment(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}comments WHERE id = @id", new { id });
        public int DeleteComment2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}comments WHERE id = @id", new { id });
        public int DeleteConfiguration(Guid? id) => Execute($"DELETE FROM diku_mod_configuration{(IsMySql ? "_" : ".")}config_data WHERE id = @id", new { id });
        public int DeleteConfiguration2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}configurations WHERE id = @id", new { id });
        public int DeleteContact(Guid? id) => Execute($"DELETE FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}contacts WHERE id = @id", new { id });
        public int DeleteContact2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contacts WHERE id = @id", new { id });
        public int DeleteContactAddress(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_addresses WHERE id = @id", new { id });
        public int DeleteContactAddressCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_address_categories WHERE id = @id", new { id });
        public int DeleteContactCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_categories WHERE id = @id", new { id });
        public int DeleteContactEmail(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_emails WHERE id = @id", new { id });
        public int DeleteContactEmailCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_email_categories WHERE id = @id", new { id });
        public int DeleteContactPhoneNumber(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_phone_numbers WHERE id = @id", new { id });
        public int DeleteContactPhoneNumberCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_phone_number_categories WHERE id = @id", new { id });
        public int DeleteContactType(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_types WHERE id = @id", new { id });
        public int DeleteContactUrl(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_urls WHERE id = @id", new { id });
        public int DeleteContactUrlCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contact_url_categories WHERE id = @id", new { id });
        public int DeleteContributor(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contributors WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteContributorNameType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_name_type WHERE id = @id", new { id });
        public int DeleteContributorNameType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contributor_name_types WHERE id = @id", new { id });
        public int DeleteContributorType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}contributor_type WHERE id = @id", new { id });
        public int DeleteContributorType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}contributor_types WHERE id = @id", new { id });
        public int DeleteCountry(string alpha2Code) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}countries WHERE alpha2_code = @alpha2Code", new { alpha2Code });
        public int DeleteCurrency(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}currencies WHERE id = @id", new { id });
        public int DeleteCustomField(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}custom_fields WHERE id = @id", new { id });
        public int DeleteCustomField2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}custom_fields WHERE id = @id", new { id });
        public int DeleteCustomFieldValue(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}custom_field_values WHERE id = @id", new { id });
        public int DeleteDepartment(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}departments WHERE id = @id", new { id });
        public int DeleteDepartment2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}departments WHERE id = @id", new { id });
        public int DeleteDocument(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}documents WHERE id = @id", new { id });
        public int DeleteDocument2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}documents WHERE id = @id", new { id });
        public int DeleteEdition(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}editions WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteElectronicAccess(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}electronic_accesses WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteElectronicAccessRelationship(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}electronic_access_relationship WHERE id = @id", new { id });
        public int DeleteElectronicAccessRelationship2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}electronic_access_relationships WHERE id = @id", new { id });
        public int DeleteErrorRecord(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}error_records_lb WHERE id = @id", new { id });
        public int DeleteErrorRecord2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}error_records WHERE id = @id", new { id });
        public int DeleteEventLog(Guid? id) => Execute($"DELETE FROM diku_mod_login{(IsMySql ? "_" : ".")}event_logs WHERE id = @id", new { id });
        public int DeleteEventLog2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}event_logs WHERE id = @id", new { id });
        public int DeleteExpenseClass(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}expense_class WHERE id = @id", new { id });
        public int DeleteExpenseClass2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}expense_classes WHERE id = @id", new { id });
        public int DeleteExportConfigCredential(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}export_config_credentials WHERE id = @id", new { id });
        public int DeleteExportConfigCredential2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}export_config_credentials WHERE id = @id", new { id });
        public int DeleteExtent(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}extents WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteFee(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}accounts WHERE id = @id", new { id });
        public int DeleteFee2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fees WHERE id = @id", new { id });
        public int DeleteFeeType(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefines WHERE id = @id", new { id });
        public int DeleteFeeType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fee_types WHERE id = @id", new { id });
        public int DeleteFinanceGroup(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id });
        public int DeleteFinanceGroup2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}finance_groups WHERE id = @id", new { id });
        public int DeleteFinanceGroupAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}finance_group_acquisitions_units WHERE id = @id", new { id });
        public int DeleteFiscalYear(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fiscal_year WHERE id = @id", new { id });
        public int DeleteFiscalYear2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fiscal_years WHERE id = @id", new { id });
        public int DeleteFiscalYearAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fiscal_year_acquisitions_units WHERE id = @id", new { id });
        public int DeleteFixedDueDateSchedule(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}fixed_due_date_schedule WHERE id = @id", new { id });
        public int DeleteFixedDueDateSchedule2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedules WHERE id = @id", new { id });
        public int DeleteFixedDueDateScheduleSchedule(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fixed_due_date_schedule_schedules WHERE id = @id", new { id });
        public int DeleteFormat(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}formats WHERE id = @id", new { id });
        public int DeleteFund(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund WHERE id = @id", new { id });
        public int DeleteFund2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}funds WHERE id = @id", new { id });
        public int DeleteFundAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fund_acquisitions_units WHERE id = @id", new { id });
        public int DeleteFundTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fund_tags WHERE id = @id", new { id });
        public int DeleteFundType(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}fund_type WHERE id = @id", new { id });
        public int DeleteFundType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}fund_types WHERE id = @id", new { id });
        public int DeleteGroup(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id });
        public int DeleteGroup2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}groups WHERE id = @id", new { id });
        public int DeleteGroupFundFiscalYear(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}group_fund_fiscal_year WHERE id = @id", new { id });
        public int DeleteGroupFundFiscalYear2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}group_fund_fiscal_years WHERE id = @id", new { id });
        public int DeleteHolding(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_record WHERE id = @id", new { id });
        public int DeleteHolding2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holdings WHERE id = @id", new { id });
        public int DeleteHoldingElectronicAccess(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_electronic_accesses WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingEntry(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_entries WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingFormerId(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_former_ids WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingNote(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_notes WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingNoteType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_note_type WHERE id = @id", new { id });
        public int DeleteHoldingNoteType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_note_types WHERE id = @id", new { id });
        public int DeleteHoldingStatisticalCode(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_statistical_codes WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingTag(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_tags WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteHoldingType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_type WHERE id = @id", new { id });
        public int DeleteHoldingType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}holding_types WHERE id = @id", new { id });
        public int DeleteHridSetting(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}hrid_settings WHERE id = @id", new { id });
        public int DeleteHridSetting2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}hrid_settings WHERE id = @id", new { id });
        public int DeleteIdentifier(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}identifiers WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteIdType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}identifier_type WHERE id = @id", new { id });
        public int DeleteIdType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}id_types WHERE id = @id", new { id });
        public int DeleteIllPolicy(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}ill_policy WHERE id = @id", new { id });
        public int DeleteIllPolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}ill_policies WHERE id = @id", new { id });
        public int DeleteIndexStatement(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}index_statements WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteInstance(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance WHERE id = @id", new { id });
        public int DeleteInstance2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instances WHERE id = @id", new { id });
        public int DeleteInstanceFormat(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_format WHERE id = @id", new { id });
        public int DeleteInstanceFormat2(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_formats WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteInstanceNatureOfContentTerm(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_nature_of_content_terms WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteInstanceNoteType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_note_type WHERE id = @id", new { id });
        public int DeleteInstanceNoteType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_note_types WHERE id = @id", new { id });
        public int DeleteInstanceRelationship(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship WHERE id = @id", new { id });
        public int DeleteInstanceRelationshipType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_relationship_type WHERE id = @id", new { id });
        public int DeleteInstanceSourceMarc(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_source_marc WHERE id = @id", new { id });
        public int DeleteInstanceStatisticalCode(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_statistical_codes WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteInstanceStatus(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_status WHERE id = @id", new { id });
        public int DeleteInstanceTag(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_tags WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteInstanceType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}instance_type WHERE id = @id", new { id });
        public int DeleteInstanceType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}instance_types WHERE id = @id", new { id });
        public int DeleteInstitution(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}locinstitution WHERE id = @id", new { id });
        public int DeleteInstitution2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}institutions WHERE id = @id", new { id });
        public int DeleteInterface(Guid? id) => Execute($"DELETE FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interfaces WHERE id = @id", new { id });
        public int DeleteInterface2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}interfaces WHERE id = @id", new { id });
        public int DeleteInterfaceCredential(Guid? id) => Execute($"DELETE FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}interface_credentials WHERE id = @id", new { id });
        public int DeleteInterfaceCredential2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}interface_credentials WHERE id = @id", new { id });
        public int DeleteInterfaceType(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}interface_type WHERE id = @id", new { id });
        public int DeleteInvoice(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoices WHERE id = @id", new { id });
        public int DeleteInvoice2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoices WHERE id = @id", new { id });
        public int DeleteInvoiceAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_acquisitions_units WHERE id = @id", new { id });
        public int DeleteInvoiceAdjustment(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_adjustments WHERE id = @id", new { id });
        public int DeleteInvoiceAdjustmentFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_adjustment_fund_distributions WHERE id = @id", new { id });
        public int DeleteInvoiceItem(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}invoice_lines WHERE id = @id", new { id });
        public int DeleteInvoiceItem2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_items WHERE id = @id", new { id });
        public int DeleteInvoiceItemAdjustment(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustments WHERE id = @id", new { id });
        public int DeleteInvoiceItemAdjustmentFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_item_adjustment_fund_distributions WHERE id = @id", new { id });
        public int DeleteInvoiceItemFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_item_fund_distributions WHERE id = @id", new { id });
        public int DeleteInvoiceItemTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_item_tags WHERE id = @id", new { id });
        public int DeleteInvoiceOrderNumber(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_order_numbers WHERE id = @id", new { id });
        public int DeleteInvoiceTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_tags WHERE id = @id", new { id });
        public int DeleteInvoiceTransactionSummary(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}invoice_transaction_summaries WHERE id = @id", new { id });
        public int DeleteInvoiceTransactionSummary2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}invoice_transaction_summaries WHERE id = @id", new { id });
        public int DeleteIssuanceMode(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}mode_of_issuances WHERE id = @id", new { id });
        public int DeleteItem(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item WHERE id = @id", new { id });
        public int DeleteItem2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}items WHERE id = @id", new { id });
        public int DeleteItemDamagedStatus(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_damaged_status WHERE id = @id", new { id });
        public int DeleteItemDamagedStatus2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_damaged_statuses WHERE id = @id", new { id });
        public int DeleteItemElectronicAccess(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_electronic_accesses WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteItemFormerId(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_former_ids WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteItemNote(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_notes WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteItemNoteType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}item_note_type WHERE id = @id", new { id });
        public int DeleteItemNoteType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_note_types WHERE id = @id", new { id });
        public int DeleteItemStatisticalCode(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_statistical_codes WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteItemTag(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_tags WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteItemYearCaption(string id, Guid? itemId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}item_year_caption WHERE id = @id AND item_id = @itemId", new { id, itemId });
        public int DeleteJobExecution(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_executions WHERE id = @id", new { id });
        public int DeleteJobExecution2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}job_executions WHERE id = @id", new { id });
        public int DeleteJobExecutionProgress(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_progress WHERE id = @id", new { id });
        public int DeleteJobExecutionSourceChunk(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}job_execution_source_chunks WHERE id = @id", new { id });
        public int DeleteJobExecutionSourceChunk2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}job_execution_source_chunks WHERE id = @id", new { id });
        public int DeleteJournalRecord(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}journal_records WHERE id = @id", new { id });
        public int DeleteJournalRecord2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}journal_records WHERE id = @id", new { id });
        public int DeleteLanguage(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}languages WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteLedger(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}ledger WHERE id = @id", new { id });
        public int DeleteLedger2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}ledgers WHERE id = @id", new { id });
        public int DeleteLedgerAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}ledger_acquisitions_units WHERE id = @id", new { id });
        public int DeleteLibrary(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loclibrary WHERE id = @id", new { id });
        public int DeleteLibrary2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}libraries WHERE id = @id", new { id });
        public int DeleteLoan(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan WHERE id = @id", new { id });
        public int DeleteLoan2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}loans WHERE id = @id", new { id });
        public int DeleteLoanPolicy(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}loan_policy WHERE id = @id", new { id });
        public int DeleteLoanPolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}loan_policies WHERE id = @id", new { id });
        public int DeleteLoanType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}loan_type WHERE id = @id", new { id });
        public int DeleteLoanType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}loan_types WHERE id = @id", new { id });
        public int DeleteLocation(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}location WHERE id = @id", new { id });
        public int DeleteLocation2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}locations WHERE id = @id", new { id });
        public int DeleteLocationServicePoint(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}location_service_points WHERE id = @id", new { id });
        public int DeleteLocationSetting(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}location_settings WHERE id = @id", new { id });
        public int DeleteLogin(Guid? id) => Execute($"DELETE FROM diku_mod_login{(IsMySql ? "_" : ".")}auth_credentials WHERE id = @id", new { id });
        public int DeleteLogin2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}logins WHERE id = @id", new { id });
        public int DeleteLostItemFeePolicy(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}lost_item_fee_policy WHERE id = @id", new { id });
        public int DeleteLostItemFeePolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}lost_item_fee_policies WHERE id = @id", new { id });
        public int DeleteMappingRule(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_manager{(IsMySql ? "_" : ".")}mapping_rules WHERE id = @id", new { id });
        public int DeleteMarcRecord(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}marc_records_lb WHERE id = @id", new { id });
        public int DeleteMarcRecord2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}marc_records WHERE id = @id", new { id });
        public int DeleteMaterialType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}material_type WHERE id = @id", new { id });
        public int DeleteMaterialType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}material_types WHERE id = @id", new { id });
        public int DeleteModeOfIssuance(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}mode_of_issuance WHERE id = @id", new { id });
        public int DeleteNatureOfContentTerm(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}nature_of_content_term WHERE id = @id", new { id });
        public int DeleteNatureOfContentTerm2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}nature_of_content_terms WHERE id = @id", new { id });
        public int DeleteNote(Guid? id) => Execute($"DELETE FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_data WHERE id = @id", new { id });
        public int DeleteNote2(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}notes WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteNote3(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}notes2 WHERE id = @id", new { id });
        public int DeleteNoteLink(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}note_links WHERE id = @id", new { id });
        public int DeleteNoteType(Guid? id) => Execute($"DELETE FROM diku_mod_notes{(IsMySql ? "_" : ".")}note_type WHERE id = @id", new { id });
        public int DeleteNoteType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}note_types WHERE id = @id", new { id });
        public int DeleteOrder(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}purchase_order WHERE id = @id", new { id });
        public int DeleteOrder2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}orders WHERE id = @id", new { id });
        public int DeleteOrderAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_acquisitions_units WHERE id = @id", new { id });
        public int DeleteOrderInvoice(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_invoice_relationship WHERE id = @id", new { id });
        public int DeleteOrderInvoice2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_invoices WHERE id = @id", new { id });
        public int DeleteOrderItem(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}po_line WHERE id = @id", new { id });
        public int DeleteOrderItem2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_items WHERE id = @id", new { id });
        public int DeleteOrderItemAlert(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_alerts WHERE id = @id", new { id });
        public int DeleteOrderItemClaim(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_claims WHERE id = @id", new { id });
        public int DeleteOrderItemContributor(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_contributors WHERE id = @id", new { id });
        public int DeleteOrderItemFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_fund_distributions WHERE id = @id", new { id });
        public int DeleteOrderItemLocation2(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_locations WHERE id = @id", new { id });
        public int DeleteOrderItemProductId(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_product_ids WHERE id = @id", new { id });
        public int DeleteOrderItemReportingCode(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_reporting_codes WHERE id = @id", new { id });
        public int DeleteOrderItemTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_tags WHERE id = @id", new { id });
        public int DeleteOrderItemVolume(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_item_volumes WHERE id = @id", new { id });
        public int DeleteOrderNote(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_notes WHERE id = @id", new { id });
        public int DeleteOrderTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_tags WHERE id = @id", new { id });
        public int DeleteOrderTemplate(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}order_templates WHERE id = @id", new { id });
        public int DeleteOrderTemplate2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_templates WHERE id = @id", new { id });
        public int DeleteOrderTransactionSummary(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}order_transaction_summaries WHERE id = @id", new { id });
        public int DeleteOrderTransactionSummary2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}order_transaction_summaries WHERE id = @id", new { id });
        public int DeleteOrganization(Guid? id) => Execute($"DELETE FROM diku_mod_organizations_storage{(IsMySql ? "_" : ".")}organizations WHERE id = @id", new { id });
        public int DeleteOrganization2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organizations WHERE id = @id", new { id });
        public int DeleteOrganizationAccount(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_accounts WHERE id = @id", new { id });
        public int DeleteOrganizationAccountAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_account_acquisitions_units WHERE id = @id", new { id });
        public int DeleteOrganizationAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_acquisitions_units WHERE id = @id", new { id });
        public int DeleteOrganizationAddress(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_addresses WHERE id = @id", new { id });
        public int DeleteOrganizationAddressCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_address_categories WHERE id = @id", new { id });
        public int DeleteOrganizationAgreement(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_agreements WHERE id = @id", new { id });
        public int DeleteOrganizationAlias(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_aliases WHERE id = @id", new { id });
        public int DeleteOrganizationChangelog(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_changelogs WHERE id = @id", new { id });
        public int DeleteOrganizationContact(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_contacts WHERE id = @id", new { id });
        public int DeleteOrganizationEmail(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_emails WHERE id = @id", new { id });
        public int DeleteOrganizationEmailCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_email_categories WHERE id = @id", new { id });
        public int DeleteOrganizationInterface(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_interfaces WHERE id = @id", new { id });
        public int DeleteOrganizationPhoneNumber(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_phone_numbers WHERE id = @id", new { id });
        public int DeleteOrganizationPhoneNumberCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_phone_number_categories WHERE id = @id", new { id });
        public int DeleteOrganizationTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_tags WHERE id = @id", new { id });
        public int DeleteOrganizationUrl(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_urls WHERE id = @id", new { id });
        public int DeleteOrganizationUrlCategory(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}organization_url_categories WHERE id = @id", new { id });
        public int DeleteOverdueFinePolicy(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}overdue_fine_policy WHERE id = @id", new { id });
        public int DeleteOverdueFinePolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}overdue_fine_policies WHERE id = @id", new { id });
        public int DeleteOwner(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}owners WHERE id = @id", new { id });
        public int DeleteOwner2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}owners WHERE id = @id", new { id });
        public int DeletePatronActionSession(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_action_session WHERE id = @id", new { id });
        public int DeletePatronActionSession2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}patron_action_sessions WHERE id = @id", new { id });
        public int DeletePatronNoticePolicy(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}patron_notice_policy WHERE id = @id", new { id });
        public int DeletePatronNoticePolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}patron_notice_policies WHERE id = @id", new { id });
        public int DeletePatronNoticePolicyFeeFineNotice(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_fee_fine_notices WHERE id = @id", new { id });
        public int DeletePatronNoticePolicyLoanNotice(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_loan_notices WHERE id = @id", new { id });
        public int DeletePatronNoticePolicyRequestNotice(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}patron_notice_policy_request_notices WHERE id = @id", new { id });
        public int DeletePayment(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}feefineactions WHERE id = @id", new { id });
        public int DeletePayment2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}payments WHERE id = @id", new { id });
        public int DeletePaymentMethod(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}payments WHERE id = @id", new { id });
        public int DeletePaymentMethod2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}payment_methods WHERE id = @id", new { id });
        public int DeletePermission(Guid? id) => Execute($"DELETE FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions WHERE id = @id", new { id });
        public int DeletePermission2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permissions WHERE id = @id", new { id });
        public int DeletePermissionChildOf(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permission_child_of WHERE id = @id", new { id });
        public int DeletePermissionGrantedTo(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permission_granted_to WHERE id = @id", new { id });
        public int DeletePermissionSubPermission(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permission_sub_permissions WHERE id = @id", new { id });
        public int DeletePermissionsUser(Guid? id) => Execute($"DELETE FROM diku_mod_permissions{(IsMySql ? "_" : ".")}permissions_users WHERE id = @id", new { id });
        public int DeletePermissionsUser2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permissions_users WHERE id = @id", new { id });
        public int DeletePermissionsUserPermission(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permissions_user_permissions WHERE id = @id", new { id });
        public int DeletePermissionTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}permission_tags WHERE id = @id", new { id });
        public int DeletePhysicalDescription(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}physical_descriptions WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeletePrecedingSucceedingTitle(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}preceding_succeeding_title WHERE id = @id", new { id });
        public int DeletePrecedingSucceedingTitle2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_titles WHERE id = @id", new { id });
        public int DeletePrecedingSucceedingTitleIdentifier(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}preceding_succeeding_title_identifiers WHERE id = @id", new { id });
        public int DeletePrefix(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}prefixes WHERE id = @id", new { id });
        public int DeletePrefix2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}prefixes WHERE id = @id", new { id });
        public int DeletePrinter(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}printers WHERE id = @id", new { id });
        public int DeleteProxy(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}proxyfor WHERE id = @id", new { id });
        public int DeleteProxy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}proxies WHERE id = @id", new { id });
        public int DeletePublication(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}publications WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeletePublicationFrequency(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}publication_frequency WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeletePublicationRange(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}publication_range WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteRawRecord(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}raw_records_lb WHERE id = @id", new { id });
        public int DeleteRawRecord2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}raw_records WHERE id = @id", new { id });
        public int DeleteReceiving(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}pieces WHERE id = @id", new { id });
        public int DeleteReceiving2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}receivings WHERE id = @id", new { id });
        public int DeleteRecord(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}records_lb WHERE id = @id", new { id });
        public int DeleteRecord2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}records WHERE id = @id", new { id });
        public int DeleteRefundReason(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}refunds WHERE id = @id", new { id });
        public int DeleteRefundReason2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}refund_reasons WHERE id = @id", new { id });
        public int DeleteRelationship(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}relationships WHERE id = @id", new { id });
        public int DeleteRelationshipType(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}relationship_types WHERE id = @id", new { id });
        public int DeleteReportingCode(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}reporting_code WHERE id = @id", new { id });
        public int DeleteReportingCode2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}reporting_codes WHERE id = @id", new { id });
        public int DeleteRequest(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request WHERE id = @id", new { id });
        public int DeleteRequest2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}requests WHERE id = @id", new { id });
        public int DeleteRequestIdentifier(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}request_identifiers WHERE id = @id", new { id });
        public int DeleteRequestPolicy(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}request_policy WHERE id = @id", new { id });
        public int DeleteRequestPolicy2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}request_policies WHERE id = @id", new { id });
        public int DeleteRequestPolicyRequestType(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}request_policy_request_types WHERE id = @id", new { id });
        public int DeleteRequestTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}request_tags WHERE id = @id", new { id });
        public int DeleteScheduledNotice(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}scheduled_notice WHERE id = @id", new { id });
        public int DeleteScheduledNotice2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}scheduled_notices WHERE id = @id", new { id });
        public int DeleteSeries(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}series WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteServicePoint(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point WHERE id = @id", new { id });
        public int DeleteServicePoint2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}service_points WHERE id = @id", new { id });
        public int DeleteServicePointOwner(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}service_point_owners WHERE id = @id", new { id });
        public int DeleteServicePointStaffSlip(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}service_point_staff_slips WHERE id = @id", new { id });
        public int DeleteServicePointUser(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}service_point_user WHERE id = @id", new { id });
        public int DeleteServicePointUser2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}service_point_users WHERE id = @id", new { id });
        public int DeleteServicePointUserServicePoint(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}service_point_user_service_points WHERE id = @id", new { id });
        public int DeleteSetting(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}settings WHERE id = @id", new { id });
        public int DeleteSnapshot(Guid? id) => Execute($"DELETE FROM diku_mod_source_record_storage{(IsMySql ? "_" : ".")}snapshots_lb WHERE id = @id", new { id });
        public int DeleteSnapshot2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}snapshots WHERE id = @id", new { id });
        public int DeleteSource(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}holdings_records_source WHERE id = @id", new { id });
        public int DeleteSource2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}sources WHERE id = @id", new { id });
        public int DeleteSourceMarc(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}source_marcs WHERE id = @id", new { id });
        public int DeleteSourceMarcField(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}source_marc_fields WHERE id = @id", new { id });
        public int DeleteStaffSlip(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}staff_slips WHERE id = @id", new { id });
        public int DeleteStaffSlip2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}staff_slips WHERE id = @id", new { id });
        public int DeleteStatisticalCode(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code WHERE id = @id", new { id });
        public int DeleteStatisticalCode2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}statistical_codes WHERE id = @id", new { id });
        public int DeleteStatisticalCodeType(Guid? id) => Execute($"DELETE FROM diku_mod_inventory_storage{(IsMySql ? "_" : ".")}statistical_code_type WHERE id = @id", new { id });
        public int DeleteStatisticalCodeType2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}statistical_code_types WHERE id = @id", new { id });
        public int DeleteStatus(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}statuses WHERE id = @id", new { id });
        public int DeleteSubject(string id, Guid? instanceId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}subjects WHERE id = @id AND instance_id = @instanceId", new { id, instanceId });
        public int DeleteSuffix(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}suffixes WHERE id = @id", new { id });
        public int DeleteSuffix2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}suffixes WHERE id = @id", new { id });
        public int DeleteSupplementStatement(string id, Guid? holdingId) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}supplement_statements WHERE id = @id AND holding_id = @holdingId", new { id, holdingId });
        public int DeleteTag(Guid? id) => Execute($"DELETE FROM diku_mod_tags{(IsMySql ? "_" : ".")}tags WHERE id = @id", new { id });
        public int DeleteTag2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}tags WHERE id = @id", new { id });
        public int DeleteTemplate(Guid? id) => Execute($"DELETE FROM diku_mod_template_engine{(IsMySql ? "_" : ".")}template WHERE id = @id", new { id });
        public int DeleteTemplate2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}templates WHERE id = @id", new { id });
        public int DeleteTemplateOutputFormat(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}template_output_formats WHERE id = @id", new { id });
        public int DeleteTitle(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}titles WHERE id = @id", new { id });
        public int DeleteTitle2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}titles WHERE id = @id", new { id });
        public int DeleteTitleContributor(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}title_contributors WHERE id = @id", new { id });
        public int DeleteTitleProductId(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}title_product_ids WHERE id = @id", new { id });
        public int DeleteTransaction(Guid? id) => Execute($"DELETE FROM diku_mod_finance_storage{(IsMySql ? "_" : ".")}transaction WHERE id = @id", new { id });
        public int DeleteTransaction2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}transactions WHERE id = @id", new { id });
        public int DeleteTransactionTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}transaction_tags WHERE id = @id", new { id });
        public int DeleteTransferAccount(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfers WHERE id = @id", new { id });
        public int DeleteTransferAccount2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}transfer_accounts WHERE id = @id", new { id });
        public int DeleteTransferCriteria(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}transfer_criteria WHERE id = @id", new { id });
        public int DeleteTransferCriteria2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}transfer_criterias WHERE id = @id", new { id });
        public int DeleteUser(Guid? id) => Execute($"DELETE FROM diku_mod_users{(IsMySql ? "_" : ".")}users WHERE id = @id", new { id });
        public int DeleteUser2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}users WHERE id = @id", new { id });
        public int DeleteUserAcquisitionsUnit(Guid? id) => Execute($"DELETE FROM diku_mod_orders_storage{(IsMySql ? "_" : ".")}acquisitions_unit_membership WHERE id = @id", new { id });
        public int DeleteUserAcquisitionsUnit2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_acquisitions_units WHERE id = @id", new { id });
        public int DeleteUserAddress(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_addresses WHERE id = @id", new { id });
        public int DeleteUserDepartment(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_departments WHERE id = @id", new { id });
        public int DeleteUserRequestPreference(Guid? id) => Execute($"DELETE FROM diku_mod_circulation_storage{(IsMySql ? "_" : ".")}user_request_preference WHERE id = @id", new { id });
        public int DeleteUserRequestPreference2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_request_preferences WHERE id = @id", new { id });
        public int DeleteUserSummary(Guid? id) => Execute($"DELETE FROM diku_mod_patron_blocks{(IsMySql ? "_" : ".")}user_summary WHERE id = @id", new { id });
        public int DeleteUserSummary2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_summaries WHERE id = @id", new { id });
        public int DeleteUserSummaryOpenFeesFine(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_summary_open_fees_fines WHERE id = @id", new { id });
        public int DeleteUserSummaryOpenLoan(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_summary_open_loans WHERE id = @id", new { id });
        public int DeleteUserTag(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}user_tags WHERE id = @id", new { id });
        public int DeleteVoucher(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}vouchers WHERE id = @id", new { id });
        public int DeleteVoucher2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}vouchers WHERE id = @id", new { id });
        public int DeleteVoucherAcquisitionsUnit(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}voucher_acquisitions_units WHERE id = @id", new { id });
        public int DeleteVoucherItem(Guid? id) => Execute($"DELETE FROM diku_mod_invoice_storage{(IsMySql ? "_" : ".")}voucher_lines WHERE id = @id", new { id });
        public int DeleteVoucherItem2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}voucher_items WHERE id = @id", new { id });
        public int DeleteVoucherItemFund(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}voucher_item_fund_distributions WHERE id = @id", new { id });
        public int DeleteVoucherItemSourceId(string id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}voucher_item_source_ids WHERE id = @id", new { id });
        public int DeleteWaiveReason(Guid? id) => Execute($"DELETE FROM diku_mod_feesfines{(IsMySql ? "_" : ".")}waives WHERE id = @id", new { id });
        public int DeleteWaiveReason2(Guid? id) => Execute($"DELETE FROM uc{(IsMySql ? "_" : ".")}waive_reasons WHERE id = @id", new { id });

        public int Execute(string sql, object param = null, int? commandTimeout = null)
        {
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{sql} {(param is Dictionary<string, object> d ? $"{{ {string.Join(", ", d.Select(kvp => $"{kvp.Key} = {kvp.Value}"))} }}" : param)}");
            return Connection.Execute(sql, param, Transaction, commandTimeout ?? CommandTimeout);
        }

        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null)
        {
            var s = Stopwatch.StartNew();
            try
            {
                return Connection.ExecuteScalar<T>(sql, param, Transaction, commandTimeout ?? CommandTimeout);
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Exception while reading from stream") traceSource.TraceEvent(TraceEventType.Verbose, 0, string.Join("\r\n", Query<string>($"EXPLAIN {sql}")));
                throw e;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{sql} {(param is Dictionary<string, object> d ? $"{{ {string.Join(", ", d.Select(kvp => $"{kvp.Key} = {kvp.Value}"))} }}" : param)} {s.Elapsed}");
            }
        }

        public int Count(string sql, object param = null, int? take = null, int? commandTimeout = null)
        {
            sql = $"SELECT COUNT (*) FROM ({sql}{(take != null ? $"{(IsMySql || IsPostgreSql ? $" LIMIT {take}" : $" OFFSET 0 ROWS FETCH NEXT {take} ROWS ONLY")}" : "")}) count";
            return ExecuteScalar<int>(sql, param, commandTimeout ?? CommandTimeout);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, int? skip = null, int? take = null, int? commandTimeout = null)
        {
            if (skip != null || take != null) sql = $"{sql}{(IsMySql ? $" LIMIT {(skip == null ? $"{take}" : $"{skip}, {take ?? int.MaxValue}")}" : IsPostgreSql ? $"{(take != null ? $" LIMIT {take}" : "")}{(skip != null ? $" OFFSET {skip}" : "")}" : $" OFFSET {skip ?? 0} ROWS{(take != null ? $" FETCH NEXT {take} ROWS ONLY" : "")}")}";
            var s = Stopwatch.StartNew();
            try
            {
                return Connection.Query(sql, param, Transaction, false, commandTimeout ?? CommandTimeout);
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Exception while reading from stream") traceSource.TraceEvent(TraceEventType.Verbose, 0, string.Join("\r\n", Query<string>($"EXPLAIN {sql}")));
                throw e;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{sql} {(param is Dictionary<string, object> d ? $"{{ {string.Join(", ", d.Select(kvp => $"{kvp.Key} = {kvp.Value}"))} }}" : param)} {s.Elapsed}");
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, int? skip = null, int? take = null, int? commandTimeout = null)
        {
            if (skip != null || take != null) sql = $"{sql}{(IsMySql ? $" LIMIT {(skip == null ? $"{take}" : $"{skip}, {take ?? int.MaxValue}")}" : IsPostgreSql ? $"{(take != null ? $" LIMIT {take}" : "")}{(skip != null ? $" OFFSET {skip}" : "")}" : $" OFFSET {skip ?? 0} ROWS{(take != null ? $" FETCH NEXT {take} ROWS ONLY" : "")}")}";
            var s = Stopwatch.StartNew();
            try
            {
                return Connection.Query<T>(sql, param, Transaction, false, commandTimeout ?? CommandTimeout);
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Exception while reading from stream") traceSource.TraceEvent(TraceEventType.Verbose, 0, string.Join("\r\n", Query<string>($"EXPLAIN {sql}")));
                throw e;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{sql} {(param is Dictionary<string, object> d ? $"{{ {string.Join(", ", d.Select(kvp => $"{kvp.Key} = {kvp.Value}"))} }}" : param)} {s.Elapsed}");
            }
        }

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

        private string SqlEncode(string value) => value.Replace("'", "''").Replace(@"\", IsMySql ? @"\\" : @"\");

        public void Dispose()
        {
            if (dbConnection != null)
            {
                dbConnection.Dispose();
            }
        }
    }

    public class DbProviderFactories
    {
        internal static DbProviderFactory GetFactory(string providerName)
        {
            if (providerName == "MySql.Data.MySqlClient" || providerName == "MySql.Data.MySqlClient2")
                throw new NotSupportedException();
            else if (providerName == "Npgsql")
                return NpgsqlFactory.Instance;
            else if (providerName == "System.Data.SqlClient")
                throw new NotSupportedException();
            throw new NotImplementedException();
        }
    }
}
