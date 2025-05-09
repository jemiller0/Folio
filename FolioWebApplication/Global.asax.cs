﻿using FolioLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Telerik.Web.UI;

namespace FolioWebApplication
{
    public partial class Global : System.Web.HttpApplication
    {
        private static string emailAddress = ConfigurationManager.AppSettings["emailAddress"];
        private static string emailName = ConfigurationManager.AppSettings["emailName"];
        //private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private static string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
        private Stopwatch stopWatch;
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Application_Start(object sender, EventArgs e)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, "Starting");
            traceSource.TraceEvent(TraceEventType.Information, 0, $"ServicePointManager.DefaultConnectionLimit = {ServicePointManager.DefaultConnectionLimit}");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["addPermissions"] != "true") AddPermissionsIfNecessary();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.CurrentExecutionFilePathExtension.Equals(".aspx", StringComparison.OrdinalIgnoreCase)) stopWatch = Stopwatch.StartNew();
            if (!Request.IsSecureConnection)
            {
                var ub = new UriBuilder(Request.Url)
                {
                    Scheme = "https",
                    Port = !Request.Url.IsLoopback ? 443 : 44363
                };
                if (!ub.Host.Contains('.') && !Request.Url.IsLoopback)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var ihe = Dns.Resolve(ub.Host);
#pragma warning restore CS0618 // Type or member is obsolete
                    ub.Host = ihe.HostName.StartsWith(ub.Host, StringComparison.InvariantCultureIgnoreCase) ? ihe.HostName : ihe.Aliases.Single(s => s.StartsWith(ub.Host, StringComparison.InvariantCultureIgnoreCase));
                }
                Response.Redirect(ub.ToString());
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e)
        {
            var e2 = Server.GetLastError();
            traceSource.TraceEvent(TraceEventType.Critical, 0, $"{e2}\r\n    RequestURL={Request.Url}\r\n    UserName={User?.Identity?.Name}");
            Session["ErrorMessage"] = e2.InnerException?.Message ?? e2.Message;
            //Session["ErrorMessage"] = e2.InnerException.ToString();
            if (e2.GetType() == typeof(HttpException) && Regex.IsMatch(e2.Message, @"The file '.*' does not exist\.")) return;
            if (!Request.Url.IsLoopback && smtpHost != null && emailAddress != null && emailName != null)
            {
                using (var sc = new SmtpClient(smtpHost))
                {
                    sc.Send(new MailMessage(new MailAddress($"noreply@{Request.Url.Host}", "Folio Website"), new MailAddress(emailAddress, emailName))
                    {
                        Subject = $"Exception occurred at {Request.Url}",
                        Body = $"URL: {Request.Url}\r\nUser Host Name: {Request.UserHostName}\r\nUser Host Address: {Request.UserHostAddress}\r\nUser Agent: {Request.UserAgent}\r\nBrowser: {Request.Browser.Browser} {Request.Browser.Version}\r\nBrowser Platform: {Request.Browser.Platform}\r\nUser Name: {User?.Identity?.Name}\r\nException: {e2}\r\n"
                    });
                }
                //Server.ClearError();
            }
        }

        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            if (Request.CurrentExecutionFilePathExtension.Equals(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                //ImpersonateUser("");

                traceSource.TraceEvent(TraceEventType.Verbose, 0, $"{Request.Url}\n    UserName={User?.Identity?.Name}\n    LocalDateTime={DateTime.Now:G}");
                if (Session["UserName"] == null)
                {
                    Session["UserName"] = Regex.Replace(User.Identity.Name, @"^.+\\", "", RegexOptions.Compiled);
                    using (var fsc = FolioServiceContextPool.GetFolioServiceContext()) Session["UserId"] = fsc.User2s($"username == \"{Session["UserName"]}\"").Single().Id;
                }
                var l = Roles.GetRolesForUser();
                var userName = (string)Session["UserName"];
                if (l.Any())
                {
                    var administrator = new[]
                    {
                        "department:Library Integrated Lib Systems"
                    };
                    var acquisitions = new[]
                    {
                        "department:Library Integrated Lib Systems",
                        "department:Law Library Admin",
                        "department:Library CS Acquisitions",
                        "department:Library CS Admin",
                        "department:Library CS Collections Supp",
                        "department:Library CS Electr Resources"
                    };
                    var cataloging = new[]
                    {
                        "division:Library",
                        "user:amulyaj",
                        "user:asjiac",
                        "user:awhite6",
                        "user:lkonjoyan",
                        "user:maniza",
                        "user:tbledsoe"
                    };
                    var circulation = new[]
                    {
                        "department:Library Integrated Lib Systems",
                        "department:Law Library Admin",
                        "department:Library CS Admin",
                        "department:Library CS Collections Supp"
                    };
                    if (administrator.Intersect(l).Any())
                    {
                        SetAgreementsPermissions("View");
                        SetConfigurationPermissions("View");
                        SetPermissionsPermissions("View");
                        SetSourcePermissions("View");
                        SetTemplatesPermissions("View");
                        Session["LocationSettingsPermission"] = Session["PrintersPermission"] = Session["SettingsPermission"] = "Edit";
                    }
                    if (acquisitions.Intersect(l).Any())
                    {
                        SetFinancePermissions("View");
                        SetInvoicesPermissions("View");
                        SetOrdersPermissions("View");
                        SetOrganizationsPermissions("View");
                    }
                    if (cataloging.Intersect(l).Any())
                    {
                        SetInventoryPermissions("View");
                        Session["LabelsPermission"] = "Edit";
                    }
                    if (circulation.Intersect(l).Any())
                    {
                        SetCirculationPermissions("View");
                        SetFeesPermissions("View");
                        SetUsersPermissions("View");
                    }
                    if (userName != "jemiller") Session["Configuration2sPermission"] = null;
                }
                else
                {
                    Response.StatusCode = 401;
                    Response.End();
                }
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (User != null && Request.CurrentExecutionFilePathExtension.Equals(".aspx", StringComparison.OrdinalIgnoreCase)) traceSource.TraceEvent(TraceEventType.Information, 0, $"{Request.Url}\n    UserName={User.Identity.Name}\n    LocalDateTime={DateTime.Now:G}\n    ElapsedTime={stopWatch.Elapsed}");
        }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e) { }

        private static Dictionary<GridKnownFunction, string> ermFormats = new Dictionary<GridKnownFunction, string>
        {
            { GridKnownFunction.Between, "{0} within \"{1}\"" },
            { GridKnownFunction.Contains, "{0} == \"*{1}*\"" },
            { GridKnownFunction.Custom, "" },
            { GridKnownFunction.DoesNotContain, "{0} <> \"*{1}*\"" },
            { GridKnownFunction.EndsWith, "{0} == \"*{1}\"" },
            { GridKnownFunction.EqualTo, "{0} == \"{1}\"" },
            { GridKnownFunction.GreaterThan, "{0} > \"{1}\"" },
            { GridKnownFunction.GreaterThanOrEqualTo, "{0} >= \"{1}\"" },
            { GridKnownFunction.IsEmpty, "{0} == \"\"" },
            { GridKnownFunction.IsNull, "(cql.allRecords=1 not {0} = \"\")" },
            { GridKnownFunction.LessThan, "{0} < \"{1}\"" },
            { GridKnownFunction.LessThanOrEqualTo, "{0} <= \"{1}\"" },
            { GridKnownFunction.NoFilter, "" },
            { GridKnownFunction.NotBetween, "{0} within \"{1}\"" },
            { GridKnownFunction.NotEqualTo, "{0} <> \"{1}\"" },
            { GridKnownFunction.NotIsEmpty, "({0} = \"\" not {0} == \"\")" },
            { GridKnownFunction.NotIsNull, "{0} = \"\"" },
            { GridKnownFunction.StartsWith, "{0} == \"{1}*\"" }
        };

        private static Dictionary<GridKnownFunction, string> formats = new Dictionary<GridKnownFunction, string>
        {
            { GridKnownFunction.Between, "{0} within \"{1}\"" },
            { GridKnownFunction.Contains, "{0} == \"*{1}*\"" },
            { GridKnownFunction.Custom, "" },
            { GridKnownFunction.DoesNotContain, "{0} <> \"*{1}*\"" },
            { GridKnownFunction.EndsWith, "{0} == \"*{1}\"" },
            { GridKnownFunction.EqualTo, "{0} == \"{1}\"" },
            //{ GridKnownFunction.EqualTo, "{0} =/isoDate \"{1}\"" },
            //{ GridKnownFunction.EqualTo, "{0} = \"{1}\"" },
            { GridKnownFunction.GreaterThan, "{0} > \"{1}\"" },
            { GridKnownFunction.GreaterThanOrEqualTo, "{0} >= \"{1}\"" },
            { GridKnownFunction.IsEmpty, "{0} == \"\"" },
            { GridKnownFunction.IsNull, "(cql.allRecords=1 not {0} = \"\")" },
            { GridKnownFunction.LessThan, "{0} < \"{1}\"" },
            { GridKnownFunction.LessThanOrEqualTo, "{0} <= \"{1}\"" },
            { GridKnownFunction.NoFilter, "" },
            { GridKnownFunction.NotBetween, "{0} within \"{1}\"" },
            { GridKnownFunction.NotEqualTo, "{0} <> \"{1}\"" },
            { GridKnownFunction.NotIsEmpty, "({0} = \"\" not {0} == \"\")" },
            { GridKnownFunction.NotIsNull, "{0} = \"\"" },
            { GridKnownFunction.StartsWith, "{0} == \"{1}*\"" }
        };

        public static string GetCqlFilter(RadGrid radGrid, string dataField, string name, string name2 = null, Func<string, string, int?, int?, IEnumerable<JObject>> query = null)
        {
            var gc = radGrid.MasterTableView.Columns.FindByDataField(dataField);
            if (gc.CurrentFilterValue != "" || gc.CurrentFilterFunction == GridKnownFunction.IsEmpty || gc.CurrentFilterFunction == GridKnownFunction.IsNull || gc.CurrentFilterFunction == GridKnownFunction.NotIsEmpty || gc.CurrentFilterFunction == GridKnownFunction.NotIsNull)
            {
                if (query == null || gc.CurrentFilterFunction == GridKnownFunction.IsNull || gc.CurrentFilterFunction == GridKnownFunction.NotIsNull)
                {
                    if (gc.DataType == typeof(DateTime)) gc.CurrentFilterValue = DateTime.TryParse(gc.CurrentFilterValue, out var dt) ? dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff+00:00"/*"o"*/) : null;
                    if (gc.DataType == typeof(decimal)) gc.CurrentFilterValue = decimal.TryParse(gc.CurrentFilterValue.Replace("$", ""), out var d) ? Regex.Replace($"{d}{(!d.ToString().Contains(".") ? ".0" : "")}", @"\.00$", ".0") : null;
                    //if (gc.CurrentFilterValue == "") return null;
                    return string.Format(formats[gc.CurrentFilterFunction]/*.Replace("\"", gc.DataType == typeof(decimal) ? "" : "\"")*/, name, FolioServiceClient.EncodeCql(gc.CurrentFilterValue));
                }
                else
                    return $"{name} == ({string.Join(" or ", query(GetCqlFilter(radGrid, dataField, name2), null, null, radGrid.PageSize).Select(jo => $"\"{jo["id"]}\""))})".Replace(" == ()", " == false");
            }
            return null;
        }

        //public static string GetCqlFilter(GridColumn gc, string name, IEnumerable<string> ids = null)
        //{
        //    if (gc.CurrentFilterValue != "" || gc.CurrentFilterFunction == GridKnownFunction.IsEmpty || gc.CurrentFilterFunction == GridKnownFunction.IsNull || gc.CurrentFilterFunction == GridKnownFunction.NotIsEmpty || gc.CurrentFilterFunction == GridKnownFunction.NotIsNull)
        //    {
        //        if (ids != null)
        //        {
        //            if (gc.DataType == typeof(DateTime)) gc.CurrentFilterValue = DateTime.TryParse(gc.CurrentFilterValue, out var dt) ? dt.ToUniversalTime().ToString("o") : null;
        //            return string.Format(formats[gc.CurrentFilterFunction], name, FolioServiceClient.EncodeCql(gc.CurrentFilterValue));
        //        }
        //        else
        //        {

        //        }
        //    }
        //    return null;
        //}

        public static string GetCqlFilter(RadGrid radGrid, Dictionary<string, string> properties, string where = null)
        {
            var s = Trim(string.Join(" and ", radGrid.MasterTableView.Columns.Cast<GridColumn>().Where(gc => gc.CurrentFilterValue != "" || gc.CurrentFilterFunction == GridKnownFunction.IsEmpty || gc.CurrentFilterFunction == GridKnownFunction.IsNull || gc.CurrentFilterFunction == GridKnownFunction.NotIsEmpty || gc.CurrentFilterFunction == GridKnownFunction.NotIsNull).Select(gc =>
            {
                if (gc.DataType == typeof(DateTime)) gc.CurrentFilterValue = DateTime.TryParse(gc.CurrentFilterValue, out var dt) ? dt.ToUniversalTime().ToString("o") : null;
                return string.Format(formats[gc.CurrentFilterFunction], properties[gc.GetType() == typeof(GridHyperLinkColumn) ? ((GridHyperLinkColumn)gc).DataTextField : ((dynamic)gc).DataField], FolioServiceClient.EncodeCql(gc.CurrentFilterValue));
            })));
            return Trim($"{where}{(where != null && s != null ? " and " : "")}{s}");
        }

        private void ImpersonateUser(string userName)
        {
            Session["UserName"] = userName;
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity($@"ADLOCAL\{userName}"), System.Web.Security.Roles.GetRolesForUser($@"ADLOCAL\{userName}"));
            using (var fsc = FolioServiceContextPool.GetFolioServiceContext()) Session["UserId"] = fsc.User2s($"username == \"{Session["UserName"]}\"").Single().Id;
        }

        public static string Trim(string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Length == 0) value = null;
            }
            return value;
        }

        public static void Print(Label label, Page page, FolioServiceContext oleContext)
        {
            var computerName = page.Request.IsLocal ? Dns.GetHostName() : page.Request.UserHostAddress.StartsWith("128.135.") ? Dns.GetHostEntry(page.Request.UserHostAddress)?.HostName?.Split('.')?.FirstOrDefault() : null;
            var p = oleContext.Printers().FirstOrDefault(p2 => p2.ComputerName == computerName && p2.Enabled.Value);
            var printerName = p != null ? $@"\\{computerName}\{p.Name}" : null;
            //printerName = @"\\jrl-ils-3\Microsoft Print to PDF";
            if (printerName != null)
            {
                using (var pd = new PrintDocument { PrintController = new StandardPrintController() })
                using (var f = new System.Drawing.Font(label.Font.Family, (float)label.Font.Size, label.Font.Weight == FontWeight.Bold ? FontStyle.Bold : FontStyle.Regular))
                {
                    pd.PrinterSettings.PrinterName = printerName;
                    pd.DefaultPageSettings.Landscape = label.Orientation == Orientation.Landscape;
                    pd.PrintPage += (sender2, e2) =>
                    {
                        if (p.Width != null)
                            e2.Graphics.DrawString(label.Text, f, Brushes.Black, new RectangleF(p.Left.Value, p.Top.Value, p.Width.Value, p.Height.Value));
                        else
                            e2.Graphics.DrawString(label.Text, f, Brushes.Black, p.Left.Value, p.Top.Value);
                        //Thread.Sleep(5000);
                        //e2.Cancel = true;
                    };
                    traceSource.TraceEvent(TraceEventType.Information, 0, $"Printing to {printerName}");
                    pd.Print();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(page, typeof(Page), null, $"w = window.open(); w.document.write('<html><body><div style=\"{(label.Width != null ? $"width: {label.Width}px; " : "")}font-family: {label.Font.Family}; font-size: {label.Font.Size}pt; font-weight: {label.Font.Weight}{(label.Orientation == Orientation.Portrait || label.IsSerial ? "" : "; transform: translateX(-100%) rotate(-90deg); transform-origin: 100% 0%; position: absolute")}\">{HttpUtility.JavaScriptStringEncode(label.Text.Replace("\r\n", "<br />"))}</div></body></html>'); w.document.close(); w.print(); w.close();", true);
            }
        }

        public static string Truncate(string value, int length) => value != null ? value.Length > length ? value.Substring(0, length - 1) + "…" : value : null;

        public static CultureInfo GetCultureInfo(string currencySymbol) => currencySymbol == "USD" ? CultureInfo.CurrentCulture : CultureInfo.GetCultures(CultureTypes.SpecificCultures).FirstOrDefault(ci => new RegionInfo(ci.LCID).ISOCurrencySymbol == currencySymbol) ?? CultureInfo.CurrentCulture;

        //public override void Dispose()
        //{
        //    folioServiceContext.Dispose();
        //    base.Dispose();
        //}
    }
}
