using FolioLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolioLibraryTest
{
    [TestClass]
    public class FolioServiceClientTest
    {
        private readonly static FolioDapperContext folioDapperContext = new FolioDapperContext();
        private readonly static FolioServiceClient folioServiceClient = new FolioServiceClient();
        private readonly static TraceSource traceSource = new TraceSource("FolioLibraryTest", SourceLevels.Information);
        private readonly static int? take = 100_000;

        static FolioServiceClientTest()
        {
            traceSource.Listeners.Add(new TextWriterTraceListener(new StreamWriter("Trace.log", true) { AutoFlush = true }) { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId });
            FolioDapperContext.traceSource.Listeners.AddRange(traceSource.Listeners);
            FolioServiceClient.traceSource.Listeners.AddRange(traceSource.Listeners);
            traceSource.Switch.Level = FolioDapperContext.traceSource.Switch.Level = FolioServiceClient.traceSource.Switch.Level = SourceLevels.Information;
        }

        [TestMethod]
        public void CountAcquisitionMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAcquisitionMethods();
            var j = folioDapperContext.CountAcquisitionMethods();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAcquisitionMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersAcquisitionMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AcquisitionMethods(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.AcquisitionMethods(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAcquisitionsUnits();
            var j = folioDapperContext.CountAcquisitionsUnits();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AcquisitionsUnits(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.AcquisitionsUnits(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountAddressTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAddressTypes();
            var j = folioDapperContext.CountAddressTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAddressTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersAddressTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AddressTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.AddressTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountAlertsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAlerts();
            var j = folioDapperContext.CountAlerts();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAlertsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersAlertsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Alerts(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Alerts(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlertsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountAlternativeTitleTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAlternativeTitleTypes();
            var j = folioDapperContext.CountAlternativeTitleTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAlternativeTitleTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryAlternativeTitleTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AlternativeTitleTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.AlternativeTitleTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBatchGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBatchGroups();
            var j = folioDapperContext.CountBatchGroups();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBatchGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoicesBatchGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BatchGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BatchGroups(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBlocksTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlocks();
            var j = folioDapperContext.CountBlocks();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlocksTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesBlocksTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Blocks(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Blocks(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlocksTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBlockConditionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlockConditions();
            var j = folioDapperContext.CountBlockConditions();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlockConditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersBlockConditionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BlockConditions(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BlockConditions(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockConditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBlockLimitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlockLimits();
            var j = folioDapperContext.CountBlockLimits();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlockLimitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersBlockLimitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BlockLimits(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BlockLimits(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBoundWithPartsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBoundWithParts();
            var j = folioDapperContext.CountBoundWithParts();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBoundWithPartsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryBoundWithPartsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BoundWithParts(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BoundWithParts(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPartsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgets();
            var j = folioDapperContext.CountBudgets();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Budgets(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Budgets(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBudgetExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgetExpenseClasses();
            var j = folioDapperContext.CountBudgetExpenseClasses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceBudgetExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BudgetExpenseClasses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BudgetExpenseClasses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountBudgetGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgetGroups();
            var j = folioDapperContext.CountBudgetGroups();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceBudgetGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BudgetGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.BudgetGroups(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCallNumberTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCallNumberTypes();
            var j = folioDapperContext.CountCallNumberTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCallNumberTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryCallNumberTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CallNumberTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.CallNumberTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCampusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCampuses();
            var j = folioDapperContext.CountCampuses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCampusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryCampusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Campuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Campuses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CampusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCancellationReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCancellationReasons();
            var j = folioDapperContext.CountCancellationReasons();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCancellationReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationCancellationReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CancellationReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.CancellationReasons(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCategories();
            var j = folioDapperContext.CountCategories();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationsCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Categories(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Categories(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCheckInsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCheckIns();
            var j = folioDapperContext.CountCheckIns();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCheckInsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationCheckInsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CheckIns(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.CheckIns(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckInsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountClassificationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountClassificationTypes();
            var j = folioDapperContext.CountClassificationTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountClassificationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryClassificationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ClassificationTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ClassificationTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCloseReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCloseReasons();
            var j = folioDapperContext.CountCloseReasons();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCloseReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersCloseReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CloseReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.CloseReasons(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCommentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountComments();
            var j = folioDapperContext.CountComments();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCommentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesCommentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Comments(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Comments(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CommentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountConfigurationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountConfigurations();
            var j = folioDapperContext.CountConfigurations();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountConfigurationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void ConfigurationConfigurationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Configurations(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Configurations(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ConfigurationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountContactsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContacts();
            var j = folioDapperContext.CountContacts();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationsContactsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Contacts(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Contacts(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountContributorNameTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContributorNameTypes();
            var j = folioDapperContext.CountContributorNameTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContributorNameTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryContributorNameTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ContributorNameTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ContributorNameTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountContributorTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContributorTypes();
            var j = folioDapperContext.CountContributorTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContributorTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryContributorTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ContributorTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ContributorTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountCustomFieldsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCustomFields();
            var j = folioDapperContext.CountCustomFields();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCustomFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersCustomFieldsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CustomFields(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.CustomFields(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountDepartments();
            var j = folioDapperContext.CountDepartments();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountDepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Departments(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Departments(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountElectronicAccessRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountElectronicAccessRelationships();
            var j = folioDapperContext.CountElectronicAccessRelationships();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountElectronicAccessRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryElectronicAccessRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ElectronicAccessRelationships(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ElectronicAccessRelationships(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountExpenseClasses();
            var j = folioDapperContext.CountExpenseClasses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ExpenseClasses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ExpenseClasses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFeesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFees();
            var j = folioDapperContext.CountFees();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFeesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesFeesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Fees(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Fees(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFeeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFeeTypes();
            var j = folioDapperContext.CountFeeTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFeeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesFeeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FeeTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.FeeTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFinanceGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFinanceGroups();
            var j = folioDapperContext.CountFinanceGroups();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFinanceGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceFinanceGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FinanceGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.FinanceGroups(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFiscalYearsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFiscalYears();
            var j = folioDapperContext.CountFiscalYears();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFiscalYearsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceFiscalYearsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FiscalYears(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.FiscalYears(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFixedDueDateSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFixedDueDateSchedules();
            var j = folioDapperContext.CountFixedDueDateSchedules();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFixedDueDateSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationFixedDueDateSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FixedDueDateSchedules(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.FixedDueDateSchedules(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFundsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFunds();
            var j = folioDapperContext.CountFunds();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceFundsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Funds(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Funds(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountFundTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFundTypes();
            var j = folioDapperContext.CountFundTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFundTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceFundTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FundTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.FundTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountGroups();
            var j = folioDapperContext.CountGroups();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Groups(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Groups(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"GroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountHoldingsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldings();
            var j = folioDapperContext.CountHoldings();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryHoldingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Holdings(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Holdings(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountHoldingNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldingNoteTypes();
            var j = folioDapperContext.CountHoldingNoteTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryHoldingNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.HoldingNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.HoldingNoteTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountHoldingTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldingTypes();
            var j = folioDapperContext.CountHoldingTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryHoldingTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.HoldingTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.HoldingTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountIdTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountIdTypes();
            var j = folioDapperContext.CountIdTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountIdTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryIdTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.IdTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.IdTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountIllPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountIllPolicies();
            var j = folioDapperContext.CountIllPolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountIllPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryIllPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.IllPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.IllPolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstancesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstances();
            var j = folioDapperContext.CountInstances();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstancesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Instances(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Instances(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceFormatsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceFormats();
            var j = folioDapperContext.CountInstanceFormats();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceFormatsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceFormats(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceFormats(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceNoteTypes();
            var j = folioDapperContext.CountInstanceNoteTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceNoteTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceRelationships();
            var j = folioDapperContext.CountInstanceRelationships();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceRelationships(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceRelationships(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceRelationshipTypes();
            var j = folioDapperContext.CountInstanceRelationshipTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceRelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceRelationshipTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceRelationshipTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceRelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceStatuses();
            var j = folioDapperContext.CountInstanceStatuses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceStatuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceStatuses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceTypes();
            var j = folioDapperContext.CountInstanceTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InstanceTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInstitutionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstitutions();
            var j = folioDapperContext.CountInstitutions();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstitutionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryInstitutionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Institutions(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Institutions(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstitutionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInterfaces();
            var j = folioDapperContext.CountInterfaces();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationsInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Interfaces(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Interfaces(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInvoices();
            var j = folioDapperContext.CountInvoices();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoicesInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Invoices(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Invoices(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInvoiceItems();
            var j = folioDapperContext.CountInvoiceItems();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoicesInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InvoiceItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.InvoiceItems(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItems();
            var j = folioDapperContext.CountItems();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Items(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Items(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountItemDamagedStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItemDamagedStatuses();
            var j = folioDapperContext.CountItemDamagedStatuses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemDamagedStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryItemDamagedStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ItemDamagedStatuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ItemDamagedStatuses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountItemNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItemNoteTypes();
            var j = folioDapperContext.CountItemNoteTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryItemNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ItemNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ItemNoteTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLedgersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLedgers();
            var j = folioDapperContext.CountLedgers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLedgersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceLedgersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Ledgers(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Ledgers(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLedgerRolloversTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLedgerRollovers();
            var j = folioDapperContext.CountLedgerRollovers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLedgerRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceLedgerRolloversTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LedgerRollovers(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LedgerRollovers(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLedgerRolloverErrorsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLedgerRolloverErrors();
            var j = folioDapperContext.CountLedgerRolloverErrors();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLedgerRolloverErrorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceLedgerRolloverErrorsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LedgerRolloverErrors(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LedgerRolloverErrors(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverErrorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLedgerRolloverProgressesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLedgerRolloverProgresses();
            var j = folioDapperContext.CountLedgerRolloverProgresses();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLedgerRolloverProgressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceLedgerRolloverProgressesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LedgerRolloverProgresses(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LedgerRolloverProgresses(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgerRolloverProgressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLibrariesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLibraries();
            var j = folioDapperContext.CountLibraries();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLibrariesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryLibrariesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Libraries(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Libraries(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LibrariesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLoansTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoans();
            var j = folioDapperContext.CountLoans();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationLoansTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Loans(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Loans(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLoanPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoanPolicies();
            var j = folioDapperContext.CountLoanPolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoanPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationLoanPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LoanPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LoanPolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLoanTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoanTypes();
            var j = folioDapperContext.CountLoanTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoanTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryLoanTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LoanTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LoanTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLocationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLocations();
            var j = folioDapperContext.CountLocations();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryLocationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Locations(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Locations(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountLostItemFeePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLostItemFeePolicies();
            var j = folioDapperContext.CountLostItemFeePolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLostItemFeePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesLostItemFeePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LostItemFeePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.LostItemFeePolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountManualBlockTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountManualBlockTemplates();
            var j = folioDapperContext.CountManualBlockTemplates();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountManualBlockTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesManualBlockTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ManualBlockTemplates(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ManualBlockTemplates(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountMaterialTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountMaterialTypes();
            var j = folioDapperContext.CountMaterialTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountMaterialTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryMaterialTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.MaterialTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.MaterialTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountModeOfIssuancesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountModeOfIssuances();
            var j = folioDapperContext.CountModeOfIssuances();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountModeOfIssuancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryModeOfIssuancesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ModeOfIssuances(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ModeOfIssuances(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ModeOfIssuancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNatureOfContentTerms();
            var j = folioDapperContext.CountNatureOfContentTerms();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.NatureOfContentTerms(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.NatureOfContentTerms(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountNotesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNotes();
            var j = folioDapperContext.CountNotes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void NotesNotesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Notes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Notes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNoteTypes();
            var j = folioDapperContext.CountNoteTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void NotesNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.NoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.NoteTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOrdersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrders();
            var j = folioDapperContext.CountOrders();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrdersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersOrdersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Orders(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Orders(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrdersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOrderInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrderInvoices();
            var j = folioDapperContext.CountOrderInvoices();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrderInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersOrderInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrderInvoices(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.OrderInvoices(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrderItems();
            var j = folioDapperContext.CountOrderItems();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrderItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.OrderItems(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOrderTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrderTemplates();
            var j = folioDapperContext.CountOrderTemplates();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrderTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersOrderTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrderTemplates(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.OrderTemplates(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrganizations();
            var j = folioDapperContext.CountOrganizations();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrganizationsOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Organizations(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Organizations(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOverdueFinePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOverdueFinePolicies();
            var j = folioDapperContext.CountOverdueFinePolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOverdueFinePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesOverdueFinePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OverdueFinePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.OverdueFinePolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountOwnersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOwners();
            var j = folioDapperContext.CountOwners();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesOwnersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Owners(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Owners(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPatronActionSessionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPatronActionSessions();
            var j = folioDapperContext.CountPatronActionSessions();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPatronActionSessionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationPatronActionSessionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PatronActionSessions(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.PatronActionSessions(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSessionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPatronNoticePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPatronNoticePolicies();
            var j = folioDapperContext.CountPatronNoticePolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPatronNoticePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationPatronNoticePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PatronNoticePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.PatronNoticePolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPaymentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPayments();
            var j = folioDapperContext.CountPayments();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPaymentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesPaymentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Payments(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Payments(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPaymentMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPaymentMethods();
            var j = folioDapperContext.CountPaymentMethods();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPaymentMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesPaymentMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PaymentMethods(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.PaymentMethods(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPermissions();
            var j = folioDapperContext.CountPermissions();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionsPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Permissions(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Permissions(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPermissionsUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPermissionsUsers();
            var j = folioDapperContext.CountPermissionsUsers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPermissionsUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void PermissionsPermissionsUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PermissionsUsers(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.PermissionsUsers(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPrecedingSucceedingTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPrecedingSucceedingTitles();
            var j = folioDapperContext.CountPrecedingSucceedingTitles();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPrecedingSucceedingTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryPrecedingSucceedingTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PrecedingSucceedingTitles(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.PrecedingSucceedingTitles(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountPrefixesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPrefixes();
            var j = folioDapperContext.CountPrefixes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPrefixesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersPrefixesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Prefixes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Prefixes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrefixesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountProxiesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountProxies();
            var j = folioDapperContext.CountProxies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountProxiesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersProxiesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Proxies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Proxies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ProxiesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountReceivingsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountReceivings();
            var j = folioDapperContext.CountReceivings();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountReceivingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersReceivingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Receivings(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Receivings(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReceivingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRecords();
            var j = folioDapperContext.CountRecords();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SourceRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Records(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Records(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountRefundReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRefundReasons();
            var j = folioDapperContext.CountRefundReasons();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRefundReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesRefundReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RefundReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.RefundReasons(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountRelatedInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRelatedInstanceTypes();
            var j = folioDapperContext.CountRelatedInstanceTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRelatedInstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryRelatedInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RelatedInstanceTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.RelatedInstanceTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RelatedInstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountReportingCodesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountReportingCodes();
            var j = folioDapperContext.CountReportingCodes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountReportingCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersReportingCodesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ReportingCodes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ReportingCodes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountRequestsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRequests();
            var j = folioDapperContext.CountRequests();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRequestsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationRequestsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Requests(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Requests(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountRequestPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRequestPolicies();
            var j = folioDapperContext.CountRequestPolicies();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRequestPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationRequestPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RequestPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.RequestPolicies(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountScheduledNoticesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountScheduledNotices();
            var j = folioDapperContext.CountScheduledNotices();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountScheduledNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationScheduledNoticesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ScheduledNotices(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ScheduledNotices(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountServicePoints();
            var j = folioDapperContext.CountServicePoints();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ServicePoints(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ServicePoints(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountServicePointUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountServicePointUsers();
            var j = folioDapperContext.CountServicePointUsers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountServicePointUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryServicePointUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ServicePointUsers(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.ServicePointUsers(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountSnapshotsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSnapshots();
            var j = folioDapperContext.CountSnapshots();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSnapshotsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void SourceSnapshotsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Snapshots(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Snapshots(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SnapshotsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountSourcesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSources();
            var j = folioDapperContext.CountSources();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventorySourcesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Sources(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Sources(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStaffSlips();
            var j = folioDapperContext.CountStaffSlips();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StaffSlips(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.StaffSlips(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStatisticalCodes();
            var j = folioDapperContext.CountStatisticalCodes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StatisticalCodes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.StatisticalCodes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountStatisticalCodeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStatisticalCodeTypes();
            var j = folioDapperContext.CountStatisticalCodeTypes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStatisticalCodeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InventoryStatisticalCodeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StatisticalCodeTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.StatisticalCodeTypes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountSuffixesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSuffixes();
            var j = folioDapperContext.CountSuffixes();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSuffixesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersSuffixesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Suffixes(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Suffixes(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SuffixesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTagsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTags();
            var j = folioDapperContext.CountTags();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TagsTagsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Tags(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Tags(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTemplates();
            var j = folioDapperContext.CountTemplates();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void TemplatesTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Templates(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Templates(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTitles();
            var j = folioDapperContext.CountTitles();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Titles(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Titles(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTransactionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransactions();
            var j = folioDapperContext.CountTransactions();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransactionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FinanceTransactionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Transactions(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Transactions(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTransferAccountsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransferAccounts();
            var j = folioDapperContext.CountTransferAccounts();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransferAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesTransferAccountsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.TransferAccounts(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.TransferAccounts(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountTransferCriteriasTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransferCriterias();
            var j = folioDapperContext.CountTransferCriterias();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransferCriteriasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesTransferCriteriasTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.TransferCriterias(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.TransferCriterias(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteriasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUsers();
            var j = folioDapperContext.CountUsers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void UsersUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Users(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Users(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountUserAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUserAcquisitionsUnits();
            var j = folioDapperContext.CountUserAcquisitionsUnits();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUserAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void OrdersUserAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.UserAcquisitionsUnits(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.UserAcquisitionsUnits(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountUserRequestPreferencesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUserRequestPreferences();
            var j = folioDapperContext.CountUserRequestPreferences();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUserRequestPreferencesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CirculationUserRequestPreferencesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.UserRequestPreferences(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.UserRequestPreferences(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreferencesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountVouchersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountVouchers();
            var j = folioDapperContext.CountVouchers();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountVouchersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoicesVouchersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Vouchers(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.Vouchers(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VouchersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountVoucherItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountVoucherItems();
            var j = folioDapperContext.CountVoucherItems();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountVoucherItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void InvoicesVoucherItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.VoucherItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.VoucherItems(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void CountWaiveReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountWaiveReasons();
            var j = folioDapperContext.CountWaiveReasons();
            Assert.IsTrue(i == j);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountWaiveReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void FeesWaiveReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.WaiveReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            var l2 = folioDapperContext.WaiveReasons(take: take).Select(u => u.Id.ToString()).ToArray();
            Assert.IsTrue(l.SequenceEqual(l2));
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAcquisitionMethod2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.AcquisitionMethods(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var am2 = JsonConvert.DeserializeObject<AcquisitionMethod2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, am2.ToString());
            Assert.IsNotNull(am2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAcquisitionMethod2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAcquisitionsUnit2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.AcquisitionsUnits(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var au2 = JsonConvert.DeserializeObject<AcquisitionsUnit2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, au2.ToString());
            Assert.IsNotNull(au2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAcquisitionsUnit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAddressType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.AddressTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var at2 = JsonConvert.DeserializeObject<AddressType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, at2.ToString());
            Assert.IsNotNull(at2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAddressType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAdministrativeNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var an = JsonConvert.DeserializeObject<AdministrativeNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, an.ToString());
            Assert.IsNotNull(an);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAdministrativeNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAlert2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Alerts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var a2 = JsonConvert.DeserializeObject<Alert2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, a2.ToString());
            Assert.IsNotNull(a2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAlert2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAllocatedFromFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Funds(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var aff = JsonConvert.DeserializeObject<AllocatedFromFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, aff.ToString());
            Assert.IsNotNull(aff);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAllocatedFromFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAllocatedToFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Funds(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var atf = JsonConvert.DeserializeObject<AllocatedToFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, atf.ToString());
            Assert.IsNotNull(atf);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAllocatedToFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAlternativeTitleTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var at = JsonConvert.DeserializeObject<AlternativeTitle>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, at.ToString());
            Assert.IsNotNull(at);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAlternativeTitleTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeAlternativeTitleType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.AlternativeTitleTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var att2 = JsonConvert.DeserializeObject<AlternativeTitleType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, att2.ToString());
            Assert.IsNotNull(att2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeAlternativeTitleType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBatchGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BatchGroups(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bg2 = JsonConvert.DeserializeObject<BatchGroup2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bg2.ToString());
            Assert.IsNotNull(bg2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBatchGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBlock2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Blocks(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var b2 = JsonConvert.DeserializeObject<Block2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, b2.ToString());
            Assert.IsNotNull(b2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBlock2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBlockCondition2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BlockConditions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bc2 = JsonConvert.DeserializeObject<BlockCondition2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bc2.ToString());
            Assert.IsNotNull(bc2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBlockCondition2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBlockLimit2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BlockLimits(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bl2 = JsonConvert.DeserializeObject<BlockLimit2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bl2.ToString());
            Assert.IsNotNull(bl2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBlockLimit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBoundWithPart2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BoundWithParts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bwp2 = JsonConvert.DeserializeObject<BoundWithPart2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bwp2.ToString());
            Assert.IsNotNull(bwp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBoundWithPart2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBudget2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Budgets(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var b2 = JsonConvert.DeserializeObject<Budget2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, b2.ToString());
            Assert.IsNotNull(b2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBudget2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBudgetAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Budgets(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bau = JsonConvert.DeserializeObject<BudgetAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bau.ToString());
            Assert.IsNotNull(bau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBudgetAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBudgetExpenseClass2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BudgetExpenseClasses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bec2 = JsonConvert.DeserializeObject<BudgetExpenseClass2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bec2.ToString());
            Assert.IsNotNull(bec2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBudgetExpenseClass2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBudgetGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.BudgetGroups(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bg2 = JsonConvert.DeserializeObject<BudgetGroup2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bg2.ToString());
            Assert.IsNotNull(bg2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBudgetGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeBudgetTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Budgets(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var bt = JsonConvert.DeserializeObject<BudgetTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, bt.ToString());
            Assert.IsNotNull(bt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeBudgetTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCallNumberType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CallNumberTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cnt2 = JsonConvert.DeserializeObject<CallNumberType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cnt2.ToString());
            Assert.IsNotNull(cnt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCallNumberType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCampus2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Campuses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c2 = JsonConvert.DeserializeObject<Campus2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c2.ToString());
            Assert.IsNotNull(c2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCampus2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCancellationReason2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CancellationReasons(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cr2 = JsonConvert.DeserializeObject<CancellationReason2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cr2.ToString());
            Assert.IsNotNull(cr2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCancellationReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCategory2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Categories(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c2 = JsonConvert.DeserializeObject<Category2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c2.ToString());
            Assert.IsNotNull(c2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCategory2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCheckIn2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CheckIns(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ci2 = JsonConvert.DeserializeObject<CheckIn2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ci2.ToString());
            Assert.IsNotNull(ci2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCheckIn2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCirculationNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cn = JsonConvert.DeserializeObject<CirculationNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cn.ToString());
            Assert.IsNotNull(cn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCirculationNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeClassificationTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c = JsonConvert.DeserializeObject<Classification>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c.ToString());
            Assert.IsNotNull(c);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeClassificationTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeClassificationType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ClassificationTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ct2 = JsonConvert.DeserializeObject<ClassificationType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ct2.ToString());
            Assert.IsNotNull(ct2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeClassificationType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCloseReason2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CloseReasons(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cr2 = JsonConvert.DeserializeObject<CloseReason2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cr2.ToString());
            Assert.IsNotNull(cr2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCloseReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeComment2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Comments(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c2 = JsonConvert.DeserializeObject<Comment2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c2.ToString());
            Assert.IsNotNull(c2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeComment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeConfiguration2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Configurations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c2 = JsonConvert.DeserializeObject<Configuration2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c2.ToString());
            Assert.IsNotNull(c2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeConfiguration2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContact2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c2 = JsonConvert.DeserializeObject<Contact2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c2.ToString());
            Assert.IsNotNull(c2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContact2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactAddressTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ca = JsonConvert.DeserializeObject<ContactAddress>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ca.ToString());
            Assert.IsNotNull(ca);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactAddressTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactAddressCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cac = JsonConvert.DeserializeObject<ContactAddressCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cac.ToString());
            Assert.IsNotNull(cac);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactAddressCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cc = JsonConvert.DeserializeObject<ContactCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cc.ToString());
            Assert.IsNotNull(cc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactEmailTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ce = JsonConvert.DeserializeObject<ContactEmail>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ce.ToString());
            Assert.IsNotNull(ce);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactEmailTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactEmailCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cec = JsonConvert.DeserializeObject<ContactEmailCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cec.ToString());
            Assert.IsNotNull(cec);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactEmailCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactPhoneNumberTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cpn = JsonConvert.DeserializeObject<ContactPhoneNumber>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cpn.ToString());
            Assert.IsNotNull(cpn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactPhoneNumberTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactPhoneNumberCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cpnc = JsonConvert.DeserializeObject<ContactPhoneNumberCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cpnc.ToString());
            Assert.IsNotNull(cpnc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactPhoneNumberCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactUrlTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cu = JsonConvert.DeserializeObject<ContactUrl>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cu.ToString());
            Assert.IsNotNull(cu);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactUrlTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContactUrlCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Contacts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cuc = JsonConvert.DeserializeObject<ContactUrlCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cuc.ToString());
            Assert.IsNotNull(cuc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContactUrlCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContributorTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c = JsonConvert.DeserializeObject<Contributor>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c.ToString());
            Assert.IsNotNull(c);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContributorTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContributorNameType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ContributorNameTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cnt2 = JsonConvert.DeserializeObject<ContributorNameType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cnt2.ToString());
            Assert.IsNotNull(cnt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContributorNameType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeContributorType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ContributorTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ct2 = JsonConvert.DeserializeObject<ContributorType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ct2.ToString());
            Assert.IsNotNull(ct2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeContributorType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCurrencyTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var c = JsonConvert.DeserializeObject<Currency>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, c.ToString());
            Assert.IsNotNull(c);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCurrencyTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCustomField2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CustomFields(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cf2 = JsonConvert.DeserializeObject<CustomField2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cf2.ToString());
            Assert.IsNotNull(cf2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCustomField2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeCustomFieldValueTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.CustomFields(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var cfv = JsonConvert.DeserializeObject<CustomFieldValue>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, cfv.ToString());
            Assert.IsNotNull(cfv);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeCustomFieldValueTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeDepartment2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Departments(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var d2 = JsonConvert.DeserializeObject<Department2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, d2.ToString());
            Assert.IsNotNull(d2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeDepartment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeEditionTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var e2 = JsonConvert.DeserializeObject<Edition>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, e2.ToString());
            Assert.IsNotNull(e2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeEditionTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeElectronicAccessTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ea = JsonConvert.DeserializeObject<ElectronicAccess>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ea.ToString());
            Assert.IsNotNull(ea);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeElectronicAccessTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeElectronicAccessRelationship2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ElectronicAccessRelationships(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ear2 = JsonConvert.DeserializeObject<ElectronicAccessRelationship2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ear2.ToString());
            Assert.IsNotNull(ear2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeElectronicAccessRelationship2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeExpenseClass2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ExpenseClasses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ec2 = JsonConvert.DeserializeObject<ExpenseClass2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ec2.ToString());
            Assert.IsNotNull(ec2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeExpenseClass2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeExtentTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var e2 = JsonConvert.DeserializeObject<Extent>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, e2.ToString());
            Assert.IsNotNull(e2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeExtentTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFee2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Fees(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var f2 = JsonConvert.DeserializeObject<Fee2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, f2.ToString());
            Assert.IsNotNull(f2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFee2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFeeType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FeeTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ft2 = JsonConvert.DeserializeObject<FeeType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ft2.ToString());
            Assert.IsNotNull(ft2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFeeType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFinanceGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FinanceGroups(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fg2 = JsonConvert.DeserializeObject<FinanceGroup2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fg2.ToString());
            Assert.IsNotNull(fg2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFinanceGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFinanceGroupAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FinanceGroups(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fgau = JsonConvert.DeserializeObject<FinanceGroupAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fgau.ToString());
            Assert.IsNotNull(fgau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFinanceGroupAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFiscalYear2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FiscalYears(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fy2 = JsonConvert.DeserializeObject<FiscalYear2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fy2.ToString());
            Assert.IsNotNull(fy2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFiscalYear2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFiscalYearAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FiscalYears(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fyau = JsonConvert.DeserializeObject<FiscalYearAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fyau.ToString());
            Assert.IsNotNull(fyau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFiscalYearAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFixedDueDateSchedule2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FixedDueDateSchedules(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fdds2 = JsonConvert.DeserializeObject<FixedDueDateSchedule2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fdds2.ToString());
            Assert.IsNotNull(fdds2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFixedDueDateSchedule2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFixedDueDateScheduleScheduleTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FixedDueDateSchedules(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fddss = JsonConvert.DeserializeObject<FixedDueDateScheduleSchedule>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fddss.ToString());
            Assert.IsNotNull(fddss);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFixedDueDateScheduleScheduleTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFormatTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceFormats(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var f = JsonConvert.DeserializeObject<Format>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, f.ToString());
            Assert.IsNotNull(f);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFormatTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFund2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Funds(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var f2 = JsonConvert.DeserializeObject<Fund2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, f2.ToString());
            Assert.IsNotNull(f2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFund2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFundAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Funds(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var fau = JsonConvert.DeserializeObject<FundAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, fau.ToString());
            Assert.IsNotNull(fau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFundAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFundTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Funds(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ft = JsonConvert.DeserializeObject<FundTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ft.ToString());
            Assert.IsNotNull(ft);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFundTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeFundType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.FundTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ft2 = JsonConvert.DeserializeObject<FundType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ft2.ToString());
            Assert.IsNotNull(ft2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeFundType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeGroup2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Groups(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var g2 = JsonConvert.DeserializeObject<Group2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, g2.ToString());
            Assert.IsNotNull(g2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeGroup2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHolding2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var h2 = JsonConvert.DeserializeObject<Holding2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, h2.ToString());
            Assert.IsNotNull(h2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHolding2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingAdministrativeNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var han = JsonConvert.DeserializeObject<HoldingAdministrativeNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, han.ToString());
            Assert.IsNotNull(han);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingAdministrativeNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingElectronicAccessTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var hea = JsonConvert.DeserializeObject<HoldingElectronicAccess>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, hea.ToString());
            Assert.IsNotNull(hea);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingElectronicAccessTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingEntryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var he = JsonConvert.DeserializeObject<HoldingEntry>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, he.ToString());
            Assert.IsNotNull(he);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingEntryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingFormerIdTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var hfi = JsonConvert.DeserializeObject<HoldingFormerId>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, hfi.ToString());
            Assert.IsNotNull(hfi);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingFormerIdTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var hn = JsonConvert.DeserializeObject<HoldingNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, hn.ToString());
            Assert.IsNotNull(hn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.HoldingNoteTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var hnt2 = JsonConvert.DeserializeObject<HoldingNoteType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, hnt2.ToString());
            Assert.IsNotNull(hnt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingStatisticalCodeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var hsc = JsonConvert.DeserializeObject<HoldingStatisticalCode>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, hsc.ToString());
            Assert.IsNotNull(hsc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingStatisticalCodeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ht = JsonConvert.DeserializeObject<HoldingTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ht.ToString());
            Assert.IsNotNull(ht);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeHoldingType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.HoldingTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ht2 = JsonConvert.DeserializeObject<HoldingType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ht2.ToString());
            Assert.IsNotNull(ht2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeHoldingType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeIdentifierTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i = JsonConvert.DeserializeObject<Identifier>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i.ToString());
            Assert.IsNotNull(i);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeIdentifierTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeIdType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.IdTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it2 = JsonConvert.DeserializeObject<IdType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it2.ToString());
            Assert.IsNotNull(it2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeIdType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeIllPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.IllPolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ip2 = JsonConvert.DeserializeObject<IllPolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ip2.ToString());
            Assert.IsNotNull(ip2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeIllPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeIndexStatementTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var @is = JsonConvert.DeserializeObject<IndexStatement>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, @is.ToString());
            Assert.IsNotNull(@is);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeIndexStatementTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstance2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i2 = JsonConvert.DeserializeObject<Instance2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i2.ToString());
            Assert.IsNotNull(i2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstance2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceFormat2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var if2 = JsonConvert.DeserializeObject<InstanceFormat2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, if2.ToString());
            Assert.IsNotNull(if2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceFormat2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceNatureOfContentTermTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var inoct = JsonConvert.DeserializeObject<InstanceNatureOfContentTerm>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, inoct.ToString());
            Assert.IsNotNull(inoct);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceNatureOfContentTermTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var @in = JsonConvert.DeserializeObject<InstanceNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, @in.ToString());
            Assert.IsNotNull(@in);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceNoteTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var int2 = JsonConvert.DeserializeObject<InstanceNoteType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, int2.ToString());
            Assert.IsNotNull(int2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceStatisticalCodeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var isc = JsonConvert.DeserializeObject<InstanceStatisticalCode>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, isc.ToString());
            Assert.IsNotNull(isc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceStatisticalCodeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it = JsonConvert.DeserializeObject<InstanceTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it.ToString());
            Assert.IsNotNull(it);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstanceType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it2 = JsonConvert.DeserializeObject<InstanceType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it2.ToString());
            Assert.IsNotNull(it2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstanceType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInstitution2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Institutions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i2 = JsonConvert.DeserializeObject<Institution2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i2.ToString());
            Assert.IsNotNull(i2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInstitution2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInterface2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Interfaces(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i2 = JsonConvert.DeserializeObject<Interface2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i2.ToString());
            Assert.IsNotNull(i2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInterface2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInterfaceTypeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Interfaces(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it = JsonConvert.DeserializeObject<InterfaceType>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it.ToString());
            Assert.IsNotNull(it);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInterfaceTypeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoice2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i2 = JsonConvert.DeserializeObject<Invoice2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i2.ToString());
            Assert.IsNotNull(i2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iau = JsonConvert.DeserializeObject<InvoiceAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iau.ToString());
            Assert.IsNotNull(iau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceAdjustmentTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ia = JsonConvert.DeserializeObject<InvoiceAdjustment>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ia.ToString());
            Assert.IsNotNull(ia);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceAdjustmentTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceAdjustmentFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iaf = JsonConvert.DeserializeObject<InvoiceAdjustmentFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iaf.ToString());
            Assert.IsNotNull(iaf);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceAdjustmentFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItem2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ii2 = JsonConvert.DeserializeObject<InvoiceItem2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ii2.ToString());
            Assert.IsNotNull(ii2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItemAdjustmentTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iia = JsonConvert.DeserializeObject<InvoiceItemAdjustment>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iia.ToString());
            Assert.IsNotNull(iia);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItemAdjustmentTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItemAdjustmentFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iiaf = JsonConvert.DeserializeObject<InvoiceItemAdjustmentFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iiaf.ToString());
            Assert.IsNotNull(iiaf);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItemAdjustmentFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItemFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iif = JsonConvert.DeserializeObject<InvoiceItemFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iif.ToString());
            Assert.IsNotNull(iif);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItemFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItemReferenceNumberTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iirn = JsonConvert.DeserializeObject<InvoiceItemReferenceNumber>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iirn.ToString());
            Assert.IsNotNull(iirn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItemReferenceNumberTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceItemTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InvoiceItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iit = JsonConvert.DeserializeObject<InvoiceItemTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iit.ToString());
            Assert.IsNotNull(iit);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceItemTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceOrderNumberTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ion = JsonConvert.DeserializeObject<InvoiceOrderNumber>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ion.ToString());
            Assert.IsNotNull(ion);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceOrderNumberTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeInvoiceTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Invoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it = JsonConvert.DeserializeObject<InvoiceTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it.ToString());
            Assert.IsNotNull(it);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeInvoiceTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeIssuanceModeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ModeOfIssuances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var im = JsonConvert.DeserializeObject<IssuanceMode>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, im.ToString());
            Assert.IsNotNull(im);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeIssuanceModeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItem2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var i2 = JsonConvert.DeserializeObject<Item2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, i2.ToString());
            Assert.IsNotNull(i2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemAdministrativeNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ian = JsonConvert.DeserializeObject<ItemAdministrativeNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ian.ToString());
            Assert.IsNotNull(ian);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemAdministrativeNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemDamagedStatus2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ItemDamagedStatuses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ids2 = JsonConvert.DeserializeObject<ItemDamagedStatus2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ids2.ToString());
            Assert.IsNotNull(ids2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemDamagedStatus2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemElectronicAccessTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iea = JsonConvert.DeserializeObject<ItemElectronicAccess>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iea.ToString());
            Assert.IsNotNull(iea);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemElectronicAccessTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemFormerIdTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ifi = JsonConvert.DeserializeObject<ItemFormerId>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ifi.ToString());
            Assert.IsNotNull(ifi);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemFormerIdTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var @in = JsonConvert.DeserializeObject<ItemNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, @in.ToString());
            Assert.IsNotNull(@in);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ItemNoteTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var int2 = JsonConvert.DeserializeObject<ItemNoteType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, int2.ToString());
            Assert.IsNotNull(int2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemStatisticalCodeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var isc = JsonConvert.DeserializeObject<ItemStatisticalCode>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, isc.ToString());
            Assert.IsNotNull(isc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemStatisticalCodeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var it = JsonConvert.DeserializeObject<ItemTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, it.ToString());
            Assert.IsNotNull(it);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeItemYearCaptionTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Items(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var iyc = JsonConvert.DeserializeObject<ItemYearCaption>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, iyc.ToString());
            Assert.IsNotNull(iyc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeItemYearCaptionTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLanguageTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var l = JsonConvert.DeserializeObject<Language>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, l.ToString());
            Assert.IsNotNull(l);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLanguageTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedger2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Ledgers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var l2 = JsonConvert.DeserializeObject<Ledger2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, l2.ToString());
            Assert.IsNotNull(l2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedger2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Ledgers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lau = JsonConvert.DeserializeObject<LedgerAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lau.ToString());
            Assert.IsNotNull(lau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerRollover2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LedgerRollovers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lr2 = JsonConvert.DeserializeObject<LedgerRollover2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lr2.ToString());
            Assert.IsNotNull(lr2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerRollover2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerRolloverBudgetsRolloverTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LedgerRollovers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lrbr = JsonConvert.DeserializeObject<LedgerRolloverBudgetsRollover>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lrbr.ToString());
            Assert.IsNotNull(lrbr);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerRolloverBudgetsRolloverTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerRolloverEncumbrancesRolloverTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LedgerRollovers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lrer = JsonConvert.DeserializeObject<LedgerRolloverEncumbrancesRollover>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lrer.ToString());
            Assert.IsNotNull(lrer);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerRolloverEncumbrancesRolloverTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerRolloverError2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LedgerRolloverErrors(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lre2 = JsonConvert.DeserializeObject<LedgerRolloverError2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lre2.ToString());
            Assert.IsNotNull(lre2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerRolloverError2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLedgerRolloverProgress2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LedgerRolloverProgresses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lrp2 = JsonConvert.DeserializeObject<LedgerRolloverProgress2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lrp2.ToString());
            Assert.IsNotNull(lrp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLedgerRolloverProgress2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLibrary2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Libraries(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var l2 = JsonConvert.DeserializeObject<Library2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, l2.ToString());
            Assert.IsNotNull(l2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLibrary2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLoan2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Loans(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var l2 = JsonConvert.DeserializeObject<Loan2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, l2.ToString());
            Assert.IsNotNull(l2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLoan2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLoanPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LoanPolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lp2 = JsonConvert.DeserializeObject<LoanPolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lp2.ToString());
            Assert.IsNotNull(lp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLoanPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLoanType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LoanTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lt2 = JsonConvert.DeserializeObject<LoanType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lt2.ToString());
            Assert.IsNotNull(lt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLoanType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLocation2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Locations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var l2 = JsonConvert.DeserializeObject<Location2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, l2.ToString());
            Assert.IsNotNull(l2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLocation2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLocationServicePointTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Locations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lsp = JsonConvert.DeserializeObject<LocationServicePoint>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lsp.ToString());
            Assert.IsNotNull(lsp);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLocationServicePointTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeLostItemFeePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.LostItemFeePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var lifp2 = JsonConvert.DeserializeObject<LostItemFeePolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, lifp2.ToString());
            Assert.IsNotNull(lifp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeLostItemFeePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeManualBlockTemplate2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ManualBlockTemplates(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var mbt2 = JsonConvert.DeserializeObject<ManualBlockTemplate2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, mbt2.ToString());
            Assert.IsNotNull(mbt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeManualBlockTemplate2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeMaterialType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.MaterialTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var mt2 = JsonConvert.DeserializeObject<MaterialType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, mt2.ToString());
            Assert.IsNotNull(mt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeMaterialType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeNatureOfContentTerm2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.NatureOfContentTerms(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var noct2 = JsonConvert.DeserializeObject<NatureOfContentTerm2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, noct2.ToString());
            Assert.IsNotNull(noct2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeNatureOfContentTerm2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeNote2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Notes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var n2 = JsonConvert.DeserializeObject<Note2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, n2.ToString());
            Assert.IsNotNull(n2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeNote2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeNoteType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.NoteTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var nt2 = JsonConvert.DeserializeObject<NoteType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, nt2.ToString());
            Assert.IsNotNull(nt2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeNoteType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrder2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Orders(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var o2 = JsonConvert.DeserializeObject<Order2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, o2.ToString());
            Assert.IsNotNull(o2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrder2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Orders(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oau = JsonConvert.DeserializeObject<OrderAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oau.ToString());
            Assert.IsNotNull(oau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderInvoice2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderInvoices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oi2 = JsonConvert.DeserializeObject<OrderInvoice2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oi2.ToString());
            Assert.IsNotNull(oi2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderInvoice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItem2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oi2 = JsonConvert.DeserializeObject<OrderItem2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oi2.ToString());
            Assert.IsNotNull(oi2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemAlertTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oia = JsonConvert.DeserializeObject<OrderItemAlert>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oia.ToString());
            Assert.IsNotNull(oia);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemAlertTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemClaimTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oic = JsonConvert.DeserializeObject<OrderItemClaim>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oic.ToString());
            Assert.IsNotNull(oic);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemClaimTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemContributorTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oic = JsonConvert.DeserializeObject<OrderItemContributor>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oic.ToString());
            Assert.IsNotNull(oic);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemContributorTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oif = JsonConvert.DeserializeObject<OrderItemFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oif.ToString());
            Assert.IsNotNull(oif);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemLocation2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oil2 = JsonConvert.DeserializeObject<OrderItemLocation2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oil2.ToString());
            Assert.IsNotNull(oil2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemLocation2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemProductIdTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oipi = JsonConvert.DeserializeObject<OrderItemProductId>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oipi.ToString());
            Assert.IsNotNull(oipi);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemProductIdTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemReferenceNumberTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oirn = JsonConvert.DeserializeObject<OrderItemReferenceNumber>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oirn.ToString());
            Assert.IsNotNull(oirn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemReferenceNumberTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemReportingCodeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oirc = JsonConvert.DeserializeObject<OrderItemReportingCode>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oirc.ToString());
            Assert.IsNotNull(oirc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemReportingCodeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oit = JsonConvert.DeserializeObject<OrderItemTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oit.ToString());
            Assert.IsNotNull(oit);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderItemVolumeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oiv = JsonConvert.DeserializeObject<OrderItemVolume>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oiv.ToString());
            Assert.IsNotNull(oiv);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderItemVolumeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderNoteTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Orders(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var @on = JsonConvert.DeserializeObject<OrderNote>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, @on.ToString());
            Assert.IsNotNull(@on);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderNoteTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Orders(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ot = JsonConvert.DeserializeObject<OrderTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ot.ToString());
            Assert.IsNotNull(ot);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrderTemplate2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OrderTemplates(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ot2 = JsonConvert.DeserializeObject<OrderTemplate2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ot2.ToString());
            Assert.IsNotNull(ot2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrderTemplate2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganization2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var o2 = JsonConvert.DeserializeObject<Organization2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, o2.ToString());
            Assert.IsNotNull(o2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganization2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAccountTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oa = JsonConvert.DeserializeObject<OrganizationAccount>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oa.ToString());
            Assert.IsNotNull(oa);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAccountTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAccountAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oaau = JsonConvert.DeserializeObject<OrganizationAccountAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oaau.ToString());
            Assert.IsNotNull(oaau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAccountAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oau = JsonConvert.DeserializeObject<OrganizationAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oau.ToString());
            Assert.IsNotNull(oau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAddressTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oa = JsonConvert.DeserializeObject<OrganizationAddress>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oa.ToString());
            Assert.IsNotNull(oa);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAddressTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAddressCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oac = JsonConvert.DeserializeObject<OrganizationAddressCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oac.ToString());
            Assert.IsNotNull(oac);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAddressCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAgreementTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oa = JsonConvert.DeserializeObject<OrganizationAgreement>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oa.ToString());
            Assert.IsNotNull(oa);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAgreementTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationAliasTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oa = JsonConvert.DeserializeObject<OrganizationAlias>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oa.ToString());
            Assert.IsNotNull(oa);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationAliasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationChangelogTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oc = JsonConvert.DeserializeObject<OrganizationChangelog>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oc.ToString());
            Assert.IsNotNull(oc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationChangelogTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationContactTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oc = JsonConvert.DeserializeObject<OrganizationContact>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oc.ToString());
            Assert.IsNotNull(oc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationContactTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationEmailTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oe = JsonConvert.DeserializeObject<OrganizationEmail>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oe.ToString());
            Assert.IsNotNull(oe);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationEmailTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationEmailCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oec = JsonConvert.DeserializeObject<OrganizationEmailCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oec.ToString());
            Assert.IsNotNull(oec);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationEmailCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationInterfaceTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var oi = JsonConvert.DeserializeObject<OrganizationInterface>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, oi.ToString());
            Assert.IsNotNull(oi);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationInterfaceTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationPhoneNumberTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var opn = JsonConvert.DeserializeObject<OrganizationPhoneNumber>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, opn.ToString());
            Assert.IsNotNull(opn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationPhoneNumberTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationPhoneNumberCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var opnc = JsonConvert.DeserializeObject<OrganizationPhoneNumberCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, opnc.ToString());
            Assert.IsNotNull(opnc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationPhoneNumberCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ot = JsonConvert.DeserializeObject<OrganizationTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ot.ToString());
            Assert.IsNotNull(ot);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationTypeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ot = JsonConvert.DeserializeObject<OrganizationType>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ot.ToString());
            Assert.IsNotNull(ot);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationTypeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationUrlTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ou = JsonConvert.DeserializeObject<OrganizationUrl>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ou.ToString());
            Assert.IsNotNull(ou);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationUrlTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOrganizationUrlCategoryTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Organizations(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ouc = JsonConvert.DeserializeObject<OrganizationUrlCategory>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ouc.ToString());
            Assert.IsNotNull(ouc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOrganizationUrlCategoryTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOverdueFinePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.OverdueFinePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ofp2 = JsonConvert.DeserializeObject<OverdueFinePolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ofp2.ToString());
            Assert.IsNotNull(ofp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOverdueFinePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeOwner2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Owners(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var o2 = JsonConvert.DeserializeObject<Owner2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, o2.ToString());
            Assert.IsNotNull(o2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeOwner2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePatronActionSession2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PatronActionSessions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pas2 = JsonConvert.DeserializeObject<PatronActionSession2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pas2.ToString());
            Assert.IsNotNull(pas2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePatronActionSession2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePatronNoticePolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PatronNoticePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pnp2 = JsonConvert.DeserializeObject<PatronNoticePolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pnp2.ToString());
            Assert.IsNotNull(pnp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePatronNoticePolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePatronNoticePolicyFeeFineNoticeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PatronNoticePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pnpffn = JsonConvert.DeserializeObject<PatronNoticePolicyFeeFineNotice>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pnpffn.ToString());
            Assert.IsNotNull(pnpffn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePatronNoticePolicyFeeFineNoticeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePatronNoticePolicyLoanNoticeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PatronNoticePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pnpln = JsonConvert.DeserializeObject<PatronNoticePolicyLoanNotice>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pnpln.ToString());
            Assert.IsNotNull(pnpln);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePatronNoticePolicyLoanNoticeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePatronNoticePolicyRequestNoticeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PatronNoticePolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pnprn = JsonConvert.DeserializeObject<PatronNoticePolicyRequestNotice>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pnprn.ToString());
            Assert.IsNotNull(pnprn);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePatronNoticePolicyRequestNoticeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePayment2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Payments(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var p2 = JsonConvert.DeserializeObject<Payment2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, p2.ToString());
            Assert.IsNotNull(p2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePayment2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePaymentMethod2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PaymentMethods(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pm2 = JsonConvert.DeserializeObject<PaymentMethod2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pm2.ToString());
            Assert.IsNotNull(pm2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePaymentMethod2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermission2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Permissions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var p2 = JsonConvert.DeserializeObject<Permission2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, p2.ToString());
            Assert.IsNotNull(p2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermission2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionChildOfTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Permissions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pco = JsonConvert.DeserializeObject<PermissionChildOf>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pco.ToString());
            Assert.IsNotNull(pco);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionChildOfTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionGrantedToTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Permissions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pgt = JsonConvert.DeserializeObject<PermissionGrantedTo>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pgt.ToString());
            Assert.IsNotNull(pgt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionGrantedToTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionSubPermissionTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Permissions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var psp = JsonConvert.DeserializeObject<PermissionSubPermission>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, psp.ToString());
            Assert.IsNotNull(psp);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionSubPermissionTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionsUser2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PermissionsUsers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pu2 = JsonConvert.DeserializeObject<PermissionsUser2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pu2.ToString());
            Assert.IsNotNull(pu2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionsUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionsUserPermissionTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PermissionsUsers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pup = JsonConvert.DeserializeObject<PermissionsUserPermission>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pup.ToString());
            Assert.IsNotNull(pup);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionsUserPermissionTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePermissionTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Permissions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pt = JsonConvert.DeserializeObject<PermissionTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pt.ToString());
            Assert.IsNotNull(pt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePermissionTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePhysicalDescriptionTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pd = JsonConvert.DeserializeObject<PhysicalDescription>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pd.ToString());
            Assert.IsNotNull(pd);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePhysicalDescriptionTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePrecedingSucceedingTitle2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PrecedingSucceedingTitles(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pst2 = JsonConvert.DeserializeObject<PrecedingSucceedingTitle2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pst2.ToString());
            Assert.IsNotNull(pst2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePrecedingSucceedingTitle2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePrecedingSucceedingTitleIdentifierTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.PrecedingSucceedingTitles(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var psti = JsonConvert.DeserializeObject<PrecedingSucceedingTitleIdentifier>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, psti.ToString());
            Assert.IsNotNull(psti);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePrecedingSucceedingTitleIdentifierTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePrefix2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Prefixes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var p2 = JsonConvert.DeserializeObject<Prefix2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, p2.ToString());
            Assert.IsNotNull(p2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePrefix2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeProxy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Proxies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var p2 = JsonConvert.DeserializeObject<Proxy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, p2.ToString());
            Assert.IsNotNull(p2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeProxy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePublicationTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var p = JsonConvert.DeserializeObject<Publication>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, p.ToString());
            Assert.IsNotNull(p);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePublicationTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePublicationFrequencyTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pf = JsonConvert.DeserializeObject<PublicationFrequency>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pf.ToString());
            Assert.IsNotNull(pf);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePublicationFrequencyTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializePublicationRangeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var pr = JsonConvert.DeserializeObject<PublicationRange>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, pr.ToString());
            Assert.IsNotNull(pr);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializePublicationRangeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeReceiving2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Receivings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var r2 = JsonConvert.DeserializeObject<Receiving2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, r2.ToString());
            Assert.IsNotNull(r2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeReceiving2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRecord2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Records(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var r2 = JsonConvert.DeserializeObject<Record2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, r2.ToString());
            Assert.IsNotNull(r2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRecord2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRefundReason2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.RefundReasons(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rr2 = JsonConvert.DeserializeObject<RefundReason2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rr2.ToString());
            Assert.IsNotNull(rr2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRefundReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRelationshipTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceRelationships(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var r = JsonConvert.DeserializeObject<Relationship>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, r.ToString());
            Assert.IsNotNull(r);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRelationshipTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRelationshipTypeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceRelationshipTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rt = JsonConvert.DeserializeObject<RelationshipType>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rt.ToString());
            Assert.IsNotNull(rt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRelationshipTypeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeReportingCode2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ReportingCodes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rc2 = JsonConvert.DeserializeObject<ReportingCode2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rc2.ToString());
            Assert.IsNotNull(rc2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeReportingCode2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRequest2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Requests(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var r2 = JsonConvert.DeserializeObject<Request2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, r2.ToString());
            Assert.IsNotNull(r2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRequest2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRequestIdentifierTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Requests(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ri = JsonConvert.DeserializeObject<RequestIdentifier>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ri.ToString());
            Assert.IsNotNull(ri);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRequestIdentifierTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRequestPolicy2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.RequestPolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rp2 = JsonConvert.DeserializeObject<RequestPolicy2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rp2.ToString());
            Assert.IsNotNull(rp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRequestPolicy2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRequestPolicyRequestTypeTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.RequestPolicies(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rprt = JsonConvert.DeserializeObject<RequestPolicyRequestType>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rprt.ToString());
            Assert.IsNotNull(rprt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRequestPolicyRequestTypeTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeRequestTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Requests(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var rt = JsonConvert.DeserializeObject<RequestTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, rt.ToString());
            Assert.IsNotNull(rt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeRequestTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeScheduledNotice2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ScheduledNotices(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var sn2 = JsonConvert.DeserializeObject<ScheduledNotice2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, sn2.ToString());
            Assert.IsNotNull(sn2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeScheduledNotice2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSeriesTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Series>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSeriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeServicePoint2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ServicePoints(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var sp2 = JsonConvert.DeserializeObject<ServicePoint2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, sp2.ToString());
            Assert.IsNotNull(sp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeServicePoint2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeServicePointOwnerTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Owners(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var spo = JsonConvert.DeserializeObject<ServicePointOwner>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, spo.ToString());
            Assert.IsNotNull(spo);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeServicePointOwnerTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeServicePointStaffSlipTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ServicePoints(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var spss = JsonConvert.DeserializeObject<ServicePointStaffSlip>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, spss.ToString());
            Assert.IsNotNull(spss);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeServicePointStaffSlipTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeServicePointUser2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ServicePointUsers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var spu2 = JsonConvert.DeserializeObject<ServicePointUser2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, spu2.ToString());
            Assert.IsNotNull(spu2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeServicePointUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeServicePointUserServicePointTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.ServicePointUsers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var spusp = JsonConvert.DeserializeObject<ServicePointUserServicePoint>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, spusp.ToString());
            Assert.IsNotNull(spusp);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeServicePointUserServicePointTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSnapshot2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Snapshots(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Snapshot2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSnapshot2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSource2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Sources(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Source2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSource2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeStaffSlip2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.StaffSlips(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ss2 = JsonConvert.DeserializeObject<StaffSlip2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ss2.ToString());
            Assert.IsNotNull(ss2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeStaffSlip2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeStatisticalCode2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.StatisticalCodes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var sc2 = JsonConvert.DeserializeObject<StatisticalCode2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, sc2.ToString());
            Assert.IsNotNull(sc2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeStatisticalCode2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeStatisticalCodeType2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.StatisticalCodeTypes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var sct2 = JsonConvert.DeserializeObject<StatisticalCodeType2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, sct2.ToString());
            Assert.IsNotNull(sct2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeStatisticalCodeType2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeStatusTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.InstanceStatuses(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Status>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeStatusTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSubjectTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Instances(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Subject>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSubjectTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSuffix2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Suffixes(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var s2 = JsonConvert.DeserializeObject<Suffix2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, s2.ToString());
            Assert.IsNotNull(s2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSuffix2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeSupplementStatementTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Holdings(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ss = JsonConvert.DeserializeObject<SupplementStatement>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ss.ToString());
            Assert.IsNotNull(ss);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeSupplementStatementTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTag2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Tags(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var t2 = JsonConvert.DeserializeObject<Tag2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, t2.ToString());
            Assert.IsNotNull(t2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTag2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTemplate2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Templates(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var t2 = JsonConvert.DeserializeObject<Template2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, t2.ToString());
            Assert.IsNotNull(t2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTemplate2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTemplateOutputFormatTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Templates(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var tof = JsonConvert.DeserializeObject<TemplateOutputFormat>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, tof.ToString());
            Assert.IsNotNull(tof);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTemplateOutputFormatTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTitle2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Titles(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var t2 = JsonConvert.DeserializeObject<Title2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, t2.ToString());
            Assert.IsNotNull(t2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTitle2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTitleContributorTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Titles(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var tc = JsonConvert.DeserializeObject<TitleContributor>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, tc.ToString());
            Assert.IsNotNull(tc);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTitleContributorTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTitleProductIdTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Titles(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var tpi = JsonConvert.DeserializeObject<TitleProductId>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, tpi.ToString());
            Assert.IsNotNull(tpi);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTitleProductIdTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTransaction2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Transactions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var t2 = JsonConvert.DeserializeObject<Transaction2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, t2.ToString());
            Assert.IsNotNull(t2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTransaction2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTransactionTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Transactions(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var tt = JsonConvert.DeserializeObject<TransactionTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, tt.ToString());
            Assert.IsNotNull(tt);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTransactionTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTransferAccount2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.TransferAccounts(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ta2 = JsonConvert.DeserializeObject<TransferAccount2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ta2.ToString());
            Assert.IsNotNull(ta2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTransferAccount2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeTransferCriteria2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.TransferCriterias(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var tc2 = JsonConvert.DeserializeObject<TransferCriteria2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, tc2.ToString());
            Assert.IsNotNull(tc2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeTransferCriteria2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUser2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Users(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var u2 = JsonConvert.DeserializeObject<User2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, u2.ToString());
            Assert.IsNotNull(u2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUser2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUserAcquisitionsUnit2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.UserAcquisitionsUnits(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var uau2 = JsonConvert.DeserializeObject<UserAcquisitionsUnit2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, uau2.ToString());
            Assert.IsNotNull(uau2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUserAcquisitionsUnit2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUserAddressTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Users(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ua = JsonConvert.DeserializeObject<UserAddress>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ua.ToString());
            Assert.IsNotNull(ua);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUserAddressTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUserDepartmentTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Users(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ud = JsonConvert.DeserializeObject<UserDepartment>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ud.ToString());
            Assert.IsNotNull(ud);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUserDepartmentTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUserRequestPreference2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.UserRequestPreferences(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var urp2 = JsonConvert.DeserializeObject<UserRequestPreference2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, urp2.ToString());
            Assert.IsNotNull(urp2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUserRequestPreference2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeUserTagTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Users(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var ut = JsonConvert.DeserializeObject<UserTag>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, ut.ToString());
            Assert.IsNotNull(ut);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeUserTagTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeVoucher2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Vouchers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var v2 = JsonConvert.DeserializeObject<Voucher2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, v2.ToString());
            Assert.IsNotNull(v2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeVoucher2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeVoucherAcquisitionsUnitTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.Vouchers(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var vau = JsonConvert.DeserializeObject<VoucherAcquisitionsUnit>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, vau.ToString());
            Assert.IsNotNull(vau);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeVoucherAcquisitionsUnitTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeVoucherItem2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.VoucherItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var vi2 = JsonConvert.DeserializeObject<VoucherItem2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, vi2.ToString());
            Assert.IsNotNull(vi2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeVoucherItem2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeVoucherItemFundTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.VoucherItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var vif = JsonConvert.DeserializeObject<VoucherItemFund>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, vif.ToString());
            Assert.IsNotNull(vif);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeVoucherItemFundTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeVoucherItemInvoiceItemTest()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.VoucherItems(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var viii = JsonConvert.DeserializeObject<VoucherItemInvoiceItem>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, viii.ToString());
            Assert.IsNotNull(viii);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeVoucherItemInvoiceItemTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void DeserializeWaiveReason2Test()
        {
            var s = Stopwatch.StartNew();
            var jo = folioServiceClient.WaiveReasons(take: 1).SingleOrDefault();
            if (jo == null) Assert.Inconclusive();
            var wr2 = JsonConvert.DeserializeObject<WaiveReason2>(jo.ToString());
            traceSource.TraceEvent(TraceEventType.Information, 0, wr2.ToString());
            Assert.IsNotNull(wr2);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DeserializeWaiveReason2Test()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioDapperContext.Dispose();
            folioServiceClient.Dispose();
        }
    }
}
