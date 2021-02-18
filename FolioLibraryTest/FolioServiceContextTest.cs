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
        private readonly static FolioDapperContext folioDapperContext = new FolioDapperContext();
        private readonly static FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioLibraryTest", SourceLevels.Information);
        private readonly static int? take = 100_000;

        [TestMethod]
        public void Orders_DeserializeAcquisitionsUnit2Test()
        {
            var s = Stopwatch.StartNew();
            var au2 = folioServiceContext.AcquisitionsUnit2s(take: 1).SingleOrDefault();
            if (au2 == null) Assert.Inconclusive();
            var au3 = folioDapperContext.AcquisitionsUnit2s(take: 1).SingleOrDefault();
            au3.Content = null;
            Assert.AreEqual(au2.ToString(), au3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeAcquisitionsUnit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeAddressType2Test()
        {
            var s = Stopwatch.StartNew();
            var at2 = folioServiceContext.AddressType2s(take: 1).SingleOrDefault();
            if (at2 == null) Assert.Inconclusive();
            var at3 = folioDapperContext.AddressType2s(take: 1).SingleOrDefault();
            at3.Content = null;
            Assert.AreEqual(at2.ToString(), at3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeAddressType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeAlert2Test()
        {
            var s = Stopwatch.StartNew();
            var a2 = folioServiceContext.Alert2s(take: 1).SingleOrDefault();
            if (a2 == null) Assert.Inconclusive();
            var a3 = folioDapperContext.Alert2s(take: 1).SingleOrDefault();
            a3.Content = null;
            Assert.AreEqual(a2.ToString(), a3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeAlert2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeAlternativeTitleType2Test()
        {
            var s = Stopwatch.StartNew();
            var att2 = folioServiceContext.AlternativeTitleType2s(take: 1).SingleOrDefault();
            if (att2 == null) Assert.Inconclusive();
            var att3 = folioDapperContext.AlternativeTitleType2s(take: 1).SingleOrDefault();
            att3.Content = null;
            Assert.AreEqual(att2.ToString(), att3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeAlternativeTitleType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeBatchGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var bg2 = folioServiceContext.BatchGroup2s(take: 1).SingleOrDefault();
            if (bg2 == null) Assert.Inconclusive();
            var bg3 = folioDapperContext.BatchGroup2s(take: 1).SingleOrDefault();
            bg3.Content = null;
            Assert.AreEqual(bg2.ToString(), bg3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeBatchGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeBatchVoucherExport2Test()
        {
            var s = Stopwatch.StartNew();
            var bve2 = folioServiceContext.BatchVoucherExport2s(take: 1).SingleOrDefault();
            if (bve2 == null) Assert.Inconclusive();
            var bve3 = folioDapperContext.BatchVoucherExport2s(take: 1).SingleOrDefault();
            bve3.Content = null;
            Assert.AreEqual(bve2.ToString(), bve3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeBatchVoucherExport2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeBatchVoucherExportConfig2Test()
        {
            var s = Stopwatch.StartNew();
            var bvec2 = folioServiceContext.BatchVoucherExportConfig2s(take: 1).SingleOrDefault();
            if (bvec2 == null) Assert.Inconclusive();
            bvec2.BatchVoucherExportConfigWeekdays = null;
            var bvec3 = folioDapperContext.BatchVoucherExportConfig2s(take: 1).SingleOrDefault();
            bvec3.Content = null;
            Assert.AreEqual(bvec2.ToString(), bvec3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeBatchVoucherExportConfig2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeBlock2Test()
        {
            var s = Stopwatch.StartNew();
            var b2 = folioServiceContext.Block2s(take: 1).SingleOrDefault();
            if (b2 == null) Assert.Inconclusive();
            var b3 = folioDapperContext.Block2s(take: 1).SingleOrDefault();
            b3.Content = null;
            Assert.AreEqual(b2.ToString(), b3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeBlock2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeBlockCondition2Test()
        {
            var s = Stopwatch.StartNew();
            var bc2 = folioServiceContext.BlockCondition2s(take: 1).SingleOrDefault();
            if (bc2 == null) Assert.Inconclusive();
            var bc3 = folioDapperContext.BlockCondition2s(take: 1).SingleOrDefault();
            bc3.Content = null;
            Assert.AreEqual(bc2.ToString(), bc3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeBlockCondition2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeBlockLimit2Test()
        {
            var s = Stopwatch.StartNew();
            var bl2 = folioServiceContext.BlockLimit2s(take: 1).SingleOrDefault();
            if (bl2 == null) Assert.Inconclusive();
            var bl3 = folioDapperContext.BlockLimit2s(take: 1).SingleOrDefault();
            bl3.Content = null;
            Assert.AreEqual(bl2.ToString(), bl3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeBlockLimit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeBudget2Test()
        {
            var s = Stopwatch.StartNew();
            var b2 = folioServiceContext.Budget2s(take: 1).SingleOrDefault();
            if (b2 == null) Assert.Inconclusive();
            b2.BudgetAcquisitionsUnits = null;
            b2.BudgetTags = null;
            var b3 = folioDapperContext.Budget2s(take: 1).SingleOrDefault();
            b3.Content = null;
            Assert.AreEqual(b2.ToString(), b3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeBudget2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeBudgetExpenseClass2Test()
        {
            var s = Stopwatch.StartNew();
            var bec2 = folioServiceContext.BudgetExpenseClass2s(take: 1).SingleOrDefault();
            if (bec2 == null) Assert.Inconclusive();
            var bec3 = folioDapperContext.BudgetExpenseClass2s(take: 1).SingleOrDefault();
            bec3.Content = null;
            Assert.AreEqual(bec2.ToString(), bec3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeBudgetExpenseClass2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeCallNumberType2Test()
        {
            var s = Stopwatch.StartNew();
            var cnt2 = folioServiceContext.CallNumberType2s(take: 1).SingleOrDefault();
            if (cnt2 == null) Assert.Inconclusive();
            var cnt3 = folioDapperContext.CallNumberType2s(take: 1).SingleOrDefault();
            cnt3.Content = null;
            Assert.AreEqual(cnt2.ToString(), cnt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeCallNumberType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeCampus2Test()
        {
            var s = Stopwatch.StartNew();
            var c2 = folioServiceContext.Campus2s(take: 1).SingleOrDefault();
            if (c2 == null) Assert.Inconclusive();
            var c3 = folioDapperContext.Campus2s(take: 1).SingleOrDefault();
            c3.Content = null;
            Assert.AreEqual(c2.ToString(), c3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeCampus2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeCancellationReason2Test()
        {
            var s = Stopwatch.StartNew();
            var cr2 = folioServiceContext.CancellationReason2s(take: 1).SingleOrDefault();
            if (cr2 == null) Assert.Inconclusive();
            var cr3 = folioDapperContext.CancellationReason2s(take: 1).SingleOrDefault();
            cr3.Content = null;
            Assert.AreEqual(cr2.ToString(), cr3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeCancellationReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_DeserializeCategory2Test()
        {
            var s = Stopwatch.StartNew();
            var c2 = folioServiceContext.Category2s(take: 1).SingleOrDefault();
            if (c2 == null) Assert.Inconclusive();
            var c3 = folioDapperContext.Category2s(take: 1).SingleOrDefault();
            c3.Content = null;
            Assert.AreEqual(c2.ToString(), c3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organizations_DeserializeCategory2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeCheckIn2Test()
        {
            var s = Stopwatch.StartNew();
            var ci2 = folioServiceContext.CheckIn2s(take: 1).SingleOrDefault();
            if (ci2 == null) Assert.Inconclusive();
            var ci3 = folioDapperContext.CheckIn2s(take: 1).SingleOrDefault();
            ci3.Content = null;
            Assert.AreEqual(ci2.ToString(), ci3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeCheckIn2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeClassificationType2Test()
        {
            var s = Stopwatch.StartNew();
            var ct2 = folioServiceContext.ClassificationType2s(take: 1).SingleOrDefault();
            if (ct2 == null) Assert.Inconclusive();
            var ct3 = folioDapperContext.ClassificationType2s(take: 1).SingleOrDefault();
            ct3.Content = null;
            Assert.AreEqual(ct2.ToString(), ct3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeClassificationType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeCloseReason2Test()
        {
            var s = Stopwatch.StartNew();
            var cr2 = folioServiceContext.CloseReason2s(take: 1).SingleOrDefault();
            if (cr2 == null) Assert.Inconclusive();
            var cr3 = folioDapperContext.CloseReason2s(take: 1).SingleOrDefault();
            cr3.Content = null;
            Assert.AreEqual(cr2.ToString(), cr3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeCloseReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeComment2Test()
        {
            var s = Stopwatch.StartNew();
            var c2 = folioServiceContext.Comment2s(take: 1).SingleOrDefault();
            if (c2 == null) Assert.Inconclusive();
            var c3 = folioDapperContext.Comment2s(take: 1).SingleOrDefault();
            c3.Content = null;
            Assert.AreEqual(c2.ToString(), c3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeComment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_DeserializeConfiguration2Test()
        {
            var s = Stopwatch.StartNew();
            var c2 = folioServiceContext.Configuration2s(take: 1).SingleOrDefault();
            if (c2 == null) Assert.Inconclusive();
            var c3 = folioDapperContext.Configuration2s(take: 1).SingleOrDefault();
            c3.Content = null;
            Assert.AreEqual(c2.ToString(), c3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration_DeserializeConfiguration2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_DeserializeContact2Test()
        {
            var s = Stopwatch.StartNew();
            var c2 = folioServiceContext.Contact2s(take: 1).SingleOrDefault();
            if (c2 == null) Assert.Inconclusive();
            c2.ContactAddresses = null;
            c2.ContactCategories = null;
            c2.ContactEmails = null;
            c2.ContactPhoneNumbers = null;
            c2.ContactUrls = null;
            var c3 = folioDapperContext.Contact2s(take: 1).SingleOrDefault();
            c3.Content = null;
            Assert.AreEqual(c2.ToString(), c3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organizations_DeserializeContact2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeContributorNameType2Test()
        {
            var s = Stopwatch.StartNew();
            var cnt2 = folioServiceContext.ContributorNameType2s(take: 1).SingleOrDefault();
            if (cnt2 == null) Assert.Inconclusive();
            var cnt3 = folioDapperContext.ContributorNameType2s(take: 1).SingleOrDefault();
            cnt3.Content = null;
            Assert.AreEqual(cnt2.ToString(), cnt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeContributorNameType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeContributorType2Test()
        {
            var s = Stopwatch.StartNew();
            var ct2 = folioServiceContext.ContributorType2s(take: 1).SingleOrDefault();
            if (ct2 == null) Assert.Inconclusive();
            var ct3 = folioDapperContext.ContributorType2s(take: 1).SingleOrDefault();
            ct3.Content = null;
            Assert.AreEqual(ct2.ToString(), ct3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeContributorType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeCustomField2Test()
        {
            var s = Stopwatch.StartNew();
            var cf2 = folioServiceContext.CustomField2s(take: 1).SingleOrDefault();
            if (cf2 == null) Assert.Inconclusive();
            cf2.CustomFieldValues = null;
            var cf3 = folioDapperContext.CustomField2s(take: 1).SingleOrDefault();
            cf3.Content = null;
            Assert.AreEqual(cf2.ToString(), cf3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeCustomField2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeDepartment2Test()
        {
            var s = Stopwatch.StartNew();
            var d2 = folioServiceContext.Department2s(take: 1).SingleOrDefault();
            if (d2 == null) Assert.Inconclusive();
            var d3 = folioDapperContext.Department2s(take: 1).SingleOrDefault();
            d3.Content = null;
            Assert.AreEqual(d2.ToString(), d3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeDepartment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeElectronicAccessRelationship2Test()
        {
            var s = Stopwatch.StartNew();
            var ear2 = folioServiceContext.ElectronicAccessRelationship2s(take: 1).SingleOrDefault();
            if (ear2 == null) Assert.Inconclusive();
            var ear3 = folioDapperContext.ElectronicAccessRelationship2s(take: 1).SingleOrDefault();
            ear3.Content = null;
            Assert.AreEqual(ear2.ToString(), ear3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeElectronicAccessRelationship2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeExpenseClass2Test()
        {
            var s = Stopwatch.StartNew();
            var ec2 = folioServiceContext.ExpenseClass2s(take: 1).SingleOrDefault();
            if (ec2 == null) Assert.Inconclusive();
            var ec3 = folioDapperContext.ExpenseClass2s(take: 1).SingleOrDefault();
            ec3.Content = null;
            Assert.AreEqual(ec2.ToString(), ec3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeExpenseClass2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeFee2Test()
        {
            var s = Stopwatch.StartNew();
            var f2 = folioServiceContext.Fee2s(take: 1).SingleOrDefault();
            if (f2 == null) Assert.Inconclusive();
            var f3 = folioDapperContext.Fee2s(take: 1).SingleOrDefault();
            f3.Content = null;
            Assert.AreEqual(f2.ToString(), f3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeFee2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeFeeType2Test()
        {
            var s = Stopwatch.StartNew();
            var ft2 = folioServiceContext.FeeType2s(take: 1).SingleOrDefault();
            if (ft2 == null) Assert.Inconclusive();
            var ft3 = folioDapperContext.FeeType2s(take: 1).SingleOrDefault();
            ft3.Content = null;
            Assert.AreEqual(ft2.ToString(), ft3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeFeeType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeFinanceGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var fg2 = folioServiceContext.FinanceGroup2s(take: 1).SingleOrDefault();
            if (fg2 == null) Assert.Inconclusive();
            fg2.FinanceGroupAcquisitionsUnits = null;
            var fg3 = folioDapperContext.FinanceGroup2s(take: 1).SingleOrDefault();
            fg3.Content = null;
            Assert.AreEqual(fg2.ToString(), fg3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeFinanceGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeFiscalYear2Test()
        {
            var s = Stopwatch.StartNew();
            var fy2 = folioServiceContext.FiscalYear2s(take: 1).SingleOrDefault();
            if (fy2 == null) Assert.Inconclusive();
            fy2.FiscalYearAcquisitionsUnits = null;
            var fy3 = folioDapperContext.FiscalYear2s(take: 1).SingleOrDefault();
            fy3.Content = null;
            Assert.AreEqual(fy2.ToString(), fy3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeFiscalYear2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeFixedDueDateSchedule2Test()
        {
            var s = Stopwatch.StartNew();
            var fdds2 = folioServiceContext.FixedDueDateSchedule2s(take: 1).SingleOrDefault();
            if (fdds2 == null) Assert.Inconclusive();
            fdds2.FixedDueDateScheduleSchedules = null;
            var fdds3 = folioDapperContext.FixedDueDateSchedule2s(take: 1).SingleOrDefault();
            fdds3.Content = null;
            Assert.AreEqual(fdds2.ToString(), fdds3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeFixedDueDateSchedule2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeFormatTest()
        {
            var s = Stopwatch.StartNew();
            var f = folioServiceContext.Formats(take: 1).SingleOrDefault();
            if (f == null) Assert.Inconclusive();
            var f2 = folioDapperContext.Formats(take: 1).SingleOrDefault();
            f2.Content = null;
            Assert.AreEqual(f.ToString(), f2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeFormatTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeFund2Test()
        {
            var s = Stopwatch.StartNew();
            var f2 = folioServiceContext.Fund2s(take: 1).SingleOrDefault();
            if (f2 == null) Assert.Inconclusive();
            f2.AllocatedFromFunds = null;
            f2.AllocatedToFunds = null;
            f2.FundAcquisitionsUnits = null;
            f2.FundTags = null;
            var f3 = folioDapperContext.Fund2s(take: 1).SingleOrDefault();
            f3.Content = null;
            Assert.AreEqual(f2.ToString(), f3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeFund2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeFundType2Test()
        {
            var s = Stopwatch.StartNew();
            var ft2 = folioServiceContext.FundType2s(take: 1).SingleOrDefault();
            if (ft2 == null) Assert.Inconclusive();
            var ft3 = folioDapperContext.FundType2s(take: 1).SingleOrDefault();
            ft3.Content = null;
            Assert.AreEqual(ft2.ToString(), ft3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeFundType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var g2 = folioServiceContext.Group2s(take: 1).SingleOrDefault();
            if (g2 == null) Assert.Inconclusive();
            var g3 = folioDapperContext.Group2s(take: 1).SingleOrDefault();
            g3.Content = null;
            Assert.AreEqual(g2.ToString(), g3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeGroupFundFiscalYear2Test()
        {
            var s = Stopwatch.StartNew();
            var gffy2 = folioServiceContext.GroupFundFiscalYear2s(take: 1).SingleOrDefault();
            if (gffy2 == null) Assert.Inconclusive();
            var gffy3 = folioDapperContext.GroupFundFiscalYear2s(take: 1).SingleOrDefault();
            gffy3.Content = null;
            Assert.AreEqual(gffy2.ToString(), gffy3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeGroupFundFiscalYear2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeHolding2Test()
        {
            var s = Stopwatch.StartNew();
            var h2 = folioServiceContext.Holding2s(take: 1).SingleOrDefault();
            if (h2 == null) Assert.Inconclusive();
            h2.Extents = null;
            h2.HoldingElectronicAccesses = null;
            h2.HoldingEntries = null;
            h2.HoldingFormerIds = null;
            h2.HoldingNotes = null;
            h2.HoldingStatisticalCodes = null;
            h2.HoldingTags = null;
            h2.IndexStatements = null;
            h2.SupplementStatements = null;
            var h3 = folioDapperContext.Holding2s(take: 1).SingleOrDefault();
            h3.Content = null;
            Assert.AreEqual(h2.ToString(), h3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeHolding2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeHoldingNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var hnt2 = folioServiceContext.HoldingNoteType2s(take: 1).SingleOrDefault();
            if (hnt2 == null) Assert.Inconclusive();
            var hnt3 = folioDapperContext.HoldingNoteType2s(take: 1).SingleOrDefault();
            hnt3.Content = null;
            Assert.AreEqual(hnt2.ToString(), hnt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeHoldingNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeHoldingType2Test()
        {
            var s = Stopwatch.StartNew();
            var ht2 = folioServiceContext.HoldingType2s(take: 1).SingleOrDefault();
            if (ht2 == null) Assert.Inconclusive();
            var ht3 = folioDapperContext.HoldingType2s(take: 1).SingleOrDefault();
            ht3.Content = null;
            Assert.AreEqual(ht2.ToString(), ht3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeHoldingType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeIdType2Test()
        {
            var s = Stopwatch.StartNew();
            var it2 = folioServiceContext.IdType2s(take: 1).SingleOrDefault();
            if (it2 == null) Assert.Inconclusive();
            var it3 = folioDapperContext.IdType2s(take: 1).SingleOrDefault();
            it3.Content = null;
            Assert.AreEqual(it2.ToString(), it3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeIdType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeIllPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var ip2 = folioServiceContext.IllPolicy2s(take: 1).SingleOrDefault();
            if (ip2 == null) Assert.Inconclusive();
            var ip3 = folioDapperContext.IllPolicy2s(take: 1).SingleOrDefault();
            ip3.Content = null;
            Assert.AreEqual(ip2.ToString(), ip3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeIllPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeInstance2Test()
        {
            var s = Stopwatch.StartNew();
            var i2 = folioServiceContext.Instance2s(take: 1).SingleOrDefault();
            if (i2 == null) Assert.Inconclusive();
            i2.AlternativeTitles = null;
            i2.Classifications = null;
            i2.Contributors = null;
            i2.Editions = null;
            i2.ElectronicAccesses = null;
            i2.Identifiers = null;
            i2.InstanceFormat2s = null;
            i2.InstanceNatureOfContentTerms = null;
            i2.InstanceStatisticalCodes = null;
            i2.InstanceTags = null;
            i2.Languages = null;
            i2.Note2s = null;
            i2.PhysicalDescriptions = null;
            i2.PublicationFrequencies = null;
            i2.PublicationRanges = null;
            i2.Publications = null;
            i2.Series = null;
            i2.Subjects = null;
            var i3 = folioDapperContext.Instance2s(take: 1).SingleOrDefault();
            i3.Content = null;
            Assert.AreEqual(i2.ToString(), i3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeInstance2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeInstanceNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var int2 = folioServiceContext.InstanceNoteType2s(take: 1).SingleOrDefault();
            if (int2 == null) Assert.Inconclusive();
            var int3 = folioDapperContext.InstanceNoteType2s(take: 1).SingleOrDefault();
            int3.Content = null;
            Assert.AreEqual(int2.ToString(), int3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeInstanceNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeInstanceType2Test()
        {
            var s = Stopwatch.StartNew();
            var it2 = folioServiceContext.InstanceType2s(take: 1).SingleOrDefault();
            if (it2 == null) Assert.Inconclusive();
            var it3 = folioDapperContext.InstanceType2s(take: 1).SingleOrDefault();
            it3.Content = null;
            Assert.AreEqual(it2.ToString(), it3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeInstanceType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeInstitution2Test()
        {
            var s = Stopwatch.StartNew();
            var i2 = folioServiceContext.Institution2s(take: 1).SingleOrDefault();
            if (i2 == null) Assert.Inconclusive();
            var i3 = folioDapperContext.Institution2s(take: 1).SingleOrDefault();
            i3.Content = null;
            Assert.AreEqual(i2.ToString(), i3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeInstitution2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_DeserializeInterface2Test()
        {
            var s = Stopwatch.StartNew();
            var i2 = folioServiceContext.Interface2s(take: 1).SingleOrDefault();
            if (i2 == null) Assert.Inconclusive();
            i2.InterfaceTypes = null;
            var i3 = folioDapperContext.Interface2s(take: 1).SingleOrDefault();
            i3.Content = null;
            Assert.AreEqual(i2.ToString(), i3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organizations_DeserializeInterface2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeInvoice2Test()
        {
            var s = Stopwatch.StartNew();
            var i2 = folioServiceContext.Invoice2s(take: 1).SingleOrDefault();
            if (i2 == null) Assert.Inconclusive();
            i2.InvoiceAcquisitionsUnits = null;
            i2.InvoiceAdjustments = null;
            i2.InvoiceOrderNumbers = null;
            i2.InvoiceTags = null;
            var i3 = folioDapperContext.Invoice2s(take: 1).SingleOrDefault();
            i3.Content = null;
            Assert.AreEqual(i2.ToString(), i3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeInvoice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeInvoiceItem2Test()
        {
            var s = Stopwatch.StartNew();
            var ii2 = folioServiceContext.InvoiceItem2s(take: 1).SingleOrDefault();
            if (ii2 == null) Assert.Inconclusive();
            ii2.InvoiceItemAdjustmentFunds = null;
            ii2.InvoiceItemAdjustments = null;
            ii2.InvoiceItemFunds = null;
            ii2.InvoiceItemTags = null;
            var ii3 = folioDapperContext.InvoiceItem2s(take: 1).SingleOrDefault();
            ii3.Content = null;
            Assert.AreEqual(ii2.ToString(), ii3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeInvoiceItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeIssuanceModeTest()
        {
            var s = Stopwatch.StartNew();
            var im = folioServiceContext.IssuanceModes(take: 1).SingleOrDefault();
            if (im == null) Assert.Inconclusive();
            var im2 = folioDapperContext.IssuanceModes(take: 1).SingleOrDefault();
            im2.Content = null;
            Assert.AreEqual(im.ToString(), im2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeIssuanceModeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeItem2Test()
        {
            var s = Stopwatch.StartNew();
            var i2 = folioServiceContext.Item2s(take: 1).SingleOrDefault();
            if (i2 == null) Assert.Inconclusive();
            i2.CirculationNotes = null;
            i2.ItemElectronicAccesses = null;
            i2.ItemFormerIds = null;
            i2.ItemNotes = null;
            i2.ItemStatisticalCodes = null;
            i2.ItemTags = null;
            i2.ItemYearCaptions = null;
            var i3 = folioDapperContext.Item2s(take: 1).SingleOrDefault();
            i3.Content = null;
            Assert.AreEqual(i2.ToString(), i3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeItemDamagedStatus2Test()
        {
            var s = Stopwatch.StartNew();
            var ids2 = folioServiceContext.ItemDamagedStatus2s(take: 1).SingleOrDefault();
            if (ids2 == null) Assert.Inconclusive();
            var ids3 = folioDapperContext.ItemDamagedStatus2s(take: 1).SingleOrDefault();
            ids3.Content = null;
            Assert.AreEqual(ids2.ToString(), ids3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeItemDamagedStatus2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeItemNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var int2 = folioServiceContext.ItemNoteType2s(take: 1).SingleOrDefault();
            if (int2 == null) Assert.Inconclusive();
            var int3 = folioDapperContext.ItemNoteType2s(take: 1).SingleOrDefault();
            int3.Content = null;
            Assert.AreEqual(int2.ToString(), int3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeItemNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeLedger2Test()
        {
            var s = Stopwatch.StartNew();
            var l2 = folioServiceContext.Ledger2s(take: 1).SingleOrDefault();
            if (l2 == null) Assert.Inconclusive();
            l2.LedgerAcquisitionsUnits = null;
            var l3 = folioDapperContext.Ledger2s(take: 1).SingleOrDefault();
            l3.Content = null;
            Assert.AreEqual(l2.ToString(), l3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeLedger2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeLibrary2Test()
        {
            var s = Stopwatch.StartNew();
            var l2 = folioServiceContext.Library2s(take: 1).SingleOrDefault();
            if (l2 == null) Assert.Inconclusive();
            var l3 = folioDapperContext.Library2s(take: 1).SingleOrDefault();
            l3.Content = null;
            Assert.AreEqual(l2.ToString(), l3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeLibrary2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeLoan2Test()
        {
            var s = Stopwatch.StartNew();
            var l2 = folioServiceContext.Loan2s(take: 1).SingleOrDefault();
            if (l2 == null) Assert.Inconclusive();
            var l3 = folioDapperContext.Loan2s(take: 1).SingleOrDefault();
            l3.Content = null;
            Assert.AreEqual(l2.ToString(), l3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeLoan2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeLoanPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var lp2 = folioServiceContext.LoanPolicy2s(take: 1).SingleOrDefault();
            if (lp2 == null) Assert.Inconclusive();
            var lp3 = folioDapperContext.LoanPolicy2s(take: 1).SingleOrDefault();
            lp3.Content = null;
            Assert.AreEqual(lp2.ToString(), lp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeLoanPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeLoanType2Test()
        {
            var s = Stopwatch.StartNew();
            var lt2 = folioServiceContext.LoanType2s(take: 1).SingleOrDefault();
            if (lt2 == null) Assert.Inconclusive();
            var lt3 = folioDapperContext.LoanType2s(take: 1).SingleOrDefault();
            lt3.Content = null;
            Assert.AreEqual(lt2.ToString(), lt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeLoanType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeLocation2Test()
        {
            var s = Stopwatch.StartNew();
            var l2 = folioServiceContext.Location2s(take: 1).SingleOrDefault();
            if (l2 == null) Assert.Inconclusive();
            l2.LocationServicePoints = null;
            var l3 = folioDapperContext.Location2s(take: 1).SingleOrDefault();
            l3.Content = null;
            Assert.AreEqual(l2.ToString(), l3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeLocation2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_DeserializeLocationSettingTest()
        {
            var s = Stopwatch.StartNew();
            var ls = folioServiceContext.LocationSettings(take: 1).SingleOrDefault();
            if (ls == null) Assert.Inconclusive();
            var ls2 = folioDapperContext.LocationSettings(take: 1).SingleOrDefault();
            Assert.AreEqual(ls.ToString(), ls2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration_DeserializeLocationSettingTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeLostItemFeePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var lifp2 = folioServiceContext.LostItemFeePolicy2s(take: 1).SingleOrDefault();
            if (lifp2 == null) Assert.Inconclusive();
            var lifp3 = folioDapperContext.LostItemFeePolicy2s(take: 1).SingleOrDefault();
            lifp3.Content = null;
            Assert.AreEqual(lifp2.ToString(), lifp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeLostItemFeePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeMaterialType2Test()
        {
            var s = Stopwatch.StartNew();
            var mt2 = folioServiceContext.MaterialType2s(take: 1).SingleOrDefault();
            if (mt2 == null) Assert.Inconclusive();
            var mt3 = folioDapperContext.MaterialType2s(take: 1).SingleOrDefault();
            mt3.Content = null;
            Assert.AreEqual(mt2.ToString(), mt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeMaterialType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeNatureOfContentTerm2Test()
        {
            var s = Stopwatch.StartNew();
            var noct2 = folioServiceContext.NatureOfContentTerm2s(take: 1).SingleOrDefault();
            if (noct2 == null) Assert.Inconclusive();
            var noct3 = folioDapperContext.NatureOfContentTerm2s(take: 1).SingleOrDefault();
            noct3.Content = null;
            Assert.AreEqual(noct2.ToString(), noct3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeNatureOfContentTerm2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_DeserializeNote3Test()
        {
            var s = Stopwatch.StartNew();
            var n3 = folioServiceContext.Note3s(take: 1).SingleOrDefault();
            if (n3 == null) Assert.Inconclusive();
            n3.NoteLinks = null;
            var n4 = folioDapperContext.Note3s(take: 1).SingleOrDefault();
            n4.Content = null;
            Assert.AreEqual(n3.ToString(), n4.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Notes_DeserializeNote3Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_DeserializeNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var nt2 = folioServiceContext.NoteType2s(take: 1).SingleOrDefault();
            if (nt2 == null) Assert.Inconclusive();
            var nt3 = folioDapperContext.NoteType2s(take: 1).SingleOrDefault();
            nt3.Content = null;
            Assert.AreEqual(nt2.ToString(), nt3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Notes_DeserializeNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeOrder2Test()
        {
            var s = Stopwatch.StartNew();
            var o2 = folioServiceContext.Order2s(take: 1).SingleOrDefault();
            if (o2 == null) Assert.Inconclusive();
            o2.OrderAcquisitionsUnits = null;
            o2.OrderNotes = null;
            o2.OrderTags = null;
            var o3 = folioDapperContext.Order2s(take: 1).SingleOrDefault();
            o3.Content = null;
            Assert.AreEqual(o2.ToString(), o3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeOrder2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeOrderInvoice2Test()
        {
            var s = Stopwatch.StartNew();
            var oi2 = folioServiceContext.OrderInvoice2s(take: 1).SingleOrDefault();
            if (oi2 == null) Assert.Inconclusive();
            var oi3 = folioDapperContext.OrderInvoice2s(take: 1).SingleOrDefault();
            oi3.Content = null;
            Assert.AreEqual(oi2.ToString(), oi3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeOrderInvoice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeOrderItem2Test()
        {
            var s = Stopwatch.StartNew();
            var oi2 = folioServiceContext.OrderItem2s(take: 1).SingleOrDefault();
            if (oi2 == null) Assert.Inconclusive();
            oi2.OrderItemAlerts = null;
            oi2.OrderItemClaims = null;
            oi2.OrderItemContributors = null;
            oi2.OrderItemFunds = null;
            oi2.OrderItemLocation2s = null;
            oi2.OrderItemProductIds = null;
            oi2.OrderItemReportingCodes = null;
            oi2.OrderItemTags = null;
            oi2.OrderItemVolumes = null;
            var oi3 = folioDapperContext.OrderItem2s(take: 1).SingleOrDefault();
            oi3.Content = null;
            Assert.AreEqual(oi2.ToString(), oi3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeOrderItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeOrderTemplate2Test()
        {
            var s = Stopwatch.StartNew();
            var ot2 = folioServiceContext.OrderTemplate2s(take: 1).SingleOrDefault();
            if (ot2 == null) Assert.Inconclusive();
            var ot3 = folioDapperContext.OrderTemplate2s(take: 1).SingleOrDefault();
            ot3.Content = null;
            Assert.AreEqual(ot2.ToString(), ot3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeOrderTemplate2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeOrderTransactionSummary2Test()
        {
            var s = Stopwatch.StartNew();
            var ots2 = folioServiceContext.OrderTransactionSummary2s(take: 1).SingleOrDefault();
            if (ots2 == null) Assert.Inconclusive();
            var ots3 = folioDapperContext.OrderTransactionSummary2s(take: 1).SingleOrDefault();
            ots3.Content = null;
            Assert.AreEqual(ots2.ToString(), ots3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeOrderTransactionSummary2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_DeserializeOrganization2Test()
        {
            var s = Stopwatch.StartNew();
            var o2 = folioServiceContext.Organization2s(take: 1).SingleOrDefault();
            if (o2 == null) Assert.Inconclusive();
            o2.Currencies = null;
            o2.OrganizationAccounts = null;
            o2.OrganizationAcquisitionsUnits = null;
            o2.OrganizationAddresses = null;
            o2.OrganizationAgreements = null;
            o2.OrganizationAliases = null;
            o2.OrganizationChangelogs = null;
            o2.OrganizationContacts = null;
            o2.OrganizationEmails = null;
            o2.OrganizationInterfaces = null;
            o2.OrganizationPhoneNumbers = null;
            o2.OrganizationTags = null;
            o2.OrganizationUrls = null;
            var o3 = folioDapperContext.Organization2s(take: 1).SingleOrDefault();
            o3.Content = null;
            Assert.AreEqual(o2.ToString(), o3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Organizations_DeserializeOrganization2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeOverdueFinePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var ofp2 = folioServiceContext.OverdueFinePolicy2s(take: 1).SingleOrDefault();
            if (ofp2 == null) Assert.Inconclusive();
            var ofp3 = folioDapperContext.OverdueFinePolicy2s(take: 1).SingleOrDefault();
            ofp3.Content = null;
            Assert.AreEqual(ofp2.ToString(), ofp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeOverdueFinePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeOwner2Test()
        {
            var s = Stopwatch.StartNew();
            var o2 = folioServiceContext.Owner2s(take: 1).SingleOrDefault();
            if (o2 == null) Assert.Inconclusive();
            o2.ServicePointOwners = null;
            var o3 = folioDapperContext.Owner2s(take: 1).SingleOrDefault();
            o3.Content = null;
            Assert.AreEqual(o2.ToString(), o3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeOwner2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializePatronActionSession2Test()
        {
            var s = Stopwatch.StartNew();
            var pas2 = folioServiceContext.PatronActionSession2s(take: 1).SingleOrDefault();
            if (pas2 == null) Assert.Inconclusive();
            var pas3 = folioDapperContext.PatronActionSession2s(take: 1).SingleOrDefault();
            pas3.Content = null;
            Assert.AreEqual(pas2.ToString(), pas3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializePatronActionSession2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializePatronNoticePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var pnp2 = folioServiceContext.PatronNoticePolicy2s(take: 1).SingleOrDefault();
            if (pnp2 == null) Assert.Inconclusive();
            pnp2.PatronNoticePolicyFeeFineNotices = null;
            pnp2.PatronNoticePolicyLoanNotices = null;
            pnp2.PatronNoticePolicyRequestNotices = null;
            var pnp3 = folioDapperContext.PatronNoticePolicy2s(take: 1).SingleOrDefault();
            pnp3.Content = null;
            Assert.AreEqual(pnp2.ToString(), pnp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializePatronNoticePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializePayment2Test()
        {
            var s = Stopwatch.StartNew();
            var p2 = folioServiceContext.Payment2s(take: 1).SingleOrDefault();
            if (p2 == null) Assert.Inconclusive();
            var p3 = folioDapperContext.Payment2s(take: 1).SingleOrDefault();
            p3.Content = null;
            Assert.AreEqual(p2.ToString(), p3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializePayment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializePaymentMethod2Test()
        {
            var s = Stopwatch.StartNew();
            var pm2 = folioServiceContext.PaymentMethod2s(take: 1).SingleOrDefault();
            if (pm2 == null) Assert.Inconclusive();
            var pm3 = folioDapperContext.PaymentMethod2s(take: 1).SingleOrDefault();
            pm3.Content = null;
            Assert.AreEqual(pm2.ToString(), pm3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializePaymentMethod2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_DeserializePermission2Test()
        {
            var s = Stopwatch.StartNew();
            var p2 = folioServiceContext.Permission2s(take: 1).SingleOrDefault();
            if (p2 == null) Assert.Inconclusive();
            p2.PermissionChildOfs = null;
            p2.PermissionGrantedTos = null;
            p2.PermissionSubPermissions = null;
            p2.PermissionTags = null;
            var p3 = folioDapperContext.Permission2s(take: 1).SingleOrDefault();
            p3.Content = null;
            Assert.AreEqual(p2.ToString(), p3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permissions_DeserializePermission2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_DeserializePermissionsUser2Test()
        {
            var s = Stopwatch.StartNew();
            var pu2 = folioServiceContext.PermissionsUser2s(take: 1).SingleOrDefault();
            if (pu2 == null) Assert.Inconclusive();
            pu2.PermissionsUserPermissions = null;
            var pu3 = folioDapperContext.PermissionsUser2s(take: 1).SingleOrDefault();
            pu3.Content = null;
            Assert.AreEqual(pu2.ToString(), pu3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Permissions_DeserializePermissionsUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializePrecedingSucceedingTitle2Test()
        {
            var s = Stopwatch.StartNew();
            var pst2 = folioServiceContext.PrecedingSucceedingTitle2s(take: 1).SingleOrDefault();
            if (pst2 == null) Assert.Inconclusive();
            pst2.PrecedingSucceedingTitleIdentifiers = null;
            var pst3 = folioDapperContext.PrecedingSucceedingTitle2s(take: 1).SingleOrDefault();
            pst3.Content = null;
            Assert.AreEqual(pst2.ToString(), pst3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializePrecedingSucceedingTitle2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializePrefix2Test()
        {
            var s = Stopwatch.StartNew();
            var p2 = folioServiceContext.Prefix2s(take: 1).SingleOrDefault();
            if (p2 == null) Assert.Inconclusive();
            var p3 = folioDapperContext.Prefix2s(take: 1).SingleOrDefault();
            p3.Content = null;
            Assert.AreEqual(p2.ToString(), p3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializePrefix2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_DeserializePrinterTest()
        {
            var s = Stopwatch.StartNew();
            var p = folioServiceContext.Printers(take: 1).SingleOrDefault();
            if (p == null) Assert.Inconclusive();
            var p2 = folioDapperContext.Printers(take: 1).SingleOrDefault();
            Assert.AreEqual(p.ToString(), p2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration_DeserializePrinterTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeProxy2Test()
        {
            var s = Stopwatch.StartNew();
            var p2 = folioServiceContext.Proxy2s(take: 1).SingleOrDefault();
            if (p2 == null) Assert.Inconclusive();
            var p3 = folioDapperContext.Proxy2s(take: 1).SingleOrDefault();
            p3.Content = null;
            Assert.AreEqual(p2.ToString(), p3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeProxy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeReceiving2Test()
        {
            var s = Stopwatch.StartNew();
            var r2 = folioServiceContext.Receiving2s(take: 1).SingleOrDefault();
            if (r2 == null) Assert.Inconclusive();
            var r3 = folioDapperContext.Receiving2s(take: 1).SingleOrDefault();
            r3.Content = null;
            Assert.AreEqual(r2.ToString(), r3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeReceiving2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_DeserializeRecord2Test()
        {
            var s = Stopwatch.StartNew();
            var r2 = folioServiceContext.Record2s(take: 1).SingleOrDefault();
            if (r2 == null) Assert.Inconclusive();
            var r3 = folioDapperContext.Record2s(take: 1).SingleOrDefault();
            Assert.AreEqual(r2.ToString(), r3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source_DeserializeRecord2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeRefundReason2Test()
        {
            var s = Stopwatch.StartNew();
            var rr2 = folioServiceContext.RefundReason2s(take: 1).SingleOrDefault();
            if (rr2 == null) Assert.Inconclusive();
            var rr3 = folioDapperContext.RefundReason2s(take: 1).SingleOrDefault();
            rr3.Content = null;
            Assert.AreEqual(rr2.ToString(), rr3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeRefundReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeRelationshipTest()
        {
            var s = Stopwatch.StartNew();
            var r = folioServiceContext.Relationships(take: 1).SingleOrDefault();
            if (r == null) Assert.Inconclusive();
            var r2 = folioDapperContext.Relationships(take: 1).SingleOrDefault();
            r2.Content = null;
            Assert.AreEqual(r.ToString(), r2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeRelationshipTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeRelationshipTypeTest()
        {
            var s = Stopwatch.StartNew();
            var rt = folioServiceContext.RelationshipTypes(take: 1).SingleOrDefault();
            if (rt == null) Assert.Inconclusive();
            var rt2 = folioDapperContext.RelationshipTypes(take: 1).SingleOrDefault();
            rt2.Content = null;
            Assert.AreEqual(rt.ToString(), rt2.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeRelationshipTypeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeReportingCode2Test()
        {
            var s = Stopwatch.StartNew();
            var rc2 = folioServiceContext.ReportingCode2s(take: 1).SingleOrDefault();
            if (rc2 == null) Assert.Inconclusive();
            var rc3 = folioDapperContext.ReportingCode2s(take: 1).SingleOrDefault();
            rc3.Content = null;
            Assert.AreEqual(rc2.ToString(), rc3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeReportingCode2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeRequest2Test()
        {
            var s = Stopwatch.StartNew();
            var r2 = folioServiceContext.Request2s(take: 1).SingleOrDefault();
            if (r2 == null) Assert.Inconclusive();
            r2.RequestIdentifiers = null;
            r2.RequestTags = null;
            var r3 = folioDapperContext.Request2s(take: 1).SingleOrDefault();
            r3.Content = null;
            Assert.AreEqual(r2.ToString(), r3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeRequest2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeRequestPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var rp2 = folioServiceContext.RequestPolicy2s(take: 1).SingleOrDefault();
            if (rp2 == null) Assert.Inconclusive();
            rp2.RequestPolicyRequestTypes = null;
            var rp3 = folioDapperContext.RequestPolicy2s(take: 1).SingleOrDefault();
            rp3.Content = null;
            Assert.AreEqual(rp2.ToString(), rp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeRequestPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeScheduledNotice2Test()
        {
            var s = Stopwatch.StartNew();
            var sn2 = folioServiceContext.ScheduledNotice2s(take: 1).SingleOrDefault();
            if (sn2 == null) Assert.Inconclusive();
            var sn3 = folioDapperContext.ScheduledNotice2s(take: 1).SingleOrDefault();
            sn3.Content = null;
            Assert.AreEqual(sn2.ToString(), sn3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeScheduledNotice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeServicePoint2Test()
        {
            var s = Stopwatch.StartNew();
            var sp2 = folioServiceContext.ServicePoint2s(take: 1).SingleOrDefault();
            if (sp2 == null) Assert.Inconclusive();
            sp2.ServicePointStaffSlips = null;
            var sp3 = folioDapperContext.ServicePoint2s(take: 1).SingleOrDefault();
            sp3.Content = null;
            Assert.AreEqual(sp2.ToString(), sp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeServicePoint2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeServicePointUser2Test()
        {
            var s = Stopwatch.StartNew();
            var spu2 = folioServiceContext.ServicePointUser2s(take: 1).SingleOrDefault();
            if (spu2 == null) Assert.Inconclusive();
            spu2.ServicePointUserServicePoints = null;
            var spu3 = folioDapperContext.ServicePointUser2s(take: 1).SingleOrDefault();
            spu3.Content = null;
            Assert.AreEqual(spu2.ToString(), spu3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeServicePointUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_DeserializeSettingTest()
        {
            var s = Stopwatch.StartNew();
            var s2 = folioServiceContext.Settings(take: 1).SingleOrDefault();
            if (s2 == null) Assert.Inconclusive();
            var s3 = folioDapperContext.Settings(take: 1).SingleOrDefault();
            Assert.AreEqual(s2.ToString(), s3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration_DeserializeSettingTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_DeserializeSnapshot2Test()
        {
            var s = Stopwatch.StartNew();
            var s2 = folioServiceContext.Snapshot2s(take: 1).SingleOrDefault();
            if (s2 == null) Assert.Inconclusive();
            var s3 = folioDapperContext.Snapshot2s(take: 1).SingleOrDefault();
            Assert.AreEqual(s2.ToString(), s3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Source_DeserializeSnapshot2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeSource2Test()
        {
            var s = Stopwatch.StartNew();
            var s2 = folioServiceContext.Source2s(take: 1).SingleOrDefault();
            if (s2 == null) Assert.Inconclusive();
            var s3 = folioDapperContext.Source2s(take: 1).SingleOrDefault();
            s3.Content = null;
            Assert.AreEqual(s2.ToString(), s3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeSource2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeStaffSlip2Test()
        {
            var s = Stopwatch.StartNew();
            var ss2 = folioServiceContext.StaffSlip2s(take: 1).SingleOrDefault();
            if (ss2 == null) Assert.Inconclusive();
            var ss3 = folioDapperContext.StaffSlip2s(take: 1).SingleOrDefault();
            ss3.Content = null;
            Assert.AreEqual(ss2.ToString(), ss3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeStaffSlip2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeStatisticalCode2Test()
        {
            var s = Stopwatch.StartNew();
            var sc2 = folioServiceContext.StatisticalCode2s(take: 1).SingleOrDefault();
            if (sc2 == null) Assert.Inconclusive();
            var sc3 = folioDapperContext.StatisticalCode2s(take: 1).SingleOrDefault();
            sc3.Content = null;
            Assert.AreEqual(sc2.ToString(), sc3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeStatisticalCode2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeStatisticalCodeType2Test()
        {
            var s = Stopwatch.StartNew();
            var sct2 = folioServiceContext.StatisticalCodeType2s(take: 1).SingleOrDefault();
            if (sct2 == null) Assert.Inconclusive();
            var sct3 = folioDapperContext.StatisticalCodeType2s(take: 1).SingleOrDefault();
            sct3.Content = null;
            Assert.AreEqual(sct2.ToString(), sct3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeStatisticalCodeType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_DeserializeStatusTest()
        {
            var s = Stopwatch.StartNew();
            var s2 = folioServiceContext.Statuses(take: 1).SingleOrDefault();
            if (s2 == null) Assert.Inconclusive();
            var s3 = folioDapperContext.Statuses(take: 1).SingleOrDefault();
            s3.Content = null;
            Assert.AreEqual(s2.ToString(), s3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Inventory_DeserializeStatusTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeSuffix2Test()
        {
            var s = Stopwatch.StartNew();
            var s2 = folioServiceContext.Suffix2s(take: 1).SingleOrDefault();
            if (s2 == null) Assert.Inconclusive();
            var s3 = folioDapperContext.Suffix2s(take: 1).SingleOrDefault();
            s3.Content = null;
            Assert.AreEqual(s2.ToString(), s3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeSuffix2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Tags_DeserializeTag2Test()
        {
            var s = Stopwatch.StartNew();
            var t2 = folioServiceContext.Tag2s(take: 1).SingleOrDefault();
            if (t2 == null) Assert.Inconclusive();
            var t3 = folioDapperContext.Tag2s(take: 1).SingleOrDefault();
            t3.Content = null;
            Assert.AreEqual(t2.ToString(), t3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Tags_DeserializeTag2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Templates_DeserializeTemplate2Test()
        {
            var s = Stopwatch.StartNew();
            var t2 = folioServiceContext.Template2s(take: 1).SingleOrDefault();
            if (t2 == null) Assert.Inconclusive();
            t2.TemplateOutputFormats = null;
            var t3 = folioDapperContext.Template2s(take: 1).SingleOrDefault();
            t3.Content = null;
            Assert.AreEqual(t2.ToString(), t3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Templates_DeserializeTemplate2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeTitle2Test()
        {
            var s = Stopwatch.StartNew();
            var t2 = folioServiceContext.Title2s(take: 1).SingleOrDefault();
            if (t2 == null) Assert.Inconclusive();
            t2.TitleContributors = null;
            t2.TitleProductIds = null;
            var t3 = folioDapperContext.Title2s(take: 1).SingleOrDefault();
            t3.Content = null;
            Assert.AreEqual(t2.ToString(), t3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeTitle2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_DeserializeTransaction2Test()
        {
            var s = Stopwatch.StartNew();
            var t2 = folioServiceContext.Transaction2s(take: 1).SingleOrDefault();
            if (t2 == null) Assert.Inconclusive();
            t2.TransactionTags = null;
            var t3 = folioDapperContext.Transaction2s(take: 1).SingleOrDefault();
            t3.Content = null;
            Assert.AreEqual(t2.ToString(), t3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Finance_DeserializeTransaction2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeTransferAccount2Test()
        {
            var s = Stopwatch.StartNew();
            var ta2 = folioServiceContext.TransferAccount2s(take: 1).SingleOrDefault();
            if (ta2 == null) Assert.Inconclusive();
            var ta3 = folioDapperContext.TransferAccount2s(take: 1).SingleOrDefault();
            ta3.Content = null;
            Assert.AreEqual(ta2.ToString(), ta3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeTransferAccount2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeTransferCriteria2Test()
        {
            var s = Stopwatch.StartNew();
            var tc2 = folioServiceContext.TransferCriteria2s(take: 1).SingleOrDefault();
            if (tc2 == null) Assert.Inconclusive();
            var tc3 = folioDapperContext.TransferCriteria2s(take: 1).SingleOrDefault();
            tc3.Content = null;
            Assert.AreEqual(tc2.ToString(), tc3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeTransferCriteria2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_DeserializeUser2Test()
        {
            var s = Stopwatch.StartNew();
            var u2 = folioServiceContext.User2s(take: 1).SingleOrDefault();
            if (u2 == null) Assert.Inconclusive();
            u2.UserAddresses = null;
            u2.UserDepartments = null;
            u2.UserTags = null;
            var u3 = folioDapperContext.User2s(take: 1).SingleOrDefault();
            u3.Content = null;
            Assert.AreEqual(u2.ToString(), u3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Users_DeserializeUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_DeserializeUserAcquisitionsUnit2Test()
        {
            var s = Stopwatch.StartNew();
            var uau2 = folioServiceContext.UserAcquisitionsUnit2s(take: 1).SingleOrDefault();
            if (uau2 == null) Assert.Inconclusive();
            var uau3 = folioDapperContext.UserAcquisitionsUnit2s(take: 1).SingleOrDefault();
            uau3.Content = null;
            Assert.AreEqual(uau2.ToString(), uau3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Orders_DeserializeUserAcquisitionsUnit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_DeserializeUserRequestPreference2Test()
        {
            var s = Stopwatch.StartNew();
            var urp2 = folioServiceContext.UserRequestPreference2s(take: 1).SingleOrDefault();
            if (urp2 == null) Assert.Inconclusive();
            var urp3 = folioDapperContext.UserRequestPreference2s(take: 1).SingleOrDefault();
            urp3.Content = null;
            Assert.AreEqual(urp2.ToString(), urp3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Circulation_DeserializeUserRequestPreference2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeVoucher2Test()
        {
            var s = Stopwatch.StartNew();
            var v2 = folioServiceContext.Voucher2s(take: 1).SingleOrDefault();
            if (v2 == null) Assert.Inconclusive();
            v2.VoucherAcquisitionsUnits = null;
            var v3 = folioDapperContext.Voucher2s(take: 1).SingleOrDefault();
            v3.Content = null;
            Assert.AreEqual(v2.ToString(), v3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeVoucher2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_DeserializeVoucherItem2Test()
        {
            var s = Stopwatch.StartNew();
            var vi2 = folioServiceContext.VoucherItem2s(take: 1).SingleOrDefault();
            if (vi2 == null) Assert.Inconclusive();
            vi2.VoucherItemFunds = null;
            vi2.VoucherItemSourceIds = null;
            var vi3 = folioDapperContext.VoucherItem2s(take: 1).SingleOrDefault();
            vi3.Content = null;
            Assert.AreEqual(vi2.ToString(), vi3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoices_DeserializeVoucherItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_DeserializeWaiveReason2Test()
        {
            var s = Stopwatch.StartNew();
            var wr2 = folioServiceContext.WaiveReason2s(take: 1).SingleOrDefault();
            if (wr2 == null) Assert.Inconclusive();
            var wr3 = folioDapperContext.WaiveReason2s(take: 1).SingleOrDefault();
            wr3.Content = null;
            Assert.AreEqual(wr2.ToString(), wr3.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, $"Fees_DeserializeWaiveReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioDapperContext.Dispose();
            folioServiceContext.Dispose();
        }
    }
}
