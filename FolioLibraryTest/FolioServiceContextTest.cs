using FolioLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace FolioLibraryTest
{
    [TestClass]
    public class FolioServiceContextTest
    {
        private readonly static FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioLibraryTest", SourceLevels.Information);

        [TestMethod]
        public void QueryAcquisitionMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.AcquisitionMethod2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAcquisitionMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.AcquisitionsUnit2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryActualCostRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ActualCostRecord2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryActualCostRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAddressType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.AddressType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAddressType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreement2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Agreement2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAgreement2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAgreementItem2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.AgreementItem2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAgreementItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryAlternativeTitleType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.AlternativeTitleType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryAlternativeTitleType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBatchGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BatchGroup2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBatchGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlock2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Block2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBlock2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockCondition2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BlockCondition2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBlockCondition2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBlockLimit2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BlockLimit2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBlockLimit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBoundWithPart2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BoundWithPart2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBoundWithPart2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Budget2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBudget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BudgetExpenseClass2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBudgetExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryBudgetGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.BudgetGroup2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryBudgetGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCallNumberType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.CallNumberType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCallNumberType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCampus2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Campus2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCampus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCancellationReason2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.CancellationReason2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCancellationReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCategory2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Category2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCategory2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCheckIn2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.CheckIn2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCheckIn2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryClassificationType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ClassificationType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryClassificationType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCloseReason2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.CloseReason2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCloseReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryComment2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Comment2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryComment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryConfiguration2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Configuration2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryConfiguration2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContact2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Contact2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryContact2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorNameType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ContributorNameType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryContributorNameType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryContributorType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ContributorType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryContributorType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryCustomField2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.CustomField2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryCustomField2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDateType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.DateType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryDateType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDepartment2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Department2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryDepartment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryDocument2sTest()
        {
            var s = Stopwatch.StartNew();
            Assert.Inconclusive();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryDocument2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryElectronicAccessRelationship2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ElectronicAccessRelationship2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryElectronicAccessRelationship2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryExpenseClass2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ExpenseClass2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryExpenseClass2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFee2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Fee2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFee2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFeeType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.FeeType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFeeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFinanceGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.FinanceGroup2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFinanceGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFiscalYear2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.FiscalYear2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFiscalYear2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFixedDueDateSchedule2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.FixedDueDateSchedule2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFixedDueDateSchedule2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFormatsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Formats(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFund2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Fund2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFund2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryFundType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.FundType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryFundType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryGroup2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Group2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryGroup2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHolding2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Holding2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryHolding2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.HoldingNoteType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryHoldingNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryHoldingType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.HoldingType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryHoldingType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIdType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.IdType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryIdType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIllPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.IllPolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryIllPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstance2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Instance2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInstance2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.InstanceNoteType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInstanceNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstanceType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.InstanceType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInstanceType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInstitution2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Institution2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInstitution2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInterface2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Interface2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInterface2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Invoice2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInvoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryInvoiceItem2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.InvoiceItem2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryInvoiceItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryIssuanceModesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.IssuanceModes(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryIssuanceModesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItem2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Item2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemDamagedStatus2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ItemDamagedStatus2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryItemDamagedStatus2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryItemNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ItemNoteType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryItemNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLedger2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Ledger2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLedger2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLibrary2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Library2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLibrary2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoan2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Loan2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLoan2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.LoanPolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLoanPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLoanType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.LoanType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLoanType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocation2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Location2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLocation2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLocationSettingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.LocationSettings(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLocationSettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryLostItemFeePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.LostItemFeePolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryLostItemFeePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryManualBlockTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ManualBlockTemplate2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryManualBlockTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryMaterialType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.MaterialType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryMaterialType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNatureOfContentTerm2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.NatureOfContentTerm2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryNatureOfContentTerm2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNote2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Note2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryNote2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryNoteType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.NoteType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryNoteType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrder2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Order2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOrder2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderInvoice2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.OrderInvoice2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOrderInvoice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrderItem2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.OrderItem2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOrderItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganization2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Organization2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOrganization2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOrganizationType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.OrganizationType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOrganizationType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOverdueFinePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.OverdueFinePolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOverdueFinePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryOwner2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Owner2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryOwner2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronActionSession2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.PatronActionSession2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPatronActionSession2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPatronNoticePolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.PatronNoticePolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPatronNoticePolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPayment2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Payment2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPayment2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPaymentMethod2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.PaymentMethod2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPaymentMethod2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermission2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Permission2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPermission2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPermissionsUser2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.PermissionsUser2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPermissionsUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrecedingSucceedingTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.PrecedingSucceedingTitle2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPrecedingSucceedingTitle2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryPrintersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Printers(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryPrintersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryProxy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Proxy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryProxy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReceiving2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Receiving2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryReceiving2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRecord2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Record2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRecord2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryReferenceData2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ReferenceData2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryReferenceData2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRefundReason2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RefundReason2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRefundReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Relationships(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RelationshipTypes(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequest2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Request2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRequest2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRequestPolicy2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RequestPolicy2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRequestPolicy2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRollover2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Rollover2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRollover2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverBudget2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RolloverBudget2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRolloverBudget2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverError2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RolloverError2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRolloverError2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryRolloverProgress2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.RolloverProgress2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryRolloverProgress2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryScheduledNotice2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ScheduledNotice2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryScheduledNotice2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePoint2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ServicePoint2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryServicePoint2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryServicePointUser2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.ServicePointUser2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryServicePointUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySettingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Settings(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QuerySettingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySnapshot2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Snapshot2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QuerySnapshot2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySource2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Source2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QuerySource2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStaffSlip2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.StaffSlip2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryStaffSlip2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCode2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.StatisticalCode2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryStatisticalCode2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatisticalCodeType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.StatisticalCodeType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryStatisticalCodeType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Statuses(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySubjectSource2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.SubjectSource2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QuerySubjectSource2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QuerySubjectType2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.SubjectType2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QuerySubjectType2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTag2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Tag2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTag2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTemplate2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Template2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTemplate2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTitle2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Title2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTitle2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransaction2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Transaction2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTransaction2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferAccount2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.TransferAccount2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTransferAccount2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryTransferCriteria2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.TransferCriteria2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryTransferCriteria2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUser2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.User2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryUser2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserAcquisitionsUnit2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.UserAcquisitionsUnit2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryUserAcquisitionsUnit2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryUserRequestPreference2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.UserRequestPreference2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryUserRequestPreference2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucher2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.Voucher2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryVoucher2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryVoucherItem2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.VoucherItem2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryVoucherItem2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void QueryWaiveReason2sTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceContext.WaiveReason2s(take: 1, cache: false).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"QueryWaiveReason2sTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioServiceContext.Dispose();
        }
    }
}
