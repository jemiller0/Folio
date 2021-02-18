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
b2.desc AS ""Desc"",
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
ec2.external_account_number_ext AS ""ExternalAccountNumberExt"",
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
f2.external_account_no AS ""ExternalAccountNo"",
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
        public void QueryGroupFundFiscalYear2sTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.GroupFundFiscalYear2s(take: 1).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"GroupFundFiscalYear2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GroupFundFiscalYear2sQueryTest()
        {
            var s = Stopwatch.StartNew();
            folioDapperContext.Query(@"
SELECT
gffy2.id AS ""Id"",
b.name AS ""Budget"",
gffy2.budget_id AS ""BudgetId"",
g.name AS ""Group"",
gffy2.group_id AS ""GroupId"",
fy.name AS ""FiscalYear"",
gffy2.fiscal_year_id AS ""FiscalYearId"",
f.name AS ""Fund"",
gffy2.fund_id AS ""FundId"",
gffy2.content AS ""Content"" 
FROM uc.group_fund_fiscal_years AS gffy2
LEFT JOIN uc.budgets AS b ON b.id = gffy2.budget_id
LEFT JOIN uc.finance_groups AS g ON g.id = gffy2.group_id
LEFT JOIN uc.fiscal_years AS fy ON fy.id = gffy2.fiscal_year_id
LEFT JOIN uc.funds AS f ON f.id = gffy2.fund_id
 ORDER BY gffy2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"GroupFundFiscalYear2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
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
h2.hrid AS ""ShortId"",
ht.name AS ""HoldingType"",
h2.holding_type_id AS ""HoldingTypeId"",
i.title AS ""Instance"",
h2.instance_id AS ""InstanceId"",
l.name AS ""Location"",
h2.permanent_location_id AS ""LocationId"",
tl.name AS ""TemporaryLocation"",
h2.temporary_location_id AS ""TemporaryLocationId"",
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
i2.hrid AS ""ShortId"",
i2.match_key AS ""MatchKey"",
i2.source AS ""Source"",
i2.title AS ""Title"",
i2.author AS ""Author"",
i2.publication_year AS ""PublicationYear"",
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
i2.chk_subscription_overlap AS ""ChkSubscriptionOverlap"",
i2.currency AS ""Currency"",
i2.enclosure_needed AS ""EnclosureNeeded"",
i2.exchange_rate AS ""ExchangeRate"",
i2.export_to_accounting AS ""ExportToAccounting"",
i2.folio_invoice_no AS ""Number"",
i2.invoice_date AS ""InvoiceDate"",
i2.lock_total AS ""LockTotal"",
i2.note AS ""Note"",
i2.payment_due AS ""PaymentDue"",
i2.payment_terms AS ""PaymentTerms"",
i2.payment_method AS ""PaymentMethod"",
i2.status AS ""Status"",
i2.source AS ""Source"",
i2.sub_total AS ""SubTotal"",
i2.total AS ""Total"",
i2.vendor_invoice_no AS ""VendorInvoiceNo"",
i2.disbursement_number AS ""DisbursementNumber"",
i2.voucher_number AS ""VoucherNumber"",
p.amount AS ""Payment"",
i2.payment_id AS ""PaymentId"",
i2.disbursement_date AS ""DisbursementDate"",
v.name AS ""Vendor"",
i2.vendor_id AS ""VendorId"",
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
ii2.subscription_start AS ""SubscriptionStart"",
ii2.subscription_end AS ""SubscriptionEnd"",
ii2.sub_total AS ""SubTotal"",
ii2.total AS ""Total"",
ii2.vendor_ref_no AS ""VendorRefNo"",
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
i2.hrid AS ""ShortId"",
h.hrid AS ""Holding"",
i2.holding_id AS ""HoldingId"",
i2.discovery_suppress AS ""DiscoverySuppress"",
i2.accession_number AS ""AccessionNumber"",
i2.barcode AS ""Barcode"",
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
o2.po_number_prefix AS ""NumberPrefix"",
o2.po_number_suffix AS ""NumberSuffix"",
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
o2.workflow_status AS ""WorkflowStatus"",
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
oi2.cost_quantity_physical AS ""PhysicalQuantity"",
oi2.cost_quantity_electronic AS ""ElectronicQuantity"",
oi2.cost_po_line_estimated_price AS ""EstimatedPrice"",
oi2.description AS ""Description"",
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
oi2.po_line_description AS ""Description2"",
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
oi2.vendor_detail_ref_number AS ""VendorReferenceNumber"",
oi2.vendor_detail_ref_number_type AS ""VendorReferenceNumberType"",
oi2.vendor_detail_vendor_account AS ""VendorAccount"",
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
o2.erp_code AS ""ErpCode"",
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
o2.desc AS ""Desc"",
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
p2.created_at AS ""CreatedAt"",
p2.source AS ""Source"",
p2.payment_method AS ""PaymentMethod"",
f.title AS ""Fee"",
p2.fee_id AS ""FeeId"",
u.username AS ""User"",
p2.user_id AS ""UserId"",
p2.content AS ""Content"" 
FROM uc.payments AS p2
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
ta2.desc AS ""Desc"",
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
v2.amount AS ""Amount"",
bg.name AS ""BatchGroup"",
v2.batch_group_id AS ""BatchGroupId"",
v2.disbursement_number AS ""DisbursementNumber"",
v2.disbursement_date AS ""DisbursementDate"",
v2.disbursement_amount AS ""DisbursementAmount"",
v2.invoice_currency AS ""InvoiceCurrency"",
i.folio_invoice_no AS ""Invoice"",
v2.invoice_id AS ""InvoiceId"",
v2.exchange_rate AS ""ExchangeRate"",
v2.export_to_accounting AS ""ExportToAccounting"",
v2.status AS ""Status"",
v2.system_currency AS ""SystemCurrency"",
v2.type AS ""Type"",
v2.voucher_date AS ""VoucherDate"",
v2.voucher_number AS ""VoucherNumber"",
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
LEFT JOIN uc.users AS cu ON cu.id = v2.created_by_user_id
LEFT JOIN uc.users AS lwu ON lwu.id = v2.updated_by_user_id
 ORDER BY v2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
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
vi2.external_account_number AS ""ExternalAccountNumber"",
st.amount AS ""SubTransaction"",
vi2.sub_transaction_id AS ""SubTransactionId"",
v.id AS ""Voucher"",
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
 ORDER BY vi2.id
", take: 1);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sQueryTest()\r\n    ElapsedTime={s.Elapsed}");
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
