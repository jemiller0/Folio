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
            TraceConfiguration.Register();
        }

        [TestMethod]
        public void QueryAcquisitionMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AcquisitionMethod2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAcquisitionMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAcquisitionMethod2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryActualCostRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ActualCostRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryActualCostRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridActualCostRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryActualCostRecordContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ActualCostRecordContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecordContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryActualCostRecordContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridActualCostRecordContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecordContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryActualCostRecordIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ActualCostRecordIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecordIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryActualCostRecordIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridActualCostRecordIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecordIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Addresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAddressType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AddressType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAddressType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAddressType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreement2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Agreement2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Agreement2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAgreement2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAgreement2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Agreement2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreementItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AgreementItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAgreementItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAgreementItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreementItemOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AgreementItemOrderItems(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItemOrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAgreementItemOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAgreementItemOrderItems(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItemOrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreementOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AgreementOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAgreementOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAgreementOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreementPeriodsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AgreementPeriods(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementPeriodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAgreementPeriodsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAgreementPeriods(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementPeriodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlert2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Alert2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Alert2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAlert2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAlert2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Alert2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAllocatedFromFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AllocatedFromFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedFromFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAllocatedFromFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAllocatedFromFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedFromFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAllocatedToFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AllocatedToFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedToFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAllocatedToFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAllocatedToFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AllocatedToFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlternativeTitlesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AlternativeTitles(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAlternativeTitlesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAlternativeTitles(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlternativeTitleType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AlternativeTitleType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAlternativeTitleType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAlternativeTitleType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAuthAttempt2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AuthAttempt2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthAttempt2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAuthAttempt2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAuthAttempt2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthAttempt2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAuthCredentialsHistory2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.AuthCredentialsHistory2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthCredentialsHistory2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryAuthCredentialsHistory2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridAuthCredentialsHistory2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AuthCredentialsHistory2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BatchGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBatchGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBatchGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlock2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Block2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBlock2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBlock2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockCondition2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BlockCondition2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBlockCondition2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBlockCondition2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockLimit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BlockLimit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBlockLimit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBlockLimit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBoundWithPart2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BoundWithPart2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPart2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBoundWithPart2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBoundWithPart2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPart2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Budget2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBudget2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BudgetAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBudgetAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBudgetAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BudgetExpenseClass2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBudgetExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBudgetExpenseClass2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BudgetGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBudgetGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBudgetGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.BudgetTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryBudgetTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridBudgetTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCallNumberType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CallNumberType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCallNumberType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCallNumberType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCampus2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Campus2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCampus2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCampus2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCancellationReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CancellationReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCancellationReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCancellationReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCategory2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Category2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCategory2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCategory2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCheckIn2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CheckIn2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCheckIn2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCheckIn2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCirculationNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CirculationNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCirculationNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCirculationNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCirculationRule2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CirculationRule2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationRule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCirculationRule2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCirculationRule2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CirculationRule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryClassificationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Classifications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryClassificationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridClassifications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryClassificationType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ClassificationType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryClassificationType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridClassificationType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCloseReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CloseReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCloseReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCloseReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryComment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Comment2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryComment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridComment2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryConfiguration2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Configuration2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryConfiguration2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridConfiguration2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContact2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Contact2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContact2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContact2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactAddressCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactAddressCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactEmailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactEmails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactEmailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactEmails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactEmailCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactEmailCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactPhoneNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactPhoneNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactPhoneNumberCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactPhoneNumberCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactUrlsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactUrls(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactUrlsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactUrls(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContactUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContactUrlCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContactUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContactUrlCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Contributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorNameType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContributorNameType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContributorNameType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContributorNameType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ContributorType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryContributorType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridContributorType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCountriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Countries(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCountriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCountries(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCurrenciesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Currencies(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CurrenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCurrenciesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCurrencies(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CurrenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCustomField2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CustomField2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCustomField2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCustomField2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCustomFieldValuesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.CustomFieldValues(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldValuesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryCustomFieldValuesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridCustomFieldValues(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldValuesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDepartment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Department2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryDepartment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridDepartment2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryEditionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Editions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryEditionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridEditions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryElectronicAccessRelationship2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ElectronicAccessRelationship2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryElectronicAccessRelationship2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridElectronicAccessRelationship2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryErrorRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ErrorRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ErrorRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryErrorRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridErrorRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ErrorRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryEventLog2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.EventLog2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EventLog2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryEventLog2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridEventLog2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"EventLog2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ExpenseClass2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridExpenseClass2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExtentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Extents(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExtentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryExtentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridExtents(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExtentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFee2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Fee2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFee2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFee2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFeeType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FeeType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFeeType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFeeType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFinanceGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FinanceGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFinanceGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFinanceGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFinanceGroupAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FinanceGroupAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFinanceGroupAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFinanceGroupAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFiscalYear2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FiscalYear2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFiscalYear2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFiscalYear2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFiscalYearAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FiscalYearAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFiscalYearAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFiscalYearAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFixedDueDateSchedule2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FixedDueDateSchedule2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFixedDueDateSchedule2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFixedDueDateSchedule2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFixedDueDateScheduleSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FixedDueDateScheduleSchedules(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateScheduleSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFixedDueDateScheduleSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFixedDueDateScheduleSchedules(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateScheduleSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFormatsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Formats(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFormatsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFormats(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFund2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Fund2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFund2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFund2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FundAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFundAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFundAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FundLocation2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFundLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFundLocation2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FundOrganization2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundOrganization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFundOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFundOrganization2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundOrganization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FundTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFundTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFundTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.FundType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryFundType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridFundType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Group2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridGroup2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHolding2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Holding2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHolding2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHolding2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingAdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingAdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingAdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingAdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingDonorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingDonors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingDonorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingDonorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingDonors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingDonorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingEntriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingEntries(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingEntriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingEntriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingEntries(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingEntriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingFormerIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingFormerIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HoldingType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHoldingType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHoldingType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHridSetting2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.HridSetting2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HridSetting2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryHridSetting2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridHridSetting2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HridSetting2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Identifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIdType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.IdType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIdType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIdType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIllPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.IllPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIllPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIllPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIndexStatementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.IndexStatements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IndexStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIndexStatementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIndexStatements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IndexStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstance2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Instance2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstance2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstance2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceFormat2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceFormat2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormat2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceFormat2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceFormat2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormat2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceNatureOfContentTerms(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceNatureOfContentTerms(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InstanceType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstanceType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstanceType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstitution2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Institution2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInstitution2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInstitution2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterface2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Interface2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInterface2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInterface2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterfaceCredential2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InterfaceCredential2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceCredential2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInterfaceCredential2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInterfaceCredential2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceCredential2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterfaceTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InterfaceTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInterfaceTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInterfaceTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfaceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Invoice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceAdjustments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceAdjustments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceAdjustmentFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceAdjustmentFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItemAdjustments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItemAdjustmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItemAdjustments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItemAdjustmentFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItemAdjustmentFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItemAdjustmentFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemAdjustmentFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItemReferenceNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItemReferenceNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceOrderNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceOrderNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceOrderNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceOrderNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceOrderNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceOrderNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.InvoiceTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryInvoiceTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridInvoiceTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIsbnsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Isbns(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IsbnsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIsbnsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIsbns(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IsbnsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIssnsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Issns(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssnsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIssnsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIssns(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssnsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIssuanceModesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.IssuanceModes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryIssuanceModesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridIssuanceModes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Item2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemAdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemAdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemAdministrativeNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemAdministrativeNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemAdministrativeNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemDamagedStatus2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemDamagedStatus2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemDamagedStatus2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemDamagedStatus2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemDonorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemDonors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDonorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemDonorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemDonors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDonorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemElectronicAccessesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemElectronicAccesses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemElectronicAccessesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemFormerIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemFormerIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemFormerIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemFormerIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemNoteType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemStatisticalCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemYearCaptionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ItemYearCaptions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemYearCaptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryItemYearCaptionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridItemYearCaptions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemYearCaptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLanguagesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Languages(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LanguagesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLanguagesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLanguages(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LanguagesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedger2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Ledger2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLedger2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLedger2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedgerAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LedgerAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLedgerAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLedgerAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLibrary2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Library2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLibrary2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLibrary2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoan2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Loan2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLoan2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLoan2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanEvent2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LoanEvent2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanEvent2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLoanEvent2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLoanEvent2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanEvent2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LoanPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLoanPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLoanPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LoanType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLoanType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLoanType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Location2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLocation2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocationServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LocationServicePoints(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLocationServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLocationServicePoints(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocationSettingsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LocationSettings(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLocationSettingsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLocationSettings(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLogin2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Login2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Login2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLogin2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLogin2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Login2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLostItemFeePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.LostItemFeePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryLostItemFeePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridLostItemFeePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryManualBlockTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ManualBlockTemplate2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryManualBlockTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridManualBlockTemplate2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryMarcRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.MarcRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MarcRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryMarcRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridMarcRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MarcRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryMaterialType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.MaterialType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryMaterialType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridMaterialType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNatureOfContentTerm2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.NatureOfContentTerm2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryNatureOfContentTerm2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridNatureOfContentTerm2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOclcNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OclcNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OclcNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOclcNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOclcNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OclcNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrder2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Order2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrder2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrder2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderInvoice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderInvoice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemAlertsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemAlerts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemAlertsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemAlertsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemAlerts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemAlertsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemClaimsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemClaims(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemClaimsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemClaimsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemClaims(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemClaimsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemLocation2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemLocation2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemProductIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemProductIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemReferenceNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemReferenceNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemReferenceNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReferenceNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemReportingCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemReportingCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReportingCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemReportingCodesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemReportingCodes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemReportingCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemSearchLocationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemSearchLocations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemSearchLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemSearchLocationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemSearchLocations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemSearchLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItemVolumesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderItemVolumes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemVolumesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderItemVolumesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderItemVolumes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemVolumesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderTemplate2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderTemplate2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrderTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrderTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrderTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Organization2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganization2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAccountsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAccounts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAccountsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAccounts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAccountAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAccountAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAccountAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAccountAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAccountAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAddressCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAddressCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAddressCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAddressCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAgreementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAgreements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAgreementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAgreements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAgreementOrgsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAgreementOrgs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementOrgsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAgreementOrgsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAgreementOrgs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementOrgsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAgreementPeriodsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAgreementPeriods(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementPeriodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAgreementPeriodsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAgreementPeriods(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAgreementPeriodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationAliasesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationAliases(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAliasesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationAliasesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationAliases(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationAliasesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationChangelogsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationChangelogs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationChangelogsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationChangelogsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationChangelogs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationChangelogsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationContactsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationContacts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationContactsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationContacts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationEmailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationEmails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationEmailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationEmails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationEmailCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationEmailCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationEmailCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationEmailCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationInterfaces(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationInterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationInterfaces(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationInterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationPhoneNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationPhoneNumbersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationPhoneNumbers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumbersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationPhoneNumberCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationPhoneNumberCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationPhoneNumberCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPhoneNumberCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationPrivilegedContactsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationPrivilegedContacts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPrivilegedContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationPrivilegedContactsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationPrivilegedContacts(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationPrivilegedContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationUrlsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationUrls(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationUrlsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationUrls(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OrganizationUrlCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOrganizationUrlCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOrganizationUrlCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationUrlCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOverdueFinePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.OverdueFinePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOverdueFinePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOverdueFinePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOwner2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Owner2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryOwner2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridOwner2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronActionSession2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PatronActionSession2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPatronActionSession2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPatronActionSession2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PatronNoticePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPatronNoticePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPatronNoticePolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyFeeFineNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PatronNoticePolicyFeeFineNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyFeeFineNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPatronNoticePolicyFeeFineNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPatronNoticePolicyFeeFineNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyFeeFineNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyLoanNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PatronNoticePolicyLoanNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyLoanNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPatronNoticePolicyLoanNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPatronNoticePolicyLoanNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyLoanNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicyRequestNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PatronNoticePolicyRequestNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyRequestNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPatronNoticePolicyRequestNoticesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPatronNoticePolicyRequestNotices(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicyRequestNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPayment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Payment2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPayment2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPayment2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPaymentMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PaymentMethod2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPaymentMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPaymentMethod2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPaymentTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PaymentTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPaymentTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPaymentTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermission2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Permission2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermission2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermission2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionChildOfsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionChildOfs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionChildOfsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionChildOfsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionChildOfs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionChildOfsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionGrantedTosTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionGrantedTos(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionGrantedTosTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionGrantedTosTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionGrantedTos(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionGrantedTosTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionSubPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionSubPermissions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionSubPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionSubPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionSubPermissions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionSubPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionsUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionsUser2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionsUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionsUser2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionsUserPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionsUserPermissions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUserPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionsUserPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionsUserPermissions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUserPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PermissionTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPermissionTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPermissionTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPhysicalDescriptionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PhysicalDescriptions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PhysicalDescriptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPhysicalDescriptionsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPhysicalDescriptions(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PhysicalDescriptionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrecedingSucceedingTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PrecedingSucceedingTitle2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPrecedingSucceedingTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPrecedingSucceedingTitle2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrecedingSucceedingTitleIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PrecedingSucceedingTitleIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitleIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPrecedingSucceedingTitleIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPrecedingSucceedingTitleIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitleIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPreferredEmailCommunicationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PreferredEmailCommunications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PreferredEmailCommunicationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPreferredEmailCommunicationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPreferredEmailCommunications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PreferredEmailCommunicationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrefix2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Prefix2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Prefix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPrefix2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPrefix2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Prefix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrintersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Printers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPrintersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPrinters(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryProxy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Proxy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryProxy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridProxy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Publications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPublicationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPublications(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationFrequenciesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PublicationFrequencies(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationFrequenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPublicationFrequenciesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPublicationFrequencies(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationFrequenciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPublicationRangesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.PublicationRanges(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationRangesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryPublicationRangesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridPublicationRanges(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PublicationRangesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRawRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RawRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RawRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRawRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRawRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RawRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReceiptStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ReceiptStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReceiptStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryReceiptStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridReceiptStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReceiptStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReceiving2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Receiving2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryReceiving2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridReceiving2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Record2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRecord2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReferenceData2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ReferenceData2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReferenceData2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryReferenceData2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridReferenceData2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReferenceData2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRefundReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RefundReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRefundReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRefundReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Relationships(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRelationships(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RelationshipTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRelationshipTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReportingCode2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ReportingCode2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryReportingCode2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridReportingCode2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequest2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Request2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequest2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequest2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RequestIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequestIdentifiersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequestIdentifiers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestIdentifiersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RequestNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequestNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequestNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RequestPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequestPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequestPolicy2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestPolicyRequestTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RequestPolicyRequestTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicyRequestTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequestPolicyRequestTypesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequestPolicyRequestTypes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicyRequestTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RequestTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRequestTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRequestTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRollover2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Rollover2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Rollover2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRollover2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRollover2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Rollover2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudget2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudget2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetAcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetAcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetAllocatedFromNamesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetAllocatedFromNames(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAllocatedFromNamesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetAllocatedFromNamesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetAllocatedFromNames(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAllocatedFromNamesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetAllocatedToNamesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetAllocatedToNames(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAllocatedToNamesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetAllocatedToNamesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetAllocatedToNames(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetAllocatedToNamesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetExpenseClassDetailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetExpenseClassDetails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetExpenseClassDetailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetExpenseClassDetailsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetExpenseClassDetails(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetExpenseClassDetailsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetFromFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetFromFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetFromFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetFromFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetFromFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetFromFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetLocationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetLocations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetLocationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetLocations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetOrganizations(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetsRolloversTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetsRollovers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetsRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetsRolloversTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetsRollovers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetsRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudgetToFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverBudgetToFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetToFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverBudgetToFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverBudgetToFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetToFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverEncumbrancesRolloversTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverEncumbrancesRollovers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverEncumbrancesRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverEncumbrancesRolloversTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverEncumbrancesRollovers(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverEncumbrancesRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverError2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverError2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverError2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverError2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverError2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverError2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverProgress2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.RolloverProgress2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverProgress2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryRolloverProgress2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridRolloverProgress2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverProgress2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryScheduledNotice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ScheduledNotice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryScheduledNotice2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridScheduledNotice2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySeriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Series(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SeriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySeriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSeries(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SeriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePoint2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ServicePoint2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryServicePoint2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridServicePoint2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointOwnersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ServicePointOwners(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointOwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryServicePointOwnersTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridServicePointOwners(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointOwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ServicePointStaffSlips(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointStaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryServicePointStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridServicePointStaffSlips(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointStaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ServicePointUser2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryServicePointUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridServicePointUser2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointUserServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.ServicePointUserServicePoints(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUserServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryServicePointUserServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridServicePointUserServicePoints(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUserServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySettingsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Settings(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySettingsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSettings(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySnapshot2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Snapshot2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySnapshot2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSnapshot2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySource2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Source2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySource2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSource2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySourceMarcsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.SourceMarcs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySourceMarcsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSourceMarcs(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySourceMarcFieldsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.SourceMarcFields(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySourceMarcFieldsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSourceMarcFields(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourceMarcFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStaffSlip2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.StaffSlip2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryStaffSlip2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridStaffSlip2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCode2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.StatisticalCode2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryStatisticalCode2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridStatisticalCode2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCodeType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.StatisticalCodeType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryStatisticalCodeType2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridStatisticalCodeType2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Statuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySubjectsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Subjects(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySubjectsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSubjects(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySuffix2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Suffix2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Suffix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySuffix2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSuffix2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Suffix2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySupplementStatementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.SupplementStatements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SupplementStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQuerySupplementStatementsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridSupplementStatements(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SupplementStatementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Template2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTemplate2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTemplateOutputFormatsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TemplateOutputFormats(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplateOutputFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTemplateOutputFormatsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTemplateOutputFormats(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplateOutputFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Title2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTitle2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TitleAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTitleAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTitleAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleBindItemIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TitleBindItemIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleBindItemIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTitleBindItemIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTitleBindItemIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleBindItemIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TitleContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTitleContributorsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTitleContributors(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleContributorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitleProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TitleProductIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTitleProductIdsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTitleProductIds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitleProductIdsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransaction2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Transaction2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTransaction2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTransaction2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransactionTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TransactionTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTransactionTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTransactionTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferAccount2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TransferAccount2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTransferAccount2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTransferAccount2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferCriteria2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.TransferCriteria2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryTransferCriteria2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridTransferCriteria2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.User2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUser2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUser2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserAcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserAcquisitionsUnit2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserAddressesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserAddresses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAddressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserCategories(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserDepartments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserDepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserDepartments(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserDepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserNotesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserNotes(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserRequestPreference2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserRequestPreference2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserRequestPreference2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserRequestPreference2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummary2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserSummary2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserSummary2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserSummary2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummaryOpenFeesFinesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserSummaryOpenFeesFines(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenFeesFinesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserSummaryOpenFeesFinesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserSummaryOpenFeesFines(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenFeesFinesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserSummaryOpenLoansTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserSummaryOpenLoans(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenLoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserSummaryOpenLoansTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserSummaryOpenLoans(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserSummaryOpenLoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.UserTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryUserTagsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridUserTags(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucher2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.Voucher2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucher2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucher2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.VoucherAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucherAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucherAcquisitionsUnits(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.VoucherItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucherItem2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucherItem2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.VoucherItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucherItemFundsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucherItemFunds(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItemInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.VoucherItemInvoiceItems(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemInvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucherItemInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucherItemInvoiceItems(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemInvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.VoucherStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryVoucherStatusesTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridVoucherStatuses(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryWaiveReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.WaiveReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void GridQueryWaiveReason2sTest()
        {
            var s = Stopwatch.StartNew();
            using (var fdc = new FolioDapperContext())
                fdc.GridWaiveReason2s(take: 0).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioDapperContext.Dispose();
        }
    }
}
