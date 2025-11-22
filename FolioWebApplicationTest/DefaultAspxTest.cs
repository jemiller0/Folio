using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;

namespace FolioWebApplicationTest
{
    [TestClass]
    public class DefaultAspxTest
    {
        private readonly static HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }) { /*Timeout = Timeout.InfiniteTimeSpan*/ };
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplicationTest", SourceLevels.Information);
        private static string Url { get; set; } = ConfigurationManager.AppSettings["url"];

        [TestMethod]
        public void AcquisitionMethod2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/AcquisitionMethod2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionMethod2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AcquisitionsUnit2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/AcquisitionsUnit2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ActualCostRecord2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ActualCostRecord2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ActualCostRecord2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AddressType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/AddressType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Agreement2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Agreement2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Agreement2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AgreementItem2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/AgreementItem2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AgreementItem2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AlternativeTitleType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/AlternativeTitleType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BatchGroup2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BatchGroup2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Block2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Block2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BlockCondition2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BlockCondition2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BlockLimit2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BlockLimit2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BoundWithPart2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BoundWithPart2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BoundWithPart2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Budget2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Budget2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BudgetExpenseClass2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BudgetExpenseClass2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BudgetGroup2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/BudgetGroup2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetGroup2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CallNumberType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/CallNumberType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Campus2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Campus2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CancellationReason2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/CancellationReason2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Category2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Category2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CheckIn2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/CheckIn2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ClassificationType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ClassificationType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CloseReason2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/CloseReason2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Comment2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Comment2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Configuration2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Configuration2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Contact2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Contact2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ContributorNameType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ContributorNameType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ContributorType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ContributorType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CustomField2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/CustomField2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void DateType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/DateType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"DateType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Department2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Department2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ElectronicAccessRelationship2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ElectronicAccessRelationship2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ExpenseClass2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ExpenseClass2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Fee2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Fee2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FeeType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/FeeType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FinanceGroup2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/FinanceGroup2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FiscalYear2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/FiscalYear2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FixedDueDateSchedule2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/FixedDueDateSchedule2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FormatsDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Formats/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Fund2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Fund2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FundType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/FundType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Group2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Group2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Holding2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Holding2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void HoldingNoteType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/HoldingNoteType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void HoldingType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/HoldingType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IdType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/IdType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IllPolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/IllPolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Instance2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Instance2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InstanceNoteType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/InstanceNoteType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InstanceType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/InstanceType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Institution2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Institution2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Interface2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Interface2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Invoice2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Invoice2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InvoiceItem2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/InvoiceItem2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IssuanceModesDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/IssuanceModes/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Item2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Item2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ItemDamagedStatus2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ItemDamagedStatus2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ItemNoteType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ItemNoteType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Ledger2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Ledger2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Library2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Library2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Loan2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Loan2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LoanPolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/LoanPolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LoanType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/LoanType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Location2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Location2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LocationSettingsDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/LocationSettings/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LostItemFeePolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/LostItemFeePolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ManualBlockTemplate2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ManualBlockTemplate2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ManualBlockTemplate2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void MaterialType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/MaterialType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void NatureOfContentTerm2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/NatureOfContentTerm2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Order2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Order2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderInvoice2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/OrderInvoice2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderItem2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/OrderItem2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Organization2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Organization2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OverdueFinePolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/OverdueFinePolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Owner2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Owner2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PatronActionSession2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/PatronActionSession2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PatronNoticePolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/PatronNoticePolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Payment2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Payment2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PaymentMethod2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/PaymentMethod2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Permission2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Permission2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PermissionsUser2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/PermissionsUser2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PrecedingSucceedingTitle2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/PrecedingSucceedingTitle2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PrintersDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Printers/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Proxy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Proxy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Receiving2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Receiving2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Record2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Record2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ReferenceData2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ReferenceData2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ReferenceData2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RefundReason2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RefundReason2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RelationshipsDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Relationships/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RelationshipTypesDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RelationshipTypes/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Request2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Request2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RequestPolicy2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RequestPolicy2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Rollover2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Rollover2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Rollover2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RolloverBudget2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RolloverBudget2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverBudget2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RolloverError2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RolloverError2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverError2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RolloverProgress2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/RolloverProgress2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RolloverProgress2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ScheduledNotice2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ScheduledNotice2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ServicePoint2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ServicePoint2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ServicePointUser2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/ServicePointUser2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void SettingsDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Settings/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Snapshot2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Snapshot2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Source2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Source2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StaffSlip2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/StaffSlip2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatisticalCode2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/StatisticalCode2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatisticalCodeType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/StatisticalCodeType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatusesDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Statuses/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void SubjectSource2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/SubjectSource2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectSource2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void SubjectType2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/SubjectType2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"SubjectType2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Template2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Template2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Title2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Title2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Transaction2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Transaction2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void TransferAccount2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/TransferAccount2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void TransferCriteria2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/TransferCriteria2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void User2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/User2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void UserAcquisitionsUnit2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/UserAcquisitionsUnit2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void UserRequestPreference2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/UserRequestPreference2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Voucher2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/Voucher2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void VoucherItem2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/VoucherItem2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void WaiveReason2sDefaultAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hrm = httpClient.GetAsync($"{Url}/WaiveReason2s/Default.aspx").Result;
                hrm.EnsureSuccessStatusCode();
                Assert.IsTrue(s.Elapsed < TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sDefaultAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            httpClient.Dispose();
        }
    }
}
