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
        private readonly static int? take = 100;

        static FolioServiceClientTest()
        {
            TraceConfiguration.Register();
        }

        [TestMethod]
        public void Orders_CountAcquisitionMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAcquisitionMethods();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAcquisitionMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryAcquisitionMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AcquisitionMethods(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAcquisitionsUnits();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AcquisitionsUnits(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountActualCostRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountActualCostRecords();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountActualCostRecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryActualCostRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ActualCostRecords(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountAddressTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAddressTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAddressTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryAddressTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AddressTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_CountAgreementsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAgreements();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAgreementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_QueryAgreementsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Agreements(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_CountAgreementItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAgreementItems();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAgreementItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_QueryAgreementItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AgreementItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountAlternativeTitleTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountAlternativeTitleTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountAlternativeTitleTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryAlternativeTitleTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.AlternativeTitleTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountBatchGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBatchGroups();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBatchGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryBatchGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BatchGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountBlocksTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlocks();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlocksTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryBlocksTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Blocks(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlocksTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountBlockConditionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlockConditions();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlockConditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryBlockConditionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BlockConditions(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockConditionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountBlockLimitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBlockLimits();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBlockLimitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryBlockLimitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BlockLimits(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountBoundWithPartsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBoundWithParts();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBoundWithPartsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryBoundWithPartsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BoundWithParts(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPartsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgets();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Budgets(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountBudgetExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgetExpenseClasses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryBudgetExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BudgetExpenseClasses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountBudgetGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountBudgetGroups();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountBudgetGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryBudgetGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.BudgetGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountCallNumberTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCallNumberTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCallNumberTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryCallNumberTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CallNumberTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountCampusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCampuses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCampusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryCampusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Campuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CampusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountCancellationReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCancellationReasons();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCancellationReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryCancellationReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CancellationReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_CountCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCategories();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_QueryCategoriesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Categories(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CategoriesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountCheckInsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCheckIns();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCheckInsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryCheckInsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CheckIns(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckInsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountClassificationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountClassificationTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountClassificationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryClassificationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ClassificationTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountCloseReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCloseReasons();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCloseReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryCloseReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CloseReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountCommentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountComments();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCommentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryCommentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Comments(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CommentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_CountConfigurationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountConfigurations();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountConfigurationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Configuration_QueryConfigurationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Configurations(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ConfigurationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_CountContactsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContacts();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_QueryContactsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Contacts(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContactsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountContributorNameTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContributorNameTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContributorNameTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryContributorNameTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ContributorNameTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountContributorTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountContributorTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountContributorTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryContributorTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ContributorTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountCustomFieldsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountCustomFields();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountCustomFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryCustomFieldsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.CustomFields(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomFieldsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountDateTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountDateTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountDateTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryDateTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.DateTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DateTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountDepartments();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountDepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryDepartmentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Departments(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DepartmentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountDocumentsTest()
        {
            var s = Stopwatch.StartNew();
            Assert.Inconclusive();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountDocumentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryDocumentsTest()
        {
            var s = Stopwatch.StartNew();
            Assert.Inconclusive();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"DocumentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountElectronicAccessRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountElectronicAccessRelationships();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountElectronicAccessRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryElectronicAccessRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ElectronicAccessRelationships(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountExpenseClasses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryExpenseClassesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ExpenseClasses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClassesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountFeesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFees();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFeesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryFeesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Fees(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountFeeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFeeTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFeeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryFeeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FeeTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountFinanceGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFinanceGroups();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFinanceGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryFinanceGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FinanceGroups(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountFiscalYearsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFiscalYears();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFiscalYearsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryFiscalYearsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FiscalYears(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYearsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountFixedDueDateSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFixedDueDateSchedules();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFixedDueDateSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryFixedDueDateSchedulesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FixedDueDateSchedules(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedulesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountFundsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFunds();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryFundsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Funds(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountFundTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountFundTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountFundTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryFundTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.FundTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"FundTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountGroups();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountGroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryGroupsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Groups(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"GroupsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountHoldingsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldings();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryHoldingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Holdings(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountHoldingNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldingNoteTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryHoldingNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.HoldingNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountHoldingTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountHoldingTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountHoldingTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryHoldingTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.HoldingTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountIdTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountIdTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountIdTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryIdTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.IdTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IdTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountIllPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountIllPolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountIllPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryIllPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.IllPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstancesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstances();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstancesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Instances(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceFormatsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceFormats();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceFormatsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceFormats(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceFormatsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceNoteTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceRelationships();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceRelationshipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceRelationships(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceRelationshipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceRelationshipTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceRelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceRelationshipTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceRelationshipTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceRelationshipTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceStatuses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceStatuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstanceTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstanceTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InstanceTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountInstitutionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInstitutions();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInstitutionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryInstitutionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Institutions(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InstitutionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_CountInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInterfaces();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_QueryInterfacesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Interfaces(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InterfacesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInvoices();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Invoices(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountInvoiceItems();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountInvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryInvoiceItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.InvoiceItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItems();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Items(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountItemDamagedStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItemDamagedStatuses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemDamagedStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryItemDamagedStatusesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ItemDamagedStatuses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatusesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountItemNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountItemNoteTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountItemNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryItemNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ItemNoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountLedgersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLedgers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLedgersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryLedgersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Ledgers(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LedgersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountLibrariesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLibraries();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLibrariesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryLibrariesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Libraries(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LibrariesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountLoansTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoans();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryLoansTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Loans(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoansTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountLoanPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoanPolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoanPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryLoanPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LoanPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountLoanTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLoanTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLoanTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryLoanTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LoanTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountLocationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLocations();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryLocationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Locations(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountLostItemFeePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountLostItemFeePolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountLostItemFeePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryLostItemFeePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.LostItemFeePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountManualBlockTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountManualBlockTemplates();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountManualBlockTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryManualBlockTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ManualBlockTemplates(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountMaterialTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountMaterialTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountMaterialTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryMaterialTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.MaterialTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountModeOfIssuancesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountModeOfIssuances();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountModeOfIssuancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryModeOfIssuancesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ModeOfIssuances(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ModeOfIssuancesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNatureOfContentTerms();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryNatureOfContentTermsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.NatureOfContentTerms(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTermsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_CountNotesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNotes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_QueryNotesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Notes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NotesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_CountNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountNoteTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountNoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Notes_QueryNoteTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.NoteTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"NoteTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountOrdersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrders();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrdersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryOrdersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Orders(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrdersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountOrderInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrderInvoices();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrderInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryOrderInvoicesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrderInvoices(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoicesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrderItems();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryOrderItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrderItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_CountOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrganizations();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_QueryOrganizationsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Organizations(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_CountOrganizationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOrganizationTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOrganizationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Organizations_QueryOrganizationTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OrganizationTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OrganizationTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountOverdueFinePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOverdueFinePolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOverdueFinePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryOverdueFinePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.OverdueFinePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountOwnersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountOwners();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountOwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryOwnersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Owners(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"OwnersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountPatronActionSessionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPatronActionSessions();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPatronActionSessionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryPatronActionSessionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PatronActionSessions(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSessionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountPatronNoticePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPatronNoticePolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPatronNoticePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryPatronNoticePoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PatronNoticePolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountPaymentsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPayments();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPaymentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryPaymentsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Payments(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountPaymentMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPaymentMethods();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPaymentMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryPaymentMethodsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PaymentMethods(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethodsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_CountPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPermissions();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_QueryPermissionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Permissions(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_CountPermissionsUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPermissionsUsers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPermissionsUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Permissions_QueryPermissionsUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PermissionsUsers(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountPrecedingSucceedingTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountPrecedingSucceedingTitles();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountPrecedingSucceedingTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryPrecedingSucceedingTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.PrecedingSucceedingTitles(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountProxiesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountProxies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountProxiesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryProxiesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Proxies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ProxiesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountReceivingsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountReceivings();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountReceivingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryReceivingsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Receivings(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReceivingsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_CountRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRecords();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_QueryRecordsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Records(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RecordsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_CountReferenceDatasTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountReferenceDatas();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountReferenceDatasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Agreements_QueryReferenceDatasTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ReferenceDatas(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ReferenceDatasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountRefundReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRefundReasons();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRefundReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryRefundReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RefundReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountRequestsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRequests();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRequestsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryRequestsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Requests(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountRequestPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRequestPolicies();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRequestPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryRequestPoliciesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RequestPolicies(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPoliciesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountRolloversTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRollovers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryRolloversTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Rollovers(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloversTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountRolloverBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRolloverBudgets();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRolloverBudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryRolloverBudgetsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RolloverBudgets(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudgetsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountRolloverErrorsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRolloverErrors();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRolloverErrorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryRolloverErrorsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RolloverErrors(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverErrorsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountRolloverProgressesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountRolloverProgresses();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountRolloverProgressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryRolloverProgressesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.RolloverProgresses(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverProgressesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountScheduledNoticesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountScheduledNotices();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountScheduledNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryScheduledNoticesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ScheduledNotices(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNoticesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountServicePoints();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryServicePointsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ServicePoints(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountServicePointUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountServicePointUsers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountServicePointUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryServicePointUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.ServicePointUsers(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_CountSnapshotsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSnapshots();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSnapshotsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Source_QuerySnapshotsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Snapshots(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SnapshotsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountSourcesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSources();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QuerySourcesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Sources(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStaffSlips();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryStaffSlipsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StaffSlips(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlipsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStatisticalCodes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryStatisticalCodesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StatisticalCodes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountStatisticalCodeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountStatisticalCodeTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountStatisticalCodeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QueryStatisticalCodeTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.StatisticalCodeTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountSubjectSourcesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSubjectSources();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSubjectSourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QuerySubjectSourcesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.SubjectSources(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectSourcesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_CountSubjectTypesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountSubjectTypes();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountSubjectTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Inventory_QuerySubjectTypesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.SubjectTypes(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectTypesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Tags_CountTagsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTags();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Tags_QueryTagsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Tags(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TagsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Templates_CountTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTemplates();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Templates_QueryTemplatesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Templates(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TemplatesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTitles();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryTitlesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Titles(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TitlesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_CountTransactionsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransactions();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransactionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Finance_QueryTransactionsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Transactions(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransactionsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountTransferAccountsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransferAccounts();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransferAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryTransferAccountsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.TransferAccounts(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccountsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountTransferCriteriasTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountTransferCriterias();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountTransferCriteriasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryTransferCriteriasTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.TransferCriterias(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteriasTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_CountUsersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUsers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Users_QueryUsersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Users(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UsersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_CountUserAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUserAcquisitionsUnits();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUserAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Orders_QueryUserAcquisitionsUnitsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.UserAcquisitionsUnits(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnitsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_CountUserRequestPreferencesTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountUserRequestPreferences();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountUserRequestPreferencesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Circulation_QueryUserRequestPreferencesTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.UserRequestPreferences(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreferencesTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountVouchersTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountVouchers();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountVouchersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryVouchersTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.Vouchers(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VouchersTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_CountVoucherItemsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountVoucherItems();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountVoucherItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Invoices_QueryVoucherItemsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.VoucherItems(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItemsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_CountWaiveReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var i = folioServiceClient.CountWaiveReasons();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"CountWaiveReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [TestMethod]
        public void Fees_QueryWaiveReasonsTest()
        {
            var s = Stopwatch.StartNew();
            var l = folioServiceClient.WaiveReasons(take: take).Select(jo => (string)jo["id"]).ToArray();
            traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReasonsTest()\r\n    ElapsedTime={s.Elapsed}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            folioDapperContext.Dispose();
            folioServiceClient.Dispose();
        }
    }
}
