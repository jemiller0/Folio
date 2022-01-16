using FolioLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolioLibraryTest
{
    [TestClass]
    public class FolioDapperContextTest
    {
        private readonly static FolioDapperContext folioDapperContext = new FolioDapperContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioLibraryTest", SourceLevels.All);

        static FolioDapperContextTest()
        {
            traceSource.Listeners.Add(new TextWriterTraceListener(new StreamWriter("Trace.log", true) { AutoFlush = true }) { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId });
            FolioDapperContext.traceSource.Listeners.AddRange(traceSource.Listeners);
            traceSource.Switch.Level = FolioDapperContext.traceSource.Switch.Level = SourceLevels.Information;
        }

        [TestMethod]
        public void QueryAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AcquisitionsUnit2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AcquisitionsUnit2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
au2.id AS ""Id"",
au2.name AS ""Name"",
au2.is_deleted AS ""IsDeleted"",
au2.protect_create AS ""ProtectCreate"",
au2.protect_read AS ""ProtectRead"",
au2.protect_update AS ""ProtectUpdate"",
au2.protect_delete AS ""ProtectDelete"",
au2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
au2.created_by_user_id AS ""CreationUserId"",
au2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
au2.updated_by_user_id AS ""LastWriteUserId"",
au2.content AS ""Content"" 
FROM uc.acquisitions_units AS au2
LEFT JOIN uc.users AS cu ON cu.id = au2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = au2.updated_by_user_id
 ORDER BY au2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAddressesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Addresses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AddressesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
a.name AS ""Name"",
a.content AS ""Content"",
a.enabled AS ""Enabled"",
a.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
a.created_by_user_id AS ""CreationUserId"",
a.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
a.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.addresses AS a
LEFT JOIN uc.users AS cu ON cu.id = a.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = a.updated_by_user_id
 ORDER BY a.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAddressType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AddressType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AddressType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
at2.id AS ""Id"",
at2.address_type AS ""Name"",
at2.desc AS ""Description"",
at2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
at2.created_by_user_id AS ""CreationUserId"",
at2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
at2.updated_by_user_id AS ""LastWriteUserId"",
at2.content AS ""Content"" 
FROM uc.address_types AS at2
LEFT JOIN uc.users AS cu ON cu.id = at2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = at2.updated_by_user_id
 ORDER BY at2.address_type
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlert2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Alert2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Alert2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Alert2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
a2.id AS ""Id"",
a2.alert AS ""Alert"",
a2.content AS ""Content"" 
FROM uc.alerts AS a2
 ORDER BY a2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Alert2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAllocatedFromFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AllocatedFromFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedFromFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AllocatedFromFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f.name AS ""Fund"",
aff.fund_id AS ""FundId"",
ff.name AS ""FromFund"",
aff.from_fund_id AS ""FromFundId"" 
FROM uc.allocated_from_funds AS aff
LEFT JOIN uc.funds AS f ON f.id = aff.fund_id
LEFT JOIN uc.funds AS ff ON ff.id = aff.from_fund_id
 ORDER BY aff.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedFromFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAllocatedToFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AllocatedToFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedToFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AllocatedToFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f.name AS ""Fund"",
atf.fund_id AS ""FundId"",
tf.name AS ""ToFund"",
atf.to_fund_id AS ""ToFundId"" 
FROM uc.allocated_to_funds AS atf
LEFT JOIN uc.funds AS f ON f.id = atf.fund_id
LEFT JOIN uc.funds AS tf ON tf.id = atf.to_fund_id
 ORDER BY atf.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedToFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlternativeTitlesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AlternativeTitles(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AlternativeTitlesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
at.instance_id AS ""InstanceId"",
att.name AS ""AlternativeTitleType"",
at.alternative_title_type_id AS ""AlternativeTitleTypeId"",
at.alternative_title AS ""Content"" 
FROM uc.alternative_titles AS at
LEFT JOIN uc.instances AS i ON i.id = at.instance_id
LEFT JOIN uc.alternative_title_types AS att ON att.id = at.alternative_title_type_id
 ORDER BY at.alternative_title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitlesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlternativeTitleType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AlternativeTitleType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AlternativeTitleType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
att2.id AS ""Id"",
att2.name AS ""Name"",
att2.source AS ""Source"",
att2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
att2.created_by_user_id AS ""CreationUserId"",
att2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
att2.updated_by_user_id AS ""LastWriteUserId"",
att2.content AS ""Content"" 
FROM uc.alternative_title_types AS att2
LEFT JOIN uc.users AS cu ON cu.id = att2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = att2.updated_by_user_id
 ORDER BY att2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAuthAttempt2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AuthAttempt2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthAttempt2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AuthAttempt2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
aa2.id AS ""Id"",
u.username AS ""User"",
aa2.user_id AS ""UserId"",
aa2.last_attempt AS ""LastAttempt"",
aa2.attempt_count AS ""AttemptCount"",
aa2.content AS ""Content"" 
FROM uc.auth_attempts AS aa2
LEFT JOIN uc.users AS u ON u.id = aa2.user_id
 ORDER BY aa2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthAttempt2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAuthCredentialsHistory2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.AuthCredentialsHistory2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthCredentialsHistory2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void AuthCredentialsHistory2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ach2.id AS ""Id"",
u.username AS ""User"",
ach2.user_id AS ""UserId"",
ach2.hash AS ""Hash"",
ach2.salt AS ""Salt"",
ach2.date AS ""Date"",
ach2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ach2.created_by_user_id AS ""CreationUserId"",
ach2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ach2.updated_by_user_id AS ""LastWriteUserId"",
ach2.content AS ""Content"" 
FROM uc.auth_credentials_histories AS ach2
LEFT JOIN uc.users AS u ON u.id = ach2.user_id
LEFT JOIN uc.users AS cu ON cu.id = ach2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ach2.updated_by_user_id
 ORDER BY ach2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthCredentialsHistory2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchGroup2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchGroup2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bg2.id AS ""Id"",
bg2.name AS ""Name"",
bg2.description AS ""Description"",
bg2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bg2.created_by_user_id AS ""CreationUserId"",
bg2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bg2.updated_by_user_id AS ""LastWriteUserId"",
bg2.content AS ""Content"" 
FROM uc.batch_groups AS bg2
LEFT JOIN uc.users AS cu ON cu.id = bg2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bg2.updated_by_user_id
 ORDER BY bg2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucher2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucher2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucher2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucher2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bv2.id AS ""Id"",
bv2.batch_group AS ""BatchGroup"",
bv2.created AS ""Created"",
bv2.start AS ""Start"",
bv2.end AS ""End"",
bv2.total_records AS ""TotalRecords"",
bv2.content AS ""Content"" 
FROM uc.batch_vouchers AS bv2
 ORDER BY bv2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucher2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherBatchedVouchersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherBatchedVouchers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVouchersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherBatchedVouchersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bv.id AS ""BatchVoucher"",
bvbv.batch_voucher_id AS ""BatchVoucherId"",
bvbv.accounting_code AS ""AccountingCode"",
bvbv.account_no AS ""AccountNumber"",
bvbv.amount AS ""Amount"",
bvbv.disbursement_number AS ""DisbursementNumber"",
bvbv.disbursement_date AS ""DisbursementDate"",
bvbv.disbursement_amount AS ""DisbursementAmount"",
bvbv.enclosure_needed AS ""Enclosure"",
bvbv.exchange_rate AS ""ExchangeRate"",
bvbv.folio_invoice_no AS ""FolioInvoiceNumber"",
bvbv.invoice_currency AS ""InvoiceCurrency"",
bvbv.invoice_note AS ""InvoiceNote"",
bvbv.status AS ""Status"",
bvbv.system_currency AS ""SystemCurrency"",
bvbv.type AS ""Type"",
bvbv.vendor_invoice_no AS ""VendorInvoiceNumber"",
bvbv.vendor_name AS ""VendorName"",
bvbv.voucher_date AS ""VoucherDate"",
bvbv.voucher_number AS ""VoucherNumber"",
bvbv.vendor_address_address_line1 AS ""VendorStreetAddress1"",
bvbv.vendor_address_address_line2 AS ""VendorStreetAddress2"",
bvbv.vendor_address_city AS ""VendorCity"",
bvbv.vendor_address_state_region AS ""VendorState"",
bvbv.vendor_address_zip_code AS ""VendorPostalCode"",
bvbv.vendor_address_country AS ""VendorCountryCode"" 
FROM uc.batch_voucher_batched_vouchers AS bvbv
LEFT JOIN uc.batch_vouchers AS bv ON bv.id = bvbv.batch_voucher_id
 ORDER BY bvbv.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVouchersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherBatchedVoucherBatchedVoucherLinesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherBatchedVoucherBatchedVoucherLines(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVoucherBatchedVoucherLinesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherBatchedVoucherBatchedVoucherLinesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bvbv.id AS ""BatchVoucherBatchedVoucher"",
bvbvbvl.batch_voucher_batched_voucher_id AS ""BatchVoucherBatchedVoucherId"",
bvbvbvl.amount AS ""Amount"",
bvbvbvl.external_account_number AS ""AccountNumber"" 
FROM uc.batch_voucher_batched_voucher_batched_voucher_lines AS bvbvbvl
LEFT JOIN uc.batch_voucher_batched_vouchers AS bvbv ON bvbv.id = bvbvbvl.batch_voucher_batched_voucher_id
 ORDER BY bvbvbvl.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVoucherBatchedVoucherLinesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherBatchedVoucherBatchedVoucherLineFundCodesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVoucherBatchedVoucherLineFundCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherBatchedVoucherBatchedVoucherLineFundCodesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bvbvbvl.id AS ""BatchVoucherBatchedVoucherBatchedVoucherLine"",
bvbvbvlfc.batch_voucher_batched_voucher_batched_voucher_line_id AS ""BatchVoucherBatchedVoucherBatchedVoucherLineId"",
bvbvbvlfc.content AS ""Content"" 
FROM uc.batch_voucher_batched_voucher_batched_voucher_line_fund_codes AS bvbvbvlfc
LEFT JOIN uc.batch_voucher_batched_voucher_batched_voucher_lines AS bvbvbvl ON bvbvbvl.id = bvbvbvlfc.batch_voucher_batched_voucher_batched_voucher_line_id
 ORDER BY bvbvbvlfc.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherBatchedVoucherBatchedVoucherLineFundCodesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherExport2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherExport2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExport2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherExport2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bve2.id AS ""Id"",
bve2.status AS ""Status"",
bve2.message AS ""Message"",
bg.name AS ""BatchGroup"",
bve2.batch_group_id AS ""BatchGroupId"",
bve2.start AS ""Start"",
bve2.end AS ""End"",
bv.id AS ""BatchVoucher"",
bve2.batch_voucher_id AS ""BatchVoucherId"",
bve2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bve2.created_by_user_id AS ""CreationUserId"",
bve2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bve2.updated_by_user_id AS ""LastWriteUserId"",
bve2.content AS ""Content"" 
FROM uc.batch_voucher_exports AS bve2
LEFT JOIN uc.batch_groups AS bg ON bg.id = bve2.batch_group_id
LEFT JOIN uc.batch_vouchers AS bv ON bv.id = bve2.batch_voucher_id
LEFT JOIN uc.users AS cu ON cu.id = bve2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bve2.updated_by_user_id
 ORDER BY bve2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExport2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherExportConfig2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherExportConfig2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExportConfig2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherExportConfig2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bvec2.id AS ""Id"",
bg.name AS ""BatchGroup"",
bvec2.batch_group_id AS ""BatchGroupId"",
bvec2.enable_scheduled_export AS ""EnableScheduledExport"",
bvec2.format AS ""Format"",
bvec2.start_time AS ""StartTime"",
bvec2.upload_uri AS ""UploadUri"",
bvec2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bvec2.created_by_user_id AS ""CreationUserId"",
bvec2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bvec2.updated_by_user_id AS ""LastWriteUserId"",
bvec2.content AS ""Content"" 
FROM uc.batch_voucher_export_configs AS bvec2
LEFT JOIN uc.batch_groups AS bg ON bg.id = bvec2.batch_group_id
LEFT JOIN uc.users AS cu ON cu.id = bvec2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bvec2.updated_by_user_id
 ORDER BY bvec2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExportConfig2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchVoucherExportConfigWeekdaysTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BatchVoucherExportConfigWeekdays(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExportConfigWeekdaysTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BatchVoucherExportConfigWeekdaysQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bvec.id AS ""BatchVoucherExportConfig"",
bvecw.batch_voucher_export_config_id AS ""BatchVoucherExportConfigId"",
bvecw.content AS ""Content"" 
FROM uc.batch_voucher_export_config_weekdays AS bvecw
LEFT JOIN uc.batch_voucher_export_configs AS bvec ON bvec.id = bvecw.batch_voucher_export_config_id
 ORDER BY bvecw.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExportConfigWeekdaysQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlock2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Block2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Block2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
b2.id AS ""Id"",
b2.type AS ""Type"",
b2.desc AS ""Description"",
b2.code AS ""Code"",
b2.staff_information AS ""StaffInformation"",
b2.patron_message AS ""PatronMessage"",
b2.expiration_date AS ""ExpirationDate"",
b2.borrowing AS ""Borrowing"",
b2.renewals AS ""Renewals"",
b2.requests AS ""Requests"",
u.username AS ""User"",
b2.user_id AS ""UserId"",
b2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
b2.created_by_user_id AS ""CreationUserId"",
b2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
b2.updated_by_user_id AS ""LastWriteUserId"",
b2.content AS ""Content"" 
FROM uc.blocks AS b2
LEFT JOIN uc.users AS u ON u.id = b2.user_id
LEFT JOIN uc.users AS cu ON cu.id = b2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = b2.updated_by_user_id
 ORDER BY b2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockCondition2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BlockCondition2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BlockCondition2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bc2.id AS ""Id"",
bc2.name AS ""Name"",
bc2.block_borrowing AS ""BlockBorrowing"",
bc2.block_renewals AS ""BlockRenewals"",
bc2.block_requests AS ""BlockRequests"",
bc2.value_type AS ""ValueType"",
bc2.message AS ""Message"",
bc2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bc2.created_by_user_id AS ""CreationUserId"",
bc2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bc2.updated_by_user_id AS ""LastWriteUserId"",
bc2.content AS ""Content"" 
FROM uc.block_conditions AS bc2
LEFT JOIN uc.users AS cu ON cu.id = bc2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bc2.updated_by_user_id
 ORDER BY bc2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockLimit2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BlockLimit2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BlockLimit2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bl2.id AS ""Id"",
g.group AS ""Group"",
bl2.group_id AS ""GroupId"",
c.name AS ""Condition"",
bl2.condition_id AS ""ConditionId"",
bl2.value AS ""Value"",
bl2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bl2.created_by_user_id AS ""CreationUserId"",
bl2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bl2.updated_by_user_id AS ""LastWriteUserId"",
bl2.content AS ""Content"" 
FROM uc.block_limits AS bl2
LEFT JOIN uc.groups AS g ON g.id = bl2.group_id
LEFT JOIN uc.block_conditions AS c ON c.id = bl2.condition_id
LEFT JOIN uc.users AS cu ON cu.id = bl2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bl2.updated_by_user_id
 ORDER BY bl2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBoundWithPart2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BoundWithPart2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPart2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BoundWithPart2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bwp2.id AS ""Id"",
h.hrid AS ""Holding"",
bwp2.holding_id AS ""HoldingId"",
i.hrid AS ""Item"",
bwp2.item_id AS ""ItemId"",
bwp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
bwp2.created_by_user_id AS ""CreationUserId"",
bwp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
bwp2.updated_by_user_id AS ""LastWriteUserId"",
bwp2.content AS ""Content"" 
FROM uc.bound_with_parts AS bwp2
LEFT JOIN uc.holdings AS h ON h.id = bwp2.holding_id
LEFT JOIN uc.items AS i ON i.id = bwp2.item_id
LEFT JOIN uc.users AS cu ON cu.id = bwp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = bwp2.updated_by_user_id
 ORDER BY bwp2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPart2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Budget2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Budget2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
b2.id AS ""Id"",
b2.name AS ""Name"",
b2.budget_status AS ""BudgetStatus"",
b2.allowable_encumbrance AS ""AllowableEncumbrance"",
b2.allowable_expenditure AS ""AllowableExpenditure"",
b2.allocated AS ""Allocated"",
b2.awaiting_payment AS ""AwaitingPayment"",
b2.available AS ""Available"",
b2.encumbered AS ""Encumbered"",
b2.expenditures AS ""Expenditures"",
b2.net_transfers AS ""NetTransfers"",
b2.unavailable AS ""Unavailable"",
b2.over_encumbrance AS ""OverEncumbrance"",
b2.over_expended AS ""OverExpended"",
f.name AS ""Fund"",
b2.fund_id AS ""FundId"",
fy.name AS ""FiscalYear"",
b2.fiscal_year_id AS ""FiscalYearId"",
b2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
b2.created_by_user_id AS ""CreationUserId"",
b2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
b2.updated_by_user_id AS ""LastWriteUserId"",
b2.initial_allocation AS ""InitialAllocation"",
b2.allocation_to AS ""AllocationTo"",
b2.allocation_from AS ""AllocationFrom"",
b2.total_funding AS ""TotalFunding"",
b2.cash_balance AS ""CashBalance"",
b2.content AS ""Content"" 
FROM uc.budgets AS b2
LEFT JOIN uc.funds AS f ON f.id = b2.fund_id
LEFT JOIN uc.fiscal_years AS fy ON fy.id = b2.fiscal_year_id
LEFT JOIN uc.users AS cu ON cu.id = b2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = b2.updated_by_user_id
 ORDER BY b2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BudgetAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BudgetAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
b.name AS ""Budget"",
bau.budget_id AS ""BudgetId"",
au.name AS ""AcquisitionsUnit"",
bau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.budget_acquisitions_units AS bau
LEFT JOIN uc.budgets AS b ON b.id = bau.budget_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = bau.acquisitions_unit_id
 ORDER BY bau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BudgetExpenseClass2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BudgetExpenseClass2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bec2.id AS ""Id"",
b.name AS ""Budget"",
bec2.budget_id AS ""BudgetId"",
ec.name AS ""ExpenseClass"",
bec2.expense_class_id AS ""ExpenseClassId"",
bec2.status AS ""Status"",
bec2.content AS ""Content"" 
FROM uc.budget_expense_classes AS bec2
LEFT JOIN uc.budgets AS b ON b.id = bec2.budget_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = bec2.expense_class_id
 ORDER BY bec2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BudgetGroup2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BudgetGroup2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
bg2.id AS ""Id"",
b.name AS ""Budget"",
bg2.budget_id AS ""BudgetId"",
g.name AS ""Group"",
bg2.group_id AS ""GroupId"",
fy.name AS ""FiscalYear"",
bg2.fiscal_year_id AS ""FiscalYearId"",
f.name AS ""Fund"",
bg2.fund_id AS ""FundId"",
bg2.content AS ""Content"" 
FROM uc.budget_groups AS bg2
LEFT JOIN uc.budgets AS b ON b.id = bg2.budget_id
LEFT JOIN uc.finance_groups AS g ON g.id = bg2.group_id
LEFT JOIN uc.fiscal_years AS fy ON fy.id = bg2.fiscal_year_id
LEFT JOIN uc.funds AS f ON f.id = bg2.fund_id
 ORDER BY bg2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroup2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.BudgetTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void BudgetTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
b.name AS ""Budget"",
bt.budget_id AS ""BudgetId"",
t.id AS ""Tag"",
bt.tag_id AS ""TagId"" 
FROM uc.budget_tags AS bt
LEFT JOIN uc.budgets AS b ON b.id = bt.budget_id
LEFT JOIN uc.tags AS t ON t.id = bt.tag_id
 ORDER BY bt.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCallNumberType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CallNumberType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CallNumberType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cnt2.id AS ""Id"",
cnt2.name AS ""Name"",
cnt2.source AS ""Source"",
cnt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
cnt2.created_by_user_id AS ""CreationUserId"",
cnt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cnt2.updated_by_user_id AS ""LastWriteUserId"",
cnt2.content AS ""Content"" 
FROM uc.call_number_types AS cnt2
LEFT JOIN uc.users AS cu ON cu.id = cnt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cnt2.updated_by_user_id
 ORDER BY cnt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCampus2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Campus2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Campus2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c2.id AS ""Id"",
c2.name AS ""Name"",
c2.code AS ""Code"",
i.name AS ""Institution"",
c2.institution_id AS ""InstitutionId"",
c2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
c2.created_by_user_id AS ""CreationUserId"",
c2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
c2.updated_by_user_id AS ""LastWriteUserId"",
c2.content AS ""Content"" 
FROM uc.campuses AS c2
LEFT JOIN uc.institutions AS i ON i.id = c2.institution_id
LEFT JOIN uc.users AS cu ON cu.id = c2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = c2.updated_by_user_id
 ORDER BY c2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCancellationReason2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CancellationReason2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CancellationReason2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cr2.id AS ""Id"",
cr2.name AS ""Name"",
cr2.description AS ""Description"",
cr2.public_description AS ""PublicDescription"",
cr2.requires_additional_information AS ""RequiresAdditionalInformation"",
cr2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
cr2.created_by_user_id AS ""CreationUserId"",
cr2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cr2.updated_by_user_id AS ""LastWriteUserId"",
cr2.content AS ""Content"" 
FROM uc.cancellation_reasons AS cr2
LEFT JOIN uc.users AS cu ON cu.id = cr2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cr2.updated_by_user_id
 ORDER BY cr2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCategory2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Category2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Category2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c2.id AS ""Id"",
c2.value AS ""Name"",
c2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
c2.created_by_user_id AS ""CreationUserId"",
c2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
c2.updated_by_user_id AS ""LastWriteUserId"",
c2.content AS ""Content"" 
FROM uc.categories AS c2
LEFT JOIN uc.users AS cu ON cu.id = c2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = c2.updated_by_user_id
 ORDER BY c2.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCheckIn2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CheckIn2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CheckIn2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ci2.id AS ""Id"",
ci2.occurred_date_time AS ""OccurredDateTime"",
i.hrid AS ""Item"",
ci2.item_id AS ""ItemId"",
ci2.item_status_prior_to_check_in AS ""ItemStatusPriorToCheckIn"",
ci2.request_queue_size AS ""RequestQueueSize"",
il.name AS ""ItemLocation"",
ci2.item_location_id AS ""ItemLocationId"",
sp.name AS ""ServicePoint"",
ci2.service_point_id AS ""ServicePointId"",
pbu.username AS ""PerformedByUser"",
ci2.performed_by_user_id AS ""PerformedByUserId"",
ci2.content AS ""Content"" 
FROM uc.check_ins AS ci2
LEFT JOIN uc.items AS i ON i.id = ci2.item_id
LEFT JOIN uc.locations AS il ON il.id = ci2.item_location_id
LEFT JOIN uc.service_points AS sp ON sp.id = ci2.service_point_id
LEFT JOIN uc.users AS pbu ON pbu.id = ci2.performed_by_user_id
 ORDER BY ci2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCirculationNotesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CirculationNotes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationNotesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
cn.id2 AS ""Id2"",
cn.note_type AS ""NoteType"",
cn.note AS ""Note"",
cn.source_id AS ""SourceId"",
cn.source_personal_last_name AS ""SourcePersonalLastName"",
cn.source_personal_first_name AS ""SourcePersonalFirstName"",
cn.date AS ""Date"",
cn.staff_only AS ""StaffOnly"" 
FROM uc.circulation_notes AS cn
LEFT JOIN uc.items AS i ON i.id = cn.item_id
 ORDER BY cn.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationNotesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCirculationRule2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CirculationRule2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationRule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationRule2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cr2.id AS ""Id"",
cr2.rules_as_text AS ""RulesAsText"",
cr2.content AS ""Content"",
cr2.lock AS ""Lock"" 
FROM uc.circulation_rules AS cr2
 ORDER BY cr2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationRule2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryClassificationsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Classifications(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ClassificationsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
c.classification_number AS ""Number"",
ct.name AS ""ClassificationType"",
c.classification_type_id AS ""ClassificationTypeId"" 
FROM uc.classifications AS c
LEFT JOIN uc.instances AS i ON i.id = c.instance_id
LEFT JOIN uc.classification_types AS ct ON ct.id = c.classification_type_id
 ORDER BY c.classification_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryClassificationType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ClassificationType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ClassificationType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ct2.id AS ""Id"",
ct2.name AS ""Name"",
ct2.source AS ""Source"",
ct2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ct2.created_by_user_id AS ""CreationUserId"",
ct2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ct2.updated_by_user_id AS ""LastWriteUserId"",
ct2.content AS ""Content"" 
FROM uc.classification_types AS ct2
LEFT JOIN uc.users AS cu ON cu.id = ct2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ct2.updated_by_user_id
 ORDER BY ct2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCloseReason2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CloseReason2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CloseReason2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cr2.id AS ""Id"",
cr2.name AS ""Name"",
cr2.source AS ""Source"",
cr2.content AS ""Content"" 
FROM uc.close_reasons AS cr2
 ORDER BY cr2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryComment2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Comment2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Comment2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c2.id AS ""Id"",
c2.paid AS ""Paid"",
c2.waived AS ""Waived"",
c2.refunded AS ""Refunded"",
c2.transferred_manually AS ""TransferredManually"",
c2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
c2.created_by_user_id AS ""CreationUserId"",
c2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
c2.updated_by_user_id AS ""LastWriteUserId"",
c2.content AS ""Content"" 
FROM uc.comments AS c2
LEFT JOIN uc.users AS cu ON cu.id = c2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = c2.updated_by_user_id
 ORDER BY c2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryConfiguration2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Configuration2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c2.id AS ""Id"",
c2.module AS ""Module"",
c2.config_name AS ""ConfigName"",
c2.code AS ""Code"",
c2.description AS ""Description"",
c2.default AS ""Default"",
c2.enabled AS ""Enabled"",
c2.value AS ""Value"",
c2.user_id AS ""UserId"",
c2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
c2.created_by_user_id AS ""CreationUserId"",
c2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
c2.updated_by_user_id AS ""LastWriteUserId"",
c2.content AS ""Content"" 
FROM uc.configurations AS c2
LEFT JOIN uc.users AS cu ON cu.id = c2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = c2.updated_by_user_id
 ORDER BY c2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContact2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Contact2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Contact2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c2.id AS ""Id"",
c2.name AS ""Name"",
c2.prefix AS ""Prefix"",
c2.first_name AS ""FirstName"",
c2.last_name AS ""LastName"",
c2.language AS ""Language"",
c2.notes AS ""Notes"",
c2.inactive AS ""Inactive"",
c2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
c2.created_by_user_id AS ""CreationUserId"",
c2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
c2.updated_by_user_id AS ""LastWriteUserId"",
c2.content AS ""Content"" 
FROM uc.contacts AS c2
LEFT JOIN uc.users AS cu ON cu.id = c2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = c2.updated_by_user_id
 ORDER BY c2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactAddressesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactAddresses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactAddressesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.name AS ""Contact"",
ca.contact_id AS ""ContactId"",
ca.address_line1 AS ""StreetAddress1"",
ca.address_line2 AS ""StreetAddress2"",
ca.city AS ""City"",
ca.state_region AS ""State"",
ca.zip_code AS ""PostalCode"",
ca.country AS ""CountryCode"",
ca.is_primary AS ""IsPrimary"",
ca.language AS ""Language"",
ca.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ca.created_by_user_id AS ""CreationUserId"",
ca.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ca.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.contact_addresses AS ca
LEFT JOIN uc.contacts AS c ON c.id = ca.contact_id
LEFT JOIN uc.users AS cu ON cu.id = ca.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ca.updated_by_user_id
 ORDER BY ca.address_line1
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactAddressCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactAddressCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ca.address_line1 AS ""ContactAddress"",
cac.contact_address_id AS ""ContactAddressId"",
c.value AS ""Category"",
cac.category_id AS ""CategoryId"" 
FROM uc.contact_address_categories AS cac
LEFT JOIN uc.contact_addresses AS ca ON ca.id = cac.contact_address_id
LEFT JOIN uc.categories AS c ON c.id = cac.category_id
 ORDER BY cac.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.name AS ""Contact"",
cc.contact_id AS ""ContactId"",
c2.value AS ""Category"",
cc.category_id AS ""CategoryId"" 
FROM uc.contact_categories AS cc
LEFT JOIN uc.contacts AS c ON c.id = cc.contact_id
LEFT JOIN uc.categories AS c2 ON c2.id = cc.category_id
 ORDER BY cc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactEmailsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactEmails(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactEmailsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.name AS ""Contact"",
ce.contact_id AS ""ContactId"",
ce.value AS ""Value"",
ce.description AS ""Description"",
ce.is_primary AS ""IsPrimary"",
ce.language AS ""Language"",
ce.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ce.created_by_user_id AS ""CreationUserId"",
ce.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ce.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.contact_emails AS ce
LEFT JOIN uc.contacts AS c ON c.id = ce.contact_id
LEFT JOIN uc.users AS cu ON cu.id = ce.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ce.updated_by_user_id
 ORDER BY ce.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactEmailCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactEmailCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ce.value AS ""ContactEmail"",
cec.contact_email_id AS ""ContactEmailId"",
c.value AS ""Category"",
cec.category_id AS ""CategoryId"" 
FROM uc.contact_email_categories AS cec
LEFT JOIN uc.contact_emails AS ce ON ce.id = cec.contact_email_id
LEFT JOIN uc.categories AS c ON c.id = cec.category_id
 ORDER BY cec.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactPhoneNumbers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactPhoneNumbersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.name AS ""Contact"",
cpn.contact_id AS ""ContactId"",
cpn.phone_number AS ""PhoneNumber"",
cpn.type AS ""Type"",
cpn.is_primary AS ""IsPrimary"",
cpn.language AS ""Language"",
cpn.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
cpn.created_by_user_id AS ""CreationUserId"",
cpn.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cpn.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.contact_phone_numbers AS cpn
LEFT JOIN uc.contacts AS c ON c.id = cpn.contact_id
LEFT JOIN uc.users AS cu ON cu.id = cpn.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cpn.updated_by_user_id
 ORDER BY cpn.phone_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumbersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactPhoneNumberCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactPhoneNumberCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cpn.phone_number AS ""ContactPhoneNumber"",
cpnc.contact_phone_number_id AS ""ContactPhoneNumberId"",
c.value AS ""Category"",
cpnc.category_id AS ""CategoryId"" 
FROM uc.contact_phone_number_categories AS cpnc
LEFT JOIN uc.contact_phone_numbers AS cpn ON cpn.id = cpnc.contact_phone_number_id
LEFT JOIN uc.categories AS c ON c.id = cpnc.category_id
 ORDER BY cpnc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumberCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactTypesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactTypes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactTypesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ct.name AS ""Name"" 
FROM uc.contact_types AS ct
 ORDER BY ct.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactTypesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactUrlsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactUrls(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactUrlsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.name AS ""Contact"",
cu.contact_id AS ""ContactId"",
cu.id2 AS ""Id2"",
cu.value AS ""Value"",
cu.description AS ""Description"",
cu.language AS ""Language"",
cu.is_primary AS ""IsPrimary"",
cu.notes AS ""Notes"",
cu.created_date AS ""CreationTime"",
cu2.username AS ""CreationUser"",
cu.created_by_user_id AS ""CreationUserId"",
cu.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cu.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.contact_urls AS cu
LEFT JOIN uc.contacts AS c ON c.id = cu.contact_id
LEFT JOIN uc.users AS cu2 ON cu2.id = cu.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cu.updated_by_user_id
 ORDER BY cu.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContactUrlCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContactUrlCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cu.value AS ""ContactUrl"",
cuc.contact_url_id AS ""ContactUrlId"",
c.value AS ""Category"",
cuc.category_id AS ""CategoryId"" 
FROM uc.contact_url_categories AS cuc
LEFT JOIN uc.contact_urls AS cu ON cu.id = cuc.contact_url_id
LEFT JOIN uc.categories AS c ON c.id = cuc.category_id
 ORDER BY cuc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Contributors(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContributorsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
c.name AS ""Name"",
ct.name AS ""ContributorType"",
c.contributor_type_id AS ""ContributorTypeId"",
c.contributor_type_text AS ""ContributorTypeText"",
cnt.name AS ""ContributorNameType"",
c.contributor_name_type_id AS ""ContributorNameTypeId"",
c.primary AS ""Primary"" 
FROM uc.contributors AS c
LEFT JOIN uc.instances AS i ON i.id = c.instance_id
LEFT JOIN uc.contributor_types AS ct ON ct.id = c.contributor_type_id
LEFT JOIN uc.contributor_name_types AS cnt ON cnt.id = c.contributor_name_type_id
 ORDER BY c.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorNameType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContributorNameType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContributorNameType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cnt2.id AS ""Id"",
cnt2.name AS ""Name"",
cnt2.ordering AS ""Ordering"",
cnt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
cnt2.created_by_user_id AS ""CreationUserId"",
cnt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cnt2.updated_by_user_id AS ""LastWriteUserId"",
cnt2.content AS ""Content"" 
FROM uc.contributor_name_types AS cnt2
LEFT JOIN uc.users AS cu ON cu.id = cnt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cnt2.updated_by_user_id
 ORDER BY cnt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ContributorType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ContributorType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ct2.id AS ""Id"",
ct2.name AS ""Name"",
ct2.code AS ""Code"",
ct2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ct2.created_by_user_id AS ""CreationUserId"",
ct2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ct2.updated_by_user_id AS ""LastWriteUserId"",
ct2.content AS ""Content"" 
FROM uc.contributor_types AS ct2
LEFT JOIN uc.users AS cu ON cu.id = ct2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ct2.updated_by_user_id
 ORDER BY ct2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCountriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Countries(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
c.alpha2_code AS ""Alpha2Code"",
c.alpha3_code AS ""Alpha3Code"",
c.name AS ""Name"" 
FROM uc.countries AS c
 ORDER BY c.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCurrenciesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Currencies(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CurrenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CurrenciesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
c.organization_id AS ""OrganizationId"",
c.content AS ""Content"" 
FROM uc.currencies AS c
LEFT JOIN uc.organizations AS o ON o.id = c.organization_id
 ORDER BY c.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CurrenciesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCustomField2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CustomField2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CustomField2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cf2.id AS ""Id"",
cf2.name AS ""Name"",
cf2.ref_id AS ""RefId"",
cf2.type AS ""Type"",
cf2.entity_type AS ""EntityType"",
cf2.visible AS ""Visible"",
cf2.required AS ""Required"",
cf2.is_repeatable AS ""IsRepeatable"",
cf2.order AS ""Order"",
cf2.help_text AS ""HelpText"",
cf2.checkbox_field_default AS ""CheckboxFieldDefault"",
cf2.select_field_multi_select AS ""SelectFieldMultiSelect"",
cf2.select_field_options_sorting_order AS ""SelectFieldOptionsSortingOrder"",
cf2.text_field_field_format AS ""TextFieldFieldFormat"",
cf2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
cf2.created_by_user_id AS ""CreationUserId"",
cf2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
cf2.updated_by_user_id AS ""LastWriteUserId"",
cf2.content AS ""Content"" 
FROM uc.custom_fields AS cf2
LEFT JOIN uc.users AS cu ON cu.id = cf2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = cf2.updated_by_user_id
 ORDER BY cf2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCustomFieldValuesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.CustomFieldValues(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldValuesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CustomFieldValuesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
cf.name AS ""CustomField"",
cfv.custom_field_id AS ""CustomFieldId"",
cfv.id2 AS ""Id2"",
cfv.value AS ""Value"",
cfv.default AS ""Default"" 
FROM uc.custom_field_values AS cfv
LEFT JOIN uc.custom_fields AS cf ON cf.id = cfv.custom_field_id
 ORDER BY cfv.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldValuesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDepartment2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Department2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Department2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
d2.id AS ""Id"",
d2.name AS ""Name"",
d2.code AS ""Code"",
d2.usage_number AS ""UsageNumber"",
d2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
d2.created_by_user_id AS ""CreationUserId"",
d2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
d2.updated_by_user_id AS ""LastWriteUserId"",
d2.content AS ""Content"" 
FROM uc.departments AS d2
LEFT JOIN uc.users AS cu ON cu.id = d2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = d2.updated_by_user_id
 ORDER BY d2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDocument2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Document2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Document2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Document2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
d2.id AS ""Id"",
d2.document_metadata_name AS ""DocumentMetadataName"",
dmi.folio_invoice_no AS ""DocumentMetadataInvoice"",
d2.document_metadata_invoice_id AS ""DocumentMetadataInvoiceId"",
d2.document_metadata_url AS ""DocumentMetadataUrl"",
d2.document_metadata_metadata_created_date AS ""DocumentMetadataMetadataCreatedDate"",
dmmcbu.username AS ""DocumentMetadataMetadataCreatedByUser"",
d2.document_metadata_metadata_created_by_user_id AS ""DocumentMetadataMetadataCreatedByUserId"",
d2.document_metadata_metadata_created_by_username AS ""DocumentMetadataMetadataCreatedByUsername"",
d2.document_metadata_metadata_updated_date AS ""DocumentMetadataMetadataUpdatedDate"",
dmmubu.username AS ""DocumentMetadataMetadataUpdatedByUser"",
d2.document_metadata_metadata_updated_by_user_id AS ""DocumentMetadataMetadataUpdatedByUserId"",
d2.document_metadata_metadata_updated_by_username AS ""DocumentMetadataMetadataUpdatedByUsername"",
d2.contents_data AS ""ContentsData"",
d2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
d2.created_by_user_id AS ""CreationUserId"",
d2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
d2.updated_by_user_id AS ""LastWriteUserId"",
d2.content AS ""Content"",
d2.invoiceid AS ""Invoiceid"",
d2.document_data AS ""DocumentData"" 
FROM uc.documents AS d2
LEFT JOIN uc.invoices AS dmi ON dmi.id = d2.document_metadata_invoice_id
LEFT JOIN uc.users AS dmmcbu ON dmmcbu.id = d2.document_metadata_metadata_created_by_user_id
LEFT JOIN uc.users AS dmmubu ON dmmubu.id = d2.document_metadata_metadata_updated_by_user_id
LEFT JOIN uc.users AS cu ON cu.id = d2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = d2.updated_by_user_id
 ORDER BY d2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Document2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryEditionsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Editions(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void EditionsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
e2.content AS ""Content"" 
FROM uc.editions AS e2
LEFT JOIN uc.instances AS i ON i.id = e2.instance_id
 ORDER BY e2.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EditionsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ElectronicAccesses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ElectronicAccessesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
ea.uri AS ""Uri"",
ea.link_text AS ""LinkText"",
ea.materials_specification AS ""MaterialsSpecification"",
ea.public_note AS ""PublicNote"",
r.id AS ""Relationship"",
ea.relationship_id AS ""RelationshipId"" 
FROM uc.electronic_accesses AS ea
LEFT JOIN uc.instances AS i ON i.id = ea.instance_id
LEFT JOIN uc.relationships AS r ON r.id = ea.relationship_id
 ORDER BY ea.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryElectronicAccessRelationship2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ElectronicAccessRelationship2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ElectronicAccessRelationship2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ear2.id AS ""Id"",
ear2.name AS ""Name"",
ear2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ear2.created_by_user_id AS ""CreationUserId"",
ear2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ear2.updated_by_user_id AS ""LastWriteUserId"",
ear2.content AS ""Content"" 
FROM uc.electronic_access_relationships AS ear2
LEFT JOIN uc.users AS cu ON cu.id = ear2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ear2.updated_by_user_id
 ORDER BY ear2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryErrorRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ErrorRecord2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ErrorRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ErrorRecord2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
er2.id AS ""Id"",
r2.id AS ""Record2"",
er2.content AS ""Content"",
er2.description AS ""Description"" 
FROM uc.error_records AS er2
LEFT JOIN uc.records AS r2 ON r2.id = er2.id
 ORDER BY er2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ErrorRecord2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryEventLog2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.EventLog2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EventLog2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void EventLog2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
el2.id AS ""Id"",
el2.tenant AS ""Tenant"",
u.username AS ""User"",
el2.user_id AS ""UserId"",
el2.ip AS ""Ip"",
el2.browser_information AS ""BrowserInformation"",
el2.timestamp AS ""Timestamp"",
el2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
el2.created_by_user_id AS ""CreationUserId"",
el2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
el2.updated_by_user_id AS ""LastWriteUserId"",
el2.content AS ""Content"" 
FROM uc.event_logs AS el2
LEFT JOIN uc.users AS u ON u.id = el2.user_id
LEFT JOIN uc.users AS cu ON cu.id = el2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = el2.updated_by_user_id
 ORDER BY el2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EventLog2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ExpenseClass2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ExpenseClass2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ec2.id AS ""Id"",
ec2.code AS ""Code"",
ec2.external_account_number_ext AS ""AccountNumberExtension"",
ec2.name AS ""Name"",
ec2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ec2.created_by_user_id AS ""CreationUserId"",
ec2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ec2.updated_by_user_id AS ""LastWriteUserId"",
ec2.content AS ""Content"" 
FROM uc.expense_classes AS ec2
LEFT JOIN uc.users AS cu ON cu.id = ec2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ec2.updated_by_user_id
 ORDER BY ec2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExportConfigCredential2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ExportConfigCredential2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExportConfigCredential2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ExportConfigCredential2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ecc2.id AS ""Id"",
ecc2.username AS ""Username"",
ecc2.password AS ""Password"",
ec.id AS ""ExportConfig"",
ecc2.export_config_id AS ""ExportConfigId"",
ecc2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ecc2.created_by_user_id AS ""CreationUserId"",
ecc2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ecc2.updated_by_user_id AS ""LastWriteUserId"",
ecc2.content AS ""Content"" 
FROM uc.export_config_credentials AS ecc2
LEFT JOIN uc.batch_voucher_export_configs AS ec ON ec.id = ecc2.export_config_id
LEFT JOIN uc.users AS cu ON cu.id = ecc2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ecc2.updated_by_user_id
 ORDER BY ecc2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExportConfigCredential2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExtentsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Extents(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExtentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ExtentsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
e2.holding_id AS ""HoldingId"",
e2.statement AS ""Content"",
e2.note AS ""Note"",
e2.staff_note AS ""StaffNote"" 
FROM uc.extents AS e2
LEFT JOIN uc.holdings AS h ON h.id = e2.holding_id
 ORDER BY e2.statement
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExtentsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFee2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Fee2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fee2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f2.id AS ""Id"",
f2.amount AS ""Amount"",
f2.remaining AS ""RemainingAmount"",
f2.status_name AS ""StatusName"",
f2.payment_status_name AS ""PaymentStatusName"",
f2.title AS ""Title"",
f2.call_number AS ""CallNumber"",
f2.barcode AS ""Barcode"",
f2.material_type AS ""MaterialType"",
f2.item_status_name AS ""ItemStatusName"",
f2.location AS ""Location"",
f2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
f2.created_by_user_id AS ""CreationUserId"",
f2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
f2.updated_by_user_id AS ""LastWriteUserId"",
f2.due_date AS ""DueTime"",
f2.returned_date AS ""ReturnedTime"",
l.id AS ""Loan"",
f2.loan_id AS ""LoanId"",
u.username AS ""User"",
f2.user_id AS ""UserId"",
i.hrid AS ""Item"",
f2.item_id AS ""ItemId"",
mt1.name AS ""MaterialType1"",
f2.material_type_id AS ""MaterialTypeId"",
ft.fee_fine_type AS ""FeeType"",
f2.fee_type_id AS ""FeeTypeId"",
o.owner AS ""Owner"",
f2.owner_id AS ""OwnerId"",
h.hrid AS ""Holding"",
f2.holding_id AS ""HoldingId"",
i2.title AS ""Instance"",
f2.instance_id AS ""InstanceId"",
f2.content AS ""Content"" 
FROM uc.fees AS f2
LEFT JOIN uc.users AS cu ON cu.id = f2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = f2.updated_by_user_id
LEFT JOIN uc.loans AS l ON l.id = f2.loan_id
LEFT JOIN uc.users AS u ON u.id = f2.user_id
LEFT JOIN uc.items AS i ON i.id = f2.item_id
LEFT JOIN uc.material_types AS mt1 ON mt1.id = f2.material_type_id
LEFT JOIN uc.fee_types AS ft ON ft.id = f2.fee_type_id
LEFT JOIN uc.owners AS o ON o.id = f2.owner_id
LEFT JOIN uc.holdings AS h ON h.id = f2.holding_id
LEFT JOIN uc.instances AS i2 ON i2.id = f2.instance_id
 ORDER BY f2.title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFeeType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FeeType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeeType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ft2.id AS ""Id"",
ft2.automatic AS ""Automatic"",
ft2.fee_fine_type AS ""Name"",
ft2.default_amount AS ""DefaultAmount"",
cn.name AS ""ChargeNotice"",
ft2.charge_notice_id AS ""ChargeNoticeId"",
an.name AS ""ActionNotice"",
ft2.action_notice_id AS ""ActionNoticeId"",
o.owner AS ""Owner"",
ft2.owner_id AS ""OwnerId"",
ft2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ft2.created_by_user_id AS ""CreationUserId"",
ft2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ft2.updated_by_user_id AS ""LastWriteUserId"",
ft2.content AS ""Content"" 
FROM uc.fee_types AS ft2
LEFT JOIN uc.templates AS cn ON cn.id = ft2.charge_notice_id
LEFT JOIN uc.templates AS an ON an.id = ft2.action_notice_id
LEFT JOIN uc.owners AS o ON o.id = ft2.owner_id
LEFT JOIN uc.users AS cu ON cu.id = ft2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ft2.updated_by_user_id
 ORDER BY ft2.fee_fine_type
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFinanceGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FinanceGroup2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceGroup2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fg2.id AS ""Id"",
fg2.code AS ""Code"",
fg2.description AS ""Description"",
fg2.name AS ""Name"",
fg2.status AS ""Status"",
fg2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
fg2.created_by_user_id AS ""CreationUserId"",
fg2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
fg2.updated_by_user_id AS ""LastWriteUserId"",
fg2.content AS ""Content"" 
FROM uc.finance_groups AS fg2
LEFT JOIN uc.users AS cu ON cu.id = fg2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = fg2.updated_by_user_id
 ORDER BY fg2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFinanceGroupAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FinanceGroupAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceGroupAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fg.name AS ""FinanceGroup"",
fgau.finance_group_id AS ""FinanceGroupId"",
au.name AS ""AcquisitionsUnit"",
fgau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.finance_group_acquisitions_units AS fgau
LEFT JOIN uc.finance_groups AS fg ON fg.id = fgau.finance_group_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = fgau.acquisitions_unit_id
 ORDER BY fgau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFiscalYear2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FiscalYear2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FiscalYear2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fy2.id AS ""Id"",
fy2.name AS ""Name"",
fy2.code AS ""Code"",
fy2.currency AS ""Currency"",
fy2.description AS ""Description"",
fy2.period_start AS ""StartDate"",
fy2.period_end AS ""EndDate"",
fy2.series AS ""Series"",
fy2.financial_summary_allocated AS ""FinancialSummaryAllocated"",
fy2.financial_summary_available AS ""FinancialSummaryAvailable"",
fy2.financial_summary_unavailable AS ""FinancialSummaryUnavailable"",
fy2.financial_summary_initial_allocation AS ""FinancialSummaryInitialAllocation"",
fy2.financial_summary_allocation_to AS ""FinancialSummaryAllocationTo"",
fy2.financial_summary_allocation_from AS ""FinancialSummaryAllocationFrom"",
fy2.financial_summary_total_funding AS ""FinancialSummaryTotalFunding"",
fy2.financial_summary_cash_balance AS ""FinancialSummaryCashBalance"",
fy2.financial_summary_awaiting_payment AS ""FinancialSummaryAwaitingPayment"",
fy2.financial_summary_encumbered AS ""FinancialSummaryEncumbered"",
fy2.financial_summary_expenditures AS ""FinancialSummaryExpenditures"",
fy2.financial_summary_over_encumbrance AS ""FinancialSummaryOverEncumbrance"",
fy2.financial_summary_over_expended AS ""FinancialSummaryOverExpended"",
fy2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
fy2.created_by_user_id AS ""CreationUserId"",
fy2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
fy2.updated_by_user_id AS ""LastWriteUserId"",
fy2.content AS ""Content"" 
FROM uc.fiscal_years AS fy2
LEFT JOIN uc.users AS cu ON cu.id = fy2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = fy2.updated_by_user_id
 ORDER BY fy2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFiscalYearAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FiscalYearAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FiscalYearAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fy.name AS ""FiscalYear"",
fyau.fiscal_year_id AS ""FiscalYearId"",
au.name AS ""AcquisitionsUnit"",
fyau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.fiscal_year_acquisitions_units AS fyau
LEFT JOIN uc.fiscal_years AS fy ON fy.id = fyau.fiscal_year_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = fyau.acquisitions_unit_id
 ORDER BY fyau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFixedDueDateSchedule2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FixedDueDateSchedule2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FixedDueDateSchedule2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fdds2.id AS ""Id"",
fdds2.name AS ""Name"",
fdds2.description AS ""Description"",
fdds2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
fdds2.created_by_user_id AS ""CreationUserId"",
fdds2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
fdds2.updated_by_user_id AS ""LastWriteUserId"",
fdds2.content AS ""Content"" 
FROM uc.fixed_due_date_schedules AS fdds2
LEFT JOIN uc.users AS cu ON cu.id = fdds2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = fdds2.updated_by_user_id
 ORDER BY fdds2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFixedDueDateScheduleSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FixedDueDateScheduleSchedules(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateScheduleSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FixedDueDateScheduleSchedulesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
fdds.name AS ""FixedDueDateSchedule"",
fddss.fixed_due_date_schedule_id AS ""FixedDueDateScheduleId"",
fddss.from AS ""From"",
fddss.to AS ""To"",
fddss.due AS ""Due"" 
FROM uc.fixed_due_date_schedule_schedules AS fddss
LEFT JOIN uc.fixed_due_date_schedules AS fdds ON fdds.id = fddss.fixed_due_date_schedule_id
 ORDER BY fddss.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateScheduleSchedulesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFormatsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Formats(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FormatsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f.id AS ""Id"",
f.name AS ""Name"",
f.code AS ""Code"",
f.source AS ""Source"",
f.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
f.created_by_user_id AS ""CreationUserId"",
f.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
f.updated_by_user_id AS ""LastWriteUserId"",
f.content AS ""Content"" 
FROM uc.formats AS f
LEFT JOIN uc.users AS cu ON cu.id = f.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = f.updated_by_user_id
 ORDER BY f.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFund2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Fund2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fund2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f2.id AS ""Id"",
f2.code AS ""Code"",
f2.description AS ""Description"",
f2.external_account_no AS ""AccountNumber"",
f2.fund_status AS ""FundStatus"",
ft.name AS ""FundType"",
f2.fund_type_id AS ""FundTypeId"",
l.name AS ""Ledger"",
f2.ledger_id AS ""LedgerId"",
f2.name AS ""Name"",
f2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
f2.created_by_user_id AS ""CreationUserId"",
f2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
f2.updated_by_user_id AS ""LastWriteUserId"",
f2.content AS ""Content"" 
FROM uc.funds AS f2
LEFT JOIN uc.fund_types AS ft ON ft.id = f2.fund_type_id
LEFT JOIN uc.ledgers AS l ON l.id = f2.ledger_id
LEFT JOIN uc.users AS cu ON cu.id = f2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = f2.updated_by_user_id
 ORDER BY f2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FundAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FundAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f.name AS ""Fund"",
fau.fund_id AS ""FundId"",
au.name AS ""AcquisitionsUnit"",
fau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.fund_acquisitions_units AS fau
LEFT JOIN uc.funds AS f ON f.id = fau.fund_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = fau.acquisitions_unit_id
 ORDER BY fau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FundTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FundTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
f.name AS ""Fund"",
ft.fund_id AS ""FundId"",
t.id AS ""Tag"",
ft.tag_id AS ""TagId"" 
FROM uc.fund_tags AS ft
LEFT JOIN uc.funds AS f ON f.id = ft.fund_id
LEFT JOIN uc.tags AS t ON t.id = ft.tag_id
 ORDER BY ft.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.FundType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FundType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ft2.id AS ""Id"",
ft2.name AS ""Name"",
ft2.content AS ""Content"" 
FROM uc.fund_types AS ft2
 ORDER BY ft2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Group2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Group2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
g2.id AS ""Id"",
g2.group AS ""Name"",
g2.desc AS ""Description"",
g2.expiration_offset_in_days AS ""ExpirationOffsetInDays"",
g2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
g2.created_by_user_id AS ""CreationUserId"",
g2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
g2.updated_by_user_id AS ""LastWriteUserId"",
g2.content AS ""Content"" 
FROM uc.groups AS g2
LEFT JOIN uc.users AS cu ON cu.id = g2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = g2.updated_by_user_id
 ORDER BY g2.group
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHolding2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Holding2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Holding2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h2.id AS ""Id"",
h2._version AS ""Version"",
h2.hrid AS ""ShortId"",
ht.name AS ""HoldingType"",
h2.holding_type_id AS ""HoldingTypeId"",
i.title AS ""Instance"",
h2.instance_id AS ""InstanceId"",
l.name AS ""Location"",
h2.permanent_location_id AS ""LocationId"",
tl.name AS ""TemporaryLocation"",
h2.temporary_location_id AS ""TemporaryLocationId"",
el.name AS ""EffectiveLocation"",
h2.effective_location_id AS ""EffectiveLocationId"",
cnt.name AS ""CallNumberType"",
h2.call_number_type_id AS ""CallNumberTypeId"",
h2.call_number_prefix AS ""CallNumberPrefix"",
h2.call_number AS ""CallNumber"",
h2.call_number_suffix AS ""CallNumberSuffix"",
h2.shelving_title AS ""ShelvingTitle"",
h2.acquisition_format AS ""AcquisitionFormat"",
h2.acquisition_method AS ""AcquisitionMethod"",
h2.receipt_status AS ""ReceiptStatus"",
ip.name AS ""IllPolicy"",
h2.ill_policy_id AS ""IllPolicyId"",
h2.retention_policy AS ""RetentionPolicy"",
h2.digitization_policy AS ""DigitizationPolicy"",
h2.copy_number AS ""CopyNumber"",
h2.number_of_items AS ""ItemCount"",
h2.receiving_history_display_type AS ""ReceivingHistoryDisplayType"",
h2.discovery_suppress AS ""DiscoverySuppress"",
h2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
h2.created_by_user_id AS ""CreationUserId"",
h2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
h2.updated_by_user_id AS ""LastWriteUserId"",
s.name AS ""Source"",
h2.source_id AS ""SourceId"",
h2.content AS ""Content"" 
FROM uc.holdings AS h2
LEFT JOIN uc.holding_types AS ht ON ht.id = h2.holding_type_id
LEFT JOIN uc.instances AS i ON i.id = h2.instance_id
LEFT JOIN uc.locations AS l ON l.id = h2.permanent_location_id
LEFT JOIN uc.locations AS tl ON tl.id = h2.temporary_location_id
LEFT JOIN uc.locations AS el ON el.id = h2.effective_location_id
LEFT JOIN uc.call_number_types AS cnt ON cnt.id = h2.call_number_type_id
LEFT JOIN uc.ill_policies AS ip ON ip.id = h2.ill_policy_id
LEFT JOIN uc.users AS cu ON cu.id = h2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = h2.updated_by_user_id
LEFT JOIN uc.sources AS s ON s.id = h2.source_id
 ORDER BY h2.hrid
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingElectronicAccesses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingElectronicAccessesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
hea.uri AS ""Uri"",
hea.link_text AS ""LinkText"",
hea.materials_specification AS ""MaterialsSpecification"",
hea.public_note AS ""PublicNote"",
r.id AS ""Relationship"",
hea.relationship_id AS ""RelationshipId"" 
FROM uc.holding_electronic_accesses AS hea
LEFT JOIN uc.holdings AS h ON h.id = hea.holding_id
LEFT JOIN uc.relationships AS r ON r.id = hea.relationship_id
 ORDER BY hea.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingElectronicAccessesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingEntriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingEntries(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingEntriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingEntriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
he.public_display AS ""PublicDisplay"",
he.enumeration AS ""Enumeration"",
he.chronology AS ""Chronology"" 
FROM uc.holding_entries AS he
LEFT JOIN uc.holdings AS h ON h.id = he.holding_id
 ORDER BY he.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingEntriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingFormerIds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingFormerIdsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
hfi.content AS ""Content"" 
FROM uc.holding_former_ids AS hfi
LEFT JOIN uc.holdings AS h ON h.id = hfi.holding_id
 ORDER BY hfi.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingFormerIdsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingNotesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingNotes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingNotesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
hnt.name AS ""HoldingNoteType"",
hn.holding_note_type_id AS ""HoldingNoteTypeId"",
hn.note AS ""Note"",
hn.staff_only AS ""StaffOnly"" 
FROM uc.holding_notes AS hn
LEFT JOIN uc.holdings AS h ON h.id = hn.holding_id
LEFT JOIN uc.holding_note_types AS hnt ON hnt.id = hn.holding_note_type_id
 ORDER BY hn.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNotesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingNoteType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingNoteType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
hnt2.id AS ""Id"",
hnt2.name AS ""Name"",
hnt2.source AS ""Source"",
hnt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
hnt2.created_by_user_id AS ""CreationUserId"",
hnt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
hnt2.updated_by_user_id AS ""LastWriteUserId"",
hnt2.content AS ""Content"" 
FROM uc.holding_note_types AS hnt2
LEFT JOIN uc.users AS cu ON cu.id = hnt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = hnt2.updated_by_user_id
 ORDER BY hnt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingStatisticalCodes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingStatisticalCodesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
sc.name AS ""StatisticalCode"",
hsc.statistical_code_id AS ""StatisticalCodeId"" 
FROM uc.holding_statistical_codes AS hsc
LEFT JOIN uc.holdings AS h ON h.id = hsc.holding_id
LEFT JOIN uc.statistical_codes AS sc ON sc.id = hsc.statistical_code_id
 ORDER BY hsc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingStatisticalCodesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
ht.content AS ""Content"" 
FROM uc.holding_tags AS ht
LEFT JOIN uc.holdings AS h ON h.id = ht.holding_id
 ORDER BY ht.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HoldingType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HoldingType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ht2.id AS ""Id"",
ht2.name AS ""Name"",
ht2.source AS ""Source"",
ht2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ht2.created_by_user_id AS ""CreationUserId"",
ht2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ht2.updated_by_user_id AS ""LastWriteUserId"",
ht2.content AS ""Content"" 
FROM uc.holding_types AS ht2
LEFT JOIN uc.users AS cu ON cu.id = ht2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ht2.updated_by_user_id
 ORDER BY ht2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHridSetting2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.HridSetting2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HridSetting2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void HridSetting2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
hs2.id AS ""Id"",
hs2.instances_prefix AS ""InstancesPrefix"",
hs2.instances_start_number AS ""InstancesStartNumber"",
hs2.holdings_prefix AS ""HoldingsPrefix"",
hs2.holdings_start_number AS ""HoldingsStartNumber"",
hs2.items_prefix AS ""ItemsPrefix"",
hs2.items_start_number AS ""ItemsStartNumber"",
hs2.common_retain_leading_zeroes AS ""ZeroPad"",
hs2.content AS ""Content"",
hs2.lock AS ""Lock"" 
FROM uc.hrid_settings AS hs2
 ORDER BY hs2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HridSetting2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Identifiers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void IdentifiersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.title AS ""Instance"",
i.instance_id AS ""InstanceId"",
i.value AS ""Content"",
it.name AS ""IdentifierType"",
i.identifier_type_id AS ""IdentifierTypeId"" 
FROM uc.identifiers AS i
LEFT JOIN uc.instances AS i2 ON i2.id = i.instance_id
LEFT JOIN uc.id_types AS it ON it.id = i.identifier_type_id
 ORDER BY i.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdentifiersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIdType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.IdType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void IdType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
it2.id AS ""Id"",
it2.name AS ""Name"",
it2.source AS ""Source"",
it2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
it2.created_by_user_id AS ""CreationUserId"",
it2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
it2.updated_by_user_id AS ""LastWriteUserId"",
it2.content AS ""Content"" 
FROM uc.id_types AS it2
LEFT JOIN uc.users AS cu ON cu.id = it2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = it2.updated_by_user_id
 ORDER BY it2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIllPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.IllPolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void IllPolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ip2.id AS ""Id"",
ip2.name AS ""Name"",
ip2.source AS ""Source"",
ip2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ip2.created_by_user_id AS ""CreationUserId"",
ip2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ip2.updated_by_user_id AS ""LastWriteUserId"",
ip2.content AS ""Content"" 
FROM uc.ill_policies AS ip2
LEFT JOIN uc.users AS cu ON cu.id = ip2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ip2.updated_by_user_id
 ORDER BY ip2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIndexStatementsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.IndexStatements(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IndexStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void IndexStatementsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
@is.statement AS ""Statement"",
@is.note AS ""Note"",
@is.staff_note AS ""StaffNote"" 
FROM uc.index_statements AS @is
LEFT JOIN uc.holdings AS h ON h.id = @is.holding_id
 ORDER BY @is.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IndexStatementsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstance2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Instance2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Instance2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.id AS ""Id"",
i2._version AS ""Version"",
i2.hrid AS ""ShortId"",
i2.match_key AS ""MatchKey"",
i2.source AS ""Source"",
i2.title AS ""Title"",
i2.author AS ""Author"",
i2.publication_year AS ""PublicationYear"",
i2.publication_period_start AS ""PublicationPeriodStart"",
i2.publication_period_end AS ""PublicationPeriodEnd"",
it.name AS ""InstanceType"",
i2.instance_type_id AS ""InstanceTypeId"",
im.name AS ""IssuanceMode"",
i2.mode_of_issuance_id AS ""IssuanceModeId"",
i2.cataloged_date AS ""CatalogedDate"",
i2.previously_held AS ""PreviouslyHeld"",
i2.staff_suppress AS ""StaffSuppress"",
i2.discovery_suppress AS ""DiscoverySuppress"",
i2.source_record_format AS ""SourceRecordFormat"",
s.name AS ""Status"",
i2.status_id AS ""StatusId"",
i2.status_updated_date AS ""StatusLastWriteTime"",
i2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
i2.created_by_user_id AS ""CreationUserId"",
i2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
i2.updated_by_user_id AS ""LastWriteUserId"",
i2.content AS ""Content"" 
FROM uc.instances AS i2
LEFT JOIN uc.instance_types AS it ON it.id = i2.instance_type_id
LEFT JOIN uc.mode_of_issuances AS im ON im.id = i2.mode_of_issuance_id
LEFT JOIN uc.statuses AS s ON s.id = i2.status_id
LEFT JOIN uc.users AS cu ON cu.id = i2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = i2.updated_by_user_id
 ORDER BY i2.title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceFormat2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceFormat2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormat2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceFormat2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
f.name AS ""Format"",
if2.format_id AS ""FormatId"" 
FROM uc.instance_formats AS if2
LEFT JOIN uc.instances AS i ON i.id = if2.instance_id
LEFT JOIN uc.formats AS f ON f.id = if2.format_id
 ORDER BY if2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormat2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceNatureOfContentTerms(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceNatureOfContentTermsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
noct.name AS ""NatureOfContentTerm"",
inoct.nature_of_content_term_id AS ""NatureOfContentTermId"" 
FROM uc.instance_nature_of_content_terms AS inoct
LEFT JOIN uc.instances AS i ON i.id = inoct.instance_id
LEFT JOIN uc.nature_of_content_terms AS noct ON noct.id = inoct.nature_of_content_term_id
 ORDER BY inoct.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNatureOfContentTermsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceNoteType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceNoteType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
int2.id AS ""Id"",
int2.name AS ""Name"",
int2.source AS ""Source"",
int2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
int2.created_by_user_id AS ""CreationUserId"",
int2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
int2.updated_by_user_id AS ""LastWriteUserId"",
int2.content AS ""Content"" 
FROM uc.instance_note_types AS int2
LEFT JOIN uc.users AS cu ON cu.id = int2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = int2.updated_by_user_id
 ORDER BY int2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceStatisticalCodes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceStatisticalCodesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
sc.name AS ""StatisticalCode"",
isc.statistical_code_id AS ""StatisticalCodeId"" 
FROM uc.instance_statistical_codes AS isc
LEFT JOIN uc.instances AS i ON i.id = isc.instance_id
LEFT JOIN uc.statistical_codes AS sc ON sc.id = isc.statistical_code_id
 ORDER BY isc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatisticalCodesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
it.content AS ""Content"" 
FROM uc.instance_tags AS it
LEFT JOIN uc.instances AS i ON i.id = it.instance_id
 ORDER BY it.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InstanceType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InstanceType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
it2.id AS ""Id"",
it2.name AS ""Name"",
it2.code AS ""Code"",
it2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
it2.created_by_user_id AS ""CreationUserId"",
it2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
it2.updated_by_user_id AS ""LastWriteUserId"",
it2.content AS ""Content"" 
FROM uc.instance_types AS it2
LEFT JOIN uc.users AS cu ON cu.id = it2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = it2.updated_by_user_id
 ORDER BY it2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstitution2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Institution2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Institution2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.id AS ""Id"",
i2.name AS ""Name"",
i2.code AS ""Code"",
i2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
i2.created_by_user_id AS ""CreationUserId"",
i2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
i2.updated_by_user_id AS ""LastWriteUserId"",
i2.content AS ""Content"" 
FROM uc.institutions AS i2
LEFT JOIN uc.users AS cu ON cu.id = i2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = i2.updated_by_user_id
 ORDER BY i2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterface2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Interface2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Interface2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.id AS ""Id"",
i2.name AS ""Name"",
i2.uri AS ""Uri"",
i2.notes AS ""Notes"",
i2.available AS ""Available"",
i2.delivery_method AS ""DeliveryMethod"",
i2.statistics_format AS ""StatisticsFormat"",
i2.locally_stored AS ""LocallyStored"",
i2.online_location AS ""OnlineLocation"",
i2.statistics_notes AS ""StatisticsNotes"",
i2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
i2.created_by_user_id AS ""CreationUserId"",
i2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
i2.updated_by_user_id AS ""LastWriteUserId"",
i2.content AS ""Content"" 
FROM uc.interfaces AS i2
LEFT JOIN uc.users AS cu ON cu.id = i2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = i2.updated_by_user_id
 ORDER BY i2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterfaceCredential2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InterfaceCredential2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceCredential2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InterfaceCredential2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ic2.id AS ""Id"",
ic2.username AS ""Username"",
ic2.password AS ""Password"",
i.name AS ""Interface"",
ic2.interface_id AS ""InterfaceId"",
ic2.content AS ""Content"" 
FROM uc.interface_credentials AS ic2
LEFT JOIN uc.interfaces AS i ON i.id = ic2.interface_id
 ORDER BY ic2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceCredential2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterfaceTypesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InterfaceTypes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InterfaceTypesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.name AS ""Interface"",
it.interface_id AS ""InterfaceId"",
it.content AS ""Content"" 
FROM uc.interface_type AS it
LEFT JOIN uc.interfaces AS i ON i.id = it.interface_id
 ORDER BY it.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceTypesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Invoice2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoice2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.id AS ""Id"",
i2.accounting_code AS ""AccountingCode"",
i2.adjustments_total AS ""AdjustmentsTotal"",
ab.username AS ""ApprovedBy"",
i2.approved_by_id AS ""ApprovedById"",
i2.approval_date AS ""ApprovalDate"",
bg.name AS ""BatchGroup"",
i2.batch_group_id AS ""BatchGroupId"",
bt.id AS ""BillTo"",
i2.bill_to_id AS ""BillToId"",
i2.chk_subscription_overlap AS ""CheckSubscriptionOverlap"",
i2.cancellation_note AS ""CancellationNote"",
i2.currency AS ""Currency"",
i2.enclosure_needed AS ""Enclosure"",
i2.exchange_rate AS ""ExchangeRate"",
i2.export_to_accounting AS ""ExportToAccounting"",
i2.folio_invoice_no AS ""Number"",
i2.invoice_date AS ""InvoiceDate"",
i2.lock_total AS ""LockTotal"",
i2.note AS ""Note"",
i2.payment_due AS ""PaymentDueDate"",
i2.payment_date AS ""PaymentDate"",
i2.payment_terms AS ""PaymentTerms"",
i2.payment_method AS ""PaymentMethod"",
i2.status AS ""Status"",
i2.source AS ""Source"",
i2.sub_total AS ""SubTotal"",
i2.total AS ""Total"",
i2.vendor_invoice_no AS ""VendorInvoiceNumber"",
i2.disbursement_number AS ""DisbursementNumber"",
i2.voucher_number AS ""VoucherNumber"",
p.amount AS ""Payment"",
i2.payment_id AS ""PaymentId"",
i2.disbursement_date AS ""DisbursementDate"",
v.name AS ""Vendor"",
i2.vendor_id AS ""VendorId"",
i2.account_no AS ""AccountNumber"",
i2.manual_payment AS ""ManualPayment"",
i2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
i2.created_by_user_id AS ""CreationUserId"",
i2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
i2.updated_by_user_id AS ""LastWriteUserId"",
i2.content AS ""Content"",
its2.id AS ""InvoiceTransactionSummary2"" 
FROM uc.invoices AS i2
LEFT JOIN uc.users AS ab ON ab.id = i2.approved_by_id
LEFT JOIN uc.batch_groups AS bg ON bg.id = i2.batch_group_id
LEFT JOIN uc.configurations AS bt ON bt.id = i2.bill_to_id
LEFT JOIN uc.transactions AS p ON p.id = i2.payment_id
LEFT JOIN uc.organizations AS v ON v.id = i2.vendor_id
LEFT JOIN uc.users AS cu ON cu.id = i2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = i2.updated_by_user_id
LEFT JOIN uc.invoice_transaction_summaries AS its2 ON its2.id = i2.id
 ORDER BY i2.folio_invoice_no
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.folio_invoice_no AS ""Invoice"",
iau.invoice_id AS ""InvoiceId"",
au.name AS ""AcquisitionsUnit"",
iau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.invoice_acquisitions_units AS iau
LEFT JOIN uc.invoices AS i ON i.id = iau.invoice_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = iau.acquisitions_unit_id
 ORDER BY iau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceAdjustments(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceAdjustmentsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.folio_invoice_no AS ""Invoice"",
ia.invoice_id AS ""InvoiceId"",
ia.id2 AS ""Id2"",
ia.adjustment_id AS ""AdjustmentId"",
ia.description AS ""Description"",
ia.export_to_accounting AS ""ExportToAccounting"",
ia.prorate AS ""Prorate"",
ia.relation_to_total AS ""RelationToTotal"",
ia.type AS ""Type"",
ia.value AS ""Value"" 
FROM uc.invoice_adjustments AS ia
LEFT JOIN uc.invoices AS i ON i.id = ia.invoice_id
 ORDER BY ia.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceAdjustmentFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceAdjustmentFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
iaf.invoice_adjustment_id AS ""InvoiceAdjustmentId"",
iaf.code AS ""FundCode"",
e2.amount AS ""Encumbrance"",
iaf.encumbrance_id AS ""EncumbranceId"",
f.name AS ""Fund"",
iaf.fund_id AS ""FundId"",
ii.invoice_line_number AS ""InvoiceItem"",
iaf.invoice_item_id AS ""InvoiceItemId"",
iaf.distribution_type AS ""DistributionType"",
ec.name AS ""ExpenseClass"",
iaf.expense_class_id AS ""ExpenseClassId"",
iaf.value AS ""Value"" 
FROM uc.invoice_adjustment_fund_distributions AS iaf
LEFT JOIN uc.transactions AS e2 ON e2.id = iaf.encumbrance_id
LEFT JOIN uc.funds AS f ON f.id = iaf.fund_id
LEFT JOIN uc.invoice_items AS ii ON ii.id = iaf.invoice_item_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = iaf.expense_class_id
 ORDER BY iaf.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItem2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItem2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItem2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ii2.id AS ""Id"",
ii2.accounting_code AS ""AccountingCode"",
ii2.account_number AS ""AccountNumber"",
ii2.adjustments_total AS ""AdjustmentsTotal"",
ii2.comment AS ""Comment"",
ii2.description AS ""Description"",
i.folio_invoice_no AS ""Invoice"",
ii2.invoice_id AS ""InvoiceId"",
ii2.invoice_line_number AS ""Number"",
ii2.invoice_line_status AS ""InvoiceLineStatus"",
oi.po_line_number AS ""OrderItem"",
ii2.po_line_id AS ""OrderItemId"",
ii2.product_id AS ""ProductId"",
pit.name AS ""ProductIdType"",
ii2.product_id_type_id AS ""ProductIdTypeId"",
ii2.quantity AS ""Quantity"",
ii2.release_encumbrance AS ""ReleaseEncumbrance"",
ii2.subscription_info AS ""SubscriptionInfo"",
ii2.subscription_start AS ""SubscriptionStartDate"",
ii2.subscription_end AS ""SubscriptionEndDate"",
ii2.sub_total AS ""SubTotal"",
ii2.total AS ""Total"",
ii2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ii2.created_by_user_id AS ""CreationUserId"",
ii2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ii2.updated_by_user_id AS ""LastWriteUserId"",
ii2.content AS ""Content"" 
FROM uc.invoice_items AS ii2
LEFT JOIN uc.invoices AS i ON i.id = ii2.invoice_id
LEFT JOIN uc.order_items AS oi ON oi.id = ii2.po_line_id
LEFT JOIN uc.id_types AS pit ON pit.id = ii2.product_id_type_id
LEFT JOIN uc.users AS cu ON cu.id = ii2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ii2.updated_by_user_id
 ORDER BY ii2.invoice_line_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItemAdjustments(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItemAdjustmentsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ii.invoice_line_number AS ""InvoiceItem"",
iia.invoice_item_id AS ""InvoiceItemId"",
iia.id2 AS ""Id2"",
iia.adjustment_id AS ""AdjustmentId"",
iia.description AS ""Description"",
iia.export_to_accounting AS ""ExportToAccounting"",
iia.prorate AS ""Prorate"",
iia.relation_to_total AS ""RelationToTotal"",
iia.type AS ""Type"",
iia.value AS ""Value"" 
FROM uc.invoice_item_adjustments AS iia
LEFT JOIN uc.invoice_items AS ii ON ii.id = iia.invoice_item_id
 ORDER BY iia.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItemAdjustmentFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItemAdjustmentFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
iiaf.invoice_item_adjustment_id AS ""InvoiceItemAdjustmentId"",
iiaf.code AS ""FundCode"",
e2.amount AS ""Encumbrance"",
iiaf.encumbrance_id AS ""EncumbranceId"",
f.name AS ""Fund"",
iiaf.fund_id AS ""FundId"",
ii.invoice_line_number AS ""InvoiceItem"",
iiaf.invoice_item_id AS ""InvoiceItemId"",
iiaf.distribution_type AS ""DistributionType"",
ec.name AS ""ExpenseClass"",
iiaf.expense_class_id AS ""ExpenseClassId"",
iiaf.value AS ""Value"" 
FROM uc.invoice_item_adjustment_fund_distributions AS iiaf
LEFT JOIN uc.transactions AS e2 ON e2.id = iiaf.encumbrance_id
LEFT JOIN uc.funds AS f ON f.id = iiaf.fund_id
LEFT JOIN uc.invoice_items AS ii ON ii.id = iiaf.invoice_item_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = iiaf.expense_class_id
 ORDER BY iiaf.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItemFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItemFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ii.invoice_line_number AS ""InvoiceItem"",
iif.invoice_item_id AS ""InvoiceItemId"",
iif.code AS ""FundCode"",
e2.amount AS ""Encumbrance"",
iif.encumbrance_id AS ""EncumbranceId"",
f.name AS ""Fund"",
iif.fund_id AS ""FundId"",
iif.distribution_type AS ""DistributionType"",
ec.name AS ""ExpenseClass"",
iif.expense_class_id AS ""ExpenseClassId"",
iif.value AS ""Value"" 
FROM uc.invoice_item_fund_distributions AS iif
LEFT JOIN uc.invoice_items AS ii ON ii.id = iif.invoice_item_id
LEFT JOIN uc.transactions AS e2 ON e2.id = iif.encumbrance_id
LEFT JOIN uc.funds AS f ON f.id = iif.fund_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = iif.expense_class_id
 ORDER BY iif.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItemReferenceNumbers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItemReferenceNumbersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ii.invoice_line_number AS ""InvoiceItem"",
iirn.invoice_item_id AS ""InvoiceItemId"",
iirn.ref_number AS ""ReferenceNumber"",
iirn.ref_number_type AS ""ReferenceNumberType"",
iirn.vendor_details_source AS ""VendorDetailsSource"" 
FROM uc.invoice_item_reference_numbers AS iirn
LEFT JOIN uc.invoice_items AS ii ON ii.id = iirn.invoice_item_id
 ORDER BY iirn.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemReferenceNumbersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceItemTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceItemTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ii.invoice_line_number AS ""InvoiceItem"",
iit.invoice_item_id AS ""InvoiceItemId"",
iit.content AS ""Content"" 
FROM uc.invoice_item_tags AS iit
LEFT JOIN uc.invoice_items AS ii ON ii.id = iit.invoice_item_id
 ORDER BY iit.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceOrderNumbersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceOrderNumbers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceOrderNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceOrderNumbersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.folio_invoice_no AS ""Invoice"",
ion.invoice_id AS ""InvoiceId"",
ion.content AS ""Content"" 
FROM uc.invoice_order_numbers AS ion
LEFT JOIN uc.invoices AS i ON i.id = ion.invoice_id
 ORDER BY ion.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceOrderNumbersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.folio_invoice_no AS ""Invoice"",
it.invoice_id AS ""InvoiceId"",
it.content AS ""Content"" 
FROM uc.invoice_tags AS it
LEFT JOIN uc.invoices AS i ON i.id = it.invoice_id
 ORDER BY it.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceTransactionSummary2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.InvoiceTransactionSummary2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTransactionSummary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoiceTransactionSummary2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
its2.id AS ""Id"",
i2.folio_invoice_no AS ""Invoice2"",
its2.num_pending_payments AS ""NumPendingPayments"",
its2.num_payments_credits AS ""NumPaymentsCredits"",
its2.content AS ""Content"" 
FROM uc.invoice_transaction_summaries AS its2
LEFT JOIN uc.invoices AS i2 ON i2.id = its2.id
 ORDER BY its2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTransactionSummary2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIssuanceModesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.IssuanceModes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void IssuanceModesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
im.id AS ""Id"",
im.name AS ""Name"",
im.source AS ""Source"",
im.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
im.created_by_user_id AS ""CreationUserId"",
im.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
im.updated_by_user_id AS ""LastWriteUserId"",
im.content AS ""Content"" 
FROM uc.mode_of_issuances AS im
LEFT JOIN uc.users AS cu ON cu.id = im.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = im.updated_by_user_id
 ORDER BY im.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItem2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Item2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Item2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i2.id AS ""Id"",
i2._version AS ""Version"",
i2.hrid AS ""ShortId"",
h.hrid AS ""Holding"",
i2.holding_id AS ""HoldingId"",
i2.discovery_suppress AS ""DiscoverySuppress"",
i2.accession_number AS ""AccessionNumber"",
i2.barcode AS ""Barcode"",
i2.effective_shelving_order AS ""EffectiveShelvingOrder"",
i2.call_number AS ""CallNumber"",
i2.call_number_prefix AS ""CallNumberPrefix"",
i2.call_number_suffix AS ""CallNumberSuffix"",
cnt.name AS ""CallNumberType"",
i2.call_number_type_id AS ""CallNumberTypeId"",
i2.effective_call_number AS ""EffectiveCallNumber"",
i2.effective_call_number_prefix AS ""EffectiveCallNumberPrefix"",
i2.effective_call_number_suffix AS ""EffectiveCallNumberSuffix"",
ecnt.name AS ""EffectiveCallNumberType"",
i2.effective_call_number_type_id AS ""EffectiveCallNumberTypeId"",
i2.volume AS ""Volume"",
i2.enumeration AS ""Enumeration"",
i2.chronology AS ""Chronology"",
i2.item_identifier AS ""ItemIdentifier"",
i2.copy_number AS ""CopyNumber"",
i2.number_of_pieces AS ""PiecesCount"",
i2.description_of_pieces AS ""PiecesDescription"",
i2.number_of_missing_pieces AS ""MissingPiecesCount"",
i2.missing_pieces AS ""MissingPiecesDescription"",
i2.missing_pieces_date AS ""MissingPiecesTime"",
ds.name AS ""DamagedStatus"",
i2.item_damaged_status_id AS ""DamagedStatusId"",
i2.item_damaged_status_date AS ""DamagedStatusTime"",
i2.status_name AS ""StatusName"",
i2.status_date AS ""StatusDate"",
mt.name AS ""MaterialType"",
i2.material_type_id AS ""MaterialTypeId"",
plt.name AS ""PermanentLoanType"",
i2.permanent_loan_type_id AS ""PermanentLoanTypeId"",
tlt.name AS ""TemporaryLoanType"",
i2.temporary_loan_type_id AS ""TemporaryLoanTypeId"",
pl.name AS ""PermanentLocation"",
i2.permanent_location_id AS ""PermanentLocationId"",
tl.name AS ""TemporaryLocation"",
i2.temporary_location_id AS ""TemporaryLocationId"",
el.name AS ""EffectiveLocation"",
i2.effective_location_id AS ""EffectiveLocationId"",
itdsp.name AS ""InTransitDestinationServicePoint"",
i2.in_transit_destination_service_point_id AS ""InTransitDestinationServicePointId"",
oi.po_line_number AS ""OrderItem"",
i2.order_item_id AS ""OrderItemId"",
i2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
i2.created_by_user_id AS ""CreationUserId"",
i2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
i2.updated_by_user_id AS ""LastWriteUserId"",
i2.last_check_in_date_time AS ""LastCheckInDateTime"",
lcisp.name AS ""LastCheckInServicePoint"",
i2.last_check_in_service_point_id AS ""LastCheckInServicePointId"",
lcism.username AS ""LastCheckInStaffMember"",
i2.last_check_in_staff_member_id AS ""LastCheckInStaffMemberId"",
i2.content AS ""Content"" 
FROM uc.items AS i2
LEFT JOIN uc.holdings AS h ON h.id = i2.holding_id
LEFT JOIN uc.call_number_types AS cnt ON cnt.id = i2.call_number_type_id
LEFT JOIN uc.call_number_types AS ecnt ON ecnt.id = i2.effective_call_number_type_id
LEFT JOIN uc.item_damaged_statuses AS ds ON ds.id = i2.item_damaged_status_id
LEFT JOIN uc.material_types AS mt ON mt.id = i2.material_type_id
LEFT JOIN uc.loan_types AS plt ON plt.id = i2.permanent_loan_type_id
LEFT JOIN uc.loan_types AS tlt ON tlt.id = i2.temporary_loan_type_id
LEFT JOIN uc.locations AS pl ON pl.id = i2.permanent_location_id
LEFT JOIN uc.locations AS tl ON tl.id = i2.temporary_location_id
LEFT JOIN uc.locations AS el ON el.id = i2.effective_location_id
LEFT JOIN uc.service_points AS itdsp ON itdsp.id = i2.in_transit_destination_service_point_id
LEFT JOIN uc.order_items AS oi ON oi.id = i2.order_item_id
LEFT JOIN uc.users AS cu ON cu.id = i2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = i2.updated_by_user_id
LEFT JOIN uc.service_points AS lcisp ON lcisp.id = i2.last_check_in_service_point_id
LEFT JOIN uc.users AS lcism ON lcism.id = i2.last_check_in_staff_member_id
 ORDER BY i2.hrid
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemDamagedStatus2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemDamagedStatus2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemDamagedStatus2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ids2.id AS ""Id"",
ids2.name AS ""Name"",
ids2.source AS ""Source"",
ids2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ids2.created_by_user_id AS ""CreationUserId"",
ids2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ids2.updated_by_user_id AS ""LastWriteUserId"",
ids2.content AS ""Content"" 
FROM uc.item_damaged_statuses AS ids2
LEFT JOIN uc.users AS cu ON cu.id = ids2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ids2.updated_by_user_id
 ORDER BY ids2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemElectronicAccesses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemElectronicAccessesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
iea.uri AS ""Uri"",
iea.link_text AS ""LinkText"",
iea.materials_specification AS ""MaterialsSpecification"",
iea.public_note AS ""PublicNote"",
r.id AS ""Relationship"",
iea.relationship_id AS ""RelationshipId"" 
FROM uc.item_electronic_accesses AS iea
LEFT JOIN uc.items AS i ON i.id = iea.item_id
LEFT JOIN uc.relationships AS r ON r.id = iea.relationship_id
 ORDER BY iea.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemElectronicAccessesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemFormerIds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemFormerIdsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
ifi.content AS ""Content"" 
FROM uc.item_former_ids AS ifi
LEFT JOIN uc.items AS i ON i.id = ifi.item_id
 ORDER BY ifi.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemFormerIdsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemNotesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemNotes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemNotesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
@int.name AS ""ItemNoteType"",
@in.item_note_type_id AS ""ItemNoteTypeId"",
@in.note AS ""Note"",
@in.staff_only AS ""StaffOnly"" 
FROM uc.item_notes AS @in
LEFT JOIN uc.items AS i ON i.id = @in.item_id
LEFT JOIN uc.item_note_types AS @int ON @int.id = @in.item_note_type_id
 ORDER BY @in.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNotesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemNoteType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemNoteType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
int2.id AS ""Id"",
int2.name AS ""Name"",
int2.source AS ""Source"",
int2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
int2.created_by_user_id AS ""CreationUserId"",
int2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
int2.updated_by_user_id AS ""LastWriteUserId"",
int2.content AS ""Content"" 
FROM uc.item_note_types AS int2
LEFT JOIN uc.users AS cu ON cu.id = int2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = int2.updated_by_user_id
 ORDER BY int2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemStatisticalCodes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemStatisticalCodesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
sc.name AS ""StatisticalCode"",
isc.statistical_code_id AS ""StatisticalCodeId"" 
FROM uc.item_statistical_codes AS isc
LEFT JOIN uc.items AS i ON i.id = isc.item_id
LEFT JOIN uc.statistical_codes AS sc ON sc.id = isc.statistical_code_id
 ORDER BY isc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatisticalCodesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
it.content AS ""Content"" 
FROM uc.item_tags AS it
LEFT JOIN uc.items AS i ON i.id = it.item_id
 ORDER BY it.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemYearCaptionsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ItemYearCaptions(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemYearCaptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ItemYearCaptionsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.hrid AS ""Item"",
iyc.content AS ""Content"" 
FROM uc.item_year_caption AS iyc
LEFT JOIN uc.items AS i ON i.id = iyc.item_id
 ORDER BY iyc.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemYearCaptionsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryJobExecution2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.JobExecution2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecution2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void JobExecution2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
je2.id AS ""Id"",
je2.hr_id AS ""HrId"",
pj.id AS ""ParentJob"",
je2.parent_job_id AS ""ParentJobId"",
je2.subordination_type AS ""SubordinationType"",
je2.job_profile_info_name AS ""JobProfileInfoName"",
je2.job_profile_info_data_type AS ""JobProfileInfoDataType"",
je2.job_profile_snapshot_wrapper_profile_id AS ""JobProfileSnapshotWrapperProfileId"",
je2.job_profile_snapshot_wrapper_content_type AS ""JobProfileSnapshotWrapperContentType"",
je2.job_profile_snapshot_wrapper_react_to AS ""JobProfileSnapshotWrapperReactTo"",
je2.job_profile_snapshot_wrapper_order AS ""JobProfileSnapshotWrapperOrder"",
je2.source_path AS ""SourcePath"",
je2.file_name AS ""FileName"",
je2.run_by_first_name AS ""RunByFirstName"",
je2.run_by_last_name AS ""RunByLastName"",
pje.id AS ""ProgressJobExecution"",
je2.progress_job_execution_id AS ""ProgressJobExecutionId"",
je2.progress_current AS ""ProgressCurrent"",
je2.progress_total AS ""ProgressTotal"",
je2.started_date AS ""StartedDate"",
je2.completed_date AS ""CompletedDate"",
je2.status AS ""Status"",
je2.ui_status AS ""UiStatus"",
je2.error_status AS ""ErrorStatus"",
u.username AS ""User"",
je2.user_id AS ""UserId"",
je2.content AS ""Content"" 
FROM uc.job_executions AS je2
LEFT JOIN uc.job_executions AS pj ON pj.id = je2.parent_job_id
LEFT JOIN uc.job_executions AS pje ON pje.id = je2.progress_job_execution_id
LEFT JOIN uc.users AS u ON u.id = je2.user_id
 ORDER BY je2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecution2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryJobExecutionProgress2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.JobExecutionProgress2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecutionProgress2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void JobExecutionProgress2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
jep2.id AS ""Id"",
je.id AS ""JobExecution"",
jep2.job_execution_id AS ""JobExecutionId"",
jep2.currently_succeeded AS ""CurrentlySucceeded"",
jep2.currently_failed AS ""CurrentlyFailed"",
jep2.total AS ""Total"",
jep2.content AS ""Content"" 
FROM uc.job_execution_progresses AS jep2
LEFT JOIN uc.job_executions AS je ON je.id = jep2.job_execution_id
 ORDER BY jep2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecutionProgress2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryJobExecutionSourceChunk2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.JobExecutionSourceChunk2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecutionSourceChunk2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void JobExecutionSourceChunk2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
jesc2.id AS ""Id"",
je.id AS ""JobExecution"",
jesc2.job_execution_id AS ""JobExecutionId"",
jesc2.last AS ""Last"",
jesc2.state AS ""State"",
jesc2.chunk_size AS ""ChunkSize"",
jesc2.processed_amount AS ""ProcessedAmount"",
jesc2.completed_date AS ""CompletedDate"",
jesc2.error AS ""Error"",
jesc2.content AS ""Content"" 
FROM uc.job_execution_source_chunks AS jesc2
LEFT JOIN uc.job_executions AS je ON je.id = jesc2.job_execution_id
 ORDER BY jesc2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobExecutionSourceChunk2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryJobMonitoring2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.JobMonitoring2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobMonitoring2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void JobMonitoring2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
je.id AS ""JobExecution"",
jm2.job_execution_id AS ""JobExecutionId"",
jm2.last_event_timestamp AS ""LastEventTimestamp"",
jm2.notification_sent AS ""NotificationSent"" 
FROM uc.job_monitorings AS jm2
LEFT JOIN uc.job_executions AS je ON je.id = jm2.job_execution_id
 ORDER BY jm2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JobMonitoring2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryJournalRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.JournalRecord2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JournalRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void JournalRecord2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
je.id AS ""JobExecution"",
jr2.job_execution_id AS ""JobExecutionId"",
s.name AS ""Source"",
jr2.source_id AS ""SourceId"",
jr2.entity_type AS ""EntityType"",
jr2.entity_id AS ""EntityId"",
jr2.entity_hrid AS ""EntityHrid"",
jr2.action_type AS ""ActionType"",
jr2.action_status AS ""ActionStatus"",
jr2.action_date AS ""ActionDate"",
jr2.source_record_order AS ""SourceRecordOrder"",
jr2.error AS ""Error"",
jr2.title AS ""Title"" 
FROM uc.journal_records AS jr2
LEFT JOIN uc.job_executions AS je ON je.id = jr2.job_execution_id
LEFT JOIN uc.sources AS s ON s.id = jr2.source_id
 ORDER BY jr2.title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"JournalRecord2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLanguagesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Languages(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LanguagesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LanguagesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
l.content AS ""Content"" 
FROM uc.languages AS l
LEFT JOIN uc.instances AS i ON i.id = l.instance_id
 ORDER BY l.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LanguagesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedger2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Ledger2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Ledger2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l2.id AS ""Id"",
l2.name AS ""Name"",
l2.code AS ""Code"",
l2.description AS ""Description"",
fyo.name AS ""FiscalYearOne"",
l2.fiscal_year_one_id AS ""FiscalYearOneId"",
l2.ledger_status AS ""LedgerStatus"",
l2.allocated AS ""Allocated"",
l2.available AS ""Available"",
l2.net_transfers AS ""NetTransfers"",
l2.unavailable AS ""Unavailable"",
l2.currency AS ""Currency"",
l2.restrict_encumbrance AS ""RestrictEncumbrance"",
l2.restrict_expenditures AS ""RestrictExpenditures"",
l2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
l2.created_by_user_id AS ""CreationUserId"",
l2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
l2.updated_by_user_id AS ""LastWriteUserId"",
l2.initial_allocation AS ""InitialAllocation"",
l2.allocation_to AS ""AllocationTo"",
l2.allocation_from AS ""AllocationFrom"",
l2.total_funding AS ""TotalFunding"",
l2.cash_balance AS ""CashBalance"",
l2.awaiting_payment AS ""AwaitingPayment"",
l2.encumbered AS ""Encumbered"",
l2.expenditures AS ""Expenditures"",
l2.over_encumbrance AS ""OverEncumbrance"",
l2.over_expended AS ""OverExpended"",
l2.content AS ""Content"" 
FROM uc.ledgers AS l2
LEFT JOIN uc.fiscal_years AS fyo ON fyo.id = l2.fiscal_year_one_id
LEFT JOIN uc.users AS cu ON cu.id = l2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = l2.updated_by_user_id
 ORDER BY l2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l.name AS ""Ledger"",
lau.ledger_id AS ""LedgerId"",
au.name AS ""AcquisitionsUnit"",
lau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.ledger_acquisitions_units AS lau
LEFT JOIN uc.ledgers AS l ON l.id = lau.ledger_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = lau.acquisitions_unit_id
 ORDER BY lau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerRollover2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerRollover2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRollover2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerRollover2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lr2.id AS ""Id"",
l.name AS ""Ledger"",
lr2.ledger_id AS ""LedgerId"",
ffy.name AS ""FromFiscalYear"",
lr2.from_fiscal_year_id AS ""FromFiscalYearId"",
tfy.name AS ""ToFiscalYear"",
lr2.to_fiscal_year_id AS ""ToFiscalYearId"",
lr2.restrict_encumbrance AS ""RestrictEncumbrance"",
lr2.restrict_expenditures AS ""RestrictExpenditures"",
lr2.need_close_budgets AS ""NeedCloseBudgets"",
lr2.currency_factor AS ""CurrencyFactor"",
lr2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lr2.created_by_user_id AS ""CreationUserId"",
lr2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lr2.updated_by_user_id AS ""LastWriteUserId"",
lr2.content AS ""Content"" 
FROM uc.ledger_rollovers AS lr2
LEFT JOIN uc.ledgers AS l ON l.id = lr2.ledger_id
LEFT JOIN uc.fiscal_years AS ffy ON ffy.id = lr2.from_fiscal_year_id
LEFT JOIN uc.fiscal_years AS tfy ON tfy.id = lr2.to_fiscal_year_id
LEFT JOIN uc.users AS cu ON cu.id = lr2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lr2.updated_by_user_id
 ORDER BY lr2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRollover2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerRolloverBudgetsRolloversTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerRolloverBudgetsRollovers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverBudgetsRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerRolloverBudgetsRolloversQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lr.id AS ""LedgerRollover"",
lrbr.ledger_rollover_id AS ""LedgerRolloverId"",
ft.name AS ""FundType"",
lrbr.fund_type_id AS ""FundTypeId"",
lrbr.rollover_allocation AS ""RolloverAllocation"",
lrbr.rollover_available AS ""RolloverAvailable"",
lrbr.set_allowances AS ""SetAllowances"",
lrbr.adjust_allocation AS ""AdjustAllocation"",
lrbr.add_available_to AS ""AddAvailableTo"",
lrbr.allowable_encumbrance AS ""AllowableEncumbrance"",
lrbr.allowable_expenditure AS ""AllowableExpenditure"" 
FROM uc.ledger_rollover_budgets_rollover AS lrbr
LEFT JOIN uc.ledger_rollovers AS lr ON lr.id = lrbr.ledger_rollover_id
LEFT JOIN uc.fund_types AS ft ON ft.id = lrbr.fund_type_id
 ORDER BY lrbr.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverBudgetsRolloversQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerRolloverEncumbrancesRolloversTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerRolloverEncumbrancesRollovers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverEncumbrancesRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerRolloverEncumbrancesRolloversQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lr.id AS ""LedgerRollover"",
lrer.ledger_rollover_id AS ""LedgerRolloverId"",
lrer.order_type AS ""OrderType"",
lrer.based_on AS ""BasedOn"",
lrer.increase_by AS ""IncreaseBy"" 
FROM uc.ledger_rollover_encumbrances_rollover AS lrer
LEFT JOIN uc.ledger_rollovers AS lr ON lr.id = lrer.ledger_rollover_id
 ORDER BY lrer.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverEncumbrancesRolloversQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerRolloverError2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerRolloverError2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverError2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerRolloverError2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lre2.id AS ""Id"",
lr.id AS ""LedgerRollover"",
lre2.ledger_rollover_id AS ""LedgerRolloverId"",
lre2.error_type AS ""ErrorType"",
lre2.failed_action AS ""FailedAction"",
lre2.error_message AS ""ErrorMessage"",
lre2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lre2.created_by_user_id AS ""CreationUserId"",
lre2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lre2.updated_by_user_id AS ""LastWriteUserId"",
lre2.content AS ""Content"" 
FROM uc.ledger_rollover_errors AS lre2
LEFT JOIN uc.ledger_rollovers AS lr ON lr.id = lre2.ledger_rollover_id
LEFT JOIN uc.users AS cu ON cu.id = lre2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lre2.updated_by_user_id
 ORDER BY lre2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverError2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerRolloverProgress2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LedgerRolloverProgress2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverProgress2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LedgerRolloverProgress2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lrp2.id AS ""Id"",
lr.id AS ""LedgerRollover"",
lrp2.ledger_rollover_id AS ""LedgerRolloverId"",
lrp2.overall_rollover_status AS ""OverallRolloverStatus"",
lrp2.budgets_closing_rollover_status AS ""BudgetsClosingRolloverStatus"",
lrp2.financial_rollover_status AS ""FinancialRolloverStatus"",
lrp2.orders_rollover_status AS ""OrdersRolloverStatus"",
lrp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lrp2.created_by_user_id AS ""CreationUserId"",
lrp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lrp2.updated_by_user_id AS ""LastWriteUserId"",
lrp2.content AS ""Content"" 
FROM uc.ledger_rollover_progresses AS lrp2
LEFT JOIN uc.ledger_rollovers AS lr ON lr.id = lrp2.ledger_rollover_id
LEFT JOIN uc.users AS cu ON cu.id = lrp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lrp2.updated_by_user_id
 ORDER BY lrp2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverProgress2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLibrary2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Library2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Library2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l2.id AS ""Id"",
l2.name AS ""Name"",
l2.code AS ""Code"",
c.name AS ""Campus"",
l2.campus_id AS ""CampusId"",
l2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
l2.created_by_user_id AS ""CreationUserId"",
l2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
l2.updated_by_user_id AS ""LastWriteUserId"",
l2.content AS ""Content"" 
FROM uc.libraries AS l2
LEFT JOIN uc.campuses AS c ON c.id = l2.campus_id
LEFT JOIN uc.users AS cu ON cu.id = l2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = l2.updated_by_user_id
 ORDER BY l2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoan2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Loan2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Loan2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l2.id AS ""Id"",
u.username AS ""User"",
l2.user_id AS ""UserId"",
pu.username AS ""ProxyUser"",
l2.proxy_user_id AS ""ProxyUserId"",
i.hrid AS ""Item"",
l2.item_id AS ""ItemId"",
ielaco.name AS ""ItemEffectiveLocationAtCheckOut"",
l2.item_effective_location_at_check_out_id AS ""ItemEffectiveLocationAtCheckOutId"",
l2.status_name AS ""StatusName"",
l2.loan_date AS ""LoanTime"",
l2.due_date AS ""DueTime"",
l2.return_date AS ""ReturnTime"",
l2.system_return_date AS ""SystemReturnTime"",
l2.action AS ""Action"",
l2.action_comment AS ""ActionComment"",
l2.item_status AS ""ItemStatus"",
l2.renewal_count AS ""RenewalCount"",
lp.name AS ""LoanPolicy"",
l2.loan_policy_id AS ""LoanPolicyId"",
csp.name AS ""CheckoutServicePoint"",
l2.checkout_service_point_id AS ""CheckoutServicePointId"",
csp2.name AS ""CheckinServicePoint"",
l2.checkin_service_point_id AS ""CheckinServicePointId"",
g.group AS ""Group"",
l2.group_id AS ""GroupId"",
l2.due_date_changed_by_recall AS ""DueDateChangedByRecall"",
l2.declared_lost_date AS ""DeclaredLostDate"",
l2.claimed_returned_date AS ""ClaimedReturnedDate"",
ofp.name AS ""OverdueFinePolicy"",
l2.overdue_fine_policy_id AS ""OverdueFinePolicyId"",
lip.name AS ""LostItemPolicy"",
l2.lost_item_policy_id AS ""LostItemPolicyId"",
l2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
l2.created_by_user_id AS ""CreationUserId"",
l2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
l2.updated_by_user_id AS ""LastWriteUserId"",
l2.aged_to_lost_delayed_billing_lost_item_has_been_billed AS ""AgedToLostDelayedBillingLostItemHasBeenBilled"",
l2.aged_to_lost_delayed_billing_date_lost_item_should_be_billed AS ""AgedToLostDelayedBillingDateLostItemShouldBeBilled"",
l2.aged_to_lost_delayed_billing_aged_to_lost_date AS ""AgedToLostDelayedBillingAgedToLostDate"",
l2.content AS ""Content"" 
FROM uc.loans AS l2
LEFT JOIN uc.users AS u ON u.id = l2.user_id
LEFT JOIN uc.users AS pu ON pu.id = l2.proxy_user_id
LEFT JOIN uc.items AS i ON i.id = l2.item_id
LEFT JOIN uc.locations AS ielaco ON ielaco.id = l2.item_effective_location_at_check_out_id
LEFT JOIN uc.loan_policies AS lp ON lp.id = l2.loan_policy_id
LEFT JOIN uc.service_points AS csp ON csp.id = l2.checkout_service_point_id
LEFT JOIN uc.service_points AS csp2 ON csp2.id = l2.checkin_service_point_id
LEFT JOIN uc.groups AS g ON g.id = l2.group_id
LEFT JOIN uc.overdue_fine_policies AS ofp ON ofp.id = l2.overdue_fine_policy_id
LEFT JOIN uc.lost_item_fee_policies AS lip ON lip.id = l2.lost_item_policy_id
LEFT JOIN uc.users AS cu ON cu.id = l2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = l2.updated_by_user_id
 ORDER BY l2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanEvent2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LoanEvent2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanEvent2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LoanEvent2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
le2.id AS ""Id"",
le2.operation AS ""Operation"",
le2.creation_date AS ""CreationTime"",
le2.loan_user_id AS ""LoanUserId"",
le2.loan_proxy_user_id AS ""LoanProxyUserId"",
le2.loan_item_id AS ""LoanItemId"",
le2.loan_item_effective_location_id_at_check_out_id AS ""LoanItemEffectiveLocationIdAtCheckOutId"",
le2.loan_status_name AS ""LoanStatusName"",
le2.loan_loan_date AS ""LoanLoanDate"",
le2.loan_due_date AS ""LoanDueDate"",
le2.loan_return_date AS ""LoanReturnDate"",
le2.loan_system_return_date AS ""LoanSystemReturnDate"",
le2.loan_action AS ""LoanAction"",
le2.loan_action_comment AS ""LoanActionComment"",
le2.loan_item_status AS ""LoanItemStatus"",
le2.loan_renewal_count AS ""LoanRenewalCount"",
le2.loan_loan_policy_id AS ""LoanLoanPolicyId"",
le2.loan_checkout_service_point_id AS ""LoanCheckoutServicePointId"",
le2.loan_checkin_service_point_id AS ""LoanCheckinServicePointId"",
le2.loan_patron_group_id_at_checkout AS ""LoanPatronGroupIdAtCheckout"",
le2.loan_due_date_changed_by_recall AS ""LoanDueDateChangedByRecall"",
le2.loan_declared_lost_date AS ""LoanDeclaredLostDate"",
le2.loan_claimed_returned_date AS ""LoanClaimedReturnedDate"",
le2.loan_overdue_fine_policy_id AS ""LoanOverdueFinePolicyId"",
le2.loan_lost_item_policy_id AS ""LoanLostItemPolicyId"",
le2.loan_metadata_created_date AS ""LoanMetadataCreatedDate"",
le2.loan_metadata_created_by_user_id AS ""LoanMetadataCreatedByUserId"",
le2.loan_metadata_created_by_username AS ""LoanMetadataCreatedByUsername"",
le2.loan_metadata_updated_date AS ""LoanMetadataUpdatedDate"",
le2.loan_metadata_updated_by_user_id AS ""LoanMetadataUpdatedByUserId"",
le2.loan_metadata_updated_by_username AS ""LoanMetadataUpdatedByUsername"",
le2.loan_aged_to_lost_delayed_billing_lost_item_has_been_billed AS ""LoanAgedToLostDelayedBillingLostItemHasBeenBilled"",
le2.loan_aged_to_lost_delayed_billing_date_lost_item_should_be_bill AS ""LoanAgedToLostDelayedBillingDateLostItemShouldBeBill"",
le2.loan_aged_to_lost_delayed_billing_aged_to_lost_date AS ""LoanAgedToLostDelayedBillingAgedToLostDate"",
le2.content AS ""Content"" 
FROM uc.loan_events AS le2
 ORDER BY le2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanEvent2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LoanPolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LoanPolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lp2.id AS ""Id"",
lp2.name AS ""Name"",
lp2.description AS ""Description"",
lp2.loanable AS ""Loanable"",
lp2.loans_policy_profile_id AS ""LoansPolicyProfileId"",
lp2.loans_policy_period_duration AS ""LoansPolicyPeriodDuration"",
lp2.loans_policy_period_interval_id AS ""LoansPolicyPeriodInterval"",
lp2.loans_policy_closed_library_due_date_management_id AS ""LoansPolicyClosedLibraryDueDateManagementId"",
lp2.loans_policy_grace_period_duration AS ""LoansPolicyGracePeriodDuration"",
lp2.loans_policy_grace_period_interval_id AS ""LoansPolicyGracePeriodInterval"",
lp2.loans_policy_opening_time_offset_duration AS ""LoansPolicyOpeningTimeOffsetDuration"",
lp2.loans_policy_opening_time_offset_interval_id AS ""LoansPolicyOpeningTimeOffsetInterval"",
lpfdds.name AS ""LoansPolicyFixedDueDateSchedule"",
lp2.loans_policy_fixed_due_date_schedule_id AS ""LoansPolicyFixedDueDateScheduleId"",
lp2.loans_policy_item_limit AS ""LoansPolicyItemLimit"",
lp2.renewable AS ""Renewable"",
lp2.renewals_policy_unlimited AS ""RenewalsPolicyUnlimited"",
lp2.renewals_policy_number_allowed AS ""RenewalsPolicyNumberAllowed"",
lp2.renewals_policy_renew_from_id AS ""RenewalsPolicyRenewFromId"",
lp2.renewals_policy_different_period AS ""RenewalsPolicyDifferentPeriod"",
lp2.renewals_policy_period_duration AS ""RenewalsPolicyPeriodDuration"",
lp2.renewals_policy_period_interval_id AS ""RenewalsPolicyPeriodInterval"",
rpafdds.name AS ""RenewalsPolicyAlternateFixedDueDateSchedule"",
lp2.renewals_policy_alternate_fixed_due_date_schedule_id AS ""RenewalsPolicyAlternateFixedDueDateScheduleId"",
lp2.recalls_alternate_grace_period_duration AS ""RecallsAlternateGracePeriodDuration"",
lp2.recalls_alternate_grace_period_interval_id AS ""RecallsAlternateGracePeriodInterval"",
lp2.recalls_minimum_guaranteed_loan_period_duration AS ""RecallsMinimumGuaranteedLoanPeriodDuration"",
lp2.recalls_minimum_guaranteed_loan_period_interval_id AS ""RecallsMinimumGuaranteedLoanPeriodInterval"",
lp2.recalls_recall_return_interval_duration AS ""RecallsRecallReturnIntervalDuration"",
lp2.recalls_recall_return_interval_interval_id AS ""RecallsRecallReturnIntervalInterval"",
lp2.recalls_allow_recalls_to_extend_overdue_loans AS ""RecallsAllowRecallsToExtendOverdueLoans"",
lp2.recalls_alternate_recall_return_interval_duration AS ""RecallsAlternateRecallReturnIntervalDuration"",
lp2.recalls_alternate_recall_return_interval_interval_id AS ""RecallsAlternateRecallReturnIntervalInterval"",
lp2.holds_alternate_checkout_loan_period_duration AS ""HoldsAlternateCheckoutLoanPeriodDuration"",
lp2.holds_alternate_checkout_loan_period_interval_id AS ""HoldsAlternateCheckoutLoanPeriodInterval"",
lp2.holds_renew_items_with_request AS ""HoldsRenewItemsWithRequest"",
lp2.holds_alternate_renewal_loan_period_duration AS ""HoldsAlternateRenewalLoanPeriodDuration"",
lp2.holds_alternate_renewal_loan_period_interval_id AS ""HoldsAlternateRenewalLoanPeriodInterval"",
lp2.pages_alternate_checkout_loan_period_duration AS ""PagesAlternateCheckoutLoanPeriodDuration"",
lp2.pages_alternate_checkout_loan_period_interval_id AS ""PagesAlternateCheckoutLoanPeriodInterval"",
lp2.pages_renew_items_with_request AS ""PagesRenewItemsWithRequest"",
lp2.pages_alternate_renewal_loan_period_duration AS ""PagesAlternateRenewalLoanPeriodDuration"",
lp2.pages_alternate_renewal_loan_period_interval_id AS ""PagesAlternateRenewalLoanPeriodInterval"",
lp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lp2.created_by_user_id AS ""CreationUserId"",
lp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lp2.updated_by_user_id AS ""LastWriteUserId"",
lp2.content AS ""Content"" 
FROM uc.loan_policies AS lp2
LEFT JOIN uc.fixed_due_date_schedules AS lpfdds ON lpfdds.id = lp2.loans_policy_fixed_due_date_schedule_id
LEFT JOIN uc.fixed_due_date_schedules AS rpafdds ON rpafdds.id = lp2.renewals_policy_alternate_fixed_due_date_schedule_id
LEFT JOIN uc.users AS cu ON cu.id = lp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lp2.updated_by_user_id
 ORDER BY lp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LoanType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LoanType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lt2.id AS ""Id"",
lt2.name AS ""Name"",
lt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lt2.created_by_user_id AS ""CreationUserId"",
lt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lt2.updated_by_user_id AS ""LastWriteUserId"",
lt2.content AS ""Content"" 
FROM uc.loan_types AS lt2
LEFT JOIN uc.users AS cu ON cu.id = lt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lt2.updated_by_user_id
 ORDER BY lt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Location2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Location2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l2.id AS ""Id"",
l2.name AS ""Name"",
l2.code AS ""Code"",
l2.description AS ""Description"",
l2.discovery_display_name AS ""DiscoveryDisplayName"",
l2.is_active AS ""IsActive"",
i.name AS ""Institution"",
l2.institution_id AS ""InstitutionId"",
c.name AS ""Campus"",
l2.campus_id AS ""CampusId"",
l.name AS ""Library"",
l2.library_id AS ""LibraryId"",
psp.name AS ""PrimaryServicePoint"",
l2.primary_service_point_id AS ""PrimaryServicePointId"",
l2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
l2.created_by_user_id AS ""CreationUserId"",
l2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
l2.updated_by_user_id AS ""LastWriteUserId"",
l2.content AS ""Content"" 
FROM uc.locations AS l2
LEFT JOIN uc.institutions AS i ON i.id = l2.institution_id
LEFT JOIN uc.campuses AS c ON c.id = l2.campus_id
LEFT JOIN uc.libraries AS l ON l.id = l2.library_id
LEFT JOIN uc.service_points AS psp ON psp.id = l2.primary_service_point_id
LEFT JOIN uc.users AS cu ON cu.id = l2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = l2.updated_by_user_id
 ORDER BY l2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocationServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LocationServicePoints(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LocationServicePointsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l.name AS ""Location"",
lsp.location_id AS ""LocationId"",
sp.name AS ""ServicePoint"",
lsp.service_point_id AS ""ServicePointId"" 
FROM uc.location_service_points AS lsp
LEFT JOIN uc.locations AS l ON l.id = lsp.location_id
LEFT JOIN uc.service_points AS sp ON sp.id = lsp.service_point_id
 ORDER BY lsp.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationServicePointsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocationSettingsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LocationSettings(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LocationSettingsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l.name AS ""Location"",
ls.location_id AS ""LocationId"",
s.name AS ""Settings"",
ls.settings_id AS ""SettingsId"",
ls.enabled AS ""Enabled"",
ls.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ls.created_by_user_id AS ""CreationUserId"",
ls.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ls.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.location_settings AS ls
LEFT JOIN uc.locations AS l ON l.id = ls.location_id
LEFT JOIN uc.settings AS s ON s.id = ls.settings_id
LEFT JOIN uc.users AS cu ON cu.id = ls.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ls.updated_by_user_id
 ORDER BY ls.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLogin2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Login2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Login2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Login2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
l2.id AS ""Id"",
u.username AS ""User"",
l2.user_id AS ""UserId"",
l2.hash AS ""Hash"",
l2.salt AS ""Salt"",
l2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
l2.created_by_user_id AS ""CreationUserId"",
l2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
l2.updated_by_user_id AS ""LastWriteUserId"",
l2.content AS ""Content"" 
FROM uc.logins AS l2
LEFT JOIN uc.users AS u ON u.id = l2.user_id
LEFT JOIN uc.users AS cu ON cu.id = l2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = l2.updated_by_user_id
 ORDER BY l2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Login2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLostItemFeePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.LostItemFeePolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void LostItemFeePolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
lifp2.id AS ""Id"",
lifp2.name AS ""Name"",
lifp2.description AS ""Description"",
lifp2.item_aged_lost_overdue_duration AS ""ItemAgedLostOverdueDuration"",
lifp2.item_aged_lost_overdue_interval_id AS ""ItemAgedLostOverdueInterval"",
lifp2.patron_billed_after_aged_lost_duration AS ""PatronBilledAfterAgedLostDuration"",
lifp2.patron_billed_after_aged_lost_interval_id AS ""PatronBilledAfterAgedLostInterval"",
lifp2.recalled_item_aged_lost_overdue_duration AS ""RecalledItemAgedLostOverdueDuration"",
lifp2.recalled_item_aged_lost_overdue_interval_id AS ""RecalledItemAgedLostOverdueInterval"",
lifp2.patron_billed_after_recalled_item_aged_lost_duration AS ""PatronBilledAfterRecalledItemAgedLostDuration"",
lifp2.patron_billed_after_recalled_item_aged_lost_interval_id AS ""PatronBilledAfterRecalledItemAgedLostInterval"",
lifp2.charge_amount_item_charge_type AS ""ChargeAmountItemChargeType"",
lifp2.charge_amount_item_amount AS ""ChargeAmountItemAmount"",
lifp2.lost_item_processing_fee AS ""LostItemProcessingFee"",
lifp2.charge_amount_item_patron AS ""ChargeAmountItemPatron"",
lifp2.charge_amount_item_system AS ""ChargeAmountItemSystem"",
lifp2.lost_item_charge_fee_fine_duration AS ""LostItemChargeFeeFineDuration"",
lifp2.lost_item_charge_fee_fine_interval_id AS ""LostItemChargeFeeFineInterval"",
lifp2.returned_lost_item_processing_fee AS ""ReturnedLostItemProcessingFee"",
lifp2.replaced_lost_item_processing_fee AS ""ReplacedLostItemProcessingFee"",
lifp2.replacement_processing_fee AS ""ReplacementProcessingFee"",
lifp2.replacement_allowed AS ""ReplacementAllowed"",
lifp2.lost_item_returned AS ""LostItemReturned"",
lifp2.fees_fines_shall_refunded_duration AS ""FeesFinesShallRefundedDuration"",
lifp2.fees_fines_shall_refunded_interval_id AS ""FeesFinesShallRefundedInterval"",
lifp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
lifp2.created_by_user_id AS ""CreationUserId"",
lifp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
lifp2.updated_by_user_id AS ""LastWriteUserId"",
lifp2.content AS ""Content"" 
FROM uc.lost_item_fee_policies AS lifp2
LEFT JOIN uc.users AS cu ON cu.id = lifp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = lifp2.updated_by_user_id
 ORDER BY lifp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryManualBlockTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ManualBlockTemplate2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ManualBlockTemplate2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
mbt2.id AS ""Id"",
mbt2.name AS ""Name"",
mbt2.code AS ""Code"",
mbt2.desc AS ""Description"",
mbt2.block_template_desc AS ""BlockTemplateDescription"",
mbt2.block_template_patron_message AS ""BlockTemplatePatronMessage"",
mbt2.block_template_borrowing AS ""BlockTemplateBorrowing"",
mbt2.block_template_renewals AS ""BlockTemplateRenewals"",
mbt2.block_template_requests AS ""BlockTemplateRequests"",
mbt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
mbt2.created_by_user_id AS ""CreationUserId"",
mbt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
mbt2.updated_by_user_id AS ""LastWriteUserId"",
mbt2.content AS ""Content"" 
FROM uc.manual_block_templates AS mbt2
LEFT JOIN uc.users AS cu ON cu.id = mbt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = mbt2.updated_by_user_id
 ORDER BY mbt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplate2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryMarcRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.MarcRecord2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MarcRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void MarcRecord2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
mr2.id AS ""Id"",
r2.id AS ""Record2"",
mr2.content AS ""Content"" 
FROM uc.marc_records AS mr2
LEFT JOIN uc.records AS r2 ON r2.id = mr2.id
 ORDER BY mr2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MarcRecord2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryMaterialType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.MaterialType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void MaterialType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
mt2.id AS ""Id"",
mt2.name AS ""Name"",
mt2.source AS ""Source"",
mt2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
mt2.created_by_user_id AS ""CreationUserId"",
mt2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
mt2.updated_by_user_id AS ""LastWriteUserId"",
mt2.content AS ""Content"" 
FROM uc.material_types AS mt2
LEFT JOIN uc.users AS cu ON cu.id = mt2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = mt2.updated_by_user_id
 ORDER BY mt2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNatureOfContentTerm2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.NatureOfContentTerm2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void NatureOfContentTerm2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
noct2.id AS ""Id"",
noct2.name AS ""Name"",
noct2.source AS ""Source"",
noct2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
noct2.created_by_user_id AS ""CreationUserId"",
noct2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
noct2.updated_by_user_id AS ""LastWriteUserId"",
noct2.content AS ""Content"" 
FROM uc.nature_of_content_terms AS noct2
LEFT JOIN uc.users AS cu ON cu.id = noct2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = noct2.updated_by_user_id
 ORDER BY noct2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNote2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Note2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Note2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Note2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
@int.name AS ""InstanceNoteType"",
n2.instance_note_type_id AS ""InstanceNoteTypeId"",
n2.note AS ""Note"",
n2.staff_only AS ""StaffOnly"" 
FROM uc.notes AS n2
LEFT JOIN uc.instances AS i ON i.id = n2.instance_id
LEFT JOIN uc.instance_note_types AS @int ON @int.id = n2.instance_note_type_id
 ORDER BY n2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Note2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrder2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Order2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Order2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o2.id AS ""Id"",
o2.approved AS ""Approved"",
ab.username AS ""ApprovedBy"",
o2.approved_by_id AS ""ApprovedById"",
o2.approval_date AS ""ApprovalDate"",
at.username AS ""AssignedTo"",
o2.assigned_to_id AS ""AssignedToId"",
bt.name AS ""BillTo"",
o2.bill_to_id AS ""BillToId"",
o2.close_reason_reason AS ""CloseReasonReason"",
o2.close_reason_note AS ""CloseReasonNote"",
o2.date_ordered AS ""OrderDate"",
o2.manual_po AS ""Manual"",
o2.po_number AS ""Number"",
o2.order_type AS ""OrderType"",
o2.re_encumber AS ""Reencumber"",
o2.ongoing_interval AS ""OngoingInterval"",
o2.ongoing_is_subscription AS ""OngoingIsSubscription"",
o2.ongoing_manual_renewal AS ""OngoingManualRenewal"",
o2.ongoing_notes AS ""OngoingNotes"",
o2.ongoing_review_period AS ""OngoingReviewPeriod"",
o2.ongoing_renewal_date AS ""OngoingRenewalDate"",
o2.ongoing_review_date AS ""OngoingReviewDate"",
st.name AS ""ShipTo"",
o2.ship_to_id AS ""ShipToId"",
t.id AS ""Template"",
o2.template_id AS ""TemplateId"",
v.name AS ""Vendor"",
o2.vendor_id AS ""VendorId"",
o2.status AS ""Status"",
o2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
o2.created_by_user_id AS ""CreationUserId"",
o2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
o2.updated_by_user_id AS ""LastWriteUserId"",
o2.content AS ""Content"",
ots2.id AS ""OrderTransactionSummary2"" 
FROM uc.orders AS o2
LEFT JOIN uc.users AS ab ON ab.id = o2.approved_by_id
LEFT JOIN uc.users AS at ON at.id = o2.assigned_to_id
LEFT JOIN uc.addresses AS bt ON bt.id = o2.bill_to_id
LEFT JOIN uc.addresses AS st ON st.id = o2.ship_to_id
LEFT JOIN uc.order_templates AS t ON t.id = o2.template_id
LEFT JOIN uc.organizations AS v ON v.id = o2.vendor_id
LEFT JOIN uc.users AS cu ON cu.id = o2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = o2.updated_by_user_id
LEFT JOIN uc.order_transaction_summaries AS ots2 ON ots2.id = o2.id
 ORDER BY o2.po_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.po_number AS ""Order"",
oau.order_id AS ""OrderId"",
au.name AS ""AcquisitionsUnit"",
oau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.order_acquisitions_units AS oau
LEFT JOIN uc.orders AS o ON o.id = oau.order_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = oau.acquisitions_unit_id
 ORDER BY oau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderInvoice2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderInvoice2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi2.id AS ""Id"",
o.po_number AS ""Order"",
oi2.order_id AS ""OrderId"",
i.folio_invoice_no AS ""Invoice"",
oi2.invoice_id AS ""InvoiceId"",
oi2.content AS ""Content"" 
FROM uc.order_invoices AS oi2
LEFT JOIN uc.orders AS o ON o.id = oi2.order_id
LEFT JOIN uc.invoices AS i ON i.id = oi2.invoice_id
 ORDER BY oi2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItem2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItem2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItem2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi2.id AS ""Id"",
oi2.edition AS ""Edition"",
oi2.checkin_items AS ""CheckinItems"",
oi2.agreement_id AS ""AgreementId"",
oi2.acquisition_method AS ""AcquisitionMethod"",
oi2.cancellation_restriction AS ""CancellationRestriction"",
oi2.cancellation_restriction_note AS ""CancellationRestrictionNote"",
oi2.collection AS ""Collection"",
oi2.cost_list_unit_price AS ""PhysicalUnitListPrice"",
oi2.cost_list_unit_price_electronic AS ""ElectronicUnitListPrice"",
oi2.cost_currency AS ""Currency"",
oi2.cost_additional_cost AS ""AdditionalCost"",
oi2.cost_discount AS ""Discount"",
oi2.cost_discount_type AS ""DiscountType"",
oi2.cost_exchange_rate AS ""ExchangeRate"",
oi2.cost_quantity_physical AS ""PhysicalQuantity"",
oi2.cost_quantity_electronic AS ""ElectronicQuantity"",
oi2.cost_po_line_estimated_price AS ""EstimatedPrice"",
oi2.cost_fyro_adjustment_amount AS ""FiscalYearRolloverAdjustmentAmount"",
oi2.description AS ""InternalNote"",
oi2.details_receiving_note AS ""ReceivingNote"",
oi2.details_subscription_from AS ""SubscriptionFrom"",
oi2.details_subscription_interval AS ""SubscriptionInterval"",
oi2.details_subscription_to AS ""SubscriptionTo"",
oi2.donor AS ""Donor"",
oi2.eresource_activated AS ""EresourceActivated"",
oi2.eresource_activation_due AS ""EresourceActivationDue"",
oi2.eresource_create_inventory AS ""EresourceCreateInventory"",
oi2.eresource_trial AS ""EresourceTrial"",
oi2.eresource_expected_activation AS ""EresourceExpectedActivationDate"",
oi2.eresource_user_limit AS ""EresourceUserLimit"",
eap.name AS ""EresourceAccessProvider"",
oi2.eresource_access_provider_id AS ""EresourceAccessProviderId"",
oi2.eresource_license_code AS ""EresourceLicenseCode"",
oi2.eresource_license_description AS ""EresourceLicenseDescription"",
oi2.eresource_license_reference AS ""EresourceLicenseReference"",
emt.name AS ""EresourceMaterialType"",
oi2.eresource_material_type_id AS ""EresourceMaterialTypeId"",
oi2.eresource_resource_url AS ""EresourceResourceUrl"",
i.title AS ""Instance"",
oi2.instance_id AS ""InstanceId"",
oi2.is_package AS ""IsPackage"",
oi2.order_format AS ""OrderFormat"",
poi.po_line_number AS ""PackageOrderItem"",
oi2.package_po_line_id AS ""PackageOrderItemId"",
oi2.payment_status AS ""PaymentStatus"",
oi2.physical_create_inventory AS ""PhysicalCreateInventory"",
pmt.name AS ""PhysicalMaterialType"",
oi2.physical_material_type_id AS ""PhysicalMaterialTypeId"",
pms.name AS ""PhysicalMaterialSupplier"",
oi2.physical_material_supplier_id AS ""PhysicalMaterialSupplierId"",
oi2.physical_expected_receipt_date AS ""PhysicalExpectedReceiptDate"",
oi2.physical_receipt_due AS ""PhysicalReceiptDue"",
oi2.po_line_description AS ""Description"",
oi2.po_line_number AS ""Number"",
oi2.publication_year AS ""PublicationYear"",
oi2.publisher AS ""Publisher"",
o.po_number AS ""Order"",
oi2.order_id AS ""OrderId"",
oi2.receipt_date AS ""ReceiptDate"",
oi2.receipt_status AS ""ReceiptStatus"",
oi2.requester AS ""Requester"",
oi2.rush AS ""Rush"",
oi2.selector AS ""Selector"",
oi2.source AS ""Source"",
oi2.title_or_package AS ""TitleOrPackage"",
oi2.vendor_detail_instructions AS ""VendorInstructions"",
oi2.vendor_detail_note_from_vendor AS ""VendorNote"",
oi2.vendor_detail_vendor_account AS ""VendorCustomerId"",
oi2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
oi2.created_by_user_id AS ""CreationUserId"",
oi2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
oi2.updated_by_user_id AS ""LastWriteUserId"",
oi2.content AS ""Content"" 
FROM uc.order_items AS oi2
LEFT JOIN uc.organizations AS eap ON eap.id = oi2.eresource_access_provider_id
LEFT JOIN uc.material_types AS emt ON emt.id = oi2.eresource_material_type_id
LEFT JOIN uc.instances AS i ON i.id = oi2.instance_id
LEFT JOIN uc.order_items AS poi ON poi.id = oi2.package_po_line_id
LEFT JOIN uc.material_types AS pmt ON pmt.id = oi2.physical_material_type_id
LEFT JOIN uc.organizations AS pms ON pms.id = oi2.physical_material_supplier_id
LEFT JOIN uc.orders AS o ON o.id = oi2.order_id
LEFT JOIN uc.users AS cu ON cu.id = oi2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = oi2.updated_by_user_id
 ORDER BY oi2.po_line_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemAlertsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemAlerts(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemAlertsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemAlertsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oia.order_item_id AS ""OrderItemId"",
a.id AS ""Alert"",
oia.alert_id AS ""AlertId"" 
FROM uc.order_item_alerts AS oia
LEFT JOIN uc.order_items AS oi ON oi.id = oia.order_item_id
LEFT JOIN uc.alerts AS a ON a.id = oia.alert_id
 ORDER BY oia.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemAlertsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemClaimsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemClaims(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemClaimsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemClaimsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oic.order_item_id AS ""OrderItemId"",
oic.claimed AS ""Claimed"",
oic.sent AS ""Sent"",
oic.grace AS ""Grace"" 
FROM uc.order_item_claims AS oic
LEFT JOIN uc.order_items AS oi ON oi.id = oic.order_item_id
 ORDER BY oic.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemClaimsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemContributorsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemContributors(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemContributorsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oic.order_item_id AS ""OrderItemId"",
oic.contributor AS ""Contributor"",
cnt.name AS ""ContributorNameType"",
oic.contributor_name_type_id AS ""ContributorNameTypeId"" 
FROM uc.order_item_contributors AS oic
LEFT JOIN uc.order_items AS oi ON oi.id = oic.order_item_id
LEFT JOIN uc.contributor_name_types AS cnt ON cnt.id = oic.contributor_name_type_id
 ORDER BY oic.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemContributorsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oif.order_item_id AS ""OrderItemId"",
oif.code AS ""FundCode"",
e2.amount AS ""Encumbrance"",
oif.encumbrance_id AS ""EncumbranceId"",
f.name AS ""Fund"",
oif.fund_id AS ""FundId"",
ec.name AS ""ExpenseClass"",
oif.expense_class_id AS ""ExpenseClassId"",
oif.distribution_type AS ""DistributionType"",
oif.value AS ""Value"" 
FROM uc.order_item_fund_distributions AS oif
LEFT JOIN uc.order_items AS oi ON oi.id = oif.order_item_id
LEFT JOIN uc.transactions AS e2 ON e2.id = oif.encumbrance_id
LEFT JOIN uc.funds AS f ON f.id = oif.fund_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = oif.expense_class_id
 ORDER BY oif.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemLocation2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemLocation2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oil2.order_item_id AS ""OrderItemId"",
l.name AS ""Location"",
oil2.location_id AS ""LocationId"",
h.hrid AS ""Holding"",
oil2.holding_id AS ""HoldingId"",
oil2.quantity AS ""Quantity"",
oil2.quantity_electronic AS ""ElectronicQuantity"",
oil2.quantity_physical AS ""PhysicalQuantity"" 
FROM uc.order_item_locations AS oil2
LEFT JOIN uc.order_items AS oi ON oi.id = oil2.order_item_id
LEFT JOIN uc.locations AS l ON l.id = oil2.location_id
LEFT JOIN uc.holdings AS h ON h.id = oil2.holding_id
 ORDER BY oil2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemLocation2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemProductIds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemProductIdsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oipi.order_item_id AS ""OrderItemId"",
oipi.product_id AS ""ProductId"",
pit.name AS ""ProductIdType"",
oipi.product_id_type_id AS ""ProductIdTypeId"",
oipi.qualifier AS ""Qualifier"" 
FROM uc.order_item_product_ids AS oipi
LEFT JOIN uc.order_items AS oi ON oi.id = oipi.order_item_id
LEFT JOIN uc.id_types AS pit ON pit.id = oipi.product_id_type_id
 ORDER BY oipi.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemProductIdsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemReferenceNumbers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemReferenceNumbersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oirn.order_item_id AS ""OrderItemId"",
oirn.ref_number AS ""ReferenceNumber"",
oirn.ref_number_type AS ""ReferenceNumberType"",
oirn.vendor_details_source AS ""VendorDetailsSource"" 
FROM uc.order_item_reference_numbers AS oirn
LEFT JOIN uc.order_items AS oi ON oi.id = oirn.order_item_id
 ORDER BY oirn.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReferenceNumbersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemReportingCodesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemReportingCodes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReportingCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemReportingCodesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oirc.order_item_id AS ""OrderItemId"",
rc.id AS ""ReportingCode"",
oirc.reporting_code_id AS ""ReportingCodeId"" 
FROM uc.order_item_reporting_codes AS oirc
LEFT JOIN uc.order_items AS oi ON oi.id = oirc.order_item_id
LEFT JOIN uc.reporting_codes AS rc ON rc.id = oirc.reporting_code_id
 ORDER BY oirc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReportingCodesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oit.order_item_id AS ""OrderItemId"",
oit.content AS ""Content"" 
FROM uc.order_item_tags AS oit
LEFT JOIN uc.order_items AS oi ON oi.id = oit.order_item_id
 ORDER BY oit.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemVolumesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderItemVolumes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemVolumesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderItemVolumesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oi.po_line_number AS ""OrderItem"",
oiv.order_item_id AS ""OrderItemId"",
oiv.content AS ""Content"" 
FROM uc.order_item_volumes AS oiv
LEFT JOIN uc.order_items AS oi ON oi.id = oiv.order_item_id
 ORDER BY oiv.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemVolumesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderNotesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderNotes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderNotesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.po_number AS ""Order"",
@on.order_id AS ""OrderId"",
@on.content AS ""Content"" 
FROM uc.order_notes AS @on
LEFT JOIN uc.orders AS o ON o.id = @on.order_id
 ORDER BY @on.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderNotesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.po_number AS ""Order"",
ot.order_id AS ""OrderId"",
ot.content AS ""Content"" 
FROM uc.order_tags AS ot
LEFT JOIN uc.orders AS o ON o.id = ot.order_id
 ORDER BY ot.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderTemplate2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderTemplate2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ot2.id AS ""Id"",
ot2.template_name AS ""TemplateName"",
ot2.template_code AS ""TemplateCode"",
ot2.template_description AS ""TemplateDescription"",
ot2.content AS ""Content"" 
FROM uc.order_templates AS ot2
 ORDER BY ot2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplate2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTransactionSummary2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrderTransactionSummary2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTransactionSummary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrderTransactionSummary2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ots2.id AS ""Id"",
o2.po_number AS ""Order2"",
ots2.num_transactions AS ""NumTransactions"",
ots2.content AS ""Content"" 
FROM uc.order_transaction_summaries AS ots2
LEFT JOIN uc.orders AS o2 ON o2.id = ots2.id
 ORDER BY ots2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTransactionSummary2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Organization2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organization2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o2.id AS ""Id"",
o2.name AS ""Name"",
o2.code AS ""Code"",
o2.description AS ""Description"",
o2.export_to_accounting AS ""ExportToAccounting"",
o2.status AS ""Status"",
o2.language AS ""Language"",
o2.erp_code AS ""AccountingCode"",
o2.payment_method AS ""PaymentMethod"",
o2.access_provider AS ""AccessProvider"",
o2.governmental AS ""Governmental"",
o2.licensor AS ""Licensor"",
o2.material_supplier AS ""MaterialSupplier"",
o2.claiming_interval AS ""ClaimingInterval"",
o2.discount_percent AS ""DiscountPercent"",
o2.expected_activation_interval AS ""ExpectedActivationInterval"",
o2.expected_invoice_interval AS ""ExpectedInvoiceInterval"",
o2.renewal_activation_interval AS ""RenewalActivationInterval"",
o2.subscription_interval AS ""SubscriptionInterval"",
o2.expected_receipt_interval AS ""ExpectedReceiptInterval"",
o2.tax_id AS ""TaxId"",
o2.liable_for_vat AS ""LiableForVat"",
o2.tax_percentage AS ""TaxPercentage"",
o2.edi_vendor_edi_code AS ""EdiVendorEdiCode"",
o2.edi_vendor_edi_type AS ""EdiVendorEdiType"",
o2.edi_lib_edi_code AS ""EdiLibEdiCode"",
o2.edi_lib_edi_type AS ""EdiLibEdiType"",
o2.edi_prorate_tax AS ""EdiProrateTax"",
o2.edi_prorate_fees AS ""EdiProrateFees"",
o2.edi_naming_convention AS ""EdiNamingConvention"",
o2.edi_send_acct_num AS ""EdiSendAcctNum"",
o2.edi_support_order AS ""EdiSupportOrder"",
o2.edi_support_invoice AS ""EdiSupportInvoice"",
o2.edi_notes AS ""EdiNotes"",
o2.edi_ftp_ftp_format AS ""EdiFtpFtpFormat"",
o2.edi_ftp_server_address AS ""EdiFtpServerAddress"",
o2.edi_ftp_username AS ""EdiFtpUsername"",
o2.edi_ftp_password AS ""EdiFtpPassword"",
o2.edi_ftp_ftp_mode AS ""EdiFtpFtpMode"",
o2.edi_ftp_ftp_conn_mode AS ""EdiFtpFtpConnMode"",
o2.edi_ftp_ftp_port AS ""EdiFtpFtpPort"",
o2.edi_ftp_order_directory AS ""EdiFtpOrderDirectory"",
o2.edi_ftp_invoice_directory AS ""EdiFtpInvoiceDirectory"",
o2.edi_ftp_notes AS ""EdiFtpNotes"",
o2.edi_job_schedule_edi AS ""EdiJobScheduleEdi"",
o2.edi_job_scheduling_date AS ""EdiJobSchedulingDate"",
o2.edi_job_time AS ""EdiJobTime"",
o2.edi_job_is_monday AS ""EdiJobIsMonday"",
o2.edi_job_is_tuesday AS ""EdiJobIsTuesday"",
o2.edi_job_is_wednesday AS ""EdiJobIsWednesday"",
o2.edi_job_is_thursday AS ""EdiJobIsThursday"",
o2.edi_job_is_friday AS ""EdiJobIsFriday"",
o2.edi_job_is_saturday AS ""EdiJobIsSaturday"",
o2.edi_job_is_sunday AS ""EdiJobIsSunday"",
o2.edi_job_send_to_emails AS ""EdiJobSendToEmails"",
o2.edi_job_notify_all_edi AS ""EdiJobNotifyAllEdi"",
o2.edi_job_notify_invoice_only AS ""EdiJobNotifyInvoiceOnly"",
o2.edi_job_notify_error_only AS ""EdiJobNotifyErrorOnly"",
o2.edi_job_scheduling_notes AS ""EdiJobSchedulingNotes"",
o2.is_vendor AS ""IsVendor"",
o2.san_code AS ""SanCode"",
o2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
o2.created_by_user_id AS ""CreationUserId"",
o2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
o2.updated_by_user_id AS ""LastWriteUserId"",
o2.content AS ""Content"" 
FROM uc.organizations AS o2
LEFT JOIN uc.users AS cu ON cu.id = o2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = o2.updated_by_user_id
 ORDER BY o2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAccountsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAccounts(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAccountsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oa.organization_id AS ""OrganizationId"",
oa.name AS ""Name"",
oa.account_no AS ""AccountNumber"",
oa.description AS ""Description"",
oa.app_system_no AS ""AppSystemNumber"",
oa.payment_method AS ""PaymentMethod"",
oa.account_status AS ""AccountStatus"",
oa.contact_info AS ""ContactInfo"",
oa.library_code AS ""LibraryCode"",
oa.library_edi_code AS ""LibraryEdiCode"",
oa.notes AS ""Notes"" 
FROM uc.organization_accounts AS oa
LEFT JOIN uc.organizations AS o ON o.id = oa.organization_id
 ORDER BY oa.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAccountAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAccountAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAccountAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oa.name AS ""OrganizationAccount"",
oaau.organization_account_id AS ""OrganizationAccountId"",
au.name AS ""AcquisitionsUnit"",
oaau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.organization_account_acquisitions_units AS oaau
LEFT JOIN uc.organization_accounts AS oa ON oa.id = oaau.organization_account_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = oaau.acquisitions_unit_id
 ORDER BY oaau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oau.organization_id AS ""OrganizationId"",
au.name AS ""AcquisitionsUnit"",
oau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.organization_acquisitions_units AS oau
LEFT JOIN uc.organizations AS o ON o.id = oau.organization_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = oau.acquisitions_unit_id
 ORDER BY oau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAddressesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAddresses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAddressesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oa.organization_id AS ""OrganizationId"",
oa.address_line1 AS ""StreetAddress1"",
oa.address_line2 AS ""StreetAddress2"",
oa.city AS ""City"",
oa.state_region AS ""State"",
oa.zip_code AS ""PostalCode"",
oa.country AS ""CountryCode"",
oa.is_primary AS ""IsPrimary"",
oa.language AS ""Language"",
oa.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
oa.created_by_user_id AS ""CreationUserId"",
oa.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
oa.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.organization_addresses AS oa
LEFT JOIN uc.organizations AS o ON o.id = oa.organization_id
LEFT JOIN uc.users AS cu ON cu.id = oa.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = oa.updated_by_user_id
 ORDER BY oa.address_line1
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAddressCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAddressCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oa.address_line1 AS ""OrganizationAddress"",
oac.organization_address_id AS ""OrganizationAddressId"",
c.value AS ""Category"",
oac.category_id AS ""CategoryId"" 
FROM uc.organization_address_categories AS oac
LEFT JOIN uc.organization_addresses AS oa ON oa.id = oac.organization_address_id
LEFT JOIN uc.categories AS c ON c.id = oac.category_id
 ORDER BY oac.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAgreementsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAgreements(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAgreementsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oa.organization_id AS ""OrganizationId"",
oa.name AS ""Name"",
oa.discount AS ""Discount"",
oa.reference_url AS ""ReferenceUrl"",
oa.notes AS ""Notes"" 
FROM uc.organization_agreements AS oa
LEFT JOIN uc.organizations AS o ON o.id = oa.organization_id
 ORDER BY oa.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAliasesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationAliases(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAliasesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationAliasesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oa.organization_id AS ""OrganizationId"",
oa.value AS ""Value"",
oa.description AS ""Description"" 
FROM uc.organization_aliases AS oa
LEFT JOIN uc.organizations AS o ON o.id = oa.organization_id
 ORDER BY oa.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAliasesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationChangelogsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationChangelogs(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationChangelogsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationChangelogsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oc.organization_id AS ""OrganizationId"",
oc.description AS ""Description"",
oc.timestamp AS ""Timestamp"" 
FROM uc.organization_changelogs AS oc
LEFT JOIN uc.organizations AS o ON o.id = oc.organization_id
 ORDER BY oc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationChangelogsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationContactsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationContacts(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationContactsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oc.organization_id AS ""OrganizationId"",
c.name AS ""Contact"",
oc.contact_id AS ""ContactId"" 
FROM uc.organization_contacts AS oc
LEFT JOIN uc.organizations AS o ON o.id = oc.organization_id
LEFT JOIN uc.contacts AS c ON c.id = oc.contact_id
 ORDER BY oc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationContactsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationEmailsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationEmails(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationEmailsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oe.organization_id AS ""OrganizationId"",
oe.value AS ""Value"",
oe.description AS ""Description"",
oe.is_primary AS ""IsPrimary"",
oe.language AS ""Language"",
oe.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
oe.created_by_user_id AS ""CreationUserId"",
oe.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
oe.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.organization_emails AS oe
LEFT JOIN uc.organizations AS o ON o.id = oe.organization_id
LEFT JOIN uc.users AS cu ON cu.id = oe.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = oe.updated_by_user_id
 ORDER BY oe.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationEmailCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationEmailCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
oe.value AS ""OrganizationEmail"",
oec.organization_email_id AS ""OrganizationEmailId"",
c.value AS ""Category"",
oec.category_id AS ""CategoryId"" 
FROM uc.organization_email_categories AS oec
LEFT JOIN uc.organization_emails AS oe ON oe.id = oec.organization_email_id
LEFT JOIN uc.categories AS c ON c.id = oec.category_id
 ORDER BY oec.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationInterfaces(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationInterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationInterfacesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
oi.organization_id AS ""OrganizationId"",
i.name AS ""Interface"",
oi.interface_id AS ""InterfaceId"" 
FROM uc.organization_interfaces AS oi
LEFT JOIN uc.organizations AS o ON o.id = oi.organization_id
LEFT JOIN uc.interfaces AS i ON i.id = oi.interface_id
 ORDER BY oi.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationInterfacesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationPhoneNumbers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationPhoneNumbersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
opn.organization_id AS ""OrganizationId"",
opn.phone_number AS ""PhoneNumber"",
opn.type AS ""Type"",
opn.is_primary AS ""IsPrimary"",
opn.language AS ""Language"",
opn.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
opn.created_by_user_id AS ""CreationUserId"",
opn.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
opn.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.organization_phone_numbers AS opn
LEFT JOIN uc.organizations AS o ON o.id = opn.organization_id
LEFT JOIN uc.users AS cu ON cu.id = opn.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = opn.updated_by_user_id
 ORDER BY opn.phone_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumbersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationPhoneNumberCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationPhoneNumberCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
opn.phone_number AS ""OrganizationPhoneNumber"",
opnc.organization_phone_number_id AS ""OrganizationPhoneNumberId"",
c.value AS ""Category"",
opnc.category_id AS ""CategoryId"" 
FROM uc.organization_phone_number_categories AS opnc
LEFT JOIN uc.organization_phone_numbers AS opn ON opn.id = opnc.organization_phone_number_id
LEFT JOIN uc.categories AS c ON c.id = opnc.category_id
 ORDER BY opnc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumberCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
ot.organization_id AS ""OrganizationId"",
ot.content AS ""Content"" 
FROM uc.organization_tags AS ot
LEFT JOIN uc.organizations AS o ON o.id = ot.organization_id
 ORDER BY ot.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationUrlsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationUrls(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationUrlsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.name AS ""Organization"",
ou.organization_id AS ""OrganizationId"",
ou.value AS ""Value"",
ou.description AS ""Description"",
ou.language AS ""Language"",
ou.is_primary AS ""IsPrimary"",
ou.notes AS ""Notes"",
ou.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ou.created_by_user_id AS ""CreationUserId"",
ou.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ou.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.organization_urls AS ou
LEFT JOIN uc.organizations AS o ON o.id = ou.organization_id
LEFT JOIN uc.users AS cu ON cu.id = ou.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ou.updated_by_user_id
 ORDER BY ou.value
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OrganizationUrlCategories(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationUrlCategoriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ou.value AS ""OrganizationUrl"",
ouc.organization_url_id AS ""OrganizationUrlId"",
c.value AS ""Category"",
ouc.category_id AS ""CategoryId"" 
FROM uc.organization_url_categories AS ouc
LEFT JOIN uc.organization_urls AS ou ON ou.id = ouc.organization_url_id
LEFT JOIN uc.categories AS c ON c.id = ouc.category_id
 ORDER BY ouc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlCategoriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOverdueFinePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.OverdueFinePolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OverdueFinePolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ofp2.id AS ""Id"",
ofp2.name AS ""Name"",
ofp2.description AS ""Description"",
ofp2.overdue_fine_quantity AS ""OverdueFineQuantity"",
ofp2.overdue_fine_interval_id AS ""OverdueFineInterval"",
ofp2.count_closed AS ""CountClosed"",
ofp2.max_overdue_fine AS ""MaxOverdueFine"",
ofp2.forgive_overdue_fine AS ""ForgiveOverdueFine"",
ofp2.overdue_recall_fine_quantity AS ""OverdueRecallFineQuantity"",
ofp2.overdue_recall_fine_interval_id AS ""OverdueRecallFineInterval"",
ofp2.grace_period_recall AS ""GracePeriodRecall"",
ofp2.max_overdue_recall_fine AS ""MaxOverdueRecallFine"",
ofp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ofp2.created_by_user_id AS ""CreationUserId"",
ofp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ofp2.updated_by_user_id AS ""LastWriteUserId"",
ofp2.content AS ""Content"" 
FROM uc.overdue_fine_policies AS ofp2
LEFT JOIN uc.users AS cu ON cu.id = ofp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ofp2.updated_by_user_id
 ORDER BY ofp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOwner2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Owner2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Owner2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o2.id AS ""Id"",
o2.owner AS ""Name"",
o2.desc AS ""Description"",
dcn.name AS ""DefaultChargeNotice"",
o2.default_charge_notice_id AS ""DefaultChargeNoticeId"",
dan.name AS ""DefaultActionNotice"",
o2.default_action_notice_id AS ""DefaultActionNoticeId"",
o2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
o2.created_by_user_id AS ""CreationUserId"",
o2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
o2.updated_by_user_id AS ""LastWriteUserId"",
o2.content AS ""Content"" 
FROM uc.owners AS o2
LEFT JOIN uc.templates AS dcn ON dcn.id = o2.default_charge_notice_id
LEFT JOIN uc.templates AS dan ON dan.id = o2.default_action_notice_id
LEFT JOIN uc.users AS cu ON cu.id = o2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = o2.updated_by_user_id
 ORDER BY o2.owner
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronActionSession2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PatronActionSession2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PatronActionSession2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pas2.id AS ""Id"",
p.username AS ""Patron"",
pas2.patron_id AS ""PatronId"",
l.id AS ""Loan"",
pas2.loan_id AS ""LoanId"",
pas2.action_type AS ""ActionType"",
pas2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
pas2.created_by_user_id AS ""CreationUserId"",
pas2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
pas2.updated_by_user_id AS ""LastWriteUserId"",
pas2.content AS ""Content"" 
FROM uc.patron_action_sessions AS pas2
LEFT JOIN uc.users AS p ON p.id = pas2.patron_id
LEFT JOIN uc.loans AS l ON l.id = pas2.loan_id
LEFT JOIN uc.users AS cu ON cu.id = pas2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = pas2.updated_by_user_id
 ORDER BY pas2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PatronNoticePolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PatronNoticePolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pnp2.id AS ""Id"",
pnp2.name AS ""Name"",
pnp2.description AS ""Description"",
pnp2.active AS ""Active"",
pnp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
pnp2.created_by_user_id AS ""CreationUserId"",
pnp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
pnp2.updated_by_user_id AS ""LastWriteUserId"",
pnp2.content AS ""Content"" 
FROM uc.patron_notice_policies AS pnp2
LEFT JOIN uc.users AS cu ON cu.id = pnp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = pnp2.updated_by_user_id
 ORDER BY pnp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyFeeFineNoticesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PatronNoticePolicyFeeFineNotices(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyFeeFineNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PatronNoticePolicyFeeFineNoticesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pnp.name AS ""PatronNoticePolicy"",
pnpffn.patron_notice_policy_id AS ""PatronNoticePolicyId"",
pnpffn.name AS ""Name"",
t.name AS ""Template"",
pnpffn.template_id AS ""TemplateId"",
pnpffn.template_name AS ""TemplateName"",
pnpffn.format AS ""Format"",
pnpffn.frequency AS ""Frequency"",
pnpffn.real_time AS ""RealTime"",
pnpffn.send_options_send_how AS ""SendOptionsSendHow"",
pnpffn.send_options_send_when AS ""SendOptionsSendWhen"",
pnpffn.send_options_send_by_duration AS ""SendOptionsSendByDuration"",
pnpffn.send_options_send_by_interval_id AS ""SendOptionsSendByInterval"",
pnpffn.send_options_send_every_duration AS ""SendOptionsSendEveryDuration"",
pnpffn.send_options_send_every_interval_id AS ""SendOptionsSendEveryInterval"" 
FROM uc.patron_notice_policy_fee_fine_notices AS pnpffn
LEFT JOIN uc.patron_notice_policies AS pnp ON pnp.id = pnpffn.patron_notice_policy_id
LEFT JOIN uc.templates AS t ON t.id = pnpffn.template_id
 ORDER BY pnpffn.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyFeeFineNoticesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyLoanNoticesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PatronNoticePolicyLoanNotices(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyLoanNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PatronNoticePolicyLoanNoticesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pnp.name AS ""PatronNoticePolicy"",
pnpln.patron_notice_policy_id AS ""PatronNoticePolicyId"",
pnpln.name AS ""Name"",
t.name AS ""Template"",
pnpln.template_id AS ""TemplateId"",
pnpln.template_name AS ""TemplateName"",
pnpln.format AS ""Format"",
pnpln.frequency AS ""Frequency"",
pnpln.real_time AS ""RealTime"",
pnpln.send_options_send_how AS ""SendOptionsSendHow"",
pnpln.send_options_send_when AS ""SendOptionsSendWhen"",
pnpln.send_options_send_by_duration AS ""SendOptionsSendByDuration"",
pnpln.send_options_send_by_interval_id AS ""SendOptionsSendByInterval"",
pnpln.send_options_send_every_duration AS ""SendOptionsSendEveryDuration"",
pnpln.send_options_send_every_interval_id AS ""SendOptionsSendEveryInterval"" 
FROM uc.patron_notice_policy_loan_notices AS pnpln
LEFT JOIN uc.patron_notice_policies AS pnp ON pnp.id = pnpln.patron_notice_policy_id
LEFT JOIN uc.templates AS t ON t.id = pnpln.template_id
 ORDER BY pnpln.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyLoanNoticesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyRequestNoticesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PatronNoticePolicyRequestNotices(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyRequestNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PatronNoticePolicyRequestNoticesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pnp.name AS ""PatronNoticePolicy"",
pnprn.patron_notice_policy_id AS ""PatronNoticePolicyId"",
pnprn.name AS ""Name"",
t.name AS ""Template"",
pnprn.template_id AS ""TemplateId"",
pnprn.template_name AS ""TemplateName"",
pnprn.format AS ""Format"",
pnprn.frequency AS ""Frequency"",
pnprn.real_time AS ""RealTime"",
pnprn.send_options_send_how AS ""SendOptionsSendHow"",
pnprn.send_options_send_when AS ""SendOptionsSendWhen"",
pnprn.send_options_send_by_duration AS ""SendOptionsSendByDuration"",
pnprn.send_options_send_by_interval_id AS ""SendOptionsSendByInterval"",
pnprn.send_options_send_every_duration AS ""SendOptionsSendEveryDuration"",
pnprn.send_options_send_every_interval_id AS ""SendOptionsSendEveryInterval"" 
FROM uc.patron_notice_policy_request_notices AS pnprn
LEFT JOIN uc.patron_notice_policies AS pnp ON pnp.id = pnprn.patron_notice_policy_id
LEFT JOIN uc.templates AS t ON t.id = pnprn.template_id
 ORDER BY pnprn.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyRequestNoticesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPayment2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Payment2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Payment2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p2.id AS ""Id"",
p2.date_action AS ""CreationTime"",
p2.type_action AS ""TypeAction"",
p2.comments AS ""Comments"",
p2.notify AS ""Notify"",
p2.amount_action AS ""Amount"",
p2.balance AS ""RemainingAmount"",
p2.transaction_information AS ""TransactionInformation"",
sp.name AS ""ServicePoint"",
p2.service_point_id AS ""ServicePointId"",
p2.source AS ""Source"",
p2.payment_method AS ""PaymentMethod"",
f.title AS ""Fee"",
p2.fee_id AS ""FeeId"",
u.username AS ""User"",
p2.user_id AS ""UserId"",
p2.content AS ""Content"" 
FROM uc.payments AS p2
LEFT JOIN uc.service_points AS sp ON sp.id = p2.service_point_id
LEFT JOIN uc.fees AS f ON f.id = p2.fee_id
LEFT JOIN uc.users AS u ON u.id = p2.user_id
 ORDER BY p2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPaymentMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PaymentMethod2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PaymentMethod2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pm2.id AS ""Id"",
pm2.name AS ""Name"",
pm2.allowed_refund_method AS ""AllowedRefundMethod"",
pm2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
pm2.created_by_user_id AS ""CreationUserId"",
pm2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
pm2.updated_by_user_id AS ""LastWriteUserId"",
o.owner AS ""Owner"",
pm2.owner_id AS ""OwnerId"",
pm2.content AS ""Content"" 
FROM uc.payment_methods AS pm2
LEFT JOIN uc.users AS cu ON cu.id = pm2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = pm2.updated_by_user_id
LEFT JOIN uc.owners AS o ON o.id = pm2.owner_id
 ORDER BY pm2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermission2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Permission2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permission2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p2.id AS ""Id"",
p2.permission_name AS ""Code"",
p2.display_name AS ""Name"",
p2.description AS ""Description"",
p2.mutable AS ""Editable"",
p2.visible AS ""Visible"",
p2.dummy AS ""Dummy"",
p2.deprecated AS ""Deprecated"",
p2.module_name AS ""ModuleName"",
p2.module_version AS ""ModuleVersion"",
p2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
p2.created_by_user_id AS ""CreationUserId"",
p2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
p2.updated_by_user_id AS ""LastWriteUserId"",
p2.content AS ""Content"" 
FROM uc.permissions AS p2
LEFT JOIN uc.users AS cu ON cu.id = p2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = p2.updated_by_user_id
 ORDER BY p2.display_name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionChildOfsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionChildOfs(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionChildOfsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionChildOfsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p.display_name AS ""Permission"",
pco.permission_id AS ""PermissionId"",
pco.content AS ""Content"" 
FROM uc.permission_child_of AS pco
LEFT JOIN uc.permissions AS p ON p.id = pco.permission_id
 ORDER BY pco.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionChildOfsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionGrantedTosTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionGrantedTos(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionGrantedTosTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionGrantedTosQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p.display_name AS ""Permission"",
pgt.permission_id AS ""PermissionId"",
pu.id AS ""PermissionsUser"",
pgt.permissions_user_id AS ""PermissionsUserId"" 
FROM uc.permission_granted_to AS pgt
LEFT JOIN uc.permissions AS p ON p.id = pgt.permission_id
LEFT JOIN uc.permissions_users AS pu ON pu.id = pgt.permissions_user_id
 ORDER BY pgt.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionGrantedTosQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionSubPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionSubPermissions(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionSubPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionSubPermissionsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p.display_name AS ""Permission"",
psp.permission_id AS ""PermissionId"",
psp.content AS ""Content"" 
FROM uc.permission_sub_permissions AS psp
LEFT JOIN uc.permissions AS p ON p.id = psp.permission_id
 ORDER BY psp.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionSubPermissionsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionsUser2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionsUser2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionsUser2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pu2.id AS ""Id"",
u.username AS ""User"",
pu2.user_id AS ""UserId"",
pu2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
pu2.created_by_user_id AS ""CreationUserId"",
pu2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
pu2.updated_by_user_id AS ""LastWriteUserId"",
pu2.content AS ""Content"" 
FROM uc.permissions_users AS pu2
LEFT JOIN uc.users AS u ON u.id = pu2.user_id
LEFT JOIN uc.users AS cu ON cu.id = pu2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = pu2.updated_by_user_id
 ORDER BY pu2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionsUserPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionsUserPermissions(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUserPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionsUserPermissionsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pu.id AS ""PermissionsUser"",
pup.permissions_user_id AS ""PermissionsUserId"",
pup.content AS ""Content"" 
FROM uc.permissions_user_permissions AS pup
LEFT JOIN uc.permissions_users AS pu ON pu.id = pup.permissions_user_id
 ORDER BY pup.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUserPermissionsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PermissionTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p.display_name AS ""Permission"",
pt.permission_id AS ""PermissionId"",
pt.content AS ""Content"" 
FROM uc.permission_tags AS pt
LEFT JOIN uc.permissions AS p ON p.id = pt.permission_id
 ORDER BY pt.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPhysicalDescriptionsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PhysicalDescriptions(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PhysicalDescriptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PhysicalDescriptionsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
pd.content AS ""Content"" 
FROM uc.physical_descriptions AS pd
LEFT JOIN uc.instances AS i ON i.id = pd.instance_id
 ORDER BY pd.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PhysicalDescriptionsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrecedingSucceedingTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PrecedingSucceedingTitle2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PrecedingSucceedingTitle2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pst2.id AS ""Id"",
pi.title AS ""PrecedingInstance"",
pst2.preceding_instance_id AS ""PrecedingInstanceId"",
si.title AS ""SucceedingInstance"",
pst2.succeeding_instance_id AS ""SucceedingInstanceId"",
pst2.title AS ""Title"",
pst2.hrid AS ""Hrid"",
pst2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
pst2.created_by_user_id AS ""CreationUserId"",
pst2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
pst2.updated_by_user_id AS ""LastWriteUserId"",
pst2.content AS ""Content"" 
FROM uc.preceding_succeeding_titles AS pst2
LEFT JOIN uc.instances AS pi ON pi.id = pst2.preceding_instance_id
LEFT JOIN uc.instances AS si ON si.id = pst2.succeeding_instance_id
LEFT JOIN uc.users AS cu ON cu.id = pst2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = pst2.updated_by_user_id
 ORDER BY pst2.title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrecedingSucceedingTitleIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PrecedingSucceedingTitleIdentifiers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitleIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PrecedingSucceedingTitleIdentifiersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
pst.title AS ""PrecedingSucceedingTitle"",
psti.preceding_succeeding_title_id AS ""PrecedingSucceedingTitleId"",
psti.value AS ""Value"",
it.name AS ""IdentifierType"",
psti.identifier_type_id AS ""IdentifierTypeId"" 
FROM uc.preceding_succeeding_title_identifiers AS psti
LEFT JOIN uc.preceding_succeeding_titles AS pst ON pst.id = psti.preceding_succeeding_title_id
LEFT JOIN uc.id_types AS it ON it.id = psti.identifier_type_id
 ORDER BY psti.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitleIdentifiersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrefix2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Prefix2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Prefix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Prefix2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p2.id AS ""Id"",
p2.name AS ""Name"",
p2.description AS ""Description"",
p2.content AS ""Content"" 
FROM uc.prefixes AS p2
 ORDER BY p2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Prefix2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrintersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Printers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PrintersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p.computer_name AS ""ComputerName"",
p.name AS ""Name"",
p.left AS ""Left"",
p.top AS ""Top"",
p.width AS ""Width"",
p.height AS ""Height"",
p.enabled AS ""Enabled"",
p.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
p.created_by_user_id AS ""CreationUserId"",
p.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
p.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.printers AS p
LEFT JOIN uc.users AS cu ON cu.id = p.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = p.updated_by_user_id
 ORDER BY p.computer_name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryProxy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Proxy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Proxy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
p2.id AS ""Id"",
u.username AS ""User"",
p2.user_id AS ""UserId"",
pu.username AS ""ProxyUser"",
p2.proxy_user_id AS ""ProxyUserId"",
p2.request_for_sponsor AS ""RequestForSponsor"",
p2.notifications_to AS ""NotificationsTo"",
p2.accrue_to AS ""AccrueTo"",
p2.status AS ""Status"",
p2.expiration_date AS ""ExpirationDate"",
p2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
p2.created_by_user_id AS ""CreationUserId"",
p2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
p2.updated_by_user_id AS ""LastWriteUserId"",
p2.content AS ""Content"" 
FROM uc.proxies AS p2
LEFT JOIN uc.users AS u ON u.id = p2.user_id
LEFT JOIN uc.users AS pu ON pu.id = p2.proxy_user_id
LEFT JOIN uc.users AS cu ON cu.id = p2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = p2.updated_by_user_id
 ORDER BY p2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Publications(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PublicationsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
p.publisher AS ""Publisher"",
p.place AS ""Place"",
p.date_of_publication AS ""PublicationYear"",
p.role AS ""Role"" 
FROM uc.publications AS p
LEFT JOIN uc.instances AS i ON i.id = p.instance_id
 ORDER BY p.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationFrequenciesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PublicationFrequencies(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationFrequenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PublicationFrequenciesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
pf.content AS ""Content"" 
FROM uc.publication_frequency AS pf
LEFT JOIN uc.instances AS i ON i.id = pf.instance_id
 ORDER BY pf.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationFrequenciesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationRangesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.PublicationRanges(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationRangesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PublicationRangesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
pr.content AS ""Content"" 
FROM uc.publication_range AS pr
LEFT JOIN uc.instances AS i ON i.id = pr.instance_id
 ORDER BY pr.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationRangesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRawRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RawRecord2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RawRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RawRecord2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rr2.id AS ""Id"",
r2.id AS ""Record2"",
rr2.content AS ""Content"" 
FROM uc.raw_records AS rr2
LEFT JOIN uc.records AS r2 ON r2.id = rr2.id
 ORDER BY rr2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RawRecord2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReceiving2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Receiving2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Receiving2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r2.id AS ""Id"",
r2.caption AS ""Caption"",
r2.comment AS ""Comment"",
r2.format AS ""Format"",
i.hrid AS ""Item"",
r2.item_id AS ""ItemId"",
l.name AS ""Location"",
r2.location_id AS ""LocationId"",
oi.po_line_number AS ""OrderItem"",
r2.po_line_id AS ""OrderItemId"",
t.title AS ""Title"",
r2.title_id AS ""TitleId"",
h.hrid AS ""Holding"",
r2.holding_id AS ""HoldingId"",
r2.display_on_holding AS ""DisplayOnHolding"",
r2.enumeration AS ""Enumeration"",
r2.chronology AS ""Chronology"",
r2.discovery_suppress AS ""DiscoverySuppress"",
r2.receiving_status AS ""ReceivingStatus"",
r2.supplement AS ""Supplement"",
r2.receipt_date AS ""ReceiptTime"",
r2.received_date AS ""ReceiveTime"",
r2.content AS ""Content"" 
FROM uc.receivings AS r2
LEFT JOIN uc.items AS i ON i.id = r2.item_id
LEFT JOIN uc.locations AS l ON l.id = r2.location_id
LEFT JOIN uc.order_items AS oi ON oi.id = r2.po_line_id
LEFT JOIN uc.titles AS t ON t.id = r2.title_id
LEFT JOIN uc.holdings AS h ON h.id = r2.holding_id
 ORDER BY r2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Record2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Record2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r2.id AS ""Id"",
s.id AS ""Snapshot"",
r2.snapshot_id AS ""SnapshotId"",
r2.matched_id AS ""MatchedId"",
r2.generation AS ""Generation"",
r2.record_type AS ""RecordType"",
i.title AS ""Instance"",
r2.instance_id AS ""InstanceId"",
r2.state AS ""State"",
r2.leader_record_status AS ""LeaderRecordStatus"",
r2.order AS ""Order"",
r2.suppress_discovery AS ""SuppressDiscovery"",
cu.username AS ""CreationUser"",
r2.creation_user_id AS ""CreationUserId"",
r2.creation_time AS ""CreationTime"",
lwu.username AS ""LastWriteUser"",
r2.last_write_user_id AS ""LastWriteUserId"",
r2.last_write_time AS ""LastWriteTime"",
r2.instance_hrid AS ""InstanceHrid"",
er2.id AS ""ErrorRecord2"",
mr2.id AS ""MarcRecord2"",
rr2.id AS ""RawRecord2"" 
FROM uc.records AS r2
LEFT JOIN uc.snapshots AS s ON s.id = r2.snapshot_id
LEFT JOIN uc.instances AS i ON i.id = r2.instance_id
LEFT JOIN uc.users AS cu ON cu.id = r2.creation_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = r2.last_write_user_id
LEFT JOIN uc.error_records AS er2 ON er2.id = r2.id
LEFT JOIN uc.marc_records AS mr2 ON mr2.id = r2.id
LEFT JOIN uc.raw_records AS rr2 ON rr2.id = r2.id
 ORDER BY r2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRefundReason2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RefundReason2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RefundReason2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rr2.id AS ""Id"",
rr2.name AS ""Name"",
rr2.description AS ""Description"",
rr2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
rr2.created_by_user_id AS ""CreationUserId"",
rr2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
rr2.updated_by_user_id AS ""LastWriteUserId"",
a.title AS ""Account"",
rr2.account_id AS ""AccountId"",
rr2.content AS ""Content"" 
FROM uc.refund_reasons AS rr2
LEFT JOIN uc.users AS cu ON cu.id = rr2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = rr2.updated_by_user_id
LEFT JOIN uc.fees AS a ON a.id = rr2.account_id
 ORDER BY rr2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Relationships(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RelationshipsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r.id AS ""Id"",
si.title AS ""SuperInstance"",
r.super_instance_id AS ""SuperInstanceId"",
si2.title AS ""SubInstance"",
r.sub_instance_id AS ""SubInstanceId"",
irt.name AS ""InstanceRelationshipType"",
r.instance_relationship_type_id AS ""InstanceRelationshipTypeId"",
r.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
r.created_by_user_id AS ""CreationUserId"",
r.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
r.updated_by_user_id AS ""LastWriteUserId"",
r.content AS ""Content"" 
FROM uc.relationships AS r
LEFT JOIN uc.instances AS si ON si.id = r.super_instance_id
LEFT JOIN uc.instances AS si2 ON si2.id = r.sub_instance_id
LEFT JOIN uc.relationship_types AS irt ON irt.id = r.instance_relationship_type_id
LEFT JOIN uc.users AS cu ON cu.id = r.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = r.updated_by_user_id
 ORDER BY r.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RelationshipTypes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RelationshipTypesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rt.id AS ""Id"",
rt.name AS ""Name"",
rt.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
rt.created_by_user_id AS ""CreationUserId"",
rt.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
rt.updated_by_user_id AS ""LastWriteUserId"",
rt.content AS ""Content"" 
FROM uc.relationship_types AS rt
LEFT JOIN uc.users AS cu ON cu.id = rt.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = rt.updated_by_user_id
 ORDER BY rt.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReportingCode2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ReportingCode2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ReportingCode2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rc2.id AS ""Id"",
rc2.code AS ""Code"",
rc2.description AS ""Description"",
rc2.content AS ""Content"" 
FROM uc.reporting_codes AS rc2
 ORDER BY rc2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCode2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequest2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Request2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Request2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r2.id AS ""Id"",
r2.request_type AS ""RequestType"",
r2.request_date AS ""RequestDate"",
r2.patron_comments AS ""PatronComments"",
r.username AS ""Requester"",
r2.requester_id AS ""RequesterId"",
pu.username AS ""ProxyUser"",
r2.proxy_user_id AS ""ProxyUserId"",
i.hrid AS ""Item"",
r2.item_id AS ""ItemId"",
r2.status AS ""Status"",
cr.name AS ""CancellationReason"",
r2.cancellation_reason_id AS ""CancellationReasonId"",
cbu.username AS ""CancelledByUser"",
r2.cancelled_by_user_id AS ""CancelledByUserId"",
r2.cancellation_additional_information AS ""CancellationAdditionalInformation"",
r2.cancelled_date AS ""CancelledDate"",
r2.position AS ""Position"",
r2.item_title AS ""ItemTitle"",
r2.item_barcode AS ""ItemBarcode"",
r2.requester_first_name AS ""RequesterFirstName"",
r2.requester_last_name AS ""RequesterLastName"",
r2.requester_middle_name AS ""RequesterMiddleName"",
r2.requester_barcode AS ""RequesterBarcode"",
r2.requester_patron_group AS ""RequesterPatronGroup"",
r2.proxy_first_name AS ""ProxyFirstName"",
r2.proxy_last_name AS ""ProxyLastName"",
r2.proxy_middle_name AS ""ProxyMiddleName"",
r2.proxy_barcode AS ""ProxyBarcode"",
r2.proxy_patron_group AS ""ProxyPatronGroup"",
r2.fulfilment_preference AS ""FulfilmentPreference"",
dat.address_type AS ""DeliveryAddressType"",
r2.delivery_address_type_id AS ""DeliveryAddressTypeId"",
r2.request_expiration_date AS ""RequestExpirationDate"",
r2.hold_shelf_expiration_date AS ""HoldShelfExpirationDate"",
psp.name AS ""PickupServicePoint"",
r2.pickup_service_point_id AS ""PickupServicePointId"",
r2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
r2.created_by_user_id AS ""CreationUserId"",
r2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
r2.updated_by_user_id AS ""LastWriteUserId"",
r2.awaiting_pickup_request_closed_date AS ""AwaitingPickupRequestClosedDate"",
r2.content AS ""Content"" 
FROM uc.requests AS r2
LEFT JOIN uc.users AS r ON r.id = r2.requester_id
LEFT JOIN uc.users AS pu ON pu.id = r2.proxy_user_id
LEFT JOIN uc.items AS i ON i.id = r2.item_id
LEFT JOIN uc.cancellation_reasons AS cr ON cr.id = r2.cancellation_reason_id
LEFT JOIN uc.users AS cbu ON cbu.id = r2.cancelled_by_user_id
LEFT JOIN uc.address_types AS dat ON dat.id = r2.delivery_address_type_id
LEFT JOIN uc.service_points AS psp ON psp.id = r2.pickup_service_point_id
LEFT JOIN uc.users AS cu ON cu.id = r2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = r2.updated_by_user_id
 ORDER BY r2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RequestIdentifiers(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RequestIdentifiersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r.id AS ""Request"",
ri.request_id AS ""RequestId"",
ri.value AS ""Value"",
it.name AS ""IdentifierType"",
ri.identifier_type_id AS ""IdentifierTypeId"" 
FROM uc.request_identifiers AS ri
LEFT JOIN uc.requests AS r ON r.id = ri.request_id
LEFT JOIN uc.id_types AS it ON it.id = ri.identifier_type_id
 ORDER BY ri.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestIdentifiersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RequestPolicy2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RequestPolicy2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rp2.id AS ""Id"",
rp2.name AS ""Name"",
rp2.description AS ""Description"",
rp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
rp2.created_by_user_id AS ""CreationUserId"",
rp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
rp2.updated_by_user_id AS ""LastWriteUserId"",
rp2.content AS ""Content"" 
FROM uc.request_policies AS rp2
LEFT JOIN uc.users AS cu ON cu.id = rp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = rp2.updated_by_user_id
 ORDER BY rp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestPolicyRequestTypesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RequestPolicyRequestTypes(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicyRequestTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RequestPolicyRequestTypesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
rp.name AS ""RequestPolicy"",
rprt.request_policy_id AS ""RequestPolicyId"",
rprt.content AS ""Content"" 
FROM uc.request_policy_request_types AS rprt
LEFT JOIN uc.request_policies AS rp ON rp.id = rprt.request_policy_id
 ORDER BY rprt.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicyRequestTypesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.RequestTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void RequestTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
r.id AS ""Request"",
rt.request_id AS ""RequestId"",
rt.content AS ""Content"" 
FROM uc.request_tags AS rt
LEFT JOIN uc.requests AS r ON r.id = rt.request_id
 ORDER BY rt.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryScheduledNotice2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ScheduledNotice2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ScheduledNotice2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sn2.id AS ""Id"",
l.id AS ""Loan"",
sn2.loan_id AS ""LoanId"",
r.id AS ""Request"",
sn2.request_id AS ""RequestId"",
p.id AS ""Payment"",
sn2.payment_id AS ""PaymentId"",
ru.username AS ""RecipientUser"",
sn2.recipient_user_id AS ""RecipientUserId"",
sn2.next_run_time AS ""NextRunTime"",
sn2.triggering_event AS ""TriggeringEvent"",
sn2.notice_config_timing AS ""NoticeConfigTiming"",
sn2.notice_config_recurring_period_duration AS ""NoticeConfigRecurringPeriodDuration"",
sn2.notice_config_recurring_period_interval_id AS ""NoticeConfigRecurringPeriodInterval"",
nct.name AS ""NoticeConfigTemplate"",
sn2.notice_config_template_id AS ""NoticeConfigTemplateId"",
sn2.notice_config_format AS ""NoticeConfigFormat"",
sn2.notice_config_send_in_real_time AS ""NoticeConfigSendInRealTime"",
sn2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
sn2.created_by_user_id AS ""CreationUserId"",
sn2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
sn2.updated_by_user_id AS ""LastWriteUserId"",
sn2.content AS ""Content"" 
FROM uc.scheduled_notices AS sn2
LEFT JOIN uc.loans AS l ON l.id = sn2.loan_id
LEFT JOIN uc.requests AS r ON r.id = sn2.request_id
LEFT JOIN uc.payments AS p ON p.id = sn2.payment_id
LEFT JOIN uc.users AS ru ON ru.id = sn2.recipient_user_id
LEFT JOIN uc.templates AS nct ON nct.id = sn2.notice_config_template_id
LEFT JOIN uc.users AS cu ON cu.id = sn2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = sn2.updated_by_user_id
 ORDER BY sn2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySeriesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Series(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SeriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SeriesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
s.content AS ""Content"" 
FROM uc.series AS s
LEFT JOIN uc.instances AS i ON i.id = s.instance_id
 ORDER BY s.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SeriesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePoint2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ServicePoint2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ServicePoint2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sp2.id AS ""Id"",
sp2.name AS ""Name"",
sp2.code AS ""Code"",
sp2.discovery_display_name AS ""DiscoveryDisplayName"",
sp2.description AS ""Description"",
sp2.shelving_lag_time AS ""ShelvingLagTime"",
sp2.pickup_location AS ""PickupLocation"",
sp2.hold_shelf_expiry_period_duration AS ""HoldShelfExpiryPeriodDuration"",
sp2.hold_shelf_expiry_period_interval_id AS ""HoldShelfExpiryPeriodInterval"",
sp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
sp2.created_by_user_id AS ""CreationUserId"",
sp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
sp2.updated_by_user_id AS ""LastWriteUserId"",
sp2.content AS ""Content"" 
FROM uc.service_points AS sp2
LEFT JOIN uc.users AS cu ON cu.id = sp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = sp2.updated_by_user_id
 ORDER BY sp2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointOwnersTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ServicePointOwners(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointOwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ServicePointOwnersQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
o.owner AS ""Owner"",
spo.owner_id AS ""OwnerId"",
sp.name AS ""ServicePoint"",
spo.service_point_id AS ""ServicePointId"",
spo.label AS ""Label"" 
FROM uc.service_point_owners AS spo
LEFT JOIN uc.owners AS o ON o.id = spo.owner_id
LEFT JOIN uc.service_points AS sp ON sp.id = spo.service_point_id
 ORDER BY spo.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointOwnersQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ServicePointStaffSlips(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointStaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ServicePointStaffSlipsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sp.name AS ""ServicePoint"",
spss.service_point_id AS ""ServicePointId"",
ss.name AS ""StaffSlip"",
spss.staff_slip_id AS ""StaffSlipId"",
spss.print_by_default AS ""PrintByDefault"" 
FROM uc.service_point_staff_slips AS spss
LEFT JOIN uc.service_points AS sp ON sp.id = spss.service_point_id
LEFT JOIN uc.staff_slips AS ss ON ss.id = spss.staff_slip_id
 ORDER BY spss.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointStaffSlipsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointUser2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ServicePointUser2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ServicePointUser2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
spu2.id AS ""Id"",
u.username AS ""User"",
spu2.user_id AS ""UserId"",
dsp.name AS ""DefaultServicePoint"",
spu2.default_service_point_id AS ""DefaultServicePointId"",
spu2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
spu2.created_by_user_id AS ""CreationUserId"",
spu2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
spu2.updated_by_user_id AS ""LastWriteUserId"",
spu2.content AS ""Content"" 
FROM uc.service_point_users AS spu2
LEFT JOIN uc.users AS u ON u.id = spu2.user_id
LEFT JOIN uc.service_points AS dsp ON dsp.id = spu2.default_service_point_id
LEFT JOIN uc.users AS cu ON cu.id = spu2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = spu2.updated_by_user_id
 ORDER BY spu2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointUserServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.ServicePointUserServicePoints(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUserServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ServicePointUserServicePointsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
spu.id AS ""ServicePointUser"",
spusp.service_point_user_id AS ""ServicePointUserId"",
sp.name AS ""ServicePoint"",
spusp.service_point_id AS ""ServicePointId"" 
FROM uc.service_point_user_service_points AS spusp
LEFT JOIN uc.service_point_users AS spu ON spu.id = spusp.service_point_user_id
LEFT JOIN uc.service_points AS sp ON sp.id = spusp.service_point_id
 ORDER BY spusp.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUserServicePointsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySettingsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Settings(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SettingsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
s.name AS ""Name"",
s.orientation AS ""Orientation"",
s.font_family AS ""FontFamily"",
s.font_size AS ""FontSize"",
s.font_weight AS ""FontWeight"",
s.enabled AS ""Enabled"",
s.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
s.created_by_user_id AS ""CreationUserId"",
s.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
s.updated_by_user_id AS ""LastWriteUserId"" 
FROM uc.settings AS s
LEFT JOIN uc.users AS cu ON cu.id = s.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = s.updated_by_user_id
 ORDER BY s.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySnapshot2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Snapshot2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Snapshot2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
s2.status AS ""Status"",
s2.processing_started_date AS ""ProcessingStartedDate"",
cu.username AS ""CreationUser"",
s2.creation_user_id AS ""CreationUserId"",
s2.creation_time AS ""CreationTime"",
lwu.username AS ""LastWriteUser"",
s2.last_write_user_id AS ""LastWriteUserId"",
s2.last_write_time AS ""LastWriteTime"" 
FROM uc.snapshots AS s2
LEFT JOIN uc.users AS cu ON cu.id = s2.creation_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = s2.last_write_user_id
 ORDER BY s2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySource2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Source2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
s2.id AS ""Id"",
s2.name AS ""Name"",
s2.source AS ""Source"",
s2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
s2.created_by_user_id AS ""CreationUserId"",
s2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
s2.updated_by_user_id AS ""LastWriteUserId"",
s2.content AS ""Content"" 
FROM uc.sources AS s2
LEFT JOIN uc.users AS cu ON cu.id = s2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = s2.updated_by_user_id
 ORDER BY s2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySourceMarcsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.SourceMarcs(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SourceMarcsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sm.id AS ""Id"",
sm.leader AS ""Leader"",
sm.content AS ""Content"" 
FROM uc.source_marcs AS sm
 ORDER BY sm.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySourceMarcFieldsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.SourceMarcFields(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SourceMarcFieldsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sm.id AS ""SourceMarc"",
smf.source_marc_id AS ""SourceMarcId"",
smf.content AS ""Content"" 
FROM uc.source_marc_fields AS smf
LEFT JOIN uc.source_marcs AS sm ON sm.id = smf.source_marc_id
 ORDER BY smf.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcFieldsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStaffSlip2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.StaffSlip2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void StaffSlip2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ss2.id AS ""Id"",
ss2.name AS ""Name"",
ss2.description AS ""Description"",
ss2.active AS ""Active"",
ss2.template AS ""Template"",
ss2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ss2.created_by_user_id AS ""CreationUserId"",
ss2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ss2.updated_by_user_id AS ""LastWriteUserId"",
ss2.content AS ""Content"" 
FROM uc.staff_slips AS ss2
LEFT JOIN uc.users AS cu ON cu.id = ss2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ss2.updated_by_user_id
 ORDER BY ss2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCode2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.StatisticalCode2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void StatisticalCode2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sc2.id AS ""Id"",
sc2.code AS ""Code"",
sc2.name AS ""Name"",
sct.name AS ""StatisticalCodeType"",
sc2.statistical_code_type_id AS ""StatisticalCodeTypeId"",
sc2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
sc2.created_by_user_id AS ""CreationUserId"",
sc2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
sc2.updated_by_user_id AS ""LastWriteUserId"",
sc2.content AS ""Content"" 
FROM uc.statistical_codes AS sc2
LEFT JOIN uc.statistical_code_types AS sct ON sct.id = sc2.statistical_code_type_id
LEFT JOIN uc.users AS cu ON cu.id = sc2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = sc2.updated_by_user_id
 ORDER BY sc2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCodeType2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.StatisticalCodeType2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void StatisticalCodeType2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
sct2.id AS ""Id"",
sct2.name AS ""Name"",
sct2.source AS ""Source"",
sct2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
sct2.created_by_user_id AS ""CreationUserId"",
sct2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
sct2.updated_by_user_id AS ""LastWriteUserId"",
sct2.content AS ""Content"" 
FROM uc.statistical_code_types AS sct2
LEFT JOIN uc.users AS cu ON cu.id = sct2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = sct2.updated_by_user_id
 ORDER BY sct2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatusesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Statuses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void StatusesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
s.id AS ""Id"",
s.code AS ""Code"",
s.name AS ""Name"",
s.source AS ""Source"",
s.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
s.created_by_user_id AS ""CreationUserId"",
s.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
s.updated_by_user_id AS ""LastWriteUserId"",
s.content AS ""Content"" 
FROM uc.statuses AS s
LEFT JOIN uc.users AS cu ON cu.id = s.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = s.updated_by_user_id
 ORDER BY s.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySubjectsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Subjects(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SubjectsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
i.title AS ""Instance"",
s.content AS ""Content"" 
FROM uc.subjects AS s
LEFT JOIN uc.instances AS i ON i.id = s.instance_id
 ORDER BY s.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySuffix2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Suffix2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Suffix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Suffix2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
s2.id AS ""Id"",
s2.name AS ""Name"",
s2.description AS ""Description"",
s2.content AS ""Content"" 
FROM uc.suffixes AS s2
 ORDER BY s2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Suffix2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySupplementStatementsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.SupplementStatements(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SupplementStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SupplementStatementsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
h.hrid AS ""Holding"",
ss.statement AS ""Statement"",
ss.note AS ""Note"",
ss.staff_note AS ""StaffNote"" 
FROM uc.supplement_statements AS ss
LEFT JOIN uc.holdings AS h ON h.id = ss.holding_id
 ORDER BY ss.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SupplementStatementsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Template2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Template2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t2.id AS ""Id"",
t2.name AS ""Name"",
t2.active AS ""Active"",
t2.category AS ""Category"",
t2.description AS ""Description"",
t2.template_resolver AS ""TemplateResolver"",
t2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
t2.created_by_user_id AS ""CreationUserId"",
t2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
t2.updated_by_user_id AS ""LastWriteUserId"",
t2.content AS ""Content"" 
FROM uc.templates AS t2
LEFT JOIN uc.users AS cu ON cu.id = t2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = t2.updated_by_user_id
 ORDER BY t2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTemplateOutputFormatsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TemplateOutputFormats(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplateOutputFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TemplateOutputFormatsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t.name AS ""Template"",
tof.template_id AS ""TemplateId"",
tof.content AS ""Content"" 
FROM uc.template_output_formats AS tof
LEFT JOIN uc.templates AS t ON t.id = tof.template_id
 ORDER BY tof.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplateOutputFormatsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Title2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Title2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t2.id AS ""Id"",
t2.expected_receipt_date AS ""ExpectedReceiptDate"",
t2.title AS ""Title"",
oi.po_line_number AS ""OrderItem"",
t2.po_line_id AS ""OrderItemId"",
i.title AS ""Instance"",
t2.instance_id AS ""InstanceId"",
t2.publisher AS ""Publisher"",
t2.edition AS ""Edition"",
t2.package_name AS ""PackageName"",
t2.po_line_number AS ""OrderItemNumber"",
t2.published_date AS ""PublishedDate"",
t2.receiving_note AS ""ReceivingNote"",
t2.subscription_from AS ""SubscriptionFrom"",
t2.subscription_to AS ""SubscriptionTo"",
t2.subscription_interval AS ""SubscriptionInterval"",
t2.is_acknowledged AS ""IsAcknowledged"",
t2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
t2.created_by_user_id AS ""CreationUserId"",
t2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
t2.updated_by_user_id AS ""LastWriteUserId"",
t2.content AS ""Content"" 
FROM uc.titles AS t2
LEFT JOIN uc.order_items AS oi ON oi.id = t2.po_line_id
LEFT JOIN uc.instances AS i ON i.id = t2.instance_id
LEFT JOIN uc.users AS cu ON cu.id = t2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = t2.updated_by_user_id
 ORDER BY t2.title
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleContributorsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TitleContributors(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TitleContributorsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t.title AS ""Title"",
tc.title_id AS ""TitleId"",
tc.contributor AS ""Contributor"",
cnt.name AS ""ContributorNameType"",
tc.contributor_name_type_id AS ""ContributorNameTypeId"" 
FROM uc.title_contributors AS tc
LEFT JOIN uc.titles AS t ON t.id = tc.title_id
LEFT JOIN uc.contributor_name_types AS cnt ON cnt.id = tc.contributor_name_type_id
 ORDER BY tc.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleContributorsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TitleProductIds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TitleProductIdsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t.title AS ""Title"",
tpi.title_id AS ""TitleId"",
tpi.product_id AS ""ProductId"",
pit.name AS ""ProductIdType"",
tpi.product_id_type_id AS ""ProductIdTypeId"",
tpi.qualifier AS ""Qualifier"" 
FROM uc.title_product_ids AS tpi
LEFT JOIN uc.titles AS t ON t.id = tpi.title_id
LEFT JOIN uc.id_types AS pit ON pit.id = tpi.product_id_type_id
 ORDER BY tpi.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleProductIdsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransaction2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Transaction2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Transaction2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t2.id AS ""Id"",
t2.amount AS ""Amount"",
ape.amount AS ""AwaitingPaymentEncumbrance"",
t2.awaiting_payment_encumbrance_id AS ""AwaitingPaymentEncumbranceId"",
t2.awaiting_payment_release_encumbrance AS ""AwaitingPaymentReleaseEncumbrance"",
t2.currency AS ""Currency"",
t2.description AS ""Description"",
t2.encumbrance_amount_awaiting_payment AS ""AwaitingPaymentAmount"",
t2.encumbrance_amount_expended AS ""ExpendedAmount"",
t2.encumbrance_initial_amount_encumbered AS ""InitialEncumberedAmount"",
t2.encumbrance_status AS ""Status"",
t2.encumbrance_order_type AS ""OrderType"",
t2.encumbrance_order_status AS ""OrderStatus"",
t2.encumbrance_subscription AS ""Subscription"",
t2.encumbrance_re_encumber AS ""ReEncumber"",
o.po_number AS ""Order"",
t2.encumbrance_source_purchase_order_id AS ""OrderId"",
oi.po_line_number AS ""OrderItem"",
t2.encumbrance_source_po_line_id AS ""OrderItemId"",
ec.name AS ""ExpenseClass"",
t2.expense_class_id AS ""ExpenseClassId"",
fy.name AS ""FiscalYear"",
t2.fiscal_year_id AS ""FiscalYearId"",
ff.name AS ""FromFund"",
t2.from_fund_id AS ""FromFundId"",
t2.invoice_cancelled AS ""InvoiceCancelled"",
pe.amount AS ""PaymentEncumbrance"",
t2.payment_encumbrance_id AS ""PaymentEncumbranceId"",
t2.source AS ""Source"",
sfy.name AS ""SourceFiscalYear"",
t2.source_fiscal_year_id AS ""SourceFiscalYearId"",
i.folio_invoice_no AS ""Invoice"",
t2.source_invoice_id AS ""InvoiceId"",
ii.invoice_line_number AS ""InvoiceItem"",
t2.source_invoice_line_id AS ""InvoiceItemId"",
tf.name AS ""ToFund"",
t2.to_fund_id AS ""ToFundId"",
t2.transaction_type AS ""TransactionType"",
t2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
t2.created_by_user_id AS ""CreationUserId"",
t2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
t2.updated_by_user_id AS ""LastWriteUserId"",
t2.content AS ""Content"" 
FROM uc.transactions AS t2
LEFT JOIN uc.transactions AS ape ON ape.id = t2.awaiting_payment_encumbrance_id
LEFT JOIN uc.orders AS o ON o.id = t2.encumbrance_source_purchase_order_id
LEFT JOIN uc.order_items AS oi ON oi.id = t2.encumbrance_source_po_line_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = t2.expense_class_id
LEFT JOIN uc.fiscal_years AS fy ON fy.id = t2.fiscal_year_id
LEFT JOIN uc.funds AS ff ON ff.id = t2.from_fund_id
LEFT JOIN uc.transactions AS pe ON pe.id = t2.payment_encumbrance_id
LEFT JOIN uc.fiscal_years AS sfy ON sfy.id = t2.source_fiscal_year_id
LEFT JOIN uc.invoices AS i ON i.id = t2.source_invoice_id
LEFT JOIN uc.invoice_items AS ii ON ii.id = t2.source_invoice_line_id
LEFT JOIN uc.funds AS tf ON tf.id = t2.to_fund_id
LEFT JOIN uc.users AS cu ON cu.id = t2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = t2.updated_by_user_id
 ORDER BY t2.amount
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransactionTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TransactionTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TransactionTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
t.amount AS ""Transaction"",
tt.transaction_id AS ""TransactionId"",
tt.content AS ""Content"" 
FROM uc.transaction_tags AS tt
LEFT JOIN uc.transactions AS t ON t.id = tt.transaction_id
 ORDER BY tt.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferAccount2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TransferAccount2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TransferAccount2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
ta2.id AS ""Id"",
ta2.name AS ""Name"",
ta2.desc AS ""Description"",
ta2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
ta2.created_by_user_id AS ""CreationUserId"",
ta2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
ta2.updated_by_user_id AS ""LastWriteUserId"",
o.owner AS ""Owner"",
ta2.owner_id AS ""OwnerId"",
ta2.content AS ""Content"" 
FROM uc.transfer_accounts AS ta2
LEFT JOIN uc.users AS cu ON cu.id = ta2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = ta2.updated_by_user_id
LEFT JOIN uc.owners AS o ON o.id = ta2.owner_id
 ORDER BY ta2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferCriteria2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.TransferCriteria2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TransferCriteria2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
tc2.id AS ""Id"",
tc2.criteria AS ""Criteria"",
tc2.type AS ""Type"",
tc2.value AS ""Value"",
tc2.interval AS ""Interval"",
tc2.content AS ""Content"" 
FROM uc.transfer_criterias AS tc2
 ORDER BY tc2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUser2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.User2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void User2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
u2.id AS ""Id"",
u2.username AS ""Username"",
u2.external_system_id AS ""ExternalSystemId"",
u2.barcode AS ""Barcode"",
u2.active AS ""Active"",
g.group AS ""Group"",
u2.group_id AS ""GroupId"",
u2.name AS ""Name"",
u2.last_name AS ""LastName"",
u2.first_name AS ""FirstName"",
u2.middle_name AS ""MiddleName"",
u2.preferred_first_name AS ""PreferredFirstName"",
u2.email AS ""EmailAddress"",
u2.phone AS ""PhoneNumber"",
u2.mobile_phone AS ""MobilePhoneNumber"",
u2.date_of_birth AS ""BirthDate"",
pct.name AS ""PreferredContactType"",
u2.preferred_contact_type_id AS ""PreferredContactTypeId"",
u2.enrollment_date AS ""StartDate"",
u2.expiration_date AS ""EndDate"",
u2.source AS ""Source"",
u2.category AS ""Category"",
u2.status AS ""Status"",
u2.statuses AS ""Statuses"",
u2.staff_status AS ""StaffStatus"",
u2.staff_privileges AS ""StaffPrivileges"",
u2.staff_division AS ""StaffDivision"",
u2.staff_department AS ""StaffDepartment"",
u2.student_id AS ""StudentId"",
u2.student_status AS ""StudentStatus"",
u2.student_restriction AS ""StudentRestriction"",
u2.student_division AS ""StudentDivision"",
u2.student_department AS ""StudentDepartment"",
u2.deceased AS ""Deceased"",
u2.collections AS ""Collections"",
u2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
u2.created_by_user_id AS ""CreationUserId"",
u2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
u2.updated_by_user_id AS ""LastWriteUserId"",
u2.content AS ""Content"" 
FROM uc.users AS u2
LEFT JOIN uc.groups AS g ON g.id = u2.group_id
LEFT JOIN uc.contact_types AS pct ON pct.id = u2.preferred_contact_type_id
LEFT JOIN uc.users AS cu ON cu.id = u2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = u2.updated_by_user_id
 ORDER BY u2.username
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserAcquisitionsUnit2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserAcquisitionsUnit2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
uau2.id AS ""Id"",
u.username AS ""User"",
uau2.user_id AS ""UserId"",
au.name AS ""AcquisitionsUnit"",
uau2.acquisitions_unit_id AS ""AcquisitionsUnitId"",
uau2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
uau2.created_by_user_id AS ""CreationUserId"",
uau2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
uau2.updated_by_user_id AS ""LastWriteUserId"",
uau2.content AS ""Content"" 
FROM uc.user_acquisitions_units AS uau2
LEFT JOIN uc.users AS u ON u.id = uau2.user_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = uau2.acquisitions_unit_id
LEFT JOIN uc.users AS cu ON cu.id = uau2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = uau2.updated_by_user_id
 ORDER BY uau2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserAddressesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserAddresses(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserAddressesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
u.username AS ""User"",
ua.user_id AS ""UserId"",
ua.country_id AS ""CountryCode"",
ua.address_line1 AS ""StreetAddress1"",
ua.address_line2 AS ""StreetAddress2"",
ua.city AS ""City"",
ua.region AS ""State"",
ua.postal_code AS ""PostalCode"",
at.address_type AS ""AddressType"",
ua.address_type_id AS ""AddressTypeId"",
ua.primary_address AS ""Default"" 
FROM uc.user_addresses AS ua
LEFT JOIN uc.users AS u ON u.id = ua.user_id
LEFT JOIN uc.address_types AS at ON at.id = ua.address_type_id
 ORDER BY ua.address_line1
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAddressesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserDepartments(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserDepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserDepartmentsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
u.username AS ""User"",
ud.user_id AS ""UserId"",
d.name AS ""Department"",
ud.department_id AS ""DepartmentId"" 
FROM uc.user_departments AS ud
LEFT JOIN uc.users AS u ON u.id = ud.user_id
LEFT JOIN uc.departments AS d ON d.id = ud.department_id
 ORDER BY ud.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserDepartmentsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserRequestPreference2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserRequestPreference2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserRequestPreference2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
urp2.id AS ""Id"",
u.username AS ""User"",
urp2.user_id AS ""UserId"",
urp2.hold_shelf AS ""HoldShelf"",
urp2.delivery AS ""Delivery"",
dsp.name AS ""DefaultServicePoint"",
urp2.default_service_point_id AS ""DefaultServicePointId"",
ddat.address_type AS ""DefaultDeliveryAddressType"",
urp2.default_delivery_address_type_id AS ""DefaultDeliveryAddressTypeId"",
urp2.fulfillment AS ""Fulfillment"",
urp2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
urp2.created_by_user_id AS ""CreationUserId"",
urp2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
urp2.updated_by_user_id AS ""LastWriteUserId"",
urp2.content AS ""Content"" 
FROM uc.user_request_preferences AS urp2
LEFT JOIN uc.users AS u ON u.id = urp2.user_id
LEFT JOIN uc.service_points AS dsp ON dsp.id = urp2.default_service_point_id
LEFT JOIN uc.address_types AS ddat ON ddat.id = urp2.default_delivery_address_type_id
LEFT JOIN uc.users AS cu ON cu.id = urp2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = urp2.updated_by_user_id
 ORDER BY urp2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummary2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserSummary2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserSummary2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
us2.id AS ""Id"",
us2._version AS ""Version"",
u.username AS ""User"",
us2.user_id AS ""UserId"",
us2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
us2.created_by_user_id AS ""CreationUserId"",
us2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
us2.updated_by_user_id AS ""LastWriteUserId"",
us2.content AS ""Content"" 
FROM uc.user_summaries AS us2
LEFT JOIN uc.users AS u ON u.id = us2.user_id
LEFT JOIN uc.users AS cu ON cu.id = us2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = us2.updated_by_user_id
 ORDER BY us2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummary2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummaryOpenFeesFinesTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserSummaryOpenFeesFines(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenFeesFinesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserSummaryOpenFeesFinesQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
us.id AS ""UserSummary"",
usoff.user_summary_id AS ""UserSummaryId"",
ff.title AS ""FeeFine"",
usoff.fee_fine_id AS ""FeeFineId"",
fft.fee_fine_type AS ""FeeFineType"",
usoff.fee_fine_type_id AS ""FeeFineTypeId"",
l.id AS ""Loan"",
usoff.loan_id AS ""LoanId"",
usoff.balance AS ""Balance"" 
FROM uc.user_summary_open_fees_fines AS usoff
LEFT JOIN uc.user_summaries AS us ON us.id = usoff.user_summary_id
LEFT JOIN uc.fees AS ff ON ff.id = usoff.fee_fine_id
LEFT JOIN uc.fee_types AS fft ON fft.id = usoff.fee_fine_type_id
LEFT JOIN uc.loans AS l ON l.id = usoff.loan_id
 ORDER BY usoff.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenFeesFinesQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummaryOpenLoansTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserSummaryOpenLoans(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenLoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserSummaryOpenLoansQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
us.id AS ""UserSummary"",
usol.user_summary_id AS ""UserSummaryId"",
l.id AS ""Loan"",
usol.loan_id AS ""LoanId"",
usol.due_date AS ""DueDate"",
usol.recall AS ""Recall"",
usol.item_lost AS ""ItemLost"",
usol.item_claimed_returned AS ""ItemClaimedReturned"",
usol.grace_period_duration AS ""GracePeriodDuration"",
usol.grace_period_interval_id AS ""GracePeriodInterval"" 
FROM uc.user_summary_open_loans AS usol
LEFT JOIN uc.user_summaries AS us ON us.id = usol.user_summary_id
LEFT JOIN uc.loans AS l ON l.id = usol.loan_id
 ORDER BY usol.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenLoansQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserTagsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.UserTags(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UserTagsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
u.username AS ""User"",
ut.user_id AS ""UserId"",
ut.content AS ""Content"" 
FROM uc.user_tags AS ut
LEFT JOIN uc.users AS u ON u.id = ut.user_id
 ORDER BY ut.content
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserTagsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucher2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Voucher2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Voucher2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
v2.id AS ""Id"",
v2.accounting_code AS ""AccountingCode"",
v2.account_no AS ""AccountNumber"",
v2.amount AS ""Amount"",
bg.name AS ""BatchGroup"",
v2.batch_group_id AS ""BatchGroupId"",
v2.disbursement_number AS ""DisbursementNumber"",
v2.disbursement_date AS ""DisbursementDate"",
v2.disbursement_amount AS ""DisbursementAmount"",
v2.enclosure_needed AS ""Enclosure"",
v2.invoice_currency AS ""InvoiceCurrency"",
i.folio_invoice_no AS ""Invoice"",
v2.invoice_id AS ""InvoiceId"",
v2.exchange_rate AS ""ExchangeRate"",
v2.export_to_accounting AS ""ExportToAccounting"",
v2.status AS ""Status"",
v2.system_currency AS ""SystemCurrency"",
v2.type AS ""Type"",
v2.voucher_date AS ""VoucherDate"",
v2.voucher_number AS ""Number"",
v.name AS ""Vendor"",
v2.vendor_id AS ""VendorId"",
v2.vendor_address_address_line1 AS ""VendorStreetAddress1"",
v2.vendor_address_address_line2 AS ""VendorStreetAddress2"",
v2.vendor_address_city AS ""VendorCity"",
v2.vendor_address_state_region AS ""VendorState"",
v2.vendor_address_zip_code AS ""VendorPostalCode"",
v2.vendor_address_country AS ""VendorCountryCode"",
v2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
v2.created_by_user_id AS ""CreationUserId"",
v2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
v2.updated_by_user_id AS ""LastWriteUserId"",
v2.content AS ""Content"" 
FROM uc.vouchers AS v2
LEFT JOIN uc.batch_groups AS bg ON bg.id = v2.batch_group_id
LEFT JOIN uc.invoices AS i ON i.id = v2.invoice_id
LEFT JOIN uc.organizations AS v ON v.id = v2.vendor_id
LEFT JOIN uc.users AS cu ON cu.id = v2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = v2.updated_by_user_id
 ORDER BY v2.voucher_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.VoucherAcquisitionsUnits(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void VoucherAcquisitionsUnitsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
v.voucher_number AS ""Voucher"",
vau.voucher_id AS ""VoucherId"",
au.name AS ""AcquisitionsUnit"",
vau.acquisitions_unit_id AS ""AcquisitionsUnitId"" 
FROM uc.voucher_acquisitions_units AS vau
LEFT JOIN uc.vouchers AS v ON v.id = vau.voucher_id
LEFT JOIN uc.acquisitions_units AS au ON au.id = vau.acquisitions_unit_id
 ORDER BY vau.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherAcquisitionsUnitsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItem2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.VoucherItem2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void VoucherItem2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
vi2.id AS ""Id"",
vi2.amount AS ""Amount"",
vi2.external_account_number AS ""AccountNumber"",
st.amount AS ""SubTransaction"",
vi2.sub_transaction_id AS ""SubTransactionId"",
v.voucher_number AS ""Voucher"",
vi2.voucher_id AS ""VoucherId"",
vi2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
vi2.created_by_user_id AS ""CreationUserId"",
vi2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
vi2.updated_by_user_id AS ""LastWriteUserId"",
vi2.content AS ""Content"" 
FROM uc.voucher_items AS vi2
LEFT JOIN uc.transactions AS st ON st.id = vi2.sub_transaction_id
LEFT JOIN uc.vouchers AS v ON v.id = vi2.voucher_id
LEFT JOIN uc.users AS cu ON cu.id = vi2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = vi2.updated_by_user_id
 ORDER BY vi2.external_account_number
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.VoucherItemFunds(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void VoucherItemFundsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
vi.external_account_number AS ""VoucherItem"",
vif.voucher_item_id AS ""VoucherItemId"",
vif.code AS ""FundCode"",
e2.amount AS ""Encumbrance"",
vif.encumbrance_id AS ""EncumbranceId"",
f.name AS ""Fund"",
vif.fund_id AS ""FundId"",
ii.invoice_line_number AS ""InvoiceItem"",
vif.invoice_item_id AS ""InvoiceItemId"",
vif.distribution_type AS ""DistributionType"",
ec.name AS ""ExpenseClass"",
vif.expense_class_id AS ""ExpenseClassId"",
vif.value AS ""Value"" 
FROM uc.voucher_item_fund_distributions AS vif
LEFT JOIN uc.voucher_items AS vi ON vi.id = vif.voucher_item_id
LEFT JOIN uc.transactions AS e2 ON e2.id = vif.encumbrance_id
LEFT JOIN uc.funds AS f ON f.id = vif.fund_id
LEFT JOIN uc.invoice_items AS ii ON ii.id = vif.invoice_item_id
LEFT JOIN uc.expense_classes AS ec ON ec.id = vif.expense_class_id
 ORDER BY vif.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemFundsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItemInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.VoucherItemInvoiceItems(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemInvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void VoucherItemInvoiceItemsQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
vi.external_account_number AS ""VoucherItem"",
viii.voucher_item_id AS ""VoucherItemId"",
ii.invoice_line_number AS ""InvoiceItem"",
viii.invoice_item_id AS ""InvoiceItemId"" 
FROM uc.voucher_item_invoice_items AS viii
LEFT JOIN uc.voucher_items AS vi ON vi.id = viii.voucher_item_id
LEFT JOIN uc.invoice_items AS ii ON ii.id = viii.invoice_item_id
 ORDER BY viii.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemInvoiceItemsQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryWaiveReason2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.WaiveReason2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void WaiveReason2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
wr2.id AS ""Id"",
wr2.name AS ""Name"",
wr2.description AS ""Description"",
wr2.created_date AS ""CreationTime"",
cu.username AS ""CreationUser"",
wr2.created_by_user_id AS ""CreationUserId"",
wr2.updated_date AS ""LastWriteTime"",
lwu.username AS ""LastWriteUser"",
wr2.updated_by_user_id AS ""LastWriteUserId"",
a.title AS ""Account"",
wr2.account_id AS ""AccountId"",
wr2.content AS ""Content"" 
FROM uc.waive_reasons AS wr2
LEFT JOIN uc.users AS cu ON cu.id = wr2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = wr2.updated_by_user_id
LEFT JOIN uc.fees AS a ON a.id = wr2.account_id
 ORDER BY wr2.name
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioDapperContext.Dispose();
        }
    }
}
