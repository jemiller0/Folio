using FolioLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace FolioWebApplicationTest
{
    [TestClass]
    public class EditAspxTest
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }) { /*Timeout = Timeout.InfiniteTimeSpan*/ };
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplicationTest", SourceLevels.Information);
        private TimeSpan timeSpan = TimeSpan.FromSeconds(30);
        private static string Url { get; set; } = ConfigurationManager.AppSettings["url"];

        [TestMethod]
        public void AcquisitionsUnit2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var au2 = folioServiceContext.AcquisitionsUnit2s(take: 1).SingleOrDefault();
                if (au2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/AcquisitionsUnit2s/Edit.aspx?Id={au2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AcquisitionsUnit2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AddressType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var at2 = folioServiceContext.AddressType2s(take: 1).SingleOrDefault();
                if (at2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/AddressType2s/Edit.aspx?Id={at2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AddressType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Alert2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var a2 = folioServiceContext.Alert2s(take: 1).SingleOrDefault();
                if (a2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Alert2s/Edit.aspx?Id={a2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Alert2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void AlternativeTitleType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var att2 = folioServiceContext.AlternativeTitleType2s(take: 1).SingleOrDefault();
                if (att2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/AlternativeTitleType2s/Edit.aspx?Id={att2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"AlternativeTitleType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BatchGroup2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bg2 = folioServiceContext.BatchGroup2s(take: 1).SingleOrDefault();
                if (bg2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BatchGroup2s/Edit.aspx?Id={bg2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchGroup2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BatchVoucherExport2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bve2 = folioServiceContext.BatchVoucherExport2s(take: 1).SingleOrDefault();
                if (bve2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BatchVoucherExport2s/Edit.aspx?Id={bve2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExport2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BatchVoucherExportConfig2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bvec2 = folioServiceContext.BatchVoucherExportConfig2s(take: 1).SingleOrDefault();
                if (bvec2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BatchVoucherExportConfig2s/Edit.aspx?Id={bvec2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BatchVoucherExportConfig2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Block2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var b2 = folioServiceContext.Block2s(take: 1).SingleOrDefault();
                if (b2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Block2s/Edit.aspx?Id={b2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Block2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BlockCondition2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bc2 = folioServiceContext.BlockCondition2s(take: 1).SingleOrDefault();
                if (bc2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BlockCondition2s/Edit.aspx?Id={bc2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockCondition2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BlockLimit2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bl2 = folioServiceContext.BlockLimit2s(take: 1).SingleOrDefault();
                if (bl2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BlockLimit2s/Edit.aspx?Id={bl2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BlockLimit2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Budget2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var b2 = folioServiceContext.Budget2s(take: 1).SingleOrDefault();
                if (b2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Budget2s/Edit.aspx?Id={b2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Budget2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void BudgetExpenseClass2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var bec2 = folioServiceContext.BudgetExpenseClass2s(take: 1).SingleOrDefault();
                if (bec2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/BudgetExpenseClass2s/Edit.aspx?Id={bec2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"BudgetExpenseClass2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CallNumberType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var cnt2 = folioServiceContext.CallNumberType2s(take: 1).SingleOrDefault();
                if (cnt2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/CallNumberType2s/Edit.aspx?Id={cnt2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CallNumberType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Campus2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var c2 = folioServiceContext.Campus2s(take: 1).SingleOrDefault();
                if (c2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Campus2s/Edit.aspx?Id={c2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Campus2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CancellationReason2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var cr2 = folioServiceContext.CancellationReason2s(take: 1).SingleOrDefault();
                if (cr2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/CancellationReason2s/Edit.aspx?Id={cr2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CancellationReason2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Category2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var c2 = folioServiceContext.Category2s(take: 1).SingleOrDefault();
                if (c2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Category2s/Edit.aspx?Id={c2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Category2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CheckIn2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ci2 = folioServiceContext.CheckIn2s(take: 1).SingleOrDefault();
                if (ci2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/CheckIn2s/Edit.aspx?Id={ci2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CheckIn2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ClassificationType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ct2 = folioServiceContext.ClassificationType2s(take: 1).SingleOrDefault();
                if (ct2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ClassificationType2s/Edit.aspx?Id={ct2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ClassificationType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CloseReason2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var cr2 = folioServiceContext.CloseReason2s(take: 1).SingleOrDefault();
                if (cr2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/CloseReason2s/Edit.aspx?Id={cr2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CloseReason2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Comment2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var c2 = folioServiceContext.Comment2s(take: 1).SingleOrDefault();
                if (c2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Comment2s/Edit.aspx?Id={c2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Comment2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Configuration2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var c2 = folioServiceContext.Configuration2s(take: 1).SingleOrDefault();
                if (c2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Configuration2s/Edit.aspx?Id={c2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Configuration2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Contact2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var c2 = folioServiceContext.Contact2s(take: 1).SingleOrDefault();
                if (c2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Contact2s/Edit.aspx?Id={c2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Contact2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ContributorNameType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var cnt2 = folioServiceContext.ContributorNameType2s(take: 1).SingleOrDefault();
                if (cnt2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ContributorNameType2s/Edit.aspx?Id={cnt2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorNameType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ContributorType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ct2 = folioServiceContext.ContributorType2s(take: 1).SingleOrDefault();
                if (ct2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ContributorType2s/Edit.aspx?Id={ct2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ContributorType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void CustomField2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var cf2 = folioServiceContext.CustomField2s(take: 1).SingleOrDefault();
                if (cf2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/CustomField2s/Edit.aspx?Id={cf2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"CustomField2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Department2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var d2 = folioServiceContext.Department2s(take: 1).SingleOrDefault();
                if (d2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Department2s/Edit.aspx?Id={d2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Department2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ElectronicAccessRelationship2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ear2 = folioServiceContext.ElectronicAccessRelationship2s(take: 1).SingleOrDefault();
                if (ear2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ElectronicAccessRelationship2s/Edit.aspx?Id={ear2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ElectronicAccessRelationship2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ExpenseClass2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ec2 = folioServiceContext.ExpenseClass2s(take: 1).SingleOrDefault();
                if (ec2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ExpenseClass2s/Edit.aspx?Id={ec2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ExpenseClass2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Fee2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var f2 = folioServiceContext.Fee2s(take: 1).SingleOrDefault();
                if (f2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Fee2s/Edit.aspx?Id={f2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Fee2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FeeType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ft2 = folioServiceContext.FeeType2s(take: 1).SingleOrDefault();
                if (ft2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/FeeType2s/Edit.aspx?Id={ft2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FeeType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FinanceGroup2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var fg2 = folioServiceContext.FinanceGroup2s(take: 1).SingleOrDefault();
                if (fg2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/FinanceGroup2s/Edit.aspx?Id={fg2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FinanceGroup2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FiscalYear2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var fy2 = folioServiceContext.FiscalYear2s(take: 1).SingleOrDefault();
                if (fy2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/FiscalYear2s/Edit.aspx?Id={fy2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FiscalYear2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FixedDueDateSchedule2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var fdds2 = folioServiceContext.FixedDueDateSchedule2s(take: 1).SingleOrDefault();
                if (fdds2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/FixedDueDateSchedule2s/Edit.aspx?Id={fdds2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FixedDueDateSchedule2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FormatsEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var f = folioServiceContext.Formats(take: 1).SingleOrDefault();
                if (f != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Formats/Edit.aspx?Id={f.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FormatsEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Fund2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var f2 = folioServiceContext.Fund2s(take: 1).SingleOrDefault();
                if (f2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Fund2s/Edit.aspx?Id={f2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Fund2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void FundType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ft2 = folioServiceContext.FundType2s(take: 1).SingleOrDefault();
                if (ft2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/FundType2s/Edit.aspx?Id={ft2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"FundType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Group2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var g2 = folioServiceContext.Group2s(take: 1).SingleOrDefault();
                if (g2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Group2s/Edit.aspx?Id={g2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Group2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void GroupFundFiscalYear2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var gffy2 = folioServiceContext.GroupFundFiscalYear2s(take: 1).SingleOrDefault();
                if (gffy2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/GroupFundFiscalYear2s/Edit.aspx?Id={gffy2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"GroupFundFiscalYear2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Holding2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var h2 = folioServiceContext.Holding2s(take: 1).SingleOrDefault();
                if (h2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Holding2s/Edit.aspx?Id={h2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Holding2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void HoldingNoteType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var hnt2 = folioServiceContext.HoldingNoteType2s(take: 1).SingleOrDefault();
                if (hnt2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/HoldingNoteType2s/Edit.aspx?Id={hnt2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingNoteType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void HoldingType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ht2 = folioServiceContext.HoldingType2s(take: 1).SingleOrDefault();
                if (ht2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/HoldingType2s/Edit.aspx?Id={ht2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"HoldingType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IdType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var it2 = folioServiceContext.IdType2s(take: 1).SingleOrDefault();
                if (it2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/IdType2s/Edit.aspx?Id={it2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IdType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IllPolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ip2 = folioServiceContext.IllPolicy2s(take: 1).SingleOrDefault();
                if (ip2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/IllPolicy2s/Edit.aspx?Id={ip2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IllPolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Instance2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var i2 = folioServiceContext.Instance2s(take: 1).SingleOrDefault();
                if (i2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Instance2s/Edit.aspx?Id={i2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Instance2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InstanceNoteType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var int2 = folioServiceContext.InstanceNoteType2s(take: 1).SingleOrDefault();
                if (int2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/InstanceNoteType2s/Edit.aspx?Id={int2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceNoteType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InstanceType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var it2 = folioServiceContext.InstanceType2s(take: 1).SingleOrDefault();
                if (it2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/InstanceType2s/Edit.aspx?Id={it2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InstanceType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Institution2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var i2 = folioServiceContext.Institution2s(take: 1).SingleOrDefault();
                if (i2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Institution2s/Edit.aspx?Id={i2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Institution2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Interface2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var i2 = folioServiceContext.Interface2s(take: 1).SingleOrDefault();
                if (i2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Interface2s/Edit.aspx?Id={i2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Interface2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Invoice2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var i2 = folioServiceContext.Invoice2s(take: 1).SingleOrDefault();
                if (i2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Invoice2s/Edit.aspx?Id={i2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Invoice2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void InvoiceItem2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ii2 = folioServiceContext.InvoiceItem2s(take: 1).SingleOrDefault();
                if (ii2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/InvoiceItem2s/Edit.aspx?Id={ii2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"InvoiceItem2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void IssuanceModesEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var im = folioServiceContext.IssuanceModes(take: 1).SingleOrDefault();
                if (im != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/IssuanceModes/Edit.aspx?Id={im.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"IssuanceModesEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Item2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var i2 = folioServiceContext.Item2s(take: 1).SingleOrDefault();
                if (i2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Item2s/Edit.aspx?Id={i2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Item2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ItemDamagedStatus2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ids2 = folioServiceContext.ItemDamagedStatus2s(take: 1).SingleOrDefault();
                if (ids2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ItemDamagedStatus2s/Edit.aspx?Id={ids2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemDamagedStatus2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ItemNoteType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var int2 = folioServiceContext.ItemNoteType2s(take: 1).SingleOrDefault();
                if (int2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ItemNoteType2s/Edit.aspx?Id={int2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ItemNoteType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Ledger2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var l2 = folioServiceContext.Ledger2s(take: 1).SingleOrDefault();
                if (l2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Ledger2s/Edit.aspx?Id={l2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Ledger2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Library2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var l2 = folioServiceContext.Library2s(take: 1).SingleOrDefault();
                if (l2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Library2s/Edit.aspx?Id={l2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Library2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Loan2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var l2 = folioServiceContext.Loan2s(take: 1).SingleOrDefault();
                if (l2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Loan2s/Edit.aspx?Id={l2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Loan2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LoanPolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var lp2 = folioServiceContext.LoanPolicy2s(take: 1).SingleOrDefault();
                if (lp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/LoanPolicy2s/Edit.aspx?Id={lp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanPolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LoanType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var lt2 = folioServiceContext.LoanType2s(take: 1).SingleOrDefault();
                if (lt2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/LoanType2s/Edit.aspx?Id={lt2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LoanType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Location2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var l2 = folioServiceContext.Location2s(take: 1).SingleOrDefault();
                if (l2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Location2s/Edit.aspx?Id={l2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Location2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LocationSettingsEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ls = folioServiceContext.LocationSettings(take: 1).SingleOrDefault();
                if (ls != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/LocationSettings/Edit.aspx?Id={ls.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LocationSettingsEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void LostItemFeePolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var lifp2 = folioServiceContext.LostItemFeePolicy2s(take: 1).SingleOrDefault();
                if (lifp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/LostItemFeePolicy2s/Edit.aspx?Id={lifp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"LostItemFeePolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void MaterialType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var mt2 = folioServiceContext.MaterialType2s(take: 1).SingleOrDefault();
                if (mt2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/MaterialType2s/Edit.aspx?Id={mt2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"MaterialType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void NatureOfContentTerm2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var noct2 = folioServiceContext.NatureOfContentTerm2s(take: 1).SingleOrDefault();
                if (noct2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/NatureOfContentTerm2s/Edit.aspx?Id={noct2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"NatureOfContentTerm2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Order2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var o2 = folioServiceContext.Order2s(take: 1).SingleOrDefault();
                if (o2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Order2s/Edit.aspx?Id={o2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Order2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderInvoice2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var oi2 = folioServiceContext.OrderInvoice2s(take: 1).SingleOrDefault();
                if (oi2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/OrderInvoice2s/Edit.aspx?Id={oi2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderInvoice2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderItem2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var oi2 = folioServiceContext.OrderItem2s(take: 1).SingleOrDefault();
                if (oi2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/OrderItem2s/Edit.aspx?Id={oi2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderItem2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderTemplate2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ot2 = folioServiceContext.OrderTemplate2s(take: 1).SingleOrDefault();
                if (ot2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/OrderTemplate2s/Edit.aspx?Id={ot2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTemplate2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OrderTransactionSummary2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ots2 = folioServiceContext.OrderTransactionSummary2s(take: 1).SingleOrDefault();
                if (ots2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/OrderTransactionSummary2s/Edit.aspx?Id={ots2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OrderTransactionSummary2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Organization2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var o2 = folioServiceContext.Organization2s(take: 1).SingleOrDefault();
                if (o2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Organization2s/Edit.aspx?Id={o2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Organization2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void OverdueFinePolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ofp2 = folioServiceContext.OverdueFinePolicy2s(take: 1).SingleOrDefault();
                if (ofp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/OverdueFinePolicy2s/Edit.aspx?Id={ofp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"OverdueFinePolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Owner2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var o2 = folioServiceContext.Owner2s(take: 1).SingleOrDefault();
                if (o2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Owner2s/Edit.aspx?Id={o2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Owner2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PatronActionSession2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var pas2 = folioServiceContext.PatronActionSession2s(take: 1).SingleOrDefault();
                if (pas2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/PatronActionSession2s/Edit.aspx?Id={pas2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronActionSession2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PatronNoticePolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var pnp2 = folioServiceContext.PatronNoticePolicy2s(take: 1).SingleOrDefault();
                if (pnp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/PatronNoticePolicy2s/Edit.aspx?Id={pnp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PatronNoticePolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Payment2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var p2 = folioServiceContext.Payment2s(take: 1).SingleOrDefault();
                if (p2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Payment2s/Edit.aspx?Id={p2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Payment2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PaymentMethod2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var pm2 = folioServiceContext.PaymentMethod2s(take: 1).SingleOrDefault();
                if (pm2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/PaymentMethod2s/Edit.aspx?Id={pm2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PaymentMethod2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Permission2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var p2 = folioServiceContext.Permission2s(take: 1).SingleOrDefault();
                if (p2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Permission2s/Edit.aspx?Id={p2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Permission2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PermissionsUser2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var pu2 = folioServiceContext.PermissionsUser2s(take: 1).SingleOrDefault();
                if (pu2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/PermissionsUser2s/Edit.aspx?Id={pu2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PermissionsUser2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PrecedingSucceedingTitle2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var pst2 = folioServiceContext.PrecedingSucceedingTitle2s(take: 1).SingleOrDefault();
                if (pst2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/PrecedingSucceedingTitle2s/Edit.aspx?Id={pst2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PrecedingSucceedingTitle2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Prefix2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var p2 = folioServiceContext.Prefix2s(take: 1).SingleOrDefault();
                if (p2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Prefix2s/Edit.aspx?Id={p2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Prefix2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void PrintersEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var p = folioServiceContext.Printers(take: 1).SingleOrDefault();
                if (p != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Printers/Edit.aspx?Id={p.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"PrintersEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Proxy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var p2 = folioServiceContext.Proxy2s(take: 1).SingleOrDefault();
                if (p2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Proxy2s/Edit.aspx?Id={p2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Proxy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Receiving2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var r2 = folioServiceContext.Receiving2s(take: 1).SingleOrDefault();
                if (r2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Receiving2s/Edit.aspx?Id={r2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Receiving2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Record2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var r2 = folioServiceContext.Record2s(take: 1).SingleOrDefault();
                if (r2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Record2s/Edit.aspx?Id={r2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Record2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RefundReason2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var rr2 = folioServiceContext.RefundReason2s(take: 1).SingleOrDefault();
                if (rr2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/RefundReason2s/Edit.aspx?Id={rr2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RefundReason2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RelationshipsEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var r = folioServiceContext.Relationships(take: 1).SingleOrDefault();
                if (r != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Relationships/Edit.aspx?Id={r.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipsEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RelationshipTypesEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var rt = folioServiceContext.RelationshipTypes(take: 1).SingleOrDefault();
                if (rt != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/RelationshipTypes/Edit.aspx?Id={rt.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RelationshipTypesEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ReportingCode2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var rc2 = folioServiceContext.ReportingCode2s(take: 1).SingleOrDefault();
                if (rc2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ReportingCode2s/Edit.aspx?Id={rc2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ReportingCode2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Request2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var r2 = folioServiceContext.Request2s(take: 1).SingleOrDefault();
                if (r2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Request2s/Edit.aspx?Id={r2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Request2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void RequestPolicy2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var rp2 = folioServiceContext.RequestPolicy2s(take: 1).SingleOrDefault();
                if (rp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/RequestPolicy2s/Edit.aspx?Id={rp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"RequestPolicy2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ScheduledNotice2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var sn2 = folioServiceContext.ScheduledNotice2s(take: 1).SingleOrDefault();
                if (sn2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ScheduledNotice2s/Edit.aspx?Id={sn2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ScheduledNotice2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ServicePoint2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var sp2 = folioServiceContext.ServicePoint2s(take: 1).SingleOrDefault();
                if (sp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ServicePoint2s/Edit.aspx?Id={sp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePoint2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void ServicePointUser2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var spu2 = folioServiceContext.ServicePointUser2s(take: 1).SingleOrDefault();
                if (spu2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/ServicePointUser2s/Edit.aspx?Id={spu2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointUser2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void SettingsEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var s2 = folioServiceContext.Settings(take: 1).SingleOrDefault();
                if (s2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Settings/Edit.aspx?Id={s2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"SettingsEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Snapshot2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var s2 = folioServiceContext.Snapshot2s(take: 1).SingleOrDefault();
                if (s2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Snapshot2s/Edit.aspx?Id={s2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Snapshot2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Source2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var s2 = folioServiceContext.Source2s(take: 1).SingleOrDefault();
                if (s2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Source2s/Edit.aspx?Id={s2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Source2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StaffSlip2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ss2 = folioServiceContext.StaffSlip2s(take: 1).SingleOrDefault();
                if (ss2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/StaffSlip2s/Edit.aspx?Id={ss2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StaffSlip2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatisticalCode2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var sc2 = folioServiceContext.StatisticalCode2s(take: 1).SingleOrDefault();
                if (sc2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/StatisticalCode2s/Edit.aspx?Id={sc2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCode2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatisticalCodeType2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var sct2 = folioServiceContext.StatisticalCodeType2s(take: 1).SingleOrDefault();
                if (sct2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/StatisticalCodeType2s/Edit.aspx?Id={sct2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatisticalCodeType2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void StatusesEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var s2 = folioServiceContext.Statuses(take: 1).SingleOrDefault();
                if (s2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Statuses/Edit.aspx?Id={s2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"StatusesEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Suffix2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var s2 = folioServiceContext.Suffix2s(take: 1).SingleOrDefault();
                if (s2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Suffix2s/Edit.aspx?Id={s2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Suffix2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Template2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var t2 = folioServiceContext.Template2s(take: 1).SingleOrDefault();
                if (t2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Template2s/Edit.aspx?Id={t2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Template2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Title2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var t2 = folioServiceContext.Title2s(take: 1).SingleOrDefault();
                if (t2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Title2s/Edit.aspx?Id={t2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Title2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Transaction2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var t2 = folioServiceContext.Transaction2s(take: 1).SingleOrDefault();
                if (t2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Transaction2s/Edit.aspx?Id={t2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Transaction2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void TransferAccount2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var ta2 = folioServiceContext.TransferAccount2s(take: 1).SingleOrDefault();
                if (ta2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/TransferAccount2s/Edit.aspx?Id={ta2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferAccount2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void TransferCriteria2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var tc2 = folioServiceContext.TransferCriteria2s(take: 1).SingleOrDefault();
                if (tc2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/TransferCriteria2s/Edit.aspx?Id={tc2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"TransferCriteria2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void User2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var u2 = folioServiceContext.User2s(take: 1).SingleOrDefault();
                if (u2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/User2s/Edit.aspx?Id={u2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"User2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void UserAcquisitionsUnit2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var uau2 = folioServiceContext.UserAcquisitionsUnit2s(take: 1).SingleOrDefault();
                if (uau2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/UserAcquisitionsUnit2s/Edit.aspx?Id={uau2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"UserAcquisitionsUnit2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void UserRequestPreference2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var urp2 = folioServiceContext.UserRequestPreference2s(take: 1).SingleOrDefault();
                if (urp2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/UserRequestPreference2s/Edit.aspx?Id={urp2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"UserRequestPreference2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void Voucher2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var v2 = folioServiceContext.Voucher2s(take: 1).SingleOrDefault();
                if (v2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/Voucher2s/Edit.aspx?Id={v2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"Voucher2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void VoucherItem2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var vi2 = folioServiceContext.VoucherItem2s(take: 1).SingleOrDefault();
                if (vi2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/VoucherItem2s/Edit.aspx?Id={vi2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"VoucherItem2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [TestMethod]
        public void WaiveReason2sEditAspxTest()
        {
            var s = Stopwatch.StartNew();
            try
            {
                var wr2 = folioServiceContext.WaiveReason2s(take: 1).SingleOrDefault();
                if (wr2 != null)
                {
                    var hrm = httpClient.GetAsync($"{Url}/WaiveReason2s/Edit.aspx?Id={wr2.Id}").Result;
                    hrm.EnsureSuccessStatusCode();
                    Assert.IsTrue(s.Elapsed < timeSpan);
                }
                else
                    Assert.Inconclusive();
            }
            catch (Exception e)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
                throw;
            }
            finally
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, $"WaiveReason2sEditAspxTest()\r\n    ElapsedTime={s.Elapsed}");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            httpClient.Dispose();
        }
    }
}
